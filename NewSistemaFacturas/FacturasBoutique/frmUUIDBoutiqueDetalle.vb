Public Class frmUUIDBoutiqueDetalle

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub frmUUIDBoutiqueDetalle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.Ugr, "Select fefact as Factura, feuuid as UUID, Fetotal as Importe from fencabez where tid = " & DevTienda & " and sid = " & DevSucursal & " and fetip in ('F') order by fefact desc ", "bgxfacelec", Motor)
        Me.Ugr.DataBind()
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 300
            .DisplayLayout.Bands(0).Columns(2).Width = 150
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Try
            txtUUID.Text = Ugr.ActiveRow.Cells("UUID").Value
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación ..")
        End Try
    End Sub

    Private Sub frmUUIDBoutiqueDetalle_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Dispose()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtUUID.Text <> "" Then
            If MsgBox("¿Seguro del UUID ?.", vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
                DevUUID = txtUUID.Text
                Me.Dispose()
            End If
        Else
            MsgBox("No selecciono UUID relacionado", MsgBoxStyle.Information, "Facturación ..")
        End If
    End Sub

End Class