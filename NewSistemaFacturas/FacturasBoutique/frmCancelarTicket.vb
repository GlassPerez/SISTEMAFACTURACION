Imports System
Imports N400
Imports N400.Componentes



Public Class frmCancelarTicket
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones


    Dim Tabla, TSucursal, TablaSucursal, DatGrid, TablaTienda, datosCliente, facturas As Data.DataTable
    Dim terminal, folio, impo As Integer
    Dim estatus, fecha As String
    Dim suc As String
#End Region

    Private Sub btnGenerarFactura_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        If MessageBox.Show("ESTA SEGURO DE CANCELAR EL TICKET PARA FACTURAR ? ", "CANCELA TICKET PARA FACTURA", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            If terminal <> 0 And folio <> 0 And impo <> 0 Then
                ''vamos a eliminar ese tiecket
                Try
                    Application.DoEvents()
                    ''ELIMINO DETALLE
                    fecha = Date.Now.ToString.Substring(0, 10) ''"27/02/2024"
                    objpro.Indicacion("UPDATE VTAPOS SET VPSTSF = '' WHERE VPSUC = " & CInt(suc) & " And VPFOL = " & folio & " And VPTER = " & terminal & "and day(VPFEC) = " & fecha.ToString.Substring(0, 2) & " and month(VPFEC) = " & fecha.ToString.Substring(3, 2) & " and year(VPFEC) = " & fecha.ToString.Substring(6, 4) & " ", MotorBoutique)
                    upb.Value += 25
                    ''EN CASO DE QUE YA SE HAYA PASO LA INFO AL MAESTRO
                    objpro.Indicacion("UPDATE MVTAPOS SET MVPSTSF = '' WHERE MVPSUC = " & CInt(suc) & " And MVPFOL = " & folio & " And MVPTER = " & terminal & "and day(MVPFEC) = " & fecha.ToString.Substring(0, 2) & " and month(MVPFEC) = " & fecha.ToString.Substring(3, 2) & " and year(MVPFEC) = " & fecha.ToString.Substring(6, 4) & " ", MotorBoutique)
                    Application.DoEvents()
                    upb.Value += 25
                    ''ELIMINA DETALLE
                    objpro.Indicacion("DELETE FROM VENTADET WHERE Sid = " & Sucursal & " And VDIDFOLV = " & folio & " And VDTERM = " & terminal & " and day(vdfecha) = " & fecha.ToString.Substring(0, 2) & " and month(vdfecha) = " & fecha.ToString.Substring(3, 2) & " and year(vdfecha) = " & fecha.ToString.Substring(6, 4) & "", MotorBoutique)
                    upb.Value += 25
                    Application.DoEvents()
                    ''ELIMINO ENCABEZADOS
                    objpro.Indicacion("DELETE FROM VENTAENC WHERE sid = " & Sucursal & " and VEIDFOLV = " & folio & " And VeTERM = " & terminal & " and day(vefecha) = " & fecha.ToString.Substring(0, 2) & " and month(vefecha) = " & fecha.ToString.Substring(3, 2) & " and year(vefecha) = " & fecha.ToString.Substring(6, 4) & "", MotorBoutique)
                    upb.Value += 25
                    MsgBox("TICKET CANCELADO CORRECTAMENTE", MsgBoxStyle.Information, "Facturación .. ")
                    Inicializa()
                    CargaGridConceptos()
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación.. ")
                End Try
            End If
        End If
    End Sub

    Private Sub frmCancelarTicket_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        grbTienda.Value = ""
        grbSucursal.Value = ""
        UGTicket.DataSource = Nothing
        Inicializa()
        UGDetalle.DataSource = Nothing
        objpro.CargaCombos(grbTienda, "Select tId, trSocial as Tienda FROM CatTien", 0, 1, Motor)
        Me.grbTienda.Value = Tienda
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Value = Sucursal
        Application.DoEvents()
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Application.DoEvents()
    End Sub

    Private Sub UGDetalle_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UGDetalle.InitializeLayout
        With UGDetalle
            .DisplayLayout.Bands(0).Columns(0).Width = 80
            .DisplayLayout.Bands(0).Columns(1).Width = 80
            .DisplayLayout.Bands(0).Columns(2).Width = 80
            '.DisplayLayout.Bands(0).Columns(3).Width = 300
            .DisplayLayout.Bands(0).Columns(3).Width = 80
            .DisplayLayout.Bands(0).Columns(4).Width = 80
            .DisplayLayout.Bands(0).Columns(5).Width = 200
            .DisplayLayout.Bands(0).Columns(6).Width = 80
            '.DisplayLayout.Bands(0).Columns(7).Width = 80
        End With
    End Sub

    Private Sub UGTicket_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UGTicket.InitializeLayout
        With UGTicket
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 100
            .DisplayLayout.Bands(0).Columns(2).Width = 100
            .DisplayLayout.Bands(0).Columns(3).Width = 100
            .DisplayLayout.Bands(0).Columns(4).Width = 100
            .DisplayLayout.Bands(0).Columns(5).Width = 100
        End With
    End Sub

    Private Sub UGTicket_DoubleClick(sender As Object, e As EventArgs) Handles UGTicket.DoubleClick
        Inicializa()
        terminal = UGTicket.ActiveRow.Cells("TERMINAL").Value
        folio = UGTicket.ActiveRow.Cells("FOLIO").Value
        estatus = UGTicket.ActiveRow.Cells("ESTATUS").Value.ToString

        Application.DoEvents()
        If objpro.DatoExiste("veestatu", "ventaenc", " veidfolv =  " & folio & " and veterm =  " & terminal & " and veestatu = 0 and vefac = '0' and day(vefecha) = " & fecha.ToString.Substring(0, 2) & " and month(vefecha) = " & fecha.ToString.Substring(3, 2) & " and year(vefecha) = " & fecha.ToString.Substring(6, 4) & " ", MotorBoutique) = True Then
            Application.DoEvents()
            CargaGridDetalle()

            txtFecha.Text = UGTicket.ActiveRow.Cells("FECHA").Value.ToString.Substring(0, 10)
            txtTicket.Text = UGTicket.ActiveRow.Cells("TERMINAL").Value.ToString & " - " & UGTicket.ActiveRow.Cells("FOLIO").Value.ToString
            txtImporte.Text = UGTicket.ActiveRow.Cells("TOTAL").Value
            impo = UGTicket.ActiveRow.Cells("TOTAL").Value
        Else
            MsgBox("La nota NO puede ser cancelada porque ya fue facturada ... ", MsgBoxStyle.Information, "Nota Facturada .. ")
            UGDetalle.DataSource = Nothing
        End If
    End Sub

    Private Sub CargaGridDetalle()
        objpro.LLenaGrid(Me.UGDetalle, "SELECT VTAPOS1.VPTER as TERMINAL, VTAPOS1.VPFOL As FOLIO, VTAPOS1.cpren As RENGLON, VTAPOS1.VPCANT As PZAS, VTAPOS1.CPCODIGO AS CODIGO, CATPROD.CPDESCRI AS DESCRIPCION, VTAPOS1.VPPREC As PRECIO FROM VTAPOS1 INNER JOIN CATPROD ON CATPROD.CPCODIGO = VTAPOS1.CPCODIGO WHERE VTAPOS1.VPSUC =" & CInt(suc) & " And VTAPOS1.VPFOL = " & folio & " And VTAPOS1.VPTER = " & terminal & " ", " ", MotorBoutique)
        ''Inicializa()
    End Sub

    Private Sub grbSucursal_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbSucursal.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbSucursal
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub


    Private Sub Inicializa()
        txtFecha.Text = "Fecha"
        txtTicket.Text = "Ticket"
        txtImporte.Text = "$$"
        UGDetalle.DataSource = Nothing
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargaGridConceptos()
        If UGTicket.Rows.Count = 0 Then
            MsgBox("No hay Ticket para facturar el día de hoy .. ", MsgBoxStyle.Information, "Tickets .. ")
        End If
    End Sub

    Private Sub CargaGridConceptos()
        upb.Value = 0
        upb.Minimum = 0
        upb.Maximum = 100
        TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & CInt(suc) & " ", Motor).Tables(0)

        suc = objpro.DatoString("CSUNUM", "CATCIA", "CCSUCFE = " & CInt(Sucursal) & " ", MotorBoutique)
        fecha = Date.Now.ToString.Substring(0, 10) ''"27/02/2024"
        'objpro.LLenaGrid(Me.UGTicket, "Select VTAPOS.VPTER AS TERMINAL,  VTAPOS.VPFOL AS FOLIO, VTAPOS.VPFEC AS FECHA, VTAPOS.VPSTSF AS ESTATUS, VTAPOS.VPTOTP AS TOTAL, VENTAENC.VEFAC AS NUM_FACTURA  from VTAPOS inner join VENTAENC ON VENTAENC.VEIDFOLV = VTAPOS.VPFOL where VTAPOS.VPSUC = " & CInt(suc) & " and " &
        '                 " day(VTAPOS.VPFEC) = " & fecha.Substring(0, 2) & " and month(VTAPOS.VPFEC) = " & fecha.Substring(3, 2) & " and year(VTAPOS.VPFEC) = " & fecha.Substring(6, 4) & " AND VTAPOS.VPSTS = 'P' AND VTAPOS.VPSTSF = 'S' ", " ", MotorBoutique)

        '20260428
        objpro.LLenaGrid(Me.UGTicket, "Select VENTAENC.VETERM AS TERMINAL,  VENTAENC.VEIDFOLV AS FOLIO, VENTAENC.VEFECHA AS FECHA, VENTAENC.VEESTATU AS ESTATUS, VENTAENC.VETOTAL AS TOTAL, VENTAENC.VEFAC AS NUM_FACTURA  from VENTAENC  where VENTAENC.VEFOLIO = " & CInt(grbTienda.Value) & " and " &
                         " day(VENTAENC.VEFECHA) = " & fecha.Substring(0, 2) & " and month(VENTAENC.VEFECHA) = " & fecha.Substring(3, 2) & " and year(VENTAENC.VEFECHA) = " & fecha.Substring(6, 4) & " AND VENTAENC.VEESTATU = 0 AND VENTAENC.VEFAC = '0' ", " ", MotorBoutique)
        If UGTicket.Rows.Count <> 0 Then
            'For Each column In UGTicket.DisplayLayout.Bands(0).Columns
            '    If column.ToString = "NUM_FACTURA" Then
            '        For Each r In Me.UGTicket.Rows
            '            Dim nofac As Integer = r.Cells("NUM_FACTURA").Value
            '            If nofac <> 0 Then
            '                r.Cells(column.Index).Appearance.BackColor = Color.Red  '''SOLO PINTA UNA COLUMNA DETERMINADA
            '            End If
            '        Next
            '    End If
            'Next

            For Each r In Me.UGTicket.Rows
                Dim nofac As Integer = r.Cells("NUM_FACTURA").Value
                If nofac <> 0 Then
                    r.Appearance.BackColor = Color.Red  '''PINTA TODA LA FILA
                End If
            Next
        End If


        'PintaColUltragrid

        Me.UGTicket.DataBind()


    End Sub
End Class