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

Public Class frmGlobalNutrisa

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Datos de Factura"
    Dim txtFactura As String
    Dim txtSerie As String
    Dim txtNumAprobacion As String
    Dim txtAmoAprob As String
    Dim txtLugarExpedicion As String
#End Region

#Region "VariablesGlobales"
    Dim FeReg As String
    Dim TIP As String
    Dim Tabla, TSucursal, TablaSucursal, TablaTienda, TabExpeTicket, facturado, yafac As Data.DataTable
    Dim ArrayArchivos As New ArrayList
    Dim fechventa As String
    Dim Timbrado As Boolean = False
    Dim FechaFactura As String
    Dim ArchivoGeneral As String
    Dim Transaccion, TpDev As Integer
    Dim td As Integer
    Dim va As Integer
    Dim va2 As Integer
    Dim hra As String
    Dim serie, folio As String
    Dim serieNC, folioNC As String
    Dim rou As Integer = 0
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

#Region "Anexo"
    Dim fp As String
    Dim Cadena As String
    Dim NumPiezas As Integer
    Dim NoCuenta As String

    Dim subG As Double = 0
    Dim decG As Double = 0
    Dim ivaG As Double = 0
    Dim iepsG As Double = 0
    Dim toG As Double = 0

    Dim subtk As Double = 0
    Dim dectk As Double = 0
    Dim decieps As Double = 0
    Dim decero As Double = 0
    Dim ivatk As Double = 0
    Dim iepstk As Double = 0
    Dim totk As Double = 0
#End Region

#Region "Sat"
    Dim TpRegimen As String
    Dim DesRegimen As String
    Dim Sat_UsoCFDI As String
    Dim e_uuid As String
