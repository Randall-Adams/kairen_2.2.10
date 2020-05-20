Imports System.Net.Sockets
Imports System.Text
Imports System.Reflection
'Kairen name of client prog
'Kaires name of Server Prog
Public Class OnlineForm
    Public CloseFormLoaderToo As Boolean = True
    Dim lb As New CommonLibrary("EQOA", "")
    Private Client() As TCPManager
    Private CurrentClient As Integer

    Dim TurnedOn As Boolean = False
    Dim NewLine = vbNewLine

    Dim Data As Integer
    Dim Message As String
    Private cServer As TcpListener
    Private cClient As New TcpClient
    Public OnlineName As String = "Bob"
    Private Delegate Sub MessageDelegate(ByVal Message As String)

    Private IP As String
    Private Port As Integer = 30000

    Dim BufferSize(1024) As Byte

    Private Sub OnlineForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 208, 132)
        CheckBox1.Checked = True ' Pressing [Enter] Sends Chat Message
    End Sub

    'Turn On
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TextBox1.Text = OnlineName
        Button3.Enabled = True
        Button4.Enabled = True

        TurnedOn = True
        Label2.Text = "Turned On"
        Button1.Enabled = False
    End Sub

    Private Sub MessageBox_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MessageBox.KeyDown
        If Label5.Text = "Offline" Then Exit Sub
        If CheckBox1.Checked = True Then
            If e.KeyCode = Keys.Enter Then
                SendChatMessage()
            End If
        End If
    End Sub
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        SendChatMessage()
    End Sub

    Private Sub ChatBox_AddLine(ByVal _message As String)
        ChatBox.AppendText(_message & NewLine)
    End Sub
    Private Sub SendChatMessage(Optional ByVal _chatMessage As String = Nothing)
        If TurnedOn = False Then Exit Sub
        If _chatMessage Is Nothing Then
            _chatMessage = MessageBox.Text
        End If

        AttemptToSendMessage(OnlineName.Length & ":" & OnlineName & ":" & "11:ChatMessage:" & _chatMessage.Length & ":" & _chatMessage)
        'SendData(OnlineName.Length & ":" & OnlineName & ":" & "11:ChatMessage:" & _chatMessage.Length & ":" & _chatMessage)
        'ReceiveChatMessage(OnlineName & " Says: " & _chatMessage)
        '#:OnlineName:#:command:
        '3:Bob:11:ChatMessage:#:message
        MessageBox.Clear()
    End Sub
    Private Sub ReceiveChatMessage(ByVal Message As String)
        ChatBox_AddLine(Message)
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If d_tb_IP.Text = "" Then
            d_tb_IP.Text = "192.168.0.8"
        End If
        If d_tb_Port.Text = "" Then
            d_tb_Port.Text = "30000"
        End If
        ConnectToServer(d_tb_IP.Text)
        'Server_Connect("127.0.0.1", 30000)
        Label5.Text = "Online"
        Button2.Enabled = True
    End Sub

#Region "Network Code"
    Private Sub Server_Connect(_ip As String, _port As Integer)
        Dim i As Integer
        Dim shouldstop As Boolean = False
        Do Until shouldstop = True
            Try
                If Client(i).ID = 0 Then
                    'MsgBox("Client(" & i & ") = " & Client(i).ID)
                    ' Client(i) exists, try next client.
                End If
                i += 1
            Catch ex As Exception
                If ex.Message.ToString = _
                    "Object reference not set to an instance of an object." Or ex.Message.ToString = _
                    "Index was outside the bounds of the array." Then
                    ReDim Preserve Client(i)
                    Try
                        ChatBox_AddLine("Trying with Client(" & i & ")")
                        ChatBox_AddLine("IP:Port = " & _ip & ":" & _port)
                        Client(i) = New TCPManager(_ip, _port, i)
                        CurrentClient = Client(i).ID
                        ''UpdateServerStatus(sender, e, 2)
                        shouldstop = True
                        ChatBox_AddLine("Connected.")
                    Catch ex2 As Exception
                        If ex2.Message.ToString = "No connection could be made because the target machine actively refused it " & _ip & ":" & _port Then
                            ' Server is not hosting
                            ChatBox_AddLine("Error: Server not hosting.")
                            shouldstop = True
                        Else
                            MsgBox(ex2.Message.ToString & NewLine & NewLine & ex2.Data.ToString)
                            MsgBox("shouldstopping a little less prematurely.")
                            shouldstop = True
                        End If

                        ''UpdateServerStatus(sender, e, 0)
                    End Try
                Else
                    MsgBox(ex.Message.ToString & NewLine & NewLine & ex.Data.ToString)
                    MsgBox("shouldstopping prematurely.")
                    shouldstop = True
                End If
            End Try
        Loop

        ChatBox_AddLine("")
    End Sub

    Private Sub Server_Disconnect(sender, e)
        'UpdateServerStatus(sender, e, 3)
        Client(CurrentClient).EndConnection()
        'UpdateServerStatus(sender, e, 0)
    End Sub

    Private Sub SendData(Data As String)
        Client(CurrentClient).Send(Data)
    End Sub
