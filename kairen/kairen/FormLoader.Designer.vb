<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormLoader
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
        Me.d_btn_OpenForm = New System.Windows.Forms.Button()
        Me.d_btn_CloseForm = New System.Windows.Forms.Button()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.d_btn_SingleKanizahUpdate = New System.Windows.Forms.Button()
        Me.d_cb_ConstantKanizahUpdates = New System.Windows.Forms.CheckBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button4 = New System.Windows.Forms.Button()
        Me.d_cb_KeepInToolTray = New System.Windows.Forms.CheckBox()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'd_btn_OpenForm
        '
        Me.d_btn_OpenForm.Location = New System.Drawing.Point(89, 367)
        Me.d_btn_OpenForm.Name = "d_btn_OpenForm"
        Me.d_btn_OpenForm.Size = New System.Drawing.Size(187, 23)
        Me.d_btn_OpenForm.TabIndex = 0
        Me.d_btn_OpenForm.Text = "Open Form"
        Me.d_btn_OpenForm.UseVisualStyleBackColor = True
        '
        'd_btn_CloseForm
        '
        Me.d_btn_CloseForm.Location = New System.Drawing.Point(89, 396)
        Me.d_btn_CloseForm.Name = "d_btn_CloseForm"
        Me.d_btn_CloseForm.Size = New System.Drawing.Size(187, 23)
        Me.d_btn_CloseForm.TabIndex = 2
        Me.d_btn_CloseForm.Text = "Close Form"
        Me.d_btn_CloseForm.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'd_btn_SingleKanizahUpdate
        '
        Me.d_btn_SingleKanizahUpdate.Location = New System.Drawing.Point(420, 301)
        Me.d_btn_SingleKanizahUpdate.Name = "d_btn_SingleKanizahUpdate"
        Me.d_btn_SingleKanizahUpdate.Size = New System.Drawing.Size(202, 23)
        Me.d_btn_SingleKanizahUpdate.TabIndex = 3
        Me.d_btn_SingleKanizahUpdate.Text = "Do Single Kanizah Update"
        Me.d_btn_SingleKanizahUpdate.UseVisualStyleBackColor = True
        '
        'd_cb_ConstantKanizahUpdates
        '
        Me.d_cb_ConstantKanizahUpdates.Appearance = System.Windows.Forms.Appearance.Button
        Me.d_cb_ConstantKanizahUpdates.AutoSize = True
        Me.d_cb_ConstantKanizahUpdates.Location = New System.Drawing.Point(420, 330)
        Me.d_cb_ConstantKanizahUpdates.Name = "d_cb_ConstantKanizahUpdates"
        Me.d_cb_ConstantKanizahUpdates.Size = New System.Drawing.Size(208, 27)
        Me.d_cb_ConstantKanizahUpdates.TabIndex = 4
        Me.d_cb_ConstantKanizahUpdates.Text = "Do Constant Kanizah Updates"
        Me.d_cb_ConstantKanizahUpdates.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Button4
        '
        Me.Button4.Location = New System.Drawing.Point(420, 36)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(202, 23)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "Close Entire Application"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'd_cb_KeepInToolTray
        '
        Me.d_cb_KeepInToolTray.AutoSize = True
        Me.d_cb_KeepInToolTray.Location = New System.Drawing.Point(479, 409)
        Me.d_cb_KeepInToolTray.Name = "d_cb_KeepInToolTray"
        Me.d_cb_KeepInToolTray.Size = New System.Drawing.Size(143, 21)
        Me.d_cb_KeepInToolTray.TabIndex = 6
        Me.d_cb_KeepInToolTray.Text = "Keep In Tool Tray"
        Me.d_cb_KeepInToolTray.UseVisualStyleBackColor = True
        '
        'Timer2
        '
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.Location = New System.Drawing.Point(89, 13)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(187, 344)
        Me.CheckedListBox1.TabIndex = 7
        '
        'Timer3
        '
        Me.Timer3.Interval = 973
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(479, 382)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(125, 21)
        Me.CheckBox1.TabIndex = 8
        Me.CheckBox1.Text = "Always On Top"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'FormLoader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 442)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.CheckedListBox1)
        Me.Controls.Add(Me.d_cb_KeepInToolTray)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.d_cb_ConstantKanizahUpdates)
        Me.Controls.Add(Me.d_btn_SingleKanizahUpdate)
        Me.Controls.Add(Me.d_btn_CloseForm)
        Me.Controls.Add(Me.d_btn_OpenForm)
        Me.Name = "FormLoader"
        Me.Text = "FormLoader"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents d_btn_OpenForm As System.Windows.Forms.Button
    Friend WithEvents d_btn_CloseForm As System.Windows.Forms.Button
    Friend WithEvents NotifyIcon1 As System.Windows.Forms.NotifyIcon
    Friend WithEvents d_btn_SingleKanizahUpdate As System.Windows.Forms.Button
    Friend WithEvents d_cb_ConstantKanizahUpdates As System.Windows.Forms.CheckBox
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents d_cb_KeepInToolTray As System.Windows.Forms.CheckBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents CheckedListBox1 As System.Windows.Forms.CheckedListBox
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