#End Region

    Private Sub frmGlobalNutrisa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtFechaVenta.Value = Now.Date
        FechaFactura = Date.Now.ToString.Substring(0, 10)
        va = 0
        va2 = 0
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from bgxfacelec.satmpag ", 0, 1, As400)
        grbPagos.Value = "PUE"
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where tTipo3 = 'N' ", 0, 1, Motor)
        CargaGridConceptos()

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year
        Application.DoEvents()

        If DGFacturas.Rows.Count = 1 Then
            Button2.Visible = False
        End If
    End Sub

    Private Sub CargaGridConceptos()
        objpro.LLenaGrid(Me.UgrTicket, "select clavesucursal,NoTicket,FechaTicket,ClaveProducto as Clave,Tipo_Factura,DescripcionProducto as Descripcion," &
        "UnidadMedida as unidad,Cantidad as cantidad, PrecioVenta as PrecioUnitario,DV_Subtotal,DV_Descuento,DV_Subtotacondescuento," &
        "ProdExento, ieps8, ieps25, ImportePeso, Iva, porcIva, DV_Total, ClaveSat, tipo_factura " &
        "from nutrisa where clavesucursal = 0", "nutrisa", MotorBoutique)
        Me.UgrTicket.DataBind()

    End Sub

    Private Sub frmGlobalNutrisa_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
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
            objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
            Me.grbSucursal.Value = Nothing
            grbSucursal.Select()
        Catch ex As Exception
            MsgBox("Sucursal: " & ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        DevVenta.Value = 0
        DevIeps.Value = 0
        DevVenta0.Value = 0
        DevIps.Value = 0
        DevIva.Value = 0

        UgrTicket.DataSource = Nothing
        If grbTienda.Text <> "" Then
            If Me.grbSucursal.Text <> "" Then
                Try
                    fechventa = "01/" + objfun.EscojeMes(Me.grbMes.Text) + "/" + Me.grbPeriodo.Text
                    txtFechaVenta.Value = CDate(fechventa)
                    CargaListaNotas()
                Catch ex As Exception

                End Try
            Else
                MsgBox("Falta seleccionar sucursal", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta seleccionar tienda o razón social", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub CargaListaNotas()
        Try
            Application.DoEvents()
            TablaSucursal = Nothing
            TablaSucursal = objpro.ObtenDatos("select facsucursal, facfecha, facsts from facnutrisa where facsucursal = " & grbSucursal.Value & " and Month(facfecha) = " & objfun.EscojeMes(Me.grbMes.Text) & " and year(facfecha) = " & Me.grbPeriodo.Text & " order by facfecha ", Motor).Tables(0)
            If TablaSucursal.Rows.Count <> 0 Then
                Me.ChlisFact.Items.Clear()
                Dim i As Integer = 0
                For Each row As DataRow In TablaSucursal.Rows
                    Application.DoEvents()
                    Me.ChlisFact.Items.Add("Venta " & row.Item("facfecha").ToString)
                    If row.Item("facsts").ToString <> "" Then
                        ChlisFact.SetItemChecked(i, CheckState.Checked)
                    Else
                        ChlisFact.SetItemChecked(i, CheckState.Unchecked)
                    End If
                    i += 1
                Next
            Else
                MsgBox("A un no se carga los datos para facturar en este mes ", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub btnFactura_Click(sender As Object, e As EventArgs) Handles btnFactura.Click
        upb.Value = 0
        Application.DoEvents()
        UgrTicket.DataSource = Nothing
        inicializa()
        DGFacturas.Rows.Clear()
        If grbTienda.Text <> "" Then
            If grbSucursal.Text <> "" Then
                Dim EstadoVenta, term As String
                If objpro.DatoExiste("facfecha", "facnutrisa", "facsucursal = " & Me.grbSucursal.Value & " and day(facfecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(facfecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(facfecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) = True Then
                    EstadoVenta = objpro.DatoString("facfecha", "facnutrisa", "facsucursal = " & Me.grbSucursal.Value & " and day(facfecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(facfecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(facfecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " and facsts <> '' ", Motor)
                    If EstadoVenta = "" Then
                        Application.DoEvents()
                        objpro.LLenaGrid(Me.UgrTicket, "select Cantidad,UnidadMedida as Unidad,ClaveProducto as Clave,DescripcionProducto as Descripcion," &
                        "PrecioVenta as PrecioUnitario, DV_Subtotal as Importe, DV_Descuento as descu, ieps8, Iva, DV_Total, claveSat as codsat, tipo_factura, NoTicket " &
                        "from nutrisa where clavesucursal = " & grbSucursal.Value & " and ClaveProducto <> '535102' and  DV_Total > 0 and" &
                        " day(FechaTicket) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(FechaTicket) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(FechaTicket) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " order by NoTicket desc ", "nutrisa", MotorBoutique)
                        Application.DoEvents()

                        If UgrTicket.Rows.Count <> 0 Then
                            TIP = "VG"
                            TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
                            txtFactura = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                            txtSerie = TSucursal.Rows(0).Item("Seserie").ToString
                            txtNumAprobacion = TSucursal.Rows(0).Item("seNumAp").ToString
                            txtAmoAprob = TSucursal.Rows(0).Item("seAmoAp").ToString
                            txtLugarExpedicion = TSucursal.Rows(0).Item("seLugarExp").ToString

                            'inicializa()
                            lblDiaFactura.Text = txtFechaVenta.Value.ToString.Substring(0, 2) & "/" & txtFechaVenta.Value.ToString.Substring(3, 2) & "/" & txtFechaVenta.Value.ToString.Substring(6, 4)
                            For Each row In UgrTicket.Rows
                                Subtotal.Value += Format(CDbl(row.Cells("Importe").Value), "###,###.00")
                                Descuento.Value += Format(CDbl(row.Cells("Descu").Value), "###,###.00")
                                Iva.Value += Format(CDbl(row.Cells("iva").Value), "###,###.00")
                                IEPS.Value += Format(CDbl(row.Cells("ieps8").Value), "###,###.00")
                                Total.Value += Format(CDbl(row.Cells("DV_Total").Value), "###,###.00")
                                Application.DoEvents()
                            Next
                            Traslado.Value = (Iva.Value + IEPS.Value)
                        End If
                        subG = Subtotal.Value
                        decG = Descuento.Value
                        ivaG = Iva.Value
                        iepsG = IEPS.Value
                        toG = Total.Value
                    Else
                        MsgBox("La nota ya fue facturada ", MsgBoxStyle.Information, "Facturación .. ")
                    End If
                End If
            Else
                MsgBox("Falta Sucursal", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Razón Social a consultar", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub inicializa()
        lblDiaFactura.Text = ""
        Subtotal.Value = 0
        Descuento.Value = 0
        Iva.Value = 0
        IEPS.Value = 0
        Traslado.Value = 0
        Total.Value = 0

        DevVenta.Value = 0
        DevIeps.Value = 0
        DevVenta0.Value = 0
        DevIps.Value = 0
        DevIva.Value = 0

        txtFactura = ""
        txtSerie = ""
    End Sub


    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If grbTienda.Text <> "" Then
            If grbSucursal.Text <> "" Then
                If txtFechaVenta.Value IsNot Nothing Then
                    Formulario(frmTicketCte)
                    DGFacturas.Show()
                    If DGFacturas.Rows.Count > 1 Then
                        Button2.Visible = True
                        btnFacturar.Visible = False
                    End If
                Else
                    MsgBox("Seleccione una Fecha para agregar una factura de cliente", MsgBoxStyle.Information, "Facturación .. ")
                End If
            Else
                MsgBox("Seleccione una Sucursal para agregar una factura del cliente", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Seleccione una Tienda para agregar una factura del cliente", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        upb.Value = 0
        Application.DoEvents()
        inicializa()
        If grbTienda.Text <> "" Then
            If grbSucursal.Text <> "" Then
                Dim EstadoVenta, term As String
                If objpro.DatoExiste("facfecha", "facnutrisa", "facsucursal = " & Me.grbSucursal.Value & " and day(facfecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(facfecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(facfecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", Motor) = True Then
                    EstadoVenta = objpro.DatoString("facfecha", "facnutrisa", "facsucursal = " & Me.grbSucursal.Value & " and day(facfecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(facfecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(facfecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " and facsts <> '' ", Motor)
                    If EstadoVenta = "" Then
                        DGFacturas.Sort(DGFacturas.Columns("Ticket"), System.ComponentModel.ListSortDirection.Descending)
                        For i = 0 To Me.UgrTicket.Rows.Count - 1 'SACO TOTALES
                            For dg = 0 To Me.DGFacturas.Rows.Count - 1
                                If Me.DGFacturas.Rows(dg).Cells("Ticket").Value IsNot Nothing Then
                                    If Me.DGFacturas.Rows(dg).Cells("Ticket").Value = Me.UgrTicket.Rows(i).Cells("NoTicket").Value Then
                                        'Me.UgrTicket.Rows(i).Appearance.Reset()
                                        Me.UgrTicket.Rows(i).Appearance.ForeColor = Color.LightCoral
                                    Else
                                        'Me.UgrTicket.Rows(i).Appearance.Reset()
                                        Me.UgrTicket.Rows(i).Appearance.ForeColor = Color.Black
                                    End If
                                End If
                            Next
                        Next
                        ''UgrTicket.Refresh()
                        TIP = "VG"
                        TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
                        txtFactura = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                        txtSerie = TSucursal.Rows(0).Item("Seserie").ToString
                        txtNumAprobacion = TSucursal.Rows(0).Item("seNumAp").ToString
                        txtAmoAprob = TSucursal.Rows(0).Item("seAmoAp").ToString
                        txtLugarExpedicion = TSucursal.Rows(0).Item("seLugarExp").ToString

                        lblDiaFactura.Text = txtFechaVenta.Value.ToString.Substring(0, 2) & "/" & txtFechaVenta.Value.ToString.Substring(3, 2) & "/" & txtFechaVenta.Value.ToString.Substring(6, 4)

                        '''SUMO TODAS LAS FACTURA DE CLIENTE POR DETALLE PARA HACER UNA DIFERENCIA EN LOS TOTALES
                        '''
                        subtk = 0
                        dectk = 0
                        ivatk = 0
                        iepstk = 0
                        totk = 0

                        For dg = 0 To Me.DGFacturas.Rows.Count - 1
                            If Me.DGFacturas.Rows(dg).Cells("Ticket").Value IsNot Nothing Then
                                subtk += Me.DGFacturas.Rows(dg).Cells("Subtotal_Fac").Value
                                dectk += Me.DGFacturas.Rows(dg).Cells("Descuento_Fac").Value
                                'decieps += Me.DGFacturas.Rows(dg).Cells("Descuento_Ieps").Value
                                'decero += Me.DGFacturas.Rows(dg).Cells("Descuento_Cero").Value
                                ivatk += Me.DGFacturas.Rows(dg).Cells("Iva_Fac").Value
                                iepstk += Me.DGFacturas.Rows(dg).Cells("Ieps_Fac").Value
                                totk += Me.DGFacturas.Rows(dg).Cells("Total_Facturado").Value
                            End If
                        Next
                        'SACO POR DIFERENCIA LOS NUEVOS TOTALES
                        Subtotal.Value = subG - subtk
                        Descuento.Value = decG - dectk '- decieps - decero
                        Iva.Value = ivaG - ivatk
                        IEPS.Value = iepsG - iepstk
                        Total.Value = toG - totk
                        Traslado.Value = (Iva.Value + IEPS.Value)
                        MsgBox("Los totales se actualizaron correctamente ", MsgBoxStyle.Information, "Facturación .. ")
                        btnFacturar.Visible = True
                    Else
                        MsgBox("La nota ya fue facturada ", MsgBoxStyle.Information, "Facturación .. ")
                    End If
                End If
            Else
                MsgBox("Falta Sucursal", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Razón Social a consultar", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub Timbra()
        Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
        Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
        Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
        Dim FecVenta As String = a & m & d
        Dim n_serie As String
        Dim n_notacredito As String
        If UgrTicket.Rows.Count <> 0 Then
            Dim Directorios, RutFacGlobal, CarpTienda, CarpSucursal, RutNotaCredito, RutFacClientes, rutlogo As String
            Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, RutFac As String
            If MessageBox.Show("Esta seguro de Generar Factura", "FACTURAS GLOBALES", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                Timbrado = False
                upb.Value = 0
                upb.Maximum = 100

                TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", MotorBoutique).Tables(0)
                TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
                TRfc = TablaTienda.Rows(0).Item("trfc").ToString
                TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
                TCp = TablaTienda.Rows(0).Item("tcp").ToString
                TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
                descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
                direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
                RutFac = TablaTienda.Rows(0).Item("sForm1").ToString

                Directorios = TablaTienda.Rows(0).Item("sUbic").ToString
                RutFacClientes = TablaTienda.Rows(0).Item("sForm1").ToString
                RutFacGlobal = TablaTienda.Rows(0).Item("sForm2").ToString
                RutNotaCredito = TablaTienda.Rows(0).Item("sForm3").ToString
                CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
                CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
                rutlogo = TablaTienda.Rows(0).Item("rutLogo").ToString

                Dim CerFactura As String = TablaTienda.Rows(0).Item("sRCer1").ToString
                Dim Key As String = TablaTienda.Rows(0).Item("sRKey").ToString
                Dim Pwd As String = TablaTienda.Rows(0).Item("sPwd").ToString

                TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
                n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                n_serie = TSucursal.Rows(0).Item("Seserie").ToString

                RutFacGlobal = TablaTienda.Rows(0).Item("sForm2").ToString
                RutNotaCredito = TablaTienda.Rows(0).Item("sForm3").ToString
                CarpTienda = TablaTienda.Rows(0).Item("sCtien").ToString
                CarpSucursal = TablaTienda.Rows(0).Item("sCsuc").ToString
                Directorios = TablaTienda.Rows(0).Item("sUbic").ToString

                Dim f1, f2, f3, freal, hr As String
                f1 = txtFechaVenta.Text.ToString.Substring(0, 2)
                f2 = txtFechaVenta.Text.ToString.Substring(3, 2)
                f3 = txtFechaVenta.Text.ToString.Substring(6, 4)
                FeReg = txtFechaVenta.Text.ToString.Substring(6, 4) & f2 & f1

                Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)

                DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
                PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
                Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
                PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
                SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
                PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
                DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
                AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
                MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString

                ''''''nuevoooooooooooooooooooooooooooooooooooooooooo
                Dim DS As DataSet = New DataSet("DataSetFac")
                Dim Tabla1 As DataTable = DS.Tables.Add("DatosGen")

                Tabla1.Columns.Add("Erfc")
                Tabla1.Columns.Add("Erazonsoc")
                Tabla1.Columns.Add("Eregfis")
                Tabla1.Columns.Add("Edireccion")
                Tabla1.Columns.Add("Edircomp")
                Tabla1.Columns.Add("NoDoc")
                Tabla1.Columns.Add("foliofisc")
                Tabla1.Columns.Add("provcert")
                Tabla1.Columns.Add("tipocomp")
                Tabla1.Columns.Add("usocfdi")
                Tabla1.Columns.Add("metpago")
                Tabla1.Columns.Add("formpago")
                Tabla1.Columns.Add("exporta")
                Tabla1.Columns.Add("fechaemi")
                Tabla1.Columns.Add("lugaremi")
                Tabla1.Columns.Add("serieceremi")
                Tabla1.Columns.Add("Rrfc")
                Tabla1.Columns.Add("Rrazonsoc")
                Tabla1.Columns.Add("Rdireccion")
                Tabla1.Columns.Add("Rcp")
                Tabla1.Columns.Add("moneda")
                Tabla1.Columns.Add("sellodig")
                Tabla1.Columns.Add("sellosat")
                Tabla1.Columns.Add("complesat")
                Tabla1.Columns.Add("Rregimen")
                Tabla1.Columns.Add("Fsubtotal")
                Tabla1.Columns.Add("Fdescuento")
                Tabla1.Columns.Add("Fiva")
                Tabla1.Columns.Add("Ftotal")
                Tabla1.Columns.Add("cantidadl")
                Tabla1.Columns.Add("certSAT")
                Tabla1.Columns.Add("hryfechcer")
                Tabla1.Columns.Add("sucursal")
                Tabla1.Columns.Add("rieps")
                Tabla1.Columns.Add("FTasa08")

                Dim Fila As DataRow

                Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
                Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
                Fila("Erazonsoc") = TRazonSocial
                Fila("Eregfis") = TRFiscal + " - " + descreg.Trim
                Fila("Edireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
                Fila("Edircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

                Dim RFacImpresas As String
                RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

                Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & "-" & FeReg.ToString & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

                Dim TipoCambio As Int16 = 1
                upb.Value += 10

                'EXPORTACION: 01 - NO APLICA
                EasyOne_cfdi33_SetCfdi40(p, "01", "")

                Fila("exporta") = "01-No Aplica"

                Select Case TRFiscal
                    Case 601
                        TpRegimen = c_RegimenFiscal.GeneralDeLeyPersonasMorales
                        DesRegimen = "General De Ley Personas Morales"
                    Case 603
                        TpRegimen = c_RegimenFiscal.PersonasMoralesConFinesNoLucrativos
                        DesRegimen = "Personas Morales Con Fines No Lucrativos"
                    Case 605
                        TpRegimen = c_RegimenFiscal.SueldosYSalariosEIngresosAsimiladosASalarios
                        DesRegimen = "Sueldos Y Salarios E Ingresos Asimilados A Salarios"
                    Case 606
                        TpRegimen = c_RegimenFiscal.Arrendamiento
                        DesRegimen = "Arrendamiento"
                    Case 608
                        TpRegimen = c_RegimenFiscal.DemasIngresos
                        DesRegimen = "Demas Ingresos"
                    Case 609
                        TpRegimen = c_RegimenFiscal.Consolidacion
                        DesRegimen = "Consolidacion"
                    Case 610
                        TpRegimen = c_RegimenFiscal.ResidentesEnElExtranjeroSinEstablecimientoPermanenteEnMexico
                        DesRegimen = "Residentes En El Extranjero Sin Establecimiento Permanente En Mexico"
                    Case 611
                        TpRegimen = c_RegimenFiscal.IngresosPorDividendos
                        DesRegimen = "Ingresos Por Dividendos"
                    Case 612
                        TpRegimen = c_RegimenFiscal.PersonasFisicasConActividadesEmpresarialesYProfesionales
                        DesRegimen = "Personas Fisicas Con Actividades Empresariales Y Profesionales"
                    Case 614
                        TpRegimen = c_RegimenFiscal.IngresosPorIntereses
                        DesRegimen = "Ingresos Por Intereses"
                    Case 616
                        TpRegimen = c_RegimenFiscal.SinObligacionesFiscales
                        DesRegimen = "Sin Obligaciones Fiscales"
                    Case 607
                        TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                        DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
                    Case 621
                        TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                        DesRegimen = "Incorporacion Fiscal"
                    Case 626
                        TpRegimen = "626"
                        DesRegimen = "Régimen Simplificado de Confianza"
                End Select

                'Llenamos la sección del Emisor
                'EasyOne_cfdi33_SetEmisor(p, TRfc, "PLAZA DELI", "XXX")
                EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, TpRegimen)

                'Llenamos la sección del Receptor
                'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                If grbPagos.Value = "PPD" Then
                    fp = "POR DEFINIR"  '"99 - Otros"
                    EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PÚBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
                    EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616

                    Fila("Rcp") = TCp
                    Fila("Rrfc") = "XAXX010101000"
                    Fila("Rrazonsoc") = "PÚBLICO EN GENERAL"
                    Fila("Rdireccion") = ""
                    Fila("Rregimen") = "616-Sin Obligaciones Fiscales"
                    Fila("usocfdi") = "S01"
                    Fila("moneda") = "MXN"
                Else
                    fp = "EFECTIVO"  '"99 - Otros"
                    '' // Periodicidad 01 Diario| 02 Semanal | 03 Quincenal | 04 Mensual | 05 Bimensual
                    ''// Meses 01-12 Enero a Diciembre | 13-18 Enero-Febrero a Noviembre-Diciembre  // Año
                    EasyOne_cfdi33_SetInformacionGlobal40(p, "01", f2, f3)

                    EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
                    EasyOne_cfdi33_SetReceptor40(p, TCp, "616") 'regimen fiscal para global 616

                    Fila("Rcp") = TCp
                    Fila("Rrfc") = "XAXX010101000"
                    Fila("Rrazonsoc") = "PUBLICO EN GENERAL"
                    Fila("Rdireccion") = ""
                    Fila("Rregimen") = "616-Sin Obligaciones Fiscales"
                    Fila("usocfdi") = "S01"
                    Fila("moneda") = "MXN"
                End If


                Dim i As Integer
                Dim sb As Double = 0
                Dim des As Double = 0
                Dim iv As Double = 0
                Dim ieps As Double = 0
                Dim iiv As Double = 0
                Dim PuCa As Double = 0
                Dim Imp As Double = 0
                Dim bas As Double = 0
                Dim des1 As Double = 0
                Dim Descu16 As Double = 0
                Dim Descucero As Double = 0
                Dim Descuieps As Double = 0
                Dim sumbase As Double = 0
                Dim T16 As Double = 0
                Dim TCERO As Double = 0
                Dim TIEPS As Double = 0


                Dim Importe As Double = 0
                Dim ImporteIva As Double = 0
                Dim flag As Integer = 1
                Dim flag2 As Integer = 1

                Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
                Tabla2.Columns.Add("Cantidad")
                Tabla2.Columns.Add("Unidad")
                Tabla2.Columns.Add("UniSAT")
                Tabla2.Columns.Add("Descrip")
                Tabla2.Columns.Add("Punitario")
                Tabla2.Columns.Add("Importe")
                Dim Fila2 As DataRow
                Dim bandera As String = ""
                For i = 0 To Me.UgrTicket.Rows.Count - 1 'SACO TOTALES
                    For dg = 0 To Me.DGFacturas.Rows.Count - 1
                        If Me.DGFacturas.Rows(dg).Cells("Ticket").Value IsNot Nothing Then
                            If Me.DGFacturas.Rows(dg).Cells("Ticket").Value = Me.UgrTicket.Rows(i).Cells("NoTicket").Value Then
                                bandera = "Si"
                            Else
                                bandera = "NO"
                            End If
                        End If
                    Next
                    If bandera = "NO" Or DGFacturas.Rows.Count = 1 Then

                        Select Case Me.UgrTicket.Rows(i).Cells("tipo_factura").Value
                            Case "Iva Tasa16"
                                Dim puniv As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                                NumPiezas = Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                                T16 += puniv * NumPiezas

                                If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") - Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") > 0 Then
                                    Descu16 += Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                Else
                                    Descu16 += (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                                End If
                            Case "Iva Tasa0"
                                Dim punce As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                                NumPiezas = Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                                TCERO += punce * NumPiezas
                                If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") - Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") > 0 Then
                                    Descucero += Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                Else
                                    Descucero += (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                                End If
                                flag2 = 0
                            Case "IEPS"
                                Dim punie As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                                NumPiezas = Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                                TIEPS += punie * NumPiezas
                                If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") - Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") > 0 Then
                                    Descuieps += Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                Else
                                    Descuieps += (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.1)
                                End If
                        End Select
                    End If
                Next
                des = Descu16 + Descucero + Descuieps
                    If Me.Iva.Value <> 0 Then
                        Imp = 0
                        If (Format(T16, "###,###.00") - Format(Descu16, "###,###.00")) <> 0 Then
                            bas = Format((T16 - Descu16), "######.00")
                            Imp = Format(((T16 - Descu16) * 0.16), "######.00")
                            iv += Imp
                            If Descu16 <> 0 Then
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((T16), "######.00"), Format(T16, "######.00"), Format((Descu16), "######.00"))
                            Else
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((T16), "######.00"), Format(T16, "######.00"), "")
                            End If
                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))
                        Else
                            bas = Format((T16 - Descu16) - 0.1, "######.00")
                            Imp = Format(((T16 - Descu16) * 0.16), "###,###.00")
                            iv += Imp
                            If Descu16 <> 0 Then
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((T16), "######.00"), Format(T16, "######.00"), Format((Descu16), "######.00"))
                            Else
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((T16), "######.00"), Format(T16, "######.00"), "")
                            End If
                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.160000", Format(Imp, "######.00"))
                        End If
                        Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                        Fila2("Cantidad") = "1"
                        Fila2("Unidad") = "ACT"
                        Fila2("UniSAT") = Format(Descu16, "###,###.00")
                        Fila2("Descrip") = "VENTA"
                        Fila2("Punitario") = Format(T16, "###,###.00")
                        Fila2("Importe") = Format(T16, "###,###.00")
                        DS.Tables(1).Rows.Add(Fila2)
                    End If
                    If flag2 = 0 Then
                        Imp = 0
                        If (Format(TCERO, "###,###.00") - Format(Descucero, "###,###.00")) <> 0 Then
                            bas = (TCERO - Descucero)
                            Imp = ((TCERO - Descucero) * 0)
                            flag = 0
                            Dim im As String = Imp.ToString.Replace(".00", "0.00")
                            If Descucero <> 0 Then
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TCERO), "######.00"), Format(TCERO, "######.00"), Format((Descucero), "######.00"))
                            Else
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TCERO), "######.00"), Format(TCERO, "######.00"), "")
                            End If
                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.000000", im)
                        Else
                            bas = Format(TCERO, "###,###.00") - (Format(Descucero, "###,###.00") - 0.1)
                            Imp = ((TCERO - (Descucero - 0.1)) * 0)
                            flag = 0
                            Dim im As String = Imp.ToString.Replace(".00", "0.00")
                            If Descucero <> 0 Then
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TCERO), "######.00"), Format(TCERO, "######.00"), Format((Descucero), "######.00"))
                            Else
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TCERO), "######.00"), Format(TCERO, "######.00"), "")
                            End If
                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "002", "Tasa", "0.000000", im)
                        End If
                        Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                        Fila2("Cantidad") = "1"
                        Fila2("Unidad") = "ACT"
                        Fila2("UniSAT") = Format(Descucero, "###,###.00")
                        Fila2("Descrip") = "VENTA"
                        Fila2("Punitario") = Format(TCERO, "###,###.00")
                        Fila2("Importe") = Format(TCERO, "###,###.00")
                        DS.Tables(1).Rows.Add(Fila2)
                    End If
                    If Me.IEPS.Value <> 0 Then
                        Imp = 0
                        If (Format(TIEPS, "###,###.00") - Format(Descuieps, "###,###.00")) <> 0 Then
                            bas = Format((TIEPS - Descuieps), "###,###.00")
                            Imp = Format(((TIEPS - Descuieps) * 0.08), "###,###.00")
                            ieps += Imp
                            If Descuieps <> 0 Then
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TIEPS), "######.00"), Format(TIEPS, "######.00"), Descuieps)
                            Else
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TIEPS), "######.00"), Format(TIEPS, "######.00"), "")
                            End If
                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            EasyOne_cfdi33_AddTraslado(p, Format(bas, "######.00"), "003", "Tasa", "0.080000", Imp)
                        Else
                            bas = TIEPS - Format(Descuieps - 0.1, "###,###.00")
                            Imp = Format(((TIEPS - Descuieps - 0.1) * 0.08), "###,###.00")
                            ieps += Imp
                            If Descuieps <> 0 Then
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TIEPS), "######.00"), Format(TIEPS, "######.00"), Format((Descuieps), "######.00"))
                            Else
                                EasyOne_cfdi33_AddConcepto(p, "01010101", FeReg.ToString, "1", "ACT", "", "Venta", Format((TIEPS), "######.00"), Format(TIEPS, "######.00"), "")
                            End If
                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            If Imp < 0 Then
                                Dim completa As String
                                completa = "0" & Imp.ToString
                                EasyOne_cfdi33_AddTraslado(p, bas.ToString, "003", "Tasa", "0.080000", completa)
                            Else
                                EasyOne_cfdi33_AddTraslado(p, bas.ToString, "003", "Tasa", "0.080000", Format(Imp, "######.00"))
                            End If
                        End If
                        Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                        Fila2("Cantidad") = "1"
                        Fila2("Unidad") = "ACT"
                        Fila2("UniSAT") = Format(Descuieps, "###,###.00")
                        Fila2("Descrip") = "VENTA"
                        Fila2("Punitario") = Format(TIEPS, "###,###.00")
                        Fila2("Importe") = Format(TIEPS, "###,###.00")
                        DS.Tables(1).Rows.Add(Fila2)
                    End If

                    Dim t_traslado = Format((iv + ieps), "######.00")
                    EasyOne_cfdi33_SetComprobanteImpuestos(p, "", t_traslado)
                    'Agregamos los Impuestos
                    If Me.Iva.Value <> 0 Then
                        EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.160000", Format(iv, "######.00"))
                    End If

                    If flag = 0 Then
                        EasyOne_cfdi33_AddTrasladosTraslado(p, "002", "Tasa", "0.000000", "0.00")
                    End If

                    If Me.IEPS.Value <> 0 Then
                        EasyOne_cfdi33_AddTrasladosTraslado(p, "003", "Tasa", "0.080000", Format(ieps, "###,###.00"))
                    End If

                    NoCuenta = ""
                    Dim Total As Double = 0
                    Dim Subt As Double = 0
                    sb = T16 + TCERO + TIEPS

                    Subt = Format(sb, "###,###.00")
                    Total = Format(((Subt - des) + iv + ieps), "###,###.00")

                    Cadena = txtFechaVenta.Value.ToString.Substring(0, 10) & " Nota " & txtFactura & " - Importe - " & " $" & Str(FormatCurrency(Total))
                    Dim ValorTicket As String = Cadena
                    Fila("sucursal") = Cadena
                    Fila("Fsubtotal") = Format(Subt, "###,###.00")
                    Fila("Fdescuento") = Format(des, "###,###.00")
                    Fila("Fiva") = Format(iv, "###,###.00")
                    Fila("rieps") = Format(ieps, "###,###.00")
                    Fila("Ftotal") = Format(Total, "###,###.00")

                    Dim cantletra As String
                    cantletra = Letras(Total)
                    Fila("cantidadl") = cantletra

                    Dim metpg As String
                    Dim tipcom As String
                    Dim formpg As String
                    metpg = grbPagos.Value
                    tipcom = "I"

                    Fila("metpago") = metpg
                    If tipcom = "I" Then
                        Fila("tipocomp") = "I - Ingreso"
                    Else
                        Fila("tipocomp") = "E - Egreso"
                    End If
                    Fila("formpago") = fp

                    If grbPagos.Value = "PPD" Then
                        formpg = "99"
                        Fila("formpago") = "POR DEFINIR"
                    Else
                        formpg = "01"
                        Fila("formpago") = "EFECTIVO"
                    End If

                    Application.DoEvents()
                    upb.Value += 30  'Suma 40

                    'En la siguiente línea se deben colocar los nombres de los archivos de certificado de sellos del Emisor.
                    'Para fines de esta prueba se usarán los certificados de sellos del Emisor AAA010101AAA.
                    'Favor de colocar estos archivos (.CER y .KEY) en la misma carpeta donde se encuentra la aplicación.

                    'Timbra XML, que se encuentra en variable P
                    Dim x As Integer = 99
                    If va = 0 Then
                        hr = Now.ToLocalTime.ToString.Substring(11, 8)
                        If hra <> "" Then
                            'MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                            hr = hra
                        End If

                        freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

                        If (serie = "" And folio = "") Or (folio <> txtFactura) Then
                            'Llenamos la sección del comprobante
                            If des = 0 Then
                                EasyOne_cfdi33_SetComprobante(p, txtSerie, txtFactura, freal, formpg, "", Format((Subt), "######.00"), "", "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
                            Else
                                EasyOne_cfdi33_SetComprobante(p, txtSerie, txtFactura, freal, formpg, "", Format((Subt), "######.00"), Format((des), "######.00"), "MXN", TipoCambio, Format((Total), "######.00"), tipcom, metpg, TCp, "")
                            End If

                            Dim folf As Integer = 0 'consulto el ultimo folio de serie
                            folf = CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor))
                            If CInt(txtFactura) > folf Then

                            'Enviamos el CFDI al metodo de PRUEBAS
                            'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                            'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL")
                            EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

                            Dim b As Integer
                                b = p
                                x = EasyOne_FirmaComprobante(b, CerFactura, Key, Pwd)
                                p = b
                                b = 0
                                Thread.Sleep(10000) 'espera por 10 segundos
                                If x = 0 Then

                                objpro.Indicacion("update facnutrisa set facsts = 'A' where facsucursal = " & grbSucursal.Value & " and day(facfecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(facfecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(facfecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "   ", Motor)
                                objpro.Indicacion("Update Serie set seCant = " & CInt(txtFactura) & " Where seSerie = '" & txtSerie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                                upb.Value += 10 'Suma 60

                                Dim resultadoXML As String = [String].Empty
                                    resultadoXML = EasyOne_LeeResultado(p, 8)
                                    'Obtenemos el XML completo y timbrado
                                    Console.WriteLine(resultadoXML)

                                    Application.DoEvents()
                                    Dim archivoXMLPDF As String = ""
                                    Dim archivoREPX As String = ""
                                    Dim archivoPDF As String = ""

                                    IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & "-" & FeReg.ToString & ".xml"), resultadoXML)
                                    upb.Value += 10 'Suma 50

                                    va += 1
                                    hra = hr
                                    serie = txtSerie
                                    folio = txtFactura
                                    'Si el resultado es 0 = El CFDI se procesó correctamente
                                    If x = 0 And va = 1 Then
                                        '''''''''''''''''''''''''''''' RECORRO XML''''''''''''''''''''''''''''
                                        Dim Version_xml As String
                                        'Dim VarConceptos As XmlNodeList
                                        Dim version As String
                                        Dim uuid As String
                                        Dim ftimb As String
                                        Dim rfcprov As String
                                        Dim sellocfd As String
                                        Dim nocert As String

                                        Dim nav As XPathNavigator
                                        Dim documento As XmlDocument = New XmlDocument()
                                        Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
                                        documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & "-" & FeReg.ToString & ".xml"))
                                        VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                                        VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                                        Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText

                                        If Version_xml = "4.0" Then
                                            Fila("NoDoc") = documento.SelectSingleNode("/cfdi:Comprobante/@Folio", VarManager).InnerText
                                            Fila("sellodig") = documento.SelectSingleNode("/cfdi:Comprobante/@Sello", VarManager).InnerText
                                            Fila("lugaremi") = documento.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", VarManager).InnerText
                                            Fila("fechaemi") = documento.SelectSingleNode("/cfdi:Comprobante/@Fecha", VarManager).InnerText
                                            Fila("serieceremi") = documento.SelectSingleNode("/cfdi:Comprobante/@NoCertificado", VarManager).InnerText
                                            uuid = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", VarManager).InnerText
                                            Fila("foliofisc") = uuid
                                            rfcprov = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", VarManager).InnerText
                                            Fila("provcert") = rfcprov
                                            Fila("sellosat") = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloSAT", VarManager).InnerText
                                            nocert = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@NoCertificadoSAT", VarManager).InnerText
                                            Fila("certSAT") = nocert
                                            ftimb = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", VarManager).InnerText
                                            Fila("hryfechcer") = ftimb
                                            version = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@Version", VarManager).InnerText
                                            sellocfd = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloCFD", VarManager).InnerText
                                            Fila("complesat") = "|" + version + "|" + uuid + "|" + ftimb + "|" + rfcprov + "||" + sellocfd + "|" + nocert + "||"

                                            RutaLogo = rutlogo 'RutFacGlobal & "\RecursosCFD33\Penguin.bmp"
                                            RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & "-" & FeReg.ToString & ".xml.png")

                                            DS.Tables(0).Rows.Add(Fila)
                                            Dim objreport As New CRGlobalNut

                                            objreport.SetParameterValue("Logo", RutaLogo)
                                            objreport.SetParameterValue("QR", RutaQR)
                                            Dim rpFact As New CRGlobalNut
                                            rpFact.SetDataSource(DS)
                                            rpFact.SetParameterValue("QR", RutaQR)
                                            rpFact.SetParameterValue("Logo", RutaLogo)
                                            GlobalNut.CrystalReportViewer1.ReportSource = objreport
                                            GlobalNut.CrystalReportViewer1.ReportSource = rpFact
                                            GlobalNut.WindowState = FormWindowState.Maximized
                                            GlobalNut.Show()

                                            upb.Value += 10 'Suma 70
                                            Dim Ruta As String = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie & "-" & txtFactura & "-" & FeReg.ToString & ".pdf")
                                            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                                            Dim cryRpt As New ReportDocument

                                            Try
                                                Dim CrExportOptions As ExportOptions
                                                Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                                                Dim crf As New PdfRtfWordFormatOptions()
                                                CrDiskFileDestinationOptions.DiskFileName = Ruta
                                                CrExportOptions = rpFact.ExportOptions
                                                With CrExportOptions
                                                    .ExportDestinationType = ExportDestinationType.DiskFile
                                                    .ExportFormatType = ExportFormatType.PortableDocFormat
                                                    .DestinationOptions = CrDiskFileDestinationOptions
                                                    .FormatOptions = CrFormatTypeOptions
                                                End With
                                                rpFact.Export()
                                                rpFact.Close()
                                                rpFact.Dispose()
                                                upb.Value += 10 'Suma 80 
                                            Catch ex As Exception
                                                MsgBox(ex.ToString)
                                            End Try

                                            If grbPagos.Value = "PPD" Then

                                                Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF As String
                                                Fecha11 = txtFechaVenta.Text.ToString.Substring(0, 2)
                                                Fecha22 = txtFechaVenta.Text.ToString.Substring(3, 2)
                                                Fecha33 = txtFechaVenta.Text.ToString.Substring(6, 4)
                                                FePnotasF = Fecha33 & Fecha22 & Fecha11
                                                fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                                                FeRegistro = Now.Date.ToString.Substring(6, 4) & Now.Date.ToString.Substring(3, 2) & Now.Date.ToString.Substring(0, 2) ''txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha22 & Fecha11
                                                Fecha = fechareal

                                                Dim cantidadconletra As String = ""
                                                Dim FeCan = 0
                                                Dim FeMotCan As String = ""
                                                Dim ferisr = 0
                                                Dim feriva = 0
                                                Dim Tipo As String = "G"
                                                Dim Estatus As String = 1
                                                Dim PorIva As Double = 16.0
                                                Dim ts As String = Trim(Me.grbTienda.Value) & Trim(Str(Me.grbSucursal.Value))
                                                ts = Trim(ts)
                                                Dim sucur As Integer
                                                sucur = CInt(Me.grbSucursal.Value.ToString.Substring(1, 4))

                                                Dim Subtotal = Me.Subtotal.Value
                                                Dim Iva = Me.Iva.Value
                                                Dim Total2 = Me.Total.Value
                                                Dim Descuento = Me.Descuento.Value
                                                Dim totalabono As Integer
                                                totalabono = 0
                                                Dim ExisteClave As String
                                                ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = 'XAXX010101000' ", As400))

                                                objpro.Indicacion("Insert into bgxfacelec.FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid," &
                                                " feFecha,feHora,Cid,FeSubt,fePorIva,FeIva, " &
                                                " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
                                                " FeCanlet,FeAbonos,FeEstatu,FeReg,fefeccan, femotcan, feTip, feuuid, ferisr, feriva) " &
                                                " values (" & FeReg & "," & grbTienda.Value & "," & ts & "," & CInt(txtFactura) & ",'" & txtSerie & "'," & sucur & ",'" & FeReg & "'," &
                                                " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ", " & Subtotal & ", " & CDbl(PorIva) & "," & Iva & "," &
                                                " " & Descuento & "," & Descuento & ", " & Descuento & ", " & Descuento & ", " & Total2 & ", " &
                                                " '" & cantidadconletra & "', " & totalabono & "," & Estatus & "," & FeRegistro & ",'" & FeCan & "', '" & FeMotCan & "','" & Tipo & "','" & uuid & "', " & ferisr & "," & feriva & ")", As400)
                                            End If


                                            If DevVenta.Value <> 0 Or DevIeps.Value <> 0 Or DevVenta0.Value <> 0 Then
                                                Application.DoEvents()
                                                REM e_uuid = "4AC99C4A-36E7-4AB9-8FDC-3A09043DBF99"
                                                NotaCredito(freal, TRazonSocial, TRfc, TRFiscal, RutNotaCredito, CarpTienda, CarpSucursal, Directorios, CerFactura, Key, Pwd, uuid, TCp, DevVenta.Value, DevIeps.Value, DevVenta0.Value)
                                                Application.DoEvents()
                                            End If
                                            upb.Value += 20 'Suma 100
                                            BorrarRegistro()
                                        btnFacturar.Visible = True
                                        DGFacturas.Rows.Clear()
                                        Button2.Visible = False
                                        va = 0
                                        Else
                                            MsgBox("Archivo XML no es una factura version 4.0")
                                        End If 'Cierra validacion de timbrado de pac
                                    Else
                                        'En caso de error mostrar el detalle.
                                        Dim resultadoXML2 As String = [String].Empty
                                        resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                        Console.WriteLine(resultadoXML2)
                                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                        btnFacturar.Visible = True
                                    End If 'Cierra validacion de generar xml
                                Else
                                    btnFacturar.Visible = True
                                    Dim resultadoXML2 As String = [String].Empty
                                    resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                    MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                    hra = hr
                                End If
                            Else
                                MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
                                btnFacturar.Visible = True
                            End If
                        Else
                            MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
                            btnFacturar.Visible = True
                        End If
                    Else
                        MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
                        btnFacturar.Visible = True
                    End If
                Else
                    btnFacturar.Visible = True
            End If 'Cierra validacion de si desea generar factura
        Else
            btnFacturar.Visible = True
        End If  'Cierra validación de articulos en pantalla
        CargaListaNotas()
    End Sub

    Private Sub BorrarRegistro()
        UgrTicket.DataSource = Nothing
        limpiar()
        Subtotal.Value = "0.0"
        Descuento.Value = "0.0"
        Iva.Value = "0.0"
        Total.Value = "0.0"
        IEPS.Value = "0.0"
        Traslado.Value = "0.0"
    End Sub

    Private Sub limpiar()
        txtSerie = ""
        txtFactura = ""
        txtNumAprobacion = ""
        txtAmoAprob = ""
        txtLugarExpedicion = ""

        DevVenta.Value = 0
        DevIeps.Value = 0
        DevVenta0.Value = 0
        DevIps.Value = 0
        DevIva.Value = 0

        subtk = 0
        dectk = 0
        ivatk = 0
        iepstk = 0
        totk = 0
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Public Function NotaCredito(ByVal fechaNC As String, ByVal n_TRazonSocial As String, ByVal n_TRfc As String, ByVal n_TRFiscal As String, ByVal n_RutNotaCredito As String,
                                ByVal n_CarpTienda As String, ByVal n_CarpSucursal As String, ByVal n_Directorios As String, ByVal cer As String, ByVal key As String,
                                ByVal psw As String, ByVal n_uuid As String, n_LExpedicion As String, ByVal n_devVen As Double, ByVal n_devIeps As Double, ByVal n_devVtacero As Double)
        Timbrado = False
        td = 2
        Dim FechaHora As String = Date.Now.ToString.Substring(10, 10)
        Dim n_serie As String
        Dim n_notacredito As String
        DesRegimen = ""
        NumPiezas = 0

        'upb.Value = 0
        'upb.Maximum = 100

        TIP = "NC"
        Dim TRazonSocial, TRfc, TRFiscal, descreg, TCp, direccion, RutFac As String
        TablaTienda = objpro.ObtenDatos("Select * from CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
        TRazonSocial = TablaTienda.Rows(0).Item("trsocial").ToString
        TRfc = TablaTienda.Rows(0).Item("trfc").ToString
        TCp = TablaTienda.Rows(0).Item("tcp").ToString
        TRFiscal = TablaTienda.Rows(0).Item("satRegimen").ToString
        descreg = TablaTienda.Rows(0).Item("sRFiscal").ToString
        direccion = TablaTienda.Rows(0).Item("sRFiscal").ToString
        RutFac = TablaTienda.Rows(0).Item("sForm1").ToString
        TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
        n_notacredito = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
        n_serie = TSucursal.Rows(0).Item("Seserie").ToString

        Dim DSNC As DataSet = New DataSet("DataSetFac")
        Dim Tabla3 As DataTable = DSNC.Tables.Add("DatosNotaCred")

        Tabla3.Columns.Add("Nrfc")
        Tabla3.Columns.Add("Nrazonsoc")
        Tabla3.Columns.Add("Nregfis")
        Tabla3.Columns.Add("Ndireccion")
        Tabla3.Columns.Add("Ndircomp")
        Tabla3.Columns.Add("Ndoc")
        Tabla3.Columns.Add("Nfolfisc")
        Tabla3.Columns.Add("Nprovcert")
        Tabla3.Columns.Add("Ntipocomp")
        Tabla3.Columns.Add("Nusocfdi")
        Tabla3.Columns.Add("Nmetpag")
        Tabla3.Columns.Add("Nformpag")
        Tabla3.Columns.Add("Nexporta")
        Tabla3.Columns.Add("Nfechaemi")
        Tabla3.Columns.Add("Nlugaremi")
        Tabla3.Columns.Add("Nserieceremi")
        Tabla3.Columns.Add("Nrrfc")
        Tabla3.Columns.Add("Nrrazonsoc")
        Tabla3.Columns.Add("Nrdireccion")
        Tabla3.Columns.Add("Nrcp")
        Tabla3.Columns.Add("Nmoneda")
        Tabla3.Columns.Add("Nsellodig")
        Tabla3.Columns.Add("Nsellosat")
        Tabla3.Columns.Add("Ncomplesat")
        Tabla3.Columns.Add("Nrregimen")
        Tabla3.Columns.Add("Nfsubtotal")
        Tabla3.Columns.Add("Ndescuento")
        Tabla3.Columns.Add("Nfiva")
        Tabla3.Columns.Add("Nieps")
        Tabla3.Columns.Add("Nftotal")
        Tabla3.Columns.Add("Ncantidadl")
        Tabla3.Columns.Add("NcertSAT")
        Tabla3.Columns.Add("Nhryfechcer")
        Tabla3.Columns.Add("Nsucursal")

        Dim Fila As DataRow

        Fila = DSNC.Tables(0).NewRow() ' Acá insertas una fila nueva
        Fila("Nrfc") = n_TRfc  ' Acá insertas valores a la nueva fila
        Fila("Nrazonsoc") = TRazonSocial
        Fila("Nregfis") = TRFiscal + " - " + descreg.Trim
        Fila("Ndireccion") = TablaTienda.Rows(0).Item("tdir").ToString + " " + TablaTienda.Rows(0).Item("tnumext").ToString + " " + TablaTienda.Rows(0).Item("tcolonia").ToString
        Fila("Ndircomp") = TablaTienda.Rows(0).Item("tmun").ToString + " " + TablaTienda.Rows(0).Item("testado").ToString + " " + TablaTienda.Rows(0).Item("tlocal").ToString + " " + TablaTienda.Rows(0).Item("tpais").ToString

        Dim RFacImpresas2 As String
        RFacImpresas2 = n_Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)
        Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg.ToString & ".xml")

        Dim TipoCambio As Int16 = 1
        Fila("Nexporta") = "01-No Aplica"
        '(1)
        'EXPORTACION: 01 - NO APLICA
        EasyOne_cfdi33_SetCfdi40(pN, "01", "")

        '######################## PROCESO DE CANCELACION
        'Iniciamos el proceso de cancelacion
        'Certificado Sello Digital .cer
        'LLave Privada .key
        'Password llave privada
        'UUID
        'Rfc Emisor
        'Clave del Agente
        'SoftwareId
        'Rfc receptor
        'Total
        'Motivo de cancelacion (Nuevo)

        Dim metpg As String
        Dim tipcom As String
        Dim formpg As String

        Fila("Ntipocomp") = "E - EGRESO"

        If grbPagos.Value = "PPD" Then
            formpg = "99"
            metpg = "PPD"
            tipcom = "E"
            Fila("Nformpag") = "POR DEFINIR" '"Otros"
        Else
            formpg = "01"
            metpg = "PUE"
            tipcom = "E"
            Fila("Nformpag") = "EFECTIVO" '"Otros"
        End If
        Fila("Nmetpag") = metpg
        'Tipo de relacion
        EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")
        'CFDI Relacionado
        EasyOne_cfdi33_AddCfdiRelacionado(pN, n_uuid)

        Select Case n_TRFiscal
            Case 601
                TpRegimen = c_RegimenFiscal.GeneralDeLeyPersonasMorales
                DesRegimen = "General De Ley Personas Morales"
            Case 603
                TpRegimen = c_RegimenFiscal.PersonasMoralesConFinesNoLucrativos
                DesRegimen = "Personas Morales Con Fines No Lucrativos"
            Case 605
                TpRegimen = c_RegimenFiscal.SueldosYSalariosEIngresosAsimiladosASalarios
                DesRegimen = "Sueldos Y Salarios E Ingresos Asimilados A Salarios"
            Case 606
                TpRegimen = c_RegimenFiscal.Arrendamiento
                DesRegimen = "Arrendamiento"
            Case 608
                TpRegimen = c_RegimenFiscal.DemasIngresos
                DesRegimen = "Demas Ingresos"
            Case 609
                TpRegimen = c_RegimenFiscal.Consolidacion
                DesRegimen = "Consolidacion"
            Case 610
                TpRegimen = c_RegimenFiscal.ResidentesEnElExtranjeroSinEstablecimientoPermanenteEnMexico
                DesRegimen = "Residentes En El Extranjero Sin Establecimiento Permanente En Mexico"
            Case 611
                TpRegimen = c_RegimenFiscal.IngresosPorDividendos
                DesRegimen = "Ingresos Por Dividendos"
            Case 612
                TpRegimen = c_RegimenFiscal.PersonasFisicasConActividadesEmpresarialesYProfesionales
                DesRegimen = "Personas Fisicas Con Actividades Empresariales Y Profesionales"
            Case 614
                TpRegimen = c_RegimenFiscal.IngresosPorIntereses
                DesRegimen = "Ingresos Por Intereses"
            Case 616
                TpRegimen = c_RegimenFiscal.SinObligacionesFiscales
                DesRegimen = "Sin Obligaciones Fiscales"
            Case 607
                TpRegimen = c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
            Case 621
                TpRegimen = c_RegimenFiscal.IncorporacionFiscal
                DesRegimen = "Incorporacion Fiscal"
        End Select

        'Llenamos la sección del Emisor
        EasyOne_cfdi33_SetEmisor(pN, n_TRfc, TRazonSocial, TpRegimen)

        'Llenamos la sección del Receptor
        'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
        EasyOne_cfdi33_SetReceptor(pN, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
        EasyOne_cfdi33_SetReceptor40(pN, n_LExpedicion, "616") 'regimen fiscal para global 616
        Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")

        Fila("Nrcp") = TCp
        Fila("Nrrfc") = "XAXX010101000"
        Fila("Nrrazonsoc") = "PUBLICO EN GENERAL"
        Fila("Nrdireccion") = ""
        Fila("Nrregimen") = "616-Sin Obligaciones Fiscales"
        Fila("Nusocfdi") = "S01"
        Fila("Nmoneda") = "MXN"

        Dim Put16 As Double = Format((Me.DevVenta.Value / 1.16), "###,###.00") 'devolucion con iva, se le quita
        Dim Put8 As Double = Format((Me.DevIeps.Value / 1.08), "###,###.00")
        Dim Put0 As Double = Format((Me.DevVenta0.Value / 1.0), "###,###.00")

        Dim ieps As Double = 0
        Dim totales As Double = 0
        Dim i As Integer = 0
        Dim Impuesto As String = ""
        Dim TasaOCuota As String = ""
        Dim TipoFactor As String = ""
        Dim Impu As String = ""
        Dim Import As Double = 0
        Dim Imp As Double = 0
        Dim bas As Double = 0
        Dim des1 As Double = 0
        Dim ToC As String = ""
        Dim TFac As String = ""
        Dim Tabla4 As DataTable = DSNC.Tables.Add("DatNota")
        Tabla4.Columns.Add("Ncantidad")
        Tabla4.Columns.Add("Nunidad")
        Tabla4.Columns.Add("NuniSAT")
        Tabla4.Columns.Add("Ndescip")
        Tabla4.Columns.Add("Npunitario")
        Tabla4.Columns.Add("Nimporte")
        Tabla4.Columns.Add("NclaveSAT")
        Dim Fila3 As DataRow
        If Put16 <> 0 Then
            EasyOne_cfdi33_AddConcepto(pN, "84111506", FeReg.ToString, "1", "ACT", "SERVICIO", "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con tasa del 16 % y referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Put16, "###,###.00"), Format(Put16, "######.00"), Format(Put16, "######.00"), "")
            EasyOne_cfdi33_SetConcepto40(pN, "02")

            Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
            Fila3("Ncantidad") = "1"
            Fila3("Nunidad") = "SERVICIO"
            Fila3("NuniSAT") = "ACT"
            Fila3("Ndescip") = "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con tasa del 16 % y referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Put16, "###,###.00")
            Fila3("Npunitario") = Put16
            Fila3("Nimporte") = Put16
            Fila3("NclaveSAT") = "84111506"
            DSNC.Tables(1).Rows.Add(Fila3)

            bas = Format(Put16, "######.00")
            Imp = Format((Put16 * 0.16), "######.00")
            iv += Imp
            EasyOne_cfdi33_AddTraslado(pN, bas.ToString, "002", "Tasa", "0.160000", Imp.ToString)

            i += 1
        End If

        If Put8 <> 0 Then
            EasyOne_cfdi33_AddConcepto(pN, "84111506", FeReg.ToString, "1", "ACT", "SERVICIO", "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con tasa del 8 % y referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Put8, "###,###.00"), Format(Put8, "######.00"), Format(Put8, "######.00"), "")
            EasyOne_cfdi33_SetConcepto40(pN, "02")

            Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
            Fila3("Ncantidad") = "1"
            Fila3("Nunidad") = "SERVICIO"
            Fila3("NuniSAT") = "ACT"
            Fila3("Ndescip") = "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con tasa del 8 % y referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Put8, "###,###.00")
            Fila3("Npunitario") = Put8
            Fila3("Nimporte") = Put8
            Fila3("NclaveSAT") = "84111506"
            DSNC.Tables(1).Rows.Add(Fila3)

            bas = Format(Put8, "######.00")
            Imp = Format((Put8 * 0.08), "######.00")
            ieps += Imp

            EasyOne_cfdi33_AddTraslado(pN, bas.ToString, "003", "Tasa", "0.080000", Imp.ToString)
            i += 1
        End If


        If Put0 <> 0 Then
            EasyOne_cfdi33_AddConcepto(pN, "84111506", FeReg.ToString, "1", "ACT", "SERVICIO", "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con tasa del 0 % y referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Put0, "###,###.00"), Format(Put0, "######.00"), Format(Put0, "######.00"), "")
            EasyOne_cfdi33_SetConcepto40(pN, "02")

            Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
            Fila3("Ncantidad") = "1"
            Fila3("Nunidad") = "SERVICIO"
            Fila3("NuniSAT") = "ACT"
            Fila3("Ndescip") = "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10) & " con tasa del 0 % y referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Put0, "###,###.00")
            Fila3("Npunitario") = Put0
            Fila3("Nimporte") = Put0
            Fila3("NclaveSAT") = "84111506"
            DSNC.Tables(1).Rows.Add(Fila3)

            bas = Format(Put0, "######.00")
            Imp = Format((Put0 * 0), "######.00")
            iv += Imp
            EasyOne_cfdi33_AddTraslado(pN, bas.ToString, "002", "Tasa", "0.000000", Imp.ToString)
        End If

        Dim total As Double = 0
        Dim PUNI As Double = 0
        total = Format((Put16 + Put8 + Put0) + (iv + ieps), "######.00")

        PUNI = Format((Put16 + Put8 + Put0), "######.00")

        Dim t_traslado = Format((iv + ieps), "######.00")
        t_traslado = t_traslado.ToString.Replace(",", "")
        EasyOne_cfdi33_SetComprobanteImpuestos(pN, "", t_traslado)

        If Put16 <> 0 Then
            EasyOne_cfdi33_AddTrasladosTraslado(pN, "002", "Tasa", "0.160000", Format(iv, "######.00"))
        End If

        If Put8 <> 0 Then
            EasyOne_cfdi33_AddTrasladosTraslado(pN, "003", "Tasa", "0.080000", Format(ieps, "######.00"))
        End If

        If Put0 <> 0 Then
            EasyOne_cfdi33_AddTrasladosTraslado(pN, "002", "Tasa", "0.000000", "0.00")
        End If

        Dim ValorTicket As String = "Devoluciones del día " & txtFechaVenta.Value.ToString.Substring(0, 10)
        NoCuenta = ""
        Dim Subt As Double = PUNI
        totales = Format(((Subt) + iv + ieps), "###,###.00")
        Fila("Nfsubtotal") = Subt
        Fila("Nfiva") = iv
        Fila("nieps") = ieps
        Fila("Nftotal") = totales

        Dim cantl As String
        cantl = Letras(totales)
        Fila("Ncantidadl") = cantl

        'Timbra XML, que se encuentra en variable P
        Dim nc As Integer = 99
        If va2 = 0 Then
            If (serieNC = "" And folioNC = "") Or (n_notacredito <> folioNC) Then
                'Llenamos la sección del comprobante (2)
                EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, fechaNC, "01", "", PUNI.ToString, "", "MXN", TipoCambio, total.ToString, tipcom, metpg, n_LExpedicion, "")

                Dim folf As Integer = 0 'consulto el ultimo folio de serie
                folf = CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor))
                If CInt(n_notacredito) > folf Then
                    'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, "RHUUMRGL", "RHUUMRGL")
                    EasyOne_SetInvoiceOneFachadaInfo(pN, "RHUUMRGL", "RHUUMRGL")

                    Dim bN As Integer
                    bN = pN
                    nc = EasyOne_FirmaComprobante(bN, cer, key, psw)
                    pN = bN
                    bN = 0
                    Thread.Sleep(10000) 'espera por 10 segundos
                    If nc = 0 Then
                        '''''''''''''''''''''''''''''''''''''''''''''''''''
                        objpro.Indicacion("Update Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & " and seTipo = '" & TIP & " and seEstat = 1", Motor)
                        '''''nuevoooooooooooooooooooooooooooooooooooooooooo
                        Dim resultadoXMLNC As String = [String].Empty

                        'Obtenemos el XML completo y timbrado
                        resultadoXMLNC = EasyOne_LeeResultado(pN, 8)

                        Console.WriteLine(resultadoXMLNC)
                        Dim archivoXMLPDFNC As String = ""
                        Dim archivoREPXNC As String = ""
                        Dim archivoPDFNC As String = ""

                        IO.File.WriteAllText(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg.ToString & ".xml"), resultadoXMLNC)

                        va2 += 1
                        serieNC = n_serie
                        folioNC = n_notacredito

                        If nc = 0 And va2 = 1 Then
                            Application.DoEvents()
                            '''''''''''''''''''''''''''''' RECORRO XML NOTA CREDITO''''''''''''''''''''''''''''
                            Dim Version_xmlNC As String
                            Dim versionNC As String
                            Dim uuidNC As String
                            Dim ftimbNC As String
                            Dim rfcprovNC As String
                            Dim sellocfdNC As String
                            Dim nocertNC As String

                            Dim documentoNC As XmlDocument = New XmlDocument()
                            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documentoNC.NameTable)
                            documentoNC.Load(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg.ToString & ".xml"))
                            VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                            Version_xmlNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                            If Version_xmlNC = "4.0" Then
                                Fila("Ndoc") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Folio", VarManager).InnerText
                                Fila("Nsellodig") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Sello", VarManager).InnerText
                                Fila("Nlugaremi") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", VarManager).InnerText
                                Fila("Nfechaemi") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@Fecha", VarManager).InnerText
                                Fila("Nserieceremi") = documentoNC.SelectSingleNode("/cfdi:Comprobante/@NoCertificado", VarManager).InnerText
                                uuidNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", VarManager).InnerText
                                Fila("Nfolfisc") = uuidNC
                                rfcprovNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", VarManager).InnerText
                                Fila("Nprovcert") = rfcprovNC
                                Fila("Nsellosat") = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloSAT", VarManager).InnerText
                                nocertNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@NoCertificadoSAT", VarManager).InnerText
                                Fila("NcertSAT") = nocertNC
                                ftimbNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", VarManager).InnerText
                                Fila("Nhryfechcer") = ftimbNC
                                versionNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@Version", VarManager).InnerText
                                sellocfdNC = documentoNC.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloCFD", VarManager).InnerText
                                Fila("Ncomplesat") = "|" + versionNC + "|" + uuidNC + "|" + ftimbNC + "|" + rfcprovNC + "||" + sellocfdNC + "|" + nocertNC + "||"

                                RutaQRN = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg.ToString & ".xml.png")

                                DSNC.Tables(0).Rows.Add(Fila)
                                Dim objrep As New CRNotaGlobalNut
                                objrep.SetParameterValue("NLOGO", RutaLogo)
                                objrep.SetParameterValue("NQR", RutaQRN)
                                Dim NotaC As New CRNotaGlobalNut
                                NotaC.SetDataSource(DSNC)
                                NotaC.SetParameterValue("NQR", RutaQRN)
                                NotaC.SetParameterValue("NLOGO", RutaLogo)
                                GNotaNut.CrystalReportViewer1.ReportSource = objrep
                                GNotaNut.CrystalReportViewer1.ReportSource = NotaC
                                GNotaNut.WindowState = FormWindowState.Maximized
                                GNotaNut.Show()

                                Dim Ruta As String = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & "-" & FeReg.ToString & ".pdf")
                                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                                Dim cryRpt As New ReportDocument

                                Try
                                    Dim CrExportOptions As ExportOptions
                                    Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                                    Dim crf As New PdfRtfWordFormatOptions()
                                    CrDiskFileDestinationOptions.DiskFileName = Ruta
                                    CrExportOptions = NotaC.ExportOptions
                                    With CrExportOptions
                                        .ExportDestinationType = ExportDestinationType.DiskFile
                                        .ExportFormatType = ExportFormatType.PortableDocFormat
                                        .DestinationOptions = CrDiskFileDestinationOptions
                                        .FormatOptions = CrFormatTypeOptions
                                    End With
                                    NotaC.Export()
                                    NotaC.Close()
                                    NotaC.Dispose()
                                Catch ex As Exception
                                    MsgBox(ex.ToString)
                                End Try
                                va2 = 0
                                '''''nuevoooooooooooooooooooooooooooooooooooooooooo
                            Else
                                MsgBox("Archivo XML no es una factura version 4.0")
                            End If
                        Else
                            Console.WriteLine(EasyOne_LeeResultado(pN, 8))
                            Console.WriteLine(EasyOne_LeeResultado(pN, 11))

                            Console.WriteLine("*******************************")
                            Console.WriteLine("*********** ERROR *************")
                            Console.WriteLine("*******************************")
                            Dim resultadoXML3 As String = [String].Empty
                            resultadoXML3 = EasyOne_LeeResultado(pN, 11)
                            Console.ReadLine()
                            MsgBox("ERROR AL GENERAR LA NOTA DE CREDITO 4.0" & resultadoXML3)
                        End If
                    Else
                        Dim resultadoXML2 As String = [String].Empty
                        resultadoXML2 = EasyOne_LeeResultado(pN, 11)
                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación Nota de Credito.. ")
                    End If
                Else
                    MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va2, MsgBoxStyle.Information).ToString()
                End If
            Else
                MsgBox("Intenta tmbrar nuevamente la Nota de crédito con el mismo FOLIO Y SERIE !!! Serie: " & serieNC & "   Folio: " & folioNC, MsgBoxStyle.Information).ToString()
            End If
        Else
            MsgBox("Archivo ya Timbrado !!!" & " " & " No. de Veces de Timbrado " & va2, MsgBoxStyle.Information).ToString()
        End If
        Return ""
    End Function

    Private Sub DevVenta_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DevVenta.KeyPress
        If e.KeyChar = Chr(13) Then
            DevIeps.Focus()
        End If
    End Sub

    Private Sub DevIeps_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DevIeps.KeyPress
        If e.KeyChar = Chr(13) Then
            DevVenta0.Focus()
        End If
    End Sub

    Private Sub DevVenta_ValueChanged(sender As Object, e As EventArgs) Handles DevVenta.ValueChanged
        DevIva.Value = (DevVenta.Value * (16 / 100))
    End Sub

    Private Sub DevIeps_ValueChanged(sender As Object, e As EventArgs) Handles DevIeps.ValueChanged
        DevIps.Value = (DevIeps.Value * (8 / 100))
    End Sub
End Class