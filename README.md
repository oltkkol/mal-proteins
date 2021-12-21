# Uniprot To MAL Tables

Software reading SIFTS observation table, UniProtKB Reviewed XML file and additional ID filter to produce summarization table on proteins and their secondary structure information (numbers, average lengths, sequence lengths, codes, etc.).

## Workflow
The software assumes the following workflow:

1. Download the data:
    1. Download reviewed protein data from UniProtKB in XML (https://www.uniprot.org/downloads).
    2. Download SIFTS Observed segments CSV (https://www.ebi.ac.uk/pdbe/docs/sifts/quick.html; uniprot_segments_observed.csv.gz).

2. Process through the program:
    1. Start the program, load SIFTS CSV file,
    2. load UniProtKB XML file,
    3. Export FASTA sequences (reviewed and experimental evidence at protein level),
    4. Use any kind of clustering (MMseqs2, CD-HIT, PISCES) to obtain identifiers of non-redundant sequences,
    5. save the non-redundant identifiers into .filter file,
    6. load .filter file into the program,
    7. export the MA tables.

3. Analyze the data.

## Publication and used data
The whole dataset produced from steps 2-iii, 2-vi, 2-vii are available in folder `dataset`.

## Language
Extension of an older project in VB.NET, use Visual Studio.NET for compilation.