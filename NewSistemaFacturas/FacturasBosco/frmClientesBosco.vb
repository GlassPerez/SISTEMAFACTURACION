Public Class frmClientesBosco

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub frmClientesBosco_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.txtdesreg.ReadOnly = True
        Me.txtidreg.ReadOnly = True
        txtrfc.Focus()
        ActivaCasillas(False)
        CargaGrid()

    End Sub

    Private Sub ActivaCasillas(ByVal Opcion As Boolean)
        txtrfc.ReadOnly = Opcion
        txtRazonSocial.ReadOnly = Opcion
        txtEmail.ReadOnly = Opcion
        Me.txtCalle.ReadOnly = Opcion
        Me.txtnoint.ReadOnly = Opcion
        Me.txtnoext.ReadOnly = Opcion
        Me.txtCol.ReadOnly = Opcion
        Me.txtEdo.ReadOnly = Opcion
        Me.txtMun.ReadOnly = Opcion
        Me.txtLoc.ReadOnly = Opcion
        Me.txtCP.ReadOnly = Opcion

    End Sub

    Dim tablaclientes As Data.DataTable

    Private Sub CargaGrid()
        objpro.LLenaGrid(Ugr, "select cid, crfc as rfc, crsocial as RazonSocial, cemail as Correo, clientes.cusocfdi as UsoCFDI, cfdides as Uso_CFDI, clientes.cconfecr as RegimenFiscal from bgxfacelec.catclie as clientes inner join bgxfacelec.satuscfdi as uso on clientes.cusocfdi = uso.cfdiuso ", "catclie", As400)

        'objpro.LLenaGrid(Ugr, "select cid, crfc as rfc, crsocial as RazonSocial, cemail as Correo, clientes.cusocfdi as UsoCFDI, cfdides as Uso_CFDI, catclie.cconfecr as RegimenFiscal, satregfis.regdes as Reg_Fiscal  from bgxfacelec.catclie as clientes inner join bgxfacelec.satuscfdi as uso on clientes.cusocfdi = uso.cfdiuso inner join bgxfacelec.satregfis as regfis on convert(int(clientes.cconfecr)) = regfis.regimen", "catclie", As400)
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


    Private Sub Ugr_ClickCell(sender As Object, e As Infragistics.Win.UltraWinGrid.ClickCellEventArgs) Handles Ugr.ClickCell
        ActivaCasillas(True)
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
        Me.txtdesreg.Text = ""
        regimen = ""
        Try
            Me.txtrfc.Text = Ugr.ActiveRow.Cells("rfc").Value
            Me.txtRazonSocial.Text = Ugr.ActiveRow.Cells("RazonSocial").Value
            Me.txtEmail.Text = Ugr.ActiveRow.Cells("Correo").Value
            Me.txtId.Text = Ugr.ActiveRow.Cells("cid").Value
            idCliente = Ugr.ActiveRow.Cells("cid").Value
            cfdiuso = Ugr.ActiveRow.Cells("UsoCFDI").Value
            If cfdiuso <> "" Then
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
            Else
                txtUsoCfdi.Text = Ugr.ActiveRow.Cells("Uso_CFDI").Value
            End If
        Catch ex As Exception
            Me.txtrfc.Text = ""
            Me.txtRazonSocial.Text = ""
            Me.txtEmail.Text = ""
            Me.txtId.Text = ""
            Me.txtUsoCfdi.Text = ""
            Me.txtclvUsoCfdi.Text = ""
            cfdiuso = ""
            'cfdides = ""
        End Try

        Try
            tablaclientes = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CATCLIE WHERE cRFC = '" & Me.txtrfc.Text & "'", As400).Tables(0)
            Me.txtCalle.Text = tablaclientes.Rows(0).Item("cDir").ToString.Trim
            Me.txtnoint.Text = tablaclientes.Rows(0).Item("cnumint").ToString.Trim
            Me.txtnoext.Text = tablaclientes.Rows(0).Item("cnumext").ToString.Trim
            Me.txtCol.Text = tablaclientes.Rows(0).Item("ccolonia").ToString.Trim
            Me.txtEdo.Text = tablaclientes.Rows(0).Item("cestado").ToString.Trim
            Me.txtMun.Text = tablaclientes.Rows(0).Item("cmun").ToString.Trim
            Me.txtLoc.Text = tablaclientes.Rows(0).Item("clocal").ToString.Trim
            Me.txtCP.Text = tablaclientes.Rows(0).Item("ccp").ToString.Trim
            regimenFisc = tablaclientes.Rows(0).Item("CCONFECR").ToString.Trim 'Ugr.ActiveRow.Cells("RegimenFiscal").Value

            If regimenFisc <> 0 Then
                txtidreg.Text = regimenFisc.ToString
                txtdesreg.Text = objpro.Dato("regdes", "bgxfacelec.satregfis", "regimen = " & txtidreg.Text & "", As400)
                regimen = txtdesreg.Text
            End If
        Catch ex As Exception
            Me.txtdesreg.Text = ""
            Me.txtidreg.Text = ""
            Me.txtCalle.Text = ""
            Me.txtnoint.Text = ""
            Me.txtnoext.Text = ""
            Me.txtCol.Text = ""
            Me.txtEdo.Text = ""
            Me.txtMun.Text = ""
            Me.txtLoc.Text = ""
            Me.txtCP.Text = ""
            regimen = ""

        End Try
    End Sub

    Private Sub frmClientesBosco_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDIBosco)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
        'If txtId.Text <> "ID" Then
        '    Formulario(satUsoCFDIBosco)
        '    txtclvUsoCfdi.Text = cfdiuso
        '    txtUsoCfdi.Text = cfdides
        'Else
        '    MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        'End If
    End Sub

    Private Sub btnregfis_Click(sender As Object, e As EventArgs) Handles btnregfis.Click
        Formulario(frmRegimenFiscal)
        txtidreg.Text = regimenFisc
        txtdesreg.Text = regimen 'DESCRIPCION
        'If txtidreg.Text <> "ID" Then
        '    Formulario(frmRegimenFiscal)
        '    txtidreg.Text = regimen
        '    txtdesreg.Text = regimenFisc
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
        If Me.txtnoext.Text = "" Then
            Me.txtnoext.Text = 0
        End If
        Dim Existerfc As String
        Existerfc = objpro.DatoString("crfc", "bgxfacelec.catclie", "crfc = '" & Trim(txtrfc.Text) & "' ", As400)
        If Existerfc = "" Then
            Try
                objpro.Indicacion("Insert Into bgxfacelec.catclie (crsocial, crfc, cEmail, cusocfdi, cdir, cnumext, cnumint, ccolonia, cmun, cestado, clocal, cpais, ccp, cconfecr ) " &
                                                         "values ('" & Me.txtRazonSocial.Text.ToUpper & "', '" & Me.txtrfc.Text.ToUpper & "', '" & Me.txtEmail.Text & "', '" & Me.txtclvUsoCfdi.Text & "', " &
                                                         " '" & Me.txtCalle.Text.ToUpper & "', " & Me.txtnoext.Text & ", '" & Me.txtnoint.Text & "', '" & Me.txtCol.Text.ToUpper & "', '" & Me.txtMun.Text.ToUpper & "', '" & Me.txtEdo.Text.ToUpper & "', '" & Me.txtLoc.Text.ToUpper & "', '" & "MEXICO" & "', " & Me.txtCP.Text & ", '" & txtidreg.Text & "' ) ", As400)

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
                Me.txtdesreg.Text = ""
                regimen = ""
                CargaGrid()
                txtrfc.Focus()
            Catch EEX As Exception
                MsgBox(EEX.Message, MsgBoxStyle.Information, "Facturación")
            End Try

        Else
            MsgBox("Cliente ya existe .. ", MsgBoxStyle.Information, "Facturación")
        End If
        If txtId.Text <> "ID" Then
            Try
                objpro.Indicacion("update bgxfacelec.catclie set crfc = '" & txtrfc.Text & "', crsocial = '" & txtRazonSocial.Text & "', " &
                                      " cemail = '" & txtEmail.Text & "', cusoCFDI = '" & txtclvUsoCfdi.Text & "' " &
                                      " ,cconfecr = '" & txtidreg.Text & "', cdir = '" & txtCalle.Text & "', cnumext = '" & txtnoext.Text & "', " &
                                      "cnumint = '" & txtnoint.Text & "', ccolonia = '" & txtCol.Text & "',  cmun = '" & txtMun.Text & "', " &
                                       "cestado = '" & txtEdo.Text & "', clocal = '" & txtLoc.Text & "', ccp = '" & txtCP.Text & "' " &
                                      " where cid = " & txtId.Text & "", As400)
                ProcesoInicio()
                CargaGrid()
                MsgBox("Cliente actualizado correctamente .. ", MsgBoxStyle.Information, "Facturación")
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación")
            End Try
        Else
            MsgBox("No ha seleccionado cliente para actualizar.. ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub ProcesoInicio()
        ActivaCasillas(True)        
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

    Private Sub Eliminar_Click(sender As Object, e As EventArgs) Handles Eliminar.Click
        If txtId.Text <> "ID" Then
            If MsgBox("¿Seguro de eliminar a ?. " & txtRazonSocial.Text, vbYesNo + vbExclamation + vbDefaultButton2, "Facturación .. ") = MsgBoxResult.Yes Then
                objpro.Indicacion("delete from bgxfacelec.catclie where cid = " & txtId.Text & "", As400)
                Me.txtrfc.Text = ""
                Me.txtRazonSocial.Text = ""
                Me.txtEmail.Text = ""
                Me.txtId.Text = ""
                Me.txtclvUsoCfdi.Text = ""
                Me.txtUsoCfdi.Text = ""
                Me.txtidreg.Text = ""
                Me.txtdesreg.Text = ""
                cfdiuso = ""
                cfdides = ""
                regimenFisc = ""
                Me.txtCalle.Text = ""
                Me.txtnoint.Text = ""
                Me.txtnoext.Text = ""
                Me.txtCol.Text = ""
                Me.txtEdo.Text = ""
                Me.txtMun.Text = ""
                Me.txtLoc.Text = ""
                Me.txtCP.Text = ""
                MsgBox("Registro Eliminado .. ", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("No ha seleccionado cliente.. ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

End Class