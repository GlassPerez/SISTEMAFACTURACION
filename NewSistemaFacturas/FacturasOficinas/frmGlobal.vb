
Imports System
    Imports N400
    Imports N400.Componentes
    Imports N400.Componentes.CFDI.CFDI33
    Imports N400.Framework
    Imports N400.QR
    Imports System.IO
    Imports System.Xml
    Imports System.Xml.XPath

Public Class frmGlobal
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket, tablaclientes As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim FolioFinal
    Dim Cadena As String
    Dim ExisteClave As String
    Dim fechventa, FeReg, FechaFactura As String

    Dim b_iva As Double
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
#End Region

    Private Sub frmGlobal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "VG" '"FC"
        CargaGridConceptos()
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Empresa FROM CatTien ", 0, 1, Motor)
        Application.DoEvents()
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        fechventa = "01/" + Now.Date.Month.ToString + "/" + Now.Date.Year.ToString
        txtFechaVenta.Value = CDate(fechventa)
        Application.DoEvents()

    End Sub


    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.ugr, "Select VdCant as Cantidad, VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion, " &
                     "  VdPreUni as PrecioUnitario, '0' as Descuento, '0' as Iva, " &
                     " VdPreTot as Importe From VentaDet " &
                     " Where VdFolio = 0", "VentaDet", Motor)


        Me.ugr.DataBind()
    End Sub

    Private Sub frmGlobal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
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

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Focus()
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbSucursal_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbSucursal.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbSucursal
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        If txtEmail.Text <> "" Then
            Tabla = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", Motor).Tables(0)
            Application.DoEvents()
            txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
            txtSerie.Text = objpro.Dato("seserie", "serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1", Motor)

        Else
                    MsgBox("Falta Email", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub txtPUnitario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPUnitario.KeyPress
        b_iva = 0
        If e.KeyChar = Chr(13) Then
            b_iva = Format(CDbl((1 * txtPUnitario.Value) * (txtIva.Text / 100)), "###,###.00")
            txtImport.Value = Format(CDbl((b_iva + (txtPUnitario.Value * 1))), "###,###.00")
        End If
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If txtImport.Value <> 0 Then
            Dim concepto As String = Trim("VENTA")
            FormatoGrid(concepto)
            If ugr.Rows.Count <> 0 Then
                sumariza_grid()
            End If
        Else
            MsgBox("El importe no puede ser valor 0 ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Sub sumariza_grid()
        Importe.Value = 0
        Descuento.Value = 0
        Subtotal.Value = 0
        Iva.Value = 0
        Total.Value = 0
        For Each r In ugr.Rows
            Importe.Value += (r.Cells("PrecioUnitario").Value * 1)
            Descuento.Value += r.Cells("Descuento").Value
            Subtotal.Value = (Importe.Value - Descuento.Value)
            Iva.Value += r.Cells("Iva").Value
            Total.Value += r.Cells("Importe").Value
        Next
    End Sub

    Private Sub FormatoGrid(concepto As String)
        ugr.Rows.Band.AddNew()
        ColocarArticulo(ugr.Rows.Count - 1, concepto)
        LimpiarCajas()
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, concepto As String)
        With ugr
            .Rows(Renglon).Cells("Cantidad").Value = 1
            .Rows(Renglon).Cells("Unidad").Value = "SERVICIO"
            .Rows(Renglon).Cells("Clave").Value = "84111506"
            .Rows(Renglon).Cells("Descripcion").Value = Trim(concepto)
            .Rows(Renglon).Cells("PrecioUnitario").Value = Me.txtPUnitario.Value
            .Rows(Renglon).Cells("Descuento").Value = 0
            .Rows(Renglon).Cells("Iva").Value = b_iva
            .Rows(Renglon).Cells("Importe").Value = txtImport.Value
        End With
    End Sub

    Private Sub LimpiarCajas()
        'Me.txtCantidad.Text = "1"
        'txtClave.Text = "01010101"
        txtPUnitario.Value = 0.0
        txtIva.Text = 16
        b_iva = 0
        Me.txtImport.Value = 0.0
    End Sub


    Private Sub txtDescripcion_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            btnAgregar.Focus()
        End If
    End Sub

    Private Sub ugr_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugr.AfterRowsDeleted
        sumariza_grid()
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click

        TIP = "VG"
            FolioFinal = objpro.Dato("seffin", "serie", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seSerie = '" & txtSerie.Text & "' and seTipo = '" & TIP & "' and seEstat = 1  ", Motor)
            upb.Value = 0
            upb.Maximum = 100

        If CInt(Me.txtFactura.Text) <= FolioFinal Then

            'TablaTienda = objpro.ObtenDatos("Select * from CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
            TRazonSocial = Tabla.Rows(0).Item("trsocial").ToString
            TRfc = Tabla.Rows(0).Item("trfc").ToString
            TCp = Tabla.Rows(0).Item("tcp").ToString
            TRFiscal = Tabla.Rows(0).Item("satRegimen").ToString
            'descreg = Tabla.Rows(0).Item("sRFiscal").ToString
            'direccion = Tabla.Rows(0).Item("sRFiscal").ToString
            'invoice = Trim(Tabla.Rows(0).Item("tuserpac").ToString)

            Dim f1, f2, f3, freal, hr As String
            f1 = txtFechaVenta.Text.ToString.Substring(0, 2)
            f2 = txtFechaVenta.Text.ToString.Substring(3, 2)
            f3 = txtFechaVenta.Text.ToString.Substring(6, 4)
            hr = Now.ToLocalTime.ToString.Substring(11, 8)

            Application.DoEvents()
            TRazonSocial = Tabla.Rows(0).Item("trSocial").ToString
            TRfc = Tabla.Rows(0).Item("trfc").ToString
            TRFiscal = Tabla.Rows(0).Item("satRegimen").ToString
            TCp = Tabla.Rows(0).Item("sCp").ToString

            Dim Directorios As String = Tabla.Rows(0).Item("sUbic").ToString
            Dim CerFactura As String = Tabla.Rows(0).Item("sRCer1").ToString
            Dim Key As String = Tabla.Rows(0).Item("sRKey").ToString
            Dim Pwd As String = Tabla.Rows(0).Item("sPwd").ToString
            Dim RutFacClientes = Tabla.Rows(0).Item("sForm1").ToString
            Dim CarpTienda = Tabla.Rows(0).Item("sCtien").ToString
            Dim CarpSucursal = Tabla.Rows(0).Item("sCsuc").ToString
            Dim rutlogo As String = Trim(Tabla.Rows(0).Item("rutLogo").ToString) '"\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png" 
            Dim descreg As String = Trim(Tabla.Rows(0).Item("sRFiscal").ToString)
            Dim FeReg As String = txtFechaVenta.Text.ToString.Substring(6, 4) & f2 & f1
            'Dim FechaFactura As DateTime = Now
            'Dim FechaHora As String = FechaFactura.ToString.Substring(10, 10)

            DirEmail = Tabla.Rows(0).Item("EMAILDIR").ToString
            PwdEmail = Tabla.Rows(0).Item("EMAILPWD").ToString
            Pop = Tabla.Rows(0).Item("EMAILPOP").ToString
            PuertoPop = Tabla.Rows(0).Item("EMAILPPOP").ToString
            SmtpHost = Tabla.Rows(0).Item("EMAILSMTP").ToString
            PuertoSmtp = Tabla.Rows(0).Item("EMAILPSMTP").ToString
            DescEmail = Tabla.Rows(0).Item("EMAILDESC").ToString
            AsuntoEmail = Tabla.Rows(0).Item("EMAILASU").ToString
            MsjEmail = Tabla.Rows(0).Item("EMAILMSG").ToString

            Application.DoEvents()
            ''''''nuevoooooooooooooooooooooooooooooooooooooooooo
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

            Dim Fila As DataRow

            Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
            Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
            Fila("Erazonsoc") = TRazonSocial
            Fila("Eregfis") = TRFiscal.ToString + " - " + descreg.Trim
            Fila("Edireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
            Fila("Edircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString
            Dim RFacImpresas As String
            RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

            Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

            Dim TipoCambio As Int16 = 1
            Dim CondicionesDePago As String = "CONTADO"

            fp = "EFECTIVO" ' "99 - Otros"
            upb.Value += 10 'inicia en 10

            EasyOne_cfdi33_SetCfdi40(p, "01", "")

            Fila("exporta") = "01-No Aplica"

            Select Case TRFiscal
                Case 601
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.GeneralDeLeyPersonasMorales
                    DesRegimen = "General De Ley Personas Morales"
                Case 603
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.PersonasMoralesConFinesNoLucrativos
                    DesRegimen = "Personas Morales Con Fines No Lucrativos"
                Case 605
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.SueldosYSalariosEIngresosAsimiladosASalarios
                    DesRegimen = "Sueldos Y Salarios E Ingresos Asimilados A Salarios"
                Case 606
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.Arrendamiento
                    DesRegimen = "Arrendamiento"
                Case 608
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.DemasIngresos
                    DesRegimen = "Demas Ingresos"
                Case 609
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.Consolidacion
                    DesRegimen = "Consolidacion"
                Case 610
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.ResidentesEnElExtranjeroSinEstablecimientoPermanenteEnMexico
                    DesRegimen = "Residentes En El Extranjero Sin Establecimiento Permanente En Mexico"
                Case 611
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IngresosPorDividendos
                    DesRegimen = "Ingresos Por Dividendos"
                Case 612
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.PersonasFisicasConActividadesEmpresarialesYProfesionales
                    DesRegimen = "Personas Fisicas Con Actividades Empresariales Y Profesionales"
                Case 614
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IngresosPorIntereses
                    DesRegimen = "Ingresos Por Intereses"
                Case 616
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.SinObligacionesFiscales
                    DesRegimen = "Sin Obligaciones Fiscales"
                Case 607
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                    DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
            End Select



            EasyOne_cfdi33_SetInformacionGlobal40(p, "01", f2, f3)
            'Llenamos la sección del Emisor
            EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TRFiscal)
            'EasyOne_cfdi33_SetEmisor(p, TRfc, "POVISA", "601")

            'Llenamos la sección del Receptor
            'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
            EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
            'EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616
            EasyOne_cfdi33_SetReceptor40(p, TCp, "616")

            Dim NumPiezas As Integer
            Cadena = " Fecha Venta: " & txtFechaVenta.Value.ToString.Substring(0, 10) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))

            Fila("sucursal") = Cadena
            Fila("Rcp") = TCp
            Fila("Rrfc") = "XAXX010101000"
            Fila("Rrazonsoc") = "PUBLICO EN GENERAL"
            Fila("Rdireccion") = ""
            Fila("Rregimen") = "616-Sin Obligaciones Fiscales"
            Fila("usocfdi") = "S01"
            Fila("moneda") = "MXN"

            Dim i As Integer
            Dim sb As Double = 0
            Dim des As Double = 0
            Dim iv As Double = 0
            Dim iiv As Double = 0
            Dim PuCa As Double = 0
            Dim Imp As Double = 0
            Dim bas As Double = 0
            Dim des1 As Double = 0
            Dim Descu As Double = 0

            Dim Importe As Double = 0
            Dim Impuesto As String = ""
            Dim TasaOCuota As String = ""
            Dim TipoFactor As String = ""
            Dim Impu As String = ""
            Dim ToC As String = ""
            Dim TFac As String = ""
            Dim dcto As String = ""
            Dim ipt As String = ""

            Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
            Tabla2.Columns.Add("Cantidad")
            Tabla2.Columns.Add("Unidad")
            Tabla2.Columns.Add("UniSAT")
            Tabla2.Columns.Add("Descrip")
            Tabla2.Columns.Add("Punitario")
            Tabla2.Columns.Add("Importe")
            Dim Fila2 As DataRow

            For i = 0 To Me.ugr.Rows.Count - 1
                Dim pun As Double = Format(Me.ugr.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                Dim impt As Double = Format((Me.ugr.Rows(i).Cells("PrecioUnitario").Value) * (Me.ugr.Rows(i).Cells("Cantidad").Value), "###,###.00")
                NumPiezas += Me.ugr.Rows(i).Cells("Cantidad").Value
                Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                Fila2("Cantidad") = "1" 'Trim(Me.ugr.Rows(i).Cells("Cantidad").Value)
                Fila2("Unidad") = "ACT"
                Fila2("UniSAT") = "01010101" ''Trim(Me.ugr.Rows(i).Cells("Unidad").Value)
                Fila2("Descrip") = Trim(Me.ugr.Rows(i).Cells("Descripcion").Value)
                Fila2("Punitario") = pun
                Fila2("Importe") = impt
                DS.Tables(1).Rows.Add(Fila2)
                If ugr.Rows(i).Cells("Descuento").Value <> 0 Then
                    Descu = Format(Me.ugr.Rows(i).Cells("Descuento").Value, "###,###.00")
                    des += Format(Me.ugr.Rows(i).Cells("Descuento").Value, "###,###.00")
                    EasyOne_cfdi33_AddConcepto(p, "01010101", "", "1", "ACT", "", "Venta", Format(pun, "######.00"), Format(impt, "######.00"), Format(Descu, "######.00"))
                    'EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), 1, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "PZA", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                Else
                    EasyOne_cfdi33_AddConcepto(p, "01010101", "", "1", "ACT", "", "Venta", Format(pun, "######.00"), Format(impt, "######.00"), "")
                    'EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), 1, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "PZA", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), "")
                End If
                'Se debe de agregar si el concepto es Objeto de impuesto
                EasyOne_cfdi33_SetConcepto40(p, "02")
                Imp = Format((((Me.ugr.Rows(i).Cells("PrecioUnitario").Value * 1) - Me.ugr.Rows(i).Cells("Descuento").Value) * 0.16), "###,###.00")
                iv += Imp
                bas = Format((((Me.ugr.Rows(i).Cells("PrecioUnitario").Value * 1) - Me.ugr.Rows(i).Cells("Descuento").Value)), "######.00")
                Impu = "002"
                ToC = "0.160000"
                TFac = "Tasa"
                EasyOne_cfdi33_AddTraslado(p, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
                'End With
            Next

            Impuesto = "002"
            TasaOCuota = "0.160000"
            TipoFactor = "Tasa"
            Importe = Format(iv, "###,###.00")

            Dim TotalImpuestosTrasladados As Double = 0
            TotalImpuestosTrasladados = Format(iv, "###,###.00")

            'Agregamos los Impuestos
            EasyOne_cfdi33_SetComprobanteImpuestos(p, "", TotalImpuestosTrasladados)
            EasyOne_cfdi33_AddTrasladosTraslado(p, Impuesto, TipoFactor, TasaOCuota, Importe)

            Dim Total As Double = 0
            Dim Subt As Double = 0
            Subt = Format(Subtotal.Value, "###,###.00")
            Total = Format(((Subt - des) + iv), "###,###.00")
            Fila("Fsubtotal") = Str(FormatCurrency(Subt))
            Fila("Fdescuento") = Str(FormatCurrency(des))
            Fila("Fiva") = Str(FormatCurrency(iv))
            Fila("Ftotal") = Str(FormatCurrency(Total))

            Dim cantletra As String
            cantletra = Letras(Total)
            Fila("cantidadl") = cantletra

            Dim ValorTicket As String = Cadena

            'Dim f1, f2, f3, freal, FeReg, hr As String
            'f1 = Today.ToString.Substring(0, 2)
            'f2 = Today.ToString.Substring(3, 2)
            'f3 = Today.ToString.Substring(6, 4)
            'hr = Now.ToLocalTime.ToString.Substring(11, 8)
            freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
            FeReg = Today.ToString.Substring(6, 4) & f2 & f1
            If des = 0 Then
                EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, "01", "", Format((Subt), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "I", "PUE", TCp, "")
            Else
                EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, "01", "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", "1", Format((Total), "######.00"), "I", "PUE", TCp, "")
            End If
            Fila("metpago") = "PUE"
            Fila("tipocomp") = "I - Ingreso"
            Fila("formpago") = "EFECTIVO"

            'Enviamos el CFDI al metodo de PRUEBAS
            'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
            'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBAS
            EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

            upb.Value += 20  'Suma 30
            Dim x As Integer = EasyOne_FirmaComprobante(p, CerFactura, Key, Pwd)
            Application.DoEvents()

            If x = 0 Then
                Application.DoEvents()

                Dim resultadoXML As String = [String].Empty
                resultadoXML = EasyOne_LeeResultado(p, 8)
                'Obtenemos el XML completo y timbrado
                Console.WriteLine(resultadoXML)

                Application.DoEvents()
                IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml"), resultadoXML)

                upb.Value += 20 'Suma 50
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
                documento.Load(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml")
                VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
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
                    RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml.png")

                    Application.DoEvents()
                    upb.Value += 10 'Suma 60

                    ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

                    DS.Tables(0).Rows.Add(Fila)
                    Dim objreport As New factura40
                    objreport.SetParameterValue("Logo", RutaLogo)
                    objreport.SetParameterValue("QR", RutaQR)
                    Dim rpFact As New factura40
                    rpFact.SetDataSource(DS)
                    rpFact.SetParameterValue("QR", RutaQR)
                    rpFact.SetParameterValue("Logo", RutaLogo)
                    GFactura.CrystalReportViewer1.ReportSource = objreport
                    GFactura.CrystalReportViewer1.ReportSource = rpFact
                    GFactura.WindowState = FormWindowState.Maximized
                    GFactura.Show()

                    rutaArchivoPDF = RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".pdf"
                    rutaArchivoXML = RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml"
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

                    Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF As String
                    Fecha11 = Now.Date.ToString.Substring(0, 2)
                    Fecha22 = Now.Date.ToString.Substring(3, 2)
                    Fecha33 = Now.Date.ToString.Substring(6, 4)
                    FePnotasF = Fecha33 & Fecha22 & Fecha11
                    fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                    FeRegistro = Now.Date.ToString.Substring(6, 4) & Fecha22 & Fecha11
                    Fecha = fechareal

                    Dim Tterm = Trim(Str(grbTienda.Value)) & Trim(Str(grbSucursal.Value))
                    Dim Nnota = CInt(FeRegistro)
                    Dim NotaVta As String = FeRegistro
                    Dim TieSuc As String = Me.grbTienda.Value & Me.grbSucursal.Value
                    Dim PorIva As Double = 16.0

                    objpro.Indicacion("Update Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                    'ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = '" & Trim(Me.txtRfc.Text) & "' ", As400))
                    Dim Tipo As String = "F"
                    Dim cantidadconletra As String = ""
                    Dim FeCan = 0
                    Dim FeMotCan As String = ""
                    Dim ferisr = 0
                    Dim feriva = 0

                    Dim Subtotal = Me.Subtotal.Value
                    Dim Iva = Me.Iva.Value
                    Dim Descuento = Me.Descuento.Value

                    upb.Value += 10 'Suma 80

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
                    MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                    BorrarRegistro()
                    CargaGridConceptos()

                Else
                    MsgBox("Archivo XML no es una factura version 4.0")
                End If 'Cierra respuesta del PAC
            Else
                'En caso de error mostrar el detalle.
                Dim resultadoXML2 As String = [String].Empty
                resultadoXML2 = EasyOne_LeeResultado(p, 11)
                Console.WriteLine(resultadoXML2)
                MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")

            End If 'Cierra cracion de XML
        End If 'Cierra validacion de factura y folio
    End Sub

    Private Sub BorrarRegistro()
        ugr.DataSource = Nothing
        limpiar()
        Importe.Value = "0.0"
        Subtotal.Value = "0.0"
        Descuento.Value = "0.0"
        Iva.Value = "0.0"
        Total.Value = "0.0"

        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
    End Sub

    Private Sub limpiar()
        Me.txtSerie.Text = ""
        Me.txtFactura.Text = ""
    End Sub


    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

End Class