Imports System.Xml
Imports System.Xml.XPath
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.IO

Public Class Form1
    Private _ReadedData As List(Of Protein) = Nothing
    Private _FilterPDBs As HashSet(Of String) = Nothing
    Private _FilterUniRef As HashSet(Of String) = Nothing
    Private _SIFTSPDBObservedMapper As Dictionary(Of String, Protein) = Nothing
    Private _currentFilterFileName As String = ""

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "UniprotProteinQuantifier (OLTK InternalVersion) v1"
    End Sub

    Private Sub cmdBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBrowse.Click
        Dim k As New OpenFileDialog
        Dim checkProteinExperimentalExistence = chkCheckProteinExistence.Checked

        k.Filter = "*.xml|*.xml"

        If k.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFile.Text = k.FileName

            EnableUI(False)
            ProcessXML(k.FileName, checkProteinExperimentalExistence)
            EnableUI(True)
        End If
    End Sub

    Private Sub EnableUI(ByVal b As Boolean)
        cmdBrowse.Enabled = b
        cmdSaveResults.Enabled = b
        cmdSaveFasta.Enabled = b
        cmdLoadSIFTS.Enabled = b
    End Sub

    Private Sub ProcessXML(ByVal sFile As String, ByVal bCheckProteinExperimentalExistence As Boolean)
        Dim q As New UniprotParser
        Dim actualCount As Integer = 0
        Dim maxCount As Integer = 0

        Me.SetStatusLabel("Parsing...")
        _ReadedData = q.GetProteinsFromXMLFile(sFile,
                                                bCheckProteinExperimentalExistence,
                                                _SIFTSPDBObservedMapper,
                                                    Sub(s) Me.SetStatusLabel("Processed: " + s))

        maxCount = _ReadedData.Count
        SetPGMax(maxCount)

        Me.SetStatusLabel("Directioning proteins...")

        'FILTER
        Dim nonTumorProteins As New List(Of Protein)(10 ^ 6)

        For Each p As Protein In _ReadedData
            If Not Regex.IsMatch(p.ProteinFullName, "(tumor)|(necro)|(antigen)") Then
                'If Regex.IsMatch(p.ProteinFullName, "(tumor)|(necro)") Then
                nonTumorProteins.Add(p)
            End If
        Next

        _ReadedData = nonTumorProteins

        SetPGDone()

        Me.SetStatusLabel("Ready. Got " & _ReadedData.Count.ToString() & " proteins... Click Save to export data")
        MessageBox.Show("Done!" & vbNewLine, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub SetStatusLabel(ByVal s As String)
        lblStatus.Text = s
        Application.DoEvents()
    End Sub

    Private Sub SetPGMax(ByVal n As Integer)
        tsPG.Maximum = n
        tsPG.Visible = True
    End Sub

    Private Sub SetPGAndIncrement(ByRef n As Integer)
        tsPG.Value = n
        n += 1
    End Sub

    Private Sub SetPGDone()
        tsPG.Visible = False
        tsPG.Value = 0
    End Sub

    Private Sub cmdSaveResults_Click(sender As Object, e As EventArgs) Handles cmdSaveResults.Click
        Dim d As New SaveFileDialog()
        d.Filter = "*.csv|*.csv"
        d.FileName = _currentFilterFileName + "_rich.csv"

        If d.ShowDialog() = DialogResult.OK Then
            SaveStatsToFile(d.FileName, CInt(numMinNumberOfDomains.Value))
        End If
    End Sub

    Private Sub SaveStatsToFile(ByVal sFile As String, ByVal minimalNumberOfDomains As Integer)
        If (IO.File.Exists(sFile)) Then
            System.IO.File.Delete(sFile)
        End If

        Dim savedProteins As New List(Of Protein)
        Dim file = My.Computer.FileSystem.OpenTextFileWriter(sFile, True)

        file.WriteLine("ProteinName" + vbTab +
                           "FullName" + vbTab +
                           "PDBs" + vbTab +
                           "SsLens" + vbTab +
                           "SeqLen" + vbTab +
                           "SsCount" + vbTab +
                           "SsAvgLen" + vbTab +
                           "SsLenSum" + vbTab +
                           "DomainCount")

        For Each p In _ReadedData
            If _FilterPDBs IsNot Nothing Then
                Dim filterOut = True

                For Each id As String In p.PdbNames
                    If _FilterPDBs.Contains(id) Then
                        filterOut = False
                        Exit For
                    End If
                Next

                If filterOut Then Continue For
            End If

            If _FilterUniRef IsNot Nothing Then
                If Not _FilterUniRef.Contains(p.ProteinName) Then Continue For
            End If

            If minimalNumberOfDomains > 0 Then
                If p.DomainCount < minimalNumberOfDomains Then Continue For
            End If

            savedProteins.Add(p)

            file.WriteLine(p.ProteinName & vbTab &
                            p.ProteinFullName & vbTab &
                            String.Join("/", p.PdbNames) & vbTab &
                            String.Join(",", p.SecondaryStructures.Select(Of String)(Function(s) s.Length.ToString())) & vbTab &
                            p.ProteinSequence.Length & vbTab &
                            p.SecondaryStructures.Count & vbTab &
                            p.SecondaryStructures.Average(Function(s) s.Length) & vbTab &
                            p.SecondaryStructures.Sum(Function(s) s.Length) & vbTab &
                            p.DomainCount)
        Next

        file.Close()

        Dim secondaryStructureSizes As New List(Of Double)
        Dim sStats As String = ""

        For Each p In savedProteins
            For Each s In p.SecondaryStructures
                secondaryStructureSizes.Add(s.Length)
            Next
        Next

        sStats += secondaryStructureSizes.Min() & ", " &
                    secondaryStructureSizes.Max() & ", " &
                    secondaryStructureSizes.Average() & ", " &
                    Stats.Median(secondaryStructureSizes.ToArray())

        MessageBox.Show("Done!" + vbNewLine + sFile + vbNewLine + sStats)
    End Sub

    Private Sub SaveAllToFasta(ByVal sFile As String)
        Dim file = My.Computer.FileSystem.OpenTextFileWriter(sFile, True, Encoding.ASCII)

        For Each p In _ReadedData
            file.WriteLine(">" + p.ProteinName)
            file.WriteLine(p.ProteinSequence)
            file.WriteLine("")
        Next

        file.Close()
    End Sub



    Private Sub cmdLoadUniRefFilter_Click(sender As Object, e As EventArgs) Handles cmdLoadUniRefFilter.Click
        Dim k As New OpenFileDialog
        k.Filter = "Text file with ids per line (*.filter)|*.filter"

        If k.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtUniRefFilter.Text = k.FileName
            _currentFilterFileName = Path.GetFileName(k.FileName)
            _FilterUniRef = New HashSet(Of String)

            Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(k.FileName)
            Dim id As String

            Do
                id = reader.ReadLine()
                _FilterUniRef.Add(id)
            Loop Until id Is Nothing

            reader.Close()

            MessageBox.Show("UniRef ids filters loaded: " & _FilterUniRef.Count.ToString("### ### ###"))
        End If
    End Sub

    Private Sub cmdSaveFasta_Click(sender As Object, e As EventArgs) Handles cmdSaveFasta.Click
        Dim d As New SaveFileDialog()
        d.Filter = "*.fasta|*.fasta"

        If d.ShowDialog() = DialogResult.OK Then
            SaveAllToFasta(d.FileName)
        End If
    End Sub

    Private Sub cmdLoadSIFTS_Click(sender As Object, e As EventArgs) Handles cmdLoadSIFTS.Click
        Dim d As New OpenFileDialog()
        d.Filter = "*.csv|*.csv"
        d.FileName = "uniprot_segments_observed.csv"

        If d.ShowDialog = DialogResult.OK Then
            txtSIFTSFile.Text = d.FileName

            _SIFTSPDBObservedMapper = ReadSIFTSObservedTable(d.FileName)
            MessageBox.Show("Loaded: " + _SIFTSPDBObservedMapper.Keys.Count.ToString("### ### ###") + " observation mappings...")
        End If
    End Sub

    Private Function ReadSIFTSObservedTable(ByVal sFileName As String) As Dictionary(Of String, Protein)
        Dim lines = Regex.Split(IO.File.ReadAllText(sFileName), "\n")
        Dim haveHeader = False
        Dim pdbIndex = -1
        Dim uniprotIndex = -1
        Dim spBegIndex = -1
        Dim spEndIndex = -1
        Dim chainIndex = -1
        Dim headerLength = -1
        Dim pdbToObserved = New Dictionary(Of String, Protein)

        For Each line In lines
            If line.StartsWith("#") Then
                Continue For
            End If

            Dim cells = line.Split(",")

            If Not haveHeader Then
                Dim header = cells.ToList()
                pdbIndex = header.IndexOf("PDB")
                chainIndex = header.IndexOf("CHAIN")
                uniprotIndex = header.IndexOf("SP_PRIMARY")
                spBegIndex = header.IndexOf("SP_BEG")
                spEndIndex = header.IndexOf("SP_END")
                headerLength = header.Count

                haveHeader = True
                Continue For
            End If

            If haveHeader AndAlso cells.Count = headerLength Then
                Dim p = New Protein()
                Dim chain = cells(chainIndex)
                Dim pdbId = cells(pdbIndex)

                If chain = "A" Then
                    If pdbToObserved.ContainsKey(pdbId) Then
                        pdbToObserved(pdbId).SiftsTrustable = False     ' this protein has gaps inbetween observations detected by multiple pdbs for the chain
                        Continue For
                    End If

                    p.UniprotId = cells(uniprotIndex)
                    p.PdbNames.Add(pdbId)
                    p.ObservedStart = cells(spBegIndex)
                    p.ObservedEnd = cells(spEndIndex)

                    pdbToObserved(pdbId) = p
                End If
            End If
        Next

        pdbToObserved = pdbToObserved.Values.
                            Where(Function(p) p.SiftsTrustable).
                            ToDictionary(Of String)(Function(p) p.PdbNames.First)

        Return pdbToObserved
    End Function

End Class

Public Class UniprotParser
    Public Function GetProteinsFromXMLFile(ByVal sXMLData As String,
                                           ByVal bCheckProteinExperimentalExistence As Boolean,
                                           ByVal siftsObservedData As Dictionary(Of String, Protein),
                                           ByVal callback As Action(Of String)) As List(Of Protein)
        Dim p As Protein = Nothing
        Dim proteins As New List(Of Protein)(10 ^ 6)
        Dim seenEntries As Long = 0

        Dim reader As XmlReader = XmlReader.Create(sXMLData)
        Dim entries As XmlNodeList = Nothing
        Dim features As XmlNodeList = Nothing
        Dim t As String
        Dim startPos As String
        Dim endPos As String

        Do While reader.Read()
            Select Case reader.NodeType
                Case XmlNodeType.Element
                    Select Case reader.Name
                        Case "entry"
                            If reader.ReadToDescendant("name") Then
                                p = New Protein()
                                p.ProteinName = reader.ReadElementString
                            End If

                            If reader.ReadToNextSibling("protein") Then
                                If reader.ReadToDescendant("fullName") Then
                                    p.ProteinFullName = reader.ReadElementString
                                End If
                            End If

                            seenEntries += 1

                        Case "proteinExistence"
                            p.Evidence = reader.GetAttribute("type")                        ' https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2689360/

                        Case "dbReference"
                            Dim typeValue = reader.GetAttribute("type")
                            Dim value = reader.GetAttribute("id").ToLower()

                            If typeValue = "PDB" Then
                                If siftsObservedData.ContainsKey(value) Then
                                    p.PdbNames.Add(value)
                                End If

                                'If reader.ReadToDescendant("property") Then
                                '    Do
                                '        Dim propertyType = reader.GetAttribute("type")

                                '        If propertyType = "chains" Then
                                '            Dim chainsInfoString = reader.GetAttribute("value")
                                '            Dim startEndString = Regex.Match(chainsInfoString, "\d+-\d+$").Value

                                '            If startEndString.Length > 0 Then
                                '                Dim chainsInfo = startEndString.Split("-")
                                '                Dim observedStart = CInt(chainsInfo(0))
                                '                Dim observedEnd = CInt(chainsInfo(1))
                                '                Dim observedLength = observedEnd - observedStart + 1

                                '                If observedLength > p.ObservedLength Then
                                '                    p.ObservedStart = observedStart
                                '                    p.ObservedEnd = observedEnd
                                '                End If
                                '            End If
                                '        End If
                                '    Loop While reader.ReadToNextSibling("property")
                                'End If
                            End If

                        Case "feature"
                            t = reader.GetAttribute("type")

                            Select Case t
                                'Case "turn", "helix", "strand"
                                Case "helix", "strand"
                                    startPos = Nothing
                                    endPos = Nothing

                                    If reader.ReadToDescendant("begin") Then
                                        startPos = reader.GetAttribute("position")
                                    End If

                                    If reader.ReadToNextSibling("end") Then
                                        endPos = reader.GetAttribute("position")
                                    End If

                                    If String.IsNullOrEmpty(startPos) OrElse String.IsNullOrEmpty(endPos) Then
                                        MessageBox.Show("ERROR")
                                    End If

                                    p.AddSecondaryStructure(t, startPos, endPos)

                                Case "domain"
                                    p.DomainCount += 1
                            End Select

                        Case "sequence"
                            reader.Read()
                            p.ProteinSequence = Regex.Replace(reader.Value.ToString().Trim(), "\W", "")
                    End Select

                Case XmlNodeType.EndElement
                    If reader.Name = "entry" Then

                        If p.PdbNames.Count > 0 AndAlso p.HasSecondaryStructure Then
                            If bCheckProteinExperimentalExistence Then
                                If p.Evidence <> "evidence at protein level" Then Continue Do
                            End If

                            Dim observations As List(Of Protein) = p.PdbNames.Select(Function(pdbId) siftsObservedData(pdbId)).ToList()
                            Dim bestObservation As Protein = observations.First

                            For Each observationInfo In observations
                                observationInfo.ProteinSequence = p.ProteinSequence
                                If observationInfo.ObservedRatio > bestObservation.ObservedRatio Then
                                    bestObservation = observationInfo
                                End If
                            Next

                            If bestObservation.ObservationGapLength < 4 Then
                                'If bestObservation.ObservedRatio > 0.95 Then
                                proteins.Add(p)
                            End If
                        End If
                    End If
            End Select

            If seenEntries Mod 10000 = 0 Then
                callback(seenEntries.ToString("### ### ###") + ", with s.s.: " + proteins.Count.ToString("### ### ###"))
                Application.DoEvents()
            End If
        Loop

        Return proteins
    End Function
End Class

Public Class Protein
    Public Property ProteinName As String = Nothing
    Public Property ProteinFullName As String = Nothing
    Public Property SecondaryStructures As New List(Of SecondaryStructure)
    Public Property PdbNames As New HashSet(Of String)
    Public Property Evidence As String = ""
    Public Property ProteinSequence As String = ""
    Public Property DomainCount As Integer = 0
    Public Property UniprotId As String = ""
    Public Property ObservedStart As Double = Double.NaN
    Public Property ObservedEnd As Double = Double.NaN
    Public Property SiftsTrustable As Boolean = True

    Public ReadOnly Property ObservedLength As Integer
        Get
            If Double.IsNaN(Me.ObservedStart) OrElse Double.IsNaN(Me.ObservedEnd) Then
                Return -1
            End If

            Return Me.ObservedEnd - Me.ObservedStart + 1
        End Get
    End Property

    Public ReadOnly Property HasSecondaryStructure As Boolean
        Get
            Return Me.SecondaryStructures.Count <> 0
        End Get
    End Property

    Public ReadOnly Property ObservedRatio As Double
        Get
            Return CDbl(Me.ObservedLength) / Me.ProteinSequence.Length
        End Get
    End Property

    Public ReadOnly Property ObservationGapLength As Integer
        Get
            Return Me.ProteinSequence.Length - Me.ObservedLength + 1
        End Get
    End Property

    Public Function AddSecondaryStructure(ByVal type As String, ByVal startPosition As String, ByVal endPosition As String) As SecondaryStructure
        Dim s As New SecondaryStructure(type, startPosition, endPosition)
        Me.SecondaryStructures.Add(s)
        Return s
    End Function

    Public Function GetSecondaryStructureSequence(ByVal s As SecondaryStructure) As String
        Return ProteinSequence.Substring(s.PositionStart - 1, s.Length)
    End Function

    Public Overrides Function ToString() As String
        Return Me.ProteinName + ", PDB: " + Me.PdbNames.First() + ", seen: " + Math.Round(Me.ObservedRatio * 100, 1).ToString() + " %"
    End Function
End Class

Public Class SecondaryStructure
    Public Property Type As eSecondaryStructureType = eSecondaryStructureType.eUnknown
    Public Property PositionStart As Integer = -1
    Public Property PositionEnd As Integer = -1

    Public ReadOnly Property Length As Integer
        Get
            Return Me.PositionEnd - Me.PositionStart + 1
        End Get
    End Property

    Public Sub New(ByVal type As eSecondaryStructureType, ByVal startPosition As Integer, ByVal endPosition As Integer)
        Me.InitProtein(type, startPosition, endPosition)
    End Sub

    Public Sub New(ByVal type As String, ByVal startPosition As String, ByVal endPosition As String)
        Dim t As eSecondaryStructureType = eSecondaryStructureType.eUnknown

        Select Case type.ToLower()
            Case "helix"
                t = eSecondaryStructureType.eHelix
            Case "strand"
                t = eSecondaryStructureType.eStrand
            Case "turn"
                t = eSecondaryStructureType.eTurn
            Case Else
                t = eSecondaryStructureType.eUnknown
        End Select

        Me.InitProtein(t, Integer.Parse(startPosition), Integer.Parse(endPosition))
    End Sub

    Private Sub InitProtein(ByVal type As eSecondaryStructureType, ByVal startPosition As Integer, ByVal endPosition As Integer)
        Me.Type = type
        Me.PositionStart = startPosition
        Me.PositionEnd = endPosition
    End Sub
End Class

Public Enum eSecondaryStructureType
    eUnknown
    eHelix
    eStrand
    eTurn
End Enum

Public Module Stats
    Public Function Median(data As Double()) As Double
        Array.Sort(data)

        If data.Length Mod 2 = 0 Then
            Return (data(data.Length \ 2 - 1) + data(data.Length \ 2)) / 2
        Else
            Return data(data.Length \ 2)
        End If
    End Function
End Module