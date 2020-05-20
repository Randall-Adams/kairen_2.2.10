Public Class ResourceFileHandler
    Public RFC() As ResourceFileClass_v2

    Sub New()

    End Sub
    Public Function EverythingIsInstalled(Optional ByVal IgnoreList() As String = Nothing) As Boolean
        If IgnoreList Is Nothing Then
            For Each item In RFC
                If item.FileExists = False Then Return False
            Next
            Return True
        Else
            For Each item In RFC
                For Each ignoreitem In IgnoreList
                    If item.FilePath.Contains(ignoreitem) = False Then
                        If item.FileExists = False Then Return False
                    End If
                Next
            Next
            Return True
        End If
    End Function
    Public Function TheseAreInstalled(ByVal CheckList() As String) As Boolean
        For Each item In RFC
            For Each ignoreitem In CheckList
                If ignoreitem Is Nothing Then

                ElseIf item.FilePath.Contains(ignoreitem) = True Then
                    If item.FileExists = False Then
                        Return False
                    End If
                End If
            Next
        Next
        Return True
    End Function
    Public Sub AddFile(ByVal _filepath As String, ByVal _filename As String, ByVal _fileextension As String, ByRef _resourcefile As Object) ' For Files
        If RFC Is Nothing Then
            ReDim Preserve RFC(0)
        Else
            ReDim Preserve RFC(RFC.Length)
        End If
        RFC(RFC.Length - 1) = New ResourceFileClass_v2(_filepath, _filename, _fileextension, _resourcefile)
    End Sub
    Public Sub AddFile(ByVal _folderpath As String) ' For Folders
        If RFC Is Nothing Then
            ReDim Preserve RFC(0)
        Else
            ReDim Preserve RFC(RFC.Length)
        End If
        RFC(RFC.Length - 1) = New ResourceFileClass_v2(_folderpath)
    End Sub
    Public Sub InstallMissingFiles(Optional ByVal IgnoreList() As String = Nothing)
        If IgnoreList Is Nothing Then
            For Each item In RFC
                If item.FileExists = False Then item.InstallFile()
            Next
        Else
            For Each item In RFC
                For Each ignoreitem In IgnoreList
                    If item.FilePath.Contains(ignoreitem) = False Then
                        If item.FileExists = False Then item.InstallFile()
                    End If
                Next
            Next
        End If
    End Sub
    Public Sub InstallTheseMissingFiles(ByVal CheckList() As String)
        For Each item In RFC
            For Each checkitem In CheckList
                If item.FilePath.Contains(checkitem) = True Then
                    If item.FileExists = False Then item.InstallFile()
                End If
            Next
        Next
    End Sub
End Class
