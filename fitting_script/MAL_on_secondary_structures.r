# Data preparation --------------------------------------------------------

d = read.csv("datafiles/min95obs_mmseq_30_rich.csv", sep="\t", dec=",", head=T)
aggregatedData = aggregate(d$SsAvgLen, list(d$SsCount), mean)
colnames(aggregatedData) = c("SsCount", "SsAvgLen")


# Table 1 - correlation coefficients with CIs -----------------------------

library(boot)

b = boot(d, function(d, i){
  aggregatedData = aggregate(d[i,"SsAvgLen"], list(d[i,"SsCount"]), mean)
  colnames(aggregatedData) = c("SsCount", "SsAvgLen")
  c(
    #original
    cor(d[i,"SsCount"], d[i,"SsAvgLen"], method = "pearson"),
    cor(d[i,"SsCount"], d[i,"SsAvgLen"], method = "spearman"),
    sqrt(summary(lm(d[i,"SsAvgLen"] ~ factor(d[i,"SsCount"])))$r.square),
    
    #binned
    cor(aggregatedData$SsCount, aggregatedData$SsAvgLen, method = "pearson"),
    cor(aggregatedData$SsCount, aggregatedData$SsAvgLen, method = "spearman")
  )
}, R=10) #10000

cor_res = data.frame(
  coef = c("Pearson r", "Spearman rho", "Correlation ratio", "Pearson r (binned)", "Spearman rho"),
  val = b$t0,
  lwr = apply(b$t, 2, quantile, 0.05/2),
  upr = apply(b$t, 2, quantile, 1-0.05/2)
)
write.csv2(cor_res, file = "table1.csv", row.names = F)


# Fitting nonlinear models ------------------------------------------------

par_estimates = function(model_id, data, log_ls=F, log_rs=F, alpha=0.05, R=0, weighted=T)
{
  #right side of the equation
  rs = c(
    "a*x^b * exp(c*x)",
    "a*x^b",
    "a/x",
    "d + a/x",
    "sqrt(d + a/x)"
  )[model_id]
  if(log_rs){
    rs = paste0("log(",rs,")")
  }
  
  #left side of the equation
  if(log_ls){
    ls = "log_y"
  }else{
    ls = "y"
  }
  
  #model and parameters starting values
  model = as.formula(paste0(ls," ~ ",rs))
  pars = c(a=15, b=-0.25, c=0.007, d=10)[setdiff(all.vars(model),c("x","y","log_y"))]
  
  #data
  aggregatedData = aggregate(data$SsAvgLen, list(data$SsCount), mean, na.rm=T)
  aggregatedData$n = c(table(data$SsCount))
  colnames(aggregatedData) <- c("x", "y", "n")
  plot(aggregatedData$x,aggregatedData$y)
  if(log_ls) aggregatedData$log_y = log(aggregatedData$y)
  if(!weighted) aggregatedData$n = 1
  
  #point estimates
  fit = nls(model, data=aggregatedData, start=pars, weights = n*x )
  pars = coef(fit)
  
  #bootstrap CI
  b = boot(data, R=R, statistic = function(data,indices){
    sampleData = data[indices,]
    sampleAggregatedData = aggregate(sampleData$SsAvgLen, list(sampleData$SsCount), mean)
    sampleAggregatedData$n = c(table(sampleData$SsCount))
    colnames(sampleAggregatedData) <- c("x", "y", "n")
    if(log_ls) sampleAggregatedData$log_y = log(sampleAggregatedData$y)
    
    fit = nls(model, data=sampleAggregatedData, start=pars, weights = n*x  )
    return(c(coef(fit), aic = AIC(fit), sigma = summary(fit)$sigma))
  })
  b_vals = apply(b$t, 2, quantile, c(alpha/2, 1-(alpha/2)))
  
  #fitted values
  fitted_values = predict(fit, newdata = data.frame(x=1:max(aggregatedData$x)))
  if(log_ls) fitted_values = exp(fitted_values)
  
  #returning coefficients & sigma, AIC, with CIs
  return(list(
    formula = model,
    coefficients = cbind(coefficients(summary(fit)), t(b_vals[,1:length(pars)])),
    sigma = c(sigma = summary(fit)$sigma, b_vals[,ncol(b_vals)]),
    aic = c(aic = AIC(fit), b_vals[,ncol(b_vals)-1]),
    fitted_values = fitted_values,
    fit = fit
  ))
}


# Table 2 - fitting models 1-5 --------------------------------------------

