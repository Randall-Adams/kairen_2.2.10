Public Class InstallationPrompt
    Public ReadOnly RDSYFolder As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\..\..\..\Rundatshityo\"
    Public InstallationList As New TextFileClass(RDSYFolder & "Kairen\Additional Installations.txt", "--")
    Public SendingObj As Object
    Public ActuallyClose As Boolean = False
    Private Sub InstallationPrompt_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(0, 0)
        InstallationList.LoadFile()
        If InstallationList.NumberOfLines > 0 Then
            If InstallationList.ReadLine() = "1.0" Then
                InstallationList.ReadLine()
                Do Until InstallationList.CurrentIndex + 1 >= InstallationList.NumberOfLines
                    Dim InstallationFolderPath As String = InstallationList.ReadLine()
                    InstallationList.ReadLine()
                    If InstallationFolderPath = "EQOA\" Then
                        ListBox1.Items.Add("NO HACKING HACKER")
                    Else
                        ListBox1.Items.Add(InstallationFolderPath)
                    End If
                Loop
            Else
                'error wrong file version
            End If
        Else
            LoadParentObject() 'no parameter loads the default installation folder
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            LoadParentObject(ListBox1.Text)
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        LoadParentObject()
    End Sub

    Private Sub InstallationPrompt_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = (ActuallyClose = False)
    End Sub

    Sub LoadParentObject(Optional ByVal AlternatePath As String = Nothing)
        SendingObj.Enabled = True
        SendingObj.LoadProcess(AlternatePath)
        Me.TopMost = False
    End Sub

    Private Sub InstallationPrompt_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        SendingObj.TopMost = True
        Me.TopMost = True
    End Sub

    Private Sub ListBox1_DoubleClick(sender As System.Object, e As System.EventArgs) Handles ListBox1.DoubleClick
        If sender.SelectedItem IsNot Nothing Then
            LoadParentObject(ListBox1.Text)
        End If
    End Sub

    Private Sub ListBox1_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles ListBox1.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            Button2.PerformClick()
        End If
    End Sub
End Class