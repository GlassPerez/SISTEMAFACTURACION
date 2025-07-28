Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class frmBoutiqueReimpresion


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
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket, TF As Data.DataTable
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

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        REM objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        objpro.CargaCombos(grbTipoFactura, "select setipo, sedesc from serie where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", 0, 1, Motor)
        grbTipoFactura.Focus()
        Ugr.DataSource = Nothing
    End Sub

    Private Sub frmBoutiqueReimpresion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where Tid = " & Tienda & "", 0, 1, Motor)
        Me.grbTienda.Value = Tienda
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Value = Sucursal

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year

        txtFechaFactura.Value = Now.Date

    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        CargaGrid()
    End Sub

    Private Sub CargaGrid()
        Try
            FechaBusqueda = Me.grbPeriodo.Text & objfun.EscojeMes(Me.grbMes.Text)
            objpro.LLenaGrid(Ugr, "SELECT FEncabezado.FeFact as Factura,FEncabezado.fefecha as FechaTIM, FEncabezado.fereg as FechaVTA, FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado,FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,Clientes.cemail as Email,FEncabezado.FeSubt as Subtotal, FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total,FEncabezado.tid FROM FEncabez as FEncabezado left join CatClie as Clientes on FEncabezado.Cid = Clientes.Cid  Where mid(FEncabezado.FeReg,1,6) = " & FechaBusqueda & " and FEncabezado.Tid = " & Me.grbTienda.Value & " and FEncabezado.Sid = " & grbSucursal.Value & " order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", Motor) 'and FEncabezado.feSerie = '" & grbTipoFactura.Value & "'
            Me.Ugr.DataBind()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 50
            .DisplayLayout.Bands(0).Columns(1).Width = 100
            .DisplayLayout.Bands(0).Columns(2).Width = 100
            .DisplayLayout.Bands(0).Columns(3).Width = 55
            .DisplayLayout.Bands(0).Columns(4).Width = 65
            .DisplayLayout.Bands(0).Columns(5).Width = 70
            .DisplayLayout.Bands(0).Columns(6).Hidden = True
            .DisplayLayout.Bands(0).Columns(7).Width = 100
            .DisplayLayout.Bands(0).Columns(8).Width = 450
            .DisplayLayout.Bands(0).Columns(9).Width = 250
            .DisplayLayout.Bands(0).Columns(10).Width = 100
            .DisplayLayout.Bands(0).Columns(11).Width = 100
            .DisplayLayout.Bands(0).Columns(12).Width = 100
            .DisplayLayout.Bands(0).Columns(13).Width = 100
            .DisplayLayout.Bands(0).Columns(14).Hidden = True
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        DatosEnvio("")
        Try
            txtFactura.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
            txtSerie.Text = Ugr.ActiveRow.Cells("Serie").Value.ToString
            txtFechaFactura.Value = Ugr.ActiveRow.Cells("FechaTIM").Value.ToString
            txtfecvta.Text = Ugr.ActiveRow.Cells("FechaVTA").Value.ToString
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "FACTURAS-E")
        End Try
    End Sub

    Private Sub DatosEnvio(ByVal comodin As String)
        txtFactura.Text = comodin
        txtSerie.Text = comodin
        txtfecvta.Text = comodin
    End Sub

    Private Sub btnReImpresion_Click(sender As Object, e As EventArgs) Handles btnReImpresion.Click
        If Me.txtFactura.Text <> "" Then
            TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
            Dim TRfc = Me.TablaTienda.Rows(0).Item("trfc").ToString
            Dim Directorios As String = Me.TablaTienda.Rows(0).Item("sUbic").ToString
            Dim mfac As String = txtFechaFactura.Value.ToString.Substring(3, 2) ''mes de factura para saber en que carpeta se guardo
            Dim rutaArchivoPDF As String = ""
            If grbTipoFactura.Value = "VG" Then
                rutaArchivoPDF = Directorios & "\" & Me.grbPeriodo.Text & "\" & mfac & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & "-" & txtfecvta.Text & ".pdf"
            Else
                rutaArchivoPDF = Directorios & "\" & Me.grbPeriodo.Text & "\" & objfun.EscojeMes(Me.grbMes.Text) & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & ".pdf"
            End If

            'TF = objpro.ObtenDatos("select seserie, sedesc from serie where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " AND setipo = 'NC' ", Motor).Tables(0)
            'Dim serie = Me.TF.Rows(0).Item("seserie").ToString

            'If grbTipoFactura.Value = "NC" Then

            '    rutaArchivoPDF = Directorios & "\" & Me.grbPeriodo.Text & "\" & mfac & "\" & Trim(TRfc) & "-" & serie & "-" & txtFactura.Text & "-" & txtfecvta.Text & ".pdf"
            'Else
            '    rutaArchivoPDF = Directorios & "\" & Me.grbPeriodo.Text & "\" & objfun.EscojeMes(Me.grbMes.Text) & "\" & Trim(TRfc) & "-" & serie & "-" & txtFactura.Text & ".pdf"
            'End If

            Dim loPSI As New ProcessStartInfo
            Dim loProceso As New Process
            loPSI.FileName = rutaArchivoPDF '"PATH DE TU FICHERO CON EXTENSION"
            Try
                loProceso = Process.Start(loPSI)
            Catch Exp As Exception
                MessageBox.Show(Exp.Message, "XXXX", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try


            'PDF(rutaArchivoPDF)
            DatosEnvio("")
        Else
            MsgBox("A un no selecciona la factura a Re-Imprimir ", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub frmBoutiqueReimpresion_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub
End Class