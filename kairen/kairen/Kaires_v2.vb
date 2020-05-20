Imports System.Net
Public Class Kaires_v2
    Dim Newline = vbNewLine
    Private Parentform As Form
    Private tcpSender As tcpSender_v1
    Private tcpReceiver As tcpReceiver_v1
    Delegate Sub OutputDelegate(ByVal _dataType As String, ByVal _dataObject As Object)
    'Private OutputDelegate_UI As OutputDelegate
    Private DataReceivedDelegate As OutputDelegate
    Private ServerTokenList(-1) As String
    Private ClientTokenList(-1) As String
    Private ClientUserList(-1) As String
    Private ClientIPAddressess(-1) As String
    Private MyClientToken As String
    Private TheServerToken As String
    Private ServerOperatorName As String = "The Server Operator"
    Private OperationMode As String = "Off"
    Private MysteriousPrompt As Boolean = True
    Private Usernames(-1) As String
    Private Passwords(-1) As String
    Sub New(ByRef _senderForm As Form, _dataReceivedDelegate As OutputDelegate)
        Parentform = _senderForm
        DataReceivedDelegate = _dataReceivedDelegate
    End Sub
    Private Function CheckPW(ByVal _userName As String, ByVal _password As String)

        Return False
    End Function
    Public Function StartHearing(ByVal _ip As IPAddress, ByVal _port As Integer, ByVal _interval As Integer, ByRef _dataReceivedDelegate As OutputDelegate)
        DataReceivedDelegate = _dataReceivedDelegate
        tcpReceiver = New tcpReceiver_v1(_ip, _port, _interval, AddressOf BreakdownPacket)
        Select Case tcpReceiver.StartListening
            Case 0
                'timer_Client.Start()
                OperationMode = "Waiting"
                Return 0
            Case -1
                DataReceivedDelegate("console message", "The selected IP is not assigned to this computer.")
                Return -1
            Case -2
                DataReceivedDelegate("console message", "General error trying to listen. (-2)")
                Return -2
            Case -3
                DataReceivedDelegate("console message", "The selected port is already being used. (-3)")
                Return -3
            Case Else
                DataReceivedDelegate("console message", "General error trying to listen. (-9000)")
        End Select
        Return -900
    End Function
    Private Function StartListening(ByVal _ip As IPAddress, ByVal _port As Integer, ByVal _interval As Integer, ByRef _dataReceivedDelegate As OutputDelegate)
        DataReceivedDelegate = _dataReceivedDelegate
        tcpReceiver = New tcpReceiver_v1(_ip, _port, _interval, AddressOf BreakdownPacket)
        Select Case tcpReceiver.StartListening
            Case 0
                'timer_Client.Start()
                Return 0
            Case -1
                DataReceivedDelegate("console message", "The selected IP is not assigned to this computer.")
                Return -1
            Case -2
                DataReceivedDelegate("console message", "General error trying to listen. (-2)")
                Return -2
            Case -3
                DataReceivedDelegate("console message", "The selected port is already being used. (-3)")
                Return -3
            Case Else
                DataReceivedDelegate("console message", "General error trying to listen. (-9000)")
        End Select
        Return -900
    End Function
    Public Function StartHosting(ByVal _ip As IPAddress, ByVal _port As Integer, ByVal _interval As Integer) ', ByRef _outputDelegateConsole As OutputDelegate, ByRef _outputDelegateChatWindow As OutputDelegate)

        Return -900
    End Function
    Public Sub StopListening()
        'DataReceivedDelegate("console message", "Closing Connection")
        If OperationMode = "Server" Then
            For i As Integer = 0 To ClientIPAddressess.Length - 1
                tcpSender.SendData("2:7:" & ServerTokenList(i), ClientIPAddressess(i), tcpReceiver.Port)
            Next
            DataReceivedDelegate("console message", "Hosting Ceased")
        ElseIf OperationMode = "Client" Then
            LogoffUserNotify()
            DataReceivedDelegate("console message", "Connection Closed")
        ElseIf OperationMode = "Waiting" Then
            DataReceivedDelegate("console message", "Waiting Stopped")
        Else
            DataReceivedDelegate("console message", "Unknown operation mode ended")
        End If
        tcpReceiver.StopListening()
        tcpReceiver = Nothing
        ReDim ClientTokenList(-1)
        ReDim ClientUserList(-1)
        ReDim ClientIPAddressess(-1)
        MyClientToken = ""
        OperationMode = "Off"
    End Sub

    Private Sub BreakdownPacket(ByVal _data() As String)
        Dim segmentLength As Integer = 1
        If Microsoft.VisualBasic.Right(Microsoft.VisualBasic.Left(_data(2), segmentLength + 1), 1) <> ":" Then
            If MysteriousPrompt Then
                DataReceivedDelegate("console message", "someone was being mysterious")
                DataReceivedDelegate("console message", _data)
            End If
        Else
            Dim segment(-1) As String
            Do Until _data(2).Length = 0
                ReDim Preserve segment(segment.Length)
                If _data(2).Contains(":") Then
                    segment(segment.Length - 1) = Microsoft.VisualBasic.Left(_data(2), _data(2).IndexOf(":"))
                    _data(2) = Microsoft.VisualBasic.Right(_data(2), _data(2).Length - _data(2).IndexOf(":") - 1)
                Else
                    segment(segment.Length - 1) = _data(2)
                    Exit Do
                End If
            Loop
            Dim I As Integer = -1
            Try
                Select Case segment(A(I))
                    Case "2" ' Connection
                        Select Case segment(A(I))
                            Case "0" ' "allow client to send connect to server request" request 'CLIENT
                                Dim DataObject(0) As String
                                If segment(A(I)) = "Okay, try now breh" Then
                                    DataObject(0) = "allow connection request sending"
                                ElseIf segment(I) = "Nvm brah, just quit" Then
                                    DataObject(0) = "disallow connection request sending"
                                End If
                                DataReceivedDelegate("program command", DataObject)

                            Case "2" ' "connect to server request" connection test response 'CLIENT
                                MyClientToken = segment(A(I))
                                TheServerToken = segment(A(I))
                                tcpSender.SendData("2:3:" & MyClientToken) 'can connect test response response 
                                'tcpSender.SendData("2:3:" & MyClientToken) 'can connect test response response ' cs rendition

                            Case "4" 'CLIENT
                                DataReceivedDelegate("console message", segment(A(I))) ' output that the connection was established


                            Case "7" 'CLIENT
                                If OperationMode = "Server" Then
                                    If segment(A(I)) = TheServerToken Then
                                        Dim DataObject(1) As String
                                        DataObject(0) = "Server Admin"
                                        DataObject(1) = ">> You have been logged off <<"
                                        DataReceivedDelegate("chat alert", DataObject)
                                    End If
                                Else
                                    If segment(A(I)) = TheServerToken Then
                                        segment = segment
                                        Dim DataObject(1) As String
                                        DataObject(0) = "Server Admin"
                                        DataObject(1) = ">> You have been logged off <<"
                                        DataReceivedDelegate("chat alert", DataObject)
                                        Me.StopListening()
                                    Else
                                        MsgBox("Got: " & segment(I) & Newline & "Wanted: " & TheServerToken)
                                        '                                        MsgBox()
                                    End If
                                End If
                        End Select
                    Case "5" ' Game
                        Select Case segment(A(I))
                            Case "1" ' XYZ Updates
                                Select Case segment(A(I))

                                    Case "2" ' XYZ Update 'CLIENT
                                        If segment(A(I)) = TheServerToken Then
                                            Dim locData(4) As String
                                            locData(0) = segment(A(I))
                                            locData(1) = segment(A(I))
                                            locData(2) = segment(A(I))
                                            locData(3) = segment(A(I))
                                            locData(4) = segment(A(I))
                                            DataReceivedDelegate("game update", locData)
                                        End If
                                End Select
                            Case "2" ' NPC Control Aspects
                                Select Case segment(A(I))
                                    Case "1" ' NPC Spawning
                                        Select Case segment(A(I))
                                            Case "1" ' Spawn  NPC
                                                A(I) ' server token
                                                A(I) ' username sent to
                                                Dim npcData(1) As String
                                                npcData(0) = "SpawnNPC"
                                                npcData(1) = segment(A(I)) ' NPC Target
                                                DataReceivedDelegate("game command", npcData)
                                            Case "2" ' Despawn NPC
                                                A(I) ' server token
                                                A(I) ' username sent to
                                                Dim npcData(1) As String
                                                npcData(0) = "DespawnNPC"
                                                npcData(1) = segment(A(I)) ' NPC Target
                                                DataReceivedDelegate("game command", npcData)
                                        End Select
                                    Case "2" ' NPC File Stuffs
                                        Select Case segment(A(I))
                                            Case "1" ' Create NPC File
                                                A(I) ' server token
                                                A(I) ' username sent to
                                                Dim npcData(1) As String
                                                npcData(0) = "CreateNPCFile"
                                                npcData(1) = segment(A(I)) ' NPC Target
                                                DataReceivedDelegate("game command", npcData)
                                            Case "2" ' Delete NPC File
                                                A(I) ' server token
                                                A(I) ' username sent to
                                                Dim npcData(1) As String
                                                npcData(0) = "DeleteNPCFile"
                                                npcData(1) = segment(A(I)) ' NPC Target
                                                DataReceivedDelegate("game command", npcData)
                                            Case "3" ' Move NPC File
                                            Case "4" ' Retrieve NPC File To Server
                                        End Select
                                    Case "3" ' NPC Chat Stuffs
                                        Select Case segment(A(I))
                                            Case "1" ' NPC Message Box: NPC Name: Message
                                                A(I) ' server token
                                                A(I) ' username sent to
                                                Dim npcData(1) As String
                                                npcData(0) = segment(A(I))
                                                npcData(1) = " says: " & segment(A(I))
                                                DataReceivedDelegate("NPCMessage1", npcData)
                                        End Select
                                End Select
                                ' Case "" Popup Box [X-message Only]
                                ' Case ""
                        End Select
                    Case "7" ' Social
                        Select Case segment(A(I))
                            Case "1" ' Chat Messages
                                Select Case segment(A(I))

                                    Case "2" ' Chat Message 'CLIENT
                                        If segment(A(I)) = TheServerToken Then
                                            Dim DataObject(1) As String
                                            DataObject(0) = segment(A(I))
                                            DataObject(1) = segment(A(I))
                                            DataReceivedDelegate("chat message", DataObject)
                                        End If
                                End Select
                        End Select
                        'Return "<< Someone" & " Says:  " & Microsoft.VisualBasic.Right(_data(2), _data(2).Length - 2) & Newline
                        'Return segment(1)
                    Case Else
                        'Return "<< Someone" & " was being malsterious." & Newline
                End Select
            Catch
            End Try
        End If
    End Sub 'Data Received

    Public Sub SendChatMessage(ByVal _message As String, Optional ByVal _sendAsServerOperator As Boolean = False)
        _message = _message.Replace(":", "")
        If _sendAsServerOperator = False Then
            If OperationMode = "Server" Then
                tcpSender.SendData("7:1:1:" & MyClientToken & ":" & _message, tcpReceiver.IP.ToString, tcpReceiver.Port) ')
            Else
                tcpSender.SendData("7:1:1:" & MyClientToken & ":" & _message)
            End If
        Else
            'sends to self instead of server.... in case you are the server.. or it errors out on the above line
            'tcpSender.SendData("7:1:" & ClientTokenList(0) & ":" & _message)
            tcpSender.SendData("7:1:1:" & ServerOperatorName & ":" & _message, tcpReceiver.IP.ToString, tcpReceiver.Port)
        End If
    End Sub ' sends message to server


    Public Sub SendGameUpdate(ByVal _locData() As String, Optional ByVal _sendAsServerOperator As Boolean = False)
        If _sendAsServerOperator = False Then
            tcpSender.SendData("5:1:1:" & MyClientToken & ":" & _locData(0) & ":" & _locData(1) & ":" & _locData(2) & ":" & _locData(3) & ":" & _locData(4)) ', tcpReceiver.IP.ToString, tcpReceiver.Port) ')
        Else
            'sends to self instead of server.... in case you are the server.. or it errors out on the above line
            'tcpSender.SendData("7:1:" & ClientTokenList(0) & ":" & _message)
            tcpSender.SendData("5:1:1:" & ServerOperatorName & ":" & _locData(0) & ":" & _locData(1) & ":" & _locData(2) & ":" & _locData(3) & ":" & _locData(4), tcpReceiver.IP.ToString, tcpReceiver.Port)
        End If
    End Sub ' sends game update to server


    Public Sub SendSpawnNPC(ByVal _userName As String, ByVal _npcName As String)
        Dim i As Integer = GetUserIDByUserName(_userName)
        'If OperationMode = "Server" Then
        tcpSender.SendData("5:2:1:1:" & ServerTokenList(i) & ":" & _userName & ":" & _npcName, ClientIPAddressess(i), tcpReceiver.Port) ' server command
        'Else
        '    tcpSender.SendData("5:2:1:1:" & MyClientToken & ":" & _userName & ":" & _npcName) ' client update
        'End If
    End Sub ' sends spawn command to single client


    Public Sub SendDespawnNPC(ByVal _userName As String, ByVal _npcName As String)
        Dim i As Integer = GetUserIDByUserName(_userName)
        'If OperationMode = "Server" Then
        tcpSender.SendData("5:2:1:2:" & ServerTokenList(i) & ":" & _userName & ":" & _npcName, ClientIPAddressess(i), tcpReceiver.Port) ' server command
        'Else
        '    tcpSender.SendData("5:2:1:2:" & MyClientToken & ":" & _userName & ":" & _npcName) ' client update
        'End If
    End Sub


    Public Sub SendNPCMessage(ByVal _userName As String, ByVal _npcName As String, ByVal _npcMessage As String)
        Dim i As Integer = GetUserIDByUserName(_userName)
        'If OperationMode = "Server" Then
        tcpSender.SendData("5:2:3:1:" & ServerTokenList(i) & ":" & _userName & ":" & _npcName & ":" & _npcMessage, ClientIPAddressess(i), tcpReceiver.Port) ' server command
        'Else
        '    tcpSender.SendData("5:2:3:1:" & MyClientToken & ":" & _userName & ":" & _npcName & ":" & _npcMessage) ' client update
        'End If
    End Sub

    Private Function GetUserIDByUserName(ByVal _userName As String)

        Return -1
    End Function

    Public Sub SendAllowConnectionRequest(ByVal _allow As Boolean, ByVal _ip As String)

    End Sub

    Public Function ConnectToServer(ByVal _serverIP As String, ByVal _serverPort As Integer, ByVal _clientUsername As String, ByVal _clientPassword As String, ByVal _clientIP As IPAddress, ByVal _clientPort As Integer)
        Select Case StartListening(_clientIP, _clientPort, 500, DataReceivedDelegate) ' Client Start Listening
            Case 0
                tcpSender = New tcpSender_v1(_serverIP, _serverPort)
                Select Case tcpSender.SendData("2:1:" & _clientUsername & ":" & _clientPassword & ":" & _clientIP.ToString) ' send connection request packet
                    Case 0
                        OperationMode = "Client"
                        DataReceivedDelegate("console message", "Connection Request sent")
                        Return 0
                    Case -1
                        tcpReceiver.StopListening()
                        DataReceivedDelegate("console message", "Server is not hosting (-1)")
                        Return -1
                    Case -2
                        tcpReceiver.StopListening()
                        DataReceivedDelegate("console message", "Unknown problem trying to connect to server.")
                        Return -2
                End Select
            Case -1
                'general error trying to connect
                Return -1
        End Select
        Return -9000
    End Function

    Public Sub LogonUser(ByVal _userName As String, ByVal _password As String)
        ReDim Preserve ClientUserList(ClientUserList.Length)
        ClientUserList(ClientUserList.Length - 1) = _userName
        MyClientToken = GenerateTokenForClient()
        ReDim Preserve ClientIPAddressess(ClientIPAddressess.Length)
        ClientIPAddressess(ClientIPAddressess.Length - 1) = ClientIPAddressess(0)
        GenerateTokenForServer()
        Dim DataObject(1) As String
        DataObject(0) = "logon notify"
        DataObject(1) = _userName
        DataReceivedDelegate("connection message", DataObject)
        'Dim DataObject(1) As String
        ReDim DataObject(1)
        DataObject(0) = _userName
        DataObject(1) = ">> You have logged on <<"
        DataReceivedDelegate("chat alert", DataObject)
    End Sub
    Public Sub LogoffUser(ByVal _userName As String)
        Dim i As Integer = 0
        Dim i2 As Integer = 0
        Do Until i >= ClientUserList.Length
            If ClientUserList(i) = _userName Then
                Dim new_ClientUserList(ClientUserList.Length - 2) As String
                Dim new_ClientIPAddressess(ClientUserList.Length - 2) As String
                Dim new_ClientTokenList(ClientUserList.Length - 2) As String
                Dim new_ServerTokenList(ClientUserList.Length - 2) As String
                ReDim ClientUserList(new_ClientUserList.Length - 1)
                ReDim ClientIPAddressess(new_ClientUserList.Length - 1)
                ReDim ClientTokenList(new_ClientUserList.Length - 1)
                ReDim ServerTokenList(new_ClientUserList.Length - 1)
                Do While i2 <> i
                    new_ClientIPAddressess(i2) = ClientUserList(i2)
                    new_ClientIPAddressess(i2) = ClientIPAddressess(i2)
                    new_ClientTokenList(i2) = ClientTokenList(i2)
                    new_ServerTokenList(i2) = ServerTokenList(i2)
                    i2 += 1
                Loop
                Dim DataObject(1) As String
                DataObject(0) = "logoff notify"
                DataObject(1) = _userName
                DataReceivedDelegate("connection message", DataObject)
                'Dim DataObject(1) As String
                ReDim DataObject(1)
                DataObject(0) = _userName
                DataObject(1) = ">> You have logged off <<"
                DataReceivedDelegate("chat alert", DataObject)
            End If
            i += 1
        Loop
    End Sub

    Public Sub LogoffUserNotify(Optional ByVal _userName As String = Nothing)
        If _userName = Nothing Then
            'this halts since server isn't listening
            'tcpSender.SendData("2:5:" & MyClientToken) ', tcpReceiver.IP.ToString, tcpReceiver.Port) ')
        Else
            For i As Integer = 0 To ClientUserList.Length
                If _userName = ClientUserList(i) Then
                    tcpSender.SendData("2:7:" & ServerTokenList(i), ClientIPAddressess(i), tcpReceiver.Port)
                    Dim DataObject(1) As String
                    DataObject(0) = "logoff notify"
                    DataObject(1) = _userName
                    DataReceivedDelegate("connection message", DataObject)
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Function GenerateTokenForServer()

    End Function
    Private Function GenerateTokenForClient()

    End Function
    Private Function GetClientUserName(ByVal _clientToken As String)

    End Function
#Region "Simple Properties"
    Public ReadOnly Property ClientIsListening As Boolean
        Get
            If tcpReceiver Is Nothing Then
                Return False
            Else
                Return tcpReceiver.ClientIsListening
            End If
        End Get
    End Property
#End Region
#Region "Helper Functions"
    ''' <summary>
    ''' Auto-Incrementer. Allows you to get the next index of the passed in index instead of manually incrementing it every time.
    ''' </summary>
    ''' <param name="I">The Index reference to increment. [Integer]</param>
    ''' <param name="IA">The amount to increment the index by. Default = 1 [Integer]</param>
    ''' <returns>Returns the new increment</returns>
    ''' <remarks>Nay! We are but men!</remarks>
    Private Function A(ByRef I As Integer, Optional IA As Integer = 1)
        I += IA
        Return I
    End Function
    Private Function NameProperizer(ByVal _name As String)
        Return Microsoft.VisualBasic.Left(_name, 1).ToUpper & Microsoft.VisualBasic.Right(_name, _name.Length - 1).ToLower
    End Function
#End Region
End Class

