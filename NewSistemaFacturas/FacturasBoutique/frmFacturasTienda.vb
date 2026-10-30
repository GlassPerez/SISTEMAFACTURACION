Public Class frmFacturasTienda

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region


    Dim UUID, nofac, serie, fetim, fevta, tipof As String

    Public Property filtro As String

    Private Sub frmFacturasTienda_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UUID = ""
        nofac = ""
        serie = ""
        fetim = ""
        fevta = ""
        tipof = ""
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.Ugr, "SELECT FEncabezado.FeUUID as UUID, FEncabezado.FeFact as Factura, FEncabezado.FeTip as TipoF ,FEncabezado.fefecha as FechaTIM, FEncabezado.fereg as FechaVTA, FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado,FEncabezado.Cid,FEncabezado.FeTerm as Terminal,FEncabezado.FeSubt as Subtotal, FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total, Clientes.crfc as rfc_Clientes, Clientes.cRSocial as Clientes FROM FEncabez as FEncabezado left join CatClie as Clientes on FEncabezado.Cid = Clientes.Cid  Where FEncabezado.Tid = " & Tienda & " and FEncabezado.Sid = " & Sucursal & " and ( Fencabezado.fetip = " & filtroP & ")" & " order by FEncabezado.fereg desc", "FEncabez", Motor) 'and FEncabezado.feSerie = '" & grbTipoFactura.Value & "'", Motor)
        Me.Ugr.DataBind()
        For i = 0 To Me.Ugr.Rows.Count - 1
            'Dim Tfac As Double = 0
            'Tfac = Ugr.Rows(i).Cells("Total").Value
            If Ugr.Rows(i).Cells("Estado").Value = 0 Then
                Ugr.Rows(i).Appearance.BackColor = Color.Red

                'Else
                '    If Ugr.Rows(i).Cells("Abono").Value <> 0 Then
                '        Ugr.Rows(i).Appearance.BackColor = Color.Gold
                '    End If
            End If
        Next
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs)
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 200
            .DisplayLayout.Bands(0).Columns(1).Width = 18
            .DisplayLayout.Bands(0).Columns(2).Width = 18
            .DisplayLayout.Bands(0).Columns(3).Width = 80
            .DisplayLayout.Bands(0).Columns(4).Width = 80
            .DisplayLayout.Bands(0).Columns(5).Width = 30
            .DisplayLayout.Bands(0).Columns(6).Hidden = True
            .DisplayLayout.Bands(0).Columns(7).Width = 50
            .DisplayLayout.Bands(0).Columns(8).Width = 60
            .DisplayLayout.Bands(0).Columns(9).Width = 100
            .DisplayLayout.Bands(0).Columns(10).Width = 100
            .DisplayLayout.Bands(0).Columns(11).Width = 100
            .DisplayLayout.Bands(0).Columns(12).Width = 60
            .DisplayLayout.Bands(0).Columns(13).Width = 100
            .DisplayLayout.Bands(0).Columns(14).Width = 180
            .DisplayLayout.Bands(0).Columns(15).Width = 100
            .DisplayLayout.Bands(0).Columns(16).Hidden = True
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Try
            If Ugr.ActiveRow.Cells("Estado").Value = 0 Then
                MsgBox("La factura que selecciono ya ha sido Relacionada anteriormente", MsgBoxStyle.Information, "Facturación ..")
            Else
                txtUUID.Text = Ugr.ActiveRow.Cells("UUID").Value
                UUID = txtUUID.Text
                nofac = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
                serie = Ugr.ActiveRow.Cells("Serie").Value.ToString
                fetim = Ugr.ActiveRow.Cells("FechaTIM").Value.ToString
                fevta = Ugr.ActiveRow.Cells("FechaVTA").Value.ToString
                tipof = Ugr.ActiveRow.Cells("TipoF").Value.ToString
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación ..")
        End Try
    End Sub

    Private Sub frmFacturasTienda_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Dispose()
    End Sub

    Public Event FormularioCerrandose(ByVal nofac As String, ByVal serie As String, ByVal uuid As String, ByVal fetim As String, ByVal fevta As String, ByVal tipof As String)

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtUUID.Text <> "" Then
            If MsgBox("¿Seguro del UUID ?.", vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
                'Dim n = frmGlobalBoutv2.DGFacturas.Rows.Add()
                'frmGlobalBoutv2.DGFacturas.Rows(n).Cells(0).Value = nofac
                'frmGlobalBoutv2.DGFacturas.Rows(n).Cells(1).Value = serie
                'frmGlobalBoutv2.DGFacturas.Rows(n).Cells(2).Value = txtUUID.Text
                'frmGlobalBoutv2.DGFacturas.Rows(n).Cells(3).Value = fetim
                'frmGlobalBoutv2.DGFacturas.Rows(n).Cells(4).Value = fevta
                'frmGlobalBoutv2.DGFacturas.Rows(n).Cells(5).Value = tipof

                RaiseEvent FormularioCerrandose(nofac, serie, txtUUID.Text, fetim, fevta, tipof)
                Me.Dispose()
            End If
        Else
            MsgBox("No selecciono UUID relacionado", MsgBoxStyle.Information, "Facturación ..")
        End If
    End Sub

End Class