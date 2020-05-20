Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class KairesTestUI
        ReadOnly AlertDebugCode As Boolean = False
        Public CloseFormLoaderToo As Boolean = True
        Private lb As CommonLibrary = FormLoader.lb
        Private Kairen As Kairen2 = FormLoader.Kairen
        Dim Kaires As New Kaires_v2(Me, AddressOf DataReceived)
        Dim BuddyBlock As Boolean = False
        Private Sub ChatTest_v2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
            ' FIRST THINGS FIRST '
            StatusLabel.Text = "Loading Window..."
        Me.Location = New Point(200, 75)

        'cloase other Kairen Windows...
        Dim itemlist(FormLoader.CheckedListBox1.CheckedItems.Count - 1) As String
        Dim i As Integer = 0
        For Each item In FormLoader.CheckedListBox1.CheckedItems
            itemlist(i) = item
            i += 1
        Next
        For Each item In itemlist
            If item.ToString <> "KairesTestUI" Then
                FormLoader.CheckedListBox1.SelectedItem = item
                FormLoader.CheckedListBox1.SetItemCheckState(FormLoader.CheckedListBox1.SelectedIndex, CheckState.Unchecked)
            End If
        Next
        'get cpu info
        'TextBox8.Text = Environment.ProcessorCount
        'TextBox9.Text = Environment.ProcessorCount
        TextBox10.Text = Environment.ProcessorCount

        ' DEBUG '
        Debug_StatusList_ComboBox.Items.Add("Loading Window...")
        Debug_StatusList_ComboBox.Items.Add("Offline")
        Debug_StatusList_ComboBox.Items.Add("Going Online...")
        Debug_StatusList_ComboBox.Items.Add("Online - Not Hosting")
        Debug_StatusList_ComboBox.Items.Add("Going Offline...")
        Debug_StatusList_ComboBox.Items.Add("Online - Starting Host...")
        Debug_StatusList_ComboBox.Items.Add("Online - Hosting")
        Debug_StatusList_ComboBox.Items.Add("Online - Ending Host...")
        Debug_StatusList_ComboBox.Items.Add("Online - Joining Host...")
        Debug_StatusList_ComboBox.Items.Add("Online - Joined")
        Debug_StatusList_ComboBox.Items.Add("Online - Leaving Host...")
        'Debug_StatusList_ComboBox.Items.Add("")
        'Debug_StatusList_ComboBox.Items.Add("")
        'Debug_StatusList_ComboBox.Items.Add("")
        'Debug_StatusList_ComboBox.Items.Add("")
        'Debug_StatusList_ComboBox.Items.Add("")
        'Debug_StatusList_ComboBox.Items.Add("")
        Button9.Text = "Timer1 is " & Timer1.Enabled
        ' END DEBUG '

        ' NORMAL STUFF
        TextBox5.MaxLength = 127
        Label10.Text = "/" & TextBox5.MaxLength ' Your Next Message ' Max Length
        Button4.Select()
        Kairen.ReinstantiateBuddyFile()
        RegisterUIElements()
        Kairen.LoadBuddyFile("BuddyFile")

        ' MORE DEBUG
        Button10.Text = "DebugBuddy is " & ListBox1.Items.Contains("Debugbuddy")
        ' END DEBUG

        ' END STUFF '
        StatusLabel.Text = "Offline"
    End Sub
        Private Sub ChatTest_v2_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            If StatusLabel.Text <> "Offline" Then
                e.Cancel = True
                Exit Sub
            End If
            FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
            'FAPI_Connection.ObjectsToUpdate.Remove(Me)
            Application.Exit()
        End Sub

        ' File Elements
        Private Sub RegisterUIElements()
            Kairen.BuddyFile.RegisterUIElement("Buddy List", ListBox1, AddressOf DataParserFunction_ListBox, , "Multiple Lines")
        End Sub
        Public Function DataParserFunction_ListBox(ByVal _dataTag As String, ByRef _dataObject As Object, ByRef _rawData As Object)
            'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
            If TypeOf _dataObject Is ListBox = False Then
                'error wrong control, return raw data
                Return _rawData
            End If
            Dim ReturnData(_dataObject.Items.Count - 1) As String
            Dim i As Integer = 0
            If _rawData IsNot Nothing Then
                If TypeOf (_rawData) Is Array Then
                    If _rawData.Length = 0 Then
                        Return "False"
                    End If
                End If
                If _rawData(0) = "False" Then Return Nothing
                For Each dataPiece In _rawData
                    i = 0
                    _dataObject.Items.Add(dataPiece)
                    'Do Until i >= _dataObject.Items.Count
                    '    If _dataObject.Items.Item(i) = dataPiece Then
                    '        _dataObject.SetItemCheckState(i, CheckState.Checked)
                    '        Exit Do
                    '    End If
                    '    i += 1
                    'Loop
                Next
                Return _rawData
            Else
                For Each item In _dataObject.Items
                    ReturnData(i) = CStr(item).Replace("id", "")
                    i += 1
                Next
                If i = 0 Then
                    ReDim ReturnData(0)
                    ReturnData(0) = "False"
                End If
                Return ReturnData
            End If
            'compare each string in array _rawData to each item in _dataObject and if they match, checked the item in _dataObject
        End Function
        Public Function DataParserFunction_BuddyField(ByVal _dataTag As String, ByRef _dataObject As Object, ByRef _rawData As Object)
            'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
            If TypeOf _dataObject Is ListBox = False Then
                'error wrong control, return raw data
                Return _rawData
            End If

            Dim ReturnData(_dataObject.Items.Count - 1) As String
            Dim i As Integer = 0
            If _rawData IsNot Nothing Then
                'finding file data and setting it to appropriate control
                If TypeOf (_rawData) Is Array Then
                    If _rawData.Length = 0 Then
                        Return "False"
                    End If
                End If
                If _rawData(0) = "False" Then Return Nothing

                _dataObject.Items.Add("Name: " & _rawData(0))
                _dataObject.Items.Add("IP: " & _rawData(1))


                Return _rawData
            Else
                'finding control and setting it's data to the file
                'Return Nothing
                ' *** this needs to save the new data to the file somehow

                ReturnData(0) = lb.Right(CStr(_dataObject.Items.Item(0)), _dataObject.Items.Item(0).Length - 6)
                ReturnData(1) = lb.Right(CStr(_dataObject.Items.Item(1)), _dataObject.Items.Item(1).Length - 4)

                Return ReturnData
            End If
            'compare each string in array _rawData to each item in _dataObject and if they match, checked the item in _dataObject
        End Function

        ' DEBUG CONTROLS '
        Private Sub DEBUG()
            If AlertDebugCode Then MsgBox("DEBUG CODE HIT")
        End Sub
        Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
            If Debug_StatusList_ComboBox.SelectedItem <> "" Then
                StatusLabel.Text = Debug_StatusList_ComboBox.SelectedItem
                Debug_StatusList_ComboBox.SelectedItem = Nothing
            End If
        End Sub
        Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
            If Timer1.Enabled = True Then
                Timer1.Enabled = False
            ElseIf Timer1.Enabled = False Then
                Timer1.Enabled = True
            End If
            Button9.Text = "Timer1 is " & Timer1.Enabled
        End Sub
        Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
            If Button10.Text = "DebugBuddy is False" Then
                ListBox1.Items.Add("Debugbuddy")
                Button10.Text = "DebugBuddy is True"
            ElseIf Button10.Text = "DebugBuddy is True" Then
                ListBox1.Items.Remove("Debugbuddy")
                Button10.Text = "DebugBuddy is False"
            End If
            Kairen.SaveBuddyFile("BuddyFile")
        End Sub

        ' Controls
        Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
            Select Case StatusLabel.Text
                'Case "Loading Window..."
                Case "Offline"
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = True ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = False ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = False ' Add 'Button
                    Button13.Enabled = False ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Going Online..."
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = False ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = False ' Add 'Button
                    Button13.Enabled = False ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Not Hosting"
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = True ' Go Offline 'Button
                    Button5.Enabled = True ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = True ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Going Offline..."
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = False ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = False ' Add 'Button
                    Button13.Enabled = False ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Starting Host..."
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Hosting"
                    GroupBox1.Enabled = True ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = True ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Ending Host..."
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Joining Host..."
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Joined"
                    GroupBox1.Enabled = True ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = True ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
                Case "Online - Leaving Host..."
                    GroupBox1.Enabled = False ' Netboard Main 'GroupBox
                    GroupBox2.Enabled = True ' Control Buttons 'GroupBox
                    Button4.Enabled = False ' Go Online 'Button
                    Button7.Enabled = False ' Go Offline 'Button
                    Button5.Enabled = False ' Start Host 'Button
                    Button6.Enabled = False ' End Host 'Button
                    Button2.Enabled = False ' Leave Host 'Button
                    GroupBox3.Enabled = True ' Buddy List 'GroupBox
                    Button1.Enabled = False ' Join 'Button
                    Button11.Enabled = False ' Invite 'Button
                    Button12.Enabled = True ' Add 'Button
                    Button13.Enabled = True ' Remove 'Button
                    GroupBox4.Enabled = True 'Status Area 'GroupBox
            End Select
        End Sub
        Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
            StatusLabel.Text = "Going Online..."
            If Kaires.StartHearing(System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName).AddressList(0), 51649, 500, AddressOf DataReceived) = 0 Then
                'successfully hosting ?
                StatusLabel.Text = "Online - Not Hosting"
                TextBox6.AppendText(" - Listening at: " & System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName).AddressList(0).ToString & ":51649" & vbNewLine)
            Else
                If Kaires.ClientIsListening = False Then StatusLabel.Text = "Offline"
                TextBox6.AppendText(" - Failed to start Listening" & vbNewLine)
            End If
        End Sub
        Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
            StatusLabel.Text = "Going Offline..."
            Kaires.StopListening()
            TextBox6.AppendText(" - End Listening at: " & System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName).AddressList(0).ToString & ":51649" & vbNewLine)
            StatusLabel.Text = "Offline"
        End Sub
        Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
            StatusLabel.Text = "Online - Starting Host..."
        End Sub
        Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
            StatusLabel.Text = "Online - Ending Host..."
        End Sub
        Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
            If ListBox1.SelectedItem = "" Then Exit Sub
            StatusLabel.Text = "Online - Joining Host..."
        End Sub
        Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
            StatusLabel.Text = "Online - Leaving Host..."
        End Sub
        Private Sub Button12_Click(sender As System.Object, e As System.EventArgs) Handles Button12.Click
            Dim responseName As String
            responseName = InputBox("Enter Buddy's ID", "Add Buddy - Enter Buddy ID").Trim
            If responseName = Nothing Then Exit Sub
            DEBUG()
            If responseName = "Debugbuddy" Then Exit Sub
            Dim responseIP As String
            responseIP = InputBox("Enter Buddy's IP", "Add Buddy - Enter Buddy IP").Trim
            If responseIP = Nothing Then Exit Sub
            Dim buddyip As IPAddress
            Try
                buddyip = System.Net.IPAddress.Parse(responseIP)
            Catch ex As Exception
                Exit Sub
            End Try
            ListBox1.Items.Add(responseName)
            Kairen.BuddyFile.RegisterUIElement("id" & responseName, ListBox2, AddressOf DataParserFunction_BuddyField, , "Multiple Lines")
            ListBox2.Items.Clear()
            ListBox2.Items.Add("Name: " & responseName)
            ListBox2.Items.Add("IP: " & responseIP)
            Kairen.SaveBuddyFile("BuddyFile")
            ListBox1.Items.Clear()
            Kairen.ReinstantiateBuddyFile()
            RegisterUIElements()
            Kairen.LoadBuddyFile("BuddyFile")
        End Sub
        Private Sub Button13_Click(sender As System.Object, e As System.EventArgs) Handles Button13.Click
            If ListBox1.SelectedItem = Nothing Then Exit Sub
            DEBUG()
            If ListBox1.SelectedItem.ToString = "Debugbuddy" Then Exit Sub
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            Kairen.BuddyFile.RemoveTag("id" & ListBox1.SelectedItem)
            Kairen.SaveBuddyFile("BuddyFile")
            ListBox2.Items.Clear()
        End Sub
        Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
            Dim response As String
            response = InputBox("Enter Invitee's IP", "Invite Random Person - Enter Person's IP").Trim
            If response = Nothing Then Exit Sub
            Dim personip As IPAddress
            Try
                personip = System.Net.IPAddress.Parse(response)
            Catch ex As Exception
                Exit Sub
            End Try
            'invite personip
        End Sub
        Private Sub ListBox1_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
            If BuddyBlock = True Then Exit Sub
            If ListBox1.SelectedItem = Nothing Then Exit Sub
            DEBUG()
            If ListBox1.SelectedItem.ToString = "Debugbuddy" Then
                ListBox2.Items.Clear()
                Exit Sub
            End If
            BuddyBlock = True
            'Kairen.ReinstantiateBuddyFile()
            'RegisterUIElements()
            'For Each item In ListBox1.Items
            'Kairen.BuddyFile.RegisterUIElement("id" & item, ListBox2, AddressOf DataParserFunction_BuddyField, , "Multiple Lines")
            'Next
            'Kairen.LoadBuddyFile("Buddy File")

            Dim selectedBuddy As String = ListBox1.SelectedItem
            'Kairen.ReinstantiateBuddyFile()
            'RegisterUIElements()
            Kairen.BuddyFile.RegisterUIElement("id" & selectedBuddy, Nothing, , , "Multiple Lines")
            'ListBox1.Items.Clear()
            ListBox2.Items.Clear()
            DataParserFunction_BuddyField("id" & selectedBuddy, ListBox2, Kairen.BuddyFile.GetDataByTag("id" & selectedBuddy, -1))

            'Kairen.LoadBuddyFile("BuddyFile")
            'ListBox1.SelectedItem = selectedBuddy


            'Kairen.BuddyFile.UpdateUIElementsByFile()
            BuddyBlock = False
        End Sub

        ' ??? '
        Public Sub DataReceived(ByVal _dataType As String, ByVal _dataObject As Object)
            Select Case _dataType
                Case "console message"
                    'If IsArray(_dataObject) Then
                    '    For i As Integer = _dataObject.Length To 1
                    '        i -= 1
                    '        TextBox6.AppendText(" >> " & _dataObject(i) & " << " & Newline)
                    '    Next
                    'Else
                    '    TextBox6.AppendText(" >> " & _dataObject & " << " & Newline)
                    'End If
                Case "chat message"
                    'If _dataObject(0) = TextBox7.Text Then
                    'TextBox6.AppendText(_dataObject(0) & " says: " & _dataObject(1) & Newline)
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
                    'TextBox6.AppendText(_dataObject(1) & Newline) ' while server show as Server
                    'If _dataObject(0) = TextBox7.Text Then
                    'TextBox1.AppendText(_dataObject(1) & Newline) ' while server show as Server
                    'Else
                    'TextBox1.AppendText(_dataObject(1) & Newline) ' while server show as Server
                    'End If
                Case "game update"
                    'If CheckBox1.Checked = False Then
                    '    TextBox6.AppendText("GU :: " & _dataObject(0) & " IA -> " & _dataObject(1) & ", " & _dataObject(2) & ", " & _dataObject(3) & ", " & _dataObject(4) & Newline)
                    'End If
                    'FAPI_Connection.IssueFAPICommand(5, "UpdatePlayerData", _dataObject)
                Case "game command"
                    'TextBox6.AppendText("GU :: " & _dataObject(0) & " IA -> " & _dataObject(1) & ", " & _dataObject(2) & ", " & _dataObject(3) & ", " & _dataObject(4) & Newline)
                    'Select Case _dataObject(0)
                    '    Case "spawn npc"
                    '        FAPI_Connection.IssueFAPICommand(6, "SpawnNPC", _dataObject(1))
                    '    Case "despawn npc"
                    '        FAPI_Connection.IssueFAPICommand(7, "DespawnNPC", _dataObject(1))
                    'End Select
                Case "program command"
                    'Select Case _dataObject(0)
                    '    Case "allow connection request sending"
                    '        GroupBox4.Enabled = True
                    '        Button1.Visible = True
                    '        Button2.Visible = True
                    '    Case "disallow connection request sending"
                    '        Button1.Visible = False
                    '        Button2.Visible = False
                    'End Select
                Case "NPCMessage1"
                    'TextBox6.AppendText("NPC MSG1 :: " & _dataObject(0) & _dataObject(1) & Newline)
                Case Else
                    'TextBox6.AppendText(Me.Name & " has received an unknown _dataType in DataReceived()" & Newline)
            End Select
        End Sub 'Data Received

    Private Sub Button14_Click(sender As System.Object, e As System.EventArgs) Handles Button14.Click

    End Sub

    Public Class ThreadHandler
        Public ReadOnly MaxThreads As Integer = (Environment.ProcessorCount * 6)
        Dim currentThread As Integer = 0
        Dim workerThread(MaxThreads - 1) As Threading.Thread
        'Delegate Sub MultithreadDelegateSub(ByVal CurrentCountInteger As Integer)
        Sub NewThread()

            workerThread(currentThread) = New Threading.Thread(New Threading.ThreadStart(AddressOf CounterSub))
            workerThread(currentThread).Start()
        End Sub
        Sub CounterSub()

        End Sub

    End Class
End Class