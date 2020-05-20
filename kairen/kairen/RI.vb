Public Class RI
    Private vName As String
    Private vIsChangeable As Boolean
    Private vIsInstallable As Boolean
    Private vIsUninstallable As Boolean
    Private vType As String
    Private vPath As String
    Public MyResource As Object
    Private vNumberOrChildren As Integer

    Public Property Name As String
        Get
            Return vName
        End Get
        Set(value As String)
            vName = value
        End Set
    End Property
    Public Property IsInstallable As Boolean
        Get
            Return vIsInstallable
        End Get
        Set(value As Boolean)
            vIsInstallable = value
        End Set
    End Property
    ''' <summary>
    ''' Checks if the file is allowed to be uninstalled. If it has not been manually set true or false, it will default to looking at the existence of vNumberOfChildren, and further default to false.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsUninstallable As Boolean
        Get
            If vIsUninstallable = True Then
                If vNumberOrChildren = Nothing Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return vIsUninstallable
            End If
        End Get
        Set(value As Boolean)
            vIsUninstallable = value
        End Set
    End Property
    Public Sub Reset_IsUninstallable()
        If vIsUninstallable <> Nothing Then
            vIsUninstallable = Nothing
        Else
            MsgBox("vIsUninstallable does not exist to reset for: " & vName)
        End If
    End Sub
    Public Property IsChangeable As Boolean
        Get
            Return vIsChangeable
        End Get
        Set(value As Boolean)
            vIsChangeable = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return vType
        End Get
        Set(value As String)
            vType = value
        End Set
    End Property

    Public Property Path As String
        Get
            Return vPath
        End Get
        Set(value As String)
            vPath = value
        End Set
    End Property

    Public Property NumberOfChildren As String
        Get
            If vNumberOrChildren = Nothing Then
                Return 0
            Else
                Return vNumberOrChildren
            End If
        End Get
        Set(value As String)
            vNumberOrChildren = value
        End Set
    End Property


    Public Sub Uninstall()
        If vType = "file" Then
            If Form1.FE(vPath) = True Then
                My.Computer.FileSystem.DeleteFile(vPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            Else
                MsgBox("Tried to delete the file " & Chr(34) & vPath & Chr(34) & ", but it doesn't exist.")
            End If
        ElseIf vType = "folder" Then
            If Form1.DE(vPath) = True Then
                My.Computer.FileSystem.DeleteDirectory(vPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            Else
                MsgBox("Tried to delete the folder " & Chr(34) & vPath & Chr(34) & ", but it doesn't exist.")
            End If
        Else
            MsgBox("RI Error: In Else of Uninstall()")
        End If
    End Sub
    Public Function Exists() As Boolean
        If vType = "file" Then
            Return Form1.FE(vPath)
        ElseIf vType = "folder" Then
            Return Form1.DE(vPath)
        Else
            MsgBox("RI Error: In Else of Exists()")
            Return False
        End If
    End Function
End Class
