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

    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda, datosCliente As Data.DataTable
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
        txtImporte.Text = "$$"
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
                txtImporte.Text = Format(Ugr.ActiveRow.Cells("Total").Value, "###,###.00")
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


            upb.Value = 0
            upb.Maximum = 100


            upb.Value += 50
            objpro.Indicacion("update fencabez set feestatu = 0, fefeccan = '" & fechareal & "', femotcan = '" & vemotcan & "' " &
                                      " where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and feid = " & Transaccion & " and feterm = " & Terminal & " and " &
                                      " fefact = " & txtId.Text & " and feserie = '" & grbTipoFactura.Value & "' ", Motor)
            upb.Value += 50
        End If
    End Sub


End Class