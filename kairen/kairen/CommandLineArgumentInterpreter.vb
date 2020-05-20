Public Class CommandLineArgumentInterpreter
    Public CommandLineArguments As String() = Environment.GetCommandLineArgs()
    Public ChooseInstallation_Flag As Integer = -1
    Public AutoLaunchCountDown_Flag As Integer = -1
    Public UserMenu_Flag As Boolean = True
    Public WorldPopulation_Flag As Boolean = False
    Public DeveloperMenu_Flag As Boolean = False
    Public CommandLineArgument_Command(-1) As String
    Public CommandLineArgument_Argument() As String
    Public Sub ParseCommandLineArguments()
        Dim i2 As Integer
        For i = 1 To CommandLineArguments.GetUpperBound(0)
            If CommandLineArguments(i) = "/ChooseInstallation" Then
                i2 = CommandLineArgument_Command.Length
                ChooseInstallation_Flag = i2
                ParseCmdArgs(i, i2)
            ElseIf CommandLineArguments(i) = "/AutoLaunchCountDown" Then
                i2 = CommandLineArgument_Command.Length
                AutoLaunchCountDown_Flag = i2
                ParseCmdArgs(i, i2)
            ElseIf CommandLineArguments(i) = "/WorldPopulation" Then
                i2 = CommandLineArgument_Command.Length
                WorldPopulation_Flag = True
                ParseCmdArgs(i, i2)
            ElseIf CommandLineArguments(i) = "/NoUserMenu" Then
                i2 = CommandLineArgument_Command.Length
                UserMenu_Flag = False
                ParseCmdArgs(i, i2)
            ElseIf CommandLineArguments(i) = "/DeveloperMenu" Then
                i2 = CommandLineArgument_Command.Length
                DeveloperMenu_Flag = True
                ParseCmdArgs(i, i2)
            End If
        Next
        If UserMenu_Flag = False And WorldPopulation_Flag = False And DeveloperMenu_Flag = False Then
            UserMenu_Flag = True
            MsgBox("Critical Error Detected: The Runtime Options that were supplied to Kairen would have resulted in no User Interface. This is a User Error and should be corrected by you.", MsgBoxStyle.Critical, "Warning - Error Avoided")
        End If
    End Sub
    Private Sub ParseCmdArgs(ByRef CurrentIndex As Integer, ByRef CommandIndex As Integer)
        ReDim Preserve CommandLineArgument_Command(CommandIndex)
        ReDim Preserve CommandLineArgument_Argument(CommandIndex)
        CommandLineArgument_Command(CommandIndex) = CommandLineArguments(CurrentIndex)
        CurrentIndex = CurrentIndex + 1
        While CurrentIndex <= CommandLineArguments.GetUpperBound(0)
            If Microsoft.VisualBasic.Left(CommandLineArguments(CurrentIndex), 1) <> "/" Then
                If CommandLineArgument_Argument(CommandIndex) <> "" Then CommandLineArgument_Argument(CommandIndex) = CommandLineArgument_Argument(CommandIndex) & " "
                CommandLineArgument_Argument(CommandIndex) = CommandLineArgument_Argument(CommandIndex) + CommandLineArguments(CurrentIndex)
            Else
                CurrentIndex = CurrentIndex - 1
                Exit While
            End If
            CurrentIndex = CurrentIndex + 1
        End While
    End Sub
End Class
