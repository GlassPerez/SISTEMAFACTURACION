Imports System.IO
Imports System.IO.FileStream
Imports System.Text.Encoding
Imports Infragistics.Win.UltraWinGrid

Public Class frmEnvioCierreNutrisa

#Region "Clases"
    Dim OBJPRO As New Procedures
    Dim OBJFUN As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False

    Dim Tipo As String = "F"
    Dim ExisteClave As String

    Dim ExpCalle, ExpNoExterior, ExpNoInterior, ExpColonia, ExpEstado, ExpMunicipio, ExpLocalidad, ExpCp, ExpPais As String
    Dim FolioFinal, FolioActual

    Dim AlgunaVez As Boolean = False
    Dim ArchivoGeneral As String
    Dim PorcIva As Double = 16.0

    Dim FormatoFacturas As String
    Dim ArrayArchivos As New ArrayList
#End Region

#Region "Email"
    Dim DirEmail As String
    Dim PwdEmail As String
    Dim Pop As String
    Dim PuertoPop As String
    Dim SmtpHost As String
    Dim PuertoSmtp As String
    Dim DescEmail As String
    Dim AsuntoEmail As String
    Dim MsjEmail As String
    Dim SeguridadEmail As Boolean
#End Region

#Region "FTP"
    Dim DireFTP, UserFTP, PwdFTP As String
#End Region

#Region "Forma Pago"
    Dim NumFormPago As String
    Dim MetPago As String
    Dim NumCta As String
