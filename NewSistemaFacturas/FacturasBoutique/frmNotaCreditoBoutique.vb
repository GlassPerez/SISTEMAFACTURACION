Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Threading

Public Class frmNotaCreditoBoutique

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim fechventa As String
    Dim FechaFactura As String

    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda, datosCliente, facturado As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim FecFactura As String
    Dim Cid As Integer
    Dim hra As String
    Dim Timbrado As Boolean = False
    Dim contador As Integer = 0
    Dim Cadena As String
    Dim NoCuenta As String
    Dim ExisteClave As String
    Dim NumPiezas As Integer
    Dim FechaBusqueda, invoice As String
    Dim va2 As Integer
    Dim serieNC, folioNC As String
    Dim FormPago As String
#End Region

#Region "sat"
    Dim TpRegimen As String
    Dim DesRegimen As String
    Dim fp As String
    Dim n_TRFiscal As String
    Dim n_desRF As String
#End Region

    Private Sub frmNotaCreditoBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Tienda FROM CatTien", 0, 1, Motor)
        Application.DoEvents()
        va2 = 0
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Application.DoEvents()
    End Sub

    Private Sub grbSucursal_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbSucursal.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbSucursal
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        objpro.CargaCombos(grbTipoFactura, "select seserie, sedesc as TipoFactura from serie where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", 0, 1, Motor)
        Inicializa()
        grbTipoFactura.Text = ""
        grbTipoFactura.Focus()
    End Sub

    Private Sub Inicializa()
        Terminal = 0
        Transaccion = 0
        txtId.Text = "ID"
        txtuuid.Text = "UUID"
        Total.Value = "0.0"
        Ugr.DataSource = Nothing
    End Sub

    Private Sub grbTipoFactura_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTipoFactura.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTipoFactura
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbTipoFactura_ValueChanged(sender As Object, e As EventArgs) Handles grbTipoFactura.ValueChanged
        Inicializa()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargaGridConceptos()
        If Ugr.Rows.Count = 0 Then
            MsgBox("No hay registros de consulta .. ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub CargaGridConceptos()
        TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", Motor).Tables(0)
        Dim fecini, fecfin As String
        FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
        fecini = FechaBusqueda & "01"
        fecfin = FechaBusqueda & DateTime.DaysInMonth(grbPeriodo.Text, objfun.EscojeMes(Me.grbMes.Text))
        objpro.LLenaGrid(Me.Ugr, "Select fefact as Factura, feuuid as UUID, fetotal as Total, feterm as Terminal, feid as folio, fereg as Fecha, cid as cliente, fformpag as FormaPago, feestatu as estatus  from fencabez where tid = " & grbTienda.Value & " and " &
                         " sid = " & grbSucursal.Value & " and fetip in ('F', 'G') and feserie = '" & grbTipoFactura.Value & "' and " &
                         " FeReg >= " & fecini & " and FeReg <= " & fecfin & "  ", "bgxfacelec", Motor)
        Me.Ugr.DataBind()

        For i = 0 To Me.Ugr.Rows.Count - 1
            'Dim Tfac As Double = 0
            'Tfac = Ugr.Rows(i).Cells("Total").Value
            If Ugr.Rows(i).Cells("estatus").Value = 0 Then
                Ugr.Rows(i).Appearance.BackColor = Color.Coral

                'Else
                '    If Ugr.Rows(i).Cells("Abono").Value <> 0 Then
                '        Ugr.Rows(i).Appearance.BackColor = Color.Gold
                '    End If
            End If
        Next
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 400
            .DisplayLayout.Bands(0).Columns(2).Width = 100
        End With
    End Sub

    Private Sub frmNotaCreditoBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Try
            If Ugr.ActiveRow.Cells("estatus").Value <> 0 Then
                txtId.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
                txtuuid.Text = Ugr.ActiveRow.Cells("uuid").Value.ToString
                Total.Value = Format(Ugr.ActiveRow.Cells("Total").Value, "###,###.00")
                Terminal = Ugr.ActiveRow.Cells("Terminal").Value.ToString
                Transaccion = Ugr.ActiveRow.Cells("Folio").Value.ToString
                FecFactura = Ugr.ActiveRow.Cells("Fecha").Value.ToString
                Cid = Ugr.ActiveRow.Cells("Cliente").Value.ToString
                fp = Ugr.ActiveRow.Cells("FormaPago").Value.ToString
            Else
                MsgBox("Esta factura ya ha sido cancelada ", MsgBoxStyle.Information, "Facturación.. ")
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub btnGenerarFactura_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        If txtuuid.Text.ToString <> "" Then
            Timbra()
            Inicializa()
            CargaGridConceptos()
        Else
            MsgBox("Seleccione una factura para cancelar ", MsgBoxStyle.Information, "Facturación.. ")
        End If

    End Sub

    Private Sub Timbra()
        'If MsgBox("¿Seguro de generar la Nota de Crédito ?. " & " no. de Factura " & txtId.Text, vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
        '    Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF, FechaSeFacturo As String
        '    Dim txtFechaVenta As DateTime = Now.Date
        '    Dim vemotcan As String = "Nota de credito de la factura " & txtId.Text
        '    Fecha11 = txtFechaVenta.ToString.Substring(0, 2)
        '    Fecha22 = txtFechaVenta.ToString.Substring(3, 2)
        '    Fecha33 = txtFechaVenta.ToString.Substring(6, 4)
        '    FePnotasF = Fecha33 & Fecha22 & Fecha11
        '    fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
        '    FeRegistro = txtFechaVenta.ToString.Substring(6, 4) & Fecha22 & Fecha11
        '    Fecha = fechareal
        '    FechaSeFacturo = FecFactura.Substring(6, 2) & "/" & FecFactura.Substring(4, 2) & "/" & FecFactura.Substring(0, 4)


        '    upb.Value = 0
        '    upb.Maximum = 100


        '    upb.Value += 50
        '    objpro.Indicacion("update fencabez set feestatu = 0, fefeccan = '" & fechareal & "', femotcan = '" & vemotcan & "' " &
        '                              " where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and feid = " & Transaccion & " and feterm = " & Terminal & " and " &
        '                              " fefact = " & txtId.Text & " and feserie = '" & grbTipoFactura.Value & "' ", Motor)
        '    upb.Value += 50
        'End If

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

            TIP = "NO" ''"NC"
            TSucursal = objpro.ObtenDatos("select * from Serie Where tid = 90 AND sid = 90 and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0) 'tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
            n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = 90 and Tid = 90 and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
            n_serie = TSucursal.Rows(0).Item("Seserie").ToString
            Application.DoEvents()

            n_TRazonSocial = objpro.Dato("trsocial", "cattien", "tid = " & grbTienda.Value & "", Motor)
            n_TRfc = objpro.Dato("trfc", "cattien", "tid = " & grbTienda.Value & "", Motor)
            n_LExpedicion = objpro.Dato("tcp", "cattien", "tid = " & grbTienda.Value & "", Motor)
            Dim rutlogo As String = objpro.Dato("rutLogo", "catsucu", "tid = " & grbTienda.Value & "", Motor) 'Trim(Tabla.Rows(0).Item("rutLogo").ToString)
            invoice = objpro.Dato("tuserpac", "cattien", "tid = " & grbTienda.Value & "", Motor)
            'Dim CarpTienda = objpro.Dato("sCtien", "catsucu", "tid = " & grbTienda.Value & " and sCsuc=" & grbSucursal.Value & "", Motor)
            Dim CarpSucursal = objpro.Dato("sCsuc", "catsucu", "tid = " & grbTienda.Value & " and sid=" & grbSucursal.Value & "", Motor)

            datosCliente = objpro.ObtenDatos("select * from catclie Where cid = " & Cid & "", Motor).Tables(0)
            n_crfc = datosCliente.Rows(0).Item("crfc").ToString
            n_cRazonSocial = datosCliente.Rows(0).Item("crsocial").ToString
            n_cDireccion = datosCliente.Rows(0).Item("cDir").ToString.Trim & " " & datosCliente.Rows(0).Item("cNumExt").ToString.Trim & " " & datosCliente.Rows(0).Item("cNumInt").ToString.Trim & " " & datosCliente.Rows(0).Item("cColonia").ToString.Trim
            n_cComDir = datosCliente.Rows(0).Item("cMun").ToString.Trim & " " & datosCliente.Rows(0).Item("cEstado").ToString.Trim & " " & datosCliente.Rows(0).Item("cLocal").ToString.Trim & " " & datosCliente.Rows(0).Item("cPais").ToString.Trim
            n_cCP = datosCliente.Rows(0).Item("cCP").ToString
            n_cUCFDI = datosCliente.Rows(0).Item("cusocfdi").ToString
            n_cregFiscal = datosCliente.Rows(0).Item("cregFiscal").ToString
            n_desRF = datosCliente.Rows(0).Item("cRegFisDes").ToString

            If n_cCP.Trim <> "" And n_cregFiscal.Trim <> "" Then

                Dim n_RutNotaCredito As String = objpro.Dato("sForm3", "catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", Motor)
                Dim CerFactura As String = objpro.Dato("sRCer1", "catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", Motor)
                Dim Key As String = objpro.Dato("sRKey", "catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", Motor)
                Dim Pwd As String = objpro.Dato("sPwd", "catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", Motor)

                Dim RFacImpresas As String
                n_Directorios = objpro.Dato("subic", "catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", Motor)
                RFacImpresas = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
                upb.Value += 10 'Suma 10

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
                Select Case fp
                    Case "01"
                        Fila("Nformpag") = "EFECTIVO"
                    Case "02"
                        Fila("Nformpag") = "CHEQUE"'CFDI.CFDI33.c_FormaPago.Cheque_02
                    Case "03"
                        Fila("Nformpag") = "TRANSFERENCIA" 'CFDI.CFDI33.c_FormaPago.Transferencia_03
                    Case "04"
                        Fila("Nformpag") = "TARJETA DE CRÉDITO"
                    Case "28"
                        Fila("Nformpag") = "TARJETA DE DÉBITO"
                    Case "99"
                        Fila("Nformpag") = "OTROS" 'CFDI.CFDI33.c_FormaPago.Otros_99
                End Select

                Dim metpg As String
                Dim tipcom As String

                metpg = "PUE" 'SE QUEDA ASI PORQUE ES PARA CLIENTES
                tipcom = "E" 'SE QUEDA ASI PORQUE ES PARA CLIENTES
                'formpg = fp '"99" 'ESTE SI CAMBIA

                'Dim sbt As Double = 0
                Dim Pu As Double = Format((Me.Total.Value / 1.16), "###,###.00")
                'sbt = Format(Pu, "###,###.00")

                Fila("Ntipocomp") = tipcom + " - Egreso"
                Fila("Nmetpag") = metpg

                'Tipo de relacion
                EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")
                'CFDI Relacionado
                EasyOne_cfdi33_AddCfdiRelacionado(pN, txtuuid.Text)

                'c.Moneda = CFDI.CFDI33.c_Moneda.MXN
                'fp = "Otros"

                n_TRFiscal = objpro.Dato("satregimen", "catsucu", "tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", Motor)
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
                    Case 621
                        TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                        DesRegimen = "Incorporacion Fiscal"
                End Select

                Fila("Nregfis") = n_TRFiscal + " - " + DesRegimen.Trim 'sucursal

                'Llenamos la sección del Emisor
                EasyOne_cfdi33_SetEmisor(pN, n_TRfc, n_TRazonSocial, n_TRFiscal)
                'Fila("usocfdi") = "I01"

                'Llenamos la sección del Receptor
                'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                EasyOne_cfdi33_SetReceptor(pN, n_crfc, n_cRazonSocial, "", "", n_cUCFDI) 'CAMBIAR PARA EL CLIENTE'estos paramentros son si o si para factura global
                EasyOne_cfdi33_SetReceptor40(pN, n_cCP, n_cregFiscal) 'CAMBIAR POR EL REGIMEN DEL CLIENTE 'regimen fiscal para global 616

                Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")
                Dim descrip As String = ""
                descrip = "Devoluciones del día " & FechaSeFacturo.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & txtuuid.Text & " con un importe de $ " & Format(Pu, "###,###.00")

                upb.Value += 10 'Suma 20
                Fila("Nrcp") = n_cCP
                Fila("Nrrfc") = n_crfc '"XAXX010101000"
                Fila("Nrrazonsoc") = n_cRazonSocial '"PUBLICO EN GENERAL"
                Fila("Nrdireccion") = n_cDireccion
                Fila("Nrregimen") = n_cregFiscal & " - " & n_desRF '"616-Sin Obligaciones Fiscales"
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

                'c.AgregaConcepto("1", "SERVICIO", "ACT", "84111506", "100", "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Pu, "###,###.00"), Format(Pu, "###,###.00"))
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
                'End If

                Dim total As Double = 0
                total = Format((Pu + iv), "###,###.00")

                Dim f1, freal, hr As String
                f1 = FechaSeFacturo.ToString.Substring(6, 4)
                hr = Now.ToLocalTime.ToString.Substring(11, 8)
                'f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                'Llenamos la sección del comprobante (2)
                EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, freal, fp, "", Pu.ToString, "", "MXN", TipoCambio, total.ToString, tipcom, metpg, n_LExpedicion, "")

                Impuesto = "002"
                TasaOCuota = "0.160000"
                TipoFactor = "Tasa"
                Import = Format(iv, "###,###.00")

                Dim TotalImpuestosTrasladados As Double = 0
                TotalImpuestosTrasladados = Format(iv, "###,###.00")

                EasyOne_cfdi33_SetComprobanteImpuestos(pN, "", TotalImpuestosTrasladados)
                EasyOne_cfdi33_AddTrasladosTraslado(pN, Impuesto, TipoFactor, TasaOCuota, Import)

                Fila("Nfsubtotal") = Pu.ToString
                'Fila("Nfdescuento") = ""
                Fila("Nfiva") = iv
                Fila("Nftotal") = total 'Pu.ToString

                Cadena = "Suc: " & Trim(CarpSucursal) & " Fecha Venta: " & FecFactura & " Nota:" & Terminal.ToString & "-" & Transaccion.ToString & " Total:" & " $" & Str(FormatCurrency(Me.Total.Value))
                Fila("NSucursal") = Cadena

                Dim cantl As String
                cantl = Letras(total)
                Fila("Ncantidadl") = cantl

                facturado = objpro.ObtenDatos("Select * from fencabez where fefact = '" & CInt(txtId.Text) & "' and feserie = '" & n_serie & "' and feestatu = 0 and tid =" & grbTienda.Value & " and sid = " & grbSucursal.Value & "  and feterm = " & Terminal & " and day(feFecha) = " & FechaSeFacturo.Substring(0, 2) & " and Month(feFecha) = " & FechaSeFacturo.Substring(3, 2) & " and year(feFecha) = " & FechaSeFacturo.Substring(6, 4) & " ", Motor).Tables(0)
                If facturado.Rows.Count = 0 Then
                    'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, invoice, invoice)
                    EasyOne_SetInvoiceOneFachadaInfo(pN, invoice, invoice)

                    Dim nc As Integer = EasyOne_FirmaComprobante(pN, CerFactura, Key, Pwd)

                    Thread.Sleep(10000) 'espera por 10 segundos
                    If nc = 0 Then
                        'If Not resultado.Errores Then
                        Application.DoEvents()
                        upb.Value += 10 'Suma 30

                        objpro.Indicacion("Update Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = 90 and tid = 90  and seEstat = 1", Motor) 'SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)

                        objpro.Indicacion("update fencabez set feestatu = 0, fefeccan = '" & fechareal & "', femotcan = '" & vemotcan & "' " &
                                      " where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and feid = " & Transaccion & " and feterm = " & Terminal & " and " &
                                      " fefact = " & txtId.Text & " and feserie = '" & grbTipoFactura.Value & "' ", Motor)

                        'objpro.Indicacion("update ventaenc set veestatu = 0, vefac = '0', vefeccan = '" & FeRegistro & "', vemotcan = '" & vemotcan & "' " &
                        '    " where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and veidfolv = " & Transaccion & " and veterm = " & Terminal & " and " &
                        '    " day(vefecha) = " & FechaSeFacturo.Substring(0, 2) & " and month(vefecha) = " & FechaSeFacturo.Substring(3, 2) & " and year(vefecha) = " & FechaSeFacturo.Substring(6, 4) & " ", MotorBoutique)

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
                        upb.Value += 10 'Suma 40
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

                            '''''nuevoooooooooooooooooooooooooooooooooooooooooo

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
                            upb.Value += 10 'Suma 50
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

                                upb.Value += 10 'Suma 60 
                            Catch ex As Exception
                                MsgBox(ex.ToString)
                            End Try

                            upb.Value += 10 'Suma 70

                            objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha," &
                                " feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                " FeCanlet,FeAbonos,FeEstatu,FeReg, FeFecCan,FeMotcan, feTip,feuuid, fformpag) " &
                                " values (" & CInt(Transaccion) & "," & grbTienda.Value & "," & Me.grbTienda.Value & Me.grbSucursal.Value & "," & CInt(n_notacredito) & ",'" & n_serie & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," &
                                " '" & Trim(FechaHora) & "', " & CInt(Cid) & ",'" & Pu.ToString & "', 16,'" & iv & "'," &
                                " '0', '0', '0', '0', '" & total & "', " &
                                " '" & cantl & "', '0',1 ," & FeRegistro & ",  " & fechareal & ", '" & vemotcan & "', 'N','" & uuidNC & "', '" & fp & "' )", Motor)

                            upb.Value += 30 'Suma 100
                            Inicializa()
                            CargaGridConceptos()
                            '''''nuevoooooooooooooooooooooooooooooooooooooooooo
                        Else
                            MsgBox("Archivo XML no es una factura version 4.0")
                        End If 'Cierra espuesta del PAC
                    Else
                        'En caso de error mostrar el detalle.
                        Dim resultadoXML2 As String = [String].Empty
                        resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                        Console.WriteLine(resultadoXML2)
                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación")
                    End If 'Cierra validación generación de xml
                Else
                    MsgBox("La factura seleccionada ya se encuentra cancelada")
                End If
            Else
                MsgBox("LA DIRECCIÓN DEL CLIENTE NO ESTA COMPLETA, FAVOR DE ACTUALIZAR DATOS EN EL CATALOGO DE CLIENTES")
            End If
        End If
    End Sub


End Class