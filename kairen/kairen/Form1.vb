Imports System.IO
Public Class Form1
    Dim MyIO As FilePath
    Dim RI() As RI
    Public IO_RootAsDesktop As String = My.Computer.FileSystem.SpecialDirectories.Desktop & "\"
    Public IO_RootAsUserData As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\..\..\..\Rundatshityo\"

    'Public IO_Root As String = IO_RootAsDesktop
    Public IO_Root As String = IO_RootAsUserData
    Public IO_EQOA As String = IO_Root & "EQOA\"
    Public IO_LUAs As String = IO_EQOA & "LUAs\"

    Private Installed_DualBox_CheatTable As Boolean
    Private Installed_DualBox_LUA1 As Boolean
    Private Installed_DualBox_LUA2 As Boolean

    Private Installed_Ghosts As Boolean
    Private Installed_NPCs As Boolean

    Dim DualBoxFilesInstalled As Boolean = False
    Dim NPCMakerFilesInstalled As Boolean = False
    Dim MainCTInstalled As Boolean = False

    Dim TabControlStartHeight As Integer
    Dim TabControlSmallHeight As Integer = 191
    Dim TabControlBigHeight As Integer = 436
    Dim DirectionStart As Integer
    Dim AllowNPCAreaNPCLists_TobeChanged As Boolean = True
    Dim DirectionsBoxWidth_Full As Integer = 1057
    Dim DirectionsBoxWidth_Half As Integer = 526
    Dim DirectionsBoxWidth_Temp As Integer

    Dim FileHandler_FileIsChangable() As Boolean
    Dim FileHandler_FileName() As String
    Dim FileHandler_FileIsInstallable() As Boolean
    Dim FileHandler_FileIsUnInstallable() As Boolean

    Dim ProgramVersion As String = "Version Beta 3.4.1"
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Text = "BETA: EQOA SS Program by Rundatshityo - " & ProgramVersion & " [April 14 2016 2:14AM] [uv45hyvh4vcv]"
        If IO_Root = IO_RootAsDesktop Then
            MsgBox("In Desktop Mode, Be Careful!!", MsgBoxStyle.Exclamation, "Warning! ! !")
            MsgBox("And why do you have this, anyway?", MsgBoxStyle.Critical, " lesigh . . .    v.v  ")
            CheckedListBox1.Enabled = False
        End If
        'If DE(IO_Root) = False Then My.Computer.FileSystem.CreateDirectory(IO_Root)
        MyIO = New FilePath
        TabControlStartHeight = TabControl1.Size.Height

        'Setup Directions Area Size
        DirectionStart = Directions.Height
        Directions.Width = DirectionsBoxWidth_Full

        DoFileCheck()
        'CheckForFiles()
        MainOnLoad()

        DualBoxOnLoad()
        'NPCMakerOnLoad()
        SpawnMakerOnLoad()
        NPCMakerOnLoad()
        PersonalBugTrackerOnLoad()

        RightArrow_Click(sender, e) 'sets the help text from 0 -> 1

        'Load Online Form
        'LoadOnlineForm("Poppledon")
    End Sub

