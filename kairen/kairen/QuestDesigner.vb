Public Class QuestDesigner
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb

    Private Sub QuestDesigner_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " In-Build Preview - Kairen  " & Launcher_v2.Version_Current_Release
    End Sub

    Private Sub QuestDesigner_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
        Application.Exit()
    End Sub
End Class