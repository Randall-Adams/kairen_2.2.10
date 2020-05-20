Public Class FAPIConnectionManager
    Dim OnOffControl As Object
    Dim lb As New CommonLibrary("EQOA", "")
    Private vMode As String
    Private WithEvents mytimer As New Windows.Forms.Timer
    Dim GrabData_FAPI As TextFileClass
    Private commandsToDo() As String
    Private commandsToDo_Data() As String

    'Form Update Flags
    Public updateKairen_Main As Boolean = True
    Public updateKaires As Boolean = False
    Public Sub New(ByRef HandlingObject As Object, Optional ByVal timerInterval As Integer = 2000)
        GrabData_FAPI = New TextFileClass(lb.Folder_Net_Streams & "o\FAPI Data" & lb.Extension_ReadWrites, "--", True)
        OnOffControl = HandlingObject
        mytimer = New Windows.Forms.Timer
        mytimer.Interval = timerInterval
    End Sub
    Public ReadOnly Property Mode
        Get
            Return vMode
        End Get
    End Property

    Public Sub Connect()
        lb.ParseOutsideCommand("ConnectToKairen")
        vMode = "SendingConnectionAttempt"
        mytimer.Start()
    End Sub

    Private Sub mytimer_Tick(sender As System.Object, e As System.EventArgs) Handles mytimer.Tick
        If lb.FE(lb.Folder_EQOA & "Temp\Outside Command" & ".txt") = False Then
            OutputCommand()
        End If
        If GrabData_FAPI.FileExists = False Then Exit Sub
        GrabData_FAPI.LoadFile()
        GrabData_FAPI.DeleteFile()
        Select Case vMode
            Case "Disconnected"
            Case "SendingConnectionAttempt"
                Select Case GrabData_FAPI.Line(0)
                    Case "ConnectionAccepted"
                        vMode = "Connected"
                        AddCommand("PrintToConsole", "Kairen has connected to FAPI. Turning on OutputPlayerData now.")
                        AddCommand("OutputPlayerData", True)
                        If Kairen_Main.d_npcmkr_btn_Load_NPC.Checked = True Or Kairen_Main.d_npcmkr_btn_New_NPC.Checked = True Then
                            Kairen_Main.d_npcmkr_btn_Grab_Data.Enabled = True
                        End If
                End Select
            Case "Connected"
                Select Case GrabData_FAPI.Line(0)
                    Case "UpdateKairen"
                        Select Case GrabData_FAPI.Line(1)
                            Case "OutputPlayerData"
                                Kairen_Main.UpdateYourData()
                                If updateKaires = True Then
                                    Kaires.UpdateYourData()
                                End If

                                'OnOffControl.enabled = True
                                'OnOffControl.PerformClick()
                                'OnOffControl.checked = GrabData_FAPI.Line(2)
                            Case "UpdateCommandFile"
                                'OutputCommand()
                        End Select
                End Select
            Case "ReceivingConnectionAttempt"
        End Select
    End Sub

    Private Sub AddCommand2(ByVal _commandToAdd As String)
        Dim i As Integer = 0
        If commandsToDo IsNot Nothing Then
            i = commandsToDo.Length
        Else
            i = 0
        End If
        ReDim Preserve commandsToDo(i)
        commandsToDo(i) = _commandToAdd
        ReDim Preserve commandsToDo_Data(i)
        commandsToDo_Data(i) = ""
    End Sub
    Public Sub AddCommand(ByVal _commandToAdd As String, Optional ByVal _commandData As String = "")
        Dim i As Integer = 0
        If commandsToDo IsNot Nothing Then
            i = commandsToDo.Length
        Else
            i = 0
        End If
        ReDim Preserve commandsToDo(i)
        ReDim Preserve commandsToDo_Data(i)
        commandsToDo(i) = _commandToAdd
        commandsToDo_Data(i) = _commandData
    End Sub

    Private Sub OutputCommand()
        If commandsToDo Is Nothing Then Exit Sub
        If commandsToDo.Length = 0 Then Exit Sub
        If commandsToDo_Data(0) <> "" Then
            lb.ParseOutsideCommand(commandsToDo(0), commandsToDo_Data(0))
            Dim tempCommandsToDo() As String
            Dim tempCommandsToDo_Data() As String
            Dim i As Integer = 1
            ReDim Preserve tempCommandsToDo(commandsToDo.Length - 2)
            ReDim Preserve tempCommandsToDo_Data(commandsToDo.Length - 2)
            While i < commandsToDo.Length
                If i <> 0 And i < commandsToDo.Length Then
                    tempCommandsToDo(i - 1) = commandsToDo(i)
                    tempCommandsToDo_Data(i - 1) = commandsToDo_Data(i)
                End If
                i = i + 1
            End While
            If i > 0 Then
                commandsToDo = tempCommandsToDo
                commandsToDo_Data = tempCommandsToDo_Data
            End If
        Else
            lb.ParseOutsideCommand(commandsToDo(0))
            Dim tempCommandsToDo() As String
            Dim tempCommandsToDo_Data() As String
            Dim i As Integer = 0
            ReDim Preserve tempCommandsToDo(commandsToDo.Length - 2)
            ReDim Preserve tempCommandsToDo_Data(commandsToDo.Length - 2)
            While i < commandsToDo.Length
                If i <> 0 And i < commandsToDo.Length Then
                    tempCommandsToDo(i - 1) = commandsToDo(i)
                    tempCommandsToDo_Data(i - 1) = commandsToDo_Data(i)
                End If
                i = i + 1
            End While
            If i > 0 Then
                commandsToDo = tempCommandsToDo
                commandsToDo_Data = tempCommandsToDo_Data
            End If
        End If
    End Sub
End Class
