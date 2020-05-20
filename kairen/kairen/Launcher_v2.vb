Public Class Launcher_v2
    Public CLA As New CommandLineArgumentInterpreter
    Public Shared lb As CommonLibrary
    Public Extension_InstallationsFile As String = ".txt"
    ' Versions
    Public ReadOnly Version_Current_Release As String = "2.2.10"
    Dim Version_Current_Launcher As String = "0.3"
    Dim Version_Current_Options As String = "1.1"

    ' File Paths
    Dim Path_File_Version As String ' = lb.IO_EQOA & "Version" & lb.ex
    Dim Path_File_Options As String ' = lb.IO_Kairen & "Options" & lb.ex
    Dim Path_Folder_Options As String

    ' Classes
    'Dim File_Options As TextFileClass
    Dim File_Options As TextFileClass ' = New TextFileClass(Path_File_Options, "--")
    Dim File_Version_Information As TextFileClass ' = New TextFileClass(Path_File_Version, "--")
    Dim RF() As ResourceFileClass
    Dim RFHandler As New ResourceFileHandler

    ' Code Execution Control Variables
    Dim AllowOptionChanging As Boolean = False

    ' Value Holders
    Dim UpdateAction_NextAction As String = Nothing
    Dim Countdown As Integer = 1500
    Dim DoLaunch(2) As Boolean '0 = LoadProcess, 1 = Initiation, 2 = Initialization
    Dim TurnOnLaunchButtonAnyway As Boolean = True
    Private IgnoreMissingInstallList(0) As String

    Dim NewLine = vbNewLine
    Private Sub InstallationPromptWindow()
        CLA.ParseCommandLineArguments()
        If CLA.AutoLaunchCountDown_Flag > -1 Then
            Countdown = CLA.CommandLineArgument_Argument(CLA.AutoLaunchCountDown_Flag)
        End If
        Dim RDSYFolder As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\..\..\..\Rundatshityo\"
        Dim InstallationList As New TextFileClass(RDSYFolder & "Kairen\Additional Installations" & Extension_InstallationsFile, "--")
        InstallationList.LoadFile()
        If InstallationList.NumberOfLines > 0 Then
            If CLA.ChooseInstallation_Flag > -1 Then
                If CLA.CommandLineArgument_Argument(CLA.ChooseInstallation_Flag) = "Default Installation" Then
                    LoadProcess()
                    Exit Sub
                Else
                    Do Until InstallationList.CurrentIndex = InstallationList.NumberOfLines - 1
                        If InstallationList.ReadLine() = CLA.CommandLineArgument_Argument(CLA.ChooseInstallation_Flag) Then
                            LoadProcess(InstallationList.Line(InstallationList.CurrentIndex - 1))
                            Exit Sub
                        End If
                    Loop
                End If
            Else
                Me.Enabled = False
                InstallationPrompt.Show()
                InstallationPrompt.SendingObj = Me
                Exit Sub
            End If
        Else
            LoadProcess()
            Exit Sub
        End If
        DoOutput_new(Chr(34) & CLA.CommandLineArgument_Argument(CLA.ChooseInstallation_Flag) & Chr(34) & " installation was told to launch but isn't listed in your Alternate Installations file. The launcher will not continue.", " *** ")
        DoOutput_new()
        'LoadProcess("EQOA\")
    End Sub
    Private Sub VespaWoman()
        Select Case Version_Current_Release
            Case "3.3.3"
                DoOutput_new("[LATE NIGHT EQOA LABS INCORPORATED]", "--")
            Case "4.4.44"
                DoOutput_new("LNELILNELILNELILNELI", "--")
                'Case "2.2"
                'DoOutput_new("Brought to you by Late Night EQOA Labs Incorporated", "--")
        End Select
    End Sub
    Private Sub Launcher_v2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(250, 100)
        'lb = New CommonLibrary("EQOA\")
        'lb = New CommonLibrary("EQOA_Syba\")
        'LaunchKairen()
        'Exit Sub
        'Countdown = 0
        VespaWoman()
        InstallationPromptWindow()
    End Sub

    Public Sub LoadProcess(Optional ByVal EQOAFolder As String = Nothing)
        If EQOAFolder = Nothing Then
            EQOAFolder = "EQOA\"
            Path_Folder_Options = ""
        Else
            Path_Folder_Options = EQOAFolder
        End If
        Button3.Enabled = True
        If InstallationPrompt IsNot Nothing Then
            InstallationPrompt.ActuallyClose = True
            InstallationPrompt.Close()
        End If
        lb = New CommonLibrary(EQOAFolder, Path_Folder_Options)
        Path_File_Version = lb.IO_EQOA & "Version" & lb.Extension_VersionFile
        Path_File_Options = lb.IO_Options & "Options" & lb.Extension_OptionsFile
        File_Options = New TextFileClass(Path_File_Options, "--")
        File_Version_Information = New TextFileClass(Path_File_Version, "--")
        IgnoreMissingInstallList(0) = lb.Folder_Custom_Data
        'lb.PositionForm(Me, 250, 100)
        'do opening output
        'load [current] version resource files
        'check options file, mark missing\exist
        'check current version fully installed, mark missing files \ inappropriate versions
        'if options file exists, process options, else, create paths and it, [then do readoptionsfile function]
        'finish loading

        'do opening output
        Me.Text = Me.Text & " " & Version_Current_Launcher
        DoOutput_new("Welcome to Kairen Launcher!", " -- ")
        DoOutput_new("[Release " & Version_Current_Release & "]", " -- ")
        DoOutput_new("The Current Launcher Version is: " & Version_Current_Launcher, " -- ")
        DoOutput_new("The Current Options File Version is: " & Version_Current_Options, " -- ")
        DoOutput_new("Your Installation Folder is: \" & lb.Folder_EQOA.Replace(lb.IO_Root, ""), " -- ")

        Initiation_new()

        Initialize_new()
        DoOutput_new()
        If DoLaunch(1) And DoLaunch(2) Then DoLaunch(0) = True Else DoLaunch(0) = False
        If DoLaunch(0) = True Or TurnOnLaunchButtonAnyway = True Then
            DoOutput_new("Enabling Launch Button.")
            Button4.Enabled = True
        Else
            DoOutput_new("Unable to allow Launch.")
        End If
        If CBool(File_Options.GetValueByAdditionalData("Auto-Launch")) = True Then
            If Countdown = -1 Or DoLaunch(0) = False Then
                DoOutput_new()
                DoOutput_new("Kairen was told not to Auto-Launch.") 'command line argument can cause this
            ElseIf Countdown >= 0 And DoLaunch(0) = True Then
                If DoLaunch(0) Then
                    DoOutput_new()
                    DoOutput_new("Auto-Launch Enabled, Launching Kairen...")
                    Button4.Text = "Press To Cancel Launch"
                    Timer1.Start()
                ElseIf TurnOnLaunchButtonAnyway = True Then
                    DoOutput_new("Auto-Launch Enabled, but Kairen was told not to.")
                    DoOutput_new("Enabling Launch Button though.")
                Else
                    DoOutput_new("Auto-Launch Enabled, but Kairen is Not Ready, so it can't.")
                End If
            End If
        End If

        'Dim k As Kairen2
        'k = New Kairen2(lb)
        'k.Changelog_KairenContent = New TextFileClass(lb.IO_Kairen & "Changelog_KairenContent" & lb.Extension_Changelog, "--")
        'k.AppendLogEntrySeparator(OutPut_TextBox)
        'k.DisplayLastXChangelogEntries(k.Changelog_KairenContent, OutPut_TextBox, 1, 1)

        'yellow launch text after files were updated
        'If Button4.Enabled = True And CBool(File_Options.GetValueByAdditionalData("Auto-Launch")) = True Then
        '    Button4.ForeColor = Color.Orange
        'ElseIf Button4.Enabled = True And CBool(File_Options.GetValueByAdditionalData("Auto-Launch")) = False Then
        '    Button4.ForeColor = Color.Green
        'ElseIf Button4.Enabled = False Then
        '    Button3.ForeColor = Color.Green
        'End If
        'green launch text after files were updated
        If DoLaunch(0) = False Then
            Button4.ForeColor = Color.Orange
        ElseIf Button4.Enabled = True And CBool(File_Options.GetValueByAdditionalData("Auto-Launch")) = True And Countdown <> -1 Then
            Button4.ForeColor = Color.Orange
        ElseIf Button4.Enabled = True And CBool(File_Options.GetValueByAdditionalData("Auto-Launch")) = True And Countdown = -1 Then
            Button4.ForeColor = Color.Green
        ElseIf Button4.Enabled = True And CBool(File_Options.GetValueByAdditionalData("Auto-Launch")) = False Then
            Button4.ForeColor = Color.Green
        ElseIf Button4.Enabled = False Then
            Button3.ForeColor = Color.Green
        End If
        If Button4.Enabled = False Then
            Button3.ForeColor = Color.Green
        End If
        Button4.Focus()
    End Sub
    Private Sub Initiation_new()
        'load [current] version resource files
        DoOutput_new("")
        DoOutput_new("Initiating..")
        If lb.FE(lb.IO_Root & "Archives\Kairen Executables\Kairen " & Version_Current_Release & ".exe") Then
            DoOutput_new("Kairen " & Version_Current_Release & ".exe Backup Found.")
        Else
            If lb.DE(lb.IO_Root & "Archives\") = False Then lb.CD(lb.IO_Root & "Archives\")
            If lb.DE(lb.IO_Root & "Archives\Kairen Executables\") = False Then lb.CD(lb.IO_Root & "Archives\Kairen Executables\")
            My.Computer.FileSystem.CopyFile(Application.ExecutablePath, lb.IO_Root & "Archives\Kairen Executables\Kairen " & Version_Current_Release & ".exe")
            DoOutput_new("Kairen " & Version_Current_Release & ".exe Backup Has Been Created.")
        End If
        LoadResourceFiles_new(Version_Current_Release)
        DoOutput_new("Resource Files Loaded.")
        DoLaunch(1) = True
        'Find or Create Installation Folder - \Rundatshityo\
        If lb.DE(lb.IO_Root) = False Then
            DoOutput_new("Installation Folder Does Not Exist, Creating It..")
            My.Computer.FileSystem.CreateDirectory(lb.IO_Root)
            DoLaunch(1) = False
        Else : DoOutput_new("Installation Folder Found.")
        End If

        'Find or Create Kairen Folder - \Rundatshityo\Kairen\
        If lb.DE(lb.IO_Kairen) = False Then
            DoOutput_new("Kairen Folder Does Not Exist, Creating It..")
            My.Computer.FileSystem.CreateDirectory(lb.IO_Kairen)
            DoLaunch(1) = False
        Else : DoOutput_new("Kairen Folder Found.")
        End If

        'if options file exists, process options, else, create paths and it, [then do readoptionsfile function]
        If File_Options.FileExists Then
            DoOutput_new("Options File Found.")
            File_Options.LoadFile()
            DoLaunch(1) = ReadOptionsFile_new()
        Else
            If Path_File_Options <> lb.IO_Kairen Then
                My.Computer.FileSystem.CreateDirectory(lb.IO_Kairen & Path_Folder_Options)
            End If
            CreateOptionsFile_new(Version_Current_Options)
            DoOutput_new("Created Option File Version: " & Version_Current_Options)
            DoLaunch(1) = False
        End If

        If lb.FE(lb.IO_Kairen & "ChangeLog_GameContent" & lb.Extension_Changelog) = False Then
            DoOutput_new("No ChangeLog_GameContent" & lb.Extension_Changelog & " file detected, writing default.")
            IO.File.WriteAllBytes(lb.IO_Options & "ChangeLog_GameContent" & lb.Extension_Changelog, My.Resources.Changelog_GameContent3)
            DoLaunch(1) = False
        End If
        'If lb.FE(lb.IO_Kairen & "ChangeLog_KairenContent" & lb.Extension_Changelog) = False Then
        '    DoOutput_new("No ChangeLog_KairenContent" & lb.Extension_Changelog & " file detected.")
        '    DoLaunch(1) = False
        'End If
        'IO.File.WriteAllBytes(lb.IO_Kairen & "ChangeLog_KairenContent" & lb.Extension_Changelog, My.Resources.ChangeLog_KairenContent)

        For Each AD In File_Options.AdditionalData
            If File_Options.GetValueByAdditionalData(AD) = "true" Or File_Options.GetValueByAdditionalData(AD) = "false" Then
                DoOutput_new(AD & " is set to " & CBool(File_Options.GetValueByAdditionalData(AD)) & ".")
            End If
        Next

        If DoLaunch(1) = False Then
            DoOutput_new("Initiation Failed.")
        Else
            DoOutput_new("Initiated.")
        End If
    End Sub
    Private Sub DoOutput_new(Optional ByVal _text As String = "", Optional ByVal _padding As String = Nothing)
        If _padding Is Nothing Then
            OutPut_TextBox.AppendText(_text & NewLine)
        Else
            OutPut_TextBox.AppendText(_padding & _text & _padding & NewLine)
        End If
    End Sub
    Private Sub CreateOptionsFile_new(ByVal _version As String)
        Select Case _version
            Case "0.1"
                Dim _filelines(9) As String
                _filelines(0) = "-- Version"
                _filelines(1) = "0.1"
                _filelines(2) = "-- Auto-Install Latest Versions"
                _filelines(3) = "false"
                _filelines(4) = "-- Auto-Repair Missing Files"
                _filelines(5) = "false"
                _filelines(6) = "-- Auto-Repair Broken Files"
                _filelines(7) = "false"
                _filelines(8) = "-- Auto-Launch"
                _filelines(9) = "false"

                File_Options.WriteAllLines(_filelines)
                File_Options.SaveFile()
                File_Options.LoadFile()
                
            Case "1.0"
                File_Options.WriteLine("1.0")
                File_Options.WriteLine("Version")

                File_Options.WriteLine("false")
                File_Options.WriteLine("Auto-Install Files")

                File_Options.WriteLine("false")
                File_Options.WriteLine("Auto-Launch")

                File_Options.SaveFile()
                File_Options = New TextFileClass(Path_File_Options, "--")
                File_Options.LoadFile()
                ReadOptionsFile_new()

            Case "1.1"
                File_Options.WriteLine(_version)
                File_Options.WriteLine("Version")

                File_Options.WriteLine("false")
                File_Options.WriteLine("Auto-Install Files")

                File_Options.WriteLine("false")
                File_Options.WriteLine("Auto-Launch")

                File_Options.WriteLine("false")
                File_Options.WriteLine("Auto-Revive")

                File_Options.SaveFile()
                File_Options = New TextFileClass(Path_File_Options, "--")
                File_Options.LoadFile()
                ReadOptionsFile_new()

            Case Else
                DoOutput_new("File Unable to be created - Unkown Version Request: " & _version)
        End Select
    End Sub
    Private Function ReadOptionsFile_new()
        'File_Options.LoadFile() ' Open File..

        Select Case File_Options.ReadLine ' File Version..
            Case "0.1" ' File Version 0.1
                File_Options.AdditionalData(File_Options.CurrentIndex) = "File Version"
                File_Options.ReadLine()
                File_Options.AdditionalData(File_Options.CurrentIndex) = "Auto-Install Latest Versions"
                File_Options.ReadLine()
                File_Options.AdditionalData(File_Options.CurrentIndex) = "Auto-Repair Missing Files"
                File_Options.ReadLine()
                File_Options.AdditionalData(File_Options.CurrentIndex) = "Auto-Repair Broken Files"
                File_Options.ReadLine()
                File_Options.AdditionalData(File_Options.CurrentIndex) = "Auto-Launch"
                AllowOptionChanging = True
            Case "1.0", "1.1" ' File Version 1.0
                File_Options.AdditionalData(File_Options.CurrentIndex) = "File Version"
                File_Options.ReadLine()
                Dim loopcap As Integer = 0
                Do While File_Options.CurrentIndex + 2 <= File_Options.NumberOfLines
                    File_Options.ReadLine()
                    File_Options.AdditionalData(File_Options.CurrentIndex) = File_Options.ReadLine()
                    loopcap = loopcap + 1
                    If loopcap > 1000 Then Exit Do
                Loop
                AllowOptionChanging = True
            Case Else
                'error unrecognized version
                Return False
        End Select

        'display file options
        Dim AddData As String
        Dim result As String
        AddData = "Auto-Install Files"
        result = File_Options.GetValueByAdditionalData(AddData)
        If result <> "-1" And result <> "-2" Then
            CheckedListBox1.Items.Add(AddData, CBool(result))
        Else
            CheckedListBox1.Items.Add(AddData, False)
        End If

        AddData = "Auto-Launch"
        result = File_Options.GetValueByAdditionalData(AddData)
        If result <> "-1" And result <> "-2" Then
            CheckedListBox1.Items.Add(AddData, CBool(result))
        Else
            CheckedListBox1.Items.Add(AddData, False)
        End If

        AddData = "Auto-Revive"
        result = File_Options.GetValueByAdditionalData(AddData)
        If result <> "-1" And result <> "-2" Then
            CheckedListBox1.Items.Add(AddData, CBool(result))
        Else
            CheckedListBox1.Items.Add(AddData, False)
        End If

        If File_Options.GetValueByAdditionalData("File Version") <> Version_Current_Options Then
            CheckedListBox1.Items.Clear()
            UpdateKairenOptions()
            DoOutput_new("Kairen Options File was out of date and was updated.")
            Return False
        End If
        Return True
    End Function
    Private Sub UpdateKairenOptions()
        Select Case File_Options.GetValueByAdditionalData("File Version")
            'case is the OLD version.
            Case "0.1"
                Dim fotemp As TextFileClass = File_Options
                File_Options = New TextFileClass(fotemp.FilePath, "--")
                File_Options.WriteLine(Version_Current_Options)
                File_Options.WriteLine("File Version")
                File_Options.WriteLine(fotemp.GetValueByAdditionalData("Auto-Install Latest Versions"))
                File_Options.WriteLine("Auto-Install Files")
                File_Options.WriteLine(fotemp.GetValueByAdditionalData("Auto-Launch"))
                File_Options.WriteLine("Auto-Launch")
                File_Options.SaveFile()
                File_Options = New TextFileClass(fotemp.FilePath, "--")
                File_Options.LoadFile()
                ReadOptionsFile_new()
            Case "1.0"
                Dim fotemp As TextFileClass = File_Options
                File_Options = New TextFileClass(fotemp.FilePath, "--")
                File_Options.WriteLine(Version_Current_Options)
                File_Options.WriteLine("File Version")
                File_Options.WriteLine(fotemp.GetValueByAdditionalData("Auto-Install Files"))
                File_Options.WriteLine("Auto-Install Files")
                File_Options.WriteLine(fotemp.GetValueByAdditionalData("Auto-Launch"))
                File_Options.WriteLine("Auto-Launch")
                File_Options.WriteLine("false")
                File_Options.WriteLine("Auto-Revive")
                File_Options.SaveFile()
                File_Options = New TextFileClass(fotemp.FilePath, "--")
                File_Options.LoadFile()
                ReadOptionsFile_new()
        End Select
    End Sub
    Private Sub Initialize_new()
        DoOutput_new()
        DoOutput_new("Initializing..")
        If lb.DE(lb.IO_EQOA) Then
            If File_Version_Information.FileExists = True Then
                File_Version_Information.LoadFile()
                Dim releaseversion As String = ""
                If File_Version_Information.NumberOfLines > 0 Then
                    releaseversion = ReadReleaseVersionFile_new()
                End If
                If File_Version_Information.NumberOfLines = 0 Then
                    If File_Options.GetValueByAdditionalData("Auto-Install Files") = "true" Then
                        DoInstallSequence("Full")
                        DoOutput_new("Release Version File was not found, new installation completed.")
                        DoLaunch(2) = True
                    Else
                        DoOutput_new("Release Version File is empty.")
                        DoLaunch(2) = False
                    End If
                ElseIf releaseversion = Version_Current_Release Or releaseversion = "AUTD" Then
                    DoOutput_new("Latest Version Detected.")
                    If RFHandler.EverythingIsInstalled(IgnoreMissingInstallList) Then
                        DoOutput_new("All Files Detected")
                        DoLaunch(2) = True
                    ElseIf File_Options.GetValueByAdditionalData("Auto-Install Files") = "true" Then
                        DoInstallSequence("MissingOnly")
                        DoOutput_new("Missing files detected and replaced.")
                        DoLaunch(2) = True
                    Else
                        DoOutput_new("Missing files detected.")
                        DoLaunch(2) = False
                    End If
                ElseIf File_Options.GetValueByAdditionalData("Auto-Install Files") = "true" Then
                    DoOutput_new("Other Version Detected, Moving to Archive Folder..")
                    DoInstallSequence("Full")
                    DoLaunch(2) = True
                Else
                    DoOutput_new("Other Installation Version Detected.")
                    DoLaunch(2) = False
                End If
            ElseIf File_Options.GetValueByAdditionalData("Auto-Install Files") = "true" Then
                DoOutput_new("No Version File Found, Proceeding With New Install..")
                DoOutput_new("Moving Other Installation to Archive Folder..")
                DoInstallSequence("Full")
                'RFHandler.InstallTheseMissingFiles(IgnoreMissingInstallList)
                DoLaunch(2) = True
            Else
                DoOutput_new("No Version Detected.")
                DoLaunch(2) = False
            End If
        Else
            If File_Options.GetValueByAdditionalData("Auto-Install Files") = "true" Then
                DoInstallSequence("Full")
                DoOutput_new("Installation Complete.")
                DoLaunch(2) = True
            Else
                DoOutput_new("No Installation Found.")
                DoLaunch(2) = False
            End If
        End If

        If RFHandler.TheseAreInstalled(IgnoreMissingInstallList) = False Then
            If File_Options.GetValueByAdditionalData("Auto-Install Files") = "true" Then
                RFHandler.InstallTheseMissingFiles(IgnoreMissingInstallList)
                DoOutput_new("Custom Data folder was missing and has been replaced.")
                DoLaunch(2) = True
            Else
                DoOutput_new("Custom Data folder detected missing.")
                DoLaunch(2) = False
            End If
        End If
        If DoLaunch(2) = False Then
            TurnOnLaunchButtonAnyway = False
            DoOutput_new("Initialization Failed.")
        Else
            DoOutput_new("Initialized.")
        End If
    End Sub
    Private Function ReadReleaseVersionFile_new()
        Dim fml As String = File_Version_Information.ReadLine
        Select Case fml
            Case "1.0", "1.1"
                Dim _temp As String = File_Version_Information.ReadLine
                File_Version_Information.AdditionalData(File_Version_Information.CurrentIndex) = "Release Version"
                Return _temp
            Case Else
                Return Nothing
        End Select
    End Function
    Private Sub CreateReleaseVersionFile()
        Select Case Version_Current_Options
            Case "0.1", "1.0", "1.1"
                File_Version_Information = New TextFileClass(Path_File_Version, "--")
                File_Version_Information.WriteLine(Version_Current_Options)
                File_Version_Information.WriteLine(Version_Current_Release)
                File_Version_Information.SaveFile()
                'File_Version_Information.LoadFile()
                File_Version_Information = New TextFileClass(Path_File_Version, "--")
                File_Version_Information.LoadFile()
                ReadReleaseVersionFile_new()
        End Select
    End Sub 'updates with Version_Current_Options
    Private Function DoLaunchCheck()
        If lb.DE(lb.IO_EQOA) = False Then Return False
        If File_Version_Information.FileExists = False Then Return False
        File_Version_Information.LoadFile()
        If File_Version_Information.NumberOfLines = 0 Then : Return False
        ElseIf ReadReleaseVersionFile_new() = Version_Current_Release Then
            If RFHandler.EverythingIsInstalled Then : Return True : Else : Return False : End If
        Else : Return False
        End If
    End Function

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If Countdown <= 0 Then
            Timer1.Stop()
            LaunchKairen()
        Else
            Countdown = Countdown - Timer1.Interval
        End If
    End Sub


    Private Sub LaunchButtonHandler()
        If Button4.Text = "Press To Cancel Launch" Then
            Timer1.Stop()
            DoOutput_new()
            DoOutput_new("Auto-Launch Aborted.")
            Button4.Text = "Launch"
            If Button4.Enabled = False And Button3.Enabled = True Then
                Button3.ForeColor = Color.Green
                Button4.ForeColor = Color.Black
            ElseIf Button4.Enabled = True Then
                Button4.ForeColor = Color.Green
                Button3.ForeColor = Color.Black
            End If
            Exit Sub
        End If
        LaunchKairen()
    End Sub
    Private Sub LaunchKairen()
        'MsgBox("Consider Kairen Launched! .. Was it supposed to be launchable?" & NewLine & "Or are you trying to get this message to display but it's not?", MsgBoxStyle.OkOnly, "This is totally the NPC maker")
        'Exit Sub
        'lb.DisplayMessage("NOTIFICATION: LaunchKairen Current launches FormLoader instead.", "NOTIFICATION:", "Launcher.LaunchKairen")
        FormLoader.Show()
        FormLoader.Close()
        FormLoader.ReleaseVersion = Version_Current_Release
        FormLoader.AutoRevive = File_Options.GetValueByAdditionalData("Auto-Revive")
        Dim UserMenuControl As Object
        If UserMenu IsNot Nothing Then
            UserMenuControl = UserMenu
        ElseIf UserMenu_v2 IsNot Nothing Then
            UserMenuControl = UserMenu_v2
        End If
        If UserMenuControl IsNot Nothing Then UserMenuControl.Text = "Kairen " & Version_Current_Release & " - " & UserMenuControl.Text
        Me.Close()
        Exit Sub
        'Form1.Visible = True
        Kairen_Main.Visible = True
        Me.Close()
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        If Button4.Text = "Press To Cancel Launch" Then LaunchButtonHandler()
        If AllowOptionChanging = False Then Exit Sub
        Dim TextToFind As String = sender.Items.Item(e.Index).ToString
        File_Options.UpdateValueByAdditionalData(TextToFind, e.NewValue)
        File_Options.SaveFile()
    End Sub

    Private Sub Launcher_v2_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        'InstallationPrompt.Close()
        OutPut_TextBox.AppendText("")
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        LaunchButtonHandler()
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If Timer1.Enabled = True Then
            LaunchButtonHandler()
        End If
        sender.Enabled = False
        DoOutput_new()
        DoOutput_new("Starting Installation.")
        Button4.Enabled = False
        DoInstallSequence("Full")
        DoOutput_new("Installation Complete.")
        Button4.Enabled = True

        If Button4.Enabled = False And Button3.Enabled = True Then
            Button3.ForeColor = Color.Green
            Button4.ForeColor = Color.Black
        ElseIf Button4.Enabled = True Then
            Button4.ForeColor = Color.Green
            Button3.ForeColor = Color.Black
        End If
        sender.Enabled = True
    End Sub
    Private Sub DoInstallSequence(ByVal arg As String)
        'Me.Close()
        'Exit Sub
        Countdown = -1
        If arg = "MissingOnly" Then
            RFHandler.InstallMissingFiles(IgnoreMissingInstallList)
        End If
        If arg = "Full" Then
            'DoOutput_new("Let the developer know if this doesn't actually work.", "-- ! --")
            Dim NewPath As String = GetNewPath()
            DoOutput_new("This may take a moment, please wait..")
            If lb.DE(lb.IO_EQOA) Then
                Try
                    My.Computer.FileSystem.MoveDirectory(lb.IO_EQOA, NewPath)
                Catch ex As Exception
                End Try
            End If
            DoInstallSequence("MissingOnly")
            'If RFHandler.EverythingIsInstalled(IgnoreMissingInstallList) Then
            'DoOutput_new("Missing Files replaced only.")
            'Else
            DoInstallSequence("VersionFile")
            If lb.DE(NewPath) Then
                CopyCustomData(NewPath)
            Else
                RFHandler.InstallTheseMissingFiles(IgnoreMissingInstallList)
                DoOutput_new("Custom Data folder was missing and has been replaced.")
            End If
            'DoOutput_new("Full Installation was done.")
            'End If
        End If
        If arg = "VersionFile" Then
            CreateReleaseVersionFile()
            File_Version_Information.UpdateValueByAdditionalData("Release Version", Version_Current_Release)
            File_Version_Information.SaveFile()
        End If
    End Sub
    Public Function GetNewPath() As String
        Dim dirline As String = lb.IO_EQOA.Replace(lb.IO_Root, "").Replace("\", "")
        If lb.DE(lb.IO_EQOA) Then
            If File_Version_Information.FileExists = True Then
                File_Version_Information = New TextFileClass(Path_File_Version, "--")
                File_Version_Information.LoadFile()
                If File_Version_Information.NumberOfLines = 0 Then
                    File_Version_Information.DeleteFile()
                    Return GetNewPath()
                End If
                Dim _releaseVersion As String
                _releaseVersion = ReadReleaseVersionFile_new()
                Dim fp As String = lb.IO_Root & "Archives\EQOA\" & dirline & "-" & _releaseVersion & "\"
                If lb.DE(lb.IO_Root & "Archives\EQOA\" & dirline & "-" & _releaseVersion & "\") = True Then
                    Dim i As Integer = 2
                    fp = lb.IO_Root & "Archives\EQOA\" & dirline & "-" & _releaseVersion & "-Copy-" & i & "\"
                    Do Until lb.DE(fp) = False
                        i = i + 1
                        fp = lb.IO_Root & "Archives\EQOA\" & dirline & "-" & _releaseVersion & "-Copy-" & i & "\"
                    Loop
                End If
                Return fp
            Else
                Dim unNumber As Integer = 1
                Do While lb.DE(lb.IO_Root & "Archives\EQOA\" & dirline & "-UV-" & unNumber & "\") = True
                    unNumber = unNumber + 1
                Loop
                Return lb.IO_Root & "Archives\EQOA\" & dirline & "-UV-" & unNumber & "\"
            End If
        Else
            Return ""
        End If
    End Function
    Private Sub CopyCustomData(ByVal ArchivePath As String)
        If lb.DE(ArchivePath & "Custom Data\") Then
            My.Computer.FileSystem.CopyDirectory(ArchivePath & "Custom Data\", lb.Folder_Custom_Data)
            DoOutput_new("Custom Data Folder Copied Over.")
        Else
            DoOutput_new("Custom Data Folder not found for copying.")
            RFHandler.InstallTheseMissingFiles(IgnoreMissingInstallList)
            DoOutput_new("New Custom Data Folder created.")
        End If
    End Sub

    ''' <summary>
    ''' Custom Code Per Version, that loads each version into memory.
    ''' </summary>
    ''' <param name="_releaseVersion">The Version to load As String</param>
    ''' <remarks>This needs updated with every launcher release, even when none of it changes.</remarks>
    Private Sub LoadResourceFiles_new(ByVal _releaseVersion As String)
        Select Case _releaseVersion
            Case 0
                Dim i As Integer = -1
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA, "Version", ".txt", My.Resources.Version)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "Main", ".lua", My.Resources.Main)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\", "Area", ".lua", My.Resources.Area)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\", "Ghost", ".lua", My.Resources.Ghost)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\NPC Maker\", "NPC Maker", ".lua.", My.Resources.NPC_Maker)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\NPC Maker\", "Settings", ".lua.", My.Resources.Settings_NPCMaker)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\i\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\o\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\NPC Maker\NPC Areas\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\NPC Maker\NPC Areas\")
                ResourceFileClass.RFNext(RF, i, lb.Folder_CheatEngine)
                ResourceFileClass.RFNext(RF, i, lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTable)
            Case 1
                Dim i As Integer = -1
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "Main", ".lua", My.Resources.Main1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Functions\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings1)
                modename = "PsuedoNormal"
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\NPCs\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\i\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\o\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.Folder_CheatEngine)
                ResourceFileClass.RFNext(RF, i, lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTable1)
            Case 2
                Dim i As Integer = -1
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "Main", ".lua", My.Resources.Main2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Functions\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal2)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings2)
                modename = "PsuedoNormal"
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                ResourceFileClass.RFNext(RF, i, lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\NPCs\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\i\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Net Streams\o\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\Spawn Points\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\Spawn Camps\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Custom Data\Spawn Nests\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\")
                ResourceFileClass.RFNext(RF, i, lb.IO_EQOA & "Temp\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, lb.Folder_CheatEngine)
                ResourceFileClass.RFNext(RF, i, lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTable1)

            Case "2.1.5"
                Dim i As Integer = -1
                RFHandler.AddFile(lb.IO_EQOA)
                'RFHandler.AddFile(lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                RFHandler.AddFile(lb.IO_LUAs)
                RFHandler.AddFile(lb.IO_LUAs, "Main", ".lua", My.Resources.Main2)
                RFHandler.AddFile(lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput)
                RFHandler.AddFile(lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "Kanizah", ".lua", My.Resources.Kanizah)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal4)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings4)
                modename = "PsuedoNormal"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\i\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\o\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\NPC Maker\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Points\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Camps\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Nests\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\NPC Maker\")
                RFHandler.AddFile(lb.Folder_CheatEngine)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTable1)

            Case "2.1.6", "2.1.7", "2.1.8", "2.1.9", "2.1.10", "2.1.11", "2.1.12"
                Dim i As Integer = -1
                RFHandler.AddFile(lb.IO_EQOA)
                'RFHandler.AddFile(lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                RFHandler.AddFile(lb.IO_LUAs)
                RFHandler.AddFile(lb.IO_LUAs, "Main", ".lua", My.Resources.Main3)
                RFHandler.AddFile(lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput)
                RFHandler.AddFile(lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "Kanizah", ".lua", My.Resources.Kanizah2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal4)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings4)
                modename = "PsuedoNormal"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                modename = "World Population"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.World_Population)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.World_Population_Settings)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\i\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\o\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\NPC Maker\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Points\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Camps\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Nests\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\NPC Maker\")
                RFHandler.AddFile(lb.Folder_CheatEngine)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTable2)
                'this installation, and probably more, installed ce\io\Write_StringToBitArrayAddress1  and io\Write_StringToBitArrayAddress2

            Case "2.1.13", "2.1.14", "2.1.15", "2.1.16", "2.1.17", "2.1.18", "2.1.19"
                Dim i As Integer = -1
                RFHandler.AddFile(lb.IO_EQOA)
                RFHandler.AddFile(lb.IO_Kairen, "Changelog_GameContent", ".txt", My.Resources.ChangeLog_GameContent2)
                'RFHandler.AddFile(lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                RFHandler.AddFile(lb.IO_LUAs)
                RFHandler.AddFile(lb.IO_LUAs, "Main", ".lua", My.Resources.Main3)
                RFHandler.AddFile(lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput)
                RFHandler.AddFile(lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "Kanizah", ".lua", My.Resources.Kanizah2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal4)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings4)
                modename = "PsuedoNormal"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                modename = "World Population"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.World_Population)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.World_Population_Settings)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\i\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\o\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\NPC Maker\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Points\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Camps\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Nests\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\NPC Maker\")
                RFHandler.AddFile(lb.Folder_CheatEngine)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey)

            Case "2.2.0", "2.2.-1", "2.2.1", "2.2.-2", "2.2.2"
                Dim i As Integer = -1
                RFHandler.AddFile(lb.IO_EQOA)
                'RFHandler.AddFile(lb.IO_Kairen, "Changelog_GameContent", ".txt", My.Resources.ChangeLog_GameContent2)
                'RFHandler.AddFile(lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                RFHandler.AddFile(lb.IO_LUAs)
                RFHandler.AddFile(lb.IO_LUAs, "Main", ".lua", My.Resources.Main6)
                RFHandler.AddFile(lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput3)
                RFHandler.AddFile(lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC7)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "Kanizah", ".lua", My.Resources.Kanizah3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "HEX_DEC", ".lua", My.Resources.HEX_DEC)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal4)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings4)
                'modename = "PsuedoNormal"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                'modename = "Custom"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                modename = "World Population"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.World_Population3)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.World_Population_Settings3)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\i\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\o\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\NPC Maker\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Points\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Camps\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Nests\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\NPC Maker\")
                RFHandler.AddFile(lb.Folder_CheatEngine)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable_Silent", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey_SilentMode)
                
            Case "2.2.3", "2.2.4", "2.2.5", "2.2.6", "2.2.7"
                Dim i As Integer = -1
                RFHandler.AddFile(lb.IO_EQOA)
                'RFHandler.AddFile(lb.IO_Kairen, "Changelog_GameContent", ".txt", My.Resources.ChangeLog_GameContent2)
                'RFHandler.AddFile(lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                RFHandler.AddFile(lb.IO_LUAs)
                RFHandler.AddFile(lb.IO_LUAs, "Main", ".lua", My.Resources.Main6)
                RFHandler.AddFile(lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput3)
                RFHandler.AddFile(lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC7)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "Kanizah", ".lua", My.Resources.Kanizah3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "HEX_DEC", ".lua", My.Resources.HEX_DEC)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal4)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings4)
                'modename = "PsuedoNormal"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                'modename = "Custom"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                modename = "World Population"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.World_Population3)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.World_Population_Settings3)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Items\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\i\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\o\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\NPC Maker\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Points\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Camps\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Nests\")
                'RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Items\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\NPC Maker\")
                RFHandler.AddFile(lb.Folder_CheatEngine)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable_Silent", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey_SilentMode)

            Case "2.2.8", "2.2.9", "2.2.10"
                Dim i As Integer = -1
                RFHandler.AddFile(lb.IO_EQOA)
                'RFHandler.AddFile(lb.IO_Kairen, "Changelog_GameContent", ".txt", My.Resources.ChangeLog_GameContent2)
                'RFHandler.AddFile(lb.IO_EQOA, "Version", ".txt", My.Resources.Version1)
                RFHandler.AddFile(lb.IO_LUAs)
                RFHandler.AddFile(lb.IO_LUAs, "Main", ".lua", My.Resources.Main6)
                RFHandler.AddFile(lb.IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput3)
                RFHandler.AddFile(lb.IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address5)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress3)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC7)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Classes\", "Kanizah", ".lua", My.Resources.Kanizah4)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Helper Functions\", "HEX_DEC", ".lua", My.Resources.HEX_DEC)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\")
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                RFHandler.AddFile(lb.IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal4)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings4)
                'modename = "PsuedoNormal"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                'modename = "Custom"
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                'RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                modename = "World Population"
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\")
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.World_Population3)
                RFHandler.AddFile(lb.IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.World_Population_Settings4)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\NPCs\", "Randall_Adams", ".txt.", My.Resources.Randall_Adams1)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\")
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                RFHandler.AddFile(lb.IO_EQOA & "Game Data\Items\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\i\")
                RFHandler.AddFile(lb.IO_EQOA & "Net Streams\o\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\NPC Maker\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Points\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Camps\")
                RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Spawn Nests\")
                'RFHandler.AddFile(lb.IO_EQOA & "Custom Data\Items\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\")
                RFHandler.AddFile(lb.IO_EQOA & "Temp\NPC Maker\")
                RFHandler.AddFile(lb.Folder_CheatEngine)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey)
                RFHandler.AddFile(lb.Folder_CheatEngine, "MainTable_Silent", lb.Extension_CheatTable, My.Resources.MainTableJustWorldPopulationKey_SilentMode)

            Case Else
                DoOutput_new("Error! LoadResourceFiles() was passed an un-programmed _releaseVersion: " & _releaseVersion)
                DoOutput_new("Unknown Errors May Occur.. sorry. I have not had time to program this part properly yet.")
                DoOutput_new("")
                MsgBox("Must Close: No Resource Set Loaded", MsgBoxStyle.Critical, "Error: I shall not pass.")
                Application.Exit()
        End Select
    End Sub
End Class