Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports System.Runtime.InteropServices
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml.Linq
Imports System.Xml.XPath

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Threading

Public Class frmNotaCreditoOfi

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
    Dim serieNC, folioNC As String
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

    Private Sub frmNotaCreditoOfi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "NCV"
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        'va = 0
        va2 = 0
        CargaGridConceptos()
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM bgxfacelec.CatTien where ttipo3 = 'G' ", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from bgxfacelec.satmpag ", 0, 1, As400)
        grbPagos.Value = "PUE"
        Application.DoEvents()
        objpro.CargaCombos(grbUnidad, "Select IdUni as Id, Unides as Unidad FROM BGXFacElec.satUnidad where unists = 'A'", 0, 1, As400)
        Application.DoEvents()
        periodod = Date.Now.ToString.Substring(6, 4)
        cmbperiodo.Items.Add(0)
        cmbperiodo.Items.Add(2010)
        cmbperiodo.Items.Add(periodod - 1)
        cmbperiodo.Items.Add(periodod)
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.ugrnotacred, "Select VdCant as Cantidad, VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion, " &
                         "  VdPreUni as PrecioUnitario, '0' as Descuento, '0' as Iva, " &
                         " VdPreTot as Importe From bgxfacelec.VentaDet " &
                         " Where VdFolio = 0", "VentaDet", As400)
        Me.ugrnotacred.DataBind()

        objpro.LLenaGrid(Me.ugr, "Select VdDescri as UUID_RELACIONADO From bgxfacelec.VentaDet Where VdFolio = 0", "VentaDet", As400)
        Me.ugr.DataBind()
    End Sub

    Private Sub grbTienda_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbTienda.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.grbSucursal.Focus()
        End If
    End Sub

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        Try
            objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM bgxfacelec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
            Me.grbSucursal.Value = Nothing
            grbSucursal.Focus()
        Catch ex As Exception
            MsgBox("Sucursal: " & ex.Message, MsgBoxStyle.Information, "FACTURAS-E")
        End Try
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
        With ugrnotacred
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

    Private Sub ugrnotacredr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrnotacred.InitializeLayout
        With Me.ugrnotacred
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

    Private Sub ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugr.InitializeLayout
        With Me.ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 400
        End With
    End Sub

    Private Sub FormatoGrid(concepto As String)
        ugrnotacred.Rows.Band.AddNew()
        ColocarArticulo(ugrnotacred.Rows.Count - 1, concepto)
        LimpiarCajas()
    End Sub
    Private Sub ugrnotacred_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugrnotacred.AfterRowsDeleted
        sumariza_grid()
    End Sub

    Private Sub ugr_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugr.AfterRowsDeleted
        'sumariza_grid()
    End Sub


    Sub sumariza_grid()
        Importe.Value = 0
        Descuento.Value = 0
        Subtotal.Value = 0
        Iva.Value = 0
        Total.Value = 0
        For Each r In ugrnotacred.Rows
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
                If ugrnotacred.Rows.Count <> 0 Then
                    sumariza_grid()
                End If
            Else
                MsgBox("El importe no puede ser valor 0 ", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta agregar descripcioón a Faturar ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Sub inicializa_cliente()
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

    Private Sub ColocarArticuloUuid(ByVal Renglon As Integer, uuidref As String)
        With ugr
            .Rows(Renglon).Cells("UUID_RELACIONADO").Value = uuidref
        End With
    End Sub

    Private Sub btnBuscarNota_Click(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        If grbTienda.Value <> 0 Then
            If grbSucursal.Value <> 0 Then
                If ckTIP.Checked = False Then
                    TIP = "NCV"
                Else
                    TIP = "NCE"
                End If
                Tabla = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", As400).Tables(0)
                Application.DoEvents()
                If objpro.DatoExiste("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1 ", As400) = True Then
                    txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1 ", As400)) + 1, "00000")
                    txtSerie.Text = objpro.Dato("seserie", "bgxfacelec.serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and SEAMOAP = " & cmbperiodo.SelectedItem & " and seEstat = 1", As400)

                Else
                    MsgBox("No se encontró la serie... ", MsgBoxStyle.Information, "Facturación.. ")
                End If
            Else
                MsgBox("Falta seleccionar sucursal .. ", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("Falta seleccionar tienda correspondiente .. ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub FormatoGriduuid(uuidref As String)
        ugr.Rows.Band.AddNew()
        ColocarArticuloUuid(ugr.Rows.Count - 1, uuidref)
        Me.txtUUID.Text = ""
    End Sub

    Private Sub btnBuscaUUID_Click(sender As Object, e As EventArgs) Handles btnBuscaUUID.Click
        If txtUUID.Text <> "" Then
            Dim uuidr As String = ""

            uuidr = txtUUID.Text.Trim.Replace(" ", "")

            If uuidr.Length = 36 Then
                Dim uuidrel As String = Trim(txtUUID.Text)
                'uuidrel = txtDescripcion.Text.Replace(vbCrLf, "")
                FormatoGriduuid(uuidrel)
                'If ugr.Rows.Count <> 0 Then
                '    sumariza_grid()
                'End If
            Else
                MsgBox("El UUID no tiene una longitud correcta ", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Escriba el UUID ", MsgBoxStyle.Information, "Facturación .. ")
        End If
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

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        Timbrado = False
        'td = 2
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim n_serie As String
        Dim n_notacredito As String
        Dim freal, hr As String
        DesRegimen = ""

        upb.Value = 0
        upb.Maximum = 100

        TIP = "NCV"


        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, RutFac, CarpSucursal, Directorios As String
        TablaTienda = objpro.ObtenDatos("Select * from bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", As400).Tables(0)
        TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
        TRfc = TablaTienda.Rows(0).Item("trfc").ToString
        TCp = TablaTienda.Rows(0).Item("tcp").ToString
        TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
        RutFac = TablaTienda.Rows(0).Item("sForm1").ToString
        CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString

        Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
        Dim Cer As String = TablaTienda.Rows(0).Item("sRCer1").ToString
        Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
        Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString

        n_notacredito = txtFactura.Text
        n_serie = txtSerie.Text
        'TSucursal = objpro.ObtenDatos("SELECT * FROM bgxfacelec.Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400).Tables(0)
        'n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")
        'n_serie = TSucursal.Rows(0).Item("Seserie").ToString

        Dim RFacImpresas As String
        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
        Transaccion = Now.Date.ToString.Substring(6, 4) & Now.Date.ToString.Substring(3, 2) & Now.Date.ToString.Substring(0, 2)

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

        Dim Fila As DataRow

        Fila = DSNC.Tables(0).NewRow() ' Acá insertas una fila nueva
        Fila("Nrfc") = TRfc  ' Acá insertas valores a la nueva fila
        Fila("Nrazonsoc") = TRazonSocial
        Fila("Nregfis") = TRFiscal + " - " + descreg.Trim
        Fila("Ndireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
        Fila("Ndircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

        Dim RFacImpresas2 As String
        RFacImpresas2 = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
        Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")

        Dim TipoCambio As Int16 = 1
        Fila("Nexporta") = "01-No Aplica"
        '(1)
        'EXPORTACION: 01 - NO APLICA
        EasyOne_cfdi33_SetCfdi40(pN, "01", "")

        upb.Value += 20

        '######################## PROCESO DE CANCELACION
        'Iniciamos el proceso de cancelacion
        'Certificado Sello Digital .cer
        'LLave Privada .key
        'Password llave privada
        'UUID
        'Rfc Emisor
        'Clave del Agente
        'SoftwareId
        'Rfc receptor
        'Total
        'Motivo de cancelacion (Nuevo)

        Dim metpg As String
        Dim tipcom As String
        Dim formpg As String

        If grbPagos.Value = "PPD" Then
            formpg = "99"
            metpg = "PPD"
            tipcom = "E"
            Fila("Nformpag") = "POR DEFINIR" '"Otros"
        Else
            formpg = "01"
            metpg = "PUE"
            tipcom = "E"
            Fila("Nformpag") = "EFECTIVO" '"Otros"
        End If

        Fila("Ntipocomp") = tipcom + " - Egreso"
        Fila("Nmetpag") = metpg
        'Fila("Nformpag") = formpg


        'Tipo de relacion
        EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")

        Dim Tabla5 As DataTable = DSNC.Tables.Add("PAGOS")
        Tabla5.Columns.Add("CFDIpricipal")
        Dim Fila4 As DataRow
        upb.Value += 20

        For i = 0 To Me.ugr.Rows.Count - 1
            Fila4 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
            'CFDI Relacionado
            EasyOne_cfdi33_AddCfdiRelacionado(pN, Me.ugr.Rows(i).Cells("UUID_RELACIONADO").Value.ToString.Trim)
            Fila4("CFDIpricipal") = Me.ugr.Rows(i).Cells("UUID_RELACIONADO").Value
            DSNC.Tables(1).Rows.Add(Fila4)
        Next

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
        End Select

        'Llenamos la sección del Emisor
        EasyOne_cfdi33_SetEmisor(pN, TRfc, TRazonSocial, TpRegimen)

        EasyOne_cfdi33_SetReceptor(pN, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
        EasyOne_cfdi33_SetReceptor40(pN, TCp, "616") 'regimen fiscal para global 616

        Dim iv As Double = 0

        Fila("Nrcp") = TCp
        Fila("Nrrfc") = "XAXX010101000"
        Fila("Nrrazonsoc") = "PUBLICO EN GENERAL"
        Fila("Nrdireccion") = ""
        Fila("Nrregimen") = "616-Sin Obligaciones Fiscales"
        Fila("Nusocfdi") = "S01"
        Fila("Nmoneda") = "MXN"

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
        upb.Value += 20

        For i = 0 To Me.ugrnotacred.Rows.Count - 1
            Dim pun As Double = Format(Me.ugrnotacred.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
            Dim impt As Double = Format((Me.ugrnotacred.Rows(i).Cells("PrecioUnitario").Value) * (Me.ugrnotacred.Rows(i).Cells("Cantidad").Value), "###,###.00")
            NumPiezas += Me.ugrnotacred.Rows(i).Cells("Cantidad").Value

            Fila3 = DSNC.Tables(2).NewRow() ' Acá insertas una fila nueva
            Fila3("Ncantidad") = "1"
            Fila3("Nunidad") = "SERVICIO"
            Fila3("NuniSAT") = "ACT"
            Fila3("Ndescip") = Trim(Me.ugrnotacred.Rows(i).Cells("Descripcion").Value) 'descrip.Trim
            Fila3("Npunitario") = Format(pun, "###,###.00") ' Str(FormatCurrency(pun))
            Fila3("Nimporte") = Format(impt, "###,###.00") 'Str(FormatCurrency(impt))
            Fila3("NclaveSAT") = Trim(Me.ugrnotacred.Rows(i).Cells("Clave").Value) '"84111506"
            DSNC.Tables(2).Rows.Add(Fila3)

            If ugrnotacred.Rows(i).Cells("Descuento").Value <> 0 Then
                Descu = Format(Me.ugrnotacred.Rows(i).Cells("Descuento").Value, "###,###.00")
                des += Format(Me.ugrnotacred.Rows(i).Cells("Descuento").Value, "###,###.00")
                EasyOne_cfdi33_AddConcepto(pN, Trim(Me.txtClave.Text), Trim(Me.ugrnotacred.Rows(i).Cells("Clave").Value), Me.ugrnotacred.Rows(i).Cells("Cantidad").Value, Trim(Me.ugrnotacred.Rows(i).Cells("Unidad").Value), "SERVICIO", Trim(Me.ugrnotacred.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                'EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), "", Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
            Else
                EasyOne_cfdi33_AddConcepto(pN, Trim(Me.txtClave.Text), Transaccion.ToString, Me.ugrnotacred.Rows(i).Cells("Cantidad").Value, Trim(Me.ugrnotacred.Rows(i).Cells("Unidad").Value), "SERVICIO", Trim(Me.ugrnotacred.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), "")
                'EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), "", Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), "")
            End If

            EasyOne_cfdi33_SetConcepto40(pN, "02")
            Imp = Format((((Me.ugrnotacred.Rows(i).Cells("PrecioUnitario").Value * Me.ugrnotacred.Rows(i).Cells("Cantidad").Value) - Me.ugrnotacred.Rows(i).Cells("Descuento").Value) * 0.16), "######.00")
            iv += Imp
            bas = Format((((Me.ugrnotacred.Rows(i).Cells("PrecioUnitario").Value * Me.ugrnotacred.Rows(i).Cells("Cantidad").Value) - Me.ugrnotacred.Rows(i).Cells("Descuento").Value)), "######.00")
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

        Dim f1, f2, f3, FeReg As String
        f1 = Today.ToString.Substring(0, 2)
        f2 = Today.ToString.Substring(3, 2)
        f3 = Today.ToString.Substring(6, 4)

        'Timbra XML, que se encuentra en variable P
        Dim nc As Integer = 99
        If va2 = 0 Then
            hr = Now.ToLocalTime.ToString.Substring(11, 8)

            If hra <> "" Then
                'MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                hr = hra
            End If

            freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

            If (serie = "" And folio = "") Then
                If des = 0 Then
                    EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, freal, formpg, "", Format((Subt), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "E", grbPagos.Value, TCp, "")
                Else
                    EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, freal, formpg, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", "1", Format((Total), "######.00"), "E", grbPagos.Value, TCp, "")
                End If

                'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBA
                EasyOne_SetInvoiceOneFachadaInfo(pN, "RHUUMRGL", "RHUUMRGL")

                Dim bN As Integer
                bN = pN
                nc = EasyOne_FirmaComprobante(bN, Cer, Key, Pwd)
                pN = bN
                bN = 0
                Thread.Sleep(10000) 'espera por 10 segundos
                If nc = 0 Then
                    objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1 and setipo = '" & TIP & "' ", As400)
                    upb.Value += 10
                    Dim resultadoXMLNC As String = [String].Empty

                    'Obtenemos el XML completo y timbrado
                    resultadoXMLNC = EasyOne_LeeResultado(pN, 8)

                    Console.WriteLine(resultadoXMLNC)
                    Dim archivoXMLPDFNC As String = ""
                    Dim archivoREPXNC As String = ""
                    Dim archivoPDFNC As String = ""

                    IO.File.WriteAllText(Trim(RFacImpresas2 & "\" & Trim(TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"), resultadoXMLNC)

                    va2 += 1
                    hra = hr
                    serie = n_serie
                    folio = n_notacredito

                    'Obtención del resultado de la solicitud
                    If nc = 0 And va2 = 1 Then

                        '''''''''''''''''''''''''''''' RECORRO XML NOTA CREDITO''''''''''''''''''''''''''''
                        Dim Version_xmlNC As String
                        'Dim VarConceptos As XmlNodeList
                        Dim versionNC As String
                        Dim uuidNC As String
                        Dim ftimbNC As String
                        Dim rfcprovNC As String
                        Dim sellocfdNC As String
                        Dim nocertNC As String

                        Dim documentoNC As XmlDocument = New XmlDocument()
                        Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documentoNC.NameTable)
                        documentoNC.Load(Trim(RFacImpresas2 & "\" & Trim(TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"))
                        VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                        VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                        Version_xmlNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
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

                            'RutaLogoN = RutFac & "\RecursosCFD33\Penguin.bmp"
                            RutaQRN = Trim(RFacImpresas2 & "\" & Trim(TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml.png")
                            upb.Value += 10
                            ''''''''''''''''''''''''''''''''''''''''''''''''''
                            Dim FeFecCan As Date = "01/01/1900"
                            Dim FeMotcan As String = ""
                            Dim Tipo As String = "F"
                            Dim termin As Integer = 0
                            If grbSucursal.Value.ToString.Length = 2 Then
                                termin = grbSucursal.Value
                            Else
                                termin = grbTienda.Value & grbSucursal.Value
                            End If
                            Dim ts As String = Trim(Str(termin))  ''Trim(Me.grbTienda.Value) & Trim(Str(Me.grbSucursal.Value))
                            ts = Trim(ts)

                            objpro.Indicacion("Insert into bgxfacelec.NCREREL (NCFECF,NCSERIE, NCFOL, NCUUID, NCRTDA, NCSUC, NCTIPO, NCMETP, NCFORP, NCIMPT, NCDESC, NCSUBT, NCIVA, NCTOT) " &
                                    " values (" & Transaccion & ",'" & n_serie & "'," & CInt(n_notacredito) & ", '" & uuidNC & "', " & grbTienda.Value & ", " & Me.grbSucursal.Value & ",'" & TIP & "'," &
                                    " '" & grbPagos.Value & "', '" & formpg & "', " & Importe.Value & ", " & Descuento.Value & "," &
                                    " " & Subtotal.Value & "," & Iva.Value & ", " & Total & ")", As400)
                            Dim CUENTA As Integer
                            CUENTA = Me.ugr.Rows.Count
                            For i = 0 To Me.ugr.Rows.Count - 1
                                If CUENTA > 0 Then
                                    objpro.Indicacion("Insert into bgxfacelec.NOTCRERE (NRFECF,NRSERIE, NRFOL, NRUUID, NRID, NRUUIDREL) " &
                                   " values (" & Transaccion & ",'" & n_serie & "'," & CInt(n_notacredito) & ", '" & uuidNC & "'," & CUENTA & ", '" & Me.ugr.Rows(i).Cells("UUID_RELACIONADO").Value.ToString.Trim & "')", As400)
                                    CUENTA = CUENTA - 1
                                End If
                            Next

                            '''''nuevoooooooooooooooooooooooooooooooooooooooooo

                            RutaLogo = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO_BOSCO.png"
                            DSNC.Tables(0).Rows.Add(Fila)
                            Dim objrep As New NotaCreditoReferenciada
                            objrep.SetParameterValue("NLOGO", RutaLogo)
                            objrep.SetParameterValue("NQR", RutaQRN)
                            Dim NotaC As New NotaCreditoReferenciada
                            NotaC.SetDataSource(DSNC)
                            NotaC.SetParameterValue("NQR", RutaQRN)
                            NotaC.SetParameterValue("NLOGO", RutaLogo)
                            NotaReferenciada.CrystalReportViewer1.ReportSource = objrep
                            NotaReferenciada.CrystalReportViewer1.ReportSource = NotaC
                            NotaReferenciada.WindowState = FormWindowState.Maximized
                            NotaReferenciada.Show()

                            Dim Ruta As String = Trim(RFacImpresas2 & "\" & Trim(TRfc) & "-" & n_serie & "-" & n_notacredito & ".pdf")
                            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                            ''Dim cryRpt As New ReportDocument
                            upb.Value += 20
                            inicializa_cliente()
                            CargaGridConceptos()
                            Try
                                Dim CrExportOptions As ExportOptions
                                Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                                Dim crf As New PdfRtfWordFormatOptions()
                                CrDiskFileDestinationOptions.DiskFileName = Ruta
                                CrExportOptions = NotaC.ExportOptions
                                With CrExportOptions
                                    .ExportDestinationType = ExportDestinationType.DiskFile
                                    .ExportFormatType = ExportFormatType.PortableDocFormat
                                    .DestinationOptions = CrDiskFileDestinationOptions
                                    .FormatOptions = CrFormatTypeOptions
                                End With
                                NotaC.Export()
                                NotaC.Close()
                                NotaC.Dispose()
                            Catch ex As Exception
                                MsgBox(ex.ToString)
                            End Try
                            '''''nuevoooooooooooooooooooooooooooooooooooooooooo
                            va2 = 0
                        Else
                            MsgBox("Archivo XML no es una factura version 4.0")
                        End If
                    Else
                        Console.WriteLine(EasyOne_LeeResultado(pN, 8))
                        Console.WriteLine(EasyOne_LeeResultado(pN, 11))

                        Console.WriteLine("*******************************")
                        Console.WriteLine("*********** ERROR *************")
                        Console.WriteLine("*******************************")
                        Dim resultadoXML3 As String = [String].Empty
                        resultadoXML3 = EasyOne_LeeResultado(pN, 11)
                        Console.ReadLine()
                        MsgBox("ERROR AL GENERAR LA NOTA DE CREDITO 4.0" & resultadoXML3)
                    End If
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
            MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va2, MsgBoxStyle.Information).ToString()
        End If
    End Sub
End Class