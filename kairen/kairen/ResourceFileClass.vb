Public Class ResourceFileClass
    Private vFilePath As String
    Private vFileName As String
    Private vFileExtension As String
    Private vResourceFile As Object
    Private vType As String

    Public Sub New(ByVal _filepath As String, ByVal _filename As String, ByVal _fileextension As String, ByRef _resourcefile As Object) ' For Files
        vFilePath = _filepath
        vFileName = _filename
        vFileExtension = _fileextension
        vResourceFile = _resourcefile
        vType = "File"
    End Sub
    Public Sub New(ByVal _folderpath As String) ' For Folders
        vFilePath = _folderpath
        vType = "Folder"
    End Sub

    Public Sub InstallFile()
        Select Case vType
            Case "File"
                Dim b() As Byte = vResourceFile 'Name of file in resources
                IO.File.WriteAllBytes(vFilePath & vFileName & vFileExtension, b)
            Case "Folder"
                My.Computer.FileSystem.CreateDirectory(vFilePath)
        End Select
    End Sub
    Public Function FileExists() As Boolean
        Select Case vType
            Case "File"
                Return My.Computer.FileSystem.FileExists(vFilePath & vFileName & vFileExtension)
            Case "Folder"
                Return My.Computer.FileSystem.DirectoryExists(vFilePath)
            Case Else
                Return False
        End Select
    End Function
#Region "Shared"
    Public Shared Function RFNext(ByRef _resourceFileArray() As ResourceFileClass, ByRef i As Integer, ByVal _path As String, Optional ByVal _fileName As String = Nothing, Optional ByVal _fileExtension As String = Nothing, Optional ByVal _resourceFile As Object = Nothing) As ResourceFileClass
        i = i + 1
        ReDim Preserve _resourceFileArray(i)
        If _fileName = Nothing Then
            _resourceFileArray(i) = New ResourceFileClass(_path)
        Else
            _resourceFileArray(i) = New ResourceFileClass(_path, _fileName, _fileExtension, _resourceFile)
        End If
        Return _resourceFileArray(i)
    End Function
    Public Shared Sub InstallAllFiles(ByRef _resourceFileArray() As ResourceFileClass)
        For Each file In _resourceFileArray
            file.InstallFile()
        Next
    End Sub
    Public Shared Function EverythingIsInstalled(ByRef _resourceFileArray() As ResourceFileClass) As Boolean
        For Each file In _resourceFileArray
            If file.FileExists = False Then Return False
        Next
        Return True
    End Function
    Public Shared Sub InstallAllMissingFiles(ByRef _resourceFileArray() As ResourceFileClass)
        For Each file In _resourceFileArray
            If file.FileExists = False Then file.InstallFile()
        Next
    End Sub
    Public Shared Function NothingIsInstalled(ByRef _resourceFileArray() As ResourceFileClass) As Boolean
        For Each file In _resourceFileArray
            If file.FileExists = True Then
                Return False
            End If
        Next
        Return True
    End Function
#End Region

End Class