#End Region

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim _filepath As String
        _filepath = Form1.IO_Root & "net streams/writes/MyLocation.txt"
        If My.Computer.FileSystem.FileExists(_filepath) Then
            Button4.Enabled = False
            Label7.Text = "Linked"
            Timer1.Start()
        Else
            ChatBox_AddLine("Console: Error - Lua file output not found.")
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim _filepath, _MyX, _MyY, _MyZ, _MyF As String
        _filepath = Form1.IO_Root & "net streams/writes/MyLocation.txt"
        If My.Computer.FileSystem.FileExists(_filepath) Then
            Dim sr = New IO.StreamReader(_filepath)
            _MyX = sr.ReadLine()
            _MyY = sr.ReadLine()
            _MyZ = sr.ReadLine()
            _MyF = sr.ReadLine()
            sr.Close()
            Dim sendString As String
            'sendString = OnlineName "[" & _MyX.Length & "]" & _MyX
            ':#:name:#:datatype:
            ':3:Bob:8:Location:
            ''
            ':#:name:#:datatype:
            ':3:Bob:8:Location:#:MYX:#:MYY:#:MYZ:#:MYF
        End If
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        MsgBox(Me.Location.X & "," & Me.Location.Y)
        If My.Computer.FileSystem.FileExists(Form1.IO_EQOA & "Temp/Outside Command.txt") = True Then
            Dim _result As MsgBoxResult
            _result = MsgBox("A command already exists, do you want to overwrite it? Click No if you want to give it more time to run." & NewLine _
                             & "  (Is the Savestate accepting Outside Commands?)", MsgBoxStyle.YesNo, "Warning: Previous Command Not Yet Executed")
            If _result = MsgBoxResult.Yes Then
                ChatBox_AddLine("Console: Overwriting previous Command. (Make sure Outside Commands are being processed!")
                'over write
            ElseIf _result = MsgBoxResult.No Then
                ChatBox_AddLine("Console: Allowing previous Command more time to be executed. (Make sure Outside Commands are being processed!")
            End If
        End If
    End Sub

#Region "Client Code"

    Private Sub d_btn_Connect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ConnectToServer(IP)
    End Sub 'Handles d_c_btn_Connect.Click

    Private Sub d_btn_Send_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        AttemptToSendMessage(MessageBox.Text)
    End Sub 'Handles d_c_btn_Send.Click
    ' -------------------------------- '

    Private Sub ConnectToServer(ByVal ipadress As String)
        Try
            cClient.BeginConnect(ipadress, Port, AddressOf OnClientConnect, Nothing)
            ''cClient.BeginConnect(ipadress, 5900, AddressOf OnClientConnect, Nothing)
        Catch ex As Exception
            If ex.Message = "No connection could be made because the target machine actively refused it " & IP & ":" & Port Then
                'ChatBoxOutput(d_s_tb_ChatBox, "Error: The server refused the connection to " & ipadress & ":" & d_c_tb_Port.Text)
                MsgBox("Error: The server refused the connection to " & IP & ":" & Port, MsgBoxStyle.OkOnly, "Error:")
            Else
                ErrorOutput(ex)
            End If
        End Try
    End Sub 'connect to server
    Private Sub OnClientConnect(ByVal AR As IAsyncResult)
        Try
            cClient.EndConnect(AR)
            cClient.GetStream.BeginRead(BufferSize, 0, BufferSize.Length, AddressOf OnClientRead, Nothing)
        Catch ex As Exception
            If ex.Message = "No connection could be made because the target machine actively refused it " & IP & ":" & Port Then
                'ChatBoxOutput(d_s_tb_ChatBox, "Error: The server refused the connection to " & ipadress & ":" & d_c_tb_Port.Text)
                MsgBox("Error: The server refused the connection to " & IP & ":" & Port, MsgBoxStyle.OkOnly, "Error:")
            Else
                ErrorOutput(ex)
            End If
        End Try
    End Sub 'if client connects
    Private Sub OnClientRead(ByVal AR As IAsyncResult)
        Data = cClient.GetStream.EndRead(AR)
        Message = Encoding.ASCII.GetString(BufferSize, 0, Data)
        Dim Args As Object() = {Message}
        Me.Invoke(New MessageDelegate(AddressOf PrintMessageClient), Args)

        cClient.GetStream.BeginRead(BufferSize, 0, BufferSize.Length, AddressOf OnClientRead, Nothing)
    End Sub 'Reads from Server?
    Private Sub PrintMessageClient(ByVal Message As String)
        Try
            ' Server Processes Client Input Here
            ChatBox_AddLine(Message)
            'SendCommand(Message)

            'My.Computer.Audio.Play(Application.StartupPath & "/Message.wav", AudioPlayMode.Background)
        Catch ex As Exception
            'MsgBox(ex.Message, MsgBoxStyle.Critical)
            ErrorOutput(ex)
        End Try
    End Sub

    Private Sub AttemptToSendMessage(ByVal _message As String)
        If Not String.IsNullOrEmpty(_message) Then

            'd_c_tb_ChatBox.Text = d_c_tb_ChatBox.Text & "Me:" & _message & vbCrLf
            SendMessage(_message)

        End If
    End Sub

    Private Sub SendMessage(ByVal message As String)
        If cClient.Connected = True Then
            Dim Writer As New IO.StreamWriter(cClient.GetStream)
            Writer.Write(message)
            Writer.Flush()
        Else
            ChatBox_AddLine("Error: Not Connected to a Server")
        End If

        MessageBox.Text = ""
    End Sub
#End Region

    Private Sub ErrorOutput(ByVal ex As Exception)
        InputBox("ex.Message:" & vbNewLine & ex.Message, "Error:", ex.Message)
    End Sub

    Private Sub OnlineForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If MsgBox("Exit the form?", MsgBoxStyle.YesNo, "Are you sure you want to exit?") <> MsgBoxResult.Yes Then
            e.Cancel = True
            Exit Sub
        End If
        'FormLoader.ActuallyClose = CloseFormLoaderToo
        'Application.Exit()
    End Sub
End Class