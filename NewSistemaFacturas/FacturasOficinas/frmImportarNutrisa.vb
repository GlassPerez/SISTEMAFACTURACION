Imports System.IO
Imports System.IO.FileStream
Imports System.Text.Encoding
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Excel

Imports System
Imports System.Text
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid.ExcelExport

Imports System.Net
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Public Class frmImportarNutrisa

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaSucursal, TablaTienda, TabExpeTicket As Data.DataTable

    Dim ArrayArchivos As New ArrayList
    Dim rdt As New DataTable
#End Region

    Private Sub frmImportarNutrisa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FechaInicio.Value = Now.Date
        Me.FechaFin.Value = Now.Date
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where tTipo3 = 'N' ", 0, 1, Motor)
        REM objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        CargaGridConceptos()

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        Application.DoEvents()
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.UgrTicket, "select clavesucursal,NoTicket,FechaTicket,ClaveProducto,Tipo_Factura,DescripcionProducto," & _
        "UnidadMedida,Cantidad,PrecioVenta,DV_Subtotal,DV_Descuento,DV_Subtotacondescuento," & _
        "ProdExento, ieps8, ieps25, ImportePeso, Iva, porcIva, DV_Total, ClaveSat " & _
        "from nutrisa where clavesucursal = 0", "nutrisa", MotorBoutique)
        Me.UgrTicket.DataBind()
    End Sub

    Private Sub grbTienda_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles grbTienda.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.grbSucursal.Focus()
        Else
            e.KeyChar = UCase(e.KeyChar)
        End If
    End Sub

    Private Sub grbTienda_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grbTienda.ValueChanged
        Try
            OBJPRO.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
            Me.ChlisFact.Items.Clear()
            REM CargaGridConceptos()
            Me.grbSucursal.Value = Nothing
            grbSucursal.Select()
        Catch ex As Exception
            MsgBox("Sucursal: " & ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If grbPeriodo.Text <> "" Then
            If grbMes.Text <> "" Then
                RegistroBusqueda()
            Else
                grbMes.Focus()
                MsgBox("Falta Mes de consulta ", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            grbPeriodo.Focus()
            MsgBox("Falta Año de consulta ", MsgBoxStyle.Information, "Facturación .. ")
        End If

    End Sub

    Private Sub RegistroBusqueda()
        If grbTienda.Text <> "" Then
            If Me.grbSucursal.Text <> "" Then
                CargaListaNotas()
            Else
                MsgBox("Falta sucursal", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta seleccionar Tienda o Razón social", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub CargaListaNotas()
        Try
            TablaSucursal = Nothing
            TablaSucursal = OBJPRO.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)

            Me.ChlisFact.Items.Clear()

            For Each Archivo As String In My.Computer.FileSystem.GetFiles(Trim(TablaSucursal.Rows(0).Item("FtpRuta").ToString) & "\" & Me.grbPeriodo.Text & "\" & OBJFUN.EscojeMes(Me.grbMes.Text), FileIO.SearchOption.SearchAllSubDirectories, "*.txt")
                Dim NombreArchivo() As String = Split(Archivo, "\")
                Dim max As Integer = NombreArchivo.Count - 1
                Dim TipoArchivo() As String = Split(Trim(NombreArchivo(max)), "_")
                If Len(TipoArchivo(2)) <= 8 Then
                    Me.ChlisFact.Items.Add(Trim(NombreArchivo(max)))
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación ..")
        End Try
    End Sub

    Private Sub btnMonto_Click(sender As Object, e As EventArgs) Handles btnMonto.Click
        CalcularValores()
        MsgBox("Fin de consulta:", MsgBoxStyle.Information, "FACTURAS-E")
        RegistroBusqueda()
    End Sub

    Private Sub CalcularValores()
        TablaSucursal = Nothing
        TablaSucursal = OBJPRO.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)

        ArrayArchivos.Clear()
        REM Upgbar.Value = 0
        REM Dim contador As Integer = 0
        REM Dim NumReg As Integer = 0

        'Dim PrecioVenta, Importe8, Importe16 As Double
        'Dim Des0, Des16, DesIeps, DesEx As Double
        'Dim VtasEx, Vtas0, Vtas16, VtasIEPS As Double
        'Dim SubtotalDescuento As Double
        'Dim Ieps8, Iva16 As Double
        'Dim TotalVenta As Double

        For i = 0 To Me.ChlisFact.Items.Count - 1
            If ChlisFact.GetItemChecked(i) = True Then
                Upgbar.Value = 0
                Dim contador As Integer = 0
                Dim NumReg As Integer = 0

                Dim Linea As String = ""
                Dim RutaArchivo As String = Trim(TablaSucursal.Rows(0).Item("FtpRuta").ToString) & "\" & Me.grbPeriodo.Text & "\" & OBJFUN.EscojeMes(Me.grbMes.Text) & "\" & Trim(ChlisFact.Items.Item(i).ToString)
                Dim ruta() As String = Split(RutaArchivo, "\")
                Dim max As Integer = ruta.Count - 1
                Dim FechaReporte() As String = Split(Trim(ruta(max)), "_")
                Dim cargandoArchivo As String = Trim(ruta(max))
                Dim FF As String = FechaReporte(2).ToString.Substring(4, 2) & "/" & FechaReporte(2).ToString.Substring(6, 2) & "/" & FechaReporte(2).ToString.Substring(0, 4)

                If OBJPRO.DatoExiste("facfecha", "facnutrisa", "facsucursal = " & grbSucursal.Value & " and facfecha = #" & FF & "#", Motor) = False Then
                    REM day(FechaTicket) = " & FechaInicio.Value.ToString.Substring(0, 2) & " and month(FechaTicket) = " & FechaInicio.Value.ToString.Substring(3, 2) & " and year(FechaTicket) = " & FechaInicio.Value.ToString.Substring(6, 4) & "

                    ArrayArchivos.Add(Trim(RutaArchivo))
                    Dim objReader As New StreamReader(Trim(RutaArchivo))
                    Do
                        Linea = objReader.ReadLine
                        If Not Linea Is Nothing Then

                            contador += 1
                            NumReg += 1
                            Dim Cadenalinea() As String = Split(Linea, "|")
                            If Cadenalinea(0) <> "ClaveSucursal" Then
                                REM Me.UgrTicket.Rows.Band.AddNew()
                                If Cadenalinea(3) <> "100999" Then
                                    Dim idRegistro As Integer
                                    Application.DoEvents()

                                    If objpro.DatoExiste("cpcodigo", "catprod", "cpcodigo = '" & Cadenalinea(3) & "'", Motor) = True Then
                                        Cadenalinea(18) = "SUPLEMENTO"
                                    End If
                                    Application.DoEvents()
                                    Dim unidad As String = "H87"
                                    'If Cadenalinea(5) = "" Then
                                    'unidad = "PZ"
                                    'Else
                                    'unidad = Cadenalinea(5)
                                    'End If
                                    If Cadenalinea(4) <> "APERTURA-CIERRE DE TIENDA" Then
                                        objpro.Indicacion("insert into nutrisa(ClaveSucursal,NoTicket,FechaTicket,ClaveProducto,idRegistro,DescripcionProducto,UnidadMedida, " & _
                                                          "Cantidad,PrecioVenta,DV_Subtotal,DV_Descuento,DV_Subtotacondescuento,ProdExento, " & _
                                                          "ieps8,ieps25,ImportePeso,Iva,porcIva,DV_Total,Tipo_Factura,clavesat) " & _
                                                          "values(" & Cadenalinea(0) & ", " & Cadenalinea(1) & ", '" & Cadenalinea(2).ToString.Substring(0, 10) & "', '" & Cadenalinea(3) & "'," & NumReg & ", '" & Cadenalinea(4) & "', '" & unidad & "', " & _
                                                          "" & Cadenalinea(6) & "," & Cadenalinea(7) & "," & Cadenalinea(8) & "," & Cadenalinea(9) & "," & Cadenalinea(10) & ",'" & Cadenalinea(11) & "', " & _
                                                          "" & Cadenalinea(12) & ", " & Cadenalinea(13) & ", " & Cadenalinea(14) & "," & Cadenalinea(15) & ", " & Cadenalinea(16) & "," & Cadenalinea(17) & ",'" & Cadenalinea(18) & "', '" & Cadenalinea(19) & "')", Motor)
                                    End If
                                End If
                                REM ColocarArticulo(NumReg, Cadenalinea(2), Cadenalinea(1), Cadenalinea(3), Cadenalinea(4), Cadenalinea(6), Cadenalinea(5), FormatCurrency(Cadenalinea(15)), FormatCurrency(Cadenalinea(7)), FormatCurrency(Cadenalinea(9)), FormatCurrency(Cadenalinea(17)))
                                Application.DoEvents()
                                Me.Upgbar.Maximum = contador
                                Me.Upgbar.Value += 1
                                REM NumReg += 1
                            End If
                        End If
                    Loop Until Linea Is Nothing
                    objReader.Close()
                    Try
                        Dim l As Integer
                        REM For l = 0 To Me.ChlisFact.Items.Count - 1
                        REM If ChlisFact.GetItemChecked(l) = True Then
                        REM For Each Archivo In ArrayArchivos
                        REM My.Computer.FileSystem.RenameFile(Trim(Archivo), Trim(ChlisFact.Items.Item(l).ToString) & ".pdf")                        
                        My.Computer.FileSystem.RenameFile(Trim(RutaArchivo), Trim(cargandoArchivo) & ".pdf")
                        REM Next
                        REM End If
                        REM Next
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
                    End Try
                    REM Actualiza la fecha de la captura de la Venta que indica que se cargo la venta del día                            
                    OBJPRO.Indicacion("Insert into facnutrisa(facsucursal, facfecha) values(" & grbSucursal.Value & ", #" & FF & "#)", Motor)
                    REM Actualiza la fecha de la captura de la Venta que indica que se cargo la venta del día
                Else
                    MsgBox("Ya se cargo la Factura del día " & FF.ToString, MsgBoxStyle.Information, "Facturación .. ")
                End If REM Cierra bloque de valida existe fecha
            End If

            Application.DoEvents()
        Next
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        If grbTienda.Text <> "" Then
            If grbSucursal.Text <> "" Then
                objpro.LLenaGrid(Me.UgrTicket, "select clavesucursal,NoTicket,FechaTicket,ClaveProducto,Tipo_Factura,DescripcionProducto," &
                "UnidadMedida,Cantidad,PrecioVenta,DV_Subtotal,DV_Descuento,DV_Subtotacondescuento," &
                "ProdExento, ieps8, ieps25, ImportePeso, Iva, porcIva, DV_Total, claveSat " &
                "from nutrisa where clavesucursal = " & grbSucursal.Value & " and " &
                "day(FechaTicket) >= " & FechaInicio.Value.ToString.Substring(0, 2) & " and month(FechaTicket) >= " & FechaInicio.Value.ToString.Substring(3, 2) & " and year(FechaTicket) >= " & FechaInicio.Value.ToString.Substring(6, 4) & " and " &
                "day(FechaTicket) <= " & FechaFin.Value.ToString.Substring(0, 2) & " and month(FechaTicket) <= " & FechaFin.Value.ToString.Substring(3, 2) & " and year(FechaTicket) <= " & FechaFin.Value.ToString.Substring(6, 4) & " ", "nutrisa", MotorBoutique)
                If UgrTicket.Rows.Count = 0 Then
                    MsgBox("No hay información a consultar ..  ", MsgBoxStyle.Information, "Facturacion .. ")
                End If
            Else
                MsgBox("Falta Sucursal", MsgBoxStyle.Information, "Facturacion .. ")
            End If
        Else
            MsgBox("Razón Social a consultar", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub



    Private Sub Estructura_DataSet()
        With rdt
            .Columns.Add("ClaveSucursal")
            .Columns.Add("NoTicket")
            .Columns.Add("FechaTicket")
            .Columns.Add("ClaveProducto")
            .Columns.Add("idRegistro")
            .Columns.Add("DescripcionProducto")
            .Columns.Add("UnidadMedida")
            .Columns.Add("Cantidad")
            .Columns.Add("PrecioVenta")
            .Columns.Add("DV_Subtotal")
            .Columns.Add("DV_Descuento")
            .Columns.Add("DV_Subtotacondescuento")
            .Columns.Add("ProdExento")
            .Columns.Add("ieps8")
            .Columns.Add("ieps25")
            .Columns.Add("ImportePeso")
            .Columns.Add("Iva")
            .Columns.Add("porcIva")
            .Columns.Add("DV_Total")
            .Columns.Add("Tipo_Factura")
            .Columns.Add("Sucursal")
        End With
    End Sub

    Private Sub frmImportarNutrisa_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Aexcel.Export(UgrTicket, "c:\VentasNutrisa.xls")
        MsgBox("Archivo guardado correctamente en c:\VentasNutrisa.xls ", MsgBoxStyle.Information, "Archivo Exportado a Excel !!! ")
    End Sub
End Class