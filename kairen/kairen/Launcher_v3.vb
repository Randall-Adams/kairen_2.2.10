Public Class Launcher_v3
    Dim Version_Current_Launcher As String = "3.0.1"
    Public ReadOnly Version_Current_Release As String = "2.2.11"
    Dim Version_Current_Options As String = "1.1"
    'note what launcher version installed the install files, updated them, run successfully with them, and failed with them
    Public GUI_Controls As GUI_ControlsClass
    Private WithEvents GUI_Timer As New Timer
    Private Sub Launcher_v3_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Location = New Point(250, 50)
        Me.Text = Me.Text & " " & Version_Current_Launcher
        GUI_Controls = New GUI_ControlsClass(TextBox1)
        GUI_Timer.Interval = 1
        GUI_Timer.Start()

        GUI_Controls.OutputTextLine("Welcome to Kairen Launcher!", " -- ")
        GUI_Controls.OutputTextLine("[Release " & Version_Current_Release & "]", " -- ")
        GUI_Controls.OutputTextLine("The Current Launcher Version is: " & Version_Current_Launcher, " -- ")
        GUI_Controls.OutputTextLine("The Current Options File Version is: " & Version_Current_Options, " -- ")
        'GUI_Controls.OutputTextLine("Your Installation Folder is: \" & lb.Folder_EQOA.Replace(lb.IO_Root, ""), " -- ")


        '
        ''
        'CommonLibrary needs redone to incorporate text file reading, which needs multithreading incorporated.
        ''
        '


        'load cli argument data
        'load options file
        'check/load installation files

        'wait for cli argument data
        'wait for options file data
        'wait for install file data

        'act on cli arguments
        'act on options file

        'allow launch button and install button to be pressed if supposed to
    End Sub
    
    Private Sub GUI_Timer_Tick(sender As System.Object, e As System.EventArgs) Handles GUI_Timer.Tick
        'Update Progress Bars
        If ProgressBar1.Value >= ProgressBar1.Maximum Then
            ProgressBar1.Step = -1
            ProgressBar2.Step = -1
            If ProgressBar1.RightToLeftLayout = True Then
                ProgressBar1.RightToLeftLayout = False
                ProgressBar1.RightToLeft = Windows.Forms.RightToLeft.No
                ProgressBar2.RightToLeftLayout = True
                ProgressBar2.RightToLeft = Windows.Forms.RightToLeft.Yes
            ElseIf ProgressBar1.RightToLeftLayout = False Then
                ProgressBar1.RightToLeftLayout = True
                ProgressBar1.RightToLeft = Windows.Forms.RightToLeft.Yes
                ProgressBar2.RightToLeftLayout = False
                ProgressBar2.RightToLeft = Windows.Forms.RightToLeft.No
            End If
        ElseIf ProgressBar1.Value <= ProgressBar1.Minimum Then
            ProgressBar1.Step = 1
            ProgressBar2.Step = 1
        End If
        ProgressBar1.PerformStep()
        ProgressBar2.PerformStep()

        '
    End Sub
    Public Class GUI_ControlsClass
        Public ReadOnly OutputTextBox As TextBox
        Sub New(ByRef _outputTextBox As TextBox)
            OutputTextBox = _outputTextBox
        End Sub
        Sub OutputTextLine(ByVal _newTextLine As String, Optional ByVal _padding As String = Nothing)
            If _padding Is Nothing Then
                OutputTextBox.AppendText(_newTextLine & Environment.NewLine)
            Else
                OutputTextBox.AppendText(_padding & _newTextLine & _padding & Environment.NewLine)
            End If
        End Sub
    End Class

    Private Sub ProgressBar2_Click(sender As System.Object, e As System.EventArgs) Handles ProgressBar2.Click
        If ProgressBar2.Size.Width = 570 Then
            ProgressBar2.Size = New Point(285, 23)
            ProgressBar2.Location = New Point(297, 655)
        Else
            ProgressBar2.Size = New Point(570, 23)
            ProgressBar2.Location = New Point(12, 655)
        End If
    End Sub
End Class