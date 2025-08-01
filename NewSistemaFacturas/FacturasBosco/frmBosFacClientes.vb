Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class frmBosFacClientes

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
    Dim va As Integer
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



    Private Sub frmBosFacClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        verNumCta(False)
        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        Me.txtFechaVenta.Value = dia + "/" + mes + "/" + amo
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Empresa FROM BGXFacElec.CatTien Where ttipo3 = 'G'", 0, 1, As400)
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        txtRfc.Focus()
        va = 0
        'fecConstancia.Value = Now.ToString.Substring(0, 10)
        REM Clientes()
    End Sub

    'Private Function Clientes() As Boolean
    'objpro.CargaCombos(grbClientes, "Select crfc,CRSOCIAL FROM BGXFacElec.catclie ORDER BY CRSOCIAL ASC ", 0, 1, As400)
    'Return False
    'End Function

    Private Sub grbTienda_TextChanged(sender As Object, e As EventArgs) Handles grbTienda.TextChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        Me.grbSucursal.Focus()
    End Sub

    Private Sub grbClientes_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs) Handles grbClientes.RowSelected
        Try
            Dim tablaclientes As Data.DataTable
            tablaclientes = objpro.ObtenDatos("SELECT crfc, crsocial, cemail, cusoCFDI as cusocfdi, cconrfc, cconfec, cconrfcr FROM bgxfacelec.catclie WHERE cRFC = '" & Me.grbClientes.Value & "'", As400).Tables(0)
            txtRfc.Text = tablaclientes.Rows(0).Item("crfc").ToString
            txtRazonSocial.Text = tablaclientes.Rows(0).Item("crSocial").ToString
            txtEmail.Text = tablaclientes.Rows(0).Item("cemail").ToString
            cfdiuso = tablaclientes.Rows(0).Item("cUsoCFDI").ToString()
            regimenFisc = tablaclientes.Rows(0).Item("cconrfcr").ToString()


            If cfdiuso <> "" Then
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
            End If

            If regimenFisc <> "" Then
                txtidrf.Text = regimenFisc
                txtdesrf.Text = objpro.Dato("regdes", "bgxfacelec.satregfis", "regimen = '" & txtidrf.Text & "'", As400)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub grbClientes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbClientes.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            Me.txtRfc.Focus()
        End If
    End Sub

    Private Sub btnBuscarNota_Click(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        If Me.txtRfc.Text <> "" Then
            If txtRazonSocial.Text <> "" Then
                If txtEmail.Text <> "" Then
                    If txtclvUsoCfdi.Text <> "" Then
                        If txtidrf.Text <> "" Then
                            Try
                                TIP = "FC"
                                Dim EstadoVenta As String
                                Terminal = Mid(Me.txtTransaccion.Text, 1, 3)
                                Dim trn = Len(Me.txtTransaccion.Text)
                                Transaccion = Mid(Me.txtTransaccion.Text, 5, trn)

                                EstadoVenta = objpro.Dato("Cast(veEstatu as varchar(5))", "BGXFacElec.VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and veFecha = '" & txtFechaVenta.Value.ToString.Substring(6, 4) & txtFechaVenta.Value.ToString.Substring(3, 2) & txtFechaVenta.Value.ToString.Substring(0, 2) & "' ", As400)
                                If EstadoVenta = 0 Then
                                    Application.DoEvents()
                                    Tabla = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", As400).Tables(0)
                                    Application.DoEvents()
                                    TSucursal = objpro.ObtenDatos("SELECT * FROM bgxfacelec.Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400).Tables(0)
                                    Me.txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")
                                    Me.txtSerie.Text = TSucursal.Rows(0).Item("Seserie").ToString

                                    Application.DoEvents()
                                    CargaGridInicio()
                                    Application.DoEvents()

                                Else
                                    MsgBox("Ya se facturó la nota", MsgBoxStyle.Information, "Facturacion")
                                End If
                            Catch ex As Exception
                                MsgBox(ex.Message)
                            End Try REM Cierra el Try de Inicio
                        Else
                            MsgBox("Regimen Fiscal no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
                        End If
                    Else
                        MsgBox("Uso del CFDI no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
                    End If
                Else
                    MsgBox("Falta Email", MsgBoxStyle.Information, "Facturación")
                End If
            Else
                MsgBox("Razón Social, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("RFC, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub CargaGridInicio()
        If Me.txtRfc.Text <> "" Then
            If txtRazonSocial.Text <> "" Then
                If txtEmail.Text <> "" Then
                    Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
                    Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
                    Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
                    Dim FechaBuscar As String = a & m & d
                    objpro.LLenaGrid(Me.UgrTicket, "SELECT vdcodsat as codSat, vdclvuni as ClaveSat,  VdClaPro as ClaveInterno, VdDescri as Descripcion, " &
                                                    " VdCant as Cantidad, vdUnidad as Unidad, vdPorIva as PorIva, VdIva as Iva, " &
                                                    " VdPreUni as PrecioUnitario, vdDesc1 as Desc1, vdDesc2 as Desc2, VdPreTot as Importe, vdFEcha FROM BGXFacElec.VentaDet Where vdfolio = 1 and sid = " & Me.grbSucursal.Value & " and  vdTerm = " & Terminal & "  and VdIdFolv = " & Transaccion & " and vdFecha = '" & FechaBuscar & "' ", "VentaDet", As400)
                    Me.UgrTicket.DataBind()

                    Importe.Value = 0
                    If UgrTicket.Rows.Count <> 0 Then
                        For i = 0 To UgrTicket.Rows.Count - 1
                            Importe.Value += (Me.UgrTicket.Rows(i).Cells("Cantidad").Value * Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value)
                            Descuento.Value += (Me.UgrTicket.Rows(i).Cells("Cantidad").Value * Me.UgrTicket.Rows(i).Cells("Desc1").Value)
                        Next
                    End If

                    Dim PantallaSubtotal, PantallaDesc1, PantallaDesc2, PantallaIva, PantallaTotal As Double
                    PantallaSubtotal = 0 : PantallaDesc1 = 0 : PantallaDesc2 = 0 : PantallaIva = 0 : PantallaTotal = 0

                    Subtotal.Value = (Importe.Value - Descuento.Value)
                    Iva.Value = (Subtotal.Value * 0.16)
                    Total.Value = (Subtotal.Value + Iva.Value)
                Else
                    MsgBox("Falta Email", MsgBoxStyle.Information, "Facturación")
                End If
            Else
                MsgBox("Razón Social, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
            End If 'Cierra validacion de Razon Social
        Else
            MsgBox("RFC, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
        End If 'Cierra el RFC
    End Sub

    Private Sub UgrTicket_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UgrTicket.InitializeLayout
        With Me.UgrTicket
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 80
            .DisplayLayout.Bands(0).Columns(2).Width = 100
            .DisplayLayout.Bands(0).Columns(3).Width = 300
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Width = 60
            .DisplayLayout.Bands(0).Columns(6).Hidden = True
            .DisplayLayout.Bands(0).Columns(7).Width = 70
            .DisplayLayout.Bands(0).Columns(8).Width = 100
            .DisplayLayout.Bands(0).Columns(9).Width = 70
            .DisplayLayout.Bands(0).Columns(10).Hidden = True
            .DisplayLayout.Bands(0).Columns(11).Width = 80
            .DisplayLayout.Bands(0).Columns(12).Hidden = True
        End With
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDIBosco)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Private Sub btnregimen_Click(sender As Object, e As EventArgs) Handles btnregimen.Click
        Formulario(frmRegimenFiscal)
        txtidrf.Text = regimenFisc
        txtdesrf.Text = regimen
    End Sub

    Private Sub frmBosFacClientes_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        cfdiuso = ""
        cfdides = ""
        Me.Dispose()
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        TIP = "FC"
        'va = 0
        If Me.txtEmail.Text <> "" And Me.txtRazonSocial.Text <> "" And Me.txtRfc.Text <> "" And txtclvUsoCfdi.Text <> "" Then
            FolioFinal = objpro.Dato("seffin", "bgxfacelec.serie", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seSerie = '" & txtSerie.Text & "' and seTipo = '" & TIP & "' and seEstat = 1  ", As400)
            upb.Value = 0
            upb.Maximum = 100
            Dim ExisterfcClie As String
            ExisterfcClie = objpro.DatoString("crfc", "bgxfacelec.catclie", "crfc = '" & Trim(txtRfc.Text) & "' ", As400)
            If ExisterfcClie <> "" Then
                tablaclientes = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CATCLIE WHERE cRFC = '" & Me.txtRfc.Text & "'", As400).Tables(0)
                Dim ne As String = tablaclientes.Rows(0).Item("cNumExt").ToString.Trim
                If ne.ToString = "0" Then
                    ne = " "
                End If
                cDireccion = tablaclientes.Rows(0).Item("cDir").ToString.Trim & " " & ne & " " & tablaclientes.Rows(0).Item("cNumInt").ToString.Trim & " " & tablaclientes.Rows(0).Item("cColonia").ToString.Trim
                cComDir = tablaclientes.Rows(0).Item("cMun").ToString.Trim & " " & tablaclientes.Rows(0).Item("cEstado").ToString.Trim & " " & tablaclientes.Rows(0).Item("cLocal").ToString.Trim & " " & tablaclientes.Rows(0).Item("cPais").ToString.Trim
                'cCP = tablaclientes.Rows(0).Item("cCP").ToString()
            End If

            If txtCP.Text <> "" Then

                If grbformaPago.Text <> "" Then
                    If CInt(Me.txtFactura.Text) <= FolioFinal Then
                        Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
                        Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
                        Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
                        Dim FecBuscFPago As String = a & m & d

                        Try
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
                            Dim UserPAC As String = Trim(Tabla.Rows(0).Item("tUserPAC").ToString)
                            Dim PwdPAC As String = Trim(Tabla.Rows(0).Item("tPwdPAC").ToString)
                            Dim rutlogo As String = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO_BOSCO.png" 'Trim(Tabla.Rows(0).Item("rutLogo").ToString)
                            Dim descreg As String = Trim(Tabla.Rows(0).Item("sRFiscal").ToString)

                            Dim FechaFactura As DateTime = Now
                            Dim FechaHora As String = FechaFactura.ToString.Substring(10, 10)

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
                            Tabla1.Columns.Add("rcomdir")
                            Tabla1.Columns.Add("rusocfdi")
                            Tabla1.Columns.Add("nocuenta")

                            Dim Fila As DataRow

                            Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
                            Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
                            Fila("Erazonsoc") = TRazonSocial
                            Fila("Eregfis") = TRFiscal.ToString + " - " + descreg.Trim
                            Fila("Edireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
                            Fila("Edircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString

                            Dim RFacImpresas As String
                            RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

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

                            Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

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


                            'Llenamos la sección del Emisor
                            EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TRFiscal)
                            'EasyOne_cfdi33_SetEmisor(p, TRfc, "POVISA", "601")

                            'Llenamos la sección del Receptor
                            'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                            'EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01")
                            EasyOne_cfdi33_SetReceptor(p, txtRfc.Text, txtRazonSocial.Text, "", "", txtclvUsoCfdi.Text) 'estos paramentros son si o si para factura global
                            'EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616
                            EasyOne_cfdi33_SetReceptor40(p, txtCP.Text, txtidrf.Text)

                            Dim NumPiezas As Integer
                            Cadena = txtFechaVenta.Value.ToString.Substring(0, 10) & "- " & Trim(Me.txtTransaccion.Text) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))

                            REM Datos clientes ***************************************
                            Try
                                Dim Existerfc As String
                                Existerfc = objpro.DatoString("crfc", "bgxfacelec.catclie", "crfc = '" & Trim(txtRfc.Text) & "' ", As400)
                                If Existerfc = "" Then
                                    Dim cdir As String = ""
                                    Dim cnumext = 0
                                    Dim cnumint = 0
                                    Dim ccolonia As String = ""
                                    Dim cmun As String = ""
                                    Dim cestado As String = ""
                                    Dim clocal As String = ""
                                    Dim cpais As String = ""

                                    objpro.Indicacion("Insert Into bgxfacelec.catclie (crsocial, crfc, cEmail, cusocfdi, cdir, cnumext, cnumint, ccolonia, cmun, cestado, clocal, cpais, ccp, cconrfc, cconfec, cconfecr ) " &
                                    "values ('" & Me.txtRazonSocial.Text & "', '" & Me.txtRfc.Text & "', '" & Me.txtEmail.Text & "', '" & Me.txtclvUsoCfdi.Text & "', " &
                                    " '" & cdir & "', " & cnumext & ", '" & cnumint & "', '" & ccolonia & "', '" & cmun & "', '" & cestado & "', '" & clocal & "', '" & cpais & "', " & Me.txtCP.Text & ", 'N', " & 0 & ", " & txtidrf.Text & " ) ", As400)
                                End If
                            Catch ex As Exception
                                MsgBox(ex.Message & "No se pudo registrar al cliente .. ", MsgBoxStyle.Information, "Facturación .. ")
                            End Try
                            REM Datos clientes ***************************************

                            Fila("sucursal") = Cadena
                            Fila("Rcp") = txtCP.Text 'TCp
                            Fila("Rrfc") = txtRfc.Text '"XAXX010101000"
                            Fila("Rrazonsoc") = txtRazonSocial.Text '"PUBLICO EN GENERAL"
                            Fila("Rdireccion") = cDireccion '""
                            Fila("Rregimen") = txtidrf.Text & " - " & txtdesrf.Text '
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
                            Dim IMTE As Double = 0


                            Dim Importe As Double = 0
                            Dim Impuesto As String = ""
                            Dim TasaOCuota As String = ""
                            Dim TipoFactor As String = ""
                            Dim Impu As String = ""
                            Dim ToC As String = ""
                            Dim TFac As String = ""
                            Dim dcto As String = ""
                            'Dim ipt As String = ""

                            Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
                            Tabla2.Columns.Add("Cantidad")
                            Tabla2.Columns.Add("Unidad")
                            Tabla2.Columns.Add("UniSAT")
                            Tabla2.Columns.Add("Descrip")
                            Tabla2.Columns.Add("Punitario")
                            Tabla2.Columns.Add("Importe")
                            Dim Fila2 As DataRow

                            For i = 0 To Me.UgrTicket.Rows.Count - 1
                                Dim codsat As String
                                If Me.UgrTicket.Rows(i).Cells("codsat").Value = "" Then
                                    codsat = "01010101"
                                Else
                                    codsat = Me.UgrTicket.Rows(i).Cells("codsat").Value
                                End If
                                Dim pun As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                                Dim producto As String = Trim(Me.UgrTicket.Rows(i).Cells("DESCRIPCION").Value)
                                'Dim Puni As String
                                'Puni = pun.Replace(",", "")
                                Dim impt As Double = Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00")
                                ' ipt = impt.ToString.Replace(",", "")
                                IMTE = Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00")
                                sb += Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00")
                                NumPiezas += Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                                Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                                Fila2("Cantidad") = Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                Fila2("Unidad") = "PZA"
                                Fila2("UniSAT") = "H87"
                                Fila2("Descrip") = producto
                                Fila2("Punitario") = Format(pun, "###,###.00")
                                Fila2("Importe") = Format(impt, "###,###.00")
                                DS.Tables(1).Rows.Add(Fila2)
                                Dim descto As Double = 0

                                If UgrTicket.Rows(i).Cells("Desc1").Value <> 0 Then
                                    If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") <> 0 Then
                                        Dim Descu As Double = (Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                        dcto = Descu.ToString.Replace(",", "")
                                        descto = Convert.ToDouble(dcto)
                                        'EasyOne_cfdi33_AddConcepto(p, "01010101", "", Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "ACT", "", "Venta", Puni, impt.ToString, dcto)
                                        EasyOne_cfdi33_AddConcepto(p, codsat, Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), "PZA", producto, Format((pun), "######.00"), Format((impt), "######.00"), descto) 'Format((Descu), "######.00")
                                        des += (Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                    Else
                                        Dim Descu As Double = ((Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") - 0.01) * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                        dcto = Descu.ToString.Replace(",", "")
                                        'EasyOne_cfdi33_AddConcepto(p, "01010101", "", Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "ACT", "", "Venta", Puni, impt.ToString, dcto)
                                        EasyOne_cfdi33_AddConcepto(p, codsat, Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), "PZA", producto, Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                                        des += ((Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") - 0.01) * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                    End If
                                Else
                                    'EasyOne_cfdi33_AddConcepto(p, "01010101", "", Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "ACT", "", "Venta", Puni, impt, "")
                                    EasyOne_cfdi33_AddConcepto(p, codsat, Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), "PZA", producto, Format((pun), "######.00"), Format((impt), "######.00"), "")
                                End If
                                'Se debe de agregar si el concepto es Objeto de impuesto
                                EasyOne_cfdi33_SetConcepto40(p, "02")

                                If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") <> 0 Then
                                    Imp = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value - Me.UgrTicket.Rows(i).Cells("Desc1").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value)) * 0.16), "###,###.00")
                                    iv += Imp
                                    bas = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value - Me.UgrTicket.Rows(i).Cells("Desc1").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value))), "###,###.00")
                                    Impu = "002"
                                    ToC = "0.160000"
                                    TFac = "Tasa"
                                    EasyOne_cfdi33_AddTraslado(p, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
                                Else
                                    Imp = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value - Me.UgrTicket.Rows(i).Cells("Desc1").Value) - ((Me.UgrTicket.Rows(i).Cells("Cantidad").Value - 0.01))) * 0.16), "###,###.00")
                                    iv += Imp
                                    bas = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value - Me.UgrTicket.Rows(i).Cells("Desc1").Value) - ((Me.UgrTicket.Rows(i).Cells("Cantidad").Value - 0.01)))), "###,###.00")
                                    Impu = "002"
                                    ToC = "0.160000"
                                    TFac = "Tasa"
                                    EasyOne_cfdi33_AddTraslado(p, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
                                End If

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

                            'Subt = Format(Me.Subtotal.ToString, "###,###.00")
                            'Subt = Format(sb, "###,###.00")
                            Subt = Format(sb, "###,###.00")
                            Total = Format(((Subt - des) + iv), "###,###.00")
                            'Total = Format((Subt + iv), "###,###.00")
                            Fila("Fsubtotal") = Format(Subt, "###,###.00")
                            Fila("Fdescuento") = Format(des, "###,###.00")
                            Fila("Fiva") = Format(iv, "###,###.00")
                            Fila("Ftotal") = Format(Total, "###,###.00")

                            Dim cantletra As String
                            cantletra = Letras(Total)
                            Fila("cantidadl") = cantletra

                            Dim ValorTicket As String = Cadena
                            NoCuenta = txtNumCuenta.Text

                            Dim f1, f2, f3, freal, FeReg, hr As String
                            f1 = txtFechaVenta.Text.ToString.Substring(0, 2)
                            f2 = txtFechaVenta.Text.ToString.Substring(3, 2)
                            f3 = txtFechaVenta.Text.ToString.Substring(6, 4)
                            hr = Now.ToLocalTime.ToString.Substring(11, 8)
                            freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                            FeReg = txtFechaVenta.Text.ToString.Substring(6, 4) & f2 & f1

                            Dim metpg As String
                            Dim tipcom As String
                            Dim formpg As String
                            metpg = "PUE"
                            tipcom = "I"
                            formpg = grbformaPago.Value.ToString '& " - " & FormPago.ToString'"99""99"

                            Dim Descue As String = ""
                            Dim desc2 As String
                            Descue = Format(des, "###,###.00")
                            desc2 = Descue.Replace(",", "")

                            Dim TipoCambio As Int16 = 1
                            'Llenamos la sección del comprobante
                            If desc2 = ".00" Or desc2 = "" Then
                                EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, formpg, "", Format((Subt), "######.00"), "", "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
                            Else
                                EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, formpg, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
                            End If

                            Fila("metpago") = metpg
                            If tipcom = "I" Then
                                Fila("tipocomp") = "I - Ingreso"
                            Else
                                Fila("tipocomp") = "E - Egreso"
                            End If
                            Fila("formpago") = FormPago 'fp

                            'Enviamos el CFDI al metodo de PRUEBAS
                            'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                            'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBAS
                            EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")
                            Application.DoEvents()
                            upb.Value += 20  'Suma 30
                            'Dim resultado As CFDI.CFDI33.ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)

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

                            'Si el resultado es 0 = El CFDI se procesó correctamente
                            If x = 0 And va = 1 Then
                                Application.DoEvents()
                                Dim resultadoXML As String = [String].Empty
                                resultadoXML = EasyOne_LeeResultado(p, 8)
                                'Obtenemos el XML completo y timbrado
                                Console.WriteLine(resultadoXML)

                                Application.DoEvents()
                                IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"), resultadoXML)

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
                                documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"))
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
                                    RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml.png")

                                    '''''''''''''''''''''''''''''''''''''''''''''''''''
                                    Dim Fecha11, Fecha22, Fecha33, FePnotasF As String
                                    Fecha11 = txtFechaVenta.Text.ToString.Substring(0, 2)
                                    Fecha22 = txtFechaVenta.Text.ToString.Substring(3, 2)
                                    Fecha33 = txtFechaVenta.Text.ToString.Substring(6, 4)
                                    FePnotasF = Fecha33 & Fecha22 & Fecha11
                                    '    fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                                    '    FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha22 & Fecha11
                                    '    Fecha = fechareal

                                    Dim Tterm = CInt(Mid(Me.txtTransaccion.Text, 1, 3))
                                    Dim Nnota = CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text)))
                                    Dim NotaVta As String = Tterm & Format(CDbl(Nnota), "0000")
                                    Dim TieSuc As String = Me.grbTienda.Value & Me.grbSucursal.Value
                                    Dim PorIva As Double = 16.0

                                    objpro.Indicacion("Update bgxfacelec.VentaEnc set veEstatu = 1, veFac = '" & CInt(Me.txtFactura.Text) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text))) & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & CInt(Mid(Me.txtTransaccion.Text, 1, 3)) & " and  veEstado = 1 ", As400)
                                    objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", As400)
                                    objpro.Indicacion("Update bgxfacelec.pfnotasf set NFSTS = 'F' Where NFSUC = " & CInt(TieSuc) & " and NFFECHA = " & Trim(FePnotasF) & " and NFNOTA = " & NotaVta & "", As400)
                                    upb.Value += 10 'Suma 40 
                                    ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = '" & Trim(Me.txtRfc.Text) & "' ", As400))
                                    Dim Tipo As String = "F"
                                    Dim cantidadconletra As String = ""
                                    Dim FeCan = 0
                                    Dim FeMotCan As String = ""
                                    Dim ferisr = 0
                                    Dim feriva = 0

                                    Dim Subtotal = Me.Subtotal.Value
                                    Dim Iva = Me.Iva.Value
                                    Dim Total1 = Me.Total.Value
                                    Dim Descuento = Me.Descuento.Value

                                    Dim FeRegistro, Fecha, fechareal, Fecha1, Fecha2, Fecha3 As String
                                    Fecha1 = txtFechaVenta.Text.ToString.Substring(0, 2)
                                    Fecha2 = txtFechaVenta.Text.ToString.Substring(3, 2)
                                    Fecha3 = txtFechaVenta.Text.ToString.Substring(6, 4)
                                    FePnotasF = Fecha3 & Fecha2 & Fecha1
                                    fechareal = Fecha1 & "/" & Fecha2 & "/" & Fecha3.Substring(2, 2)
                                    FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha2 & Fecha1
                                    Fecha = fechareal

                                    objpro.Indicacion("Insert into BGXFacElec.FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid," &
                                            " feFecha,feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                            " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                            " FeCanlet,FeAbonos,FeEstatu, FeReg, fefeccan, femotcan, fetip, feuuid, ferisr, feriva) " &
                                            " values (" & CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text))) & "," & grbTienda.Value & "," & CInt(Mid(Me.txtTransaccion.Text, 1, 3)) & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," &
                                            " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & "," & Subtotal & ", " & CDbl(PorIva) & "," & Me.Iva.Value & "," &
                                            " " & Descuento & "," & Descuento & ", " & Descuento & ", " & Descuento & ", " & Me.Total.Value & ", " &
                                            " '" & cantletra & "', " & Total1 & "," & Estatus & "," & FeRegistro & ", '" & FeCan & "', '" & FeMotCan & "', '" & Tipo & "','" & uuid & "', " & ferisr & "," & feriva & ")", As400)

                                    upb.Value += 20 'Suma 60
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
                                    BorrarRegistro()
                                    DatClientes()
                                    btnFacturar.Visible = True
                                    va = 0
                                Else
                                    MsgBox("Archivo XML no es una factura version 4.0")
                                End If 'Cierra validación de operación exitosa
                            Else
                                'En caso de error mostrar el detalle.
                                Dim resultadoXML2 As String = [String].Empty
                                resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                Console.WriteLine(resultadoXML2)
                                MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                btnFacturar.Visible = True
                            End If 'Cierra validación generación de XML
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                        End Try 'Cierra validación del try
                    Else
                        btnFacturar.Visible = True
                    End If 'Cierra validación de folio inicial contra folio final
                Else
                    MsgBox("Falta seleccionar forma de pago .. ", MsgBoxStyle.Information, "Facturación .. ")
                    btnFacturar.Visible = True
                End If 'Cierra la validacion de forma de pago
            Else
                MsgBox("Informacion fiscal del cliente incompleta, favor de actualizar en el catalogo de clientes .. ", MsgBoxStyle.Information, "Facturación .. ")
                btnFacturar.Visible = True
            End If
        Else
            btnFacturar.Visible = True
        End If 'Cierra validación de campos
    End Sub

    Private Sub BorrarRegistro()
        UgrTicket.DataSource = Nothing
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
        Me.txtFechaVenta.Value = dia + "/" + mes + "/" + amo
        Me.txtTransaccion.Text = ""
    End Sub

    Private Sub limpiar()
        Me.txtSerie.Text = ""
        Me.txtFactura.Text = ""
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function


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

    Private Sub btnInicializa_Click(sender As Object, e As EventArgs) Handles btnInicializa.Click
        DatClientes()
    End Sub

    Private Sub DatClientes()
        grbClientes.Text = ""
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        txtCP.Text = ""
        txtidrf.Text = ""
        txtdesrf.Text = ""

        'uop.Value = ""
        'fecConstancia.Value = Now.ToString.Substring(0, 10)
    End Sub

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "bgxfacelec.satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", As400) = True Then
            verNumCta(True)
        Else
            verNumCta(False)
        End If
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub grbSucursal_TextChanged(sender As Object, e As EventArgs) Handles grbSucursal.TextChanged
        Me.txtTransaccion.Focus()
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
            txtCP.Text = cCP
            cfdiuso = objpro.Dato("cusocfdi", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtclvUsoCfdi.Text = cfdiuso
            txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
            txtidrf.Text = regimenFisc
            txtdesrf.Text = regimen

            'Try
            '    Select Case objpro.DatoString("cconrfc", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            '        Case Is = "S"
            '            uop.Value = "S"
            '            Dim feccon = objpro.DatoString("cconfec", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            '            If feccon <> "0" And feccon <> "" Then
            '                Dim fecreg As String = feccon.Substring(6, 2) & "/" & feccon.Substring(4, 2) & "/" & feccon.Substring(0, 4)
            '                fecConstancia.Value = CDate(fecreg)
            '            End If
            '        Case Is = "N"
            '            uop.Value = "N"
            '            fecConstancia.Value = ""
            '        Case Else
            '            uop.Value = "N"
            '            fecConstancia.Value = ""

            '    End Select
            'Catch ex As Exception
            '    uop.Value = "N"
            '    REM fecConstancia.Value = CDate("01/01/1900")
            '    fecConstancia.Value = ""
            'End Try
        End If
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
            btnUsoCfdi.Focus()
        End If
    End Sub

    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        ExportarExcel.Export(UgrTicket, "c:\Archivo.xls")
    End Sub
End Class