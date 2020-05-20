Public Class TextFileClass

    Private vLines() As String
    Private vIsComment() As Boolean ' Not a Property
    Private vFilePath As String
    Private vCommentMarker As String
    Private vindex_ReadFile As Integer = -1
    Private vIOMode As String
    Private vAdditionalData(0) As String

    Private vvTag(-1, -1) As String '(y=0 tag, y=1 tag's index)
    Private vvControlList(-1, -1) As Object '(y=0 tag's index, y=1 control)
    Private vDataParserFunctionSet() As Boolean

    Private vTag(-1) As String
    Private vControlList(-1) As Object
    Private vDataParser(-1) As DataParserDelegate
    Delegate Function DataParserDelegate(ByVal _additionaldata As String, ByRef _control As Object, ByVal _text As String)

    Private vIsLoaded As Boolean = False
    Private AllowFileDeletion As Boolean = False
    Private NewLine = vbNewLine

    Public Sub New(ByVal _filepath As String, ByVal _commentMarker As String, Optional ByVal WithDeletes As Boolean = False, Optional AutoLoad As Boolean = False)
        vFilePath = _filepath
        If _commentMarker <> Nothing Then
            vCommentMarker = _commentMarker
        End If
        AllowFileDeletion = WithDeletes
        If AutoLoad = True Then
            LoadFile()
        End If
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
    Public Property FilePath() As String
        Get
            Return vFilePath
        End Get
        Set(value As String)
            vFilePath = value
        End Set
    End Property
    Public Property CommentMarker(ByVal line As String) As String
        Get
            Return vCommentMarker
        End Get
        Set(value As String)
            vCommentMarker = value
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
    Private Sub RegisterUIElement0(ByVal _tag As String, ByRef _control As Windows.Forms.Control)
        Dim _tagindex As Integer
        Dim x As Integer = vvTag.GetLength(0)
        Do While _tagindex < x
            If vvTag(_tagindex, 0) = _tag Then
                'this is the tag we are adding a control to
                Dim _controlindex As Integer
                Do While _controlindex <= vvControlList.GetLength(1)
                    If vvControlList(vvTag(_tagindex, 1), _controlindex) Is _control Then
                        'this is the control we are adding to the list, so exit
                        Exit Sub
                    End If
                    _controlindex = _controlindex + 1
                Loop
                'control is not in the tag's list, so add it
                ReDim Preserve vvControlList(vvTag(_tagindex, 1), _controlindex)
                vvControlList(vvTag(_tagindex, 1), _controlindex) = _control
                Exit Sub
            End If
            _tagindex = _tagindex + 1
        Loop
        'create tag
        ReDim Preserve vvTag(_tagindex, 1)
        vvTag(_tagindex, 0) = _tag
        x = vvTag.GetLength(0) - 1
        vvTag(_tagindex, 1) = x ' make the vTag value equal to the index of the tag's index in the vControlList
        ReDim Preserve vvControlList(vvTag(_tagindex, 1), 1)
        vvControlList(vvTag(_tagindex, 1), 0) = vvTag(_tagindex, 1)
        vvControlList(vvTag(_tagindex, 1), 1) = _control
        Exit Sub
        x = vvControlList.GetLength(0) - 1
    End Sub
    Private Sub UpdateUIElements0()
        'updates the ui when the file loads and stuff
        Dim _tagindex As Integer
        'Dim _controlindex As Integer
        '        vControlList(vTag(_tagindex, 1), 1) = GetValueByAdditionalData(vTag(_tagindex, _controlindex))
        Dim x As Integer = vvTag.GetLength(0)
        Do While _tagindex < x
            'this is the tag we are adding a control to
            Dim _controlindex As Integer
            Do While _controlindex < vvControlList.GetLength(1) - 1
                Dim poo As String = GetValueByAdditionalData(vvTag(_tagindex, _controlindex))
                vvControlList(vvTag(_tagindex, 1), 1).Text = poo
                _controlindex = _controlindex + 1
            Loop
            _tagindex = _tagindex + 1
        Loop
    End Sub
    Private Sub RegisterUIElement1(ByVal _tag As String, ByRef _control As Windows.Forms.Control)
        Dim _tagindex As Integer
        Dim x As Integer = vvTag.GetLength(1)
        Do While _tagindex < x
            If vvTag(0, _tagindex) = _tag Then
                'this is the tag we are adding a control to
                Dim _controlindex As Integer
                Do While _controlindex <= vvControlList.GetLength(0)
                    If vvControlList(_controlindex, vvTag(1, _tagindex)) Is _control Then
                        'this is the control we are adding to the list, so exit
                        Exit Sub
                    End If
                    _controlindex = _controlindex + 1
                Loop
                'control is not in the tag's list, so add it
                ReDim Preserve vvControlList(_controlindex, vvTag(1, _tagindex))
                vvControlList(_controlindex, vvTag(1, _tagindex)) = _control
                Exit Sub
            End If
            _tagindex = _tagindex + 1
        Loop
        'create tag
        ReDim Preserve vvTag(1, _tagindex)
        vvTag(0, _tagindex) = _tag
        x = vvTag.GetLength(1) - 1
        vvTag(1, _tagindex) = x ' make the vTag value equal to the index of the tag's index in the vControlList
        ReDim Preserve vvControlList(1, vvTag(1, _tagindex))
        vvControlList(0, vvTag(1, _tagindex)) = vvTag(1, _tagindex)
        vvControlList(1, vvTag(1, _tagindex)) = _control
        Exit Sub
        x = vvControlList.GetLength(1) - 1
    End Sub
    Private Sub UpdateUIElements1()
        'updates the ui when the file loads and stuff
        Dim _tagindex As Integer
        'Dim _controlindex As Integer
        '        vControlList(vTag(_tagindex, 1), 1) = GetValueByAdditionalData(vTag(_tagindex, _controlindex))
        Dim x As Integer = vvTag.GetLength(1)
        Do While _tagindex < x
            Dim _controlindex As Integer
            Do While _controlindex < vvControlList.GetLength(0) - 1
                Dim poo As String = GetValueByAdditionalData(vvTag(_controlindex, _tagindex))
                vvControlList(1, vvTag(1, _tagindex)).Text = poo
                _controlindex = _controlindex + 1
            Loop
            _tagindex = _tagindex + 1
        Loop
    End Sub
    Public Sub RegisterUIElement(ByVal _tag As String, ByRef _control As Windows.Forms.Control, Optional _dataParser As DataParserDelegate = Nothing, Optional ByVal OverWrite As Boolean = False)
        If _tag.Trim = "" Then Exit Sub
        If _control Is Nothing Then Exit Sub
        Dim i As Integer
        Do Until i = vTag.Length
            If vTag(i) = _tag Then
                'this is the tag we are registering, it's already registered so..
                If OverWrite = False Then
                    'tag already registered, overwrite flag is off, so exit
                    Exit Sub
                End If
                'over flag's old contol with new one
                vControlList(i) = _control
                vDataParser(i) = _dataParser
                Exit Sub
            End If
            i += 1
        Loop
        'tag was not found, so let's register it now
        ReDim Preserve vTag(i)
        vTag(i) = _tag
        ReDim Preserve vControlList(i)
        vControlList(i) = _control
        ReDim Preserve vDataParser(i)
        vDataParser(i) = _dataParser
    End Sub
    Public Sub UpdateUIElements()
        Dim i As Integer
        Do Until i = vTag.Length
            GetValueByAdditionalData(vTag(i), vControlList(i), vDataParser(i))
            i += 1
        Loop
    End Sub
    Public Sub UpdateFileByUIElements()
        'updates the file based off the data in the UI elements
        Dim i As Integer
        Do Until i = vTag.Length
            UpdateValueByAdditionalData(vTag(i), vControlList(i))
            i += 1
        Loop
        SaveFile()
    End Sub