tab2 = t(sapply(1:5, function(model){
  fit = par_estimates(model, d, log_ls=F, log_rs=F, R=0, weighted = T)
  res = c(model=model,a=NA, b=NA, c=NA, d=NA, SE_a=NA, SE_b=NA, SE_c=NA, SE_d=NA, s=NA, AIC=NA)
  res[rownames(fit$coefficients)] = fit$coefficients[,"Estimate"]
  res[paste0("SE_",rownames(fit$coefficients))] = fit$coefficients[,"Std. Error"]
  res["s"] = fit$sigma[1]
  res["AIC"] = fit$aic[1]
  return(res)
}))

write.csv2(tab2, file = "table2.csv", row.names = F, na="")



# Comparison of data files ------------------------------------------------

res_variance = t(sapply(list.files("datafiles"), function(datafile){
  data = read.csv(paste0("datafiles/",datafile), sep="\t", dec=",", head=T)
  sapply(1:5, function(model){
    fit = par_estimates(model, data, R=0)
    return(fit$sigma[1])
  })
}))
plot(data.frame(var = c(res_variance[,-3]), model = factor(rep(c(1,2,4,5), each=nrow(res_variance)))))
range(apply(res_variance, 2, function(x) x/x["min95obs_mmseq_30_rich.csv"] ))-1 #relative increase of S


# Comparison of estimation methods ----------------------------------------

res_variance = t(sapply(c(F,T), function(l){
  sapply(1:5, function(model){
    fit = par_estimates(model, log_ls=l, log_rs=l, d, R=0)
    sigma = sqrt(sum((fit$fitted_values[aggregatedData$SsCount]-aggregatedData$SsAvgLen)^2 * fit$fit$weights) / (length(resid(fit$fit))-length(coef(fit$fit))))
    #sum(resid(fit$fit)^2 * fit$fit$weights) / (length(resid(fit$fit))-length(coef(fit$fit)))
    #summary(fit$fit)$sigma^2
    return(sigma)
  })
}))
res_variance[2,]/res_variance[1,]-1 #relative increase of S

# Figure 1 ----------------------------------------------------------------

library(ggplot2)
library(latex2exp)
library(showtext)
font_add("Palatino", "pala.ttf")
showtext_auto()

plot_data = data.frame(
  model = rep(1:5, each = max(d$SsCount)),
  SsCount = 1:max(d$SsCount),
  SsAvgLen = as.numeric(tapply(d$SsAvgLen, factor(d$SsCount, levels = 1:max(d$SsCount)), mean)),
  SsAvgLen_fitted = c(
    sapply(1:5, function(model){
      fit = par_estimates(model, d, log_ls=F, log_rs=F, R=0, weighted = T)
      fit$fitted_values
    }),
    sapply(1:5, function(model){
      fit = par_estimates(model, d, log_ls=F, log_rs=F, R=0, weighted = F)
      fit$fitted_values
    })),
  Weights = rep(c(T,F), each=max(d$SsCount)*5),
  N = as.numeric(table(factor(d$SsCount, levels = 1:max(d$SsCount))))
)
plot_data = plot_data[plot_data$model!=3,] #skipping model 3
plot_data$model = factor(plot_data$model, levels =  unique(plot_data$model), labels = paste0("Model ", unique(plot_data$model)))
plot_data$Weights = factor(plot_data$Weights, levels = c(T,F), labels = c("NWLS","NLS"))

labels = data.frame(
  model = levels(plot_data$model),
  label = c(as.character(TeX(r'($y = ax^be^{cx}$)')),
            as.character(TeX(r'($y = ax^b$)')),
            as.character(TeX(r'($y = d + ax^{-1}$)')),
            as.character(TeX(r'($y = \sqrt{d + ax^{-1}}$)'))
  )
)

ggplot(plot_data, aes(x=SsCount, y=SsAvgLen))+
  geom_point(shape=16, aes(alpha=N))+ #alpha=0.5
  geom_line(aes(y=SsAvgLen_fitted, lty=Weights, group=Weights), color="red")+
  #annotate('text', x = 80, y = 15, label = "Value~is~sigma~R^{2}==0.6 ",parse = TRUE,size=5) +
  geom_text(data=labels, aes(label = label), x = 80, y = 15,  size=14, parse = TRUE, family="Palatino", hjust=0)+
  labs(x="Number of secondary structures in protein (x)", y="Average secondary structure length (y)", alpha="Count", lty="Estimator")+
  scale_alpha_continuous(breaks=c(1,50,100,150), range = c(0.25,1), labels=paste0(c(1,50,100,150)," obs.") )+
  facet_wrap(~ model)+
  coord_cartesian(ylim = c(4,16), clip = "on")+
  theme_bw()+
  theme(text = element_text(family = 'Palatino', size=34, lineheight = 0.3), legend.key.width=unit(5,"mm"), legend.spacing.x = unit(3,"mm"))

ggsave("fig1.png", scale = 1, width=160, height=140, units = "mm", dpi=300) 
