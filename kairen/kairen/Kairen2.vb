Public Class Kairen2
    Private lb As CommonLibrary

    Sub New(ByRef _lb As CommonLibrary)
        lb = _lb
        'ItemFile = New TextFileClass2("--", "--[[", "]]--", False)
        'AbilityFile = New TextFileClass2("--", "--[[", "]]--", False)
    End Sub

#Region "TEST CODE"
    '.LUA File Code
    Dim dotLUAFile As TextFileClass
    Public Sub LoadLUAFile(ByVal FullFilePath As String, Optional ByVal _commentString As String = "--")
        If lb.FE(FullFilePath) Then
            dotLUAFile = New TextFileClass(FullFilePath, _commentString)
            dotLUAFile.LoadFile()

            Dim i As Integer = 0

            If dotLUAFile.Line(i) = "--[[ Meta-Data" Then
                dotLUAFile.AdditionalData(i) = "Meta-Data Start"
                i = i + 1
                If lb.Left(dotLUAFile.Line(i), 19) = "Meta-Data-Version: " Then
                    If lb.Right(dotLUAFile.Line(i), dotLUAFile.Line(i).Length - 19) = "1.0" Then
                        MsgBox("meta data 1.0 detected")
                    Else
                    End If
                End If
            End If
        End If
    End Sub

#End Region