#End Region

#Region "Functions"
    Public Sub SaveFile()
        Dim sw As New IO.StreamWriter(vFilePath)
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
        sw.Close()
    End Sub
    Public Sub LoadFile()
        If My.Computer.FileSystem.FileExists(vFilePath) = False Then Exit Sub
        Try
            Dim sr As New IO.StreamReader(vFilePath)
            Dim i As Integer = 0
            Do Until sr.EndOfStream()
                ReDim Preserve vLines(i)
                ReDim Preserve vIsComment(i)
                vLines(i) = sr.ReadLine ' Reads file's line to variable
                If vLines(i).Trim <> "" Then 'if the line is blank, don't read it and skip over it
                    If vCommentMarker <> Nothing Then
                        If Left(vLines(i), vCommentMarker.Length) = vCommentMarker Then ' Check if Line is a Comment
                            ' Line is a Comment
                            vIsComment(i) = True
                        Else
                            ' Line is Not a Comment
                            vIsComment(i) = False
                        End If
                    Else
                        ' Line is Not a Comment
                        vIsComment(i) = False
                    End If
                    i = i + 1
                End If
            Loop
            sr.Close()
            vIsLoaded = True
        Catch ex As Exception

        End Try
    End Sub
    Public Function ReadLine()
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
            If vCommentMarker = Nothing Then
                Exit Do
            Else
                If Left(_line, vCommentMarker.Length) = vCommentMarker Then ' if it's a comment..
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
        If _newline = Nothing Then Exit Sub
        If _newline.Trim = "" Then Exit Sub
        vindex_ReadFile = vindex_ReadFile + 1 ' increase index to next line..
        ReDim Preserve vLines(vindex_ReadFile)
        vLines(vindex_ReadFile) = _newline ' replace old line with the new one..
    End Sub
    Public Sub WriteAllLines(ByVal _newlines() As String)
        vLines = _newlines
    End Sub
    Public Sub UpdateValueByAdditionalData(ByVal _text As String, ByVal _newvalue As Object)
        If TypeOf (_newvalue) Is String Then
        ElseIf TypeOf (_newvalue) Is TextBox Then
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
    Public Function GetValueByAdditionalData(ByVal _text As String, Optional ByRef _objectToSetTo As Object = Nothing, Optional _dataParser As DataParserDelegate = Nothing)
        'this could be altered to return an error/success code only, and only set data to sent in object
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
                    _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
                Else
                    Dim _objectType As String = TypeName(_objectToSetTo)
                    Select Case _objectType
                        Case "TextBox", "Label"
                            If _dataParser IsNot Nothing Then
                                _objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
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
                                        _objectToSetTo.Text = _dataParser.Invoke(_text, _objectToSetTo, vLines(i))
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

    Public Function NumberOfLines()
        If vLines Is Nothing Then Return 0
        Return vLines.Length
    End Function
    Sub DeleteFile()
        Try
            If AllowFileDeletion = False Then
                My.Computer.FileSystem.DeleteFile(vFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            ElseIf AllowFileDeletion = True Then
                My.Computer.FileSystem.DeleteFile(vFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region
    Dim TheQuestion As QuestionParserDelegate
    Delegate Function QuestionParserDelegate(ByVal _additionaldata As String, ByRef _control As Object)

    Public Sub GetAnswer()
        MsgBox("Your answer was: " & TheQuestion(InputBox(TheQuestion("What is the question?", Nothing), "The Question is:", ""), Nothing), MsgBoxStyle.OkOnly, "The Result of Your Answer is:")
    End Sub

    Public Sub SetQuestionAndAnswer(Optional _theQuestion As QuestionParserDelegate = Nothing)
        TheQuestion = _theQuestion
    End Sub
End Class

