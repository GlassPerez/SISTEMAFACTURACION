Imports N400
Imports N400.Componentes.CFDI
Imports N400.Componentes.CFDI.PDF
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Threading

Public Class frmComplementoCafeteria
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
    'Dim FechaBusqueda As String
    'Dim Tabla, TablaTienda, DatGrid As DataTable
    'Dim FechaInicio, FechaFin As String
    'Dim FechaBus As String
    'Dim Mes As Integer = 0
    Dim tablaFacturas As Data.DataTable
    Dim va As Integer
    Dim hra As String
    Dim seriee, folio As String
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

    Private Sub frmComplementoCafeteria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TIP = "PA"
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
            objpro.LLenaGrid(Ugr, "SELECT feid as Id, feuuid AS uuid, FEncabezado.FeFact as factura,FEncabezado.feReg as fechaVenta,FEncabezado.FeSerie as Serie, FEncabezado.FeTotal as Total, FEncabezado.feabonos as Abono, FEncabezado.FeEstatu as Estado, " &
                             " FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,FEncabezado.FeSubt as Subtotal, " &
                             " FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.tid FROM FEncabez as FEncabezado " &
                             " left join CatClie as Clientes on FEncabezado.Cid = Clientes.Cid " &
                             " Where (FEncabezado.FeReg >= " & FechaInicio & " and FEncabezado.FeReg <= " & FechaFin & ") and FEncabezado.fformpag = '99' and FEncabezado.fetip <> 'P' and FEncabezado.Tid = " & Me.grbTienda.Value & " and " &
                             " FEncabezado.Sid = " & grbSucursal.Value & " and FEncabezado.FeEstatu = 1 order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", Motor)
            Me.Ugr.DataBind()

            For i = 0 To Me.Ugr.Rows.Count - 1
                If Ugr.Rows(i).Cells("Abono").Value <> 0 Then
                    Ugr.Rows(i).Appearance.BackColor = Color.GreenYellow

                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturacion .. ")
        End Try
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Hidden = False
            .DisplayLayout.Bands(0).Columns(0).Width = 5
            .DisplayLayout.Bands(0).Columns(1).Width = 300
            .DisplayLayout.Bands(0).Columns(2).Width = 60
            .DisplayLayout.Bands(0).Columns(3).Width = 80
            .DisplayLayout.Bands(0).Columns(4).Width = 50
            .DisplayLayout.Bands(0).Columns(5).Width = 80
            .DisplayLayout.Bands(0).Columns(6).Width = 80
            .DisplayLayout.Bands(0).Columns(7).Width = 40
            .DisplayLayout.Bands(0).Columns(8).Width = 40
            .DisplayLayout.Bands(0).Columns(9).Width = 40
            .DisplayLayout.Bands(0).Columns(10).Width = 100
            .DisplayLayout.Bands(0).Columns(11).Width = 250
            .DisplayLayout.Bands(0).Columns(12).Width = 80
            .DisplayLayout.Bands(0).Columns(13).Width = 80
            .DisplayLayout.Bands(0).Columns(14).Width = 80
            .DisplayLayout.Bands(0).Columns(15).Hidden = True
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Pagos_Datos()
        Try

            If Ugr.ActiveRow.Cells("Abono").Value = 0 Then

                P_Id = Ugr.ActiveRow.Cells("Id").Value
                P_Tid = grbTienda.Value
                P_Suc = grbSucursal.Value
                P_Term = Ugr.ActiveRow.Cells("Terminal").Value
                P_Fact = Ugr.ActiveRow.Cells("Factura").Value
                P_Serie = Ugr.ActiveRow.Cells("Serie").Value.ToString

                txtFactura.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
                txtSerie.Text = Ugr.ActiveRow.Cells("Serie").Value.ToString
                Dim ventaf, fevta As String
                ventaf = Ugr.ActiveRow.Cells("FechaVenta").Value.ToString
                fevta = ventaf.Substring(6, 2) & "/" & ventaf.Substring(4, 2) & "/" & ventaf.Substring(0, 4)
                txtFechaFactura.Value = CDate(fevta)
                txtUUID.Text = Ugr.ActiveRow.Cells("UUID").Value.ToString
                Total.Value = Ugr.ActiveRow.Cells("Total").Value.ToString
                Abono.Value = Ugr.ActiveRow.Cells("Abono").Value.ToString
                Saldo.Value = Format((Total.Value - Abono.Value), "######.00")
            Else
                MsgBox("Esta factura ya ha sido cubierta en su totalidad ", MsgBoxStyle.Information, "Facturación.. ")
            End If

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
                Formulario(frmPagosCoffe)
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

    Private Sub frmComplementoCafeteria_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub frmComplementoCafeteria_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub
End Class