Public Class ItemMaker
    Public CloseFormLoaderToo As Boolean = True
    Private lb As CommonLibrary = FormLoader.lb
    'Private FAPI_Connection As FAPIConnectionManager2 = FormLoader.FAPI_Connection
    Private Kairen As Kairen2 = FormLoader.Kairen
    'Dim s As DataParserDelegate
    'Delegate Function DataParserDelegate(ByVal _additionaldata As String, ByRef _control As Object, ByRef _text As String)
    Dim ContextMenuSender As Object
    Dim LastIndexOfFileNameList As Integer = 0
#Region "Current Code"
    'form functions
    Private Sub ItemMaker_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " Release Version 1.3 - Kairen  " & Launcher_v2.Version_Current_Release
        'Kairen.ReinstantiateAbilityFile() 'replaced this with below when i randomly saw it
        Kairen.ReinstantiateItemFile()
        RegisterUIElements()
        lb.PositionForm(Me, 90, 70)
        GroupBox3.Enabled = False ' Edit File Data Controls GroupBox
        GroupBox4.Enabled = False ' Extended Data GroupBox
        GroupBox5.Enabled = False ' Control Restriction Options GroupBox
        Dim RaceList(9) As String
        RaceList(0) = "Dark Elf"
        RaceList(1) = "Troll"
        RaceList(2) = "Ogre"
        RaceList(3) = "Elf"
        RaceList(4) = "Dwarf"
        RaceList(5) = "Gnome"
        RaceList(6) = "Halfling"
        RaceList(7) = "Erudite"
        RaceList(8) = "Human"
        RaceList(9) = "Barbarian"
        For Each line In RaceList
            CheckedListBox1.Items.Add(line, False)
        Next


        Dim Classlist(14) As String
        Classlist(0) = "Warrior"
        Classlist(1) = "Paladin"
        Classlist(2) = "Shadowknight"
        Classlist(3) = "Enchanter"
        Classlist(4) = "Magician"
        Classlist(5) = "Wizard"
        Classlist(6) = "Alchemist"
        Classlist(7) = "Necromancer"
        Classlist(8) = "Monk"
        Classlist(9) = "Rogue"
        Classlist(10) = "Ranger"
        Classlist(11) = "Bard"
        Classlist(12) = "Druid"
        Classlist(13) = "Shaman"
        Classlist(14) = "Cleric"
        For Each line In Classlist
            CheckedListBox2.Items.Add(line, False)
        Next


        Dim RestrictionList(3) As String
        RestrictionList(0) = "NO RENT"
        RestrictionList(1) = "NO TRADE"
        RestrictionList(2) = "LORE"
        RestrictionList(3) = "CRAFTABLE"
        For Each line In RestrictionList
            CheckedListBox5.Items.Add(line, False)
        Next


        Dim Stats(17) As String
        Stats(0) = "Strength"
        Stats(1) = "Stamina"
        Stats(2) = "Agility"
        Stats(3) = "Dexterity"
        Stats(4) = "Wisdom"
        Stats(5) = "Intelligence"
        Stats(6) = "Charisma"
        Stats(7) = "FR"
        Stats(8) = "CR"
        Stats(9) = "LR"
        Stats(10) = "AR"
        Stats(11) = "PR"
        Stats(12) = "DR"
        Stats(13) = "HoT"
        Stats(14) = "PoT"
        Stats(15) = "HPMAX"
        Stats(16) = "POWMAX"
        Stats(17) = "AC"
        For Each line In Stats
            CheckedListBox3.Items.Add(line, False)
        Next


        Dim Stats_Tradeskills(6) As String
        Stats_Tradeskills(0) = "Fishing"
        Stats_Tradeskills(1) = "Jewel Crafting"
        Stats_Tradeskills(2) = "Armor Crafting"
        Stats_Tradeskills(3) = "Weapon Crafting"
        Stats_Tradeskills(4) = "Tailoring"
        Stats_Tradeskills(5) = "Alchemy"
        Stats_Tradeskills(6) = "Carpentry"
        For Each line In Stats_Tradeskills
            CheckedListBox7.Items.Add(line, False)
        Next


        Dim Stats_Atypical(3) As String
        Stats_Atypical(0) = "Def Mod"
        Stats_Atypical(1) = "Off Mod"
        Stats_Atypical(2) = "HP Factor"
        Stats_Atypical(3) = "Movement Rate"
        For Each line In Stats_Atypical
            CheckedListBox6.Items.Add(line, False)
        Next


        'Default Effect in case the actual effect the item has is not present in the list
        CheckedListBox4.Items.Add("HAS PROCESS", False)
        'Loads files from the CUSTOM effects folder into the CheckedListBox so they can be toggled
        If lb.DE(lb.Folder_Custom_Procs) Then
            lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Procs, lb.Extension_ProcFile), CheckedListBox4, False, False)
        Else
            My.Computer.FileSystem.CreateDirectory(lb.Folder_Custom_Procs)
        End If


        ComboBox2.Items.Add("none")
        ComboBox2.Items.Add("HAS GRAPHIC")
        ComboBox3.Items.Add("HAS ICON")


        'Loads files from the CUSTOM items folder into the ComboBox so they can be loaded
        If lb.DE(lb.Folder_Custom_Items) Then
            lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Items, lb.Extension_ItemFile), ComboBox1, True, True)
        Else
            My.Computer.FileSystem.CreateDirectory(lb.Folder_Custom_Items)
        End If


        RadioButton50.Checked = True 'Equip Type Not Set
        RadioButton51.Checked = True 'Attack Type Not Set
        Button6.Enabled = False
        RadioButton29.Checked = True
        Label26.Visible = False
        RadioButton49.Enabled = False
    End Sub
    Private Sub ItemMaker_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'FormLoader.FAPI_Connection.ObjectsToUpdate.Remove(Me)
        'lb.DisplayMessage("Closing " & Me.Text, "Alert: ", Me.Text)
        FormLoader.ActuallyClose = FormLoader.CloseFormLoaderToo(CloseFormLoaderToo)
        Application.Exit()
    End Sub

    'ui functions
    'valued checked list box functions
    Private Sub CheckedListBox3_6_7ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox3.ItemCheck, CheckedListBox6.ItemCheck, CheckedListBox7.ItemCheck
        'Initiates the Adding/Removal of the values in the Valued CheckedListBoxes
        If e.NewValue = CheckState.Checked Then
            If sender.items.Item(e.Index).Contains("+") = False Then
                If sender.items.Item(e.Index).Contains("-") = False Then
                    AskForStatModAmount(sender, e)
                End If
            End If
        Else
            If sender.items.Item(e.Index).Contains("+") Then
                sender.items.Item(e.Index) = Microsoft.VisualBasic.Left(sender.items.Item(e.Index), sender.items.Item(e.Index).IndexOf("+") - 1)
            ElseIf sender.items.Item(e.Index).Contains("-") Then
                sender.items.Item(e.Index) = Microsoft.VisualBasic.Left(sender.items.Item(e.Index), sender.items.Item(e.Index).IndexOf("-") - 1)
            End If
        End If
    End Sub
    Private Sub AskForStatModAmount(ByRef sender As System.Object, ByRef e As System.Windows.Forms.ItemCheckEventArgs)
        'Asks for the amount to modify the Valued CheckedListBoxes by
        Dim result As String = InputBox("Enter Stat Increase/Decrease:", "Enter Stat Modification Amount").Trim
        Do While result.Contains("+ ")
            result = result.Replace("+ ", "+")
        Loop
        Do While result.Contains(" +")
            result = result.Replace(" +", "+")
        Loop
        Do While result.Contains("- ")
            result = result.Replace("- ", "-")
        Loop
        Do While result.Contains(" -")
            result = result.Replace(" -", "-")
        Loop
        Try
            If CInt(result) Then
            End If
        Catch ex As Exception
            e.NewValue = CheckState.Unchecked
            Exit Sub
        End Try
        If AlterCheckedListBoxItemsStatMod(sender.Items.Item(e.Index), result) <> 0 Then e.NewValue = CheckState.Unchecked
    End Sub
    Private Function AlterCheckedListBoxItemsStatMod(ByRef _senderItem As System.Object, ByVal modvalue As String)
        'Adds or Removes the value from the Valued CheckedListBoxes
        Try
            Dim wasNegative As Boolean = modvalue.Contains("-")
            modvalue = modvalue.Replace("+", "")
            If modvalue.Replace("-", "").Length > 3 Then Return -2
            If _senderItem.Contains("+") Or _senderItem.Contains("-") Then Return -1
            If CInt(modvalue) >= 0 Then
                If CInt(modvalue) = 0 And wasNegative Then
                    _senderItem = _senderItem & " - " & modvalue.Replace("-", "")
                Else
                    _senderItem = _senderItem & " + " & modvalue.Replace("-", "")
                End If
            ElseIf CInt(modvalue) < 0 Then
                modvalue = modvalue.Replace("-", "").Trim
                _senderItem = _senderItem & " - " & modvalue
            End If
        Catch ex As Exception
            Return -3
        End Try
        Return 0
    End Function
    'effects checked list box
    Private Sub CheckedListBox4_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) Handles CheckedListBox4.ItemCheck
        If e.NewValue = CheckState.Checked Then
            If RadioButton29.Checked Or _
                RadioButton30.Checked Or _
                RadioButton31.Checked Or _
                RadioButton32.Checked Or _
                RadioButton33.Checked Then
                sender.enabled = False
            End If
        Else
            If CheckedListBox4.CheckedItems.Count = 1 Then
                sender.enabled = True
            End If
        End If
    End Sub
    'radio button functions
    Private Sub CheckBoxes_3_4_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBox3.CheckedChanged, CheckBox4.CheckedChanged
        ' the (select) "ALL" buttons for Race & Class respectively.
        Dim UseControl As Object
        If sender Is CheckBox3 Then
            UseControl = CheckedListBox1
        ElseIf sender Is CheckBox4 Then
            UseControl = CheckedListBox2
        End If

        If sender.Checked = True Then
            For index As Integer = 0 To UseControl.Items.Count - 1
                UseControl.SetItemChecked(index, True)
            Next
            UseControl.Enabled = False
        Else
            UseControl.Enabled = True
        End If
    End Sub
    Private Sub RadioButtonEnabler(ByVal _toEnable As String, Optional ByRef sender As Object = Nothing, Optional e As Object = Nothing)
        Select Case _toEnable
            Case "Worn"
                'Worn
                ComboBox2.Enabled = True ' In-Game Graphic
                Label15.Enabled = True ' In-Game Graphic Label
                TextBox1.Enabled = True ' HP
                Label7.Enabled = True ' Item HP Label
                TextBox2.Enabled = True ' Level
                Label2.Enabled = True ' Level Label
                TextBox3.Enabled = True ' Description
                TextBox4.Enabled = True ' Durability
                Label9.Enabled = True ' Duability Label
                TextBox9.Enabled = False ' Damage
                Label17.Enabled = False ' Damage Label
                TextBox17.Enabled = False ' Range
                Label28.Enabled = False ' Range Label
                TextBox16.Enabled = False ' Max Stack
                Label24.Enabled = False ' Max Stack Label
                TextBox15.Enabled = False ' Charges
                Label23.Enabled = False ' Charges Label
                CheckedListBox1.Enabled = True ' Race
                CheckedListBox2.Enabled = True ' Class
                CheckedListBox3.Enabled = True ' Stats
                CheckedListBox4.Enabled = True ' Effects
                CheckedListBox5.Enabled = True ' Restrictions
                Label3.Enabled = True ' Race Label
                Label4.Enabled = True ' Class Label
                Label6.Enabled = True ' Stats Label
                Label8.Enabled = True ' Effects Label
                Label13.Enabled = True ' Restrictions Label
                CheckBox3.Enabled = True ' Race - ALL
                CheckBox4.Enabled = True ' Class - ALL
                RadioButton1.Enabled = False ' NO EQUIP
                RadioButton2.Enabled = True ' HELM
                'RadioButton2.Checked = True ' HELM
                RadioButton3.Enabled = True ' ROBE
                RadioButton4.Enabled = True ' EARRING
                RadioButton5.Enabled = True ' NECK
                RadioButton6.Enabled = True ' CHEST
                RadioButton7.Enabled = True ' FOREARM
                RadioButton8.Enabled = True ' 2FOREARM
                RadioButton9.Enabled = True ' RING
                RadioButton10.Enabled = True ' BELT
                RadioButton11.Enabled = True ' PANTS
                RadioButton12.Enabled = True ' FEET
                RadioButton13.Enabled = False ' PRIMARY
                RadioButton14.Enabled = False ' SHIELD
                RadioButton15.Enabled = False ' SECONDARY
                RadioButton16.Enabled = False ' 2HAND
                RadioButton52.Enabled = False ' BOW
                RadioButton53.Enabled = False ' THROWN
                RadioButton19.Enabled = False ' HELD
                RadioButton20.Enabled = True ' GLOVES
                RadioButton21.Enabled = False ' FISHING
                RadioButton22.Enabled = False ' BAIT
                RadioButton23.Enabled = False ' WEAPON CRAFT
                RadioButton24.Enabled = False ' ARMOR CRAFT
                RadioButton25.Enabled = False ' TAILORING
                RadioButton26.Enabled = False ' JEWEL CRAFT
                RadioButton27.Enabled = False ' CARPENTRY
                RadioButton28.Enabled = False ' ALCHEMY
                GroupBox1.Enabled = True ' Equip Type Group
                GroupBox6.Enabled = False ' Attack Type Group
                TextBox8.Enabled = True ' Quest Category #
                Label14.Enabled = True ' Quest Category # Label
                TextBox14.Enabled = True ' Pattern Family
                Label22.Enabled = True ' Pattern Family Label

                RadioButton45.Checked = False ' Magic Item Category 
                RadioButton45.Enabled = False ' Magic Item Category
                RadioButton46.Checked = True ' Equipment Item Category
                RadioButton46.Enabled = True ' Equipment Item Category
                RadioButton47.Checked = False ' Traadeskill Item Category
                RadioButton47.Enabled = False ' Traadeskill Item Category
                RadioButton48.Checked = False ' Misc. Item Category
                RadioButton48.Enabled = False ' Misc. Item Category

                RadioButton34.Enabled = False ' No Attack Type

                TextBox10.Enabled = True ' BLUE
                Label18.Enabled = True ' BLUE Label
                TextBox11.Enabled = True ' RED
                Label19.Enabled = True ' RED Label
                TextBox12.Enabled = True ' GREEN
                Label20.Enabled = True ' GREEN Label
                TextBox13.Enabled = True ' ???
                Label21.Enabled = True ' ??? Label

                CheckedListBox7.Enabled = False ' Tradeskill Stats
                Label27.Enabled = False ' Tradeskill Stats Label
                CheckedListBox6.Enabled = False ' Stats Atypical
                Label25.Enabled = False ' Stats Atypical Label
            Case "Wielded"
                'Wielded
                ComboBox2.Enabled = True ' In-Game Graphic
                Label15.Enabled = True ' In-Game Graphic Label
                TextBox1.Enabled = True ' Item HP
                Label7.Enabled = True ' Item HP Label
                TextBox2.Enabled = True ' Level
                Label2.Enabled = True ' Level Label
                TextBox3.Enabled = True ' Description
                TextBox4.Enabled = True ' Durability
                Label9.Enabled = True ' Duability Label
                TextBox9.Enabled = True ' Damage
                Label17.Enabled = True ' Damage Label
                TextBox17.Enabled = True ' Range
                Label28.Enabled = True ' Range Label
                TextBox16.Enabled = True ' Max Stack
                Label24.Enabled = True ' Max Stack Label
                TextBox15.Enabled = False ' Charges
                Label23.Enabled = False ' Charges Label
                CheckedListBox1.Enabled = True ' Race
                CheckedListBox2.Enabled = True ' Class
                CheckedListBox3.Enabled = True ' Stats
                CheckedListBox4.Enabled = True ' Effects
                CheckedListBox5.Enabled = True ' Restrictions
                Label3.Enabled = True ' Race Label
                Label4.Enabled = True ' Class Label
                Label6.Enabled = True ' Stats Label
                Label8.Enabled = True ' Effects Label
                Label13.Enabled = True ' Restrictions Label
                CheckBox3.Enabled = True ' Race - ALL
                CheckBox4.Enabled = True ' Class - ALL
                RadioButton1.Enabled = False ' NO EQUIP
                RadioButton2.Enabled = False ' HELM
                RadioButton3.Enabled = False ' ROBE
                RadioButton4.Enabled = False ' EARRING
                RadioButton5.Enabled = False ' NECK
                RadioButton6.Enabled = False ' CHEST
                RadioButton7.Enabled = False ' FOREARM
                RadioButton8.Enabled = False ' 2FOREARM
                RadioButton9.Enabled = False ' RING
                RadioButton10.Enabled = False ' BELT
                RadioButton11.Enabled = False ' PANTS
                RadioButton12.Enabled = False ' FEET
                RadioButton13.Enabled = True ' PRIMARY
                'RadioButton13.Checked = True ' PRIMARY
                RadioButton14.Enabled = True ' SHIELD
                RadioButton15.Enabled = True ' SECONDARY
                RadioButton16.Enabled = True ' 2HAND
                RadioButton52.Enabled = True ' BOW
                RadioButton53.Enabled = True ' THROWN
                RadioButton19.Enabled = True ' HELD
                RadioButton20.Enabled = False ' GLOVES
                RadioButton21.Enabled = False ' FISHING
                RadioButton22.Enabled = False ' BAIT
                RadioButton23.Enabled = False ' WEAPON CRAFT
                RadioButton24.Enabled = False ' ARMOR CRAFT
                RadioButton25.Enabled = False ' TAILORING
                RadioButton26.Enabled = False ' JEWEL CRAFT
                RadioButton27.Enabled = False ' CARPENTRY
                RadioButton28.Enabled = False ' ALCHEMY
                GroupBox1.Enabled = True ' Equip Type Group
                GroupBox6.Enabled = True ' Attack Type Group
                TextBox8.Enabled = True ' Quest Category #
                Label14.Enabled = True ' Quest Category # Label
                TextBox14.Enabled = True ' Pattern Family
                Label22.Enabled = True ' Pattern Family Label

                RadioButton45.Checked = False ' Magic Item Category 
                RadioButton45.Enabled = False ' Magic Item Category
                RadioButton46.Checked = True ' Equipment Item Category
                RadioButton46.Enabled = True ' Equipment Item Category
                RadioButton47.Checked = False ' Traadeskill Item Category
                RadioButton47.Enabled = False ' Traadeskill Item Category
                RadioButton48.Checked = False ' Misc. Item Category
                RadioButton48.Enabled = False ' Misc. Item Category

                RadioButton34.Enabled = False ' No Attack Type

                TextBox10.Enabled = False ' BLUE
                Label18.Enabled = False ' BLUE Label
                TextBox11.Enabled = False ' RED
                Label19.Enabled = False ' RED Label
                TextBox12.Enabled = False ' GREEN
                Label20.Enabled = False ' GREEN Label
                TextBox13.Enabled = False ' ???
                Label21.Enabled = False ' ??? Label

                CheckedListBox7.Enabled = False ' Tradeskill Stats
                Label27.Enabled = False ' Tradeskill Stats Label
                CheckedListBox6.Enabled = False ' Stats Atypical
                Label25.Enabled = False ' Stats Atypical Label
            Case "Tools"
                'Tools

                ComboBox2.Enabled = True ' In-Game Graphic
                Label15.Enabled = True ' In-Game Graphic Label
                TextBox1.Enabled = True ' Item HP
                Label7.Enabled = True ' Item HP Label
                TextBox2.Enabled = True ' Level
                Label2.Enabled = True ' Level Label
                TextBox3.Enabled = True ' Description
                TextBox4.Enabled = True ' Durability
                Label9.Enabled = True ' Durability Label
                TextBox9.Enabled = False ' Damage
                Label17.Enabled = False ' Damage Label
                TextBox17.Enabled = False ' Range
                Label28.Enabled = False ' Range Label
                TextBox16.Enabled = False ' Max Stack
                Label24.Enabled = False ' Max Stack Label
                TextBox15.Enabled = False ' Charges
                Label23.Enabled = False ' Charges Label
                CheckedListBox1.Enabled = False ' Race
                CheckedListBox2.Enabled = False ' Class
                CheckedListBox3.Enabled = False ' Stats
                CheckedListBox4.Enabled = False ' Effects
                CheckedListBox5.Enabled = False ' Restrictions
                Label3.Enabled = False ' Race Label
                Label4.Enabled = False ' Class Label
                Label6.Enabled = False ' Stats Label
                Label8.Enabled = False ' Effects Label
                Label13.Enabled = False ' Restrictions Label
                CheckBox3.Enabled = False ' Race - ALL
                CheckBox4.Enabled = False ' Class - ALL
                CheckBox3.Checked = True ' Race - ALL
                CheckBox4.Checked = True ' Class - ALL
                RadioButton1.Enabled = False ' NO EQUIP
                RadioButton2.Enabled = False ' HELM
                RadioButton3.Enabled = False ' ROBE
                RadioButton4.Enabled = False ' EARRING
                RadioButton5.Enabled = False ' NECK
                RadioButton6.Enabled = False ' CHEST
                RadioButton7.Enabled = False ' FOREARM
                RadioButton8.Enabled = False ' 2FOREARM
                RadioButton9.Enabled = False ' RING
                RadioButton10.Enabled = False ' BELT
                RadioButton11.Enabled = False ' PANTS
                RadioButton12.Enabled = False ' FEET
                RadioButton13.Enabled = False ' PRIMARY
                RadioButton14.Enabled = False ' SHIELD
                RadioButton15.Enabled = False ' SECONDARY
                RadioButton16.Enabled = False ' 2HAND
                RadioButton52.Enabled = False ' BOW
                RadioButton53.Enabled = False ' THROWN
                RadioButton19.Enabled = False ' HELD
                RadioButton20.Enabled = False ' GLOVES
                'RadioButton21.Checked = True ' FISHING
                RadioButton21.Enabled = True ' FISHING
                RadioButton22.Enabled = True ' BAIT
                RadioButton23.Enabled = True ' WEAPON CRAFT
                RadioButton24.Enabled = True ' ARMOR CRAFT
                RadioButton25.Enabled = True ' TAILORING
                RadioButton26.Enabled = True ' JEWEL CRAFT
                RadioButton27.Enabled = True ' CARPENTRY
                RadioButton28.Enabled = True ' ALCHEMY
                GroupBox1.Enabled = True ' Equip Type Group
                GroupBox6.Enabled = False ' Attack Type Group
                TextBox8.Enabled = True ' Quest Category #
                Label14.Enabled = True ' Quest Category # Label
                TextBox14.Enabled = False ' Pattern Family
                Label22.Enabled = False ' Pattern Family Label

                RadioButton45.Checked = False ' Magic Item Category 
                RadioButton45.Enabled = False ' Magic Item Category
                RadioButton46.Checked = True ' Equipment Item Category
                RadioButton46.Enabled = True ' Equipment Item Category
                RadioButton47.Checked = False ' Traadeskill Item Category
                RadioButton47.Enabled = False ' Traadeskill Item Category
                RadioButton48.Checked = False ' Misc. Item Category
                RadioButton48.Enabled = False ' Misc. Item Category

                RadioButton34.Enabled = False ' No Attack Type

                TextBox10.Enabled = False ' BLUE
                Label18.Enabled = False ' BLUE Label
                TextBox11.Enabled = False ' RED
                Label19.Enabled = False ' RED Label
                TextBox12.Enabled = False ' GREEN
                Label20.Enabled = False ' GREEN Label
                TextBox13.Enabled = False ' ???
                Label21.Enabled = False ' ??? Label

                CheckedListBox7.Enabled = True ' Tradeskill Stats
                Label27.Enabled = True ' Tradeskill Stats Label
                CheckedListBox6.Enabled = False ' Stats Atypical
                Label25.Enabled = False ' Stats Atypical Label
            Case "Gem"
                'Gem
                ComboBox2.Enabled = False ' In-Game Graphic
                Label15.Enabled = False ' In-Game Graphic Label
                TextBox1.Enabled = False ' Item HP
                Label7.Enabled = False ' Item HP Label
                TextBox2.Enabled = False ' Level
                Label2.Enabled = False ' Level Label
                TextBox3.Enabled = True ' Description
                TextBox4.Enabled = False ' Durability
                Label9.Enabled = False ' Duability Label
                TextBox9.Enabled = False ' Damage
                Label17.Enabled = False ' Damage Label
                TextBox17.Enabled = False ' Range
                Label28.Enabled = False ' Range Label
                TextBox16.Enabled = False ' Max Stack
                Label24.Enabled = False ' Max Stack Label
                TextBox15.Enabled = False ' Charges
                Label23.Enabled = False ' Charges Label
                CheckedListBox1.Enabled = False ' Race
                CheckedListBox2.Enabled = False ' Class
                CheckedListBox3.Enabled = False ' Stats
                CheckedListBox4.Enabled = False ' Effects
                CheckedListBox5.Enabled = False ' Restrictions
                Label3.Enabled = False ' Race Label
                Label4.Enabled = False ' Class Label
                Label6.Enabled = False ' Stats Label
                Label8.Enabled = False ' Effects Label
                Label13.Enabled = False ' Restrictions Label
                CheckBox3.Enabled = False ' Race - ALL
                CheckBox4.Enabled = False ' Class - ALL
                RadioButton1.Enabled = False ' NO EQUIP
                RadioButton1.Checked = True ' NO EQUIP
                RadioButton2.Enabled = False ' HELM
                RadioButton3.Enabled = False ' ROBE
                RadioButton4.Enabled = False ' EARRING
                RadioButton5.Enabled = False ' NECK
                RadioButton6.Enabled = False ' CHEST
                RadioButton7.Enabled = False ' FOREARM
                RadioButton8.Enabled = False ' 2FOREARM
                RadioButton9.Enabled = False ' RING
                RadioButton10.Enabled = False ' BELT
                RadioButton11.Enabled = False ' PANTS
                RadioButton12.Enabled = False ' FEET
                RadioButton13.Enabled = False ' PRIMARY
                RadioButton14.Enabled = False ' SHIELD
                RadioButton15.Enabled = False ' SECONDARY
                RadioButton16.Enabled = False ' 2HAND
                RadioButton52.Enabled = False ' BOW
                RadioButton53.Enabled = False ' THROWN
                RadioButton19.Enabled = False ' HELD
                RadioButton20.Enabled = False ' GLOVES
                RadioButton21.Enabled = False ' FISHING
                RadioButton22.Enabled = False ' BAIT
                RadioButton23.Enabled = False ' WEAPON CRAFT
                RadioButton24.Enabled = False ' ARMOR CRAFT
                RadioButton25.Enabled = False ' TAILORING
                RadioButton26.Enabled = False ' JEWEL CRAFT
                RadioButton27.Enabled = False ' CARPENTRY
                RadioButton28.Enabled = False ' ALCHEMY
                GroupBox1.Enabled = False ' Equip Type Group
                GroupBox6.Enabled = False ' Attack Type Group
                TextBox8.Enabled = True ' Quest Category #
                Label14.Enabled = True ' Quest Category # Label
                TextBox14.Enabled = False ' Pattern Family
                Label22.Enabled = False ' Pattern Family Label

                RadioButton45.Checked = False ' Magic Item Category 
                RadioButton45.Enabled = False ' Magic Item Category
                RadioButton46.Checked = False ' Equipment Item Category
                RadioButton46.Enabled = False ' Equipment Item Category
                RadioButton47.Checked = True ' Tradeskill Item Category
                RadioButton47.Enabled = True ' Tradeskill Item Category
                RadioButton48.Checked = False ' Misc. Item Category
                RadioButton48.Enabled = False ' Misc. Item Category

                RadioButton34.Enabled = False ' No Attack Type

                TextBox10.Enabled = False ' BLUE
                Label18.Enabled = False ' BLUE Label
                TextBox11.Enabled = False ' RED
                Label19.Enabled = False ' RED Label
                TextBox12.Enabled = False ' GREEN
                Label20.Enabled = False ' GREEN Label
                TextBox13.Enabled = False ' ???
                Label21.Enabled = False ' ??? Label

                CheckedListBox7.Enabled = False ' Tradeskill Stats
                Label27.Enabled = False ' Tradeskill Stats Label
                CheckedListBox6.Enabled = False ' Stats Atypical
                Label25.Enabled = False ' Stats Atypical Label
            Case "Misc"
                'Misc
                ComboBox2.Enabled = False ' In-Game Graphic
                Label15.Enabled = False ' In-Game Graphic Label
                TextBox1.Enabled = False ' Item HP
                Label7.Enabled = False ' Item HP Label
                TextBox2.Enabled = False ' Level
                Label2.Enabled = False ' Level Label
                TextBox3.Enabled = True ' Description
                TextBox4.Enabled = False ' Durability
                Label9.Enabled = False ' Duability Label
                TextBox9.Enabled = False ' Damage
                Label17.Enabled = False ' Damage Label
                TextBox17.Enabled = False ' Range
                Label28.Enabled = False ' Range Label
                TextBox16.Enabled = True ' Max Stack
                Label24.Enabled = True ' Max Stack Label
                TextBox15.Enabled = False ' Charges
                Label23.Enabled = False ' Charges Label
                CheckedListBox1.Enabled = False ' Race
                CheckedListBox2.Enabled = False ' Class
                CheckedListBox3.Enabled = False ' Stats
                CheckedListBox4.Enabled = False ' Effects
                CheckedListBox5.Enabled = True ' Restrictions
                Label3.Enabled = False ' Race Label
                Label4.Enabled = False ' Class Label
                Label6.Enabled = False ' Stats Label
                Label8.Enabled = False ' Effects Label
                Label13.Enabled = True ' Restrictions Label
                CheckBox3.Enabled = False ' Race - ALL
                CheckBox4.Enabled = False ' Class - ALL
                RadioButton1.Enabled = False ' NO EQUIP
                RadioButton1.Checked = True ' NO EQUIP
                RadioButton2.Enabled = False ' HELM
                RadioButton3.Enabled = False ' ROBE
                RadioButton4.Enabled = False ' EARRING
                RadioButton5.Enabled = False ' NECK
                RadioButton6.Enabled = False ' CHEST
                RadioButton7.Enabled = False ' FOREARM
                RadioButton8.Enabled = False ' 2FOREARM
                RadioButton9.Enabled = False ' RING
                RadioButton10.Enabled = False ' BELT
                RadioButton11.Enabled = False ' PANTS
                RadioButton12.Enabled = False ' FEET
                RadioButton13.Enabled = False ' PRIMARY
                RadioButton14.Enabled = False ' SHIELD
                RadioButton15.Enabled = False ' SECONDARY
                RadioButton16.Enabled = False ' 2HAND
                RadioButton52.Enabled = False ' BOW
                RadioButton53.Enabled = False ' THROWN
                RadioButton19.Enabled = False ' HELD
                RadioButton20.Enabled = False ' GLOVES
                RadioButton21.Enabled = False ' FISHING
                RadioButton22.Enabled = False ' BAIT
                RadioButton23.Enabled = False ' WEAPON CRAFT
                RadioButton24.Enabled = False ' ARMOR CRAFT
                RadioButton25.Enabled = False ' TAILORING
                RadioButton26.Enabled = False ' JEWEL CRAFT
                RadioButton27.Enabled = False ' CARPENTRY
                RadioButton28.Enabled = False ' ALCHEMY
                GroupBox1.Enabled = False ' Equip Type Group
                GroupBox6.Enabled = False ' Attack Type Group
                TextBox8.Enabled = True ' Quest Category #
                Label14.Enabled = True ' Quest Category # Label
                TextBox14.Enabled = False ' Pattern Family
                Label22.Enabled = False ' Pattern Family Label

                RadioButton45.Checked = False ' Magic Item Category 
                RadioButton45.Enabled = False ' Magic Item Category
                RadioButton46.Checked = False ' Equipment Item Category
                RadioButton46.Enabled = False ' Equipment Item Category
                RadioButton47.Checked = False ' Tradeskill Item Category
                RadioButton47.Enabled = False ' Tradeskill Item Category
                RadioButton48.Checked = True ' Misc. Item Category
                RadioButton48.Enabled = True ' Misc. Item Category

                RadioButton34.Enabled = False ' No Attack Type

                TextBox10.Enabled = False ' BLUE
                Label18.Enabled = False ' BLUE Label
                TextBox11.Enabled = False ' RED
                Label19.Enabled = False ' RED Label
                TextBox12.Enabled = False ' GREEN
                Label20.Enabled = False ' GREEN Label
                TextBox13.Enabled = False ' ???
                Label21.Enabled = False ' ??? Label

                CheckedListBox7.Enabled = False ' Tradeskill Stats
                Label27.Enabled = False ' Tradeskill Stats Label
                CheckedListBox6.Enabled = False ' Stats Atypical
                Label25.Enabled = False ' Stats Atypical Label
            Case "NONE"
                'NONE
                'RadioButtons_EquipType_CheckChanged(sender, e)

                ComboBox2.Enabled = True ' In-Game Graphic
                Label15.Enabled = True ' In-Game Graphic Label
                TextBox1.Enabled = True ' HP
                Label7.Enabled = True ' Item HP Label
                TextBox2.Enabled = True ' Level
                Label2.Enabled = True ' Level Label
                TextBox3.Enabled = True ' Description
                TextBox4.Enabled = True ' Durability
                Label9.Enabled = True ' Duability Label
                TextBox9.Enabled = True ' Damage
                Label17.Enabled = True ' Damage Label
                TextBox17.Enabled = True ' Range
                Label28.Enabled = True ' Range Label
                TextBox16.Enabled = True ' Max Stack
                Label24.Enabled = True ' Max Stack Label
                TextBox15.Enabled = True ' Charges
                Label23.Enabled = True ' Charges Label
                CheckedListBox1.Enabled = True ' Race
                CheckedListBox2.Enabled = True ' Class
                CheckedListBox3.Enabled = True ' Stats
                CheckedListBox4.Enabled = True ' Effects
                CheckedListBox5.Enabled = True ' Restrictions
                Label3.Enabled = True ' Race Label
                Label4.Enabled = True ' Class Label
                Label6.Enabled = True ' Stats Label
                Label8.Enabled = True ' Effects Label
                Label13.Enabled = True ' Restrictions Label
                CheckBox3.Enabled = True ' Race - ALL
                CheckBox4.Enabled = True ' Class - ALL
                RadioButton1.Enabled = True ' NO EQUIP
                RadioButton2.Enabled = True ' HELM
                RadioButton3.Enabled = True ' ROBE
                RadioButton4.Enabled = True ' EARRING
                RadioButton5.Enabled = True ' NECK
                RadioButton6.Enabled = True ' CHEST
                RadioButton7.Enabled = True ' FOREARM
                RadioButton8.Enabled = True ' 2FOREARM
                RadioButton9.Enabled = True ' RING
                RadioButton10.Enabled = True ' BELT
                RadioButton11.Enabled = True ' PANTS
                RadioButton12.Enabled = True ' FEET
                RadioButton13.Enabled = True ' PRIMARY
                RadioButton14.Enabled = True ' SHIELD
                RadioButton15.Enabled = True ' SECONDARY
                RadioButton16.Enabled = True ' 2HAND
                RadioButton52.Enabled = True ' BOW
                RadioButton53.Enabled = True ' THROWN
                RadioButton19.Enabled = True ' HELD
                RadioButton20.Enabled = True ' GLOVES
                RadioButton21.Enabled = True ' FISHING
                RadioButton22.Enabled = True ' BAIT
                RadioButton23.Enabled = True ' WEAPON CRAFT
                RadioButton24.Enabled = True ' ARMOR CRAFT
                RadioButton25.Enabled = True ' TAILORING
                RadioButton26.Enabled = True ' JEWEL CRAFT
                RadioButton27.Enabled = True ' CARPENTRY
                RadioButton28.Enabled = True ' ALCHEMY
                GroupBox1.Enabled = True ' Equip Type Group
                GroupBox6.Enabled = True ' Attack Type Group
                TextBox8.Enabled = True ' Quest Category #
                Label14.Enabled = True ' Quest Category # Label
                TextBox14.Enabled = True ' Pattern Family
                Label22.Enabled = True ' Pattern Family Label

                RadioButton45.Enabled = True ' Magic Item Category
                RadioButton46.Enabled = True ' Equipment Item Category
                RadioButton47.Enabled = True ' Tradeskill Item Category
                RadioButton48.Enabled = True ' Misc. Item Category

                RadioButton34.Enabled = True ' No Attack Type

                TextBox10.Enabled = True ' BLUE
                Label18.Enabled = True ' BLUE Label
                TextBox11.Enabled = True ' RED
                Label19.Enabled = True ' RED Label
                TextBox12.Enabled = True ' GREEN
                Label20.Enabled = True ' GREEN Label
                TextBox13.Enabled = True ' ???
                Label21.Enabled = True ' ??? Label

                CheckedListBox7.Enabled = True ' Tradeskill Stats
                Label27.Enabled = True ' Tradeskill Stats Label
                CheckedListBox6.Enabled = True ' Stats Atypical
                Label25.Enabled = True ' Stats Atypical Label
        End Select
    End Sub
    Private Sub RadioButton29_30_31_32_33_49CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton29.CheckedChanged, RadioButton30.CheckedChanged, RadioButton31.CheckedChanged, RadioButton32.CheckedChanged, RadioButton33.CheckedChanged, RadioButton49.CheckedChanged
        'Control Restriction Options radio buttons
        'Item Type Selection
        '  ClearForm() 'this should have a custom check to see if you can chenge the item type you are creating
        If sender.Checked = False Then Exit Sub
        If sender IsNot RadioButton49 Then ClearForm("Most")
        If sender Is RadioButton29 Then
            'Worn
            RadioButtonEnabler("Worn")

        ElseIf sender Is RadioButton30 Then
            'Wielded
            RadioButtonEnabler("Wielded")

        ElseIf sender Is RadioButton33 Then
            'Tools
            RadioButtonEnabler("Tools")

        ElseIf sender Is RadioButton31 Then
            'Gem
            RadioButtonEnabler("Gem")

        ElseIf sender Is RadioButton32 Then
            'Misc
            RadioButtonEnabler("Misc")

        ElseIf sender Is RadioButton49 Then
            'NONE
            RadioButtonEnabler("NONE", sender, e)

        Else
            'let's just turn everything on!

        End If
    End Sub
    Private Sub RadioButtons_EquipType_CheckChanged(sender As System.Object, e As System.EventArgs) Handles RadioButton1.CheckedChanged, RadioButton13.CheckedChanged, RadioButton14.CheckedChanged, RadioButton15.CheckedChanged, RadioButton16.CheckedChanged, RadioButton19.CheckedChanged, RadioButton2.CheckedChanged, RadioButton3.CheckedChanged, RadioButton4.CheckedChanged, RadioButton5.CheckedChanged, RadioButton6.CheckedChanged, RadioButton7.CheckedChanged, RadioButton8.CheckedChanged, RadioButton9.CheckedChanged, RadioButton10.CheckedChanged, RadioButton11.CheckedChanged, RadioButton12.CheckedChanged, RadioButton20.CheckedChanged, RadioButton21.CheckedChanged, RadioButton22.CheckedChanged, RadioButton23.CheckedChanged, RadioButton24.CheckedChanged, RadioButton25.CheckedChanged, RadioButton26.CheckedChanged, RadioButton27.CheckedChanged, RadioButton28.CheckedChanged, RadioButton52.CheckedChanged, RadioButton53.CheckedChanged
        If sender Is RadioButton13 Or sender Is RadioButton1 Or sender Is RadioButton34 Then
            If 1 = 2 Then Exit Sub
        End If
        ' RadioButton49.Checked = False
        'Sets the correct Attack Type radio button settings for the currently checked Equip Type radio button
        If sender.Checked = False Then Exit Sub
        If RadioButton49.Checked Then 'if control restrictions is nothing then
            GroupBox6.Enabled = True
            RadioButton34.Enabled = True ' No Attack
            RadioButton35.Enabled = True ' 1 HS
            RadioButton36.Enabled = True ' 1 HB
            RadioButton37.Enabled = True ' 1 HP
            RadioButton38.Enabled = True ' BOW
            RadioButton39.Enabled = True ' 2 HS
            RadioButton40.Enabled = True ' 2 HB
            RadioButton41.Enabled = True ' 2 HP
            RadioButton42.Enabled = True ' 2 HCROSSBOW
            RadioButton43.Enabled = True ' 1 HCROSSBOW
            RadioButton44.Enabled = True ' THROWN
        Else
            'if there are control restrictions in place..
            If sender Is RadioButton1 Then
                'EQUIP TYPE NONE
                RadioButton34.Checked = True ' No Attack

                GroupBox6.Enabled = False
                RadioButton34.Enabled = False ' No Attack
                RadioButton35.Enabled = False ' 1 HS
                RadioButton36.Enabled = False ' 1 HB
                RadioButton37.Enabled = False ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' THROWN
            ElseIf sender Is RadioButton19 Then
                'EQUIP TYPE HELD
                RadioButton34.Checked = True ' No Attack

                GroupBox6.Enabled = True
                RadioButton34.Enabled = True ' No Attack
                RadioButton35.Enabled = False ' 1 HS
                RadioButton36.Enabled = False ' 1 HB
                RadioButton37.Enabled = False ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' 
            ElseIf sender Is RadioButton13 Then
                'EQUIP TYPE PRIMARY
                RadioButton35.Checked = True ' 1 HS

                GroupBox6.Enabled = True
                RadioButton34.Enabled = False ' No Attack
                RadioButton35.Enabled = True ' 1 HS
                RadioButton36.Enabled = True ' 1 HB
                RadioButton37.Enabled = True ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' THROWN
            ElseIf sender Is RadioButton14 Then
                'EQUIP TYPE SHIELD
                RadioButton34.Checked = True ' No Attack

                GroupBox6.Enabled = True
                RadioButton34.Enabled = True ' No Attack
                RadioButton35.Enabled = False ' 1 HS
                RadioButton36.Enabled = False ' 1 HB
                RadioButton37.Enabled = False ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' THROWN
            ElseIf sender Is RadioButton15 Then
                'EQUIP TYPE SECONDARY
                RadioButton35.Checked = True ' 1 HS

                GroupBox6.Enabled = True
                RadioButton34.Enabled = False ' No Attack
                RadioButton35.Enabled = True ' 1 HS
                RadioButton36.Enabled = True ' 1 HB
                RadioButton37.Enabled = True ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' THROWN
            ElseIf sender Is RadioButton16 Then
                'EQUIP TYPE 2HAND
                RadioButton39.Checked = True ' 2 HS

                GroupBox6.Enabled = True
                RadioButton34.Enabled = False ' No Attack
                RadioButton35.Enabled = False ' 1 HS
                RadioButton36.Enabled = False ' 1 HB
                RadioButton37.Enabled = False ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = True ' 2 HS
                RadioButton40.Enabled = True ' 2 HB
                RadioButton41.Enabled = True ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' THROWN
            ElseIf sender Is RadioButton52 Then
                'EQUIP TYPE BOW
                RadioButton38.Checked = True ' Bow

                GroupBox6.Enabled = True
                RadioButton34.Enabled = False ' No Attack
                RadioButton35.Enabled = False ' 1 HS
                RadioButton36.Enabled = False ' 1 HB
                RadioButton37.Enabled = False ' 1 HP
                RadioButton38.Enabled = True ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = True ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = False ' THROWN
            ElseIf sender Is RadioButton53 Then
                'EQUIP TYPE THROWN
                RadioButton44.Checked = True ' Thrown

                GroupBox6.Enabled = True
                RadioButton34.Enabled = False ' No Attack
                RadioButton35.Enabled = False ' 1 HS
                RadioButton36.Enabled = False ' 1 HB
                RadioButton37.Enabled = False ' 1 HP
                RadioButton38.Enabled = False ' BOW
                RadioButton39.Enabled = False ' 2 HS
                RadioButton40.Enabled = False ' 2 HB
                RadioButton41.Enabled = False ' 2 HP
                RadioButton42.Enabled = False ' 2 HCROSSBOW
                RadioButton43.Enabled = False ' 1 HCROSSBOW
                RadioButton44.Enabled = True ' THROWN
            Else
                'EQUIP TYPE that doesn't have attack type
                If RadioButton49.Checked = False Then
                    RadioButton34.Checked = True ' No Attack

                    GroupBox6.Enabled = False
                    RadioButton34.Enabled = False ' No Attack
                    RadioButton35.Enabled = False ' 1 HS
                    RadioButton36.Enabled = False ' 1 HB
                    RadioButton37.Enabled = False ' 1 HP
                    RadioButton38.Enabled = False ' BOW
                    RadioButton39.Enabled = False ' 2 HS
                    RadioButton40.Enabled = False ' 2 HB
                    RadioButton41.Enabled = False ' 2 HP
                    RadioButton42.Enabled = False ' 2 HCROSSBOW
                    RadioButton43.Enabled = False ' 1 HCROSSBOW
                    RadioButton44.Enabled = False ' THROWN
                End If
            End If
        End If
    End Sub
    Private Function GetRadioButtonByDataName(ByVal _additionalData As String, ByRef _control As Object, ByRef _rawData As Object)
        'if i just have the additional data, i am getting the control and raw data.
        'if i have the additional data and the raw data, i and getting the control
        If _additionalData = "Equip Type" Or _rawData = "Equip Type" Then 'Or _additionalData = "control restriction options" Or _rawData = "control restriction options" Then
            If 1 = 2 Then Return 1111
        End If
        If _rawData Is Nothing Then
            'set the file data to the control data based off of additional data
            Select Case _additionalData
                Case "control restriction options"
                    If RadioButton29.Checked Then 'worn
                        _control = RadioButton29
                        _rawData = "Worn"
                    ElseIf RadioButton30.Checked Then 'wielded
                        _control = RadioButton30
                        _rawData = "Wielded"
                    ElseIf RadioButton33.Checked Then 'tools
                        _control = RadioButton33
                        _rawData = "Tools"
                    ElseIf RadioButton31.Checked Then 'gems
                        _control = RadioButton31
                        _rawData = "Gems"
                    ElseIf RadioButton32.Checked Then 'misc
                        _control = RadioButton32
                        _rawData = "Misc"
                    ElseIf RadioButton49.Checked Then 'none
                        _control = RadioButton49
                        _rawData = "NONE"
                    Else
                        'error
                    End If

                Case "Inventory Category"
                    If RadioButton45.Checked Then 'Magic
                        _control = RadioButton45
                        _rawData = "Magic"
                    ElseIf RadioButton46.Checked Then 'Equipment
                        _control = RadioButton46
                        _rawData = "Equipment"
                    ElseIf RadioButton47.Checked Then 'Tradeskills
                        _control = RadioButton47
                        _rawData = "Tradeskills"
                    ElseIf RadioButton48.Checked Then 'Misc
                        _control = RadioButton48
                        _rawData = "Misc"
                    Else
                        'eror
                    End If

                Case "Equip Type"
                    If RadioButton1.Checked Then 'none
                        _control = RadioButton1
                        _rawData = "NONE"
                    ElseIf RadioButton2.Checked Then 'helm
                        _control = RadioButton2
                        _rawData = "HELM"
                    ElseIf RadioButton3.Checked Then 'robe
                        _control = RadioButton3
                        _rawData = "ROBE"
                    ElseIf RadioButton4.Checked Then 'earring
                        _control = RadioButton4
                        _rawData = "EARRING"
                    ElseIf RadioButton5.Checked Then 'neck
                        _control = RadioButton5
                        _rawData = "NECK"
                    ElseIf RadioButton6.Checked Then 'chest
                        _control = RadioButton6
                        _rawData = "CHEST"
                    ElseIf RadioButton7.Checked Then 'forearm
                        _control = RadioButton7
                        _rawData = "FOREARM"
                    ElseIf RadioButton8.Checked Then '2forearm
                        _control = RadioButton8
                        _rawData = "2FOREARM"
                    ElseIf RadioButton9.Checked Then 'ring
                        _control = RadioButton9
                        _rawData = "RING"
                    ElseIf RadioButton10.Checked Then 'belt
                        _control = RadioButton10
                        _rawData = "BELT"
                    ElseIf RadioButton11.Checked Then 'pants
                        _control = RadioButton11
                        _rawData = "PANTS"
                    ElseIf RadioButton12.Checked Then 'feet
                        _control = RadioButton12
                        _rawData = "FEET"
                    ElseIf RadioButton20.Checked Then 'gloves
                        _control = RadioButton20
                        _rawData = "GLOVES"
                    ElseIf RadioButton13.Checked Then 'primary
                        _control = RadioButton13
                        _rawData = "PRIMARY"
                    ElseIf RadioButton14.Checked Then 'shield
                        _control = RadioButton14
                        _rawData = "SHIELD"
                    ElseIf RadioButton15.Checked Then 'secondary
                        _control = RadioButton15
                        _rawData = "SECONDARY"
                    ElseIf RadioButton16.Checked Then '2hand
                        _control = RadioButton16
                        _rawData = "2HAND"
                    ElseIf RadioButton52.Checked Then 'bow
                        _control = RadioButton52
                        _rawData = "BOW"
                    ElseIf RadioButton53.Checked Then 'thrown
                        _control = RadioButton53
                        _rawData = "THROWN"
                    ElseIf RadioButton19.Checked Then 'held
                        _control = RadioButton19
                        _rawData = "HELD"
                    ElseIf RadioButton21.Checked Then 'fishing
                        _control = RadioButton21
                        _rawData = "FISHING"
                    ElseIf RadioButton22.Checked Then 'bait
                        _control = RadioButton22
                        _rawData = "BAIT"
                    ElseIf RadioButton23.Checked Then 'wep
                        _control = RadioButton23
                        _rawData = "WEAPON CRAFT"
                    ElseIf RadioButton24.Checked Then 'arm
                        _control = RadioButton24
                        _rawData = "ARMORM CRAFT"
                    ElseIf RadioButton25.Checked Then 'tail
                        _control = RadioButton25
                        _rawData = "TAILORING"
                    ElseIf RadioButton26.Checked Then 'jew
                        _control = RadioButton26
                        _rawData = "JEWEL CRAFT"
                    ElseIf RadioButton27.Checked Then 'carp
                        _control = RadioButton27
                        _rawData = "CARPENTRY"
                    ElseIf RadioButton28.Checked Then 'alc
                        _control = RadioButton28
                        _rawData = "ALCHEMY"
                    Else
                        'error
                    End If

                Case "Attack Type"
                    'Attack Type
                    If RadioButton34.Checked = True Then
                        _control = RadioButton34
                        _rawData = "NO ATTACK"
                    ElseIf RadioButton35.Checked = True Then
                        _control = RadioButton35
                        _rawData = "1HS"
                    ElseIf RadioButton36.Checked = True Then
                        _control = RadioButton36
                        _rawData = "1HB"
                    ElseIf RadioButton37.Checked = True Then
                        _control = RadioButton37
                        _rawData = "1HP"
                    ElseIf RadioButton43.Checked = True Then
                        _control = RadioButton43
                        _rawData = "1HCROSSBOW"
                    ElseIf RadioButton38.Checked = True Then
                        _control = RadioButton38
                        _rawData = "BOW"
                    ElseIf RadioButton44.Checked = True Then
                        _control = RadioButton44
                        _rawData = "THROWN"
                    ElseIf RadioButton39.Checked = True Then
                        _control = RadioButton39
                        _rawData = "2HS"
                    ElseIf RadioButton40.Checked = True Then
                        _control = RadioButton40
                        _rawData = "2HB"
                    ElseIf RadioButton41.Checked = True Then
                        _control = RadioButton41
                        _rawData = "2HP"
                    ElseIf RadioButton42.Checked = True Then
                        _control = RadioButton42
                        _rawData = "2HCROSSBOW"
                    Else
                        'error
                    End If

                Case Else
                    'error

            End Select
        ElseIf _control Is Nothing Then
            'set the control based off of raw data
            Select Case _rawData
                'Attack Type
                Case "NO ATTACK"
                    _control = RadioButton34
                Case "1HS"
                    _control = RadioButton35
                Case "1HB"
                    _control = RadioButton36
                Case "1HP"
                    _control = RadioButton37
                Case "1HCROSSBOW"
                    _control = RadioButton43
                Case "BOW"
                    _control = RadioButton38
                Case "THROWN"
                    _control = RadioButton44
                Case "2HS"
                    _control = RadioButton39
                Case "2HB"
                    _control = RadioButton40
                Case "2HP"
                    _control = RadioButton41
                Case "2HCROSSBOW"
                    _control = RadioButton42

                    'Equip Type
                Case "NO EQUIP"
                    _control = RadioButton1
                Case "HELM"
                    _control = RadioButton2
                Case "ROBE"
                    _control = RadioButton3
                Case "EARRING"
                    _control = RadioButton4
                Case "NECK"
                    _control = RadioButton5
                Case "CHEST"
                    _control = RadioButton6
                Case "FOREARM"
                    _control = RadioButton7
                Case "2FOREARM"
                    _control = RadioButton8
                Case "RING"
                    _control = RadioButton9
                Case "BELT"
                    _control = RadioButton10
                Case "PANTS"
                    _control = RadioButton11
                Case "FEET"
                    _control = RadioButton12
                Case "GLOVES"
                    _control = RadioButton20
                Case "PRIMARY"
                    _control = RadioButton13
                Case "SHIELD"
                    _control = RadioButton14
                Case "SECONDARY"
                    _control = RadioButton15
                Case "2HAND"
                    _control = RadioButton16
                Case "BOW"
                    _control = RadioButton52
                Case "THROWN"
                    _control = RadioButton53
                Case "HELD"
                    _control = RadioButton19
                Case "FISHING"
                    _control = RadioButton21
                Case "BAIT"
                    _control = RadioButton22
                Case "WEAPON CRAFT"
                    _control = RadioButton23
                Case "ARMOR CRAFT"
                    _control = RadioButton24
                Case "TAILORING"
                    _control = RadioButton25
                Case "JEWEL CRAFT"
                    _control = RadioButton26
                Case "CARPENTRY"
                    _control = RadioButton27
                Case "ALCHEMY"
                    _control = RadioButton28
                Case Else
                    Select Case _additionalData
                        'Control Restriction Option Type
                        Case "control restriction options"
                            If _rawData = "Worn" Then
                                _control = RadioButton29
                            ElseIf _rawData = "Wielded" Then
                                _control = RadioButton30
                            ElseIf _rawData = "Tools" Then
                                _control = RadioButton33
                            ElseIf _rawData = "Gems" Then
                                _control = RadioButton31
                            ElseIf _rawData = "Misc" Then
                                _control = RadioButton32
                            ElseIf _rawData = "NONE" Then
                                _control = RadioButton49
                            Else
                                'error
                            End If

                            'Inventory Category Type
                        Case "Inventory Category"
                            If _rawData = "Magic" Then
                                _control = RadioButton45
                            ElseIf _rawData = "Equipment" Then
                                _control = RadioButton46
                            ElseIf _rawData = "Tradeskills" Then
                                _control = RadioButton47
                            ElseIf _rawData = "Misc" Then
                                _control = RadioButton48
                            Else
                                'error
                            End If

                        Case Else
                            'error
                    End Select
            End Select
        Else
            'error
        End If
        Return 0
    End Function 'get radio button
    'helper functions
    'right click stuff
    Private Sub ToolStripSetterUpper(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles CheckBox3.MouseDown, CheckBox4.MouseDown, Label6.MouseDown, Label25.MouseDown, Label8.MouseDown, Label13.MouseDown
        'Sets up Right-Click menus based off the right-clicked control.
        If e.Button = MouseButtons.Left Then
        ElseIf e.Button = MouseButtons.Right Then
            'show option to clear all [aka select none]
            ContextMenuSender = sender
            If sender Is CheckBox3 Or _
                sender Is CheckBox4 Then
                DefaultToolStripMenuItem.Text = "NONE"
            ElseIf sender Is Label6 Or _
                sender Is Label8 Or _
                sender Is Label25 Or _
                sender Is Label13 Then
                DefaultToolStripMenuItem.Text = "Clear All"
            End If
            ContextMenuStrip1.Show(CType(sender, Control), e.Location)
        End If
    End Sub
    Sub ToggleMouseCursor(sender As System.Object, e As System.EventArgs) Handles Label6.MouseEnter, Label6.MouseLeave, Label8.MouseEnter, Label25.MouseEnter, Label25.MouseLeave, Label8.MouseLeave, Label13.MouseEnter, Label13.MouseLeave
        'Change's the mouse's icon when over an applicable control
        If Me.Cursor = Cursors.Default Then
            Me.Cursor = Cursors.Help
        ElseIf Me.Cursor = Cursors.Help Then
            Me.Cursor = Cursors.Default
        End If
    End Sub
    Private Sub DefaultToolStripMenuItem_MouseDown(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles DefaultToolStripMenuItem.MouseDown
        'Uses the data setup by the ToolStripSetterUpper to determinee when to do when the Right-Click Toolstrip is clicked on
        If e.Button = MouseButtons.Left Then
            If ContextMenuSender Is CheckBox3 Or _
                ContextMenuSender Is CheckBox4 Or _
                ContextMenuSender Is Label6 Or _
                ContextMenuSender Is Label25 Or _
                ContextMenuSender Is Label8 Or _
                ContextMenuSender Is Label13 Then
                Dim UseControl As Object
                If ContextMenuSender Is CheckBox3 Then
                    UseControl = CheckedListBox1
                    ContextMenuSender.Checked = False
                ElseIf ContextMenuSender Is CheckBox4 Then
                    UseControl = CheckedListBox2
                    ContextMenuSender.Checked = False
                ElseIf ContextMenuSender Is Label6 Then
                    UseControl = CheckedListBox3
                ElseIf ContextMenuSender Is Label25 Then
                    UseControl = CheckedListBox6
                ElseIf ContextMenuSender Is Label8 Then
                    UseControl = CheckedListBox4
                ElseIf ContextMenuSender Is Label13 Then
                    UseControl = CheckedListBox5
                End If

                For index As Integer = 0 To UseControl.Items.Count - 1
                    UseControl.SetItemChecked(index, False)
                Next
            Else
                'error unprogrammed contextmenusender
            End If
            'ElseIf e.Button = MouseButtons.Right Then
        End If
    End Sub

    'file controls
    Private Sub CheckBox1_Click(sender As System.Object, e As System.EventArgs) Handles CheckBox1.Click
        If sender.checked = False Then
            sender.checked = True
            'make new
            GroupBox3.Enabled = True
            ComboBox1.Enabled = False
            LastIndexOfFileNameList = ComboBox1.SelectedIndex
            ComboBox1.SelectedIndex = -1
            CheckBox6.Enabled = False
            GroupBox4.Enabled = True ' Extended Data GroupBox
            GroupBox5.Enabled = True ' Control Restriction Options GroupBox
        Else
            If DisplayDiscardChangesDialog() = True Then
                If MsgBox("Do you want to cancel creating the current item file?", MsgBoxStyle.YesNo, "Abort New File Creation?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
            sender.checked = False
            'cancel / end make new
            GroupBox3.Enabled = False
            ComboBox1.Enabled = True
            ComboBox1.SelectedIndex = LastIndexOfFileNameList
            LastIndexOfFileNameList = 0
            CheckBox6.Enabled = True
            RadioButton1.Checked = True ' No Equip Type
            RadioButton34.Checked = True ' No Attack Type
            GroupBox4.Enabled = False ' Extended Data GroupBox
            GroupBox5.Enabled = False ' Control Restriction Options GroupBox
            ClearForm()
        End If
    End Sub 'new button
    Private Sub CheckBox6_Click(sender As System.Object, e As System.EventArgs) Handles CheckBox6.Click
        If sender.Checked = False Then
            If ComboBox1.Text.Trim <> "" Then
                Kairen.ReinstantiateItemFile()
                RegisterUIElements()
                Select Case Kairen.LoadItemFile(ComboBox1.Text)
                    Case 0
                        sender.Checked = True
                        'load
                        'Label26.Visible = False ' hides NAME TAKEN label
                        GroupBox3.Enabled = True 'File Data Controls Group Box
                        ComboBox1.Enabled = False 'File List ComboBox
                        CheckBox1.Enabled = False 'New Button
                        Button6.Enabled = True 'Delete Button
                        GroupBox4.Enabled = True ' Extended Data GroupBox
                        GroupBox5.Enabled = True ' Control Restriction Options GroupBox
                        Exit Sub
                    Case -1
                        lb.DisplayMessage("Error: Item File could not be found.", "Error: Item Maker - Load", "Item Maker - Load Item")
                    Case -3
                        lb.DisplayMessage("Error: Item File Version out of date.", "Error: Item Maker - Load", "Item Maker - Load Item")
                    Case -6
                        lb.DisplayMessage("Error: Item File has portions of empty data.", "Error: Item Maker - Load", "Item Maker - Load Item")
                    Case -7
                        lb.DisplayMessage("Error: Item File Error - Error Alert -7.", "Error: Item Maker - Load", "Item Maker - Load Item")
                    Case Else
                        lb.DisplayMessage("Error: Item File Error - Error Alert Not Individualized.", "Error: Item Maker - Load", "Item Maker - Load Item")
                End Select
                ClearForm("All")
            End If
        Else
            If DisplayDiscardChangesDialog() = True Then
                If MsgBox("You have made unsaved changes to the loaded item file. Do you want to discard these changes?", MsgBoxStyle.YesNo, "Discard file changes?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If

            sender.checked = False
            'unload
            GroupBox3.Enabled = False
            ComboBox1.Enabled = True
            CheckBox1.Enabled = True
            Button6.Enabled = False
            RadioButton1.Checked = True ' No Equip Type
            RadioButton34.Checked = True ' No Attack Type
            GroupBox4.Enabled = False ' Extended Data GroupBox
            GroupBox5.Enabled = False ' Control Restriction Options GroupBox
            ClearForm()
            Kairen.ReinstantiateItemFile()
            RegisterUIElements()
            End If
    End Sub 'load button
    Private Sub Button5_Click(sender As System.Object, e As System.EventArgs) Handles Button5.Click
        If Label26.Visible Then
            lb.DisplayMessage("Alert: This Item name is already taken, save cancelled.", "Item Maker - Save Cancelled", "Item Maker - Save Item")
            Exit Sub
        End If
        Dim topass As String = TextBox6.Text
        If CheckBox6.Checked Then 'load button is checked
            If TextBox6.Text <> Kairen.ItemFile.GetDataByTag("nameSafe") Then
                If MsgBox("This will rename the file from " & Kairen.ItemFile.GetDataByTag("nameSafe") & " to " & TextBox6.Text & ", do you want to continue?", MsgBoxStyle.YesNo, "Rename Item File?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If
        Select Case Kairen.SaveItemFile(Nothing, topass)
            Case 0, 1
                'save successful
                lb.DisplayMessage("Success: Item File has been saved.", "Item Maker - Save Success", "Item Maker - Save Item")
                'changes from New state to Loaded state
                If CheckBox1.Checked Then
                    CheckBox1.Checked = False
                    CheckBox1.Enabled = False
                    CheckBox6.Checked = True
                    CheckBox6.Enabled = True
                    lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Items, lb.Extension_ItemFile), ComboBox1, True, True)
                    ComboBox1.SelectedItem = TextBox6.Text
                End If
                If topass <> Nothing Then
                    lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Items, lb.Extension_ItemFile), ComboBox1, True, True)
                    ComboBox1.SelectedItem = TextBox6.Text
                End If
            Case -1
                lb.DisplayMessage("Error: Item File has not been loaded to chnange.", "Error: Item Maker - Save", "Item Maker - Save Item")
            Case -2, -3
                lb.DisplayMessage("Error: Item File attempted to save empty data to the file.", "Error: Item Maker - Save", "Item Maker - Save Item")
            Case -4
                lb.DisplayMessage("Error: Item File Error - Error Alert Not Known.", "Error: Item Maker - Save", "Item Maker - Save Item")
            Case -5
                lb.DisplayMessage("Error: Item File you are updating does not exist.", "Error: Item Maker - Save", "Item Maker - Save Item")
            Case Else
                lb.DisplayMessage("Error: Item File Error - Error Alert Not Individualized.", "Error: Item Maker - Save", "Item Maker - Save Item")
        End Select
    End Sub 'save button
    Private Sub Button6_Click(sender As System.Object, e As System.EventArgs) Handles Button6.Click
        If MsgBox("This will delete the file " & Kairen.ItemFile.GetDataByTag("nameSafe") & ", are you sure you want to continue?", MsgBoxStyle.YesNo, "Delete this file?") = MsgBoxResult.Yes Then
            Kairen.ItemFile.DeleteFile()
            lb.AddItemsToControl(lb.ReturnFilesFromFolder(lb.Folder_Custom_Items, lb.Extension_ItemFile), ComboBox1, True, True)
            CheckBox6.Checked = False
            'unload
            GroupBox3.Enabled = False
            ComboBox1.Enabled = True
            CheckBox1.Enabled = True
            Button6.Enabled = False
            RadioButton1.Checked = True ' No Equip Type
            RadioButton34.Checked = True ' No Attack Type
            GroupBox4.Enabled = False ' Extended Data GroupBox
            GroupBox5.Enabled = False ' Control Restriction Options GroupBox
            ClearForm()
            Kairen.ReinstantiateItemFile()
            RegisterUIElements()
        End If
    End Sub 'delete button

    'form clearing, file checking, etc
    Private Function DisplayDiscardChangesDialog()
        'Checks if you made changes to the file to see if you want to save those changes of discard them
        Kairen.ItemFile.UpdateFileByUIElements()
        Return Kairen.ItemFile.FileHasChanged
        If TextBox5.Text <> "" Then
            Return True
        End If
        Return False
    End Function
    Private Sub ClearForm(Optional ByVal _clearAction As String = "All")
        'Clears the form when appropriate.. but maybe the textfileclass should do this automatically when it unloads.
        Select Case _clearAction
            Case "All"
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox3.Text = ""
                TextBox4.Text = ""
                TextBox5.Text = ""
                TextBox6.Text = ""
                TextBox7.Text = ""
                TextBox8.Text = ""
                TextBox9.Text = ""
                TextBox10.Text = ""
                TextBox11.Text = ""
                TextBox12.Text = ""
                TextBox13.Text = ""
                TextBox14.Text = ""
                TextBox15.Text = ""
                TextBox16.Text = ""
                For Each item In CheckedListBox1.CheckedIndices
                    CheckedListBox1.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox2.CheckedIndices
                    CheckedListBox2.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox3.CheckedIndices
                    CheckedListBox3.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox4.CheckedIndices
                    CheckedListBox4.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox5.CheckedIndices
                    CheckedListBox5.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox6.CheckedIndices
                    CheckedListBox6.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox7.CheckedIndices
                    CheckedListBox7.SetItemChecked(item, False)
                Next
                CheckedListBox1.SelectedIndex = 0
                CheckedListBox2.SelectedIndex = 0
                CheckedListBox3.SelectedIndex = 0
                CheckedListBox4.SelectedIndex = 0
                CheckedListBox5.SelectedIndex = 0
                CheckedListBox6.SelectedIndex = 0
                CheckedListBox7.SelectedIndex = 0
                CheckedListBox1.SelectedIndex = -1
                CheckedListBox2.SelectedIndex = -1
                CheckedListBox3.SelectedIndex = -1
                CheckedListBox4.SelectedIndex = -1
                CheckedListBox5.SelectedIndex = -1
                CheckedListBox6.SelectedIndex = -1
                CheckedListBox7.SelectedIndex = -1
                ComboBox2.SelectedIndex = 0
                ComboBox3.SelectedIndex = 0
                CheckBox3.Checked = False
                CheckBox4.Checked = False
                RadioButton50.Checked = True ' Equip Type Not Set
                RadioButton51.Checked = True ' Attack Type Not Set

            Case "Most"
                TextBox1.Text = ""
                TextBox2.Text = ""
                TextBox4.Text = ""
                TextBox8.Text = ""
                TextBox9.Text = ""
                TextBox10.Text = ""
                TextBox11.Text = ""
                TextBox12.Text = ""
                TextBox13.Text = ""
                TextBox14.Text = ""
                TextBox15.Text = ""
                TextBox16.Text = ""
                For Each item In CheckedListBox1.CheckedIndices
                    CheckedListBox1.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox2.CheckedIndices
                    CheckedListBox2.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox3.CheckedIndices
                    CheckedListBox3.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox4.CheckedIndices
                    CheckedListBox4.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox5.CheckedIndices
                    CheckedListBox5.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox6.CheckedIndices
                    CheckedListBox6.SetItemChecked(item, False)
                Next
                For Each item In CheckedListBox7.CheckedIndices
                    CheckedListBox7.SetItemChecked(item, False)
                Next
                CheckedListBox1.SelectedIndex = 0
                CheckedListBox2.SelectedIndex = 0
                CheckedListBox3.SelectedIndex = 0
                CheckedListBox4.SelectedIndex = 0
                CheckedListBox5.SelectedIndex = 0
                CheckedListBox6.SelectedIndex = 0
                CheckedListBox7.SelectedIndex = 0
                CheckedListBox1.SelectedIndex = -1
                CheckedListBox2.SelectedIndex = -1
                CheckedListBox3.SelectedIndex = -1
                CheckedListBox4.SelectedIndex = -1
                CheckedListBox5.SelectedIndex = -1
                CheckedListBox6.SelectedIndex = -1
                CheckedListBox7.SelectedIndex = -1
                ComboBox2.SelectedIndex = 0
                ComboBox3.SelectedIndex = 0
                CheckBox3.Checked = False
                CheckBox4.Checked = False
                RadioButton50.Checked = True ' Equip Type Not Set
                RadioButton51.Checked = True ' Attack Type Not Set
            Case Else
                'error incorrect _clearAction specified
                ClearForm("All")
        End Select
    End Sub
    Private Sub TextBox6_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox6.TextChanged
        If CheckBox1.Checked = False And CheckBox6.Checked = False Then 'if not new or loaded then do not warn
            'Label26.Text = "NAME TAKEN"
            Label26.Visible = False
        Else
            'Label26.Text = "NAME TAKEN"
            If CheckBox1.Checked = False And TextBox6.Text = ComboBox1.Text Then
                Label26.Visible = False
                Exit Sub
            End If

            If lb.FE(lb.Folder_Custom_Items & TextBox6.Text & lb.Extension_ItemFile) Then
                'if file exists in custom folder
                Label26.Visible = True
            Else
                'if file doesn't exist in custom folder
                '   If TextBox6.Text.Length > 0 And TextBox6.Text.Trim = "" Then
                ''if file name is blank
                'Label26.Text = "NAME INVALID"
                'Label26.Visible = True
                'Else
                'if file name isn't blank
                Label26.Visible = False
                'End If
            End If
        End If

    End Sub 'displays NAME TAKEN label

    'file functions
    Private Sub RegisterUIElements()
        Kairen.ItemFile.RegisterUIElement("control restriction options", Nothing, AddressOf DataParserFunction_RadioButtons)
        Kairen.ItemFile.RegisterUIElement("nameSafe", TextBox6)
        Kairen.ItemFile.RegisterUIElement("nameGame", TextBox5)
        Kairen.ItemFile.RegisterUIElement("Inventory Category", Nothing, AddressOf DataParserFunction_RadioButtons)
        Kairen.ItemFile.RegisterUIElement("Value", TextBox7)
        Kairen.ItemFile.RegisterUIElement("Max Stack", TextBox16)
        Kairen.ItemFile.RegisterUIElement("Charges", TextBox15)
        Kairen.ItemFile.RegisterUIElement("Level", TextBox2)
        Kairen.ItemFile.RegisterUIElement("Item HP", TextBox1)
        Kairen.ItemFile.RegisterUIElement("Durability", TextBox4)
        Kairen.ItemFile.RegisterUIElement("Icon", ComboBox3) '.SelectedItem
        Kairen.ItemFile.RegisterUIElement("Graphic", ComboBox2) '.SelectedItem
        'Kairen.ItemFile.RegisterUIElement("gr1", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("gr2", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("gr3", Nothing, AddressOf DataParserFunction_UnusedTag)
        Kairen.ItemFile.RegisterUIElement("RED", TextBox11)
        Kairen.ItemFile.RegisterUIElement("GREEN", TextBox12)
        Kairen.ItemFile.RegisterUIElement("BLUE", TextBox10)
        'Kairen.ItemFile.RegisterUIElement("RGBFF", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("ukrgb1", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("ukrgb2", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("ukrgb3", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("ukrgb4", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("ukrgb5", Nothing, AddressOf DataParserFunction_UnusedTag)
        Kairen.ItemFile.RegisterUIElement("NO RENT", CheckedListBox5)
        Kairen.ItemFile.RegisterUIElement("NO TRADE", CheckedListBox5)
        Kairen.ItemFile.RegisterUIElement("LORE", CheckedListBox5)
        Kairen.ItemFile.RegisterUIElement("CRAFTABLE", CheckedListBox5)
        Kairen.ItemFile.RegisterUIElement("Dark Elf", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Troll", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Ogre", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Elf", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Dwarf", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Gnome", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Halfling", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Erudite", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Human", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Barbarian", CheckedListBox1)
        Kairen.ItemFile.RegisterUIElement("Warrior", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Paladin", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Shadowknight", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Enchanter", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Magician", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Wizard", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Alchemist", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Necromancer", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Monk", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Rogue", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Ranger", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Bard", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Druid", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Shaman", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Cleric", CheckedListBox2)
        Kairen.ItemFile.RegisterUIElement("Strength", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Stamina", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Agility", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Dexterity", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Wisdom", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Intelligence", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Charisma", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("FR", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("CR", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("LR", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("AR", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("PR", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("DR", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("HoT", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("PoT", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("HPMAX", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("POWMAX", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("AC", CheckedListBox3, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Def Mod", CheckedListBox6, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Off Mod", CheckedListBox6, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("HP Factor", CheckedListBox6, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Movement Rate", CheckedListBox6, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Fishing", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Jewel Crafting", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Armor Crafting", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Weapon Crafting", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Tailoring", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Alchemy", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Carpentry", CheckedListBox7, AddressOf DataParserFunction_ValuedCheckedListBox)
        Kairen.ItemFile.RegisterUIElement("Description", TextBox3)
        Kairen.ItemFile.RegisterUIElement("Equip Type", Nothing, AddressOf DataParserFunction_RadioButtons)
        Kairen.ItemFile.RegisterUIElement("Attack Damage", TextBox9)
        Kairen.ItemFile.RegisterUIElement("Attack Type", Nothing, AddressOf DataParserFunction_RadioButtons)
        'Kairen.ItemFile.RegisterUIElement("uv1", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv2", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv3", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv4", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv5", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv6", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv7", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv8", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv9", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv10", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv11", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv12", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("uv13", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("fut1", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("fut2", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("fut3", Nothing, AddressOf DataParserFunction_UnusedTag)
        Kairen.ItemFile.RegisterUIElement("Pattern Family", TextBox14)
        'Kairen.ItemFile.RegisterUIElement("pattern res1", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("pattern res2", Nothing, AddressOf DataParserFunction_UnusedTag)
        Kairen.ItemFile.RegisterUIElement("Quest Number", TextBox8)
        'Kairen.ItemFile.RegisterUIElement("qr1", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("qr2", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("qr3", Nothing, AddressOf DataParserFunction_UnusedTag)
        'Kairen.ItemFile.RegisterUIElement("Effects", CheckedListBox4, AddressOf DataParserFunction_ProcsCheckedListBox, False, "Multiple Lines") changed to below..
        Kairen.ItemFile.RegisterUIElement("Processess", CheckedListBox4, AddressOf DataParserFunction_ProcsCheckedListBox, False, "Multiple Lines")

        Kairen.ItemFile.UIElementsRegistered = True
    End Sub
    Public Function DataParserFunction_ProcsCheckedListBox(ByVal _dataTag As String, ByRef _dataObject As Object, ByRef _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        If TypeOf _dataObject Is CheckedListBox = False Then
            'error wrong control, return raw data
            Return _rawData
        End If
        Dim ReturnData(_dataObject.CheckedItems.Count - 1) As String
        Dim i As Integer = 0
        If _rawData IsNot Nothing Then
            If TypeOf (_rawData) Is Array Then
                If _rawData.Length = 0 Then
                    Return "False"
                End If
            End If
            If _rawData(0) = "False" Then Return Nothing
            For Each dataPiece In _rawData
                i = 0
                Do Until i >= _dataObject.Items.Count
                    If _dataObject.Items.Item(i) = dataPiece Then
                        _dataObject.SetItemCheckState(i, CheckState.Checked)
                        Exit Do
                    End If
                    i += 1
                Loop
            Next
            Return _rawData
        Else
            For Each item In _dataObject.CheckedItems
                ReturnData(i) = CStr(item)
                i += 1
            Next
            If i = 0 Then
                ReDim ReturnData(0)
                ReturnData(0) = "False"
            End If
            Return ReturnData
        End If
        'compare each string in array _rawData to each item in _dataObject and if they match, checked the item in _dataObject
    End Function 'parse Checked List Box
    Public Function DataParserFunction_ValuedCheckedListBox(ByVal _dataTag As String, ByRef _dataObject As Object, ByRef _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        If TypeOf _dataObject Is CheckedListBox = False Then
            'error wrong control, return raw data
            Return _rawData
        End If
        Dim ReturnData As String = _rawData
        Dim ModAmount As Integer
        Dim ControlItem_String As String
        Dim ControlItem_Index As Integer = -1
        Dim i As Integer
        i = 0
        Do Until i >= _dataObject.Items.Count
            If _dataObject.Items.Item(i).Contains("+") Then
                ControlItem_String = Microsoft.VisualBasic.Left(_dataObject.Items.Item(i), _dataObject.items.Item(i).IndexOf("+") - 1)
                'If ControlItem_String <> _dataTag Then ControlItem_String = _dataTag
                ModAmount = Microsoft.VisualBasic.Right(_dataObject.items.Item(i), _dataObject.items.Item(i).Length - _dataObject.items.Item(i).IndexOf("+") - 2)
            ElseIf _dataObject.Items.Item(i).Contains("-") Then
                ControlItem_String = Microsoft.VisualBasic.Left(_dataObject.items.Item(i), _dataObject.items.Item(i).IndexOf("-") - 1)
                'If ControlItem_String <> _dataTag Then ControlItem_String = _dataTag1
                ModAmount = Microsoft.VisualBasic.Right(_dataObject.items.Item(i), _dataObject.items.Item(i).Length - _dataObject.items.Item(i).IndexOf("-")).Replace(" ", "")
            Else
                ControlItem_String = _dataObject.Items.Item(i)
            End If
            If ControlItem_String = _dataTag Then
                ControlItem_Index = i
                ControlItem_String = _dataObject.Items.Item(ControlItem_Index)
                Exit Do
            Else
                ModAmount = 0
            End If
            i += 1
        Loop
        If _rawData IsNot Nothing Then 'setting control - convert file to control
            If TypeOf (_rawData) Is Array Then
                If _rawData.Length = 0 Then
                    ReturnData = "False"
                End If
            End If
            If _rawData = "False" Then
                '_dataControl.Text = ""
                'ReturnData = _dataControl.Text
            Else
                If AlterCheckedListBoxItemsStatMod(_dataObject.Items.Item(ControlItem_Index), _rawData) = 0 Then
                    _dataObject.SetItemCheckState(ControlItem_Index, CheckState.Checked)
                End If
                ReturnData = _rawData
            End If
        Else 'getting control - convert control to file
            If _dataObject.Items.Item(ControlItem_Index).trim = _dataTag Then
                ReturnData = "False"
            Else
                ReturnData = ModAmount
            End If
        End If
        Return ReturnData
    End Function 'parse Valued Checked List Box
    Public Function DataParserFunction_RadioButtons(ByVal _dataTag As String, ByRef _dataControl As Object, ByRef _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        If _rawData Is Nothing Then
            'finding file data and setting it to appropriate control
            GetRadioButtonByDataName(_dataTag, _dataControl, _rawData) 'updates _rawData
            If _rawData = "True" Then
                _dataControl.Checked = True
            ElseIf _rawData = "False" Then
                _dataControl.Checked = False
            Else
                '_dataControl.Checked = False
            End If

        ElseIf _dataControl Is Nothing Then
            'finding control and setting it's data to the file
            GetRadioButtonByDataName(_dataTag, _dataControl, _rawData) 'updates _dataControl
            If _rawData <> "True" And _rawData <> "False" Then
                _dataControl.Checked = True
            ElseIf _dataControl.Checked = False Then
                _rawData = "False"
            ElseIf _dataControl.Checked = True Then
                _rawData = "True"
            Else
                _rawData = "True"
            End If
        Else
            If 1 = 2 Then Return 3984789
            'error
        End If
        Return _rawData
    End Function 'parse Radio Button
    Public Function DataParserFunction_UnusedTag(ByVal _dataTag As String, ByRef _dataControl As Object, ByRef _rawData As Object)
        If _rawData IsNot Nothing Then
            'if saving the file, then
            Return ""
        Else
            Return "[Blank Line]"
        End If
    End Function 'parse Unused

#End Region
#Region "Old Code"
    Private Sub CheckBox2_Click(sender As System.Object, e As System.EventArgs)
        If sender.Checked = False Then
            If ComboBox1.Text.Trim <> "" Then
                If lb.FE(lb.Folder_Custom_Items & ComboBox1.Text & lb.Extension_ItemFile) Then
                    sender.Checked = True
                    'load
                    GroupBox3.Enabled = True
                    ComboBox1.Enabled = False
                    CheckBox1.Enabled = False
                    'Button2.Enabled = True
                    GroupBox4.Enabled = True ' Extended Data GroupBox
                    GroupBox5.Enabled = True ' Control Restriction Options GroupBox
                    ReadItemData()
                Else
                    lb.DisplayMessage("Error: Item File could not be found.", "Error: Item Maker", "Item Maker - Save Item")
                End If
            End If
        Else
            If DisplayDiscardChangesDialog() = True Then
                If MsgBox("You have made unsaved changes to the loaded item file. Do you want to discard these changes?", MsgBoxStyle.YesNo, "Discard file changes?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If

            sender.checked = False
            'unload
            GroupBox3.Enabled = False
            ComboBox1.Enabled = True
            CheckBox1.Enabled = True
            'Button2.Enabled = False
            RadioButton1.Checked = True ' No Equip Type
            RadioButton34.Checked = True ' No Attack Type
            GroupBox4.Enabled = False ' Extended Data GroupBox
            GroupBox5.Enabled = False ' Control Restriction Options GroupBox
            ClearForm()
        End If
    End Sub 'load button #1
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs)
        'save
        If CanSaveCheck() = True Then
            If CheckBox1.Checked = True Then
                'save new file
                SaveSequence()
                'ElseIf CheckBox2.Checked = True Then
                '    'save loaded file
                '    Kairen.ItemFile.UpdateFileByUIElements()
                '    'MsgBox("old save succeeded", MsgBoxStyle.Information, "old file did save")
            Else
                MsgBox("error - save failed - no mode selected", MsgBoxStyle.Information, "could not save")
            End If
        Else
            MsgBox("save failed", MsgBoxStyle.Information, "could not save")
        End If
    End Sub 'save button #1
    Private Sub CheckBox5_Click(sender As System.Object, e As System.EventArgs)
        If sender.Checked = False Then
            If ComboBox1.Text.Trim <> "" Then
                If lb.FE(lb.Folder_Custom_Items & ComboBox1.Text & lb.Extension_ItemFile) Then
                    sender.Checked = True
                    'load
                    GroupBox3.Enabled = True
                    ComboBox1.Enabled = False
                    CheckBox1.Enabled = False
                    'Button2.Enabled = True
                    GroupBox4.Enabled = True ' Extended Data GroupBox
                    GroupBox5.Enabled = True ' Control Restriction Options GroupBox
                    'ReadItemData()
                    Kairen.LoadItemFile3(ComboBox1.Text)
                    Kairen.ItemFile.UpdateUIElementsByFile()
                Else
                    lb.DisplayMessage("Error: Item File could not be found.", "Error: Item Maker", "Item Maker - Save Item")
                End If
            End If
        Else
            If DisplayDiscardChangesDialog() = True Then
                If MsgBox("You have made unsaved changes to the loaded item file. Do you want to discard these changes?", MsgBoxStyle.YesNo, "Discard file changes?") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If

            sender.checked = False
            'unload
            GroupBox3.Enabled = False
            ComboBox1.Enabled = True
            CheckBox1.Enabled = True
            'Button2.Enabled = False
            RadioButton1.Checked = True ' No Equip Type
            RadioButton34.Checked = True ' No Attack Type
            GroupBox4.Enabled = False ' Extended Data GroupBox
            GroupBox5.Enabled = False ' Control Restriction Options GroupBox
            ClearForm()
        End If
    End Sub 'load button #2
    Private Sub Button4_Click(sender As System.Object, e As System.EventArgs)
        'save
        If CanSaveCheck() = True Then
            If CheckBox1.Checked = True Then
                'save new file
                SaveSequence()
                'ElseIf CheckBox2.Checked = True Then
                '    'save loaded file
                '    Kairen.ItemFile.UpdateFileByUIElements()
                '    'MsgBox("old save succeeded", MsgBoxStyle.Information, "old file did save")
            Else
                MsgBox("error - save failed - no mode selected", MsgBoxStyle.Information, "could not save")
            End If
        Else
            MsgBox("save failed", MsgBoxStyle.Information, "could not save")
        End If
    End Sub 'save button #2
    Private Sub CheckedListBox6_ItemCheck(sender As System.Object, e As System.Windows.Forms.ItemCheckEventArgs) 'Handles CheckedListBox6.ItemCheck
        If e.NewValue = CheckState.Checked Then
            Dim result As String = InputBox("Enter Stat Increase/Decrease:", "Enter Stat Modification Amount").Trim
            Try
                If CInt(result) Then
                End If
            Catch ex As Exception
                e.NewValue = CheckState.Unchecked
                Exit Sub
            End Try
            If CInt(result) >= 0 Then
                CheckedListBox6.Items(e.Index) = CheckedListBox6.Items(e.Index) & " + " & result
            ElseIf CInt(result) < 0 Then
                result = result.Replace("-", "").Trim
                CheckedListBox6.Items(e.Index) = CheckedListBox6.Items(e.Index) & " - " & result
            End If
        Else
            If CheckedListBox6.Items(e.Index).Contains("+") Then
                CheckedListBox6.Items(e.Index) = Microsoft.VisualBasic.Left(CheckedListBox6.Items(e.Index), CheckedListBox6.Items(e.Index).IndexOf("+") - 1)
            ElseIf CheckedListBox6.Items(e.Index).Contains("-") Then
                CheckedListBox6.Items(e.Index) = Microsoft.VisualBasic.Left(CheckedListBox6.Items(e.Index), CheckedListBox6.Items(e.Index).IndexOf("-") - 1)
            End If
        End If
    End Sub
    Private Sub RadioButton1_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton1.CheckedChanged
        'EQUIP TYPE NONE
        If sender.Checked = True Then
            RadioButton34.Enabled = True ' No Attack
            RadioButton35.Enabled = False ' 1 HS
            RadioButton36.Enabled = False ' 1 HB
            RadioButton37.Enabled = False ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton19_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton19.CheckedChanged
        'EQUIP TYPE HELD
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = True ' No Attack
            RadioButton35.Enabled = False ' 1 HS
            RadioButton36.Enabled = False ' 1 HB
            RadioButton37.Enabled = False ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton13_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton13.CheckedChanged
        'EQUIP TYPE PRIMARY
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = False ' No Attack
            RadioButton35.Enabled = True ' 1 HS
            RadioButton36.Enabled = True ' 1 HB
            RadioButton37.Enabled = True ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton14_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton14.CheckedChanged
        'EQUIP TYPE SHIELD
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = True ' No Attack
            RadioButton35.Enabled = False ' 1 HS
            RadioButton36.Enabled = False ' 1 HB
            RadioButton37.Enabled = False ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton15_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton15.CheckedChanged
        'EQUIP TYPE SECONDARY
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = False ' No Attack
            RadioButton35.Enabled = True ' 1 HS
            RadioButton36.Enabled = True ' 1 HB
            RadioButton37.Enabled = True ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton16_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton16.CheckedChanged
        'EQUIP TYPE 2HAND
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = False ' No Attack
            RadioButton35.Enabled = False ' 1 HS
            RadioButton36.Enabled = False ' 1 HB
            RadioButton37.Enabled = False ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = True ' 2 HS
            RadioButton40.Enabled = True ' 2 HB
            RadioButton41.Enabled = True ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton17_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton17.CheckedChanged
        'EQUIP TYPE BOW
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = False ' No Attack
            RadioButton35.Enabled = False ' 1 HS
            RadioButton36.Enabled = False ' 1 HB
            RadioButton37.Enabled = False ' 1 HP
            RadioButton38.Enabled = True ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = True ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = False ' THROWN
        End If
    End Sub
    Private Sub RadioButton18_CheckedChanged(sender As System.Object, e As System.EventArgs) 'Handles RadioButton18.CheckedChanged
        'EQUIP TYPE THROWN
        If sender.Checked = True Then
            RadioButton34.Checked = True ' No Attack

            RadioButton34.Enabled = False ' No Attack
            RadioButton35.Enabled = False ' 1 HS
            RadioButton36.Enabled = False ' 1 HB
            RadioButton37.Enabled = False ' 1 HP
            RadioButton38.Enabled = False ' BOW
            RadioButton39.Enabled = False ' 2 HS
            RadioButton40.Enabled = False ' 2 HB
            RadioButton41.Enabled = False ' 2 HP
            RadioButton42.Enabled = False ' 2 HCROSSBOW
            RadioButton43.Enabled = False ' 1 HCROSSBOW
            RadioButton44.Enabled = True ' THROWN
        End If
    End Sub
    Private Property EmptyBlankData(Optional ByVal _additionaldata As String = Nothing, Optional ByVal _control As Object = Nothing)
        Get
            Return Nothing
        End Get
        Set(value)
            If _additionaldata = Nothing Then Exit Property
            If TypeOf (_control) Is CheckedListBox Then
                Dim i As Integer
                Do Until i = _control.Items.Count - 1
                    If _control.Items.Item(i) = _additionaldata Then
                        If AlterCheckedListBoxItemsStatMod(_control.Items.Item(i), EmptyOutBlankDatas(_additionaldata, _control)) = 0 Then
                            _control.SetItemCheckState(i, CheckState.Checked)
                        End If
                        Exit Do
                    End If
                    i += 1
                Loop
            Else
                EmptyOutBlankDatas(_additionaldata, _control)
            End If
        End Set
    End Property
    Public Function DataParserFunction2(ByVal _dataTag As String, ByRef _dataControl As Object, ByVal _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        Dim ReturnData As String = _rawData
        'this is out here in case different checked list boxes need different handling procedures
        Dim ControlItem_Index As Integer = -1
        If TypeOf _dataControl Is CheckedListBox Then
            Dim i As Integer
            Do Until i = _dataControl.Items.Count
                If _dataControl.Items.Item(i) = _dataTag Then
                    ControlItem_Index = i
                    Exit Do
                End If
                i += 1
            Loop
        End If
        If _dataTag = "Strength" Then
            If 1 = 2 Then Return -184389493
        End If
        If _dataControl Is CheckedListBox3 Or _dataControl Is CheckedListBox6 Then
            If _rawData IsNot Nothing Then 'setting control - convert file to control
                If _rawData = "False" Then
                    '_dataControl.Text = ""
                    'ReturnData = _dataControl.Text
                Else
                    If AlterCheckedListBoxItemsStatMod(_dataControl.Items.Item(ControlItem_Index), _rawData) = 0 Then
                        _dataControl.SetItemCheckState(ControlItem_Index, CheckState.Checked)
                    End If
                    ReturnData = _rawData
                    End If
            Else 'getting control - convert control to file
                    If _dataControl.Text.trim = "" Then
                        ReturnData = "False"
                    Else
                        ReturnData = EmptyOutBlankDatas(_dataTag, _dataControl)
                    End If
            End If
        Else
            Return Kairen.DataParserFunction(_dataTag, _dataControl, _rawData)
        End If
        Return ReturnData
    End Function
    Public Sub ReadItemData()
        Kairen.LoadItemFile3(ComboBox1.Text)
        'control restriction options
        'GetRadioButtonByDataName(EmptyOutBlankDatas("control restriction options")).Checked = True
        TextBox6.Text = EmptyOutBlankDatas("nameSafe")
        'game name
        TextBox5.Text = EmptyOutBlankDatas("nameGame")
        'GetRadioButtonByDataName(EmptyOutBlankDatas("Inventory Category")).Checked = True
        'value
        TextBox7.Text = EmptyOutBlankDatas("Value")
        'max stack
        TextBox16.Text = EmptyOutBlankDatas("Max Stack")
        'level
        TextBox2.Text = EmptyOutBlankDatas("Level")
        'item hp
        TextBox1.Text = EmptyOutBlankDatas("Item HP")
        'durability
        TextBox4.Text = EmptyOutBlankDatas("Durability")
        'icon
        ComboBox3.SelectedItem = EmptyOutBlankDatas("Icon")
        'graphic 1
        ComboBox2.SelectedItem = EmptyOutBlankDatas("Graphic")
        EmptyOutBlankDatas("gr1")
        'gr1
        EmptyOutBlankDatas("gr1")
        'gr2
        EmptyOutBlankDatas("gr2")
        'gr3
        EmptyOutBlankDatas("gr3")
        'red
        TextBox11.Text = EmptyOutBlankDatas("RED")
        'green
        TextBox12.Text = EmptyOutBlankDatas("GREEN")
        'blue
        TextBox10.Text = EmptyOutBlankDatas("BLUE")
        'rgbff
        EmptyOutBlankDatas("RGBFF")
        'ukrgb1
        EmptyOutBlankDatas("ukrgb1")
        'ukrgb2
        EmptyOutBlankDatas("ukrgb2")
        'ukrgb3
        EmptyOutBlankDatas("ukrgb3")
        'ukrgb4
        EmptyOutBlankDatas("ukrgb4")
        'ukrgb5
        EmptyOutBlankDatas("ukrgb5")
        ' NO RENT
        SetItemCheckStateInCheckedListBox("NO RENT", CheckedListBox5)
        ' NO TRADE
        SetItemCheckStateInCheckedListBox("NO TRADE", CheckedListBox5)
        ' LORE
        SetItemCheckStateInCheckedListBox("LORE", CheckedListBox5)
        ' CRAFTABLE
        SetItemCheckStateInCheckedListBox("CRAFTABLE", CheckedListBox5)

        ' dark elf
        SetItemCheckStateInCheckedListBox("Dark Elf", CheckedListBox1)
        ' troll
        SetItemCheckStateInCheckedListBox("Troll", CheckedListBox1)
        ' ogre
        SetItemCheckStateInCheckedListBox("Ogre", CheckedListBox1)
        ' elf
        SetItemCheckStateInCheckedListBox("Elf", CheckedListBox1)
        ' dwarf
        SetItemCheckStateInCheckedListBox("Dwarf", CheckedListBox1)
        ' gnome
        SetItemCheckStateInCheckedListBox("Gnome", CheckedListBox1)
        ' halfling
        SetItemCheckStateInCheckedListBox("Halfling", CheckedListBox1)
        ' erudite
        SetItemCheckStateInCheckedListBox("Erudite", CheckedListBox1)
        ' human
        SetItemCheckStateInCheckedListBox("Human", CheckedListBox1)
        ' barbarian
        SetItemCheckStateInCheckedListBox("Barbarian", CheckedListBox1)

        ' warrior
        SetItemCheckStateInCheckedListBox("Warrior", CheckedListBox2)
        ' paladin
        SetItemCheckStateInCheckedListBox("Paladin", CheckedListBox2)
        ' shadowknight
        SetItemCheckStateInCheckedListBox("Shadowknight", CheckedListBox2)
        ' enchanter
        SetItemCheckStateInCheckedListBox("Enchanter", CheckedListBox2)
        ' magician
        SetItemCheckStateInCheckedListBox("Magician", CheckedListBox2)
        ' wizard
        SetItemCheckStateInCheckedListBox("Wizard", CheckedListBox2)
        ' alchemist
        SetItemCheckStateInCheckedListBox("Alchemist", CheckedListBox2)
        ' necromancer
        SetItemCheckStateInCheckedListBox("Necromancer", CheckedListBox2)
        ' monk
        SetItemCheckStateInCheckedListBox("Monk", CheckedListBox2)
        ' rogue
        SetItemCheckStateInCheckedListBox("Rogue", CheckedListBox2)
        ' ranger
        SetItemCheckStateInCheckedListBox("Ranger", CheckedListBox2)
        ' bard
        SetItemCheckStateInCheckedListBox("Bard", CheckedListBox2)
        ' druid
        SetItemCheckStateInCheckedListBox("Druid", CheckedListBox2)
        ' shaman
        SetItemCheckStateInCheckedListBox("Shaman", CheckedListBox2)
        ' cleric
        SetItemCheckStateInCheckedListBox("Cleric", CheckedListBox2)

        ' strength
        SetItemModificationAmountInCheckedListBox("Strength", CheckedListBox3)
        ' stamina
        SetItemModificationAmountInCheckedListBox("Stamina", CheckedListBox3)
        ' agility
        SetItemModificationAmountInCheckedListBox("Agility", CheckedListBox3)
        ' dexterity
        SetItemModificationAmountInCheckedListBox("Dexterity", CheckedListBox3)
        ' wisdom
        SetItemModificationAmountInCheckedListBox("Wisdom", CheckedListBox3)
        ' intelligence
        SetItemModificationAmountInCheckedListBox("Intelligence", CheckedListBox3)
        ' charisma
        SetItemModificationAmountInCheckedListBox("Charisma", CheckedListBox3)
        ' fr
        SetItemModificationAmountInCheckedListBox("FR", CheckedListBox3)
        ' cr
        SetItemModificationAmountInCheckedListBox("CR", CheckedListBox3)
        ' lr
        SetItemModificationAmountInCheckedListBox("LR", CheckedListBox3)
        ' ar
        SetItemModificationAmountInCheckedListBox("AR", CheckedListBox3)
        ' pr
        SetItemModificationAmountInCheckedListBox("PR", CheckedListBox3)
        ' dr
        SetItemModificationAmountInCheckedListBox("DR", CheckedListBox3)
        ' hot
        SetItemModificationAmountInCheckedListBox("HoT", CheckedListBox3)
        ' pot
        SetItemModificationAmountInCheckedListBox("PoT", CheckedListBox3)
        ' hpmax
        SetItemModificationAmountInCheckedListBox("HPMAX", CheckedListBox3)
        ' powmax
        SetItemModificationAmountInCheckedListBox("POWMAX", CheckedListBox3)
        ' ac
        SetItemModificationAmountInCheckedListBox("AC", CheckedListBox3)

        ' defmod
        SetItemModificationAmountInCheckedListBox("Def Mod", CheckedListBox6)
        ' off mod
        SetItemModificationAmountInCheckedListBox("Off Mod", CheckedListBox6)
        ' hp factor
        SetItemModificationAmountInCheckedListBox("HP Factor", CheckedListBox6)
        ' movement rate
        SetItemModificationAmountInCheckedListBox("Movement Rate", CheckedListBox6)
        ' fishing
        SetItemModificationAmountInCheckedListBox("Fishing", CheckedListBox6)
        ' jewel crafting
        SetItemModificationAmountInCheckedListBox("Jewel Crafting", CheckedListBox6)
        ' armor crafting
        SetItemModificationAmountInCheckedListBox("Armor Crafting", CheckedListBox6)
        ' weapon crafting
        SetItemModificationAmountInCheckedListBox("Weapon Crafting", CheckedListBox6)
        ' tailoring
        SetItemModificationAmountInCheckedListBox("Tailoring", CheckedListBox6)
        ' alchemy
        SetItemModificationAmountInCheckedListBox("Alchemy", CheckedListBox6)
        ' carpentry
        SetItemModificationAmountInCheckedListBox("Carpentry", CheckedListBox6)

        'description
        TextBox3.Text = EmptyOutBlankDatas("Description")
        'equip type
        'GetRadioButtonByDataName(EmptyOutBlankDatas("Equip Type")).Checked = True
        'attack damage value
        EmptyOutBlankDatas("Attack Damage")
        'attack damage type
        If EmptyOutBlankDatas("Damage Type").trim <> "" Then
            '    GetRadioButtonByDataName(EmptyOutBlankDatas("Damage Type")).Checked = True
        End If
        'unknown value 1
        EmptyOutBlankDatas("uv1")
        'unknown value 2
        EmptyOutBlankDatas("uv2")
        'unknown value 3
        EmptyOutBlankDatas("uv3")
        'unknown value 4
        EmptyOutBlankDatas("uv4")
        'unknown value 5
        EmptyOutBlankDatas("uv5")
        'unknown value 6
        EmptyOutBlankDatas("uv6")
        'unknown value 7
        EmptyOutBlankDatas("uv7")
        'unknown value 8
        EmptyOutBlankDatas("uv8")
        'unknown value 9
        EmptyOutBlankDatas("uv9")
        'unknown value 10
        EmptyOutBlankDatas("uv10")
        'unknown value 11
        EmptyOutBlankDatas("uv11")
        'unknown value 12
        EmptyOutBlankDatas("uv12")
        'unknown value 13
        EmptyOutBlankDatas("uv13")
        'fut1
        EmptyOutBlankDatas("fut1")
        'fut2
        EmptyOutBlankDatas("fut2")
        'fut3
        EmptyOutBlankDatas("fut3")
        TextBox14.Text = EmptyOutBlankDatas("pattern family")
        EmptyOutBlankDatas("pattern res1")
        EmptyOutBlankDatas("pattern res2")
        'quest catelog number
        TextBox8.Text = EmptyOutBlankDatas("Quest Number")
        'quest res 1
        EmptyOutBlankDatas("qr1")
        'quest res 2
        EmptyOutBlankDatas("qr2")
        'quest res 3
        EmptyOutBlankDatas("qr3")
        Dim itemeffect() As String = Kairen.GetEffectsFromItemFile()
        For i = 0 To CheckedListBox4.Items.Count - 1
            For Each line In itemeffect
                If CheckedListBox4.Items.Item(i).ToString = line Then
                    CheckedListBox4.SetItemCheckState(i, CheckState.Checked)
                End If
            Next
        Next
    End Sub
    Public Function DataParserFunction1(ByVal _additionaldata As String, ByRef _control As Object, ByVal _result As String)
        Dim ControlItem_Index As Integer = -1
        If TypeOf (_control) Is CheckedListBox Then
            Dim i As Integer
            Do Until i = _control.Items.Count
                If _control.Items.Item(i) = _additionaldata Then
                    ControlItem_Index = i
                    Exit Do
                End If
                i += 1
            Loop
        End If
        Dim ReturnThing As Object
        If _control Is CheckedListBox3 Or _control Is CheckedListBox6 Then
            If _result Is Nothing Then
            Else
                If _result = "False" Then
                    '_control.SetItemCheckState(ControlItem_Index, CheckState.Unchecked)
                Else
                    If AlterCheckedListBoxItemsStatMod(_control.Items.Item(ControlItem_Index), _result) = 0 Then
                        _control.SetItemCheckState(ControlItem_Index, CheckState.Checked)
                    End If
                End If
            End If
        ElseIf TypeOf (_control) Is CheckedListBox Then
            If _result = "False" Then
                '_control.SetItemCheckState(ControlItem_Index, CheckState.Unchecked)
            Else
                _control.SetItemCheckState(ControlItem_Index, CheckState.Checked)
            End If
        ElseIf TypeOf (_control) Is CheckBox Or TypeOf (_control) Is RadioButton Then
            If _result = "False" Then
                'ControlItem = CheckState.Unchecked
            Else
                _control.Checked = True
            End If
        ElseIf TypeOf (_control) Is TextBox Then
            If _result = "False" Then
                _control.Text = ""
            Else
                _control.Text = _result
            End If
        Else
            ReturnThing = -1
        End If
        If _additionaldata = "Inventory Category" Then
            Return "IC:" & ReturnThing
        ElseIf ReturnThing Is Nothing Then
            Return _result
        Else
            Return ReturnThing
        End If
    End Function
    Public Function EmptyOutBlankDatas1(ByVal _additionaldata As String, Optional ByRef _control As Object = Nothing)
        Dim value As String = Kairen.ItemFile.GetValueByAdditionalData(_additionaldata)
        Dim ReturnThing As Object
        If value = "False" Then
            If TypeOf (_control) Is CheckedListBox Then
                ReturnThing = CheckState.Unchecked
            Else
                ReturnThing = ""
            End If
        Else
            If TypeOf (_control) Is CheckedListBox Then
                ReturnThing = CheckState.Checked
            Else
                ReturnThing = value
            End If
        End If

        If _additionaldata = "Inventory Category" Then
            Return "IC:" & ReturnThing
        Else
            Return ReturnThing
        End If
    End Function
    Public Function EmptyOutBlankDatas(ByVal _additionaldata As String, Optional ByRef _control As Object = Nothing, Optional ByVal _rawData As Object = Nothing)
        Dim value As String
        If _rawData IsNot Nothing Then
            value = _rawData
        End If
        Dim ReturnThing As Object
        If value = "False" Or value Is Nothing Then
            If TypeOf _control Is CheckedListBox Then
                ReturnThing = CheckState.Unchecked
            Else
                ReturnThing = ""
            End If
        Else
            If TypeOf _control Is CheckedListBox Then
                ReturnThing = CheckState.Checked
            Else
                ReturnThing = value
            End If
        End If

        If _additionaldata = "Inventory Category" Then
            Return "IC:" & ReturnThing
        Else
            Return ReturnThing
        End If
    End Function
    Private Sub SetItemCheckStateInCheckedListBox(ByVal _item As String, ByVal _control As CheckedListBox, Optional ByVal _checkstate As CheckState = Nothing)
        For i = 0 To _control.Items.Count - 1
            If _control.Items.Item(i).ToString = _item Then
                If _checkstate = Nothing Then
                    _control.SetItemCheckState(i, EmptyOutBlankDatas(_item, _control))
                Else
                    _control.SetItemCheckState(i, _checkstate)
                End If
                Exit Sub
            End If
        Next
    End Sub
    Private Sub SetItemModificationAmountInCheckedListBox(ByVal _item As String, ByVal _control As CheckedListBox)
        Dim result As String = EmptyOutBlankDatas(_item).trim
        If result = "" Then Exit Sub
        For i = 0 To _control.Items.Count - 1
            If _control.Items.Item(i) = _item Then
                If CInt(result) >= 0 Then
                    _control.Items.Item(i) = _control.Items.Item(i) & " + " & result.Replace("-", "")
                    _control.SetItemCheckState(i, CheckState.Checked)
                ElseIf CInt(result) < 0 Then
                    result = result.Replace("-", "").Trim
                    _control.Items.Item(i) = _control.Items.Item(i) & " - " & result
                    _control.SetItemCheckState(i, CheckState.Checked)
                End If
            End If
        Next
    End Sub
    Private Function CanSaveCheck()
        Return True
        If TextBox1.Text.Trim = "" Or _
            TextBox2.Text.Trim = "" Or _
            TextBox4.Text.Trim = "" Or _
            TextBox5.Text.Trim = "" Or _
            TextBox6.Text.Trim = "" Or _
            TextBox7.Text.Trim = "" Or _
            RadioButton1.Checked = True Or _
            ComboBox2.SelectedItem = Nothing Or _
            ComboBox3.SelectedItem = Nothing Then
            '           (CheckedListBox1.CheckedItems.Count = 0 And CheckBox3.Checked = False) Or _
            '            (CheckedListBox2.CheckedItems.Count = 0 And CheckBox4.Checked = False) Or _
            'TextBox8.Text.Trim = "" Or _
            'TextBox9.Text.Trim = "" Or _
            'TextBox10.Text.Trim = "" Or _
            'TextBox11.Text.Trim = "" Or _
            'TextBox12.Text.Trim = "" Or _
            'TextBox13.Text.Trim = "" Or _
            'TextBox14.Text.Trim = "" Or _
            'TextBox15.Text.Trim = "" Or _
            'TextBox16.Text.Trim = "" Or _
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub SaveSequence()
        'save new file
        Dim InventoryCategory As String
        If RadioButton45.Checked Then
            InventoryCategory = "Magic"
        ElseIf RadioButton46.Checked Then
            InventoryCategory = "Equipment"
        ElseIf RadioButton47.Checked Then
            InventoryCategory = "Tradeskill"
        ElseIf RadioButton48.Checked Then
            InventoryCategory = "Misc"
        End If

        Dim Flag_NoRent As Boolean = False
        Dim Flag_NoTrade As Boolean = False
        Dim Flag_Lore As Boolean = False
        Dim Flag_Craftable As Boolean = False
        For Each item In CheckedListBox5.CheckedItems
            If item = "NO RENT" Then
                Flag_NoRent = True
            ElseIf item = "NO TRADE" Then
                Flag_NoTrade = True
            ElseIf item = "LORE" Then
                Flag_Lore = True
            ElseIf item = "CRAFTABLE" Then
                Flag_Craftable = True
            End If
        Next

        Dim Flag_DarkElf As Boolean = False
        Dim Flag_Troll As Boolean = False
        Dim Flag_Ogre As Boolean = False
        Dim Flag_Elf As Boolean = False
        Dim Flag_Dwarf As Boolean = False
        Dim Flag_Gnome As Boolean = False
        Dim Flag_Halfling As Boolean = False
        Dim Flag_Erudite As Boolean = False
        Dim Flag_Human As Boolean = False
        Dim Flag_Barbarian As Boolean = False
        For Each item In CheckedListBox1.CheckedItems
            If item = "Dark Elf" Then
                Flag_DarkElf = True
            ElseIf item = "Troll" Then
                Flag_Troll = True
            ElseIf item = "Ogre" Then
                Flag_Ogre = True
            ElseIf item = "Elf" Then
                Flag_Elf = True
            ElseIf item = "Dwarf" Then
                Flag_Dwarf = True
            ElseIf item = "Gnome" Then
                Flag_Gnome = True
            ElseIf item = "Halfling" Then
                Flag_Halfling = True
            ElseIf item = "Erudite" Then
                Flag_Erudite = True
            ElseIf item = "Human" Then
                Flag_Human = True
            ElseIf item = "Barbarian" Then
                Flag_Barbarian = True
            End If
        Next

        Dim Flag_Warrior As Boolean = False
        Dim Flag_Paladin As Boolean = False
        Dim Flag_Shadowknight As Boolean = False
        Dim Flag_Enchanter As Boolean = False
        Dim Flag_Magician As Boolean = False
        Dim Flag_Wizard As Boolean = False
        Dim Flag_Alchemist As Boolean = False
        Dim Flag_Necromancer As Boolean = False
        Dim Flag_Monk As Boolean = False
        Dim Flag_Rogue As Boolean = False
        Dim Flag_Ranger As Boolean = False
        Dim Flag_Bard As Boolean = False
        Dim Flag_Druid As Boolean = False
        Dim Flag_Shaman As Boolean = False
        Dim Flag_Cleric As Boolean = False
        For Each item In CheckedListBox2.CheckedItems
            If item = "Warrior" Then
                Flag_Warrior = True
            ElseIf item = "Paladin" Then
                Flag_Paladin = True
            ElseIf item = "Shadowknight" Then
                Flag_Shadowknight = True
            ElseIf item = "Enchanter" Then
                Flag_Enchanter = True
            ElseIf item = "Magician" Then
                Flag_Magician = True
            ElseIf item = "Wizard" Then
                Flag_Wizard = True
            ElseIf item = "Alchemist" Then
                Flag_Alchemist = True
            ElseIf item = "Necromancer" Then
                Flag_Necromancer = True
            ElseIf item = "Monk" Then
                Flag_Monk = True
            ElseIf item = "Rogue" Then
                Flag_Rogue = True
            ElseIf item = "Ranger" Then
                Flag_Ranger = True
            ElseIf item = "Bard" Then
                Flag_Bard = True
            ElseIf item = "Druid" Then
                Flag_Druid = True
            ElseIf item = "Shaman" Then
                Flag_Shaman = True
            ElseIf item = "Cleric" Then
                Flag_Cleric = True
            End If
        Next
        Dim Mod_Strength As String = "False"
        Dim Mod_Stamina As String = "False"
        Dim Mod_Agility As String = "False"
        Dim Mod_Dexterity As String = "False"
        Dim Mod_Wisdom As String = "False"
        Dim Mod_Intelligence As String = "False"
        Dim Mod_Charisma As String = "False"
        Dim Mod_FR As String = "False"
        Dim Mod_CR As String = "False"
        Dim Mod_LR As String = "False"
        Dim Mod_AR As String = "False"
        Dim Mod_PR As String = "False"
        Dim Mod_DR As String = "False"
        Dim Mod_HoT As String = "False"
        Dim Mod_PoT As String = "False"
        Dim Mod_HPMAX As String = "False"
        Dim Mod_POWMAX As String = "False"
        Dim Mod_AC As String = "False"
        For Each item In CheckedListBox3.CheckedItems
            Dim Stat As String
            Dim NewValue As String
            Dim ContainItem As String
            If item.Contains("+") Or item.Contains("-") Then
                If item.Contains("+") = True Then
                    ContainItem = "+"
                ElseIf item.Contains("-") = True Then
                    ContainItem = "-"
                End If
                Stat = Microsoft.VisualBasic.Left(item, item.IndexOf(ContainItem) - 1)
                NewValue = item.replace(Stat, "").replace(ContainItem, "").trim
                If ContainItem = "-" Then
                    NewValue = "-" & NewValue
                End If
                If Stat = "Strength" Then
                    Mod_Strength = NewValue
                ElseIf Stat = "Stamina" Then
                    Mod_Stamina = NewValue
                ElseIf Stat = "Agility" Then
                    Mod_Agility = NewValue
                ElseIf Stat = "Dexterity" Then
                    Mod_Dexterity = NewValue
                ElseIf Stat = "Wisdom" Then
                    Mod_Wisdom = NewValue
                ElseIf Stat = "Intelligence" Then
                    Mod_Intelligence = NewValue
                ElseIf Stat = "Charisma" Then
                    Mod_Charisma = NewValue
                ElseIf Stat = "FR" Then
                    Mod_FR = NewValue
                ElseIf Stat = "CR" Then
                    Mod_CR = NewValue
                ElseIf Stat = "LR" Then
                    Mod_LR = NewValue
                ElseIf Stat = "AR" Then
                    Mod_AR = NewValue
                ElseIf Stat = "PR" Then
                    Mod_PR = NewValue
                ElseIf Stat = "DR" Then
                    Mod_DR = NewValue
                ElseIf Stat = "HoT" Then
                    Mod_HoT = NewValue
                ElseIf Stat = "PoT" Then
                    Mod_PoT = NewValue
                ElseIf Stat = "HPMAX" Then
                    Mod_HPMAX = NewValue
                ElseIf Stat = "POWMAX" Then
                    Mod_POWMAX = NewValue
                ElseIf Stat = "AC" Then
                    Mod_AC = NewValue
                End If
            End If
        Next
        Dim Mod_DefMod As String = "False"
        Dim Mod_OffMod As String = "False"
        Dim Mod_HPFactor As String = "False"
        Dim Mod_MovementRate As String = "False"
        Dim Mod_Fishing As String = "False"
        Dim Mod_JewelCrafting As String = "False"
        Dim Mod_ArmorCrafting As String = "False"
        Dim Mod_WeaponCrafting As String = "False"
        Dim Mod_Tailoring As String = "False"
        Dim Mod_Alchemy As String = "False"
        Dim Mod_Carpentry As String = "False"
        For Each item In CheckedListBox4.CheckedItems
            Dim Stat As String
            Dim NewValue As String
            Dim ContainItem As String
            If item.Contains("+") Or item.Contains("-") Then
                If item.Contains("+") = True Then
                    ContainItem = "+"
                ElseIf item.Contains("-") = True Then
                    ContainItem = "-"
                End If
                Stat = Microsoft.VisualBasic.Left(item, item.IndexOf(ContainItem) - 1)
                NewValue = item.replace(Stat, "").replace(ContainItem, "").trim
                If ContainItem = "-" Then
                    NewValue = "-" & NewValue
                End If
                If Stat = "Def Mod" Then
                    Mod_DefMod = NewValue
                ElseIf Stat = "Off Mod" Then
                    Mod_OffMod = NewValue
                ElseIf Stat = "HP Factor" Then
                    Mod_HPFactor = NewValue
                ElseIf Stat = "Movement Rate" Then
                    Mod_MovementRate = NewValue
                ElseIf Stat = "Fishing" Then
                    Mod_Fishing = NewValue
                ElseIf Stat = "Jewel Crafting" Then
                    Mod_JewelCrafting = NewValue
                ElseIf Stat = "Armor Crafting" Then
                    Mod_ArmorCrafting = NewValue
                ElseIf Stat = "Weapon Crafting" Then
                    Mod_WeaponCrafting = NewValue
                ElseIf Stat = "Tailoring" Then
                    Mod_Tailoring = NewValue
                ElseIf Stat = "Alchemy" Then
                    Mod_Alchemy = NewValue
                ElseIf Stat = "Carpentry" Then
                    Mod_Carpentry = NewValue
                End If
            End If
        Next
        Dim EquipType As String
        If RadioButton1.Checked Then
            EquipType = "False"
        ElseIf RadioButton2.Checked Then
            EquipType = "HELM"
        ElseIf RadioButton3.Checked Then
            EquipType = "ROBE"
        ElseIf RadioButton4.Checked Then
            EquipType = "EARRING"
        ElseIf RadioButton5.Checked Then
            EquipType = "NECK"
        ElseIf RadioButton6.Checked Then
            EquipType = "CHEST"
        ElseIf RadioButton7.Checked Then
            EquipType = "FOREARM"
        ElseIf RadioButton8.Checked Then
            EquipType = "2FOREARM"
        ElseIf RadioButton9.Checked Then
            EquipType = "RING"
        ElseIf RadioButton10.Checked Then
            EquipType = "BELT"
        ElseIf RadioButton11.Checked Then
            EquipType = "PANTS"
        ElseIf RadioButton12.Checked Then
            EquipType = "FEET"
        ElseIf RadioButton13.Checked Then
            EquipType = "PRIMARY"
        ElseIf RadioButton14.Checked Then
            EquipType = "SHIELD"
        ElseIf RadioButton15.Checked Then
            EquipType = "SECONDARY"
        ElseIf RadioButton16.Checked Then
            EquipType = "2HAND"
            'ElseIf RadioButton17.Checked Then
            '    EquipType = "BOW"
            'ElseIf RadioButton18.Checked Then
            '    EquipType = "THROWN"
        ElseIf RadioButton19.Checked Then
            EquipType = "HELD"
        ElseIf RadioButton20.Checked Then
            EquipType = "GLOVES"
        ElseIf RadioButton21.Checked Then
            EquipType = "FISHING"
        ElseIf RadioButton22.Checked Then
            EquipType = "BAIT"
        ElseIf RadioButton23.Checked Then
            EquipType = "WEAPON CRAFT"
        ElseIf RadioButton24.Checked Then
            EquipType = "ARMOR CRAFT"
        ElseIf RadioButton25.Checked Then
            EquipType = "TAILORING"
        ElseIf RadioButton26.Checked Then
            EquipType = "JEWEL CRAFT"
        ElseIf RadioButton27.Checked Then
            EquipType = "CARPENTRY"
        ElseIf RadioButton28.Checked Then
            EquipType = "ALCHEMY"
        End If
        Dim AttackDamageType As String
        If RadioButton34.Checked Then
            AttackDamageType = "False"
        ElseIf RadioButton35.Checked Then
            AttackDamageType = "1HS"
        ElseIf RadioButton36.Checked Then
            AttackDamageType = "1HB"
        ElseIf RadioButton37.Checked Then
            AttackDamageType = "1HP"
        ElseIf RadioButton38.Checked Then
            AttackDamageType = "BOW"
        ElseIf RadioButton39.Checked Then
            AttackDamageType = "2HS"
        ElseIf RadioButton40.Checked Then
            AttackDamageType = "2HB"
        ElseIf RadioButton41.Checked Then
            AttackDamageType = "2HP"
        ElseIf RadioButton42.Checked Then
            AttackDamageType = "2HCROSSBOW"
        ElseIf RadioButton43.Checked Then
            AttackDamageType = "1HCROSSBOW"
        ElseIf RadioButton44.Checked Then
            AttackDamageType = "THROWN"
        End If
        Dim ControlRestrictionOptions As String
        If RadioButton29.Checked Then
            ControlRestrictionOptions = "CRO:Worn"
        ElseIf RadioButton30.Checked Then
            ControlRestrictionOptions = "CRO:Wielded"
        ElseIf RadioButton33.Checked Then
            ControlRestrictionOptions = "CRO:Tools"
        ElseIf RadioButton31.Checked Then
            ControlRestrictionOptions = "CRO:Gem"
        ElseIf RadioButton32.Checked Then
            ControlRestrictionOptions = "CRO:Misc"
        End If
        Dim QuestCatelogNumber As String
        If TextBox8.Text = "" Then
            QuestCatelogNumber = "False"
        Else
            QuestCatelogNumber = TextBox8.Text
        End If
        Dim PatternFamily As String
        If TextBox14.Text = "" Then
            PatternFamily = "False"
        Else
            PatternFamily = TextBox14.Text
        End If
        Dim EffectsList(CheckedListBox4.CheckedItems.Count) As String
        Dim i As Integer = -1
        For Each item In CheckedListBox4.CheckedItems
            i = i + 1
            ReDim Preserve EffectsList(i)
            EffectsList(i) = item
        Next
        Kairen.CreateNewItemFile(TextBox6.Text, TextBox5.Text, InventoryCategory, TextBox7.Text, TextBox16.Text, TextBox2.Text, TextBox1.Text, TextBox4.Text, ComboBox3.SelectedText, ComboBox2.SelectedText, , , , TextBox11.Text, TextBox12.Text, TextBox10.Text, , , , , , , Flag_NoRent, Flag_NoTrade, Flag_Lore, Flag_Craftable, Flag_DarkElf, Flag_Troll, Flag_Ogre, Flag_Elf, Flag_Dwarf, Flag_Gnome, Flag_Halfling, Flag_Erudite, Flag_Human, Flag_Barbarian, Flag_Warrior, Flag_Paladin, Flag_Shadowknight, Flag_Enchanter, Flag_Magician, Flag_Wizard, Flag_Alchemist, Flag_Necromancer, Flag_Monk, Flag_Rogue, Flag_Ranger, Flag_Bard, Flag_Druid, Flag_Shaman, Flag_Cleric, Mod_Strength, Mod_Stamina, Mod_Agility, Mod_Dexterity, Mod_Wisdom, Mod_Intelligence, Mod_Charisma, Mod_FR, Mod_CR, Mod_LR, Mod_AR, Mod_PR, Mod_DR, Mod_HoT, Mod_PoT, Mod_HPMAX, Mod_POWMAX, Mod_AC, Mod_DefMod, Mod_OffMod, Mod_HPFactor, Mod_MovementRate, Mod_Fishing, Mod_JewelCrafting, Mod_ArmorCrafting, Mod_WeaponCrafting, Mod_Tailoring, Mod_Alchemy, Mod_Carpentry, TextBox3.Text, EquipType, TextBox9.Text, AttackDamageType, , , , , , , , , , , , , , ControlRestrictionOptions, , , , PatternFamily, , , QuestCatelogNumber, , , , EffectsList)
    End Sub
#End Region
#Region "Test & Other Code"
    Function TheQuestionAndAnswer(ByVal _text As String, Optional ByRef _control As Object = Nothing)
        If _text = "What is the question?" Then
            Return "What do you say?"
        Else
            If _text.ToLower <> "please" Then
                Return "Incorrect"
            Else
                Return "Correct"
            End If
        End If
    End Function 'the question
    Private Sub Label1_Click(sender As System.Object, e As System.EventArgs) Handles Label1.Click
        If Kairen.ItemFile Is Nothing Then Exit Sub
        Kairen.ItemFile.SetQuestionAndAnswer(AddressOf TheQuestionAndAnswer)
        Kairen.ItemFile.GetAnswer()
    End Sub 'the question

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs)
        TextBox1.Text = "1"
        TextBox2.Text = "1"
        'TextBox3.Text = "1"
        TextBox4.Text = "1"
        TextBox5.Text = "1"
        TextBox6.Text = "1"
        TextBox7.Text = "1"
        TextBox10.Text = "1"
        TextBox11.Text = "1"
        TextBox12.Text = "1"
        TextBox13.Text = "1"
        RadioButton2.Checked = True
        CheckedListBox1.SetItemChecked(0, True)
        CheckedListBox2.SetItemChecked(0, True)
        CheckedListBox3.SetItemChecked(0, False)
        AlterCheckedListBoxItemsStatMod(CheckedListBox3.Items.Item(0), 22)
        CheckedListBox3.SetItemChecked(0, True)
        CheckedListBox3.SetItemChecked(2, False)
        AlterCheckedListBoxItemsStatMod(CheckedListBox3.Items.Item(2), -14)
        CheckedListBox3.SetItemChecked(2, True)
        CheckedListBox4.SetItemChecked(0, True)
        ComboBox2.SelectedIndex = 0
        ComboBox3.SelectedIndex = 0
    End Sub
    Private Sub Button3_Click(sender As System.Object, e As System.EventArgs)
        ClearForm()
    End Sub
#End Region
#Region "old code i'm redoing RIGHT NOW"
    Public Function DataParserFunction_RadioButtons1(ByVal _dataTag As String, ByRef _dataControl As Object, ByRef _rawData As Object)
        'if _rawText is false then this is being asked what the rawtext is, if it's provided it wants the rawtext translated into UI text
        If _dataTag = "Equip Type" Then
            If 1 = 2 Then Return 34
        End If
        Dim ReturnData As String = _rawData
        If _rawData = "False" Then
            Return _rawData
        End If
        GetRadioButtonByDataName(_dataTag, _dataControl, _rawData)
        If _dataControl Is Nothing Then
            Return _rawData 'error
        End If
        Select Case _dataTag
            Case "control restriction options", "Inventory Category", "Equip Type", "Attack Type"
                If _dataControl IsNot Nothing Then
                    _dataControl.Checked = True
                    ReturnData = _rawData
                Else
                    ReturnData = "False"
                End If

            Case Else
                If _rawData IsNot Nothing Then 'setting control - convert file to control
                    If TypeOf (_rawData) Is Array Then
                        If _rawData.Length = 0 Then
                            _dataControl.Checked = False
                            Return "False"
                        End If
                    End If
                    If _rawData = "True" Then
                        _dataControl.Checked = True
                    Else
                        _dataControl.Checked = False
                    End If
                Else 'getting control - convert control to file
                    If _dataControl.Checked = False Then
                        ReturnData = "False"
                    Else
                        ReturnData = "True"
                    End If
                End If
                'Return Kairen.DataParserFunction(_dataTag, ControlToUse, _rawData)
        End Select
        Return ReturnData
    End Function
    Private Function GetRadioButtonByDataName1(ByVal _additionalData As String, ByRef _control As Object, ByRef _rawData As Object)
        If _additionalData Is Nothing Then Return Nothing
        If _additionalData.Trim = "" Then Return Nothing
        Select Case _additionalData
            'Attack Type
            Case "NO ATTACK"
                _control = RadioButton34
                _rawData = "NO ATTACK"
                Return _rawData
            Case "1HS"
                _control = RadioButton35
                _rawData = "1HS"
                Return _rawData
            Case "1HB"
                _control = RadioButton36
                _rawData = "1HB"
                Return _rawData
            Case "1HP"
                _control = RadioButton37
                _rawData = "1HP"
                Return _rawData
            Case "1HCROSSBOW"
                _control = RadioButton43
                _rawData = "1HCROSSBOW"
                Return _rawData
            Case "BOW"
                _control = RadioButton38
                _rawData = "BOW"
                Return _rawData
            Case "THROWN"
                _control = RadioButton44
                _rawData = "THROWN"
                Return _rawData
            Case "2HS"
                _control = RadioButton39
                _rawData = "2HS"
                Return _rawData
            Case "2HB"
                _control = RadioButton40
                _rawData = "2HB"
                Return _rawData
            Case "2HP"
                _control = RadioButton41
                _rawData = "2HP"
                Return _rawData
            Case "2HCROSSBOW"
                _control = RadioButton42
                _rawData = "2HCROSSBOW"
                Return _rawData

                'Equip Type
            Case "NO EQUIP"
                _control = RadioButton1
                _rawData = "NO EQUIP"
                Return _rawData
            Case "HELM"
                _control = RadioButton2
                _rawData = "HELM"
                Return _rawData
            Case "ROBE"
                _control = RadioButton3
                _rawData = "ROBE"
                Return _rawData
            Case "EARRING"
                _control = RadioButton4
                _rawData = "EARRING"
                Return _rawData
            Case "NECK"
                _control = RadioButton5
                _rawData = "NECK"
                Return _rawData
            Case "CHEST"
                _control = RadioButton6
                _rawData = "CHEST"
                Return _rawData
            Case "FOREARM"
                _control = RadioButton7
                _rawData = "FOREARM"
                Return _rawData
            Case "2FOREARM"
                _control = RadioButton8
                _rawData = "2FOREARM"
                Return _rawData
            Case "RING"
                _rawData = "RING"
                Return _rawData
                _control = RadioButton9
            Case "BELT"
                _control = RadioButton10
                _rawData = "BELT"
                Return _rawData
            Case "PANTS"
                _control = RadioButton10
                _rawData = "PANTS"
                Return _rawData
            Case "FEET"
                _control = RadioButton11
                _rawData = "FEET"
                Return _rawData
            Case "GLOVES"
                _control = RadioButton20
                _rawData = "GLOVES"
                Return _rawData
            Case "PRIMARY"
                _control = RadioButton13
                _rawData = "PRIMARY"
                Return _rawData
            Case "SHIELD"
                _control = RadioButton14
                _rawData = "SHIELD"
                Return _rawData
            Case "SECONDARY"
                _control = RadioButton15
                _rawData = "SECONDARY"
                Return _rawData
            Case "2HAND"
                _control = RadioButton16
                _rawData = "2HAND"
                Return _rawData
            Case "BOW"
                '    _control = RadioButton17
                _rawData = "BOW"
                Return _rawData
            Case "THROWN"
                '    _control = RadioButton18
                _rawData = "THROWN"
                Return _rawData
            Case "HELD"
                _control = RadioButton19
                _rawData = "HELD"
                Return _rawData
            Case "FISHING"
                _control = RadioButton21
                _rawData = "FISHING"
                Return _rawData
            Case "BAIT"
                _control = RadioButton22
                _rawData = "BAIT"
                Return _rawData
            Case "WEAPON CRAFT"
                _control = RadioButton23
                _rawData = "WEAPON CRAFT"
                Return _rawData
            Case "ARMOR CRAFT"
                _control = RadioButton24
                _rawData = "ARMOR CRAFT"
                Return _rawData
            Case "TAILORING"
                _control = RadioButton25
                _rawData = "TAILORING"
                Return _rawData
            Case "JEWEL CRAFT"
                _control = RadioButton26
                _rawData = "JEWEL CRAFT"
                Return _rawData
            Case "CARPENTRY"
                _control = RadioButton27
                _rawData = "CARPENTRY"
                Return _rawData
            Case "ALCHEMY"
                _control = RadioButton28
                _rawData = "ALCHEMY"
                Return _rawData

                'control restriction options
            Case "CRO:Worn"
                _control = RadioButton29
                _rawData = "Worn"
                Return _rawData
            Case "CRO:Wielded"
                _control = RadioButton30
                _rawData = "Wielded"
                Return _rawData
            Case "CRO:Tools"
                _control = RadioButton33
                _rawData = "Tools"
                Return _rawData
            Case "CRO:Gem"
                _control = RadioButton31
                _rawData = "Gem"
                Return _rawData
            Case "CRO:Misc"
                _control = RadioButton32
                _rawData = "Misc"
                Return _rawData

                'inventory category
            Case "IC:Magic"
                _control = RadioButton45
                _rawData = "Magic"
                Return _rawData
            Case "IC:Equipment"
                _control = RadioButton46
                _rawData = "Equipment"
                Return _rawData
            Case "IC:Tradeskill"
                _control = RadioButton47
                _rawData = "Tradeskill"
                Return _rawData
            Case "IC:Misc"
                _control = RadioButton48
                _rawData = "Misc"
                Return _rawData

            Case "control restriction options"
                If _rawData Is Nothing Then
                    If RadioButton29.Checked Then 'worn
                        _control = RadioButton29
                        _rawData = "Worn"
                        Return _rawData
                    ElseIf RadioButton30.Checked Then 'wielded
                        _control = RadioButton30
                        _rawData = "Wielded"
                        Return _rawData
                    ElseIf RadioButton33.Checked Then 'tools
                        _control = RadioButton33
                        _rawData = "Tools"
                        Return _rawData
                    ElseIf RadioButton31.Checked Then 'gems
                        _control = RadioButton31
                        _rawData = "Gems"
                        Return _rawData
                    ElseIf RadioButton32.Checked Then 'misc
                        _control = RadioButton32
                        _rawData = "Misc"
                        Return _rawData
                    ElseIf RadioButton49.Checked Then 'none
                        _control = RadioButton49
                        _rawData = "NONE"
                        Return _rawData
                    End If
                Else
                    If _rawData = "Worn" Then
                        _control = RadioButton29
                        Return RadioButton29
                    ElseIf _rawData = "Wielded" Then
                        _control = RadioButton30
                        Return RadioButton30
                    ElseIf _rawData = "Tools" Then
                        _control = RadioButton33
                        Return RadioButton33
                    ElseIf _rawData = "Gems" Then
                        _control = RadioButton31
                        Return RadioButton31
                    ElseIf _rawData = "Misc" Then
                        _control = RadioButton32
                        Return RadioButton32
                    ElseIf _rawData = "NONE" Then
                        _control = RadioButton49
                        Return RadioButton49
                    End If
                End If

            Case "Inventory Category"
                If _rawData Is Nothing Then
                    If RadioButton45.Checked Then 'Magic
                        _control = RadioButton45
                        _rawData = "Magic"
                        Return _rawData
                    ElseIf RadioButton46.Checked Then 'Equipment
                        _control = RadioButton46
                        _rawData = "Equipment"
                        Return _rawData
                    ElseIf RadioButton47.Checked Then 'Tradeskills
                        _control = RadioButton47
                        _rawData = "Tradeskills"
                        Return _rawData
                    ElseIf RadioButton48.Checked Then 'Misc
                        _control = RadioButton48
                        _rawData = "Misc"
                        Return _rawData
                    End If
                Else
                    If _rawData = "Magic" Then
                        _control = RadioButton45
                        Return RadioButton45
                    ElseIf _rawData = "Equipment" Then
                        _control = RadioButton46
                        Return RadioButton46
                    ElseIf _rawData = "Tradeskills" Then
                        _control = RadioButton47
                        Return RadioButton47
                    ElseIf _rawData = "Misc" Then
                        _control = RadioButton48
                        Return RadioButton48
                    End If
                End If

            Case "Equip Type"
                If _rawData Is Nothing Then
                    If RadioButton1.Checked Then 'none
                        _control = RadioButton1
                        _rawData = "NONE"
                        Return _rawData
                    ElseIf RadioButton2.Checked Then 'helm
                        _control = RadioButton2
                        _rawData = "HELM"
                        Return _rawData
                    ElseIf RadioButton3.Checked Then 'robe
                        _control = RadioButton3
                        _rawData = "ROBE"
                        Return _rawData
                    ElseIf RadioButton4.Checked Then 'earring
                        _control = RadioButton4
                        _rawData = "EARRING"
                        Return _rawData
                    ElseIf RadioButton5.Checked Then 'neck
                        _control = RadioButton5
                        _rawData = "NECK"
                        Return _rawData
                    ElseIf RadioButton6.Checked Then 'chest
                        _control = RadioButton6
                        _rawData = "CHEST"
                        Return _rawData
                    ElseIf RadioButton7.Checked Then 'forearm
                        _control = RadioButton7
                        _rawData = "FOREARM"
                        Return _rawData
                    ElseIf RadioButton8.Checked Then '2forearm
                        _control = RadioButton8
                        _rawData = "2FOREARM"
                        Return _rawData
                    ElseIf RadioButton9.Checked Then 'ring
                        _control = RadioButton9
                        _rawData = "RING"
                        Return _rawData
                    ElseIf RadioButton10.Checked Then 'belt
                        _control = RadioButton10
                        _rawData = "BELT"
                        Return _rawData
                    ElseIf RadioButton11.Checked Then 'pants
                        _control = RadioButton11
                        _rawData = "PANTS"
                        Return _rawData
                    ElseIf RadioButton12.Checked Then 'feet
                        _control = RadioButton12
                        _rawData = "FEET"
                        Return _rawData
                    ElseIf RadioButton20.Checked Then 'gloves
                        _control = RadioButton20
                        _rawData = "GLOVES"
                        Return _rawData
                    ElseIf RadioButton13.Checked Then 'primary
                        _control = RadioButton13
                        _rawData = "PRIMARY"
                        Return _rawData
                    ElseIf RadioButton14.Checked Then 'shield
                        _control = RadioButton14
                        _rawData = "SHIELD"
                        Return _rawData
                    ElseIf RadioButton15.Checked Then 'secondary
                        _control = RadioButton15
                        _rawData = "SECONDARY"
                        Return _rawData
                    ElseIf RadioButton16.Checked Then '2hand
                        _control = RadioButton16
                        _rawData = "2HAND"
                        Return _rawData
                        'ElseIf RadioButton17.Checked Then 'bow
                        '    _control = RadioButton17
                        '    _rawData = "BOW"
                        '    Return _rawData
                        'ElseIf RadioButton18.Checked Then 'thrown
                        '    _control = RadioButton18
                        '    _rawData = "THROWN"
                        '    Return _rawData
                    ElseIf RadioButton19.Checked Then 'held
                        _control = RadioButton19
                        _rawData = "HELD"
                        Return _rawData
                    ElseIf RadioButton21.Checked Then 'fishing
                        _control = RadioButton21
                        _rawData = "FISHING"
                        Return _rawData
                    ElseIf RadioButton22.Checked Then 'bait
                        _control = RadioButton22
                        _rawData = "BAIT"
                        Return _rawData
                    ElseIf RadioButton23.Checked Then 'wep
                        _control = RadioButton23
                        _rawData = "WEAPON CRAFT"
                        Return _rawData
                    ElseIf RadioButton24.Checked Then 'arm
                        _control = RadioButton24
                        _rawData = "ARMORM CRAFT"
                        Return _rawData
                    ElseIf RadioButton25.Checked Then 'tail
                        _control = RadioButton25
                        _rawData = "TAILORING"
                        Return _rawData
                    ElseIf RadioButton26.Checked Then 'jew
                        _control = RadioButton26
                        _rawData = "JEWEL CRAFT"
                        Return _rawData
                    ElseIf RadioButton27.Checked Then 'carp
                        _control = RadioButton27
                        _rawData = "CARPENTRY"
                        Return _rawData
                    ElseIf RadioButton28.Checked Then 'alc
                        _control = RadioButton28
                        _rawData = "ALCHEMY"
                        Return _rawData
                    End If
                Else
                    If _rawData = "NONE" Then
                        _control = RadioButton1
                        Return RadioButton1
                    ElseIf _rawData = "HELM" Then
                        _control = RadioButton2
                        Return RadioButton2
                    ElseIf _rawData = "ROBE" Then
                        _control = RadioButton3
                        Return RadioButton3
                    ElseIf _rawData = "EARRING" Then
                        _control = RadioButton4
                        Return RadioButton4
                    ElseIf _rawData = "NECK" Then
                        _control = RadioButton5
                        Return RadioButton5
                    ElseIf _rawData = "CHEST" Then
                        _control = RadioButton6
                        Return RadioButton6
                    ElseIf _rawData = "FOREARM" Then
                        _control = RadioButton7
                        Return RadioButton7
                    ElseIf _rawData = "2FOREAR" Then
                        _control = RadioButton8
                        Return RadioButton8
                    ElseIf _rawData = "RING" Then
                        _control = RadioButton9
                        Return RadioButton9
                    ElseIf _rawData = "PANTS" Then
                        _control = RadioButton10
                        Return RadioButton10
                    ElseIf _rawData = "FEET" Then
                        _control = RadioButton11
                        Return RadioButton11
                    ElseIf _rawData = "GLOVES" Then
                        _control = RadioButton20
                        Return RadioButton20
                    ElseIf _rawData = "PRIMARY" Then
                        _control = RadioButton13
                        Return RadioButton13
                    ElseIf _rawData = "SHIELD" Then
                        _control = RadioButton14
                        Return RadioButton14
                    ElseIf _rawData = "SECONDARY" Then
                        _control = RadioButton15
                        Return RadioButton15
                    ElseIf _rawData = "2HAND" Then
                        _control = RadioButton16
                        Return RadioButton16
                    ElseIf _rawData = "BOW" Then
                        '    _control = RadioButton17
                        '    Return RadioButton17
                    ElseIf _rawData = "THROWN" Then
                        '    _control = RadioButton18
                        '    Return RadioButton18
                    ElseIf _rawData = "HELD" Then
                        _control = RadioButton19
                        Return RadioButton19
                    ElseIf _rawData = "FISHING" Then
                        _control = RadioButton21
                        Return RadioButton21
                    ElseIf _rawData = "BAIT" Then
                        _control = RadioButton22
                        Return RadioButton22
                    ElseIf _rawData = "WEAPON CRAFT" Then
                        _control = RadioButton23
                        Return RadioButton23
                    ElseIf _rawData = "ARMOR CRAFT" Then
                        _control = RadioButton24
                        Return RadioButton24
                    ElseIf _rawData = "TAILORING" Then
                        _control = RadioButton25
                        Return RadioButton25
                    ElseIf _rawData = "JEWEL CRAFT" Then
                        _control = RadioButton26
                        Return RadioButton26
                    ElseIf _rawData = "CARPENTRY" Then
                        _control = RadioButton27
                        Return RadioButton27
                    ElseIf _rawData = "ALCHEMY" Then
                        _control = RadioButton28
                        Return RadioButton28
                    End If
                End If
            Case "Attack Type"
                If _rawData Is Nothing Then
                    'Attack Type
                    If RadioButton34.Checked = True Then
                        _control = RadioButton34
                        _rawData = "NO ATTACK"
                        Return _rawData
                    ElseIf RadioButton35.Checked = True Then
                        _control = RadioButton35
                        _rawData = "1HS"
                        Return _rawData
                    ElseIf RadioButton36.Checked = True Then
                        _control = RadioButton36
                        _rawData = "1HB"
                        Return _rawData
                    ElseIf RadioButton37.Checked = True Then
                        _control = RadioButton37
                        _rawData = "1HP"
                        Return _rawData
                    ElseIf RadioButton43.Checked = True Then
                        _control = RadioButton43
                        _rawData = "1HCROSSBOW"
                        Return _rawData
                    ElseIf RadioButton38.Checked = True Then
                        _control = RadioButton38
                        _rawData = "BOW"
                        Return _rawData
                    ElseIf RadioButton44.Checked = True Then
                        _control = RadioButton44
                        _rawData = "THROWN"
                        Return _rawData
                    ElseIf RadioButton39.Checked = True Then
                        _control = RadioButton39
                        _rawData = "2HS"
                        Return _rawData
                    ElseIf RadioButton40.Checked = True Then
                        _control = RadioButton40
                        _rawData = "2HB"
                        Return _rawData
                    ElseIf RadioButton41.Checked = True Then
                        _control = RadioButton41
                        _rawData = "2HP"
                        Return _rawData
                    ElseIf RadioButton42.Checked = True Then
                        _control = RadioButton42
                        _rawData = "2HCROSSBOW"
                        Return _rawData
                    End If
                Else
                    If _rawData = "NO ATTACK" Then
                        _control = RadioButton1
                        _rawData = "NO ATTACK"
                        Return _rawData
                    ElseIf _rawData = "1HS" Then
                        _control = RadioButton35
                        _rawData = "1HS"
                        Return _rawData
                    ElseIf _rawData = "1HB" Then
                        _control = RadioButton36
                        _rawData = "1HB"
                        Return _rawData
                    ElseIf _rawData = "1HP" Then
                        _control = RadioButton37
                        _rawData = "1HP"
                        Return _rawData
                    ElseIf _rawData = "1HCROSSBOW" Then
                        _control = RadioButton43
                        _rawData = "1HCROSSBOW"
                        Return _rawData
                    ElseIf _rawData = "BOW" Then
                        _control = RadioButton38
                        _rawData = "BOW"
                        Return _rawData
                    ElseIf _rawData = "THROWN" Then
                        _control = RadioButton44
                        _rawData = "THROWN"
                        Return _rawData
                    ElseIf _rawData = "2HS" Then
                        _control = RadioButton39
                        _rawData = "2HS"
                        Return _rawData
                    ElseIf _rawData = "2HB" Then
                        _control = RadioButton40
                        _rawData = "2HB"
                        Return _rawData
                    ElseIf _rawData = "2HP" Then
                        _control = RadioButton41
                        _rawData = "2HP"
                        Return _rawData
                    ElseIf _rawData = "2HCROSSBOW" Then
                        _control = RadioButton42
                        _rawData = "2HCROSSBOW"
                        Return _rawData
                    End If
                End If
            Case Else
                '
        End Select
    End Function 'get radio button
#End Region

End Class