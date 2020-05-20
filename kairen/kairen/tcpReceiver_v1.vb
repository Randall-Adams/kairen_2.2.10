Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Public Class tcpReceiver_v1
    Private Listener As TcpListener
    Private Client As New TcpClient
    Private Data As String = ""
    Private vIP As IPAddress
    Private vPort As Integer
    Private vClientIsListening As Boolean = False
    Private Newline = vbNewLine
    Private WithEvents TimerListener As New Timer
    Private FloodMessageGiven As Boolean = False
    Delegate Sub OutputDelegate(ByVal _data() As String)
    Public vOutputDelegate As OutputDelegate
#Region "Properties"
    'Public ReadOnlys
    Public ReadOnly Property IP As IPAddress
        Get
            Return vIP
        End Get
    End Property
    Public ReadOnly Property Port As Integer
        Get
            Return vPort
        End Get
    End Property
    Public ReadOnly Property ClientIsListening As Boolean
        Get
            Return vClientIsListening
        End Get
    End Property
#End Region
    Sub New(ByVal _ip As IPAddress, ByVal _port As Integer, ByVal _interval As Integer, ByRef _outputDelegate As OutputDelegate)
        vIP = _ip
        vPort = _port
        TimerListener.Interval = _interval
        vOutputDelegate = _outputDelegate
    End Sub
    Public Function StartListening()
        Listener = New TcpListener(vIP, vPort)
        Try
            Listener.Start()
        Catch ex As Exception
            Listener.Stop()
            If ex.Message = "The requested address is not valid in its context" Then
                Return -1
            ElseIf ex.Message = "Only one usage of each socket address (protocol/network address/port) is normally permitted" Then
                Return -3
            Else
                Return -2
            End If
        End Try
        TimerListener.Start()
        vClientIsListening = True
        Return 0
    End Function
    Public Sub StopListening()
        Listener.Stop()
        TimerListener.Stop()
        vClientIsListening = False
    End Sub
    Private Sub timer_Client_Modified() Handles TimerListener.Tick
        If Listener.Pending = True Then
            Data = ""
            Client = Listener.AcceptTcpClient()
            Dim FullData(2) As String
            FullData(0) = Microsoft.VisualBasic.Left(Listener.LocalEndpoint.ToString, Listener.LocalEndpoint.ToString.IndexOf(":"))
            FullData(1) = Microsoft.VisualBasic.Right(Listener.LocalEndpoint.ToString, Listener.LocalEndpoint.ToString.Length - Listener.LocalEndpoint.ToString.IndexOf(":") - 1)
            'Dim IPListenedOn As String = Microsoft.VisualBasic.Left(Listener.LocalEndpoint.ToString, Listener.LocalEndpoint.ToString.IndexOf(":"))
            'Dim PortListenedOn As String = Microsoft.VisualBasic.Right(Listener.LocalEndpoint.ToString, Listener.LocalEndpoint.ToString.Length - Listener.LocalEndpoint.ToString.IndexOf(":") - 1)
            If Client.Available > 0 Then
                Dim Reader As New StreamReader(Client.GetStream())
                Try
                    While Reader.Peek > -1
                        Data = Data + Convert.ToChar(Reader.Read()).ToString
                    End While
                Catch ex As Exception
                    If ex.Message = "Unable to read data from the transport connection: An existing connection was forcibly closed by the remote host." Then
                        If FloodMessageGiven = False Then
                            FloodMessageGiven = True
                            StopListening()
                            MsgBox("I think you were flooded out..", MsgBoxStyle.OkOnly, "Flood Warning")
                            Exit Sub
                        End If
                    End If
                End Try
            End If
            FullData(2) = Data
            RaiseEvent DataReceived(Me, New DataReceivedEventArgs(FullData))
            'TextBox6.Text += "<< " & Data + vbCrLf
            'Here you can enter anything you would like 
            'to happen when a message is received,
            'For instance; Play a sound, Show a message Box, A Balloon Tip etc.
        End If
    End Sub
    Private Sub sDataReceived(sender As Object, e As DataReceivedEventArgs) Handles Me.DataReceived
        vOutputDelegate(e.Data)
    End Sub

    Public Event DataReceived As EventHandler(Of DataReceivedEventArgs) ' The DataReceived event
    Public Class DataReceivedEventArgs
        Inherits EventArgs
        Public Data() As String
        Public Sub New(ByVal _receivedData() As String)
            ' TODO: Insert constructor code here
            Data = _receivedData
        End Sub
    End Class ' Event argument class for the DataReceived event
End Class
