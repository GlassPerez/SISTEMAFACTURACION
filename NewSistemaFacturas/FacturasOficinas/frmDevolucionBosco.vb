Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml

Public Class frmDevolucionBosco

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim fechventa As String
    Dim FechaFactura As String

    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim contador As Integer = 0
    Dim Cadena As String    
    Dim NoCuenta As String
    Dim ExisteClave As String
    Dim NumPiezas As Integer
#End Region

#Region "sat"
    Dim TpRegimen As String
    Dim DesRegimen As String
    Dim fp As String
    Dim n_TRFiscal As String
#End Region

    Private Sub frmDevolucionBosco_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DevUUID = ""
        DevTienda = 0
        DevSucursal = 0
        grbTienda.ReadOnly = True
        grbSucursal.ReadOnly = True
        FechaFactura = Date.Now.ToString.Substring(0, 10)

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM bgxfacelec.CatTien where ttipo3 = 'G' ", 0, 1, As400)
        grbTienda.Value = frmGlobalBosco.grbTienda.Value
        Application.DoEvents()
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM bgxfacelec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        grbSucursal.Value = frmGlobalBosco.grbSucursal.Value
        Application.DoEvents()

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
        txtFechaVenta.Value = CDate(fechventa)
        Application.DoEvents()
        CargaListaCredito("N")
    End Sub


    Private Sub CargaListaCredito(ByVal tp As String)
        Try
            Application.DoEvents()
            TablaSucursal = Nothing
            TablaSucursal = objpro.ObtenDatos("select * from bgxfacelec.ventaenc where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and vetm = '" & tp & "'  and veTerm = " & grbTienda.Value & grbSucursal.Value & " and substr(vefecha,1,6) = '" & Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text) & "' and substr(digits(veidfolv),1,6) = '" & Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text) & "'  ", As400).Tables(0)
            Me.ChlisNota.Items.Clear()
            Dim i As Integer = 0
            For Each row As DataRow In TablaSucursal.Rows
                Application.DoEvents()
                Me.ChlisNota.Items.Add("Dev. " & row.Item("vefecha").ToString)
                If row.Item("veEstatu") = 1 Then
                    ChlisNota.SetItemChecked(i, CheckState.Checked)
                Else
                    ChlisNota.SetItemChecked(i, CheckState.Unchecked)
                End If
                i += 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación ..")
        End Try
    End Sub

    Private Sub frmDevolucionBosco_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        consulta_registros()
    End Sub

    Public Function consulta_registros()
        If grbTienda.Text <> "" Then
            If Me.grbSucursal.Text <> "" Then
                InicializaCantidades()
                fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
                txtFechaVenta.Value = CDate(fechventa)                           
                Application.DoEvents()
                CargaListaCredito("N")
                Application.DoEvents()
            Else
                MsgBox("Falta seleccionar sucursal", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta seleccionar Tienda o Razón social", MsgBoxStyle.Information, "Facturación .. ")
        End If
        Return ""
    End Function

    Private Sub InicializaCantidades()
        Devoluciones.Value = 0
        txtUUID.Text = ""
        DevUUID = ""
    End Sub

    Private Sub btnFactura_Click(sender As Object, e As EventArgs) Handles btnFactura.Click
        'Devolucion
        Dim EstadoVenta, term As String
        Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
        Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
        Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
        Dim FecVenta As String = a & m & d
        Transaccion = Val(Trim(FecVenta))
        term = Trim(Str(grbTienda.Value)) + Trim(Str(Me.grbSucursal.Value))
        Terminal = Val(Trim(term))

        EstadoVenta = objpro.Dato("veEstatu", "bgxfacelec.VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and vetm = 'N' and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and veFecha = '" & FecVenta & "' ", As400)
        If EstadoVenta = 0 Then
            Me.Devoluciones.Value = objpro.Dato("veTotal", "bgxfacelec.VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and vetm = 'N' and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and veFecha = '" & FecVenta & "' ", As400)
        Else
            MsgBox("Ya esta generada la nota de crédito ", MsgBoxStyle.Information, "Facturación .. ")
        End If


        'Devolucion
    End Sub

    Private Sub btnGenerarFactura_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        If txtUUID.Text <> "" Then
            NotaCredito()
        Else
            MsgBox("UUID, Falta de factura relacionada ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub NotaCredito()
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim n_serie As String
        Dim n_notacredito As String
        Dim n_Directorios As String
        Dim n_TRazonSocial As String
        Dim n_TRfc As String
        Dim n_LExpedicion As String
        upb.Value = 0
        upb.Maximum = 100

        TIP = "NC"
        TSucursal = objpro.ObtenDatos("select * from bgxfacelec.Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400).Tables(0)
        n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")
        n_serie = TSucursal.Rows(0).Item("Seserie").ToString            
        Application.DoEvents()

        n_TRazonSocial = objpro.Dato("trsocial", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
        n_TRfc = objpro.Dato("trfc", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
        n_LExpedicion = objpro.Dato("tcp", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
        Dim UserPAC As String = objpro.Dato("tUserPAC", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
        Dim PwdPAC As String = objpro.Dato("tPwdPAC", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)

        Dim n_RutNotaCredito As String = objpro.Dato("sForm3", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
        Dim CerFactura As String = objpro.Dato("sRCer1", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
        Dim Key As String = objpro.Dato("sRKey", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
        Dim Pwd As String = objpro.Dato("sPwd", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)

        Dim RFacImpresas As String
        n_Directorios = objpro.Dato("subic", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
        RFacImpresas = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
        Dim c As New CFDI.CFDI33.Comprobante(Today, n_serie, n_notacredito, c_TipoDeComprobante.Egreso, c_FormaPago.Otros_99, c_MetodoPago.PagoEnUnaSolaExhibicion_PUE)
        c.Moneda = CFDI.CFDI33.c_Moneda.MXN
        fp = "Otros"
        upb.Value += 10 'Suma 10

        n_TRFiscal = objpro.Dato("satregimen", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
        Application.DoEvents()
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
        End Select

        c.AgregaDatosEmisor(n_TRfc, n_TRazonSocial, TpRegimen)
        c.AgregaDatosReceptor("XAXX010101000", "PUBLICO EN GENERAL", c_UsoCFDI.Otros, c_Pais.Mexico)
        REM c.AgregaDatosReceptor("ALV961213V22", "AUTOLINEAS VILLARREAL SA DE CV", c_UsoCFDI.Otros, c_Pais.Mexico)
        c.LugarExpedicion = n_LExpedicion
        Application.DoEvents()
        Dim Pu As Double = Format((Me.Devoluciones.Value / 1.16), "###,###.00")
        Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")
        c.AgregaConcepto("1", "SERVICIO", "ACT", "84111506", "100", "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & txtUUID.Text & " con un importe de $ " & Format(Pu, "###,###.00"), Format(Pu, "###,###.00"))
        REM c.AgregaConcepto("1", "SERVICIO", "ACT", "84111506", "100", "Bonificación por antiguedad de mercancia del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & txtUUID.Text & " con un importe de $ " & Format(Pu, "###,###.00"), Format(Pu, "###,###.00"))
            c.Conceptos(0).Impuestos.Traslados.Add(New CFDI.CFDI33.ComprobanteConceptoImpuestosTraslado) 'CFDI.CFDI33.ComprobanteConceptoImpuestosTraslado)            
            With c.Conceptos(0).Impuestos.Traslados(0)
                .Base = Format(Pu, "###,###.00")
                .Importe = Format((Pu * 0.16), "###,###.00")
                .ImporteSpecified = True
                iv += .Importe
                .Impuesto = c_Impuesto.IVA  REM CFDI.CFDI33.c_Impuesto.IVA
                .TasaOCuota = 0.16
                .TasaOCuotaSpecified = True
                .TipoFactor = c_TipoFactor.Tasa  REM CFDI.CFDI33.c_TipoFactor.Tasa
        End With

        Dim impuestoIVA As New ComprobanteImpuestosTraslado
        With impuestoIVA
            .Importe = Format(iv, "###,###.00")
            .Impuesto = c_Impuesto.IVA
            .TasaOCuota = 0.16
            .TipoFactor = c_TipoFactor.Tasa
        End With
        c.Impuestos.Traslados.Add(impuestoIVA)

        c.Impuestos.TotalImpuestosTrasladados = Format(iv, "###,###.00")
        c.Impuestos.TotalImpuestosTrasladadosSpecified = True

        c.TruncarDecimales = True
        c.TruncarDecimalesLongitud = 2

        Dim ValorTicket As String = "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10)
        NoCuenta = ""
        Dim DatosImpresionExtra As New Hashtable
        DatosImpresionExtra.Add("NumTicket", ValorTicket)
        DatosImpresionExtra.Add("TotPiezas", NumPiezas)
        DatosImpresionExtra.Add("FP", fp)
        DatosImpresionExtra.Add("NoCuenta", NoCuenta)
        DatosImpresionExtra.Add("DesRegimen", DesRegimen)

        c.Total = Format((Pu + iv), "###,###.00")
        c.DescuentoSpecified = False

        'cfdi relacionados
        Dim cr As New N400.Componentes.CFDI.CFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado
        cr.UUID = txtUUID.Text    'aqui va el UUID del documento a relacionar
        c.CfdiRelacionados = New ComprobanteCfdiRelacionados
        c.CfdiRelacionados.CfdiRelacionado = New List(Of ComprobanteCfdiRelacionadosCfdiRelacionado)
        c.CfdiRelacionados.TipoRelacion = c_TipoRelacion.NotaDeCreditoDocumentosRelacionados_01
        c.CfdiRelacionados.CfdiRelacionado.Add(cr)
        Application.DoEvents()

        Dim motorcfdi As New CFDI.CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
        CFDI.Licenciamiento.AgregaLicencia("NxfUWVt27sSyPaPK/VSpaM1KxObdAq1pQgX7SmJWTEWljBDrg2XqOSmxSth7pZ6FRsViFC+MzuubR0AfCl1wgYiKnOWpT6XWJMXAy58SbcE=")
        motorcfdi.CapitalizacionCantidadConLetra = CFDI.Globales.Capitalizacion.PrimeraLetraFrase
        Application.DoEvents()
        motorcfdi.Comprobantes.Add(c)
        Application.DoEvents()
        upb.Value += 30  'Suma 40
        Dim resultado As ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)     REM CFDI.CFDI33.ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)
        Application.DoEvents()

        Dim archivoREPX As String = ""
        Dim archivoPDF As String = ""
        If Not resultado.Errores Then
            Application.DoEvents()
            upb.Value += 10 'Suma 50
            Dim svcNew As New fel.WSCFDI33Client
            Dim Respuesta As New fel.RespuestaTFD33           
            svcNew.Open()
            Dim ReferenciaPac As String = Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito
            Respuesta = svcNew.TimbrarCFDI(Trim(UserPAC), Trim(PwdPAC), IO.File.ReadAllText(c.ArchivoXMLSellado), Trim(ReferenciaPac))
            upb.Value += 10 'Suma 60
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
                    Application.DoEvents()

                    upb.Value += 10 'Suma 70
                    Try
                        Dim RutReporte As String
                        RutReporte = n_RutNotaCredito & "\RecursosCFD33"
                        archivoREPX = CFDI.PDF.Generar.ArchivoReporteBase(RutReporte, False)
                        archivoPDF = CFDI.PDF.Generar.ArchivoPDF2(archivoREPX, archivoXMLPDF)
                        upb.Value += 10 'Suma 80                             
                    Catch ex As Exception
                    End Try

                    Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF As String
                    Fecha11 = txtFechaVenta.Text.ToString.Substring(0, 2)
                    Fecha22 = txtFechaVenta.Text.ToString.Substring(3, 2)
                    Fecha33 = txtFechaVenta.Text.ToString.Substring(6, 4)
                    FePnotasF = Fecha33 & Fecha22 & Fecha11
                    fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                    FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha22 & Fecha11
                    Fecha = fechareal
                    Dim ts As String = Trim(Me.grbTienda.Value) & Trim(Str(Me.grbSucursal.Value))
                    ts = Trim(ts)
                    upb.Value += 10 'Suma 90

                    objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1 and setipo = 'NC'", As400)
                    objpro.Indicacion("Update bgxfacelec.VentaEnc set veEstatu = 1, veFac = '" & CInt(n_notacredito) & "' Where veFolio = " & Me.grbTienda.Value & " and vetm = 'N' and  veIdFolv = " & FeRegistro & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & ts & " and  veEstado = 1 and veFecha = '" & FeRegistro & "' ", As400)

                    Application.DoEvents()
                    upb.Value += 10 'Suma 100

                    PDF(archivoPDF)
                    InicializaCantidades()
                    consulta_registros()

                End If 'Cierra el XML timbrado
            End If ' Cierra la validación del PAC
        End If 'Cierra la generación del CFDI


    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub btnBuscaUUID_Click(sender As Object, e As EventArgs) Handles btnBuscaUUID.Click
        DevUUID = ""
        DevTienda = grbTienda.Value
        DevSucursal = grbSucursal.Value

        Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
        Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
        Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
        Dim FecVenta As String = a & m & d
        Devfeid = Val(FecVenta)
        Formulario(frmUUIDBosco)
        txtUUID.Text = DevUUID
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function
End Class