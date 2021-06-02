Imports System
Imports System.IO.Ports

Public Class MainForm
    '*******************************************************************************
    '* Stop polling when the form is not visible in order to reduce communications
    '* Copy this section of code to every new form created
    '*******************************************************************************
    Private NotFirstShow As Boolean
    Dim vpb_sy, vpb_ly As Integer
    Dim TempL, OxiL, pHL As Integer
    Dim Temp, Oxi, pH, TempResult, OxiResult, pHResult, M1Result As String
    Dim I1, I2, I3, V1, V2, V3, I1R, I2R, I3R, V1R, V2R, V3R, M1 As String
    Dim I1L, I2L, I3L, V1L, V2L, V3L, M1L As Integer
    Dim TempToProgressBar As Single
    Dim ChartLimit As Integer = 30

    Private Sub PilotoOxigeno_Click(sender As Object, e As EventArgs) Handles PilotoOxigeno.Click

    End Sub

    Private Sub Motor1_Click(sender As Object, e As EventArgs) Handles Motor1.Click

    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click

    End Sub

    Private Sub DigitalPanelVoltaje1_Click(sender As Object, e As EventArgs) Handles DigitalPanelVoltaje1.Click

    End Sub

    Private Sub Label18_Click(sender As Object, e As EventArgs) Handles Label18.Click

    End Sub

    Private Sub PictureBox12_Click(sender As Object, e As EventArgs) Handles PictureBox12.Click

    End Sub

    Private Sub DigitalPanelVoltaje2_Click(sender As Object, e As EventArgs) Handles DigitalPanelVoltaje2.Click

    End Sub

    Private Sub Label33_Click(sender As Object, e As EventArgs) Handles Label33.Click

    End Sub

    Private Sub DigitalPanelCorriente3_Click(sender As Object, e As EventArgs) Handles DigitalPanelCorriente3.Click

    End Sub

    Private Sub Label19_Click(sender As Object, e As EventArgs) Handles Label19.Click

    End Sub

    Dim StrSerialIn, StrSerialInRam As String
    Dim SetpTemperatura, SetpOxigeno, SetpPh, HTempe, HOxi, HPh, potencia As Integer
    Dim ValorMinOxigeno, ValorMaxOxigeno, ValorMinPh, ValorMaxPh, ValorMinTemp, ValorMaxTemp As Integer
    Private Sub Form_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        '* Do not start comms on first show in case it was set to disable in design mode
        If NotFirstShow Then
            AdvancedHMIDrivers.Utilities.StopComsOnHidden(components, Me)
        Else
            NotFirstShow = True
        End If
    End Sub

    '***************************************************************
    '* .NET does not close hidden forms, so do it here
    '* to make sure forms are disposed and drivers close
    '***************************************************************
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim index As Integer
        While index < My.Application.OpenForms.Count
            If My.Application.OpenForms(index) IsNot Me Then
                My.Application.OpenForms(index).Close()
            End If
            index += 1
        End While
    End Sub

    Private Sub ComboBoxPort1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxPort1.SelectedIndexChanged
        PanelConnection1.Focus()
    End Sub
    Private Sub ComboBoxPort1_DropDown(sender As Object, e As EventArgs) Handles ComboBoxPort1.DropDown
        PanelConnection1.Focus()
    End Sub

    Private Sub GuardarDatosSp_Click(sender As Object, e As EventArgs) Handles GuardarDatosSp.Click

        SetpOxigeno = Val(TextSpOxigeno.Text)
        SetpTemperatura = Val(TextSpTemp.Text)
        SetpPh = Val(TextSpPh.Text)
        HTempe = Val(TextHisteresisTemp.Text)
        HOxi = Val(TextHisteresisOxi.Text)
        HPh = Val(TextHisteresisPh.Text)
        My.Settings.SetpOxigeno = SetpOxigeno
        My.Settings.SetpTemperatura = SetpTemperatura
        My.Settings.SetpPh = SetpPh
        My.Settings.HTempe = HTempe
        My.Settings.HPh = HPh
        My.Settings.HOxi = HOxi

        ValorMinOxigeno = SetpOxigeno - HOxi
        ValorMaxOxigeno = SetpOxigeno + HOxi
        ValorMinTemp = SetpTemperatura - HTempe
        ValorMaxTemp = SetpTemperatura + HTempe
        ValorMinPh = SetpPh - HPh
        ValorMaxPh = SetpPh + HPh

        Try
            Dim data1 As String
            data1 = "S" & "," & Format(SetpOxigeno) & "," & Format(HOxi) & "," & Format(SetpTemperatura) & "," & Format(HTempe) & "," & Format(SetpPh) & "," & Format(HPh) & ">"
            SP_Write(data1)  'Writing / Sending / Printing data to serial port
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error Message")
        End Try
    End Sub

    Private Sub ComboBoxPort1_Click(sender As Object, e As EventArgs) Handles ComboBoxPort1.Click
        If LabelStatus.Text = "Estado : Conectado" Then
            MsgBox("Conexión en proceso, por favor desconecte o cambie el puerto COM.", MsgBoxStyle.Critical, "Warning !!!")
            Return
        End If
    End Sub

    Private Sub ComboBoxBaudRate1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxBaudRate1.SelectedIndexChanged
        PanelConnection1.Focus()
    End Sub

    Private Sub ComboBoxBaudRate1_Click(sender As Object, e As EventArgs) Handles ComboBoxBaudRate1.Click
        If LabelStatus.Text = "Estado : Conectado" Then
            MsgBox("Conexión en proceso, por favor desconecte o cambie el BaudRate.", MsgBoxStyle.Critical, "Warning !!!")
            Return
        End If
    End Sub

    Private Sub ButtonScanPort1_Click(sender As Object, e As EventArgs) Handles ButtonScanPort1.Click
        PanelConnection1.Focus()
        If LabelStatus.Text = "Estado : Conectado" Then
            MsgBox("Conexión en progreso, por favor desconecte para escanear el nuevo puerto COM.", MsgBoxStyle.Critical, "Warning !!!")
            Return
        End If
        ComboBoxPort1.Items.Clear()
        Dim myPort As Array
        Dim i As Integer
        myPort = IO.Ports.SerialPort.GetPortNames()
        ComboBoxPort1.Items.AddRange(myPort)
        i = ComboBoxPort1.Items.Count
        i -= i
        Try
            ComboBoxPort1.SelectedIndex = i
            ButtonConnect1.Enabled = True
        Catch ex As Exception
            MsgBox("Puerto COM no detectado", MsgBoxStyle.Critical, "Warning !!!")
            ComboBoxPort1.Text = ""
            ComboBoxPort1.Items.Clear()
            Return
        End Try
        ComboBoxPort1.DroppedDown = True
    End Sub

    Private Sub Inicio1_Click(sender As Object, e As EventArgs) Handles Inicio1.Click
        TabControlPrincipal.SelectedTab = TabControlPrincipal.TabPages.Item(0)
    End Sub

    Private Sub Sensor2_Click(sender As Object, e As EventArgs) Handles Sensor2.Click
        TabControlPrincipal.SelectedTab = TabControlPrincipal.TabPages.Item(1)
    End Sub

    Private Sub Datalogger1_Click(sender As Object, e As EventArgs) Handles Datalogger1.Click
        TabControlPrincipal.SelectedTab = TabControlPrincipal.TabPages.Item(2)
    End Sub

    Private Sub Fotovoltaico1_Click(sender As Object, e As EventArgs) Handles Fotovoltaico1.Click
        TabControlPrincipal.SelectedTab = TabControlPrincipal.TabPages.Item(3)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Alarmas.Click
        TabControlPrincipal.SelectedTab = TabControlPrincipal.TabPages.Item(4)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TabControlPrincipal.SelectedTab = TabControlPrincipal.TabPages.Item(5)
    End Sub

    Private Sub ButtonConnect1_Click(sender As Object, e As EventArgs) Handles ButtonConnect1.Click
        PanelConnection1.Focus()
        Try
            With SerialPort1
                .BaudRate = 9600
                .DataBits = 8
                .Parity = Parity.None
                .StopBits = 1
                .PortName = ComboBoxPort1.Text
                .Open()
                If .IsOpen Then
                    LabelStatus.Text = "Estado : Conectado"
                    ButtonDisconnect1.BringToFront()
                    ButtonConnect1.SendToBack()
                    PictureBoxStatusConnection.BackColor = Color.Green
                    TimerSerial.Start()
                Else
                    MsgBox("CONEXION FALLIDA", MsgBoxStyle.Critical)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub ButtonDisconnect1_Click(sender As Object, e As EventArgs) Handles ButtonDisconnect1.Click
        SerialPort1.Close()
        LabelStatus.Text = "Estado = Desconectado"
        ButtonDisconnect1.SendToBack()
        ButtonConnect1.BringToFront()
        PictureBoxStatusConnection.BackColor = Color.Red

    End Sub



    Private Sub TimerSerial_Tick(sender As Object, e As EventArgs) Handles TimerSerial.Tick
        Try
            Dim StrSerialIn As String = SerialPort1.ReadExisting
            Dim StrSerialInRam As String

            Dim TB As New TextBox
            TB.Multiline = True
            TB.Text = StrSerialIn '--> Enter serial data into the textbox

            StrSerialIn = SerialPort1.ReadExisting  '--> Read incoming serial data

            StrSerialInRam = TB.Lines(0).Substring(0, 1)
            If StrSerialInRam = "O" Then
                Oxi = TB.Lines(0)
                OxiL = Oxi.Length
            Else
                Oxi = Oxi
            End If

            StrSerialInRam = TB.Lines(1).Substring(0, 1)
            If StrSerialInRam = "T" Then
                Temp = TB.Lines(1)
                TempL = Temp.Length
            Else
                Temp = Temp
            End If

            StrSerialInRam = TB.Lines(2).Substring(0, 1)
            If StrSerialInRam = "P" Then
                pH = TB.Lines(2)
                pHL = pH.Length
            Else
                pH = pH
            End If

            StrSerialInRam = TB.Lines(3).Substring(0, 2)
            If StrSerialInRam = "I1" Then
                I1 = TB.Lines(3)
                I1L = I1.Length
            Else
                I1 = I1
            End If

            StrSerialInRam = TB.Lines(4).Substring(0, 2)
            If StrSerialInRam = "I2" Then
                I2 = TB.Lines(4)
                I2L = I2.Length
            Else
                I2 = I2
            End If

            StrSerialInRam = TB.Lines(5).Substring(0, 2)
            If StrSerialInRam = "I3" Then
                I3 = TB.Lines(5)
                I3L = I3.Length
            Else
                I3 = I3
            End If

            StrSerialInRam = TB.Lines(6).Substring(0, 2)
            If StrSerialInRam = "v1" Then
                V1 = TB.Lines(6)
                V1L = V1.Length
            Else
                V1 = V1
            End If

            StrSerialInRam = TB.Lines(7).Substring(0, 2)
            If StrSerialInRam = "v2" Then
                V2 = TB.Lines(7)
                V2L = V2.Length
            Else
                V2 = V2
            End If

            StrSerialInRam = TB.Lines(8).Substring(0, 2)
            If StrSerialInRam = "v3" Then
                V3 = TB.Lines(8)
                V3L = V3.Length
            Else
                V3 = V3
            End If

            StrSerialInRam = TB.Lines(9).Substring(0, 2)
            If StrSerialInRam = "m1" Then
                M1 = TB.Lines(9)
                M1L = M1.Length
            Else
                M1 = M1
            End If

            OxiResult = Mid(Oxi, 2, OxiL)
            TempResult = Mid(Temp, 2, TempL)
            pHResult = Mid(pH, 2, pHL)
            I1R = Mid(I1, 3, I1L)
            I2R = Mid(I2, 3, I2L)
            I3R = Mid(I3, 3, I3L)
            V1R = Mid(V1, 3, V1L)
            V2R = Mid(V2, 3, V2L)
            V3R = Mid(V3, 3, V3L)
            M1Result = Mid(M1, 3, M1L)


            CircularProgressBarOxigeno1.Value = OxiResult
            CircularProgressBarOxigeno1.Text = CircularProgressBarOxigeno1.Value & " ppm"

            CircularProgressBarTemp1.Value = TempResult
            CircularProgressBarTemp1.Text = CircularProgressBarTemp1.Value & " °C"

            CircularProgressBarPh1.Value = pHResult
            CircularProgressBarPh1.Text = CircularProgressBarPh1.Value

            DigitalPanelCorriente1.Value = I1R
            DigitalPanelCorriente2.Value = I2R
            DigitalPanelCorriente3.Value = I3R

            DigitalPanelVoltaje1.Value = V1R
            DigitalPanelVoltaje2.Value = V2R
            DigitalPanelVoltaje3.Value = V3R

            DigitalPanelPotencia.Value = (I1R * V1R) / 1000

            '-----------Enter the temperature and humidity values into the chart-----------------------------------
            Chart1.Series("Temperatura").Points.AddY(TempResult)
            If Chart1.Series(0).Points.Count = ChartLimit Then
                Chart1.Series(0).Points.RemoveAt(0)
            End If

            Chart2.Series("Oxigeno       ").Points.AddY(OxiResult)
            If Chart2.Series(0).Points.Count = ChartLimit Then
                Chart2.Series(0).Points.RemoveAt(0)
            End If

            Chart3.Series("Ph                ").Points.AddY(pHResult)
            If Chart3.Series(0).Points.Count = ChartLimit Then
                Chart3.Series(0).Points.RemoveAt(0)
            End If


            ValorMinOxigeno = SetpOxigeno - HOxi
            ValorMaxOxigeno = SetpOxigeno + HOxi
            ValorMinTemp = SetpTemperatura - HTempe
            ValorMaxTemp = SetpTemperatura + HTempe
            ValorMinPh = SetpPh - HPh
            ValorMaxPh = SetpPh + HPh

            If OxiResult >= 0 And OxiResult <= ValorMinOxigeno Then
                PilotoOxigeno.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Yellow
                PilotoOxigeno.Value = True
                AlarmaOxigeno.Text = "Oxigeno = Bajo"
            ElseIf OxiResult >= ValorMinOxigeno And OxiResult < ValorMaxOxigeno Then
                PilotoOxigeno.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Green
                PilotoOxigeno.Value = True
                AlarmaOxigeno.Text = "Oxigeno = Normal"
            ElseIf OxiResult >= ValorMaxOxigeno Then
                PilotoOxigeno.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Red
                PilotoOxigeno.Value = True
                AlarmaOxigeno.Text = "Oxigeno = Elevado"
            End If

            If TempResult >= 0 And TempResult <= ValorMinTemp Then
                PilotoTemperatura.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Green
                PilotoTemperatura.Value = True
                AlarmaTemp.Text = "Temperatura = Baja"
            ElseIf TempResult >= ValorMinTemp And TempResult < ValorMaxTemp Then
                PilotoTemperatura.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Yellow
                PilotoTemperatura.Value = True
                AlarmaTemp.Text = "Temperatura = Normal"
            ElseIf TempResult >= ValorMaxTemp Then
                PilotoTemperatura.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Red
                PilotoTemperatura.Value = True
                AlarmaTemp.Text = "Temperatura = Elevada"
            End If

            If pHResult >= 0 And pHResult < 6 Then
                PilotoPh.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Red
                PilotoPh.Value = True
                AlarmaPh.Text = "Ph = Acido"
            ElseIf pHResult = 7 Then
                PilotoPh.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Green
                AlarmaPh.Text = "Ph = Neutro"
                PilotoPh.Value = True
            ElseIf pHResult >= 8 And pHResult <= 14 Then
                PilotoPh.LightColor = AdvancedHMI.Controls.PilotLight.LightColors.Blue
                AlarmaPh.Text = "Ph = Alcalino"
                PilotoPh.Value = True
            End If

            LabelSpOxigeno.Text = SetpOxigeno
            SpMinO.Text = ValorMinOxigeno
            SpMaxO.Text = ValorMaxOxigeno
            LabelSpTemperatura.Text = SetpTemperatura
            SpMinT.Text = ValorMinTemp
            SpMaxT.Text = ValorMaxTemp
            LabelSpPh.Text = SetpPh
            SpMinP.Text = ValorMinPh
            SpMaxP.Text = ValorMaxPh
            '------------------------------------------------------------------------------------------------------

            '-----------If the Then connection Is successful And running, PictureBoxStatusConnection will blink----
            If PictureBoxStatusConnection.Visible = True Then
                PictureBoxStatusConnection.Visible = False
            ElseIf PictureBoxStatusConnection.Visible = False Then
                PictureBoxStatusConnection.Visible = True
            End If
            '------------------------------------------------------------------------------------------------------
            If M1Result = 1 Then
                EstadoMotor.Text = "Estado : Operativo"
                Motor1.Value = True
            ElseIf M1Result = 0 Then
                EstadoMotor.Text = "Estado : Inactivo "
                Motor1.Value = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CenterToScreen()
        PanelConnection1.Focus()
        CircularProgressBarOxigeno1.Value = 0
        CircularProgressBarPh1.Value = 0
        CircularProgressBarTemp1.Value = 0
        DigitalPanelCorriente1.Value = 0
        DigitalPanelCorriente2.Value = 0
        DigitalPanelCorriente3.Value = 0
        DigitalPanelVoltaje1.Value = 0
        DigitalPanelVoltaje2.Value = 0
        DigitalPanelVoltaje3.Value = 0
        ComboBoxBaudRate1.SelectedIndex = 0

        For i = 0 To 30 Step 1
            Chart1.Series("Temperatura").Points.AddY(0)
            If Chart1.Series(0).Points.Count = ChartLimit Then
                Chart1.Series(0).Points.RemoveAt(0)
            End If

            Chart2.Series("Oxigeno       ").Points.AddY(0)
            If Chart2.Series(0).Points.Count = ChartLimit Then
                Chart2.Series(0).Points.RemoveAt(0)
            End If

            Chart3.Series("Ph                ").Points.AddY(0)
            If Chart3.Series(0).Points.Count = ChartLimit Then
                Chart3.Series(0).Points.RemoveAt(0)
            End If
        Next

        Chart1.ChartAreas(0).AxisY.Maximum = 100
        Chart1.ChartAreas(0).AxisY.Minimum = 0
        Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Enabled = False
        Chart2.ChartAreas(0).AxisY.Maximum = 30
        Chart2.ChartAreas(0).AxisY.Minimum = 0
        Chart2.ChartAreas("ChartArea2").AxisX.LabelStyle.Enabled = False
        Chart3.ChartAreas(0).AxisY.Maximum = 20
        Chart3.ChartAreas(0).AxisY.Minimum = 0
        Chart3.ChartAreas("ChartArea3").AxisX.LabelStyle.Enabled = False

        Try
            SetpOxigeno = My.Settings.SetpOxigeno
            SetpTemperatura = My.Settings.SetpTemperatura
            SetpPh = My.Settings.SetpPh
            HTempe = My.Settings.HTempe
            HPh = My.Settings.HPh
            HOxi = My.Settings.HOxi

        Catch ex As Exception

        End Try


    End Sub

    Public Function SP_Write(ByVal Str As String)
        SerialPort1.WriteLine(Str)
        Return Str
    End Function

    Private Sub GuardarDatosSp_MouseClick(sender As Object, e As MouseEventArgs) Handles GuardarDatosSp.MouseClick

    End Sub

    Private Sub Chart1_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        Me.Chart1.Series("Temperatura").ToolTip = "#VALY{F}"
    End Sub

    Private Sub Chart2_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        Me.Chart2.Series("Oxigeno       ").ToolTip = "#VALY{F}"
    End Sub

    Private Sub Chart3_MouseMove(sender As Object, e As MouseEventArgs) Handles Chart1.MouseMove
        Me.Chart3.Series("Ph                ").ToolTip = "#VALY{F}"
    End Sub
End Class