#Region "Main"
    Private Sub MainOnLoad()

    End Sub
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        InstallFiles()
        CheckForFiles()
    End Sub 'install everything
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        Dim Result As MsgBoxResult
        Result = MsgBox("Are you sure you want to uninstall everything? You will dete all of your custom data too.", MsgBoxStyle.OkCancel, "Confirm Delete")
        If Result = MsgBoxResult.Cancel Then Exit Sub
        Try
            'System.IO.Directory.Delete(IO_Root, True)
        Catch ex As Exception

        End Try
        CheckForFiles()
    End Sub 'uninstall everything
    Private Sub DoFileCheck()
        'check for and establish if this program should auto-update
        'check for and create folders
        'check for and create luas, move old versions to archive
        'check for and create CE & other files
    End Sub
    Private Sub CheckForFiles(Optional ByVal FileGroup As String = "All")
        'Check for : 
        ' Folders : Zones, 
        ' Ghosts
        ' CE File
        ' LUA Files
        Dim CF = IO_EQOA & "LUAs\" ' Current Folder
        FileHandler("check", IO_Root, "folder", "Root Folder", "", True, True, False)
        FileHandler("check", IO_Root & "Cheat Engine\", "folder", "Cheat Engine Folder", "", True, True, False)
        FileHandler("check", IO_Root & "Cheat Engine\MainTable.ct", "file", "MainTable", "Cheat Engine Table", True, True, True)

        FileHandler("check", IO_EQOA, "folder", "EQOA Folder", "", True, True, False)

        FileHandler("check", IO_EQOA & "Game Data\", "folder", "Game Data Folder", "", True, True, False)
        FileHandler("check", CF, "folder", "LUA Folder", "", True, True, False)

        Dim word, ext As String
        ext = ".lua"
        word = "Main"
        FileHandler("check", CF & word & ext, "file", word, "LUA - " & word & ext, True, True, False)
        word = "File Paths"
        FileHandler("check", CF & word & ext, "file", word, "LUA - " & word & ext, True, True, False)
        word = "Locations"
        FileHandler("check", CF & word & ext, "file", word, "LUA - " & word & ext, True, True, False)
        word = "Box1"
        FileHandler("check", CF & word & ext, "file", word, "LUA - " & word & ext, True, True, True)
        word = "Box2"
        FileHandler("check", CF & word & ext, "file", word, "LUA - " & word & ext, True, True, True)
        word = "NPC Maker"
        FileHandler("check", CF & "Modes\", "folder", "Modes Folder", "", True, True, False)
        FileHandler("check", CF & "Modes\" & word & ext, "file", word, "LUA Mode - " & word, True, True, False)

        CF = IO_EQOA & "Game Data\Ghosts\"
        FileHandler("check", CF, "folder", "Ghosts Folder", "", True, True, False)
        word = ""
        ext = ".txt"
        Dim i As Integer = 1
        Dim name(24) As String
        name(1) = "Coachman_Ronks"
        name(2) = "Aloj_Tilsteran"
        name(3) = "Merchant_Kari"
        name(4) = "Dr_Killian"
        name(5) = "Guard_Saolen"
        name(6) = "Guard_Jahn"
        name(7) = "Bowyer_Koll"
        name(8) = "Tailor_Bariston"
        name(9) = "Tailor_Zixar"
        name(10) = "Guard_Serenda"
        name(11) = "a_badger"
        name(12) = "an_undead_mammoth"
        name(13) = "Angry_Patron"
        name(14) = "Arch_Familiar"
        name(15) = "Finalquestt"
        name(16) = "Guard_Perinen"
        name(17) = "Manoarmz"
        name(18) = "Marona_Jofranka"
        name(19) = "Merchant_Ahkham"
        name(20) = "Nukenurplace"
        name(21) = "Raam"
        name(22) = "Royce_Tilsteran"
        name(23) = "sign_post"
        Do Until i >= 24
            word = name(i)
            FileHandler("check", CF & word & ext, "file", word, "Ghost - " & word & ext, True, True, True)
            i += 1
        Loop
        CF = IO_EQOA & "Game Data\"
        FileHandler("check", CF & "NPCs\", "folder", "NPC Folder", "", True, True, False)
        FileHandler("check", CF & "NPCs\NPC Areas\", "folder", "NPC Areas Folder", "", True, True, False)
        FileHandler("check", CF & "NPCs\NPC Areas\Zones\", "folder", "Area Zones", "", True, True, False)
        CF = IO_EQOA & "Temp\"
        FileHandler("check", CF, "folder", "Temp Folder", "", True, True, False)
        FileHandler("check", CF & "NPC Maker\", "folder", "Temp NPC Maker", "", True, True, False)
        FileHandler("check", CF & "NPC Maker\NPC Areas\", "folder", "Temp NPC Areas", "", True, True, False)
        CF = IO_EQOA & "Custom Data\"
        FileHandler("check", CF, "folder", "Custom Data", "", True, True, False)
        FileHandler("check", CF & "NPC Maker\", "folder", "Custom NPC Maker", "", True, True, False)
        FileHandler("check", CF & "NPC Maker\NPC Areas\", "folder", "Custom NPC Areas", "", True, True, False)
        FileHandler("check", CF & "NPC Maker\NPC Areas\Zones\", "folder", "Custom Area Zones", "", True, True, False)
    End Sub
    Private Sub FileHandler(ByVal _option As String, ByVal _path As Object, ByVal _type As String, ByVal _name As String, Optional ByVal _text As String = "", Optional ByVal _isChangable As Boolean = False, Optional ByVal _isInstallable As Boolean = False, Optional ByVal _isUninstallable As Boolean = False)
        If _text = "" Then _text = _name

        Select Case _option
            Case Is = "check"
                Dim i As Integer = CheckedListBox1.Items.Count
                ReDim Preserve FileHandler_FileIsChangable(i)
                ReDim Preserve FileHandler_FileName(i)
                FileHandler_FileIsChangable(i) = True
                FileHandler_FileName(i) = _path

                ReDim Preserve RI(i)
                RI(i) = New RI
                RI(i).Name = _name
                RI(i).Path = _path
                RI(i).Type = _type
                RI(i).IsChangeable = True
                RI(i).IsInstallable = _isInstallable
                RI(i).IsUninstallable = _isUninstallable
                CheckedListBox1.Items.Add(_text & " : \Rundatshityo\" & _path.replace(IO_Root, ""), RI(i).Exists)
                RI(i).IsChangeable = _isChangable

                If RI(i).Name = "MainTable" Then
                    d_all_btn_StartCE.Enabled = RI(i).Exists
                End If

                'Select Case _type
                'Case Is = "folder"
                'Dim i As Integer = CheckedListBox1.Items.Count
                'ReDim Preserve FileHandler_FileIsChangable(i)
                'FileHandler_FileIsChangable(i) = True
                'CheckedListBox1.Items.Add(_text & " : " & _path, DE(_path))
                'FileHandler_FileIsChangable(i) = _isChangable
                'Case Is = "file"
                'CheckedListBox1.Items.Add(_text & " : " & _path, FE(_path))
                'End Select
                FileHandler_FileIsChangable(i) = _isChangable
            Case Is = "install"
                Dim location As String
                Select Case _name
                    Case Is = "Coachman_Ronks"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Coachman_Ronks", ".txt", My.Resources.Coachman_Ronks)
                    Case Is = "Aloj_Tilsteran"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Aloj_Tilsteran", ".txt", My.Resources.Aloj_Tilsteran)
                    Case Is = "Merchant_Kari"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Merchant_Kari", ".txt", My.Resources.Merchant_Kari)
                    Case Is = "Dr_Killian"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Dr_Killian", ".txt", My.Resources.Dr_Killian)
                    Case Is = "Guard_Saolen"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Guard_Saolen", ".txt", My.Resources.Guard_Saolen)
                    Case Is = "Guard_Jahn"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Guard_Jahn", ".txt", My.Resources.Guard_Jahn)
                    Case Is = "Bowyer_Koll"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Bowyer_Koll", ".txt", My.Resources.Bowyer_Koll)
                    Case Is = "Tailor_Bariston"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Tailor_Bariston", ".txt", My.Resources.Tailor_Bariston)
                    Case Is = "Tailor_Zixar"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Tailor_Zixar", ".txt", My.Resources.Tailor_Zixar)
                    Case Is = "Guard_Serenda"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Guard_Serenda", ".txt", My.Resources.Guard_Serenda)
                    Case Is = "a_badger"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "a_badger", ".txt", My.Resources.a_badger)
                    Case Is = "an_undead_mammoth"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "an_undead_mammoth", ".txt", My.Resources.an_undead_mammoth)
                    Case Is = "Angry_Patron"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Angry_Patron", ".txt", My.Resources.Angry_Patron)
                    Case Is = "Arch_Familiar"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Arch_Familiar", ".txt", My.Resources.Arch_Familiar)
                    Case Is = "Finalquestt"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Finalquestt", ".txt", My.Resources.Finalquestt)
                    Case Is = "Guard_Perinen"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Guard_Perinen", ".txt", My.Resources.Guard_Perinen)
                    Case Is = "Manoarmz"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Manoarmz", ".txt", My.Resources.Manoarmz)
                    Case Is = "Marona_Jofranka"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Marona_Jofranka", ".txt", My.Resources.Marona_Jofranka)
                    Case Is = "Merchant_Ahkham"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Merchant_Ahkham", ".txt", My.Resources.Merchant_Ahkham)
                    Case Is = "Nukenurplace"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Nukenurplace", ".txt", My.Resources.Nukenurplace)
                    Case Is = "Raam"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Raam", ".txt", My.Resources.Raam)
                    Case Is = "Royce_Tilsteran"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "Royce_Tilsteran", ".txt", My.Resources.Royce_Tilsteran)
                    Case Is = "sign_post"
                        location = IO_EQOA & "Game Data\Ghosts\"
                        SaveFile(location, "sign_post", ".txt", My.Resources.sign_post)

                    Case Is = "Root Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_Root)
                    Case Is = "Cheat Engine Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_Root & "Cheat Engine\")
                    Case Is = "MainTable"
                        location = IO_Root & "Cheat Engine\"
                        SaveFile(location, "MainTable", ".ct", My.Resources.MainTable)
                        d_all_btn_StartCE.Enabled = True
                    Case Is = "EQOA Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA)
                    Case Is = "LUA Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_LUAs)
                    Case Is = "Modes Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_LUAs & "Modes\")
                    Case Is = "NPC Maker"
                        location = IO_LUAs & "Modes\"
                        SaveFile(location, "NPC Maker", ".lua", My.Resources.NPC_Maker)
                    Case Is = "Main"
                        location = IO_LUAs
                        SaveFile(location, "Main", ".lua", My.Resources.Main)
                    Case Is = "File Paths"
                        location = IO_LUAs
                        SaveFile(location, "File Paths", ".lua", My.Resources.File_Paths)
                    Case Is = "Locations"
                        location = IO_LUAs
                        SaveFile(location, "Locations", ".lua", My.Resources.Locations)
                    Case Is = "Box1"
                        location = IO_LUAs
                        SaveFile(location, "Box1", ".lua", My.Resources.Box1)
                    Case Is = "Box2"
                        location = IO_LUAs
                        SaveFile(location, "Box2", ".lua", My.Resources.Box2)
                    Case Is = "Game Data Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Game Data\")
                    Case Is = "Ghosts Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Game Data\Ghosts\")
                    Case Is = "NPC Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Game Data\NPCs\")
                    Case Is = "NPC Areas Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Game Data\NPCs\NPC Areas\")
                    Case Is = "Area Zones"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Game Data\NPCs\NPC Areas\Zones\")
                    Case Is = "Temp Folder"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Temp\")
                    Case Is = "Temp NPC Maker"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Temp\NPC Maker\")
                    Case Is = "Temp NPC Areas"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Temp\NPC Maker\NPC Areas\")
                    Case Is = "Custom Data"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Custom Data\")
                    Case Is = "Custom NPC Maker"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Custom Data\NPC Maker\")
                    Case Is = "Custom NPC Areas"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\")
                    Case Is = "Custom Area Zones"
                        My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\")
                    Case Else
                        MsgBox("Nothing for '" & _name & "'")
                        Dim tempfile As String = IO_RootAsDesktop & "EQOA\Temp\Awesome Output.txt"
                        Dim addoldlines As Boolean = False
                        Dim readline() As String
                        Dim newline As String = "Case Is = " & Chr(34) & _name & Chr(34)
                        Dim i As Integer = 0
                        If FE(tempfile) Then
                            Dim sr As New IO.StreamReader(tempfile)
                            Do Until sr.EndOfStream()
                                ReDim Preserve readline(i)
                                readline(i) = sr.ReadLine()
                                If readline(i) = newline Then
                                    sr.Close()
                                    Exit Sub
                                End If
                                i = i + 1
                            Loop
                            sr.Close()
                            addoldlines = True
                        End If
                        i = 0
                        Dim sw As New IO.StreamWriter(tempfile)
                        If addoldlines = True Then
                            Do Until i = readline.Length
                                sw.WriteLine(readline(i))
                                i = i + 1
                            Loop
                        End If
                        sw.WriteLine(newline)
                        sw.Close()
                End Select
            Case Is = "uninstall"
                Dim i As Integer = 0
                Do Until RI(i).Name = _name Or RI(i) Is Nothing
                    i = i + 1
                Loop
                If RI(i) Is Nothing Then
                    MsgBox("Error: RI(i) Is Nothing in FileHandler()")
                    Exit Sub
                End If
                If RI(i).Name = "MainTable" Then
                    d_all_btn_StartCE.Enabled = False
                End If
                RI(i).Uninstall()
        End Select
    End Sub

    Public Sub SaveFile(ByVal _path As String, ByVal _name As String, ByVal _extension As String, ByVal resourcefile As Object)
        If DE(_path) = False Then
            My.Computer.FileSystem.CreateDirectory(_path)
        End If
        Dim b() As Byte = resourcefile 'Name of file in resources
        IO.File.WriteAllBytes(_path & _name & _extension, b)
    End Sub


    Private Sub CheckedListBox2_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox2.ItemCheck

    End Sub

#End Region
#Region "Dual Box Setup"
    Private Sub DualBoxOnLoad()
        'd_dep_Button1.Enabled = False
        'd_dep_Button2.Enabled = False
        'Button3.Enabled = False
    End Sub
#End Region
#Region "NPC Maker"
    Private Sub NPCMakerOnLoad()
        '' Populate Load NPC Drop Down
        'ComboBox3.Items.Add("tr")
        'ComboBox3.SelectedIndex = 0
        ' make a reference to a directory
        If DE(IO_EQOA & "Custom Data\NPC Maker\") = False Then Exit Sub
        Dim di As New IO.DirectoryInfo(IO_EQOA & "Custom Data\NPC Maker\")
        Dim diar1 As IO.FileInfo() = di.GetFiles()
        Dim dra As IO.FileInfo

        ComboBox3.Items.Clear()
        'list the names of all files in the specified directory
        For Each dra In diar1
            If Microsoft.VisualBasic.Right(dra.ToString(), 4) = ".txt" Then
                ComboBox3.Items.Add(Microsoft.VisualBasic.Left(dra.ToString(), dra.ToString().Length - 4))
                ComboBox3.SelectedIndex = 0
            End If
        Next

        di = New IO.DirectoryInfo(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\")
        diar1 = di.GetFiles()
        dra = Nothing

        ComboBox2.Items.Clear()
        'list the names of all files in the specified directory
        For Each dra In diar1
            If Microsoft.VisualBasic.Right(dra.ToString(), 4) = ".txt" Then
                ComboBox2.Items.Add(Microsoft.VisualBasic.Left(dra.ToString(), dra.ToString().Length - 4))
                'ComboBox2.SelectedIndex = 0
            End If
        Next
        '' End Populate Load NPC Drop Down
    End Sub
    Private Sub d_npcmkr_btn_Load_NPC_Click(sender As System.Object, e As System.EventArgs) Handles d_npcmkr_btn_Load_NPC.Click
        ClearNPCMakerForms()
        If My.Computer.FileSystem.FileExists(IO_EQOA & "Custom Data\NPC Maker\" & ComboBox3.SelectedItem & ".txt") = False Then
            MsgBox("File does not exist or could not be located: " & ComboBox3.SelectedItem)
            Exit Sub
        End If
        Dim sr As New IO.StreamReader(IO_EQOA & "Custom Data\NPC Maker\" & ComboBox3.SelectedItem.ToString & ".txt")
        Dim fileVersion As String
        Dim SafeName, GameName, X, Y, Z, F, Race, Gender, vClass, Level, Hp As String

        '0.1.1
        fileVersion = IO_ReadNextLine(sr, "--")

        If fileVersion = "0.1.1" Then
            Label34.Text = fileVersion
            SafeName = IO_ReadNextLine(sr, "--")
            GameName = IO_ReadNextLine(sr, "--")
            X = IO_ReadNextLine(sr, "--")
            Y = IO_ReadNextLine(sr, "--")
            Z = IO_ReadNextLine(sr, "--")
            F = IO_ReadNextLine(sr, "--")
            Race = IO_ReadNextLine(sr, "--")
            Gender = IO_ReadNextLine(sr, "--")
            vClass = IO_ReadNextLine(sr, "--")
            Level = IO_ReadNextLine(sr, "--")
            Hp = IO_ReadNextLine(sr, "--")
        Else
            MsgBox("File Version Not Supported for: " & ComboBox3.SelectedText & "; Version: " & fileVersion)
            sr.Close()
            Exit Sub
        End If
        sr.Close()

        TextBox1.Text = GameName
        TextBox2.Text = SafeName
        TextBox3.Text = X
        TextBox4.Text = Y
        TextBox5.Text = Z
        TextBox6.Text = F
        TextBox7.Text = Level
    End Sub
    Private Sub ClearNPCMakerForms()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        TextBox7.Text = ""
        Label34.Text = "-"
    End Sub
    Private Sub d_npcmkr_btn_Grab_Data_Click(sender As System.Object, e As System.EventArgs) Handles d_npcmkr_btn_Grab_Data.Click
        Label34.Text = "0.1.1"
        Dim x, y, z, f As String
        Dim sr As New IO.StreamReader(IO_EQOA & "Temp\NPC Maker\New_NPC.txt")
        x = sr.ReadLine()
        y = sr.ReadLine()
        z = sr.ReadLine()
        f = sr.ReadLine()
        sr.Close()

        TextBox3.Text = x
        TextBox4.Text = y
        TextBox5.Text = z
        TextBox6.Text = f
    End Sub
    Private Sub d_btn_npcmkr_Make_File_Click(sender As System.Object, e As System.EventArgs) Handles d_npcmkr_btn_Make_File.Click
        Label34.Text = "0.1.1"
        Dim sw As New IO.StreamWriter(IO_EQOA & "Custom Data\NPC Maker\" & TextBox2.Text & ".txt")
        sw.WriteLine("0.1.1")
        sw.WriteLine("-- Safe Name (A " & Chr(34) & "Safe Name" & Chr(34) & " is a name that is able to be a file name.)")
        sw.WriteLine(TextBox2.Text)
        sw.WriteLine("-- In Game Name")
        sw.WriteLine(TextBox1.Text)
        sw.WriteLine("---- World Placement ----")
        sw.WriteLine("--x, y, z, facing,")
        sw.WriteLine(TextBox3.Text)
        sw.WriteLine(TextBox4.Text)
        sw.WriteLine(TextBox5.Text)
        sw.WriteLine(TextBox6.Text)
        sw.WriteLine("---- Characteristics ----")
        sw.WriteLine("-- Race, Gender, Class,")
        sw.WriteLine("Human")
        sw.WriteLine("Male")
        sw.WriteLine("Guard")
        sw.WriteLine("---- Statistics ----")
        sw.WriteLine("-- level, hp, mp, ac,")
        sw.WriteLine(TextBox7.Text)
        sw.WriteLine("255")
        'ComboBox1'graphic
        'TextBox8'class
        'gender

        sw.Close()
        sw = New IO.StreamWriter(IO_EQOA & "Temp\NPC Maker\last file made.txt")
        sw.WriteLine(TextBox1.Text)
        sw.WriteLine(TextBox2.Text)
        sw.Close()
    End Sub
#End Region
#Region "Spawn Maker"
    Private Sub SpawnMakerOnLoad()
        ComboBox4.Items.Clear()
        ListBox1.Items.Clear()
        ComboBox7.SelectedIndex = 0 ' Selects latest file version
        RadioButton1.Checked = True
    End Sub
    Private Sub SpawnMakerSelectVersion()
        Select Case ComboBox7.Text
            Case Is = "1.0"
                If DE(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\") = False Then Exit Sub
                If DE(IO_EQOA & "Custom Data\NPC Maker\") = False Then Exit Sub
                AddFilesToControl(ComboBox4, IO_EQOA & "Custom Data\NPC Maker\NPC Areas\", ".txt")
                AddFilesToControl(ListBox1, IO_EQOA & "Custom Data\NPC Maker\", ".txt")
                ComboBox5.Enabled = False
                ComboBox6.Enabled = False
                ComboBox5.Items.Clear()
                ComboBox6.Items.Clear()
            Case Is = "1.1"
                If DE(IO_EQOA & "Custom Data\NPC Maker\") = False Then Exit Sub
                If DE(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\") = False Then Exit Sub
                AddFilesToControl(ListBox1, IO_EQOA & "Custom Data\NPC Maker\", ".txt")
                Dim lineread As String
                ComboBox5.Items.Clear()
                ComboBox6.Items.Clear()
                ComboBox5.Enabled = True
                ComboBox6.Enabled = True
                'AddFilesToControl(ComboBox4, IO_EQOA & "Custom Data\NPC Maker\NPC Areas\", ".txt", True)
                'AddFilesToControl(ListBox1, IO_EQOA & "Custom Data\NPC Maker\", ".txt", True)
                ''Add all zones to zone list
                Dim Path As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\"
                Dim file As String = Path & "index" & ".txt"
                If FE(file) = True Then
                    Dim i As Integer = 0
                    Dim sr As New IO.StreamReader(file)
                    Do
                        lineread = IO_ReadNextLine(sr, "--")
                    Loop Until lineread = ":zones:" Or sr.EndOfStream

                    Do Until sr.EndOfStream
                        lineread = IO_ReadNextLine(sr, "--")
                        If DE(Path & lineread) Then
                            ComboBox5.Items.Add(lineread)
                            i += 1
                        End If
                    Loop
                    sr.Close()
                End If
            Case Else

        End Select
    End Sub
    Private Sub d_btn_spwnmkr_Grab_Data_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_Grab_Data.Click
        'Label32.Text = "1.0"
        Dim x, y, z, f, Zone, ZoneSub As String
        Dim file As String = IO_Root & "EQOA\Temp\NPC Maker\New_Wall.txt"
        If FE(IO_Root & "EQOA\Temp\NPC Maker\New_Wall.txt") = False Then Exit Sub
        Dim sr As New IO.StreamReader(file)
        x = sr.ReadLine()
        y = sr.ReadLine()
        z = sr.ReadLine()
        Zone = sr.ReadLine()
        ZoneSub = sr.ReadLine()
        sr.Close()
        Dim FilledWallNumber As Integer = 0
        If RadioButton1.Checked = True Then
            TextBox9.Text = x
            TextBox10.Text = y
            TextBox11.Text = z
            RadioButton2.Checked = True
            FilledWallNumber = 1
        ElseIf RadioButton2.Checked = True Then
            TextBox14.Text = x
            TextBox13.Text = y
            TextBox12.Text = z
            RadioButton3.Checked = True
            FilledWallNumber = 2
        ElseIf RadioButton3.Checked = True Then
            TextBox17.Text = x
            TextBox16.Text = y
            TextBox15.Text = z
            RadioButton4.Checked = True
            FilledWallNumber = 3
        ElseIf RadioButton4.Checked = True Then
            TextBox20.Text = x
            TextBox19.Text = y
            TextBox18.Text = z
            RadioButton4.Checked = False
            FilledWallNumber = 4
        End If
        Dim output(4) As String
        output(0) = FilledWallNumber
        output(1) = x
        output(2) = y
        output(3) = z
        ' output(4) = z
        ParseOutsideCommand("Spawn Wall Marker", output)
        If ComboBox5.Items.Contains(Zone) = False Then
            ComboBox5.Items.Add(Zone)
        End If
        If ComboBox6.Items.Contains(ZoneSub) = False Then
            ComboBox6.Items.Add(ZoneSub)
        End If

    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_Save.Click
        Select Case ComboBox7.Text
            Case Is = "1.0"
                If TextBox9.Text = "" Or _
                    TextBox10.Text = "" Or _
                    TextBox11.Text = "" Or _
                    TextBox12.Text = "" Or _
                    TextBox13.Text = "" Or _
                    TextBox14.Text = "" Or _
                    TextBox15.Text = "" Or _
                    TextBox16.Text = "" Or _
                    TextBox17.Text = "" Or _
                    TextBox18.Text = "" Or _
                    TextBox19.Text = "" Or _
                    TextBox20.Text = "" Or _
                    ComboBox5.SelectedItem = Nothing Or _
                    ComboBox6.SelectedItem = Nothing Then
                    Exit Sub
                End If
                If TextBox9.Text = "" Then
                    Button5_Click(sender, e)
                End If
                'Label32.Text = "1.0"
                Dim x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4 As Integer
                Dim Zone, SubZone As String

                x1 = TextBox9.Text
                y1 = TextBox10.Text
                z1 = TextBox11.Text

                x2 = TextBox14.Text
                y2 = TextBox13.Text
                z2 = TextBox12.Text

                x3 = TextBox17.Text
                y3 = TextBox16.Text
                z3 = TextBox15.Text

                x4 = TextBox20.Text
                y4 = TextBox19.Text
                z4 = TextBox18.Text

                Dim leastx, mostx, leasty, mosty, leastz, mostz As String
                Dim order() As String
                order = SortGreatestToLeast(x1, x2, x3, x4)
                leastx = order(0)
                mostx = order(3)

                order = SortGreatestToLeast(y1, y2, y3, y4)
                leasty = order(0)
                mosty = order(3)

                order = SortGreatestToLeast(z1, z2, z3, z4)
                leastz = order(0)
                mostz = order(3)
                Dim file As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\" & TextBox21.Text & ".txt"
                Dim sw As New IO.StreamWriter(file)
                sw.WriteLine("1.0")
                sw.WriteLine(TextBox21.Text)
                sw.WriteLine(ComboBox5.Text)
                sw.WriteLine(leastx)
                sw.WriteLine(mostx)
                sw.WriteLine(leasty)
                sw.WriteLine(mosty)
                sw.WriteLine(leastz - 15000000)
                sw.WriteLine(mostz + 15000000)
                sw.Close()

                sw = New IO.StreamWriter(IO_EQOA & "Temp\NPC Maker\last area made.txt")
                sw.WriteLine(TextBox21.Text)
                sw.Close()
            Case Is = "1.1" ' IO_EQOA & "Custom Data\NPC Maker\NPC Areas\" & ComboBox5.SelectedItem.ToString & "\" & ComboBox6.SelectedItem.ToString & TextBox21.Text & ".txt"
                If TextBox9.Text = "" Or _
                TextBox10.Text = "" Or _
                TextBox11.Text = "" Or _
                TextBox18.Text = "" Or _
                TextBox19.Text = "" Or _
                TextBox20.Text = "" Or _
                ComboBox5.Text = Nothing Or _
                ComboBox6.Text = Nothing Then
                    Exit Sub
                End If
                If TextBox9.Text = "" Then
                    Button5_Click(sender, e)
                End If
                'Label32.Text = "1.0"
                Dim x1, y1, z1, x2, y2, z2, x3, y3, z3, x4, y4, z4 As Integer
                Dim Zone, SubZone As String
                Dim leastx, mostx, leasty, mosty, leastz, mostz As String
                Dim order() As String


                If TextBox9.Text <> "" And TextBox10.Text <> "" And TextBox11.Text <> "" And _
                TextBox12.Text <> "" And TextBox13.Text <> "" And TextBox14.Text <> "" And _
                TextBox15.Text <> "" And TextBox16.Text <> "" And TextBox17.Text <> "" And _
                TextBox20.Text <> "" And TextBox19.Text <> "" And TextBox18.Text <> "" Then
                    x1 = TextBox9.Text
                    y1 = TextBox10.Text
                    z1 = TextBox11.Text

                    x2 = TextBox14.Text
                    y2 = TextBox13.Text
                    z2 = TextBox12.Text

                    x3 = TextBox17.Text
                    y3 = TextBox16.Text
                    z3 = TextBox15.Text

                    x4 = TextBox20.Text
                    y4 = TextBox19.Text
                    z4 = TextBox18.Text

                    order = SortGreatestToLeast(x1, x2, x3, x4)
                    leastx = order(0)
                    mostx = order(3)

                    order = SortGreatestToLeast(y1, y2, y3, y4)
                    leasty = order(0)
                    mosty = order(3)

                    order = SortGreatestToLeast(z1, z2, z3, z4)
                    leastz = order(0)
                    mostz = order(3)
                ElseIf TextBox9.Text <> "" And TextBox10.Text <> "" And TextBox11.Text <> "" And _
                TextBox12.Text = "" And TextBox13.Text = "" And TextBox14.Text = "" And _
                TextBox15.Text = "" And TextBox16.Text = "" And TextBox17.Text = "" And _
                TextBox20.Text <> "" And TextBox19.Text <> "" And TextBox18.Text <> "" Then

                    leastx = TextBox9.Text
                    mostx = TextBox20.Text

                    leasty = TextBox10.Text
                    mosty = TextBox19.Text

                    leastz = TextBox11.Text
                    mostz = TextBox18.Text
                End If

                'Create Area File
                Dim folder As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\" & ComboBox5.Text & "\" & ComboBox6.Text & "\"
                Dim file As String
                If DE(folder) = False Then My.Computer.FileSystem.CreateDirectory(folder)
                file = folder & TextBox21.Text & ".txt"
                Dim sw As New IO.StreamWriter(file)
                sw.WriteLine("1.1")
                sw.WriteLine(TextBox21.Text)
                sw.WriteLine(ComboBox5.Text)
                sw.WriteLine(ComboBox6.Text)
                sw.WriteLine(leastx)
                sw.WriteLine(mostx)
                sw.WriteLine(leasty)
                sw.WriteLine(mosty)
                sw.WriteLine(leastz - 15000000)
                sw.WriteLine(mostz + 15000000)
                sw.Close()

                'Make \Zone\--\--\index.txt
                file = folder & "index.txt"
                If FE(file) Then
                    Dim sr As New IO.StreamReader(file)
                    Dim readLine() As String
                    Dim i As Integer = 1
                    Dim _continue As Boolean = True
                    Do Until sr.EndOfStream Or _continue = False
                        ReDim Preserve readLine(i)
                        readLine(i) = sr.ReadLine()
                        If readLine(i) = TextBox21.Text Then
                            _continue = False
                        End If
                        i = i + 1
                    Loop
                    sr.Close()
                    If _continue = True Then
                        sw = New IO.StreamWriter(file)
                        sw.WriteLine(TextBox21.Text)
                        sw.Close()
                    End If
                Else
                    sw = New IO.StreamWriter(file)
                    sw.WriteLine(TextBox21.Text)
                    sw.Close()
                End If

                'Make \Zones\index.txt
                file = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\index.txt"
                If FE(file) Then
                    Dim sr As New IO.StreamReader(file)
                    Dim readLine() As String
                    Dim i As Integer = 1
                    Dim _continue As Boolean = True
                    Do Until sr.EndOfStream Or _continue = False
                        ReDim Preserve readLine(i)
                        readLine(i) = sr.ReadLine()
                        If readLine(i) = ComboBox6.Text Then
                            _continue = False
                        End If
                        i = i + 1
                    Loop
                    sr.Close()
                    If _continue = True Then
                        i = 0
                        sw = New IO.StreamWriter(file)
                        For Each line In readLine
                            If line = ":zones:" Then sw.WriteLine(ComboBox6.Text)
                            sw.WriteLine(line)
                        Next
                        sw.WriteLine(ComboBox5.Text)
                        sw.Close()
                    End If
                Else
                    sw = New IO.StreamWriter(file)
                    sw.WriteLine(":subzones:")
                    sw.WriteLine(ComboBox6.Text)
                    sw.WriteLine(":zones:")
                    sw.WriteLine(ComboBox5.Text)
                    sw.Close()
                End If

                sw = New IO.StreamWriter(IO_EQOA & "Temp\NPC Maker\last area made.txt")
                sw.WriteLine("1.1")
                sw.WriteLine(TextBox21.Text) ' Area Name
                sw.WriteLine(ComboBox5.Text) ' Zone Name
                sw.WriteLine(ComboBox6.Text) ' Subzone Name
                sw.Close()

        End Select


    End Sub
    Private Function SortGreatestToLeast(ByVal input1 As Integer, ByVal input2 As Integer, ByVal input3 As Integer, ByVal input4 As Integer)
        Dim inputs() As String = {input1, input2, input3, input4}
        Dim order() As String = {"1", "2", "3", "4"}
        Array.Sort(inputs, order)
        input1 = inputs(0)
        'MsgBox(input1)
        input2 = inputs(1)
        'MsgBox(input2)
        input3 = inputs(2)
        'MsgBox(input3)
        input4 = inputs(3)
        'MsgBox(input4)
        'MsgBox(order(0))
        'MsgBox(order(1))
        'MsgBox(order(2))
        'MsgBox(order(3))
        'Return order
        Return inputs
    End Function
    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_FillFPBank.Click
        TextBox9.Text = 1187391721
        TextBox10.Text = 1181896867
        TextBox11.Text = 1114552729

        TextBox14.Text = 1187414535
        TextBox13.Text = 1181770971
        TextBox12.Text = 1114368818

        TextBox17.Text = 1187431836
        TextBox16.Text = 1181789526
        TextBox15.Text = 1114745467

        TextBox20.Text = 1187355985
        TextBox19.Text = 1181842760
        TextBox18.Text = 1114665392
        RadioButton1.Checked = True
        RadioButton1.Checked = False
        TextBox21.Text = "Freeport_Bank"
        ComboBox5.Text = "Freeport"
        ComboBox6.Text = "(S)"
    End Sub
#End Region

#Region "Shared"
    Private Sub Button3_Click_1(sender As System.Object, e As System.EventArgs) Handles d_all_btn_StartCE.Click
        Try
            'Process.Start(MyIO.Root & "Cheat Engine\DualBox.ct")
            'Process.Start(MyIO.Root & "Cheat Engine\MainTable.ct")
            Process.Start(IO_Root & "Cheat Engine\MainTable.ct") 'IO_RootAsUserData
        Catch ex As Exception

        End Try
    End Sub 'Open Cheat Table
    Private Sub ParseOutsideCommand(ByVal command As String, ByVal lines() As String)
        Dim filewrite As String = IO_EQOA & "Temp\Outside Command" & ".txt"

        Dim sw As New IO.StreamWriter(filewrite)
        sw.WriteLine(command)
        Select Case command
            Case "Spawn_NPC"
                sw.WriteLine(lines(0))
            Case "Spawn Wall Marker"
                For Each line In lines
                    sw.WriteLine(line)
                Next
        End Select
        sw.Close()
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Direction_Number.Text = "0"
        RightArrow_Click(sender, e) 'sets the help text
        'UpdateAdderDisplay("CheckHide")
        If TabControl1.SelectedTab.Text IsNot TabControl1.TabPages(3).Text Then
            SpawnMakerOnLoad()
        End If
    End Sub

    Private Sub AddFilesToControl(ByRef Control As Object, ByVal Folder As String, Optional ByVal Extension As String = "", Optional ByVal IncludeSubFolderFiles As Boolean = False)
        ComboBox4.Items.Clear()
        If IncludeSubFolderFiles = False Then
            Dim di As New IO.DirectoryInfo(Folder)
            Dim diar1 As IO.FileInfo() = di.GetFiles()
            Dim dra As IO.FileInfo

            'list the names of all files in the specified directory
            For Each dra In diar1
                If Microsoft.VisualBasic.Right(dra.ToString(), Extension.Length) = Extension Then
                    Control.Items.Add(Microsoft.VisualBasic.Left(dra.ToString(), dra.ToString().Length - Extension.Length))
                    Control.SelectedIndex = 0
                End If
            Next
        ElseIf IncludeSubFolderFiles = True Then
            ' Get recursive List of all files starting in this directory.
            Dim list As List(Of String) = GetFilesRecursive(Folder, Extension)

            ' Loop through and display each path.
            For Each path In list
                'Console.WriteLine(path)
                Control.Items.Add(System.IO.Path.GetFileName(path).Replace(Extension, ""))
            Next
            Control.SelectedIndex = 0

            ' Write total number of paths found.
            'Console.WriteLine(list.Count)
        End If
    End Sub
    ''' <summary>
    ''' This method starts at the specified directory.
    ''' It traverses all subdirectories.
    ''' It returns a List of those directories.
    ''' </summary>
    Public Function GetFilesRecursive(ByVal initial As String, Optional ByVal Extension As String = ".*") As List(Of String)
        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(Directory.GetFiles(dir, "*" & Extension))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop

        ' Return the list
        Return result
    End Function
#End Region
#Region "Independent"
    Private Sub Direction_Number_Click(sender As System.Object, e As System.EventArgs) Handles Direction_Number.Click
        Shell("explorer.exe " & Chr(34) & MyIO.Root & Chr(34))
        'Shell("explorer.exe " & Chr(34) & IO_Root & Chr(34))
    End Sub
    Private Sub Directions_Box(ByVal Direction As Integer)
        Dim NewLine = vbCrLf
        If TabControl1.SelectedIndex = 0 Then ' Start Window
            Select Case Direction
                Case Is = 1
                    Directions.Text = ""
                    Directions.AppendText("Welcome to " & ProgramVersion & " of Rundatshityo's EQOA Save State Program [Working Title].")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("This program was created by Robert Randazzio. [DOT1204150255A]")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Special Thanks to Dustin Faxon for finding the memory addresses and for other misc. things.")
                    Directions.AppendText("Also thanks to Daniel Wallace and Jeremiah Johnson for testing this program and other things.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Right now, it is assumed you understand you need Cheat Engine and the emulator, and how to work them both.")
                    Directions.AppendText(NewLine)
                Case Is = 2
                    Directions.Text = ""
                    Directions.AppendText("If a file is installed, it will have a check next to it. If it is not installed, it will not have a check")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Checking and unchecking the boxes will install and unstall the files.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Right now you cannot uninstall the folders and most of the LUA files.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                Case Else
                    Exit Sub
            End Select
        ElseIf TabControl1.SelectedIndex = 1 Then ' Dual Boxing
            Select Case Direction
                Case Is = 1
                    Directions.Text = ""
                    'Directions.AppendText("Click " & Chr(34) & d_dep_Button1.Text & Chr(34) & " to install the files.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    'Directions.AppendText("(Click " & Chr(34) & d_dep_Button2.Text & Chr(34) & " to uninstall the files.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("If any files or folders are opened in the installation directory, you will probably have to close them first.)")
                    Directions.AppendText(NewLine)

                Case Is = 2
                    Directions.Text = ""
                    Directions.AppendText("Click " & Chr(34) & d_all_btn_StartCE.Text & Chr(34) & " to open up the cheat table.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Click it twice to open up two of the cheat table.")
                    Directions.AppendText(NewLine)

                Case Is = 3
                    Directions.Text = ""
                    Directions.AppendText("Attach the first Cheat Engine to your first PS2 box.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Click the box to the left of the blue words, " & Chr(34) & "DUALBOX1" & Chr(34) & ".")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("This will turn on Dual Boxing for your first box.")
                    Directions.AppendText(NewLine)

                Case Is = 4
                    Directions.Text = ""
                    Directions.AppendText("Attach the second Cheat Engine to your second PS2 box.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Click the box to the left of the blue words, " & Chr(34) & "DUALBOX2" & Chr(34) & ".")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("This will turn on Dual Boxing for your second box.")
                    Directions.AppendText(NewLine)

                Case Is = 5
                    Directions.Text = ""
                    Directions.AppendText("You should be done.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("You should have both characters in each savestate updating now.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Both players should be Coachman Ronks in the other box's world.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("You can only run two boxes at a time right now, sorry. I wanted to make sure this worked for others before I spent time writing potentially useless code.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Once this is confirmed working for somebody else, I will add multiple boxes and correct character names and graphics.")
                    Directions.AppendText(NewLine)

                Case Else
                    Exit Sub
            End Select

        ElseIf TabControl1.SelectedIndex = 2 Then ' NPC Maker
            Select Case Direction
                Case Is = 1
                    Directions.Text = ""
                    Directions.AppendText("NPC Maker")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("To Install NPC Maker, Click the " & Chr(34) & TabPage1.Text & Chr(34) & " tab and then click " & Chr(34) & "Install" & Chr(34) & " under Everything or under " & Chr(34) & "NPC Maker" & Chr(34) & ".")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("To start the NPC Maker, click " & Chr(34) & "Start the Cheat Table" & Chr(34) & ".")
                Case Is = 2
                    Directions.Text = ""
                    Directions.AppendText("Next, Attach Cheat Engine to the emulator like normal.")
                    Directions.AppendText(NewLine)
                Case Is = 3
                    Directions.Text = ""
                    Directions.AppendText("Click the box to the left of the words " & Chr(34) & "Main.lua - NPC Maker" & Chr(34))
                    Directions.AppendText(NewLine)
                Case Is = 4
                    Directions.Text = ""
                    Directions.AppendText("Hold L2 and Square to mark where you are standing in the emulator.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("This will be where the new NPC will be located.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Once you've hit L2 + Square, come back to this program and click 'Grab Data'.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("Once you've done that, fill in the NPC's name, a File Name for the NPC (Called a 'Safe Name') and the NPC's Level.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("You can hold L2 + Circle to spawn the last NPC you made.")
                    Directions.AppendText(NewLine)
                    Directions.AppendText("If you don't like your new NPC, just remake the file and it will overwrite the old one.")
                    Directions.AppendText(NewLine)
                Case Else
                    Exit Sub
            End Select
        End If
        Direction_Number.Text = Direction
        'Directions.AppendText("")
        'Directions.AppendText("")
        'Directions.AppendText("")
    End Sub
    Private Sub LeftArrow_Click(sender As System.Object, e As System.EventArgs) Handles LeftArrow.Click
        Directions_Box(Direction_Number.Text - 1)
    End Sub
    Private Sub RightArrow_Click(sender As System.Object, e As System.EventArgs) Handles RightArrow.Click
        Directions_Box(Direction_Number.Text + 1)
    End Sub


    Public Function CE_WriteKey()
        Dim _lines() As String
        Dim i As Integer = 0
        _lines(0) = "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "utf-8" & Chr(34) & "?>"
        i = i + 1
        _lines(0) = "<CheatTable>"
        i = i + 1
        _lines(0) = "  <CheatEntries>"
        i = i + 1
        _lines(0) = "    <CheatEntry>"
        i = i + 1
        _lines(0) = "      <ID>4454</ID>"
        i = i + 1
        _lines(0) = "      <Description>" & Chr(34) & "Main.lua - NPC Maker" & Chr(34) & "</Description>"
        i = i + 1
        _lines(0) = "      <LastState Activated=" & Chr(34) & "0" & Chr(34) & "\>"
        i = i + 1
        _lines(0) = "      <Color>FF0000</Color>"
        i = i + 1
        _lines(0) = "      <VariableType>Auto Assembler Script</VariableType>"
        i = i + 1
        _lines(0) = "      <AssemblerScript>[ENABLE]"
        i = i + 1
        _lines(0) = "\\code from here to '[DISABLE]' will be used to enable the cheat"
        i = i + 1
        _lines(0) = "{$lua}"
        i = i + 1
        _lines(0) = ""
        i = i + 1
        _lines(0) = "mode = " & Chr(34) & "NPC Maker" & Chr(34)
        i = i + 1
        _lines(0) = "dofile(" & Chr(34) & "..\EQOA\luas\Main.lua" & Chr(34) & ")"
        i = i + 1
        _lines(0) = "[DISABLE]"
        i = i + 1
        _lines(0) = "\\code from here till the end of the code will be used to disable the cheat"
        i = i + 1
        _lines(0) = "</AssemblerScript>"
        i = i + 1
        _lines(0) = "    </CheatEntry>"
        i = i + 1
        _lines(0) = "  </CheatEntries>"
        i = i + 1
        _lines(0) = "</CheatTable>"
        i = i + 1
        _lines(0) = ""

        Return _lines

    End Function
#End Region

    Private Sub ListBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.SelectedItem = Nothing Then Exit Sub
        If AllowNPCAreaNPCLists_TobeChanged = True Then
            AllowNPCAreaNPCLists_TobeChanged = False
            For Each line In ListBox2.Items
                If ListBox1.SelectedItem = line Then
                    ListBox2.SelectedItem = line
                    AllowNPCAreaNPCLists_TobeChanged = True
                    Exit For
                End If
            Next
            AllowNPCAreaNPCLists_TobeChanged = True
        End If
    End Sub
    Private Sub ListBox2_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox2.SelectedIndexChanged
        If ListBox2.SelectedItem = Nothing Then Exit Sub
        If AllowNPCAreaNPCLists_TobeChanged = True Then
            AllowNPCAreaNPCLists_TobeChanged = False
            For Each line In ListBox1.Items
                If ListBox2.SelectedItem = line Then
                    ListBox1.SelectedItem = line
                    Exit For
                End If
            Next
            AllowNPCAreaNPCLists_TobeChanged = True
        End If
    End Sub
    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_Remove.Click
        Dim item = ListBox2.SelectedItem
        ListBox2.SelectedItem = Nothing
        ListBox1.SelectedItem = Nothing
        ListBox2.Items.Remove(item)
    End Sub

    Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_LoadAreaFile.Click
        Select Case ComboBox7.Text
            Case Is = "1.0"
                If My.Computer.FileSystem.FileExists(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\" & ComboBox4.Text & ".txt") = True Then
                    Dim sr As New IO.StreamReader(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\" & ComboBox4.Text & ".txt")
                    Dim line As String
                    line = IO_ReadNextLine(sr, "--")
                    If line = "1.0" Then
                        ClearAreaFileControls()

                        'Label32.Text = line

                        TextBox21.Text = IO_ReadNextLine(sr, "--")
                        ComboBox5.Text = IO_ReadNextLine(sr, "--")

                        TextBox9.Text = IO_ReadNextLine(sr, "--")
                        TextBox20.Text = IO_ReadNextLine(sr, "--")

                        TextBox10.Text = IO_ReadNextLine(sr, "--")
                        TextBox19.Text = IO_ReadNextLine(sr, "--")

                        TextBox11.Text = IO_ReadNextLine(sr, "--")
                        TextBox18.Text = IO_ReadNextLine(sr, "--")
                        line = IO_ReadNextLine(sr, "--")
                        Do Until line = Nothing
                            ListBox2.Items.Add(line)
                            line = IO_ReadNextLine(sr, "--")
                        Loop
                    Else
                        MsgBox("File version not supported for: " & ComboBox4.Text & " : " & line)
                    End If
                    sr.Close()
                Else
                    MsgBox("Error: File does not exist or cannot be found")
                    ClearAreaFileControls()
                End If
            Case Is = "1.1"
                Dim file As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\" & ComboBox5.Text & "\" & ComboBox6.Text & "\" & ComboBox4.Text & ".txt"
                If My.Computer.FileSystem.FileExists(file) = True Then
                    Dim sr As New IO.StreamReader(file)
                    Dim line As String
                    line = IO_ReadNextLine(sr, "--")
                    If line = "1.1" Then
                        ClearAreaFileControls()

                        'Label32.Text = line

                        TextBox21.Text = IO_ReadNextLine(sr, "--")
                        ComboBox5.Text = IO_ReadNextLine(sr, "--")
                        ComboBox6.Text = IO_ReadNextLine(sr, "--")

                        TextBox9.Text = IO_ReadNextLine(sr, "--")
                        TextBox20.Text = IO_ReadNextLine(sr, "--")

                        TextBox10.Text = IO_ReadNextLine(sr, "--")
                        TextBox19.Text = IO_ReadNextLine(sr, "--")

                        TextBox11.Text = IO_ReadNextLine(sr, "--")
                        TextBox18.Text = IO_ReadNextLine(sr, "--")
                        line = IO_ReadNextLine(sr, "--")
                        Do Until line = Nothing
                            ListBox2.Items.Add(line)
                            line = IO_ReadNextLine(sr, "--")
                        Loop
                    Else
                        MsgBox("File version not supported for: " & ComboBox4.Text & " : " & line)
                    End If
                    sr.Close()
                Else
                    MsgBox("Error: File does not exist or cannot be found")
                    'ClearAreaFileControls()
                End If
            Case Else

        End Select

    End Sub
    Private Sub ClearAreaFileControls()
        TextBox9.Text = ""
        TextBox10.Text = ""
        TextBox11.Text = ""

        TextBox14.Text = ""
        TextBox13.Text = ""
        TextBox12.Text = ""

        TextBox17.Text = ""
        TextBox16.Text = ""
        TextBox15.Text = ""

        TextBox20.Text = ""
        TextBox19.Text = ""
        TextBox18.Text = ""

        TextBox21.Text = ""
        ComboBox5.Text = ""

        'Label32.Text = "-"

        ListBox2.Items.Clear()
    End Sub

    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_UpdateFile.Click
        Select Case ComboBox7.Text
            Case Is = "1.0"
                Dim sw As New IO.StreamWriter(IO_EQOA & "Custom Data\NPC Maker\NPC Areas\" & ComboBox4.Text & ".txt")
                sw.WriteLine("1.0")
                sw.WriteLine(TextBox21.Text)
                sw.WriteLine(ComboBox5.SelectedItem.ToString)
                sw.WriteLine(TextBox9.Text)
                sw.WriteLine(TextBox20.Text)
                sw.WriteLine(TextBox10.Text)
                sw.WriteLine(TextBox19.Text)
                sw.WriteLine(TextBox11.Text)
                sw.WriteLine(TextBox18.Text)
                For Each line In ListBox2.Items
                    sw.WriteLine(line)
                Next
                sw.Close()
            Case Is = "1.1"
                Dim file, folder As String
                folder = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\" & ComboBox5.Text & "\" & ComboBox6.Text & "\"
                If ComboBox4.Text = "" Then
                    file = folder & TextBox21.Text & ".txt"
                Else
                    file = folder & ComboBox4.Text & ".txt"
                End If

                Dim sw As New IO.StreamWriter(file)
                sw.WriteLine("1.1")
                sw.WriteLine(TextBox21.Text)
                sw.WriteLine(ComboBox5.Text)
                sw.WriteLine(ComboBox6.Text)
                sw.WriteLine(TextBox9.Text)
                sw.WriteLine(TextBox20.Text)
                sw.WriteLine(TextBox10.Text)
                sw.WriteLine(TextBox19.Text)
                sw.WriteLine(TextBox11.Text)
                sw.WriteLine(TextBox18.Text)
                For Each line In ListBox2.Items
                    sw.WriteLine(line)
                Next
                sw.Close()
        End Select
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox7.SelectedIndexChanged
        SpawnMakerSelectVersion()
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        If ComboBox6.Items.Count = 0 Then
            Dim Path As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\"
            Dim file As String = Path & "index" & ".txt"
            Dim lineread As String
            If FE(file) = True Then
                Dim i As Integer = 0
                Dim sr As New IO.StreamReader(file)
                Do
                    lineread = IO_ReadNextLine(sr, "--")
                Loop Until lineread = ":subzones:" Or sr.EndOfStream
                If sr.EndOfStream = False And lineread = ":subzones:" Then lineread = IO_ReadNextLine(sr, "--")
                Do While sr.EndOfStream = False
                    If lineread <> ":zones:" Then
                        If DE(Path & ComboBox5.SelectedItem.ToString & "\" & lineread & "\") Then
                            ComboBox6.Items.Add(lineread)
                        End If
                    End If
                    lineread = IO_ReadNextLine(sr, "--")
                Loop
                sr.Close()
            End If
        Else

        End If

    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox6.SelectedIndexChanged
        'Dim Path As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\" & ComboBox5.SelectedItem.ToString & "\" & ComboBox6.SelectedItem.ToString & "\"

        'AddFilesToControl(ComboBox4, Path, ".txt")
    End Sub

    Private Sub d_dep_btn_RemSpace_Click(sender As System.Object, e As System.EventArgs) Handles d_dep_btn_RemSpace.Click
        'Replaces space out of every NPC File
        TextBox1.Text = TextBox1.Text.Replace("  ", "") 'name
        If Right(TextBox1.Text, 1) = " " Then TextBox1.Text = Left(TextBox1.Text, TextBox1.Text.Length - 1)
        TextBox2.Text = TextBox2.Text.Replace(" ", "") 'file
        'TextBox2.Text = TextBox2.Text.Replace(" ", "_")
        d_btn_npcmkr_Make_File_Click(sender, e)
        ComboBox3.SelectedIndex = ComboBox3.SelectedIndex + 1
        d_npcmkr_btn_Load_NPC_Click(sender, e)
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        Dim i As Integer = e.Index 'CheckedListBox1.SelectedIndex.ToString
        If RI(i).IsChangeable = True Then
            ' File is Changable
            'change check
            'if unchecked, uninstall
            'if checked, install
            If e.NewValue = CheckState.Checked Then
                If CheckedListBox1.SelectedIndex.ToString <> -1 Then
                    If RI(i).IsInstallable = True Then
                        FileHandler("install", "", "", RI(i).Name)
                        'FileHandler("install", FileHandler_FileName(i))
                    End If
                End If
            ElseIf e.NewValue = CheckState.Unchecked Then
                If RI(i).IsUninstallable = True Then
                    FileHandler("uninstall", "", "", RI(i).Name)
                    'FileHandler("uninstall", "", "", FileHandler_FileName(i))
                Else
                    e.NewValue = e.CurrentValue
                End If
            End If
        Else
            ' File is not Changable
            e.NewValue = e.CurrentValue
        End If
    End Sub

    Private Sub Button16_Click(sender As System.Object, e As System.EventArgs) Handles d_main_btn_InstallEverything.Click
        Dim i As Integer = 0
        Do Until i >= CheckedListBox1.Items.Count
            CheckedListBox1.SelectedIndex = i
            If CheckedListBox1.GetItemCheckState(i) = CheckState.Unchecked Then
                CheckedListBox1.SetItemCheckState(i, CheckState.Checked)
            End If
            i = i + 1
        Loop
    End Sub

    Private Sub Button17_Click(sender As System.Object, e As System.EventArgs) Handles d_main_btn_UninstallEverything.Click
        Dim i As Integer = 0
        Do Until i >= CheckedListBox1.Items.Count
            CheckedListBox1.SelectedIndex = i
            If CheckedListBox1.GetItemCheckState(i) = CheckState.Checked Then
                CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
            End If
            i = i + 1
        Loop
    End Sub

    Private Sub Button18_Click(sender As System.Object, e As System.EventArgs) Handles d_npcmkr_btn_SpawnLastNPC.Click
        Dim filewrite As String = IO_EQOA & "Temp\Outside Command" & ".txt"

        Dim lastnpc As String
        Dim file As String = IO_EQOA & "Temp\NPC Maker\last file made.txt"
        If FE(file) = False Then Exit Sub
        Dim sr As New IO.StreamReader(file)
        sr.ReadLine() 'in game name
        lastnpc = sr.ReadLine() 'safe name
        sr.Close()
        Dim lastnpc_arg(0) As String
        lastnpc_arg(0) = lastnpc
        ParseOutsideCommand("Spawn_NPC", lastnpc_arg)

    End Sub

    Private Sub Button19_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_UpdateAreaList.Click
        If ComboBox5.Text = Nothing Or ComboBox6.Text = Nothing Then Exit Sub
        Dim Path As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\" & ComboBox5.Text & "\" & ComboBox6.Text & "\"
        'Dim Path As String = IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\" & ComboBox5.text & "\" & ComboBox6.Text & "\"
        If DE(Path) = False Then Exit Sub
        AddFilesToControl(ComboBox4, Path, ".txt")
        Dim itemstoremove() As String
        Dim i As Integer
        For Each item In ComboBox4.Items
            If item.ToString = "index" Then
                i = i + 1
                ReDim Preserve itemstoremove(i)
                itemstoremove(i) = item
            End If
        Next
        For Each item In itemstoremove
            ComboBox4.Items.Remove(item)
        Next
    End Sub

    Private Sub Button20_Click(sender As System.Object, e As System.EventArgs) Handles d_modsctn_btn_MakeMode.Click
        CE_WriteKey()
    End Sub

    Private Sub Button21_Click(sender As System.Object, e As System.EventArgs) Handles d_modsctn_btn_Convert.Click
        Dim lines() As String = TextBox23.Lines.Clone ' i is index
        Dim newlineNumber As Integer ' index for below newline_ variables
        Dim newline_description(), newline_variableType(), newline_address(), newline_offeset() As String
        Dim errorline() As String
        Dim i As Integer ' lines() index
        'Dim i2 As Integer ' newline() index
        Dim _step As Integer = 0
        Do While i < lines.Length
            Select Case _step
                Case 0
                    Select Case lines(i)
                        Case "<?xml version=" & Chr(34) & "1.0" & Chr(34) & " encoding=" & Chr(34) & "utf-8" & Chr(34) & "?>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 1, exit sub.")
                            Exit Do
                    End Select
                Case 1
                    Select Case lines(i).Trim(" ")
                        Case "<CheatTable>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 2, exit sub.")
                            Exit Do
                    End Select
                Case 2
                    Select Case lines(i).Trim(" ")
                        Case "<CheatEntries>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 3
                    If lines(i).Trim(" ") = "<CheatEntry>" Then
                        _step = _step + 1
                    ElseIf lines(i).Trim(" ") = "</CheatEntries>" Then
                        _step = 14 ' jumps up here
                    Else
                        MsgBox("error 3.0, exit sub.")
                    End If
                Case 4
                    Select Case Left(lines(i).Trim(" "), 4)
                        Case "<ID>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 5
                    Select Case Left(lines(i).Trim(" "), 13)
                        Case "<Description>"
                            newlineNumber = newlineNumber + 1
                            ReDim Preserve newline_description(newlineNumber)
                            newline_description(newlineNumber) = lines(i).Replace("<Description>" & Chr(34), "").Replace(Chr(34) & "</Description>", "").Trim(" ")
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 6
                    Select Case Left(lines(i).Trim(" "), 10)
                        Case "<LastState"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 7
                    Select Case Left(lines(i).Trim(" "), 7)
                        Case "<Color>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 8
                    Select Case Left(lines(i).Trim(" "), 14)
                        Case "<VariableType>"
                            ReDim Preserve newline_variableType(newlineNumber)
                            newline_variableType(newlineNumber) = lines(i).Replace("<VariableType>", "").Replace("</VariableType>", "").Trim(" ")
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 9
                    Select Case newline_variableType(newlineNumber)
                        Case "4 Bytes"
                            Select Case Left(lines(i).Trim(" "), 9)
                                Case "<Address>"
                                    ReDim Preserve newline_address(newlineNumber)
                                    newline_address(newlineNumber) = lines(i).Replace("<Address>", "").Replace("</Address>", "").Trim(" ")
                                    _step = _step + 1
                                Case Else
                                    MsgBox("error case 9, exit sub.")
                                    Exit Do
                            End Select
                        Case "String"

                    End Select

                Case 10
                    Select Case Left(lines(i).Trim(" "), 14)
                        Case "<Offsets>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 11
                    Select Case Left(lines(i).Trim(" "), 8)
                        Case "<Offset>"
                            ReDim Preserve newline_offeset(newlineNumber)
                            newline_offeset(newlineNumber) = lines(i).Replace("<Offset>", "").Replace("</Offset>", "").Trim(" ")
                            _step = _step + 1
                        Case "<Offset>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 12
                    Select Case Left(lines(i).Trim(" "), 10)
                        Case "</Offsets>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 13
                    Select Case Left(lines(i).Trim(" "), 13)
                        Case "</CheatEntry>"
                            _step = 3 ' jumps back here
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 14
                    Select Case Left(lines(i).Trim(" "), 13)
                        Case "</CheatTable>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select

            End Select
            i = i + 1
        Loop
        TextBox23.Clear()
        i = 1
        'newlineNumber = 0
        Do Until i > newlineNumber
            TextBox23.AppendText(newline_address(i) & Chr(34) & "+" & newline_offeset(i) & " -- " & newline_description(i) & " -- type:" & newline_variableType(i))
            TextBox23.AppendText(vbNewLine)
            i = i + 1
        Loop
        'TextBox23.AppendText(newline)
    End Sub

    Private Sub PersonalBugTrackerOnLoad()
        Label42.Text = IO_RootAsUserData
    End Sub

    Private Sub Button23_Click(sender As System.Object, e As System.EventArgs) Handles d_main_btn_RefreshList.Click
        d_main_btn_RefreshList.Text = "Refreshing.."
        CheckedListBox1.Items.Clear()
        CheckForFiles()
        d_main_btn_RefreshList.Text = "Refresh List"
    End Sub

    Private Sub Button24_Click(sender As System.Object, e As System.EventArgs) Handles d_npcmkr_btn_SpawnThisNPC.Click
        If ComboBox3.Text = "" Then Exit Sub
        Dim npc(0) As String
        npc(0) = ComboBox3.Text
        ParseOutsideCommand("Spawn_NPC", npc)
    End Sub

    Private Sub Button25_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_SpawnThisNPC_All.Click
        If ListBox1.Text = "" Then Exit Sub
        Dim npc(0) As String
        npc(0) = ListBox1.Text
        ParseOutsideCommand("Spawn_NPC", npc)
    End Sub

    Private Sub Button26_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_SpawnThisNPC_Zone.Click
        If ListBox1.Text = "" Then Exit Sub
        Dim npc(0) As String
        npc(0) = ListBox1.Text
        ParseOutsideCommand("Spawn_NPC", npc)
    End Sub

    Private Sub Label38_DoubleClick(sender As System.Object, e As System.EventArgs) Handles Label38.DoubleClick
        LoadOnlineForm()
    End Sub
    Private Sub LoadOnlineForm(Optional ByVal input As String = "")
        If input = "" Then
            input = InputBox("Enter the passphrase to identify yourself or who you got this copy from: ", "Should You Be Here? D8<")
        End If
        Select Case input
            Case "Poppledon"
                'MsgBox("Success!")
                OnlineForm.Visible = True
                OnlineForm.OnlineName = "Bob"
            Case "gibbyshib1"
                'MsgBox("Success!")
                OnlineForm.Visible = True
                OnlineForm.OnlineName = "Daniel"
            Case "houladon1"
                'MsgBox("Success!")
                OnlineForm.Visible = True
                OnlineForm.OnlineName = "Jeremiah"
            Case "dipbiptie1"
                'MsgBox("Success!")
                OnlineForm.Visible = True
                OnlineForm.OnlineName = "Christopher"
            Case Else
                MsgBox("PWORGTFO!")
        End Select
    End Sub

    Private Sub Button28_Click(sender As System.Object, e As System.EventArgs) Handles d_all_btn_BobsButton.Click

    End Sub

#Region "MISC"
    Public Function DE(_path) As Boolean
        If My.Computer.FileSystem.DirectoryExists(_path) Then
            Return True
        Else : Return False
        End If
    End Function
    Public Function FE(_path) As Boolean
        If My.Computer.FileSystem.FileExists(_path) Then
            Return True
        Else : Return False
        End If
    End Function
    Private Function IO_ReadNextLine(ByRef sr As IO.StreamReader, Optional ByVal commentString As String = Nothing)
        Dim line As String
        If sr.EndOfStream = False Then
            If commentString = Nothing Then
                Return sr.ReadLine()
            Else
                line = sr.ReadLine()
                Do While Left(line, commentString.Length) = commentString
                    line = sr.ReadLine()
                Loop
                Return line
            End If
        Else
            Return Nothing
        End If
    End Function
    ''' <summary>
    ''' Returns characters from the right end of a string.
    ''' </summary>
    ''' <param name="Stringaling">The string to retrieve from.</param>
    ''' <param name="Number">How many charaters to return.</param>
    ''' <returns>The new string as String.</returns>
    ''' <remarks>The string retrieved as String</remarks>
    Shadows Function Right(ByVal Stringaling As String, ByVal Number As Integer)
        Return Microsoft.VisualBasic.Right(Stringaling, Number)
    End Function
    ''' <summary>
    ''' Returns characters from the left end of a string.
    ''' </summary>
    ''' <param name="Stringaling">The string to retrieve from.</param>
    ''' <param name="Number">How many charaters to return.</param>
    ''' <returns>The new string as String.</returns>
    ''' <remarks>The string retrieved as String</remarks>
    Shadows Function Left(ByVal Stringaling As String, ByVal Number As Integer)
        Return Microsoft.VisualBasic.Left(Stringaling, Number)
    End Function
#End Region
#Region "Deprycated"
    Private Sub InstallFiles()
        DoMassOutput("Dual Box")
        DoMassOutput("NPC Maker")
    End Sub 'deprycated
    Private Sub v1_CheckForFiles(Optional ByVal FileGroup As String = "All")
        'Dim i As Integer = 0
        'Dim i_max As Integer = 2
        'Dim fileName(2) As String
        'Dim fileExists(2) As Boolean
        'Dim fileChecked(2) As Boolean
        'Do Until i = i_max
        'If "DualBox" = FileGroup Or "All" = FileGroup Then
        'fileName(i) = FileGroup
        'ElseIf "NPC Maker" = FileGroup Or "All" = FileGroup Then
        '
        'End If
        'Loop

        'Main Cheat Table
        If FE(MyIO.Root & "Cheat Engine\MainTable.ct") Then : Installed_DualBox_CheatTable = True : Else : Installed_DualBox_CheatTable = False : End If
        ''Dual Box
        'Dual Box LUA1
        If FE(MyIO.EQOA & "\luas\Box1.lua") Then : Installed_DualBox_LUA1 = True : Else : Installed_DualBox_LUA1 = False : End If
        'Dual Box LUA2
        If FE(MyIO.EQOA & "\luas\Box2.lua") Then : Installed_DualBox_LUA2 = True : Else : Installed_DualBox_LUA2 = False : End If


        If FE(MyIO.EQOA & "\luas\Main.lua") Then : NPCMakerFilesInstalled = True : Else : NPCMakerFilesInstalled = False : End If

        'Dual Box - All
        If Installed_DualBox_CheatTable = True And _
            Installed_DualBox_LUA1 = True And _
            Installed_DualBox_LUA2 = True Then
            ''Dual Box - Installed
            DualBoxFilesInstalled = True
            d_lbl_status.Text = "Files Are Installed"
            'd_dep_Button1.Enabled = False
            'd_dep_Button2.Enabled = True
            d_all_btn_StartCE.Enabled = True
        Else
            'Dual Box - Not Installed
            DualBoxFilesInstalled = False
            d_lbl_status.Text = "Files Are Not Installed"
            'd_dep_Button1.Enabled = True
            'd_dep_Button2.Enabled = False
            d_all_btn_StartCE.Enabled = False
        End If
        If NPCMakerFilesInstalled = True Then
            d_npcmkr_btn_Grab_Data.Enabled = True
            d_npcmkr_btn_Make_File.Enabled = True
        Else
            d_npcmkr_btn_Grab_Data.Enabled = False
            d_npcmkr_btn_Make_File.Enabled = False
        End If
    End Sub 'deprycated
    Private Sub DoMassOutput(ByVal trigger As String)
        Dim sw As IO.StreamWriter
        Select Case trigger
            Case Is = "Dual Box"
                My.Computer.FileSystem.CreateDirectory(MyIO.Root)
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\player data\Dualbox Data\")
                sw = New IO.StreamWriter(MyIO.Root & "EQOA\player data\Dualbox Data\Box1.txt")
                sw.WriteLine("1187345244")
                sw.WriteLine("1182112840")
                sw.WriteLine("1113096192")
                sw.WriteLine("0")
                sw.Close()
                sw = New IO.StreamWriter(MyIO.Root & "EQOA\player data\Dualbox Data\Box2.txt")
                sw.WriteLine("1187345244")
                sw.WriteLine("1182112840")
                sw.WriteLine("1113096192")
                sw.WriteLine("0")
                sw.Close()

                DoMassOutput("Ghosts")
                'DoMassOutput("NPCs")

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\luas\")

                SaveFile(MyIO.Root & "EQOA\luas\", "Box1", ".lua", My.Resources.Box1)

                SaveFile(MyIO.Root & "EQOA\luas\", "Box2", ".lua", My.Resources.Box2)

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "Cheat Engine\")
                'SaveFile(MyIO.Root & "Cheat Engine\DualBox.ct", My.Resources.DualBox)

                DualBoxFilesInstalled = True
            Case Is = "NPCs"
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Game Data\NPCs\")
                sw = New IO.StreamWriter(MyIO.Root & "EQOA\Game Data\NPCs\Coachman_Ronks.txt")
                sw.WriteLine("0.1.1")
                sw.WriteLine("Coachman_Ronks")
                sw.WriteLine("Coachman Ronks")
                sw.WriteLine("1187345244")
                sw.WriteLine("1182112840")
                sw.WriteLine("1113096192")
                sw.WriteLine("0")
                sw.WriteLine("Human")
                sw.WriteLine("Male")
                sw.WriteLine("Coachman")
                sw.WriteLine("10")
                sw.WriteLine("255")
                sw.Close()
            Case Is = "NPC Maker"
                DoMassOutput("Ghosts")
                'DoMassOutput("NPCs")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "Cheat Engine\")
                SaveFile(MyIO.Root & "Cheat Engine\", "MainTable", ".ct", My.Resources.MainTable)

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\luas\")
                SaveFile(MyIO.Root & "EQOA\luas\", "Main", ".lua", My.Resources.Main)
                SaveFile(MyIO.Root & "EQOA\luas\", "File Paths", ".lua", My.Resources.File_Paths)
                SaveFile(MyIO.Root & "EQOA\luas\", "Locations", ".lua", My.Resources.Locations)

                MainCTInstalled = True

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Temp\")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Temp\NPC Maker\")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Temp\NPC Maker\NPC Areas\")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Custom Data\")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Custom Data\NPC Maker\")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Custom Data\NPC Maker\NPC Areas\")
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Game Data\NPCs\NPC Areas\")

            Case Is = "Ghosts"
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Game Data\Ghosts\")
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Coachman_Ronks", ".txt", My.Resources.Coachman_Ronks)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Aloj_Tilsteran", ".txt", My.Resources.Aloj_Tilsteran)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Merchant_Kari", ".txt", My.Resources.Merchant_Kari)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Dr_Killian", ".txt", My.Resources.Dr_Killian)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Guard_Saolen", ".txt", My.Resources.Guard_Saolen)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Guard_Jahn", ".txt", My.Resources.Guard_Jahn)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Bowyer_Koll", ".txt", My.Resources.Bowyer_Koll)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Tailor_Bariston", ".txt", My.Resources.Tailor_Bariston)
                SaveFile(MyIO.Root & "EQOA\Game Data\Ghosts\", "Tailor_Zixar", ".txt", My.Resources.Tailor_Zixar)

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Game Data\NPCs\")
        End Select
    End Sub 'deprycated
    Private Sub v1_DoMassOutput(ByVal trigger As Integer)
        Select Case trigger
            Case Is = 0
                My.Computer.FileSystem.CreateDirectory(MyIO.Root)
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\player data\Dualbox Data\")
                Dim sw As New IO.StreamWriter(MyIO.Root & "EQOA\player data\Dualbox Data\Box1.txt")
                sw.WriteLine("1187345244")
                sw.WriteLine("1182112840")
                sw.WriteLine("1113096192")
                sw.WriteLine("0")
                sw.Close()
                sw = New IO.StreamWriter(MyIO.Root & "EQOA\player data\Dualbox Data\Box2.txt")
                sw.WriteLine("1187345244")
                sw.WriteLine("1182112840")
                sw.WriteLine("1113096192")
                sw.WriteLine("0")
                sw.Close()
                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Game Data\Ghosts\")
                sw = New IO.StreamWriter(MyIO.Root & "EQOA\Game Data\Ghosts\Coachman_Ronks.txt")
                sw.WriteLine("0.1.1")
                sw.WriteLine("Coachman_Ronks")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFFD4]+624")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFFD4]+644")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+0D8")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+0C0")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+0C8")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+0C4")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+080")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+0F0")
                sw.WriteLine("[" & Chr(34) & "pcsx2-r3878.exe" & Chr(34) & "+003FFE64]+099")
                sw.Close()

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\Game Data\NPCs\")
                sw = New IO.StreamWriter(MyIO.Root & "EQOA\Game Data\NPCs\Coachman_Ronks.txt")
                sw.WriteLine("Coachman_Ronks")
                sw.WriteLine("Coachman Ronks")
                sw.WriteLine("1187345244")
                sw.WriteLine("1182112840")
                sw.WriteLine("1113096192")
                sw.WriteLine("0")
                sw.WriteLine("Human")
                sw.WriteLine("Male")
                sw.WriteLine("Coachman")
                sw.WriteLine("10")
                sw.WriteLine("255")
                sw.Close()

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "EQOA\luas\")

                SaveFile(MyIO.Root & "EQOA\luas\", "Box1", ".lua", My.Resources.Box1)

                SaveFile(MyIO.Root & "EQOA\luas\", "Box2", ".lua", My.Resources.Box2)

                'SaveFile(Path.Root & "EQOA\Game Data\Ghosts\Coachman_Ronks.txt", My.Resources.Coachman_Ronks)

                My.Computer.FileSystem.CreateDirectory(MyIO.Root & "Cheat Engine\")
                SaveFile(MyIO.Root & "Cheat Engine\", "DualBox", ".ct", My.Resources.DualBox)

                DualBoxFilesInstalled = True
            Case Is = 1
                SaveFile(MyIO.Root & "Cheat Engine\", "DualBox", ".ct", My.Resources.DualBox)
        End Select
    End Sub 'deprycated
    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles d_spwnmkr_btn_AddToList.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            ListBox2.Items.Add(ListBox1.SelectedItem)
        End If
    End Sub 'deprycated
    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs)
        'If d_dep_btn_NPCAdder.Text = "NPC Adder >>" Then ' If small make big
        'd_dep_btn_NPCAdder.Text = "NPC Adder <<"
        'ElseIf d_dep_btn_NPCAdder.Text = "NPC Adder <<" Then ' If big make small
        'd_dep_btn_NPCAdder.Text = "NPC Adder >>"
        'End If
        'UpdateAdderDisplay("Natural")
    End Sub 'deprycated
    Private Sub UpdateAdderDisplay(ByVal action As String)
        Select Case action
            Case Is = "Natural"
                'If d_dep_btn_NPCAdder.Text = "NPC Adder >>" Then ' If is small
                'action = "Make Small"
                'ElseIf d_dep_btn_NPCAdder.Text = "NPC Adder <<" Then ' If is big
                'action = "Make Big"
                'End If
            Case Is = "Make Big"
            Case Is = "Make Small"
            Case Is = "CheckHide"
                If TabControl1.SelectedTab.Text IsNot TabControl1.TabPages(3).Text Then ' If tab not selected, make adder small
                    action = "Make Small"
                Else ' if tab is selected, run through this sub again but tell it to do it naturally
                    UpdateAdderDisplay("Natural")
                    Exit Sub ' exit sub now since Adder was updated in the above line
                End If
            Case Else
                Exit Sub
        End Select

        Select Case action
            Case Is = "Make Big"
                TabControl1.Height = TabControlBigHeight
            Case Is = "Make Small"
                TabControl1.Height = TabControlSmallHeight
        End Select
    End Sub 'deprycated
#End Region

End Class

