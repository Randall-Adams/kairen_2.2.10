Public Class AbilityMaker
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    Private Kairen As Kairen2 = FormLoader.Kairen
    ' Delegate Function DataParserDelegate(ByVal _dataTag As String, ByRef _dataControl As Object, ByVal _rawData As Object)

    Private Sub AbilityMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If lb.DE(lb.Folder_Custom_Abilities) = False Then My.Computer.FileSystem.CreateDirectory(lb.Folder_Custom_Abilities)
        lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Abilities, lb.Extension_AbilityFile), ComboBox1, True)

        Kairen.AbilityFile.RegisterUIElement("nameSafe", TextBox9, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("nameGame", TextBox1, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Description", TextBox2, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Cast Time", TextBox4, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Recast Time", TextBox5, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Range", TextBox3, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Power Cost", TextBox6, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Level Requirement", TextBox7, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("AoE Range", TextBox8, AddressOf DataParserFunction)
        Kairen.AbilityFile.RegisterUIElement("Requires Line of Sight", CheckBox1, AddressOf DataParserFunction)

        GroupBox1.Enabled = False

        Me.Text = Me.Text & " In-Build Preview - Kairen  " & Launcher_v2.Version_Current_Release
    End Sub
    Private Sub AbilityMaker_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
        'FAPI_Connection.ObjectsToUpdate.Remove(Me)
        Application.Exit()
    End Sub
    Public Function DataParserFunction(ByVal _dataTag As String, ByRef _dataControl As Object, ByRef _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        Dim ReturnData As String
        ReturnData = _rawData
        If _dataControl Is TextBox1 Or _
            _dataControl Is TextBox2 Or _
            _dataControl Is TextBox3 Or _
            _dataControl Is TextBox4 Or _
            _dataControl Is TextBox5 Or _
            _dataControl Is TextBox6 Or _
            _dataControl Is TextBox7 Or _
            _dataControl Is TextBox8 Or _
            _dataControl Is TextBox9 Then
            If _rawData IsNot Nothing Then 'setting control - convert file to control
                If _rawData = "False" Then
                    _dataControl.Text = ""
                    ReturnData = _dataControl.Text
                Else
                    _dataControl.Text = _rawData
                    ReturnData = _rawData
                End If
            Else 'getting control - convert control to file
                If _dataControl.Text.trim = "" Then
                    ReturnData = "False"
                Else
                    ReturnData = _dataControl.Text
                End If
            End If
        ElseIf _dataControl Is CheckBox1 Then
            If _rawData IsNot Nothing Then 'setting control - convert file to control
                If _rawData = "True" Then
                    _dataControl.Checked = True
                    ReturnData = True
                Else
                    _dataControl.Checked = False
                    ReturnData = False
                End If
            Else 'getting control - convert control to file
                If _dataControl.Checked = True Then
                    ReturnData = True
                Else
                    ReturnData = False
                End If
            End If
        End If
        Return ReturnData
    End Function

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If ComboBox1.Text.Trim = "" Then Exit Sub
        If ComboBox1.Enabled = False Then 'if already loaded something, unload it
            ComboBox1.Enabled = True
            GroupBox1.Enabled = False
        Else 'if nothing loaded, load something
            If Kairen.LoadAbilityFile(ComboBox1.Text) = 0 Then ' success
                ComboBox1.Enabled = False
                GroupBox1.Enabled = True
            Else 'unsuccess
            End If
        End If
    End Sub 'load button
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Kairen.SaveAbilityFile(lb.Folder_Custom_Abilities & TextBox9.Text & lb.Extension_AbilityFile)
    End Sub 'save button
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        ComboBox1.Enabled = False
        GroupBox1.Enabled = True
    End Sub 'new button
End Class