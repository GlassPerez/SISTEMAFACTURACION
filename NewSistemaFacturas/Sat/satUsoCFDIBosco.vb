Public Class satUsoCFDIBosco

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub satUsoCFDIBosco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargaGridConceptos()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.Ugr, "Select cfdiuso as Id, cfdides as descripcion from bgxfacelec.satuscfdi order by cfdiuso", "satuscfdi", As400)
        Me.Ugr.DataBind()
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
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
            Select Case cfdiuso
                Case "G01"
                Case "G02"
                Case "G03"


            End Select
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