
Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml.XPath
Imports System.Threading

Public Class frmTicketCte
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones

    'Dim dt As New DataTable
#End Region

    Dim Detalle As  Data.DataTable

    Private Sub frmTicketCte_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Ticket.Focus()
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where tTipo3 = 'N' ", 0, 1, Motor)
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & frmGlobalNutrisa.grbTienda.Value & " ", 0, 1, Motor)
        grbTienda.Value = frmGlobalNutrisa.grbTienda.Value
        grbSucursal.Value = frmGlobalNutrisa.grbSucursal.Value
        txtFechaVenta.Value = frmGlobalNutrisa.txtFechaVenta.Value
        'dt.Columns.Add("Ticket")
        'dt.Columns.Add("Total Facturado")
    End Sub

    Private Sub frmTicketCte_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim sucur As String = ""
        'Dim ticket As String = ""
        'Dim fech As String = ""
        'Dim tipo As String = ""
        'Dim vunit As Double = 0
        'Dim cant As Integer = 0
        'Dim desc As Double = 0
        'Dim ieps As Double = 0
        'Dim iva As Double = 0
        Button1.Visible = False
        Dim total, subtot, descto, desctoieps, desctocero, iva, ieps As Double
        total = 0
        subtot = 0
        iva = 0
        ieps = 0
        descto = 0
        desctoieps = 0
        desctocero = 0
        If Ticket.Text.Trim <> "" Then
            Detalle = objpro.ObtenDatos("SELECT * FROM Nutrisa Where ClaveProducto <> '535102' and clavesucursal = " & grbSucursal.Value & " and noticket = " & Ticket.Text & "and day(fechaticket) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(fechaticket)= " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(fechaticket)= " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique).Tables(0)

            Dim subt As Double = 0
            If Detalle.Rows.Count Then
                For Each r In Detalle.Rows
                    Dim sucur As String = r("clavesucursal").ToString
                    Dim ticket As String = r("noticket").ToString
                    Dim tipo As String = r("Tipo_Factura").ToString


                    'subt += (CDbl(r("DV_Subtotal").ToString)) - (CDbl(r("DV_Descuento").ToString))

                    If tipo.Trim <> "Iva Tasa16" Then
                        If tipo.Trim = "IEPS" Then
                            'Dim vunit As Double = (CDbl(r("PrecioVenta").ToString))
                            'Dim cant As Integer = (CInt(r("Cantidad").ToString))
                            'Dim subt As Double = 0
                            subt = (CDbl(r("DV_Subtotal").ToString)) - (CDbl(r("DV_Descuento").ToString)) ' (vunit * cant) - (CDbl(r("DV_Descuento").ToString))
                            descto += (CDbl(r("DV_Descuento").ToString))
                            subtot += subt
                            iva += (CDbl(r("Iva").ToString))
                            ieps += (CDbl(r("ieps8").ToString))
                            total += subt + (CDbl(r("ieps8").ToString)) + (CDbl(r("Iva").ToString))
                        Else
                            'Dim vunit As Double = (CDbl(r("PrecioVenta").ToString))
                            'Dim cant As Integer = (CInt(r("Cantidad").ToString))
                            'Dim subt As Double = 0
                            'subt = (vunit * cant) - (CDbl(r("DV_Descuento").ToString))
                            subt = (CDbl(r("DV_Subtotal").ToString)) - (CDbl(r("DV_Descuento").ToString))
                            descto += (CDbl(r("DV_Descuento").ToString))
                            subtot += subt
                            total += subt '- descto
                        End If

                    Else
                        'Dim vunit As Double = (CDbl(r("PrecioVenta").ToString))
                        'Dim cant As Integer = (CInt(r("Cantidad").ToString))
                        'Dim subt As Double = 0
                        'subt = (vunit * cant) - (CDbl(r("DV_Descuento").ToString))
                        subt = (CDbl(r("DV_Subtotal").ToString)) - (CDbl(r("DV_Descuento").ToString))
                        descto += (CDbl(r("DV_Descuento").ToString))
                        subtot += subt
                        iva += (CDbl(r("Iva").ToString))
                        total += subt + (CDbl(r("Iva").ToString))
                    End If

                    ' O usar el índice numérico: Dim valor As String = fila(0).ToString()
                Next

                'TcFactura.Rows.Add(Ticket.Text, total.ToString)


                'Dim dr As DataRow = dt.NewRow
                'dr("Ticket") = Ticket.Text
                'dr("Total Facturado") = total.ToString
                'dt.Rows.Add(dr)
                'frmGlobalNutrisa.DGFacturas.DataSource = dt
                Dim n = frmGlobalNutrisa.DGFacturas.Rows.Add()
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(0).Value = Ticket.Text
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(1).Value = total.ToString
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(2).Value = subtot.ToString
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(3).Value = descto.ToString
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(4).Value = desctoieps.ToString
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(5).Value = desctocero.ToString
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(6).Value = iva.ToString
                frmGlobalNutrisa.DGFacturas.Rows(n).Cells(7).Value = ieps.ToString

                MsgBox("Ticket Facturado agregado correctamente", MsgBoxStyle.Information, "Facturación .. ")
                Ticket.Text = ""
                total = 0
                subtot = 0
                descto = 0
                desctoieps = 0
                desctocero = 0
                iva = 0
                ieps = 0
                Ticket.Focus()
                Button1.Visible = True
                'Me.Dispose()
            Else
                MsgBox("El numero de ticket ingresado no existe, favor de verificar..", MsgBoxStyle.Information, "Facturación .. ")
                Button1.Visible = True
            End If
        Else
            MsgBox("Ingrese el número de ticket..", MsgBoxStyle.Information, "Facturación .. ")
            Button1.Visible = True
        End If


    End Sub
End Class