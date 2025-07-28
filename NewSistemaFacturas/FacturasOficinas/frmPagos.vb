Imports System
Imports N400
Imports N400.Componentes.CFDI
Imports N400.Componentes.CFDI.PDF
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml


Public Class frmPagos

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Variables"
    Dim TIP As String
    Dim TablaTienda As DataTable

    Dim banSolCuenta As Boolean
    Dim fp As String
    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim TRFiscal As String
    Dim TpRegimen As String
    Dim Uso_CFDI As String
    Dim lugarPago As String
#End Region

#Region "DatosEmisor"
    Dim TRazonSocial As String
    Dim TRfc As String
    Dim RegimenFiscal As String
    Dim DesRegimenFiscal As String
    Dim DesCFDI As String
    Dim TCp As String
    Dim cCP As String
    Dim cDireccion As String
    Dim cComDir As String
#End Region

#Region "Forma Pago"
    Dim FormPago As String
    Dim MetPago As String    
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
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

    Private Sub frmPagos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "PA"
        verNumCta(False)
        verFormaPago(True)
        verEmpresa(False)
        txtUUID.Text = frmComplementoPagos.txtUUID.Text
        txtFactura.Text = frmComplementoPagos.txtFactura.Text
        txtSerie.Text = frmComplementoPagos.txtSerie.Text
        txtFechaFactura.Value = frmComplementoPagos.txtFechaFactura.Value
        Total.Value = frmComplementoPagos.Total.Value
        Abono.Value = frmComplementoPagos.Abono.Value
        Saldo.Value = frmComplementoPagos.Saldo.Value
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        fechaPago.Value = Now.Date
        Application.DoEvents()
    End Sub

    Private Sub frmPagos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnAplicarPago_Click(sender As Object, e As EventArgs) Handles btnAplicarPago.Click
        If txtUUID.Text <> "" Then
            If Importe.Value > 0 Then
                If MsgBox("Seguro de realizar pago .. ", MsgBoxStyle.OkCancel, "Facturación .. ") = MsgBoxResult.Ok Then
                    If Importe.Value <= Saldo.Value Then
                        Dim sumSaldo As Double = 0
                        ' sumSaldo = (Importe.Value + Abono.Value)
                        sumSaldo = Importe.Value
                        If sumSaldo <= Saldo.Value Then
                            If txtRfc.Text <> "" Then
                                If txtRazonSocial.Text <> "" Then
                                    Pago()
                                Else
                                    MsgBox("Falta seleccionar Razon Social receptor .. ", MsgBoxStyle.Information, "Facturación .. ")
                                End If
                            Else
                                MsgBox("Falta seleccionar rfc receptor .. ", MsgBoxStyle.Information, "Facturación .. ")
                            End If
                        Else
                            MsgBox("Importe + Abono, no puede ser mayor a saldo .. ", MsgBoxStyle.Information, "Facturación .. ")
                        End If
                    Else
                        MsgBox("Importe no puede ser mayor a saldo .. ", MsgBoxStyle.Information, "Facturación .. ")
                    End If
                End If
            Else
                MsgBox("Importe a aplicar no puede ser menor o igual a cero ", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("Falta UUID relacionado ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub Pago()
        upb.Value = 0
        upb.Maximum = 100
        Dim d As String = fechaPago.Value.ToString.Substring(0, 2)
        Dim m As String = fechaPago.Value.ToString.Substring(3, 2)
        Dim a As String = fechaPago.Value.ToString.Substring(6, 4)
        Dim FecPAgo As String = a & "-" & m & "-" & d & "T12:28:16"

        Dim serie As String = objpro.Dato("seserie", "bgxfacelec.Serie", "sId = " & frmComplementoPagos.grbSucursal.Value & " and Tid = " & frmComplementoPagos.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)
        Dim NumPago As String = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & frmComplementoPagos.grbSucursal.Value & " and Tid = " & frmComplementoPagos.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")

        TablaTienda = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & frmComplementoPagos.grbTienda.Value & " and Sucursal.sid = " & frmComplementoPagos.grbSucursal.Value & " ", As400).Tables(0)

        DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
        PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
        Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
        PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
        SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
        PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
        DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
        AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
        MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString

        Dim Directorios, RutFacClientes, RutFacGlobal, CarpTienda, CarpSucursal, RutNotaCredito As String
        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, rutlogo As String
        Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
        Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
        Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
        Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
        Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
        Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
        Dim RutPagos = TablaTienda.Rows(0).Item("sForm4").ToString
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim FechaFactura As String = Date.Now.ToString.Substring(0, 10)
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
        TRfc = TablaTienda.Rows(0).Item("trfc").ToString
        TCp = TablaTienda.Rows(0).Item("tcp").ToString
        TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
        CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
        CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
        rutlogo = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png"
        Dim RFacImpresas As String
        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

        Dim rfcEmisor As String = objpro.Dato("trfc", "bgxfacelec.cattien", "Tid = " & frmComplementoPagos.grbTienda.Value & " ", As400)
        Dim rsEmisor As String = objpro.Dato("trsocial", "bgxfacelec.cattien", "Tid = " & frmComplementoPagos.grbTienda.Value & " ", As400)
        TRFiscal = objpro.Dato("satregimen", "bgxfacelec.catsucu", "Tid = " & frmComplementoPagos.grbTienda.Value & " and sid = " & frmComplementoPagos.grbSucursal.Value & " ", As400)
        lugarPago = objpro.Dato("selugarexp", "bgxfacelec.Serie", "sId = " & frmComplementoPagos.grbSucursal.Value & " and Tid = " & frmComplementoPagos.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)

        Dim c As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml") 'New CFDI33.Comprobante

        EasyOne_cfdi33_SetCfdi40(c, "01", "")

        Dim DS As DataSet = New DataSet("DataSetFac")
        Dim Tabla1 As DataTable = DS.Tables.Add("DatosGen")

        Tabla1.Columns.Add("Erfc")
        Tabla1.Columns.Add("Erazonsoc")
        Tabla1.Columns.Add("Eregfis")
        Tabla1.Columns.Add("Edireccion")
        Tabla1.Columns.Add("Edircomp")
        Tabla1.Columns.Add("NoDoc")
        Tabla1.Columns.Add("foliofisc")
        Tabla1.Columns.Add("provcert")
        Tabla1.Columns.Add("tipocomp")
        Tabla1.Columns.Add("usocfdi")
        Tabla1.Columns.Add("metpago")
        Tabla1.Columns.Add("formpago")
        Tabla1.Columns.Add("exporta")
        Tabla1.Columns.Add("fechaemi")
        Tabla1.Columns.Add("lugaremi")
        Tabla1.Columns.Add("serieceremi")
        Tabla1.Columns.Add("Rrfc")
        Tabla1.Columns.Add("Rrazonsoc")
        Tabla1.Columns.Add("Rdireccion")
        Tabla1.Columns.Add("Rcp")
        Tabla1.Columns.Add("moneda")
        Tabla1.Columns.Add("sellodig")
        Tabla1.Columns.Add("sellosat")
        Tabla1.Columns.Add("complesat")
        Tabla1.Columns.Add("Rregimen")
        Tabla1.Columns.Add("Fsubtotal")
        Tabla1.Columns.Add("Fdescuento")
        Tabla1.Columns.Add("Fiva")
        Tabla1.Columns.Add("Ftotal")
        Tabla1.Columns.Add("cantidadl")
        Tabla1.Columns.Add("certSAT")
        Tabla1.Columns.Add("hryfechcer")
        Tabla1.Columns.Add("sucursal")
        Tabla1.Columns.Add("rcomdir")
        Tabla1.Columns.Add("rusocfdi")
        Tabla1.Columns.Add("nocuenta")

        Dim Fila As DataRow

        Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
        Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
        Fila("Erazonsoc") = TRazonSocial
        Fila("Eregfis") = TRFiscal + " - " + descreg.Trim
        Fila("Edireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
        Fila("Edircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString
        'Dim c As New CFDI33.Comprobante
        'With c            
        '    .Serie = serie
        '    .Folio = NumPago
        '    .TipoCambio = 1
        '    .TipoCambioSpecified = False
        '    .Fecha = Now.ToString("yyyy-MM-ddTHH:mm:ss")
        '    .SubTotal = 0
        '    .Moneda = c_Moneda.XXX
        '    .Total = 0
        '    .TipoDeComprobante = c_TipoDeComprobante.Pago
        '    .LugarExpedicion = objpro.Dato("selugarexp", "bgxfacelec.Serie", "sId = " & frmComplementoPagos.grbSucursal.Value & " and Tid = " & frmComplementoPagos.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)
        '    .Impuestos = Nothing
        'End With
        EasyOne_cfdi33_SetComprobante(c, serie, NumPago, Now.ToString("yyyy-MM-ddTHH:mm:ss"), "", "", 0, "", "XXX", "", 0, "P", "", lugarPago, "")

        upb.Value += 10 'Inicia valor

        'Dim rfcEmisor As String = objpro.Dato("trfc", "bgxfacelec.cattien", "Tid = " & frmComplementoPagos.grbTienda.Value & " ", As400)
        'Dim rsEmisor As String = objpro.Dato("trsocial", "bgxfacelec.cattien", "Tid = " & frmComplementoPagos.grbTienda.Value & " ", As400)
        TRFiscal = objpro.Dato("satregimen", "bgxfacelec.catsucu", "Tid = " & frmComplementoPagos.grbTienda.Value & " and sid = " & frmComplementoPagos.grbSucursal.Value & " ", As400)

        Dim rfcReceptor As String = Trim(txtRfc.Text)
        Dim rsReceptor As String = Trim(txtRazonSocial.Text)

        REM Dim rfcReceptor As String = objpro.Dato("trfc", "bgxfacelec.cattien", "Tid = " & grbTienda.Value & " ", As400)
        REM Dim rsReceptor As String = objpro.Dato("trsocial", "bgxfacelec.cattien", "Tid = " & grbTienda.Value & " ", As400)

        Select Case TRFiscal
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
            Case 626
                TpRegimen = "626"
                DesRegimen = "Régimen Simplificado de Confianza"
        End Select

        'c.AgregaDatosEmisor(rfcEmisor, rsEmisor, TpRegimen)
        EasyOne_cfdi33_SetEmisor(c, rfcEmisor, rsEmisor, TpRegimen)
        'c.AgregaDatosReceptor(rfcReceptor, rsReceptor, c_UsoCFDI.Otros, CFDI33.c_Pais.Mexico)
        EasyOne_cfdi33_SetReceptor(c, rfcReceptor, rsReceptor, "", "", "CP01")

        EasyOne_cfdi33_SetReceptor40(c, cCP, RegimenFiscal)

        Fila("Rcp") = cCP
        Fila("Rrfc") = rfcReceptor
        Fila("Rrazonsoc") = rsReceptor
        Fila("Rdireccion") = ""
        Fila("Rregimen") = RegimenFiscal & "-" & DesRegimen
        Fila("usocfdi") = "CP01"
        Fila("moneda") = "XXX"
        Fila("rcomdir") = cComDir
        Fila("nocuenta") = txtNumCuenta.Text

        Dim Tabla2 As DataTable = DS.Tables.Add("PAGOS")
        Tabla2.Columns.Add("FechPago")
        Tabla2.Columns.Add("CFDIpricipal")
        Tabla2.Columns.Add("Serie")
        Tabla2.Columns.Add("Folio")
        Tabla2.Columns.Add("SaldoAnt")
        Tabla2.Columns.Add("Importe")
        Tabla2.Columns.Add("SaldoInsoluto")
        Tabla2.Columns.Add("RFCBeneficiario")
        Tabla2.Columns.Add("NoCtaBenef")
        Tabla2.Columns.Add("RFCOrdenante")
        Tabla2.Columns.Add("BancoOrd")
        Tabla2.Columns.Add("NoCtaOrd")
        Dim Fila2 As DataRow

        Dim Iva As Double
        Iva = Format((Importe.Value * 0.16), "######.00")

        EasyOne_cfdi33_AddConcepto(c, "84111506", "", "1", "ACT", "", "Pago", 0, 0, "")

        EasyOne_cfdi33_SetConcepto40(c, "01")

        Dim p As Integer = EasyOne_pago10_Create()

        EasyOne_pago10_SetVersion(p, "2.0")

        upb.Value += 10 '20
        'c.AgregaConcepto(1, "", "ACT", "84111506", "", "Pago", 0)

        'c.Conceptos(0).Impuestos = Nothing 'al primer y unico concepto, quitale los impuestos.

        ''Paso 1: crea un objeto de complemento
        'Dim comp As New Complemento.Pagos10.Pagos
        'Dim pago As New Complemento.Pagos10.PagosPago
        'Dim docRelacionado As New Complemento.Pagos10.PagosPagoDoctoRelacionado

        ''la factura por la que se recibe el pago
        'With docRelacionado
        '    .ImpSaldoAnt = Saldo.Value
        '    .ImpSaldoAntSpecified = True
        '    .ImpPagado = Importe.Value
        '    .ImpPagadoSpecified = True
        '    .ImpSaldoInsoluto = (Saldo.Value - Importe.Value)
        '    .ImpSaldoInsolutoSpecified = True
        '    .IdDocumento = txtUUID.Text
        '    .Serie = txtSerie.Text
        '    .Folio = txtFactura.Text
        '    .MonedaDR = Complemento.Pagos10.c_Moneda.MXN
        '    .MetodoDePagoDR = Complemento.Pagos10.c_MetodoPago.PagoEnUnaSolaExhibicion_PUE
        'End With

        Select Case grbformaPago.Value
            Case "01"
                FormPago = "EFECTIVO" 'CFDI.CFDI33.c_FormaPago.Efectivo_01
            Case "02"
                FormPago = "CHEQUE"'CFDI.CFDI33.c_FormaPago.Cheque_02
            Case "03"
                FormPago = "TRANSFERENCIA" 'CFDI.CFDI33.c_FormaPago.Transferencia_03
            Case "04"
                FormPago = "TARJETA DE CRÉDITO" 'CFDI.CFDI33.c_FormaPago.TarjetaCredito_04
            Case "05"
                FormPago = "MONEDERO ELECTRÓNICO" ' CFDI.CFDI33.c_FormaPago.MonederoElectronico_05
            Case "06"
                FormPago = "DINERO ELECTRÓNICO" 'CFDI.CFDI33.c_FormaPago.DineroElectronico_06
            Case "08"
                FormPago = "VALES DE DESPENSA" 'CFDI.CFDI33.c_FormaPago.ValesDespensa_08
            Case "12"
                FormPago = "DACION EN PAGO" 'CFDI.CFDI33.c_FormaPago.DacionEnPago_12
            Case "13"
                FormPago = "PAGO POR SUBROGACIÓN" 'CFDI.CFDI33.c_FormaPago.PagoPorSubrogacion_13
            Case "14"
                FormPago = "PAGO POR CONSIGNACIÓN" 'CFDI.CFDI33.c_FormaPago.PagoPorConsignacion_14
            Case "15"
                FormPago = "CONDONACIÓN" 'CFDI.CFDI33.c_FormaPago.Condonacion_15
            Case "17"
                FormPago = "COMPENSACIÓN" 'CFDI.CFDI33.c_FormaPago.Compensacion_17
            Case "23"
                FormPago = "NOVACIÓN" ' CFDI.CFDI33.c_FormaPago.Novacion_23
            Case "24"
                FormPago = "CONFUSIÓN" ' CFDI.CFDI33.c_FormaPago.Confusion_24
            Case "25"
                FormPago = "REMISION DEUDA" 'CFDI.CFDI33.c_FormaPago.RemisionDeuda_25
            Case "26"
                FormPago = "PRESCRIPCIÓN CADUCIDAD" 'CFDI.CFDI33.c_FormaPago.PrescripcionCaducidad_26
            Case "27"
                FormPago = "SATISFACCION ACREADOR" 'CFDI.CFDI33.c_FormaPago.SatisfaccionAcreedor_27
            Case "28"
                FormPago = "TARJETA DE DÉBITO" 'CFDI.CFDI33.c_FormaPago.TarjetaDebito_28
            Case "29"
                FormPago = "TARJETA DE SERVICIO" 'CFDI.CFDI33.c_FormaPago.TarjetaServicio_29
            Case "30"
                FormPago = "NA" 'CFDI.CFDI33.c_FormaPago.NA_98
            Case "99"
                FormPago = "OTROS" 'CFDI.CFDI33.c_FormaPago.Otros_99
        End Select

        Fila("metpago") = "PUE"
        Fila("tipocomp") = "P - PAGO"
        Fila("formpago") = FormPago

        'datos del pago
        'fechapag = Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss")
        'Aqui la informacion de un pago, en un mismo pago nos pueden pagar varios documentos relacionados
        'Fecha de Pago, FormaDePagoP, MonedaP, TipoCambioP, Monto, NumOperacion, RfcEmisorCtaOrd, NomBancoOrdExt, CtaOrdenante, RfcEmisorCtaBen, CtaBeneficiario, TipoCadPago, CertPago, CadPago, SelloPago
        If grbformaPago.Text = "Efectivo" Then
            EasyOne_pago10_AddPago(p, FecPAgo, grbformaPago.Value, "MXN", "01", Importe.Value, txtNumOperacion.Text, "", "", "", "", "", "", "", "", "")
        Else
            EasyOne_pago10_AddPago(p, FecPAgo, grbformaPago.Value, "MXN", "01", Importe.Value, txtNumOperacion.Text, rfcEmisor, grbBancos.Text, txtCtaOrdenante.Text, rfcReceptor, txtCtaBeneficiario.Text, "", "", "", "")
        End If
        If grbformaPago.Text <> "Efectivo" Then
            If txtCtaOrdenante.Text = "" Then
                Me.txtCtaBeneficiario.Text = ""
                Me.txtCtaOrdenante.Text = ""
                Me.grbBancos.Text = ""
            End If
        End If
        upb.Value += 10 '30
        'With pago
        '    .FechaPago = Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss")
        '    REM .FormaDePagoP = Complemento.Pagos10.c_FormaPago.Item02
        '    .FormaDePagoP = FormPago
        '    .MonedaP = Complemento.Pagos10.c_Moneda.MXN
        '    .Monto = Importe.Value

        '    If grbformaPago.Text <> "Efectivo" Then
        '        .NumOperacion = txtNumOperacion.Text

        '        If txtCtaOrdenante.Text = "" Then
        '            Me.txtCtaBeneficiario.Text = ""
        '            Me.txtCtaOrdenante.Text = ""
        '            Me.grbBancos.Text = ""
        '        Else
        '            .RfcEmisorCtaOrd = rfcReceptor
        '            .RfcEmisorCtaBen = rfcEmisor
        '            .NomBancoOrdExt = grbBancos.Text
        '            .CtaOrdenante = txtCtaOrdenante.Text
        '            .CtaBeneficiario = txtCtaBeneficiario.Text
        '        End If
        '    End If

        '.DoctoRelacionado = New List(Of Complemento.Pagos10.PagosPagoDoctoRelacionado)
        '    .DoctoRelacionado.Add(docRelacionado)
        'End With

        ''agrega los datos del pago al complemento
        'With comp
        '    .Pago = New List(Of Complemento.Pagos10.PagosPago)
        '    .Pago.Add(pago)
        'End With

        Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
        Fila2("FechPago") = FecPAgo
        Fila2("CFDIpricipal") = txtUUID.Text
        Fila2("Serie") = txtSerie.Text
        Fila2("Folio") = txtFactura.Text
        Fila2("SaldoAnt") = Saldo.Value
        Fila2("Importe") = Importe.Value
        Fila2("SaldoInsoluto") = Format((Saldo.Value - Importe.Value), "###,###.00")
        If grbformaPago.Text <> "Efectivo" Then
            Fila2("RFCBeneficiario") = rfcReceptor
            Fila2("NoCtaBenef") = txtCtaBeneficiario.Text
            Fila2("RFCOrdenante") = rfcEmisor
            Fila2("BancoOrd") = grbBancos.Text
            Fila2("NoCtaOrd") = txtCtaOrdenante.Text
        End If
        DS.Tables(1).Rows.Add(Fila2)

        Dim SdoInsoluto As Double = 0
        Dim totaliva, totalsiniva As Double
        'totaliva = Format((Importe.Value + Iva), "###,###.00")
        'totalsiniva = Format((Importe.Value - Iva), "###,###.00")
        SdoInsoluto = Format((Saldo.Value - Importe.Value), "###,###.00")

        ''Informacion del documento relacionado
        ''En este caso es un pago parcial de un documento, recordemos que en un pago puede haber uno o muchos documentos relacionados
        ''IdDocumento, serie, Folio, MonedaDR, TipoCambioDR(No se usa en Pagos 2.0), MetodoDePagoDR(No se usa en Pagos 2.0), NumParcialidad, ImpSaldoAnt, ImpPagado, ImpSaldoInsoluto
        EasyOne_pago10_AddDoctoRelacionado(p, txtUUID.Text, txtSerie.Text, txtFactura.Text, "MXN", "", "", "1", Saldo.Value, Importe.Value, SdoInsoluto)

        'Reforma Fiscal 2022 Informacion de Pagos 2.0
        ''EquivalenciaDR, ObjetoImpuestoDR
        EasyOne_pago10_SetDoctoRelacionado20(p, "1", "02")

        ''Paso 2: asigna el contenido del objeto complemento al objeto concepto (hay que asignarselo a un objeto de tipo ComprobanteComplemento primero)
        'Dim oC As New CFDI33.ComprobanteComplemento
        'oC.Any = comp.ToXMLElementList
        ''c.Complemento.Add(oC)
        upb.Value += 10  'Suma 40

        'Paso 3: agrega los namespaces (XML) que utiliza el complemento al comprobante
        'nota: este ultimo paso solo se requiere ejecutar UNA SOLA VEZ
        'c.Namespaces.Add(comp.Prefix, comp.Namespace)
        'c.Schemas.Add(comp.Namespace, comp.SchemaLocation)

        'Agregamos el iva del documento relacionado que nos estan pagando
        'BaseDR, ImpuestoDR, TipoFactorDR, TasaOCuotaDR, ImporteDR

        EasyOne_pago10_AddTrasladoDR20(p, Importe.Value, "002", "Tasa", "0.160000", Iva)

        ''Informacion Global del Pago - Aqui tenemos que poner un resumen de los traslados del documento relacionado
        'BaseP, ImpuestoP, TipoFactorP, TasaOCuotaP, ImporteP
        EasyOne_pago10_AddTrasladoP20(p, Importe.Value, "002", "Tasa", "0.160000", Iva)

        ''Informacion Global de totales del Pago - Aqui se debe registrar la suma de todos los importes y bases de todos los pagos
        'TotalRetencionesIVA, TotalRetencionesISR, TotalRetencionesIEPS, TotalTrasladosBaseIVA16, TotalTrasladosImpuestoIVA16, TotalTrasladosBaseIVA8, TotalTrasladosImpuestoIVA8, TotalTrasladosBaseIVA0, TotalTrasladosImpuestoIVA0, TotalTrasladosBaseIVAExento, MontoTotalPagos

        EasyOne_pago10_SetTotales20(p, "", "", "", Importe.Value, Iva, "", "", "", "", "", Importe.Value)

        ''Se agrega el complemento al comprobante
        EasyOne_AddComplemento(c, p)

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        TablaTienda = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & frmComplementoPagos.grbTienda.Value & " and Sucursal.sid = " & frmComplementoPagos.grbSucursal.Value & " ", As400).Tables(0)

        DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
        PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
        Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
        PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
        SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
        PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
        DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
        AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
        MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString

        'Dim Directorios As String = TablaTienda.Rows(0).Item("sUbic").ToString
        'Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
        'Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
        'Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
        'Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
        'Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
        'Dim RutPagos = TablaTienda.Rows(0).Item("sForm4").ToString
        'Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        'Dim FechaFactura As String = Date.Now.ToString.Substring(0, 10)
        'Dim RFacImpresas As String
        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)


        'Dim ValorTicket As String = ""
        'Dim NumPiezas As Integer = 0
        'If banSolCuenta = True Then
        '    NoCuenta = txtNumCuenta.Text
        'Else
        '    NoCuenta = ""
        'End If
        'Dim DatosImpresionExtra As New Hashtable
        'DatosImpresionExtra.Add("NumTicket", NumPago)
        'DatosImpresionExtra.Add("TotPiezas", NumPiezas)
        'DatosImpresionExtra.Add("FP", grbformaPago.Text)
        'DatosImpresionExtra.Add("NoCuenta", NoCuenta)
        'DatosImpresionExtra.Add("DesRegimen", DesRegimen)


        ''Dim motor As New CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
        'EasyOne_SetInvoiceOneFachadaInfoPrueba(c, "RHUUMRGL", "RHUUMRGL")
        EasyOne_SetInvoiceOneFachadaInfo(c, "RHUUMRGL", "RHUUMRGL")

        ''Licenciamiento.AgregaLicencia("NxfUWVt27sSyPaPK/VSpaM1KxObdAq1pQgX7SmJWTEWljBDrg2XqOSmxSth7pZ6FRsViFC+MzuubR0AfCl1wgYiKnOWpT6XWJMXAy58SbcE=")

        'c.Impuestos = Nothing 'quita los impuestos

        Dim x As Integer = EasyOne_FirmaComprobante(c, CerFactura, Key, Pwd)
        Application.DoEvents()

        'If Not resultado.Errores Then
        If x = 0 Then
            Application.DoEvents()
            Dim resultadoXML As String = [String].Empty
            resultadoXML = EasyOne_LeeResultado(c, 8)
            'Obtenemos el XML completo y timbrado
            Console.WriteLine(resultadoXML)

            Application.DoEvents()
            IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"), resultadoXML)

            '''''''''''''''''''''''''''''' RECORRO XML''''''''''''''''''''''''''''
            Dim Version_xml As String
            'Dim VarConceptos As XmlNodeList
            Dim version As String
            Dim uuid As String
            Dim ftimb As String
            Dim rfcprov As String
            Dim sellocfd As String
            Dim nocert As String

            'Dim nav As XPathNavigator
            Dim documento As XmlDocument = New XmlDocument()
            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
            documento.Load(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"))
            VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
            Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
            upb.Value += 10 '50
            Dim Estatus As String = 1
            If Version_xml = "4.0" Then
                Fila("NoDoc") = documento.SelectSingleNode("/cfdi:Comprobante/@Folio", VarManager).InnerText
                Fila("sellodig") = documento.SelectSingleNode("/cfdi:Comprobante/@Sello", VarManager).InnerText
                Fila("lugaremi") = documento.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", VarManager).InnerText
                Fila("fechaemi") = documento.SelectSingleNode("/cfdi:Comprobante/@Fecha", VarManager).InnerText
                Fila("serieceremi") = documento.SelectSingleNode("/cfdi:Comprobante/@NoCertificado", VarManager).InnerText
                uuid = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", VarManager).InnerText
                Fila("foliofisc") = uuid
                rfcprov = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", VarManager).InnerText
                Fila("provcert") = rfcprov
                Fila("sellosat") = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloSAT", VarManager).InnerText
                nocert = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@NoCertificadoSAT", VarManager).InnerText
                Fila("certSAT") = nocert
                ftimb = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", VarManager).InnerText
                Fila("hryfechcer") = ftimb
                version = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@Version", VarManager).InnerText
                sellocfd = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloCFD", VarManager).InnerText
                Fila("complesat") = "|" + version + "|" + uuid + "|" + ftimb + "|" + rfcprov + "||" + sellocfd + "|" + nocert + "||"

                RutaLogo = rutlogo 'RutFacGlobal & "\RecursosCFD33\Penguin.bmp"
                RutaQR = Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml.png")

                '''''''nuevoooooooooooooooooooooooooooooooooooooooooo

                DS.Tables(0).Rows.Add(Fila)
                Dim objreport As New Pagos
                objreport.SetParameterValue("Logo", RutaLogo)
                objreport.SetParameterValue("QR", RutaQR)
                Dim rpFact As New Pagos
                rpFact.SetDataSource(DS)
                rpFact.SetParameterValue("QR", RutaQR)
                rpFact.SetParameterValue("Logo", RutaLogo)
                ComPago.CrystalReportViewer1.ReportSource = objreport
                ComPago.CrystalReportViewer1.ReportSource = rpFact
                ComPago.WindowState = FormWindowState.Maximized
                ComPago.Show()

                upb.Value += 10 'Suma 60
                rutaArchivoPDF = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".pdf"
                rutaArchivoXML = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                Dim cryRpt As New ReportDocument

                Try
                    Dim CrExportOptions As ExportOptions
                    Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                    Dim crf As New PdfRtfWordFormatOptions()
                    CrDiskFileDestinationOptions.DiskFileName = rutaArchivoPDF
                    CrExportOptions = rpFact.ExportOptions
                    With CrExportOptions
                        .ExportDestinationType = ExportDestinationType.DiskFile
                        .ExportFormatType = ExportFormatType.PortableDocFormat
                        .DestinationOptions = CrDiskFileDestinationOptions
                        .FormatOptions = CrFormatTypeOptions
                    End With
                    rpFact.Export()
                    upb.Value += 10 'Suma 70  
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

                REM *********************************************************************************************
                objpro.Indicacion("update bgxfacelec.serie set secant = " & CInt(NumPago) & " where tid = " & frmComplementoPagos.grbTienda.Value & " and sid = " & frmComplementoPagos.grbSucursal.Value & " and seserie = '" & serie & "' ", As400)
                objpro.Indicacion("update bgxfacelec.fencabez set feabonos = feabonos + " & Importe.Value & " where feid = " & P_Id & " and feterm = " & P_Term & " and tid = " & P_Tid & " and sid = " & P_Suc & " and feserie = '" & P_Serie & "' and fefact = " & P_Fact & " ", As400)
                REM *********************************************************************************************

                upb.Value += 80 'Suma 90

                Dim correo As New System.Net.Mail.MailMessage()
                correo.From = New System.Net.Mail.MailAddress("gladysperezg19@gmail.com") 'DirEmail
                correo.To.Add(Trim(Me.txtEmail.Text))

                correo.Subject = Trim(AsuntoEmail)
                correo.Body = Trim(MsjEmail)
                correo.Body += Environment.NewLine & Environment.NewLine
                correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
                correo.Body += Environment.NewLine & "BOSCO. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                correo.Body += " ó a través de una solicitud vía correo electrónico a hfortiz@grupovigil.com.mx"

                correo.IsBodyHtml = False
                correo.Priority = System.Net.Mail.MailPriority.Normal
                Dim Archivo As New System.Net.Mail.Attachment(rutaArchivoPDF)
                For i = 0 To 1
                    If i = 0 Then
                        Archivo = New System.Net.Mail.Attachment(rutaArchivoPDF)
                        upb.Value += 5 'Suma 85
                    Else
                        Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                        upb.Value += 5 'Suma 90
                    End If
                    correo.Attachments.Add(Archivo)
                Next
                Dim smtp As New System.Net.Mail.SmtpClient
                smtp.Host = SmtpHost
                smtp.Port = Int(PuertoSmtp)
                smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                smtp.EnableSsl = False
                Application.DoEvents()
                smtp.Send(correo)
                upb.Value += 10 'Suma 100
                Application.DoEvents()
                MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                'BorrarRegistro()
                'DatClientes()
            Else
                MsgBox("Archivo XML no es una factura version 4.0")
            End If 'Cierra validación de operación exitosa
        Else
            'En caso de error mostrar el detalle.
            Dim resultadoXML2 As String = [String].Empty
            resultadoXML2 = EasyOne_LeeResultado(c, 11)
            Console.WriteLine(resultadoXML2)
            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
            'For Each cNG As CFDI.CFDI33.Comprobante In resultado.ComprobantesNoGenerados
            '    MessageBox.Show(cNG.ErrorGeneracion.Message)
            'Next
        End If 'Cierra validación generación de XML

        'motor.Comprobantes.Add(c)
        'upb.Value += 20  'Suma 50
        ''Dim resultado As CFDI33.ResultadoProceso = motor.GenerarArchivosPAC(True, False)
        'If Not resultado.Errores Then
        '    Application.DoEvents()
        '    Dim svcNew As New fel.WSCFDI33Client
        '    Dim Respuesta As New fel.RespuestaTFD33
        '    svcNew.Open()
        '    Dim ReferenciaPac As String = Trim(rfcEmisor) & "-" & serie & "-" & NumPago
        '    'Respuesta = svcNew.TimbrarCFDI(Trim(UserPAC), Trim(PwdPAC), IO.File.ReadAllText(c.ArchivoXMLSellado), Trim(ReferenciaPac))
        '    svcNew.Close()

        '    Dim archivoREPX As String
        '    Dim archivoPDF As String
        '    If Respuesta.OperacionExitosa Then
        '        'If IO.File.Exists(c.ArchivoXMLSellado) Then
        '        '    Application.DoEvents()
        '        '    upb.Value += 10  'Suma 60
        '        '    Dim archivoXMLPDF As String
        '        '    IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"), Respuesta.XMLResultado)
        '        '    CFDI33.Generar.ArchivoCadenaTFD(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml")
        '        '    CFDI33.Generar.ArchivoCBB(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml")
        '        '    CFDI33.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml")
        '        '    CFDI33.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml", Componentes.CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", Componentes.CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
        '        '    Dim NombrePDF As String = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
        '        '    archivoXMLPDF = CFDI33.Generar.ArchivoDatosPDF(Trim(NombrePDF), Componentes.CFDI.Globales.Capitalizacion.PrimeraLetraPalabra, "", Componentes.CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)

        '        '    upb.Value += 10  'Suma 70
        '        '    Try
        '        '        Dim RutReporte As String
        '        '        RutReporte = RutPagos & "\RecursosCFD33"
        '        '        archivoREPX = PDF.Generar.ArchivoReporteBase(RutReporte, False)
        '        '        archivoPDF = PDF.Generar.ArchivoPDF2(archivoREPX, archivoXMLPDF)
        '        '        upb.Value += 10  'Suma 80
        '        '        Application.DoEvents()
        '        '        rutaArchivoPDF = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".pdf"
        '        '        rutaArchivoXML = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
        '        '    Catch ex As Exception
        '        '        MsgBox(ex.Message.ToString)
        '        '    End Try

        '        '    REM *********************************************************************************************
        '        '    objpro.Indicacion("update bgxfacelec.serie set secant = " & CInt(NumPago) & " where tid = " & frmComplementoPagos.grbTienda.Value & " and sid = " & frmComplementoPagos.grbSucursal.Value & " and seserie = '" & serie & "' ", As400)
        ''objpro.Indicacion("update bgxfacelec.fencabez set feabonos = feabonos + " & Importe.Value & " where feid = " & P_Id & " and feterm = " & P_Term & " and tid = " & P_Tid & " and sid = " & P_Suc & " and feserie = '" & P_Serie & "' and fefact = " & P_Fact & " ", As400)
        '        '    REM *********************************************************************************************

        '        '    Dim correo As New System.Net.Mail.MailMessage()
        '        '    correo.From = New System.Net.Mail.MailAddress(DirEmail)
        '        '    correo.To.Add(Trim(Me.txtEmail.Text))

        '        '    correo.Subject = Trim(AsuntoEmail)
        '        '    correo.Body = Trim(MsjEmail)
        '        '    correo.Body += Environment.NewLine & Environment.NewLine
        '        '    correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
        '        '    correo.Body += Environment.NewLine & "GRUPO VIGIL. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
        '        '    correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
        '        '    correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
        '        '    correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
        '        '    correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
        '        '    correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
        '        '    correo.Body += " ó a través de una solicitud vía correo electrónico a boutique@grupovigil.com.mx"

        '        '    correo.IsBodyHtml = False
        '        '    correo.Priority = System.Net.Mail.MailPriority.Normal
        '        '    Dim Archivo As New System.Net.Mail.Attachment(rutaArchivoPDF)
        '        '    For i = 0 To 1
        '        '        If i = 0 Then
        '        '            Archivo = New System.Net.Mail.Attachment(rutaArchivoPDF)
        '        '            upb.Value += 5 'Suma 85
        '        '        Else
        '        '            Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
        '        '            upb.Value += 5 'Suma 90
        '        '        End If
        '        '        correo.Attachments.Add(Archivo)
        '        '    Next
        '        '    Dim smtp As New System.Net.Mail.SmtpClient
        '        '    smtp.Host = SmtpHost
        '        '    smtp.Port = Int(PuertoSmtp)
        '        '    smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
        '        '    smtp.EnableSsl = SeguridadEmail


        '        '    Inicializa()
        '        '    upb.Value += 10  'Suma 100
        '        '    APDF(archivoPDF)

        '        'End If 'Valida si se sella el XML
        '    Else
        '        'Dim sll = c.ArchivoSello
        '        'Dim txt = sll.ToString.Replace(".sll", ".txt")
        '        'Dim xml = sll.ToString.Replace(".sll", ".xml")
        '        'IO.File.Delete(sll) : IO.File.Delete(txt) : IO.File.Delete(xml)
        '        MsgBox(Respuesta.CodigoRespuesta.ToString & " " & Respuesta.MensajeError.ToString, MsgBoxStyle.Information, "Facturación .. ")
        '    End If 'Cierra validación de lo que dice el PAC
        'Else
        '    For Each cNG As CFDI33.Comprobante In resultado.ComprobantesNoGenerados
        '        MessageBox.Show(cNG.ErrorGeneracion.Message)
        '    Next
        'End If 'Cierra validación de Error de generar XML

    End Sub

    Private Sub Inicializa()
        txtUUID.Text = ""
        txtFactura.Text = ""
        txtSerie.Text = ""
        txtFechaFactura.Value = Now.Date
        Total.Value = 0.0
        Abono.Value = 0.0
        Saldo.Value = 0.0        
        grbformaPago.Text = ""
        txtNumCuenta.Text = ""        
        txtNumOperacion.Text = ""
        txtCtaBeneficiario.Text = ""
        grbBancos.Text = ""
        txtCtaOrdenante.Text = ""
        Importe.Value = 0.0
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
    End Sub

    Public Function APDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            REM .DisplayLayout.Bands(0).Columns(1).Width = 250
        End With
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "bgxfacelec.satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", As400) = True Then
            banSolCuenta = True
            verNumCta(True)
            verEmpresa(True)
            Me.txtNumCuenta.Focus()
        Else
            banSolCuenta = False
            verNumCta(False)
            verEmpresa(True)
            btnClientes.Focus()
        End If

    End Sub

    'Private Sub btnusocfdi_click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
    '    formulario(satusocfdibosco)
    '    txtclvusocfdi.text = cfdiuso
    '    txtusocfdi.text = cfdides
    'End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub txtNumOperacion_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNumOperacion.KeyPress
        If e.KeyChar = Chr(13) Then
            txtCtaBeneficiario.Focus()
        End If
    End Sub

    Private Sub txtCtaBeneficiario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCtaBeneficiario.KeyPress
        If e.KeyChar = Chr(13) Then
            grbBancos.Focus()
        End If
    End Sub

    Private Sub grbBancos_TextChanged(sender As Object, e As EventArgs) Handles grbBancos.TextChanged
        If grbBancos.Text <> "" Then
            txtCtaOrdenante.Focus()
        End If
    End Sub

    Private Sub txtCtaOrdenante_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCtaOrdenante.KeyPress
        If e.KeyChar = Chr(13) Then
            Importe.Focus()
        End If
    End Sub

    Private Sub Importe_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Importe.KeyPress
        If e.KeyChar = Chr(13) Then
            btnAplicarPago.Focus()
        End If
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub verFormaPago(opcion As Boolean)
        lblMetodoPago.Visible = opcion
        grbformaPago.Visible = opcion
    End Sub

    Private Sub verEmpresa(opcion As Boolean)
        lblEmpresa.Visible = opcion
        lblrfc.Visible = opcion
        lblrasoncial.Visible = opcion
        lblEmail.Visible = opcion
        txtEmail.Visible = opcion
        txtRazonSocial.Visible = opcion
        txtRfc.Visible = opcion
        btnClientes.Visible = opcion
    End Sub

    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        idCliente = 0
        Formulario(frmClientesBosco)
        If idCliente <> 0 Then
            txtRfc.Text = objpro.Dato("crfc", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtRazonSocial.Text = objpro.Dato("crsocial", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtEmail.Text = objpro.Dato("cemail", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            cCP = objpro.Dato("cCP", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            If cCP.Trim.Length = 4 Then
                cCP = "0" & cCP
            End If
            cfdiuso = objpro.Dato("cusocfdi", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            RegimenFiscal = regimenFisc
            DesRegimenFiscal = regimen
            DesCFDI = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)

            Me.gpoDatos.Visible = True
            Me.txtNumOperacion.Focus()
        End If
    End Sub

End Class