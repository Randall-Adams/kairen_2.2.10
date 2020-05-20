Imports System.IO
Imports System.IO.Compression
Public Class TestForm
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb

    Private Sub TestForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 0, 525)
        Me.TopMost = True
        If lb.FE(lb.Folder_Custom_Data & "NPC_Files.zip") Then My.Computer.FileSystem.DeleteFile(lb.Folder_Custom_Data & "NPC_Files.zip")
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim filepath As String = lb.Folder_Custom_Data & "NPC_Files.zip"
        IO.File.WriteAllBytes(filepath, My.Resources.NPC_Files)
        Dim startPath As String = "c:\example\start.zip"
        Dim zipPath As String = "c:\example\start.zip"
        Dim extractPath As String = "c:\example\extract"

        'System.IO.Compression.ZipFile.CreateFromDirectory(startPath, zipPath)
        'System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath)
    End Sub

    Private Sub TestForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If lb.FE(lb.Folder_Custom_Data & "NPC_Files.zip") Then My.Computer.FileSystem.DeleteFile(lb.Folder_Custom_Data & "NPC_Files.zip")
        'FormLoader.ActuallyClose() = CloseFormLoaderToo
        'Application.Exit()
    End Sub

End Class