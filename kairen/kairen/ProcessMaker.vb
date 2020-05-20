Public Class ProcessMaker
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    Private Kairen As Kairen2 = FormLoader.Kairen


    Private Sub ProcessMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If lb.DE(lb.Folder_Custom_Procs) = False Then lb.CD(lb.Folder_Custom_Procs)
        lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Procs, lb.Extension_ProcFile), ComboBox1, True)
        GroupBox1.Enabled = False
        Kairen.ReinstantiateProcFile()
        Kairen.ProcFile.RegisterUIElement("nameSafe", TextBox1, AddressOf DataParserFunction)
        Kairen.ProcFile.RegisterUIElement("Description", TextBox3, AddressOf DataParserFunction)
        Me.Text = Me.Text & " In-Build Preview - Kairen  " & Launcher_v2.Version_Current_Release
    End Sub
    Private Sub ProcessMaker_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
        'FAPI_Connection.ObjectsToUpdate.Remove(Me)
        Application.Exit()
    End Sub
    Public Function DataParserFunction(ByVal _dataTag As String, ByRef _dataControl As Object, ByRef _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        '' if control is control then 
        ''    do control-specfic stuff here
        ''    return
        '' else
        'pass args onto global data parser function
        Return Kairen.DataParserFunction(_dataTag, _dataControl, _rawData)
        '' end if
    End Function
    Private Sub CheckBox1_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles CheckBox1.MouseDown
        If ComboBox1.Enabled = True Then 'if something's not loaded..
            'load something now
            If Kairen.LoadProcFile(ComboBox1.Text) = 0 Then
                ComboBox1.Enabled = False 'reverse switch
                GroupBox1.Enabled = True
                CheckBox1.Checked = True
                CheckBox2.Enabled = False
            End If
        Else
            'unload something now
            ComboBox1.Enabled = True  'reverse switch
            GroupBox1.Enabled = False
            CheckBox1.Checked = False
            CheckBox2.Enabled = True
        End If
    End Sub 'load button
    Private Sub CheckBox2_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles CheckBox2.MouseDown
        If ComboBox1.Enabled = True Then 'if something's not loaded..
            'let's let a new file be made
            ComboBox1.Enabled = False
            GroupBox1.Enabled = True
            CheckBox2.Checked = True
            CheckBox1.Enabled = False
        Else
            'let's actually not make a new file!
            ComboBox1.Enabled = True
            GroupBox1.Enabled = False
            CheckBox2.Checked = False
            CheckBox1.Enabled = True
        End If
    End Sub 'new button
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Kairen.SaveProcFile(lb.Folder_Custom_Procs & TextBox1.Text & lb.Extension_ProcFile)
    End Sub 'save button
End Class