#End Region

    Private Sub frmEnvioCierreNutrisa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        dtFechaFactura.Text = dia + "/" + mes + "/" + amo

        OBJPRO.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where Tid = " & Tienda & "", 0, 1, Motor)
        Me.grbTienda.Value = Tienda
        OBJPRO.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Value = Sucursal
        CargaGridConceptos()

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        Application.DoEvents()
    End Sub

    Private Sub CargaGridConceptos()
        OBJPRO.LLenaGrid(Me.UgrTicket, "Select VdUnidad as Unidad, VdClaPro as Clave, VdDescri as Descripcion " & _
                         " ,VdCant as Cantidad, VdPorIva as Iva, VdPreUni as PrecioUnitario " & _
                         " ,VdDesc1 as Desc1, VdDesc2 as Desc2, VdPreTot as Total From VentaDet " & _
                         " Where VdFolio = 0", "VentaDet", MotorBoutique)
        Me.UgrTicket.DataBind()
    End Sub

    Private Sub EsquemaConceptos()
        Me.UgrTicket.DataSource = Nothing
        CargaGridConceptos()
    End Sub

    Private Sub TotalesFacturar()
        Subtotal.Value = 0
        Descuento.Value = 0
        Iva.Value = 0
        IEPS.Value = 0
        Total.Value = 0
    End Sub

    Private Sub btnBuscarNota_Click(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        Try
            EsquemaConceptos()
            TotalesFacturar()
            TIP = "VG"
            TSucursal = OBJPRO.ObtenDatos("SELECT * FROM Serie Where tid = " & grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
            CargaListaNotas()
        Catch ex As Exception
            MsgBox("Busqueda: " & ex.Message.ToString, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub CargaListaNotas()        
            Me.ChlisFact.Items.Clear()
            Try
                For Each Archivo As String In My.Computer.FileSystem.GetFiles(Trim(RutaArchivos) & "\" & Me.grbPeriodo.Text & "\" & OBJFUN.EscojeMes(Me.grbMes.Text), FileIO.SearchOption.SearchAllSubDirectories, "*.txt")
                    Dim NombreArchivo() As String = Split(Archivo, "\")
                    Dim Maximo As Integer = NombreArchivo.Count - 1
                    Dim TipoArchivo() As String = Split(Trim(NombreArchivo(Maximo)), "_")
                    If Len(TipoArchivo(2)) <= 8 Then
                        Me.ChlisFact.Items.Add(Trim(NombreArchivo(Maximo)))
                    End If
                Next
            Catch ex As Exception
            MsgBox("Error busca notas: " & ex.Message.ToString, MsgBoxStyle.Information, "Facturación .. ")
            End Try

    End Sub

    Private Sub btnMonto_Click(sender As Object, e As EventArgs) Handles btnMonto.Click
        EsquemaConceptos()
        TotalesFacturar()
        CalcularValores()
    End Sub

    Private Sub CalcularValores()
        ArrayArchivos.Clear()
        Upgbar.Value = 0
        Dim contador As Integer = 0

        TotalesFacturar()

        Dim PrecioVenta, Importe8, Importe16 As Double
        Dim Des0, Des16, DesIeps As Double
        Dim Vtas0, Vtas16, VtasIEPS As Double
        Dim SubtotalDescuento As Double
        Dim Ieps8, Iva16 As Double
        Dim TotalVenta As Double

        For i = 0 To Me.ChlisFact.Items.Count - 1
            If ChlisFact.GetItemChecked(i) = True Then
                Dim Linea As String = ""
                Dim RutaArchivo As String = Trim(RutaArchivos) & "\" & Me.grbPeriodo.Text & "\" & OBJFUN.EscojeMes(Me.grbMes.Text) & "\" & Trim(ChlisFact.Items.Item(i).ToString)
                ArrayArchivos.Add(Trim(RutaArchivo))
                Dim objReader As New StreamReader(Trim(RutaArchivo))
                Do
                    Linea = objReader.ReadLine
                    If Not Linea Is Nothing Then
                        contador += 1
                        Dim Cadenalinea() As String = Split(Linea, "|")
                        If Cadenalinea(0) <> "ClaveSucursal" Then
                            REM Cadenalinea(8)'Precio de Venta
                            REM Cadenalinea(9)'Importe
                            Subtotal.Value += Cadenalinea(8) REM Importe de articulo ya con descuento
                            IEPS.Value += Cadenalinea(12) REM IEPS
                            Iva.Value += Cadenalinea(15) REM IVA
                            Descuento.Value += Cadenalinea(9) REM linea de descuento
                            Total.Value += Cadenalinea(17) REM Total

                            'ClaveProducto = Cadenalinea(3) REM ClaveProducto
                            'DescripcionProducto = Cadenalinea(4) REM DescripcionProducto
                            'Cantidad = Cadenalinea(6) REM Cantidad
                            'Unidad = Cadenalinea(5) REM UnidadMedida
                            'IvaVenta = Cadenalinea(15) REM iva
                            'PUnitario = Cadenalinea(7) REM PrecioVenta
                            'Desc1 = Cadenalinea(9) REM Descuento
                            'Importe = Cadenalinea(10) REM DV_Subtotacondescuento()

                            Dim TipoVenta As String = Trim(Cadenalinea(18))
                            REM 1
                            Select Case UCase(TipoVenta)
                                'Dim PrecioVenta, Importe0, Importe16 As Double
                                'Dim Des0, Des16 As Double
                                'Dim SubtotalDescuento As Double
                                'Dim Ieps8, IvaVenta As Double
                                'Dim TotalVenta As Double
                                Case "IVA TASA0"
                                    REM Vtas0 += Cadenalinea(10)
                                    Vtas0 += Cadenalinea(8)
                                    Des0 += Cadenalinea(9)
                                Case "IVA TASA16"
                                    REM Vtas16 += Cadenalinea(10)
                                    Vtas16 += Cadenalinea(8)
                                    Des16 += Cadenalinea(9)
                                    Iva16 += Cadenalinea(15)
                                Case "IEPS"
                                    'VtasIEPS += Cadenalinea(10)
                                    'DesIeps += Cadenalinea(9)
                                    Vtas0 += Cadenalinea(8)
                                    Des0 += Cadenalinea(9)
                                    Ieps8 += Cadenalinea(12)
                            End Select

                            REM Carga Grid
                            REM FormatoGrid(Cadenalinea(3), Cadenalinea(4), Cadenalinea(6), Cadenalinea(5), FormatCurrency(Cadenalinea(15)), FormatCurrency(Cadenalinea(7)), FormatCurrency(Cadenalinea(9)), FormatCurrency(Cadenalinea(10)))
                            Me.Upgbar.Maximum = contador
                            Application.DoEvents()
                            Me.Upgbar.Value += 1
                            REM Carga Grid
                        End If
                    End If
                Loop Until Linea Is Nothing
                objReader.Close()
                Me.Upgbar.Value = contador

                REM MsgBox("===VENTAS AL 0%===" & Vtas0 & vbCrLf & "===DESCUENTO 0%===" & Des0 & vbCrLf & "===VENTAS AL 16%===" & Vtas16 & vbCrLf & "===DESCUENTO AL 16%===" & Des16.ToString & vbCrLf & "===IVA===" & Iva16.ToString & vbCrLf & "===VENTAS IEPS===" & VtasIEPS.ToString & vbCrLf & "===IEPS===" & Ieps8.ToString & vbCrLf & "===DESCUENTO IEPS===" & DesIeps)

                Dim cpf, dpf, cdf, Udf As String
                Dim ivf, puf, dcf, impf As Double
                Dim k As Integer
                For k = 0 To 7
                    Me.UgrTicket.Rows.Band.AddNew()
                    Select Case k
                        Case Is = 0
                            cpf = k : dpf = "VENTAS AL 16%" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = Vtas16 : dcf = 0 : impf = Vtas16
                        Case Is = 1
                            cpf = k : dpf = "VENTAS AL 0%" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = Vtas0 : dcf = 0 : impf = Vtas0
                            'Case Is = 2
                            '    cpf = k : dpf = "VENTAS IEPS" : cdf = "1" : Udf = "NO APLICA"
                            '    ivf = 0 : puf = VtasIEPS : dcf = 0 : impf = VtasIEPS
                        Case Is = 2
                            cpf = k : dpf = "VENTAS EXCENTAS" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = 0 : dcf = 0 : impf = 0
                        Case Is = 3
                            cpf = k : dpf = "DESCUENTO SOBRE VENTA AL 16%" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = Des16 : dcf = 0 : impf = Des16
                        Case Is = 4
                            cpf = k : dpf = "DESCUENTO SOBRE VENTA AL 0%" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = Des0 : dcf = 0 : impf = Des0
                        Case Is = 5
                            cpf = k : dpf = "DEVOLUCION SOBRE VENTA AL 16%" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = 0 : dcf = 0 : impf = 0
                        Case Is = 6
                            cpf = k : dpf = "DEVOLUCION SOBRE VENTA AL 0%" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = 0 : dcf = 0 : impf = 0
                        Case Is = 7
                            cpf = k : dpf = "IEPS" : cdf = "1" : Udf = "NO APLICA"
                            ivf = 0 : puf = FormatCurrency(Ieps8) : dcf = 0 : impf = FormatCurrency(Ieps8)
                    End Select
                    ColocarArticulo(k, cpf, dpf, cdf, Udf, ivf, puf, dcf, impf)
                Next

            End If
        Next
    End Sub

    Private Sub frmEnvioCierreNutrisa_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, ByVal Cp As String, ByVal DP As String, ByVal Cd As String, ByVal Ud As String, ByVal IV As Double, ByVal PU As Double, ByVal DC As Double, ByVal ImP As Double)
        With UgrTicket
            .Rows(Renglon).Cells("Clave").Value = Trim(Cp)
            .Rows(Renglon).Cells("Descripcion").Value = Trim(DP)
            .Rows(Renglon).Cells("Cantidad").Value = Trim(Cd)
            .Rows(Renglon).Cells("Unidad").Value = Trim(Ud)
            .Rows(Renglon).Cells("Iva").Value = IV
            .Rows(Renglon).Cells("PrecioUnitario").Value = PU
            .Rows(Renglon).Cells("Desc1").Value = DC
            .Rows(Renglon).Cells("Desc2").Value = 0
            .Rows(Renglon).Cells("Total").Value = ImP
        End With
    End Sub

    Private Sub UgrTicket_InitializeLayout(sender As Object, e As InitializeLayoutEventArgs) Handles UgrTicket.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With e.Layout.Override
            .FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains
            .AllowUpdate = Infragistics.Win.DefaultableBoolean.False
        End With

        e.Layout.Bands(0).Columns(0).Width = 150
        e.Layout.Bands(0).Columns(1).Width = 80
        e.Layout.Bands(0).Columns(2).Width = 400
        e.Layout.Bands(0).Columns(3).Width = 110
        e.Layout.Bands(0).Columns(4).Hidden = True
        e.Layout.Bands(0).Columns(5).Width = 110
        e.Layout.Bands(0).Columns(6).Hidden = True
        e.Layout.Bands(0).Columns(7).Hidden = True
        e.Layout.Bands(0).Columns(8).Width = 110
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        TablaTienda = OBJPRO.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
        DireFTP = Trim(TablaTienda.Rows(0).Item("FtpDir").ToString)
        UserFTP = Trim(TablaTienda.Rows(0).Item("FtpUser").ToString)
        PwdFTP = Trim(TablaTienda.Rows(0).Item("FtpPwd").ToString)

        REM Renombrar el Archivo Que se acaba de facturar y enviarlo a FTP-".txt"
        For Each ArchivoTexto In ArrayArchivos
            Dim NombreArc() As String = Split(ArchivoTexto, "\")
            Dim maximo As Integer = NombreArc.Count - 1
            Dim TipoArc() As String = Split(Trim(NombreArc(maximo)), "_")
            Try
                Dim ftp As System.Net.FtpWebRequest = CType(Net.FtpWebRequest.Create(DireFTP & Me.grbPeriodo.Text & "/" & OBJFUN.EscojeMes(Me.grbMes.Text) & "/" & NombreArc(maximo)), Net.FtpWebRequest)
                ftp.Credentials = New System.Net.NetworkCredential(UserFTP, PwdFTP)
                ftp.KeepAlive = False
                ftp.UseBinary = True
                ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile

                Dim BufferArchivo() As Byte = File.ReadAllBytes(ArchivoTexto)
                ' Subir archivo        
                Dim ElStream As System.IO.Stream = ftp.GetRequestStream
                Application.DoEvents()
                ElStream.Write(BufferArchivo, 0, BufferArchivo.Length)
                Application.DoEvents()
                ElStream.Close()
                Application.DoEvents()
                ElStream.Dispose()
                My.Computer.FileSystem.RenameFile(ArchivoTexto.ToString, Trim(NombreArc(maximo)) & ".pdf")
                MsgBox("El archivo se envío correctamente", MsgBoxStyle.Information, "FACTURAS-E")
            Catch ex As Exception
                MsgBox("Ftp: " & ex.Message, MsgBoxStyle.Information, "FACTURAS-E")
            End Try
        Next
        EsquemaConceptos()
        TotalesFacturar()
        CargaListaNotas()
        REM Renombrar el Archivo Que se acaba de facturar y enviarlo a FTP-".txt"
    End Sub
End Class