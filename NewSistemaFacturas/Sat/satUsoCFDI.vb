Public Class satUsoCFDI

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub satUsoCFDI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.Ugr, "Select cfdiuso as Id, cfdides as descripcion from satuscfdi order by cfdiuso", "satuscfdi", Motor)
        Me.Ugr.DataBind()
    End Sub

    Private Sub Ugr_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With e.Layout.Override
            .FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains
            .AllowUpdate = Infragistics.Win.DefaultableBoolean.False
        End With
        e.Layout.Bands(0).Columns(0).Hidden = True        
        e.Layout.Bands(0).Columns(1).Width = 250
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Dispose()
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        Try
            cfdiuso = Ugr.ActiveRow.Cells("Id").Value
            cfdides = Ugr.ActiveRow.Cells("descripcion").Value
            txtUsoCFDI.Text = cfdides
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación")
        End Try
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If MsgBox("¿Seguro del Uso del CFDI ?.", vbYesNo + vbExclamation + vbDefaultButton2, "Facturación") = MsgBoxResult.Yes Then
            If txtUsoCFDI.Text <> "" Then
                Me.Dispose()
            Else
                MsgBox("Falta seleccionar uso CFDI", MsgBoxStyle.Information, "Facturación")
            End If
        End If
    End Sub
End Class