Namespace My

    ' The following events are available for MyApplication:
    ' 
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim result As MsgBoxResult = _
                MessageBox.Show("Error: Unhandled Exception encountered." & vbNewLine & _
                                "Would you like Kairen to close?" & vbNewLine & vbNewLine & _
                                "Excepton Message: " & e.Exception.Message, "Would you like Kairen to flee?", _
                                MessageBoxButtons.YesNo, _
                                MessageBoxIcon.Error)
            If result = MsgBoxResult.Yes Then
                FormLoader.ActuallyClose = True
                e.ExitApplication = True
            End If
        End Sub
        'Private Sub MyApplication_NetworkAvailabilityChanged(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.Devices.NetworkAvailableEventArgs) Handles Me.NetworkAvailabilityChanged
        '    MsgBox(e.IsNetworkAvailable)
        'End Sub
    End Class
End Namespace

