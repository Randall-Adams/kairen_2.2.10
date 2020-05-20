Public Class LineEntryClass
    Private vLine(-1) As String
    Private vName As String
    Private vAdditionalDataTag As String
    Private vEntryType As String 'single line, multiple lines
    Public FileHasChanged_IsLocked As Boolean = True
    Private vFileHasChanged As Boolean = False
    Private vThisTagIsRegistered As Boolean
    Private vControl As Object
    Private vDefaultTagIndex As Integer
    Private vDataParserFunction As DataParserDelegate
    Delegate Function DataParserDelegate(ByVal _dataTag As String, ByRef _dataControl As Object, ByRef _rawData As Object)
    Private Kairen As Kairen2 = FormLoader.Kairen
    Sub New(ByVal _name As String, ByVal _entryType As String, ByRef _dataControl As Object, Optional ByRef _dataParser As DataParserDelegate = Nothing, Optional ByVal _defaultTagIndex As Integer = Nothing, Optional ByVal _thisTagIsRegistered As Boolean = True)
        vName = _name
        If _entryType = "Not Setup" Then
            vEntryType = _entryType
            ReDim vLine(0)
        ElseIf _entryType = "Single Line" Then
            vEntryType = _entryType
            ReDim vLine(0)
        ElseIf _entryType = "Multiple Lines" Then
            vEntryType = _entryType
            'ReDim vLine(0)
        Else
            'error
        End If
        vControl = _dataControl
        vDataParserFunction = _dataParser
        vDefaultTagIndex = _defaultTagIndex
        vThisTagIsRegistered = _thisTagIsRegistered
    End Sub
    Public ReadOnly Property Name() As String
        Get
            Return vName
        End Get
    End Property
    Public Property FileHasChanged() As Boolean
        Set(value As Boolean)
            If FileHasChanged_IsLocked = False Then
                vFileHasChanged = value
            End If
        End Set
        Get
            Return vFileHasChanged
        End Get
    End Property
    Public ReadOnly Property ThisTagIsRegistered() As Boolean
        Get
            Return vThisTagIsRegistered
        End Get
    End Property

    Public Function SetData(ByVal _newData As String)
        If vEntryType = "Not Setup" Then
            UpdateData(_newData)
        ElseIf vEntryType = "Single Line" Then
            UpdateData(_newData)
        ElseIf vEntryType = "Multiple Lines" Then
            AddData(_newData)
        Else
            'error
            Return -1
        End If
        Return 0
    End Function

    Public Function UpdateData(ByVal _updateData As String)
        Dim vlineCopy() As String = CopyStupidArrays(vLine)
        vLine(0) = _updateData
        CheckDataChange(vlineCopy, vLine)
        Return 0
    End Function
    Public Function AddData(ByVal _addData As String)
        If _addData Is Nothing Or _addData.Trim = "" Then Return -1
        Dim currentIndex As Integer = vLine.Count
        Dim vlineCopy() As String = CopyStupidArrays(vLine)
        ReDim Preserve vLine(currentIndex)
        vLine(currentIndex) = _addData
        CheckDataChange(vlineCopy, vLine)
        Return 0
    End Function
    Public Function RemoveData(ByVal _removeData As String)
        Dim newVLines(vLine.Count - 1) As String
        Dim i As Integer
        Dim i2 As Integer
        Dim vlineCopy() As String = CopyStupidArrays(vLine)
        Do Until vLine(i) = vLine.Count - 1
            If vLine(i) <> _removeData Then
                newVLines(i2) = vLine(i)
                i2 += i2
            End If
            i += 1
        Loop
        CheckDataChange(vlineCopy, vLine)
        If i2 = i - 1 Then
            vLine = newVLines
            Return 0
        Else
            Return -1
        End If
    End Function

    Public Function UpdateUI(Optional ByVal _entryLine As Integer = Nothing)
        If _entryLine = Nothing Then _entryLine = 0
        If _entryLine > vLine.Count Then Return -1
        If vDataParserFunction Is Nothing Then
            ControlParserFunction(vControl, vName) = vLine(_entryLine)
        Else
            If vEntryType = "Multiple Lines" Then
                vDataParserFunction(vName, vControl, vLine)
            Else
                vDataParserFunction(vName, vControl, vLine(_entryLine))
            End If
        End If
        Return 0
    End Function
    Public Function UpdateRAM(Optional ByVal _entryLine As Integer = Nothing)
        If vThisTagIsRegistered = False Then Return 0
        If _entryLine = Nothing Then _entryLine = 0
        If _entryLine > vLine.Count Then Return -1
        If vLine.Length = 0 Then Return -2
        Dim vlineCopy() As String = CopyStupidArrays(vLine)
        If vDataParserFunction Is Nothing Then
            vLine(_entryLine) = ControlParserFunction(vControl, vName)
        Else
            If vEntryType = "Multiple Lines" Then
                vLine = vDataParserFunction(vName, vControl, Nothing)
            Else
                vLine(_entryLine) = vDataParserFunction(vName, vControl, Nothing)
            End If
        End If
        CheckDataChange(vlineCopy, vLine)
        Return 0
    End Function
    Private Sub CheckDataChange(ByRef _string1() As String, ByRef _string2() As String)
        If FileHasChanged_IsLocked Then Exit Sub
        If _string1 IsNot _string2 Then
            If _string1.Length <> _string2.Length Then Exit Sub
            If _string1.Length = 0 Then Exit Sub
            If _string2.Length = 0 Then Exit Sub
            Dim i As Integer = 0
            Do While i < _string2.Length
                If _string1(i) <> _string2(i) Then
                    FileHasChanged = True
                    Exit Do
                End If
                i += 1
            Loop
        Else
            If 1 = 2 Then Exit Sub
        End If
    End Sub
    Private Function CopyStupidArrays(ByRef _original() As String)
        Dim NewArray(_original.Length - 1) As String
        For i = 0 To _original.Length - 1
            NewArray(i) = _original(i)
        Next
        Return NewArray
    End Function

    Public ReadOnly Property GetData()
        Get
            Return vLine(0)
        End Get
    End Property
    Public ReadOnly Property GetDataArray()
        Get
            Dim vlineCopy() As String = CopyStupidArrays(vLine)
            ' If vLine.Count = 0 Then
            If vEntryType = "Multiple Lines" Then
                If ThisTagIsRegistered = True Then
                    vLine = vDataParserFunction(vName, vControl, Nothing)
                End If
            ElseIf vDataParserFunction IsNot Nothing Then
                vLine(0) = vDataParserFunction(vName, vControl, Nothing)
            End If
            'End If
            CheckDataChange(vlineCopy, vLine)
            Return vLine
        End Get
    End Property
    Public ReadOnly Property GetData(ByVal _entryLine As Integer)
        Get
            If _entryLine > vLine.Count Then Return -1
            Return vLine(_entryLine)
        End Get
    End Property

    Private Property ControlParserFunction(ByVal _dataControl As Object, ByVal _dataTag As String)
        Get
            Dim ReturnData As Object
            If TypeOf _dataControl Is TextBox Or TypeOf _dataControl Is Label Or TypeOf _dataControl Is ComboBox Then
                If _dataControl.Text.trim = "" Then
                    ReturnData = "False"
                Else
                    ReturnData = _dataControl.Text
                End If
            ElseIf TypeOf _dataControl Is CheckBox Then
                If _dataControl.Checked = True Then
                    ReturnData = True
                Else
                    ReturnData = False
                End If
            ElseIf TypeOf _dataControl Is CheckedListBox Then
                Dim ControlItem_Index As Integer = -1
                Dim i As Integer
                Do Until i = _dataControl.Items.Count
                    If _dataControl.Items.Item(i) = _dataTag Then
                        ControlItem_Index = i
                        Exit Do
                    End If
                    i += 1
                Loop
                If _dataControl.GetItemCheckState(ControlItem_Index) = CheckState.Checked Then
                    ReturnData = "True"
                Else
                    ReturnData = "False"
                End If
            ElseIf TypeOf _dataControl Is ListBox Then
                Dim i As Integer
                Dim returnArray(_dataControl.Items.Count - 1) As String
                Do Until i = _dataControl.Items.Count
                    MsgBox(_dataControl.Items.Item(i))
                    returnArray(i) = _dataControl.Items.Item(i)
                    i += 1
                Loop
                ReturnData = returnArray
            Else
                'error, but does something atleast lol
                Return _dataControl
            End If
            Return ReturnData
        End Get
        Set(_rawData)
            If TypeOf _dataControl Is TextBox Or TypeOf _dataControl Is Label Or TypeOf _dataControl Is ComboBox Then
                If _rawData = "False" Then
                    _dataControl.Text = ""
                Else
                    _dataControl.Text = _rawData
                End If
            ElseIf TypeOf _dataControl Is CheckBox Then
                If _rawData = "True" Then
                    _dataControl.Checked = True
                Else
                    _dataControl.Checked = False
                End If
            ElseIf TypeOf _dataControl Is CheckedListBox Then
                Dim ControlItem_Index As Integer = -1
                Dim i As Integer
                Do Until i = _dataControl.Items.Count
                    If _dataControl.Items.Item(i) = _dataTag Then
                        ControlItem_Index = i
                        Exit Do
                    End If
                    i += 1
                Loop
                If _rawData = "False" Then
                    _dataControl.SetItemCheckState(ControlItem_Index, CheckState.Unchecked)
                Else
                    _dataControl.SetItemCheckState(ControlItem_Index, CheckState.Checked)
                End If
            Else
                'error
            End If
        End Set
    End Property
    Private Function kControlParserFunction(ByRef _dataControl As Object, Optional ByVal _dataTag As String = Nothing, Optional ByVal _rawData As String = Nothing)
        If _rawData IsNot Nothing Then
            If TypeOf _dataControl Is TextBox Or TypeOf _dataControl Is Label Or TypeOf _dataControl Is ComboBox Then
                _dataControl.Text = _rawData
                Return _rawData
            ElseIf TypeOf _dataControl Is CheckBox Then
                _dataControl.Checked = CBool(_rawData)
                Return CBool(_rawData)
            ElseIf TypeOf _dataControl Is CheckedListBox Then
                Dim ControlItem_Index As Integer = -1
                Dim i As Integer
                Do Until i = _dataControl.Items.Count
                    If _dataControl.Items.Item(i) = _dataTag Then
                        ControlItem_Index = i
                        Exit Do
                    End If
                    i += 1
                Loop
                If CBool(_rawData) = True Then
                    _dataControl.SetItemCheckState(ControlItem_Index, CheckState.Checked)
                    Return CheckState.Checked
                Else
                    _dataControl.SetItemCheckState(ControlItem_Index, CheckState.Unchecked)
                    Return CheckState.Unchecked
                End If
            End If
        Else
            If TypeOf _dataControl Is TextBox Or TypeOf _dataControl Is Label Or TypeOf _dataControl Is ComboBox Then
                Return _dataControl.Text
            ElseIf TypeOf _dataControl Is CheckBox Then
                Return _dataControl.Checked
            ElseIf TypeOf _dataControl Is CheckedListBox Then
                Dim ControlItem_Index As Integer = -1
                Dim i As Integer
                Do Until i = _dataControl.Items.Count
                    If _dataControl.Items.Item(i) = _dataTag Then
                        ControlItem_Index = i
                        Exit Do
                    End If
                    i += 1
                Loop
                Return _dataControl.GetItemCheckState(ControlItem_Index)
            End If
        End If
        Return Nothing
    End Function
End Class