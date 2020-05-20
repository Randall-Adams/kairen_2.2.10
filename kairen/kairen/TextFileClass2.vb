Public Class TextFileClass2

    Private vLines(-1) As String
    Private vIsCommented() As Boolean ' Not a Property
    Private vFilePath As String
    Private vFileName As String
    Private vCommentMarker_singleLine As String
    Private vCommentMarker_multiLineStart As String
    Private vCommentMarker_multiLineEnd As String
    Private vindex_ReadFile As Integer = -1
    Private vIOMode As String
    Private vAdditionalData(0) As String

    Private vTag(-1) As String
    Private vControlList(-1) As Object
    Private vControlList_DefaultControlIndex(-1) As Integer 'this variable holds the index value of the control in vControlList that is the default control for this control's tag
    'Delegate Function DataParserDelegate(ByVal _dataTag As String, ByVal _rawData As Object)
    Private vDataParser(-1) As LineEntryClass.DataParserDelegate

    Public vDataEntries(0) As LineEntryClass

    Private vFileChangeMade As Boolean = False
    Private vAllowUIElementsRegistered As Boolean = False
    Private vUIElementsRegistered As Boolean = False
    Private vIsLoaded As Boolean = False
    Private AllowFileDeletion As Boolean = False
    Private OldPath As String
    Private NewLine = vbNewLine

    Public Sub New(ByVal _commentMarker_singleLine As String, ByVal _commentMarker_multiLineStart As String, ByVal _commentMarker_multiLineEnd As String, Optional ByVal _allowFileDeletion As Boolean = False)
        If _commentMarker_singleLine <> Nothing Then
            vCommentMarker_singleLine = _commentMarker_singleLine
        End If
        If _commentMarker_multiLineStart <> Nothing Then
            vCommentMarker_multiLineStart = _commentMarker_multiLineStart
        End If
        If _commentMarker_multiLineEnd <> Nothing Then
            vCommentMarker_multiLineEnd = _commentMarker_multiLineEnd
        End If
        AllowFileDeletion = _allowFileDeletion
    End Sub

#Region "Properties"
    Public Property Line(ByVal i As Integer) As String
        Get
            If vLines(i) <> Nothing Then
                Return vLines(i)
            Else
                'out of bounds error
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            'If i < 0 Then Exit Property
            If vLines.Length < i Then
                ReDim Preserve vLines(i)
            End If
            vLines(i) = value
        End Set
    End Property
    Public ReadOnly Property Line() As String()
        Get
            Return vLines
        End Get
    End Property
    Public ReadOnly Property FullPath() As String
        Get
            Return vFilePath & vFileName
        End Get
    End Property
    Public Property FilePath() As String
        Get
            Return vFilePath
        End Get
        Set(value As String)
            If My.Computer.FileSystem.FileExists(value & vFileName) = False Then
                My.Computer.FileSystem.MoveFile(vFilePath & vFileName, value & vFileName)
                vFilePath = value
            End If
        End Set
    End Property
    Public Property FileName() As String
        Get
            Return vFileName
        End Get
        Set(value As String)
            If My.Computer.FileSystem.FileExists(vFilePath & value) = False Then
                My.Computer.FileSystem.RenameFile(vFilePath & vFileName, value)
                vFileName = value
            End If
        End Set
    End Property
    Public Property CommentMarker(ByVal line As String) As String
        Get
            Return vCommentMarker_singleLine
        End Get
        Set(value As String)
            vCommentMarker_singleLine = value
        End Set
    End Property
    Public Property CurrentIndex As Integer
        Get
            Return vindex_ReadFile
        End Get
        Set(value As Integer)
            vindex_ReadFile = value
        End Set
    End Property
    Public Property AdditionalData(ByVal i As Integer) As String
        Get
            Return vAdditionalData(i)
        End Get
        Set(value As String)
            If vAdditionalData.Length <= i Then
                ReDim Preserve vAdditionalData(i)
            End If
            vAdditionalData(i) = value
        End Set
    End Property
    Public ReadOnly Property AdditionalData() As String()
        Get
            Return vAdditionalData
        End Get
    End Property
    Public ReadOnly Property IsLoaded() As Boolean
        Get
            Return vIsLoaded
        End Get
    End Property
    Public Property UIElementsRegistered() As Boolean
        Get
            Return vUIElementsRegistered
        End Get
        Set(value As Boolean)
            If value = True Then
                If vAllowUIElementsRegistered = True Then
                    vUIElementsRegistered = value
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property FileHasChanged() As Boolean
        Get
            For Each entry In vDataEntries
                If entry.FileHasChanged = True Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property
