Public Class Kaires
    Public CloseFormLoaderToo As Boolean = True

    'GLOBAL Classes
    Dim lb As CommonLibrary = Kairen_Main.lb

    'LOCAL Classes
    Dim CEKey() As CEKey
    'Dim FAPI_Connection = Kairen_Main.FAPI_Connection
    Dim GrabData_Addresses As TextFileClass

#Region "Shared"
    Private Sub Kaires_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        GrabData_Addresses = New TextFileClass(lb.Folder_Net_Streams & "o/Address List" & lb.Extension_ReadWrites, "--", True)
    End Sub
#End Region


    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim lines() As String = TextBox1.Lines.Clone ' i is index
        Dim i As Integer ' lines() index
        Dim luaComment As String
        Do While i < lines.Length
            lines(i) = lines(i).Trim
            If lb.Left(lines(i), 5) = "<?xml" Then

            ElseIf lb.Left(lines(i), 12) = "<CheatTable>" Then

            ElseIf lb.Left(lines(i), 14) = "<CheatEntries>" Then
            ElseIf lb.Left(lines(i), 12) = "<CheatEntry>" Then
            ElseIf lb.Left(lines(i), 4) = "<ID>" Then

            ElseIf lb.Left(lines(i), 13) = "<Description>" Then
                luaComment = lines(i).Replace("<Description>" & Chr(34), "").Replace(Chr(34) & "</Description>", "")
            ElseIf lb.Left(lines(i), 10) = "<LastState" Then
            ElseIf lb.Left(lines(i), 11) = "<ShowAsHex>" Then
            ElseIf lb.Left(lines(i), 7) = "<Color>" Then
            ElseIf lb.Left(lines(i), 14) = "<VariableType>" Then
                luaComment = luaComment & ", " & lines(i).Replace("<VariableType>", "").Replace("</VariableType>", "")
            ElseIf lb.Left(lines(i), 12) = "<ByteLength>" Then
                'luaComment = lines(i).Replace("", "").Replace("", "")
            ElseIf lb.Left(lines(i), 9) = "<Address>" Then
                TextBox2.AppendText(Chr(34) & "[" & lines(i).Replace("<Address>", "").Replace(Chr(34), "").Replace("</Address>", "") & "]+")
            ElseIf lb.Left(lines(i), 9) = "<Offsets>" Then
            ElseIf lb.Left(lines(i), 8) = "<Offset>" Then
                TextBox2.AppendText(lines(i).Replace("<Offset>", "").Replace("</Offset>", "") & Chr(34) & "--" & luaComment & vbNewLine)
                'luaComment = lines(i).Replace("", "").Replace("", "")
            ElseIf lb.Left(lines(i), 10) = "</Offsets>" Then
            ElseIf lb.Left(lines(i), 13) = "</CheatEntry>" Then
            ElseIf lb.Left(lines(i), 8) = "<Length>" Then
                'luaComment = lines(i).Replace("", "").Replace("", "")
            ElseIf lb.Left(lines(i), 9) = "<Unicode>" Then
            ElseIf lb.Left(lines(i), 15) = "<ZeroTerminate>" Then
            ElseIf lb.Left(lines(i), 8) = "<Options" Then
            ElseIf lb.Left(lines(i), 15) = "</CheatEntries>" Then
            ElseIf lb.Left(lines(i), 13) = "</CheatTable>" Then
            Else
                If CheckBox2.Checked = False Then
                    TextBox2.AppendText(lines(i) & vbNewLine)
                End If
            End If
            i = i + 1
        Loop


        Exit Sub
        'Dim lines() As String = TextBox23.Lines.Clone ' i is index
        Dim newlineNumber As Integer ' index for below newline_ variables
        Dim newline_description(), newline_variableType(), newline_address(), newline_offeset() As String
        Dim errorline() As String
        'Dim i As Integer ' lines() index
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
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 4)
                        Case "<ID>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 5
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 13)
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
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 10)
                        Case "<LastState"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 7
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 7)
                        Case "<Color>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 8
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 14)
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
                            Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 9)
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
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 14)
                        Case "<Offsets>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 11
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 8)
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
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 10)
                        Case "</Offsets>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 13
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 13)
                        Case "</CheatEntry>"
                            _step = 3 ' jumps back here
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select
                Case 14
                    Select Case Microsoft.VisualBasic.Left(lines(i).Trim(" "), 13)
                        Case "</CheatTable>"
                            _step = _step + 1
                        Case Else
                            MsgBox("error 3, exit sub.")
                            Exit Do
                    End Select

            End Select
            i = i + 1
        Loop
        'TextBox23.Clear()
        i = 1
        'newlineNumber = 0
        Do Until i > newlineNumber
            '    TextBox23.AppendText(newline_address(i) & Chr(34) & "+" & newline_offeset(i) & " -- " & newline_description(i) & " -- type:" & newline_variableType(i))
            '    TextBox23.AppendText(vbNewLine)
            i = i + 1
        Loop
        'TextBox23.AppendText(newline)
    End Sub

    Private Sub CheckBox1_Click(sender As System.Object, e As System.EventArgs) Handles CheckBox1.Click
        If Kairen_Main.FAPI_Connection Is Nothing Then
            CheckBox1.Checked = False
            Exit Sub
        End If
        Kairen_Main.FAPI_Connection.updateKaires = True
        'lb.ParseOutsideCommand("DevModeCommand", "Do Output")
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        'ComboBox1.SelectedIndex = 0
        'ComboBox2.SelectedIndex = 0
        'TextBox4.Text = "MyX"
        If Kairen_Main.FAPI_Connection Is Nothing Then Exit Sub
        Dim lines(5) As String
        lines(0) = "Add Address" 'command
        lines(1) = ComboBox1.Text 'address
        lines(2) = ComboBox2.Text 'type
        lines(3) = TextBox4.Text 'name
        lines(4) = TextBox5.Text 'opt length
        lines(5) = ComboBox3.Text 'opt subtype
        lb.WriteArrayToFile(lb.Folder_Net_Streams & "o/DevModeFile" & lb.Extension_ReadWrites, lines)
        Kairen_Main.FAPI_Connection.AddCommand("DevModeCommand")
    End Sub
    Public Sub UpdateYourData()
        If Kairen_Main.FAPI_Connection Is Nothing Then Exit Sub
        If GrabData_Addresses.FileExists = False Then Exit Sub
        GrabData_Addresses = New TextFileClass(lb.Folder_Net_Streams & "o/Address List" & lb.Extension_ReadWrites, "--", True)
        GrabData_Addresses.LoadFile()
        'GrabData_Addresses.DeleteFile()
        ListBox2.Items.Clear()
        While GrabData_Addresses.CurrentIndex <= GrabData_Addresses.NumberOfLines()
            ListBox2.Items.Add(GrabData_Addresses.ReadLine() & ": " & GrabData_Addresses.ReadLine())
        End While
        'GrabData_Addresses.ReadLine()
        'TextBox7.Text = GrabData_Addresses.ReadLine()
        'If TextBox7.Text <> "" Then
        'MsgBox("hwoah")
        'End If
        Kairen_Main.FAPI_Connection.AddCommand("DevModeCommand", "Do Output")
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If TextBox8.Text.Trim = "" Then Exit Sub
        Kairen_Main.FAPI_Connection.AddCommand("DevModeCommand", TextBox8.Text)
    End Sub

    Private Function ii(ByRef i As Integer) As Integer
        i = i + 1
        Return i
    End Function
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Dim i As Integer = -1
        Dim chatSlots(11) As String
        chatSlots(ii(i)) = "Add Address" 'command
        chatSlots(ii(i)) = Chr(34) & "[pcsx2-r3878.exe+003FCE14]+1B8" & Chr(34) 'address
        chatSlots(ii(i)) = "String" 'type
        chatSlots(ii(i)) = "ChatSlot1" 'name
        chatSlots(ii(i)) = 156 'opt length
        chatSlots(ii(i)) = "TypeGameChat" 'opt subtype

        chatSlots(ii(i)) = "Add Address" 'command
        chatSlots(ii(i)) = Chr(34) & "[pcsx2-r3878.exe+003FCE14]+258" & Chr(34) 'address
        chatSlots(ii(i)) = "String" 'type
        chatSlots(ii(i)) = "ChatSlot2" 'name
        chatSlots(ii(i)) = 150 'opt length
        chatSlots(ii(i)) = "TypeGameChat" 'opt subtype

        lb.WriteArrayToFile(lb.Folder_Net_Streams & "o/DevModeFile" & lb.Extension_ReadWrites, chatSlots)
        Kairen_Main.FAPI_Connection.AddCommand("DevModeCommand")
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        TextBox1.Clear()
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        TextBox2.Clear()
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        If TextBox2.Text.Trim <> "" Then
            If TextBox6.Text <> "" Then
                TextBox6.AppendText(vbNewLine)
            End If
            TextBox6.AppendText(TextBox2.Text)
            TextBox2.Clear()
        End If
    End Sub

    Private Sub Kaires_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'FormLoader.ActuallyClose = CloseFormLoaderToo
        'Application.Exit()
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        Dim objShell As Object
        Dim objLink As Object
        Try
            objShell = CreateObject("WScript.Shell")
            ' Automatically creates a shortcut on the desktop with the product name of the application. 
            objLink = objShell.CreateShortcut(My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & linkName.Text & ".lnk")

            ' Sets the shortcut's link to the application's main executable file. 
            'objLink.TargetPath = TextBox10.Text
            For Each index In CheckedListBox1.CheckedIndices
                If CheckedListBox1.Items(index) = "ISO Location" Then
                    objLink.Arguments = Chr(34) & InputBox("What is the ISO Location?", "Paste your EQOA .iso location", "C:\Users\Kaynewihza\Documents\EQOA Revival\Frontiers\FRONTSPATCH.iso") & Chr(34)
                End If
            Next
            objLink.TargetPath = FilePath.Text
            Dim b As String = lb.Left(FilePath.Text, FilePath.Text.Length - (FilePath.Text.Length - FilePath.Text.LastIndexOf("\")))
            objLink.WorkingDirectory = lb.Left(FilePath.Text, FilePath.Text.Length - (FilePath.Text.Length - FilePath.Text.LastIndexOf("\")))
            objLink.WindowStyle = 1
            '"C:\Users\Kaynewihza\Documents\EQOA Revival\PCSX2 0.9.7\pcsx2-r3878.exe" "C:\Users\Kaynewihza\Documents\EQOA Revival\Frontiers\FRONTSPATCH.iso"
            objLink.Save()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        Dim lines() As String = TextBox9.Lines.Clone ' i is index
        Dim _lines2((lines.Count * 3) - 1) As String
        Dim i As Integer ' lines() index
        Dim i2 As Integer
        Do While i < lines.Length
            lines(i) = lines(i).Trim
            'Kairen.ItemFile.RegisterUIElement("nameSafe", TextBox6, AddressOf ParseRawFileDataForUIPresentation)
            lines(i).Replace("Kairen.ItemFile.RegisterUIElement(" & Chr(34), "")
            lines(i) = lb.Right(lines(i), lines(i).IndexOf(","))
            MsgBox(lines(i))



            i = i + 1
        Loop

        If CheckBox2.Checked = False Then
            TextBox2.AppendText(lines(i) & vbNewLine)
        End If
    End Sub
End Class