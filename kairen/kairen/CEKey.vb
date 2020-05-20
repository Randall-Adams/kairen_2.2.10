Public Class CEKey
    Private vType As String

    Public Property Type(ByVal input As String) As String
        Get
            Return vType
        End Get
        Set(value As String)
            vType = value
        End Set
    End Property
End Class
