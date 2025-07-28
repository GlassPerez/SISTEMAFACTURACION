Public Class frmBoscoReimpresion


#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Variables"
    Dim AlgunaVez As Boolean = False
    Dim Algunavez2 As Boolean = False
    Dim TId, SId As Integer
    Dim TIP, FechaBusqueda As String
    Dim Mes As Integer = 0
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket As Data.DataTable
    Dim ExpCalle, ExpNoExterior, ExpNoInterior, ExpColonia, ExpEstado, ExpMunicipio, ExpLocalidad, ExpCp, ExpPais As String
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

    Private Sub frmBoscoReimpresion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Empresa FROM BGXFacElec.CatTien Where ttipo3 = 'G'", 0, 1, As400)
        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year

        txtFechaFactura.Value = Now.Date
    End Sub

    Private Sub frmBoscoReimpresion_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
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

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        grbSucursal.Focus()
    End Sub

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        REM objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        objpro.CargaCombos(grbTipoFactura, "select seserie, sedesc from bgxfacelec.serie where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", 0, 1, As400)
        grbTipoFactura.Focus()
        Ugr.DataSource = Nothing
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        CargaGrid()
    End Sub

    Private Sub CargaGrid()
        Try
            Dim fecini, fecfin As String
            FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
            fecini = FechaBusqueda & "01"
            fecfin = FechaBusqueda & DateTime.DaysInMonth(grbPeriodo.Text, objfun.EscojeMes(Me.grbMes.Text))
            objpro.LLenaGrid(Ugr, "SELECT FEncabezado.FeFact as Factura,FEncabezado.fefecha as Fecha,FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado,FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,Clientes.cemail as Email,FEncabezado.FeSubt as Subtotal, FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total,FEncabezado.tid " & _
                             " FROM bgxfacelec.FEncabez as FEncabezado left join bgxfacelec.CatClie as Clientes on FEncabezado.Cid = Clientes.Cid  " & _
                             " Where FEncabezado.FeReg >= " & fecini & " and  FEncabezado.FeReg <= " & fecfin & " and  FEncabezado.Tid = " & Me.grbTienda.Value & " and FEncabezado.Sid = " & grbSucursal.Value & " and FEncabezado.FeSerie = '" & grbTipoFactura.Value & "' order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", As400)
            Me.Ugr.DataBind()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 50
            .DisplayLayout.Bands(0).Columns(1).Width = 100
            .DisplayLayout.Bands(0).Columns(2).Width = 55
            .DisplayLayout.Bands(0).Columns(3).Width = 65
            .DisplayLayout.Bands(0).Columns(4).Hidden = True
            .DisplayLayout.Bands(0).Columns(5).Width = 70
            .DisplayLayout.Bands(0).Columns(6).Width = 100
            .DisplayLayout.Bands(0).Columns(7).Width = 450
            .DisplayLayout.Bands(0).Columns(8).Width = 250
            .DisplayLayout.Bands(0).Columns(9).Width = 100
            .DisplayLayout.Bands(0).Columns(10).Width = 100
            .DisplayLayout.Bands(0).Columns(11).Width = 100
            .DisplayLayout.Bands(0).Columns(12).Width = 100
            .DisplayLayout.Bands(0).Columns(13).Hidden = True
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        DatosEnvio("")
        Try
            txtFactura.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
            txtSerie.Text = Ugr.ActiveRow.Cells("Serie").Value.ToString
            txtFechaFactura.Value = Ugr.ActiveRow.Cells("Fecha").Value.ToString

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub DatosEnvio(ByVal comodin As String)
        txtFactura.Text = comodin
        txtSerie.Text = comodin    
    End Sub

    Private Sub btnReImpresion_Click(sender As Object, e As EventArgs) Handles btnReImpresion.Click
        If Me.txtFactura.Text <> "" Then
            TablaTienda = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", As400).Tables(0)
            Dim TRfc = Me.TablaTienda.Rows(0).Item("trfc").ToString
            Dim Directorios As String = Me.TablaTienda.Rows(0).Item("sUbic").ToString

            Dim rutaArchivoPDF As String = Directorios & "\" & Me.grbPeriodo.Text & "\" & objfun.EscojeMes(Me.grbMes.Text) & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & ".pdf"

            Dim loPSI As New ProcessStartInfo
            Dim loProceso As New Process
            loPSI.FileName = rutaArchivoPDF '"PATH DE TU FICHERO CON EXTENSION"
            Try
                loProceso = Process.Start(loPSI)
            Catch Exp As Exception
                MessageBox.Show(Exp.Message, "XXXX", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try

            DatosEnvio("")
        Else
            MsgBox("A un no selecciona la factura a Re-Imprimir ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub grbTipoFactura_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTipoFactura.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTipoFactura
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub
End Class