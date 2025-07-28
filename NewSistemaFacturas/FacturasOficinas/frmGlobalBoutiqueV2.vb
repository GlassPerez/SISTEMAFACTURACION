Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports System.Runtime.InteropServices
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml.Linq
Imports System.Xml.XPath

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class frmGlobalBoutiqueV2

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
    'Dim datosG As New dsFactura40.dsFac40DataTable
#End Region

#Region "VariablesGlobales"
    Dim contador As Integer = 0
    Dim TIP As String
    Dim fechventa As String
    Dim FechaFactura As String
    Dim va As Integer
    Dim va2 As Integer
    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda, tiendaPrinc, sucursal, vtaenca, vtadetalle As Data.DataTable
    'Dim datosGen As Data.DataSet
    Dim Terminal, TpDev As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim TpRegimen As String
    Dim fp As String
    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim Cadena As String
    Dim e_uuid As String
    Dim PorIVa As Double
    Dim NumPiezas As Integer
    Dim td As Integer
#End Region

#Region "Datos de Factura"
    Dim txtFactura As String
    Dim txtSerie As String
    Dim txtNumAprobacion As String
    Dim txtAmoAprob As String
    Dim txtLugarExpedicion As String
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

    Private Sub frmGlobalBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub frmGlobalBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        va = 0
        va2 = 0
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien ", 0, 1, Motor)
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
        txtFechaVenta.Value = CDate(fechventa)
        Application.DoEvents()
    End Sub

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        Try
            objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
            Me.ChlisFact.Items.Clear()
            Me.ChlisNota.Items.Clear()
            Me.grbSucursal.Value = Nothing
            grbSucursal.Focus()
        Catch ex As Exception
            MsgBox("Sucursal: " & ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub grbTienda_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbTienda.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.grbSucursal.Focus()
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        consulta_registros()
    End Sub

    Public Function consulta_registros()
        If grbTienda.Text <> "" Then
            If Me.grbSucursal.Text <> "" Then


                fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
                txtFechaVenta.Value = CDate(fechventa)
                Ugr.DataSource = Nothing
                CargaListaNotas(1)
                CargaListaCredito(2)
            Else
                MsgBox("Falta seleccionar sucursal", MsgBoxStyle.Information, "Facturación.. ")
            End If
        Else
            MsgBox("Falta seleccionar Tienda o Razón social", MsgBoxStyle.Information, "Facturación.. ")
        End If
        Return ""
    End Function

    Private Sub InicializaCantidades()
        Me.Devoluciones.Value = 0
        Me.Importe.Value = 0
        Me.Subtotal.Value = 0
        Me.Descuento.Value = 0
        Me.Iva.Value = 0
        Me.Total.Value = 0
    End Sub

    Private Sub CargaListaNotas(ByVal tp As Integer)
        Try
            Application.DoEvents()
            TablaSucursal = Nothing
            TablaSucursal = objpro.ObtenDatos("select * from ventaenc where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and veidfolv = " & tp & " and Month(veFecha) = " & objfun.EscojeMes(Me.grbMes.Text) & " and year(veFecha) = " & Me.grbPeriodo.Text & " order by vefecha ", Motor).Tables(0)
            Me.ChlisFact.Items.Clear()
            Dim i As Integer = 0
            For Each row As DataRow In TablaSucursal.Rows
                Application.DoEvents()
                Me.ChlisFact.Items.Add("Venta " & row.Item("vefecha").ToString.Substring(0, 10))
                If row.Item("veEstatu") = 1 Then
                    ChlisFact.SetItemChecked(i, CheckState.Checked)
                Else
                    ChlisFact.SetItemChecked(i, CheckState.Unchecked)
                End If
                i += 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub CargaListaCredito(ByVal tp As Integer)
        Try
            Application.DoEvents()
            TablaSucursal = Nothing
            TablaSucursal = objpro.ObtenDatos("select * from ventaenc where vefolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and veidfolv = " & tp & " and Month(veFecha) = " & objfun.EscojeMes(Me.grbMes.Text) & " and year(veFecha) = " & Me.grbPeriodo.Text & " order by vefecha ", Motor).Tables(0)
            Me.ChlisNota.Items.Clear()
            Dim i As Integer = 0
            For Each row As DataRow In TablaSucursal.Rows
                Application.DoEvents()
                Me.ChlisNota.Items.Add("Dev. " & row.Item("vefecha").ToString.Substring(0, 10))
                If row.Item("veEstatu") = 1 Then
                    ChlisNota.SetItemChecked(i, CheckState.Checked)
                Else
                    ChlisNota.SetItemChecked(i, CheckState.Unchecked)
                End If
                i += 1
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub btnFactura_Click(sender As Object, e As EventArgs) Handles btnFactura.Click
        Application.DoEvents()
        Dim EstadoVenta, term As String
        Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
        Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
        Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
        Dim FecVenta As String = a & m & d
        Transaccion = 1
        TpDev = 2
        InicializaCantidades()
        If Me.grbSucursal.Value > 9 Then
            term = Trim(grbTienda.Value.ToString.Substring(0, 1)) & Trim(Str(Me.grbSucursal.Value))
        Else
            term = Trim(Str(grbTienda.Value)) + Trim(Str(Me.grbSucursal.Value))
        End If
        Terminal = Val(Trim(term))
        If ChlisFact.Items.Count > 0 Then
            If objpro.DatoExiste("veEstatu", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) = True Then
                EstadoVenta = objpro.Dato("veEstatu", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)
                If EstadoVenta = 0 Then
                    Application.DoEvents()
                    InicializaGrid()
                    TIP = "VG"
                    upb.Value = 0

                    Application.DoEvents()
                    TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
                    txtFactura = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                    txtSerie = TSucursal.Rows(0).Item("Seserie").ToString
                    txtLugarExpedicion = TSucursal.Rows(0).Item("seLugarExp").ToString

                    CargaGridInicio()
                    Application.DoEvents()

                    'Me.Subtotal.Value = objpro.Dato("vesubt", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)
                    'If objpro.DatoExiste("veDesc1", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "", Motor) = True Then
                    'Me.Descuento.Value = objpro.Dato("veDesc1", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)
                    'Else
                    'Me.Descuento.Value = 0
                    'End If
                    'Me.Iva.Value = objpro.Dato("veIva", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)
                    'Me.Total.Value = objpro.Dato("veTotal", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)

                    'Devolucion
                    If objpro.DatoExiste("veTotal", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & TpDev & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) Then
                        Me.Devoluciones.Value = objpro.Dato("veTotal", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & TpDev & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor)
                        Application.DoEvents()
                    Else
                        Me.Devoluciones.Value = 0
                    End If
                    'Devolucion
                Else
                    MsgBox("La nota ya fue facturada ", MsgBoxStyle.Information, "Facturación.. ")
                End If
            Else
                MsgBox("Aun no se carga el día ", MsgBoxStyle.Information, "Facturación.. ")
            End If
        Else
            MsgBox("Aun no carga los dias de ventas ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub InicializaGrid()
        Ugr.DataSource = Nothing
        objpro.LLenaGrid(Ugr, "select '' as codsat, '' as vdclvUni, vdClaPro as ClaveInterno, vdDescri as Descripcion, " &
                                  " vdCant as Cantidad, '' as Unidad, vdPreUni as PrecioUnitario, vdIva as Iva, " &
                                   " vdDesc1 as Descu, vdPreTot as Importe from ventadet " &
                                   " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " &
                                   " vdterm = 0 and " &
                                   " day(vdFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and " &
                                   " year(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", "ventadet", Motor)
        Ugr.DataBind()
    End Sub



    Private Sub CargaGridInicio()
        Application.DoEvents()
        DatGrid = objpro.ObtenDatos(" select vdcodsat as codsat, vdclvuni as vdclvUni, vdClaPro as ClaveInterno, vdDescri as Descripcion, " &
                                    " vdCant as Cantidad, vdPreUni as PrecioUnitario, vdIva as Iva, " &
                                    " vdDesc1 as Descu, vdPreTot as Importe from ventadet " &
                                    " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " &
                                    " vdterm = " & Terminal & " and " &
                                    " day(vdFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and " &
                                    " year(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor).Tables(0)

        If DatGrid.Rows.Count <> 0 Then
            Me.Importe.Value = 0
            upgb1.Value = 0
            upgb1.Minimum = 0
            upgb1.Maximum = DatGrid.Rows.Count
            For Each r In DatGrid.Rows
                Application.DoEvents()
                Dim pu1 As Double = CDbl(r("PrecioUnitario").ToString)
                Dim PU As Double = Format(pu1, "###,###.00")
                Dim descu As Double = Format(CDbl(r("Descu").ToString), "###,###.00")
                Dim iva1 As Double = Format(CDbl(r("Iva").ToString), "###,###.00")

                'Dim impt As Double = ((PU * CDbl(r("cantidad").ToString)) - descu)

                'Dim codsat As String
                'If r("codsat").ToString = "" Then
                '    codsat = "01010101"
                'Else
                '    codsat = r("codsat").ToString
                'End If
                'Dim Borrar As String = Trim(RTrim(LTrim(r("Descripcion").ToString)))
                'Borrar = Borrar.Trim
                'While Borrar.Contains("  ")
                '    Borrar = Borrar.Replace("  ", " ")
                'End While

                'FormatoGrid(codsat, r("vdclvUni").ToString, r("ClaveInterno").ToString, Borrar,
                '            r("Cantidad").ToString, "PZA", PU, iva1,
                '            descu, r("Importe").ToString)
                Me.Importe.Value += (r("Importe").ToString)
                upgb1.Value += 1
                Application.DoEvents()
                'Console.WriteLine(descu.ToString)
                Me.Descuento.Value += descu
                Me.Subtotal.Value = (Importe.Value - Descuento.Value)
                Me.Iva.Value += iva1
                Me.Total.Value = (Subtotal.Value + Iva.Value)

            Next
            'Console.WriteLine("checa montos")
        Else
            MsgBox("No se encontró información.. ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub FormatoGrid(codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String,
                               Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double,
                               Descu As Double, Importe As Double)
        Ugr.Rows.Band.AddNew()
        ColocarArticulo(Ugr.Rows.Count - 1, codSat, cvlUni, ClaveInterno, Descripcion, Cantidad, Unidad, PrecioUnitario, Iva, Descu, Importe)
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String,
                               Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double,
                               Descu As Double, Importe As Double)
        With Ugr
            .Rows(Renglon).Cells("CodSat").Value = codSat
            .Rows(Renglon).Cells("vdclvUni").Value = cvlUni
            .Rows(Renglon).Cells("ClaveInterno").Value = ClaveInterno
            .Rows(Renglon).Cells("Descripcion").Value = Descripcion
            .Rows(Renglon).Cells("Cantidad").Value = Cantidad
            .Rows(Renglon).Cells("Unidad").Value = Unidad
            .Rows(Renglon).Cells("PrecioUnitario").Value = PrecioUnitario
            .Rows(Renglon).Cells("Iva").Value = Iva
            .Rows(Renglon).Cells("Descu").Value = Descu
            .Rows(Renglon).Cells("Importe").Value = Importe
        End With
    End Sub

    Private Sub btnGenerarFactura_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        btnGenerarFactura.Visible = False
        Factura()
        consulta_registros()
    End Sub

    Public Function Factura()
        'If Me.Ugr.Rows.Count <> 0 Then
        td = 1
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
            Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)

            Dim f1, f2, f3, freal, FeReg, hr As String
            f1 = txtFechaVenta.Text.ToString.Substring(0, 2)
            f2 = txtFechaVenta.Text.ToString.Substring(3, 2)
            f3 = txtFechaVenta.Text.ToString.Substring(6, 4)
            hr = Now.ToLocalTime.ToString.Substring(11, 8)

            FeReg = txtFechaVenta.Text.ToString.Substring(6, 4) & f2 & f1

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

            Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

            Dim TipoCambio As Int16 = 1
            Dim CondicionesDePago As String = "CONTADO"

            fp = "EFECTIVO" ' "99 - Otros"
            upb.Value += 10

            'EXPORTACION: 01 - NO APLICA
            EasyOne_cfdi33_SetCfdi40(p, "01", "")

            Fila("exporta") = "01-No Aplica"
            ''CARTA PORTE

            'EasyOne_cfdi33_SetCfdiRelacionado(p, "01")

            'EasyOne_cfdi33_AddCfdiRelacionado(p, "832AB0D1-F482-44B7-975F-C565C21C983B")

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
                Case 607
                    TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                    DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
                Case 621
                    TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                    DesRegimen = "Incorporacion Fiscal"
            End Select

            ''/ Periodicidad 01 Diario| 02 Semanal | 03 Quincenal | 04 Mensual | 05 Bimensual
            ''// Meses 01-12 Enero a Diciembre | 13-18 Enero-Febrero a Noviembre-Diciembre  // Año
            EasyOne_cfdi33_SetInformacionGlobal40(p, "01", f2, f3)

            'Llenamos la sección del Emisor
            'EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, "X")
            EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TpRegimen)

            'Llenamos la sección del Receptor
            'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
            EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
            EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616

            Cadena = "Suc: " & Trim(CarpSucursal) & " Fecha Venta: " & txtFechaVenta.Value.ToString.Substring(0, 10) & " Nota: " & txtFactura & " - Importe - " & " $" & Str(FormatCurrency(Me.Total.Value))

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
            Dim ipt As String = ""

            Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
            Tabla2.Columns.Add("Cantidad")
            Tabla2.Columns.Add("Unidad")
            Tabla2.Columns.Add("UniSAT")
            Tabla2.Columns.Add("Descrip")
            Tabla2.Columns.Add("Punitario")
            Tabla2.Columns.Add("Importe")
            Dim Fila2 As DataRow

            For Each r In DatGrid.Rows

                Dim pun As Double = Format(CDbl(r("PrecioUnitario").ToString), "###,###.00")
                NumPiezas = (r("Cantidad").ToString)
                sb += pun * NumPiezas
                If Format(CDbl(r("Importe").ToString), "###,###.00") - Format(CDbl(r("Descu").ToString), "###,###.00") <> 0 Then
                    Descu += Format(CDbl(r("Descu").ToString), "######.00")
                Else
                    Descu += Format(CDbl(r("Descu").ToString) - 0.1, "######.00")
                End If
            Next

            If (Format(sb, "######.00") - Format(Descu, "######.00")) <> 0 Then
                If Descu <> 0 Then
                    EasyOne_cfdi33_AddConcepto(p, "01010101", "", "1", "ACT", "", "Venta", Format((sb), "######.00"), Format(sb, "######.00"), Format((Descu), "######.00"))
                Else
                    EasyOne_cfdi33_AddConcepto(p, "01010101", "", "1", "ACT", "", "Venta", Format((sb), "######.00"), Format(sb, "######.00"), "")
                End If
                EasyOne_cfdi33_SetConcepto40(p, "02")
                Imp = Format(((sb - Descu) * 0.16), "######.00")
                bas = Format((sb - Descu), "######.00")
                iv += Imp
                EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))

            Else
                If Descu <> 0 Then
                    EasyOne_cfdi33_AddConcepto(p, "01010101", "", "1", "ACT", "", "Venta", Format((sb), "######.00"), Format(sb, "######.00"), Format((Descu), "######.00"))
                Else
                    EasyOne_cfdi33_AddConcepto(p, "01010101", "", "1", "ACT", "", "Venta", Format((sb), "######.00"), Format(sb, "######.00"), "")
                End If
                EasyOne_cfdi33_SetConcepto40(p, "02")
                Imp = Format(((sb - Descu) * 0.16), "######.00")
                iv += Imp
                bas = Format((sb - Descu), "######.00")

                EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))
            End If

            Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
            Fila2("Cantidad") = "1"
            Fila2("Unidad") = "ACT"
            Fila2("UniSAT") = Format(Descu, "###,###.00")
            Fila2("Descrip") = "VENTA"
            Fila2("Punitario") = Format(sb, "###,###.00")
            Fila2("Importe") = Format(sb, "###,###.00")
            DS.Tables(1).Rows.Add(Fila2)

            ''-----CARTA PORTE -------
            ''Dim c As Integer = EasyOne_cartaporte20_Create()
            ''EasyOne_cartaporte20_SetCartaPorte(c, "No", "", "", "", "2000.00")
            ''EasyOne_cartaporte20_AddUbicacion(c, "Origen", "", "SED1102088J7 ", "", "", "", "", "", "", "2022-06-10T13:45:38", "", "")
            ''EasyOne_cartaporte20_SetUbicacionDomicilio(c, "", "", "", "", "", "", "", "OAX", "MEX", "70390")
            ''EasyOne_cartaporte20_AddUbicacion(c, "Destino", "", "SED1102088J7 ", "", "", "", "", "", "", "2022-06-14T00:00:00", "", "2000.00")
            ''EasyOne_cartaporte20_SetUbicacionDomicilio(c, "", "", "", "", "", "", "", "CMX", "MEX", "07700")
            ''EasyOne_cartaporte20_AddMercancia(c, "44121504", "", "Sobres de ventana", "7", "H87", "", "", "", "", "", "", "8", "0.00", "MXN", "", "")
            ''EasyOne_cartaporte20_SetMercancias(c, "8.00", "XBB", "", "1", "")
            ''EasyOne_cartaporte20_SetAutotransporte(c, "TPAF10", "1234554321")
            ''EasyOne_cartaporte20_SetAutotransporteIdentificacionVehicular(c, "C2R2", "3674ZP", "2005")
            ''EasyOne_cartaporte20_SetAutotransporteSeguros(c, "QUALITAS", "592-06-000-12345", "", "", "", "", "")
            ''EasyOne_cartaporte20_AddAutotransporteRemolque(c, "CTR003", "555ABC")
            ''EasyOne_cartaporte20_AddFiguraTransporte(c, "01", "SED1102088J7", "Prueba", "", "", "")
            ''EasyOne_cartaporte20_SetFiguraTransporteDomicilio(c, "", "", "", "", "", "", "", "NLE", "MEX", "67178 ")

            Dim ValorTicket As String = Cadena
            NoCuenta = ""

            Dim TotalImpuestosTrasladados As Double = 0
            TotalImpuestosTrasladados = Format(iv, "######.00")

            ''PARA CARTA ´PORTE
            ''EasyOne_AddComplemento(p, c)

            'Agregamos los Impuestos
            EasyOne_cfdi33_SetComprobanteImpuestos(p, "", TotalImpuestosTrasladados)
            EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.160000", Format(iv, "######.00"))
            Dim Total As Double = 0
            Dim Subt As Double = 0

            'Subt = Format(Me.Subtotal.ToString, "###,###.00")
            Subt = Format(sb, "######.00")
            If iv > 0 Then
                Total = Format(((Subt - Convert.ToDecimal(Descu)) + iv), "######.00")
                Fila("Fsubtotal") = Format(Subt, "###,###.00")
                Fila("Fdescuento") = Format(Descu, "###,###.00")
                Fila("Fiva") = Format(iv, "###,###.00")
                Fila("Ftotal") = Format(Total, "###,###.00")
            End If
            Dim cantletra As String
            cantletra = Letras(Total)
            Fila("cantidadl") = cantletra
            Dim metpg As String
            Dim tipcom As String
            Dim formpg As String
            metpg = "PUE"
            tipcom = "I"
            formpg = "01" '"99"

            Dim Descue As String = ""
            Dim desc2 As String
            Descue = Format(Descu, "###,###.00")
            desc2 = Descue.Replace(",", "")
            freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
            'freal = "2023-01-03T" & hr
            'Llenamos la sección del comprobante
            If desc2 = ".00" Or desc2 = "" Then
                EasyOne_cfdi33_SetComprobante(p, txtSerie, txtFactura, freal, formpg, "", Format((Subt), "######.00"), "", "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
            Else
                EasyOne_cfdi33_SetComprobante(p, txtSerie, txtFactura, freal, formpg, "", Format((Subt), "######.00"), Format((Descu), "######.00"), "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
            End If

            Fila("metpago") = metpg
            If tipcom = "I" Then
                Fila("tipocomp") = "I - Ingreso"
            Else
                Fila("tipocomp") = "E - Egreso"
            End If
            Fila("formpago") = fp

            'Enviamos el CFDI al metodo de PRUEBAS
            'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
            'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL")
            EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")
            Application.DoEvents()
            upb.Value += 30  'Suma 40

            'En la siguiente línea se deben colocar los nombres de los archivos de certificado de sellos del Emisor.
            'Para fines de esta prueba se usarán los certificados de sellos del Emisor AAA010101AAA.
            'Favor de colocar estos archivos (.CER y .KEY) en la misma carpeta donde se encuentra la aplicación.
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
                Dim resultadoXML As String = [String].Empty
                resultadoXML = EasyOne_LeeResultado(p, 8)
                'Obtenemos el XML completo y timbrado
                Console.WriteLine(resultadoXML)

                Application.DoEvents()
                upb.Value += 20 'Suma 60
                Dim archivoXMLPDF As String = ""
                Dim archivoREPX As String = ""
                Dim archivoPDF As String = ""

                IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml"), resultadoXML)

                '''''''''''''''''''''''''''''' RECORRO XML''''''''''''''''''''''''''''
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
                documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml"))
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
                    RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".xml.png")

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Dim Estatus As String
                    Dim ExisteClave As String = 10
                    Dim fecha1, fecha2, fecha3, fechareal, FeRegistro As String
                    fecha1 = txtFechaVenta.Text.ToString.Substring(0, 2)
                    fecha2 = txtFechaVenta.Text.ToString.Substring(3, 2)
                    fecha3 = txtFechaVenta.Text.ToString.Substring(6, 4)
                    fechareal = Now.Date.ToString.Substring(0, 2) & "/" & Now.Date.ToString.Substring(3, 2) & "/" & fecha3
                    FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & fecha2 & fecha1
                    Dim FeFecCan As Date = "01/01/1900"
                    Dim FeMotcan As String = ""
                    Dim Tipo As String = "F"

                    Try
                        objpro.Indicacion("Update VentaEnc Set veEstatu =1, veFac = '" & CInt(txtFactura) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & td & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "", Motor)
                    Catch ex As Exception
                        MsgBox("Actualiza VentaEnc " & ex.ToString, MsgBoxStyle.Information, "Factura Electronica")
                    End Try

                    Try
                        objpro.Indicacion("Update Serie set seCant = " & CInt(txtFactura) & " Where seSerie = '" & txtSerie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                    Catch ex As Exception
                        MsgBox("Actualiza Serie " & ex.ToString, MsgBoxStyle.Information, "Factura Electronica")
                    End Try

                    upb.Value += 10 'Suma 70

                    Dim r_sub As Double = Subtotal.Value
                    Dim r_des As Double = Descuento.Value
                    Dim r_iva As Double = Iva.Value
                    Dim r_total As Double = Total
                    'Dim terminal As String = Me.grbTienda.Value & Me.grbSucursal.Value

                    Estatus = 1

                    objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha," &
                " feHora,Cid,FeSubt,fePorIva,FeIva, " &
                " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                " FeCanlet,FeAbonos,FeEstatu,FeReg, FeFecCan,FeMotcan, feTip,feuuid) " &
                " values (" & CInt(Transaccion) & "," & grbTienda.Value & "," & Me.grbTienda.Value & Me.grbSucursal.Value & "," & CInt(txtFactura) & ",'" & txtSerie & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," &
                " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ",'" & r_sub & "', '" & CDbl(PorIVa) & "','" & r_iva & "'," &
                " '" & r_des & "','" & r_des & "', '" & r_des & "', '" & r_des & "', '" & r_total & "', " &
                " '" & cantletra & "', '" & r_total & "'," & Estatus & "," & FeRegistro & ",  " & FeFecCan & ", '" & FeMotcan & "', '" & Tipo & "','" & uuid & "')", Motor)

                    ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

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

                    upb.Value += 10 'Suma 80
                    Dim Ruta As String = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & ".pdf")
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
                        upb.Value += 10 'Suma 90  
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try

                    Application.DoEvents()
                    upb.Value += 10 'Suma 100

                    Dim dev As Double = 0
                    If Devoluciones.Value <> 0 Then
                        Application.DoEvents()
                        dev = Devoluciones.Value
                        NotaCredito(freal, TRfc, TRFiscal, Directorios, CerFactura, Key, Pwd, uuid, TCp)
                    End If

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
                btnGenerarFactura.Visible = True
            End If
        Else
            btnGenerarFactura.Visible = True
        End If 'Cierra validación en la pregunta del msgbox
        'Else
        '    MsgBox("No hay registros para facturar.. ", MsgBoxStyle.Information, "Facturación")
        'End If 'Cierra validación de registros en el Grid
        Return ""
    End Function

    Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

    Public Function NotaCredito(ByVal fechaNC As String, ByVal n_TRfc As String, ByVal n_TRFiscal As String, ByVal n_Directorios As String, ByVal cer As String, ByVal key As String,
                                ByVal psw As String, ByVal n_uuid As String, n_LExpedicion As String)
        Dim n_serie As String
        Dim n_notacredito As String
        DesRegimen = ""
        NumPiezas = 0
        td = 2

        upb.Value = 0
        upb.Maximum = 100

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
        Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")

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
        metpg = "PUE"
        tipcom = "E"
        formpg = "01"

        'Dim sbt As Double = 0
        Dim Pu As Double = Format((Me.Devoluciones.Value / 1.16), "###,###.00")
        'sbt = Format(Pu, "###,###.00")

        Fila("Ntipocomp") = "EGRESO"
        Fila("Nmetpag") = metpg
        Fila("Nformpag") = "EFECTIVO"


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
            Case 616
                TpRegimen = c_RegimenFiscal.SinObligacionesFiscales
                DesRegimen = "Sin Obligaciones Fiscales"
            Case 607
                TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
        End Select


        'Llenamos la sección del Emisor
        EasyOne_cfdi33_SetEmisor(pN, n_TRfc, TRazonSocial, TpRegimen)

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

        EasyOne_cfdi33_AddConcepto(pN, "84111506", "100", "1", "ACT", "SERVICIO", descrip.Trim, Pu, Pu, "")
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

        If Iva.Value <> 0 Then
            bas = Format(Pu, "###,###.00")
            Imp = Format((Pu * 0.16), "###,###.00")
            iv += Imp
            Impu = "002"
            ToC = "0.160000"
            TFac = "Tasa"
            EasyOne_cfdi33_AddTraslado(pN, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
        End If

        Dim total As Double = 0
        total = Format((Pu + iv), "###,###.00")

        'Llenamos la sección del comprobante (2)
        EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, fechaNC, "01", "", Pu.ToString, "", "MXN", TipoCambio, total.ToString, tipcom, "PUE", n_LExpedicion, "")

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

        'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, "RHUUMRGL", "RHUUMRGL")
        EasyOne_SetInvoiceOneFachadaInfo(pN, "RHUUMRGL", "RHUUMRGL")

        'Timbra XML, que se encuentra en variable P
        Dim nc As Integer = 99
        If va2 = 0 Then
            nc = EasyOne_FirmaComprobante(pN, cer, key, psw)
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

        'Obtención del resultado de la solicitud
        If nc = 0 And va2 = 1 Then
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

                'RutaLogoN = RutFac & "\RecursosCFD33\Penguin.bmp"
                RutaQRN = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml.png")

                '''''''''''''''''''''''''''''''''''''''''''''''''''
                objpro.Indicacion("Update VentaEnc set veEstatu = 1, veFac = '" & CInt(n_notacredito) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & td & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "", Motor)
                objpro.Indicacion("Update Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
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

                Dim Ruta As String = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".pdf")
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
            MsgBox("ERROR AL GENERAR LA NOTA DE CREDITO 4.0")
        End If
        Return ""
    End Function

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

End Class