Public Class FilePath
    Public Root As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\..\..\..\Rundatshityo\"
    Public EQOA As String = Root & "EQOA\"
    'EQOA
    'Public Custom_Data As New Key(EQOA.Path & "Custom Data\")
    'Public Game_Data As New Key(EQOA.Path & "Game Data\")
    'Public LUAs As New Key(EQOA.Path & "luas\")
    'Public Net_Streams As New Key(EQOA.Path & "net streams\")
    'Public Player_Data As New Key(EQOA.Path & "player data\")
    'Public Temp As New Key(EQOA.Path & "Temp\")

    'Private vRoot As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData & "\..\..\..\Rundatshityo\"
    'Private vEQOA As String = vRoot & "EQOA\"

End Class
Public Class Key
    Public Key As New Key
    Private vPath As String

    Public Sub New(Optional ByVal _path As String = "")
        If _path <> "" Then
            vPath = _path
        End If
    End Sub
    Public Property Path As String
        Get
            Return vPath
        End Get
        Set(value As String)
            vPath = value
        End Set
    End Property
    Public ReadOnly Property Exists As Boolean
        Get
            If My.Computer.FileSystem.DirectoryExists(vPath) Then
                Return True
            Else : Return False
            End If
        End Get
    End Property
End Class