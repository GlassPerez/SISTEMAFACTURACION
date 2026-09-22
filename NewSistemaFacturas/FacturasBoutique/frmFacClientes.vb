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

Public Class frmFacClientes

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket, DatGrid, facturado, yafac As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim FolioFinal
    Dim Cadena As String
    Dim ExisteClave As String
    Dim PorIva As Double
    Dim FechaFactura As String
    Dim va As Integer
    Private clickCount As Integer = 0
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
    Dim fp As String
    Dim NoCuenta As String
    Dim metodoPago As String
#End Region

    Private Sub frmFacClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        verNumCta(False)
        btnFacturar.Enabled = True
        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        Me.txtFechaVenta.Value = dia + "/" + mes + "/" + amo
        FechaFactura = Date.Now.ToString.Substring(0, 10) 'agregado

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where Tid = " & Tienda & "", 0, 1, Motor)
        Me.grbTienda.Value = Tienda
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Value = Sucursal
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from satmpag ", 0, 1, Motor)
        'Me.grbPagos.Value =
        Application.DoEvents()
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from satfpag ", 0, 1, Motor)
        Application.DoEvents()
        Clientes()
        InicializaGrid()
        va = 0
    End Sub

    Private Sub InicializaGrid()
        UgrTicket.DataSource = Nothing
        objpro.LLenaGrid(UgrTicket, "select '' as codsat, '' as clavesat, vdClaPro as ClaveInterno, vdDescri as Descripcion, " &
                                  " vdCant as Cantidad, '' as Unidad, vdPreUni as PrecioUnitario, vdDesc1 as Descu, " &
                                   " vdIva as Iva, vdPreTot as Importe from ventadet " &
                                   " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " &
                                   " vdterm = 0 ", "ventadet", Motor)
        UgrTicket.DataBind()
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDI)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Private Sub btnRF_Click(sender As Object, e As EventArgs) Handles btnRF.Click
        FormularioRF(frmRegimenFiscal)
        TextRegFiscal.Text = regimen
        Textrf.Text = regimenFisc
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Public Function FormularioRF(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Function Clientes() As Boolean
        objpro.CargaCombos(grbClientes, "Select distinct crfc,CRSOCIAL from CATCLIE", 0, 1, Motor)
        Return False
    End Function

    Private Sub grbClientes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbClientes.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            Me.txtRfc.Focus()
        End If
    End Sub

    Private Sub grbClientes_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs) Handles grbClientes.RowSelected
        Try
            Dim noext As String
            Dim tablaclientes As Data.DataTable
            REM tablaclientes = objpro.ObtenDatos("SELECT * FROM CATCLIE WHERE cRFC = '" & Me.grbClientes.Value & "'", Motor).Tables(0)
            tablaclientes = objpro.ObtenDatos("SELECT * FROM CATCLIE WHERE cRFC = '" & Me.grbClientes.Value & "'", Motor).Tables(0)
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
                Textrf.Text = regimenFisc
                TextRegFiscal.Text = tablaclientes.Rows(0).Item("cRegFisDes").ToString() 'objpro.Dato("regdes", "satregfis", "regimen = '" & CInt(regimenFisc) & "'", Motor)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnBuscarNota_Click(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        Dim chkglobal As String = ""
        ''consulto si la factura global ya se hio contra la fecha del tickt que quieren facturar
        chkglobal = objpro.Dato("veEstatu", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = 1 and veEstado = 1 and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)
        If chkglobal = 0 Then
            If Me.txtRfc.Text <> "" Then
                If txtRazonSocial.Text <> "" Then
                    If txtEmail.Text <> "" Then
                        If txtclvUsoCfdi.Text <> "" Then
                            If Textrf.Text <> "" Then
                                If cDireccion <> "" Then
                                    If cComDir <> "" Then
                                        If cCP <> "" Then
                                            Try
                                                TIP = "FC"
                                                Dim EstadoVenta As String
                                                Terminal = Mid(Me.txtTransaccion.Text, 1, 3)
                                                Dim trn = Len(Me.txtTransaccion.Text)
                                                Transaccion = Mid(Me.txtTransaccion.Text, 5, trn)

                                                EstadoVenta = objpro.Dato("veEstatu", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique)

                                                If EstadoVenta = 0 Then
                                                    Application.DoEvents()
                                                    Tabla = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", Motor).Tables(0)
                                                    Application.DoEvents()


                                                    CargaGridInicio()

                                                    Dim h As Integer
                                                    Dim Import, Subtot, Iv, Tot, PIVa, Desc1, Desc2, Desc As Double
                                                    For h = 0 To Me.UgrTicket.Rows.Count - 1
                                                        Import += (Me.UgrTicket.Rows(h).Cells("PrecioUnitario").Value() * Me.UgrTicket.Rows(h).Cells("Cantidad").Value())
                                                        Desc1 += Me.UgrTicket.Rows(h).Cells("Descu").Value()
                                                        Iv += Me.UgrTicket.Rows(h).Cells("Iva").Value()
                                                    Next
                                                    Tot = Import + Iv
                                                    Desc = Desc1 + Desc2
                                                    Me.Importe.Value = Import
                                                    Me.Descuento.Value = Desc
                                                    Me.Iva.Value = Iv
                                                    Me.Subtotal.Value = (Import - Desc)
                                                    Me.Total.Value = (Subtotal.Value + Iva.Value)
                                                Else
                                                    MsgBox("La nota ya fue facturada ..", MsgBoxStyle.Information, "Facturación .. ")
                                                End If
                                            Catch ex As Exception
                                                MsgBox(ex.Message)
                                            End Try
                                        Else
                                            MsgBox("Codigo Postal, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
                                        End If
                                    Else
                                        MsgBox("Dirección no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
                                    End If
                                Else
                                    MsgBox("Direccion no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
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
        Else
            MsgBox("Esta Nota ya no se puede facturar porque la Factura Global ya fue realizada, consulte con el area de Sistemas", MsgBoxStyle.Information, "Facturación")
        End If

    End Sub


    Private Sub CargaGridInicio()

        Application.DoEvents()
        DatGrid = objpro.ObtenDatos("SELECT VdCodSat as CodSat, vdClvUni as ClaveSat, VdClaPro as ClaveInterno, VdDescri as Descripcion, " &
                                    " VdCant as Cantidad, vdUnidad as Unidad, vdPorIva as PorIva, VdIva as Iva, " &
                                    " VdPreUni as PrecioUnitario, vdDesc1 as Descu, VdPreTot as Importe, vdFEcha FROM VentaDet Where vdfolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and  vdTerm = " & Terminal & " and VdIdFolv = " & Transaccion & " and day(vdFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique).Tables(0)

        If DatGrid.Rows.Count <> 0 Then
            For Each r In DatGrid.Rows
                Application.DoEvents()
                Dim pu1 As Double = (CDbl(r("PrecioUnitario").ToString))
                Dim PU As Double = Format(pu1, "###,###.00")
                Dim des As Double = CDbl(r("Descu").ToString)
                Dim descu As Double = Format(des, "###,###.00")
                Dim iva2 As Double = (((PU * CDbl(r("cantidad").ToString)) - descu) * 0.16)
                Dim iva1 As Double = Format(iva2, "###,###.00")
                Dim impt As Double = ((PU * CDbl(r("cantidad").ToString)) - descu)

                Dim codsat As String
                If r("codsat").ToString = "" Or r("codsat").ToString = "1010101" Then
                    codsat = "01010101"
                Else
                    codsat = r("codsat").ToString
                End If
                Dim Borrar As String = Trim(r("Descripcion").ToString)
                While Borrar.Contains("  ")
                    Borrar = Borrar.Replace("  ", " ")
                End While

                FormatoGrid(codsat, r("ClaveSat").ToString, r("ClaveInterno").ToString, Borrar,
                            r("Cantidad").ToString, "PZA", PU, iva1,
                            descu, impt)
                Application.DoEvents()
            Next

            TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
            Me.txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
            Me.txtSerie.Text = TSucursal.Rows(0).Item("Seserie").ToString
            btnFacturar.Enabled = True
        Else
            MsgBox("No se encontró información.. ", MsgBoxStyle.Information, "Facturación.. ")
            btnFacturar.Enabled = False
        End If
    End Sub

    Private Sub UgrTicket_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UgrTicket.InitializeLayout
        With Me.UgrTicket
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 80
            .DisplayLayout.Bands(0).Columns(2).Width = 100
            .DisplayLayout.Bands(0).Columns(3).Width = 300
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Width = 60
            .DisplayLayout.Bands(0).Columns(6).Width = 90
            .DisplayLayout.Bands(0).Columns(7).Width = 70
            .DisplayLayout.Bands(0).Columns(8).Width = 100
            .DisplayLayout.Bands(0).Columns(9).Width = 70
        End With
    End Sub

    Private Sub FormatoGrid(codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String,
                            Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double,
                            Descu As Double, Importe As Double)
        UgrTicket.Rows.Band.AddNew()
        ColocarArticulo(UgrTicket.Rows.Count - 1, codSat, cvlUni, ClaveInterno, Descripcion, Cantidad, Unidad, PrecioUnitario, Iva, Descu, Importe)
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String,
                            Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double,
                            Descu As Double, Importe As Double)
        With UgrTicket
            .Rows(Renglon).Cells("CodSat").Value = codSat
            .Rows(Renglon).Cells("ClaveSat").Value = cvlUni
            .Rows(Renglon).Cells("ClaveInterno").Value = ClaveInterno
            .Rows(Renglon).Cells("Descripcion").Value = Descripcion
            .Rows(Renglon).Cells("Cantidad").Value = Cantidad
            .Rows(Renglon).Cells("Unidad").Value = Unidad
            .Rows(Renglon).Cells("PrecioUnitario").Value = PrecioUnitario
            .Rows(Renglon).Cells("Descu").Value = Descu
            .Rows(Renglon).Cells("Iva").Value = Iva
            .Rows(Renglon).Cells("Importe").Value = Importe
        End With
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        If grbPagos.Text <> "" Then
            TIP = "FC"
            If Me.txtEmail.Text <> "" And Me.txtRazonSocial.Text <> "" And Me.txtRfc.Text <> "" And txtclvUsoCfdi.Text <> "" Then
                FolioFinal = objpro.Dato("SEFFIN", "SERIE", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seSerie = '" & txtSerie.Text & "' and seTipo = '" & TIP & "' and seEstat = 1  ", Motor)
                upb.Value = 0
                upb.Maximum = 100

                If grbformaPago.Text.Trim <> "" Then
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
                            Dim rutlogo As String = Trim(Tabla.Rows(0).Item("rutLogo").ToString)
                            Dim descreg As String = Trim(Tabla.Rows(0).Item("sRFiscal").ToString)
                            Dim invoice As String = Trim(Tabla.Rows(0).Item("tuserpac").ToString)

                            Dim FechaFac As DateTime = Now
                            Dim FechaHora As String = FechaFac.ToString.Substring(10, 10)

                            DirEmail = Tabla.Rows(0).Item("EMAILDIR").ToString
                            PwdEmail = Tabla.Rows(0).Item("EMAILPWD").ToString
                            Pop = Tabla.Rows(0).Item("EMAILPOP").ToString
                            PuertoPop = Tabla.Rows(0).Item("EMAILPPOP").ToString
                            SmtpHost = Tabla.Rows(0).Item("EMAILSMTP").ToString
                            PuertoSmtp = Tabla.Rows(0).Item("EMAILPSMTP").ToString
                            DescEmail = Tabla.Rows(0).Item("EMAILDESC").ToString
                            AsuntoEmail = Tabla.Rows(0).Item("EMAILASU").ToString
                            MsjEmail = Tabla.Rows(0).Item("EMAILMSG").ToString
                            If Tabla.Rows(0).Item("EMAILP").ToString = Nothing Then
                                SeguridadEmail = False
                            ElseIf Tabla.Rows(0).Item("EMAILP").ToString = 0 Then
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
                            Fila("Edireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
                            Fila("Edircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString
                            'Fila("usocfdi") = "I01" ''Tabla.Rows(0).Item("tmun").ToString

                            Dim RFacImpresas As String
                            RFacImpresas = Directorios & "\" & txtFechaVenta.Value.ToString.Substring(6, 4) & "\" & txtFechaVenta.Value.ToString.Substring(3, 2) '& FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

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
                            'Dim CondicionesDePago As String = "CONTADO"

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

                            Select Case txtclvUsoCfdi.Text 'clientes
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
                            EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TRFiscal)

                            'Llenamos la sección del Receptor
                            'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                            EasyOne_cfdi33_SetReceptor(p, txtRfc.Text, txtRazonSocial.Text, "", "", txtclvUsoCfdi.Text) 'estos paramentros son si o si para factura global
                            EasyOne_cfdi33_SetReceptor40(p, cCP, Textrf.Text) 'regimen fiscal para global 616

                            Dim NumPiezas As Integer
                            Cadena = "Suc: " & Trim(CarpSucursal) & " Fecha Venta: " & txtFechaVenta.Value.ToString.Substring(0, 10) & "- " & Trim(Me.txtTransaccion.Text) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))

                            Fila("sucursal") = Cadena
                            Fila("Rcp") = cCP 'TCp
                            Fila("Rrfc") = txtRfc.Text '"XAXX010101000"
                            Fila("Rrazonsoc") = txtRazonSocial.Text '"PUBLICO EN GENERAL"
                            Fila("Rdireccion") = cDireccion '""
                            Fila("Rregimen") = Textrf.Text & " - " & TextRegFiscal.Text ' "616-Sin Obligaciones Fiscales"
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

                            Dim Importe As Double = 0
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

                            For i = 0 To Me.UgrTicket.Rows.Count - 1
                                Dim pun As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                                Dim pro As String = Trim(Me.UgrTicket.Rows(i).Cells("DESCRIPCION").Value)
                                Dim producto As String = pro.Replace(vbCrLf, "")
                                NumPiezas += Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                                Dim impt As Double = Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00")
                                ipt = impt.ToString.Replace(",", "")
                                sb += Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00")
                                'sb += Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00")
                                Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                                Fila2("Cantidad") = Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                Fila2("Unidad") = "PZA"
                                Fila2("UniSAT") = Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value) '"H87"
                                Fila2("Descrip") = producto
                                Fila2("Punitario") = Format(pun, "###,###.00")
                                Fila2("Importe") = Format(impt, "###,###.00")
                                DS.Tables(1).Rows.Add(Fila2)


                                If UgrTicket.Rows(i).Cells("Descu").Value <> 0 Then
                                    If (Me.UgrTicket.Rows(i).Cells("Cantidad").Value * Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")) - Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") <> 0 Then
                                        Dim Descu As Double = Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                        Dim dcto As String
                                        dcto = Descu.ToString.Replace(",", "")
                                        'EasyOne_cfdi33_AddConcepto(p, "01010101", "", Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "ACT", "", "Venta", Puni, ipt, dcto) 'PARA PRUEBAS
                                        EasyOne_cfdi33_AddConcepto(p, Trim(Me.UgrTicket.Rows(i).Cells("CodSat").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), "PZA", producto, Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                                        des += Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                    Else
                                        Dim Descu As Double = Format(Me.UgrTicket.Rows(i).Cells("Descu").Value - 0.1, "###,###.00")
                                        Dim dcto As String
                                        dcto = Descu.ToString.Replace(",", "")
                                        'EasyOne_cfdi33_AddConcepto(p, "01010101", "", Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "ACT", "", "Venta", Puni, ipt, dcto) 'PARA PRUEBAS
                                        EasyOne_cfdi33_AddConcepto(p, Trim(Me.UgrTicket.Rows(i).Cells("CodSat").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), "PZA", producto, Format((pun), "######.00"), Format((impt), "######.00"), Format((Descu), "######.00"))
                                        des += (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                                    End If
                                Else
                                    'EasyOne_cfdi33_AddConcepto(p, "01010101", "", Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "ACT", "", "Venta", Puni, ipt, "") ' PARA PRUEBAS
                                    EasyOne_cfdi33_AddConcepto(p, Trim(Me.UgrTicket.Rows(i).Cells("CodSat").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), "PZA", producto, Format((pun), "######.00"), Format((impt), "######.00"), "")
                                End If
                                'Se debe de agregar si el concepto es Objeto de impuesto
                                EasyOne_cfdi33_SetConcepto40(p, "02")
                                'With c.Conceptos(i).Impuestos.Traslados(0)
                                If ((Me.UgrTicket.Rows(i).Cells("Cantidad").Value * Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")) - Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")) <> 0 Then
                                    Imp = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value) - Me.UgrTicket.Rows(i).Cells("Descu").Value) * 0.16), "###,###.00")
                                    iv += Imp
                                    Dim impo As Double = Format(Imp, "######.00")

                                    bas = ((Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00") * Me.UgrTicket.Rows(i).Cells("Cantidad").Value) - Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00"))
                                    Impu = "002"
                                    ToC = "0.160000"
                                    TFac = "Tasa"
                                    EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), Impu.ToString, TFac.ToString, ToC.ToString, impo.ToString)
                                Else
                                    Imp = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value) - (Me.UgrTicket.Rows(i).Cells("Descu").Value - 0.1)) * 0.16), "###,###.00")
                                    iv += Imp
                                    Dim impo As Double = Format(Imp, "######.00")
                                    bas = Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00") - (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                                    Impu = "002"
                                    ToC = "0.160000"
                                    TFac = "Tasa"
                                    EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), Impu.ToString, TFac.ToString, ToC.ToString, impo.ToString)
                                End If
                            Next
                            Impuesto = "002"
                            TasaOCuota = "0.160000"
                            TipoFactor = "Tasa"
                            Importe = Format(iv, "######.00")

                            Dim TotalImpuestosTrasladados As Double = 0
                            TotalImpuestosTrasladados = Format(iv, "######.00")

                            'Agregamos los Impuestos
                            EasyOne_cfdi33_SetComprobanteImpuestos(p, "", TotalImpuestosTrasladados)
                            EasyOne_cfdi33_AddTrasladosTraslado(p, Impuesto, TipoFactor, TasaOCuota, Format(Importe, "######.00"))


                            Dim Total As Double = 0
                            Dim Subt As Double = 0

                            Subt = Format(sb, "######.00")
                            Total = Format(((Subt - des) + iv), "######.00")
                            Fila("Fsubtotal") = Format(Subt, "###,###.00")
                            Fila("Fdescuento") = Format(des, "###,###.00")
                            Fila("Fiva") = Format(iv, "###,###.00")
                            Fila("Ftotal") = Format(Total, "###,###.00")

                            Dim cantletra As String
                            cantletra = Letras(Total)
                            Fila("cantidadl") = cantletra

                            Dim ValorTicket As String = Cadena
                            NoCuenta = txtNumCuenta.Text

                            Dim freal, hr As String

                            Dim metpg As String
                            Dim tipcom As String
                            Dim formpg As String
                            metpg = "PUE" REM COLOCAR POR DEFAULT PUE PERO SE DEBE ACTIVAR LA OPCION PARA BOUTIQUES
                            tipcom = "I"
                            formpg = grbformaPago.Value.ToString '"99"

                            Dim Descue As String = ""
                            Dim desc2 As String
                            Descue = Format(des, "###,###.00")
                            desc2 = Descue.Replace(",", "")

                            Fila("metpago") = metpg
                            If tipcom = "I" Then
                                Fila("tipocomp") = "I - Ingreso"
                            Else
                                Fila("tipocomp") = "E - Egreso"
                            End If
                            Fila("formpago") = fp

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
                                    'MsgBox("Intenta tmbrar nuevamente pero con otra hora !!! 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                                    hr = hra
                                End If

                                freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                                If (serie = "" And folio = "") Or (folio <> txtFactura.Text) Then
                                    'Llenamos la sección del comprobante
                                    If desc2 = ".00" Or desc2 = "" Then
                                        EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, formpg, "", Format((Subt), "######.00"), "", "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
                                    Else
                                        EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, formpg, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
                                    End If

                                    yafac = objpro.ObtenDatos("Select * from ventaenc where vefac = '0' and veestatu = 0 and veFolio =" & grbTienda.Value & " and sid = " & grbSucursal.Value & "  and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique).Tables(0)
                                    If yafac.Rows.Count = 1 Then
                                        '' HAGO CONSULTA EN EL VENTAENC PARA VER SI YA SE TIMBRO de MotorBoutique para clientes
                                        facturado = objpro.ObtenDatos("Select * from ventaenc where vefac = '" & CInt(txtFactura.Text) & "' and veestatu = 1 and veFolio =" & grbTienda.Value & " and sid = " & grbSucursal.Value & "  and VeIdFolv = " & Terminal & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique).Tables(0)
                                        If facturado.Rows.Count = 0 Then
                                            EasyOne_SetInvoiceOneFachadaInfoPrueba(p, invoice, invoice)
                                            'EasyOne_SetInvoiceOneFachadaInfo(p, invoice, invoice)
                                            Dim b As Integer
                                            b = p
                                            x = EasyOne_FirmaComprobante(b, CerFactura, Key, Pwd)
                                            p = b
                                            b = 0
                                            Thread.Sleep(10000) 'espera por 10 segundos
                                            If x = 0 Then
                                                'objpro.Indicacion("Update VentaEnc set veEstatu = 1, veFac = '" & CInt(Me.txtFactura.Text) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & Transaccion & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 ", MotorBoutique)
                                                Application.DoEvents()
                                                'objpro.Indicacion("Update Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                                                Application.DoEvents()
                                                Dim resultadoXML As String = [String].Empty
                                                resultadoXML = EasyOne_LeeResultado(p, 8)
                                                'Obtenemos el XML completo y timbrado
                                                Console.WriteLine(resultadoXML)

                                                Application.DoEvents()
                                                IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"), resultadoXML)
                                                upb.Value += 10 'Suma 50
                                                va += 1
                                                hra = hr
                                                serie = txtSerie.Text
                                                folio = txtFactura.Text

                                                If x = 0 And va = 1 Then
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
                                                    documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"))
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

                                                        RutaLogo = rutlogo
                                                        RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml.png")

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

                                                        upb.Value += 10 'Suma 60

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

                                                        '''''''''''''''''''''''''''''''''''''''''''''
                                                        Dim Estatus As Int16
                                                        Dim ExisteClave As String
                                                        Dim FeFecCan As Date = "01/01/1900"
                                                        Dim FeMotcan As String = ""
                                                        Dim Tipo As String = "F"

                                                        ExisteClave = CStr(objpro.DatoString("cid", "catclie", "crfc = '" & Me.txtRfc.Text & "' ", Motor))
                                                        Dim trn = Len(Me.txtTransaccion.Text)
                                                        Dim tran As String = Mid(Me.txtTransaccion.Text, 5, trn)

                                                        Dim Subtotal = FormatCurrency(Me.Subtotal.Value)
                                                        Dim Iva = FormatCurrency(Me.Iva.Value)
                                                        Dim Total2 = FormatCurrency(Me.Total.Value)
                                                        Dim PorIva = 16.0
                                                        Dim Descuento = FormatCurrency(Me.Descuento.Value)

                                                        Dim fecha1, fecha2, fecha3, fechareal, FeRegistro As String
                                                        fecha1 = Now.Date.ToString.Substring(0, 2)
                                                        fecha2 = Now.Date.ToString.Substring(3, 2)
                                                        fecha3 = Now.Date.ToString.Substring(6, 4)
                                                        fechareal = fecha1 & "/" & fecha2 & "/" & fecha3
                                                        FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & txtFechaVenta.Text.ToString.Substring(3, 2) & txtFechaVenta.Text.ToString.Substring(0, 2) 'Now.Date.ToString.Substring(6, 4) & fecha2 & fecha1
                                                        PorIva = 16.0

                                                        upb.Value += 10 'Suma 80 

                                                        Dim feAbonos As Double = 0
                                                        Estatus = 1
                                                        'objpro.Indicacion("Insert into FEncabez (FeId, tId, FeTerm, FeFact, feSerie, sid, feFecha, feHora, Cid, FeSubt, fePorIva, FeIva, " &
                                                        '"FeDesc1, FePDesc1, FeDesc2, FePDesc2, FeTotal, FeCanlet, FeAbonos, FeEstatu, FeReg, feTip, feuuid, fformpag) " &
                                                        '"values (" & CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text))) & ", " & Me.grbTienda.Value & ", " & CInt(Mid(Trim(Me.txtTransaccion.Text), 1, 3)) & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "', " &
                                                        '" '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ", '" & Subtotal & "', " & PorIva & ",'" & Iva & "', '" & Descuento & "','" & Descuento & "', '" & Descuento & "', '" & Descuento & "', " &
                                                        '" '" & Total2 & "', '" & cantletra & "', 0 ," & Estatus & ",'" & FeRegistro & "', '" & Tipo & "', '" & uuid & "', '" & Me.grbformaPago.Value & "')", Motor)

                                                        Application.DoEvents()

                                                        ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

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
                                                        Next
                                                        Dim smtp As New System.Net.Mail.SmtpClient
                                                        smtp.Host = SmtpHost
                                                        smtp.Port = Int(PuertoSmtp)
                                                        smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                                                        smtp.EnableSsl = SeguridadEmail
                                                        upb.Value += 10 'Suma 100
                                                        Application.DoEvents()
                                                        'smtp.Send(correo)

                                                        MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                                                        BorrarRegistro()
                                                        DatClientes()
                                                        btnFacturar.Visible = True
                                                        yafac.Reset()
                                                        facturado.Reset()
                                                    Else
                                                        MsgBox("Archivo XML no es una factura version 4.0")
                                                    End If
                                                Else
                                                    'En caso de error mostrar el detalle.
                                                    Dim resultadoXML2 As String = [String].Empty
                                                    resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                                    Console.WriteLine(resultadoXML2)
                                                    'MsgBox("ERROR AL ENVIAR XML, CONSULTE ARCHIVO SOAP...", MsgBoxStyle.Information, "Facturación")
                                                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación")
                                                    btnFacturar.Visible = True
                                                End If 'Cierra generacion de xml

                                            Else
                                                btnFacturar.Visible = True
                                                Dim resultadoXML2 As String = [String].Empty
                                                resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                                MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                                'MsgBox("No se timbro archivo, contactar DEPTO. DE SISTEMAS, XML MAL  No. de Veces de Timbrado " & va, MsgBoxStyle.Critical).ToString()
                                                hra = hr
                                            End If
                                        End If


                                    Else
                                        btnFacturar.Visible = True
                                        MsgBox("La factura ya esta realizada (VALIDA VENTAENC) !!! ", MsgBoxStyle.Information).ToString()
                                    End If
                                Else
                                    btnFacturar.Visible = True
                                    MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
                                End If

                            Else
                                btnFacturar.Visible = True
                                MsgBox("Archivo ya Timbrado !!! No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
                            End If
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                            btnFacturar.Visible = True
                        End Try
                    End If 'Cierra la validación de cantidad de folios
                Else
                    MsgBox("Falta forma de pago", MsgBoxStyle.Information, "Facturación")
                    btnFacturar.Visible = True
                End If 'Cierra validacion de Forma de Pago
            Else
                MsgBox("Falta información del cliente para realizar la factura, por favor verifique", MsgBoxStyle.Information, "Facturación")
                btnFacturar.Visible = True
            End If 'Cierra validación datos de cliente

        Else
            MsgBox("Falta seleccionar metodo de pago .. ", MsgBoxStyle.Information, "Facturación ..")
            btnFacturar.Visible = True
        End If
    End Sub

    Private Sub DatClientes()
        grbClientes.Text = ""
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        Textrf.Text = ""
        TextRegFiscal.Text = ""
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

    Private Sub frmFacClientes_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        cfdiuso = ""
        cfdides = ""
        Me.Dispose()
    End Sub


    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", Motor) = True Then
            verNumCta(True)
        Else
            verNumCta(False)
        End If
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub btnInicializa_Click(sender As Object, e As EventArgs) Handles btnInicializa.Click
        DatClientes()
        txtRfc.Focus()
    End Sub

    Private Sub txtRfc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRfc.KeyPress
        If e.KeyChar = Chr(13) Then
            txtRazonSocial.Focus()
        Else
            e.KeyChar = UCase(e.KeyChar)
        End If
    End Sub

    Private Sub txtRazonSocial_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRazonSocial.KeyPress
        If e.KeyChar = Chr(13) Then
            txtEmail.Focus()
        Else
            e.KeyChar = UCase(e.KeyChar)
        End If
    End Sub


    Private Sub txtEmail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmail.KeyPress
        If e.KeyChar = Chr(13) Then
            btnUsoCfdi.Focus()
        Else
            e.KeyChar = LCase(e.KeyChar)
        End If
    End Sub

    Private Sub txtTransaccion_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtTransaccion.KeyPress
        If e.KeyChar = Chr(13) Then
            btnBuscarNota.Focus()
        End If
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
End Class