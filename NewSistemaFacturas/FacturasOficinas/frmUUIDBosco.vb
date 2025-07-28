Public Class frmUUIDBosco

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub frmUUIDBosco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()        
        objpro.LLenaGrid(Me.Ugr, "Select fefact as Factura, feuuid as UUID from bgxfacelec.fencabez where tid = " & DevTienda & " and sid = " & DevSucursal & " and fetip in ('F', 'G') and feid = " & Devfeid & "", "bgxfacelec", As400)
        Me.Ugr.DataBind()
    End Sub

    Private Sub frmUUIDBosco_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Dispose()
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Try            
            txtUUID.Text = Ugr.ActiveRow.Cells("UUID").Value
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación ..")
        End Try
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