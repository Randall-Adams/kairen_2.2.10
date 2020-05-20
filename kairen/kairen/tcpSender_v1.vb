Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class tcpSender_v1
    Dim Client As New TcpClient
    Dim Newline = vbNewLine
    Dim vIP As String
    Dim vPort As Integer
    Sub New(ByVal _IP As String, ByVal _Port As Integer)
        vIP = _IP
        vPort = _Port
    End Sub
    Sub New()

    End Sub
    Public Function SendData(ByRef _data As String)
        Try
            Client = New TcpClient(vIP, vPort) ' Declare the Client as an IP:Port Address. 
            Dim Writer As New StreamWriter(Client.GetStream())
            Writer.Write(_data)
            Writer.Flush()
        Catch ex As Exception
            'Console.WriteLine(ex)
            Dim ErrorResult As String = ex.Message
            If ErrorResult = "No such host is known" Then
                Return -1
            Else
                MessageBox.Show(ErrorResult & Newline & Newline & _
                                "Please Review Client Address", _
                                "Error Sending Message", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return -2
            End If
        End Try
        Return 0
    End Function

    Public Function SendData(ByRef _data As String, ByVal _IP As String, ByVal _Port As Integer)
        Try
            Client = New TcpClient(_IP, _Port) ' Declare the Client as an IP:Port Address. 
            Dim Writer As New StreamWriter(Client.GetStream())
            Writer.Write(_data)
            Writer.Flush()
        Catch ex As Exception
            Console.WriteLine(ex)
            Dim Errorresult As String = ex.Message
            MessageBox.Show(Errorresult & Newline & Newline & _
                            "Please Review Client Address", _
                            "Error Sending Message", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return -1
        End Try
        Return 0
    End Function
End Class
