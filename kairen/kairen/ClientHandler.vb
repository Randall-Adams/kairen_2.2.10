Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class ClientHandler
    Public vIP As IPAddress
    Public Port As Integer

    Public Sub New(_IP As String, _Port As Integer, _ID As Integer)
        ' Client Data
        vIP = IPAddress.Parse(_IP)
        Port = _Port
    End Sub

    Public Property IP(Optional ReturnType = Nothing) As Object
        Get
            If ReturnType = Nothing Or TypeOf (ReturnType) Is String Then
                Return vIP.ToString
            ElseIf TypeOf (ReturnType) Is IPAddress Then
                Return vIP
            Else
                Return vIP.ToString
            End If
        End Get
        Set(ByVal value As Object)
            vIP = IPAddress.Parse(value)
        End Set
    End Property
End Class

