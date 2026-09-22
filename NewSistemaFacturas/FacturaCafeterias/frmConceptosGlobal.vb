Imports System
Imports N400
Imports N400.Framework
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Threading



Public Class frmConceptosGlobal
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim fechventa, FeReg, fp As String
    Dim TIP As String
    Dim Tabla, fGLOBAL, TSucursal, TablaTienda, TabExpeTicket, tablaclientes As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim FolioFinal
    Dim Cadena, invoice, FechaHora As String
    Dim ExisteClave As String
    Dim va As Integer
    Dim b_iva As Double
    Dim hra As String
    Dim serie, folio As String
    Dim va2 As Integer
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

    Private Sub frmConceptosCafeteria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "VG"
        va = 0
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        fechventa = dia + "/" + mes + "/" + amo
        txtFechaVenta.Value = CDate(fechventa)
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Empresa FROM CatTien ", 0, 1, Motor)
        Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from satmpag ", 0, 1, Motor)
        grbPagos.Value = "PUE"
        Application.DoEvents()
        Inicializa()
    End Sub

    Private Sub frmConceptosCafeteria_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
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

    Private Sub DESCUENTO_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DESCUENTO.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.IMPORTE.Focus()
        End If
    End Sub

    Private Sub IMPORTE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles IMPORTE.KeyPress
        If e.KeyChar = Chr(13) Then
            If DESCUENTO.Value > IMPORTE.Value Then
                MsgBox("El DESCUENTO no puede ser mayor al Importe ", MsgBoxStyle.Information, "Facturación.. ")
            Else
                Me.TOTAL.Value = (IMPORTE.Value - DESCUENTO.Value)
                Me.SUBTOTAL.Value = Format((TOTAL.Value / 1.16), "###,###.00") '(impo - ivas)
                Me.IVA.Value = Format((TOTAL.Value - SUBTOTAL.Value), "###,###.00") 'ivas
                FacClientes()
                Me.vta.Value = (TOTAL.Value + facturacte.Value) - Devoluciones.Value
                Me.Devoluciones.Focus()
            End If
        End If

    End Sub

    Private Sub DEVOLUCIONES_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Devoluciones.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.vta.Value = (TOTAL.Value + facturacte.Value) - Devoluciones.Value
        End If
    End Sub


    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If objpro.DatoExiste("FeEstatu", "FEncabez", "tId = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and FeEstatu = 1 and feTip = 'G'  and day(feFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(feFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(feFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) = False Then
            txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
            txtSerie.Text = objpro.Dato("seserie", "serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1", Motor)
        Else
            MsgBox("YA SE ENCUENTRA UNA FACTURA GLOBAL CON ESA FECHA DE VENTA ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub FacClientes()
        facturacte.Value = 0
        If grbTienda.Value = "" Or grbTienda.Value = "" Then
            MsgBox("No ha seleccionado Tienda o Sucursal", MsgBoxStyle.Information, "Facturación.. ")
        Else
            'If objpro.DatoExiste("FeTotal", "FEncabez", "tId = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and FeEstatu = 1 and feTip = 'F' and day(feFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(feFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(feFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) = True Then
            Tabla = objpro.ObtenDatos(" select FeTotal as TOTALCLIE from FEncabez " &
                                   " where tId = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and FeEstatu = 1 and feTip = 'F' and day(feFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(feFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(feFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "  ", Motor).Tables(0)

            If Tabla.Rows.Count <> 0 Then
                For Each r In Tabla.Rows
                    Application.DoEvents()
                    facturacte.Value += Format(CDbl(r("TOTALCLIE").ToString), "###,###.00")
                Next
                'Else
                '    MsgBox("no hay fac de clientes", MsgBoxStyle.Information, "Facturación.. ")
            End If
            'Else
            '    MsgBox("no hay fac de clientes", MsgBoxStyle.Information, "Facturación.. ")
            'End If
        End If
    End Sub

    Private Sub Limpia()
        IMPORTE.Value = "0.0"
        SUBTOTAL.Value = "0.0"
        DESCUENTO.Value = "0.0"
        IVA.Value = "0.0"
        TOTAL.Value = "0.0"

        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        If txtFactura.Text = "" Or txtSerie.Text = "" Then
            MsgBox("La serie y no. de factura no pueden ir vacios ", MsgBoxStyle.Information, "Facturación.. ")
        Else

            fGLOBAL = objpro.ObtenDatos(" select FeTotal as TOTALGlobal from FEncabez " &
                                   " where tId = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and FeEstatu = 1 and feTip = 'G' and day(feFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(feFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(feFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "  ", Motor).Tables(0)

            If fGLOBAL.Rows.Count <> 0 Then
                For Each r In fGLOBAL.Rows
                    Application.DoEvents()
                    Dim vtag As Double = 0
                    vtag = Format(CDbl(r("TOTALGlobal").ToString), "###,###.00")
                    MsgBox("Ya exixste una factura Global del día por el monto: $ " & vtag.ToString & " ", MsgBoxStyle.Information, "Facturación.. ")
                Next
            Else
                Timbra()
            End If
        End If

    End Sub

    Private Sub Timbra()
        Dim Directorios, RutFacClientes, RutFacGlobal, CarpTienda, CarpSucursal, RutNotaCredito As String
        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, rutlogo As String
        If MessageBox.Show("Esta seguro de Generar Factura", "FACTURAS GLOBALES", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            upb.Value = 0
            upb.Maximum = 100
            TablaTienda = objpro.ObtenDatos("Select * from CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
            TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
            TRfc = TablaTienda.Rows(0).Item("trfc").ToString
            TCp = TablaTienda.Rows(0).Item("tcp").ToString
            TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
            descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
            direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
            invoice = Trim(TablaTienda.Rows(0).Item("tuserpac").ToString)

            Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
            RutFacClientes = TablaTienda.Rows(0).Item("sForm1").ToString
            RutFacGlobal = TablaTienda.Rows(0).Item("sForm2").ToString
            RutNotaCredito = TablaTienda.Rows(0).Item("sForm3").ToString
            CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
            CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
            rutlogo = TablaTienda.Rows(0).Item("rutLogo").ToString

            Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
            Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
            Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
            Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
            Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
            Dim FechaFactura As DateTime = Now
            FechaHora = Date.Now.ToString.Substring(10, 10)

            Dim f1, f2, f3 As String
            f1 = txtFechaVenta.Text.ToString.Substring(0, 2)
            f2 = txtFechaVenta.Text.ToString.Substring(3, 2)
            f3 = txtFechaVenta.Text.ToString.Substring(6, 4)
            FeReg = txtFechaVenta.Text.ToString.Substring(6, 4) & f2 & f1


            DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
            PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
            Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
            PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
            SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
            PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
            DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
            AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
            MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString
            If TablaTienda.Rows(0).Item("EMAILP").ToString = Nothing Then
                SeguridadEmail = False
            ElseIf TablaTienda.Rows(0).Item("EMAILP").ToString = 0 Then
                SeguridadEmail = False
            Else
                SeguridadEmail = True
            End If

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
            Fila("Eregfis") = TRFiscal + " - " + descreg.Trim
            Fila("Edireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
            Fila("Edircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

            Dim RFacImpresas As String
            RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

            Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

            Dim TipoCambio As Int16 = 1
            'Dim CondicionesDePago As String = "CONTADO"
            upb.Value += 10

            'EXPORTACION: 01 - NO APLICA
            EasyOne_cfdi33_SetCfdi40(p, "01", "")

            Fila("exporta") = "01-No Aplica"

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
                Case 626
                    TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                    DesRegimen = "Simplificado de confianza"
                Case 621
                    TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                    DesRegimen = "Incorporacion Fiscal"
            End Select


            'EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, "X")
            EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TRFiscal)


            If grbPagos.Value = "PPD" Then
                fp = "POR DEFINIR"  '"99 - Otros"
                EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PÚBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
                EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616

                Cadena = "Suc: " & Trim(CarpSucursal) & " Fecha Venta: " & txtFechaVenta.Value.ToString.Substring(0, 10) & " Nota: " & txtFactura.Text & " - Importe - " & " $" & Str(FormatCurrency(Me.TOTAL.Value))

                Fila("sucursal") = Cadena
                Fila("Rcp") = TCp
                Fila("Rrfc") = "XAXX010101000"
                Fila("Rrazonsoc") = "PÚBLICO EN GENERAL"
                Fila("Rdireccion") = ""
                Fila("Rregimen") = "616-Sin Obligaciones Fiscales"
                Fila("usocfdi") = "S01"
                Fila("moneda") = "MXN"
            Else
                fp = "EFECTIVO"  '"99 - Otros"
                'Llenamos la sección del Emisor
                ''/ Periodicidad 01 Diario| 02 Semanal | 03 Quincenal | 04 Mensual | 05 Bimensual
                ''// Meses 01-12 Enero a Diciembre | 13-18 Enero-Febrero a Noviembre-Diciembre  // Año
                EasyOne_cfdi33_SetInformacionGlobal40(p, "01", f2, f3)
                'Llenamos la sección del Receptor
                'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
                EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616

                Cadena = "Suc: " & Trim(CarpSucursal) & " Fecha Venta: " & txtFechaVenta.Value.ToString.Substring(0, 10) & " Nota: " & txtFactura.Text & " - Importe - " & " $" & Str(FormatCurrency(Me.TOTAL.Value))

                Fila("sucursal") = Cadena
                Fila("Rcp") = TCp
                Fila("Rrfc") = "XAXX010101000"
                Fila("Rrazonsoc") = "PUBLICO EN GENERAL"
                Fila("Rdireccion") = ""
                Fila("Rregimen") = "616-Sin Obligaciones Fiscales"
                Fila("usocfdi") = "S01"
                Fila("moneda") = "MXN"
            End If


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

            'Dim Importe As Double = 0
            Dim Impuesto As String = ""
            Dim TasaOCuota As String = ""
            Dim TipoFactor As String = ""
            Dim Impu As String = ""
            Dim ToC As String = ""
            Dim TFac As String = ""
            Dim ipt As String = ""

            Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
            Tabla2.Columns.Add("Cantidad")
            Tabla2.Columns.Add("Unidad")
            Tabla2.Columns.Add("UniSAT")
            Tabla2.Columns.Add("Descrip")
            Tabla2.Columns.Add("Punitario")
            Tabla2.Columns.Add("Importe")
            Dim Fila2 As DataRow

            Dim impof As Double = 0
            Dim descuf As Double = 0

            impof = Format((IMPORTE.Value / 1.16), "######.00")
            descuf = Format((DESCUENTO.Value / 1.16), "######.00")


            If DESCUENTO.Value <> 0 Then
                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format(impof, "######.00"), Format(impof, "######.00"), Format(descuf, "######.00"))
            Else
                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format(impof, "######.00"), Format(impof, "######.00"), "")
            End If

            Dim totalesf As Double = 0

            totalesf = Format(((impof - descuf) * 1.16), "###,###.00")

            EasyOne_cfdi33_SetConcepto40(p, "02")
            bas = Format((totalesf / 1.16), "######.00")
            Imp = Format((bas * 0.16), "######.00")

            EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))

            Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
            Fila2("Cantidad") = "1"
            Fila2("Unidad") = "ACT"
            Fila2("UniSAT") = Format(descuf, "###,###.00")
            Fila2("Descrip") = "VENTA"
            Fila2("Punitario") = Format(impof, "###,###.00")
            Fila2("Importe") = Format(impof, "###,###.00")
            DS.Tables(1).Rows.Add(Fila2)

            Dim ValorTicket As String = Cadena
            NoCuenta = ""

            'Agregamos los Impuestos
            EasyOne_cfdi33_SetComprobanteImpuestos(p, "", Format(Imp, "######.00"))
            EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.160000", Format(Imp, "######.00"))
            Dim Totaal As Double = 0
            Totaal = Format(TOTAL.Value, "######.00")  ' Format(((Subt - Convert.ToDecimal(Descu)) + iv), "######.00")
            Fila("Fsubtotal") = Format(impof, "###,###.00")
            Fila("Fdescuento") = Format(descuf, "###,###.00")
            Fila("Fiva") = Format(Imp, "###,###.00") ''antes iva
            Fila("Ftotal") = Format(Totaal, "###,###.00")

            Dim cantletra As String
            cantletra = Letras(Totaal)
            Fila("cantidadl") = cantletra
            Dim metpg As String
            Dim tipcom As String
            Dim formpg As String
            metpg = grbPagos.Value ' "PUE"
            tipcom = "I"

            Fila("metpago") = metpg
            If tipcom = "I" Then
                Fila("tipocomp") = "I - Ingreso"
            Else
                Fila("tipocomp") = "E - Egreso"
            End If

            If grbPagos.Value = "PPD" Then
                formpg = "99"
                Fila("formpago") = "POR DEFINIR"
            Else
                formpg = "01"
                Fila("formpago") = "EFECTIVO"
            End If

            'Timbra XML, que se encuentra en variable P
            Dim freal, hr As String
            Dim x As Integer = 99
            If va = 0 Then
                hr = Now.ToLocalTime.ToString.Substring(11, 8)

                If hra <> "" Then
                    MsgBox("Intenta tmbrar nuevamente pero con otra hora !!! 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                    hr = hra
                End If

                freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

                If (serie = "" And folio = "") Or (folio <> txtFactura.Text) Then

                    If DESCUENTO.Value = 0 Then
                        EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, formpg, "", Format((impof), "######.00"), "", "MXN", TipoCambio, Format((totalesf), "######.00"), tipcom, metpg, TCp, "")
                    Else
                        EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, formpg, "", Format((impof), "######.00"), Format((descuf), "######.00"), "MXN", TipoCambio, Format((totalesf), "######.00"), tipcom, metpg, TCp, "")
                    End If

                    Dim folf As Integer = 0 'consulto el ultimo folio de serie
                    folf = CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor))
                    If CInt(Me.txtFactura.Text) > folf Then
                        EasyOne_SetInvoiceOneFachadaInfoPrueba(p, invoice, invoice)
                        'EasyOne_SetInvoiceOneFachadaInfo(p, invoice, invoice)
                        Application.DoEvents()
                        upb.Value += 30  'Suma 40

                        Dim b As Integer
                        b = p
                        x = EasyOne_FirmaComprobante(b, CerFactura, Key, Pwd)
                        p = b
                        b = 0
                        Thread.Sleep(10000) 'espera por 10 segundos


                        If x = 0 Then
                            Try
                                objpro.Indicacion("Update Serie set seCant = " & CInt(txtFactura.Text) & " Where seSerie = '" & txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                            Catch ex As Exception
                                MsgBox("Actualiza Serie de Timbrado " & ex.ToString, MsgBoxStyle.Information, "Factura Electronica")
                            End Try

                            Dim resultadoXML As String = [String].Empty
                            resultadoXML = EasyOne_LeeResultado(p, 8)
                            'Obtenemos el XML completo y timbrado
                            Console.WriteLine(resultadoXML)

                            IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml"), resultadoXML)
                            upb.Value += 10 'Suma 50
                            va += 1
                            hra = hr
                            serie = txtSerie.Text
                            folio = txtFactura.Text

                            'Si el resultado es 0 = El CFDI se procesó correctamente
                            If x = 0 And va = 1 Then

                                upb.Value += 20 'Suma 70
                                Dim archivoXMLPDF As String = ""
                                Dim archivoREPX As String = ""
                                Dim archivoPDF As String = ""

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
                                documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml"))
                                VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                                VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                                Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
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

                                    DS.Tables(0).Rows.Add(Fila)
                                    Dim objreport As New Global40
                                    objreport.SetParameterValue("Logo", RutaLogo)
                                    objreport.SetParameterValue("QR", RutaQR)
                                    Dim rpFact As New Global40
                                    rpFact.SetDataSource(DS)
                                    rpFact.SetParameterValue("QR", RutaQR)
                                    rpFact.SetParameterValue("Logo", RutaLogo)
                                    Glabal.CrystalReportViewer1.ReportSource = objreport
                                    Glabal.CrystalReportViewer1.ReportSource = rpFact
                                    Glabal.WindowState = FormWindowState.Maximized
                                    Glabal.Show()

                                    upb.Value += 5 'Suma 75
                                    Dim Ruta As String = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".pdf")
                                    rutaArchivoXML = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg & ".xml")
                                    AsuntoEmail = "FACTURA CORRESPONDIENTE A: " & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & "-" & FeReg

                                    Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                                    Try
                                        Dim CrExportOptions As ExportOptions
                                        Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                                        Dim crf As New PdfRtfWordFormatOptions()
                                        CrDiskFileDestinationOptions.DiskFileName = Ruta
                                        CrExportOptions = rpFact.ExportOptions
                                        With CrExportOptions
                                            .ExportDestinationType = ExportDestinationType.DiskFile
                                            .ExportFormatType = ExportFormatType.PortableDocFormat
                                            .DestinationOptions = CrDiskFileDestinationOptions
                                            .FormatOptions = CrFormatTypeOptions
                                        End With
                                        rpFact.Export()
                                        rpFact.Close()
                                        rpFact.Dispose()
                                        upb.Value += 5 'Suma 80  
                                    Catch ex As Exception
                                        MsgBox(ex.ToString)
                                    End Try

                                    Application.DoEvents()
                                    upb.Value += 10 'Suma 90

                                    ''''''''''''''''''''''''''''''''''''''''''''''''''''
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


                                    ExisteClave = CStr(objpro.DatoString("cid", "catclie", "crfc = 'XAXX010101000' ", Motor))
                                    Dim Tipo As String = "G"
                                    Dim cantidadconletra As String = ""
                                    Dim FeCan = 0
                                    Dim FeMotCan As String = ""
                                    Dim ferisr = 0
                                    Dim feriva = 0

                                    Dim Subtotal = Me.SUBTOTAL.Value
                                    Dim Iva = Me.IVA.Value
                                    Dim Descuento = Me.DESCUENTO.Value

                                    Dim feAbonos As Double = 0

                                    objpro.Indicacion("Insert into FEncabez (FeId, tId, FeTerm, FeFact, feSerie, sid, feFecha, feHora, Cid, FeSubt, fePorIva, FeIva, " &
                                                        "FeDesc1, FePDesc1, FeDesc2, FePDesc2, FeTotal, FeCanlet, FeAbonos, FeEstatu, FeReg, feTip, feuuid, fformpag) " &
                                                        "values (" & FeRegistro & ", " & Me.grbTienda.Value & ", " & Tterm & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "', " &
                                                        " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ", '" & Subtotal & "', " & PorIva & ",'" & Iva & "', '" & Descuento & "','" & Descuento & "', '" & Descuento & "', '" & Descuento & "', " &
                                                        " '" & Me.TOTAL.Value & "', '" & cantletra & "', " & feAbonos & ", 1 ,'" & FeRegistro & "', '" & Tipo & "', '" & uuid & "', '" & formpg & "')", Motor)

                                    Application.DoEvents()
                                    ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

                                    Dim dev As Double = 0
                                    If Devoluciones.Value <> 0 Then
                                        Application.DoEvents()
                                        dev = Devoluciones.Value
                                        NotaCredito(freal, TRfc, TRFiscal, Directorios, CerFactura, Key, Pwd, uuid, TCp)
                                        'NotaCredito(freal, TRfc, TRFiscal, Directorios, CerFactura, Key, Pwd, " 689774CD-B353-4AB5-B3DD-7C719161367E", TCp)
                                    End If
                                    Dim mail As String
                                    mail = objpro.DatoString("cEmail", "catclie", "crfc = 'XAXX010101000' ", Motor)
                                    Dim correo As New System.Net.Mail.MailMessage()
                                    correo.From = New System.Net.Mail.MailAddress(DirEmail)
                                    correo.To.Add(Trim(mail))

                                    correo.Subject = Trim(AsuntoEmail)
                                    correo.Body = Trim(MsjEmail.Trim)
                                    correo.Body += Environment.NewLine & Environment.NewLine
                                    correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
                                    correo.Body += Environment.NewLine & "GRUPO VIGIL. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                                    correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                                    correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                                    correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                                    correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                                    correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                                    correo.Body += " ó a través de una solicitud vía correo electrónico a boutique@grupovigil.com.mx"

                                    correo.IsBodyHtml = False
                                    correo.Priority = System.Net.Mail.MailPriority.Normal
                                    Dim Archivo As New System.Net.Mail.Attachment(Ruta)
                                    For i = 0 To 1
                                        If i = 0 Then
                                            Archivo = New System.Net.Mail.Attachment(Ruta)
                                            upb.Value += 5 'Suma 95 ????
                                        Else
                                            Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                                            upb.Value += 5 'Suma 100
                                        End If

                                        correo.Attachments.Add(Archivo)
                                    Next
                                    Dim smtp As New System.Net.Mail.SmtpClient
                                    smtp.Host = SmtpHost
                                    smtp.Port = Int(PuertoSmtp)
                                    smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                                    smtp.EnableSsl = SeguridadEmail

                                    Application.DoEvents()
                                    smtp.Send(correo)


                                    ''''''''

                                    'Ejecuta el archivo .exe especificado
                                    Try
                                        Shell("C:\aplicacion\appcliente.exe") ' ejecutable de Gonzalez para seguimiento de facturacion
                                    Catch ex As Exception
                                        MessageBox.Show("Error al ejecutar el archivo Appcliente.exe: " & ex.Message)
                                    End Try
                                    MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                                    btnFacturar.Visible = True
                                    Inicializa()
                                    ''''''nuevoooooooooooooooooooooooooooooooooooooooooo
                                Else
                                    MsgBox("Archivo XML no es una factura version 4.0")
                                End If

                            Else
                                'En caso de error mostrar el detalle.
                                Dim resultadoXML2 As String = [String].Empty
                                resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                Console.WriteLine(resultadoXML2)
                                MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                btnFacturar.Visible = True
                            End If

                        Else
                            btnFacturar.Visible = True
                            Dim resultadoXML2 As String = [String].Empty
                            resultadoXML2 = EasyOne_LeeResultado(p, 11)
                            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                            hra = hr
                        End If
                    End If
                Else
                    btnFacturar.Visible = True
                    MsgBox("La factura ya esta realizada !!! ", MsgBoxStyle.Information).ToString()
                End If
            Else
                btnFacturar.Visible = True
                MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
            End If
        Else
            btnFacturar.Visible = True
            MsgBox("Archivo ya Timbrado !!! No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
        End If
        'Else
        '    btnFacturar.Visible = True
        'End If 'Cierra validación en la pregunta del msgbox
    End Sub


    Public Function NotaCredito(ByVal fechaNC As String, ByVal n_TRfc As String, ByVal n_TRFiscal As String, ByVal n_Directorios As String, ByVal cer As String, ByVal key As String,
                                ByVal psw As String, ByVal n_uuid As String, n_LExpedicion As String)
        Dim n_serie As String
        Dim n_notacredito As String
        DesRegimen = ""
        Dim FechaFactura As DateTime = Now
        'NumPiezas = 0
        'td = 2

        TIP = "NC"

        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, RutFac As String
        TablaTienda = objpro.ObtenDatos("Select * from CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
        TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
        TRfc = TablaTienda.Rows(0).Item("trfc").ToString
        TCp = TablaTienda.Rows(0).Item("tcp").ToString
        TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
        RutFac = TablaTienda.Rows(0).Item("sForm1").ToString

        TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
        n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
        n_serie = TSucursal.Rows(0).Item("Seserie").ToString

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
        Fila("Nrfc") = n_TRfc  ' Acá insertas valores a la nueva fila
        Fila("Nrazonsoc") = TRazonSocial
        Fila("Nregfis") = TRFiscal + " - " + descreg.Trim
        Fila("Ndireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
        Fila("Ndircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

        Dim RFacImpresas2 As String
        RFacImpresas2 = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
        Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg & ".xml")

        Dim TipoCambio As Int16 = 1
        Fila("Nexporta") = "01-No Aplica"
        '(1)
        'EXPORTACION: 01 - NO APLICA
        EasyOne_cfdi33_SetCfdi40(pN, "01", "")

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

        'Dim sbt As Double = 0
        Dim Pu As Double = Format((Me.Devoluciones.Value / 1.16), "###,###.00")
        'sbt = Format(Pu, "###,###.00")

        Fila("Ntipocomp") = "EGRESO"
        Fila("Nmetpag") = metpg


        'Tipo de relacion
        EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")
        'CFDI Relacionado
        EasyOne_cfdi33_AddCfdiRelacionado(pN, n_uuid)

        'c.Moneda = CFDI.CFDI33.c_Moneda.MXN
        fp = "EFECTIVO"

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
            Case 626
                TpRegimen = c_RegimenFiscal.SinObligacionesFiscales
                DesRegimen = "Simplificado de Confianza"
            Case 607
                TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
        End Select


        'Llenamos la sección del Emisor
        EasyOne_cfdi33_SetEmisor(pN, n_TRfc, TRazonSocial, TRFiscal)

        'Llenamos la sección del Receptor
        'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
        EasyOne_cfdi33_SetReceptor(pN, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
        EasyOne_cfdi33_SetReceptor40(pN, n_LExpedicion, "616") 'regimen fiscal para global 616

        'Dim Pu As Double = Format((Me.Devoluciones.Value / 1.16), "###,###.00")
        Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")
        Dim descrip As String = ""
        descrip = "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Pu, "###,###.00")

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
        Dim Tabla4 As DataTable = DSNC.Tables.Add("DatNota")
        Tabla4.Columns.Add("Ncantidad")
        Tabla4.Columns.Add("Nunidad")
        Tabla4.Columns.Add("NuniSAT")
        Tabla4.Columns.Add("Ndescip")
        Tabla4.Columns.Add("Npunitario")
        Tabla4.Columns.Add("Nimporte")
        Tabla4.Columns.Add("NclaveSAT")
        Dim Fila3 As DataRow

        EasyOne_cfdi33_AddConcepto(pN, "84111506", FeReg.ToString, "1", "ACT", "SERVICIO", descrip.Trim, Pu, Pu, "")
        EasyOne_cfdi33_SetConcepto40(pN, "02")

        Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
        Fila3("Ncantidad") = "1"
        Fila3("Nunidad") = "SERVICIO"
        Fila3("NuniSAT") = "ACT"
        Fila3("Ndescip") = descrip.Trim
        Fila3("Npunitario") = Format(Pu, "###,###.00")
        Fila3("Nimporte") = Format(Pu, "###,###.00")
        Fila3("NclaveSAT") = "84111506"
        DSNC.Tables(1).Rows.Add(Fila3)

        'If Iva.Value <> 0 Then
        bas = Format(Pu, "###,###.00")
        Imp = Format((Pu * 0.16), "###,###.00")
        iv += Imp
        Impu = "002"
        ToC = "0.160000"
        TFac = "Tasa"
        EasyOne_cfdi33_AddTraslado(pN, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
        'End If

        Dim total As Double = 0
        total = Format((Pu + iv), "###,###.00")

        Impuesto = "002"
        TasaOCuota = "0.160000"
        TipoFactor = "Tasa"
        Import = Format(iv, "###,###.00")

        Dim TotalImpuestosTrasladados As Double = 0
        TotalImpuestosTrasladados = Format(iv, "###,###.00")

        EasyOne_cfdi33_SetComprobanteImpuestos(pN, "", TotalImpuestosTrasladados)
        EasyOne_cfdi33_AddTrasladosTraslado(pN, Impuesto, TipoFactor, TasaOCuota, Import)

        Fila("Nfsubtotal") = Format(Pu, "###,###.00")
        'Fila("Nfdescuento") = ""
        Fila("Nfiva") = Format(iv, "###,###.00")
        Fila("Nftotal") = Format(total, "###,###.00")

        Dim cantl As String
        cantl = Letras(total)
        Fila("Ncantidadl") = cantl

        'Timbra XML, que se encuentra en variable P
        Dim nc As Integer = 99
        If va2 = 0 Then
            'Llenamos la sección del comprobante (2)

            If (serieNC = "" And folioNC = "") Or (n_notacredito <> folioNC) Then
                EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, fechaNC, formpg, "", Pu.ToString, "", "MXN", TipoCambio, total.ToString, tipcom, metpg, n_LExpedicion, "")

                Dim folf As Integer = 0 'consulto el ultimo folio de serie
                folf = CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor))
                If CInt(Me.txtFactura.Text) > folf Then
                    'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, invoice, invoice)
                    EasyOne_SetInvoiceOneFachadaInfo(pN, invoice, invoice)

                    Dim bN As Integer
                    bN = pN
                    nc = EasyOne_FirmaComprobante(bN, cer, key, psw)
                    pN = bN
                    bN = 0
                    Thread.Sleep(10000) 'espera por 10 segundos
                    If nc = 0 Then

                        objpro.Indicacion("Update Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)

                        Dim resultadoXMLNC As String = [String].Empty
                        'Obtenemos el XML completo y timbrado
                        resultadoXMLNC = EasyOne_LeeResultado(pN, 8)

                        Console.WriteLine(resultadoXMLNC)

                        IO.File.WriteAllText(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg & ".xml"), resultadoXMLNC)

                        va2 += 1
                        serieNC = n_serie
                        folioNC = n_notacredito

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

                            Dim nav As XPathNavigator
                            Dim documentoNC As XmlDocument = New XmlDocument()
                            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documentoNC.NameTable)
                            documentoNC.Load(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg & ".xml"))
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

                                RutaQRN = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg & ".xml.png")

                                ''''''''''''''''''''''''''''''''''''''''''''''''''''
                                '''
                                Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF As String
                                Fecha11 = Now.Date.ToString.Substring(0, 2)
                                Fecha22 = Now.Date.ToString.Substring(3, 2)
                                Fecha33 = Now.Date.ToString.Substring(6, 4)
                                FePnotasF = Fecha33 & Fecha22 & Fecha11
                                fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                                FeRegistro = Now.Date.ToString.Substring(6, 4) & Fecha22 & Fecha11

                                'Dim r_sub As Double = SUBTOTAL.Value
                                Dim r_des As Double = 0 'DESCUENTO.Value
                                'Dim r_iva As Double = IVA.Value
                                'Dim r_total As Double = total
                                Dim FeFecCan As Date = "01/01/1900"
                                Dim FeMotcan As String = ""

                                objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha," &
                                " feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                " FeCanlet,FeAbonos,FeEstatu,FeReg, FeFecCan,FeMotcan, feTip,feuuid, fformpag) " &
                                " values (" & CInt(Transaccion) & "," & grbTienda.Value & "," & Me.grbTienda.Value & Me.grbSucursal.Value & "," & CInt(n_notacredito) & ",'" & n_serie & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," &
                                " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ",'" & Pu & "', 16,'" & iv & "'," &
                                " '" & r_des & "','" & r_des & "', '" & r_des & "', '" & r_des & "', '" & total & "', " &
                                " '" & cantl & "', 0, 1 ," & FeRegistro & ",  " & FeFecCan & ", '" & FeMotcan & "', 'N','" & n_uuid & "', " & formpg & ")", Motor)
                                '''''nuevoooooooooooooooooooooooooooooooooooooooooo

                                DSNC.Tables(0).Rows.Add(Fila)
                                Dim objrep As New NotaCredito
                                objrep.SetParameterValue("NLOGO", RutaLogo)
                                objrep.SetParameterValue("NQR", RutaQRN)
                                Dim NotaC As New NotaCredito
                                NotaC.SetDataSource(DSNC)
                                NotaC.SetParameterValue("NQR", RutaQRN)
                                NotaC.SetParameterValue("NLOGO", RutaLogo)
                                GNota.CrystalReportViewer1.ReportSource = objrep
                                GNota.CrystalReportViewer1.ReportSource = NotaC
                                GNota.WindowState = FormWindowState.Maximized
                                GNota.Show()

                                Dim Ruta As String = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg & ".pdf")
                                rutaArchivoXML = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg & ".xml")
                                AsuntoEmail = "NOTA DE CREDITO REFERENTE A: " & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg
                                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                                Dim cryRpt As New ReportDocument

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
                                '''
                                Dim mail As String
                                mail = objpro.DatoString("cEmail", "catclie", "crfc = 'XAXX010101000' ", Motor)
                                Dim correo As New System.Net.Mail.MailMessage()
                                correo.From = New System.Net.Mail.MailAddress(DirEmail)
                                correo.To.Add(Trim(mail.Trim()))

                                correo.Subject = Trim(AsuntoEmail)
                                correo.Body = Trim(MsjEmail)
                                correo.Body += Environment.NewLine & Environment.NewLine
                                correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
                                correo.Body += Environment.NewLine & "GRUPO VIGIL. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                                correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                                correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                                correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                                correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                                correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                                correo.Body += " ó a través de una solicitud vía correo electrónico a boutique@grupovigil.com.mx"

                                correo.IsBodyHtml = False
                                correo.Priority = System.Net.Mail.MailPriority.Normal
                                Dim Archivo As New System.Net.Mail.Attachment(Ruta)
                                For i = 0 To 1
                                    If i = 0 Then
                                        Archivo = New System.Net.Mail.Attachment(Ruta)
                                    Else
                                        Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                                    End If
                                    correo.Attachments.Add(Archivo)
                                Next
                                Dim smtp As New System.Net.Mail.SmtpClient
                                smtp.Host = SmtpHost
                                smtp.Port = Int(PuertoSmtp)
                                smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                                smtp.EnableSsl = SeguridadEmail
                                Application.DoEvents()
                                smtp.Send(correo)
                                MsgBox("La Nota de Credito se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                            Else
                                MsgBox("Archivo XML no es una factura version 4.0")
                            End If
                        Else
                            Console.WriteLine(EasyOne_LeeResultado(pN, 8))
                            Console.WriteLine(EasyOne_LeeResultado(pN, 11))

                            Console.WriteLine("*******************************")
                            Console.WriteLine("*********** ERROR *************")
                            Console.WriteLine("*******************************")

                            Console.ReadLine()
                            Dim resultadoXML2 As String = [String].Empty
                            resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ERROR AL GENERAR LA NOTA DE CREDITO ")
                            MsgBox("ERROR AL GENERAR LA NOTA DE CREDITO 4.0")
                        End If
                    Else
                        Dim resultadoXML2 As String = [String].Empty
                        resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                        'MsgBox("No se timbro archivo, contactar DEPTO. DE SISTEMAS, XML MAL No. de Veces de Timbrado " & va2, MsgBoxStyle.Critical).ToString()
                    End If

                End If
            Else
                    MsgBox("La NOTA DE CREDITO ya esta YA ESTABA REALIZADA !!! ", MsgBoxStyle.Information).ToString()
                End If

            Else
                MsgBox("Intenta tmbrar nuevamente la Nota de crédito con el mismo FOLIO Y SERIE !!! Serie: " & serieNC & "   Folio: " & folioNC, MsgBoxStyle.Information).ToString()
            End If

        Return ""
    End Function


    'Private Sub limpiardato()
    '    Me.txtSerie.Text = ""
    '    Me.txtFactura.Text = ""
    'End Sub

    Private Sub Inicializa()
        txtFactura.Text = ""
        txtSerie.Text = ""
        TOTAL.Value = 0.0
        IMPORTE.Value = 0.0
        DESCUENTO.Value = 0.0
        SUBTOTAL.Value = 0.0
        IVA.Value = 0.0
        Devoluciones.Value = 0.0
        facturacte.Value = 0.0
        vta.Value = 0.0

    End Sub


    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

End Class