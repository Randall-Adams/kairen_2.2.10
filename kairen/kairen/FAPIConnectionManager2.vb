Public Class FAPIConnectionManager2
    Private lb As CommonLibrary
    Public o_FAPIData As TextFileClass 'input from Kanizah.lua
    Public ObjectsToUpdate As New List(Of Object) 'Forms To Update

    Public i_FAPIDataRequests As TextFileClass 'output into Kanizah.lua, requesting data to be sent
    Private Requests(-1) As FAPIConnectionRequestClass
    Private PendingRequests(-1) As String
    Private PendingRequests_Type(-1) As Integer
    Private PendingRequests_OneTime(-1) As String
    Private PendingRequests_Type_OneTime(-1) As Integer
    Private SentRequests(-1) As String
    Private SentRequests_OneTime(-1) As String

    Public i_FAPIData_Unreliable As TextFileClass 'output into Kanizah.lua, supplying it with updated values
    Private FAPIDataOutputQueue_Unreliable(-1) As String
    Private FAPIDataOutputQueue_UnreliableAdditionalData(-1) As String

    'the type of the fapi data request is known by the sender, otherwise it is only discoverable by looking up the number

    'create events so forms can be alerted by the global fapi manager that it is doing different things
    Sub New(ByRef _lb As CommonLibrary)
        lb = _lb
    End Sub
    Public Sub UpdateFAPIDisplayObjects()
        'SpoofFapiFile()
        'SendFAPIDataRequest() ' should this actually go here??
        SendFAPICommands()
        If lb.FE(lb.Folder_Net_Streams & "o\FAPI Data2" & lb.Extension_ReadWrites) = False Then 'check if there is an update
            'old'lb.DisplayMessage("No FAPI Data2 file found. Was it created?", "Error:", "FAPIConnectionManager2") 'if no update display and exit
            'if no updated file then just don't update yet
            Exit Sub
        End If
        o_FAPIData = New TextFileClass(lb.Folder_Net_Streams & "o\FAPI Data2" & lb.Extension_ReadWrites, Nothing, True) 'dim the file
        o_FAPIData.LoadFile() 'load it into memory for manipulation
        Do While o_FAPIData.CurrentIndex <= o_FAPIData.NumberOfLines And o_FAPIData.NumberOfLines > 0
            o_FAPIData.ReadLine() 'read the line data
            o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = o_FAPIData.ReadLine() 'set the additional data for the previous readline equal to the next readline
        Loop

        For Each myform In ObjectsToUpdate
            Try
                myform.UpdateFAPIDisplay()
            Catch ex As Exception
                ' this fires incorrectly because it fires when myform has a problem with it's sub, not just when it doesn't have one
                ' orrrr my error message is wrong actually haha
                '    lb.DisplayMessage("Error: " & myform.text & " does not have an UpdateFAPIDisplay() procedure.", "Programmer Error:", "FAPIConnectionManager2")
            End Try
        Next
        o_FAPIData.DeleteFile()
    End Sub 'tealls each form to update it's fapi data, reads the fapi data file

    Public Sub RequestFAPIData(ByVal RequestName As String, Optional ByVal type As Integer = 1)
        For Each request In PendingRequests
            If request = RequestName Then
                request = Nothing
                Exit Sub
            End If
        Next
        For Each request In SentRequests
            If request = RequestName Then
                Exit Sub
            End If
        Next
        Dim i As Integer = PendingRequests.Length
        ReDim Preserve PendingRequests(i)
        ReDim Preserve PendingRequests_Type(i)
        PendingRequests(i) = RequestName
        PendingRequests_Type(i) = type
    End Sub 'queues requests for fapi data from kanizah
    Public Sub SendFAPIDataRequest() 'sends the queued fapi requests to kanizah
        If PendingRequests.Length <= 0 Then Exit Sub
        If lb.FE(lb.Folder_Net_Streams & "i\FAPI Data Request" & lb.Extension_ReadWrites) = True Then 'check if there is a pending request
            'put request into queue instead, then exit.
            Exit Sub
        End If
        i_FAPIDataRequests = New TextFileClass(lb.Folder_Net_Streams & "i\FAPI Data Request" & lb.Extension_ReadWrites, "--", True) 'dim the file
        i_FAPIDataRequests.LoadFile() 'load it into memory for manipulation
        ReDim Preserve SentRequests(PendingRequests.Length - 1)
        Dim i As Integer
        For Each request In PendingRequests
            'process queue before processing current request
            Select Case PendingRequests_Type(i)
                Case 1
                    i_FAPIDataRequests.WriteLine("AddOutputByAddressName") 'write the request command
                    i_FAPIDataRequests.WriteLine(request) 'write the request data, which is an Address.lua value
                Case 2
                    i_FAPIDataRequests.WriteLine("UpdateVariableByVariableName") 'write the request command
                    i_FAPIDataRequests.WriteLine(request) 'write the request data, which is an Address.lua value
                Case 3
                    i_FAPIDataRequests.WriteLine("PrintToConsole") 'write the request command
                    i_FAPIDataRequests.WriteLine(request) 'write the request data, which is what should be output to the lua console
                Case Else
                    MsgBox("Error - SendFAPIDataRequest: Invalid Request Attempt Type: " & PendingRequests_Type(i))
            End Select
            SentRequests(i) = request
            i = i + 1
        Next
        i_FAPIDataRequests.SaveFile()
    End Sub 'writes the request file

    Public Sub QueueFAPIOutput_Unreliable(ByVal _data As String, ByVal _additionalData As String) 'unreliably sends kanizah updated fapi data
        Dim i As Integer
        If FAPIDataOutputQueue_UnreliableAdditionalData.Contains(_additionalData) = False Then
            i = FAPIDataOutputQueue_Unreliable.Length
            ReDim Preserve FAPIDataOutputQueue_Unreliable(i)
            ReDim Preserve FAPIDataOutputQueue_UnreliableAdditionalData(i)
        Else
            i = Array.IndexOf(FAPIDataOutputQueue_UnreliableAdditionalData, _additionalData)
        End If
        i = i
        FAPIDataOutputQueue_Unreliable(i) = _data
        FAPIDataOutputQueue_UnreliableAdditionalData(i) = _additionalData
    End Sub
    Public Sub SendFAPIOutput_Unreliable() 'unreliably sends kanizah updated fapi data
        'text file needs dimd
        'make a function that will add addtional data to the file, and another function that will update the additonal data for the passed variable
        'then a function that saves the file for the fapi to take in when it can.
        'maybe make an unreliable and a reliable file, one for things that can be skipped and one for things that can't
        i_FAPIData_Unreliable = New TextFileClass(lb.Folder_Net_Streams & "i\FDi_Unreliable.txt", "--")
        Dim i As Integer = 0
        i_FAPIData_Unreliable.WriteLine("UpdateVariableByVariableName")
        Do While i <= FAPIDataOutputQueue_UnreliableAdditionalData.Length - 1
            i_FAPIData_Unreliable.WriteLine(FAPIDataOutputQueue_Unreliable(i))
            i_FAPIData_Unreliable.WriteLine(FAPIDataOutputQueue_UnreliableAdditionalData(i))
            i = i + 1
        Loop
        i_FAPIData_Unreliable.SaveFile()
        ReDim FAPIDataOutputQueue_Unreliable(-1)
        ReDim FAPIDataOutputQueue_UnreliableAdditionalData(-1)
    End Sub

    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestData As String)
        'If RequestType = Nothing then Exit Sub
        If RequestData = Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, RequestData, Nothing)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestDataArray() As String)
        'If RequestType = Nothing then Exit Sub
        If RequestDataArray Is Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, Nothing, RequestDataArray)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestData As String, ByVal RequestDataArray() As String)
        'If RequestType = Nothing then Exit Sub
        If RequestData = Nothing Then Exit Sub
        If RequestDataArray Is Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, RequestData, RequestDataArray)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestDataArray() As String, ByVal RequestData As String)
        'If RequestType = Nothing then Exit Sub
        If RequestDataArray Is Nothing Then Exit Sub
        If RequestData = Nothing Then Exit Sub
        IssueFAPICommand_Real(RequestType, RequestData, RequestDataArray)
    End Sub
    Public Sub IssueFAPICommand(ByVal RequestType As Integer, ByVal RequestData1 As String, ByVal RequestData2 As String)
        'If RequestType = Nothing then Exit Sub
        If RequestData1 = Nothing Then Exit Sub
        If RequestData2 = Nothing Then Exit Sub
        Dim RequestDataArray(1) As String
        RequestDataArray(0) = RequestData1
        RequestDataArray(1) = RequestData2
        IssueFAPICommand_Real(RequestType, Nothing, RequestDataArray)
    End Sub
    Private Sub IssueFAPICommand_Real(ByVal RequestType As Integer, ByVal RequestData As String, ByVal RequestDataArray() As String)
        'the requesttype numbers appear to only exist to be able to change each command type in the future, passes english names to the lua, not a number
        Select Case RequestType
            Case 1, 2, 3, 6, 7 ' not arrays
                'these don't get resent, so their names help to identify if they've been sent
                For Each _request In Requests
                    If _request.RequestData = RequestData Then
                        'request already created
                        Exit Sub
                    End If
                Next
                Dim i As Integer = Requests.Length
                ReDim Preserve Requests(i)
                Requests(i) = New FAPIConnectionRequestClass(RequestType, RequestData, Nothing)
            Case 4 'arrays
                'these get sent multiple times, if they have been sent, then redim the old request in memory and fill it once more
                '                               if they have not been sent, then just exit, because at this time the code has the data as read only
                For Each _request In Requests
                    If _request.RequestData_Array(0) = RequestDataArray(0) Then
                        If _request.Sent = True Then
                            _request = New FAPIConnectionRequestClass(RequestType, Nothing, RequestDataArray)
                            Exit Sub
                        Else
                            'request already pending
                            Exit Sub
                        End If
                    End If
                Next
                ReDim Preserve Requests(Requests.Length)
                Requests(Requests.Length - 1) = New FAPIConnectionRequestClass(RequestType, Nothing, RequestDataArray)
            Case 5, 8 'arrays
                'these don't get resent, so their names help to identify if they've been sent
                For Each _request In Requests
                    If _request.RequestData_Array(0) = RequestDataArray(0) Then
                        If _request.Sent = True Then
                            '_request = New FAPIConnectionRequestClass(RequestType, RequestData, RequestDataArray)
                            Exit Sub
                        Else
                            'request already pending
                            Exit Sub
                        End If
                    End If
                Next
                ReDim Preserve Requests(Requests.Length)
                Requests(Requests.Length - 1) = New FAPIConnectionRequestClass(RequestType, Nothing, RequestDataArray)
        End Select
    End Sub

    Private Sub SendFAPICommands()
        If Requests.Length = -1 Then Exit Sub
        If lb.FE(lb.Folder_Net_Streams & "i\FAPI Data Request" & lb.Extension_ReadWrites) = True Then 'check if there is a pending request
            'put request into queue instead, then exit.
            Exit Sub
        End If
        Dim exitmarker As Boolean = True
        For Each _request In Requests
            If _request.Sent = False Then
                exitmarker = False
                Exit For
            End If
        Next
        If exitmarker = True Then Exit Sub
        i_FAPIDataRequests = New TextFileClass(lb.Folder_Net_Streams & "i\FAPI Data Request" & lb.Extension_ReadWrites, "--", True) 'dim the file
        i_FAPIDataRequests.LoadFile() 'load it into memory for manipulation
        For Each _request In Requests
            If _request.Sent = False Then
                'process queue before processing current request
                Select Case _request.Type
                    Case 1
                        i_FAPIDataRequests.WriteLine("AddOutputByAddressName") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the request data, which is an Address.lua value
                    Case 2
                        i_FAPIDataRequests.WriteLine("UpdateVariableByVariableName") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the request data, which is an Address.lua value
                    Case 3
                        i_FAPIDataRequests.WriteLine("PrintToConsole") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the request data, which is what should be output to the lua console
                    Case 4
                        i_FAPIDataRequests.WriteLine("UpdateAddressByAddressName") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(0)) 'write the address name
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(1)) 'write the address value
                    Case 5
                        i_FAPIDataRequests.WriteLine("UpdatePlayerData") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(0)) 'write the address value name
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(1)) 'write the address value x
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(2)) 'write the address value y
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(3)) 'write the address value z
                        i_FAPIDataRequests.WriteLine(_request.RequestData_Array(4)) 'write the address value f
                    Case 6
                        i_FAPIDataRequests.WriteLine("SpawnNPC") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc name
                    Case 7
                        i_FAPIDataRequests.WriteLine("DespawnNPC") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc name
                    Case 8
                        i_FAPIDataRequests.WriteLine("NPCMessage1") 'write the request command
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc name
                        i_FAPIDataRequests.WriteLine(_request.RequestData) 'write the npc message

                    Case Else
                        MsgBox("Error - SendFAPIDataRequest: Invalid Request Attempt Type: " & _request.Type)
                End Select
                _request.Sent = True
            End If
        Next
        i_FAPIDataRequests.SaveFile()
    End Sub

