Imports System
Imports N400
Imports N400.Componentes.CFDI
Imports N400.Componentes.CFDI.PDF
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml

Public Class frmCompPagos
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Variables"
    Dim TIP As String
    Dim banSolCuenta As Boolean
    Dim fp As String
    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim TRFiscal As String
    Dim TpRegimen As String
    Dim Uso_CFDI As String
    Dim lugarPago As String
    Dim FechaBusqueda As String
    Dim Tabla, TablaTienda, DatGrid As DataTable
    Dim FechaInicio, FechaFin As String
    Dim FechaBus As String
    Dim Mes As Integer = 0
    Dim tablaFacturas As Data.DataTable
    Dim va As Integer
#End Region

#Region "DatosEmisor"
    Dim TRazonSocial As String
    Dim TRfc As String
    Dim RegimenFiscal As String
    Dim DesRegimenFiscal As String
    Dim DesCFDI As String
    Dim TCp As String
    Dim cCP As String
    Dim cDireccion As String
    Dim cComDir As String
#End Region

#Region "Forma Pago"
    Dim FormPago As String
    Dim MetPago As String
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
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

    Private Sub frmCompPagos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        REM Dim r As Rectangle = My.Computer.Screen.WorkingArea
        REM Location = New Point(r.Width - Width, r.Height - Height)
        TIP = "PA"
        va = 0
        verNumCta(False)
        verFormaPago(True)
        verEmpresa(False)
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM BGXFacElec.CatTien ", 0, 1, As400)
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        fechaPago.Value = Now.Date
        Inicializa()
        Application.DoEvents()
    End Sub

    Private Sub frmCompPagos_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub grbTienda_TextChanged(sender As Object, e As EventArgs) Handles grbTienda.TextChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        grbSucursal.Focus()
        grbSucursal.Select()
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        With Me.grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            .DisplayLayout.Bands(0).Columns(1).Width = 250
        End With
    End Sub

    Private Sub grbSucursal_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbSucursal.InitializeLayout
        With Me.grbSucursal
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            .DisplayLayout.Bands(0).Columns(1).Width = 250
        End With
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If grbTienda.Text <> "" Then
            If grbSucursal.Text <> "" Then
                Inicializa_Totales()
                CargaGridConceptos()
                Application.DoEvents()

                CargaGrid()
            Else
                MsgBox("Falta sucursal por seleccionar .. ", MsgBoxStyle.Information, "Facturacion")
            End If
        Else
            MsgBox("Falta empresa por seleccionar .. ", MsgBoxStyle.Information, "Facturacion")
        End If
    End Sub


    Private Sub CargaGrid()
        Try

            REM FechaInicio = Me.grbPeriodo.Text.ToString & Me.grbMes.Text & "01"
            REM FechaFin = Me.grbPeriodo.SelectedItem.Text & Me.grbMes.Text & Me.grbDia.SelectedItem.Text
            FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
            FechaInicio = FechaBusqueda & "01"
            Dim bf = DateSerial(grbPeriodo.Text, objfun.EscojeMes(Me.grbMes.Text) + 1, 0)
            FechaFin = FechaBusqueda & bf.ToString.Substring(0, 2)
            objpro.LLenaGrid(Ugr, "SELECT feId as Id, feuuid AS uuid, FEncabezado.FeFact as factura,FEncabezado.fefecha as fecha,FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado, " &
                             " FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,FEncabezado.FeSubt as Subtotal, " &
                             " FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total,FEncabezado.tid, FEncabezado.feabonos as Abono FROM BGXFacElec.FEncabez as FEncabezado " &
                             " left join BGXFacElec.CatClie as Clientes on FEncabezado.Cid = Clientes.Cid " &
                             " Where (FEncabezado.FeReg >= " & FechaInicio & " and FEncabezado.FeReg <= " & FechaFin & ") and FEncabezado.Tid = " & Me.grbTienda.Value & " and " &
                             " FEncabezado.Sid = " & grbSucursal.Value & " order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", As400)
            Me.Ugr.DataBind()
            Application.DoEvents()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturacion .. ")
        End Try
    End Sub

    Private Sub CargaGridConceptos() 'uuidsel As String
        FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
        FechaInicio = FechaBusqueda & "01"
        Dim bf = DateSerial(grbPeriodo.Text, objfun.EscojeMes(Me.grbMes.Text) + 1, 0)
        FechaFin = FechaBusqueda & bf.ToString.Substring(0, 2)

        objpro.LLenaGrid(Me.ugrselect, "SELECT '' as IMPORTE, feId as Id, feuuid AS uuid, FEncabezado.FeFact as factura,FEncabezado.fefecha as fecha,FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado, " &
                             " FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,FEncabezado.FeSubt as Subtotal, " &
                             " FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total,FEncabezado.tid, FEncabezado.feabonos as Abono, '' as Saldo FROM BGXFacElec.FEncabez as FEncabezado " &
                             " left join BGXFacElec.CatClie as Clientes on FEncabezado.Cid = Clientes.Cid " &
                             " Where (FEncabezado.FeReg >= " & FechaInicio & " and FEncabezado.FeReg <= " & FechaFin & ") and FEncabezado.Tid = 0 and " &
                             " FEncabezado.Sid = 0 order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", As400)
        Me.Ugr.DataBind()
    End Sub

    Private Sub FormatoGrid(imp As Double, id As Integer, uuid As String, factura As String, fecha As String, Serie As String, edo As String, cid As Integer, terminal As Integer, rfc_Clientes As String, Clientes As String,
                            Subtotal As Double, descu As Double, iva As Double, Total As Double, Abono As Double, Saldo As Double)
        ugrselect.Rows.Band.AddNew()
        ColocarArticulo(ugrselect.Rows.Count - 1, imp, id, uuid, factura, fecha, Serie, edo, cid, terminal, rfc_Clientes, Clientes, Subtotal, descu, iva, Total, Abono, Saldo)
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, Importe As Double, id As Integer, uuid As String, factura As String, fecha As String, Serie As String, edo As String, cid As Integer, terminal As Integer, rfc_Clientes As String, Clientes As String, Subtotal As Double, descu As Double, iva As Double, Total As Double, Abono As Double, Saldo As Double)
        With ugrselect
            .Rows(Renglon).Cells("IMPORTE").Value = Importe
            .Rows(Renglon).Cells("ID").Value = id
            .Rows(Renglon).Cells("UUID").Value = uuid
            .Rows(Renglon).Cells("FACTURA").Value = factura
            .Rows(Renglon).Cells("FECHA").Value = fecha
            .Rows(Renglon).Cells("SERIE").Value = Serie
            .Rows(Renglon).Cells("ESTADO").Value = edo
            .Rows(Renglon).Cells("CID").Value = cid
            .Rows(Renglon).Cells("TERMINAL").Value = terminal
            .Rows(Renglon).Cells("RFC_CLIENTES").Value = rfc_Clientes
            .Rows(Renglon).Cells("CLIENTES").Value = Clientes.Trim
            .Rows(Renglon).Cells("SUBTOTAL").Value = Subtotal
            .Rows(Renglon).Cells("DESCUENTO").Value = descu
            .Rows(Renglon).Cells("IVA").Value = iva
            .Rows(Renglon).Cells("TOTAL").Value = Total
            .Rows(Renglon).Cells("ABONO").Value = Abono
            .Rows(Renglon).Cells("SALDO").Value = Saldo
        End With
    End Sub

    Private Sub Ugr_DoubleClickCell(sender As Object, e As Infragistics.Win.UltraWinGrid.DoubleClickCellEventArgs) Handles Ugr.DoubleClickCell
        Try
            txtFactura.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
            txtSerie.Text = Ugr.ActiveRow.Cells("Serie").Value.ToString
            txtUUID.Text = Ugr.ActiveRow.Cells("UUID").Value.ToString
            Total.Value = Ugr.ActiveRow.Cells("Total").Value.ToString
            Abono.Value = Ugr.ActiveRow.Cells("Abono").Value.ToString
            Saldo.Value = (Total.Value - Abono.Value)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            '.DisplayLayout.Bands(0).Columns(0).Width = 15
            ''.DisplayLayout.Bands(0).Columns(0).DataType = GetType(Boolean)
            '.DisplayLayout.Bands(0).Columns(0).Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox
            '.DisplayLayout.Bands(0).Columns(0).CellActivation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            .DisplayLayout.Bands(0).Columns(1).Width = 200
            .DisplayLayout.Bands(0).Columns(2).Width = 40
            .DisplayLayout.Bands(0).Columns(3).Width = 70
            .DisplayLayout.Bands(0).Columns(4).Hidden = True
            .DisplayLayout.Bands(0).Columns(5).Hidden = True
            .DisplayLayout.Bands(0).Columns(6).Hidden = True
            .DisplayLayout.Bands(0).Columns(7).Hidden = True
            .DisplayLayout.Bands(0).Columns(8).Width = 70
            .DisplayLayout.Bands(0).Columns(9).Width = 130
            .DisplayLayout.Bands(0).Columns(10).Width = 80
            .DisplayLayout.Bands(0).Columns(11).Width = 80
            .DisplayLayout.Bands(0).Columns(12).Width = 70
            .DisplayLayout.Bands(0).Columns(13).Width = 80
            .DisplayLayout.Bands(0).Columns(14).Hidden = True
            .DisplayLayout.Bands(0).Columns(15).Width = 80
        End With
    End Sub

    Private Sub ugrselect_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugrselect.InitializeLayout
        With ugrselect
            .DisplayLayout.Bands(0).Columns(0).Width = 70
            .DisplayLayout.Bands(0).Columns(1).Hidden = True
            .DisplayLayout.Bands(0).Columns(2).Width = 200
            .DisplayLayout.Bands(0).Columns(3).Width = 40
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Hidden = True
            .DisplayLayout.Bands(0).Columns(6).Hidden = True
            .DisplayLayout.Bands(0).Columns(7).Hidden = True
            .DisplayLayout.Bands(0).Columns(8).Hidden = True
            .DisplayLayout.Bands(0).Columns(9).Width = 70
            .DisplayLayout.Bands(0).Columns(10).Width = 130
            .DisplayLayout.Bands(0).Columns(11).Width = 80
            .DisplayLayout.Bands(0).Columns(12).Width = 80
            .DisplayLayout.Bands(0).Columns(13).Width = 70
            .DisplayLayout.Bands(0).Columns(14).Width = 80
            .DisplayLayout.Bands(0).Columns(15).Hidden = True
            .DisplayLayout.Bands(0).Columns(16).Width = 80
            .DisplayLayout.Bands(0).Columns(17).Width = 70
        End With
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtUUID.Text <> "" Then
            If Saldo.Value <> 0 Then
                If Importe.Value <> 0 Then
                    If Importe.Value <= Saldo.Value Then
                        Try
                            FormatoGrid(Importe.Value, Ugr.ActiveRow.Cells("id").Value, Ugr.ActiveRow.Cells("uuid").Value, Ugr.ActiveRow.Cells("factura").Value, Ugr.ActiveRow.Cells("fecha").Value, Ugr.ActiveRow.Cells("serie").Value, Ugr.ActiveRow.Cells("estado").Value, Ugr.ActiveRow.Cells("cid").Value, Ugr.ActiveRow.Cells("terminal").Value, Ugr.ActiveRow.Cells("rfc_Clientes").Value, Ugr.ActiveRow.Cells("Clientes").Value, Ugr.ActiveRow.Cells("Subtotal").Value, Ugr.ActiveRow.Cells("Descuento").Value, Ugr.ActiveRow.Cells("Iva").Value, Ugr.ActiveRow.Cells("Total").Value, Ugr.ActiveRow.Cells("Abono").Value, Saldo.Value)
                            If ugrselect.Rows.Count <> 0 Then
                                sumariza_grid()
                                Inicializa_Totales()
                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
                        End Try
                    Else
                        MsgBox("El importe a abonar no puede ser mayor al saldo ", MsgBoxStyle.Information, "Facturación ..")
                    End If
                Else
                    MsgBox("El importe a abonar no puede ser cero ", MsgBoxStyle.Information, "Facturación ..")
                End If
            Else
                MsgBox("La factura ya se cubrio en su totalidad ", MsgBoxStyle.Information, "Facturación ..")
            End If
        Else
            MsgBox("No hay UUID relacionado para el pago ", MsgBoxStyle.Information, "Facturación ..")
        End If
    End Sub

    Private Sub ugrselect_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugrselect.AfterRowsDeleted
        sumariza_grid()
    End Sub

    Sub sumariza_grid()
        TAbono.Value = 0
        Tsaldo.Value = 0
        For Each r In ugrselect.Rows
            TAbono.Value += r.Cells("IMPORTE").Value.ToString    'Ugr.ActiveRow.Cells("Total").Value.ToString
            Tsaldo.Value += (r.Cells("SALDO").Value) '+ r.Cells("Abono").Value)  'Ugr.ActiveRow.Cells("Abono").Value.ToString

        Next
    End Sub



    Private Sub Inicializa_Totales()
        txtFactura.Text = ""
        txtSerie.Text = ""
        txtUUID.Text = ""
        Importe.Value = 0.0
        Total.Value = 0.0
        Abono.Value = 0.0
        Saldo.Value = 0.0
    End Sub

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        btnBuscar.Focus()
    End Sub

    Private Sub btnAplicarPago_Click(sender As Object, e As EventArgs) Handles btnAplicarPago.Click
        If TAbono.Value > 0 Then
            If MsgBox("Seguro de realizar pago .. ", MsgBoxStyle.OkCancel, "Facturación .. ") = MsgBoxResult.Ok Then
                If txtRfc.Text <> "" Then
                    If txtRazonSocial.Text <> "" Then
                        btnAplicarPago.Visible = False
                        Pago()
                    Else
                        MsgBox("Falta seleccionar Razon Social receptor .. ", MsgBoxStyle.Information, "Facturación .. ")
                    End If
                Else
                    MsgBox("Falta seleccionar rfc receptor .. ", MsgBoxStyle.Information, "Facturación .. ")
                End If
            End If
        Else
            MsgBox("Importe a aplicar no puede ser menor o igual a cero ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub Pago()
        upb.Value = 0
        upb.Maximum = 100
        Dim d As String = fechaPago.Value.ToString.Substring(0, 2)
        Dim m As String = fechaPago.Value.ToString.Substring(3, 2)
        Dim a As String = fechaPago.Value.ToString.Substring(6, 4)
        Dim FecPAgo As String = a & "-" & m & "-" & d & "T12:00:00"

        Dim serie As String = objpro.Dato("seserie", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)
        Dim NumPago As String = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")

        TablaTienda = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", As400).Tables(0)

        DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
        PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
        Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
        PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
        SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
        PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
        DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
        AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
        MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString

        Dim Directorios, RutFacClientes, RutFacGlobal, CarpTienda, CarpSucursal, RutNotaCredito As String
        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, rutlogo As String
        Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
        Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
        Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
        Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString
        Dim UserPAC As String = Trim(TablaTienda.Rows(0).Item("tUserPAC").ToString)
        Dim PwdPAC As String = Trim(TablaTienda.Rows(0).Item("tPwdPAC").ToString)
        Dim RutPagos = TablaTienda.Rows(0).Item("sForm4").ToString
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim FechaFactura As String = Date.Now.ToString.Substring(0, 10)
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
        TRfc = TablaTienda.Rows(0).Item("trfc").ToString
        TCp = TablaTienda.Rows(0).Item("tcp").ToString
        TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
        direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
        CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
        CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
        rutlogo = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png"
        Dim RFacImpresas As String
        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

        Dim rfcEmisor As String = objpro.Dato("trfc", "bgxfacelec.cattien", "Tid = " & grbTienda.Value & " ", As400)
        Dim rsEmisor As String = objpro.Dato("trsocial", "bgxfacelec.cattien", "Tid = " & grbTienda.Value & " ", As400)
        lugarPago = objpro.Dato("selugarexp", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)

        Dim c As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml") 'New CFDI33.Comprobante

        EasyOne_cfdi33_SetCfdi40(c, "01", "")

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
        Fila("Eregfis") = TRFiscal + " - " + descreg.Trim
        Fila("Edireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
        Fila("Edircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

        upb.Value += 10 'Inicia valor

        TRFiscal = objpro.Dato("satregimen", "bgxfacelec.catsucu", "Tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)

        Dim rfcReceptor As String = Trim(txtRfc.Text)
        Dim rsReceptor As String = Trim(txtRazonSocial.Text)

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
            Case 621
                TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                DesRegimen = "Incorporacion Fiscal"
            Case 626
                TpRegimen = "626"
                DesRegimen = "Régimen Simplificado de Confianza"
        End Select

        'c.AgregaDatosEmisor(rfcEmisor, rsEmisor, TpRegimen)
        EasyOne_cfdi33_SetEmisor(c, rfcEmisor, rsEmisor, TpRegimen)
        'c.AgregaDatosReceptor(rfcReceptor, rsReceptor, c_UsoCFDI.Otros, CFDI33.c_Pais.Mexico)
        EasyOne_cfdi33_SetReceptor(c, rfcReceptor, rsReceptor, "", "", "CP01")

        EasyOne_cfdi33_SetReceptor40(c, cCP, RegimenFiscal)

        Fila("Rcp") = cCP
        Fila("Rrfc") = rfcReceptor
        Fila("Rrazonsoc") = rsReceptor
        Fila("Rdireccion") = ""
        Fila("Rregimen") = RegimenFiscal & "-" & DesRegimenFiscal
        Fila("usocfdi") = "CP01"
        Fila("moneda") = "XXX"
        Fila("rcomdir") = cComDir
        Fila("nocuenta") = txtNumCuenta.Text

        Dim Tabla2 As DataTable = DS.Tables.Add("PAGOS")
        Tabla2.Columns.Add("FechPago")
        Tabla2.Columns.Add("CFDIpricipal")
        Tabla2.Columns.Add("Serie")
        Tabla2.Columns.Add("Folio")
        Tabla2.Columns.Add("SaldoAnt")
        Tabla2.Columns.Add("Importe")
        Tabla2.Columns.Add("SaldoInsoluto")
        Tabla2.Columns.Add("RFCBeneficiario")
        Tabla2.Columns.Add("NoCtaBenef")
        Tabla2.Columns.Add("RFCOrdenante")
        Tabla2.Columns.Add("BancoOrd")
        Tabla2.Columns.Add("NoCtaOrd")
        Dim Fila2 As DataRow

        Dim Iva As Double
        Iva = Format((TAbono.Value * 0.16), "######.00")

        EasyOne_cfdi33_AddConcepto(c, "84111506", "", "1", "ACT", "", "Pago", 0, 0, "")

        EasyOne_cfdi33_SetConcepto40(c, "01")

        Dim p As Integer = EasyOne_pago10_Create()

        EasyOne_pago10_SetVersion(p, "2.0")

        upb.Value += 10 '20

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

        Fila("metpago") = "PUE"
        Fila("tipocomp") = "P - PAGO"
        Fila("formpago") = FormPago

        'Aqui la informacion de un pago, en un mismo pago nos pueden pagar varios documentos relacionados
        'Fecha de Pago, FormaDePagoP, MonedaP, TipoCambioP, Monto, NumOperacion, RfcEmisorCtaOrd, NomBancoOrdExt, CtaOrdenante, RfcEmisorCtaBen, CtaBeneficiario, TipoCadPago, CertPago, CadPago, SelloPago
        If grbformaPago.Text = "Efectivo" Then
            EasyOne_pago10_AddPago(p, FecPAgo, grbformaPago.Value, "MXN", "01", Format(TAbono.Value, "######.00"), txtNumOperacion.Text, "", "", "", "", "", "", "", "", "")
        Else
            EasyOne_pago10_AddPago(p, FecPAgo, grbformaPago.Value, "MXN", "01", Format(TAbono.Value, "######.00"), txtNumOperacion.Text, rfcEmisor, grbBancos.Text, txtCtaOrdenante.Text, rfcReceptor, txtCtaBeneficiario.Text, "", "", "", "")
        End If
        If grbformaPago.Text <> "Efectivo" Then
            If txtCtaOrdenante.Text = "" Then
                Me.txtCtaBeneficiario.Text = ""
                Me.txtCtaOrdenante.Text = ""
                Me.grbBancos.Text = ""
            End If
        End If
        upb.Value += 10 '30
        Dim basep As Double = 0
        Dim ImporteP As Double = 0
        For Each s In ugrselect.Rows
            Dim SDOINS As Double = 0
            SDOINS = (s.Cells("SALDO").Value - s.Cells("IMPORTE").Value)

            ''Informacion del documento relacionado
            ''En este caso es un pago parcial de un documento, recordemos que en un pago puede haber uno o muchos documentos relacionados
            ''IdDocumento, serie, Folio, MonedaDR, TipoCambioDR(No se usa en Pagos 2.0), MetodoDePagoDR(No se usa en Pagos 2.0), NumParcialidad, ImpSaldoAnt, ImpPagado, ImpSaldoInsoluto
            If SDOINS = 0 Then
                EasyOne_pago10_AddDoctoRelacionado(p, s.Cells("UUID").Value, s.Cells("SERIE").Value, s.Cells("FACTURA").Value, "MXN", "", "", "1", s.Cells("SALDO").Value, s.Cells("IMPORTE").Value, SDOINS)
            Else
                EasyOne_pago10_AddDoctoRelacionado(p, s.Cells("UUID").Value, s.Cells("SERIE").Value, s.Cells("FACTURA").Value, "MXN", "", "", "1", s.Cells("SALDO").Value, s.Cells("IMPORTE").Value, Format(SDOINS, "######.00"))
            End If

            Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
            Fila2("FechPago") = FecPAgo
            Fila2("CFDIpricipal") = s.Cells("UUID").Value.ToString 'txtUUID.Text
            Fila2("Serie") = s.Cells("SERIE").Value.ToString 'txtSerie.Text
            Fila2("Folio") = s.Cells("FACTURA").Value.ToString 'txtFactura.Text
            Fila2("SaldoAnt") = s.Cells("SALDO").Value  'Saldo.Value
            Fila2("Importe") = s.Cells("IMPORTE").Value 'Importe.Value
            If SDOINS = 0 Then
                Fila2("SaldoInsoluto") = SDOINS 'Format((Saldo.Value - Importe.Value), "###,###.00")
            Else
                Fila2("SaldoInsoluto") = Format(SDOINS, "######.00")
            End If
            If grbformaPago.Text <> "Efectivo" Then
                Fila2("RFCBeneficiario") = rfcReceptor
                Fila2("NoCtaBenef") = txtCtaBeneficiario.Text
                Fila2("RFCOrdenante") = rfcEmisor
                Fila2("BancoOrd") = grbBancos.Text
                Fila2("NoCtaOrd") = txtCtaOrdenante.Text
            End If
            DS.Tables(1).Rows.Add(Fila2)

            'Reforma Fiscal 2022 Informacion de Pagos 2.0
            ''EquivalenciaDR, ObjetoImpuestoDR
            EasyOne_pago10_SetDoctoRelacionado20(p, "1", "02")

            'Agregamos el iva del documento relacionado que nos estan pagando
            'BaseDR, ImpuestoDR, TipoFactorDR, TasaOCuotaDR, ImporteDR
            EasyOne_pago10_AddTrasladoDR20(p, Format(s.Cells("subtotal").Value, "######.00"), "002", "Tasa", "0.160000", Format(s.Cells("iva").Value, "######.00"))
            basep += s.Cells("subtotal").Value
            ImporteP += s.Cells("iva").Value
        Next
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''
        upb.Value += 10  'Suma 40
        Fila("Ftotal") = Format(TAbono.Value, "######.00")

        'datos del pago
        Dim fechapag, hr, fpag As String
        fpag = Now
        hr = Now.ToLocalTime.ToString.Substring(11, 8)
        fechapag = Now.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr 'AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss")

        EasyOne_cfdi33_SetComprobante(c, serie, NumPago, fechapag, "", "", 0, "", "XXX", "", 0, "P", "", lugarPago, "")

        ''Informacion Global de totales del Pago - Aqui se debe registrar la suma de todos los importes y bases de todos los pagos
        'TotalRetencionesIVA, TotalRetencionesISR, TotalRetencionesIEPS, TotalTrasladosBaseIVA16, TotalTrasladosImpuestoIVA16, TotalTrasladosBaseIVA8, TotalTrasladosImpuestoIVA8, TotalTrasladosBaseIVA0, TotalTrasladosImpuestoIVA0, TotalTrasladosBaseIVAExento, MontoTotalPagos
        EasyOne_pago10_SetTotales20(p, "", "", "", Format(basep, "######.00"), Format(ImporteP, "######.00"), "", "", "", "", "", Format(TAbono.Value, "######.00"))

        ''Informacion Global del Pago - Aqui tenemos que poner un resumen de los traslados del documento relacionado
        'BaseP, ImpuestoP, TipoFactorP, TasaOCuotaP, ImporteP
        EasyOne_pago10_AddTrasladoP20(p, Format(basep, "######.00"), "002", "Tasa", "0.160000", Format(ImporteP, "######.00"))

        ''Se agrega el complemento al comprobante
        EasyOne_AddComplemento(c, p)

        TablaTienda = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", As400).Tables(0)

        DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
        PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
        Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
        PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
        SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
        PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
        DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
        AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
        MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString

        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

        ''Dim motor As New CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
        'EasyOne_SetInvoiceOneFachadaInfoPrueba(c, "RHUUMRGL", "RHUUMRGL")
        EasyOne_SetInvoiceOneFachadaInfo(c, "RHUUMRGL", "RHUUMRGL")

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
                btnAplicarPago.Visible = True
            End If
        Else
            MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
        End If

        If x = 0 And va = 1 Then
            Application.DoEvents()
            Dim resultadoXML As String = [String].Empty
            resultadoXML = EasyOne_LeeResultado(c, 8)
            'Obtenemos el XML completo y timbrado
            Console.WriteLine(resultadoXML)

            Application.DoEvents()
            IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"), resultadoXML)

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
            documento.Load(Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"))
            VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
            Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
            upb.Value += 10 '50
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
                RutaQR = Trim(RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml.png")

                REM *********************************************************************************************

                objpro.Indicacion("update bgxfacelec.serie set secant = " & CInt(NumPago) & " where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and seserie = '" & serie & "' ", As400)
                Try
                    For Each fac In ugrselect.Rows
                        objpro.Indicacion("update bgxfacelec.fencabez set feabonos = feabonos + " & fac.Cells("Importe").Value & " where feid = " & fac.Cells("id").Value & " and feterm = " & fac.Cells("terminal").Value & " and tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and feserie = '" & fac.Cells("serie").Value & "' and fefact = " & fac.Cells("factura").Value & " ", As400)
                        REM *********************************************************************************************
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                upb.Value += 10 'Suma 60

                '''''''nuevoooooooooooooooooooooooooooooooooooooooooo

                DS.Tables(0).Rows.Add(Fila)
                Dim objreport As New Pagos
                objreport.SetParameterValue("Logo", RutaLogo)
                objreport.SetParameterValue("QR", RutaQR)
                Dim rpFact As New Pagos
                rpFact.SetDataSource(DS)
                rpFact.SetParameterValue("QR", RutaQR)
                rpFact.SetParameterValue("Logo", RutaLogo)
                ComPago.CrystalReportViewer1.ReportSource = objreport
                ComPago.CrystalReportViewer1.ReportSource = rpFact
                ComPago.WindowState = FormWindowState.Maximized
                ComPago.Show()

                upb.Value += 10 'Suma 70
                rutaArchivoPDF = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".pdf"
                rutaArchivoXML = RFacImpresas & "\" & Trim(rfcEmisor) & "-" & serie & "-" & NumPago & ".xml"
                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

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
                correo.From = New System.Net.Mail.MailAddress(DirEmail) 'DirEmail
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
                rpFact.Close()
                rpFact.Dispose()
                Inicializa_Totales()
                CargaGridConceptos()
                Application.DoEvents()
                Inicializa()
                btnAplicarPago.Visible = True
            Else
                MsgBox("Archivo XML no es una factura version 4.0")
            End If 'Cierra validación de operación exitosa
        Else
            'En caso de error mostrar el detalle.
            Dim resultadoXML2 As String = [String].Empty
            resultadoXML2 = EasyOne_LeeResultado(c, 11)
            Console.WriteLine(resultadoXML2)
            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
            btnAplicarPago.Visible = True
        End If 'Cierra validación generación de XML
    End Sub

    Private Sub Inicializa()
        Total.Value = 0.0
        Abono.Value = 0.0
        Saldo.Value = 0.0
        grbformaPago.Text = ""
        txtNumCuenta.Text = ""
        txtNumOperacion.Text = ""
        txtCtaBeneficiario.Text = ""
        grbBancos.Text = ""
        txtCtaOrdenante.Text = ""
        Importe.Value = 0.0
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
    End Sub

    Public Function APDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            REM .DisplayLayout.Bands(0).Columns(1).Width = 250
        End With
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub txtNumOperacion_KeyPress(sender As Object, e As KeyPressEventArgs)
        If e.KeyChar = Chr(13) Then
            txtCtaBeneficiario.Focus()
        End If
    End Sub

    Private Sub txtCtaBeneficiario_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCtaBeneficiario.KeyPress
        If e.KeyChar = Chr(13) Then
            grbBancos.Focus()
        End If
    End Sub

    Private Sub grbBancos_TextChanged(sender As Object, e As EventArgs) Handles grbBancos.TextChanged
        If grbBancos.Text <> "" Then
            txtCtaOrdenante.Focus()
        End If
    End Sub

    Private Sub txtCtaOrdenante_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCtaOrdenante.KeyPress
        If e.KeyChar = Chr(13) Then
            Importe.Focus()
        End If
    End Sub

    Private Sub Importe_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Importe.KeyPress
        If e.KeyChar = Chr(13) Then
            btnAplicarPago.Focus()
        End If
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub verFormaPago(opcion As Boolean)
        lblMetodoPago.Visible = opcion
        grbformaPago.Visible = opcion
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "bgxfacelec.satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", As400) = True Then
            banSolCuenta = True
            verNumCta(True)
            verEmpresa(True)
            verinfobancos(False)
            grbBancos.Visible = True
            UltraLabel2.Visible = True
            UltraLabel5.Visible = True
            UltraLabel1.Visible = True
            UltraLabel4.Visible = True
            Me.txtNumCuenta.Focus()
        Else
            banSolCuenta = False
            verNumCta(False)
            verEmpresa(True)
            verinfobancos(True)
            btnClientes.Focus()
        End If

    End Sub

    Private Sub verinfobancos(opcion As Boolean)
        UltraLabel2.Visible = opcion
        txtNumOperacion.ReadOnly = opcion
        UltraLabel5.Visible = opcion
        txtCtaBeneficiario.ReadOnly = opcion
        UltraLabel1.Visible = opcion
        grbBancos.Visible = False
        UltraLabel4.Visible = opcion
        txtCtaOrdenante.ReadOnly = opcion
    End Sub

    Private Sub verEmpresa(opcion As Boolean)
        'lblEmpresa.Visible = opcion
        lblrfc.Visible = opcion
        lblrasoncial.Visible = opcion
        lblEmail.Visible = opcion
        txtEmail.Visible = opcion
        txtRazonSocial.Visible = opcion
        txtRfc.Visible = opcion
        btnClientes.Visible = opcion
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
            cfdiuso = objpro.Dato("cusocfdi", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            RegimenFiscal = regimenFisc
            DesRegimenFiscal = regimen
            DesCFDI = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)

            Me.gpoDatos.Visible = True
            Me.txtNumOperacion.Focus()
        End If

    End Sub

End Class