Public Class UserMenu
    Public CloseFormLoaderToo As Boolean = True

    Private lb As CommonLibrary = FormLoader.lb
    Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection
    Private Kairen As Kairen2 = FormLoader.Kairen
    Dim FAPIItems(-1) As String
    Dim Timer2ElapsedTime As Integer = 0
    'Not in a tab
    Private Sub UserMenu_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
        FAPI_Connection.ObjectsToUpdate.Remove(Me)
        Application.Exit()
    End Sub
    Private Sub UserMenu_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 350, 250)
        Me.TopMost = True
        Me.TopMost = False
        Kairen.DisplayLastXChangelogEntries("Changelog_GameContent", TextBox1, 10, 10)
        If Kairen.LoadOptionFile("ProgramOptionsFile") Then
            Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data", TextBox3)
            Kairen.ProgramOptionsFile.GetValueByAdditionalData("[ISO Path] Data", TextBox4)
            Kairen.ProgramOptionsFile.GetValueByAdditionalData("[NPC Hails] Data", CheckBox1)
            Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Player Name] Data", TextBox2)
            If TextBox2.Text <> "" Then
                FAPI_Connection.IssueFAPICommand(4, "PlayerName", TextBox2.Text)
            End If
        End If
        FAPI_Connection.ObjectsToUpdate.Add(Me)
        FAPI_Connection.RequestFAPIData("PlayerName")
        FAPI_Connection.SendFAPIDataRequest()
        Timer2.Start()
        Button16.Select() ' Join Everyone Questing Online Again Forever button
    End Sub
    Public Sub UpdateFAPIDisplay()
        For Each item In FAPIItems
            Select Case item
                Case "PlayerName"
                    If TextBox2.Focused = False Then
                        TextBox2.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item)
                    End If
            End Select
        Next
    End Sub
    Private Sub AddFAPIItem(ByVal itemtoadd As String)
        If FAPIItems.Contains(itemtoadd) Then Exit Sub
        Dim i As Integer = FAPIItems.Length
        ReDim Preserve FAPIItems(i)
        FAPIItems(i) = itemtoadd
    End Sub
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        If FormLoader.AutoRevive = True Then
            FormLoader.AutoRevive = False
        End If
        Kairen.LaunchGameFullExperience()
    End Sub
    Private Sub UserMenu_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        If FormLoader.AutoRevive = True Then
            Button4.PerformClick()
        End If
    End Sub
    Sub ToggleMouseCursor(sender As System.Object, e As System.EventArgs) Handles Button8.MouseEnter, Button8.MouseLeave, Button1.MouseEnter, Button1.MouseLeave, Button9.MouseEnter, Button9.MouseLeave
        If Me.Cursor = Cursors.Default Then
            Me.Cursor = Cursors.Help
        ElseIf Me.Cursor = Cursors.Help Then
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub UserMenu_MouseDoubleClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDoubleClick
        If Me.Text.Contains(".-") Then
            Me.Text = Me.Text.Replace(".-", ".")
        Else
            Me.Text = "Kairen " & FormLoader.Version_Current_Release & " - User Menu"
        End If
    End Sub
    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        Kairen.ProgramOptionsFile = New TextFileClass(lb.IO_Options & "ProgramOptionsFile" & lb.Extension_OptionsFile, "--", False)
        'Kairen.ProgramOptionsFile.LoadFile()
        Kairen.LoadOptionFile("ProgramOptionsFile")
        If Button8.ForeColor <> Color.Green Then
            If Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data") = "-1" Then
                'red
                Button8.ForeColor = Color.Red
                'Button13.ForeColor = Color.Orange
            Else
                'green
                Button8.ForeColor = Color.Green
                'Button13.ForeColor = Color.Red
            End If
        End If

        If Button1.ForeColor <> Color.Green Then
            If Kairen.ProgramOptionsFile.GetValueByAdditionalData("[ISO Path] Data") = "-1" Then
                'red
                Button1.ForeColor = Color.Red
            Else
                'green
                Button1.ForeColor = Color.Green
            End If
        End If

        If Button13.ForeColor <> Color.Green Then
            Try

                If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", Nothing) IsNot Nothing Then
                    Dim reg_value_cheatengine_AutoLaunch As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", Nothing).ToString
                    Dim EmulatorFileName As String = lb.Right(Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data"), Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data").Length() - Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data").LastIndexOf("\") - 1)
                    If reg_value_cheatengine_AutoLaunch.Contains(EmulatorFileName) = False Then
                        'red
                        Button13.ForeColor = Color.Red
                    Else
                        'green
                        Button13.ForeColor = Color.Green
                    End If
                Else
                    'red
                    Button13.ForeColor = Color.Red
                End If

            Catch ex As Exception
            End Try
        End If

        If Button6.ForeColor <> Color.Green Then
            Dim reg_value_cheatengine As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon", "", Nothing).ToString
            'MsgBox(reg_value_cheatengine)
            Dim cheatengine_rootfolder As String
            cheatengine_rootfolder = lb.Left(reg_value_cheatengine, reg_value_cheatengine.LastIndexOf("Cheat Engine"))
            If lb.FE(cheatengine_rootfolder & "autorun\Ihtol.lua") = False Then
                If lb.FE(cheatengine_rootfolder & "Ihtol.lua") = False Then
                    'red
                    Button6.ForeColor = Color.Red
                Else
                    'green
                    Button6.ForeColor = Color.Green
                End If
            Else
                'green
                Button6.ForeColor = Color.Green
            End If
        End If
        If Button8.ForeColor = Color.Green And Button1.ForeColor = Color.Green And Button13.ForeColor = Color.Green And Button6.ForeColor = Color.Green Then
            If Timer2ElapsedTime < 1000 Then
                If FormLoader.AutoRevive = True Then
                    Button4.PerformClick()
                End If
            End If

            Button4.Enabled = True
            Button4.ForeColor = Color.Green
            Button14.Enabled = False
            If Timer2ElapsedTime > 600000 Then
                Timer2.Stop()
            End If
        Else
            Button4.Enabled = False
            Button14.ForeColor = Color.Red
        End If
        Timer2ElapsedTime = Timer2ElapsedTime + Timer2.Interval
    End Sub

    'Start Page 'tab
    Private Sub ScrollChangelogs(sender As System.Object, e As System.EventArgs) Handles Button2.Click, Button3.Click
        If Label1.Text = "Update News - Game Content" Or Label1.Text = "Changelog_KairenContent" Then
            Label1.Text = "Update News - Kairen Program"
            Kairen.DisplayLastXChangelogEntries("Changelog_KairenContent", TextBox1, 10, 10)
        ElseIf Label1.Text = "Update News - Kairen Program" Or Label1.Text = "Changelog_GameContent" Then
            Label1.Text = "Update News - Game Content"
            Kairen.DisplayLastXChangelogEntries("Changelog_GameContent", TextBox1, 10, 10)
        End If
    End Sub
    Private Sub Label1_SizeChanged(sender As System.Object, e As System.EventArgs) Handles Label1.SizeChanged
        If sender.Visible = False Then Exit Sub
        Dim NewLocation As New System.Drawing.Point(sender.Location.X + sender.Size.Width + 2, Button3.Location.Y)
        Button3.Location = NewLocation
    End Sub

    'Character & Save Sate Customization 'tab
    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If TextBox2.Text = "" Then Exit Sub
        FAPI_Connection.IssueFAPICommand(4, "PlayerName", TextBox2.Text)
        'can put a confirmation prompt here
        Kairen.UpdateOption("ProgramOptionsFile", "Player Name", TextBox2.Text)
        MsgBox("Character name saved!" & vbNewLine & "It should update in the world.", MsgBoxStyle.OkOnly, "Name Change Saved")
    End Sub

    'Program Setup 'tab
    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        Kairen.LaunchCE()
    End Sub
    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        'HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'C:\Program Files (x86)\Cheat Engine 6.4\Cheat Engine.exe,0
        Dim reg_value_cheatengine As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon", "", Nothing).ToString
        'MsgBox(reg_value_cheatengine)
        Dim cheatengine_rootfolder As String
        Dim cheatengine_autorunfolder As String
        cheatengine_rootfolder = lb.Left(reg_value_cheatengine, reg_value_cheatengine.LastIndexOf("Cheat Engine"))
        cheatengine_autorunfolder = cheatengine_rootfolder & "autorun\"
        If lb.FE(cheatengine_autorunfolder & "Ihtol.lua") Then
            'plan a installation, in ce\autorun folder
            Dim continueResponse As MsgBoxResult
            continueResponse = MsgBox("The modification file is already present. Do you want to overwrite it?", MsgBoxStyle.YesNoCancel, "Modification File Already Present")
            If continueResponse = MsgBoxResult.Yes Then
                IO.File.WriteAllBytes(cheatengine_autorunfolder & "Ihtol.lua", My.Resources.Ihtol)
                If lb.FE(cheatengine_autorunfolder) Then
                    MsgBox("Process Complete." & vbNewLine & "Modification file installed.", MsgBoxStyle.Information, "Modification Process")
                    Exit Sub
                End If
            ElseIf continueResponse = MsgBoxResult.No Then
                MsgBox("Process complete. No changes made.", MsgBoxStyle.Information, "Modification Process")
                Exit Sub
            ElseIf continueResponse = MsgBoxResult.Cancel Then
                MsgBox("No changes made. Cancelling...", MsgBoxStyle.Information, "Cancelling Installation..")
                Exit Sub
            End If
        Else
            IO.File.WriteAllBytes(cheatengine_autorunfolder & "Ihtol.lua", My.Resources.Ihtol)
            If lb.FE(cheatengine_autorunfolder & "Ihtol.lua") Then
                MsgBox("Process Complete." & vbNewLine & "Modification file installed.", MsgBoxStyle.Information, "Modification Process")
                Exit Sub
            Else
                'process a did not work, let's try b, then make c and try that i guess lol
            End If
        End If
        'plan b installation, add directly into main.lua file
        If lb.FE(cheatengine_rootfolder & "Ihtol.lua") Then
            Dim continueResponse As MsgBoxResult
            continueResponse = MsgBox("The modification file is already present. Do you want to overwrite it?", MsgBoxStyle.YesNoCancel, "Modification File Already Present")
            If continueResponse = MsgBoxResult.Yes Then
                IO.File.WriteAllBytes(cheatengine_rootfolder & "Ihtol.lua", My.Resources.Ihtol)
            ElseIf continueResponse = MsgBoxResult.No Then
                '
            ElseIf continueResponse = MsgBoxResult.Cancel Then
                MsgBox("No changes made. Cancelling...", MsgBoxStyle.OkOnly, "Cancelling Installation..")
                Exit Sub
            End If
        End If

        Dim ces_mainlua As New TextFileClass(cheatengine_rootfolder & "\main.lua", Nothing, False)
        ces_mainlua.LoadFile()
        For Each _line In ces_mainlua.Line
            If lb.Left(_line, 17) = "--Runnindatshityo" Then
                MsgBox("Process Complete." & vbNewLine & "Modification reference already detected, no change made.", MsgBoxStyle.Information, "Modification Process")
                Exit Sub
            End If
        Next
        If lb.FE(cheatengine_rootfolder & "\main - This was your original copy.lua") = False Then
            My.Computer.FileSystem.CopyFile(cheatengine_rootfolder & "\main.lua", cheatengine_rootfolder & "\main - This was your original copy.lua", False)
        End If
        ces_mainlua.CurrentIndex = ces_mainlua.NumberOfLines
        ces_mainlua.WriteLine("--Runnindatshityo " & FormLoader.Version_Current_Release)
        ces_mainlua.WriteLine("local f=io.open(" & Chr(34) & "Ihtol.lua" & Chr(34) & "," & Chr(34) & "r" & Chr(34) & ")")
        ces_mainlua.WriteLine("if f~=nil then")
        ces_mainlua.WriteLine("io.close(f)")
        ces_mainlua.WriteLine("dofile(" & Chr(34) & "Ihtol.lua" & Chr(34) & ")")
        ces_mainlua.WriteLine("else")
        ces_mainlua.WriteLine("print(" & Chr(34) & "You may not find your way today after all.." & Chr(34) & ")")
        ces_mainlua.WriteLine("end")
        ces_mainlua.SaveFile()
        'Shell(http_path & " " & action_info)
        'add error check here '"C:\Program Files (x86)\Mozilla Firefox\firefox.exe" -osint -url "%1" 'is my machine's entry
        MsgBox("Modification installed." & vbNewLine & "Modification reference wrote.", MsgBoxStyle.Information, "Modification Process")
    End Sub
    Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles Button13.Click
        'if pcsx2 isn't present then exit
        Kairen.ProgramOptionsFile = New TextFileClass(lb.IO_Options & "ProgramOptionsFile" & lb.Extension_OptionsFile, "--", False)
        Kairen.LoadOptionFile("ProgramOptionsFile")
        'MsgBox(Kairen.ProgramOptionsFile.IsLoaded)
        Dim epd As String = Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data")
        If lb.FE(epd) = False Then
            MsgBox("Warning! Your Emulator seems to be missing, cancelling Auto-Attach Setup.", MsgBoxStyle.Exclamation, "Auto-Attach Setup Error")
            Exit Sub
        End If
        'HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'HKEY_CURRENT_USER\SOFTWARE\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'C:\Program Files (x86)\Cheat Engine 6.4\Cheat Engine.exe,0
        If My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", Nothing) IsNot Nothing Then
            Dim reg_value_cheatengine_AutoLaunch As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", Nothing).ToString
            Dim EmulatorFileName As String = lb.Right(Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data"), Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data").Length() - Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data").LastIndexOf("\") - 1)
            ' MsgBox(EmulatorFileName)
            ' MsgBox(reg_value_cheatengine_AutoLaunch.Contains(EmulatorFileName))
            If reg_value_cheatengine_AutoLaunch.Contains(EmulatorFileName) = False Then
                'MsgBox("installing auto attach")
                Dim lastindexof As String = reg_value_cheatengine_AutoLaunch.LastIndexOf(";")
                Dim length As String = (reg_value_cheatengine_AutoLaunch.Length - 1)
                If length = 0 Or lastindexof <> length Then
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", reg_value_cheatengine_AutoLaunch & ";" & EmulatorFileName, Microsoft.Win32.RegistryValueKind.String)
                Else
                    My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", reg_value_cheatengine_AutoLaunch & EmulatorFileName, Microsoft.Win32.RegistryValueKind.String)
                End If
                MsgBox("Auto-Attach setup complete.", MsgBoxStyle.Information, "CE Auto-Attach Setup")
            Else
                MsgBox("Auto-Attach was already setup.", MsgBoxStyle.Information, "CE Auto-Attach Setup")
            End If
        Else
            'ce's autoattach registry key is missing
            Dim EmulatorFileName As String = lb.Right(Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data"), Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data").Length() - Kairen.ProgramOptionsFile.GetValueByAdditionalData("[Emulator Path] Data").LastIndexOf("\") - 1)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\SOFTWARE\Cheat Engine", "AutoAttach", EmulatorFileName, Microsoft.Win32.RegistryValueKind.String)
            MsgBox("Auto-Attach setup complete." & vbNewLine & "The Registry Key for Cheat Engine was created from scratch, so it was missing.", MsgBoxStyle.Information, "CE Auto-Attach Setup")
        End If
    End Sub
    Private Sub Button8_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Button8.MouseDown
        If e.Button = MouseButtons.Left Then
            TextBox3.Text = TextBox3.Text.Replace(Chr(34), "")
            If lb.FE(TextBox3.Text) Then
                Kairen.UpdateOption("ProgramOptionsFile", "Emulator Path", TextBox3.Text)
                'Kairen.ProgramOptionsFile.SaveFile()
                MsgBox("Emulator path updated!", MsgBoxStyle.OkOnly, "Emulator Path Update")
            Else
                MsgBox("No file was found at that path. Are you sure it's correct?" & vbNewLine & TextBox3.Text, MsgBoxStyle.Exclamation, "Error: Emulator Not Found")
            End If
        ElseIf e.Button = MouseButtons.Right Then
            MsgBox("Place the Full file path of your emulator here and then click save." & vbNewLine & "This should start with a letter like " & Chr(34) & "C:\" & Chr(34) & " and end with " & Chr(34) & ".exe" & Chr(34) & ".", MsgBoxStyle.Information, "Emulator Location Request")
        End If
    End Sub
    Private Sub Button1_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Button1.MouseDown
        If e.Button = MouseButtons.Left Then
            TextBox4.Text = TextBox4.Text.Replace(Chr(34), "")
            If lb.FE(TextBox4.Text) Then
                Kairen.UpdateOption("ProgramOptionsFile", "ISO Path", TextBox4.Text)
                MsgBox("Disc Image path updated!", MsgBoxStyle.OkOnly, "Disc Image Path Update")
            Else
                MsgBox("No file was found at that path. Are you sure it's correct?" & vbNewLine & TextBox4.Text, MsgBoxStyle.Exclamation, "Error: Disc Image Not Found")
            End If
        ElseIf e.Button = MouseButtons.Right Then
            MsgBox("Place the Full file path of your game disc here and then click save." & vbNewLine & "This should start with a letter like " & Chr(34) & "C:\" & Chr(34) & " and *usually* ends with " & Chr(34) & ".iso" & Chr(34) & ".", MsgBoxStyle.Information, "Disc Image Location Request")
        End If
    End Sub
    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        OpenFileDialog1.Filter = "Programs|*.exe|Any Type|*.*"
        OpenFileDialog1.DefaultExt = ".exe"
        OpenFileDialog1.Title = "Select your Playstation 2 Emulator"
        If TextBox3.Text <> "" Then
            OpenFileDialog1.InitialDirectory = lb.Left(TextBox3.Text, TextBox3.Text.LastIndexOf("\"))
            OpenFileDialog1.FileName = lb.Right(TextBox3.Text, TextBox3.Text.Length - TextBox3.Text.LastIndexOf("\") - 1)
        Else
            OpenFileDialog1.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            OpenFileDialog1.FileName = ""
        End If
        If OpenFileDialog1.ShowDialog() = 1 Then
            TextBox3.Text = OpenFileDialog1.FileName
        Else
            'they didn't select a correcto
        End If
    End Sub
    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        OpenFileDialog1.Filter = "ISO|*.iso|Any Type|*.*"
        OpenFileDialog1.DefaultExt = ".iso"
        OpenFileDialog1.Title = "Select your EQOA Disc Image"
        If TextBox4.Text <> "" Then
            OpenFileDialog1.InitialDirectory = lb.Left(TextBox4.Text, TextBox4.Text.LastIndexOf("\"))
            OpenFileDialog1.FileName = lb.Right(TextBox4.Text, TextBox4.Text.Length - TextBox4.Text.LastIndexOf("\") - 1)
        Else
            OpenFileDialog1.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
            OpenFileDialog1.FileName = ""
        End If
        If OpenFileDialog1.ShowDialog() = 1 Then
            TextBox4.Text = OpenFileDialog1.FileName
        Else
            'they didn't select a correcto
        End If
    End Sub
    Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles Button12.Click
        Kairen.LaunchEmulatorAndDiscImage()
    End Sub
    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles Button14.Click
        If Button10.ForeColor <> Color.Green Then
            Button10_Click(sender, e)
            Button8_MouseDown(sender, e)
        End If

        If Button11.ForeColor <> Color.Green Then
            Button11_Click(sender, e)
            Button1_MouseDown(sender, e)
        End If

        If Button13.ForeColor <> Color.Green Then
            ' If Button13.ForeColor <> Color.Black Then
            Button13_Click(sender, e)
            'End If
        End If

        If Button6.ForeColor <> Color.Green Then
            Button6_Click(sender, e)
        End If

        Button9_MouseDown(sender, e)
    End Sub
    Private Sub Button15_Click(sender As System.Object, e As System.EventArgs) Handles Button15.Click
        Kairen.RestartToLauncher(Me)
    End Sub

    'Content 'tab
    Private Sub Button9_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles Button9.MouseDown
        Dim ImportsFolderName As String = "Imports"
        If e.Button = MouseButtons.Left Then
            Dim CustomDataFlag As Boolean = False
            Dim Changelog_GameContentFlag As Boolean = False
            Dim selectedFolderName As String
            FolderBrowserDialog1.ShowDialog()
            selectedFolderName = FolderBrowserDialog1.SelectedPath
            selectedFolderName = lb.Right(selectedFolderName, selectedFolderName.Length - selectedFolderName.LastIndexOf("\") - 1)
            If selectedFolderName <> ImportsFolderName Then
                'error exit
                MsgBox("Please select the " & Chr(34) & ImportsFolderName & Chr(34) & " folder.", MsgBoxStyle.Exclamation, "Error: Incorrect folder selected")
            Else
                If lb.DE(FolderBrowserDialog1.SelectedPath) Then
                    Me.TopMost = True
                    Label5.Visible = False
                    Me.Enabled = False
                    MsgBox("Import folder located successfully. Press Ok to begin importing. The process may take a moment, so please wait. The screen may appear to freeze.", MsgBoxStyle.Information, "Importing Content FIles..")
                    If lb.DE(FolderBrowserDialog1.SelectedPath & "\Custom Data") Then
                        My.Computer.FileSystem.CopyDirectory(FolderBrowserDialog1.SelectedPath & "\Custom Data", lb.Folder_Custom_Data, True)
                        CustomDataFlag = True
                    End If
                    If lb.FE(FolderBrowserDialog1.SelectedPath & "\Changelog_GameContent" & lb.Extension_Changelog) Then
                        My.Computer.FileSystem.CopyFile(FolderBrowserDialog1.SelectedPath & "\Changelog_GameContent" & lb.Extension_Changelog, lb.IO_Options & "Changelog_GameContent" & lb.Extension_Changelog, True)
                        Label1.Text = "Changelog_GameContent"
                        ScrollChangelogs(sender, e)
                        Changelog_GameContentFlag = True
                        Kairen.LoadChangelogFile("Changelog_GameContent", "--")
                        Kairen.DisplayLastXChangelogEntries("Changelog_GameContent", TextBox1, 10, 10)
                    End If
                    'My.Computer.FileSystem.DeleteDirectory(lb.Folder_Temp & "Imports", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    Me.Enabled = True
                    Label5.Visible = True
                    MsgBox("Import process complete." & vbNewLine & _
                          "Imported Custom Data: " & CustomDataFlag & vbNewLine & _
                              "Imported Game Content Changelog: " & Changelog_GameContentFlag, MsgBoxStyle.Information, "Import Complete")
                    Me.TopMost = False
                End If
            End If
        ElseIf e.Button = MouseButtons.Right Then
            MsgBox("Locate and select the " & Chr(34) & ImportsFolderName & Chr(34) & " folder downloaded from the " & vbNewLine & Chr(34) & "Kaien: Return From The Abyss" & Chr(34) & " revival group on Facebook.", MsgBoxStyle.Information, "Select The " & Chr(34) & "Imports" & Chr(34) & " folder you downloaded.")
        End If
    End Sub
    Private Sub HiddenLabel_MouseEnter(sender As System.Object, e As System.EventArgs) Handles Label5.MouseEnter, Label6.MouseEnter
        sender.Forecolor = Color.Black
    End Sub
    Private Sub HiddenLabel_MouseLeave(sender As System.Object, e As System.EventArgs) Handles Label5.MouseLeave, Label6.MouseLeave
        sender.Forecolor = Color.White
    End Sub

    'Extras 'tab
    Dim URL As String = "eqoa.ddns.net"
    Dim Port As Integer = "51649"
    Private ReadOnly Property FullAddress()
        Get
            Return URL & ":" & Port
        End Get
    End Property
    Private Sub Button16_Click(sender As System.Object, e As System.EventArgs)
    End Sub 'RGC button
    Private Sub TextBox5_TextChanged(sender As System.Object, e As System.EventArgs)
        'Label9.Text = sender.Text.Length
    End Sub 'updates 0/127 label
    Private Sub CheckBox4_Click(sender As System.Object, e As System.EventArgs)
    End Sub 'Send Message 'button
    Private Sub CheckBox2_Click(sender As System.Object, e As System.EventArgs)

    End Sub 'Connect 'button
    Private Sub CheckBox3_Click(sender As System.Object, e As System.EventArgs)

    End Sub 'Diconnect 'button
    Private Sub TextBox5_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
        End If
    End Sub 'Next Message Box '[Enter] Pressed

    'Last Comment -- Code Below Not Moved Yet

    Private Sub Button16_Click_1(sender As System.Object, e As System.EventArgs) Handles Button16.Click
        'FormLoader.CheckedListBox1.SelectedItem = "ChatTest_v1"
        'FormLoader.CheckedListBox1.SelectedItem = "ChatTest_v2"
        FormLoader.CheckedListBox1.SelectedItem = "KairesTestUI"
        FormLoader.CheckedListBox1.SetItemCheckState(FormLoader.CheckedListBox1.SelectedIndex, CheckState.Checked)
    End Sub

End Class