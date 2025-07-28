Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml

Public Class frmRepeco

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim contador As Integer = 0
    Dim TIP As String
    Dim fechventa As String
    Dim fechventaFin As String
    Dim FechaFactura As String

    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda As Data.DataTable
    Dim Terminal, TpDev As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim TpRegimen As String
    Dim fp As String
    Dim FormPago As String
    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim Cadena As String
    Dim e_uuid As String
    Dim PorIVa As Double
    Dim NumPiezas As Integer
    Dim td As Integer
    Dim finDia
    Dim notaMax As Integer
    Dim notaMin As Integer
#End Region

#Region "Datos de Factura"
    Dim txtFactura As String
    Dim txtSerie As String
    Dim txtNumAprobacion As String
    Dim txtAmoAprob As String
    Dim txtLugarExpedicion As String
#End Region

#Region "Email"
    Dim DirEmail As String
    Dim PwdEmail As String
    Dim Pop As String
    Dim PuertoPop As String
    Dim SmtpHost As String
    Dim PuertoSmtp As String
    Dim DescEmail As String
    Dim AsuntoEmail As String
    Dim MsjEmail As String
    Dim SeguridadEmail As Boolean
#End Region

    Private Sub frmRepeco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FechaFactura = Date.Now.ToString.Substring(0, 10)

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where tTipo2 = 'R'", 0, 1, Motor)
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
        txtFechaIni.Value = CDate(fechventa)
        Application.DoEvents()
    End Sub

    Private Sub frmRepeco_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub grbTienda_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbTienda.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.grbSucursal.Focus()
        End If
    End Sub

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        Try
            objpro.CargaCombos(grbSucursal, "Select sId, sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
            Me.ChlisFact.Items.Clear()
            Me.ChlisNota.Items.Clear()
            Me.grbSucursal.Value = Nothing
            grbSucursal.Focus()
        Catch ex As Exception
            MsgBox("Sucursal: " & ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        consulta_registros()
    End Sub

    Public Function consulta_registros()
        If grbTienda.Text <> "" Then
            If Me.grbSucursal.Text <> "" Then
                fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
                txtFechaIni.Value = CDate(fechventa)
                finDia = (New DateTime(grbPeriodo.Text, objfun.EscojeMes(grbMes.Text), 1)).AddMonths(1).AddDays(-1).Day
                fechventaFin = Str(finDia) + "/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
                txtFechaFin.Value = CDate(fechventaFin)
                Ugr.DataSource = Nothing
                CargaListaNotas(1)
                CargaListaCredito(2)
            Else
                MsgBox("Falta seleccionar sucursal", MsgBoxStyle.Information, "Facturación.. ")
            End If
        Else
            MsgBox("Falta seleccionar Tienda o Razón social", MsgBoxStyle.Information, "Facturación.. ")
        End If
        Return ""
    End Function

    Private Sub CargaListaNotas(ByVal tp As Integer)
        Try
            Application.DoEvents()
            TablaSucursal = Nothing
            TablaSucursal = OBJPRO.ObtenDatos("select * from ventaenc where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and veidfolv = " & tp & " and Month(veFecha) = " & OBJFUN.EscojeMes(Me.grbMes.Text) & " and year(veFecha) = " & Me.grbPeriodo.Text & " order by vefecha ", Motor).Tables(0)
            Me.ChlisFact.Items.Clear()
            Dim i As Integer = 0
            For Each row As DataRow In TablaSucursal.Rows
                Application.DoEvents()
                Me.ChlisFact.Items.Add("Venta " & row.Item("vefecha").ToString.Substring(0, 10))
                If row.Item("veEstatu") = 1 Then
                    ChlisFact.SetItemChecked(i, CheckState.Checked)
                Else
                    ChlisFact.SetItemChecked(i, CheckState.Unchecked)
                End If
                i += 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub CargaListaCredito(ByVal tp As Integer)
        Try
            Application.DoEvents()
            TablaSucursal = Nothing
            TablaSucursal = OBJPRO.ObtenDatos("select * from ventaenc where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and veidfolv = " & tp & " and Month(veFecha) = " & OBJFUN.EscojeMes(Me.grbMes.Text) & " and year(veFecha) = " & Me.grbPeriodo.Text & " order by vefecha ", Motor).Tables(0)
            Me.ChlisNota.Items.Clear()
            Dim i As Integer = 0
            For Each row As DataRow In TablaSucursal.Rows
                Application.DoEvents()
                Me.ChlisNota.Items.Add("Dev. " & row.Item("vefecha").ToString.Substring(0, 10))
                If row.Item("veEstatu") = 1 Then
                    ChlisNota.SetItemChecked(i, CheckState.Checked)
                Else
                    ChlisNota.SetItemChecked(i, CheckState.Unchecked)
                End If
                i += 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub btnFactura_Click(sender As Object, e As EventArgs) Handles btnFactura.Click
        Application.DoEvents()
        Dim EstadoVenta, term As String        
        Transaccion = 1
        TpDev = 2
        InicializaCantidades()
        If Me.grbSucursal.Value > 9 Then
            term = Trim(grbTienda.Value.ToString.Substring(0, 1)) & Trim(Str(Me.grbSucursal.Value))
        Else
            term = Trim(Str(grbTienda.Value)) + Trim(Str(Me.grbSucursal.Value))
        End If
        Terminal = Val(Trim(term))
        If ChlisFact.Items.Count > 0 Then            
            Application.DoEvents()
            InicializaGrid()
            TIP = "VG"
            upb.Value = 0
            Application.DoEvents()
            TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
            txtFactura = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
            txtSerie = TSucursal.Rows(0).Item("Seserie").ToString
            txtLugarExpedicion = TSucursal.Rows(0).Item("seLugarExp").ToString

            Me.Subtotal.Value = 0
            Me.Descuento.Value = 0
            Me.Iva.Value = 0
            Me.Total.Value = 0
            Me.Devoluciones.Value = 0

            CargaGridInicio()

            Subtotal.Value = (Importe.Value - Descuento.Value)
            Total.Value = (Subtotal.Value + Iva.Value)
            Application.DoEvents()

            'Devolucion
            If objpro.DatoExiste("veTotal", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & TpDev & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaIni.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaIni.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaIni.Value.ToString.Substring(6, 4) & " ", Motor) Then
                Me.Devoluciones.Value = objpro.Dato("veTotal", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & TpDev & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaIni.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaIni.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaIni.Value.ToString.Substring(6, 4) & " ", Motor)
                Application.DoEvents()
            Else
                Me.Devoluciones.Value = 0
            End If
            'Devolucion
            Application.DoEvents()
        Else
            MsgBox("Aun no carga los dias de ventas ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub InicializaGrid()
        Ugr.DataSource = Nothing
        objpro.LLenaGrid(Ugr, "select '' as codsat, '' as vdclvUni, vdClaPro as ClaveInterno, vdDescri as Descripcion, " & _
                                  " vdCant as Cantidad, '' as Unidad, vdPreUni as PrecioUnitario, vdIva as Iva, " & _
                                   " vdDesc1 as Descu, vdPreTot as Importe from ventadet " & _
                                   " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " & _
                                   " vdterm = 0 ", "ventadet", Motor)
        Ugr.DataBind()
    End Sub

    Private Sub CargaGridInicio()
        Application.DoEvents()
        'DatGrid = objpro.ObtenDatos(" select vdcodsat as codsat, vdclvuni as vdclvUni, vdClaPro as ClaveInterno, vdDescri as Descripcion, " & _
        '                            " vdCant as Cantidad, vdPreUni as PrecioUnitario, vdIva as Iva, " & _
        '                            " vdDesc1 as Descu, vdPreTot as Importe from ventadet " & _
        '                            " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " & _
        '                            " vdterm = " & Terminal & " and " & _
        '                            " month(vdFecha)= " & txtFechaIni.Value.ToString.Substring(3, 2) & " and " & _
        '                            " year(vdFecha)= " & txtFechaIni.Value.ToString.Substring(6, 4) & " order by vdfecha, vdidfolv ", Motor).Tables(0)

        DatGrid = objpro.ObtenDatos(" select vdcodsat as codsat, vdclvuni as vdclvUni, vdClaPro as ClaveInterno, vdDescri as Descripcion, " & _
                                    " vdCant as Cantidad, vdPreUni as PrecioUnitario, vdIva as Iva, " & _
                                    " vdDesc1 as Descu, vdPreTot as Importe from ventadet " & _
                                    " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " & _
                                    " vdterm = " & Terminal & " and " & _
                                    " Day(vdFecha) >= " & txtFechaIni.Value.ToString.Substring(0, 2) & " and " & _
                                    " month(vdFecha) >= " & txtFechaIni.Value.ToString.Substring(3, 2) & " and " & _
                                    " year(vdFecha) >= " & txtFechaIni.Value.ToString.Substring(6, 4) & " and " & _
                                    " Day(vdFecha) <= " & txtFechaFin.Value.ToString.Substring(0, 2) & " and " & _
                                    " month(vdFecha) <= " & txtFechaFin.Value.ToString.Substring(3, 2) & " and " & _
                                    " year(vdFecha) <= " & txtFechaFin.Value.ToString.Substring(6, 4) & " " & _
                                    " order by vdfecha, vdidfolv ", Motor).Tables(0)

        If DatGrid.Rows.Count <> 0 Then
            Me.Importe.Value = 0
            Me.Descuento.Value = 0
            Me.Iva.Value = 0
            upgb1.Value = 0
            upgb1.Minimum = 0
            upgb1.Maximum = DatGrid.Rows.Count

            If grbTienda.Text = "JUAN FRANCISCO JIMENEZ GONZALEZ" Then
                For Each r In DatGrid.Rows
                    Application.DoEvents()
                    Dim pu1 As Double = CDbl(r("PrecioUnitario").ToString)
                    Dim PU2 As Double = Format(pu1, "###,###.00")
                    Dim descu As Double = Format((CDbl(r("Descu").ToString) * 1.16), "###,###.00")
                    Dim iva1 As Double = 0 'Cambio por Repeco no quieres que se agregue el iva
                    Dim pu As Double = Format(CDbl((PU2 * 1.16)), "###,###.00")
                    Dim imp1 As Double = CDbl(r("Importe").ToString)
                    Dim imp2 As Double = Format(imp1, "###,###.00")
                    Dim imp As Double = Format(CDbl((imp2)), "###,###.00")

                    Dim codsat As String
                    If r("codsat").ToString = "" Then
                        codsat = "01010101"
                    Else
                        codsat = r("codsat").ToString
                    End If
                    Dim Borrar As String = Trim(r("Descripcion").ToString)
                    While Borrar.Contains("  ")
                        Borrar = Borrar.Replace("  ", " ")
                        Borrar = Trim(Borrar)
                    End While

                    FormatoGrid(codsat, r("vdclvUni").ToString, r("ClaveInterno").ToString, Borrar, _
                                r("Cantidad").ToString, "PZA", pu, iva1, _
                                descu, imp)

                    Me.Importe.Value += (imp)
                    Me.Descuento.Value += (descu)
                    Me.Iva.Value += iva1
                    upgb1.Value += 1
                    Application.DoEvents()
                Next

            Else
                For Each r In DatGrid.Rows
                    Application.DoEvents()
                    Dim pu1 As Double = CDbl(r("PrecioUnitario").ToString)
                    Dim PU2 As Double = Format(pu1, "###,###.00")
                    Dim descu As Double = Format((CDbl(r("Descu").ToString) * 1.16), "###,###.00")
                    'Dim iva1 As Double = Format(CDbl(r("Iva").ToString), "###,###.00")
                    Dim iva1 As Double = 0 'Cambio por Repeco no quieres que se agregue el iva
                    Dim pu As Double = Format(CDbl((PU2 * 1.16)), "###,###.00")
                    Dim imp1 As Double = CDbl(r("Importe").ToString)
                    Dim imp2 As Double = Format(imp1, "###,###.00")
                    Dim imp As Double = Format(CDbl((imp2 * 1.16)), "###,###.00")

                    Dim codsat As String
                    If r("codsat").ToString = "" Then
                        codsat = "01010101"
                    Else
                        codsat = r("codsat").ToString
                    End If
                    Dim Borrar As String = Trim(r("Descripcion").ToString)
                    While Borrar.Contains("  ")
                        Borrar = Borrar.Replace("  ", " ")
                        Borrar = Trim(Borrar)
                    End While

                    FormatoGrid(codsat, r("vdclvUni").ToString, r("ClaveInterno").ToString, Borrar,
                                r("Cantidad").ToString, "PZA", pu, iva1,
                                descu, imp)

                    Me.Importe.Value += (imp)
                    Me.Descuento.Value += (descu)
                    Me.Iva.Value += iva1
                    upgb1.Value += 1
                    Application.DoEvents()
                Next

                'For Each r In DatGrid.Rows
                'Application.DoEvents()
                'Dim pu1 As Double = CDbl(r("PrecioUnitario").ToString)
                'Dim PU As Double = Format(pu1, "###,###.00")
                'Dim descu As Double = Format(CDbl(r("Descu").ToString), "###,###.00")
                'Dim iva1 As Double = Format(CDbl(r("Iva").ToString), "###,###.00")
                'Dim codsat As String
                'If r("codsat").ToString = "" Then
                'codsat = "01010101"
                'Else
                'codsat = r("codsat").ToString
                'End If
                'Dim Borrar As String = Trim(r("Descripcion").ToString)
                'While Borrar.Contains("  ")
                'Borrar = Borrar.Replace("  ", " ")
                'Borrar = Trim(Borrar)
                'End While

                'FormatoGrid(codsat, r("vdclvUni").ToString, r("ClaveInterno").ToString, Borrar, _
                '            r("Cantidad").ToString, "PZA", PU, iva1, _
                '            descu, r("Importe").ToString)
                'Me.Importe.Value += (r("Importe").ToString)
                'Me.Descuento.Value += (descu)
                'Me.Iva.Value += iva1
                'upgb1.Value += 1
                'Application.DoEvents()
                'Next
            End If
        Else
            MsgBox("No se encontró información.. ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub FormatoGrid(codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String, _
                             Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double, _
                             Descu As Double, Importe As Double)
        Ugr.Rows.Band.AddNew()
        ColocarArticulo(Ugr.Rows.Count - 1, codSat, cvlUni, ClaveInterno, Descripcion, Cantidad, Unidad, PrecioUnitario, Iva, Descu, Importe)
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String, _
                              Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double, _
                              Descu As Double, Importe As Double)
        With Ugr
            .Rows(Renglon).Cells("CodSat").Value = codSat
            .Rows(Renglon).Cells("vdclvUni").Value = cvlUni
            .Rows(Renglon).Cells("ClaveInterno").Value = Trim(ClaveInterno)
            .Rows(Renglon).Cells("Descripcion").Value = Trim(Descripcion)
            .Rows(Renglon).Cells("Cantidad").Value = Cantidad
            .Rows(Renglon).Cells("Unidad").Value = Unidad
            .Rows(Renglon).Cells("PrecioUnitario").Value = PrecioUnitario
            .Rows(Renglon).Cells("Iva").Value = Iva
            .Rows(Renglon).Cells("Descu").Value = Descu
            .Rows(Renglon).Cells("Importe").Value = Importe            
        End With
    End Sub

    Private Sub InicializaCantidades()
        Me.Devoluciones.Value = 0
        Me.Importe.Value = 0
        Me.Subtotal.Value = 0
        Me.Descuento.Value = 0
        Me.Iva.Value = 0
        Me.Total.Value = 0
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout

    End Sub

    Private Sub btnGenerarFactura_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        If grbTienda.Text = "JUAN FRANCISCO JIMENEZ GONZALEZ" Then
            notaMax = objpro.DatoMaximo("vdidfolv", "ventadet", "year(vdfecha)= " & txtFechaIni.Value.ToString.Substring(6, 4) & " and month(vdfecha)= " & txtFechaIni.Value.ToString.Substring(3, 2) & " ", MotorBoutique)
            notaMin = objpro.Dato("min (vdidfolv)", "ventadet", "year(vdfecha)= " & txtFechaIni.Value.ToString.Substring(6, 4) & " and month(vdfecha)= " & txtFechaIni.Value.ToString.Substring(3, 2) & "", MotorBoutique)
        Else
        End If
        Factura()
        consulta_registros()
    End Sub

    Public Function Factura()
        If Me.Ugr.Rows.Count <> 0 Then
            td = 1
            Dim Directorios, RutFacClientes, RutFacGlobal, CarpTienda, CarpSucursal, RutNotaCredito As String
            Dim TRazonSocial, TRfc, TRFiscal, TCp As String
            If MessageBox.Show("Esta seguro de Generar Factura", "FACTURAS GLOBALES", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                upb.Value = 0
                upb.Maximum = 100
                TablaTienda = objpro.ObtenDatos("Select * from CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
                TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
                TRfc = TablaTienda.Rows(0).Item("trfc").ToString
                TCp = TablaTienda.Rows(0).Item("tcp").ToString
                TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString

                Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
                RutFacClientes = TablaTienda.Rows(0).Item("sForm1").ToString
                RutFacGlobal = TablaTienda.Rows(0).Item("sForm2").ToString
                RutNotaCredito = TablaTienda.Rows(0).Item("sForm3").ToString
                CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
                CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString

                Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
                Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
                Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
                Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
                Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
                Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)

                Select Case grbTienda.Value
                    Case 10
                        FormPago = c_FormaPago.Efectivo_01
                        fp = "01 - Efectivo"
                    Case Else
                        FormPago = c_FormaPago.Otros_99
                        fp = "99 - Otros"
                End Select

                Dim RFacImpresas As String
                RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
                Dim c As New CFDI.CFDI33.Comprobante(Today, txtSerie, txtFactura, c_TipoDeComprobante.Ingreso, FormPago, c_MetodoPago.PagoEnUnaSolaExhibicion_PUE)
                c.Moneda = CFDI.CFDI33.c_Moneda.MXN
                c.CondicionesDePago = "CONTADO"
                c.TipoCambio = 1
                c.TipoCambioSpecified = True

                upb.Value += 10

                Select Case TRFiscal
                    Case 601
                        TpRegimen = c_RegimenFiscal.GeneralDeLeyPersonasMorales
                        DesRegimen = "General De Ley Personas Morales"
                    Case 603
                        TpRegimen = c_RegimenFiscal.PersonasMoralesConFinesNoLucrativos
                        DesRegimen = "Personas Morales ConFines No Lucrativos"
                    Case 605
                        TpRegimen = c_RegimenFiscal.SueldosYSalariosEIngresosAsimiladosASalarios
                        DesRegimen = "Sueldos Y Salarios E Ingresos Asimilados A Salarios"
                    Case 606
                        TpRegimen = c_RegimenFiscal.Arrendamiento
                        DesRegimen = "Arrendamiento"
                    Case 608
                        TpRegimen = c_RegimenFiscal.DemasIngresos
                        DesRegimen = "Demas Ingresos"
                    Case 609
                        TpRegimen = c_RegimenFiscal.Consolidacion
                        DesRegimen = "Consolidacion"
                    Case 610
                        TpRegimen = c_RegimenFiscal.ResidentesEnElExtranjeroSinEstablecimientoPermanenteEnMexico
                        DesRegimen = "Residentes En El Extranjero Sin Establecimiento Permanente En Mexico"
                    Case 611
                        TpRegimen = c_RegimenFiscal.IngresosPorDividendos
                        DesRegimen = "Ingresos Por Dividendos"
                    Case 612
                        TpRegimen = c_RegimenFiscal.PersonasFisicasConActividadesEmpresarialesYProfesionales
                        DesRegimen = "Personas Fisicas Con Actividades Empresariales Y Profesionales"
                    Case 614
                        TpRegimen = c_RegimenFiscal.IngresosPorIntereses
                        DesRegimen = "Ingresos Por Intereses"
                    Case 616
                        TpRegimen = c_RegimenFiscal.SinObligacionesFiscales
                        DesRegimen = "Sin Obligaciones Fiscales"
                    Case 607
                        TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                        DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
                    Case 621
                        TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                        DesRegimen = "Incorporacion Fiscal"
                End Select

                c.AgregaDatosEmisor(TRfc, TRazonSocial, TpRegimen)
                c.LugarExpedicion = TCp
                c.AgregaDatosReceptor("XAXX010101000", "PUBLICO EN GENERAL", c_UsoCFDI.Otros, c_Pais.Mexico)

                If grbTienda.Text = "JUAN FRANCISCO JIMENEZ GONZALEZ" Then
                    Cadena = "De la fecha " & txtFechaIni.Value.ToString.Substring(0, 10) & " a la fecha " & txtFechaFin.Value.ToString.Substring(0, 10) & " de la Nota " & notaMin & " a la Nota " & notaMax & " - Importe - " & " $" & Str(FormatCurrency(Me.Total.Value))
                Else
                    Cadena = "De la fecha " & txtFechaIni.Value.ToString.Substring(0, 10) & " a la fecha " & txtFechaFin.Value.ToString.Substring(0, 10) & " - Importe - " & " $" & Str(FormatCurrency(Me.Total.Value))
                End If

                Dim i As Integer
                Dim sb As Double = 0
                Dim des As Double = 0
                Dim iv As Double = 0
                Dim iiv As Double = 0
                Dim PuCa As Double = 0

                c.TruncarDecimales = True
                c.TruncarDecimalesLongitud = 2
                c.Impuestos = Nothing

                'If grbTienda.Text = "JUAN FRANCISCO JIMENEZ GONZALEZ" Then

                'Else
                For i = 0 To Me.Ugr.Rows.Count - 1
                    Dim pun As Double = Format(Me.Ugr.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                    Dim producto As String = Trim(Me.Ugr.Rows(i).Cells("DESCRIPCION").Value)
                    Dim Clave As String = Trim(Me.Ugr.Rows(i).Cells("ClaveInterno").Value)
                    Dim codsat As String
                    If Me.Ugr.Rows(i).Cells("codsat").Value = "" Then
                        codsat = "01010101"
                    Else
                        codsat = Me.Ugr.Rows(i).Cells("codsat").Value
                    End If
                    c.AgregaConcepto(Trim(Me.Ugr.Rows(i).Cells("Cantidad").Value), "PZA", "H87", codsat, Trim(Clave), Trim(producto), pun)
                    c.Conceptos(i).Impuestos = Nothing
                    'Dim pun As Double = Format(Me.Ugr.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                    'Dim producto As String = Trim(Me.Ugr.Rows(i).Cells("DESCRIPCION").Value)
                    'Dim Clave As String = Trim(Me.Ugr.Rows(i).Cells("ClaveInterno").Value)
                    'Dim codsat As String
                    'If Me.Ugr.Rows(i).Cells("codsat").Value = "" Then
                    'codsat = "01010101"
                    'Else
                    'codsat = Me.Ugr.Rows(i).Cells("codsat").Value
                    'End If
                    'c.AgregaConcepto(Trim(Me.Ugr.Rows(i).Cells("Cantidad").Value), "PZA", "H87", codsat, Trim(Clave), Trim(producto), pun)
                    sb += Format(Me.Ugr.Rows(i).Cells("Importe").Value, "###,###.00")
                    NumPiezas += Me.Ugr.Rows(i).Cells("Cantidad").Value
                    'c.Conceptos(i).Impuestos.Traslados.Add(New CFDI.CFDI33.ComprobanteConceptoImpuestosTraslado)
                    If Me.Ugr.Rows(i).Cells("Descu").Value <> 0 Then
                        If Format(Me.Ugr.Rows(i).Cells("Importe").Value, "###,###.00") - Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00") <> 0 Then
                            c.Conceptos(i).Descuento = Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00")
                            c.Conceptos(i).DescuentoSpecified = True
                            des += Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00")
                        Else
                            c.Conceptos(i).Descuento = (Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                            c.Conceptos(i).DescuentoSpecified = True
                            des += (Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                        End If
                    End If

                    'With c.Conceptos(i).Impuestos.Traslados(0)
                    'If (Format(Me.Ugr.Rows(i).Cells("Importe").Value, "###,###.00") - Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00")) <> 0 Then
                    '.Importe = Format(((Me.Ugr.Rows(i).Cells("Importe").Value - Me.Ugr.Rows(i).Cells("Descu").Value) * 0.16), "###,###.00")
                    '.ImporteSpecified = True
                    'iv += .Importe
                    '.Base = Format((Me.Ugr.Rows(i).Cells("Importe").Value - Me.Ugr.Rows(i).Cells("Descu").Value), "###,###.00")
                    '.Impuesto = CFDI.CFDI33.c_Impuesto.IVA
                    '.TasaOCuota = 0.16
                    '.TasaOCuotaSpecified = True
                    '.TipoFactor = CFDI.CFDI33.c_TipoFactor.Tasa
                    'Else
                    '.Importe = Format(((Me.Ugr.Rows(i).Cells("Importe").Value - (Me.Ugr.Rows(i).Cells("Descu").Value - 0.1)) * 0.16), "###,###.00")
                    '.ImporteSpecified = True
                    'iv += .Importe
                    '.Base = Format(Me.Ugr.Rows(i).Cells("Importe").Value, "###,###.00") - (Format(Me.Ugr.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                    '.Impuesto = CFDI.CFDI33.c_Impuesto.IVA
                    '.TasaOCuota = 0.16
                    '.TasaOCuotaSpecified = True
                    '.TipoFactor = CFDI.CFDI33.c_TipoFactor.Tasa
                    'End If
                    'End With
                Next

                'End If   'Valida Cierra la empresa a Facturar

                'Dim impuestoIVA As New ComprobanteImpuestosTraslado
                'With impuestoIVA
                '.Importe = Format(iv, "###,###.00")
                '.Impuesto = c_Impuesto.IVA
                '.TasaOCuota = 0.16
                '.TipoFactor = c_TipoFactor.Tasa
                'End With
                'c.Impuestos.Traslados.Add(impuestoIVA)

                'c.Impuestos.TotalImpuestosTrasladados = Format(iv, "###,###.00")
                'c.Impuestos.TotalImpuestosTrasladadosSpecified = True

                Dim ValorTicket As String = Cadena
                NoCuenta = ""
                Dim DatosImpresionExtra As New Hashtable
                DatosImpresionExtra.Add("NumTicket", ValorTicket)
                DatosImpresionExtra.Add("TotPiezas", NumPiezas)
                DatosImpresionExtra.Add("FP", fp)
                DatosImpresionExtra.Add("NoCuenta", NoCuenta)
                DatosImpresionExtra.Add("DesRegimen", DesRegimen)

                If Descuento.Value <> 0 Then
                    c.Descuento = des
                    c.DescuentoSpecified = True
                Else
                    c.DescuentoSpecified = False
                End If
                c.Total = Format(((c.SubTotal - des) + iv), "###,###.00")



                Dim motorcfdi As New CFDI.CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
                c.Impuestos = Nothing
                CFDI.Licenciamiento.AgregaLicencia("NxfUWVt27sSyPaPK/VSpaM1KxObdAq1pQgX7SmJWTEWljBDrg2XqOSmxSth7pZ6FRsViFC+MzuubR0AfCl1wgYiKnOWpT6XWJMXAy58SbcE=")
                motorcfdi.CapitalizacionCantidadConLetra = CFDI.Globales.Capitalizacion.PrimeraLetraFrase
                Application.DoEvents()
                motorcfdi.Comprobantes.Add(c)
                Application.DoEvents()
                upb.Value += 30  'Suma 40
                Dim resultado As ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)     REM CFDI.CFDI33.ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)
                Application.DoEvents()

                If Not resultado.Errores Then
                    Application.DoEvents()
                    upb.Value += 10 'Suma 50
                    Dim svcNew As New fel.WSCFDI33Client
                    Dim Respuesta As New fel.RespuestaTFD33

                    svcNew.Open()
                    Dim ReferenciaPac As String = Trim(TRfc) & "-" & txtSerie & "-" & txtFactura
                    Respuesta = svcNew.TimbrarCFDI(Trim(UserPAC), Trim(PwdPAC), IO.File.ReadAllText(c.ArchivoXMLSellado), Trim(ReferenciaPac))
                    upb.Value += 10 'Suma 60
                    svcNew.Close()

                    Dim archivoREPX As String = ""
                    Dim archivoPDF As String = ""
                    If Respuesta.OperacionExitosa Then
                        If IO.File.Exists(c.ArchivoXMLSellado) Then
                            Application.DoEvents()
                            Timbrado = True
                            Dim archivoXMLPDF As String
                            IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml"), Respuesta.XMLResultado)
                            CFDI.Generar.ArchivoCadenaTFD(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml")
                            CFDI.Generar.ArchivoCBB(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml")
                            CFDI.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml", CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
                            Dim NombrePDF As String = RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml"
                            archivoXMLPDF = CFDI.Generar.ArchivoDatosPDF(Trim(NombrePDF), CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
                            upb.Value += 10 'Suma 70


                            Try
                                Dim RutReporte As String
                                RutReporte = RutFacGlobal & "\RecursosCFD33"
                                archivoREPX = CFDI.PDF.Generar.ArchivoReporteBase(RutReporte, False)
                                archivoPDF = CFDI.PDF.Generar.ArchivoPDF2(archivoREPX, archivoXMLPDF)
                                upb.Value += 10 'Suma 80                               
                                PDF(archivoPDF)
                            Catch ex As Exception
                                REM MsgBox(ex.Message.ToString)
                            End Try

                            Dim CadenaUUID As String = IO.File.ReadAllText(c.ArchivoCBB)
                            Dim VectorUUID() As String = Split(CadenaUUID, "=")
                            Dim simleUuid() As String
                            simleUuid = Split(VectorUUID(1), "&")
                            e_uuid = simleUuid(0)
                            Application.DoEvents()

                            Dim CantidadLetra, Estatus As String
                            Dim ExisteClave As String = 10
                            Dim fecha1, fecha2, fecha3, fechareal, FeRegistro As String
                            fecha1 = FechaFactura.Substring(0, 2)
                            fecha2 = FechaFactura.Substring(3, 2)
                            fecha3 = FechaFactura.Substring(6, 4)
                            fechareal = Now.Date.ToString.Substring(0, 2) & "/" & Now.Date.ToString.Substring(3, 2) & "/" & fecha3
                            FeRegistro = FechaFactura.Substring(6, 4) & fecha2 & fecha1
                            Dim FeFecCan As Date = "01/01/1900"
                            Dim FeMotcan As String = ""
                            Dim Tipo As String = "F"

                            If grbTienda.Text = "JUAN FRANCISCO JIMENEZ GONZALEZ" Then
                                objpro.Indicacion("Update VentaEnc set veEstatu =1, veFac = '" & CInt(txtFactura) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & td & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 and Month(veFecha) = " & txtFechaIni.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaFin.Value.ToString.Substring(6, 4) & "", Motor)
                            Else
                                objpro.Indicacion("Update VentaEnc set veEstatu =1, veFac = '" & CInt(txtFactura) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & td & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 and Day(veFecha) >= " & txtFechaIni.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaIni.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaFin.Value.ToString.Substring(6, 4) & "", Motor)
                            End If


                            objpro.Indicacion("Update Serie set seCant = " & CInt(txtFactura) & " Where seSerie = '" & txtSerie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                            upb.Value += 10 'Suma 90

                            Dim r_sub As Double = Subtotal.Value
                            Dim r_des As Double = Descuento.Value
                            Dim r_iva As Double = Iva.Value
                            Dim r_total As Double = Total.Value

                            CantidadLetra = ""
                            Estatus = 1

                            objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha," & _
                            " feHora,Cid,FeSubt,fePorIva,FeIva, " & _
                            " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " & _
                            " FeCanlet,FeAbonos,FeEstatu,FeReg, FeFecCan,FeMotcan, feTip,feuuid) " & _
                            " values (" & CInt(Transaccion) & "," & grbTienda.Value & "," & Me.grbTienda.Value & Me.grbSucursal.Value & "," & CInt(txtFactura) & ",'" & txtSerie & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," & _
                            " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ",'" & r_sub & "', '" & CDbl(PorIVa) & "','" & r_iva & "'," & _
                            " '" & r_des & "','" & r_des & "', '" & r_des & "', '" & r_des & "', '" & r_total & "', " & _
                            " '" & CantidadLetra & "', '" & r_total & "'," & Estatus & "," & FeRegistro & ",  " & FeFecCan & ", '" & FeMotcan & "', '" & Tipo & "','" & e_uuid & "')", Motor)
                            Application.DoEvents()
                            upb.Value += 10 'Suma 100

                            If Devoluciones.Value <> 0 Then
                                Application.DoEvents()                                
                                NotaCredito(TRazonSocial, TRfc, TRFiscal, RutNotaCredito, CarpTienda, CarpSucursal, Directorios, UserPAC, PwdPAC, e_uuid, TCp)
                            End If

                        End If
                    Else
                        Dim sll = c.ArchivoSello
                        Dim txt = sll.ToString.Replace(".sll", ".txt")
                        Dim xml = sll.ToString.Replace(".sll", ".xml")
                        IO.File.Delete(sll) : IO.File.Delete(txt) : IO.File.Delete(xml)
                        MsgBox(Respuesta.CodigoRespuesta.ToString & " " & Respuesta.MensajeError.ToString, MsgBoxStyle.Information, "Facturación.. ")
                    End If 'Cierra respuesta de que el XML es timbrado correctamente
                Else
                    For Each cNG As CFDI.CFDI33.Comprobante In resultado.ComprobantesNoGenerados
                        MessageBox.Show(cNG.ErrorGeneracion.Message.ToString)
                    Next
                    REM MsgBox(resultado.Errores.ToString, MsgBoxStyle.Information, "Facturación.. ")
                End If 'Cierra validación de resultado de XML
            End If 'Cierra validación en la pregunta del msgbox
            End If 'Cierra validación de registros en el Grid
            Return ""
    End Function

    Public Function NotaCredito(ByVal n_TRazonSocial As String, ByVal n_TRfc As String, ByVal n_TRFiscal As String, _
                                ByVal n_RutNotaCredito As String, ByVal n_CarpTienda As String, ByVal n_CarpSucursal As String, ByVal n_Directorios As String, ByVal UserPAC As String, _
                                ByVal PwdPAC As String, ByVal n_uuid As String, n_LExpedicion As String)
        Timbrado = False
        td = 2
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim n_serie As String
        Dim n_notacredito As String
        DesRegimen = ""
        NumPiezas = 0


        upb.Value = 0
        upb.Maximum = 100

        TIP = "NC"
        TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
        n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
        n_serie = TSucursal.Rows(0).Item("Seserie").ToString

        Dim RFacImpresas As String
        RFacImpresas = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
        Dim c As New CFDI.CFDI33.Comprobante(Today, n_serie, n_notacredito, c_TipoDeComprobante.Egreso, c_FormaPago.Otros_99, c_MetodoPago.PagoEnUnaSolaExhibicion_PUE)
        c.Moneda = CFDI.CFDI33.c_Moneda.MXN
        fp = "Otros"

        Select Case n_TRFiscal
            Case 601
                TpRegimen = c_RegimenFiscal.GeneralDeLeyPersonasMorales
                DesRegimen = "General De Ley Personas Morales"
            Case 603
                TpRegimen = c_RegimenFiscal.PersonasMoralesConFinesNoLucrativos
                DesRegimen = "Personas Morales Con Fines No Lucrativos"
            Case 605
                TpRegimen = c_RegimenFiscal.SueldosYSalariosEIngresosAsimiladosASalarios
                DesRegimen = "Sueldos Y Salarios E Ingresos Asimilados A Salarios"
            Case 606
                TpRegimen = c_RegimenFiscal.Arrendamiento
                DesRegimen = "Arrendamiento"
            Case 608
                TpRegimen = c_RegimenFiscal.DemasIngresos
                DesRegimen = "Demas Ingresos"
            Case 609
                TpRegimen = c_RegimenFiscal.Consolidacion
                DesRegimen = "Consolidacion"
            Case 610
                TpRegimen = c_RegimenFiscal.ResidentesEnElExtranjeroSinEstablecimientoPermanenteEnMexico
                DesRegimen = "Residentes En El Extranjero Sin Establecimiento Permanente En Mexico"
            Case 611
                TpRegimen = c_RegimenFiscal.IngresosPorDividendos
                DesRegimen = "Ingresos Por Dividendos"
            Case 612
                TpRegimen = c_RegimenFiscal.PersonasFisicasConActividadesEmpresarialesYProfesionales
                DesRegimen = "Personas Fisicas Con Actividades Empresariales Y Profesionales"
            Case 614
                TpRegimen = c_RegimenFiscal.IngresosPorIntereses
                DesRegimen = "Ingresos Por Intereses"
            Case 616
                TpRegimen = c_RegimenFiscal.SinObligacionesFiscales
                DesRegimen = "Sin Obligaciones Fiscales"
            Case 607
                TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
            Case 621
                TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                DesRegimen = "Incorporacion Fiscal"
        End Select

        c.AgregaDatosEmisor(n_TRfc, n_TRazonSocial, TpRegimen)
        c.AgregaDatosReceptor("XAXX010101000", "PUBLICO EN GENERAL", c_UsoCFDI.Otros, c_Pais.Mexico)
        c.LugarExpedicion = n_LExpedicion
        c.Impuestos = Nothing

        Dim Pu As Double = Format((Me.Devoluciones.Value), "###,###.00")
        Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")
        c.AgregaConcepto("1", "SERVICIO", "ACT", "84111506", "100", "Devoluciones del día " & txtFechaIni.Value.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Pu, "###,###.00"), Format(Pu, "###,###.00"))
        c.Conceptos(0).Impuestos = Nothing

        'If Iva.Value <> 0 Then
        'c.Conceptos(0).Impuestos.Traslados.Add(New CFDI.CFDI33.ComprobanteConceptoImpuestosTraslado) 'CFDI.CFDI33.ComprobanteConceptoImpuestosTraslado)            
        'With c.Conceptos(0).Impuestos.Traslados(0)
        '.Base = Format(Pu, "###,###.00")
        '.Importe = Format((Pu * 0.16), "###,###.00")
        '.ImporteSpecified = True
        'iv += .Importe
        '.Impuesto = c_Impuesto.IVA  REM CFDI.CFDI33.c_Impuesto.IVA
        '.TasaOCuota = 0.16
        '.TasaOCuotaSpecified = True
        '.TipoFactor = c_TipoFactor.Tasa  REM CFDI.CFDI33.c_TipoFactor.Tasa
        'End With
        'End If

        'Dim impuestoIVA As New ComprobanteImpuestosTraslado
        'With impuestoIVA
        '.Importe = Format(iv, "###,###.00")
        '.Impuesto = c_Impuesto.IVA
        '.TasaOCuota = 0.16
        '.TipoFactor = c_TipoFactor.Tasa
        'End With
        'c.Impuestos.Traslados.Add(impuestoIVA)

        'c.Impuestos.TotalImpuestosTrasladados = Format(iv, "###,###.00")
        'c.Impuestos.TotalImpuestosTrasladadosSpecified = True

        c.TruncarDecimales = True
        c.TruncarDecimalesLongitud = 2

        Dim ValorTicket As String = "Devoluciones del día " & txtFechaIni.Value.ToString.Substring(0, 10)
        NoCuenta = ""
        Dim DatosImpresionExtra As New Hashtable
        DatosImpresionExtra.Add("NumTicket", ValorTicket)
        DatosImpresionExtra.Add("TotPiezas", NumPiezas)
        DatosImpresionExtra.Add("FP", fp)
        DatosImpresionExtra.Add("NoCuenta", NoCuenta)
        DatosImpresionExtra.Add("DesRegimen", DesRegimen)

        REM c.SubTotal = Format(Pu, "###,###.00")
        c.Total = Format((Pu + iv), "###,###.00")
        c.DescuentoSpecified = False

        'cfdi relacionados
        Dim cr As New N400.Componentes.CFDI.CFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado
        cr.UUID = n_uuid  'aqui va el UUID del documento a relacionar
        c.CfdiRelacionados = New ComprobanteCfdiRelacionados
        c.CfdiRelacionados.CfdiRelacionado = New List(Of ComprobanteCfdiRelacionadosCfdiRelacionado)
        c.CfdiRelacionados.TipoRelacion = c_TipoRelacion.NotaDeCreditoDocumentosRelacionados_01
        c.CfdiRelacionados.CfdiRelacionado.Add(cr)


        Dim motorcfdi As New CFDI.CFDI33.MotorCFDI(TablaTienda.Rows(0).Item("srcer1").ToString, TablaTienda.Rows(0).Item("srkey").ToString, TablaTienda.Rows(0).Item("spwd").ToString, RFacImpresas)
        CFDI.Licenciamiento.AgregaLicencia("NxfUWVt27sSyPaPK/VSpaM1KxObdAq1pQgX7SmJWTEWljBDrg2XqOSmxSth7pZ6FRsViFC+MzuubR0AfCl1wgYiKnOWpT6XWJMXAy58SbcE=")
        motorcfdi.CapitalizacionCantidadConLetra = CFDI.Globales.Capitalizacion.PrimeraLetraFrase
        Application.DoEvents()
        motorcfdi.Comprobantes.Add(c)
        Application.DoEvents()
        Dim resultado As ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)     REM CFDI.CFDI33.ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)
        Application.DoEvents()

        Dim archivoREPX As String = ""
        Dim archivoPDF As String = ""
        If Not resultado.Errores Then
            Application.DoEvents()
            Dim svcNew As New fel.WSCFDI33Client
            Dim Respuesta As New fel.RespuestaTFD33
            REM UserPAC = "POV990513D33"
            REM PwdPAC = "contRa$3na"
            svcNew.Open()
            Dim ReferenciaPac As String = Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito
            Respuesta = svcNew.TimbrarCFDI(Trim(UserPAC), Trim(PwdPAC), IO.File.ReadAllText(c.ArchivoXMLSellado), Trim(ReferenciaPac))
            svcNew.Close()

            If Respuesta.OperacionExitosa Then
                If IO.File.Exists(c.ArchivoXMLSellado) Then
                    Application.DoEvents()
                    Dim archivoXMLPDF As String
                    IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"), Respuesta.XMLResultado)
                    CFDI.Generar.ArchivoCadenaTFD(RFacImpresas & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")
                    CFDI.Generar.ArchivoCBB(RFacImpresas & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")
                    Application.DoEvents()
                    CFDI.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml", CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
                    Dim NombrePDF As String = RFacImpresas & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"
                    archivoXMLPDF = CFDI.Generar.ArchivoDatosPDF(Trim(NombrePDF), CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)

                    Try
                        Dim RutReporte As String
                        RutReporte = n_RutNotaCredito & "\RecursosCFD33"
                        archivoREPX = CFDI.PDF.Generar.ArchivoReporteBase(RutReporte, False)
                        archivoPDF = CFDI.PDF.Generar.ArchivoPDF2(archivoREPX, archivoXMLPDF)
                        PDF(archivoPDF)
                    Catch ex As Exception
                    End Try
                    objpro.Indicacion("Update VentaEnc set veEstatu = 1, veFac = '" & CInt(n_notacredito) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & td & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 and day(veFecha) = " & txtFechaIni.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaIni.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaIni.Value.ToString.Substring(6, 4) & "", Motor)
                    objpro.Indicacion("Update Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)

                End If
            Else
                Dim sll = c.ArchivoSello
                Dim txt = sll.ToString.Replace(".sll", ".txt")
                Dim xml = sll.ToString.Replace(".sll", ".xml")
                IO.File.Delete(sll) : IO.File.Delete(txt) : IO.File.Delete(xml)
                MsgBox(Respuesta.CodigoRespuesta.ToString & " " & Respuesta.MensajeError.ToString, MsgBoxStyle.Information, "Facturación")
            End If 'Cierra respuesta del PAC
        End If 'Cierra Error al generar XML

        Return ""
    End Function

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

End Class