Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Threading

Public Class frmNotaCreditoOficinasBosco
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket, tablaclientes, datosCliente As Data.DataTable
    Dim Terminal, periodod As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim FolioFinal
    Dim Cadena As String
    Dim ExisteClave As String
    Dim FechaFactura As String
    Dim b_iva As Double
    Dim n_LExpedicion As String
    Dim va2 As Integer
    Dim hra As String
    Dim serie, folio As String
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
#End Region

#Region "Sat"
    Dim TpRegimen As String
    Dim e_uuid As String
    Dim Uso_CFDI As String
#End Region

#Region "DatosEmisor"
    Dim TRazonSocial As String
    Dim TRfc As String
    Dim TRFiscal As Integer
    Dim TCp As String
    Dim DesRegimen As String
    Dim RF As String
    Dim RFD As String
    Dim cCP As String
    Dim cDireccion As String
    Dim cComDir As String
    Dim fp As String
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

#Region "Forma Pago"
    Dim FormPago As String
    Dim MetPago As String
    Dim NoCuenta As String
    Dim metodoPago As String
#End Region

    Private Sub frmNotaCreditoOficinasBosco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "NC"
        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        Me.txtFechaVenta.Value = dia + "/" + mes + "/" + amo
        txtRfc.Focus()
        CargaGridConceptos()
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Empresa FROM BGXFacElec.CatTien Where ttipo3 = 'G'", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbUnidad, "Select IdUni as Id, Unides as Unidad FROM BGXFacElec.satUnidad where unists = 'A'", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from bgxfacelec.satmpag ", 0, 1, As400)
        Application.DoEvents()
        va2 = 0
        periodod = Date.Now.ToString.Substring(6, 4)
        cmbperiodo.Items.Add(2010)
        cmbperiodo.Items.Add(periodod - 1)
        cmbperiodo.Items.Add(periodod)
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.ugr, "Select VdCant as Cantidad, VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion, " &
                         "  VdPreUni as PrecioUnitario, '0' as Descuento, '0' as Iva, " &
                         " VdPreTot as Importe From bgxfacelec.VentaDet " &
                         " Where VdFolio = 0", "VentaDet", As400)
        Me.ugr.DataBind()
    End Sub

    Private Sub txtRfc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRfc.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtRazonSocial.Focus()
        End If
    End Sub

    Private Sub txtRazonSocial_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRazonSocial.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtEmail.Focus()
        End If
    End Sub

    Private Sub txtEmail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmail.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = LCase(e.KeyChar)
        Else
            txtUsoCfdi.Focus()
        End If
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        objpro.CargaCombos(grbSucursalUUID, "Select sId, sCsuc as Sucursal FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)

        Me.grbSucursalUUID.Focus()

    End Sub

    Private Sub grbSucursalUUID_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbSucursalUUID.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbSucursalUUID
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDIBosco)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Private Sub grbUnidad_ValueChanged(sender As Object, e As EventArgs) Handles grbUnidad.ValueChanged
        txtClave.Focus()
    End Sub

    Private Sub txtClave_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtClave.KeyPress
        If e.KeyChar = Chr(13) Then
            txtCantidad.Focus()
        End If
    End Sub

    Private Sub txtCantidad_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCantidad.KeyPress
        If e.KeyChar = Chr(13) Then
            txtPUnitario.Focus()
        End If
    End Sub

    Private Sub txtPUnitario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPUnitario.KeyPress
        b_iva = 0
        If e.KeyChar = Chr(13) Then
            b_iva = Format(CDbl((txtCantidad.Text * txtPUnitario.Value) * (txtIva.Text / 100)), "###,###.00")
            txtImport.Value = Format(CDbl((b_iva + (txtPUnitario.Value * txtCantidad.Text))), "###,###.00")
        End If
    End Sub

    Private Sub ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugr.InitializeLayout
        With Me.ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 60
            .DisplayLayout.Bands(0).Columns(1).Width = 60
            .DisplayLayout.Bands(0).Columns(2).Width = 90
            .DisplayLayout.Bands(0).Columns(3).Width = 360
            .DisplayLayout.Bands(0).Columns(4).Width = 110
            .DisplayLayout.Bands(0).Columns(5).Width = 110
            .DisplayLayout.Bands(0).Columns(6).Width = 110
            .DisplayLayout.Bands(0).Columns(7).Width = 110
        End With
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        idCliente = 0
        Formulario(frmClientesBosco)
        If idCliente <> 0 Then
            txtRfc.Text = objpro.Dato("crfc", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtRazonSocial.Text = objpro.Dato("crsocial", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtEmail.Text = objpro.Dato("cemail", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            cfdiuso = objpro.Dato("cusocfdi", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtclvUsoCfdi.Text = cfdiuso
            txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
            RF = regimenFisc
            RFD = regimen
            cCP = objpro.Dato("cCP", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            If cCP.Trim.Length = 4 Then
                cCP = "0" & cCP
            End If
        End If
    End Sub

    Private Sub btnBuscaUUID_Click(sender As Object, e As EventArgs) Handles btnBuscaUUID.Click
        If grbSucursalUUID.Text <> "" Then
            DevUUID = ""
            DevTienda = grbTienda.Value
            DevSucursal = grbSucursalUUID.Value  REM grbSucursal.Value

            Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
            Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
            Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
            FecVenta = a & m & d
            Devfeid = Val(FecVenta)
            Formulario(frmUUIDBoscoDetalle)
            txtUUID.Text = DevUUID
            txtId.Text = FESERIE & " - " & dfefact
            If idCliente <> 0 Then
                txtRfc.Text = objpro.Dato("crfc", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                txtRazonSocial.Text = objpro.Dato("crsocial", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                txtEmail.Text = objpro.Dato("cemail", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                cfdiuso = objpro.Dato("cusocfdi", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
                RF = objpro.Dato("cconfecr", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                RFD = objpro.Dato("regdes", "bgxfacelec.satregfis", "regimen = " & RF & "", As400)

                cCP = objpro.Dato("cCP", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                Me.txtPUnitario.Value = Format((TotFac / 1.16), "###,###.00")
                b_iva = 0
                b_iva = Format(CDbl((txtCantidad.Text * txtPUnitario.Value) * (txtIva.Text / 100)), "###,###.00")
                txtImport.Value = Format(CDbl((b_iva + (txtPUnitario.Value * txtCantidad.Text))), "###,###.00")
            End If
        Else
            MsgBox("Sucursal a buscar el UUID Relacionado ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub LimpiarCajas()
        Me.txtCantidad.Text = "1"
        txtClave.Text = "84111506"
        Me.txtDescripcion.Text = ""
        txtPUnitario.Value = 0.0
        txtIva.Text = 16
        b_iva = 0
        Me.txtImport.Value = 0.0
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, concepto As String)
        With ugr
            .Rows(Renglon).Cells("Cantidad").Value = Me.txtCantidad.Text
            .Rows(Renglon).Cells("Unidad").Value = grbUnidad.Value
            .Rows(Renglon).Cells("Clave").Value = Me.txtClave.Text
            .Rows(Renglon).Cells("Descripcion").Value = Trim(concepto)
            .Rows(Renglon).Cells("PrecioUnitario").Value = Me.txtPUnitario.Value
            .Rows(Renglon).Cells("Descuento").Value = 0
            .Rows(Renglon).Cells("Iva").Value = b_iva
            .Rows(Renglon).Cells("Importe").Value = txtImport.Value
        End With
    End Sub

    Private Sub FormatoGrid(concepto As String)
        ugr.Rows.Band.AddNew()
        ColocarArticulo(ugr.Rows.Count - 1, concepto)
        LimpiarCajas()
    End Sub

    Private Sub btnBuscarNota_Click_1(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        If grbTienda.Value <> 0 Then
            If grbSucursalUUID.Value <> 0 Then
                If txtRfc.Text <> "" Then
                    If txtRazonSocial.Text <> "" Then
                        If txtEmail.Text <> "" Then
                            If txtclvUsoCfdi.Text <> "" Then
                                If ((RF Is Nothing) OrElse (RF Is DBNull.Value)) Then
                                    MsgBox("Regimen Fiscal no puede ir vacio", MsgBoxStyle.Information, "Facturación")
                                Else
                                    If cCP.ToString <> "0" Or cCP.ToString.Trim <> "" Then
                                        Tabla = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursalUUID.Value & " ", As400).Tables(0)
                                        Application.DoEvents()
                                        txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursalUUID.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1 ", As400)) + 1, "00000")
                                        txtSerie.Text = objpro.Dato("seserie", "bgxfacelec.serie", "sId = " & grbSucursalUUID.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1", As400)
                                    Else
                                        MsgBox("Código Postal no puede ir vacio", MsgBoxStyle.Information, "Facturación")
                                    End If
                                End If
                            Else
                                MsgBox("Uso del CFDI", MsgBoxStyle.Information, "Facturación")
                            End If
                        Else
                            MsgBox("Falta Email", MsgBoxStyle.Information, "Facturación")
                        End If
                    Else
                        MsgBox("Razón Social, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
                    End If
                Else
                    MsgBox("RFC, no puede estar en blanco .. ", MsgBoxStyle.Information, "Facturación")
                End If
            Else
                MsgBox("Falta seleccionar sucursal .. ", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("Falta seleccionar tienda correspondiente .. ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub ugr_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugr.AfterRowsDeleted
        sumariza_grid()
    End Sub


    Sub sumariza_grid()
        Importe.Value = 0
        Descuento.Value = 0
        Subtotal.Value = 0
        Iva.Value = 0
        Total.Value = 0
        For Each r In ugr.Rows
            Importe.Value += (r.Cells("PrecioUnitario").Value * r.Cells("Cantidad").Value)
            Descuento.Value += r.Cells("Descuento").Value
            Subtotal.Value = (Importe.Value - Descuento.Value)
            Iva.Value += r.Cells("Iva").Value
            Total.Value += r.Cells("Importe").Value
        Next
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtDescripcion.Text <> "" Then
            If txtImport.Value <> 0 Then
                Dim concepto As String = Trim(txtDescripcion.Text)
                concepto = txtDescripcion.Text.Replace(vbCrLf, "")
                FormatoGrid(concepto)
                If ugr.Rows.Count <> 0 Then
                    sumariza_grid()
                End If
            Else
                MsgBox("El importe no puede ser valor 0 ", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta agregar descripcioón a Faturar ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub btnInicializa_Click(sender As Object, e As EventArgs) Handles btnInicializa.Click
        inicializa_cliente()
        txtRfc.Focus()
    End Sub

    Sub inicializa_cliente()
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        txtUUID.Text = ""
        txtFactura.Text = ""
        txtSerie.Text = ""
        Importe.Text = 0.0
        Descuento.Text = 0.0
        Subtotal.Text = 0.0
        Iva.Text = 0.0
        Total.Text = 0.0
        FecFac = ""
        idCliente = 0
        TotFac = 0
        dfefact = ""
        termin = 0
        transacc = 0
        FESERIE = ""
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        If txtFactura.Text = "" And txtSerie.Text = "" Then
            MsgBox("Falta buscar la Serie y el Número de Factura.")
            btnFacturar.Visible = True
        Else
            If MsgBox("¿Seguro de generar la Nota de Crédito ?. " & " no. de Factura " & txtId.Text, vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
                Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF, FechaSeFacturo As String
                Dim txtFechaVenta As DateTime = Now.Date
                Dim vemotcan As String = "Nota de credito de la factura " & txtId.Text
                Fecha11 = txtFechaVenta.ToString.Substring(0, 2)
                Fecha22 = txtFechaVenta.ToString.Substring(3, 2)
                Fecha33 = txtFechaVenta.ToString.Substring(6, 4)
                FePnotasF = Fecha33 & Fecha22 & Fecha11
                fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                FeRegistro = txtFechaVenta.ToString.Substring(6, 4) & Fecha22 & Fecha11
                Fecha = fechareal
                If ((FecFac Is Nothing) OrElse (FecFac Is DBNull.Value)) Then
                    FechaSeFacturo = txtFechaVenta.ToString.Substring(0, 10)
                    FecFac = Fecha33 & Fecha22 & Fecha11
                Else
                    FechaSeFacturo = FecFac.Substring(6, 2) & "/" & FecFac.Substring(4, 2) & "/" & FecFac.Substring(0, 4)
                End If
                Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
                Dim n_serie As String
                Dim n_notacredito As String
                Dim n_Directorios As String
                Dim n_TRazonSocial As String
                Dim n_TRfc As String
                Dim n_crfc As String
                Dim n_cRazonSocial As String
                Dim n_cDireccion As String
                Dim n_cComDir As String
                Dim n_cCP As String
                Dim n_cUCFDI As String
                Dim n_cregFiscal As String
                Dim n_TRFiscal As String

                upb.Value = 0
                upb.Maximum = 100

                TIP = "NC"
                TSucursal = objpro.ObtenDatos("select * from bgxfacelec.Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursalUUID.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1 ", As400).Tables(0)
                n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursalUUID.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1 ", As400)) + 1, "00000")
                n_serie = TSucursal.Rows(0).Item("Seserie").ToString
                Dim rutlogo As String = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO_BOSCO.png"
                Application.DoEvents()

                DirEmail = Tabla.Rows(0).Item("EMAILDIR").ToString
                PwdEmail = Tabla.Rows(0).Item("EMAILPWD").ToString
                Pop = Tabla.Rows(0).Item("EMAILPOP").ToString
                PuertoPop = Tabla.Rows(0).Item("EMAILPPOP").ToString
                SmtpHost = Tabla.Rows(0).Item("EMAILSMTP").ToString
                PuertoSmtp = Tabla.Rows(0).Item("EMAILPSMTP").ToString
                DescEmail = Tabla.Rows(0).Item("EMAILDESC").ToString
                AsuntoEmail = Tabla.Rows(0).Item("EMAILASU").ToString
                MsjEmail = Tabla.Rows(0).Item("EMAILMSG").ToString

                n_TRazonSocial = objpro.Dato("trsocial", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                n_TRfc = objpro.Dato("trfc", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                n_LExpedicion = Tabla.Rows(0).Item("sCp").ToString

                datosCliente = objpro.ObtenDatos("select * from BGXFACELEC.catclie Where cid = " & idCliente & "", As400).Tables(0)
                n_crfc = datosCliente.Rows(0).Item("crfc").ToString
                n_cRazonSocial = datosCliente.Rows(0).Item("crsocial").ToString
                n_cDireccion = datosCliente.Rows(0).Item("cDir").ToString.Trim & " " & datosCliente.Rows(0).Item("cNumExt").ToString.Trim & " " & datosCliente.Rows(0).Item("cNumInt").ToString.Trim & " " & datosCliente.Rows(0).Item("cColonia").ToString.Trim
                n_cComDir = datosCliente.Rows(0).Item("cMun").ToString.Trim & " " & datosCliente.Rows(0).Item("cEstado").ToString.Trim & " " & datosCliente.Rows(0).Item("cLocal").ToString.Trim & " " & datosCliente.Rows(0).Item("cPais").ToString.Trim
                n_cCP = objpro.Dato("cCP", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
                If n_cCP.Trim.Length = 4 Then
                    n_cCP = "0" & n_cCP
                End If
                n_cUCFDI = datosCliente.Rows(0).Item("cusocfdi").ToString
                n_cregFiscal = datosCliente.Rows(0).Item("cconfecr").ToString 'REGIMEN FISCAL DEL CLIENTE
                If RFD = "" Then
                    RFD = objpro.Dato("regdes", "bgxfacelec.satregfis", "regimen = " & RF & "", As400)
                End If

                If n_cCP.Trim <> "" And n_cUCFDI.Trim <> "" And n_cregFiscal.Trim <> "" Then

                    Dim n_RutNotaCredito As String = objpro.Dato("sForm3", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & "", As400)
                    Dim CerFactura As String = objpro.Dato("sRCer1", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & "", As400)
                    Dim Key As String = objpro.Dato("sRKey", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & "", As400)
                    Dim Pwd As String = objpro.Dato("sPwd", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & "", As400)

                    n_Directorios = objpro.Dato("subic", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & "", As400)

                    Select Case grbformaPago.Value
                        Case "01"
                            FormPago = "EFECTIVO"
                        Case "02"
                            FormPago = "CHEQUE"
                        Case "03"
                            FormPago = "TRANSFERENCIA"
                        Case "04"
                            FormPago = "TARJETA DE CREDITO"
                        Case "05"
                            FormPago = "MONEDERO ELECTRONICO"
                        Case "06"
                            FormPago = "DINERO ELECTRONICO"
                        Case "08"
                            FormPago = "VALES DE DESPENSA"
                        Case "12"
                            FormPago = "DACION EN PAGO"
                        Case "13"
                            FormPago = "PAGO POR SUBROGACION"
                        Case "14"
                            FormPago = "PAGO POR CONSIGNACION"
                        Case "15"
                            FormPago = "CONDONACION"
                        Case "17"
                            FormPago = "COMPENSACION"
                        Case "23"
                            FormPago = "NOVACION"
                        Case "24"
                            FormPago = "CONFUSION"
                        Case "25"
                            FormPago = "REMISION DEUDA"
                        Case "26"
                            FormPago = "PRESCRIPCION CADUCIDAD"
                        Case "27"
                            FormPago = "SATISFACCION ACREADOR"
                        Case "28"
                            FormPago = "TARJETA DEBITO"
                        Case "29"
                            FormPago = "TARJETA SERVICIO"
                        Case "30"
                            FormPago = "NA"
                        Case "99"
                            FormPago = "OTROS"
                    End Select

                    Dim DSNC As DataSet = New DataSet("DataSetFac")
                    Dim Tabla3 As DataTable = DSNC.Tables.Add("DatosNotaCred")

                    Tabla3.Columns.Add("Nrfc")
                    Tabla3.Columns.Add("Nrazonsoc")
                    Tabla3.Columns.Add("Nregfis")
                    Tabla3.Columns.Add("Ndireccion")
                    Tabla3.Columns.Add("Ndircomp")
                    Tabla3.Columns.Add("Ndoc")
                    Tabla3.Columns.Add("Nfolfisc")
                    Tabla3.Columns.Add("Nprovcert")
                    Tabla3.Columns.Add("Ntipocomp")
                    Tabla3.Columns.Add("Nusocfdi")
                    Tabla3.Columns.Add("Nmetpag")
                    Tabla3.Columns.Add("Nformpag")
                    Tabla3.Columns.Add("Nexporta")
                    Tabla3.Columns.Add("Nfechaemi")
                    Tabla3.Columns.Add("Nlugaremi")
                    Tabla3.Columns.Add("Nserieceremi")
                    Tabla3.Columns.Add("Nrrfc")
                    Tabla3.Columns.Add("Nrrazonsoc")
                    Tabla3.Columns.Add("Nrdireccion")
                    Tabla3.Columns.Add("Nrcp")
                    Tabla3.Columns.Add("Nmoneda")
                    Tabla3.Columns.Add("Nsellodig")
                    Tabla3.Columns.Add("Nsellosat")
                    Tabla3.Columns.Add("Ncomplesat")
                    Tabla3.Columns.Add("Nrregimen")
                    Tabla3.Columns.Add("Nfsubtotal")
                    Tabla3.Columns.Add("Ndescuento")
                    Tabla3.Columns.Add("Nfiva")
                    Tabla3.Columns.Add("Nftotal")
                    Tabla3.Columns.Add("Ncantidadl")
                    Tabla3.Columns.Add("NcertSAT")
                    Tabla3.Columns.Add("Nhryfechcer")
                    Tabla3.Columns.Add("Nsucursal")
                    Tabla3.Columns.Add("nrcomdir")
                    Tabla3.Columns.Add("nrusocfdi")

                    Dim Fila As DataRow

                    Fila = DSNC.Tables(0).NewRow() ' Acá insertas una fila nueva
                    ''datos EMISOR
                    Fila("Nrfc") = n_TRfc  ' Acá insertas valores a la nueva fila
                    Fila("Nrazonsoc") = n_TRazonSocial
                    Fila("Ndireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
                    Fila("Ndircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString

                    Dim RFacImpresas2 As String
                    RFacImpresas2 = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
                    Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")

                    Dim TipoCambio As Int16 = 1
                    Fila("Nexporta") = "01-NO APLICA"
                    '(1)
                    'EXPORTACION: 01 - NO APLICA
                    EasyOne_cfdi33_SetCfdi40(pN, "01", "")

                    Dim metpg As String

                    metpg = grbPagos.Value

                    Dim Pu As Double = Format((TotFac / 1.16), "###,###.00")

                    Fila("Ntipocomp") = "EGRESO"
                    Fila("Nmetpag") = metpg
                    Fila("Nformpag") = FormPago


                    'Tipo de relacion
                    EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")
                    'CFDI Relacionado
                    EasyOne_cfdi33_AddCfdiRelacionado(pN, txtUUID.Text)
                    fp = "Otros"
                    upb.Value += 10 'Suma 10

                    n_TRFiscal = objpro.Dato("satregimen", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & "", As400)
                    Application.DoEvents()

                    Dim descreg As String = Trim(Tabla.Rows(0).Item("sRFiscal").ToString)
                    Fila("Nregfis") = n_TRFiscal + " - " + descreg.Trim 'sucursal

                    'Llenamos la sección del Emisor
                    EasyOne_cfdi33_SetEmisor(pN, n_TRfc, n_TRazonSocial, n_TRFiscal)

                    'Llenamos la sección del Receptor
                    'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                    EasyOne_cfdi33_SetReceptor(pN, n_crfc, n_cRazonSocial, "", "", n_cUCFDI) 'CAMBIAR PARA EL CLIENTE'estos paramentros son si o si para factura global
                    EasyOne_cfdi33_SetReceptor40(pN, n_cCP, n_cregFiscal) 'CAMBIAR POR EL REGIMEN DEL CLIENTE 'regimen fiscal para global 616

                    Dim iv As Double = 0
                    Fila("Nrcp") = n_cCP
                    Fila("Nrrfc") = n_crfc '"XAXX010101000"
                    Fila("Nrrazonsoc") = n_cRazonSocial '"PUBLICO EN GENERAL"
                    Fila("Nrdireccion") = n_cDireccion
                    Fila("Nrregimen") = n_cregFiscal & " - " & RFD 'n_desRF '"616-Sin Obligaciones Fiscales"
                    Fila("nrusocfdi") = "Uso CFDI: " & n_cUCFDI
                    Fila("Nmoneda") = "MXN"
                    Fila("nrcomdir") = n_cComDir

                    Dim Impuesto As String = ""
                    Dim TasaOCuota As String = ""
                    Dim TipoFactor As String = ""
                    Dim Impu As String = ""
                    Dim Import As Double = 0
                    Dim Imp As Double = 0
                    Dim bas As Double = 0
                    Dim des1 As Double = 0
                    Dim ToC As String = ""
                    Dim TFac As String = ""
                    Dim Descu As Double = 0
                    Dim des As Double = 0
                    Dim NumPiezas As Integer = 0
                    Dim Tabla4 As DataTable = DSNC.Tables.Add("DatNota")
                    Tabla4.Columns.Add("Ncantidad")
                    Tabla4.Columns.Add("Nunidad")
                    Tabla4.Columns.Add("NuniSAT")
                    Tabla4.Columns.Add("Ndescip")
                    Tabla4.Columns.Add("Npunitario")
                    Tabla4.Columns.Add("Nimporte")
                    Tabla4.Columns.Add("NclaveSAT")
                    Dim Fila3 As DataRow
                    For i = 0 To Me.ugr.Rows.Count - 1
                        Dim pun As Double = Format(Me.ugr.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                        Dim impt As Double = Format((Me.ugr.Rows(i).Cells("PrecioUnitario").Value) * (Me.ugr.Rows(i).Cells("Cantidad").Value), "###,###.00")
                        NumPiezas += Me.ugr.Rows(i).Cells("Cantidad").Value

                        Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
                        Fila3("Ncantidad") = "1"
                        Fila3("Nunidad") = "SERVICIO"
                        Fila3("NuniSAT") = "ACT"
                        Fila3("Ndescip") = Trim(Me.ugr.Rows(i).Cells("Descripcion").Value) 'descrip.Trim
                        Fila3("Npunitario") = Format(pun, "###,###.00") ' Str(FormatCurrency(pun))
                        Fila3("Nimporte") = Format(impt, "###,###.00") 'Str(FormatCurrency(impt))
                        Fila3("NclaveSAT") = Trim(Me.ugr.Rows(i).Cells("Clave").Value) '"84111506"
                        DSNC.Tables(1).Rows.Add(Fila3)

                        If ugr.Rows(i).Cells("Descuento").Value <> 0 Then
                            Descu = Format(Me.ugr.Rows(i).Cells("Descuento").Value, "###,###.00")
                            des += Format(Me.ugr.Rows(i).Cells("Descuento").Value, "###,###.00")
                            EasyOne_cfdi33_AddConcepto(pN, Trim(Me.txtClave.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "SERVICIO", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                            'EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), "", Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                        Else
                            EasyOne_cfdi33_AddConcepto(pN, Trim(Me.txtClave.Text), "100", Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "SERVICIO", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), "")
                            'EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), "", Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), "")
                        End If

                        EasyOne_cfdi33_SetConcepto40(pN, "02")
                        Imp = Format((((Me.ugr.Rows(i).Cells("PrecioUnitario").Value * Me.ugr.Rows(i).Cells("Cantidad").Value) - Me.ugr.Rows(i).Cells("Descuento").Value) * 0.16), "######.00")
                        iv += Imp
                        bas = Format((((Me.ugr.Rows(i).Cells("PrecioUnitario").Value * Me.ugr.Rows(i).Cells("Cantidad").Value) - Me.ugr.Rows(i).Cells("Descuento").Value)), "######.00")
                        Impu = "002"
                        ToC = "0.160000"
                        TFac = "Tasa"
                        EasyOne_cfdi33_AddTraslado(pN, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
                    Next

                    Impuesto = "002"
                    TasaOCuota = "0.160000"
                    TipoFactor = "Tasa"
                    Import = Format(iv, "###,###.00")

                    Dim TotalImpuestosTrasladados As Double = 0
                    TotalImpuestosTrasladados = Format(iv, "###,###.00")

                    EasyOne_cfdi33_SetComprobanteImpuestos(pN, "", TotalImpuestosTrasladados)
                    EasyOne_cfdi33_AddTrasladosTraslado(pN, Impuesto, TipoFactor, TasaOCuota, Import)

                    Dim Total As Double = 0
                    Dim Subt As Double = 0
                    Subt = Format(Subtotal.Value, "###,###.00")
                    Total = Format(((Subt - des) + iv), "###,###.00")
                    Fila("Nfsubtotal") = Format(Subt, "###,###.00")  'Str(FormatCurrency(Subt))
                    Fila("Nfiva") = Format(iv, "###,###.00")  'Str(FormatCurrency(iv))
                    Fila("Nftotal") = Format(Total, "###,###.00")  'Str(FormatCurrency(Total))  'Pu.ToString

                    Dim cantl As String
                    cantl = Letras(Total)
                    Fila("Ncantidadl") = cantl

                    Dim f1, f2, f3, freal, FeReg, hr As String
                    f1 = Today.ToString.Substring(0, 2)
                    f2 = Today.ToString.Substring(3, 2)
                    f3 = Today.ToString.Substring(6, 4)
                    FeReg = Today.ToString.Substring(6, 4) & f2 & f1
                    upb.Value += 10 'Suma 20
                    'Timbra XML, que se encuentra en variable P
                    Dim nc As Integer = 99
                    If va2 = 0 Then
                        hr = Now.ToLocalTime.ToString.Substring(11, 8)

                        If hra <> "" Then
                            'MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                            hr = hra
                        End If

                        freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

                        If (serie = "" And folio = "") Or (serie <> txtSerie.Text And folio <> txtFactura.Text) Then
                            If des = 0 Then
                                EasyOne_cfdi33_SetComprobante(pN, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((Subt), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "E", grbPagos.Value, n_LExpedicion, "")
                            Else
                                EasyOne_cfdi33_SetComprobante(pN, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", "1", Format((Total), "######.00"), "E", grbPagos.Value, n_LExpedicion, "")
                            End If

                            Dim folf As Integer = 0 'consulto el ultimo folio de serie
                            folf = CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursalUUID.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400))
                            If CInt(n_notacredito) > folf Then
                                'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBA
                                EasyOne_SetInvoiceOneFachadaInfo(pN, "RHUUMRGL", "RHUUMRGL")

                                Dim bN As Integer
                                bN = pN
                                nc = EasyOne_FirmaComprobante(bN, CerFactura, Key, Pwd)
                                pN = bN
                                bN = 0
                                Thread.Sleep(10000) 'espera por 10 segundos

                                If nc = 0 Then
                                    objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & n_notacredito & " Where seSerie = '" & n_serie & "' and  SId = " & grbSucursalUUID.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", As400)
                                    If txtId.Text <> "ID" Then

                                        If objpro.DatoExiste("vefolio", "bgxfacelec.ventaenc", "vefolio = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & " and veidfolv = " & transacc & " and veterm = " & termin & " and " & " vefecha = '" & FecFac & "' ", As400) = True Then
                                            Console.WriteLine("si tienen venta enc")
                                            objpro.Indicacion("update bgxfacelec.ventaenc set veestatu = 0, vefac = '0', vefeccan = '" & FeRegistro & "', vemotcan = '" & vemotcan & "' " &
                                            " where vefolio = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & " and veidfolv = " & transacc & " and veterm = " & termin & " and " &
                                            " vefecha = '" & FecFac & "' ", As400)
                                        End If
                                    End If

                                    Application.DoEvents()
                                    upb.Value += 30 'Suma 50
                                    Dim resultadoXMLNC As String = [String].Empty

                                    'Obtenemos el XML completo y timbrado
                                    resultadoXMLNC = EasyOne_LeeResultado(pN, 8)

                                    Console.WriteLine(resultadoXMLNC)
                                    Dim archivoXMLPDFNC As String = ""
                                    Dim archivoREPXNC As String = ""
                                    Dim archivoPDFNC As String = ""

                                    IO.File.WriteAllText(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"), resultadoXMLNC)

                                    va2 += 1
                                    hra = hr
                                    serie = txtSerie.Text
                                    folio = txtFactura.Text

                                    upb.Value += 10  'Suma 60
                                    If nc = 0 And va2 = 1 Then


                                        '''''''''''''''''''''''''''''' RECORRO XML NOTA CREDITO''''''''''''''''''''''''''''
                                        Dim Version_xmlNC As String
                                        Dim versionNC As String
                                        Dim uuidNC As String
                                        Dim ftimbNC As String
                                        Dim rfcprovNC As String
                                        Dim sellocfdNC As String
                                        Dim nocertNC As String

                                        'Dim nav As XPathNavigator
                                        Dim documentoNC As XmlDocument = New XmlDocument()
                                        Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documentoNC.NameTable)
                                        documentoNC.Load(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"))
                                        VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                                        VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                                        Version_xmlNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                                        upb.Value += 10 'Suma 70
                                        If Version_xmlNC = "4.0" Then
                                            Fila("Ndoc") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Folio", VarManager).InnerText
                                            Fila("Nsellodig") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Sello", VarManager).InnerText
                                            Fila("Nlugaremi") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", VarManager).InnerText
                                            Fila("Nfechaemi") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Fecha", VarManager).InnerText
                                            Fila("Nserieceremi") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@NoCertificado", VarManager).InnerText
                                            uuidNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", VarManager).InnerText
                                            Fila("Nfolfisc") = uuidNC
                                            rfcprovNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", VarManager).InnerText
                                            Fila("Nprovcert") = rfcprovNC
                                            Fila("Nsellosat") = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloSAT", VarManager).InnerText
                                            nocertNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@NoCertificadoSAT", VarManager).InnerText
                                            Fila("NcertSAT") = nocertNC
                                            ftimbNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", VarManager).InnerText
                                            Fila("Nhryfechcer") = ftimbNC
                                            versionNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@Version", VarManager).InnerText
                                            sellocfdNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloCFD", VarManager).InnerText
                                            Fila("Ncomplesat") = "|" + versionNC + "|" + uuidNC + "|" + ftimbNC + "|" + rfcprovNC + "||" + sellocfdNC + "|" + nocertNC + "||"

                                            RutaLogoN = rutlogo 'RutFac & "\RecursosCFD33\Penguin.bmp"
                                            RutaQRN = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml.png")

                                            '''''''''''''''''''''''''''''''''''

                                            If txtId.Text <> "ID" Then

                                                If objpro.DatoExiste("vefolio", "bgxfacelec.ventaenc", "vefolio = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & " and veidfolv = " & transacc & " and veterm = " & termin & " and " & " vefecha = '" & FecFac & "' ", As400) = True Then

                                                    objpro.Indicacion("update bgxfacelec.fencabez set feestatu = 0, fefeccan = '" & fechareal & "', femotcan = '" & vemotcan & "' " &
                                                              " where tid = " & grbTienda.Value & " and sid = " & grbSucursalUUID.Value & " and feid = " & transacc & " and feterm = " & termin & " and " &
                                                              " fefact = " & CInt(dfefact) & " and feserie = '" & txtSerie.Text & "' ", As400)
                                                End If

                                            End If
                                            '''''''''''''''''''''''''''''''''''''

                                            DSNC.Tables(0).Rows.Add(Fila)
                                            Dim objrep As New NotaCredito
                                            objrep.SetParameterValue("NLOGO", RutaLogoN)
                                            objrep.SetParameterValue("NQR", RutaQRN)
                                            Dim NotaC As New NotaCredito
                                            NotaC.SetDataSource(DSNC)
                                            NotaC.SetParameterValue("NQR", RutaQRN)
                                            NotaC.SetParameterValue("NLOGO", RutaLogoN)
                                            GNota.CrystalReportViewer1.ReportSource = objrep
                                            GNota.CrystalReportViewer1.ReportSource = NotaC
                                            GNota.WindowState = FormWindowState.Maximized
                                            GNota.Show()

                                            rutaArchivoPDF = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".pdf")
                                            rutaArchivoXML = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")
                                            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                                            Dim cryRpt As New ReportDocument
                                            upb.Value += 10 'Suma 80
                                            Try
                                                Dim CrExportOptions As ExportOptions
                                                Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                                                Dim crf As New PdfRtfWordFormatOptions()
                                                CrDiskFileDestinationOptions.DiskFileName = rutaArchivoPDF
                                                CrExportOptions = NotaC.ExportOptions
                                                With CrExportOptions
                                                    .ExportDestinationType = ExportDestinationType.DiskFile
                                                    .ExportFormatType = ExportFormatType.PortableDocFormat
                                                    .DestinationOptions = CrDiskFileDestinationOptions
                                                    .FormatOptions = CrFormatTypeOptions
                                                End With
                                                NotaC.Export()

                                                upb.Value += 5 'Suma 85 
                                            Catch ex As Exception
                                                MsgBox(ex.ToString)
                                            End Try

                                            Dim correo As New System.Net.Mail.MailMessage()
                                            correo.From = New System.Net.Mail.MailAddress(DirEmail)
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
                                                    upb.Value += 5 'Suma 90
                                                Else
                                                    Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                                                    upb.Value += 5 'Suma 95
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
                                            MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")

                                            CargaGridConceptos()
                                            inicializa_cliente()
                                            btnFacturar.Visible = True
                                            va2 = 0
                                        Else
                                            MsgBox("Archivo XML no es una factura version 4.0")
                                        End If
                                        'Cierra Respuesta del PAC
                                    Else
                                        'En caso de error mostrar el detalle.
                                        Dim resultadoXML2 As String = [String].Empty
                                        resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                                        Console.WriteLine(resultadoXML2)
                                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                        btnFacturar.Visible = True
                                    End If 'Cierra generacion del XML
                                Else
                                    btnFacturar.Visible = True
                                    Dim resultadoXML2 As String = [String].Empty
                                    resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                    hra = hr
                                End If

                            Else
                                MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
                            End If

                        Else
                            MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
                        End If
                    Else
                        MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va2, MsgBoxStyle.Information).ToString()
                    End If


                Else
                    MsgBox("Los datos de facturacion del cliente NO estan completos, favor de actualizar en catalogo de clientes.")
                    btnFacturar.Visible = True
                End If
            Else
                btnFacturar.Visible = True
            End If
        End If
    End Sub

End Class