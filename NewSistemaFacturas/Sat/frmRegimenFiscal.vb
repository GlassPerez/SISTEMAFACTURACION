Public Class frmRegimenFiscal
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub frmRegimenFiscal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.UgrRF, "Select regimen as IdRegimen, regdes as RegimenFiscal from satregfis order by regimen", "satregfis", Motor)
        Me.UgrRF.DataBind()
    End Sub

    Private Sub UgrRF_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UgrRF.InitializeLayout
        With e.Layout.Override
            .FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains
            .AllowUpdate = Infragistics.Win.DefaultableBoolean.False
        End With
        e.Layout.Bands(0).Columns(0).Hidden = True
        e.Layout.Bands(0).Columns(1).Width = 250
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles UgrRF.DoubleClick
        Try
            regimenFisc = UgrRF.ActiveRow.Cells("IdRegimen").Value
            regimen = UgrRF.ActiveRow.Cells("RegimenFiscal").Value
            txtRegimen.Text = UgrRF.ActiveRow.Cells("RegimenFiscal").Value
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación ..")
        End Try
    End Sub

    Private Sub frmRegimenFiscal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Dispose()
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtRegimen.Text <> "" Then
            If MsgBox("¿Seguro del Regimen Fiscal ?.", vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
                DevReg = txtRegimen.Text
                Me.Dispose()
            End If
        Else
            MsgBox("No selecciono Regimen Fiscal relacionado", MsgBoxStyle.Information, "Facturación ..")
        End If
    End Sub
End Class