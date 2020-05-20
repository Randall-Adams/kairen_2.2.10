Public Class FAPIConnectionRequestClass
    Private vType As Integer
    Private vSent As Boolean = False
    Private vRequestData As String
    Private vRequestData_Array() As String
    Sub New(ByVal _type As Integer, ByVal _requestData As String, ByVal _requestData_Array() As String)
        vType = _type
        vRequestData = _requestData
        vRequestData_Array = _requestData_Array
    End Sub

    Public Property Sent() As Boolean
        Get
            Return vSent
        End Get
        Set(value As Boolean)
            If value = True Then
                'vRequestData = Nothing
                vRequestData_Array = Nothing
                vSent = True
            End If
        End Set
    End Property

    Public ReadOnly Property Type() As String
        Get
            Return vType
        End Get
    End Property
    Public ReadOnly Property RequestData() As String
        Get
            If vRequestData = Nothing Then
                Return Nothing
            Else
                Return vRequestData
            End If
        End Get
    End Property
    Public ReadOnly Property RequestData_Array() As String()
        Get
            If vRequestData_Array Is Nothing Then
                Return Nothing
            Else
                Return vRequestData_Array
            End If
        End Get
    End Property
    Public ReadOnly Property RequestData_Array(ByVal i As Integer) As String
        Get
            If vRequestData_Array Is Nothing Then
                Return Nothing
            Else
                Return vRequestData_Array(i)
            End If
        End Get
    End Property
End Class
