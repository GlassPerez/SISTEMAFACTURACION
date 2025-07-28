Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Xml.XPath

Public Class frmNotaCreditoBosco

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim fechventa As String
    Dim FechaFactura As String
    Dim va2 As Integer
    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda, datosCliente As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim FecFactura As String
    Dim Cid As Integer

    Dim Timbrado As Boolean = False
    Dim contador As Integer = 0
    Dim Cadena As String
    Dim NoCuenta As String
    Dim ExisteClave As String
    Dim NumPiezas As Integer
    Dim FechaBusqueda As String
#End Region

#Region "sat"
    Dim TpRegimen As String
    Dim DesRegimen As String
    Dim fp As String
    Dim n_TRFiscal As String
#End Region

    Private Sub frmNotaCreditoBosco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        va2 = 0
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM bgxfacelec.CatTien where ttipo3 = 'G' ", 0, 1, As400)
        Application.DoEvents()
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

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM bgxfacelec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        Application.DoEvents()
    End Sub

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        objpro.CargaCombos(grbTipoFactura, "select seserie, sedesc from bgxfacelec.serie where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", 0, 1, As400)
        Inicializa()
        grbTipoFactura.Text = ""
        grbTipoFactura.Focus()
    End Sub

    Private Sub grbTipoFactura_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTipoFactura.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTipoFactura
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        TablaTienda = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", As400).Tables(0)
        Dim fecini, fecfin As String
        FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
        fecini = FechaBusqueda & "01"
        fecfin = FechaBusqueda & DateTime.DaysInMonth(grbPeriodo.Text, objfun.EscojeMes(Me.grbMes.Text))
        objpro.LLenaGrid(Me.Ugr, "Select fefact as Factura, feuuid as UUID, fetotal as Total, feterm as Terminal, feid as folio, fereg as Fecha, cid as cliente  from bgxfacelec.fencabez where tid = " & grbTienda.Value & " and " &
                         " sid = " & grbSucursal.Value & " and fetip in ('F', 'G') and feserie = '" & grbTipoFactura.Value & "' and FeReg >= " & fecini & " and FeReg <= " & fecfin & " and feestatu = 1 and fefeccan <> ''  ", "bgxfacelec", As400)
        Me.Ugr.DataBind()
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 400
            .DisplayLayout.Bands(0).Columns(2).Width = 100
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Try
            txtId.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
            txtuuid.Text = Ugr.ActiveRow.Cells("uuid").Value.ToString
            txtImporte.Text = Format(Ugr.ActiveRow.Cells("Total").Value, "###,###.00")
            Terminal = Ugr.ActiveRow.Cells("Terminal").Value.ToString
            Transaccion = Ugr.ActiveRow.Cells("Folio").Value.ToString
            FecFactura = Ugr.ActiveRow.Cells("Fecha").Value.ToString
            Cid = Ugr.ActiveRow.Cells("Cliente").Value.ToString
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub Inicializa()
        Terminal = 0
        Transaccion = 0
        txtId.Text = "ID"
        txtuuid.Text = "UUID"
        txtImporte.Text = "$$"
        Ugr.DataSource = Nothing
    End Sub

    Private Sub grbTipoFactura_ValueChanged(sender As Object, e As EventArgs) Handles grbTipoFactura.ValueChanged
        Inicializa()
    End Sub

    Private Sub btnGenerarFactura_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        btnGenerarFactura.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
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
            FechaSeFacturo = FecFactura.Substring(6, 2) & "/" & FecFactura.Substring(4, 2) & "/" & FecFactura.Substring(0, 4)

            Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
            Dim n_serie As String
            Dim n_notacredito As String
            Dim n_Directorios As String
            Dim n_TRazonSocial As String
            Dim n_TRfc As String
            Dim n_LExpedicion As String
            Dim n_crfc As String
            Dim n_cRazonSocial As String
            Dim n_cDireccion As String
            Dim n_cComDir As String
            Dim n_cCP As String
            Dim n_cUCFDI As String
            Dim n_cregFiscal As String

            upb.Value = 0
            upb.Maximum = 100

            TIP = "NC"
            TSucursal = objpro.ObtenDatos("select * from bgxfacelec.Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400).Tables(0)
            n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")
            n_serie = TSucursal.Rows(0).Item("Seserie").ToString
            Dim rutlogo As String = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO_BOSCO.png"
            Application.DoEvents()

            n_TRazonSocial = objpro.Dato("trsocial", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
            n_TRfc = objpro.Dato("trfc", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
            n_LExpedicion = objpro.Dato("tcp", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)

            datosCliente = objpro.ObtenDatos("select * from BGXFACELEC.catclie Where cid = " & Cid & "", As400).Tables(0)
            n_crfc = datosCliente.Rows(0).Item("crfc").ToString
            n_cRazonSocial = datosCliente.Rows(0).Item("crsocial").ToString
            n_cDireccion = datosCliente.Rows(0).Item("cDir").ToString.Trim & " " & datosCliente.Rows(0).Item("cNumExt").ToString.Trim & " " & datosCliente.Rows(0).Item("cNumInt").ToString.Trim & " " & datosCliente.Rows(0).Item("cColonia").ToString.Trim
            n_cComDir = datosCliente.Rows(0).Item("cMun").ToString.Trim & " " & datosCliente.Rows(0).Item("cEstado").ToString.Trim & " " & datosCliente.Rows(0).Item("cLocal").ToString.Trim & " " & datosCliente.Rows(0).Item("cPais").ToString.Trim
            'n_cCP = datosCliente.Rows(0).Item("cCP").ToString
            n_cCP = objpro.Dato("cCP", "bgxfacelec.catclie", "cid = " & Cid & "", As400)
            If n_cCP.Trim.Length = 4 Then
                n_cCP = "0" & n_cCP
            End If
            n_cUCFDI = datosCliente.Rows(0).Item("cusocfdi").ToString
            n_cregFiscal = datosCliente.Rows(0).Item("cconfecr").ToString 'REGIMEN FISCAL DEL CLIENTE
            'n_desRF = datosCliente.Rows(0).Item("cRegFisDes").ToString

            If n_cDireccion.Trim <> "" And n_cComDir.Trim <> "" And n_cCP.Trim <> "" And n_cUCFDI.Trim <> "" And n_cregFiscal.Trim <> "" Then

                Dim n_RutNotaCredito As String = objpro.Dato("sForm3", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
                Dim CerFactura As String = objpro.Dato("sRCer1", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
                Dim Key As String = objpro.Dato("sRKey", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
                Dim Pwd As String = objpro.Dato("sPwd", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)

                Dim RFacImpresas As String
                n_Directorios = objpro.Dato("subic", "bgxfacelec.catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", As400)
                RFacImpresas = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
                'Dim c As New CFDI.CFDI33.Comprobante(Today, n_serie, n_notacredito, c_TipoDeComprobante.Egreso, c_FormaPago.Otros_99, c_MetodoPago.PagoEnUnaSolaExhibicion_PUE)

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
                Fila("Ndireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
                Fila("Ndircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

                Dim RFacImpresas2 As String
                RFacImpresas2 = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
                Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")

                Dim TipoCambio As Int16 = 1
                Fila("Nexporta") = "01-No Aplica"
                '(1)
                'EXPORTACION: 01 - NO APLICA
                EasyOne_cfdi33_SetCfdi40(pN, "01", "")

                Dim metpg As String
                Dim tipcom As String
                Dim formpg As String
                metpg = "PUE"
                tipcom = "E"
                formpg = "01"

                'Dim sbt As Double = 0
                Dim Pu As Double = Format((txtImporte.Text / 1.16), "###,###.00")
                'sbt = Format(Pu, "###,###.00")

                Fila("Ntipocomp") = tipcom + " - EGRESO"
                Fila("Nmetpag") = metpg
                Fila("Nformpag") = formpg + " - EFECTIVO"


                'Tipo de relacion
                EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")
                'CFDI Relacionado
                EasyOne_cfdi33_AddCfdiRelacionado(pN, txtuuid.Text)
                'c.Moneda = CFDI.CFDI33.c_Moneda.MXN
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

                Fila("Nregfis") = n_TRFiscal + " - " + DesRegimen.Trim 'sucursal

                'Llenamos la sección del Emisor
                EasyOne_cfdi33_SetEmisor(pN, n_TRfc, n_TRazonSocial, n_TRFiscal)
                'Fila("Nusocfdi") = "I01" 'NO VA PARA EL EMISOR

                'Llenamos la sección del Receptor
                'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                EasyOne_cfdi33_SetReceptor(pN, n_crfc, n_cRazonSocial, "", "", n_cUCFDI) 'CAMBIAR PARA EL CLIENTE'estos paramentros son si o si para factura global
                EasyOne_cfdi33_SetReceptor40(pN, n_cCP, n_cregFiscal) 'CAMBIAR POR EL REGIMEN DEL CLIENTE 'regimen fiscal para global 616

                Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")
                Dim descrip As String = ""
                descrip = "Devoluciones del día " & FechaSeFacturo.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & txtuuid.Text & " con un importe de $ " & Format(Pu, "###,###.00")


                Fila("Nrcp") = n_cCP
                Fila("Nrrfc") = n_crfc '"XAXX010101000"
                Fila("Nrrazonsoc") = n_cRazonSocial '"PUBLICO EN GENERAL"
                Fila("Nrdireccion") = n_cDireccion
                Fila("Nrregimen") = n_cregFiscal '& " - " & n_desRF '"616-Sin Obligaciones Fiscales"
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
                Dim Tabla4 As DataTable = DSNC.Tables.Add("DatNota")
                Tabla4.Columns.Add("Ncantidad")
                Tabla4.Columns.Add("Nunidad")
                Tabla4.Columns.Add("NuniSAT")
                Tabla4.Columns.Add("Ndescip")
                Tabla4.Columns.Add("Npunitario")
                Tabla4.Columns.Add("Nimporte")
                Tabla4.Columns.Add("NclaveSAT")
                Dim Fila3 As DataRow

                EasyOne_cfdi33_AddConcepto(pN, "84111506", "100", "1", "ACT", "SERVICIO", descrip.Trim, Pu, Pu, "")
                EasyOne_cfdi33_SetConcepto40(pN, "02")

                Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
                Fila3("Ncantidad") = "1"
                Fila3("Nunidad") = "SERVICIO"
                Fila3("NuniSAT") = "ACT"
                Fila3("Ndescip") = descrip.Trim
                Fila3("Npunitario") = Pu
                Fila3("Nimporte") = Pu
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

                Dim total As Double = 0
                total = Format((Pu + iv), "###,###.00")

                Dim f1, f2, f3, freal, FeReg, hr As String
                Dim fechaventa As DateTime = Now.Date

                f1 = fechaventa.ToString.Substring(0, 2)
                f2 = fechaventa.ToString.Substring(3, 2)
                f3 = fechaventa.ToString.Substring(6, 4)
                hr = Now.ToLocalTime.ToString.Substring(11, 8)
                freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                'freal = "2023" & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                'Llenamos la sección del comprobante (2)
                EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, freal, formpg, "", Pu.ToString, "", "MXN", TipoCambio, total.ToString, tipcom, metpg, n_LExpedicion, "")

                Impuesto = "002"
                TasaOCuota = "0.160000"
                TipoFactor = "Tasa"
                Import = Format(iv, "###,###.00")

                Dim TotalImpuestosTrasladados As Double = 0
                TotalImpuestosTrasladados = Format(iv, "###,###.00")

                EasyOne_cfdi33_SetComprobanteImpuestos(pN, "", TotalImpuestosTrasladados)
                EasyOne_cfdi33_AddTrasladosTraslado(pN, Impuesto, TipoFactor, TasaOCuota, Import)

                Fila("Nfsubtotal") = Str(FormatCurrency(Pu))
                'Fila("Nfdescuento") = ""
                Fila("Nfiva") = Str(FormatCurrency(iv))
                Fila("Nftotal") = Str(FormatCurrency(total))  'Pu.ToString

                Dim cantl As String
                cantl = Letras(total)
                Fila("Ncantidadl") = cantl

                'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBA
                EasyOne_SetInvoiceOneFachadaInfo(pN, "RHUUMRGL", "RHUUMRGL")

                'Timbra XML, que se encuentra en variable P
                Dim nc As Integer = 99
                If va2 = 0 Then
                    nc = EasyOne_FirmaComprobante(pN, CerFactura, Key, Pwd)
                    If nc = 0 Then
                        va2 += 1
                    Else
                        Dim resultadoXML2 As String = [String].Empty
                        resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                        MsgBox("No se timbro archivo, contactar DEPTO. DE SISTEMAS, XML MAL" & " " & " No. de Veces de Timbrado " & va2, MsgBoxStyle.Critical).ToString()
                    End If
                Else
                    MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va2, MsgBoxStyle.Information).ToString()
                End If
                Application.DoEvents()

                upb.Value += 30  'Suma 40
                If nc = 0 And va2 = 1 Then
                    'If Not resultado.Errores Then
                    Application.DoEvents()
                    upb.Value += 10 'Suma 50
                    Dim resultadoXMLNC As String = [String].Empty

                    'Obtenemos el XML completo y timbrado
                    resultadoXMLNC = EasyOne_LeeResultado(pN, 8)

                    Console.WriteLine(resultadoXMLNC)
                    Dim archivoXMLPDFNC As String = ""
                    Dim archivoREPXNC As String = ""
                    Dim archivoPDFNC As String = ""

                    IO.File.WriteAllText(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"), resultadoXMLNC)

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
                    documentoNC.Load(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"))
                    VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                    VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                    Version_xmlNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                    upb.Value += 10 'Suma 60
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

                        Dim Ruta As String = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".pdf")
                        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                        Dim cryRpt As New ReportDocument
                        upb.Value += 10 'Suma 70
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
                            objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & n_notacredito & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", As400)
                            upb.Value += 10 'Suma 80 
                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try
                        upb.Value += 10 'Suma 90
                        objpro.Indicacion("update bgxfacelec.ventaenc set veestatu = 0, vefac = '0', vefeccan = '" & FeRegistro & "', vemotcan = '" & vemotcan & "' " &
                                " where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and veidfolv = " & Transaccion & " and veterm = " & Terminal & " and " &
                                " vefecha = '" & FecFactura & "' ", As400)

                        objpro.Indicacion("update bgxfacelec.fencabez set feestatu = 0, fefeccan = '" & fechareal & "', femotcan = '" & vemotcan & "' " &
                                          " where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and feid = " & Transaccion & " and feterm = " & Terminal & " and " &
                                          " fefact = " & txtId.Text & " and feserie = '" & grbTipoFactura.Value & "' ", As400)
                        upb.Value += 10 'Suma 100
                        CargaGridConceptos()
                        Inicializa()
                    Else
                        MsgBox("Archivo XML no es una factura version 4.0")
                    End If 'Cierra Respuesta del PAC
                Else
                    'En caso de error mostrar el detalle.
                    Dim resultadoXML2 As String = [String].Empty
                    resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                    Console.WriteLine(resultadoXML2)
                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                End If 'Cierra generacion del XML
            Else
                MsgBox("Los datos de facturacion del cliente NO estan completos, favor de actualizar en catalogo de clientes.")
                btnGenerarFactura.Visible = True
            End If
        Else
            btnGenerarFactura.Visible = True
        End If
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub frmNotaCreditoBosco_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub
End Class