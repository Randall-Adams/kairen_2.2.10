Public Class UserMenu_v2
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection
    Private Kairen As Kairen2 = FormLoader.Kairen
    Dim FAPIItems(-1) As String
    Private ElapsedTime As Integer
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        Kairen.LaunchCE()
    End Sub

    Private Sub UserMenu_v2_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.ActuallyClose() = CloseFormLoaderToo
        Application.Exit()
    End Sub
    Private Sub UserMenu_v2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 350, 250)
        Me.TopMost = True
        Me.TopMost = False
        Kairen.LoadChangelogFile("Changelog_GameContent", "--")
        Kairen.LoadChangelogFile("Changelog_KairenContent", "--")
        'Kairen.DisplayLastXChangelogEntries(Kairen.Changelog_KairenContent, TextBox1, 10, 10)
        Kairen.DisplayLastXChangelogEntries("Changelog_GameContent", TextBox1, 10, 10)
        'request corsten's name in fapi updates
        FAPI_Connection.ObjectsToUpdate.Add(Me)
        'FAPI_Connection.RequestFAPIData("TestVar")
        'FAPI_Connection.RequestFAPIData("PlayerName")
        FAPI_Connection.RequestFAPIData("CorstenPlayerName")
        FAPI_Connection.SendFAPIDataRequest()
        'AddFAPIItem("PlayerName")
        'FAPI_Connection.IssueFAPICommand(1, "StartMenuZone")
        'FAPI_Connection.IssueFAPICommand(1, "Guard_Jahn.X")
        AddFAPIItem("MyX")
        AddFAPIItem("MyY")
        AddFAPIItem("MyZ")
        AddFAPIItem("CorstenPlayerName")
        AddFAPIItem("PlayerName")
        'AddFAPIItem("StartMenuZone")
        'AddFAPIItem("Guard_Jahn.X")
        AddFAPIItem("Chat_2-1")
        AddFAPIItem("Chat_3-2")
        AddFAPIItem("Chat_4-3")
        AddFAPIItem("Chat_5-4")
        AddFAPIItem("Chat_6-5")
        AddFAPIItem("Chat_7-6")
        AddFAPIItem("Chat_8-7")
        AddFAPIItem("Chat_9-8")
        AddFAPIItem("Chat_10-9")
        AddFAPIItem("Chat_11-10")
        AddFAPIItem("Chat_12-11")
        AddFAPIItem("Chat_13-12")
        AddFAPIItem("Chat_14-13")
        AddFAPIItem("Chat_15-14")
        AddFAPIItem("Chat_16-15")
        AddFAPIItem("Chat_17-16")
        AddFAPIItem("Chat_18-17")
        AddFAPIItem("Chat_19-18")
        AddFAPIItem("Chat_20-19")
        AddFAPIItem("Chat_21-20")
        AddFAPIItem("Chat_22-21")
        AddFAPIItem("Chat_23-22")
        AddFAPIItem("Chat_24-23")
        AddFAPIItem("Chat_25-24")
        AddFAPIItem("Chat_26-25")
        AddFAPIItem("Chat_27-26")
        AddFAPIItem("Chat_28-27")
        AddFAPIItem("Chat_29-28")
        AddFAPIItem("Chat_30-29")
        AddFAPIItem("Chat_31-30")
        AddFAPIItem("Chat_0-31")
        AddFAPIItem("Chat_1-0")
        AddFAPIItem("SlotInd1")
        AddFAPIItem("SlotInd2")
        AddFAPIItem("OpenChatBox")
        AddFAPIItem("ChatIsOpen1")
        AddFAPIItem("ChatIsOpen2")
    End Sub
    Public Sub UpdateFAPIDisplay()
        ListBox1.Items.Clear()
        For Each item In FAPIItems
            Select Case item
                Case "MyX"
                    ListBox1.Items.Add("Corsten's X Value: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "MyY"
                    ListBox1.Items.Add("Corsten's Y Value: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "PlayerName"
                    ListBox1.Items.Add("Corsten's Name is: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "StartMenuZone"
                    ListBox1.Items.Add("ya zone reads as:: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "Guard_Jahn.X"
                    ListBox1.Items.Add("Guard_Jahn.X reads as:: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "SlotInd1"
                    ListBox1.Items.Add("Slot1: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "SlotInd2"
                    ListBox1.Items.Add("Slot2: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "OpenChatBox"
                    ListBox1.Items.Add("OpenChatBox: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "ChatIsOpen1"
                    ListBox1.Items.Add("ChatIsOpen1: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "ChatIsOpen2"
                    ListBox1.Items.Add("ChatIsOpen2: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                Case "CorstenPlayerName"
                    Dim data As String = FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item)
                    ListBox1.Items.Add("CPN: " & data)
                    If TextBox5.Focused = False Then
                        TextBox5.Text = data
                    End If
                Case "PlayerName"
                    Dim data As String = FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item)
                    ListBox1.Items.Add("PN: " & data)
                Case Else
                    If lb.Left(item, 5) = "Chat_" Then
                        ListBox1.Items.Add(item & ":: " & FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item))
                    Else
                        Dim tempitem = FAPI_Connection.o_FAPIData.GetValueByAdditionalData(item)
                        If tempitem = Nothing Then Exit Select
                        ListBox1.Items.Add(tempitem)
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

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        FAPI_Connection.IssueFAPICommand(4, TextBox3.Text, TextBox4.Text)
        FAPI_Connection.IssueFAPICommand(1, TextBox3.Text)
        AddFAPIItem(TextBox3.Text)
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Button1_Click(sender, e)
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If TextBox5.Text = "" Then Exit Sub
        FAPI_Connection.IssueFAPICommand(4, "CorstenPlayerName", TextBox5.Text)
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Process.Start("explorer", lb.Folder_Temp)
        ElapsedTime = 0
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        ElapsedTime += Timer1.Interval
        If ElapsedTime >= 120000 Then '(120000 / Timer1.Interval) Then
            Timer1.Stop()
            MsgBox("2mins passed breh: " & ElapsedTime)
            Exit Sub
        End If
        'Dim cdc As Boolean = False
        'Dim ccf As Boolean = False
        If lb.DE(lb.Folder_Temp & "Imports") Then
            Timer1.Stop()
            Me.TopMost = True
            If lb.DE(lb.Folder_Temp & "Imports\Custom Data") Then
                My.Computer.FileSystem.CopyDirectory(lb.Folder_Temp & "Imports\Custom Data", lb.Folder_Custom_Data, True)
                'cdc = True
            End If
            If lb.FE(lb.Folder_Temp & "Imports\Changelog_GameContent" & lb.Extension_Changelog) Then
                My.Computer.FileSystem.CopyFile(lb.Folder_Temp & "Imports\Changelog_GameContent.txt", lb.IO_Options & "Changelog_GameContent" & lb.Extension_Changelog, True)
                Label1.Text = "Update News"
                'ccf = True
            End If
            'MsgBox("imports takiin care of breh", MsgBoxStyle.Critical)
            'MsgBox("cdc = " & cdc)
            'MsgBox("ccf = " & ccf)
            Kairen.LoadChangelogFile("Changelog_GameContent", "--")
            Kairen.DisplayLastXChangelogEntries("Changelog_GameContent", TextBox1, 10, 10)
            My.Computer.FileSystem.DeleteDirectory(lb.Folder_Temp & "Imports", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            Me.TopMost = False
        End If
    End Sub

    Private Sub Label1_SizeChanged(sender As System.Object, e As System.EventArgs) Handles Label1.SizeChanged
        If sender.Visible = False Then Exit Sub
        Dim NewLocation As New System.Drawing.Point(sender.Location.X + sender.Size.Width + 2, Button8.Location.Y)
        Button8.Location = NewLocation
    End Sub

    Private Sub ScrollChangelogs(sender As System.Object, e As System.EventArgs) Handles Button7.Click, Button8.Click
        If Label1.Text = "Update News" Then
            Label1.Text = "Downdate News"
            Kairen.DisplayLastXChangelogEntries("Changelog_KairenContent", TextBox1, 10, 10)
        ElseIf Label1.Text = "Downdate News" Then
            Label1.Text = "Update News"
            Kairen.DisplayLastXChangelogEntries("Changelog_GameContent", TextBox1, 10, 10)
        End If
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        'HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon ' has my machines cheat engine location
        'C:\Program Files (x86)\Cheat Engine 6.4\Cheat Engine.exe,0
        Dim reg_value_cheatengine As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CheatEngine\DefaultIcon", "", Nothing).ToString
        'MsgBox(reg_value_cheatengine)
        Dim cheatengine_rootfolder As String
        cheatengine_rootfolder = lb.Left(reg_value_cheatengine, reg_value_cheatengine.LastIndexOf("Cheat Engine"))
        Dim ces_mainlua As New TextFileClass(cheatengine_rootfolder & "\main.lua", Nothing, False)
        ces_mainlua.LoadFile()
        For Each _line In ces_mainlua.Line
            If lb.Left(_line, 17) = "--Runnindatshityo" Then
                MsgBox("already present")
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
        IO.File.WriteAllBytes(cheatengine_rootfolder & "Ihtol.lua", My.Resources.Ihtol)
        'Shell(http_path & " " & action_info)
        'add error check here '"C:\Program Files (x86)\Mozilla Firefox\firefox.exe" -osint -url "%1" 'is my machine's entry
        MsgBox("finished.")
    End Sub
End Class