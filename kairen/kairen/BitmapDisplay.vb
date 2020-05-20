Public Class BitmapDisplay
    Dim lb As CommonLibrary = FormLoader.lb
    Private Sub BitmapDisplay_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.TopMost = True
        lb.PositionForm(Me, 100, 0)
        'PictureBox1.Image = My.Resources.OmegaAlert
    End Sub
End Class