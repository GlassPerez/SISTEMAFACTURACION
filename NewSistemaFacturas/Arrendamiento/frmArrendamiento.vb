Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath

Public Class frmArrendamiento

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Variables"
    Dim TIP As String
    Dim TablaTienda, tablaclientes, Tabla As DataTable
    Dim selOP As String = ""
    Dim impRetenido As Double = 0
    Dim iv As Double = 0
    Dim FechaFactura As String
    Dim banSolCuenta As Boolean

    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim Cadena As String
    Dim fp As String
    Dim NumPiezas As Integer
    Dim Subt As Double
    Dim ExisteClave As String

    Dim ttotal As Double = 0
    Dim tSubtotal As Double = 0
    Dim tIva As Double = 0

    Dim RF As String
    Dim RFD As String
    Dim cCP As String
    Dim cDireccion As String
    Dim cComDir As String
    Dim va As Integer
    Dim va2 As Integer
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

#Region "Sat"
    Dim TpRegimen As String
    Dim e_uuid As String
    Dim Uso_CFDI As String
    Dim metodoPago As String
    Dim TRFiscal As Integer
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
#End Region

    Private Sub frmArrendamiento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        verNumCta(False)
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbTienda, "Select tId,trSocial FROM BGXFacElec.CatTien Where TTIPO2 = 'A'", 0, 1, As400)
        Application.DoEvents()
        'objpro.CargaCombos(grbClientes, "Select crfc, crsocial from BGXFacElec.catclie order by crsocial asc ", 0, 1, As400)
        'Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from bgxfacelec.satmpag ", 0, 1, As400)
        Application.DoEvents()
        CargaGridConceptos()
        va = 0
        va2 = 0
    End Sub

    Private Sub frmArrendamiento_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.ugr, "Select VdCant as Cantidad, VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion, " &
                         "  VdPreUni as PU, " &
                         " VdPreTot as Importe From bgxfacelec.VentaDet " &
                         " Where VdFolio = 0", "VentaDet", As400)
        Me.ugr.DataBind()
    End Sub

    'Private Sub grbClientes_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs)
    '    e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
    '    With grbClientes
    '        .DisplayLayout.Bands(0).Columns(0).Hidden = True
    '    End With
    'End Sub

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

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDIBosco)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    'Private Sub grbClientes_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs)
    '    Try
    '        txtRfc.Text = objpro.Dato("crfc", "BGXFacElec.catclie", "crfc = '" & Trim(grbClientes.Value) & "'", As400)
    '        txtRazonSocial.Text = objpro.Dato("crSocial", "BGXFacElec.catclie", "crfc = '" & Trim(grbClientes.Value) & "'", As400)
    '        txtEmail.Text = objpro.Dato("cemail", "BGXFacElec.catclie", "crfc = '" & Trim(grbClientes.Value) & "'", As400)
    '        cfdiuso = objpro.Dato("cUsoCFDI", "BGXFacElec.catclie", "crfc = '" & Trim(grbClientes.Value) & "'", As400)
    '        If cfdiuso <> "" Then
    '            txtclvUsoCfdi.Text = cfdiuso
    '            txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub

    Private Sub grbClientes_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            grbTienda.Focus()
        Else
            e.KeyChar = UCase(e.KeyChar)
        End If
    End Sub

    Private Sub grbTienda_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs) Handles grbTienda.RowSelected
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " Order by Sid", 0, 1, As400)
        Me.grbSucursal.Text = ""
        Me.txtCtaPredial.Text = objpro.DatoString("tref", "BGXFacElec.CatTien", "tid = " & Me.grbTienda.Value & "", As400)
        If Me.txtCtaPredial.Text = "" Then
            Me.txtCtaPredial.ReadOnly = False
        End If
    End Sub

    Private Sub btnDatos_Click(sender As Object, e As EventArgs) Handles btnDatos.Click
        Try
            TIP = "AR"
            Tabla = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", As400).Tables(0)
            Me.txtSerie.Text = objpro.Dato("Seserie", "bgxfacelec.serie", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & "  and seTipo = '" & TIP & "' and seEstat = 1", As400)
            Application.DoEvents()
            Me.txtFactura.Text = Format(CDbl(objpro.DatoMaximo("cast(seCant as varchar(10))", "BGXFacElec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")
            Application.DoEvents()
            Me.txtLugarExpedicion.Text = objpro.Dato("selugarexp", "bgxfacelec.serie", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & "  and seTipo = '" & TIP & "' and seEstat = 1", As400)
            Application.DoEvents()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "bgxfacelec.satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", As400) = True Then
            banSolCuenta = True
            verNumCta(True)
            fp = grbformaPago.Text
        Else
            fp = grbformaPago.Text
            banSolCuenta = False
            verNumCta(False)
        End If
        txtDescripcion.Focus()
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim concepto As String = Trim(txtDescripcion.Text)
        concepto = txtDescripcion.Text.Replace(vbCrLf, "")
        FormatoGrid(concepto)
    End Sub

    Private Sub FormatoGrid(concepto As String)
        ugr.Rows.Band.AddNew()
        ColocarArticulo(ugr.Rows.Count - 1, concepto)
        LimpiarCajas()
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, concepto As String)
        With ugr
            .Rows(Renglon).Cells("Cantidad").Value = Me.txtCantidad.Text
            .Rows(Renglon).Cells("Unidad").Value = txtUnidad.Text
            .Rows(Renglon).Cells("Clave").Value = Me.txtClave.Text
            .Rows(Renglon).Cells("Descripcion").Value = Trim(concepto)
            .Rows(Renglon).Cells("PU").Value = Me.txtImport.Value
            .Rows(Renglon).Cells("Importe").Value = Format(CDbl(txtCantidad.Text * Me.txtImport.Value), "###,###.00")
        End With
    End Sub

    Private Sub LimpiarCajas()
        Me.txtCantidad.Text = "1"
        Me.txtDescripcion.Text = ""
        Me.txtImport.Value = 0.0
    End Sub

    Private Sub txtDescripcion_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDescripcion.KeyPress
        If e.KeyChar = Chr(13) Then
            txtImport.Focus()
        Else
            e.KeyChar = UCase(e.KeyChar)
        End If

    End Sub

    Private Sub ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugr.InitializeLayout
        With Me.ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 70
            .DisplayLayout.Bands(0).Columns(1).Width = 80
            .DisplayLayout.Bands(0).Columns(2).Width = 90
            .DisplayLayout.Bands(0).Columns(3).Width = 560
            .DisplayLayout.Bands(0).Columns(4).Width = 110
            .DisplayLayout.Bands(0).Columns(5).Width = 110
        End With
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        Dim TRazonSocial, TRfc, TCp, TRef As String
        If grbPagos.Text <> "" Then
            If Me.ugr.Rows.Count > 0 Then
                If chboxPM.Checked <> False Or chboxPF.Checked <> False Then
                    If grbformaPago.Text <> "" Then
                        If txtCtaPredial.Text <> "" Then
                            If txtIv.Text.Trim <> "" And txtIvISR.Text.Trim <> "" Then
                                tablaclientes = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CATCLIE WHERE cRFC = '" & Me.txtRfc.Text & "'", As400).Tables(0)
                                Dim ne As String = tablaclientes.Rows(0).Item("cNumExt").ToString.Trim
                                If ne.ToString = "0" Then
                                    ne = " "
                                End If
                                cDireccion = tablaclientes.Rows(0).Item("cDir").ToString.Trim & " " & ne & " " & tablaclientes.Rows(0).Item("cNumInt").ToString.Trim & " " & tablaclientes.Rows(0).Item("cColonia").ToString.Trim
                                cComDir = tablaclientes.Rows(0).Item("cMun").ToString.Trim & " " & tablaclientes.Rows(0).Item("cEstado").ToString.Trim & " " & tablaclientes.Rows(0).Item("cLocal").ToString.Trim & " " & tablaclientes.Rows(0).Item("cPais").ToString.Trim
                                Dim CerFactura As String = objpro.Dato("sRCer1", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                                Dim Key As String = objpro.Dato("sRKey", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                                Dim Pwd As String = objpro.Dato("sPwd", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                                Dim Directorios As String = objpro.Dato("sUbic", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                                Dim UserPAC As String = objpro.Dato("tUserPAC", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)
                                Dim PwdPAC As String = objpro.Dato("tPwdPAC", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)
                                TRFiscal = objpro.Dato("satRegimen", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                                Dim RutArrendamiento As String = objpro.Dato("sForm1", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                                Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
                                Dim descreg As String = Trim(Tabla.Rows(0).Item("sRFiscal").ToString) ''del cliente
                                DirEmail = Tabla.Rows(0).Item("EMAILDIR").ToString
                                PwdEmail = Tabla.Rows(0).Item("EMAILPWD").ToString
                                Pop = Tabla.Rows(0).Item("EMAILPOP").ToString
                                PuertoPop = Tabla.Rows(0).Item("EMAILPPOP").ToString
                                SmtpHost = Tabla.Rows(0).Item("EMAILSMTP").ToString
                                PuertoSmtp = Tabla.Rows(0).Item("EMAILPSMTP").ToString
                                DescEmail = Tabla.Rows(0).Item("EMAILDESC").ToString
                                AsuntoEmail = Tabla.Rows(0).Item("EMAILASU").ToString
                                MsjEmail = Tabla.Rows(0).Item("EMAILMSG").ToString
                                Dim rutlogo As String = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png"
                                upb.Value = 0
                                upb.Maximum = 100

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
                                Tabla1.Columns.Add("rcomdir")
                                Tabla1.Columns.Add("rusocfdi")
                                Tabla1.Columns.Add("nocuenta")
                                Tabla1.Columns.Add("retIVA")
                                Tabla1.Columns.Add("retISR")
                                Tabla1.Columns.Add("cuentapredial")
                                Tabla1.Columns.Add("UUIDRel")
                                Tabla1.Columns.Add("TipoRel")
                                Tabla1.Columns.Add("UUID2Rel")

                                Dim Fila As DataRow
                                TRfc = objpro.Dato("trfc", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                                TRazonSocial = objpro.Dato("trsocial", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                                TCp = objpro.Dato("tcp", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                                TRef = objpro.Dato("tref", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                                REM DesRegimen = "Arrendamiento"

                                Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
                                Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
                                Fila("Erazonsoc") = TRazonSocial
                                Fila("Eregfis") = TRFiscal.ToString + " - " + descreg.Trim
                                Fila("Edireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
                                Fila("Edircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString
                                Dim RFacImpresas As String
                                RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)


                                Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

                                upb.Value += 10 'inicia en 10

                                EasyOne_cfdi33_SetCfdi40(p, "01", "")

                                Fila("exporta") = "01-No Aplica"

                                Select Case txtclvUsoCfdi.Text
                                    Case "D01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.HonorariosMedicosDentalesYGastosHospitalarios
                                    Case "D02"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosMedicosPorIncapacidadODiscapacidad
                                    Case "D03"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosFunerales
                                    Case "D04"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.Donativos
                                    Case "D05"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.InteresesRealesEfectivamentePagadosCreditoHipotecario
                                    Case "D06"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.AportacionesVoluntariasAlSAR
                                    Case "D07"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.PrimasPorSegurosGastosMedicos
                                    Case "D08"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosDeTransportacionEscolarObligatoria
                                    Case "D09"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.DepositosEnCuentaParaAhorroPrimaPlanesDePensiones
                                    Case "D10"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.PagosPorServiciosEducativos
                                    Case "G01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.AdquisicionMercancias
                                    Case "G02"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.DevolucionesDescuentosOBonificaciones
                                    Case "G03"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosEnGeneral
                                    Case "I01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.Construcciones
                                    Case "I02"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.MobiliarioYEquipoOficinaInversiones
                                    Case "I03"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.EquipoDeTransporte
                                    Case "I04"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.EquipoComputoYAccesorios
                                    Case "I05"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.DadosTroqueleesMoldesMatricesYHerramental
                                    Case "I06"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.ComunicacionesTelefonicas
                                    Case "I07"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.ComunicacionesSatelitales
                                    Case "I08"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.OtraMaquinariaYEquipo
                                    Case "P01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.Otros
                                End Select

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
                                    Case 621
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IncorporacionFiscal
                                        DesRegimen = "Incorporacion Fiscal"
                                End Select

                                'Llenamos la sección del Emisor
                                EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TRFiscal) 'TRFiscal
                                'EasyOne_cfdi33_SetEmisor(p, TRfc, "POVISA", "601")

                                'Llenamos la sección del Receptor
                                'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                                'EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01")
                                EasyOne_cfdi33_SetReceptor(p, txtRfc.Text, txtRazonSocial.Text, "", "", txtclvUsoCfdi.Text) 'estos paramentros son si o si para factura global
                                'EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616
                                EasyOne_cfdi33_SetReceptor40(p, cCP, RF)

                                Fila("sucursal") = Cadena
                                Fila("Rcp") = cCP 'TCp
                                Fila("Rrfc") = txtRfc.Text '"XAXX010101000"
                                Fila("Rrazonsoc") = txtRazonSocial.Text '"PUBLICO EN GENERAL"
                                Fila("Rdireccion") = cDireccion '""
                                Fila("Rregimen") = RF & " - " & RFD '
                                Fila("rusocfdi") = "Uso CFDI: " & txtclvUsoCfdi.Text ' & " - " & txtUsoCfdi.Text '"S01"
                                Fila("moneda") = "MXN"
                                Fila("rcomdir") = cComDir
                                Fila("nocuenta") = txtNumCuenta.Text

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
                                Dim Import As Double = 0
                                Dim Import2 As Double = 0

                                Dim Importe As Double = 0
                                Dim Impuesto As String = ""
                                Dim TasaOCuota As String = ""
                                Dim TipoFactor As String = ""
                                Dim Impu As String = ""
                                Dim ToC As String = ""
                                Dim TFac As String = ""
                                Dim dcto As String = ""
                                Dim ipt As String = ""
                                Dim Impuest As String = ""
                                Dim Impuest2 As String = ""

                                Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
                                Tabla2.Columns.Add("Cantidad")
                                Tabla2.Columns.Add("Unidad")
                                Tabla2.Columns.Add("UniSAT")
                                Tabla2.Columns.Add("Descrip")
                                Tabla2.Columns.Add("Punitario")
                                Tabla2.Columns.Add("Importe")

                                Dim Fila2 As DataRow

                                For i = 0 To Me.ugr.Rows.Count - 1
                                    Dim pu As Double = Trim(Me.ugr.Rows(i).Cells("pu").Value)
                                    Dim im As Double = Trim(Me.ugr.Rows(i).Cells("Importe").Value)
                                    sb = Trim(Me.ugr.Rows(i).Cells("pu").Value)
                                    Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                                    Fila2("Cantidad") = Trim(Me.ugr.Rows(i).Cells("Cantidad").Value)
                                    Fila2("Unidad") = Trim(Me.txtClave.Text)
                                    Fila2("UniSAT") = Trim(Me.ugr.Rows(i).Cells("Unidad").Value)
                                    Fila2("Descrip") = Trim(Me.ugr.Rows(i).Cells("Descripcion").Value)
                                    Fila2("Punitario") = Format(pu, "###,###.00")
                                    Fila2("Importe") = Format(im, "###,###.00")
                                    DS.Tables(1).Rows.Add(Fila2)
                                    EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.txtUnidad.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format(pu, "######.00"), Format(im, "######.00"), "")

                                    EasyOne_cfdi33_SetConcepto40(p, "02")
                                    Imp = Format((Trim(Me.ugr.Rows(i).Cells("Importe").Value) * 0.16), "######.00")
                                    iv += Imp
                                    bas = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value), "######.00")
                                    Impu = "002"
                                    ToC = "0.160000"
                                    TFac = "Tasa"
                                    EasyOne_cfdi33_AddTraslado(p, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
                                    impRetenido = 0
                                    If selOP = "M" And txtIv.Text.Trim <> "0" And txtIvISR.Text.Trim <> "0" Then

                                        Import = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value * txtIv.Text), "######.00")
                                        impRetenido += Import
                                        Dim TasaOCuot As String = txtIv.Text
                                        Dim TipoFacto As String = "Tasa" 'c_TipoFactor.Tasa
                                        Impuest = "002" 'c_Impuesto.IVA
                                        Dim Base As Double = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value), "######.00")
                                        EasyOne_cfdi33_AddRetencion(p, Format(Base, "######.00"), Impuest, TipoFacto, TasaOCuot, Format(Import, "######.00"))

                                        Import2 = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value * txtIvISR.Text), "######.00")
                                        impRetenido += Import2
                                        Dim TasaOCuot2 As String = txtIvISR.Text
                                        Dim TipoFacto2 As String = "Tasa" 'c_TipoFactor.Tasa
                                        Impuest2 = "001" 'c_Impuesto.IVA
                                        Dim Base2 As Double = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value), "######.00")
                                        EasyOne_cfdi33_AddRetencion(p, Format(Base2, "######.00"), Impuest2, TipoFacto2, TasaOCuot2, Format(Import2, "######.00"))
                                    End If
                                Next
                                upb.Value += 20 'Suma 30

                                Impuesto = "002"
                                TasaOCuota = "0.160000"
                                TipoFactor = "Tasa"
                                Importe = Format(iv, "######.00")

                                Dim TotalImpuestosTrasladados As Double = 0
                                TotalImpuestosTrasladados = Format(iv, "######.00")

                                'Agregamos los Impuestos
                                Dim TotalImpuestosRetenidos As Double = Import + Import2
                                If TotalImpuestosRetenidos.ToString = "0" Then
                                    EasyOne_cfdi33_SetComprobanteImpuestos(p, "0.00", Format(TotalImpuestosTrasladados, "######.00"))
                                Else
                                    EasyOne_cfdi33_SetComprobanteImpuestos(p, Format(TotalImpuestosRetenidos, "######.00"), Format(TotalImpuestosTrasladados, "######.00"))
                                End If


                                If selOP = "M" And txtIv.Text.Trim <> "0" And txtIvISR.Text.Trim <> "0" Then
                                    EasyOne_cfdi33_AddRetencionesRetencion(p, Impuest, Import)
                                    EasyOne_cfdi33_AddRetencionesRetencion(p, Impuest2, Import2)
                                End If

                                EasyOne_cfdi33_AddTrasladosTraslado(p, Impuesto, TipoFactor, TasaOCuota, Importe)

                                Fila("retIVA") = Format(Import, "###,###.00")
                                Fila("retISR") = Format(Import2, "###,###.00")
                                Fila("cuentapredial") = txtCtaPredial.Text
                                Dim Total As Double = 0
                                Dim Subt As Double = 0
                                Subt = Format(sb + iv, "######.00")
                                Total = Format((Subt - impRetenido), "###,###.00")
                                Fila("Fsubtotal") = Format(Subt, "###,###.00")
                                Fila("Fdescuento") = Format(des, "###,###.00")
                                Fila("Fiva") = Format(iv, "###,###.00")
                                Fila("Ftotal") = Format(Total, "###,###.00")

                                Dim ValorTicket As String = Cadena
                                If txtNumCuenta.Text = "" Then
                                    NoCuenta = "-"
                                Else
                                    NoCuenta = txtNumCuenta.Text
                                End If

                                Dim cantletra As String
                                cantletra = Letras(Total)
                                Fila("cantidadl") = cantletra

                                Dim f1, f2, f3, freal, FeReg, hr As String
                                f1 = Today.ToString.Substring(0, 2)
                                f2 = Today.ToString.Substring(3, 2)
                                f3 = Today.ToString.Substring(6, 4)
                                hr = Now.ToLocalTime.ToString.Substring(11, 8)
                                freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                                FeReg = Today.ToString.Substring(6, 4) & f2 & f1

                                If TextBox2.Text.Trim = "04" Then
                                    EasyOne_cfdi33_SetCfdiRelacionados(p, "04")
                                    Fila("TipoRel") = "04 - Sustitución de los CFDI previos"
                                End If

                                If TextBox1.Text.Trim <> "" Then
                                    EasyOne_cfdi33_AddCfdiRelacionado(p, TextBox1.Text.Trim.ToUpper)
                                    Fila("UUIDRel") = TextBox1.Text.Trim.ToUpper '"374887F7-5AC2-4EDF-B191-A49A0F4A0A77"
                                End If

                                If TextBox3.Text.Trim <> "" Then
                                    EasyOne_cfdi33_AddCfdiRelacionado(p, TextBox3.Text.Trim.ToUpper)
                                    Fila("UUID2Rel") = TextBox3.Text.Trim.ToUpper '"374887F7-5AC2-4EDF-B191-A49A0F4A0A77"
                                End If

                                EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((sb), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "I", grbPagos.Value, TCp, "")

                                upb.Value += 10 'Suma 40

                                Fila("metpago") = grbPagos.Value
                                Fila("tipocomp") = "I - Ingreso"
                                Fila("formpago") = fp

                                'Enviamos el CFDI al metodo de PRUEBAS
                                'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                                'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBAS
                                EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

                                Application.DoEvents()
                                'Timbra XML, que se encuentra en variable P
                                Dim x As Integer = 99
                                If va = 0 Then
                                    x = EasyOne_FirmaComprobante(p, CerFactura, Key, Pwd)
                                    If x = 0 Then
                                        va += 1
                                    Else
                                        Dim resultadoXML2 As String = [String].Empty
                                        resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                        MsgBox("No se timbro archivo, contactar DEPTO. DE SISTEMAS, XML MAL" & " " & " No. de Veces de Timbrado " & va, MsgBoxStyle.Critical).ToString()
                                    End If
                                Else
                                    MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
                                End If
                                Application.DoEvents()
                                upb.Value += 10 'Suma 50

                                If x = 0 And va = 1 Then
                                    Application.DoEvents()
                                    Dim resultadoXML As String = [String].Empty
                                    resultadoXML = EasyOne_LeeResultado(p, 8)
                                    'Obtenemos el XML completo y timbrado
                                    Console.WriteLine(resultadoXML)

                                    Application.DoEvents()
                                    IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"), resultadoXML)

                                    upb.Value += 10 'Suma 60

                                    'Dim archivoREPX As String
                                    'Dim archivoPDF As String
                                    Dim Version_xml As String
                                    Dim version As String
                                    Dim uuid As String
                                    Dim ftimb As String
                                    Dim rfcprov As String
                                    Dim sellocfd As String
                                    Dim nocert As String

                                    Dim nav As XPathNavigator
                                    Dim documento As XmlDocument = New XmlDocument()
                                    Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
                                    documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"))
                                    VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                                    VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                                    Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                                    If Version_xml = "4.0" Then
                                        'If IO.File.Exists(c.ArchivoXMLSellado) Then
                                        Application.DoEvents()
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
                                        RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml.png")

                                        '''''''''''''''''''''''''''''''''''''''''''''''''''

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
                                        Dim FeCan = 0
                                        Dim FeMotCan As String = ""
                                        Dim ferisr = 0
                                        Dim feriva = 0
                                        Dim Descuento = 0
                                        Dim Tipo As String = "F"
                                        Dim Estatus As String = 1
                                        Dim PorIva As Double = 16

                                        Dim abono As Double = 0
                                        Select Case metodoPago
                                            Case 1
                                                abono = ttotal
                                            Case 3
                                                abono = 0
                                        End Select

                                        ''''''nuevoooooooooooooooooooooooooooooooooooooooooo
                                        objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", As400)
                                        ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = '" & Trim(Me.txtRfc.Text) & "' ", As400))

                                        objpro.Indicacion("Insert into BGXFacElec.FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid," &
                                            " feFecha,feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                            " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                            " FeCanlet,FeAbonos,FeEstatu, FeReg, fefeccan, femotcan, fetip, feuuid, ferisr, feriva) " &
                                            " values (" & FeRegistro & "," & grbTienda.Value & "," & Tterm & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," &
                                            " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & "," & Subt & ", " & PorIva & "," & tIva & "," &
                                            " " & Descuento & "," & Descuento & ", " & Descuento & ", " & Descuento & ", " & Total & ", " &
                                            " '" & cantletra & "', " & abono & "," & Estatus & "," & FeRegistro & ", '" & FeCan & "', '" & FeMotCan & "', '" & Tipo & "','" & uuid & "', " & ferisr & "," & feriva & ")", As400)
                                        ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

                                        DS.Tables(0).Rows.Add(Fila)
                                        Dim objreport As New GArrenda
                                        objreport.SetParameterValue("Logo", RutaLogo)
                                        objreport.SetParameterValue("QR", RutaQR)
                                        Dim rpFact As New GArrenda
                                        rpFact.SetDataSource(DS)
                                        rpFact.SetParameterValue("QR", RutaQR)
                                        rpFact.SetParameterValue("Logo", RutaLogo)
                                        Arrendamiento.CrystalReportViewer1.ReportSource = objreport
                                        Arrendamiento.CrystalReportViewer1.ReportSource = rpFact
                                        Arrendamiento.WindowState = FormWindowState.Maximized
                                        Arrendamiento.Show()

                                        upb.Value += 10 'Suma 70
                                        rutaArchivoPDF = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".pdf"
                                        rutaArchivoXML = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml"
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
                                            upb.Value += 10 'Suma 80  
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
                                        correo.Body += Environment.NewLine & "GRUPO VIGIL. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                                        correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                                        correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                                        correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                                        correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                                        correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                                        correo.Body += " ó a través de una solicitud vía correo electrónico a boutique@grupovigil.com.mx"

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
                                            Application.DoEvents()
                                        Next
                                        Dim smtp As New System.Net.Mail.SmtpClient
                                        smtp.Host = SmtpHost
                                        smtp.Port = Int(PuertoSmtp)
                                        smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                                        smtp.EnableSsl = False
                                        REM smtp.EnableSsl = SeguridadEmail 
                                        Application.DoEvents()
                                        smtp.Send(correo)
                                        upb.Value += 10 'Suma 100
                                        Application.DoEvents()
                                        MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                                        DatClientes()
                                        BorrarRegistro()
                                        'PDF(archivoPDF)
                                        upb.Value = 0
                                        CargaGridConceptos()

                                    Else
                                        MsgBox("Archivo XML no es una factura version 4.0")
                                    End If 'Valida si existe el archivo XML 
                                Else
                                    'En caso de error mostrar el detalle.
                                    Dim resultadoXML2 As String = [String].Empty
                                    resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                    Console.WriteLine(resultadoXML2)
                                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                End If 'Valida errores al generar XML
                            Else
                                MsgBox("El %IVA y el %ISR de Persona Moral, no puede ir vacío . Si la factura no lleva retencion, coloque un cero en los porcentajes.", MsgBoxStyle.Information, "Arrendamiento .. ")
                            End If
                        Else
                            MsgBox("No. de cuenta predial necesario . ", MsgBoxStyle.Information, "Arrendamiento .. ")
                        End If
                    Else
                        MsgBox("Seleccionar forma de pago . ", MsgBoxStyle.Information, "Arrendamiento .. ")
                    End If
                Else
                    MsgBox("El tipo de persona a un no se selecciona . ", MsgBoxStyle.Information, "Arrendamiento .. ")
                End If
            Else
                MsgBox("No hay registros para generar documento. ", MsgBoxStyle.Information, "Arrendamiento .. ")
            End If
        Else
            MsgBox("Falta seleccionar metodo de pago ", MsgBoxStyle.Information, "Arrendamiento .. ")
        End If
    End Sub

    Private Sub chboxPM_Click(sender As Object, e As EventArgs) Handles chboxPM.Click
        chboxPF.Checked = False
        selOP = "M"
    End Sub

    Private Sub chboxPF_Click(sender As Object, e As EventArgs) Handles chboxPF.Click
        chboxPM.Checked = False
        selOP = "F"
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function


    Private Sub btnInicializa_Click(sender As Object, e As EventArgs) Handles btnInicializa.Click
        DatClientes()
    End Sub

    Private Sub DatClientes()
        'grbClientes.Text = ""
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
    End Sub

    Private Sub BorrarRegistro()
        ugr.DataSource = Nothing
        limpiar()
        txtImport.Value = "0.0"
        impRetenido = 0
    End Sub

    Private Sub limpiar()
        txtSerie.Text = ""
        txtFactura.Text = ""
        txtLugarExpedicion.Text = ""
        grbformaPago.Text = ""
        txtNumCuenta.Text = ""
        txtCtaPredial.Text = ""
        txtDescripcion.Text = ""
        chboxPF.Checked = False
        chboxPM.Checked = False
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
    End Sub

    Private Sub grbPagos_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbPagos.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbPagos
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbPagos_ValueChanged(sender As Object, e As EventArgs) Handles grbPagos.ValueChanged
        Select Case grbPagos.Value
            Case "PUE"
                metodoPago = c_MetodoPago.PagoEnUnaSolaExhibicion_PUE
            Case "PPD"
                metodoPago = c_MetodoPago.PagoEnParcialidadesODiferido_PPD
        End Select
    End Sub

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
End Class