Public Class FormLoader
    Private LoadDevSetup As Boolean = False
    'Global Variables
    Public lb As CommonLibrary
    Public FAPI_Connection As FAPIConnectionManager2 ' New FAPIConnectionManager2(lb)
    Public Kairen As Kairen2 ' New Kairen2(lb)
    Public CLA As CommandLineArgumentInterpreter
    Public ReleaseVersion As String
    Private vVersion_Current_Release As String
    Public Property Version_Current_Release() As String
        Get
            Return vVersion_Current_Release
        End Get
        Set(ByVal value As String)
            If vVersion_Current_Release = Nothing Then
                vVersion_Current_Release = value
            Else
                'error something tried to write the file version after it has already been set.
            End If
        End Set
    End Property
    'Options passed in from the Launcher
    Public AutoRevive As Boolean

    'Local Variables
    Private FormLoaderEnabled As Boolean = False
    Private vActuallyClose As Boolean = False
    Private vDontMinize As Boolean = False
    Private BallonIconShown As Integer = 0
    Private Timer3_ElapserTime As Integer = 0
    Public Property ActuallyClose() As Boolean
        Get
            OpenFormCheckStater()
            Return vActuallyClose
        End Get
        Set(value As Boolean)
            vActuallyClose = value
            If value = False Then
                vDontMinize = True
            End If
            OpenFormCheckStater()
        End Set
    End Property
    Public ReadOnly Property KeepProgramOpen As Boolean
        Get
            Return FormLoaderEnabled
        End Get
    End Property
    Public Property CloseFormLoaderToo(ByVal _passedInFormsClosingBoolean As Boolean) As Boolean
        Get
            If _passedInFormsClosingBoolean = True Then
                If CheckedListBox1.CheckedItems.Count - 1 = 0 And WindowState = FormWindowState.Minimized Then Return True Else Return False
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            _passedInFormsClosingBoolean = value
        End Set
    End Property
    Private Sub FormLoader_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb = Launcher_v2.lb
        If lb.FE(lb.Folder_Net_Streams & "o\FAPI Data2" & lb.Extension_ReadWrites) Then My.Computer.FileSystem.DeleteFile(lb.Folder_Net_Streams & "o\FAPI Data2" & lb.Extension_ReadWrites, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Kairen = New Kairen2(lb)
        FAPI_Connection = New FAPIConnectionManager2(lb)
        CLA = Launcher_v2.CLA
        Version_Current_Release = Launcher_v2.Version_Current_Release
        lb.PositionForm(Me, 375, 125)
        NotifyIcon1.Text = "Kairen Form Loader"
        NotifyIcon1.Icon = Me.Icon 'My.Resources._64x64
        d_cb_KeepInToolTray.Checked = True 'keeps the kairen icon in the tasktray
        If LoadDevSetup Then DevVersionSetup() Else VersionSetup()
        'Me.Text = Me.Text & " NFR"
    End Sub
    Private Sub VersionSetup()
        Dim ListOfForms(17) As String
        'ListOfForms(0) = "Kairen_Main"
        'ListOfForms(1) = "PopOut_YourData"
        'ListOfForms(2) = "PopOut_NPCData"
        'ListOfForms(3) = "Kaires"
        ListOfForms(4) = "NPCPopulator_InGame"
        'ListOfForms(5) = "TestForm"
        ListOfForms(6) = "UserMenu"
        'ListOfForms(7) = "UserMenu_v2"
        'ListOfForms(8) = "OnlineForm"
        ListOfForms(9) = "ItemMaker"
        ListOfForms(10) = "AbilityMaker"
        'ListOfForms(11) = "ProcessMaker"
        ListOfForms(12) = "CMMaker"
        'ListOfForms(13) = "QuestDesigner"
        ListOfForms(14) = "RecipeMaker"
        ListOfForms(15) = "ChatTest_v1"
        ListOfForms(16) = "ChatTest_v2"
        ListOfForms(17) = "KairesTestUI"

        For Each item In ListOfForms
            If item <> Nothing Then
                If CheckedListBox1.Items.Contains(item) = False Then
                    CheckedListBox1.Items.Add(item)
                End If
            End If
        Next

        Dim TurnOnFAPIOutput_Flag As Boolean = False 'this will only get turned on, not off
        If CLA.UserMenu_Flag = True Then
            Select Case Version_Current_Release
                Case "2.1.12", "2.1.13", "2.1.14", "2.1.15", "2.1.16", "2.1.17", "2.1.18", "2.1.19", "2.2.0", "2.2.-1", "2.2.1", "2.2.-2", "2.2.2", "2.2.3", "2.2.4", "2.2.5", "2.2.6", "2.2.7", "2.2.8", "2.2.9", "2.2.10"
                    FormOpenCloser("UserMenu", CheckState.Checked)
                Case Else
                    FormOpenCloser("UserMenu", CheckState.Checked)
            End Select
        End If
        If CLA.WorldPopulation_Flag = True Then
            Select Case Version_Current_Release
                Case "2.1.12", "2.1.13", "2.1.14", "2.1.15", "2.1.16", "2.1.17", "2.1.18", "2.1.19", "2.2.0", "2.2.-1", "2.2.1", "2.2.-2", "2.2.2", "2.2.3", "2.2.4", "2.2.5", "2.2.6", "2.2.7", "2.2.8", "2.2.9", "2.2.10"
                    FormOpenCloser("NPCPopulator_InGame", CheckState.Checked)
                Case Else
                    FormOpenCloser("NPCPopulator_InGame", CheckState.Checked)
            End Select
            d_cb_ConstantKanizahUpdates.Checked = TurnOnFAPIOutput_Flag 'turns on fapi output
        End If
        If CLA.DeveloperMenu_Flag = True Then
            Select Case Version_Current_Release
                Case "2.2.4", "2.2.5", "2.2.6", "2.2.7", "2.2.8", "2.2.9", "2.2.10"
                    FormLoaderEnabled = True
            End Select
        End If
    End Sub ' normal mode
    Private Sub DevVersionSetup()
        'FormLoaderEnabled = True
        CheckedListBox1.Items.Clear()
        Dim ListOfForms(17) As String
        ListOfForms(0) = "Kairen_Main"
        ListOfForms(1) = "PopOut_YourData"
        ListOfForms(2) = "PopOut_NPCData"
        ListOfForms(3) = "Kaires"
        ListOfForms(4) = "NPCPopulator_InGame"
        ListOfForms(5) = "TestForm"
        ListOfForms(6) = "UserMenu"
        ListOfForms(7) = "UserMenu_v2"
        ListOfForms(8) = "OnlineForm"
        ListOfForms(9) = "ItemMaker"
        ListOfForms(10) = "AbilityMaker"
        ListOfForms(11) = "ProcessMaker"
        ListOfForms(12) = "CMMaker"
        ListOfForms(13) = "QuestDesigner"
        ListOfForms(14) = "RecipeMaker"
        ListOfForms(15) = "ChatTest_v1"
        ListOfForms(16) = "ChatTest_v2"
        ListOfForms(17) = "KairesTestUI"
        For Each item In ListOfForms
            If item <> Nothing Then
                If CheckedListBox1.Items.Contains(item) = False Then
                    CheckedListBox1.Items.Add(item)
                End If
            End If
        Next

        If CLA.DeveloperMenu_Flag = True Then
            'Select Case Version_Current_Release
            'Case "2.2.4", "2.2.5", "2.5.6"
            FormLoaderEnabled = True
            'End Select
        End If

        'CheckedListBox1.SelectedItem = "Kairen_Main"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "PopOut_YourData"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "PopOut_NPCData"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "Kaires"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)
        'Kaires.TabControl1.SelectTab(1)
        'Kaires.TabControl1.SelectTab(2)

        'CheckedListBox1.SelectedItem = "NPCPopulator_InGame"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)
        ''TurnOnFAPIOutput_Flag = True

        'CheckedListBox1.SelectedItem = "TestForm"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        CheckedListBox1.SelectedItem = "UserMenu"
        CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "UserMenu_v2"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)
        'UserMenu_v2.TabControl1.SelectedTab = UserMenu_v2.TabPage3

        'CheckedListBox1.SelectedItem = "OnlineForm"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "ItemMaker"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)
        'ItemMaker.RadioButton49.Enabled = True

        'CheckedListBox1.SelectedItem = "AbilityMaker"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)
        'AbilityMaker.CloseFormLoaderToo = True

        'CheckedListBox1.SelectedItem = "EffectsMaker"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)
        'EffectsMaker.CloseFormLoaderToo = True

        'CheckedListBox1.SelectedItem = "CMMaker"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "QuestDesigner"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "RecipeMaker"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

        'CheckedListBox1.SelectedItem = "ChatTest_v1"
        'CheckedListBox1.SetItemCheckState(CheckedListBox1.SelectedIndex, CheckState.Checked)

    End Sub ' dev mode
    Public Sub TurnOffFAPIOutput()
        d_cb_ConstantKanizahUpdates.Checked = False 'turns off fapi output
    End Sub
    Public Sub TurnOnFAPIOutput()
        d_cb_ConstantKanizahUpdates.Checked = True 'turns on fapi output
    End Sub
    Private Property AlwaysOnTop() As Boolean
        Get
            Return CheckBox1.Checked
        End Get
        Set(value As Boolean)
            CheckBox1.Checked = value
            Me.TopMost = value
        End Set
    End Property

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_OpenForm.Click
        'open form
        FormOpenCloser(CheckedListBox1.Text, CheckState.Checked)
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_CloseForm.Click
        'close form
        FormOpenCloser(CheckedListBox1.Text, CheckState.Unchecked)
    End Sub

    Private Sub FormLoader_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If ActuallyClose = False Then
            e.Cancel = True
        Else
            If lb.IO_RootName = "NO HACKING HACKER" Then
                MsgBox("Oh, I fookin' died!", MsgBoxStyle.Critical, "Sheeittte!!")
            End If
            Exit Sub
        End If
        If vDontMinize = False Then
            Me.WindowState = FormWindowState.Minimized
            System.Threading.Thread.Sleep(137)
            Me.Visible = False
            NotifyIcon1.Icon = Me.Icon 'My.Resources._64x64
            NotifyIcon1.Visible = True
        Else
            vDontMinize = False
        End If
    End Sub

    Private Sub NotifyIcon1_Click(sender As System.Object, e As System.EventArgs) Handles NotifyIcon1.Click
        If NPCPopulator_InGame IsNot Nothing Then
            NPCPopulator_InGame.TopMost = True
            NPCPopulator_InGame.TopMost = False
        End If
        If FormLoaderEnabled = False Then Exit Sub
        NotifyIcon1.Visible = d_cb_KeepInToolTray.Checked
        Me.Visible = True
        Me.TopMost = True
        Me.TopMost = False
        Me.TopMost = AlwaysOnTop

        System.Threading.Thread.Sleep(137)
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles d_btn_SingleKanizahUpdate.Click
        FAPI_Connection.UpdateFAPIDisplayObjects()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles d_cb_ConstantKanizahUpdates.CheckedChanged
        Timer1.Enabled = sender.checked
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If FAPI_Connection.ObjectsToUpdate.Count = 0 Then
            d_cb_ConstantKanizahUpdates.Checked = False
        Else
            FAPI_Connection.UpdateFAPIDisplayObjects()
        End If
    End Sub

    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs) Handles Button4.Click
        Me.ActuallyClose = True
        Application.Exit()
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles d_cb_KeepInToolTray.CheckedChanged
        NotifyIcon1.Visible = d_cb_KeepInToolTray.Checked
    End Sub

    Private Sub NotifyIcon1_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles NotifyIcon1.MouseMove
        If BallonIconShown <> 0 Then Exit Sub
        BallonIconShown = 4500
        NotifyIcon1.BalloonTipText = lb.IO_EQOA '.Replace("Microsoft\Kairen\1.0.0.0\..\..\..\", "")
        NotifyIcon1.BalloonTipTitle = "Installation Folder:"
        NotifyIcon1.ShowBalloonTip(BallonIconShown)
        Timer2.Interval = BallonIconShown
        Timer2.Start()
    End Sub

    Private Sub Timer2_Tick(sender As System.Object, e As System.EventArgs) Handles Timer2.Tick
        BallonIconShown = BallonIconShown - sender.Interval
        If BallonIconShown = 0 Then sender.Stop()
    End Sub

    Private Sub SetItemCheckStateInCheckedListBox(ByVal _item As String, ByVal _control As CheckedListBox, ByVal _checkstate As CheckState)
        'the original of this is in itemmaker, and needs moved into commonlibrary
        For i = 0 To _control.Items.Count
            If _control.Items.Item(i).ToString = _item Then
                _control.SetItemCheckState(i, _checkstate)
                Exit Sub
            End If
        Next
    End Sub

    Private Sub CheckedListBox1_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox1.ItemCheck
        If e.CurrentValue = CheckState.Indeterminate Then
            e.NewValue = CheckState.Indeterminate
        Else
            FormOpenCloser(CheckedListBox1.Items.Item(e.Index), e.NewValue, True)
        End If
    End Sub

    Private Sub FormOpenCloser(ByVal _formName As String, ByVal _opencloseoption As CheckState, Optional DoNotUpDateCheckBox As Boolean = False)
        If _formName <> "" Then
            If _opencloseoption = CheckState.Checked Then
                Select Case _formName
                    Case "Kairen_Main"
                        Kairen_Main.Show()
                        Kairen_Main.TopMost = True
                        Kairen_Main.TopMost = False
                    Case "PopOut_YourData"
                        PopOut_YourData.Show()
                        PopOut_YourData.TopMost = True
                        PopOut_YourData.TopMost = False
                    Case "PopOut_NPCData"
                        PopOut_NPCData.Show()
                        PopOut_NPCData.TopMost = True
                        PopOut_NPCData.TopMost = False
                    Case "Kaires"
                        Kaires.Show()
                        Kaires.TopMost = True
                        Kaires.TopMost = False
                    Case "NPCPopulator_InGame"
                        NPCPopulator_InGame.Show()
                        NPCPopulator_InGame.TopMost = True
                        NPCPopulator_InGame.TopMost = False
                    Case "TestForm"
                        TestForm.Show()
                        TestForm.TopMost = True
                        TestForm.TopMost = False
                    Case "UserMenu"
                        UserMenu.Show()
                        UserMenu.TopMost = True
                        UserMenu.TopMost = False
                        TurnOnFAPIOutput()
                    Case "UserMenu_v2"
                        UserMenu_v2.Show()
                        UserMenu_v2.TopMost = True
                        UserMenu_v2.TopMost = False
                        TurnOnFAPIOutput()
                    Case "OnlineForm"
                        OnlineForm.Show()
                        OnlineForm.TopMost = True
                        OnlineForm.TopMost = False
                    Case "ItemMaker"
                        ItemMaker.Show()
                        ItemMaker.TopMost = True
                        ItemMaker.TopMost = False
                    Case "AbilityMaker"
                        AbilityMaker.Show()
                        AbilityMaker.TopMost = True
                        AbilityMaker.TopMost = False
                    Case "ProcessMaker"
                        ProcessMaker.Show()
                        ProcessMaker.TopMost = True
                        ProcessMaker.TopMost = False
                    Case "CMMaker"
                        CMMaker.Show()
                        CMMaker.TopMost = True
                        CMMaker.TopMost = False
                    Case "QuestDesigner"
                        QuestDesigner.Show()
                        QuestDesigner.TopMost = True
                        QuestDesigner.TopMost = False
                    Case "RecipeMaker"
                        RecipeMaker.Show()
                        RecipeMaker.TopMost = True
                        RecipeMaker.TopMost = False
                    Case "ChatTest_v1"
                        ChatTest_v1.Show()
                        ChatTest_v1.TopMost = True
                        ChatTest_v1.TopMost = False
                    Case "ChatTest_v2"
                        ChatTest_v2.Show()
                        ChatTest_v2.TopMost = True
                        ChatTest_v2.TopMost = False
                    Case "KairesTestUI"
                        KairesTestUI.Show()
                        KairesTestUI.TopMost = True
                        KairesTestUI.TopMost = False

                End Select
            ElseIf _opencloseoption = CheckState.Unchecked Then
                Select Case _formName
                    Case "Kairen_Main"
                        If Kairen_Main Is Nothing Then Exit Sub
                        lb.DisplayMessage("Closing Kairen_Main in this way is not an option as this time.", "Alert:", "FormLoader")
                    Case "PopOut_YourData"
                        If PopOut_YourData Is Nothing Then Exit Sub
                        PopOut_YourData.CloseFormLoaderToo = False
                        PopOut_YourData.Close()
                    Case "PopOut_NPCData"
                        If PopOut_NPCData Is Nothing Then Exit Sub
                        PopOut_NPCData.CloseFormLoaderToo = False
                        PopOut_NPCData.Close()
                    Case "Kaires"
                        If Kaires Is Nothing Then Exit Sub
                        Kaires.CloseFormLoaderToo = False
                        Kaires.Close()
                    Case "NPCPopulator_InGame"
                        'lb.DisplayMessage("Closing NPCPopulator_InGame in this way is not an option as this time.", "Alert:", "FormLoader")
                        If NPCPopulator_InGame Is Nothing Then Exit Sub
                        NPCPopulator_InGame.CloseFormLoaderToo = False
                        NPCPopulator_InGame.Close()
                    Case "TestForm"
                        If TestForm Is Nothing Then Exit Sub
                        TestForm.CloseFormLoaderToo = False
                        TestForm.Close()
                    Case "UserMenu"
                        If UserMenu Is Nothing Then Exit Sub
                        CloseFormLoaderToo(UserMenu.CloseFormLoaderToo) = False
                        'UserMenu.CloseFormLoaderToo = False
                        UserMenu.Close()
                    Case "UserMenu_v2"
                        If UserMenu_v2 Is Nothing Then Exit Sub
                        UserMenu_v2.CloseFormLoaderToo = False
                        UserMenu_v2.Close()
                    Case "OnlineForm"
                        If OnlineForm Is Nothing Then Exit Sub
                        OnlineForm.CloseFormLoaderToo = False
                        OnlineForm.Close()
                    Case "ItemMaker"
                        If ItemMaker Is Nothing Then Exit Sub
                        'ItemMaker.CloseFormLoaderToo = False
                        ItemMaker.Close()
                    Case "AbilityMaker"
                        If AbilityMaker Is Nothing Then Exit Sub
                        'AbilityMaker.CloseFormLoaderToo = False
                        AbilityMaker.Close()
                    Case "ProcessMaker"
                        If ProcessMaker Is Nothing Then Exit Sub
                        'EffectsMaker.CloseFormLoaderToo = False
                        ProcessMaker.Close()
                    Case "CMMaker"
                        If CMMaker Is Nothing Then Exit Sub
                        CMMaker.Close()
                    Case "QuestDesigner"
                        If QuestDesigner Is Nothing Then Exit Sub
                        QuestDesigner.Close()
                    Case "RecipeMaker"
                        If RecipeMaker Is Nothing Then Exit Sub
                        RecipeMaker.Close()
                    Case "ChatTest_v1"
                        If ChatTest_v1 Is Nothing Then Exit Sub
                        ChatTest_v1.Close()
                    Case "ChatTest_v2"
                        If ChatTest_v2 Is Nothing Then Exit Sub
                        ChatTest_v2.Close()
                    Case "KairesTestUI"
                        If KairesTestUI Is Nothing Then Exit Sub
                        KairesTestUI.Close()
                End Select
            End If
            'MsgBox(CheckedListBox1.Items.IndexOf(_formName))
            'MsgBox(CheckedListBox1.Items.Item((CheckedListBox1.Items.IndexOf(_formName))))
            If DoNotUpDateCheckBox = False Then
                SetItemCheckStateInCheckedListBox(CheckedListBox1.Items.Item((CheckedListBox1.Items.IndexOf(_formName))), CheckedListBox1, _opencloseoption)
            End If
        End If
        If FAPI_Connection.ObjectsToUpdate.Count > 0 Then
            d_cb_ConstantKanizahUpdates.Checked = True
        End If
    End Sub
    Private Sub OpenFormCheckStater()
        Timer3.Start()
    End Sub
    Private Sub Timer3_Tick(sender As System.Object, e As System.EventArgs) Handles Timer3.Tick
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            Select Case CheckedListBox1.Items.Item(i)
                Case "Kairen_Main"
                    If Application.OpenForms().OfType(Of Kairen_Main).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "PopOut_YourData"
                    If Application.OpenForms().OfType(Of PopOut_YourData).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "PopOut_NPCData"
                    If Application.OpenForms().OfType(Of PopOut_NPCData).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "Kaires"
                    If Application.OpenForms().OfType(Of Kaires).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "NPCPopulator_InGame"
                    If Application.OpenForms().OfType(Of NPCPopulator_InGame).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "TestForm"
                    If Application.OpenForms().OfType(Of TestForm).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "UserMenu"
                    If Application.OpenForms().OfType(Of UserMenu).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "UserMenu_v2"
                    If Application.OpenForms().OfType(Of UserMenu_v2).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "OnlineForm"
                    If Application.OpenForms().OfType(Of OnlineForm).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "ItemMaker"
                    If Application.OpenForms().OfType(Of ItemMaker).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "AbilityMaker"
                    If Application.OpenForms().OfType(Of AbilityMaker).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "ProcessMaker"
                    If Application.OpenForms().OfType(Of ProcessMaker).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "CMMaker"
                    If Application.OpenForms().OfType(Of CMMaker).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "QuestDesigner"
                    If Application.OpenForms().OfType(Of QuestDesigner).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "RecipeMaker"
                    If Application.OpenForms().OfType(Of RecipeMaker).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "ChatTest_v1"
                    If Application.OpenForms().OfType(Of ChatTest_v1).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked)
                Case "ChatTest_v2"
                    If Application.OpenForms().OfType(Of ChatTest_v2).Any = False Then CheckedListBox1.SetItemCheckState(i, CheckState.Unchecked) Else CheckedListBox1.SetItemCheckState(i, CheckState.Checked)
            End Select
        Next
        If FAPI_Connection.ObjectsToUpdate.Count > 0 Then
            d_cb_ConstantKanizahUpdates.Checked = True
        Else
            d_cb_ConstantKanizahUpdates.Checked = False
        End If

        Timer3_ElapserTime += sender.Interval
        If Timer3_ElapserTime >= sender.Interval * 7 Then
            If Me.WindowState = FormWindowState.Minimized And CheckedListBox1.CheckedItems.Count = 0 Then
                Me.ActuallyClose = True
                Application.Exit()
            End If
            Timer3.Stop()
        End If
    End Sub

    Private Sub d_cb_ConstantKanizahUpdates_MouseClick(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles d_cb_ConstantKanizahUpdates.MouseClick
        If sender.Checked = True Then
            FAPI_Connection.ObjectsToUpdate.Add(Me)
        Else
            FAPI_Connection.ObjectsToUpdate.Remove(Me)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        AlwaysOnTop = sender.Checked
    End Sub
End Class