
Imports System
Imports N400
Imports N400.Componentes
Imports System.Runtime.InteropServices
Imports N400.QR
Imports System.Collections.Generic

Public Class frmBorraVentas

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones

    Dim tventas As Data.DataTable
    Dim fechav As String
    Dim suc As String
#End Region

    Private Sub frmBorraVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Esconde2()
        'suc = objpro.DatoString("CSUNUM", "CATCIA", "CCSUCFE = " & CInt(Sucursal) & " ", MotorBoutique)
        Label1.Text = objpro.DatoString("trsocial", "CatTien", "tid = " & Tienda & "", Motor)
        Label2.Text = "SUCURSAL: " & objpro.DatoString("scsuc", "CatSucu", "sid = " & Sucursal & " and tid = " & Tienda & " ", Motor).ToUpper
        fechav = Now.Date.ToString.Substring(0, 2) + "/" + Now.Date.ToString.Substring(3, 2) + "/" + Now.Date.ToString.Substring(6, 4)
        txtFechaVenta.Value = CDate(fechav)

        upb.Value = 0
        upb.Minimum = 0
        upb.Maximum = 100
    End Sub

    Private Sub Esconde()
        UltraLabel8.Visible = False
        USERtxt.Visible = False
        UltraLabel1.Visible = False
        PAWStxt.Visible = False
        btnEntra.Visible = False
        UltraLabel2.Visible = True
        UltraLabel3.Visible = True
        txtFechaVenta.Visible = True
        btnBorra.Visible = True
        upb.Visible = True
    End Sub

    Private Sub Esconde2()
        UltraLabel2.Visible = False
        UltraLabel3.Visible = False
        txtFechaVenta.Visible = False
        btnBorra.Visible = False
        upb.Visible = False
        UltraLabel8.Visible = True
        USERtxt.Visible = True
        USERtxt.Text = ""
        UltraLabel1.Visible = True
        PAWStxt.Visible = True
        PAWStxt.Text = ""
        btnEntra.Visible = True
    End Sub

    Private Sub btnEntra_Click(sender As Object, e As EventArgs) Handles btnEntra.Click
        If USERtxt.Text.ToUpper = USUAR And PAWStxt.Text = PASSWRD Then
            Esconde()
        Else
            MsgBox("Usuario o contraseña incorrectos .. ", MsgBoxStyle.Information, "Facturación ..")
        End If
    End Sub

    Private Sub btnBorra_Click(sender As Object, e As EventArgs) Handles btnBorra.Click
        If MessageBox.Show("ESTA SEGURO DE BORRAR LA VENTA CARGADA DEL DÍA " & txtFechaVenta.Value.ToString & " ? ", "BORRA VENTAS", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then

            Application.DoEvents()
            tventas = objpro.ObtenDatos(" select VETERM AS TERMINAL, VEFOLIO AS FOLIO from VENTAENC WHERE VEFOLIO = " & Tienda & " AND  SID = " & Sucursal & " AND day(vefecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vefecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(vefecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor).Tables(0)
            If tventas.Rows.Count <> 0 Then
                upb.Value += 25
                Dim folio, terminal As Integer
                For Each j In tventas.Rows
                    folio = CInt(j("FOLIO").ToString)
                    terminal = CInt(j("TERMINAL").ToString)
                Next
                Application.DoEvents()
                If objpro.DatoExiste("veestatu", "ventaenc", "sid = " & Sucursal & " and VEFOLIO =  " & folio & " and veterm =  " & terminal & " and veestatu = 0 and vefac = '0' and day(vefecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vefecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(vefecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) = True Then
                    upb.Value += 25
                    Application.DoEvents()
                    ''ELIMINA DETALLE
                    objpro.Indicacion("DELETE FROM VENTADET WHERE Sid = " & Sucursal & " And VDFOLIO = " & folio & " And VDTERM = " & terminal & " and day(vdfecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vdfecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(vdfecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "", Motor)
                    upb.Value += 25
                    Application.DoEvents()
                    ''ELIMINO ENCABEZADOS
                    objpro.Indicacion("DELETE FROM VENTAENC WHERE sid = " & Sucursal & " and VEFOLIO = " & folio & " And VeTERM = " & terminal & " and day(vefecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vefecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(vefecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "", Motor)
                    upb.Value += 25
                    MsgBox("SE HA BORRADO CORRECTAMENTE LA INFORMACION CARGADA DE LAS VENTAS DEL DÍA " & txtFechaVenta.Value.ToString & "  ", MsgBoxStyle.Information, "Facturación ..")
                Else
                    MsgBox("Las venta NO PUEDE SER BORRADA porque ya fue facturada ...", MsgBoxStyle.Information, "Nota Facturada .. ")
                    upb.Value = 0
                End If
            Else
                MsgBox("NO HAY DATOS PARA BORRAR EN LA FECHA SELECCIONADA ... ", MsgBoxStyle.Information, "Facturación ..")
            End If
        End If
    End Sub

    Private Sub PAWStxt_KeyPress(sender As Object, e As KeyPressEventArgs) Handles PAWStxt.KeyPress
        If Asc(e.KeyChar) = 13 Then
            'MessageBox.Show("Enter pressed!")
            e.Handled = True
            If USERtxt.Text.ToUpper = USUAR And PAWStxt.Text = PASSWRD Then
                Esconde()
            Else
                MsgBox("Usuario o contraseña incorrectos .. ", MsgBoxStyle.Information, "Facturación ..")
            End If
        End If
    End Sub
End Class