<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsPG = New System.Windows.Forms.ToolStripProgressBar()
        Me.cmdSaveResults = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkCheckProteinExistence = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.numMinNumberOfDomains = New System.Windows.Forms.NumericUpDown()
        Me.txtFile = New System.Windows.Forms.TextBox()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtUniRefFilter = New System.Windows.Forms.TextBox()
        Me.cmdLoadUniRefFilter = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cmdSaveFasta = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.txtSIFTSFile = New System.Windows.Forms.TextBox()
        Me.cmdLoadSIFTS = New System.Windows.Forms.Button()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.numMinNumberOfDomains, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.tsPG})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 570)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 19, 0)
        Me.StatusStrip1.Size = New System.Drawing.Size(830, 30)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(53, 24)
        Me.lblStatus.Text = "Ready."
        '
        'tsPG
        '
        Me.tsPG.Name = "tsPG"
        Me.tsPG.Size = New System.Drawing.Size(133, 22)
        '
        'cmdSaveResults
        '
        Me.cmdSaveResults.Enabled = False
        Me.cmdSaveResults.Location = New System.Drawing.Point(24, 491)
        Me.cmdSaveResults.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdSaveResults.Name = "cmdSaveResults"
        Me.cmdSaveResults.Size = New System.Drawing.Size(782, 59)
        Me.cmdSaveResults.TabIndex = 6
        Me.cmdSaveResults.Text = "Save MA results..."
        Me.cmdSaveResults.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkCheckProteinExistence)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.numMinNumberOfDomains)
        Me.GroupBox1.Controls.Add(Me.txtFile)
        Me.GroupBox1.Controls.Add(Me.cmdBrowse)
        Me.GroupBox1.Location = New System.Drawing.Point(24, 93)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(782, 174)
        Me.GroupBox1.TabIndex = 15
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "1) Load UniProt Reviewed DB"
        '
        'chkCheckProteinExistence
        '
        Me.chkCheckProteinExistence.AutoSize = True
        Me.chkCheckProteinExistence.Checked = True
        Me.chkCheckProteinExistence.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCheckProteinExistence.Location = New System.Drawing.Point(23, 36)
        Me.chkCheckProteinExistence.Margin = New System.Windows.Forms.Padding(4)
        Me.chkCheckProteinExistence.Name = "chkCheckProteinExistence"
        Me.chkCheckProteinExistence.Size = New System.Drawing.Size(180, 21)
        Me.chkCheckProteinExistence.TabIndex = 16
        Me.chkCheckProteinExistence.Text = "Check protein existence"
        Me.chkCheckProteinExistence.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(22, 74)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(184, 17)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Minimal number of domains:"
        '
        'numMinNumberOfDomains
        '
        Me.numMinNumberOfDomains.Location = New System.Drawing.Point(214, 72)
        Me.numMinNumberOfDomains.Margin = New System.Windows.Forms.Padding(4)
        Me.numMinNumberOfDomains.Name = "numMinNumberOfDomains"
        Me.numMinNumberOfDomains.Size = New System.Drawing.Size(252, 22)
        Me.numMinNumberOfDomains.TabIndex = 14
        '
        'txtFile
        '
        Me.txtFile.Location = New System.Drawing.Point(23, 119)
        Me.txtFile.Margin = New System.Windows.Forms.Padding(4)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.ReadOnly = True
        Me.txtFile.Size = New System.Drawing.Size(475, 22)
        Me.txtFile.TabIndex = 13
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Location = New System.Drawing.Point(522, 112)
        Me.cmdBrowse.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(245, 36)
        Me.cmdBrowse.TabIndex = 12
        Me.cmdBrowse.Text = "Load Uniprot File..."
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtUniRefFilter)
        Me.GroupBox2.Controls.Add(Me.cmdLoadUniRefFilter)
        Me.GroupBox2.Location = New System.Drawing.Point(24, 396)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(782, 76)
        Me.GroupBox2.TabIndex = 16
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "3) Apply filters for UniProt proteins"
        '
        'txtUniRefFilter
        '
        Me.txtUniRefFilter.Location = New System.Drawing.Point(13, 29)
        Me.txtUniRefFilter.Margin = New System.Windows.Forms.Padding(4)
        Me.txtUniRefFilter.Name = "txtUniRefFilter"
        Me.txtUniRefFilter.ReadOnly = True
        Me.txtUniRefFilter.Size = New System.Drawing.Size(485, 22)
        Me.txtUniRefFilter.TabIndex = 17
        '
        'cmdLoadUniRefFilter
        '
        Me.cmdLoadUniRefFilter.Location = New System.Drawing.Point(522, 22)
        Me.cmdLoadUniRefFilter.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdLoadUniRefFilter.Name = "cmdLoadUniRefFilter"
        Me.cmdLoadUniRefFilter.Size = New System.Drawing.Size(245, 36)
        Me.cmdLoadUniRefFilter.TabIndex = 16
        Me.cmdLoadUniRefFilter.Text = "Load UniRef filter"
        Me.cmdLoadUniRefFilter.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cmdSaveFasta)
        Me.GroupBox3.Location = New System.Drawing.Point(24, 278)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(779, 112)
        Me.GroupBox3.TabIndex = 17
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "2) Save trusted proteins to FASTA for redundancy removal by CDHIT/MMSEQS/PISCES"
        '
        'cmdSaveFasta
        '
        Me.cmdSaveFasta.Enabled = False
        Me.cmdSaveFasta.Location = New System.Drawing.Point(12, 41)
        Me.cmdSaveFasta.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdSaveFasta.Name = "cmdSaveFasta"
        Me.cmdSaveFasta.Size = New System.Drawing.Size(755, 59)
        Me.cmdSaveFasta.TabIndex = 18
        Me.cmdSaveFasta.Text = "Save FASTA results..."
        Me.cmdSaveFasta.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.txtSIFTSFile)
        Me.GroupBox4.Controls.Add(Me.cmdLoadSIFTS)
        Me.GroupBox4.Location = New System.Drawing.Point(24, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(782, 74)
        Me.GroupBox4.TabIndex = 18
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "0) Load SIFTS observed table"
        '
        'txtSIFTSFile
        '
        Me.txtSIFTSFile.Location = New System.Drawing.Point(19, 26)
        Me.txtSIFTSFile.Margin = New System.Windows.Forms.Padding(4)
        Me.txtSIFTSFile.Name = "txtSIFTSFile"
        Me.txtSIFTSFile.ReadOnly = True
        Me.txtSIFTSFile.Size = New System.Drawing.Size(475, 22)
        Me.txtSIFTSFile.TabIndex = 15
        '
        'cmdLoadSIFTS
        '
        Me.cmdLoadSIFTS.Location = New System.Drawing.Point(518, 19)
        Me.cmdLoadSIFTS.Margin = New System.Windows.Forms.Padding(4)
        Me.cmdLoadSIFTS.Name = "cmdLoadSIFTS"
        Me.cmdLoadSIFTS.Size = New System.Drawing.Size(245, 36)
        Me.cmdLoadSIFTS.TabIndex = 14
        Me.cmdLoadSIFTS.Text = "Load SIFTS observed table ..."
        Me.cmdLoadSIFTS.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(830, 600)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdSaveResults)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.Text = "Uniprot Secondary Structure Dumper"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.numMinNumberOfDomains, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tsPG As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents cmdSaveResults As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents chkCheckProteinExistence As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents numMinNumberOfDomains As NumericUpDown
    Friend WithEvents txtFile As TextBox
    Friend WithEvents cmdBrowse As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtUniRefFilter As TextBox
    Friend WithEvents cmdLoadUniRefFilter As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents cmdSaveFasta As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents txtSIFTSFile As TextBox
    Friend WithEvents cmdLoadSIFTS As Button
End Class
