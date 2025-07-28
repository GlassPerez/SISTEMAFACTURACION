Public Class frmComplementoPagoBoutique

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region


#Region "Variables"
    Dim FechaBusqueda As String
    Dim Tabla, TablaTienda As DataTable
    Dim FechaInicio, FechaFin As String
    Dim FechaBus As String
    Dim Mes As Integer = 0
#End Region

    Private Sub frmComplementoPagoBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien ", 0, 1, Motor)

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        With Me.grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            .DisplayLayout.Bands(0).Columns(1).Width = 250
        End With
    End Sub

    Private Sub grbTienda_TextChanged(sender As Object, e As EventArgs) Handles grbTienda.TextChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        grbSucursal.Focus()
        grbSucursal.Select()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If grbTienda.Text <> "" Then
            If grbSucursal.Text <> "" Then
                Inicializa_Totales()
                CargaGrid()
            Else
                MsgBox("Falta sucursal por seleccionar .. ", MsgBoxStyle.Information, "Facturacion")
            End If
        Else
            MsgBox("Falta empresa por seleccionar .. ", MsgBoxStyle.Information, "Facturacion")
        End If
    End Sub

    Private Sub Inicializa_Totales()
        txtFactura.Text = ""
        txtSerie.Text = ""
        txtFechaFactura.Value = ""
        txtUUID.Text = ""
        Total.Value = 0.0
        Abono.Value = 0.0
        Saldo.Value = 0.0
    End Sub

    Private Sub CargaGrid()
        Try
            FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
            FechaInicio = FechaBusqueda & "01"
            Dim bf = DateSerial(grbPeriodo.Text, objfun.EscojeMes(Me.grbMes.Text) + 1, 0)
            FechaFin = FechaBusqueda & bf.ToString.Substring(0, 2)
            objpro.LLenaGrid(Ugr, "SELECT feId as Id,  feuuid AS uuid, FEncabezado.FeFact as factura,FEncabezado.fefecha as fecha,FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado, " & _
                             " FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,FEncabezado.FeSubt as Subtotal, " & _
                             " FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total,FEncabezado.tid, FEncabezado.feabonos as Abono FROM FEncabez as FEncabezado " & _
                             " left join CatClie as Clientes on FEncabezado.Cid = Clientes.Cid " & _
                             " Where (FEncabezado.FeReg >= " & FechaInicio & " and FEncabezado.FeReg <= " & FechaFin & ") and FEncabezado.Tid = " & Me.grbTienda.Value & " and " & _
                             " FEncabezado.Sid = " & grbSucursal.Value & " order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", Motor)
            Me.Ugr.DataBind()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturacion .. ")
        End Try
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            .DisplayLayout.Bands(0).Columns(1).Width = 180
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

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Pagos_Datos()
        Try
            P_Id = Ugr.ActiveRow.Cells("Id").Value
            P_Tid = grbTienda.Value
            P_Suc = grbSucursal.Value
            P_Term = Ugr.ActiveRow.Cells("Terminal").Value
            P_Fact = Ugr.ActiveRow.Cells("Factura").Value
            P_Serie = Ugr.ActiveRow.Cells("Serie").Value.ToString

            txtFactura.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
            txtSerie.Text = Ugr.ActiveRow.Cells("Serie").Value.ToString
            txtFechaFactura.Value = Ugr.ActiveRow.Cells("Fecha").Value.ToString
            txtUUID.Text = Ugr.ActiveRow.Cells("UUID").Value.ToString
            Total.Value = Ugr.ActiveRow.Cells("Total").Value.ToString
            Abono.Value = Ugr.ActiveRow.Cells("Abono").Value.ToString
            Saldo.Value = (Total.Value - Abono.Value)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub Pagos_Datos()
        P_Id = 0
        P_Tid = 0
        P_Suc = 0
        P_Term = 0
        P_Fact = 0
        P_Serie = 0
    End Sub

    Private Sub btnAplicarPago_Click(sender As Object, e As EventArgs) Handles btnAplicarPago.Click
        If txtUUID.Text <> "" Then
            If Saldo.Value <> 0 Then
                Formulario(frmPagosBoutique)
                Pagos_Datos()
            Else
                MsgBox("La factura ya se cubrio en su totalidad ", MsgBoxStyle.Information, "Facturación ..")
            End If
        Else
            MsgBox("No hay UUID relacionado para el pago ", MsgBoxStyle.Information, "Facturación ..")
        End If
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        btnBuscar.Focus()
    End Sub

    Private Sub frmComplementoPagoBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub frmComplementoPagoBoutique_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub
End Class