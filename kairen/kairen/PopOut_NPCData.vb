Public Class PopOut_NPCData
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection

    Private Sub PopOut_NPCData_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 777, 500)
        FAPI_Connection.ObjectsToUpdate.Add(Me)
    End Sub

    Private Sub PopOut_NPCData_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.FAPI_Connection.ObjectsToUpdate.Remove(Me)
        lb.DisplayMessage("Closing " & Me.Text, "Alert:", Me.Text)
        FormLoader.ActuallyClose = CloseFormLoaderToo
        'Application.Exit()
    End Sub

    Public Sub UpdateFAPIDisplay()
        Dim nudvalue As Integer = NumericUpDown1.Value
        TextBox1.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("NPC" & nudvalue & "Name")
        TextBox2.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("NPC" & nudvalue & "X")
        TextBox3.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("NPC" & nudvalue & "Y")
        TextBox4.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("NPC" & nudvalue & "Z")
        TextBox5.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("NPC" & nudvalue & "F")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Me.TopMost = sender.checked
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        If lb.FE(lb.Folder_EQOA & "LUAs\Modules\FAPI\Classes\NPC.lua") Then
            Process.Start(lb.Folder_EQOA & "LUAs\Modules\FAPI\Classes\NPC.lua")
        End If
    End Sub
End Class