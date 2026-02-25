Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Threading

Public Class frmHonorarios

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket, tablaclientes As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim FolioFinal
    Dim Cadena As String
    Dim TpRegimen As String
    Dim impRetenido As Double = 0
    Dim fp As String
    Dim DesRegimen As String
    Dim DesUsoCFDI As String
    Dim fechventa As String
    Dim ExisteClave As String
    Dim Subt As Double
    Dim va As Integer
    Dim hra As String
    Dim serie, folio As String
#End Region

#Region "Forma Pago"
    Dim FormPago As String
    Dim metodoPago As String
    Dim MetPago As String
    Dim NoCuenta As String
#End Region

#Region "Datos de Factura"
    Dim txtLugarExpedicion As String
#End Region

#Region "Sat"
    Dim e_uuid As String
    Dim Uso_CFDI As String
    Dim RF As String
    Dim RFD As String
    Dim cCP As String
    Dim cDireccion As String
    Dim cComDir As String
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
#End Region

    Private Sub frmHonorarios_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub frmHonorarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        objpro.CargaCombos(grbTienda, "Select tId, trSocial from bgxfacelec.CatTien where ttipo1 = 'F'", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbConceptos, "Select ccoid as Id, ccodesc as Concepto from bgxfacelec.catconc order by ccodesc", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from bgxfacelec.satmpag ", 0, 1, As400)
        Application.DoEvents()
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from bgxfacelec.satfpag ", 0, 1, As400)
        REM Clientes()        
        txtRfc.Focus()
        va = 0
    End Sub

    'Private Function Clientes() As Boolean
    'objpro.CargaCombos(grbClientes, "Select crfc,crsocial from BGXFacElec.catclie order by crsocial asc ", 0, 1, As400)
    'Return False
    'End Function

    Private Sub grbClientes_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs) Handles grbClientes.RowSelected
        Try
            Dim tablaclientes As Data.DataTable
            tablaclientes = objpro.ObtenDatos("SELECT crfc, crsocial, cemail, cusoCFDI as cusocfdi FROM bgxfacelec.catclie WHERE cRFC = '" & Me.grbClientes.Value & "'", As400).Tables(0)
            txtRfc.Text = tablaclientes.Rows(0).Item("crfc").ToString
            txtRazonSocial.Text = tablaclientes.Rows(0).Item("crSocial").ToString
            txtEmail.Text = tablaclientes.Rows(0).Item("cemail").ToString
            cfdiuso = tablaclientes.Rows(0).Item("cUsoCFDI").ToString()
            If cfdiuso <> "" Then
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDIBosco)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub grbTienda_TextChanged(sender As Object, e As EventArgs) Handles grbTienda.TextChanged
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM bgxfacelec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
    End Sub

    Private Sub btnDatosFactura_Click(sender As Object, e As EventArgs) Handles btnDatosFactura.Click
        Try
            TIP = "FC"
            Tabla = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CatTien as Tienda left join bgxfacelec.Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", As400).Tables(0)
            TSucursal = objpro.ObtenDatos("select * from bgxfacelec.Serie where tid = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400).Tables(0)
            txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400)) + 1, "00000")
            txtSerie.Text = TSucursal.Rows(0).Item("Seserie").ToString
            txtLugarExpedicion = TSucursal.Rows(0).Item("seLugarExp").ToString
            InicializaGrid()

            txtIv.Text = 0
            txtIvISR.Text = 0
        Catch ex As Exception
            MsgBox("Busqueda: " & ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub btnLimpiarRegistros_Click(sender As Object, e As EventArgs) Handles btnLimpiarRegistros.Click
        Me.txtRfc.Text = ""
        Me.txtRazonSocial.Text = ""
        Me.txtEmail.Text = ""
        Me.txtclvUsoCfdi.Text = ""
        Me.txtUsoCfdi.Text = ""
        Me.grbPagos.Text = ""
    End Sub

    Private Sub btnAgregaConceptos_Click(sender As Object, e As EventArgs) Handles btnAgregaConceptos.Click
        If grbTienda.Text <> "" And grbSucursal.Text <> "" Then
            If Me.grbConceptos.Text <> "" And Importe.Value <> 0 Then
                Dim Cantidad As Integer = 1
                Dim Clave As String = "84111500"
                Dim Unidad As String = "E48"
                Dim iva As Double = (Importe.Value * 0.16)
                Dim Desc1, Desc2 As Double
                Dim total As Double = (Importe.Value + iva)
                FormatoGrid(Cantidad, Clave, Unidad, grbConceptos.Text, Importe.Value, iva, Desc1, Desc2, total)
                totales()
                Me.grbConceptos.Text = ""
                Importe.Value = 0.0
            Else
                MsgBox("Falta descripción de producto y/o Importe.. ", MsgBoxStyle.Information, "Facturación.. ")
            End If
        Else
            MsgBox("Falta Empresa y/o Sucursal.. ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub totales()
        Subtotal.Value = 0.0
        Iva.Value = 0.0
        Descuento.Value = 0.0
        Total.Value = 0.0
        Retencion.Value = 0.0
        Subt = 0

        For Each r In ugr.Rows
            Subtotal.Value += Format(r.Cells("Importe").Value, "###,###.00")
        Next
        Iva.Value = (Subtotal.Value * 0.16)
        Retencion.Value = (Subtotal.Value * (txtIv.Text / 100))
        Total.Value = (Subtotal.Value + Iva.Value - Retencion.Value)
    End Sub

    Private Sub FormatoGrid(Cantidad As Integer, Clave As String, Unidad As String, descripcion As String, Importe As Double, Iva As Double,
                            Desc1 As Double, Desc2 As Double, Total As Double)
        ugr.Rows.Band.AddNew()
        ColocarArticulo(ugr.Rows.Count - 1, Cantidad, Clave, Unidad, descripcion, Importe, Iva, Desc1, Desc2, Total)
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, Cantidad As Integer, Clave As String, Unidad As String, descripcion As String, Importe As Double, Iva As Double,
                                Desc1 As Double, Desc2 As Double, Total As Double)
        With ugr
            .Rows(Renglon).Cells("CodSat").Value = Clave
            .Rows(Renglon).Cells("vdclvUni").Value = Unidad
            .Rows(Renglon).Cells("Descripcion").Value = descripcion
            .Rows(Renglon).Cells("Cantidad").Value = Cantidad
            .Rows(Renglon).Cells("Importe").Value = Format(Importe, "###,###.00")
            .Rows(Renglon).Cells("Iva").Value = Format(Iva, "###,###.00")
            .Rows(Renglon).Cells("Desc1").Value = Desc1
            .Rows(Renglon).Cells("Desc2").Value = Desc2
            .Rows(Renglon).Cells("Total").Value = Format(Total, "###,###.00")
        End With
    End Sub

    Private Sub InicializaGrid()
        ugr.DataSource = Nothing
        objpro.LLenaGrid(ugr, "select '' as codsat, '' as vdclvUni, vdDescri as Descripcion, " &
                                  " vdCant as Cantidad, vdPreUni as Importe, vdIva as Iva, " &
                                   " vdDesc1 as Desc1, vdDesc2 as Desc2, vdPreTot as Total from bgxfacelec.ventadet " &
                                   " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and vdtm = 'G' and " &
                                   " vdterm = 0 and vdfecha = '0' ", "ventadet", As400)
        ugr.DataBind()
    End Sub

    Private Sub ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles ugr.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 70
            .DisplayLayout.Bands(0).Columns(1).Width = 70
            .DisplayLayout.Bands(0).Columns(2).Width = 250
            .DisplayLayout.Bands(0).Columns(3).Width = 70
            .DisplayLayout.Bands(0).Columns(4).Width = 90
            .DisplayLayout.Bands(0).Columns(5).Width = 70
            .DisplayLayout.Bands(0).Columns(6).Width = 60
            .DisplayLayout.Bands(0).Columns(7).Width = 60
            .DisplayLayout.Bands(0).Columns(8).Width = 90
        End With
    End Sub

    Private Sub ugr_AfterRowsDeleted(sender As Object, e As EventArgs) Handles ugr.AfterRowsDeleted
        totales()
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "bgxfacelec.satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", As400) = True Then
            verNumCta(True)
            fp = grbformaPago.Text
        Else
            verNumCta(False)
            fp = grbformaPago.Text
        End If
        FormPago = grbformaPago.Value
    End Sub
    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Sub grbClientes_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbClientes.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbClientes
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbTienda_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbTienda.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbTienda
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbSucursal_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbSucursal.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbSucursal
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbPagos_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbPagos.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbPagos
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbConceptos_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbConceptos.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbConceptos
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        btnFacturar.Visible = False
        Timbra()
    End Sub

    Private Sub Timbra()
        Dim Directorios, RutFacClientes, RutFacGlobal, CarpTienda, CarpSucursal, RutNotaCredito As String
        Dim TRazonSocial, TRfc, trFiscal, TCp, TRef As String
        If Me.ugr.Rows.Count <> 0 Then
            If grbPagos.Text <> "" Then
                If grbformaPago.Text <> "" Then
                    If MessageBox.Show("Esta seguro de Generar Recibo ", "Honorarios", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
                        upb.Value = 0
                        upb.Maximum = 100

                        tablaclientes = objpro.ObtenDatos("SELECT * FROM bgxfacelec.CATCLIE WHERE cRFC = '" & Me.txtRfc.Text & "'", As400).Tables(0)
                        Dim ne As String = tablaclientes.Rows(0).Item("cNumExt").ToString.Trim
                        If ne.ToString = "0" Then
                            ne = " "
                        End If
                        cDireccion = tablaclientes.Rows(0).Item("cDir").ToString.Trim & " " & ne & " " & tablaclientes.Rows(0).Item("cNumInt").ToString.Trim & " " & tablaclientes.Rows(0).Item("cColonia").ToString.Trim
                        cComDir = tablaclientes.Rows(0).Item("cMun").ToString.Trim & " " & tablaclientes.Rows(0).Item("cEstado").ToString.Trim & " " & tablaclientes.Rows(0).Item("cLocal").ToString.Trim & " " & tablaclientes.Rows(0).Item("cPais").ToString.Trim

                        TRazonSocial = objpro.Dato("trsocial", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)
                        TRfc = objpro.Dato("trfc", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)
                        TCp = objpro.Dato("tcp", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)
                        TRef = objpro.Dato("tref", "bgxfacelec.cattien", "tid = " & grbTienda.Value & "", As400)
                        Dim UserPAC As String = objpro.Dato("tUserPAC", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)
                        Dim PwdPAC As String = objpro.Dato("tPwdPAC", "bgxfacelec.cattien", " tid = " & grbTienda.Value & " ", As400)

                        trFiscal = objpro.Dato("satRegimen", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        Directorios = objpro.Dato("sUbic", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        Dim CerFactura As String = objpro.Dato("sRCer1", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        Dim Key As String = objpro.Dato("sRKey", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        Dim Pwd As String = objpro.Dato("sPwd", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        RutFacClientes = objpro.Dato("sForm1", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        CarpTienda = objpro.Dato("sCtien", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        CarpSucursal = objpro.Dato("sCsuc", "bgxfacelec.catsucu", " tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " ", As400)
                        Dim rutlogo As String = "\\172.16.150.24\c\inetpub\wwwroot\sistemafac\Tiendas\Povisa\Formatos\FactCliente\RecursosCFD33\LOGO.png"
                        Dim FechaFactura As DateTime = Now
                        Dim FechaHora As String = FechaFactura.ToString.Substring(10, 10)
                        Dim descreg As String = Trim(Tabla.Rows(0).Item("sRFiscal").ToString)

                        Application.DoEvents()
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
                        Tabla1.Columns.Add("rcomdir")
                        Tabla1.Columns.Add("rusocfdi")
                        Tabla1.Columns.Add("nocuenta")
                        Tabla1.Columns.Add("retIVA")
                        Tabla1.Columns.Add("retISR")

                        Dim Fila As DataRow

                        Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
                        Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
                        Fila("Erazonsoc") = TRazonSocial
                        Fila("Eregfis") = trFiscal.ToString + " - " + descreg.Trim
                        Fila("Edireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
                        Fila("Edircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString
                        Dim RFacImpresas As String
                        RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

                        Dim p As Integer = EasyOne_cfdi33_Create(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml") ''(RFacImpresas & "\" & "EJEMPLO5.xml")

                        upb.Value += 10 'inicia en 10

                        EasyOne_cfdi33_SetCfdi40(p, "01", "")

                        Fila("exporta") = "01-No Aplica"

                        Select Case grbformaPago.Value
                            Case "01"
                                FormPago = CFDI.CFDI33.c_FormaPago.Efectivo_01
                            Case "02"
                                FormPago = CFDI.CFDI33.c_FormaPago.Cheque_02
                            Case "03"
                                FormPago = CFDI.CFDI33.c_FormaPago.Transferencia_03
                            Case "04"
                                FormPago = CFDI.CFDI33.c_FormaPago.TarjetaCredito_04
                            Case "05"
                                FormPago = CFDI.CFDI33.c_FormaPago.MonederoElectronico_05
                            Case "06"
                                FormPago = CFDI.CFDI33.c_FormaPago.DineroElectronico_06
                            Case "08"
                                FormPago = CFDI.CFDI33.c_FormaPago.ValesDespensa_08
                            Case "12"
                                FormPago = CFDI.CFDI33.c_FormaPago.DacionEnPago_12
                            Case "13"
                                FormPago = CFDI.CFDI33.c_FormaPago.PagoPorSubrogacion_13
                            Case "14"
                                FormPago = CFDI.CFDI33.c_FormaPago.PagoPorConsignacion_14
                            Case "15"
                                FormPago = CFDI.CFDI33.c_FormaPago.Condonacion_15
                            Case "17"
                                FormPago = CFDI.CFDI33.c_FormaPago.Compensacion_17
                            Case "23"
                                FormPago = CFDI.CFDI33.c_FormaPago.Novacion_23
                            Case "24"
                                FormPago = CFDI.CFDI33.c_FormaPago.Confusion_24
                            Case "25"
                                FormPago = CFDI.CFDI33.c_FormaPago.RemisionDeuda_25
                            Case "26"
                                FormPago = CFDI.CFDI33.c_FormaPago.PrescripcionCaducidad_26
                            Case "27"
                                FormPago = CFDI.CFDI33.c_FormaPago.SatisfaccionAcreedor_27
                            Case "28"
                                FormPago = CFDI.CFDI33.c_FormaPago.TarjetaDebito_28
                            Case "29"
                                FormPago = CFDI.CFDI33.c_FormaPago.TarjetaServicio_29
                            Case "30"
                                FormPago = CFDI.CFDI33.c_FormaPago.NA_98
                            Case "99"
                                FormPago = CFDI.CFDI33.c_FormaPago.Otros_99
                        End Select

                        Select Case trFiscal
                            Case 601
                                TpRegimen = c_RegimenFiscal.GeneralDeLeyPersonasMorales
                                DesRegimen = "General De Ley PersonasMorales"
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
                        End Select

                        Select Case txtclvUsoCfdi.Text
                            Case "G01"
                                DesUsoCFDI = c_UsoCFDI.AdquisicionMercancias
                            Case "G02"
                                DesUsoCFDI = c_UsoCFDI.DevolucionesDescuentosOBonificaciones
                            Case "G03"
                                DesUsoCFDI = c_UsoCFDI.GastosEnGeneral
                            Case "I01"
                                DesUsoCFDI = c_UsoCFDI.Construcciones
                            Case "I02"
                                DesUsoCFDI = c_UsoCFDI.MobiliarioYEquipoOficinaInversiones
                            Case "I03"
                                DesUsoCFDI = c_UsoCFDI.EquipoDeTransporte
                            Case "I04"
                                DesUsoCFDI = c_UsoCFDI.EquipoComputoYAccesorios
                            Case "I05"
                                DesUsoCFDI = c_UsoCFDI.DadosTroqueleesMoldesMatricesYHerramental
                            Case "I06"
                                DesUsoCFDI = c_UsoCFDI.ComunicacionesTelefonicas
                            Case "I07"
                                DesUsoCFDI = c_UsoCFDI.ComunicacionesSatelitales
                            Case "I08"
                                DesUsoCFDI = c_UsoCFDI.OtraMaquinariaYEquipo
                            Case "D01"
                                DesUsoCFDI = c_UsoCFDI.HonorariosMedicosDentalesYGastosHospitalarios
                            Case "D02"
                                DesUsoCFDI = c_UsoCFDI.GastosMedicosPorIncapacidadODiscapacidad
                            Case "D03"
                                DesUsoCFDI = c_UsoCFDI.GastosFunerales
                            Case "D04"
                                DesUsoCFDI = c_UsoCFDI.Donativos
                            Case "D05"
                                DesUsoCFDI = c_UsoCFDI.InteresesRealesEfectivamentePagadosCreditoHipotecario
                            Case "D06"
                                DesUsoCFDI = c_UsoCFDI.AportacionesVoluntariasAlSAR
                            Case "D07"
                                DesUsoCFDI = c_UsoCFDI.PrimasPorSegurosGastosMedicos
                            Case "D08"
                                DesUsoCFDI = c_UsoCFDI.GastosDeTransportacionEscolarObligatoria
                            Case "D09"
                                DesUsoCFDI = c_UsoCFDI.DepositosEnCuentaParaAhorroPrimaPlanesDePensiones
                            Case "D10"
                                DesUsoCFDI = c_UsoCFDI.PagosPorServiciosEducativos
                            Case "P01"
                                DesUsoCFDI = c_UsoCFDI.Otros
                        End Select

                        'Llenamos la sección del Emisor
                        EasyOne_cfdi33_SetEmisor(p, TRfc, TRazonSocial, trFiscal) 'TRFiscal

                        'Llenamos la sección del Receptor
                        'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
                        'EasyOne_cfdi33_SetReceptor(p, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01")
                        EasyOne_cfdi33_SetReceptor(p, txtRfc.Text, txtRazonSocial.Text, "", "", txtclvUsoCfdi.Text) 'estos paramentros son si o si para factura global
                        EasyOne_cfdi33_SetReceptor40(p, cCP, RF)

                        Fila("sucursal") = Cadena
                        Fila("Rcp") = cCP 'TCp
                        Fila("Rrfc") = txtRfc.Text '"XAXX010101000"
                        Fila("Rrazonsoc") = txtRazonSocial.Text '"PUBLICO EN GENERAL"
                        Fila("Rdireccion") = cDireccion '""
                        Fila("Rregimen") = RF & " - " & RFD '
                        Fila("rusocfdi") = "Uso CFDI: " & txtclvUsoCfdi.Text ' & " - " & txtUsoCfdi.Text '"S01"
                        Fila("moneda") = "MXN"
                        Fila("rcomdir") = cComDir
                        Fila("nocuenta") = txtNumCuenta.Text

                        Dim i As Integer
                        Dim sb As Double = 0
                        Dim des As Double = 0
                        Dim iv As Double = 0
                        Dim iiv As Double = 0
                        Dim PuCa As Double = 0
                        Dim Imp As Double = 0
                        Dim bas As Double = 0
                        Dim des1 As Double = 0
                        Dim Descu As Double = 0
                        Dim Import As Double = 0
                        Dim Import2 As Double = 0

                        Dim Importe As Double = 0
                        Dim Impuesto As String = ""
                        Dim TasaOCuota As String = ""
                        Dim TipoFactor As String = ""
                        Dim Impu As String = ""
                        Dim ToC As String = ""
                        Dim TFac As String = ""
                        Dim dcto As String = ""
                        Dim ipt As String = ""
                        Dim Impuest As String = ""
                        Dim Impuest2 As String = ""

                        Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
                        Tabla2.Columns.Add("Cantidad")
                        Tabla2.Columns.Add("Unidad")
                        Tabla2.Columns.Add("UniSAT")
                        Tabla2.Columns.Add("Descrip")
                        Tabla2.Columns.Add("Punitario")
                        Tabla2.Columns.Add("Importe")

                        Dim Fila2 As DataRow

                        Dim NumPiezas As Integer
                        Cadena = ""

                        For i = 0 To Me.ugr.Rows.Count - 1
                            Dim pun As Double = Format(Me.ugr.Rows(i).Cells("Importe").Value, "###,###.00")
                            NumPiezas += Me.ugr.Rows(i).Cells("Cantidad").Value
                            sb = Trim(Me.ugr.Rows(i).Cells("Importe").Value)
                            Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
                            Fila2("Cantidad") = Trim(Me.ugr.Rows(i).Cells("Cantidad").Value)
                            Fila2("Unidad") = Trim(Me.ugr.Rows(i).Cells("CodSat").Value)
                            Fila2("UniSAT") = Trim(Me.ugr.Rows(i).Cells("VdclvUni").Value)
                            Fila2("Descrip") = Trim(Me.ugr.Rows(i).Cells("Descripcion").Value)
                            Fila2("Punitario") = Format(pun, "###,###.00")
                            Fila2("Importe") = Format(pun, "###,###.00")
                            DS.Tables(1).Rows.Add(Fila2)
                            EasyOne_cfdi33_AddConcepto(p, Trim(Me.ugr.Rows(i).Cells("CodSat").Value), "E48", Me.ugr.Rows(i).Cells("Cantidad").Value, Trim(Me.ugr.Rows(i).Cells("VdclvUni").Value), Trim(Me.ugr.Rows(i).Cells("VdclvUni").Value), Trim(Me.ugr.Rows(i).Cells("Descripcion").Value), Format(pun, "######.00"), Format(pun, "######.00"), "")

                            EasyOne_cfdi33_SetConcepto40(p, "02")
                            Imp = Format((Trim(Me.ugr.Rows(i).Cells("Importe").Value) * 0.16), "######.00")
                            iv += Imp
                            bas = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value), "######.00")
                            Impu = "002"
                            ToC = "0.160000"
                            TFac = "Tasa"
                            EasyOne_cfdi33_AddTraslado(p, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)

                            If txtIv.Text <> 0 Then
                                Import = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value * (txtIv.Text / 100)), "######.00")
                                impRetenido += Import
                                Dim TasaOCuot As String = (txtIv.Text / 100)
                                Dim TipoFacto As String = "Tasa" 'c_TipoFactor.Tasa
                                Impuest = "002" 'c_Impuesto.IVA
                                Dim Base As Double = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value), "######.00")
                                EasyOne_cfdi33_AddRetencion(p, Format(Base, "######.00"), Impuest, TipoFacto, TasaOCuot, Format(Import, "######.00"))

                                Import2 = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value * (txtIvISR.Text / 100)), "######.00")
                                impRetenido += Import2
                                Dim TasaOCuot2 As String = (txtIvISR.Text / 100)
                                Dim TipoFacto2 As String = "Tasa" 'c_TipoFactor.Tasa
                                Impuest2 = "001" 'c_Impuesto.IVA
                                Dim Base2 As Double = Format(CDbl(Me.ugr.Rows(i).Cells("Importe").Value), "######.00")
                                EasyOne_cfdi33_AddRetencion(p, Format(Base2, "######.00"), Impuest2, TipoFacto2, TasaOCuot2, Format(Import2, "######.00"))
                            End If
                        Next
                        upb.Value += 20 'Suma 30
                        Impuesto = "002"
                        TasaOCuota = "0.160000"
                        TipoFactor = "Tasa"
                        Importe = Format(iv, "######.00")

                        Dim TotalImpuestosTrasladados As Double = 0
                        TotalImpuestosTrasladados = Format(iv, "######.00")

                        'Agregamos los Impuestos
                        Dim TotalImpuestosRetenidos As Double = Import + Import2
                        If TotalImpuestosRetenidos = 0 Then
                            EasyOne_cfdi33_SetComprobanteImpuestos(p, "0.00", Format(TotalImpuestosTrasladados, "######.00"))
                        Else
                            EasyOne_cfdi33_SetComprobanteImpuestos(p, Format(TotalImpuestosRetenidos, "######.00"), Format(TotalImpuestosTrasladados, "######.00"))
                        End If

                        If txtIv.Text <> 0 Then
                            EasyOne_cfdi33_AddRetencionesRetencion(p, Impuest, Import)
                            EasyOne_cfdi33_AddRetencionesRetencion(p, Impuest2, Import2)
                        End If

                        EasyOne_cfdi33_AddTrasladosTraslado(p, Impuesto, TipoFactor, TasaOCuota, Importe)

                        If Import = 0 Then
                            Fila("retIVA") = "0.00"
                        Else
                            Fila("retIVA") = Format(Import, "###,###.00")
                        End If
                        If Import2 = 0 Then
                            Fila("retISR") = "0.00"
                        Else
                            Fila("retISR") = Format(Import2, "###,###.00")
                        End If

                        Dim Total As Double = 0
                        Dim Subt As Double = 0
                        Subt = Format(sb + iv, "######.00")
                        Total = Format((Subt - impRetenido), "###,###.00")
                        Fila("Fsubtotal") = Format(Subt, "###,###.00")
                        Fila("Fdescuento") = Format(des, "###,###.00")
                        Fila("Fiva") = Format(iv, "###,###.00")
                        Fila("Ftotal") = Format(Total, "###,###.00")


                        Dim ValorTicket As String = Cadena
                        If txtNumCuenta.Text = "" Then
                            NoCuenta = "-"
                        Else
                            NoCuenta = txtNumCuenta.Text
                        End If

                        Dim cantletra As String
                        cantletra = Letras(Total)
                        Fila("cantidadl") = cantletra

                        Dim f1, f2, f3, freal, FeReg, hr As String
                        f1 = Today.ToString.Substring(0, 2)
                        f2 = Today.ToString.Substring(3, 2)
                        f3 = Today.ToString.Substring(6, 4)
                        FeReg = Today.ToString.Substring(6, 4) & f2 & f1

                        Fila("metpago") = grbPagos.Value
                        Fila("tipocomp") = "I - Ingreso"
                        Fila("formpago") = fp
                        upb.Value += 10 'Suma 40

                        'Timbra XML, que se encuentra en variable P
                        Dim x As Integer = 99
                        If va = 0 Then
                            hr = Now.ToLocalTime.ToString.Substring(11, 8)

                            If hra <> "" Then
                                'MsgBox("Intenta tmbrar nuevamente pero con otra hora !!!" & " " & " 1er hora " & hra & "   2da hora " & hr, MsgBoxStyle.Information).ToString()
                                hr = hra
                            End If

                            freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

                            If (serie = "" And folio = "") Or (folio <> txtFactura.Text) Then

                                EasyOne_cfdi33_SetComprobante(p, txtSerie.Text, txtFactura.Text, freal, grbformaPago.Value, "", Format((sb), "######.00"), "", "MXN", "1", Format((Total), "######.00"), "I", grbPagos.Value, TCp, "")

                                Dim folf As Integer = 0 'consulto el ultimo folio de serie
                                folf = CDbl(objpro.DatoMaximo("seCant", "bgxfacelec.Serie", "sId = " & grbSucursal.Value & " and Tid = " & grbTienda.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", As400))
                                If CInt(Me.txtFactura.Text) > folf Then
                                    'Enviamos el CFDI al metodo de PRUEBAS
                                    'En la siguiente línea es necesario incluir la Clave de Integración que se proporciona en la Guía de Implementación (Menú "Ayuda" de su Oficina Virtual).
                                    'EasyOne_SetInvoiceOneFachadaInfoPrueba(p, "RHUUMRGL", "RHUUMRGL") 'PARA PRUEBAS
                                    'EasyOne_SetInvoiceOneFachadaInfo(p, UserPAC, UserPAC)
                                    EasyOne_SetInvoiceOneFachadaInfo(p, "RHUUMRGL", "RHUUMRGL")

                                    Dim b As Integer
                                    b = p
                                    x = EasyOne_FirmaComprobante(b, CerFactura, Key, Pwd)
                                    p = b
                                    b = 0
                                    Thread.Sleep(10000) 'espera por 10 segundos
                                    If x = 0 Then
                                        objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", As400)

                                        Application.DoEvents()
                                        upb.Value += 10 'Suma 50
                                        Dim resultadoXML As String = [String].Empty
                                        resultadoXML = EasyOne_LeeResultado(p, 8)
                                        'Obtenemos el XML completo y timbrado
                                        Console.WriteLine(resultadoXML)

                                        IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"), resultadoXML)
                                        upb.Value += 10 'Suma 60
                                        va += 1
                                        hra = hr
                                        serie = txtSerie.Text
                                        folio = txtFactura.Text

                                        'Si el resultado es 0 = El CFDI se procesó correctamente
                                        If x = 0 And va = 1 Then
                                            Dim Version_xml As String
                                            Dim version As String
                                            Dim uuid As String
                                            Dim ftimb As String
                                            Dim rfcprov As String
                                            Dim sellocfd As String
                                            Dim nocert As String

                                            Dim documento As XmlDocument = New XmlDocument()
                                            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
                                            documento.Load(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"))
                                            VarManager.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/4") ''para identificar el prefijo CFDI
                                            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
                                            Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText
                                            If Version_xml = "4.0" Then
                                                Application.DoEvents()
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
                                                RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml.png")

                                                ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

                                                DS.Tables(0).Rows.Add(Fila)
                                                Dim objreport As New GArrenda
                                                objreport.SetParameterValue("Logo", RutaLogo)
                                                objreport.SetParameterValue("QR", RutaQR)
                                                Dim rpFact As New GArrenda
                                                rpFact.SetDataSource(DS)
                                                rpFact.SetParameterValue("QR", RutaQR)
                                                rpFact.SetParameterValue("Logo", RutaLogo)
                                                Arrendamiento.CrystalReportViewer1.ReportSource = objreport
                                                Arrendamiento.CrystalReportViewer1.ReportSource = rpFact
                                                Arrendamiento.WindowState = FormWindowState.Maximized
                                                Arrendamiento.Show()

                                                upb.Value += 10 'Suma 70
                                                rutaArchivoPDF = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".pdf"
                                                rutaArchivoXML = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml"
                                                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

                                                Dim cryRpt As New ReportDocument

                                                Try
                                                    Dim CrExportOptions As ExportOptions
                                                    Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                                                    Dim crf As New PdfRtfWordFormatOptions()
                                                    CrDiskFileDestinationOptions.DiskFileName = rutaArchivoPDF
                                                    CrExportOptions = rpFact.ExportOptions
                                                    With CrExportOptions
                                                        .ExportDestinationType = ExportDestinationType.DiskFile
                                                        .ExportFormatType = ExportFormatType.PortableDocFormat
                                                        .DestinationOptions = CrDiskFileDestinationOptions
                                                        .FormatOptions = CrFormatTypeOptions
                                                    End With
                                                    rpFact.Export()
                                                    upb.Value += 10 'Suma 80  
                                                Catch ex As Exception
                                                    MsgBox(ex.ToString)
                                                End Try

                                                Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF As String
                                                Fecha11 = FechaFactura.ToString.Substring(0, 2)
                                                Fecha22 = FechaFactura.ToString.Substring(3, 2)
                                                Fecha33 = FechaFactura.ToString.Substring(6, 4)
                                                FePnotasF = Fecha33 & Fecha22 & Fecha11
                                                fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                                                FeRegistro = FechaFactura.ToString.Substring(6, 4) & Fecha22 & Fecha11
                                                Fecha = fechareal

                                                Dim TieSuc As String = Me.grbTienda.Value & Me.grbSucursal.Value
                                                Dim PorIva As Double = 16.0


                                                Dim d As String = FechaFactura.ToString.Substring(0, 2)
                                                Dim m As String = FechaFactura.ToString.Substring(3, 2)
                                                Dim a As String = FechaFactura.ToString.Substring(6, 4)
                                                Dim FecVenta As String = a & m & d
                                                fechventa = FecVenta
                                                Transaccion = Val(Trim(FecVenta))

                                                Dim FeCan = 0
                                                Dim FeMotCan As String = ""
                                                Dim ferisr = 0
                                                Dim feriva = 0
                                                Dim Tipo As String = "F"
                                                Dim Estatus = 1
                                                Dim ts As String = Trim(Me.grbTienda.Value) & Trim(Str(Me.grbSucursal.Value))
                                                ts = Trim(ts)

                                                Dim Abono As Double = 0
                                                Dim Subtotal = Me.Subtotal.Value
                                                Dim Iva = Me.Iva.Value
                                                Dim Total2 = Me.Total.Value
                                                Dim Descuento = Me.Descuento.Value
                                                ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = '" & txtRfc.Text & "' ", As400))
                                                upb.Value += 10 'Suma 90
                                                Dim serie As String = txtSerie.Text

                                                Dim numfactura = CInt(txtFactura.Text)
                                                Try
                                                    objpro.Indicacion("Insert into BGXFacElec.fencabez (FeId,tId,FeTerm,FeFact,feSerie,sid, " &
                                                                 " feFecha,feHora,Cid,FeSubt, " &
                                                                 " fePorIva,FeIva, FeDesc1,FePDesc1,FeDesc2, " &
                                                                 " FePDesc2,FeTotal, FeCanlet,FeAbonos,FeEstatu, " &
                                                                 " FeReg,fefeccan, femotcan, feTip, feuuid, " &
                                                                 " ferisr, feriva) " &
                                                                 " values (" & Transaccion & ", " & grbTienda.Value & ", " & ts & "," & numfactura & ", '" & serie & "', " & grbSucursal.Value & ", " &
                                                                 " '" & fechareal & "', '" & Trim(FechaHora) & "', " & ExisteClave & ", " & Subtotal & ", " &
                                                                 " " & PorIva & ", " & Iva & ", " & Descuento & ", " & Descuento & ", " & Descuento & ", " &
                                                                 " " & Descuento & ", " & Total2 & ", '" & cantletra & "', " & Abono & ", " & Estatus & ", " &
                                                                 " " & FeRegistro & ", '" & FeCan & "', '" & FeMotCan & "', '" & Tipo & "', '" & uuid & "', " &
                                                                 " " & ferisr & ", " & feriva & ")", As400)
                                                Catch ex As Exception
                                                    MsgBox(ex.ToString)
                                                End Try
                                                upb.Value += 10 'Suma 100
                                                BorrarRegistro()
                                                DatClientes()
                                                btnFacturar.Visible = True
                                                va = 0
                                            Else
                                                MsgBox("Archivo XML no es una factura version 4.0")
                                            End If 'Cierra validacion de operacion exitosa
                                        Else
                                            'En caso de error mostrar el detalle.
                                            Dim resultadoXML2 As String = [String].Empty
                                            resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                            Console.WriteLine(resultadoXML2)
                                            MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                            btnFacturar.Visible = True
                                        End If 'Cierra validacion generacion del XML

                                    Else
                                        btnFacturar.Visible = True
                                        Dim resultadoXML2 As String = [String].Empty
                                        resultadoXML2 = EasyOne_LeeResultado(p, 11)
                                        MsgBox(resultadoXML2, MsgBoxStyle.Information, "Facturación .. ")
                                        hra = hr
                                    End If
                                End If

                            Else
                                MsgBox("Intenta tmbrar nuevamente con el mismo FILO Y SERIE !!! Serie: " & serie & "   Folio: " & folio, MsgBoxStyle.Information).ToString()
                            End If
                        Else
                            MsgBox("Archivo ya Timbrado !!! No. de Veces de Timbrado " & va, MsgBoxStyle.Information).ToString()
                        End If


                    End If
                Else
                    MsgBox("Seleccionar forma de pago .. ", MsgBoxStyle.Information, "Honorarios ...")
                    btnFacturar.Visible = True
                End If
            Else
                MsgBox("Metodo de pago .. ", MsgBoxStyle.Information, "Honorarios ...")
                btnFacturar.Visible = True
            End If
        Else
            MsgBox("No hay registros para generar ", MsgBoxStyle.Information, "Honorarios ...")
            btnFacturar.Visible = True
        End If
    End Sub


    Private Sub BorrarRegistro()
        ugr.DataSource = Nothing
        limpiar()
        Importe.Value = "0.0"
        Descuento.Value = "0.0"
        Subtotal.Value = "0.0"
        Iva.Value = "0.0"
        Retencion.Value = "0.0"
        Total.Value = "0.0"
        Subt = 0

        txtIvISR.Text = 0
        txtIv.Text = 0
        InicializaGrid()

        grbformaPago.Text = ""
        txtNumCuenta.Text = ""
    End Sub

    Private Sub limpiar()
        txtSerie.Text = ""
        txtFactura.Text = ""
    End Sub

    Private Sub DatClientes()
        grbClientes.Text = ""
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub grbPagos_ValueChanged(sender As Object, e As EventArgs) Handles grbPagos.ValueChanged
        Select Case grbPagos.Value
            Case "PUE"
                metodoPago = c_MetodoPago.PagoEnUnaSolaExhibicion_PUE
            Case "PPD"
                metodoPago = c_MetodoPago.PagoEnParcialidadesODiferido_PPD
        End Select
    End Sub

    Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click
        idCliente = 0
        Formulario(frmClientesBosco)
        If idCliente <> 0 Then
            txtRfc.Text = objpro.Dato("crfc", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtRazonSocial.Text = objpro.Dato("crsocial", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtEmail.Text = objpro.Dato("cemail", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            cfdiuso = objpro.Dato("cusocfdi", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            txtclvUsoCfdi.Text = cfdiuso
            txtUsoCfdi.Text = objpro.Dato("cfdides", "bgxfacelec.satuscfdi", "cfdiuso = '" & cfdiuso & "'", As400)
            RF = regimenFisc
            RFD = regimen
            cCP = objpro.Dato("cCP", "bgxfacelec.catclie", "cid = " & idCliente & "", As400)
            If cCP.Trim.Length = 4 Then
                cCP = "0" & cCP
            End If
        End If
    End Sub
End Class