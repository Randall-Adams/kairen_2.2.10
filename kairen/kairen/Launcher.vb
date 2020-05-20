Public Class Launcher
    ' Versions
    Dim Version_Current_Release As String = "2"
    Dim Version_Current_Launcher As String = "0.1.0"
    Dim Version_Current_Options As String = "0.1"
    Dim Version_Current_Kairen As String = "-0"

    ' Classes
    Dim lb As New CommonLibrary("EQOA\", "")
    Dim File_Options As TextFileClass
    Dim File_Version_Information As TextFileClass
    Dim IO_Root As String = lb.IO_Root
    Dim RF() As ResourceFileClass

    ' File Paths
    Dim IO_EQOA As String = lb.IO_EQOA
    Dim IO_LUAs As String = lb.IO_LUAs
    Dim IO_Kairen As String = lb.IO_Kairen
    Dim Path_File_Version As String = IO_EQOA & "Version.txt"
    Dim Path_File_Options As String = IO_Kairen & "Options.txt"

    ' Code Execution Control Variables
    Dim AllowOptionChanging As Boolean = False

    ' Program Variables
    ' Options - "Value By Text" to pass to TextFileClass
    Dim o_Version As String = ""
    Dim o_AutoInstallNew As String = "Auto-Install Latest Versions"
    Dim o_AutoRepairMissing As String = "Auto-Repair Missing Files"
    Dim o_AutoRepairBroken As String = "Auto-Repair Broken Files"
    Dim o_AutoLaunch As String = "Auto-Launch"
    ' Value Holders
    Dim UpdateAction_NextAction As String = Nothing
    Dim Countdown As Integer = 1500

    Dim NewLine = vbNewLine


    Private Sub Launcher_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If ("Do New Launcher" <> "") = True Then
            ''Launcher_v2.Show()
            Me.Close()
            Exit Sub
        End If
        'Launcher_v2.Show()
        'Me.Close()
        'Exit Sub
        'Countdown = 0
        lb.PositionForm(Me, 250, 100)
        'do opening output
        'load [current] version resource files
        'check options file, mark missing\exist
        'check current version fully installed, mark missing files \ inappropriate versions
        'if options file exists, process options, else, create paths and it, [then do readoptionsfile function]
        'finish loading

        'do opening output
        Me.Text = Me.Text & " " & Version_Current_Launcher
        DoOutput("Welcome to Kairen Launcher!", " -- ")
        DoOutput("[Release " & Version_Current_Release & "]", " -- ")
        DoOutput("The Current Launcher Version is: " & Version_Current_Launcher, " -- ")
        DoOutput("The Current Options File Version is: " & Version_Current_Options, " -- ")

        'load [current] version resource files
        UpdateAction("Starting...")

        UpdateAction("Loading Resource Files..", "Done.")
        LoadResourceFiles(Version_Current_Release)

        'if options file exists, process options, else, create paths and it, [then do readoptionsfile function]

        LoadOptionsFolder()


        'finish loading
        If ResourceFileClass.EverythingIsInstalled(RF) Then d_pb_Progress.Value = 100
        If d_pb_Progress.Value = d_pb_Progress.Maximum Then
            UpdateAction("All files found, Kairen is Ready! Enabling Launch Button.")
            d_btn_Launch.Enabled = True
        Else : UpdateAction("Some Files Are Missing, Launch Button Staying Disabled.")
        End If
        UpdateAction("Finished!")
        UpdateAction("")
        If CBool(File_Options.GetValueByAdditionalData(o_AutoLaunch)) = True Then
            If d_pb_Progress.Value = d_pb_Progress.Maximum Then
                UpdateAction("Auto-Launch Enabled, Launching Kairen...")
                UpdateAction("")
                d_btn_Launch.Text = "Press To Cancel Launch"
                Timer1.Start()
            Else
                UpdateAction("Auto-Launch Enabled, but Kairen is Not Ready, so it can't.")
                UpdateAction("")
            End If
        End If
    End Sub
    Private Sub ReadOptionsFile()
        File_Options.LoadFile() ' Open File..

        Select Case File_Options.ReadLine ' File Version..
            Case "0.1" ' File Version 0.1
                d_chkbx_ToggleOptions.Items.Add(o_AutoInstallNew, CBool(File_Options.ReadLine)) ' Adds Text to Option Box ' Reads Next Line, Auto Install..
                File_Options.AdditionalData(File_Options.CurrentIndex) = d_chkbx_ToggleOptions.Items.Item(d_chkbx_ToggleOptions.Items.Count - 1).ToString

                d_chkbx_ToggleOptions.Items.Add(o_AutoRepairMissing, CBool(File_Options.ReadLine)) ' Auto Repair Missing..
                File_Options.AdditionalData(File_Options.CurrentIndex) = d_chkbx_ToggleOptions.Items.Item(d_chkbx_ToggleOptions.Items.Count - 1).ToString

                d_chkbx_ToggleOptions.Items.Add(o_AutoRepairBroken, CBool(File_Options.ReadLine)) ' Auto Repair Broken..
                File_Options.AdditionalData(File_Options.CurrentIndex) = d_chkbx_ToggleOptions.Items.Item(d_chkbx_ToggleOptions.Items.Count - 1).ToString

                d_chkbx_ToggleOptions.Items.Add(o_AutoLaunch, CBool(File_Options.ReadLine)) ' Auto Launch..
                File_Options.AdditionalData(File_Options.CurrentIndex) = d_chkbx_ToggleOptions.Items.Item(d_chkbx_ToggleOptions.Items.Count - 1).ToString

                AllowOptionChanging = True
        End Select
    End Sub
    Private Function ReadReleaseVersionFile()
        Select Case File_Version_Information.ReadLine
            Case "1.0"
                Dim _temp As String = File_Version_Information.ReadLine
                Return _temp
            Case Else
                Return Nothing
        End Select
    End Function

    Private Sub d_chkbx_ToggleOptions_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles d_chkbx_ToggleOptions.ItemCheck
        If d_btn_Launch.Text = "Press To Cancel Launch" Then LaunchButtonHandler()
        If AllowOptionChanging = False Then Exit Sub
        Dim TextToFind As String = d_chkbx_ToggleOptions.Items.Item(e.Index).ToString
        File_Options.UpdateValueByAdditionalData(TextToFind, e.NewValue)
        File_Options.SaveFile()
    End Sub
    Private Sub d_btn_Launch_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_Launch.Click
        LaunchButtonHandler()
    End Sub
    Private Sub LaunchButtonHandler()
        If d_btn_Launch.Text = "Press To Cancel Launch" Then
            Timer1.Stop()
            UpdateAction("Auto-Launch Aborted.")
            UpdateAction("")
            d_btn_Launch.Text = "Launch"
            Exit Sub
        End If
        LaunchKairen()
    End Sub
    Private Sub LaunchKairen()
        lb.DisplayMessage("NOTIFICATION: LaunchKairen Current launches FormLoader instead.", "NOTIFICATION:", "Launcher.LaunchKairen")
        FormLoader.Show()
        'FormLoader.Close()
        Me.Close()
        Exit Sub
        'Form1.Visible = True
        Kairen_Main.Visible = True
        Me.Close()
    End Sub
    Private Sub DoOutput(ByVal _text As String, Optional ByVal _padding As String = Nothing)
        If _padding Is Nothing Then
            d_tb_Output.AppendText(_text & NewLine)
        Else
            d_tb_Output.AppendText(_padding & _text & _padding & NewLine)
        End If
    End Sub
    Private Sub UpdateAction(ByVal _newAction As String, Optional ByVal _nextAction As String = Nothing)
        'could make this\dooutput append text too, instead of adding in the next line automatically.
        If UpdateAction_NextAction <> Nothing Then
            DoOutput(d_lbl_CurrentAction.Text)
            d_lbl_CurrentAction.Text = UpdateAction_NextAction

            DoOutput(d_lbl_CurrentAction.Text)
            d_lbl_CurrentAction.Text = _newAction
            UpdateAction_NextAction = Nothing
        Else
            DoOutput(d_lbl_CurrentAction.Text)
            d_lbl_CurrentAction.Text = _newAction
        End If
        If _nextAction <> Nothing Then UpdateAction_NextAction = _nextAction
    End Sub

    Private Sub CreateOptionsFile(ByVal _version As String)
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

            Case Else
                UpdateAction("File Unable to be created - Unkown Version Request: " & _version)
        End Select
    End Sub

    Private Sub d_btn_DeleteAndClose_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_DeleteAndClose.Click
        If lb.FE(Path_File_Options) Then
            My.Computer.FileSystem.DeleteFile(Path_File_Options)
            Me.Close()
        Else
            DoOutput("File not found to delete.")
        End If
    End Sub
    Private Sub InstallNewFiles()

    End Sub
    Private Sub CheckForFiles(ByVal _releaseVersion As String, Optional ByVal Install As Boolean = False)
        Select Case _releaseVersion
            Case 0
                If lb.DE(IO_EQOA) = False Then
                    If Install Then My.Computer.FileSystem.CreateDirectory(IO_EQOA)
                Else
                End If
                If lb.DE(IO_LUAs) = False Then
                    If Install Then My.Computer.FileSystem.CreateDirectory(IO_LUAs)
                Else
                End If
                If lb.FE(IO_LUAs & "Main.lua") = False Then
                    UpdateAction("File Not Found: Main.lua")
                    If Install = False Then UpdateAction("Auto-Install Turned Off, so this will not be installed.")
                    If Install Then UpdateAction("Installing Main.lua..")
                    If Install Then RF(0).InstallFile()
                Else

                End If
                If lb.FE(IO_LUAs & "File Paths.lua") = False Then
                Else

                End If
                If lb.DE(IO_LUAs & "Modules\") = False Then
                    If Install Then My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Modules\")
                Else
                End If
                If lb.DE(IO_LUAs & "Modules\FAPI\") = False Then
                    If Install Then My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Modules\FAPI\")
                Else
                End If
                If lb.FE(IO_LUAs & "Modules\FAPI\FAPI.lua") = False Then
                Else

                End If
                If lb.DE(IO_LUAs & "Modules\FAPI\Classes\") = False Then
                    If Install Then My.Computer.FileSystem.CreateDirectory(IO_EQOA & "Modules\FAPI\Classes\")
                Else
                End If
                If lb.FE(IO_LUAs & "Modules\FAPI\Classes\Area.lua") = False Then
                Else

                End If
                If lb.FE(IO_LUAs & "Modules\FAPI\Classes\Ghost.lua") = False Then
                Else

                End If
                If lb.FE(IO_LUAs & "Modules\FAPI\Classes\NPC.lua") = False Then
                Else

                End If
                'custom data, cd npc maker, cd npc maker npc areas, ?, 
                'game data, gd ghosts, gd npcs, gd npc areas, ?, 
                'temp, t npc maker, t npc mkr npc areas, 
                'eqoa luas modes ?, 

            Case Else
                UpdateAction("File Unable to be Checked for - Unkown Version Request: " & _releaseVersion)
        End Select
    End Sub
    ''' <summary>
    ''' Custom Code Per Version, that loads each version into memory.
    ''' </summary>
    ''' <param name="_releaseVersion">The Version to load As String</param>
    ''' <remarks>This needs updated with every launcher release, even when none of it changes.</remarks>
    Private Sub LoadResourceFiles(ByVal _releaseVersion As String)
        Select Case _releaseVersion
            Case 0
                Dim i As Integer = -1
                ResourceFileClass.RFNext(RF, i, IO_EQOA)
                ResourceFileClass.RFNext(RF, i, IO_EQOA, "Version", ".txt", My.Resources.Version)
                ResourceFileClass.RFNext(RF, i, IO_LUAs)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "Main", ".lua", My.Resources.Main)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\", "Area", ".lua", My.Resources.Area)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\", "Ghost", ".lua", My.Resources.Ghost)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\NPC Maker\", "NPC Maker", ".lua.", My.Resources.NPC_Maker)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\NPC Maker\", "Settings", ".lua.", My.Resources.Settings_NPCMaker)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\i\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\o\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\NPC Maker\NPC Areas\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\NPC Maker\NPC Areas\Zones\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\NPC Maker\NPC Areas\")
                ResourceFileClass.RFNext(RF, i, IO_Root & "Cheat Engine\")
                ResourceFileClass.RFNext(RF, i, IO_Root & "Cheat Engine\", "MainTable", ".ct", My.Resources.MainTable)
            Case 1
                Dim i As Integer = -1
                ResourceFileClass.RFNext(RF, i, IO_EQOA)
                ResourceFileClass.RFNext(RF, i, IO_EQOA, "Version", ".txt", My.Resources.Version1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "Main", ".lua", My.Resources.Main1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\IO\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Functions\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings1)
                modename = "PsuedoNormal"
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\NPCs\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\i\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\o\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_Root & "Cheat Engine\")
                ResourceFileClass.RFNext(RF, i, IO_Root & "Cheat Engine\", "MainTable", ".ct", My.Resources.MainTable1)
            Case 2
                Dim i As Integer = -1
                ResourceFileClass.RFNext(RF, i, IO_EQOA)
                ResourceFileClass.RFNext(RF, i, IO_EQOA, "Version", ".txt", My.Resources.Version1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "Main", ".lua", My.Resources.Main2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "ConsoleOutput", ".lua", My.Resources.ConsoleOutput)
                ResourceFileClass.RFNext(RF, i, IO_LUAs, "File Paths", ".lua", My.Resources.File_Paths2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\", "FAPI", ".lua", My.Resources.FAPI2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\", "Address", ".lua", My.Resources.Address2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\IO\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\CE\IO\", "Write_StringToBitArrayAddress", ".lua", My.Resources.Write_StringToBitArrayAddress1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\", "NPC", ".lua", My.Resources.NPC2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Classes\", "SpawnHandler", ".lua", My.Resources.SpawnHandler2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Functions\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Functions\", "RunOptions", ".lua", My.Resources.RunOptions2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Helper Functions\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Helper Functions\", "Right", ".lua", My.Resources.Right2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Helper Functions\", "Left", ".lua", My.Resources.Left2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Helper Functions\", "Convert_BooleanToString", ".lua", My.Resources.Convert_BooleanToString2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "File_Exists", ".lua", My.Resources.File_Exists1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "ReadNextLine", ".lua", My.Resources.ReadNextLine2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "Read_BitArrayToString", ".lua", My.Resources.Read_BitArrayToString2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "Read_FileToStringArray", ".lua", My.Resources.Read_FileToStringArray2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\IO\", "Write_StringArrayToFile", ".lua", My.Resources.Write_StringArrayToFile1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "ProcessOutsideCommands", ".lua", My.Resources.ProcessOutsideCommands2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "CreateNPCFile", ".lua", My.Resources.CreateNPCFile1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "OutputPlayerData", ".lua", My.Resources.OutputPlayerData2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData", ".lua", My.Resources.Alert_OutputPlayerData2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_OutputPlayerData_ConsoleOutput", ".lua", My.Resources.Alert_OutputPlayerData_ConsoleOutput2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modules\FAPI\Kanizah\", "Alert_ProcessOutsideCommands", ".lua", My.Resources.Alert_ProcessOutsideCommands2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\")
                Dim modename As String = "Normal"
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Normal2)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Normal_Settings2)
                modename = "PsuedoNormal"
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.PsuedoNormal1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.PsuedoNormal_Settings1)
                modename = "Custom"
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\")
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename, ".lua.", My.Resources.Custom1)
                ResourceFileClass.RFNext(RF, i, IO_LUAs & "Modes\" & modename & "\", modename & "-Settings", ".lua.", My.Resources.Custom_Settings1)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\NPCs\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "a_badger", ".txt.", My.Resources.a_badger)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Aloj_Tilsteran", ".txt.", My.Resources.Aloj_Tilsteran)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "an_undead_mammoth", ".txt.", My.Resources.an_undead_mammoth)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Angry_Patron", ".txt.", My.Resources.Angry_Patron)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Arch_Familiar", ".txt.", My.Resources.Arch_Familiar)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Bowyer_Koll", ".txt.", My.Resources.Bowyer_Koll)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Coachman_Ronks", ".txt.", My.Resources.Coachman_Ronks)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Dr_Killian", ".txt.", My.Resources.Dr_Killian)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Finalquestt", ".txt.", My.Resources.Finalquestt)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Jahn", ".txt.", My.Resources.Guard_Jahn)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Perinen", ".txt.", My.Resources.Guard_Perinen)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Saolen", ".txt.", My.Resources.Guard_Saolen)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Guard_Serenda", ".txt.", My.Resources.Guard_Serenda)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Manoarmz", ".txt.", My.Resources.Manoarmz)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Marona_Jofranka", ".txt.", My.Resources.Marona_Jofranka)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Merchant_Ahkham", ".txt.", My.Resources.Merchant_Ahkham)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Merchant_Kari", ".txt.", My.Resources.Merchant_Kari)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Nukenurplace", ".txt.", My.Resources.Nukenurplace)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Raam", ".txt.", My.Resources.Raam)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Royce_Tilsteran", ".txt.", My.Resources.Royce_Tilsteran)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "sign_post", ".txt.", My.Resources.sign_post)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Tailor_Bariston", ".txt.", My.Resources.Tailor_Bariston)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Game Data\Ghosts\", "Tailor_Zixar", ".txt.", My.Resources.Tailor_Zixar)
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\i\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Net Streams\o\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\Spawn Points\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\Spawn Camps\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Custom Data\Spawn Nests\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\")
                ResourceFileClass.RFNext(RF, i, IO_EQOA & "Temp\NPC Maker\")
                ResourceFileClass.RFNext(RF, i, IO_Root & "Cheat Engine\")
                ResourceFileClass.RFNext(RF, i, IO_Root & "Cheat Engine\", "MainTable", ".ct", My.Resources.MainTable1)

            Case Else
                UpdateAction("Error! LoadResourceFiles() was passed an un-programmed _releaseVersion: " & _releaseVersion)
                UpdateAction("Unknown Errors May Occur.. sorry. I have not had time to program this part properly yet.")
                UpdateAction("")
        End Select

        If ResourceFileClass.EverythingIsInstalled(RF) = False Then
            UpdateAction("Some Files Are Missing..")
        End If

    End Sub
    Private Sub InstallAllFiles1() 'deprycated
        For Each File In RF
            File.InstallFile()
        Next
    End Sub

    Private Sub DoFAPIInstallation(ByVal _fileVersion_Path As String)
        UpdateAction("Checking for Current Installation..")
        If lb.DE(IO_EQOA) = True Then
            UpdateAction("Current Installation Found, Checking Release Version..")
            If lb.FE(_fileVersion_Path) Then
                File_Version_Information = New TextFileClass(_fileVersion_Path, "--")
                File_Version_Information.LoadFile()
                Dim _releaseVersion As String
                _releaseVersion = ReadReleaseVersionFile()
                If _releaseVersion <> Version_Current_Release Then
                    UpdateAction("Other Version Detected, Moving to Archive Folder..", "Done.")
                    My.Computer.FileSystem.CopyDirectory(IO_EQOA, IO_Root & "Archives\EQOA\EQOA-" & _releaseVersion & "\")
                ElseIf _releaseVersion = Version_Current_Release Then
                    UpdateAction("Latest Version Already installed.")
                    Exit Sub
                End If
            Else
                UpdateAction("No Version File Found, Proceeding With New Install..")
                UpdateAction("Other Version Detected, Moving to Archive Folder..", "Done.")
                Dim unNumber As Integer = 1
                Do While lb.DE(IO_Root & "Archives\EQOA\EQOA-UV-" & unNumber & "\") = True
                    unNumber = unNumber + 1
                Loop
                My.Computer.FileSystem.CopyDirectory(IO_EQOA, IO_Root & "Archives\EQOA\EQOA-UV-" & unNumber & "\")
            End If
        Else
            UpdateAction("No Current Installation Detected.")
        End If
        UpdateAction("Installing Selected Release Version..", "Done.")
        ResourceFileClass.InstallAllFiles(RF)
        UpdateAction("")
    End Sub

    Private Sub InstallMissingFiles()
        UpdateAction("Checking Installation Status..")
        If ResourceFileClass.EverythingIsInstalled(RF) = False Then
            UpdateAction("Some Files Are Missing, Reinstalling..", "Done.")
            ResourceFileClass.InstallAllMissingFiles(RF)
            If ResourceFileClass.EverythingIsInstalled(RF) Then d_pb_Progress.Value = 100
            If d_pb_Progress.Value = d_pb_Progress.Maximum Then
                UpdateAction("Everything Installed Successfully, Enabling Launch Button..")
            End If
            UpdateAction("")
        Else
            UpdateAction("Everything is already installed.")
            UpdateAction("")
        End If
    End Sub

    Private Sub LoadOptionsFolder()
        File_Options = New TextFileClass(Path_File_Options, "--")

        UpdateAction("Checking for Installation Folder..")
        If lb.DE(IO_Root) = False Then
            UpdateAction("Installation Folder Does Not Exist, Creating It..", "Done.")
            My.Computer.FileSystem.CreateDirectory(IO_Root)
        Else : UpdateAction("Installation Folder Found.")
        End If

        UpdateAction("Checking for Kairen Folder..")
        If lb.DE(IO_Kairen) = False Then
            UpdateAction("Kairen Folder Does Not Exist, Creating It..", "Done.")
            My.Computer.FileSystem.CreateDirectory(IO_Kairen)
        Else : UpdateAction("Kairen Folder Found.")
        End If

        UpdateAction("Checking for Options File..")
        If File_Options.FileExists = False Then
            UpdateAction("Options File Does Not Exist..")
            UpdateAction("Creating Option File Version: " & Version_Current_Options, "Done.")
            CreateOptionsFile(Version_Current_Options)
        Else : UpdateAction("Options File Found.")
        End If
        UpdateAction("Reading Options File To Memory..", "Done.")
        ReadOptionsFile()
        If CBool(File_Options.GetValueByAdditionalData(o_AutoInstallNew)) = True Then
            UpdateAction(o_AutoInstallNew & " is set to true.")

            DoFAPIInstallation(Path_File_Version)
        Else : UpdateAction(o_AutoInstallNew & " is set to false.")
        End If
        If CBool(File_Options.GetValueByAdditionalData(o_AutoRepairMissing)) = True Then
            UpdateAction(o_AutoRepairMissing & " is set to true.")
            InstallMissingFiles()
        Else : UpdateAction(o_AutoRepairMissing & " is set to false.")
        End If
        If CBool(File_Options.GetValueByAdditionalData(o_AutoRepairBroken)) = True Then
            UpdateAction(o_AutoRepairBroken & " is set to true.")
        Else : UpdateAction(o_AutoRepairBroken & " is set to false.")
        End If
        If CBool(File_Options.GetValueByAdditionalData(o_AutoLaunch)) = True Then
            UpdateAction(o_AutoLaunch & " is set to true.")
            UpdateAction("Kairen will launch automatically if it is able to.")
        Else : UpdateAction(o_AutoLaunch & " is set to false.")
        End If
    End Sub

#Region "Display Control Handlers"
    Private Sub d_btn_InstallMissingFiles_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_InstallMissingFiles.Click
        InstallMissingFiles()
    End Sub

    Private Sub d_btn_InstallLatestVersion_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_InstallLatestVersion.Click
        DoFAPIInstallation(Path_File_Version)
        If ResourceFileClass.EverythingIsInstalled(RF) Then
            UpdateAction("Kairen is Ready! Enabling Launch Button.")
            UpdateAction("")
            d_btn_Launch.Enabled = True
        End If
    End Sub
#End Region

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If Countdown <= 0 Then
            Timer1.Stop()
            LaunchKairen()
        Else
            Countdown = Countdown - Timer1.Interval
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If lb.DE(lb.Folder_EQOA) Then My.Computer.FileSystem.DeleteDirectory(lb.Folder_EQOA, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
    End Sub
End Class