#Region "SpawnNestFile Code"
    Dim SpawnNestFile As TextFileClass
    Public Sub CreateNewNestFile(ByVal x As Integer, ByVal y As Integer, Optional ByVal _commentString As String = "--")
        Dim FileVersionToCreate As String = "0.1.0"

        Select Case FileVersionToCreate
            Case "0.1.0"
                My.Computer.FileSystem.CreateDirectory(lb.Folder_Custom_SpawnNests & x & "\")
                SpawnNestFile = New TextFileClass(lb.Folder_Custom_SpawnNests & x & "\" & y & lb.Extension_ReadWrites, _commentString)
                SpawnNestFile.WriteLine("-- File Version")
                SpawnNestFile.WriteLine("0.1.0")
                SpawnNestFile.WriteLine("-- Safe Name")
                SpawnNestFile.WriteLine(x & " " & y)
                SpawnNestFile.WriteLine("-- Game Name")
                SpawnNestFile.WriteLine("(" & x & "\" & y & ")")
                SpawnNestFile.WriteLine("-- Spawn Points")
                SpawnNestFile.SaveFile()
                SpawnNestFile.LoadFile()
        End Select
    End Sub
    Public Sub LoadSpawnNestFile(ByVal x As Integer, ByVal y As Integer)
        If lb.DE(lb.Folder_Custom_SpawnNests & x & "\") Then
            If lb.FE(lb.Folder_Custom_SpawnNests & x & "\" & y & lb.Extension_ReadWrites) Then
                SpawnNestFile = New TextFileClass(lb.Folder_Custom_SpawnNests & x & "\" & y & lb.Extension_ReadWrites, "--")
                SpawnNestFile.LoadFile()

                Dim FileVersion As String = SpawnNestFile.ReadLine() 'file version
                SpawnNestFile.AdditionalData(SpawnNestFile.CurrentIndex) = "File Version"
                If FileVersion = "0.1.0" Then
                    SpawnNestFile.ReadLine() 'safe name
                    SpawnNestFile.AdditionalData(SpawnNestFile.CurrentIndex) = "nameSafe"
                    SpawnNestFile.ReadLine() 'game name
                    SpawnNestFile.AdditionalData(SpawnNestFile.CurrentIndex) = "nameGame"
                    Dim NextLine As String = SpawnNestFile.ReadLine() 'spawn camps
                    SpawnNestFile.AdditionalData(SpawnNestFile.CurrentIndex) = "First Nest"
                    Do While NextLine <> Nothing
                        NextLine = SpawnNestFile.ReadLine()
                    Loop
                Else
                    'error unrec file version
                End If

            End If
        Else
            MsgBox("no?")
        End If
    End Sub
    Public Sub FillListBoxWithNestCoords(ByRef obj As Object)
        Dim x As Integer = 4000
        Do While x < 40500
            obj.Items.Add(x)
            x = x + 500
        Loop
    End Sub
    Public Function GetSpawnCampsFromSpawnNestFile(ByVal x As Integer, ByVal y As Integer)
        Return TextFileClass_ReturnFromTextToEnd(SpawnNestFile, "First Nest")
    End Function
    Public Sub RemoveSpawnCampFromLoadedSpawnNestFile(ByVal nameSafe As String)
        SpawnNestFile.Line(SpawnNestFile.GetIndexByValue(nameSafe, SpawnNestFile.GetIndexByAdditionalData("First Nest"))) = ""
        SpawnNestFile.SaveFile()
    End Sub
#End Region

#Region "SpawnCampFile Code"
    Public SpawnCampFile As TextFileClass

    Public Function LoadSpawnCampFile(ByVal nameSafe As String, Optional ByVal _commentString As String = "--")
        If lb.FE(lb.Folder_Custom_SpawnCamps & nameSafe & lb.Extension_ReadWrites) Then
            SpawnCampFile = New TextFileClass(lb.Folder_Custom_SpawnCamps & nameSafe & lb.Extension_ReadWrites, _commentString)
            SpawnCampFile.LoadFile()

            Dim FileVersion As String = SpawnCampFile.ReadLine() 'file version
            SpawnCampFile.AdditionalData(SpawnCampFile.CurrentIndex) = "File Version"
            If FileVersion = "0.1.0" Then
                SpawnCampFile.ReadLine() 'safe name
                SpawnCampFile.AdditionalData(SpawnCampFile.CurrentIndex) = "nameSafe"
                SpawnCampFile.ReadLine() 'game name
                SpawnCampFile.AdditionalData(SpawnCampFile.CurrentIndex) = "nameGame"
                Dim NextLine As String = SpawnCampFile.ReadLine() 'spawn points
                If NextLine <> Nothing Then SpawnCampFile.AdditionalData(SpawnCampFile.CurrentIndex) = "First Spawn Point"
                Do While NextLine <> Nothing
                    NextLine = SpawnCampFile.ReadLine()
                Loop
                Return True
            Else
                lb.DisplayMessage("The file " & Chr(34) & nameSafe & Chr(34) & " is an unknown version", "Error: Cannot Load Spawn Camp", "Kairen.LoadSpawnCampFile")
                'error unrec file version
                Return False
            End If
        Else
            lb.DisplayMessage("The file " & Chr(34) & lb.Folder_Custom_SpawnCamps & nameSafe & lb.Extension_ReadWrites & Chr(34) & " cannot be found", "Error: Cannot Load Spawn Camp", "Kairen.LoadSpawnCampFile")
            Return False
        End If
    End Function
    Public Function GetSpawnPointsFromSpawnCampFile()
        Return TextFileClass_ReturnFromTextToEnd(SpawnCampFile, "First Spawn Point")
    End Function

    Public Sub CreateNewCampFile(ByVal nameSafe As String, ByVal nameGame As String)
        Dim FileVersionToCreate As String = "0.1.0"

        Select Case FileVersionToCreate
            Case "0.1.0"
                SpawnCampFile = New TextFileClass(lb.Folder_Custom_SpawnCamps & nameSafe & lb.Extension_ReadWrites, "--")
                SpawnCampFile.WriteLine("-- File Version")
                SpawnCampFile.WriteLine("0.1.0")
                SpawnCampFile.WriteLine("-- Safe Name")
                SpawnCampFile.WriteLine(nameSafe)
                SpawnCampFile.WriteLine("-- Game Name")
                SpawnCampFile.WriteLine(nameGame)
                SpawnCampFile.WriteLine("-- Spawn Points")
                SpawnCampFile.SaveFile()
                SpawnCampFile.LoadFile()
        End Select
    End Sub

    Public Sub AddSpawnCampToLoadedNestFile(ByVal SpawnCampToAdd As String)
        TextFileClass_AddLineToEndOfFile(SpawnCampToAdd, SpawnNestFile)
    End Sub
    Public Sub RemoveSpawnPointFromLoadedSpawnCampFile(ByVal nameSafe As String)
        SpawnCampFile.Line(SpawnCampFile.GetIndexByValue(nameSafe, SpawnCampFile.GetIndexByAdditionalData("First Spawn Point"))) = ""
        SpawnCampFile.SaveFile()
    End Sub
#End Region

#Region "SpawnPointFile Code"
    Public SpawnPointFile As TextFileClass
    Public Sub LoadSpawnPointFile(ByVal nameSafe As String, Optional ByVal _commentString As String = "--")
        If lb.FE(lb.Folder_Custom_SpawnPoints & nameSafe & lb.Extension_ReadWrites) Then
            SpawnPointFile = New TextFileClass(lb.Folder_Custom_SpawnPoints & nameSafe & lb.Extension_ReadWrites, _commentString)
            SpawnPointFile.LoadFile()
            Dim FileVersion As String = SpawnPointFile.ReadLine() 'file version
            SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "File Version"
            If FileVersion = "1.0" Then
                SpawnPointFile.ReadLine() 'safe name
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "nameSafe"
                SpawnPointFile.ReadLine() 'x min
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "X Min"
                SpawnPointFile.ReadLine() 'x max
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "X Max"
                SpawnPointFile.ReadLine() 'y min
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "Y Min"
                SpawnPointFile.ReadLine() 'y max
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "Y Max"
                SpawnPointFile.ReadLine() 'z min
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "Z Min"
                SpawnPointFile.ReadLine() 'z max
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "Z Max"
                Dim NextLine As String = SpawnPointFile.ReadLine() 'npc
                If NextLine <> Nothing Then SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "First NPC"
                Do While NextLine <> Nothing
                    NextLine = SpawnPointFile.ReadLine()
                Loop
            ElseIf FileVersion = "1.1" Then
                SpawnPointFile.ReadLine() 'safe name
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "nameSafe"
                Dim NextLine As String = SpawnPointFile.ReadLine() 'npc
                SpawnPointFile.AdditionalData(SpawnPointFile.CurrentIndex) = "First NPC"
                Do While NextLine <> Nothing
                    NextLine = SpawnPointFile.ReadLine()
                Loop
            Else
                'error unrec file version
            End If

        End If
    End Sub
    Public Function GetNPCsFromSpawnPointFile()
        Return TextFileClass_ReturnFromTextToEnd(SpawnPointFile, "First NPC")
    End Function

    Public Sub CreateNewSpawnPointFile(ByVal nameSafe As String)
        Dim FileVersionToCreate As String = "1.1"

        Select Case FileVersionToCreate
            Case "1.1"
                SpawnPointFile = New TextFileClass(lb.Folder_Custom_SpawnPoints & nameSafe & lb.Extension_ReadWrites, "--")
                SpawnPointFile.WriteLine("-- File Version")
                SpawnPointFile.WriteLine("1.1")
                SpawnPointFile.WriteLine("-- Safe Name (A " & Chr(34) & "Safe Name" & Chr(34) & " is a name that is able to be a file name.)")
                SpawnPointFile.WriteLine(nameSafe)
                SpawnPointFile.WriteLine("-- NPCs")
                SpawnPointFile.SaveFile()
                SpawnPointFile.LoadFile()
        End Select
    End Sub
    Public Sub AddSpawnPointToLoadedCampFile(ByVal SpawnPointToAdd As String)
        TextFileClass_AddLineToEndOfFile(SpawnPointToAdd, SpawnCampFile)
    End Sub
    Public Sub RemoveNPCFromLoadedSpawnPointFile(ByVal nameSafe As String)
        'determine the range of lines to search for the nameSafe and then remove it and tell the file to save then in kairen_main redraw the file contents
        Dim i As Integer = SpawnPointFile.GetIndexByAdditionalData("First NPC")
        i = SpawnPointFile.GetIndexByValue(nameSafe, SpawnPointFile.GetIndexByAdditionalData("First NPC"))
        If i >= 0 Then
            SpawnPointFile.Line(i) = ""
            SpawnPointFile.SaveFile()
        End If
    End Sub
#End Region

#Region "NPC File Code"
    Public NPCFile As TextFileClass

    Public Sub LoadNPCFile(ByVal _nameSafe As String, Optional ByVal _commentString As String = "--")
        If lb.FE(lb.Folder_Custom_NPCs & _nameSafe & lb.Extension_ReadWrites) = False Then
            'npc not found error
            Exit Sub
        End If
        NPCFile = New TextFileClass(lb.Folder_Custom_NPCs & _nameSafe & lb.Extension_ReadWrites, _commentString)
        NPCFile.LoadFile()
        Dim fileversion As String = NPCFile.ReadLine() ' Version
        NPCFile.AdditionalData(NPCFile.CurrentIndex) = "File Version"

        If fileversion = "0.1.1" Or fileversion = "0.1.2" Then
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "nameSafe"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "nameGame"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "X"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Y"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Z"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "F"
            NPCFile.ReadLine() 'd_npcmkr_tb_Race()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Race"
            NPCFile.ReadLine() 'd_npcmkr_tb_Gender()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Gender"
            NPCFile.ReadLine() 'd_npcmkr_tb_Class()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Class"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Level"
            NPCFile.ReadLine() 'd_npcmkr_tb_HP()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "HP"
        ElseIf fileversion = "0.2.0" Then
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "nameSafe"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "nameGame"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "X"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Y"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Z"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "F"
            NPCFile.ReadLine() 'd_npcmkr_tb_Race()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Race"
            NPCFile.ReadLine() 'd_npcmkr_tb_Gender()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Gender"
            NPCFile.ReadLine() 'd_npcmkr_tb_Class()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Class"
            NPCFile.ReadLine()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "Level"
            NPCFile.ReadLine() 'd_npcmkr_tb_HP()
            NPCFile.AdditionalData(NPCFile.CurrentIndex) = "HP"
        Else
            MsgBox("File Version Not Supported: " & fileversion)
            Exit Sub
        End If
    End Sub
    Public Sub AddNPCToLoadedSpawnPointFile(ByVal NPCToAdd As String)
        TextFileClass_AddLineToEndOfFile(NPCToAdd, SpawnPointFile)
    End Sub
#End Region

#Region "Changlog File Code"
    Public Changelog_GameContent As TextFileClass
    Public Changelog_KairenContent As TextFileClass
    Public Function LoadChangelogFile(ByRef _changelogtype As String, Optional ByVal _commentString As String = "--")
        Dim FilePath As String
        If _changelogtype = "Changelog_GameContent" Then
            FilePath = lb.IO_Options & "Changelog_GameContent" & lb.Extension_Changelog
        ElseIf _changelogtype = "Changelog_KairenContent" Then
            FilePath = lb.IO_Kairen & "Changelog_KairenContent" & lb.Extension_Changelog
            'ElseIf TypeOf _changelog Is String Then
            'FilePath = _changelog
        Else
            'error, exit ' @@@@@@@@ THIS LINE NEEDS CHANGED @@@@@@@
            Return Nothing
        End If
        If lb.FE(FilePath) = False Then
            'npc not found error
            If _changelogtype = "Changelog_GameContent" Then
                IO.File.WriteAllBytes(FilePath, My.Resources.Changelog_GameContent3)
            ElseIf _changelogtype = "Changelog_KairenContent" Then
                IO.File.WriteAllBytes(FilePath, My.Resources.Changelog_KairenContent5)
            End If
        End If
        Dim _changelog As TextFileClass
        _changelog = New TextFileClass(FilePath, _commentString)
        _changelog.LoadFile()

        Dim fileversion As String = _changelog.ReadLine() ' Version
        _changelog.AdditionalData(_changelog.CurrentIndex) = "File Version"
        If fileversion = "0.1.0" Then
            _changelog.ReadLine()
            _changelog.AdditionalData(_changelog.CurrentIndex) = "nameSafe"
            _changelog.ReadLine()
            _changelog.AdditionalData(_changelog.CurrentIndex) = "nameGame"
            Dim NextLine As String = _changelog.ReadLine() 'log entry command
            Do While NextLine <> Nothing
                If NextLine = "[Log Entry Start]" Then
                    _changelog.AdditionalData(_changelog.CurrentIndex) = "[Log Entry Start]"
                    _changelog.ReadLine() 'log entry number
                    _changelog.AdditionalData(_changelog.CurrentIndex) = "[Log Entry Number]"
                    _changelog.ReadLine() 'log entry first line
                    _changelog.AdditionalData(_changelog.CurrentIndex) = "[Log Entry First Line]"
                    NextLine = _changelog.ReadLine()
                ElseIf NextLine = "[Log Entry End]" Then
                    _changelog.AdditionalData(_changelog.CurrentIndex) = "[Log Entry End]"
                    NextLine = _changelog.ReadLine()
                Else
                    NextLine = _changelog.ReadLine() 'log entry line
                End If
            Loop
        Else
            MsgBox("File Version Not Supported: " & fileversion)
            Return Nothing
        End If
        Return _changelog
    End Function

    Public Sub DisplayChangelogFile(ByRef _changelogtype As String, ByRef ControlToAddTo As Object, Optional ByVal EntryNumber As Integer = Nothing, Optional ClearTextBox As Boolean = True)
        Dim ChangelogToLoad As TextFileClass
        If ClearTextBox = True Then ControlToAddTo.Clear()
        ChangelogToLoad = LoadChangelogFile(_changelogtype)
        If ChangelogToLoad Is Nothing Then Exit Sub
        If EntryNumber = Nothing Then
            Dim i As Integer
            Do Until ChangelogToLoad.Line(i) = "[Log Entry Start]"
                If i >= ChangelogToLoad.NumberOfLines Then Exit Sub
                i = i + 1
            Loop
            i = i + 2
            Do Until i >= ChangelogToLoad.NumberOfLines
                Do Until ChangelogToLoad.Line(i) = "[Log Entry End]"
                    If ChangelogToLoad.Line(i) = "[bl]" Then
                        'TextBox1.AppendText(vbNewLine)
                    Else
                        ControlToAddTo.AppendText(ChangelogToLoad.Line(i))
                    End If
                    ControlToAddTo.AppendText(vbNewLine)
                    If i >= ChangelogToLoad.NumberOfLines Then Exit Sub
                    i = i + 1
                Loop
                If ChangelogToLoad.Line(i) = "[Log Entry End]" Then
                    If ((i + 3) < ChangelogToLoad.NumberOfLines) Then
                        AppendLogEntrySeparator(ControlToAddTo)
                    End If
                End If
                i = i + 3
            Loop
        Else
            'find first [Log Entry Start]
            Dim NextLine As String
            ChangelogToLoad.CurrentIndex = (ChangelogToLoad.GetIndexByAdditionalData("[Log Entry Start]") - 1)
            Dim Line_EntryMarker As String
            Dim Line_EntryNumber As Integer
            Line_EntryMarker = ChangelogToLoad.ReadLine()
            Line_EntryNumber = ChangelogToLoad.ReadLine()
            'keep doing .ReadLine() until you found your log entry
            Do Until Line_EntryMarker = "[Log Entry Start]" And Line_EntryNumber = EntryNumber
                If ChangelogToLoad.CurrentIndex + 2 >= ChangelogToLoad.NumberOfLines Then
                    Exit Do
                End If
                'keep doing .ReadLine() until you find the next [Log Entry Start]
                NextLine = ChangelogToLoad.ReadLine()
                Do Until NextLine = "[Log Entry Start]"
                    If ChangelogToLoad.CurrentIndex + 1 >= ChangelogToLoad.NumberOfLines Then
                        Exit Sub
                    End If
                    NextLine = ChangelogToLoad.ReadLine()
                    Line_EntryMarker = NextLine
                Loop
                Line_EntryNumber = ChangelogToLoad.ReadLine()
            Loop
            'print the lines of the log entry, exit sub at next [log entry end]
            If ChangelogToLoad.CurrentIndex >= ChangelogToLoad.NumberOfLines Then
                Exit Sub
            End If
            NextLine = ChangelogToLoad.ReadLine()
            Do Until NextLine = "[Log Entry End]"
                If NextLine = "[bl]" Then
                    'TextBox1.AppendText(vbNewLine)
                Else
                    ControlToAddTo.AppendText(NextLine)
                End If
                ControlToAddTo.AppendText(vbNewLine)
                NextLine = ChangelogToLoad.ReadLine()
                If ChangelogToLoad.CurrentIndex >= ChangelogToLoad.NumberOfLines Then
                    Exit Do
                End If
            Loop

        End If
    End Sub
    Public Sub DisplayLastXChangelogEntries(ByRef ChangelogToLoad As String, ByRef ControlToAddTo As Object, ByVal NumberOfEntriesToDisplay As Integer, ByVal EntryNumberToStartWith As Integer, Optional ClearControl As Boolean = False)
        ControlToAddTo.Clear()
        Dim i As Integer = 0
        Do Until i >= NumberOfEntriesToDisplay
            DisplayChangelogFile(ChangelogToLoad, ControlToAddTo, EntryNumberToStartWith - i, False)
            If (i + 1) < NumberOfEntriesToDisplay Then
                If ControlToAddTo.Text <> "" Then
                    AppendLogEntrySeparator(ControlToAddTo)
                End If
            End If
            i = i + 1
        Loop
        ControlToAddTo.SelectionStart = 0
        ControlToAddTo.ScrollToCaret()
    End Sub
    Public Sub AppendLogEntrySeparator(ByRef ControlToAddTo As Object)
        ControlToAddTo.AppendText(vbNewLine & "------------------------------------------------------------------------------------------" & vbNewLine & vbNewLine)
    End Sub
    'sub to find log entry by supplied number and the sub will search for each line that has additional data as log entry number
    ''and then read that line and keep doing that until it finds the right log entry and then displays all the data from the line that has
    ''additional data as log entry start to the line that has additional data as log entry end

#End Region

#Region "Options Files Handler"
    Private Name_OptionFile As String = "Program Options"
    Public ProgramOptionsFile As TextFileClass

    Private Function SelectOptionFile(ByVal _optionFileString As String)
        If _optionFileString = "" Then
            'error option file not loaded
            Return -1
        ElseIf _optionFileString = "ProgramOptionsFile" Then
            If ProgramOptionsFile Is Nothing Then
                ProgramOptionsFile = New TextFileClass(lb.IO_Options & "ProgramOptionsFile" & lb.Extension_OptionsFile, "--", False)
                ProgramOptionsFile.LoadFile()
            End If
            ProgramOptionsFile.LoadFile()
            Return ProgramOptionsFile
        Else
            Return -1
        End If
    End Function
    Public Function LoadOptionFile(ByVal _optionFileString As String)
        Dim _optionFile As TextFileClass
        _optionFile = SelectOptionFile(_optionFileString)
        If _optionFile.FileExists Then
            If _optionFile Is ProgramOptionsFile Then
                'ProgramOptionsFile = New TextFileClass(lb.IO_Options & _optionFileString & lb.Extension_OptionsFile, "--", False)
                'ProgramOptionsFile.LoadFile()
                Dim fileversion As String = _optionFile.ReadLine() ' version number
                _optionFile.AdditionalData(_optionFile.CurrentIndex) = "File Version"
                If fileversion = "0.1.0" Then
                    Dim NextLine As String = _optionFile.ReadLine() 'option name
                    Do While NextLine <> Nothing
                        If NextLine = "[Emulator Path]" Then
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[Emulator Path] Name"
                            _optionFile.ReadLine()
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[Emulator Path] Data"
                        ElseIf NextLine = "[ISO Path]" Then
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[ISO Path] Name"
                            _optionFile.ReadLine()
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[ISO Path] Data"
                        ElseIf NextLine = "[Player Name]" Then
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[Player Name] Name"
                            _optionFile.ReadLine()
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[Player Name] Data"
                        ElseIf NextLine = "[NPC Hails]" Then
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[NPC Hails] Name"
                            _optionFile.ReadLine()
                            _optionFile.AdditionalData(_optionFile.CurrentIndex) = "[NPC Hails] Data"
                        Else
                            'probably an error
                        End If
                        NextLine = _optionFile.ReadLine
                    Loop
                    Return True
                Else
                    'unknown file version
                    Return False
                End If
            Else
                'unknown option file
                Return False
            End If
        Else
            'file needs created
            Return False
        End If
    End Function
    Public Sub CreateOptionFile(ByVal _optionFileString As String)
        Dim _optionFile As TextFileClass
        _optionFile = SelectOptionFile(_optionFileString)
        If _optionFile.FileExists = False Then
            If _optionFile Is ProgramOptionsFile Then
                _optionFile.WriteLine("0.1.0")
                _optionFile.SaveFile()
                ProgramOptionsFile = New TextFileClass(lb.IO_Options & "ProgramOptionsFile" & lb.Extension_OptionsFile, "--", False)
                LoadOptionFile("ProgramOptionsFile")
            Else
                'unknown option file
            End If
        Else
            'file needs created
        End If
    End Sub
    Public Sub UpdateOption(ByVal _optionFileString As String, ByVal _option As String, Optional ByVal _valueString As String = Nothing, Optional ByVal _valueArray As String = Nothing)
        Dim _optionFile As TextFileClass
        _optionFile = SelectOptionFile(_optionFileString)
        If _optionFile.FileExists = False Then
            CreateOptionFile(_optionFileString)
            'write value
        End If
        AddOrUpdateOption(_optionFile, _option, _valueString, _optionFileString)
    End Sub
    Private Sub AddOrUpdateOption(ByRef _optionFile As TextFileClass, ByVal _optionName As String, ByVal _optionValue As String, ByVal _optionFileString As String)
        If _optionFile.GetIndexByAdditionalData("[" & _optionName & "] Name") = -1 Then
            _optionFile.WriteLine("[" & _optionName & "]")
            _optionFile.WriteLine(_optionValue)
            _optionFile.SaveFile()
            If _optionFileString <> Nothing Then
                LoadOptionFile(_optionFileString)
            End If
        Else
            _optionFile.UpdateValueByAdditionalData("[" & _optionName & "] Data", _optionValue)
            If _optionFileString <> Nothing Then
                LoadOptionFile(_optionFileString)
            End If
            _optionFile.SaveFile()
        End If
    End Sub
#End Region

#Region "ItemFile Code"
    Public ItemFile As New TextFileClass2("--", "--[[", "]]--", False)
    Public Function LoadItemFile(ByVal _nameSafe As String)
        Return ItemFile.LoadFile_new(lb.Folder_Custom_Items, _nameSafe & lb.Extension_ItemFile)
    End Function
    Public Function SaveItemFile(Optional ByVal _newFilePath As String = Nothing, Optional ByVal _newFileName As String = Nothing)
        Return ItemFile.SaveFile_new(lb.Folder_Custom_Items, _newFileName & lb.Extension_ItemFile)
        If _newFileName = Nothing Then
            If ItemFile.FileExists = False Then Return -5 'error trying to save over a file that doesn't exist
            Return ItemFile.SaveFile_new(ItemFile.FilePath)
        Else
            Return ItemFile.SaveFile_new(lb.Folder_Custom_Items & _newFileName & lb.Extension_ItemFile)
        End If
    End Function
    Public Sub ReinstantiateItemFile()
        ItemFile = New TextFileClass2("--", "--[[", "]]--", False)
    End Sub
#End Region

#Region "AbilityFile Code"
    Public AbilityFile As New TextFileClass2("--", "--[[", "]]--", False)
    Public Function LoadAbilityFile(ByVal _nameSafe As String)
        Return AbilityFile.LoadFile_new(lb.Folder_Custom_Abilities, _nameSafe & lb.Extension_AbilityFile)
        'If lb.FE(lb.Folder_Custom_Abilities & nameSafe & lb.Extension_AbilityFile) Then
        '    'AbilityFile.FilePath = lb.Folder_Custom_Abilities & nameSafe & lb.Extension_AbilityFile
        '    AbilityFile.LoadFile_new(lb.Folder_Custom_Abilities & nameSafe & lb.Extension_AbilityFile)
        '     AbilityFile.UpdateUIElements()
        'Else
        '    Return -1
        'End If
        'Return 0
    End Function
    Public Function SaveAbilityFile(Optional ByVal _newFilePath As String = Nothing)
        Return AbilityFile.SaveFile_new(_newFilePath)
    End Function
    Public Sub ReinstantiateAbilityFile()
        AbilityFile = New TextFileClass2("--", "--[[", "]]--", False)
    End Sub
#End Region

#Region "ProcsFile Code"
    Public ProcFile As New TextFileClass2("--", "--[[", "]]--", False)
    Public Function LoadProcFile(ByVal nameSafe As String)
        Return ProcFile.LoadFile_new(lb.Folder_Custom_Procs, nameSafe & lb.Extension_ProcFile)
    End Function
    Public Function SaveProcFile(Optional ByVal _newFilePath As String = Nothing)
        Return ProcFile.SaveFile_new(_newFilePath)
    End Function
    Public Sub ReinstantiateProcFile()
        ProcFile = New TextFileClass2("--", "--[[", "]]--", False)
    End Sub
#End Region

#Region "Buddyfile Code"
    Public BuddyFile As New TextFileClass3("--", "--[[", "]]--", False)
    Public Function LoadBuddyFile(ByVal _nameSafe As String)
        Return BuddyFile.LoadFile(lb.Folder_BuddyList, _nameSafe & lb.Extension_BuddyFile)
        'If lb.FE(lb.Folder_Custom_Abilities & nameSafe & lb.Extension_AbilityFile) Then
        '    'AbilityFile.FilePath = lb.Folder_Custom_Abilities & nameSafe & lb.Extension_AbilityFile
        '    AbilityFile.LoadFile_new(lb.Folder_Custom_Abilities & nameSafe & lb.Extension_AbilityFile)
        '     AbilityFile.UpdateUIElements()
        'Else
        '    Return -1
        'End If
        'Return 0
    End Function
    Public Function SaveBuddyFile(Optional ByVal _newFilePath As String = Nothing)
        Return BuddyFile.SaveFile(lb.Folder_BuddyList, _newFilePath & lb.Extension_BuddyFile)
    End Function
    Public Sub ReinstantiateBuddyFile()
        BuddyFile = New TextFileClass3("--", "--[[", "]]--", False)
    End Sub
#End Region

#Region "Multi-Use Code"
    Private Function TextFileClass_ReturnFromTextToEnd(ByRef TextFileObject As TextFileClass, ByVal LocatorText As String) As String()
        Dim i As Integer = TextFileObject.GetIndexByAdditionalData(LocatorText)
        If i = -1 Then Return Nothing
        Dim i2 As Integer = 0
        Dim returnArray() As String
        Do While i < TextFileObject.NumberOfLines
            ReDim Preserve returnArray(i2)
            returnArray(i2) = TextFileObject.Line(i)
            i = i + 1
            i2 = i2 + 1
        Loop
        Return returnArray
    End Function
    Private Function TextFileClass_ReturnFromTextToEnd(ByRef TextFileObject As TextFileClass2, ByVal LocatorText As String) As String()
        Dim i As Integer = TextFileObject.GetIndexByAdditionalData(LocatorText)
        If i = -1 Then Return Nothing
        Dim i2 As Integer = 0
        Dim returnArray() As String
        Do While i < TextFileObject.NumberOfLines
            ReDim Preserve returnArray(i2)
            returnArray(i2) = TextFileObject.Line(i)
            i = i + 1
            i2 = i2 + 1
        Loop
        Return returnArray
    End Function
    Private Sub TextFileClass_AddLineToEndOfFile(ByVal lineToAdd As String, ByVal TextFileClass_Object As TextFileClass)
        TextFileClass_Object.WriteLine(lineToAdd)
        TextFileClass_Object.SaveFile()
    End Sub
    Private Sub TextFileClass_AddLineToEndOfFile(ByVal lineToAdd As String, ByVal TextFileClass_Object As TextFileClass2)
        TextFileClass_Object.WriteLine(lineToAdd)
        TextFileClass_Object.SaveFile()
    End Sub

    Public Sub LaunchCE(Optional ByVal _suffix As String = "")
        Try
            'Shell() works too
            Process.Start(lb.Folder_CheatEngine & lb.FileName_CheatEngineFile_MainTable & _suffix & lb.Extension_CheatTable)
        Catch ex As Exception
        End Try
    End Sub
    Public Sub LaunchEmulatorAndDiscImage()
        Try
            Dim path As String = ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data")
            Dim arg As String = Chr(34) & ProgramOptionsFile.GetValueByAdditionalData("[ISO Path] Data") & Chr(34)
            Dim myProcess As New Process
            myProcess.StartInfo.WorkingDirectory = lb.Left(path, path.LastIndexOf("\"))
            myProcess.StartInfo.FileName = lb.Right(path, path.Length - path.LastIndexOf("\") - 1)
            myProcess.StartInfo.Arguments = arg
            myProcess.Start()
        Catch ex As Exception
        End Try
    End Sub
    Public Sub LaunchGameFullExperience()
        If lb.FE(ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data")) And lb.FE(ProgramOptionsFile.GetValueByAdditionalData("[ISO Path] Data")) Then
            Try
                Dim path As String = ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data")
                Dim arg As String = Chr(34) & ProgramOptionsFile.GetValueByAdditionalData("[ISO Path] Data") & Chr(34)
                Dim myProcess As New Process
                myProcess.StartInfo.WorkingDirectory = lb.Left(path, path.LastIndexOf("\"))
                myProcess.StartInfo.FileName = lb.Right(path, path.Length - path.LastIndexOf("\") - 1)
                myProcess.StartInfo.Arguments = arg
                LaunchCE("_Silent")
                Threading.Thread.Sleep(500)
                myProcess.Start()
            Catch ex As Exception
            End Try
        Else
            LaunchCE()
        End If
    End Sub

    Public Function CreateShortcut(ByVal ShortcutName As String, ByVal PathToFile As String, Optional ByVal CommandLineArguments As String = Nothing)
        Dim objShell As Object
        Dim objLink As Object
        Try
            objShell = CreateObject("WScript.Shell")
            ' Automatically creates a shortcut on the desktop with the product name of the application. 
            objLink = objShell.CreateShortcut(My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & ShortcutName & ".lnk")
            ' Sets the shortcut's link to the application's main executable file. 
            'objLink.TargetPath = TextBox10.Text
            objLink.Arguments(CommandLineArguments) 'eqoa disk
            objLink.TargetPath = PathToFile
            objLink.WorkingDirectory = lb.Left(PathToFile, PathToFile.Length - (PathToFile.Length - PathToFile.LastIndexOf("\")))
            objLink.WindowStyle = 1
            '"C:\Users\Kaynewihza\Documents\EQOA Revival\PCSX2 0.9.7\pcsx2-r3878.exe" "C:\Users\Kaynewihza\Documents\EQOA Revival\Frontiers\FRONTSPATCH.iso"
            objLink.Save()
        Catch ex As Exception
            'MessageBox.Show(ex.Message)
            Return False
        End Try
        Return True
    End Function

    Public Sub RestartToLauncher(Optional ByRef sendingForm As Form = Nothing)
        If lb.FE(Application.ExecutablePath) Then
            Try
                Dim path As String = Application.ExecutablePath
                Dim arg As String = "/AutoLaunchCountDown -1"
                Dim myProcess As New Process
                myProcess.StartInfo.WorkingDirectory = lb.Left(path, path.LastIndexOf("\"))
                myProcess.StartInfo.FileName = lb.Right(path, path.Length - path.LastIndexOf("\") - 1)
                myProcess.StartInfo.Arguments = arg
                myProcess.Start()
                If sendingForm IsNot Nothing Then
                    sendingForm.Close()
                End If
            Catch ex As Exception
                MsgBox("Error: Could not restart.", MsgBoxStyle.Critical, "Restart to Launcher Error")
            End Try
        Else
            MsgBox("Kairen no longer seems to exist where he once was..", MsgBoxStyle.Critical, "Error: Kairen Program Not Detected")
        End If
    End Sub

    ' listbox.SelectionMode = SelectionMode.None will disable checking the listbox but keep it enabled for scrolling
    Public Function DataParserFunction(ByVal _dataTag As String, ByRef _dataControl As Object, ByVal _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        Dim ReturnData As String
        ReturnData = _rawData
        If TypeOf _dataControl Is TextBox Or _
            TypeOf _dataControl Is Label Or _
            TypeOf _dataControl Is ComboBox Then
            If _rawData IsNot Nothing Then 'setting control - convert file to control
                If TypeOf (_rawData) Is Array Then
                    If _rawData.Length = 0 Then
                        _dataControl.Text = ""
                        ReturnData = "False"
                    End If
                End If
                If _rawData = "False" Then
                    _dataControl.Text = ""
                    ReturnData = _dataControl.Text
                Else
                    _dataControl.Text = _rawData
                    ReturnData = _rawData
                End If
            Else 'getting control - convert control to file
                If _dataControl.Text.trim = "" Then
                    ReturnData = "False"
                Else
                    ReturnData = _dataControl.Text
                End If
            End If
        ElseIf TypeOf _dataControl Is CheckBox Then
            If _rawData IsNot Nothing Then 'setting control - convert file to control
                If _rawData = "True" Then
                    _dataControl.Checked = True
                    ReturnData = True
                Else
                    _dataControl.Checked = False
                    ReturnData = False
                End If
            Else 'getting control - convert control to file
                If _dataControl.Checked = True Then
                    ReturnData = True
                Else
                    ReturnData = False
                End If
            End If
        ElseIf TypeOf _dataControl Is CheckedListBox Then
            Dim ControlItem_Index As Integer = -1
            Dim i As Integer
            Do Until i = _dataControl.Items.Count
                If _dataControl.Items.Item(i) = _dataTag Then
                    ControlItem_Index = i
                    Exit Do
                End If
                i += 1
            Loop
            If _rawData IsNot Nothing Then 'setting control - convert file to control
                If _rawData = "False" Then
                    '_dataControl.Text = ""
                    'ReturnData = _dataControl.Text
                Else
                    _dataControl.SetItemCheckState(ControlItem_Index, CheckState.Checked)
                    ReturnData = _rawData
                End If
            Else 'getting control - convert control to file
                If _dataControl.GetItemCheckState(ControlItem_Index) = True Then
                    ReturnData = "True"
                Else
                    ReturnData = "False"
                End If
            End If
        Else
            Exit Function
        End If
        Return ReturnData
    End Function
#End Region

#Region "Old Code"
    Public Function LoadItemFile2(ByVal nameSafe As String)
        '  If lb.FE(lb.Folder_Items & nameSafe & lb.Extension_ItemFile) Then
        'ItemFile = New TextFileClass(lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile, _commentString)
        'ItemFile.LoadFile()
        ItemFile.FilePath = lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile
        Dim FileVersion As String = ItemFile.ReadLine() 'file version
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "File Version"
        'If FileVersion = "0.1.0" Then
        ItemFile.ReadLine() 'safe name
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "nameSafe"
        ItemFile.ReadLine() 'game name
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "nameGame"
        ItemFile.ReadLine() 'inventory category
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Inventory Category"
        ItemFile.ReadLine() 'value
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Value"
        ItemFile.ReadLine() 'max stack
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Max Stack"
        ItemFile.ReadLine() 'level
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Level"
        ItemFile.ReadLine() 'item hp
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Item HP"
        ItemFile.ReadLine() 'durability
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Durability"
        ItemFile.ReadLine() 'icon
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Icon"
        ItemFile.ReadLine() 'graphic 1
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Graphic"
        ItemFile.ReadLine() 'gr1
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "gr1"
        ItemFile.ReadLine() 'gr2
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "gr2"
        ItemFile.ReadLine() 'gr3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "gr3"
        ItemFile.ReadLine() 'red
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "RED"
        ItemFile.ReadLine() 'green
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "GREEN"
        ItemFile.ReadLine() 'blue
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "BLUE"
        ItemFile.ReadLine() 'rgbff
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "RGBFF"
        ItemFile.ReadLine() 'ukrgb1
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb1"
        ItemFile.ReadLine() 'ukrgb2
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb2"
        ItemFile.ReadLine() 'ukrgb3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb3"
        ItemFile.ReadLine() 'ukrgb4
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb4"
        ItemFile.ReadLine() 'ukrgb5
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb5"
        ItemFile.ReadLine() ' NO RENT
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "NO RENT"
        ItemFile.ReadLine() ' NO TRADE
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "NO TRADE"
        ItemFile.ReadLine() ' LORE
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "LORE"
        ItemFile.ReadLine() ' CRAFTABLE
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "CRAFTABLE"

        ItemFile.ReadLine() ' dark elf
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Dark Elf"
        ItemFile.ReadLine() ' troll
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Troll"
        ItemFile.ReadLine() ' ogre
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Ogre"
        ItemFile.ReadLine() ' elf
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Elf"
        ItemFile.ReadLine() ' dwarf
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Dwarf"
        ItemFile.ReadLine() ' gnome
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Gnome"
        ItemFile.ReadLine() ' halfling
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Halfling"
        ItemFile.ReadLine() ' erudite
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Erudite"
        ItemFile.ReadLine() ' human
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Human"
        ItemFile.ReadLine() ' barbarian
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Barbarian"

        ItemFile.ReadLine() ' warrior
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Warrior"
        ItemFile.ReadLine() ' paladin
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Paladin"
        ItemFile.ReadLine() ' shadowknight
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Shadowknight"
        ItemFile.ReadLine() ' enchanter
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Enchanter"
        ItemFile.ReadLine() ' magician
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Magician"
        ItemFile.ReadLine() ' wizard
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Wizard"
        ItemFile.ReadLine() ' alchemist
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Alchemist"
        ItemFile.ReadLine() ' necromancer
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Necromancer"
        ItemFile.ReadLine() ' monk
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Monk"
        ItemFile.ReadLine() ' rogue
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Rogue"
        ItemFile.ReadLine() ' ranger
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Ranger"
        ItemFile.ReadLine() ' bard
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Bard"
        ItemFile.ReadLine() ' druid
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Druid"
        ItemFile.ReadLine() ' shaman
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Shaman"
        ItemFile.ReadLine() ' cleric
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Cleric"

        ItemFile.ReadLine() ' strength
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Strength"
        ItemFile.ReadLine() ' stamina
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Stamina"
        ItemFile.ReadLine() ' agility
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Agility"
        ItemFile.ReadLine() ' dexterity
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Dexterity"
        ItemFile.ReadLine() ' wisdom
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Wisdom"
        ItemFile.ReadLine() ' intelligence
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Intelligence"
        ItemFile.ReadLine() ' charisma
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Charisma"
        ItemFile.ReadLine() ' fr
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "FR"
        ItemFile.ReadLine() ' cr
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "CR"
        ItemFile.ReadLine() ' lr
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "LR"
        ItemFile.ReadLine() ' ar
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "AR"
        ItemFile.ReadLine() ' pr
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "PR"
        ItemFile.ReadLine() ' dr
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "DR"
        ItemFile.ReadLine() ' hot
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "HoT"
        ItemFile.ReadLine() ' pot
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "PoT"
        ItemFile.ReadLine() ' hpmax
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "HPMAX"
        ItemFile.ReadLine() ' powmax
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "POWMAX"
        ItemFile.ReadLine() ' ac
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "AC"

        ItemFile.ReadLine() ' defmod
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Def Mod"
        ItemFile.ReadLine() ' off mod
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Off Mod"
        ItemFile.ReadLine() ' hp factor
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "HP Factor"
        ItemFile.ReadLine() ' movement rate
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Movement Rate"
        ItemFile.ReadLine() ' fishing
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Fishing"
        ItemFile.ReadLine() ' jewel crafting
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Jewel Crafting"
        ItemFile.ReadLine() ' armor crafting
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Armor Crafting"
        ItemFile.ReadLine() ' weapon crafting
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Weapon Crafting"
        ItemFile.ReadLine() ' tailoring
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Tailoring"
        ItemFile.ReadLine() ' alchemy
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Alchemy"
        ItemFile.ReadLine() ' carpentry
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Carpentry"

        ItemFile.ReadLine() 'description
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Description"
        ItemFile.ReadLine() 'equip type
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Equip Type"
        ItemFile.ReadLine() 'attack damage value
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Attack Damage"
        ItemFile.ReadLine() 'attack damage type
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Damage Type"
        ItemFile.ReadLine() 'unknown value 1
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv1"
        ItemFile.ReadLine() 'unknown value 2
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv2"
        ItemFile.ReadLine() 'unknown value 3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv3"
        ItemFile.ReadLine() 'unknown value 4
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv4"
        ItemFile.ReadLine() 'unknown value 5
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv5"
        ItemFile.ReadLine() 'unknown value 6
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv6"
        ItemFile.ReadLine() 'unknown value 7
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv7"
        ItemFile.ReadLine() 'unknown value 8
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv8"
        ItemFile.ReadLine() 'unknown value 9
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv9"
        ItemFile.ReadLine() 'unknown value 10
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv10"
        ItemFile.ReadLine() 'unknown value 11
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv11"
        ItemFile.ReadLine() 'unknown value 12
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv12"
        ItemFile.ReadLine() 'unknown value 13
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv13"
        ItemFile.ReadLine() 'control restriction options
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "control restriction options"
        ItemFile.ReadLine() 'fut1
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "fut1"
        ItemFile.ReadLine() 'fut2
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "fut2"
        ItemFile.ReadLine() 'fut3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "fut3"
        ItemFile.ReadLine() 'fut3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "pattern family"
        ItemFile.ReadLine() 'fut3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "pattern res1"
        ItemFile.ReadLine() 'fut3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "pattern res2"
        ItemFile.ReadLine() 'quest catelog number
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Quest Number"
        ItemFile.ReadLine() 'quest res 1
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "qr1"
        ItemFile.ReadLine() 'quest res 2
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "qr2"
        ItemFile.ReadLine() 'quest res 3
        ItemFile.AdditionalData(ItemFile.CurrentIndex) = "qr3"
        Dim NextLine As String = ItemFile.ReadLine() 'process effects
        'If NextLine <> Nothing Then ItemFile.AdditionalData(ItemFile.CurrentIndex) = "First Effect"
        'Do While NextLine <> Nothing
        '    NextLine = ItemFile.ReadLine()
        'Loop
        '    Return True
        'Else
        '    lb.DisplayMessage("The file " & Chr(34) & nameSafe & Chr(34) & " is an unknown version", "Error: Cannot Load Item File", "Kairen.LoadItemFile")
        '    error unrec file version
        '    Return False
        'End If
        'Else
        'lb.DisplayMessage("The file " & Chr(34) & lb.Folder_Items & nameSafe & lb.Extension_ItemFile & Chr(34) & " cannot be found", "Error: Cannot Load Item File", "Kairen.LoadItemFile")
        'Return False
        'End If
    End Function
    Public Function LoadItemFile3(ByVal nameSafe As String)
        If lb.FE(lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile) Then
            ItemFile.FilePath = lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile
            ItemFile.LoadFile()

            Dim FileVersion As String = ItemFile.ReadLine() 'file version
            ItemFile.AdditionalData(ItemFile.CurrentIndex) = "File Version"
            If FileVersion = "0.1.0" Then
                ItemFile.ReadLine() 'safe name
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "nameSafe"
                ItemFile.ReadLine() 'game name
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "nameGame"
                ItemFile.ReadLine() 'inventory category
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Inventory Category"
                ItemFile.ReadLine() 'value
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Value"
                ItemFile.ReadLine() 'max stack
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Max Stack"
                ItemFile.ReadLine() 'level
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Level"
                ItemFile.ReadLine() 'item hp
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Item HP"
                ItemFile.ReadLine() 'durability
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Durability"
                ItemFile.ReadLine() 'icon
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Icon"
                ItemFile.ReadLine() 'graphic 1
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Graphic"
                ItemFile.ReadLine() 'gr1
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "gr1"
                ItemFile.ReadLine() 'gr2
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "gr2"
                ItemFile.ReadLine() 'gr3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "gr3"
                ItemFile.ReadLine() 'red
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "RED"
                ItemFile.ReadLine() 'green
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "GREEN"
                ItemFile.ReadLine() 'blue
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "BLUE"
                ItemFile.ReadLine() 'rgbff
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "RGBFF"
                ItemFile.ReadLine() 'ukrgb1
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb1"
                ItemFile.ReadLine() 'ukrgb2
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb2"
                ItemFile.ReadLine() 'ukrgb3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb3"
                ItemFile.ReadLine() 'ukrgb4
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb4"
                ItemFile.ReadLine() 'ukrgb5
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "ukrgb5"
                ItemFile.ReadLine() ' NO RENT
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "NO RENT"
                ItemFile.ReadLine() ' NO TRADE
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "NO TRADE"
                ItemFile.ReadLine() ' LORE
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "LORE"
                ItemFile.ReadLine() ' CRAFTABLE
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "CRAFTABLE"

                ItemFile.ReadLine() ' dark elf
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Dark Elf"
                ItemFile.ReadLine() ' troll
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Troll"
                ItemFile.ReadLine() ' ogre
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Ogre"
                ItemFile.ReadLine() ' elf
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Elf"
                ItemFile.ReadLine() ' dwarf
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Dwarf"
                ItemFile.ReadLine() ' gnome
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Gnome"
                ItemFile.ReadLine() ' halfling
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Halfling"
                ItemFile.ReadLine() ' erudite
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Erudite"
                ItemFile.ReadLine() ' human
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Human"
                ItemFile.ReadLine() ' barbarian
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Barbarian"

                ItemFile.ReadLine() ' warrior
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Warrior"
                ItemFile.ReadLine() ' paladin
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Paladin"
                ItemFile.ReadLine() ' shadowknight
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Shadowknight"
                ItemFile.ReadLine() ' enchanter
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Enchanter"
                ItemFile.ReadLine() ' magician
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Magician"
                ItemFile.ReadLine() ' wizard
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Wizard"
                ItemFile.ReadLine() ' alchemist
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Alchemist"
                ItemFile.ReadLine() ' necromancer
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Necromancer"
                ItemFile.ReadLine() ' monk
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Monk"
                ItemFile.ReadLine() ' rogue
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Rogue"
                ItemFile.ReadLine() ' ranger
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Ranger"
                ItemFile.ReadLine() ' bard
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Bard"
                ItemFile.ReadLine() ' druid
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Druid"
                ItemFile.ReadLine() ' shaman
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Shaman"
                ItemFile.ReadLine() ' cleric
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Cleric"

                ItemFile.ReadLine() ' strength
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Strength"
                ItemFile.ReadLine() ' stamina
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Stamina"
                ItemFile.ReadLine() ' agility
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Agility"
                ItemFile.ReadLine() ' dexterity
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Dexterity"
                ItemFile.ReadLine() ' wisdom
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Wisdom"
                ItemFile.ReadLine() ' intelligence
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Intelligence"
                ItemFile.ReadLine() ' charisma
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Charisma"
                ItemFile.ReadLine() ' fr
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "FR"
                ItemFile.ReadLine() ' cr
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "CR"
                ItemFile.ReadLine() ' lr
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "LR"
                ItemFile.ReadLine() ' ar
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "AR"
                ItemFile.ReadLine() ' pr
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "PR"
                ItemFile.ReadLine() ' dr
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "DR"
                ItemFile.ReadLine() ' hot
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "HoT"
                ItemFile.ReadLine() ' pot
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "PoT"
                ItemFile.ReadLine() ' hpmax
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "HPMAX"
                ItemFile.ReadLine() ' powmax
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "POWMAX"
                ItemFile.ReadLine() ' ac
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "AC"

                ItemFile.ReadLine() ' defmod
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Def Mod"
                ItemFile.ReadLine() ' off mod
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Off Mod"
                ItemFile.ReadLine() ' hp factor
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "HP Factor"
                ItemFile.ReadLine() ' movement rate
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Movement Rate"
                ItemFile.ReadLine() ' fishing
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Fishing"
                ItemFile.ReadLine() ' jewel crafting
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Jewel Crafting"
                ItemFile.ReadLine() ' armor crafting
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Armor Crafting"
                ItemFile.ReadLine() ' weapon crafting
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Weapon Crafting"
                ItemFile.ReadLine() ' tailoring
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Tailoring"
                ItemFile.ReadLine() ' alchemy
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Alchemy"
                ItemFile.ReadLine() ' carpentry
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Carpentry"

                ItemFile.ReadLine() 'description
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Description"
                ItemFile.ReadLine() 'equip type
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Equip Type"
                ItemFile.ReadLine() 'attack damage value
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Attack Damage"
                ItemFile.ReadLine() 'attack damage type
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Damage Type"
                ItemFile.ReadLine() 'unknown value 1
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv1"
                ItemFile.ReadLine() 'unknown value 2
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv2"
                ItemFile.ReadLine() 'unknown value 3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv3"
                ItemFile.ReadLine() 'unknown value 4
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv4"
                ItemFile.ReadLine() 'unknown value 5
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv5"
                ItemFile.ReadLine() 'unknown value 6
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv6"
                ItemFile.ReadLine() 'unknown value 7
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv7"
                ItemFile.ReadLine() 'unknown value 8
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv8"
                ItemFile.ReadLine() 'unknown value 9
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv9"
                ItemFile.ReadLine() 'unknown value 10
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv10"
                ItemFile.ReadLine() 'unknown value 11
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv11"
                ItemFile.ReadLine() 'unknown value 12
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv12"
                ItemFile.ReadLine() 'unknown value 13
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "uv13"
                ItemFile.ReadLine() 'control restriction options
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "control restriction options"
                ItemFile.ReadLine() 'fut1
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "fut1"
                ItemFile.ReadLine() 'fut2
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "fut2"
                ItemFile.ReadLine() 'fut3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "fut3"
                ItemFile.ReadLine() 'fut3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "pattern family"
                ItemFile.ReadLine() 'fut3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "pattern res1"
                ItemFile.ReadLine() 'fut3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "pattern res2"
                ItemFile.ReadLine() 'quest catelog number
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "Quest Number"
                ItemFile.ReadLine() 'quest res 1
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "qr1"
                ItemFile.ReadLine() 'quest res 2
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "qr2"
                ItemFile.ReadLine() 'quest res 3
                ItemFile.AdditionalData(ItemFile.CurrentIndex) = "qr3"
                Dim NextLine As String = ItemFile.ReadLine() 'process effects
                If NextLine <> Nothing Then ItemFile.AdditionalData(ItemFile.CurrentIndex) = "First Effect"
                Do While NextLine <> Nothing
                    NextLine = ItemFile.ReadLine()
                Loop
                Return True
            Else
                lb.DisplayMessage("The file " & Chr(34) & nameSafe & Chr(34) & " is an unknown version", "Error: Cannot Load Item File", "Kairen.LoadItemFile")
                'error unrec file version
                Return False
            End If
        Else
            lb.DisplayMessage("The file " & Chr(34) & lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile & Chr(34) & " cannot be found", "Error: Cannot Load Item File", "Kairen.LoadItemFile")
            Return False
        End If
    End Function
    Public Sub CreateNewItemFile(ByVal nameSafe As String, ByVal nameGame As String, ByVal _inventoryCategory As String, _
 Optional ByVal _value As Object = "0", _
 Optional ByVal _maxStack As Object = "False", _
 Optional ByVal _level As Object = "False", _
 Optional ByVal _itemHP As Object = "False", _
 Optional ByVal _durability As Object = "False", _
 Optional ByVal _icon As Object = "HAS ICON", _
 Optional ByVal _graphic1 As Object = "HAS GRAPHIC", _
 Optional ByVal _gr1 As Object = "False", _
 Optional ByVal _gr2 As Object = "False", _
 Optional ByVal _gr3 As Object = "False", _
 Optional ByVal _red As Object = "False", _
 Optional ByVal _green As Object = "False", _
 Optional ByVal _blue As Object = "False", _
 Optional ByVal _rgbff As Object = "FF", _
 Optional ByVal _ukrgb1 As Object = "False", _
 Optional ByVal _ukrgb2 As Object = "False", _
 Optional ByVal _ukrgb3 As Object = "False", _
 Optional ByVal _ukrgb4 As Object = "False", _
 Optional ByVal _ukrgb5 As Object = "False", _
 Optional ByVal _NORENT As Object = "False", _
 Optional ByVal _NOTRADE As Object = "False", _
 Optional ByVal _LORE As Object = "False", _
 Optional ByVal _CRAFTABLE As Object = "False", _
 Optional ByVal _darkelf As Object = "0", _
 Optional ByVal _troll As Object = "0", _
 Optional ByVal _ogre As Object = "0", _
 Optional ByVal _elf As Object = "0", _
 Optional ByVal _dwarf As Object = "0", _
 Optional ByVal _gnome As Object = "0", _
 Optional ByVal _halfling As Object = "0", _
 Optional ByVal _erudite As Object = "0", _
 Optional ByVal _human As Object = "0", _
 Optional ByVal _barbarian As Object = "0", _
 Optional ByVal _warrior As Object = "0", _
 Optional ByVal _paladin As Object = "0", _
 Optional ByVal _shadowknight As Object = "0", _
 Optional ByVal _enchanter As Object = "0", _
 Optional ByVal _magician As Object = "0", _
 Optional ByVal _wizard As Object = "0", _
 Optional ByVal _alchemist As Object = "0", _
 Optional ByVal _necromancer As Object = "0", _
 Optional ByVal _monk As Object = "0", _
 Optional ByVal _rogue As Object = "0", _
 Optional ByVal _ranger As Object = "0", _
 Optional ByVal _bard As Object = "0", _
 Optional ByVal _druid As Object = "0", _
 Optional ByVal _shaman As Object = "0", _
 Optional ByVal _cleric As Object = "0", _
 Optional ByVal _strength As Object = "False", _
 Optional ByVal _stamina As Object = "False", _
 Optional ByVal _agility As Object = "False", _
 Optional ByVal _dexterity As Object = "False", _
 Optional ByVal _wisdom As Object = "False", _
 Optional ByVal _intelligence As Object = "False", _
 Optional ByVal _charisma As Object = "False", _
 Optional ByVal _fr As Object = "False", _
 Optional ByVal _cr As Object = "False", _
 Optional ByVal _lr As Object = "False", _
 Optional ByVal _ar As Object = "False", _
 Optional ByVal _pr As Object = "False", _
 Optional ByVal _dr As Object = "False", _
 Optional ByVal _hot As Object = "False", _
 Optional ByVal _pot As Object = "False", _
 Optional ByVal _hpmax As Object = "False", _
 Optional ByVal _powmax As Object = "False", _
 Optional ByVal _ac As Object = "False", _
 Optional ByVal _defmod As Object = "False", _
 Optional ByVal _offmod As Object = "False", _
 Optional ByVal _hpfactor As Object = "False", _
 Optional ByVal _movementrate As Object = "False", _
 Optional ByVal _fishing As Object = "False", _
 Optional ByVal _jewelcrafting As Object = "False", _
 Optional ByVal _armorcrafting As Object = "False", _
 Optional ByVal _weaponcrafting As Object = "False", _
 Optional ByVal _tailoring As Object = "False", _
 Optional ByVal _alchemy As Object = "False", _
 Optional ByVal _carpentry As Object = "False", _
 Optional ByVal _description As Object = "False", _
 Optional ByVal _equiptype As Object = "False", _
 Optional ByVal _attackdamagevalue As Object = "False", _
 Optional ByVal _attackdamagetype As Object = "False", _
 Optional ByVal _unknownvalue1 As Object = "False", _
 Optional ByVal _unknownvalue2 As Object = "False", _
 Optional ByVal _unknownvalue3 As Object = "False", _
 Optional ByVal _unknownvalue4 As Object = "False", _
 Optional ByVal _unknownvalue5 As Object = "False", _
 Optional ByVal _unknownvalue6 As Object = "False", _
 Optional ByVal _unknownvalue7 As Object = "False", _
 Optional ByVal _unknownvalue8 As Object = "False", _
 Optional ByVal _unknownvalue9 As Object = "False", _
 Optional ByVal _unknownvalue10 As Object = "False", _
 Optional ByVal _unknownvalue11 As Object = "False", _
 Optional ByVal _unknownvalue12 As Object = "False", _
 Optional ByVal _unknownvalue13 As Object = "False", _
 Optional ByVal _controlrestrictionoptions As Object = "False", _
 Optional ByVal _fut1 As Object = "False", _
 Optional ByVal _fut2 As Object = "False", _
 Optional ByVal _fut3 As Object = "False", _
 Optional ByVal _pattern_family As Object = "False", _
 Optional ByVal _pattern_res1 As Object = "False", _
 Optional ByVal _pattern_res2 As Object = "False", _
 Optional ByVal _questcatelognumber As Object = "False", _
 Optional ByVal _questres1 As Object = "False", _
 Optional ByVal _questres2 As Object = "False", _
 Optional ByVal _questres3 As Object = "False", _
 Optional ByVal _effects() As String = Nothing)
        ', Optional ByVal _ As String = "" 

        If _value = "" Then
            _value = "0"
        End If
        If _maxStack = "" Then
            _maxStack = "False"
        End If
        If _level = "" Then
            _level = "False"
        End If
        If _itemHP = "" Then
            _itemHP = "False"
        End If
        If _durability = "" Then
            _durability = "False"
        End If
        If _icon = "" Then
            _icon = "HAS ICON"
        End If
        If _graphic1 = "" Then
            _graphic1 = "HAS GRAPHIC"
        End If
        If _gr1 = "" Then
            _gr1 = "False"
        End If
        If _gr2 = "" Then
            _gr2 = "False"
        End If
        If _gr3 = "" Then
            _gr3 = "False"
        End If
        If _red = "" Then
            _red = "False"
        End If
        If _green = "" Then
            _green = "False"
        End If
        If _blue = "" Then
            _blue = "False"
        End If
        If _rgbff = "" Then
            _rgbff = "FF"
        End If
        If _ukrgb1 = "" Then
            _ukrgb1 = "False"
        End If
        If _ukrgb2 = "" Then
            _ukrgb2 = "False"
        End If
        If _ukrgb3 = "" Then
            _ukrgb3 = "False"
        End If
        If _ukrgb4 = "" Then
            _ukrgb4 = "False"
        End If
        If _ukrgb5 = "" Then
            _ukrgb5 = "False"
        End If
        'If _NORENT = "" Then
        '    _NORENT = "False"
        'End If
        'If _NOTRADE = "" Then
        '    _NOTRADE = "False"
        'End If
        'If _LORE = "" Then
        '    _LORE = "False"
        'End If
        'If _CRAFTABLE = "" Then
        '    _CRAFTABLE = "False"
        'End If
        'If _darkelf = "" Then
        '    _darkelf = "0"
        'End If
        'If _troll = "" Then
        '    _troll = "0"
        'End If
        'If _ogre = "" Then
        '    _ogre = "0"
        'End If
        'If _elf = "" Then
        '    _elf = "0"
        'End If
        'If _dwarf = "" Then
        '    _dwarf = "0"
        'End If
        'If _gnome = "" Then
        '    _gnome = "0"
        'End If
        'If _halfling = "" Then
        '    _halfling = "0"
        'End If
        'If _erudite = "" Then
        '    _erudite = "0"
        'End If
        'If _human = "" Then
        '    _human = "0"
        'End If
        'If _barbarian = "" Then
        '    _barbarian = "0"
        'End If
        'If _warrior = "" Then
        '    _warrior = "0"
        'End If
        'If _paladin = "" Then
        '    _paladin = "0"
        'End If
        'If _shadowknight = "" Then
        '    _shadowknight = "0"
        'End If
        'If _enchanter = "" Then
        '    _enchanter = "0"
        'End If
        'If _magician = "" Then
        '    _magician = "0"
        'End If
        'If _wizard = "" Then
        '    _wizard = "0"
        'End If
        'If _alchemist = "" Then
        '    _alchemist = "0"
        'End If
        'If _necromancer = "" Then
        '    _necromancer = "0"
        'End If
        'If _monk = "" Then
        '    _monk = "0"
        'End If
        'If _rogue = "" Then
        '    _rogue = "0"
        'End If
        'If _ranger = "" Then
        '    _ranger = "0"
        'End If
        'If _bard = "" Then
        '    _bard = "0"
        'End If
        'If _druid = "" Then
        '    _druid = "0"
        'End If
        'If _shaman = "" Then
        '    _shaman = "0"
        'End If
        'If _cleric = "" Then
        '    _cleric = "0"
        'End If
        If _strength = "" Then
            _strength = "False"
        End If
        If _stamina = "" Then
            _stamina = "False"
        End If
        If _agility = "" Then
            _agility = "False"
        End If
        If _dexterity = "" Then
            _dexterity = "False"
        End If
        If _wisdom = "" Then
            _wisdom = "False"
        End If
        If _intelligence = "" Then
            _intelligence = "False"
        End If
        If _charisma = "" Then
            _charisma = "False"
        End If
        If _fr = "" Then
            _fr = "False"
        End If
        If _cr = "" Then
            _cr = "False"
        End If
        If _lr = "" Then
            _lr = "False"
        End If
        If _ar = "" Then
            _ar = "False"
        End If
        If _pr = "" Then
            _pr = "False"
        End If
        If _dr = "" Then
            _dr = "False"
        End If
        If _hot = "" Then
            _hot = "False"
        End If
        If _pot = "" Then
            _pot = "False"
        End If
        If _hpmax = "" Then
            _hpmax = "False"
        End If
        If _powmax = "" Then
            _powmax = "False"
        End If
        If _ac = "" Then
            _ac = "False"
        End If
        If _defmod = "" Then
            _defmod = "False"
        End If
        If _offmod = "" Then
            _offmod = "False"
        End If
        If _hpfactor = "" Then
            _hpfactor = "False"
        End If
        If _movementrate = "" Then
            _movementrate = "False"
        End If
        If _fishing = "" Then
            _fishing = "False"
        End If
        If _jewelcrafting = "" Then
            _jewelcrafting = "False"
        End If
        If _armorcrafting = "" Then
            _armorcrafting = "False"
        End If
        If _weaponcrafting = "" Then
            _weaponcrafting = "False"
        End If
        If _tailoring = "" Then
            _tailoring = "False"
        End If
        If _alchemy = "" Then
            _alchemy = "False"
        End If
        If _carpentry = "" Then
            _carpentry = "False"
        End If
        If _description = "" Then
            _description = "False"
        End If
        If _equiptype = "" Then
            _equiptype = "False"
        End If
        If _attackdamagevalue = "" Then
            _attackdamagevalue = "False"
        End If
        If _attackdamagetype = "" Then
            _attackdamagetype = "False"
        End If

        If _unknownvalue1 = "" Then
            _unknownvalue1 = "0False0"
        End If
        If _unknownvalue2 = "" Then
            _unknownvalue2 = "0False0"
        End If
        If _unknownvalue3 = "" Then
            _unknownvalue3 = "0False0"
        End If
        If _unknownvalue4 = "" Then
            _unknownvalue4 = "0False0"
        End If
        If _unknownvalue5 = "" Then
            _unknownvalue5 = "0False0"
        End If
        If _unknownvalue6 = "" Then
            _unknownvalue6 = "0False0"
        End If
        If _unknownvalue7 = "" Then
            _unknownvalue7 = "0False0"
        End If
        If _unknownvalue8 = "" Then
            _unknownvalue8 = "0False0"
        End If
        If _unknownvalue9 = "" Then
            _unknownvalue9 = "0False0"
        End If
        If _unknownvalue10 = "" Then
            _unknownvalue10 = "0False0"
        End If
        If _unknownvalue11 = "" Then
            _unknownvalue11 = "0False0"
        End If
        If _unknownvalue12 = "" Then
            _unknownvalue12 = "0False0"
        End If
        If _unknownvalue13 = "" Then
            _unknownvalue13 = "0False0"
        End If
        If _controlrestrictionoptions = "" Then
            _controlrestrictionoptions = False
        End If
        If _fut1 = "" Then
            _fut1 = "0False0"
        End If
        If _fut2 = "" Then
            _fut2 = "0False0"
        End If
        If _fut3 = "" Then
            _fut3 = "0False0"
        End If
        If _questcatelognumber = "" Then
            _questcatelognumber = "False"
        End If
        If _questres1 = "" Then
            _questres1 = "0False0"
        End If
        If _questres2 = "" Then
            _questres2 = "0False0"
        End If
        If _questres3 = "" Then
            _questres3 = "0False0"
        End If

        Dim FileVersionToCreate As String = "0.1.0"
        Select Case FileVersionToCreate
            Case "0.1.0"
                'ItemFile = New TextFileClass(lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile, "--")
                ItemFile.FilePath = lb.Folder_Custom_Items & nameSafe & lb.Extension_ItemFile
                ItemFile.WriteLine("-- File Version")
                ItemFile.WriteLine("0.1.0")
                ItemFile.WriteLine("-- Safe Name")
                ItemFile.WriteLine(nameSafe)
                ItemFile.WriteLine("-- Game Name")
                ItemFile.WriteLine(nameGame)
                ItemFile.WriteLine("-- inventory category")
                ItemFile.WriteLine(_inventoryCategory)
                ItemFile.WriteLine("-- value")
                ItemFile.WriteLine(_value)
                ItemFile.WriteLine("-- max stack")
                ItemFile.WriteLine(_maxStack)
                ItemFile.WriteLine("-- level")
                ItemFile.WriteLine(_level)
                ItemFile.WriteLine("-- item hp")
                ItemFile.WriteLine(_itemHP)
                ItemFile.WriteLine("-- durability")
                ItemFile.WriteLine(_durability)
                ItemFile.WriteLine("-- icon")
                ItemFile.WriteLine(_icon)
                ItemFile.WriteLine("-- graphic 1")
                ItemFile.WriteLine(_graphic1)
                ItemFile.WriteLine("-- gr1")
                ItemFile.WriteLine(_gr1)
                ItemFile.WriteLine("-- gr2")
                ItemFile.WriteLine(_gr2)
                ItemFile.WriteLine("-- gr3")
                ItemFile.WriteLine(_gr3)
                ItemFile.WriteLine("-- red")
                ItemFile.WriteLine(_red)
                ItemFile.WriteLine("-- green")
                ItemFile.WriteLine(_green)
                ItemFile.WriteLine("-- blue")
                ItemFile.WriteLine(_blue)
                ItemFile.WriteLine("-- rgbff")
                ItemFile.WriteLine(_rgbff)
                ItemFile.WriteLine("-- ukrgb1")
                ItemFile.WriteLine(_ukrgb1)
                ItemFile.WriteLine("-- ukrgb2")
                ItemFile.WriteLine(_ukrgb2)
                ItemFile.WriteLine("-- ukrgb3")
                ItemFile.WriteLine(_ukrgb3)
                ItemFile.WriteLine("-- ukrgb4")
                ItemFile.WriteLine(_ukrgb4)
                ItemFile.WriteLine("-- ukrgb5")
                ItemFile.WriteLine(_ukrgb5)
                ItemFile.WriteLine("--  NO RENT")
                ItemFile.WriteLine(_NORENT)
                ItemFile.WriteLine("--  NO TRADE")
                ItemFile.WriteLine(_NOTRADE)
                ItemFile.WriteLine("--  LORE")
                ItemFile.WriteLine(_LORE)
                ItemFile.WriteLine("--  CRAFTABLE")
                ItemFile.WriteLine(_CRAFTABLE)

                ItemFile.WriteLine("--  dark elf")
                ItemFile.WriteLine(_darkelf)
                ItemFile.WriteLine("--  troll")
                ItemFile.WriteLine(_troll)
                ItemFile.WriteLine("--  ogre")
                ItemFile.WriteLine(_ogre)
                ItemFile.WriteLine("--  elf")
                ItemFile.WriteLine(_elf)
                ItemFile.WriteLine("--  dwarf")
                ItemFile.WriteLine(_dwarf)
                ItemFile.WriteLine("--  gnome")
                ItemFile.WriteLine(_gnome)
                ItemFile.WriteLine("--  halfling")
                ItemFile.WriteLine(_halfling)
                ItemFile.WriteLine("--  erudite")
                ItemFile.WriteLine(_erudite)
                ItemFile.WriteLine("--  human")
                ItemFile.WriteLine(_human)
                ItemFile.WriteLine("--  barbarian")
                ItemFile.WriteLine(_barbarian)

                ItemFile.WriteLine("--  warrior")
                ItemFile.WriteLine(_warrior)
                ItemFile.WriteLine("--  paladin")
                ItemFile.WriteLine(_paladin)
                ItemFile.WriteLine("--  shadowknight")
                ItemFile.WriteLine(_shadowknight)
                ItemFile.WriteLine("--  enchanter")
                ItemFile.WriteLine(_enchanter)
                ItemFile.WriteLine("--  magician")
                ItemFile.WriteLine(_magician)
                ItemFile.WriteLine("--  wizard")
                ItemFile.WriteLine(_wizard)
                ItemFile.WriteLine("--  alchemist")
                ItemFile.WriteLine(_alchemist)
                ItemFile.WriteLine("--  necromancer")
                ItemFile.WriteLine(_necromancer)
                ItemFile.WriteLine("--  monk")
                ItemFile.WriteLine(_monk)
                ItemFile.WriteLine("--  rogue")
                ItemFile.WriteLine(_rogue)
                ItemFile.WriteLine("--  ranger")
                ItemFile.WriteLine(_ranger)
                ItemFile.WriteLine("--  bard")
                ItemFile.WriteLine(_bard)
                ItemFile.WriteLine("--  druid")
                ItemFile.WriteLine(_druid)
                ItemFile.WriteLine("--  shaman")
                ItemFile.WriteLine(_shaman)
                ItemFile.WriteLine("--  cleric")
                ItemFile.WriteLine(_cleric)

                ItemFile.WriteLine("--  strength")
                ItemFile.WriteLine(_strength)
                ItemFile.WriteLine("--  stamina")
                ItemFile.WriteLine(_stamina)
                ItemFile.WriteLine("--  agility")
                ItemFile.WriteLine(_agility)
                ItemFile.WriteLine("--  dexterity")
                ItemFile.WriteLine(_dexterity)
                ItemFile.WriteLine("--  wisdom")
                ItemFile.WriteLine(_wisdom)
                ItemFile.WriteLine("--  intelligence")
                ItemFile.WriteLine(_intelligence)
                ItemFile.WriteLine("--  charisma")
                ItemFile.WriteLine(_charisma)
                ItemFile.WriteLine("--  fr")
                ItemFile.WriteLine(_fr)
                ItemFile.WriteLine("--  cr")
                ItemFile.WriteLine(_cr)
                ItemFile.WriteLine("--  lr")
                ItemFile.WriteLine(_lr)
                ItemFile.WriteLine("--  ar")
                ItemFile.WriteLine(_ar)
                ItemFile.WriteLine("--  pr")
                ItemFile.WriteLine(_pr)
                ItemFile.WriteLine("--  dr")
                ItemFile.WriteLine(_dr)
                ItemFile.WriteLine("--  hot")
                ItemFile.WriteLine(_hot)
                ItemFile.WriteLine("--  pot")
                ItemFile.WriteLine(_pot)
                ItemFile.WriteLine("--  hpmax")
                ItemFile.WriteLine(_hpmax)
                ItemFile.WriteLine("--  powmax")
                ItemFile.WriteLine(_powmax)
                ItemFile.WriteLine("--  ac")
                ItemFile.WriteLine(_ac)

                ItemFile.WriteLine("--  defmod")
                ItemFile.WriteLine(_defmod)
                ItemFile.WriteLine("--  off mod")
                ItemFile.WriteLine(_offmod)
                ItemFile.WriteLine("--  hp factor")
                ItemFile.WriteLine(_hpfactor)
                ItemFile.WriteLine("--  movement rate")
                ItemFile.WriteLine(_movementrate)
                ItemFile.WriteLine("--  fishing")
                ItemFile.WriteLine(_fishing)
                ItemFile.WriteLine("--  jewel crafting")
                ItemFile.WriteLine(_jewelcrafting)
                ItemFile.WriteLine("--  armor crafting")
                ItemFile.WriteLine(_armorcrafting)
                ItemFile.WriteLine("--  weapon crafting")
                ItemFile.WriteLine(_weaponcrafting)
                ItemFile.WriteLine("--  tailoring")
                ItemFile.WriteLine(_tailoring)
                ItemFile.WriteLine("--  alchemy")
                ItemFile.WriteLine(_alchemy)
                ItemFile.WriteLine("--  carpentry")
                ItemFile.WriteLine(_carpentry)

                ItemFile.WriteLine("-- description")
                ItemFile.WriteLine(_description)
                ItemFile.WriteLine("-- equip type")
                ItemFile.WriteLine(_equiptype)
                ItemFile.WriteLine("-- attack damage value")
                ItemFile.WriteLine(_attackdamagevalue)
                ItemFile.WriteLine("-- attack damage type")
                ItemFile.WriteLine(_attackdamagetype)
                ItemFile.WriteLine("-- unknown value 1")
                ItemFile.WriteLine(_unknownvalue1)
                ItemFile.WriteLine("-- unknown value 2")
                ItemFile.WriteLine(_unknownvalue2)
                ItemFile.WriteLine("-- unknown value 3")
                ItemFile.WriteLine(_unknownvalue3)
                ItemFile.WriteLine("-- unknown value 4")
                ItemFile.WriteLine(_unknownvalue4)
                ItemFile.WriteLine("-- unknown value 5")
                ItemFile.WriteLine(_unknownvalue5)
                ItemFile.WriteLine("-- unknown value 6")
                ItemFile.WriteLine(_unknownvalue6)
                ItemFile.WriteLine("-- unknown value 7")
                ItemFile.WriteLine(_unknownvalue7)
                ItemFile.WriteLine("-- unknown value 8")
                ItemFile.WriteLine(_unknownvalue8)
                ItemFile.WriteLine("-- unknown value 9")
                ItemFile.WriteLine(_unknownvalue9)
                ItemFile.WriteLine("-- unknown value 10")
                ItemFile.WriteLine(_unknownvalue10)
                ItemFile.WriteLine("-- unknown value 11")
                ItemFile.WriteLine(_unknownvalue11)
                ItemFile.WriteLine("-- unknown value 12")
                ItemFile.WriteLine(_unknownvalue12)
                ItemFile.WriteLine("-- unknown value 13")
                ItemFile.WriteLine(_unknownvalue13)
                ItemFile.WriteLine("-- control restriction options")
                ItemFile.WriteLine(_controlrestrictionoptions)
                ItemFile.WriteLine("-- fut1")
                ItemFile.WriteLine(_fut1)
                ItemFile.WriteLine("-- fut2")
                ItemFile.WriteLine(_fut2)
                ItemFile.WriteLine("-- fut3")
                ItemFile.WriteLine(_fut3)

                ItemFile.WriteLine("-- pattern family")
                ItemFile.WriteLine(_pattern_family)
                ItemFile.WriteLine("-- pattern res1")
                ItemFile.WriteLine(_pattern_res1)
                ItemFile.WriteLine("-- pattern res2")
                ItemFile.WriteLine(_pattern_res2)

                ItemFile.WriteLine("-- quest catelog number")
                ItemFile.WriteLine(_questcatelognumber)
                ItemFile.WriteLine("-- quest res 1")
                ItemFile.WriteLine(_questres1)
                ItemFile.WriteLine("-- quest res 2")
                ItemFile.WriteLine(_questres2)
                ItemFile.WriteLine("-- quest res 3")
                ItemFile.WriteLine(_questres3)
                Dim i As Integer = 0
                For Each line In _effects
                    i = i + 1
                    ItemFile.WriteLine("-- effect number " & i)
                    ItemFile.WriteLine(line)
                Next
                ItemFile.SaveFile()
                ItemFile.LoadFile()
        End Select
    End Sub
    Public Function GetEffectsFromItemFile() As String()
        Return TextFileClass_ReturnFromTextToEnd(ItemFile, "First Effect")
    End Function
    Public Sub AddEffectToLoadedItemFile(ByVal ItemFileToAdd As String)
        TextFileClass_AddLineToEndOfFile(ItemFileToAdd, ItemFile)
    End Sub
    Public Sub RemoveEffectFromLoadedItemFile(ByVal nameSafe As String)
        ItemFile.Line(ItemFile.GetIndexByValue(nameSafe, ItemFile.GetIndexByAdditionalData("First Effect"))) = ""
        ItemFile.SaveFile()
    End Sub
#End Region
End Class
