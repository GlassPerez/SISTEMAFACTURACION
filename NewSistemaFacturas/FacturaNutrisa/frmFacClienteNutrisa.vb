Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml

Public Class frmFacClienteNutrisa

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Datos de Factura"    
    Dim txtNumAprobacion As String
    Dim txtAmoAprob As String
    Dim txtLugarExpedicion As String
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaSucursal, TablaTienda, TabExpeTicket As Data.DataTable
    Dim ArrayArchivos As New ArrayList
    Dim fechventa As String
    Dim Timbrado As Boolean = False
    Dim va As Integer
    Dim FechaFactura As String
    Dim ArchivoGeneral As String
    Dim Transaccion, TpDev As Integer
    Dim td As Integer
    Dim FolioFinal
    Dim ExisteClave As String
    Dim Terminal As Integer
    Dim cCP As String
    Dim cDireccion As String
    Dim cComDir As String
    Dim RegimenF As String
    Dim hra As String
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

#Region "Anexo"
    Dim fp As String
    Dim Cadena As String
    Dim NumPiezas As Integer
    Dim NoCuenta As String
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
#End Region

#Region "Sat"
    Dim TpRegimen As String
    Dim DesRegimen As String
    Dim Sat_UsoCFDI As String
    Dim Uso_CFDI As String
    Dim e_uuid As String
#End Region

#Region "Forma Pago"
    Dim FormPago As String
    Dim MetPago As String    