#End Region

#Region "Functions"
    Public Sub SaveFile(Optional ByVal _newPath As String = Nothing)
        If vIsLoaded = False Then Exit Sub
        If _newPath IsNot Nothing Then
            OldPath = vFilePath
            vFilePath = _newPath
        End If
        Dim cont As Boolean = True
        Dim sw As IO.StreamWriter
        Try
            sw = New IO.StreamWriter(vFilePath)
            sw.NewLine = ""
            If vLines.Length = Nothing Then Exit Sub
            Dim lastLineMarker As Integer = vLines.Length
            Dim i As Integer = 0
            For Each _line In vLines
                If IsNothing(_line) = False Then
                    If _line.Trim <> "" Then
                        sw.WriteLine(_line)
                        If i < (lastLineMarker - 1) And _line <> "" Then
                            sw.WriteLine(vbCrLf)
                        End If
                    End If
                End If
                i = i + 1
            Next
        Catch ex As Exception
            cont = False
            vFilePath = OldPath
            OldPath = ""
        Finally
            sw.Close()
        End Try
        If cont = False Then Exit Sub
        My.Computer.FileSystem.DeleteFile(OldPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
        OldPath = ""
    End Sub
    Public Sub LoadFile()
        If My.Computer.FileSystem.FileExists(vFilePath) = False Then Exit Sub
        If UIElementsRegistered = False Then Exit Sub
        Try
            Dim sr As New IO.StreamReader(vFilePath)
            Dim i As Integer = 0
            Do Until sr.EndOfStream()
                If i >= vLines.Length Then ' this should only redim when the below trim() wasn't = ""
                    ReDim Preserve vLines(i)
                    ReDim Preserve vIsCommented(i)
                End If
                vLines(i) = sr.ReadLine() ' Reads file's line to variable
                If vLines(i).Trim <> "" Then 'if the line is blank, don't read it and skip over it
                    If vCommentMarker_singleLine <> Nothing Then
                        If Left(vLines(i), vCommentMarker_singleLine.Length) = vCommentMarker_singleLine Then ' Check if Line is a Comment
                            ' Line is a Comment
                            vIsCommented(i) = True
                        Else
                            ' Line is Not a Comment
                            vIsCommented(i) = False
                        End If
                    Else
                        ' Line is Not a Comment
                        vIsCommented(i) = False
                    End If
                    i = i + 1
                End If
            Loop
            sr.Close()
            vIsLoaded = True
            vFileChangeMade = False
        Catch ex As Exception

        End Try
    End Sub
    Public Function ReadLine()
        If UIElementsRegistered = False Then Return Nothing
        'idk if i tried this before, but what if i pass in the additional data for the present readline?
        'If vindex_ReadFile = -1 Then
        '    Return Nothing
        'End If
        vindex_ReadFile = vindex_ReadFile + 1 ' increase index to next line..
        'If vLines.Length = Nothing Then Return Nothing 'there are no lines to return
        If vLines Is Nothing Then Return Nothing
        If vindex_ReadFile >= vLines.Length Then Return Nothing 'if you are trying to read past the end of file then return nothing..
        Dim _line As String = vLines(vindex_ReadFile) ' read new index line..
        'If _line = "" Then Return Nothing ' if line is blank, don't read it and return nothing'
        Do While vLines(vindex_ReadFile) <> Nothing
            If vCommentMarker_singleLine = Nothing Then
                Exit Do
            Else
                If Left(_line, vCommentMarker_singleLine.Length) = vCommentMarker_singleLine Then ' if it's a comment..
                    vindex_ReadFile = vindex_ReadFile + 1 ' increase index..
                    If vindex_ReadFile >= vLines.Length Then Return Nothing
                    _line = vLines(vindex_ReadFile) ' read line at new index..
                Else
                    Exit Do
                End If

            End If
        Loop ' then skip the comment and attempt to loop again..

        ' when not a comment or when no more lines..
        If vLines(vindex_ReadFile) = Nothing Then Return Nothing ' if no more lines..
        Return vLines(vindex_ReadFile) ' if not a comment..
    End Function
    Public Sub WriteLine(ByVal _newline As String)
        If UIElementsRegistered = False Then Exit Sub
        If _newline = Nothing Then Exit Sub
        If _newline.Trim = "" Then Exit Sub
        vindex_ReadFile = vindex_ReadFile + 1 ' increase index to next line..
        ReDim Preserve vLines(vindex_ReadFile)
        vLines(vindex_ReadFile) = _newline ' replace old line with the new one..
        vFileChangeMade = True
    End Sub
    Public Sub WriteAllLines(ByVal _newlines() As String)
        If UIElementsRegistered = False Then Exit Sub
        If _newlines Is Nothing Then Exit Sub
        vLines = _newlines
    End Sub
    Public Sub UpdateValueByAdditionalData(ByVal _text As String, ByVal _newvalue As Object)
        If UIElementsRegistered = False Then Exit Sub
        If TypeOf _newvalue Is String Then
        ElseIf TypeOf _newvalue Is TextBox Then
            _newvalue = _newvalue.Text
        End If
        If _newvalue = "1" Then _newvalue = "true" Else If _newvalue = "0" Then _newvalue = "false"
        Dim i As Integer = 0
        Try
            Do Until vAdditionalData(i) = _text
                i = i + 1
                If i >= AdditionalData.Length Then Exit Sub
            Loop
        Catch ex As Exception
            MsgBox("An error occured in TextFileClass.UpdateValueByText(). Please give the data from the next window to the developer, as this error is completely unprogrammed for." & NewLine & _
                   "(" & _text & ", " & _newvalue & ")", MsgBoxStyle.Critical, "Error!")
            InputBox("This is the data the developer needs. Thank you, and I am sorry for the inconvenience.", "Error Help", ex.Message)
            Exit Sub
        End Try
        vLines(i) = _newvalue
    End Sub
    'Public Function GetValueByAdditionalData(ByVal _text As String, Optional ByRef _objectToSetTo As Object = Nothing, Optional _dataParser As DataParserDelegate = Nothing)
    Public Function GetValueByAdditionalData(ByVal _text As String, Optional ByRef _objectToSetTo As Object = Nothing, Optional _dataParser As LineEntryClass.DataParserDelegate = Nothing)
        'this could be altered to return an error/success code only, and only set data to sent in object
        If UIElementsRegistered = False Then Return -1
        Dim i As Integer = 0
        Dim returnValue As Object
        Dim errorRaised As Boolean = False
        Try
            Do Until (vAdditionalData.Length = i) Or (vAdditionalData(i) = _text)
                i = i + 1
                If vAdditionalData.Length = i Then
                    'additional data not found
                    errorRaised = True
                    returnValue = -1
                    Exit Do
                End If
            Loop
        Catch ex As Exception
            MsgBox("An error occured in TextFileClass.GetValueByText(). Please give the data from the next window to the developer, as this error has nev." & NewLine & _
                   "(" & _text & ")", MsgBoxStyle.Critical, "Error - GetValueByText()")
            InputBox("This is the data the developer needs. Thank you, and I am sorry for the inconvenience.", "Error Help - GetValueByText()", ex.Message)
            errorRaised = True
            returnValue = -2
        End Try
        If errorRaised = True Then
            Return returnValue
        Else
            returnValue = vLines(i)
            If _objectToSetTo IsNot Nothing Then
                If _dataParser IsNot Nothing Then
                    '_dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                Else
                    Dim _objectType As String = TypeName(_objectToSetTo)
                    Select Case _objectType
                        Case "TextBox", "Label"
                            If _dataParser IsNot Nothing Then
                                ' _objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                            Else
                                _objectToSetTo.Text = vLines(i)
                            End If
                        Case "CheckBox"
                            If vLines(i) = "true" Then
                                _objectToSetTo.Checked = True
                            ElseIf vLines(i) = "false" Then
                                _objectToSetTo.Checked = False
                            Else
                                'error or false, idk yet ..
                            End If
                        Case "CheckedListBox"
                            Dim i2 As Integer
                            Do Until i2 = _objectToSetTo.Items.Count - 1
                                If _objectToSetTo.Items.Item(i2) = _text Then
                                    If _dataParser IsNot Nothing Then
                                        '_objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                                    Else
                                        If vLines(i2) = "true" Then
                                            _objectToSetTo.Items.Item(i2).Checked = True
                                        ElseIf vLines(i2) = "false" Then
                                            _objectToSetTo.Items.Item(i2).Checked = False
                                        Else
                                            'error or false, i2dk yet ..
                                        End If
                                    End If
                                    Exit Do
                                End If
                                i2 += 1
                            Loop
                        Case "ListBox"
                            Dim i2 As Integer
                            Do Until i2 = _objectToSetTo.Items.Count - 1
                                If _objectToSetTo.Items.Item(i2) = _text Then
                                    If _dataParser IsNot Nothing Then
                                        '_objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                                    Else
                                        If vLines(i2) = "true" Then
                                            _objectToSetTo.Items.Item(i2).Checked = True
                                        ElseIf vLines(i2) = "false" Then
                                            _objectToSetTo.Items.Item(i2).Checked = False
                                        Else
                                            'error or false, i2dk yet ..
                                        End If
                                    End If
                                    Exit Do
                                End If
                                i2 += 1
                            Loop
                    End Select
                End If
            End If
            Return returnValue
        End If
    End Function
    Public Function GetIndexByAdditionalData(ByVal _text As String)
        If UIElementsRegistered = False Then Return -1
        Dim i As Integer = 0
        Try
            Do Until (vAdditionalData.Length = i) Or (vAdditionalData(i) = _text)
                i = i + 1
                If vAdditionalData.Length = i Then
                    'additional data not found
                    Return -1
                End If
            Loop
            'Do While i < NumberOfLines() And i <= vAdditionalData.Length
            '    If vAdditionalData(i) = _text Then
            '         Return i
            '    End If
            '    i = i + 1
            'Loop
        Catch ex As Exception
            MsgBox("An error occured in TextFileClass.GetIndexByText(). Please give the data from the next window to the developer, as this error is completely unprogrammed for." & NewLine & _
                  "(" & _text & ")", MsgBoxStyle.Critical, "Error - GetValueByText()")
            InputBox("This is the data the developer needs. Thank you, and I am sorry for the inconvenience.", "Error Help - GetIndexByText()", ex.Message)
            Return -2
        End Try

        Return i
    End Function
    Public Function GetIndexByValue(ByVal _value As String, Optional ByVal StartIndex As Integer = 0, Optional ByVal EndIndex As Integer = 0)
        If StartIndex < 0 Then Return StartIndex
        If EndIndex < 0 Then Return EndIndex
        'reads the start index and the end index
        Dim i As Integer
        For Each _line In vLines
            If i >= StartIndex Then
                If _line = _value Then
                    Return i
                ElseIf EndIndex <> 0 And i > EndIndex Then
                    Return -1
                End If
            End If
            i = i + 1
        Next
        Return -1
    End Function
    Public Function FileExists() As Boolean
        Return My.Computer.FileSystem.FileExists(vFilePath)
    End Function

    Public Function GetDataByTag(ByVal _tag As String, Optional ByVal _returnIndex As Integer = 0) As Object
        For Each entry In vDataEntries
            If entry.Name = _tag Then
                If _returnIndex = 0 Then
                    Return entry.GetData(_returnIndex)
                ElseIf _returnIndex = -1 Then
                    Return entry.GetDataArray
                Else
                    If entry.GetData(_returnIndex) <> Nothing Then
                        Return entry.GetData(_returnIndex)
                    Else
                        Return ""
                    End If
                End If
            End If
        Next
        Return ""
    End Function

    Public Function NumberOfLines()
        If vLines Is Nothing Then Return 0
        Return vLines.Length
    End Function
    Sub DeleteFile(Optional ByVal _path As String = Nothing)
        If _path = Nothing Then
            _path = FullPath
        End If
        Try
            If AllowFileDeletion = False Then
                My.Computer.FileSystem.DeleteFile(_path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            ElseIf AllowFileDeletion = True Then
                My.Computer.FileSystem.DeleteFile(_path, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub RegisterUIElement1(ByVal _tag As String, ByRef _control As Windows.Forms.Control, Optional _dataParser As LineEntryClass.DataParserDelegate = Nothing, Optional ByVal _AllowMultiple As Boolean = False)
        'could make registering a tag a second time be support for multiple controls, and have an unregister sub where you match the 
        ' signature of the tag/control/parser or it doesn't unregister anything
        If _tag.Trim = "" Then Exit Sub
        If _control Is Nothing Then Exit Sub
        Dim ThisIsANewTag As Boolean = True
        Dim FirstIndexOfTag As Integer = -2
        Dim i As Integer
        Do Until i >= vTag.Length 'match as #001
            If vTag(i) = _tag Then
                'this is the tag we are registering, it's already registered so..
                If ThisIsANewTag = True Then
                    FirstIndexOfTag = i
                    ThisIsANewTag = False
                End If
                If _AllowMultiple = False Then
                    'tag already registered, allowmultiple flag is off, so exit sub
                    Exit Sub
                Else
                    'tag already registered, allowmultiple flag is on, so cause next increment to raise to desired index 
                    ' and auto-exit loop and then create the tag at the new index
                    i = vTag.Length - 1 'match as #001
                End If
                ''overwrite flag's old contol with new one
                'vControlList(i) = _control
                'vDataParser(i) = _dataParser
                'Exit Sub
            End If
            i += 1
        Loop
        'tag was either not found or allowed to have a second register, so register it!
        ReDim Preserve vTag(i)
        vTag(i) = _tag
        ReDim Preserve vControlList(i)
        vControlList(i) = _control
        ReDim Preserve vControlList_DefaultControlIndex(i)
        If ThisIsANewTag Then
            vControlList_DefaultControlIndex(i) = i
        Else
            vControlList_DefaultControlIndex(i) = FirstIndexOfTag
        End If
        ReDim Preserve vDataParser(i)
        vDataParser(i) = _dataParser
        vAllowUIElementsRegistered = True
    End Sub
    Public Sub UpdateUIElements1()
        Dim i As Integer
        Do Until i = vTag.Length
            GetValueByAdditionalData(vTag(i), vControlList(i), vDataParser(i))
            i += 1
        Loop
    End Sub
    Public Sub UpdateFileByUIElements1()
        'updates the file based off the data in the UI elements
        'this should also use a function to know what the ui element data needs translated into before being saved to the raw file
        Dim i As Integer
        Do Until i = vTag.Length
            UpdateValueByAdditionalData(vTag(i), vControlList(i))
            i += 1
        Loop
        SaveFile()
    End Sub
    Public Sub RegisterUIElement2(ByVal _tag As String, ByRef _control As Windows.Forms.Control, Optional _dataParser As LineEntryClass.DataParserDelegate = Nothing, Optional ByVal _AllowMultiple As Boolean = False, Optional _entryType As String = "Single Line")
        'could make registering a tag a second time be support for multiple controls, and have an unregister sub where you match the 
        ' signature of the tag/control/parser or it doesn't unregister anything
        If _tag.Trim = "" Then Exit Sub
        If _control Is Nothing Then Exit Sub
        Dim ThisIsANewTag As Boolean = True
        Dim FirstIndexOfTag As Integer = -2
        Dim i As Integer
        Do Until i >= vDataEntries.Count 'match as #001
            If vDataEntries(i).Name = _tag Then
                'this is the tag we are registering, it's already registered so..
                If ThisIsANewTag = True Then
                    FirstIndexOfTag = i
                    ThisIsANewTag = False
                End If
                If _AllowMultiple = False Then
                    'tag already registered, allowmultiple flag is off, so exit sub
                    Exit Sub
                Else
                    'tag already registered, allowmultiple flag is on, so cause next increment to raise to desired index 
                    ' and auto-exit loop and then create the tag at the new index
                    i = vDataEntries.Count - 1 'match as #001
                End If
                ''overwrite flag's old contol with new one
                'vControlList(i) = _control
                'vDataParser(i) = _dataParser
                'Exit Sub
            End If
            i += 1
        Loop
        'tag was either not found or allowed to have a second register, so register it!
        ReDim Preserve vDataEntries(i)
        'vDataEntries(i) = New LineEntryClass(_tag, _entryType)
        'ReDim Preserve vControlList(i)
        'vControlList(i) = _control
        'ReDim Preserve vControlList_DefaultControlIndex(i)
        If ThisIsANewTag Then FirstIndexOfTag = i
        'vControlList_DefaultControlIndex(i) = i
        vDataEntries(i) = New LineEntryClass(_tag, _entryType, _control, _dataParser, i)
        
        'ReDim Preserve vDataParser(i)
        'vDataParser(i) = _dataParser
        vAllowUIElementsRegistered = True
        UIElementsRegistered = True
    End Sub
    Public Sub RegisterUIElement3(ByVal _tag As String, ByRef _control As Windows.Forms.Control, Optional _dataParser As LineEntryClass.DataParserDelegate = Nothing, Optional ByVal _AllowMultiple As Boolean = False, Optional _entryType As String = "Single Line")
        'could make registering a tag a second time be support for multiple controls, and have an unregister sub where you match the 
        ' signature of the tag/control/parser or it doesn't unregister anything
        If _tag.Trim = "" Then Exit Sub
        If _control Is Nothing Then Exit Sub
        Dim ThisIsANewTag As Boolean = True
        Dim FirstIndexOfTag As Integer = -2
        Dim i As Integer
        Do Until i > vDataEntries.Count - 1 'match as #001
            If vDataEntries(i) Is Nothing Then
                Exit Do
            ElseIf vDataEntries(i).Name = _tag Then
                'this is the tag we are registering, it's already registered so..
                If ThisIsANewTag = True Then
                    FirstIndexOfTag = i
                    ThisIsANewTag = False
                End If
                If _AllowMultiple = False Then
                    'tag already registered, allowmultiple flag is off, so exit sub
                    Exit Sub
                Else
                    'tag already registered, allowmultiple flag is on, so cause next increment to raise to desired index 
                    ' and auto-exit loop and then create the tag at the new index
                    i = vDataEntries.Count - 1 'match as #001
                End If
                ''overwrite flag's old contol with new one
                'vControlList(i) = _control
                'vDataParser(i) = _dataParser
                'Exit Sub
            End If
            i += 1
        Loop
        'tag was either not found or allowed to have a second register, so register it!
        ReDim Preserve vDataEntries(i)
        'vDataEntries(i) = New LineEntryClass(_tag, _entryType)
        'ReDim Preserve vControlList(i)
        'vControlList(i) = _control
        'ReDim Preserve vControlList_DefaultControlIndex(i)
        If ThisIsANewTag Then FirstIndexOfTag = i
        'vControlList_DefaultControlIndex(i) = i
        vDataEntries(i) = New LineEntryClass(_tag, _entryType, _control, _dataParser, FirstIndexOfTag)
        'ReDim Preserve vDataParser(i)
        'vDataParser(i) = _dataParser
        vAllowUIElementsRegistered = True
        UIElementsRegistered = True
    End Sub
    Public Sub RegisterUIElement(ByVal _tag As String, ByRef _dataControl As Object, Optional ByRef _dataPaser As LineEntryClass.DataParserDelegate = Nothing, Optional ByVal _AllowMultiple As Boolean = False, Optional _entryType As String = "Single Line")
        'could make registering a tag a second time be support for multiple controls, and have an unregister sub where you match the 
        ' signature of the tag/control/parser or it doesn't unregister anything
        If _tag.Trim = "" Then Exit Sub
        Dim ThisIsANewTag As Boolean = True
        Dim FirstIndexOfTag As Integer = -2
        Dim i As Integer
        'Loop to compare this tag with all the others in case it is already registered.
        Do Until i > vDataEntries.Count - 1 'match as #001
            If vDataEntries(i) Is Nothing Then
                Exit Do
            ElseIf vDataEntries(i).Name = _tag Then
                'this is the tag we are registering, it's already registered so..
                If ThisIsANewTag = True Then
                    FirstIndexOfTag = i
                    ThisIsANewTag = False
                End If
                If _AllowMultiple = False Then
                    'tag already registered, allowmultiple flag is off, so exit sub
                    Exit Sub
                Else
                    'tag already registered, allowmultiple flag is on, so cause next increment to raise to desired index 
                    ' and auto-exit loop and then create the tag at the new index
                    i = vDataEntries.Count - 1 'match as #001
                End If
                ''overwrite flag's old contol with new one
                'vControlList(i) = _control
                'vDataParser(i) = _dataParser
                'Exit Sub
            End If
            i += 1
        Loop
        'tag was either not found or allowed to have a second register, so register it!
        ReDim Preserve vDataEntries(i)
        'vDataEntries(i) = New LineEntryClass(_tag, _entryType)
        'ReDim Preserve vControlList(i)
        'vControlList(i) = _control
        'ReDim Preserve vControlList_DefaultControlIndex(i)
        If ThisIsANewTag Then FirstIndexOfTag = i
        'vControlList_DefaultControlIndex(i) = i
        vDataEntries(i) = New LineEntryClass(_tag, _entryType, _dataControl, _dataPaser, FirstIndexOfTag)
        'ReDim Preserve vDataParser(i)
        'vDataParser(i) = _dataParser
        vAllowUIElementsRegistered = True
        UIElementsRegistered = True
    End Sub
    Public Sub UpdateFileByUIElements()
        'updates the file based off the data in the UI elements
        'this should also use a function to know what the ui element data needs translated into before being saved to the raw file 'this is taken care of now

        For Each entry In vDataEntries
            entry.UpdateRAM()
        Next
    End Sub
    Public Sub UpdateUIElementsByFile()
        For Each entry In vDataEntries
            entry.UpdateUI()
        Next
    End Sub
    Public Function UpdateFileEntry(ByVal _additionalData As String, Optional _useInsteadOfRegisteredControl As String = Nothing)
        'since the file has it's data registered, now it can grab the new value from the control, 
        ' or it can be provided as _newValueOverride
        'this may need flags to prevent multiple/looping firings
        If UIElementsRegistered = False Then Return -1
        Dim i As Integer
        Do Until i >= vTag.Length 'match as #001
            If vTag(i) = _additionalData Then
                If _useInsteadOfRegisteredControl <> Nothing Then
                    'update file entry based off the sent-in data
                    UpdateValueByAdditionalData(vTag(i), _useInsteadOfRegisteredControl)
                Else
                    'update file entry based off the UI's control
                    UpdateValueByAdditionalData(vTag(i), vControlList(vControlList_DefaultControlIndex(i)))
                End If
                'also update the tag's UI stuff if it has any
            End If
            i += 1
        Loop
        If i > vTag.Length Then Return -2
        Return 0
    End Function
    Public Function UpdateFileEntryByUIElement(ByVal _controlToUpdateFrom As Control)
        'this function will receieve a control and then find it in the list of controls, get the corresponding tag,
        ' and then once knowing the tag of the control it will update the tag like normal, updating all controls including
        ' the passed in control

        'since the file has it's data registered, now it can grab the new value from the control, 
        ' or it can be provided as _newValueOverride
        'this may need flags to prevent multiple/looping firings
        If UIElementsRegistered = False Then Return -1
        Dim i As Integer
        Do Until i >= vControlList.Length 'match as #001
            If vControlList(i) Is _controlToUpdateFrom Then
                'update file entry based off the UI's control
                'also update the tag's UI stuff if it has any
            End If
            i += 1
        Loop
        If i > vTag.Length Then Return -2
        Return 0
    End Function
    Private Function ChangeDataEntry(ByVal _additionalData As String, ByVal _newValue As String)
        For Each entry In vDataEntries
            If entry.Name = _additionalData Then
                entry.SetData(_newValue)
                Return 0
            End If
        Next
        Return -1
    End Function
    Public Function SaveFile_new(Optional ByVal _newFilePath As String = Nothing, Optional ByVal _newFileName As String = Nothing)
        UpdateFileByUIElements()
        Dim CurrentTag As String
        If IsLoaded = False Then
            vFilePath = _newFilePath
            vFileName = _newFileName
        End If
        Try
            Dim sw As New IO.StreamWriter(FullPath & ".temp")
            sw.NewLine = ""
            Dim isEntryFirstLine As Boolean = True
            For Each entry In vDataEntries
                CurrentTag = entry.Name
                sw.WriteLine("[Start] " & entry.Name)
                sw.WriteLine(vbCrLf)
                For Each item In entry.GetDataArray
                    If item Is Nothing Then
                        sw.Close()
                        DeleteFile()
                        Return -2
                    End If
                    If item.Trim <> "" Then
                        sw.WriteLine(item.trim)
                        sw.WriteLine(vbCrLf)
                    ElseIf item.Trim = "" Then
                        sw.Close()
                        DeleteFile()
                        Return -3
                    End If
                Next
                sw.WriteLine("[End] " & entry.Name)
                If isEntryFirstLine = True Then
                    sw.WriteLine(vbCrLf)
                    isEntryFirstLine = False
                Else
                    If entry IsNot vDataEntries(vDataEntries.Count - 1) Then
                        sw.WriteLine(vbCrLf)
                    End If
                End If
            Next
            sw.Close()
        Catch ex As Exception
            MsgBox("Ut-oh! An error happened on this tag: " & CurrentTag)
            Return -4
        End Try
        For Each entry In vDataEntries
            entry.FileHasChanged = False
        Next
        If My.Computer.FileSystem.FileExists(FullPath & ".temp") Then
            DeleteFile()
            My.Computer.FileSystem.RenameFile(FullPath & ".temp", vFileName)
        End If
        If _newFilePath IsNot Nothing Then
            If _newFilePath <> vFilePath Then
                If _newFileName IsNot Nothing Then
                    If _newFileName <> vFileName Then
                        My.Computer.FileSystem.MoveFile(FullPath, _newFilePath & _newFileName)
                        vFilePath = _newFilePath
                        vFileName = _newFileName
                        Return 3 'rename and repath file successful
                    End If
                Else
                    My.Computer.FileSystem.MoveFile(FullPath, _newFilePath & vFileName)
                    vFilePath = _newFilePath
                    Return 2 'repath file successful
                End If
            End If
        End If
        If _newFileName IsNot Nothing Then
            If _newFileName <> vFileName Then
                My.Computer.FileSystem.RenameFile(FullPath, _newFileName)
                vFileName = _newFileName
                Return 1 'rename file successful
            End If
        End If
        Return 0
    End Function
    Public Function LoadFile_new(ByVal _filePath As String, ByVal _fileName As String)
        If My.Computer.FileSystem.FileExists(_filePath & _fileName) = False Then Return -1
        vFilePath = _filePath
        vFileName = _fileName
        'If UIElementsRegistered = False Then Return -2 'this isn't needed? it can declare them below as needed..
        Try
            Dim sr As New IO.StreamReader(FullPath)
            Dim i As Integer = 0
            Do Until sr.EndOfStream()
                Dim vlinespreread As String = sr.ReadLine() ' Reads file's line to variable
                If vlinespreread.Trim <> "" Then 'if the line is blank, don't read it and skip over it
                    ReDim Preserve vLines(i)
                    vLines(i) = vlinespreread
                    i = i + 1
                End If
            Loop
            sr.Close()
            'file has been read to variables, now parse those variables to sort the data into the entry class
            Dim eos As Integer = i
            i = 0
            Do Until i >= vLines.Length
                If Left(vLines(i), 8) <> "[Start] " Then
                    'error no start for a tag
                    Return -3
                Else
                    Dim i2 As Integer = i 'marks start index of current tag
                    Dim TagName As String 'current tag's name
                    TagName = Right(vLines(i), vLines(i).Length - 8)
                    Do While vLines(i2) <> "[End] " & TagName
                        i2 += 1
                    Loop
                    Dim EntryType As String 'This is only for tags that weren't registered that are found in the file
                    If i2 > i + 2 Then
                        'this is a "Multiple Lines" entry type
                        EntryType = "Multiple Lines"
                    ElseIf i2 = i + 2 Then
                        EntryType = "Single Line"
                    ElseIf i2 = i + 1 Then
                        'the data section of the tag is empty or missing
                        Return -6
                    Else
                        'error
                        Return -5
                    End If
                    If i2 >= eos Then
                        'error no end for current tag
                        Return -4
                    Else
                        'end for current tag was found, so continue
                        Dim DataEntriesIndex As Integer ' = LEC.Length - 1
                        Dim i3 As Integer = 0
                        Do While i3 <= vDataEntries.Length - 1
                            If vDataEntries(i3).Name = TagName Then
                                DataEntriesIndex = i3
                                Exit Do
                            End If
                            i3 += 1
                        Loop
                        If i3 = vDataEntries.Length Then ' does this create a new data entry for the tag if one wasn't registered for it?
                            DataEntriesIndex = vDataEntries.Length
                            ReDim Preserve vDataEntries(DataEntriesIndex)
                            vDataEntries(DataEntriesIndex) = New LineEntryClass(TagName, EntryType, Nothing, Nothing, DataEntriesIndex, False)
                        End If

                        i += 1
                        Do Until vLines(i) = "[End] " & TagName
                            Try
                                vDataEntries(DataEntriesIndex).SetData(vLines(i))
                            Catch ex As Exception
                                Return -7
                            End Try
                            i += 1
                        Loop
                        i += 1
                    End If
                End If
            Loop
            vIsLoaded = True
            vFileChangeMade = False
            If UIElementsRegistered = True Then
                UpdateUIElementsByFile()
            End If
        Catch ex As Exception
            Return -6
        End Try
        For Each entry In vDataEntries
            entry.FileHasChanged_IsLocked = False
        Next
        Return 0
    End Function
#End Region

#Region "Delegate Test"
    Dim TheQuestion As QuestionParserDelegate
    Delegate Function QuestionParserDelegate(ByVal _additionaldata As String, ByRef _control As Object)

    Public Sub GetAnswer()
        MsgBox("Your answer was: " & TheQuestion(InputBox(TheQuestion("What is the question?", Nothing), "The Question is:", ""), Nothing), MsgBoxStyle.OkOnly, "The Result of Your Answer is:")
    End Sub

    Public Sub SetQuestionAndAnswer(Optional _theQuestion As QuestionParserDelegate = Nothing)
        TheQuestion = _theQuestion
    End Sub
#End Region
End Class