#Region "FAPIFile"
    Private Sub LoadFAPIFile()
        If lb.FE(lb.Folder_Net_Streams & "o\FAPI Data2" & lb.Extension_ReadWrites) = False Then
            lb.DisplayMessage("No FAPI Data2 file found. Was it created?", "Error:", "FAPIConnectionManager2")
            Exit Sub
        End If
        o_FAPIData = New TextFileClass(lb.Folder_Net_Streams & "o\FAPI Data2" & lb.Extension_ReadWrites, "--", True)
        o_FAPIData.LoadFile()
        o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = "MyX"
        o_FAPIData.ReadLine()
        o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = "MyY"
        o_FAPIData.ReadLine()
        o_FAPIData.AdditionalData(o_FAPIData.CurrentIndex) = "MyZ"
        o_FAPIData.ReadLine()
        'd_yd_tb_X.Text = GrabData_NPC.ReadLine
        'd_yd_tb_Y.Text = GrabData_NPC.ReadLine
        'd_yd_tb_Z.Text = GrabData_NPC.ReadLine
        'd_yd_tb_F.Text = GrabData_NPC.ReadLine
        'TextBox10.Text = GrabData_NPC.ReadLine
        'TextBox14.Text = GrabData_NPC.ReadLine 'columns rows 10\14
        'TextBox11.Text = GrabData_NPC.ReadLine
        'TextBox15.Text = GrabData_NPC.ReadLine 'nest 11\15
        'TextBox5.Text = GrabData_NPC.ReadLine
        'd_yd_tb_ZoneFull.Text = GrabData_NPC.ReadLine
        'd_yd_tb_ZoneName.Text = GrabData_NPC.ReadLine
        'd_yd_tb_ZoneSub.Text = GrabData_NPC.ReadLine
        'If lb.FE(lb.Folder_Temp_NPC_Maker & "New_NPC.txt") Then
        '    My.Computer.FileSystem.DeleteFile(lb.Folder_Temp_NPC_Maker & "New_NPC.txt")
        'End If
        'GrabData_NPC = Nothing
    End Sub
#End Region
End Class
