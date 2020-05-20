Public Class MultithreadExample
    Delegate Sub CounterDelegate(ByVal CurrentCountInteger As Integer)
    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        TextBox1.AppendText("0" & vbNewLine)
        Dim workerThread As New Threading.Thread(New Threading.ThreadStart(AddressOf CounterSub))
        workerThread.Start()
    End Sub

    Sub CounterSub()
        Dim DelegateInstance As CounterDelegate = AddressOf AddTextSub
        For i As Integer = 1 To 100
            Threading.Thread.Sleep(1000)
            DelegateInstance.Invoke(i)
        Next
    End Sub
    Sub AddTextSub(ByVal i As Integer)
        If TextBox1.InvokeRequired Then
            Dim DelegateInstance As CounterDelegate = AddressOf AddTextSub
            Me.Invoke(DelegateInstance, New Object() {i})
        Else
            TextBox1.AppendText(i & vbNewLine)
        End If
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        TextBox1.AppendText("Poop!" & vbNewLine)
    End Sub

    Private Sub MultithreadExample_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class