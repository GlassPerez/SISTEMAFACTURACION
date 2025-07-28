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

Public Class frmPagosBoutique

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

    Dim RF As String
    Dim RFD As String
    Dim cCP As Integer
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
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from satfpag ", 0, 1, Motor)
        Application.DoEvents()
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
            btnClientes.Focus()
        End If
    End Sub

    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        idCliente = 0
        Formulario(frmClientesBoutique)
        If idCliente <> 0 Then
            txtRfc.Text = objpro.Dato("crfc", "catclie", "cid = " & idCliente & "", Motor)
            txtRazonSocial.Text = objpro.Dato("crsocial", "catclie", "cid = " & idCliente & "", Motor)
            txtEmail.Text = objpro.Dato("cemail", "catclie", "cid = " & idCliente & "", Motor)
            cfdiuso = objpro.Dato("cusocfdi", "catclie", "cid = " & idCliente & "", Motor)
            RF = regimenFisc
            RFD = regimen
            cCP = objpro.Dato("cCP", "catclie", "cid = " & idCliente & "", Motor)
            Me.gpoDatos.Visible = True
            Me.txtNumOperacion.Focus()
        End If
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub grbBancos_TextChanged(sender As Object, e As EventArgs) Handles grbBancos.TextChanged
        If grbBancos.Text <> "" Then
            txtCtaOrdenante.Focus()
        End If
    End Sub

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

        Dim Directorios As String = TablaTienda.Rows(0).Item("sUbic").ToString
        Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
        Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
        Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
        Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
        Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
        Dim RutPagos = TablaTienda.Rows(0).Item("sForm4").ToString
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim FechaFactura As String = Date.Now.ToString.Substring(0, 10)
        Dim RFacImpresas As String
        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

        Dim rfcEmisor As String = objpro.Dato("trfc", "cattien", "Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " ", Motor)
        Dim rsEmisor As String = objpro.Dato("trsocial", "cattien", "Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " ", Motor)
        TRFiscal = objpro.Dato("satregimen", "catsucu", "Tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and sid = " & frmComplementoPagoBoutique.grbSucursal.Value & " ", Motor)

        Dim c As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml") 'New CFDI33.Comprobante

        EasyOne_cfdi33_SetCfdi40(c, "01", "")

        'serie - Control interno
        'Folio - Control interno
        'Fecha - auto: para que tome la fecha del sistema
        'Forma de Pago - Debe de pasarse vacio
        'Condiciones de Pago - Debe de pasarse vacio
        'Subtotal - Se debe de registrar 0 o auto:2 para que la dll lo calcule la dll
        'Descuento - Se debe de pasar vacio o auto:2 para que la dll lo maneje automaticamente
        'Moneda - Siempre debe de ser XXX
        'Tipo de Cambio - Se debe de pasar vacio
        'Total - Se debe de registrar 0 o auto:2 para que la dll lo maneje automaticamente
        'Tipo de Comprbante - se debe de registrar el valo P
        'Metodo de de Pago - Se debe de pasar vacio
        'Lugar de Expedicion - Se debe de registrar el codigo postal de la matriz o sucursal 
        EasyOne_cfdi33_SetComprobante(c, "", "", "auto:", "", "", "auto:2", "auto:2", "XXX", "", "auto:2", "P", "", "64000", "")

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
        '    .LugarExpedicion = objpro.Dato("scp", "catsucu", "sId = " & frmComplementoPagoBoutique.grbSucursal.Value & " and Tid = " & frmComplementoPagoBoutique.grbTienda.Value & "  ", Motor)
        '    .Impuestos = Nothing
        'End With
        upb.Value += 10 'Inicia valor

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
        End Select

        'Rfc del emisor
        'Nombre del emisor - auto: para que sea tomado directamente del CSD
        'Regimen Fiscal
        EasyOne_cfdi33_SetEmisor(c, rfcEmisor, rsEmisor, TpRegimen)

        'Rfc del Receptor
        'Nombre del receptor 
        'NumRegIdTRib - Se debe de registrar cuando el rfcEmisor sea igual al de Extranjero
        'ResidenciaFiscal - Se debe de registrar cuando el rfcEmisor sea igual al de Extranjero
        'Uso de CFDI - Siempre debe de ir CP01
        EasyOne_cfdi33_SetReceptor(c, rfcReceptor, rsReceptor, "", "", "CP01")

        'Agregamos los datos adicionales del Emisor (Reforma Fiscal 2022)
        EasyOne_cfdi33_SetReceptor40(c,
                "42185",    ' Domicilio Fiscal del Receptor
                "621")     ' Regimen Fiscal del Recepor

        Dim p As Integer = EasyOne_pago10_Create()
        EasyOne_pago10_SetVersion(p, "2.0") ' Activamos la version 2.0 del Complemento de pago


        'c.AgregaDatosEmisor(rfcEmisor, rsEmisor, TpRegimen)
        'c.AgregaDatosReceptor(rfcReceptor, rsReceptor, c_UsoCFDI.Otros, CFDI33.c_Pais.Mexico)

        Select Case grbformaPago.Value
            Case "01"
                FormPago = Complemento.Pagos10.c_FormaPago.Item01
            Case "02"
                FormPago = Complemento.Pagos10.c_FormaPago.Item02
            Case "03"
                FormPago = Complemento.Pagos10.c_FormaPago.Item03
            Case "04"
                FormPago = Complemento.Pagos10.c_FormaPago.Item04
            Case "05"
                FormPago = Complemento.Pagos10.c_FormaPago.Item05
            Case "06"
                FormPago = Complemento.Pagos10.c_FormaPago.Item06
            Case "08"
                FormPago = Complemento.Pagos10.c_FormaPago.Item08
            Case "12"
                FormPago = Complemento.Pagos10.c_FormaPago.Item12
            Case "13"
                FormPago = Complemento.Pagos10.c_FormaPago.Item13
            Case "14"
                FormPago = Complemento.Pagos10.c_FormaPago.Item14
            Case "15"
                FormPago = Complemento.Pagos10.c_FormaPago.Item15
            Case "17"
                FormPago = Complemento.Pagos10.c_FormaPago.Item17
            Case "23"
                FormPago = Complemento.Pagos10.c_FormaPago.Item23
            Case "24"
                FormPago = Complemento.Pagos10.c_FormaPago.Item24
            Case "25"
                FormPago = Complemento.Pagos10.c_FormaPago.Item25
            Case "26"
                FormPago = Complemento.Pagos10.c_FormaPago.Item26
            Case "27"
                FormPago = Complemento.Pagos10.c_FormaPago.Item27
            Case "28"
                FormPago = Complemento.Pagos10.c_FormaPago.Item28
            Case "29"
                FormPago = Complemento.Pagos10.c_FormaPago.Item29
            Case "30"
                FormPago = Complemento.Pagos10.c_FormaPago.Item30
            Case "99"
                FormPago = Complemento.Pagos10.c_FormaPago.Item99
        End Select

        ' >>> Informacion del Pago
        ' Aqui la informacion de un pago, en un mismo pago nos pueden pagar varios documentos relacionados
        EasyOne_pago10_AddPago(p,
                Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss"),  ' Fecha de Pago
                FormPago,                   ' FormaDePagoP
                "MXN",                  ' MonedaP
                "01",                   ' TipoCambioP
                Importe.Value,          ' Monto
                txtNumOperacion.Text,   ' NumOperacion
                rfcReceptor,              ' RfcEmisorCtaOrd
                "",                     ' NomBancoOrdExt
                txtCtaOrdenante.Text,   ' CtaOrdenante
                rfcEmisor,                     ' RfcEmisorCtaBen
                txtCtaBeneficiario.Text,                     ' CtaBeneficiario
                "",                     ' TipoCadPago
                "",                     ' CertPago
                "",                     ' CadPago
                "")                     ' SelloPago


        'c.AgregaConcepto(1, "", "ACT", "84111506", "", "Pago", 0)
        'c.Conceptos(0).Impuestos = Nothing 'al primer y unico concepto, quitale los impuestos.

        'Paso 1: crea un objeto de complemento
        Dim comp As New Complemento.Pagos10.Pagos
        Dim pago As New Complemento.Pagos10.PagosPago
        Dim docRelacionado As New Complemento.Pagos10.PagosPagoDoctoRelacionado

        '>>> Informacion del documento relacionado
        ' En este caso es un pago parcial de un documento, recordemos que en un pago puede haber uno o muchos documentos relacionados
        EasyOne_pago10_AddDoctoRelacionado(p,
                txtUUID.Text, ' IdDocumento
                txtSerie.Text,                                    ' Serie
                txtFactura.Text,                                   ' Folio
                "MXN",                                  ' MonedaDR
                "",                                     ' TipoCambioDR (No se usa en Pagos 2.0)
                "",                                     ' MetodoDePagoDR (No se usa en Pagos 2.0)
                "1",                                    ' NumParcialidad
                Saldo.Value,                              ' ImpSaldoAnt
                Importe.Value,                               ' ImpPagado
                (Saldo.Value - Importe.Value))                            ' ImpSaldoInsoluto 

        EasyOne_pago10_SetDoctoRelacionado20(p, ' Reforma Fiscal 2022 Informacion de Pagos 2.0
                "1",     ' EquivalenciaDR
                "02")    ' ObjetoImpuestoDR

        ' Agregamos el iva del documento relacionado que nos estan pagando
        EasyOne_pago10_AddTrasladoDR20(p,
            "100.00",   ' BaseDR
            "002",      'ImpuestoDR
            "Tasa",     'TipoFactorDR
            "0.160000", 'TasaOCuotaDR
            "16.00")     'ImporteDR

        ' >>> Informacion Global del Pago - Aqui tenemos que poner un resumen de los traslados del documento relacionado
        EasyOne_pago10_AddTrasladoP20(p,
            "100.00",   ' BaseP
            "002",      'ImpuestoP
            "Tasa",     'TipoFactorP
            "0.160000", 'TasaOCuotaP
            "16.00")    'ImporteP

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

        ''datos del pago
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

        '    .DoctoRelacionado = New List(Of Complemento.Pagos10.PagosPagoDoctoRelacionado)
        '    .DoctoRelacionado.Add(docRelacionado)
        'End With

        '>>> Informacion Global de totales del Pago - Aqui se debe registrar la suma de todos los importes y bases de todos los pagos
        EasyOne_pago10_SetTotales20(p,
            "",         ' TotalRetencionesIVA
            "",         ' TotalRetencionesISR
            "",         ' TotalRetencionesIEPS
            "100.00",   ' TotalTrasladosBaseIVA16
            "16.00",    ' TotalTrasladosImpuestoIVA16
            "",         ' TotalTrasladosBaseIVA8
            "",         ' TotalTrasladosImpuestoIVA8
            "",         ' TotalTrasladosBaseIVA0
            "",         ' TotalTrasladosImpuestoIVA0
            "",         ' TotalTrasladosBaseIVAExento
            "116.00")  ' MontoTotalPagos

        ' Se agrega el complemento al comprobante
        EasyOne_AddComplemento(c, p)

        ' Datos de conexion al servicio de timbrado
        EasyOne_SetInvoiceOneFachadaInfoPrueba(c, "RHUUMRGL", "RHUUMRGL")

        ' Se procesa el comprobante
        Dim i As Integer = EasyOne_FirmaComprobante(c, CerFactura, Key, Pwd)

        ''agrega los datos del pago al complemento
        'With comp
        '    .Pago = New List(Of Complemento.Pagos10.PagosPago)
        '    .Pago.Add(pago)
        'End With

        ''Paso 2: asigna el contenido del objeto complemento al objeto concepto (hay que asignarselo a un objeto de tipo ComprobanteComplemento primero)
        'Dim oC As New CFDI33.ComprobanteComplemento
        'oC.Any = comp.ToXMLElementList
        'c.Complemento.Add(oC)
        'upb.Value += 20  'Suma 30

        ''Paso 3: agrega los namespaces (XML) que utiliza el complemento al comprobante
        ''nota: este ultimo paso solo se requiere ejecutar UNA SOLA VEZ
        'c.Namespaces.Add(comp.Prefix, comp.Namespace)
        'c.Schemas.Add(comp.Namespace, comp.SchemaLocation)

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


        'Dim motorFac As New CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
        'Licenciamiento.AgregaLicencia("NxfUWVt27sSyPaPK/VSpaM1KxObdAq1pQgX7SmJWTEWljBDrg2XqOSmxSth7pZ6FRsViFC+MzuubR0AfCl1wgYiKnOWpT6XWJMXAy58SbcE=")


        'c.Impuestos = Nothing 'quita los impuestos

        'motorFac.Comprobantes.Add(c)        
        upb.Value += 20  'Suma 50
        'Dim resultado As CFDI33.ResultadoProceso = motorFac.GenerarArchivosPAC(True, False)
        'If Not resultado.Errores Then
        If i = 0 Then
            Application.DoEvents()
            Dim resultadoXML As String = [String].Empty
            resultadoXML = EasyOne_LeeResultado(p, 8)
            'Obtenemos el XML completo y timbrado
            Console.WriteLine(resultadoXML)

            Application.DoEvents()
            IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"), resultadoXML)

            '''''''''''''''''''''''''''''' RECORRO XML''''''''''''''''''''''''''''
            Dim Version_xml As String
            'Dim VarConceptos As XmlNodeList
            Dim version As String
            Dim uuid As String
            Dim ftimb As String
            Dim rfcprov As String
            Dim sellocfd As String
            Dim nocert As String

            Dim nav As XPathNavigator
            Dim documento As XmlDocument = New XmlDocument()
            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
            documento.Load(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"))
            VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
            Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText

            'Dim svcNew As New fel.WSCFDI33Client
            'Dim Respuesta As New fel.RespuestaTFD33
            'svcNew.Open()
            'Dim ReferenciaPac As String = Trim(rfcEmisor) & "-" & serie & "-" & NumPago
            'Respuesta = svcNew.TimbrarCFDI(Trim(UserPAC), Trim(PwdPAC), IO.File.ReadAllText(c.ArchivoXMLSellado), Trim(ReferenciaPac))
            'svcNew.Close()

            'Dim archivoREPX As String
            'Dim archivoPDF As String
            'If Respuesta.OperacionExitosa Then
            If Version_xml = "4.0" Then
                'If IO.File.Exists(c.ArchivoXMLSellado) Then
                '    Application.DoEvents()
                '        upb.Value += 10  'Suma 60
                '        Dim archivoXMLPDF As String
                '        IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"), Respuesta.XMLResultado)
                '        CFDI33.Generar.ArchivoCadenaTFD(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml")
                '        CFDI33.Generar.ArchivoCBB(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml")
                '        CFDI33.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml")
                '        CFDI33.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml", Componentes.CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", Componentes.CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
                '        Dim NombrePDF As String = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
                '        archivoXMLPDF = CFDI33.Generar.ArchivoDatosPDF(Trim(NombrePDF), Componentes.CFDI.Globales.Capitalizacion.PrimeraLetraPalabra, "", Componentes.CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)

                '        upb.Value += 10  'Suma 70
                '        Try
                '            Dim RutReporte As String
                '            RutReporte = RutPagos & "\RecursosCFD33"
                '            archivoREPX = PDF.Generar.ArchivoReporteBase(RutReporte, False)
                '            archivoPDF = PDF.Generar.ArchivoPDF2(archivoREPX, archivoXMLPDF)
                '            upb.Value += 10  'Suma 80
                '            Application.DoEvents()
                '            rutaArchivoPDF = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".pdf"
                '            rutaArchivoXML = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
                '        Catch ex As Exception
                '            MsgBox(ex.Message.ToString)
                '        End Try

                '        REM *********************************************************************************************
                '        objpro.Indicacion("update serie set secant = " & CInt(NumPago) & " where tid = " & frmComplementoPagoBoutique.grbTienda.Value & " and sid = " & frmComplementoPagoBoutique.grbSucursal.Value & " and seserie = '" & serie & "' ", Motor)
                '        objpro.Indicacion("update fencabez set feabonos = feabonos + " & Importe.Value & " where feid = " & P_Id & " and feterm = " & P_Term & " and tid = " & P_Tid & " and sid = " & P_Suc & " and feserie = '" & P_Serie & "' and fefact = " & P_Fact & " ", Motor)
                '        REM *********************************************************************************************

                '        Dim correo As New System.Net.Mail.MailMessage()
                '        correo.From = New System.Net.Mail.MailAddress(DirEmail)
                '        correo.To.Add(Trim(Me.txtEmail.Text))

                '        correo.Subject = Trim(AsuntoEmail)
                '        correo.Body = Trim(MsjEmail)
                '        correo.Body += Environment.NewLine & Environment.NewLine
                '        correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
                '        correo.Body += Environment.NewLine & "GRUPO VIGIL. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                '        correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                '        correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                '        correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                '        correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                '        correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                '        correo.Body += " ó a través de una solicitud vía correo electrónico a boutique@grupovigil.com.mx"

                '        correo.IsBodyHtml = False
                '        correo.Priority = System.Net.Mail.MailPriority.Normal
                '        Dim Archivo As New System.Net.Mail.Attachment(rutaArchivoPDF)
                '        For i = 0 To 1
                '            If i = 0 Then
                '                Archivo = New System.Net.Mail.Attachment(rutaArchivoPDF)
                '                upb.Value += 5 'Suma 85
                '            Else
                '                Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                '                upb.Value += 5 'Suma 90
                '            End If
                '            correo.Attachments.Add(Archivo)
                '        Next
                '        Dim smtp As New System.Net.Mail.SmtpClient
                '        smtp.Host = SmtpHost
                '        smtp.Port = Int(PuertoSmtp)
                '        smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                '        smtp.EnableSsl = SeguridadEmail


                '        Inicializa()
                '        upb.Value += 10  'Suma 100
                '        APDF(archivoPDF)

                '    'End If 'Valida si se sella el XML
                'Else
                '    Dim sll = c.ArchivoSello
                '        Dim txt = sll.ToString.Replace(".sll", ".txt")
                '        Dim xml = sll.ToString.Replace(".sll", ".xml")
                '        IO.File.Delete(sll) : IO.File.Delete(txt) : IO.File.Delete(xml)
                '        MsgBox(Respuesta.CodigoRespuesta.ToString & " " & Respuesta.MensajeError.ToString, MsgBoxStyle.Information, "Facturación .. ")
                '    End If 'Cierra validación de lo que dice el PAC
                'Else
                '    For Each cNG As CFDI33.Comprobante In resultado.ComprobantesNoGenerados
                '    MessageBox.Show(cNG.ErrorGeneracion.Message)
                'Next
            End If 'Cierra validación de Error de generar XML
        Else
            Console.WriteLine(EasyOne_LeeResultado(c, 11))

            Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")
            Console.WriteLine("xxxxxxxxxxxx ERROR xxxxxxxxxxxxx")
            Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")

            Console.ReadLine()
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
        txtNumOperacion.Text = ""
        txtCtaBeneficiario.Text = ""
        grbBancos.Text = ""
        txtCtaOrdenante.Text = ""
        Importe.Value = 0.0
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
    End Sub

End Class