#End Region

    Private Sub frmFacClienteNutrisa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        verNumCta(False)
        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)        
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        va = 0
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where Tid = " & Tienda & "", 0, 1, Motor)
        Me.grbTienda.Value = Tienda
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Value = Sucursal
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from satfpag ", 0, 1, Motor)
        'Clientes()
        Application.DoEvents()
        CargaGridConceptos()
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    'Private Function Clientes() As Boolean
    '    objpro.CargaCombos(grbClientes, "Select distinct crfc,CRSOCIAL from CATCLIE", 0, 1, Motor)
    '    Return False
    'End Function

    Private Sub frmFacClienteNutrisa_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub grbClientes_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            Me.txtRfc.Focus()
        End If
    End Sub

    Private Sub grbClientes_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs)
        Try
            Dim tablaclientes As Data.DataTable
            'tablaclientes = objpro.ObtenDatos("SELECT crfc, crsocial, cemail, cusocfdi FROM CATCLIE WHERE cRFC = '" & Me.grbClientes.Value & "'", Motor).Tables(0)
            'txtRfc.Text = tablaclientes.Rows(0).Item("crfc").ToString
            'txtRazonSocial.Text = tablaclientes.Rows(0).Item("crSocial").ToString
            'txtEmail.Text = tablaclientes.Rows(0).Item("cemail").ToString
            'cfdiuso = tablaclientes.Rows(0).Item("cUsoCFDI").ToString()
            'tablaclientes = objpro.ObtenDatos("SELECT * FROM CATCLIE WHERE cRFC = '" & Me.grbClientes.Value & "'", Motor).Tables(0)
            txtRfc.Text = tablaclientes.Rows(0).Item("crfc").ToString
            txtRazonSocial.Text = tablaclientes.Rows(0).Item("crSocial").ToString
            txtEmail.Text = tablaclientes.Rows(0).Item("cemail").ToString
            cfdiuso = tablaclientes.Rows(0).Item("cUsoCFDI").ToString()
            regimenFisc = tablaclientes.Rows(0).Item("cRegFiscal").ToString()
            cDireccion = tablaclientes.Rows(0).Item("cDir").ToString.Trim & " " & tablaclientes.Rows(0).Item("cNumExt").ToString.Trim & " " & tablaclientes.Rows(0).Item("cNumInt").ToString.Trim & " " & tablaclientes.Rows(0).Item("cColonia").ToString.Trim
            cComDir = tablaclientes.Rows(0).Item("cMun").ToString.Trim & " " & tablaclientes.Rows(0).Item("cEstado").ToString.Trim & " " & tablaclientes.Rows(0).Item("cLocal").ToString.Trim & " " & tablaclientes.Rows(0).Item("cPais").ToString.Trim
            cCP = tablaclientes.Rows(0).Item("cCP").ToString()
            If cfdiuso <> "" Then
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "satuscfdi", "cfdiuso = '" & cfdiuso & "'", Motor)
            End If
            If regimenFisc <> "" Then
                RegimenF = tablaclientes.Rows(0).Item("cRegFisDes").ToString() 'objpro.Dato("regdes", "satregfis", "regimen = '" & CInt(regimenFisc) & "'", Motor)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnInicializa_Click(sender As Object, e As EventArgs) Handles btnInicializa.Click
        DatClientes()
        txtRfc.Focus()
    End Sub

    Private Sub DatClientes()
        'grbClientes.Text = ""
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        txtCP.Text = ""
        txtidrf.Text = ""
        txtdesrf.Text = ""
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDI)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub btnBuscarNota_Click(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        EsquemaConceptos()
        TotalesFacturar()
        consulta()
    End Sub

    Private Sub consulta()
        Try
            If Me.txtRfc.Text <> "" Then
                If txtRazonSocial.Text <> "" Then
                    If txtEmail.Text <> "" Then
                        If txtclvUsoCfdi.Text <> "" Then
                            If txtidrf.Text <> "" Then

                                If txtCP.Text <> "" Then
                                    If Trim(Me.txtRfc.Text) <> Trim(RFCPG) Then
                                        TIP = "FC"
                                    Else
                                        TIP = "VG"
                                    End If
                                    TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
                                    Me.txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                                    Me.txtSerie.Text = TSucursal.Rows(0).Item("Seserie").ToString
                                    txtLugarExpedicion = objpro.Dato("scp", "catsucu", "tid = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " ", Motor)
                                    CargaListaNotas()
                                Else
                                    MsgBox("Codigo Postal, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
                                End If

                            Else
                                MsgBox("Regimen Fiscal no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
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
                MsgBox("RFC, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
            End If

        Catch ex As Exception
            MsgBox("Error busqueda de registros ! " & ex.Message.ToString, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub CargaListaNotas()
        Me.ChlisFact.Items.Clear()
        Try
            For Each Archivo As String In My.Computer.FileSystem.GetFiles(Trim(RutaArchivos) & "\" & Me.grbPeriodo.Text & "\" & OBJFUN.EscojeMes(Me.grbMes.Text), FileIO.SearchOption.SearchAllSubDirectories, "*.txt")
                Dim NombreArchivo() As String = Split(Archivo, "\")
                Dim maximo As Integer = NombreArchivo.Count - 1
                Dim TipoArchivo() As String = Split(Trim(NombreArchivo(maximo)), "_")
                If Len(TipoArchivo(2)) > 8 Then
                    Me.ChlisFact.Items.Add(Trim(NombreArchivo(maximo)))
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub EsquemaConceptos()
        Me.UgrTicket.DataSource = Nothing
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.UgrTicket, "Select VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion " & _
                         " ,VdCant as Cantidad, '' as codsat, VdPreUni as PrecioUnitario, '' as Importe, VdPorIva as Impuesto " & _
                         " ,VdDesc1 as Descu, VdPreTot as Total, ''  as tipo_factura From VentaDet " & _
                         " Where VdFolio = 0", "VentaDet", MotorBoutique)
        Me.UgrTicket.DataBind()
    End Sub

    Private Sub TotalesFacturar()
        Subtotal.Value = 0
        Descuento.Value = 0
        Iva.Value = 0
        IEPS.Value = 0
        Total.Value = 0
    End Sub

    Private Sub UgrTicket_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UgrTicket.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With e.Layout.Override
            .FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains
            .AllowUpdate = Infragistics.Win.DefaultableBoolean.False
        End With
    End Sub

    Private Sub btnMonto_Click(sender As Object, e As EventArgs) Handles btnMonto.Click
        EsquemaConceptos()
        CalcularValores()
    End Sub

    Private Sub CalcularValores()
        ArrayArchivos.Clear()        
        Dim contador As Integer = 0

        TotalesFacturar()

        For i = 0 To Me.ChlisFact.Items.Count - 1
            If ChlisFact.GetItemChecked(i) = True Then
                Dim Linea As String = ""
                Dim RutaArchivo As String = Trim(RutaArchivos) & "\" & Me.grbPeriodo.Text & "\" & OBJFUN.EscojeMes(Me.grbMes.Text) & "\" & Trim(ChlisFact.Items.Item(i).ToString)
                ArrayArchivos.Add(Trim(RutaArchivo))
                Dim objReader As New StreamReader(Trim(RutaArchivo))
                Do
                    Linea = objReader.ReadLine
                    If Not Linea Is Nothing Then
                        contador += 1
                        Dim Cadenalinea() As String = Split(Linea, "|")
                        If Cadenalinea(0) <> "ClaveSucursal" Then
                            REM Cadenalinea(8)'Precio de Venta
                            REM Cadenalinea(9)'Importe
                            Importe.Value += (Cadenalinea(6) * Cadenalinea(7))
                            Subtotal.Value += Cadenalinea(10) REM Importe de articulo ya con descuento
                            IEPS.Value += Cadenalinea(12) REM IEPS
                            Iva.Value += Cadenalinea(15) REM IVA
                            Descuento.Value += Cadenalinea(9) REM linea de descuento
                            Total.Value += Cadenalinea(17) REM Total

                            'ClaveProducto = Cadenalinea(3) REM ClaveProducto
                            'DescripcionProducto = Cadenalinea(4) REM DescripcionProducto
                            'Cantidad = Cadenalinea(6) REM Cantidad
                            'Unidad = Cadenalinea(5) REM UnidadMedida
                            'IvaVenta = Cadenalinea(15) REM iva
                            'PUnitario = Cadenalinea(7) REM PrecioVenta
                            'Desc1 = Cadenalinea(9) REM Descuento
                            'Importe = Cadenalinea(10) REM DV_Subtotacondescuento()

                            Dim impu As Double = 0
                            Select Case Cadenalinea(19)
                                Case "Iva Tasa16"
                                    impu = Cadenalinea(15)
                                Case "Iva Tasa0"
                                    impu = 0
                                Case "IEPS"
                                    impu = Cadenalinea(12)
                            End Select


                            FormatoGrid(Cadenalinea(3), Cadenalinea(20), Cadenalinea(4), Cadenalinea(6), Cadenalinea(5), FormatCurrency(impu), FormatCurrency(Cadenalinea(8)), FormatCurrency(Cadenalinea(7)), FormatCurrency(Cadenalinea(9)), FormatCurrency(Cadenalinea(10)), Cadenalinea(19))
                            Application.DoEvents()

                            REM Carga Grid
                        End If
                    End If
                Loop Until Linea Is Nothing
                objReader.Close()                
                REM My.Computer.FileSystem.RenameFile(Trim(RutaArchivo), Trim(ChlisFact.Items.Item(i).ToString) & ".pdf")
            End If
        Next
    End Sub

    Public Function FormatoGrid(ByVal ClaveProducto As String, codsat As String, ByVal DescripcionProducto As String, ByVal Cantidad As String, ByVal Unidad As String, ByVal IvaVenta As Double, Impor As Double, ByVal PUnitario As Double, ByVal Desc1 As Double, ByVal Importe As Double, ByVal Nota As String)
        Me.UgrTicket.Rows.Band.AddNew()
        ColocarArticulo(Me.UgrTicket.Rows.Count - 1, ClaveProducto, codsat, DescripcionProducto, Cantidad, Unidad, IvaVenta, Impor, PUnitario, Desc1, Importe, Nota)
        Return ""
    End Function

    Private Sub ColocarArticulo(ByVal Renglon As Integer, ByVal Cp As String, cs As String, ByVal DP As String, ByVal Cd As String, ByVal Ud As String, ByVal IV As Double, impor As Double, ByVal PU As Double, ByVal DC As Double, ByVal ImP As Double, ByVal NVenta As String)
        With UgrTicket
            .Rows(Renglon).Cells("Clave").Value = Trim(Cp)
            .Rows(Renglon).Cells("codsat").Value = Trim(cs)
            .Rows(Renglon).Cells("Descripcion").Value = Trim(DP)
            .Rows(Renglon).Cells("Cantidad").Value = Trim(Cd)
            .Rows(Renglon).Cells("Unidad").Value = "H87"
            .Rows(Renglon).Cells("PrecioUnitario").Value = PU
            .Rows(Renglon).Cells("Importe").Value = Impor
            .Rows(Renglon).Cells("Impuesto").Value = IV
            .Rows(Renglon).Cells("Descu").Value = DC
            .Rows(Renglon).Cells("Total").Value = imp
            .Rows(Renglon).Cells("tipo_factura").Value = NVenta
        End With
    End Sub

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        idCliente = 0
        Formulario(frmClientesBoutique)
        If idCliente <> 0 Then
            txtRfc.Text = objpro.Dato("crfc", "catclie", "cid = " & idCliente & "", Motor)
            txtRazonSocial.Text = objpro.Dato("crsocial", "catclie", "cid = " & idCliente & "", Motor)
            txtEmail.Text = objpro.Dato("cemail", "catclie", "cid = " & idCliente & "", Motor)
            cCP = objpro.Dato("cCP", "catclie", "cid = " & idCliente & "", Motor)
            If cCP.Trim.Length = 4 Then
                cCP = "0" & cCP
            End If
            txtCP.Text = cCP
            cfdiuso = objpro.Dato("cusocfdi", "catclie", "cid = " & idCliente & "", Motor)
            txtclvUsoCfdi.Text = cfdiuso
            txtUsoCfdi.Text = objpro.Dato("cfdides", "satuscfdi", "cfdiuso = '" & cfdiuso & "'", Motor)
            txtidrf.Text = regimenFisc
            txtdesrf.Text = regimen
        End If
    End Sub

    Private Sub btnregimen_Click(sender As Object, e As EventArgs) Handles btnregimen.Click
        Formulario(frmRegimenFiscal)
        txtidrf.Text = regimenFisc
        txtdesrf.Text = regimen
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


    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", Motor) = True Then
            verNumCta(True)
        Else
            verNumCta(False)
        End If
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        TIP = "FC"
        Dim d As String = FechaFactura.Substring(0, 2)
        Dim m As String = FechaFactura.Substring(3, 2)
        Dim a As String = FechaFactura.Substring(6, 4)
        Dim FecVenta As String = a & m & d
        If UgrTicket.Rows.Count <> 0 Then
            Dim Directorios, RutFacClientes, CarpTienda, CarpSucursal, RutNotaCredito As String
            Dim TRazonSocial, TRfc, TRFiscal, TCp As String
            FolioFinal = objpro.Dato("SEFFIN", "SERIE", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seSerie = '" & txtSerie.Text & "' and seTipo = '" & TIP & "' and seEstat = 1  ", Motor)
            Timbrado = False
            upb.Value = 0
            upb.Maximum = 100

            TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", MotorBoutique).Tables(0)
            TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
            TRfc = TablaTienda.Rows(0).Item("trfc").ToString
            TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
            TCp = TablaTienda.Rows(0).Item("scp").ToString

            Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
            Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
            Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
            RutFacClientes = TablaTienda.Rows(0).Item("sForm1").ToString
            RutNotaCredito = TablaTienda.Rows(0).Item("sForm3").ToString
            CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
            CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
            Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
            Dim UserPAC As String = TablaTienda.Rows(0).Item("tUserPAC").ToString
            Dim PwdPAC As String = TablaTienda.Rows(0).Item("tPwdPAC").ToString
            Dim rutlogo As String = Trim(TablaTienda.Rows(0).Item("rutLogo").ToString)
            Dim descreg As String = Trim(TablaTienda.Rows(0).Item("sRFiscal").ToString)

            Dim FechaFactura As DateTime = Now
            Dim FechaHora As String = FechaFactura.ToString.Substring(10, 10)

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
            Fila("Edireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
            Fila("Edircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString
            'Fila("usocfdi") = "I01" ''Tabla.Rows(0).Item("tmun").ToString

            Dim RFacImpresas As String
            RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

            Select Case grbformaPago.Value
                Case "01"
                    FormPago = "EFECTIVO" 'CFDI.CFDI33.c_FormaPago.Efectivo_01
                Case "02"
                    FormPago = "CHEQUE"'CFDI.CFDI33.c_FormaPago.Cheque_02
                Case "03"
                    FormPago = "TRANSFERENCIA" 'CFDI.CFDI33.c_FormaPago.Transferencia_03
                Case "04"
                    FormPago = "TARJETA DE CRÉDITO" 'CFDI.CFDI33.c_FormaPago.TarjetaCredito_04
                Case "05"
                    FormPago = "MONEDERO ELECTRÓNICO" ' CFDI.CFDI33.c_FormaPago.MonederoElectronico_05
                Case "06"
                    FormPago = "DINERO ELECTRÓNICO" 'CFDI.CFDI33.c_FormaPago.DineroElectronico_06
                Case "08"
                    FormPago = "VALES DE DESPENSA" 'CFDI.CFDI33.c_FormaPago.ValesDespensa_08
                Case "12"
                    FormPago = "DACION EN PAGO" 'CFDI.CFDI33.c_FormaPago.DacionEnPago_12
                Case "13"
                    FormPago = "PAGO POR SUBROGACIÓN" 'CFDI.CFDI33.c_FormaPago.PagoPorSubrogacion_13
                Case "14"
                    FormPago = "PAGO POR CONSIGNACIÓN" 'CFDI.CFDI33.c_FormaPago.PagoPorConsignacion_14
                Case "15"
                    FormPago = "CONDONACIÓN" 'CFDI.CFDI33.c_FormaPago.Condonacion_15
                Case "17"
                    FormPago = "COMPENSACIÓN" 'CFDI.CFDI33.c_FormaPago.Compensacion_17
                Case "23"
                    FormPago = "NOVACIÓN" ' CFDI.CFDI33.c_FormaPago.Novacion_23
                Case "24"
                    FormPago = "CONFUSIÓN" ' CFDI.CFDI33.c_FormaPago.Confusion_24
                Case "25"
                    FormPago = "REMISION DEUDA" 'CFDI.CFDI33.c_FormaPago.RemisionDeuda_25
                Case "26"
                    FormPago = "PRESCRIPCIÓN CADUCIDAD" 'CFDI.CFDI33.c_FormaPago.PrescripcionCaducidad_26
                Case "27"
                    FormPago = "SATISFACCION ACREADOR" 'CFDI.CFDI33.c_FormaPago.SatisfaccionAcreedor_27
                Case "28"
                    FormPago = "TARJETA DE DÉBITO" 'CFDI.CFDI33.c_FormaPago.TarjetaDebito_28
                Case "29"
                    FormPago = "TARJETA DE SERVICIO" 'CFDI.CFDI33.c_FormaPago.TarjetaServicio_29
                Case "30"
                    FormPago = "NA" 'CFDI.CFDI33.c_FormaPago.NA_98
                Case "99"
                    FormPago = "OTROS" 'CFDI.CFDI33.c_FormaPago.Otros_99
            End Select

            Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

            Dim TipoCambio As Int16 = 1

            fp = FormPago '"99 - Otros"
            upb.Value += 10

            'EXPORTACION: 01 - NO APLICA
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
                Case 621
                    TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IncorporacionFiscal
                    DesRegimen = "Incorporacion Fiscal"

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

            '' // Periodicidad 01 Diario| 02 Semanal | 03 Quincenal | 04 Mensual | 05 Bimensual
            ''// Meses 01-12 Enero a Diciembre | 13-18 Enero-Febrero a Noviembre-Diciembre  // Año
            'EasyOne_cfdi33_SetInformacionGlobal40(p, "02", "12", "2022")

            'Llenamos la sección del Emisor
            'EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, "x")
            EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TpRegimen)

            'Llenamos la sección del Receptor
            'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
            'EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
            'EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616
            EasyOne_cfdi33_SetReceptor(p, txtRfc.Text, txtRazonSocial.Text, "", "", txtclvUsoCfdi.Text) 'estos paramentros son si o si para factura global
            EasyOne_cfdi33_SetReceptor40(p, txtCP.Text, txtidrf.Text)
            'EasyOne_cfdi33_SetReceptor40(p, cCP, "x") 'regimen fiscal para global 616

            Dim NumPiezas As Integer

            Fila("Rcp") = txtCP.Text 'TCp
            Fila("Rrfc") = txtRfc.Text '"XAXX010101000"
            Fila("Rrazonsoc") = txtRazonSocial.Text '"PUBLICO EN GENERAL"
            Fila("Rdireccion") = cDireccion '""
            Fila("Rregimen") = regimenFisc & " - " & regimen ' "616-Sin Obligaciones Fiscales"
            Fila("rusocfdi") = "Uso CFDI: " & txtclvUsoCfdi.Text ' & " - " & txtUsoCfdi.Text '"S01"
            Fila("moneda") = "MXN"
            Fila("rcomdir") = cComDir
            Fila("nocuenta") = txtNumCuenta.Text

            REM Datos clientes ***************************************
            'Dim Existerfc As String
            'Existerfc = objpro.DatoString("crfc", "catclie", "crfc = '" & txtRfc.Text & "' ", Motor)
            'If Existerfc = "" Then
            '    objpro.Indicacion("Insert Into catclie (crsocial,crfc,cemail, cusocfdi) " & _
            '                         "values ('" & Me.txtRazonSocial.Text & "', '" & Me.txtRfc.Text & "','" & Me.txtEmail.Text & "', '" & txtclvUsoCfdi.Text & "') ", Motor)
            'End If
            REM Datos clientes ***************************************

            Dim NotaSel(), Cantidad, Ticket, Fecha, cadenaFecha, veterm As String
            Dim i As Integer
            For i = 0 To Me.ChlisFact.Items.Count - 1
                If ChlisFact.GetItemChecked(i) = True Then
                    NotaSel = Split(ChlisFact.Items.Item(i).ToString, ".")
                    cadenaFecha = FechaFactura.ToString.Substring(0, 10)
                    'Cadena &= cadenaFecha & "-" &  & "- $"
                    Cadena = "Suc: " & Trim(CarpSucursal) & " Fecha Venta: " & cadenaFecha & "- " & NotaSel(0) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))
                    Dim j As Integer : Cantidad = 0
                    REM convertir txt a pdf *****************************************************
                    Dim RArchivo As String = Trim(RutaArchivos) & "\" & Me.grbPeriodo.Text & "\" & objfun.EscojeMes(Me.grbMes.Text) & "\" & Trim(ChlisFact.Items.Item(i).ToString)
                    My.Computer.FileSystem.RenameFile(Trim(RArchivo), Trim(ChlisFact.Items.Item(i).ToString) & ".pdf")
                    REM convertir txt a pdf *****************************************************
                End If
            Next

            Fila("sucursal") = Cadena
            Dim sb As Double = 0
            Dim des As Double = 0
            Dim iv As Double = 0
            Dim ieps As Double = 0
            Dim iiv As Double = 0
            Dim PuCa As Double = 0
            Dim Imp As Double = 0
            Dim bas As Double = 0
            Dim des1 As Double = 0
            Dim Descu As Double = 0
            'c.TruncarDecimales = True
            'c.TruncarDecimalesLongitud = 2
            Dim Importe As Double = 0
            Dim Impuesto As String = ""
            Dim TasaOCuota As String = ""
            Dim TipoFactor As String = ""
            Dim Impu As String = ""
            Dim ToC As String = ""
            Dim TFac As String = ""
            Dim ipt As String = ""
            Dim flag As Integer = 1

            Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
            Tabla2.Columns.Add("Cantidad")
            Tabla2.Columns.Add("Unidad")
            Tabla2.Columns.Add("UniSAT")
            Tabla2.Columns.Add("Descrip")
            Tabla2.Columns.Add("Punitario")
            Tabla2.Columns.Add("Importe")
            Dim Fila2 As DataRow

            For i = 0 To Me.UgrTicket.Rows.Count - 1
                Dim TOTALPROD As Double = Format(Me.UgrTicket.Rows(i).Cells("Total").Value, "###,###.00")
                If TOTALPROD <> 0 Then
                    Dim pun As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                    Dim producto As String = Trim(Me.UgrTicket.Rows(i).Cells("DESCRIPCION").Value)
                    Dim Puni As String
                    Puni = pun.ToString.Replace(",", "")
                    While producto.Contains("  ")
                        producto = producto.Replace("  ", " ")
                    End While
                    Dim Clave As String = Trim(Me.UgrTicket.Rows(i).Cells("Clave").Value)
                    Dim codsat As String
                    If Me.UgrTicket.Rows(i).Cells("codsat").Value = "" Then
                        codsat = "01010101"
                    Else
                        codsat = Me.UgrTicket.Rows(i).Cells("codsat").Value
                    End If
                    sb += Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                    Dim impt As Double = Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                    ipt = impt.ToString.Replace(",", "")
                    NumPiezas += Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                    Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                    Fila2("Cantidad") = Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                    Fila2("Unidad") = "PZA"
                    Fila2("UniSAT") = "H87"
                    Fila2("Descrip") = producto
                    Fila2("Punitario") = Format(pun, "###,###.00")
                    Fila2("Importe") = Format(impt, "###,###.00")
                    DS.Tables(1).Rows.Add(Fila2)
                    If Me.UgrTicket.Rows(i).Cells("Descu").Value <> 0 Then
                        If Format(CDbl(Me.UgrTicket.Rows(i).Cells("Importe").Value), "###,###.00") - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value), "###,###.00") <> 0 Then
                            Descu = Me.UgrTicket.Rows(i).Cells("Descu").Value
                            Dim dcto As String
                            dcto = Descu.ToString.Replace(",", "")
                            des += Me.UgrTicket.Rows(i).Cells("Descu").Value
                            EasyOne_cfdi33_AddConcepto(p, codsat, Clave, Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "H87", "PZA", producto, Puni, ipt, dcto)
                        Else
                            Descu = Me.UgrTicket.Rows(i).Cells("Descu").Value - 0.1
                            Dim dcto As String
                            dcto = Descu.ToString.Replace(",", "")
                            des += Me.UgrTicket.Rows(i).Cells("Descu").Value - 0.1
                            EasyOne_cfdi33_AddConcepto(p, codsat, Clave, Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "H87", "PZA", producto, Puni, ipt, dcto)
                        End If
                    Else
                        EasyOne_cfdi33_AddConcepto(p, codsat, Clave, Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "H87", "PZA", producto, Puni, ipt, "")
                    End If
                    'With c.Conceptos(i).Impuestos.Traslados(0)
                    'Se debe de agregar si el concepto es Objeto de impuesto
                    EasyOne_cfdi33_SetConcepto40(p, "02")
                    Select Case Me.UgrTicket.Rows(i).Cells("tipo_factura").Value.ToString.Trim
                        Case "Iva Tasa16"
                            Imp = 0
                            If (Format(CDbl(Me.UgrTicket.Rows(i).Cells("Importe").Value), "###,###.00") - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value), "###,###.00")) <> 0 Then
                                bas = (Me.UgrTicket.Rows(i).Cells("Importe").Value - Me.UgrTicket.Rows(i).Cells("Descu").Value)
                                Imp = Format((bas * 0.16), "###,###.00")
                                iv += Imp
                                EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))
                            Else
                                bas = Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value) - 0.1, "###,###.00")
                                Imp = Format((bas * 0.16), "###,###.00")
                                iv += Imp
                                EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))
                            End If
                        Case "Iva Tasa0"
                            Imp = 0
                            If (Format(CDbl(Me.UgrTicket.Rows(i).Cells("Importe").Value), "###,###.00") - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value), "###,###.00")) <> 0 Then
                                bas = Format((Me.UgrTicket.Rows(i).Cells("Importe").Value - CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value)), "###,###.00")
                                Imp = Format((bas * 0), "###,###.00")
                                flag = 0
                                'iv += Imp
                                Dim im As String = Imp.ToString.Replace(".00", "0.00")
                                EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.000000", im)
                            Else
                                bas = Me.UgrTicket.Rows(i).Cells("Importe").Value - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value) - 0.1, "###,###.00")
                                Imp = Format((bas * 0), "###,###.00")
                                Dim im As String = Imp.ToString.Replace(".00", "0.00")
                                flag = 0
                                'iv += Imp
                                Impu = "002"
                                ToC = "0.000000"
                                TFac = "Tasa"
                                EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.000000", im)
                            End If
                        Case "IEPS"
                            Imp = 0
                            If (Format(CDbl(Me.UgrTicket.Rows(i).Cells("Importe").Value), "###,###.00") - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value), "###,###.00")) <> 0 Then
                                bas = Format(Me.UgrTicket.Rows(i).Cells("Importe").Value - CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value), "###,###.00")
                                Imp = Format((bas * 0.08), "###,###.00")
                                ieps += Imp
                                If Imp.ToString.Substring(0, 2) = "0." Then
                                    Dim au As String = Imp.ToString
                                    EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "003", "Tasa", "0.080000", au)
                                Else
                                    EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "003", "Tasa", "0.080000", Format(Imp, "######.00"))
                                End If

                            Else
                                bas = Me.UgrTicket.Rows(i).Cells("Importe").Value - Format(CDbl(Me.UgrTicket.Rows(i).Cells("Descu").Value) - 0.1, "###,###.00")
                                Imp = Format((bas * 0.08), "###,###.00")
                                ieps += Imp
                                If Imp.ToString.Substring(0, 2) = "0." Then
                                    Dim au As String = Imp.ToString
                                    EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "003", "Tasa", "0.080000", au)
                                Else
                                    EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "003", "Tasa", "0.080000", Format(Imp, "######.00"))
                                End If
                            End If
                    End Select
                End If
                'End With
            Next 'Cierra el recorrido del grid
            Dim t_traslado = Format((iv + ieps), "######.00")
            If t_traslado = 0 Then
                EasyOne_cfdi33_SetComprobanteImpuestos(p, "", "0.00")
            Else
                EasyOne_cfdi33_SetComprobanteImpuestos(p, "", t_traslado)
            End If

            'Agregamos los Impuestos
            'If Me.Iva.Value <> 0 Then
            If iv <> 0 Then
                EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.160000", Format(iv, "######.00"))
            Else
                EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.160000", "0.00")
            End If

            If flag = 0 Then
                EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.000000", "0.00")
            End If

            If Me.ieps.Value <> 0 Then
                If ieps.ToString.Substring(0, 2) = "0." Then
                    Dim auxi As String = ieps.ToString
                    EasyOne_cfdi33_AddTrasladosTraslado(p, "003", "Tasa", "0.080000", auxi)
                Else
                    EasyOne_cfdi33_AddTrasladosTraslado(p, "003", "Tasa", "0.080000", Format(ieps, "######.00"))
                End If
            End If

            NoCuenta = ""
            Dim Total As Double = 0
            Dim Subt As Double = 0

            'Subt = Format(Me.Subtotal.ToString, "###,###.00")
            Subt = Format(sb, "###,###.00")
            Total = Format(((Subt - des) + iv + ieps), "###,###.00")

            'Cadena = txtFechaVenta.Value.ToString.Substring(0, 10) & " Nota " & txtFactura & " - Importe - " & " $" & Str(FormatCurrency(Total))
            Dim ValorTicket As String = Cadena

            Fila("Fsubtotal") = Format(Subt, "###,###.00")
            Fila("Fdescuento") = Format(des, "###,###.00")
            Fila("Fiva") = Format(iv, "###,###.00")
            Fila("Ftotal") = Format(Total, "###,###.00")

            Dim cantletra As String
            cantletra = Letras(Total)
            Fila("cantidadl") = cantletra

            NoCuenta = txtNumCuenta.Text

            Dim metpg As String
            Dim tipcom As String
            'Dim formpg As String
            metpg = "PUE" REM COLOCAR POR DEFAULT PUE PERO SE DEBE ACTIVAR LA OPCION PARA BOUTIQUES
            tipcom = "I"
            'formpg = "99"

            Dim Descue As String = ""
            Dim desc2 As String
            Descue = Format(des, "###,###.00")
            desc2 = Descue.Replace(",", "")

            Dim fecha1, fecha2, fecha3, fechareal, FeRegistro As String
            fecha1 = Now.Date.ToString.Substring(0, 2)
            fecha2 = Now.Date.ToString.Substring(3, 2)
            fecha3 = Now.Date.ToString.Substring(6, 4)
            fechareal = fecha1 & "/" & fecha2 & "/" & fecha3
            FeRegistro = Now.Date.ToString.Substring(6, 4) & fecha2 & fecha1

            Dim freal, hr As String
            ''hr = Now.ToLocalTime.ToString.Substring(11, 8)
            ''freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

            '''Llenamos la sección del comprobante
            ''If desc2 = ".00" Or desc2 = "0" Then
            ''    EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Subt.ToString, "", "MXN", TipoCambio, Total.ToString, tipcom, metpg, TCp, "")
            ''Else
            ''    EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Subt.ToString, desc2, "MXN", TipoCambio, Total.ToString, tipcom, metpg, TCp, "")
            ''End If

            Fila("metpago") = metpg
            If tipcom = "I" Then
                Fila("tipocomp") = "I - Ingreso"
            Else
                Fila("tipocomp") = "E - Egreso"
            End If
            Fila("formpago") = fp

            ''Enviamos el CFDI al metodo de PRUEBAS
            ''En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
            ''EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL")
            'EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

            Application.DoEvents()
            upb.Value += 30  'Suma 40

            'En la siguiente línea se deben colocar los nombres de los archivos de certificado de sellos del Emisor.
            'Para fines de esta prueba se usarán los certificados de sellos del Emisor AAA010101AAA.
            'Favor de colocar estos archivos (.CER y .KEY) en la misma carpeta donde se encuentra la aplicación.
            'Timbra XML, que se encuentra en variable P
            Dim x As Integer = 99
            If va = 0 Then
                hr = Now.ToLocalTime.ToString.Substring(11, 8)

                If hra <> "" Then
                    MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                    hr = hra
                End If

                freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

                'Llenamos la sección del comprobante
                If desc2 = ".00" Or desc2 = "0" Then
                    EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Subt.ToString, "", "MXN", TipoCambio, Total.ToString, tipcom, metpg, TCp, "")
                Else
                    EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Subt.ToString, desc2, "MXN", TipoCambio, Total.ToString, tipcom, metpg, TCp, "")
                End If

                'Enviamos el CFDI al metodo de PRUEBAS
                'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL")
                EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

                x = EasyOne_FirmaComprobante(p, CerFactura, Key, Pwd)
                If x = 0 Then
                    va += 1
                    hra = hr
                Else
                    Dim resultadoXML2 As String = [String].Empty
                    resultadoXML2 = EasyOne_LeeResultado(p, 11)
                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                    MsgBox("No se timbro archivo, contactar DEPTO. DE SISTEMAS, XML MAL" & " " & " No. de Veces de Timbrado " & va, MsgBoxStyle.Critical).ToString()
                    btnFacturar.Visible = True
                    hra = hr
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

                'Dim nav As XPathNavigator
                Dim documento As XmlDocument = New XmlDocument()
                Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
                documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"))
                VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                upb.Value += 10 'Suma 50
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

                    RutaLogo = rutlogo
                    RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml.png")

                    '''''''''''''''''''''''''''''''''''''''''''''''''''
                    ExisteClave = CStr(objpro.DatoString("cid", "catclie", "crfc = '" & Me.txtRfc.Text & "' ", Motor))
                    Dim Tipo As String = "F"
                    Dim trn1 = Trim(Str(grbTienda.Value)) & Trim(Str(grbSucursal.Value))
                    Dim trn = Trim(trn1)

                    Dim Subtotal = FormatCurrency(Me.Subtotal.Value)
                    Dim Iva = FormatCurrency(Me.Iva.Value)
                    Dim Total2 = FormatCurrency(Me.Total.Value)
                    Dim PorIva = 16.0
                    Dim Descuento = FormatCurrency(Me.Descuento.Value)
                    PorIva = 16.0

                    objpro.Indicacion("Update Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                    Application.DoEvents()

                    objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha, " &
                                      "feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                      "FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                      "FeCanlet,FeAbonos,FeEstatu,FeReg,feTip,feuuid,fformpag) " &
                                      "values (" & CInt(trn) & ", " & grbTienda.Value & ", " & trn & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "', " &
                                      " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ", '" & Subtotal & "', '" & CDbl(PorIva) & "','" & CDbl(Me.Iva.Value) & "', " &
                                      " '" & Descuento & "','" & Descuento & "', '" & Descuento & "', '" & Descuento & "', '" & Me.Total.Value & "', " &
                                      " '" & cantletra & "', '" & Total2 & "'," & Estatus & "," & FeRegistro & ",'" & Tipo & "', '" & uuid & "', '" & grbformaPago.Value & "' )", Motor)
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
                    smtp.EnableSsl = SeguridadEmail
                    upb.Value += 10 'Suma 100
                    Application.DoEvents()
                    smtp.Send(correo)
                    MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                    BorrarRegistro()
                    DatClientes()
                    CargaListaNotas()
                    btnFacturar.Visible = True
                    va = 0
                Else
                    MsgBox("Archivo XML no es una factura version 4.0")
                End If
            Else
                Dim resultadoXML2 As String = [String].Empty
                resultadoXML2 = EasyOne_LeeResultado(p, 11)
                Console.WriteLine(resultadoXML2)
                'MsgBox("ERROR AL ENVIAR XML, CONSULTE ARCHIVO SOAP...", MsgBoxStyle.Information, "Facturación")
                MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación")
            End If 'Cierra creacion del XML
        Else
            btnFacturar.Visible = True
        End If 'Cierra valida si hay registros en el grid
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub BorrarRegistro()
        UgrTicket.DataSource = Nothing
        limpiar()
        Importe.Value = "0.0"
        Subtotal.Value = "0.0"
        Descuento.Value = "0.0"
        Iva.Value = "0.0"
        ieps.Value = 0
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
End Class