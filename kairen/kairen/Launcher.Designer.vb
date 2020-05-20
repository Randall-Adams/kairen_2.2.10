<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Launcher
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Launcher))
        Me.d_pb_Progress = New System.Windows.Forms.ProgressBar()
        Me.d_lbl_CurrentAction = New System.Windows.Forms.Label()
        Me.d_tb_Output = New System.Windows.Forms.TextBox()
        Me.d_btn_Launch = New System.Windows.Forms.Button()
        Me.d_chkbx_ToggleOptions = New System.Windows.Forms.CheckedListBox()
        Me.d_btn_DeleteAndClose = New System.Windows.Forms.Button()
        Me.d_btn_InstallLatestVersion = New System.Windows.Forms.Button()
        Me.d_btn_InstallMissingFiles = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.d_lbl_Subtle1 = New System.Windows.Forms.Label()
        Me.d_lbl_Subtle2 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'd_pb_Progress
        '
        Me.d_pb_Progress.Location = New System.Drawing.Point(97, 30)
        Me.d_pb_Progress.Name = "d_pb_Progress"
        Me.d_pb_Progress.Size = New System.Drawing.Size(365, 23)
        Me.d_pb_Progress.TabIndex = 0
        '
        'd_lbl_CurrentAction
        '
        Me.d_lbl_CurrentAction.AutoSize = True
        Me.d_lbl_CurrentAction.Location = New System.Drawing.Point(97, 11)
        Me.d_lbl_CurrentAction.Name = "d_lbl_CurrentAction"
        Me.d_lbl_CurrentAction.Size = New System.Drawing.Size(0, 13)
        Me.d_lbl_CurrentAction.TabIndex = 1
        '
        'd_tb_Output
        '
        Me.d_tb_Output.Location = New System.Drawing.Point(97, 59)
        Me.d_tb_Output.Multiline = True
        Me.d_tb_Output.Name = "d_tb_Output"
        Me.d_tb_Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.d_tb_Output.Size = New System.Drawing.Size(362, 127)
        Me.d_tb_Output.TabIndex = 2
        '
        'd_btn_Launch
        '
        Me.d_btn_Launch.Enabled = False
        Me.d_btn_Launch.Location = New System.Drawing.Point(100, 292)
        Me.d_btn_Launch.Name = "d_btn_Launch"
        Me.d_btn_Launch.Size = New System.Drawing.Size(362, 72)
        Me.d_btn_Launch.TabIndex = 3
        Me.d_btn_Launch.Text = "Launch"
        Me.d_btn_Launch.UseVisualStyleBackColor = True
        '
        'd_chkbx_ToggleOptions
        '
        Me.d_chkbx_ToggleOptions.FormattingEnabled = True
        Me.d_chkbx_ToggleOptions.Location = New System.Drawing.Point(100, 192)
        Me.d_chkbx_ToggleOptions.Name = "d_chkbx_ToggleOptions"
        Me.d_chkbx_ToggleOptions.Size = New System.Drawing.Size(362, 94)
        Me.d_chkbx_ToggleOptions.TabIndex = 4
        '
        'd_btn_DeleteAndClose
        '
        Me.d_btn_DeleteAndClose.Location = New System.Drawing.Point(556, 370)
        Me.d_btn_DeleteAndClose.Name = "d_btn_DeleteAndClose"
        Me.d_btn_DeleteAndClose.Size = New System.Drawing.Size(163, 23)
        Me.d_btn_DeleteAndClose.TabIndex = 5
        Me.d_btn_DeleteAndClose.Text = "Delete Options File and Close"
        Me.d_btn_DeleteAndClose.UseVisualStyleBackColor = True
        '
        'd_btn_InstallLatestVersion
        '
        Me.d_btn_InstallLatestVersion.Location = New System.Drawing.Point(100, 370)
        Me.d_btn_InstallLatestVersion.Name = "d_btn_InstallLatestVersion"
        Me.d_btn_InstallLatestVersion.Size = New System.Drawing.Size(140, 23)
        Me.d_btn_InstallLatestVersion.TabIndex = 6
        Me.d_btn_InstallLatestVersion.Text = "Install Latest Version"
        Me.d_btn_InstallLatestVersion.UseVisualStyleBackColor = True
        '
        'd_btn_InstallMissingFiles
        '
        Me.d_btn_InstallMissingFiles.Location = New System.Drawing.Point(246, 370)
        Me.d_btn_InstallMissingFiles.Name = "d_btn_InstallMissingFiles"
        Me.d_btn_InstallMissingFiles.Size = New System.Drawing.Size(127, 23)
        Me.d_btn_InstallMissingFiles.TabIndex = 8
        Me.d_btn_InstallMissingFiles.Text = "Install Missing Files"
        Me.d_btn_InstallMissingFiles.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'd_lbl_Subtle1
        '
        Me.d_lbl_Subtle1.AutoSize = True
        Me.d_lbl_Subtle1.Location = New System.Drawing.Point(13, 419)
        Me.d_lbl_Subtle1.Name = "d_lbl_Subtle1"
        Me.d_lbl_Subtle1.Size = New System.Drawing.Size(1194, 13)
        Me.d_lbl_Subtle1.TabIndex = 9
        Me.d_lbl_Subtle1.Text = resources.GetString("d_lbl_Subtle1.Text")
        '
        'd_lbl_Subtle2
        '
        Me.d_lbl_Subtle2.AutoSize = True
        Me.d_lbl_Subtle2.Location = New System.Drawing.Point(13, 438)
        Me.d_lbl_Subtle2.Name = "d_lbl_Subtle2"
        Me.d_lbl_Subtle2.Size = New System.Drawing.Size(1058, 13)
        Me.d_lbl_Subtle2.TabIndex = 10
        Me.d_lbl_Subtle2.Text = resources.GetString("d_lbl_Subtle2.Text")
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(523, 101)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(164, 58)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "Delete EQOA folder"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Launcher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(731, 405)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.d_lbl_Subtle2)
        Me.Controls.Add(Me.d_lbl_Subtle1)
        Me.Controls.Add(Me.d_btn_InstallMissingFiles)
        Me.Controls.Add(Me.d_btn_InstallLatestVersion)
        Me.Controls.Add(Me.d_btn_DeleteAndClose)
        Me.Controls.Add(Me.d_chkbx_ToggleOptions)
        Me.Controls.Add(Me.d_btn_Launch)
        Me.Controls.Add(Me.d_tb_Output)
        Me.Controls.Add(Me.d_lbl_CurrentAction)
        Me.Controls.Add(Me.d_pb_Progress)
        Me.Name = "Launcher"
        Me.Text = "Kairen - Launcher"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents d_pb_Progress As System.Windows.Forms.ProgressBar
    Friend WithEvents d_lbl_CurrentAction As System.Windows.Forms.Label
    Friend WithEvents d_tb_Output As System.Windows.Forms.TextBox
    Friend WithEvents d_btn_Launch As System.Windows.Forms.Button
    Friend WithEvents d_chkbx_ToggleOptions As System.Windows.Forms.CheckedListBox
    Friend WithEvents d_btn_DeleteAndClose As System.Windows.Forms.Button
    Friend WithEvents d_btn_InstallLatestVersion As System.Windows.Forms.Button
    Friend WithEvents d_btn_InstallMissingFiles As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents d_lbl_Subtle1 As System.Windows.Forms.Label
    Friend WithEvents d_lbl_Subtle2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
