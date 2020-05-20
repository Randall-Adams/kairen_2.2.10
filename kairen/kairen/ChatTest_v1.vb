Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class ChatTest_v1
    Public CloseFormLoaderToo As Boolean = True
    Dim DefaultSendToIP As String = "eqoa.ddns.net"
    'Dim DefaultSendToIP As String = "192.168.0.150"
    'Dim DefaultSendToIP As String = "192.168.0.9"
    Dim DefaultPorts As Integer = "51649"
    Dim Newline = vbNewLine
    Dim ChatThrottleTimer As Integer = 0
    Dim ChatThrottleTimerIndicator As Integer = 0
    Dim GameUpdateThrottleTimer As Integer = 0
    Dim Kaires As New Kaires_v1(Me, AddressOf DataReceived)
    Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection

    'CS - CODE
    Dim LastClientIsListeningStatus As Boolean = Kaires.ClientIsListening
    Public ReadOnly Property MyIPv4 As IPAddress
        Get
            Return System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName).AddressList(0)
        End Get
    End Property
    Public ReadOnly Property MyIPv6 As IPAddress
        Get
            Return System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName).AddressList(0)
        End Get
    End Property
    Private Function SendChatMessage(ByRef _chatMessage As String, Optional _clearData As Boolean = False, Optional _sendAsServerBoolean As Boolean = False)
        If ChatThrottleTimer > -1 Then Return -1 ' this should be done by tcp class later
        Button3.Enabled = False ' Send Message Button Server
        If ChatThrottleTimerIndicator = 0 Then
            ChatThrottleTimerIndicator = 1
        ElseIf ChatThrottleTimerIndicator = 2 Then
            ChatThrottleTimerIndicator = 3
        End If
        ChatThrottleTimer = 0
        Dim lasticon = NotifyIcon1.Icon
        NotifyIcon1.Icon = My.Resources.Yellow_Light_Icon_png
        Kaires.SendChatMessage(_chatMessage, _sendAsServerBoolean)
        NotifyIcon1.Icon = lasticon
        TextBox6.AppendText(">> You try to say:  " & _chatMessage & Newline)
        If _clearData Then _chatMessage = ""
        Return 0
    End Function ' Send Chat Message
    Private Function SendGameUpdate(ByRef _locData() As String)
        If GameUpdateThrottleTimer > -1 Then Return -1 ' this should be done by tcp class later - ?? idk copy paste from sendchatmessage
        GameUpdateThrottleTimer = 0
        Dim lasticon = NotifyIcon1.Icon
        NotifyIcon1.Icon = My.Resources.Yellow_Light_Icon_png
        Kaires.SendGameUpdate(_locData)
        NotifyIcon1.Icon = lasticon
        'TextBox6.AppendText(">> You try to say:  " & _chatMessage & Newline)
        Return 0
    End Function ' Send Game Update

    'CS - UI
    Private Sub ChatTest_v1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        TextBox8.Text = "Randall Adams Super-Secret Password"
        TextBox7.Text = "Randall Adams"
        GroupBox4.Enabled = False
        'Button1.Visible = False
        'Button2.Visible = False
        Me.Location = New Point(10, 10)
        FAPI_Connection.ObjectsToUpdate.Add(Me)
        NotifyIcon1.Icon = My.Resources.White_Light_Icon_png
        Label10.Text = "/" & TextBox5.MaxLength ' Your Next Message ' Max Length
        GroupBox1.Enabled = False ' Netboard - Main 'Client Box
    End Sub
    Private Sub ChatTest_v1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
        'FAPI_Connection.ObjectsToUpdate.Remove(Me)
        Application.Exit()
    End Sub
    Private Sub timer_UI_Tick(sender As System.Object, e As System.EventArgs) Handles timer_UI.Tick
        If LastClientIsListeningStatus <> Kaires.ClientIsListening Then 'if listening change occured..
            LastClientIsListeningStatus = Kaires.ClientIsListening
            If LastClientIsListeningStatus Then
                Label12.Text = "Listening" 'status label
                NotifyIcon1.Icon = My.Resources.Green_Light_Icon_png
            Else
                Label12.Text = "Not Listening" 'status label
                NotifyIcon1.Icon = My.Resources.Red_Light_Icon_png
            End If
        End If
        If Kaires.ClientIsListening = False Then
            GroupBox1.Enabled = False ' Netboard - Main
            Button1.Enabled = True ' Logon Button ' button
            Button2.Enabled = False ' Logoff Button ' button
            TextBox7.Enabled = True ' Display Name ' textbox
            Label13.Enabled = True ' Display Name Label ' label
            TextBox8.Enabled = True ' Passcode ' textbox
            Label14.Enabled = True ' Passcode Label ' label
        Else
            GroupBox1.Enabled = True ' Netboard - Main
            Button1.Enabled = False ' Logon Button ' button
            Button2.Enabled = True ' Logoff Button ' button
            TextBox7.Enabled = False ' Display Name ' textbox
            Label13.Enabled = False ' Display Name Label ' label
            TextBox8.Enabled = False ' Passcode ' textbox
            Label14.Enabled = False ' Passcode Label ' label
        End If
        If ChatThrottleTimer > -1 Then
            If ChatThrottleTimer > 500 Then
                ChatThrottleTimer = -1
                Button3.Enabled = True ' Send Message Button
            Else
                Button3.Enabled = False ' Send Message Button
                ChatThrottleTimer += sender.Interval
            End If
        End If
        If GameUpdateThrottleTimer > -1 Then
            If GameUpdateThrottleTimer > 500 Then
                GameUpdateThrottleTimer = -1
                'Button3.Enabled = True ' Send Message Button
            Else
                'Button3.Enabled = False ' Send Message Button
                GameUpdateThrottleTimer += sender.Interval
            End If
        End If
    End Sub 'Status / UI Updater 'timer
    Private Sub TextBox5_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox5.TextChanged
        Label9.Text = sender.Text.Length
    End Sub
    Private Sub TextBox5_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles TextBox5.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendChatMessage(TextBox5.Text, True)
        End If
    End Sub 'send message 'enter pressed
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        SendChatMessage(TextBox5.Text, True)
    End Sub 'send message 'button pressed

    'CLIENT - CODE
    Public Sub DataReceived(ByVal _dataType As String, ByVal _dataObject As Object)
        Select Case _dataType
            Case "console message"
                If IsArray(_dataObject) Then
                    For i As Integer = _dataObject.Length To 1
                        i -= 1
                        TextBox6.AppendText(" >> " & _dataObject(i) & " << " & Newline)
                    Next
                Else
                    TextBox6.AppendText(" >> " & _dataObject & " << " & Newline)
                End If
            Case "chat message"
                'If _dataObject(0) = TextBox7.Text Then
                TextBox6.AppendText(_dataObject(0) & " says: " & _dataObject(1) & Newline)
                'If GroupBox2.Enabled Then
                '    TextBox1.AppendText("You say: " & _dataObject(1) & Newline)
                'End If
                'Else
                '    TextBox6.AppendText(_dataObject(0) & " says: " & _dataObject(1) & Newline) ' while server show as Server
                'TextBox6.AppendText("You say: " & _dataObject(1) & Newline) ' while server show as You
                'If GroupBox2.Enabled Then
                '    TextBox1.AppendText(_dataObject(0) & " says: " & _dataObject(1) & Newline)
                'End If
                'End If
            Case "chat alert"
                TextBox6.AppendText(_dataObject(1) & Newline) ' while server show as Server
                'If _dataObject(0) = TextBox7.Text Then
                'TextBox1.AppendText(_dataObject(1) & Newline) ' while server show as Server
                'Else
                'TextBox1.AppendText(_dataObject(1) & Newline) ' while server show as Server
                'End If
            Case "game update"
                If CheckBox1.Checked = False Then
                    TextBox6.AppendText("GU :: " & _dataObject(0) & " IA -> " & _dataObject(1) & ", " & _dataObject(2) & ", " & _dataObject(3) & ", " & _dataObject(4) & Newline)
                End If
                FAPI_Connection.IssueFAPICommand(5, "UpdatePlayerData", _dataObject)
            Case "game command"
                'TextBox6.AppendText("GU :: " & _dataObject(0) & " IA -> " & _dataObject(1) & ", " & _dataObject(2) & ", " & _dataObject(3) & ", " & _dataObject(4) & Newline)
                Select Case _dataObject(0)
                    Case "spawn npc"
                        FAPI_Connection.IssueFAPICommand(6, "SpawnNPC", _dataObject(1))
                    Case "despawn npc"
                        FAPI_Connection.IssueFAPICommand(7, "DespawnNPC", _dataObject(1))
                End Select
            Case "program command"
                Select Case _dataObject(0)
                    Case "allow connection request sending"
                        GroupBox4.Enabled = True
                        Button1.Visible = True
                        Button2.Visible = True
                    Case "disallow connection request sending"
                        Button1.Visible = False
                        Button2.Visible = False
                End Select
            Case "NPCMessage1"
                TextBox6.AppendText("NPC MSG1 :: " & _dataObject(0) & _dataObject(1) & Newline)
            Case Else
                TextBox6.AppendText(Me.Name & " has received an unknown _dataType in DataReceived()" & Newline)
        End Select
    End Sub 'Data Received

    'CLIENT - UI
    Private Sub Button1_Click_1(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If Kaires.ClientIsListening Then Kaires.StopListening()
        timer_UI.Start()
        GroupBox1.Enabled = True ' Netboard - Main
        Button1.Enabled = False ' Logon Button ' button
        Button2.Enabled = True ' Logoff Button ' button
        TextBox7.Enabled = False ' Display Name ' textbox
        Label13.Enabled = False ' Display Name Label ' label
        TextBox8.Enabled = False ' Passcode ' textbox
        Label14.Enabled = False ' Passcode Label ' label
        If Kaires.ConnectToServer(DefaultSendToIP, DefaultPorts, TextBox7.Text, TextBox8.Text, MyIPv4, DefaultPorts) <> 0 Then Button2.PerformClick()
    End Sub ' Connect Button
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        GroupBox1.Enabled = False ' Netboard - Main
        Button1.Enabled = True ' Logon Button ' button
        Button2.Enabled = False ' Logoff Button ' button
        TextBox7.Enabled = True ' Display Name ' textbox
        Label13.Enabled = True ' Display Name Label ' label
        TextBox8.Enabled = True ' Passcode ' textbox
        Label14.Enabled = True ' Passcode Label ' label
        Kaires.StopListening()
        timer_UI.Stop()
        timer_UI_Tick(sender, e)
    End Sub ' Disconnect Button

    'CLIENT - Gamestuffs
    Public Sub UpdateFAPIDisplay()
        If Kaires.ClientIsListening Then
            Dim locData(4) As String
            locData(0) = TextBox7.Text
            locData(1) = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyX")
            locData(2) = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyY")
            locData(3) = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyZ")
            locData(4) = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyF")
            SendGameUpdate(locData)
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim _dataObject(4) As String
        _dataObject(0) = TextBox1.Text
        _dataObject(1) = TextBox2.Text
        _dataObject(2) = TextBox3.Text
        _dataObject(3) = TextBox4.Text
        _dataObject(4) = TextBox9.Text
        TextBox6.AppendText("GU :: " & _dataObject(0) & " IA -> " & _dataObject(1) & ", " & _dataObject(2) & ", " & _dataObject(3) & ", " & _dataObject(4) & Newline)
        'FAPI_Connection.IssueFAPICommand(5, "UpdatePlayerData", _dataObject)
        SendGameUpdate(_dataObject)
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        TextBox2.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyX")
        TextBox3.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyY")
        TextBox4.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyZ")
        TextBox9.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyF")
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Label12.Text = "Waiting.."
        Kaires.StartHearing(MyIPv4, DefaultPorts, 500, AddressOf DataReceived)
    End Sub
End Class