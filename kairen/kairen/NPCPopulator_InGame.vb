Public Class NPCPopulator_InGame
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection
    Private Kairen As Kairen2 = FormLoader.Kairen
    Private CurrentMode As String = ""
    Dim OldNestValues(1) As String
    Dim PreventPrematureNestChangeAlert As Boolean = True

    Private Sub NPCPopulator_InGame_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        lb.PositionForm(Me, 0, 525)
        FAPI_Connection.ObjectsToUpdate.Add(Me)
        'Button6.PerformClick()
        'TextBox17.Text = 4500
        'TextBox16.Text = 4500
    End Sub
    Sub DevelopementHelperSub()
        TextBox17.Text = 4500
        TextBox16.Text = 4500
        CheckBox1.Checked = True
        CheckBox2.Checked = True
        CheckBox6.Checked = True
    End Sub

    Private Sub Button9_Click(sender As System.Object, e As System.EventArgs) Handles Button9.Click
        DevelopementHelperSub()
    End Sub

    Private Sub NPCPopulator_InGame_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        FormLoader.FAPI_Connection.ObjectsToUpdate.Remove(Me)
        lb.DisplayMessage("Closing " & Me.Text, "Alert: ", Me.Text)
        FormLoader.ActuallyClose() = CloseFormLoaderToo
        Application.Exit()
    End Sub

    Public Sub UpdateFAPIDisplay()
        Label29.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyNestX")
        Label30.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyNestY")
        Dim nestchanged As Boolean
        If OldNestValues(0) <> Label29.Text Then nestchanged = True
        If OldNestValues(1) <> Label30.Text Then nestchanged = True
        OldNestValues(0) = Label29.Text
        OldNestValues(1) = Label30.Text
        If PreventPrematureNestChangeAlert = True And nestchanged Then
            PreventPrematureNestChangeAlert = False
            Exit Sub
        End If
        If nestchanged = True Then
            lb.DisplayMessage("You have changed nests.", "Developer Alert:", "Developer Alert")
            'BitmapDisplay.Show()
        End If
        'add check box 12 here
    End Sub

    Private Sub CheckBox9_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox9.CheckedChanged
        Me.TopMost = sender.checked
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If TextBox16.Text = "" Or TextBox17.Text = "" Then
            sender.checked = False
            Exit Sub
        End If
        If sender.checked = True Then ModeHandler("EditNestOn") Else ModeHandler("EditNestOff")
    End Sub

    Private Sub ModeHandler(ByVal mode As String)
        Select Case mode
            Case "EditNestOn"
                CurrentMode = "EditNestOn"
                ComboBox1.Enabled = True
                CheckBox1.ForeColor = Color.Orange
                If ComboBox1.Text = "" Then
                    Button5.ForeColor = Color.Green
                    CheckBox2.ForeColor = Color.Red
                Else
                    CheckBox2.ForeColor = Color.Green
                    CheckBox2.Enabled = True
                End If
                If lb.FE(lb.Folder_Custom_SpawnNests & TextBox17.Text & "\" & TextBox16.Text & lb.Extension_ReadWrites) = False Then
                    lb.DisplayMessage("Creating Nest (" & TextBox17.Text & "\" & TextBox16.Text & ")", "Creating Spawn Nest", Me.Text)
                    Kairen.CreateNewNestFile(TextBox17.Text, TextBox16.Text)
                End If
                Kairen.LoadSpawnNestFile(TextBox17.Text, TextBox16.Text)
                ComboBox1.SelectedItem = Nothing
                ComboBox1.Items.Clear()
                lb.AddItemsToControl(Kairen.GetSpawnCampsFromSpawnNestFile(TextBox17.Text, TextBox16.Text), ComboBox1)
                If ComboBox1.Items.Count > 0 Then ComboBox1.SelectedIndex = 0
                TextBox9.Enabled = True
                TextBox8.Enabled = True
                Button5.Enabled = True
                Button5.ForeColor = Color.Green
                Button3.Enabled = True
            Case "EditNestOff"
                CurrentMode = ""
                CheckBox1.ForeColor = Color.Green
                ComboBox1.Enabled = False
                CheckBox2.Enabled = False
                CheckBox2.ForeColor = Color.Black
                TextBox9.Enabled = False
                TextBox9.Text = ""
                TextBox8.Enabled = False
                TextBox8.Text = ""
                Button5.Enabled = False
                Button5.ForeColor = Color.Black
                Button3.Enabled = False
                TextBox17.Text = Label29.Text
                TextBox16.Text = Label30.Text

            Case "EditCampOn"
                CurrentMode = "EditCampOn"
                If Kairen.LoadSpawnCampFile(ComboBox1.Text) = False Then Exit Sub
                CheckBox2.ForeColor = Color.Orange
                CheckBox1.Enabled = False
                CheckBox6.Enabled = True
                CheckBox6.ForeColor = Color.Green
                'CheckBox3.Enabled = True
                'CheckBox3.ForeColor = Color.Green
                ComboBox1.Enabled = False
                'ComboBox2.Enabled = True
                TextBox9.Enabled = False
                TextBox9.Text = ""
                TextBox8.Enabled = False
                TextBox8.Text = ""
                Button5.Enabled = False
                Button5.ForeColor = Color.Black
                Button3.Enabled = False
                Label27.Text = "Spawn Points in this Camp:"
                lb.AddItemsToControl(Kairen.GetSpawnPointsFromSpawnCampFile(), ListBox1)
                ListBox1.Enabled = True
                Button10.Text = "Delete This Spawn Point"
                Button10.Enabled = True
            Case "EditCampOff"
                CurrentMode = "EditNestOn"
                CheckBox2.ForeColor = Color.Green
                CheckBox1.Enabled = True
                CheckBox6.Enabled = False
                CheckBox3.Enabled = False
                ComboBox1.Enabled = True
                ComboBox2.Enabled = False
                TextBox9.Enabled = True
                TextBox8.Enabled = True
                Button5.Enabled = True
                Button5.ForeColor = Color.Green
                Button3.Enabled = True
                Label27.Text = "--"
                ListBox1.Items.Clear()
                ListBox1.Enabled = False
                Button10.Text = "--"
                Button10.Enabled = False

            Case "EditPointNormalOn"
                CurrentMode = "EditPointNormalOn"
                CheckBox2.Enabled = False
                CheckBox6.Enabled = False
                ComboBox2.Enabled = False
            Case "EditPointNormalOff"
                CurrentMode = "EditCampOn"
                CheckBox2.Enabled = True
                CheckBox6.Enabled = True
                'ComboBox2.Enabled = True
                Label27.Text = "Spawn Points in this Camp:"
                lb.AddItemsToControl(Kairen.GetSpawnPointsFromSpawnCampFile(), ListBox1)
                ListBox1.Enabled = True
                Button10.Text = "Delete This Spawn Point"
                Button10.Enabled = True

            Case "EditPointUniqueOn"
                CurrentMode = "EditPointUniqueOn"
                CheckBox2.Enabled = False
                CheckBox2.ForeColor = Color.Black
                CheckBox6.ForeColor = Color.Orange
                CheckBox3.Enabled = False
                ComboBox2.Enabled = False
                Label27.Text = "NPCs You May Add:"
                lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_NPCs, lb.Extension_NPC), ListBox1)
                ListBox1.Enabled = True
                Button10.Text = "Add NPC and Create Spawn Point"
                Button10.Enabled = True
                Button10.ForeColor = Color.Green
                Button11.Text = "Delete NPC and Personal Spawn Point"
                Button11.Enabled = True
                Button8.Text = "Add && Create NPC and Spawn Point"
                Button8.Enabled = True
                Button8.ForeColor = Color.Green
                TextBox5.Enabled = True
                TextBox1.Enabled = (CheckBox11.Checked = False)
                CheckBox11.Enabled = True
                CheckBox12.Enabled = True
                CheckBox13.Enabled = True
                Button7.Enabled = True
                TextBox3.Enabled = True
                TextBox11.Enabled = True
                TextBox10.Enabled = True
                TextBox2.Enabled = True
                TextBox4.Enabled = True
                Button7.Enabled = (CheckBox12.Checked = False)
                TextBox11.Enabled = (CheckBox12.Checked = False)
                TextBox10.Enabled = (CheckBox12.Checked = False)
                TextBox4.Enabled = (CheckBox12.Checked = False)
                TextBox2.Enabled = (CheckBox12.Checked = False)
            Case "EditPointUniqueOff"
                CurrentMode = "EditCampOn"
                CheckBox2.ForeColor = Color.Orange
                CheckBox6.ForeColor = Color.Green
                CheckBox2.Enabled = True
                'CheckBox3.Enabled = True
                'ComboBox2.Enabled = True
                Label27.Text = "--"
                ListBox1.Items.Clear()
                ListBox1.Enabled = False
                Button10.Text = "--"
                Button10.Enabled = False
                Button10.ForeColor = Color.Black
                Button11.Text = "--"
                Button11.Enabled = False
                Button8.Text = "--"
                Button8.Enabled = False
                TextBox5.Enabled = False
                TextBox1.Enabled = False
                TextBox5.Text = ""
                If CheckBox11.Checked = False Then TextBox1.Text = ""
                CheckBox11.Enabled = False
                CheckBox12.Enabled = False
                CheckBox13.Enabled = False
                Button7.Enabled = False
                TextBox3.Enabled = False
                TextBox11.Enabled = False
                TextBox10.Enabled = False
                TextBox2.Enabled = False
                TextBox4.Enabled = False
                Label27.Text = "Spawn Points in this Camp:"
                lb.AddItemsToControl(Kairen.GetSpawnPointsFromSpawnCampFile(), ListBox1)
                ListBox1.Enabled = True
                Button10.Text = "Delete This Spawn Point"
                Button10.Enabled = True
        End Select
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.Text = "" Then
            sender.checked = False
            Exit Sub
        End If
        If sender.checked = True Then ModeHandler("EditCampOn") Else ModeHandler("EditCampOff")
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If sender.checked = True Then ModeHandler("EditPointUniqueOn") Else ModeHandler("EditPointUniqueOff")
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If sender.checked = True Then ModeHandler("EditPointNormalOn") Else ModeHandler("EditPointNormalOff")
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If TextBox17.Enabled = True Then
            TextBox17.Enabled = False
            TextBox16.Enabled = False
        Else
            TextBox17.Enabled = True
            TextBox16.Enabled = True
        End If
    End Sub

    Private Sub CheckBox11_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox11.CheckedChanged
        If sender.checked = True Then
            TextBox1.Text = TextBox5.Text
            TextBox1.Enabled = False
        Else
            TextBox1.Enabled = True
        End If
    End Sub

    Private Sub TextBox5_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox5.TextChanged

        If CheckBox11.Checked = True Then
            TextBox1.Text = sender.text
        End If
    End Sub
    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged
        If sender.text = "" Then
            Label24.ForeColor = Color.Black
        Else
            If lb.FE(lb.Folder_Custom_NPCs & sender.text & lb.Extension_NPC) Then
                Label24.ForeColor = Color.Red
            Else
                Label24.ForeColor = Color.Green
            End If
        End If
    End Sub

    Private Sub Button7_Click(sender As System.Object, e As System.EventArgs) Handles Button7.Click
        If FAPI_Connection.o_FAPIData Is Nothing Then Exit Sub
        TextBox11.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyX")
        TextBox10.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyY")
        TextBox4.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyZ")
        TextBox2.Text = FAPI_Connection.o_FAPIData.GetValueByAdditionalData("MyF")
    End Sub

    Private Sub NPCSaveSequence(ByVal sequence As String, ByRef npcsafenameControl As Control, ByRef npcgamenameControl As Control)
        If CheckBox6.Checked = True Then
            If sequence = "CreateThenAdd" Then
                If lb.FE(lb.Folder_Custom_NPCs & npcsafenameControl.Text & lb.Extension_NPC) Then
                    'alert npc safe name exists already
                    Exit Sub
                End If
                If lb.FE(lb.Folder_Custom_SpawnPoints & npcgamenameControl.Text & lb.Extension_Area) Then
                    'alert spawn point safe name exists already
                    Exit Sub
                End If
                If TextBox3.Text = "" Then
                    Exit Sub
                End If
                If TextBox11.Text = "" Or TextBox10.Text = "" Or TextBox4.Text = "" Or TextBox2.Text = "" Then
                    'error needs filled
                    Exit Sub
                End If
                Kairen.CreateNewSpawnPointFile(npcsafenameControl.Text)
                Kairen.AddSpawnPointToLoadedCampFile(npcsafenameControl.Text)
                'lb.AddItemsToControl(Kairen.GetSpawnPointsFromSpawnCampFile(), ListBox6)
                lb.DisplayMessage("Spawn Point File Created.", npcsafenameControl.Text, Me.Text)
                Kairen.NPCFile = New TextFileClass(lb.Folder_Custom_NPCs & npcsafenameControl.Text & lb.Extension_NPC, "--")
                Kairen.NPCFile.WriteLine("0.2.0")
                Kairen.NPCFile.WriteLine(npcsafenameControl.Text)
                Kairen.NPCFile.WriteLine(npcgamenameControl.Text)
                Kairen.NPCFile.WriteLine(TextBox11.Text)
                Kairen.NPCFile.WriteLine(TextBox10.Text)
                Kairen.NPCFile.WriteLine(TextBox4.Text)
                Kairen.NPCFile.WriteLine(TextBox2.Text)
                Kairen.NPCFile.WriteLine("Human")
                Kairen.NPCFile.WriteLine("Male")
                Kairen.NPCFile.WriteLine("Coachman")
                Kairen.NPCFile.WriteLine(TextBox3.Text)
                Kairen.NPCFile.WriteLine("255")
                Kairen.NPCFile.SaveFile()
                Kairen.AddNPCToLoadedSpawnPointFile(npcsafenameControl.Text)
                lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_NPCs, lb.Extension_NPC), ListBox1)
                If CheckBox13.Checked = False Then
                    TextBox3.Text = ""
                End If
                npcsafenameControl.Text = ""
                npcgamenameControl.Text = ""
                TextBox4.Text = ""
                TextBox2.Text = ""
                TextBox11.Text = ""
                TextBox10.Text = ""
            ElseIf sequence = "Add" Then
                If lb.FE(lb.Folder_Custom_SpawnPoints & npcsafenameControl.Text & lb.Extension_Area) Then
                    'alert spawn point safe name exists already
                    Exit Sub
                End If
                Kairen.CreateNewSpawnPointFile(ListBox1.Text)
                Kairen.AddSpawnPointToLoadedCampFile(ListBox1.Text)
                lb.DisplayMessage("Spawn Point File Created.", ListBox1.Text, Me.Text)
                Kairen.AddNPCToLoadedSpawnPointFile(ListBox1.Text)
            End If
        ElseIf CheckBox3.Checked = True Then
            'different mode
        Else
            'error wtf mode ya in breuh
        End If
    End Sub

    Private Sub Button8_Click(sender As System.Object, e As System.EventArgs) Handles Button8.Click
        If TextBox11.Text = "" And TextBox10.Text = "" And TextBox4.Text = "" And TextBox2.Text = "" And CheckBox12.Checked = True Then
            Button7_Click(sender, e)
            'Button7.PerformClick()
        End If
        NPCSaveSequence("CreateThenAdd", TextBox1, TextBox5)
    End Sub

    Private Sub Button10_Click(sender As System.Object, e As System.EventArgs) Handles Button10.Click
        Select Case CurrentMode
            Case "EditCampOn"
                If ListBox1.Text = "" Then Exit Sub
                Dim respone As MsgBoxResult = MsgBox("This will DELETE the " & Chr(34) & ListBox1.Text & Chr(34) & " spawn point file, and all work put into it." & vbNewLine & "Do you want to delete this file?", MsgBoxStyle.YesNo, "Confirm File Delete")
                If respone = MsgBoxResult.Yes Then
                    My.Computer.FileSystem.DeleteFile(lb.Folder_Custom_SpawnPoints & ListBox1.Text & lb.Extension_ReadWrites, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    Kairen.RemoveSpawnPointFromLoadedSpawnCampFile(ListBox1.Text)
                    lb.AddItemsToControl(Kairen.GetSpawnPointsFromSpawnCampFile, ListBox1)
                End If
            Case "EditPointUniqueOn"
                NPCSaveSequence("Add", ListBox1, ListBox1)
        End Select
    End Sub

    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If TextBox9.Text = "" Or TextBox8.Text = "" Then Exit Sub
        Kairen.CreateNewCampFile(TextBox9.Text, TextBox8.Text)
        Kairen.AddSpawnCampToLoadedNestFile(TextBox9.Text)
        lb.AddItemsToControl(Kairen.GetSpawnCampsFromSpawnNestFile(TextBox17.Text, TextBox16.Text), ComboBox1)
        lb.DisplayMessage("Camp File Created.", TextBox9.Text, "Create Spawn Camp Button")
        ComboBox1.SelectedIndex = 0
        TextBox8.Text = ""
        TextBox9.Text = ""
        Button5.ForeColor = Color.Black
        CheckBox2.ForeColor = Color.Green
        CheckBox2.Enabled = True
    End Sub

    Private Sub NestBoxes_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox17.TextChanged, TextBox16.TextChanged
        If TextBox16.Text = "" Or TextBox17.Text = "" Then
            CheckBox1.ForeColor = Color.Orange
        Else
            CheckBox1.ForeColor = Color.Green
        End If
    End Sub

    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs) Handles Button3.Click
        If ComboBox1.Text = "" Then Exit Sub
        Dim respone As MsgBoxResult = MsgBox("This will DELETE the " & Chr(34) & ComboBox1.Text & Chr(34) & " camp file, and all work put into it." & vbNewLine & "Do you want to delete this file?", MsgBoxStyle.YesNo, "Confirm File Delete")
        If respone = MsgBoxResult.Yes Then
            Kairen.RemoveSpawnCampFromLoadedSpawnNestFile(ComboBox1.Text)
            My.Computer.FileSystem.DeleteFile(lb.Folder_Custom_SpawnCamps & ComboBox1.Text & lb.Extension_ReadWrites, FileIO.UIOption.AllDialogs, FileIO.RecycleOption.SendToRecycleBin)
            lb.AddItemsToControl(Kairen.GetSpawnCampsFromSpawnNestFile(TextBox17.Text, TextBox16.Text), ComboBox1)
            If ComboBox1.Items.Count > 0 Then ComboBox1.SelectedIndex = 0
            If ComboBox1.Text = "" Then
                Button5.ForeColor = Color.Green
                CheckBox2.ForeColor = Color.Red
                CheckBox2.Enabled = False
            Else
                CheckBox2.ForeColor = Color.Green
                CheckBox2.Enabled = True
            End If
        End If
    End Sub

    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        Kairen.LaunchCE()
    End Sub

    Private Sub Button11_Click(sender As System.Object, e As System.EventArgs) Handles Button11.Click
        If sender.text = "Delete NPC and Personal Spawn Point" And ListBox1.Text <> "" Then
            'needs to check if the npc has a spawn point, and if the spawn point is in the loaded camp
            DeleteNPCAndPersonalSpawnPoint(ListBox1.Text)
            lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_NPCs, lb.Extension_NPC), ListBox1)
        End If
    End Sub
    Private Sub DeleteNPCAndPersonalSpawnPoint(ByVal nameSafe As String)
        Kairen.RemoveSpawnPointFromLoadedSpawnCampFile(nameSafe)
        If lb.FE(lb.Folder_Custom_NPCs & nameSafe & lb.Extension_NPC) Then My.Computer.FileSystem.DeleteFile(lb.Folder_Custom_NPCs & nameSafe & lb.Extension_NPC)
        If lb.FE(lb.Folder_Custom_SpawnPoints & nameSafe & lb.Extension_NPC) Then My.Computer.FileSystem.DeleteFile(lb.Folder_Custom_SpawnPoints & nameSafe & lb.Extension_NPC)
    End Sub

    Private Sub CheckBox12_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox12.CheckedChanged
        Button7.Enabled = (sender.Checked = False)
        TextBox11.Enabled = (sender.Checked = False)
        TextBox10.Enabled = (sender.Checked = False)
        TextBox4.Enabled = (sender.Checked = False)
        TextBox2.Enabled = (sender.Checked = False)
    End Sub

    Private Sub Label29_TextChanged(sender As System.Object, e As System.EventArgs) Handles Label29.TextChanged
        If sender.text = "0" Then Exit Sub
        If CheckBox1.Checked = False Then
            TextBox17.Text = sender.text
        End If
    End Sub

    Private Sub Label30_TextChanged(sender As System.Object, e As System.EventArgs) Handles Label30.TextChanged
        If sender.text = "0" Then Exit Sub
        If CheckBox1.Checked = False Then
            TextBox16.Text = sender.text
        End If
    End Sub

End Class