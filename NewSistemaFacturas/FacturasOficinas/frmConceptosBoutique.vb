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

Public Class frmConceptosBoutique

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
    Dim b_iva As Double
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

    Private Sub frmConceptosBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "FC"
        va = 0
        verNumCta(False)
        txtRfc.Focus()
        CargaGridConceptos()
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Empresa FROM CatTien ", 0, 1, Motor)
        Application.DoEvents()
        objpro.CargaCombos(grbUnidad, "Select IdUni as Id, Unides as Unidad FROM BGXFacElec.satUnidad where unists = 'A'", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from bgxfacelec.satmpag ", 0, 1, As400)
        Application.DoEvents()
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.ugr, "Select VdCant as Cantidad, VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion, " &
                         "  VdPreUni as PrecioUnitario, '0' as Descuento, '0' as Iva, " &
                         " VdPreTot as Importe From VentaDet " &
                         " Where VdFolio = 0", "VentaDet", Motor)


        Me.ugr.DataBind()
    End Sub

    Private Sub frmConceptosBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
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
        If Me.txtRfc.Text <> "" Then
            If txtRazonSocial.Text <> "" Then
                If txtEmail.Text <> "" Then
                    If txtclvUsoCfdi.Text <> "" Then
                        If RF.Trim <> "" Then
                            If cCP.ToString <> "0" Or cCP.ToString.Trim <> "" Then
                                Tabla = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", Motor).Tables(0)
                                Application.DoEvents()
                                txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                                txtSerie.Text = objpro.Dato("seserie", "serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1", Motor)
                            Else
                                MsgBox("Código Postal no puede ir vacio", MsgBoxStyle.Information, "Facturación")
                            End If
                        Else
                            MsgBox("Regimen Fiscal no puede ir vacio", MsgBoxStyle.Information, "Facturación")
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
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDIBosco)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Private Sub grbUnidad_VisibleChanged(sender As Object, e As EventArgs) Handles grbUnidad.VisibleChanged
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

    Private Sub FormatoGrid(concepto As String)
        ugr.Rows.Band.AddNew()
        ColocarArticulo(ugr.Rows.Count - 1, concepto)
        LimpiarCajas()
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

    Private Sub LimpiarCajas()
        Me.txtCantidad.Text = "1"
        grbUnidad.Text = ""
        txtClave.Text = "01010101"
        Me.txtDescripcion.Text = ""
        txtPUnitario.Value = 0.0
        txtIva.Text = 16
        b_iva = 0
        Me.txtImport.Value = 0.0
    End Sub

    Private Sub txtDescripcion_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDescripcion.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            btnAgregar.Focus()
        End If
    End Sub

    Private Sub ugr_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugr.AfterRowsDeleted
        sumariza_grid()
    End Sub

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbformaPago_TextChanged(sender As Object, e As EventArgs) Handles grbformaPago.TextChanged
        grbformaPago.Text = grbformaPago.Text
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "bgxfacelec.satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", As400) = True Then
            verNumCta(True)
            Me.txtNumCuenta.Focus()
        Else
            verNumCta(False)
            Me.btnFacturar.Focus()
        End If
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        If grbformaPago.Value <> 0 Then
            TIP = "FC"
            FolioFinal = objpro.Dato("seffin", "serie", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seSerie = '" & txtSerie.Text & "' and seTipo = '" & TIP & "' and seEstat = 1  ", Motor)
            upb.Value = 0
            upb.Maximum = 100
            tablaclientes = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CATCLIE WHERE cRFC = '" & Me.txtRfc.Text & "'", As400).Tables(0)
            Dim ne As String = tablaclientes.Rows(0).Item("cNumExt").ToString.Trim
            If ne.ToString = "0" Then
                ne = " "
            End If
            cDireccion = tablaclientes.Rows(0).Item("cDir").ToString.Trim & " " & ne & " " & tablaclientes.Rows(0).Item("cNumInt").ToString.Trim & " " & tablaclientes.Rows(0).Item("cColonia").ToString.Trim
            cComDir = tablaclientes.Rows(0).Item("cMun").ToString.Trim & " " & tablaclientes.Rows(0).Item("cEstado").ToString.Trim & " " & tablaclientes.Rows(0).Item("cLocal").ToString.Trim & " " & tablaclientes.Rows(0).Item("cPais").ToString.Trim
            If CInt(Me.txtFactura.Text) <= FolioFinal Then
                Dim d As String = Now.Date.ToString.Substring(0, 2)
                Dim m As String = Now.Date.ToString.Substring(3, 2)
                Dim a As String = Now.Date.ToString.Substring(6, 4)
                Dim FecBuscFPago As String = a & m & d

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
                Dim rutlogo As String = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png" 'Trim(Tabla.Rows(0).Item("rutLogo").ToString)
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

                'Dim c As New CFDI.CFDI33.Comprobante(Today, txtSerie.Text, txtFactura.Text, c_TipoDeComprobante.Ingreso, FormPago, CFDI.CFDI33.c_MetodoPago.PagoEnUnaSolaExhibicion_PUE)
                'c.Moneda = CFDI.CFDI33.c_Moneda.MXN
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
                EasyOne_cfdi33_SetReceptor40(p, cCP, RF)

                Dim NumPiezas As Integer
                Cadena = Now.Date.ToString.Substring(0, 10) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))

                'REM Datos clientes ***************************************
                'Try
                '    Dim Existerfc As String
                '    Existerfc = objpro.DatoString("crfc", "bgxfacelec.catclie", "crfc = '" & Trim(txtRfc.Text) & "' ", As400)
                '    If Existerfc = "" Then
                '        Dim cdir As String = ""
                '        Dim cnumext = 0
                '        Dim cnumint = 0
                '        Dim ccolonia As String = ""
                '        Dim cmun As String = ""
                '        Dim cestado As String = ""
                '        Dim clocal As String = ""
                '        Dim cpais As String = ""
                '        Dim ccp = 0
                '        objpro.Indicacion("Insert Into bgxfacelec.catclie (crsocial, crfc, cEmail, cusocfdi, cdir, cnumext, cnumint, ccolonia, cmun, cestado, clocal, cpais, ccp ) " & _
                '                         "values ('" & Me.txtRazonSocial.Text & "', '" & Me.txtRfc.Text & "', '" & Me.txtEmail.Text & "', '" & Me.txtclvUsoCfdi.Text & "', " & _
                '                         " '" & cdir & "', " & cnumext & ", '" & cnumint & "', '" & ccolonia & "', '" & cmun & "', '" & cestado & "', '" & clocal & "', '" & cpais & "', " & ccp & " ) ", As400)
                '    End If
                'Catch ex As Exception
                'End Try
                REM Datos clientes ***************************************
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
                    Fila2("Cantidad") = Trim(Me.ugr.Rows(i).Cells("Cantidad").Value)
                    Fila2("Unidad") = "PZA"
                    Fila2("UniSAT") = Trim(Me.ugr.Rows(i).Cells("Unidad").Value)
                    Fila2("Descrip") = Trim(Me.ugr.Rows(i).Cells("Descripcion").Value)
                    Fila2("Punitario") = pun
                    Fila2("Importe") = impt
                    DS.Tables(1).Rows.Add(Fila2)
                    If ugr.Rows(i).Cells("Descuento").Value <> 0 Then
                        Descu = Format(Me.ugr.Rows(i).Cells("Descuento").Value, "###,###.00")
                        des += Format(Me.ugr.Rows(i).Cells("Descuento").Value, "###,###.00")
                        EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "PZA", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                    Else
                        EasyOne_cfdi33_AddConcepto(p, Trim(Me.txtClave.Text), Trim(Me.ugr.Rows(i).Cells("Clave").Value), Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("Unidad").Value), "PZA", Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format((pun), "######.00"), Format((impt), "######.00"), "")
                    End If
                    'Se debe de agregar si el concepto es Objeto de impuesto
                    EasyOne_cfdi33_SetConcepto40(p, "02")
                    Imp = Format((((Me.ugr.Rows(i).Cells("PrecioUnitario").Value * Me.ugr.Rows(i).Cells("Cantidad").Value) - Me.ugr.Rows(i).Cells("Descuento").Value) * 0.16), "###,###.00")
                    iv += Imp
                    bas = Format((((Me.ugr.Rows(i).Cells("PrecioUnitario").Value * Me.ugr.Rows(i).Cells("Cantidad").Value) - Me.ugr.Rows(i).Cells("Descuento").Value)), "######.00")
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
                NoCuenta = txtNumCuenta.Text

                Dim freal, hr As String
                'hr = Now.ToLocalTime.ToString.Substring(11, 8)
                'freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                'If des = 0 Then
                '    EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((Subt), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "I", grbPagos.Value, TCp, "")
                'Else
                '    EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", "1", Format((Total), "######.00"), "I", grbPagos.Value, TCp, "")
                'End If
                Fila("metpago") = grbPagos.Value ''"PUE" '
                Fila("tipocomp") = "I - Ingreso"
                Fila("formpago") = FormPago 'fp

                ''Enviamos el CFDI al metodo de PRUEBAS
                ''En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                ''EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBAS
                'EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

                upb.Value += 20  'Suma 30
                'Timbra XML, que se encuentra en variable P
                Dim x As Integer = 99
                If va = 0 Then
                    hr = Now.ToLocalTime.ToString.Substring(11, 8)

                    If hra <> "" Then
                        MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                        hr = hra
                    End If

                    freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

                    If (serie = "" And folio = "") Or (serie <> txtSerie.Text And folio <> txtFactura.Text) Then
                        If des = 0 Then
                            EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((Subt), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "I", grbPagos.Value, TCp, "")
                        Else
                            EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", "1", Format((Total), "######.00"), "I", grbPagos.Value, TCp, "")
                        End If

                        'Enviamos el CFDI al metodo de PRUEBAS
                        'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                        'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBAS
                        EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

                        x = EasyOne_FirmaComprobante(p, CerFactura, Key, Pwd)
                        Thread.Sleep(10000) 'espera por 10 segundos
                        If x = 0 Then
                            va += 1
                            hra = hr
                            serie = txtSerie.Text
                            folio = txtFactura.Text
                        Else
                            btnFacturar.Visible = True
                            Dim resultadoXML2 As String = [String].Empty
                            resultadoXML2 = EasyOne_LeeResultado(p, 11)
                            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                            hra = hr
                        End If
                    Else
                        MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
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
                        ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = '" & Trim(Me.txtRfc.Text) & "' ", As400))
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
                        DatClientes()
                        CargaGridConceptos()
                        btnFacturar.Visible = True
                        va = 0
                    Else
                        MsgBox("Archivo XML no es una factura version 4.0")
                    End If 'Cierra respuesta del PAC
                Else
                    'En caso de error mostrar el detalle.
                    Dim resultadoXML2 As String = [String].Empty
                    resultadoXML2 = EasyOne_LeeResultado(p, 11)
                    Console.WriteLine(resultadoXML2)
                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                    btnFacturar.Visible = True
                End If 'Cierra cracion de XML
            Else
                btnFacturar.Visible = True
            End If 'Cierra validacion de factura y folio
        Else
            btnFacturar.Visible = True
        End If 'Cierra validacion forma de pago
    End Sub

    Private Sub BorrarRegistro()
        ugr.DataSource = Nothing
        limpiar()
        Importe.Value = "0.0"
        Subtotal.Value = "0.0"
        Descuento.Value = "0.0"
        Iva.Value = "0.0"
        Total.Value = "0.0"

        grbformaPago.Text = ""
        txtNumCuenta.Text = ""
        verNumCta(False)

        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
    End Sub

    Private Sub limpiar()
        Me.txtSerie.Text = ""
        Me.txtFactura.Text = ""
    End Sub

    Private Sub DatClientes()
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function


End Class