Public Class PopOut_YourData
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection
    Private Kairen As Kairen2 = FormLoader.Kairen
    Private Sub PopOut_YourData_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 515, 460)
        FAPI_Connection.ObjectsToUpdate.Add(Me)
    End Sub

    Private Sub PopOut_YourData_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.FAPI_Connection.ObjectsToUpdate.Remove(Me)
        lb.DisplayMessage("Closing " & Me.Text, "Alert:", Me.Text)
        'FormLoader.ActuallyClose = CloseFormLoaderToo
        'Application.Exit()
    End Sub

    Public Sub UpdateFAPIDisplay()
        'lb.DisplayMessage("FAPI Display Updated: " & Me.Text, "Notification:", Me.Text)
        d_yd_tb_X.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyX")
        d_yd_tb_Y.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyY")
        d_yd_tb_Z.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyZ")
        d_yd_tb_F.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyF")
        TextBox10.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyColumn")
        TextBox14.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyRow")
        TextBox11.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyNestX")
        TextBox15.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyNestY")
        TextBox5.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyZone")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Me.TopMost = sender.checked
    End Sub
End Class