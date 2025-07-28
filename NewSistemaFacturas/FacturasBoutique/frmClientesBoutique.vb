Public Class frmClientesBoutique

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Dim tablaclientes As Data.DataTable
    Private Sub frmClientesBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ActivaCasillas(False)
        CargaGrid()
        txtrfc.Focus()
    End Sub

    Private Sub ActivaCasillas(ByVal Opcion As Boolean)
        Me.txtrfc.ReadOnly = Opcion
        Me.txtRazonSocial.ReadOnly = Opcion
        Me.txtEmail.ReadOnly = Opcion
        Me.txtCalle.ReadOnly = Opcion
        Me.txtnoint.ReadOnly = Opcion
        Me.txtnoext.ReadOnly = Opcion
        Me.txtCol.ReadOnly = Opcion
        Me.txtEdo.ReadOnly = Opcion
        Me.txtMun.ReadOnly = Opcion
        Me.txtLoc.ReadOnly = Opcion
        Me.txtCP.ReadOnly = Opcion
        'Me.txtdesrf.ReadOnly = Opcion
        'Me.txtidreg.ReadOnly = Opcion
    End Sub

    Private Sub CargaGrid()
        REM objpro.LLenaGrid(Ugr, "select cid, crfc as rfc, crsocial as RazonSocial, cemail as Correo, cusocfdi as UsoCFDI from catclie", "catclie", Motor)
        objpro.LLenaGrid(Ugr, "select cid, crfc as rfc, crsocial as RazonSocial, cemail as Correo, catclie.cusocfdi as UsoCFDI, cfdides as Uso_CFDI, catclie.cregfiscal as RegimenFiscal from catclie inner join satuscfdi on catclie.cusocfdi = satuscfdi.cfdiuso", "catclie", Motor)
    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
            .DisplayLayout.Bands(0).Columns(1).Width = 80
            .DisplayLayout.Bands(0).Columns(2).Width = 300            
            .DisplayLayout.Bands(0).Columns(3).Width = 150
            .DisplayLayout.Bands(0).Columns(4).Hidden = True
            .DisplayLayout.Bands(0).Columns(5).Width = 180
        End With
    End Sub

    Private Sub frmClientesBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub Ugr_ClickCell(sender As Object, e As Infragistics.Win.UltraWinGrid.ClickCellEventArgs) Handles Ugr.ClickCell
        ActivaCasillas(True)
        cfdiuso = ""
        cfdides = ""
        Me.txtidreg.Text = ""
        Me.txtdesrf.Text = ""
        Me.txtUsoCfdi.Text = ""
        Me.txtclvUsoCfdi.Text = ""
        Me.txtCalle.Text = ""
        Me.txtnoint.Text = ""
        Me.txtnoext.Text = ""
        Me.txtCol.Text = ""
        Me.txtEdo.Text = ""
        Me.txtMun.Text = ""
        Me.txtLoc.Text = ""
        Me.txtCP.Text = ""
        idCliente = 0
        Try
            Me.txtrfc.Text = Ugr.ActiveRow.Cells("rfc").Value
            Me.txtRazonSocial.Text = Ugr.ActiveRow.Cells("RazonSocial").Value
            Me.txtEmail.Text = Ugr.ActiveRow.Cells("Correo").Value
            Me.txtId.Text = Ugr.ActiveRow.Cells("cid").Value
            idCliente = Ugr.ActiveRow.Cells("cid").Value
            cfdiuso = Ugr.ActiveRow.Cells("UsoCFDI").Value
        Catch ex As Exception
            Me.txtrfc.Text = ""
            Me.txtRazonSocial.Text = ""
            Me.txtEmail.Text = ""
            Me.txtId.Text = ""
            Me.txtidreg.Text = ""

        End Try
        Try
            tablaclientes = objpro.ObtenDatos("SELECT * FROM CATCLIE WHERE cRFC = '" & Me.txtrfc.Text & "'", Motor).Tables(0)
            regimenFisc = Ugr.ActiveRow.Cells("RegimenFiscal").Value
            Me.txtCalle.Text = tablaclientes.Rows(0).Item("cDir").ToString.Trim
            Me.txtnoint.Text = tablaclientes.Rows(0).Item("cnumint").ToString.Trim
            Me.txtnoext.Text = tablaclientes.Rows(0).Item("cnumext").ToString.Trim
            Me.txtCol.Text = tablaclientes.Rows(0).Item("ccolonia").ToString.Trim
            Me.txtEdo.Text = tablaclientes.Rows(0).Item("cestado").ToString.Trim
            Me.txtMun.Text = tablaclientes.Rows(0).Item("cmun").ToString.Trim
            Me.txtLoc.Text = tablaclientes.Rows(0).Item("clocal").ToString.Trim
            Me.txtCP.Text = tablaclientes.Rows(0).Item("ccp").ToString.Trim
            If cfdiuso <> "" Then
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "satuscfdi", "cfdiuso = '" & cfdiuso & "'", Motor)
            End If
            If regimenFisc <> "" Then
                txtidreg.Text = regimenFisc
                txtdesrf.Text = objpro.Dato("cregfisdes", "CATCLIE", "cRFC = '" & Me.txtrfc.Text & "'", Motor)
                regimen = txtdesrf.Text
            End If
        Catch exe As Exception
            cfdiuso = ""
            cfdides = ""
            regimenFisc = ""
            regimen = ""
            Me.txtCalle.Text = ""
            Me.txtnoint.Text = ""
            Me.txtnoext.Text = ""
            Me.txtCol.Text = ""
            Me.txtEdo.Text = ""
            Me.txtMun.Text = ""
            Me.txtLoc.Text = ""
            Me.txtCP.Text = ""
            Me.txtdesrf.Text = ""
            Me.txtidreg.Text = ""
        End Try
        ActivaCasillas(True)
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDI)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
        'If txtId.Text <> "ID" Then
        '    Formulario(satUsoCFDI)
        '    txtclvUsoCfdi.Text = cfdiuso
        '    txtUsoCfdi.Text = cfdides
        'Else
        '    MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        'End If
    End Sub

    Private Sub btnregfis_Click(sender As Object, e As EventArgs) Handles btnregfis.Click
        Formulario(frmRegimenFiscal)
        txtidreg.Text = regimenFisc
        txtdesrf.Text = regimen
        'If txtidreg.Text <> "regimen" Then
        '    Formulario(frmRegimenFiscal)
        '    txtidreg.Text = regimen
        '    txtdesrf.Text = regimenFisc
        'Else
        '    MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        'End If
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub Modificar_Click(sender As Object, e As EventArgs) Handles Modificar.Click
        If txtId.Text <> "ID" Then
            ActivaCasillas(False)
            Me.txtrfc.Focus()
        Else
            MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub Guardar_Click(sender As Object, e As EventArgs) Handles Guardar.Click

        Dim Existerfc As String
        Existerfc = objpro.DatoString("crfc", "catclie", "crfc = '" & txtrfc.Text & "' ", Motor)
        If txtnoext.Text = "" Then
            txtnoext.Text = 0
        End If
        If Existerfc = "" Then
            Try
                objpro.Indicacion("Insert Into catclie (crsocial,crfc,cemail, cusocfdi, cRegFiscal, cRegFisDes, cdir, cnumext, cnumint, ccolonia ,cmun, cestado, clocal, ccp) " &
                                                 "values ('" & txtRazonSocial.Text.ToUpper & "', '" & txtrfc.Text.ToUpper & "','" & txtEmail.Text & "', '" & txtclvUsoCfdi.Text & "' , '" & txtidreg.Text & "' , '" & txtdesrf.Text & "', " &
                                                 "'" & txtCalle.Text.ToUpper & "', '" & txtnoext.Text & "','" & txtnoint.Text & "', '" & txtCol.Text.ToUpper & "' , '" & txtMun.Text.ToUpper & "' , '" & txtEdo.Text.ToUpper & " ', " &
                                                 "'" & txtLoc.Text.ToUpper & "', '" & txtCP.Text & "') ", Motor)
                MsgBox("Cliente agregado correctamente .. ", MsgBoxStyle.Information, "Facturación")
                Me.txtrfc.Text = ""
                Me.txtEmail.Text = ""
                Me.txtRazonSocial.Text = ""
                cfdiuso = ""
                cfdides = ""
                Me.txtUsoCfdi.Text = ""
                Me.txtclvUsoCfdi.Text = ""
                Me.txtidreg.Text = ""
                Me.txtCalle.Text = ""
                Me.txtnoint.Text = ""
                Me.txtnoext.Text = ""
                Me.txtCol.Text = ""
                Me.txtEdo.Text = ""
                Me.txtMun.Text = ""
                Me.txtLoc.Text = ""
                Me.txtCP.Text = ""
                Me.txtdesrf.Text = ""
                regimen = ""
                CargaGrid()
                txtrfc.Focus()
            Catch EEX As Exception
                MsgBox(EEX.Message, MsgBoxStyle.Information, "Facturación")
            End Try
        Else
            MsgBox("Cliente ya existe ", MsgBoxStyle.Information, "Facturación")
        End If
        If txtId.Text <> "ID" Then
            Try
                objpro.Indicacion("update catclie set crfc = '" & txtrfc.Text & "', crsocial = '" & txtRazonSocial.Text & "', " &
                                      " cemail = '" & txtEmail.Text & "', cusoCFDI = '" & txtclvUsoCfdi.Text & "' " &
                                      ", cregfiscal = '" & txtidreg.Text & "', cdir = '" & txtCalle.Text & "', cnumext = '" & txtnoext.Text & "', " &
                                      "cnumint = '" & txtnoint.Text & "', ccolonia = '" & txtCol.Text & "',  cmun = '" & txtMun.Text & "', " &
                                       "cestado = '" & txtEdo.Text & "', clocal = '" & txtLoc.Text & "', ccp = '" & txtCP.Text & "', " &
                                       "cregFisDes = '" & txtdesrf.Text & "' where cid = " & txtId.Text & "", Motor)
                ProcesoInicio()
                MsgBox("Cliente actualizado correctamente .. ", MsgBoxStyle.Information, "Facturación")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación")
            End Try
        Else
            MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        End If

    End Sub

    Private Sub ProcesoInicio()
        ActivaCasillas(True)
        CargaGrid()
    End Sub

    Private Sub Eliminar_Click(sender As Object, e As EventArgs) Handles Eliminar.Click
        If txtId.Text <> "ID" Then
            If MsgBox("¿Seguro de eliminar a ?. " & txtRazonSocial.Text, vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
                objpro.Indicacion("delete from catclie where cid = " & txtId.Text & "", Motor)
                CargaGrid()
                Me.txtrfc.Text = ""
                Me.txtRazonSocial.Text = ""
                Me.txtEmail.Text = ""
                Me.txtId.Text = ""
                cfdiuso = ""
                cfdides = ""
                Me.txtCalle.Text = ""
                Me.txtnoint.Text = ""
                Me.txtnoext.Text = ""
                Me.txtCol.Text = ""
                Me.txtEdo.Text = ""
                Me.txtMun.Text = ""
                Me.txtLoc.Text = ""
                Me.txtCP.Text = ""
                Me.txtidreg.Text = ""
                Me.txtdesrf.Text = ""
                MsgBox("Registro Eliminado .. ", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub txtrfc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtrfc.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtRazonSocial.Focus()
        End If
    End Sub

    Private Sub txtRazonSocial_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRazonSocial.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtEmail.Focus()
        End If
    End Sub

    Private Sub txtEmail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmail.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = LCase(e.KeyChar)
        Else
            btnUsoCfdi.Focus()
        End If
    End Sub

    Private Sub txtCalle_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCalle.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtnoext.Focus()
        End If
    End Sub

    Private Sub txtCol_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCol.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtEdo.Focus()
        End If
    End Sub

    Private Sub txtMun_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMun.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtLoc.Focus()
        End If
    End Sub

    Private Sub txtEdo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEdo.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtMun.Focus()
        End If
    End Sub

    Private Sub txtLoc_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLoc.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            txtCP.Focus()
        End If
    End Sub

End Class