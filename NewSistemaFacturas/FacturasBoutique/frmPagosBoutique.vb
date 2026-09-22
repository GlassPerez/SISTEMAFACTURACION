Imports System
Imports N400
Imports N400.Componentes.CFDI
Imports N400.Componentes.CFDI.PDF
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Threading

Public Class frmPagosBoutique

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Variables"
    Dim TIP As String
    Dim TablaTienda As DataTable
    Dim lugarPago As String
    Dim banSolCuenta As Boolean
    Dim fp As String
    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim TRFiscal As String
    Dim TpRegimen As String
    Dim Uso_CFDI As String

    Dim RF As String
    Dim RFD As String
    Dim cCP As Integer
    Dim cDireccion As String
    Dim cComDir As String
    Dim va As Integer
    Dim hra As String
    Dim seriee, folio As String
    Dim fechareal, FeRegistro, FechaHora, ExisteClave, invoice As String
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

    Private Sub frmPagosBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "PA"
        verNumCta(False)
        verFormaPago(True)
        verEmpresa(False)
        txtUUID.Text = frmComplementoPagoBoutique.txtUUID.Text
        txtFactura.Text = frmComplementoPagoBoutique.txtFactura.Text
        txtSerie.Text = frmComplementoPagoBoutique.txtSerie.Text
        txtFechaFactura.Value = frmComplementoPagoBoutique.txtFechaFactura.Value
        Total.Value = frmComplementoPagoBoutique.Total.Value
        Abono.Value = frmComplementoPagoBoutique.Abono.Value
        Saldo.Value = frmComplementoPagoBoutique.Saldo.Value
        fechaPago.Value = Now.Date
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from satfpag ", 0, 1, Motor)
        grbformaPago.Value = "01"
        Application.DoEvents()
        idCliente = 0
        'Formulario(frmClientesBoutique)
        Dim refGlobal As Data.DataTable
        refGlobal = objpro.ObtenDatos("SELECT cid FROM catclie WHERE crfc = 'XAXX010101000' ", Motor).Tables(0)
        If refGlobal.Rows.Count <> 0 Then

            For Each j In refGlobal.Rows
                Application.DoEvents()
                idCliente = Int(j("cid").ToString)
            Next
        End If

        If idCliente <> 0 Then
            txtRfc.Text = objpro.Dato("crfc", "catclie", "cid = " & idCliente & "", Motor)
            txtRazonSocial.Text = objpro.Dato("crsocial", "catclie", "cid = " & idCliente & "", Motor)
            txtEmail.Text = objpro.Dato("cemail", "catclie", "cid = " & idCliente & "", Motor)
            cfdiuso = objpro.Dato("cusocfdi", "catclie", "cid = " & idCliente & "", Motor)
            'RF = regimenFisc
            'RFD = regimen
            cCP = objpro.Dato("cCP", "catclie", "cid = " & idCliente & "", Motor)
            Me.Importe.Value = Me.Saldo.Value
            'Me.gpoDatos.Visible = True
            'Me.txtNumOperacion.Focus()
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
        'btnClientes.Visible = opcion
    End Sub

    Private Sub frmPagosBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", Motor) = True Then
            banSolCuenta = True
            verNumCta(True)
            verEmpresa(True)
            Me.txtNumCuenta.Focus()
        Else
            banSolCuenta = False
            verNumCta(False)
            verEmpresa(True)
            'btnClientes.Focus()
        End If
    End Sub

    'Private Sub btnClientes_Click(sender As Object, e As EventArgs)
    '    idCliente = 0
    '    'Formulario(frmClientesBoutique)
    '    Dim refGlobal As Data.DataTable
    '    refGlobal = objpro.ObtenDatos("SELECT cid FROM catclie WHERE crfc = 'XAXX010101000' ", Motor).Tables(0)
    '    If refGlobal.Rows.Count <> 0 Then

    '        For Each j In refGlobal.Rows
    '            Application.DoEvents()
    '            idCliente = Int(j("cid").ToString)
    '        Next
    '    End If

    '    If idCliente <> 0 Then
    '        txtRfc.Text = objpro.Dato("crfc", "catclie", "cid = " & idCliente & "", Motor)
    '        txtRazonSocial.Text = objpro.Dato("crsocial", "catclie", "cid = " & idCliente & "", Motor)
    '        txtEmail.Text = objpro.Dato("cemail", "catclie", "cid = " & idCliente & "", Motor)
    '        cfdiuso = objpro.Dato("cusocfdi", "catclie", "cid = " & idCliente & "", Motor)
    '        RF = regimenFisc
    '        RFD = regimen
    '        cCP = objpro.Dato("cCP", "catclie", "cid = " & idCliente & "", Motor)
    '        'Me.gpoDatos.Visible = True
    '        'Me.txtNumOperacion.Focus()
    '    End If
    'End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    'Private Sub grbBancos_TextChanged(sender As Object, e As EventArgs)
    '    If grbBancos.Text <> "" Then
    '        txtCtaOrdenante.Focus()
    '    End If
    'End Sub

    'Private Sub txtNumOperacion_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If e.KeyChar = Chr(13) Then
    '        txtCtaBeneficiario.Focus()
    '    End If
    'End Sub

    'Private Sub txtCtaBeneficiario_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If e.KeyChar = Chr(13) Then
    '        grbBancos.Focus()
    '    End If
    'End Sub

    'Private Sub txtCtaOrdenante_KeyPress(sender As Object, e As KeyPressEventArgs)
    '    If e.KeyChar = Chr(13) Then
    '        Importe.Focus()
    '    End If
    'End Sub

    Private Sub Importe_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Importe.KeyPress
        If e.KeyChar = Chr(13) Then
            btnAplicarPago.Focus()
        End If
    End Sub

    Public Function APDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function


    Private Sub btnAplicarPago_Click(sender As Object, e As EventArgs) Handles btnAplicarPago.Click
        If txtUUID.Text <> "" Then
            If Importe.Value > 0 Then
                If MsgBox("Seguro de realizar pago .. ", MsgBoxStyle.OkCancel, "Facturación .. ") = MsgBoxResult.Ok Then
                    If Importe.Value <= Saldo.Value Then
                        Dim sumSaldo As Double = 0
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
        Dim FecPAgo As String = a & "-" & m & "-" & d & "T12:00:00"

        Dim serie As String = objpro.Dato("seserie", "Serie", "sId = " & frmComplementoPagoBoutique.grbSucursal.Value & " and Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)
        Dim NumPago As String = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & frmComplementoPagoBoutique.grbSucursal.Value & " and Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")

        TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and Sucursal.sid = " & frmComplementoPagoBoutique.grbSucursal.Value & " ", Motor).Tables(0)

        DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
        PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
        Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
        PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
        SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
        PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
        DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
        AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
        MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString
        invoice = Trim(TablaTienda.Rows(0).Item("tuserpac").ToString)

        Dim Directorios, CarpTienda, CarpSucursal As String
        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, rutlogo As String
        Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
        Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
        Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
        Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
        Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
        Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
        Dim RutPagos = TablaTienda.Rows(0).Item("sForm4").ToString
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim FechaFactura As String = fechaPago.Value.ToString.Substring(0, 10) 'Date.Now.ToString.Substring(0, 10)
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
        TRfc = TablaTienda.Rows(0).Item("trfc").ToString
        TCp = TablaTienda.Rows(0).Item("tcp").ToString
        TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
        direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
        CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
        CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
        rutlogo = TablaTienda.Rows(0).Item("rutLogo").ToString '"\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png"
        Dim RFacImpresas As String
        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

        Dim rfcEmisor As String = objpro.Dato("trfc", "cattien", "Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " ", Motor)
        Dim rsEmisor As String = objpro.Dato("trsocial", "cattien", "Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " ", Motor)
        lugarPago = objpro.Dato("selugarexp", "Serie", "sId = " & frmComplementoPagoBoutique.grbSucursal.Value & " and Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)

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
        Tabla1.Columns.Add("fechVta")

        Dim Fila As DataRow

        Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
        Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
        Fila("Erazonsoc") = TRazonSocial
        Fila("Eregfis") = TRFiscal + " - " + descreg.Trim
        Fila("Edireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
        Fila("Edircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString
        Fila("exporta") = "01"
        upb.Value += 10 'Inicia valor

        TRFiscal = objpro.Dato("satregimen", "catsucu", "Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and sid = " & frmComplementoPagoBoutique.grbSucursal.Value & " ", Motor)

        Dim rfcReceptor As String = Trim(txtRfc.Text)
        Dim rsReceptor As String = Trim(txtRazonSocial.Text)

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

        EasyOne_cfdi33_SetEmisor(c, rfcEmisor, rsEmisor, TpRegimen)
        'EasyOne_cfdi33_SetReceptor(c, rfcReceptor, rsReceptor, "", "", "CP01")
        'EasyOne_cfdi33_SetReceptor40(c, cCP, RF)

        EasyOne_cfdi33_SetReceptor(c, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "CP01") 'estos paramentros son si o si para factura global
        EasyOne_cfdi33_SetReceptor40(c, cCP, "616") 'regimen fiscal para global 616

        'Dim FeVta As String = ""

        'FeVta = txtFechaFactura.Text.ToString.Substring(6, 4) & txtFechaFactura.Text.ToString.Substring(3, 2) & txtFechaFactura.Text.ToString.Substring(0, 2)

        Fila("Rcp") = cCP
        Fila("Rrfc") = "XAXX010101000"
        Fila("Rrazonsoc") = "PUBLICO EN GENERAL"
        Fila("Rdireccion") = ""
        Fila("Rregimen") = "616-Sin Obligaciones Fiscales"

        Fila("usocfdi") = "CP01"
        Fila("moneda") = "XXX"
        Fila("rcomdir") = cComDir
        Fila("nocuenta") = txtNumCuenta.Text
        Fila("fechVta") = txtFechaFactura.Text

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

        'Aqui la informacion de un pago, en un mismo pago nos pueden pagar varios documentos relacionados
        'Fecha de Pago, FormaDePagoP, MonedaP, TipoCambioP, Monto, NumOperacion, RfcEmisorCtaOrd, NomBancoOrdExt, CtaOrdenante, RfcEmisorCtaBen, CtaBeneficiario, TipoCadPago, CertPago, CadPago, SelloPago
        'If grbformaPago.Text = "Efectivo" Then
        EasyOne_pago10_AddPago(p, FecPAgo, grbformaPago.Value, "MXN", "01", Format(Importe.Value, "######.00"), "", "", "", "", "", "", "", "", "", "")
        'Else
        '    EasyOne_pago10_AddPago(p, FecPAgo, grbformaPago.Value, "MXN", "01", Format(Importe.Value, "######.00"), txtNumOperacion.Text, rfcReceptor, grbBancos.Text, txtCtaOrdenante.Text, rfcEmisor, txtCtaBeneficiario.Text, "", "", "", "")
        'End If
        'If grbformaPago.Text <> "Efectivo" Then
        '    If txtCtaOrdenante.Text = "" Then
        '        Me.txtCtaBeneficiario.Text = ""
        '        Me.txtCtaOrdenante.Text = ""
        '        Me.grbBancos.Text = ""
        '    End If
        'End If
        upb.Value += 10 '30
        Dim basep As Double = 0
        Dim ImporteP As Double = 0
        Dim subt As Double = 0
        Dim Ivacal As Double = 0
        'For Each s In ugrselect.Rows
        Dim SDOINS As Double = 0
        SDOINS = (Saldo.Value - Importe.Value)

        ''Informacion del documento relacionado
        ''En este caso es un pago parcial de un documento, recordemos que en un pago puede haber uno o muchos documentos relacionados
        ''IdDocumento, serie, Folio, MonedaDR, TipoCambioDR(No se usa en Pagos 2.0), MetodoDePagoDR(No se usa en Pagos 2.0), NumParcialidad, ImpSaldoAnt, ImpPagado, ImpSaldoInsoluto
        If Format(SDOINS, "######.00").Trim = ".00" Then
            EasyOne_pago10_AddDoctoRelacionado(p, txtUUID.Text, txtSerie.Text, txtFactura.Text, "MXN", "", "", "1", Format(Saldo.Value, "######.00"), Importe.Value, 0)
        Else
            EasyOne_pago10_AddDoctoRelacionado(p, txtUUID.Text, txtSerie.Text, txtFactura.Text, "MXN", "", "", "1", Format(Saldo.Value, "######.00"), Importe.Value, Format(SDOINS, "######.00"))
        End If
        subt = (Importe.Value / 1.16)
        Ivacal = Importe.Value - subt

        Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
        Fila2("FechPago") = FecPAgo
        Fila2("CFDIpricipal") = txtUUID.Text
        Fila2("Serie") = txtSerie.Text
        Fila2("Folio") = txtFactura.Text
        Fila2("SaldoAnt") = Format(Saldo.Value, "######.00")
        Fila2("Importe") = Importe.Value
        If Format(SDOINS, "######.00").Trim = ".00" Then
            Fila2("SaldoInsoluto") = 0 'Format((Saldo.Value - Importe.Value), "###,###.00")
        Else
            Fila2("SaldoInsoluto") = Format(SDOINS, "######.00")
        End If

        'If grbformaPago.Text <> "Efectivo" Then
        '    Fila2("RFCBeneficiario") = rfcEmisor
        '    Fila2("NoCtaBenef") = txtCtaBeneficiario.Text
        '    Fila2("RFCOrdenante") = rfcReceptor
        '    Fila2("BancoOrd") = grbBancos.Text
        '    Fila2("NoCtaOrd") = txtCtaOrdenante.Text
        'End If

        DS.Tables(1).Rows.Add(Fila2)

        'Reforma Fiscal 2022 Informacion de Pagos 2.0
        ''EquivalenciaDR, ObjetoImpuestoDR
        EasyOne_pago10_SetDoctoRelacionado20(p, "1", "02")

        'Agregamos el iva del documento relacionado que nos estan pagando
        'BaseDR, ImpuestoDR, TipoFactorDR, TasaOCuotaDR, ImporteDR
        If Ivacal = 0 Then
            'EasyOne_pago10_AddTrasladoDR20(p, Format(s.Cells("subtotal").Value, "######.00"), "002", "Tasa", "0.160000", s.Cells("iva").Value)
            EasyOne_pago10_AddTrasladoDR20(p, Format(subt, "######.00"), "002", "Tasa", "0.160000", Ivacal)
            basep += subt 's.Cells("subtotal").Value
            ImporteP += Ivacal 's.Cells("iva").Value
        Else
            'EasyOne_pago10_AddTrasladoDR20(p, Format(s.Cells("subtotal").Value, "######.00"), "002", "Tasa", "0.160000", Format(s.Cells("iva").Value, "######.00"))
            EasyOne_pago10_AddTrasladoDR20(p, Format(subt, "######.00"), "002", "Tasa", "0.160000", Format(Ivacal, "######.00"))
            basep += subt 's.Cells("subtotal").Value
            ImporteP += Ivacal 's.Cells("iva").Value
        End If

        'Next
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''
        upb.Value += 10  'Suma 40
        Fila("Ftotal") = Format(Importe.Value, "######.00")

        'datos del pago
        Dim fechapag, hr, fpag As String
        fpag = Now

        If ImporteP = 0 Then
            ''Informacion Global de totales del Pago - Aqui se debe registrar la suma de todos los importes y bases de todos los pagos
            'TotalRetencionesIVA, TotalRetencionesISR, TotalRetencionesIEPS, TotalTrasladosBaseIVA16, TotalTrasladosImpuestoIVA16, TotalTrasladosBaseIVA8, TotalTrasladosImpuestoIVA8, TotalTrasladosBaseIVA0, TotalTrasladosImpuestoIVA0, TotalTrasladosBaseIVAExento, MontoTotalPagos
            EasyOne_pago10_SetTotales20(p, "", "", "", Format(basep, "######.00"), ImporteP, "", "", "", "", "", Format(Importe.Value, "######.00"))

            ''Informacion Global del Pago - Aqui tenemos que poner un resumen de los traslados del documento relacionado
            'BaseP, ImpuestoP, TipoFactorP, TasaOCuotaP, ImporteP
            EasyOne_pago10_AddTrasladoP20(p, Format(basep, "######.00"), "002", "Tasa", "0.160000", ImporteP)
        Else
            EasyOne_pago10_SetTotales20(p, "", "", "", Format(basep, "######.00"), Format(ImporteP, "######.00"), "", "", "", "", "", Format(Importe.Value, "######.00"))

            EasyOne_pago10_AddTrasladoP20(p, Format(basep, "######.00"), "002", "Tasa", "0.160000", Format(ImporteP, "######.00"))
        End If


        ''Se agrega el complemento al comprobante
        EasyOne_AddComplemento(c, p)

        'Timbra XML, que se encuentra en variable P
        Dim x As Integer = 99
        If va = 0 Then
            hr = Now.ToLocalTime.ToString.Substring(11, 8)

            If hra <> "" Then
                'MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                hr = hra
            End If

            fechapag = Now.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr 'AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss")

            If seriee <> serie And folio <> NumPago Then

                EasyOne_cfdi33_SetComprobante(c, serie, NumPago, fechapag, "", "", 0, "", "XXX", "", 0, "P", "", TCp, "")

                Dim folf As Integer = 0 'consulto el ultimo folio de serie
                folf = CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & frmComplementoPagoBoutique.grbSucursal.Value & " and Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor))
                If CInt(NumPago) > folf Then
                    upb.Value += 10 'Suma 50
                    Application.DoEvents()

                    'Dim motor As New CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
                    'EasyOne_SetInvoiceOneFachadaInfoPrueba(c, invoice, invoice)
                    EasyOne_SetInvoiceOneFachadaInfo(c, invoice, invoice)

                    Dim b As Integer
                    b = c
                    x = EasyOne_FirmaComprobante(b, CerFactura, Key, Pwd)
                    c = b
                    b = 0
                    Thread.Sleep(10000) 'espera por 10 segundos
                    If x = 0 Then
                        REM *********************************************************************************************P_Term

                        objpro.Indicacion("update serie set secant = " & CInt(NumPago) & " where tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and sid = " & frmComplementoPagoBoutique.grbSucursal.Value & " and seserie = '" & serie & "'  and seTipo = '" & TIP & "' and seEstat = 1", Motor)

                        objpro.Indicacion("update fencabez set feabonos = feabonos + " & Importe.Value & " where feid = " & frmComplementoPagoBoutique.Ugr.ActiveRow.Cells("Id").Value & " and feterm = " & frmComplementoPagoBoutique.Ugr.ActiveRow.Cells("Terminal").Value & " and tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and sid = " & frmComplementoPagoBoutique.grbSucursal.Value & " and feserie = '" & txtSerie.Text & "' and fefact = " & txtFactura.Text & " ", Motor)
                        REM *********************************************************************************************

                        Dim resultadoXML As String = [String].Empty
                        resultadoXML = EasyOne_LeeResultado(c, 8)
                        'Obtenemos el XML completo y timbrado
                        Console.WriteLine(resultadoXML)

                        Application.DoEvents()
                        IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"), resultadoXML)

                        va += 1
                        hra = hr
                        seriee = serie
                        folio = NumPago
                        If x = 0 And va = 1 Then
                            '''''''''''''''''''''''''''''' RECORRO XML''''''''''''''''''''''''''''
                            Dim Version_xml As String
                            Dim version As String
                            Dim uuid As String
                            Dim ftimb As String
                            Dim rfcprov As String
                            Dim sellocfd As String
                            Dim nocert As String

                            Dim documento As XmlDocument = New XmlDocument()
                            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
                            documento.Load(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"))
                            VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                            Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                            upb.Value += 10 '60
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

                                upb.Value += 10 'Suma 70
                                rutaArchivoPDF = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".pdf"
                                rutaArchivoXML = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
                                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

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
                                    upb.Value += 10 'Suma 80  
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try

                                Dim refGlobal As Data.DataTable
                                refGlobal = objpro.ObtenDatos("SELECT cid FROM catclie WHERE crfc = 'XAXX010101000' ", Motor).Tables(0)
                                If refGlobal.Rows.Count <> 0 Then

                                    For Each j In refGlobal.Rows
                                        Application.DoEvents()
                                        ExisteClave = Int(j("cid").ToString)
                                    Next
                                End If

                                Dim FeFecCan As Date = "01/01/1900"
                                Dim fecha1, fecha2, fecha3, terminal As String
                                fecha1 = fechaPago.Text.ToString.Substring(0, 2)
                                fecha2 = fechaPago.Text.ToString.Substring(3, 2)
                                fecha3 = fechaPago.Text.ToString.Substring(6, 4)
                                fechareal = Now.Date.ToString.Substring(0, 2) & "/" & Now.Date.ToString.Substring(3, 2) & "/" & fecha3
                                FeRegistro = fechaPago.Text.ToString.Substring(6, 4) & fecha2 & fecha1
                                terminal = frmComplementoPagoBoutique.grbTienda.Value & frmComplementoPagoBoutique.grbSucursal.Value

                                objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha," &
                                " feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                " FeCanlet,FeAbonos,FeEstatu,FeReg, FeFecCan,FeMotcan, feTip,feuuid, fformpag) " &
                                " values (" & CInt(txtFactura.Text.ToString) & "," & frmComplementoPagoBoutique.grbTienda.Value & "," & CInt(terminal) & "," & CInt(NumPago) & ",'" & serie & "'," & frmComplementoPagoBoutique.grbSucursal.Value & ",'" & fechareal & "'," &
                                " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ",'" & subt.ToString & "', 16,'" & ImporteP & "'," &
                                " '" & 0 & "','" & 0 & "', '" & 0 & "', '" & 0 & "', '" & Importe.Value & "', " &
                                " ' ', '" & 0 & "'," & Estatus & "," & FeRegistro & ",  " & FeFecCan & ", '', 'P','" & uuid & "', '" & grbformaPago.Value & "' )", Motor)


                                Dim correo As New System.Net.Mail.MailMessage()
                                correo.From = New System.Net.Mail.MailAddress(DirEmail) 'DirEmail
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
                                rpFact.Close()
                                rpFact.Dispose()
                                Inicializa()
                                Application.DoEvents()

                                ''Ejecuta el archivo .exe especificado PARA OFICINAS NO
                                Try
                                    Shell("C:\aplicacion\appcliente.exe") ' ejecutable de Gonzalez para seguimiento de facturacion
                                Catch ex As Exception
                                    MessageBox.Show("Error al ejecutar el archivo Appcliente.exe: " & ex.Message)
                                End Try

                                btnAplicarPago.Visible = True
                                va = 0
                            Else
                                MsgBox("Archivo XML no es una factura version 4.0")
                            End If 'Cierra validación de operación exitosa
                        Else
                            'En caso de error mostrar el detalle.
                            Dim resultadoXML2 As String = [String].Empty
                            resultadoXML2 = EasyOne_LeeResultado(c, 11)
                            Console.WriteLine(resultadoXML2)
                            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                            btnAplicarPago.Visible = True
                        End If 'Cierra validación generación de XML

                    Else
                        btnAplicarPago.Visible = True
                        Dim resultadoXML2 As String = [String].Empty
                        resultadoXML2 = EasyOne_LeeResultado(c, 11)
                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                        hra = hr
                    End If
                Else
                    MsgBox("Archivo ya Timbrado !!!  No. de Veces de Timbrado (serie ocupada)" & va, MsgBoxStyle.Information).ToString()
                    btnAplicarPago.Visible = True
                End If

            Else
                MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & seriee & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
                btnAplicarPago.Visible = True
            End If
        Else
            MsgBox("Archivo ya Timbrado !!!  No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
            btnAplicarPago.Visible = True
        End If
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
        'txtNumOperacion.Text = ""
        'txtCtaBeneficiario.Text = ""
        'grbBancos.Text = ""
        'txtCtaOrdenante.Text = ""
        Importe.Value = 0.0
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
    End Sub

End Class