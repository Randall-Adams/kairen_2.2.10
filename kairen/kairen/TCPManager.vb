Imports System.IO
Imports System.Net
Imports System.Net.Sockets

Public Class TCPManager
    Public Client As TcpClient
    Public OutDataStream As StreamWriter
    Public ID As Integer
    Public vIP As IPAddress
    Public Port As Integer

    Public Sub New(_IP As String, _Port As Integer, _ID As Integer)
        ' Client Data
        IP = _IP
        Port = _Port
        ID = _ID
        Client = New TcpClient(IP, Port)
        OutDataStream = New StreamWriter(Client.GetStream)
    End Sub

    Public Property IP As String
        Get
            Return vIP.ToString
        End Get
        Set(ByVal value As String)
            vIP = IPAddress.Parse(value)
        End Set
    End Property

    Public Sub Send(Data As String)
        OutDataStream.Write(Data & vbCrLf)
        OutDataStream.Flush()
    End Sub

    Public Sub EndConnection()
        OutDataStream.Flush()
        OutDataStream.Close()
        Client.Close()
    End Sub
End Class
