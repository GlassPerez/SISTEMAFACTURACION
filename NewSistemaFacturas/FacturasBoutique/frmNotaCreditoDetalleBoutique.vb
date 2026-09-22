Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports N400.Framework
Imports N400.QR
Imports System.IO
Imports System.Xml

Public Class frmNotaCreditoDetalleBoutique

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "VariablesGlobales"
    Dim TIP As String
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket, DatGrid As Data.DataTable
    Dim Terminal As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim FolioFinal
    Dim Cadena As String
    Dim ExisteClave As String
    Dim PorIva As Double
#End Region

#Region "PDF"
    Dim rutaArchivoPDF As String
    Dim rutaArchivoXML As String
#End Region

#Region "Sat"
    Dim TpRegimen As String
    Dim e_uuid As String
    Dim Uso_CFDI As String
#End Region

#Region "DatosEmisor"
    Dim TRazonSocial As String
    Dim TRfc As String
    Dim TRFiscal As Integer
    Dim TCp As String
    Dim DesRegimen As String
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

#Region "Forma Pago"
    Dim FormPago As String
    Dim MetPago As String
    Dim fp As String
    Dim NoCuenta As String
    Dim metodoPago As String
#End Region

    Private Sub frmNotaCreditoDetalleBoutique_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnFacturar.Visible = False
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
        verNumCta(False)
        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        Me.txtFechaVenta.Value = dia + "/" + mes + "/" + amo

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where Tid = " & Tienda & "", 0, 1, Motor)
        Application.DoEvents()
        Me.grbTienda.Value = Tienda
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Application.DoEvents()
        Me.grbSucursal.Value = Sucursal
        objpro.CargaCombos(grbformaPago, "Select idpag as Id, pagdes as Forma_Pago from satfpag ", 0, 1, Motor)
        Application.DoEvents()
        objpro.CargaCombos(grbPagos, "Select idmetpag, metdes as Metodo_Pago from satmpag ", 0, 1, Motor)
        Application.DoEvents()
        Clientes()
        InicializaGrid()
    End Sub

    Private Sub frmNotaCreditoDetalleBoutique_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub verNumCta(opcion As Boolean)
        lblNumCuenta.Visible = opcion
        txtNumCuenta.Visible = opcion
    End Sub

    Private Function Clientes() As Boolean
        objpro.CargaCombos(grbClientes, "Select distinct crfc,CRSOCIAL from CATCLIE", 0, 1, Motor)
        Return False
    End Function

    Private Sub InicializaGrid()
        UgrTicket.DataSource = Nothing
        objpro.LLenaGrid(UgrTicket, "select '' as codsat, '' as clavesat, vdClaPro as ClaveInterno, vdDescri as Descripcion, " & _
                                  " vdCant as Cantidad, '' as Unidad, vdPreUni as PrecioUnitario, vdDesc1 as Descu, " & _
                                   " vdIva as Iva, vdPreTot as Importe from ventadet " & _
                                   " where vdfolio = " & grbTienda.Value & " and sid = " & grbSucursal.Value & " and " & _
                                   " vdterm = 0 ", "ventadet", Motor)
        UgrTicket.DataBind()
    End Sub

    Private Sub UgrTicket_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UgrTicket.InitializeLayout
        With Me.UgrTicket
            .DisplayLayout.Bands(0).Columns(0).Width = 100
            .DisplayLayout.Bands(0).Columns(1).Width = 80
            .DisplayLayout.Bands(0).Columns(2).Width = 100
            .DisplayLayout.Bands(0).Columns(3).Width = 300
            .DisplayLayout.Bands(0).Columns(4).Width = 70
            .DisplayLayout.Bands(0).Columns(5).Width = 60
            .DisplayLayout.Bands(0).Columns(6).Width = 90
            .DisplayLayout.Bands(0).Columns(7).Width = 70
            .DisplayLayout.Bands(0).Columns(8).Width = 60
            .DisplayLayout.Bands(0).Columns(9).Width = 90
            REM .DisplayLayout.Bands(0).Columns(10).Width = 80
            REM .DisplayLayout.Bands(0).Columns(11).Hidden = True
        End With
    End Sub

    Private Sub grbClientes_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbClientes.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = UCase(e.KeyChar)
        Else
            Me.txtRfc.Focus()
        End If
    End Sub

    Private Sub grbClientes_RowSelected(sender As Object, e As Infragistics.Win.UltraWinGrid.RowSelectedEventArgs) Handles grbClientes.RowSelected
        Try
            Dim tablaclientes As Data.DataTable            
            tablaclientes = objpro.ObtenDatos("SELECT crfc, crsocial, cemail, cusocfdi FROM CATCLIE WHERE cRFC = '" & Me.grbClientes.Value & "'", Motor).Tables(0)
            txtRfc.Text = tablaclientes.Rows(0).Item("crfc").ToString
            txtRazonSocial.Text = tablaclientes.Rows(0).Item("crSocial").ToString
            txtEmail.Text = tablaclientes.Rows(0).Item("cemail").ToString
            cfdiuso = tablaclientes.Rows(0).Item("cUsoCFDI").ToString()
            If cfdiuso <> "" Then
                txtclvUsoCfdi.Text = cfdiuso
                txtUsoCfdi.Text = objpro.Dato("cfdides", "satuscfdi", "cfdiuso = '" & cfdiuso & "'", Motor)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnInicializa_Click(sender As Object, e As EventArgs) Handles btnInicializa.Click
        DatClientes()
        txtRfc.Focus()
    End Sub

    Private Sub DatClientes()
        grbClientes.Text = ""
        txtRfc.Text = ""
        txtRazonSocial.Text = ""
        txtEmail.Text = ""
        txtclvUsoCfdi.Text = ""
        txtUsoCfdi.Text = ""
    End Sub

    Private Sub btnUsoCfdi_Click(sender As Object, e As EventArgs) Handles btnUsoCfdi.Click
        Formulario(satUsoCFDI)
        txtclvUsoCfdi.Text = cfdiuso
        txtUsoCfdi.Text = cfdides
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub btnBuscarNota_Click(sender As Object, e As EventArgs) Handles btnBuscarNota.Click
        If Me.txtRfc.Text <> "" Then
            If txtRazonSocial.Text <> "" Then
                If txtEmail.Text <> "" Then
                    If txtclvUsoCfdi.Text <> "" Then

                        Try
                            TIP = "NC"
                            Dim EstadoVenta As String
                            Terminal = Mid(Me.txtTransaccion.Text, 1, 3)
                            Dim trn = Len(Me.txtTransaccion.Text)
                            Transaccion = Mid(Me.txtTransaccion.Text, 5, trn)

                            EstadoVenta = objpro.Dato("veEstatu", "VentaEnc", "veFolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and VeIdFolv = " & Transaccion & " and veEstado = 1 and VeTerm = " & Terminal & " and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique)

                            If EstadoVenta = 0 Then
                                Application.DoEvents()
                                Tabla = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.Tid = Sucursal.Tid Where Tienda.tid = " & Me.grbTienda.Value & " AND Sucursal.sid = " & Me.grbSucursal.Value & " ", Motor).Tables(0)
                                Application.DoEvents()
                                TSucursal = objpro.ObtenDatos("SELECT * FROM Serie Where tid = " & Me.grbTienda.Value & " AND sid = " & Me.grbSucursal.Value & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor).Tables(0)
                                Me.txtFactura.Text = Format(CDbl(objpro.DatoMaximo("seCant", "Serie", "sId = " & TSucursal.Rows(0).Item("sid").ToString & " and Tid = " & TSucursal.Rows(0).Item("Tid").ToString & " and seTipo = '" & TIP & "' and seEstat = 1 ", Motor)) + 1, "00000")
                                Me.txtSerie.Text = TSucursal.Rows(0).Item("Seserie").ToString

                                CargaGridInicio()

                                'Importe.Value = 0
                                'Descuento.Value = 0
                                'Subtotal.Value = 0
                                'Iva.Value = 0
                                'Total.Value = 0

                                'Dim h As Integer
                                'Dim Import = 0, Iv = 0, Tot = 0, Desc1 = 0, Desc2 = 0, Desc = 0
                                'For h = 0 To Me.UgrTicket.Rows.Count - 1
                                'Import += (Me.UgrTicket.Rows(h).Cells("PrecioUnitario").Value() * Me.UgrTicket.Rows(h).Cells("Cantidad").Value())
                                'Desc1 += Me.UgrTicket.Rows(h).Cells("Descu").Value()
                                'Iv += Me.UgrTicket.Rows(h).Cells("Iva").Value()
                                'Next
                                'Tot = Import + Iv
                                'Desc = Desc1 + Desc2
                                'Me.Importe.Value = Import
                                'Me.Descuento.Value = Desc
                                'Me.Iva.Value = Iv
                                'Me.Subtotal.Value = (Import - Desc)
                                'Me.Total.Value = (Subtotal.Value + Iva.Value)
                            Else
                                MsgBox("La nota ya fue facturada ..", MsgBoxStyle.Information, "Facturación .. ")
                            End If
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try



                    Else
                        MsgBox("Uso del CFDI", MsgBoxStyle.Information, "Facturación")
                    End If
                Else
                    MsgBox("Falta Email", MsgBoxStyle.Information, "Facturación")
                End If
            Else
                MsgBox("Razón Social, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
            End If
        Else
            MsgBox("RFC, no puede estar en blanco", MsgBoxStyle.Information, "Facturación")
        End If



    End Sub


    Private Sub CargaGridInicio()        
        Application.DoEvents()
        DatGrid = objpro.ObtenDatos("SELECT VdCodSat as CodSat, vdClvUni as ClaveSat, VdClaPro as ClaveInterno, VdDescri as Descripcion, " & _
                                    " VdCant as Cantidad, vdUnidad as Unidad, vdPorIva as PorIva, VdIva as Iva, " & _
                                    " VdPreUni as PrecioUnitario, vdDesc1 as Descu, VdPreTot as Importe, vdFEcha FROM VentaDet Where vdfolio = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and  vdTerm = " & Terminal & " and VdIdFolv = " & Transaccion & " and day(vdFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and month(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(vdFecha)= " & txtFechaVenta.Value.ToString.Substring(6, 4) & " ", MotorBoutique).Tables(0)

        If DatGrid.Rows.Count <> 0 Then

            upgb1.Value = 0
            upgb1.Minimum = 0
            upgb1.Maximum = DatGrid.Rows.Count

            Importe.Value = 0
            Descuento.Value = 0
            Subtotal.Value = 0
            Iva.Value = 0
            Total.Value = 0

            For Each r In DatGrid.Rows
                Application.DoEvents()

                Dim codsat As String
                If r("codsat").ToString = "" Then
                    codsat = "84111506"
                Else
                    codsat = "84111506"
                End If
                Dim Borrar As String = Trim(r("Descripcion").ToString)
                While Borrar.Contains("  ")
                    Borrar = Borrar.Replace("  ", " ")
                End While

                Dim Cantidad As Integer = (r("Cantidad").ToString * -1)
                Dim des As Double = (CDbl(r("Descu").ToString) * -1)
                Dim descu As Double = Format(des, "###,###.00")
                Dim pu1 As Double = (CDbl(r("PrecioUnitario").ToString))
                Dim PU As Double = Format(pu1, "###,###.00")
                Dim iva2 As Double = (((PU * Cantidad) - descu) * 0.16)
                Dim iva1 As Double = Format(iva2, "###,###.00")
                Dim impt As Double = ((PU * Cantidad) - descu)


                FormatoGrid(codsat, r("ClaveSat").ToString, r("ClaveInterno").ToString, Borrar, _
                            Cantidad, "PZA", PU, iva1, _
                            descu, impt)
                upgb1.Value += 1
                Application.DoEvents()

                Importe.Value += Format((r("PrecioUnitario").ToString * Cantidad), "###,###.00")
                Descuento.Value += descu
                Iva.Value += iva1
                Application.DoEvents()
            Next

            Subtotal.Value += (Importe.Value - Descuento.Value)
            Total.Value = (Subtotal.Value + Iva.Value)
        Else
            MsgBox("No se encontró información.. ", MsgBoxStyle.Information, "Facturación.. ")
        End If
    End Sub

    Private Sub FormatoGrid(codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String, _
                           Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double, _
                           Descu As Double, Importe As Double)
        UgrTicket.Rows.Band.AddNew()
        ColocarArticulo(UgrTicket.Rows.Count - 1, codSat, cvlUni, ClaveInterno, Descripcion, Cantidad, Unidad, PrecioUnitario, Iva, Descu, Importe)
    End Sub

    Private Sub ColocarArticulo(ByVal Renglon As Integer, codSat As String, cvlUni As String, ClaveInterno As String, Descripcion As String, _
                          Cantidad As Integer, Unidad As String, PrecioUnitario As Double, Iva As Double, _
                          Descu As Double, Importe As Double)
        With UgrTicket
            .Rows(Renglon).Cells("CodSat").Value = codSat
            .Rows(Renglon).Cells("ClaveSat").Value = cvlUni
            .Rows(Renglon).Cells("ClaveInterno").Value = ClaveInterno
            .Rows(Renglon).Cells("Descripcion").Value = Descripcion
            .Rows(Renglon).Cells("Cantidad").Value = Cantidad
            .Rows(Renglon).Cells("Unidad").Value = Unidad
            .Rows(Renglon).Cells("PrecioUnitario").Value = PrecioUnitario
            .Rows(Renglon).Cells("Descu").Value = Descu
            .Rows(Renglon).Cells("Iva").Value = Iva
            .Rows(Renglon).Cells("Importe").Value = Importe
        End With
    End Sub


    Private Sub btnBuscaUUID_Click(sender As Object, e As EventArgs) Handles btnBuscaUUID.Click
        If grbSucursal.Text <> "" Then
            DevUUID = ""
            DevTienda = grbTienda.Value
            DevSucursal = grbSucursal.Value  REM grbSucursal.Value

            Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
            Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
            Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
            Dim FecVenta As String = a & m & d
            Devfeid = Val(FecVenta)
            Formulario(frmUUIDBoutiqueDetalle)
            txtUUID.Text = DevUUID
        Else
            MsgBox("Sucursal a buscar el UUID Relacionado ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub grbformaPago_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbformaPago.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbformaPago
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbformaPago_ValueChanged(sender As Object, e As EventArgs) Handles grbformaPago.ValueChanged
        If objpro.DatoExiste("pagcom", "satfpag", "idpag = '" & grbformaPago.Value & "' and pagcom = 'D'", Motor) = True Then
            verNumCta(True)
        Else
            verNumCta(False)
        End If
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        TIP = "NC"
        If grbPagos.Text <> "" Then
            If txtUUID.Text <> "" Then
                If txtUUID.Text <> txtUUID1.Text Then
                    If Me.txtEmail.Text <> "" And Me.txtRazonSocial.Text <> "" And Me.txtRfc.Text <> "" And txtclvUsoCfdi.Text <> "" Then
                        FolioFinal = objpro.Dato("seffin", "serie", "tid = " & Me.grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and seSerie = '" & txtSerie.Text & "' and seTipo = '" & TIP & "' and seEstat = 1  ", Motor)
                        upb.Value = 0
                        upb.Maximum = 100

                        If grbformaPago.Text <> "" Then
                            Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
                            Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
                            Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
                            Dim FecBuscFPago As String = a & m & d

                            Try
                                Application.DoEvents()
                                TRazonSocial = Tabla.Rows(0).Item("trSocial").ToString
                                TRfc = Tabla.Rows(0).Item("trfc").ToString
                                TRFiscal = Tabla.Rows(0).Item("satRegimen").ToString
                                TCp = Tabla.Rows(0).Item("sCp").ToString

                                Dim Directorios As String = Tabla.Rows(0).Item("sUbic").ToString
                                Dim CerFactura As String = Tabla.Rows(0).Item("sRCer1").ToString
                                Dim Key As String = Tabla.Rows(0).Item("sRKey").ToString
                                Dim Pwd As String = Tabla.Rows(0).Item("sPwd").ToString
                                Dim RutFacClientes = Tabla.Rows(0).Item("sForm3").ToString
                                Dim CarpTienda = Tabla.Rows(0).Item("sCtien").ToString
                                Dim CarpSucursal = Tabla.Rows(0).Item("sCsuc").ToString
                                Dim UserPAC As String = Trim(Tabla.Rows(0).Item("tUserPAC").ToString)
                                Dim PwdPAC As String = Trim(Tabla.Rows(0).Item("tPwdPAC").ToString)

                                Dim FechaFactura As DateTime = Now
                                Dim FechaHora As String = FechaFactura.ToString.Substring(10, 10)

                                DirEmail = Tabla.Rows(0).Item("EMAILDIR").ToString
                                PwdEmail = Tabla.Rows(0).Item("EMAILPWD").ToString
                                Pop = Tabla.Rows(0).Item("EMAILPOP").ToString
                                PuertoPop = Tabla.Rows(0).Item("EMAILPPOP").ToString
                                SmtpHost = Tabla.Rows(0).Item("EMAILSMTP").ToString
                                PuertoSmtp = Tabla.Rows(0).Item("EMAILPSMTP").ToString
                                DescEmail = Tabla.Rows(0).Item("EMAILDESC").ToString
                                AsuntoEmail = Tabla.Rows(0).Item("EMAILASU").ToString
                                MsjEmail = Tabla.Rows(0).Item("EMAILMSG").ToString

                                Application.DoEvents()
                                Dim RFacImpresas As String
                                RFacImpresas = Directorios & "\" & FechaFactura.ToString.Substring(6, 4) & "\" & FechaFactura.ToString.Substring(3, 2)

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

                                Dim c As New CFDI.CFDI33.Comprobante(Today, txtSerie.Text, txtFactura.Text, c_TipoDeComprobante.Egreso, FormPago, metodoPago)
                                c.Moneda = CFDI.CFDI33.c_Moneda.MXN
                                upb.Value += 10

                                Select Case TRFiscal
                                    Case 601
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.GeneralDeLeyPersonasMorales
                                        DesRegimen = "General De Ley Personas Morales"
                                    Case 603
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.PersonasMoralesConFinesNoLucrativos
                                        DesRegimen = "Personas Morales Con Fines No Lucrativos"
                                    Case 605
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.SueldosYSalariosEIngresosAsimiladosASalarios
                                        DesRegimen = "Sueldos Y Salarios E Ingresos Asimilados A Salarios"
                                    Case 606
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.Arrendamiento
                                        DesRegimen = "Arrendamiento"
                                    Case 608
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.DemasIngresos
                                        DesRegimen = "Demas Ingresos"
                                    Case 609
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.Consolidacion
                                        DesRegimen = "Consolidacion"
                                    Case 610
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.ResidentesEnElExtranjeroSinEstablecimientoPermanenteEnMexico
                                        DesRegimen = "Residentes En El Extranjero Sin Establecimiento Permanente En Mexico"
                                    Case 611
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IngresosPorDividendos
                                        DesRegimen = "Ingresos Por Dividendos"
                                    Case 612
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.PersonasFisicasConActividadesEmpresarialesYProfesionales
                                        DesRegimen = "Personas Fisicas Con Actividades Empresariales Y Profesionales"
                                    Case 614
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IngresosPorIntereses
                                        DesRegimen = "Ingresos Por Intereses"
                                    Case 616
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.SinObligacionesFiscales
                                        DesRegimen = "Sin Obligaciones Fiscales"
                                    Case 607
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.RegimenDeEnajenacionOAdquisicionDeBienes
                                        DesRegimen = "Regimen De Enajenacion O Adquisicion De Bienes"
                                    Case 621
                                        TpRegimen = CFDI.CFDI33.c_RegimenFiscal.IncorporacionFiscal
                                        DesRegimen = "Incorporacion Fiscal"
                                End Select

                                Select Case txtclvUsoCfdi.Text
                                    Case "D01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.HonorariosMedicosDentalesYGastosHospitalarios
                                    Case "D02"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosMedicosPorIncapacidadODiscapacidad
                                    Case "D03"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosFunerales
                                    Case "D04"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.Donativos
                                    Case "D05"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.InteresesRealesEfectivamentePagadosCreditoHipotecario
                                    Case "D06"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.AportacionesVoluntariasAlSAR
                                    Case "D07"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.PrimasPorSegurosGastosMedicos
                                    Case "D08"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosDeTransportacionEscolarObligatoria
                                    Case "D09"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.DepositosEnCuentaParaAhorroPrimaPlanesDePensiones
                                    Case "D10"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.PagosPorServiciosEducativos
                                    Case "G01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.AdquisicionMercancias
                                    Case "G02"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.DevolucionesDescuentosOBonificaciones
                                    Case "G03"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.GastosEnGeneral
                                    Case "I01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.Construcciones
                                    Case "I02"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.MobiliarioYEquipoOficinaInversiones
                                    Case "I03"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.EquipoDeTransporte
                                    Case "I04"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.EquipoComputoYAccesorios
                                    Case "I05"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.DadosTroqueleesMoldesMatricesYHerramental
                                    Case "I06"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.ComunicacionesTelefonicas
                                    Case "I07"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.ComunicacionesSatelitales
                                    Case "I08"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.OtraMaquinariaYEquipo
                                    Case "P01"
                                        Uso_CFDI = CFDI.CFDI33.c_UsoCFDI.Otros
                                End Select

                                c.AgregaDatosEmisor(TRfc, TRazonSocial, TpRegimen)
                                c.AgregaDatosReceptor(txtRfc.Text, txtRazonSocial.Text, Uso_CFDI, CFDI.CFDI33.c_Pais.Mexico)
                                c.LugarExpedicion = TCp

                                Dim NumPiezas As Integer
                                Cadena = txtFechaVenta.Value.ToString.Substring(0, 10) & "- " & Trim(Me.txtTransaccion.Text) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))

                                Dim i As Integer
                                Dim sb As Double = 0
                                Dim des As Double = 0
                                Dim iv As Double = 0
                                Dim iiv As Double = 0
                                Dim PuCa As Double = 0

                                For i = 0 To Me.UgrTicket.Rows.Count - 1
                                    Dim pun As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
                                    c.AgregaConcepto(Me.UgrTicket.Rows(i).Cells("Cantidad").Value, "PZA", Trim(Me.UgrTicket.Rows(i).Cells("ClaveSat").Value), Trim(Me.UgrTicket.Rows(i).Cells("CodSat").Value), Trim(Me.UgrTicket.Rows(i).Cells("ClaveInterno").Value), Trim(Me.UgrTicket.Rows(i).Cells("Descripcion").Value), pun)
                                    NumPiezas += Me.UgrTicket.Rows(i).Cells("Cantidad").Value
                                    c.Conceptos(i).Impuestos.Traslados.Add(New CFDI.CFDI33.ComprobanteConceptoImpuestosTraslado)
                                    If UgrTicket.Rows(i).Cells("Descu").Value <> 0 Then
                                        If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") <> 0 Then
                                            c.Conceptos(i).Descuento = Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                            REM c.Conceptos(i).Descuento = (Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                            c.Conceptos(i).DescuentoSpecified = True
                                            des += Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00")
                                            REM des += (Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                        Else
                                            c.Conceptos(i).Descuento = (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.01)
                                            REM c.Conceptos(i).Descuento = ((Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") - 0.01) * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                            c.Conceptos(i).DescuentoSpecified = True
                                            des += (Format(Me.UgrTicket.Rows(i).Cells("Descu").Value, "###,###.00") - 0.01)
                                            REM des += ((Format(Me.UgrTicket.Rows(i).Cells("Desc1").Value, "###,###.00") - 0.01) * Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
                                        End If
                                    End If
                                    With c.Conceptos(i).Impuestos.Traslados(0)
                                        If Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00") <> 0 Then
                                            .Importe = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value) - (Me.UgrTicket.Rows(i).Cells("Descu").Value)) * 0.16), "###,###.00")
                                            .ImporteSpecified = True
                                            iv += .Importe
                                            .Base = Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00")
                                            .Impuesto = CFDI.CFDI33.c_Impuesto.IVA
                                            .TasaOCuota = 0.16
                                            .TasaOCuotaSpecified = True
                                            .TipoFactor = CFDI.CFDI33.c_TipoFactor.Tasa
                                        Else
                                            .Importe = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value) - ((Me.UgrTicket.Rows(i).Cells("Descu").Value - 0.01))) * 0.16), "###,###.00")
                                            .ImporteSpecified = True
                                            iv += .Importe
                                            .Base = Format((((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value * Me.UgrTicket.Rows(i).Cells("Cantidad").Value) - ((Me.UgrTicket.Rows(i).Cells("Descu").Value - 0.01)))), "###,###.00")
                                            .Impuesto = CFDI.CFDI33.c_Impuesto.IVA
                                            .TasaOCuota = 0.16
                                            .TasaOCuotaSpecified = True
                                            .TipoFactor = CFDI.CFDI33.c_TipoFactor.Tasa
                                        End If
                                    End With
                                Next

                                Dim impuestoIVA As New ComprobanteImpuestosTraslado
                                With impuestoIVA
                                    .Importe = Format(iv, "###,###.00")
                                    .Impuesto = c_Impuesto.IVA
                                    .TasaOCuota = 0.16
                                    .TipoFactor = c_TipoFactor.Tasa
                                End With
                                c.Impuestos.Traslados.Add(impuestoIVA)

                                c.Impuestos.TotalImpuestosTrasladados = Format(iv, "###,###.00")
                                c.Impuestos.TotalImpuestosTrasladadosSpecified = True

                                If Descuento.Value <> 0 Then
                                    c.Descuento = des
                                    c.DescuentoSpecified = True
                                Else
                                    c.DescuentoSpecified = False
                                End If
                                c.Total = Format(((c.SubTotal - des) + iv), "###,###.00")

                                c.TruncarDecimales = True
                                c.TruncarDecimalesLongitud = 2

                                Dim ValorTicket As String = "Devoluciones, Venta del día " & Cadena
                                NoCuenta = txtNumCuenta.Text
                                Dim DatosImpresionExtra As New Hashtable
                                DatosImpresionExtra.Add("NumTicket", ValorTicket)
                                DatosImpresionExtra.Add("TotPiezas", NumPiezas)
                                DatosImpresionExtra.Add("FP", grbformaPago.Text)
                                DatosImpresionExtra.Add("NoCuenta", NoCuenta)
                                DatosImpresionExtra.Add("DesRegimen", DesRegimen)


                                'cfdi relacionados
                                c.CfdiRelacionados = New ComprobanteCfdiRelacionados
                                c.CfdiRelacionados.CfdiRelacionado = New List(Of ComprobanteCfdiRelacionadosCfdiRelacionado)
                                c.CfdiRelacionados.TipoRelacion = c_TipoRelacion.NotaDeCreditoDocumentosRelacionados_01

                                Dim cr As New N400.Componentes.CFDI.CFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado
                                cr.UUID = txtUUID.Text    'aqui va el UUID del documento a relacionar                                                        
                                c.CfdiRelacionados.CfdiRelacionado.Add(cr)
                                Application.DoEvents()

                                If txtUUID1.Text <> "" Then
                                    Dim cr1 As New N400.Componentes.CFDI.CFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado
                                    cr1.UUID = txtUUID1.Text    'aqui va el UUID del documento a relacionar
                                    c.CfdiRelacionados.CfdiRelacionado.Add(cr1)
                                End If                                
                                Application.DoEvents()

                                If txtUUID2.Text <> "" Then
                                    Dim cr2 As New N400.Componentes.CFDI.CFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado
                                    cr2.UUID = txtUUID2.Text    'aqui va el UUID del documento a relacionar
                                    c.CfdiRelacionados.CfdiRelacionado.Add(cr2)
                                End If
                                Application.DoEvents()

                                Dim motorcfdi As New CFDI.CFDI33.MotorCFDI(CerFactura, Key, Pwd, RFacImpresas)
                                CFDI.Licenciamiento.AgregaLicencia("NxfUWVt27sSyPaPK/VSpaM1KxObdAq1pQgX7SmJWTEWljBDrg2XqOSmxSth7pZ6FRsViFC+MzuubR0AfCl1wgYiKnOWpT6XWJMXAy58SbcE=")
                                motorcfdi.CapitalizacionCantidadConLetra = CFDI.Globales.Capitalizacion.PrimeraLetraFrase
                                Application.DoEvents()
                                motorcfdi.Comprobantes.Add(c)
                                Application.DoEvents()
                                upb.Value += 20  'Suma 30
                                Dim resultado As CFDI.CFDI33.ResultadoProceso = motorcfdi.GenerarArchivosPAC(True, False)
                                Application.DoEvents()

                                If Not resultado.Errores Then
                                    Application.DoEvents()
                                    Dim svcNew As New fel.WSCFDI33Client
                                    Dim Respuesta As New fel.RespuestaTFD33

                                    svcNew.Open()
                                    Dim ReferenciaPac As String = Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text
                                    Respuesta = svcNew.TimbrarCFDI(Trim(UserPAC), Trim(PwdPAC), IO.File.ReadAllText(c.ArchivoXMLSellado), Trim(ReferenciaPac))
                                    upb.Value += 20 'Suma 50
                                    svcNew.Close()

                                    Dim archivoREPX As String
                                    Dim archivoPDF As String
                                    Dim archivoXMLPDF As String
                                    Dim tran As Integer
                                    Dim CantidadLetra As String = ""
                                    Dim Estatus As String = 1

                                    If Respuesta.OperacionExitosa Then
                                        If IO.File.Exists(c.ArchivoXMLSellado) Then
                                            Application.DoEvents()
                                            IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml"), Respuesta.XMLResultado)
                                            CFDI.Generar.ArchivoCadenaTFD(RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml")
                                            CFDI.Generar.ArchivoCBB(RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml")
                                            CFDI.Generar.ArchivoDatosPDF(RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml", CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
                                            Dim NombrePDF As String = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml"
                                            archivoXMLPDF = CFDI.Generar.ArchivoDatosPDF(Trim(NombrePDF), CFDI.Globales.Capitalizacion.PrimeraLetraFrase, "", CFDI.Globales.Lenguaje.Espanol, DatosImpresionExtra)
                                            Application.DoEvents()
                                            upb.Value += 10 'Suma 60

                                            Try
                                                Dim RutReporte As String
                                                RutReporte = RutFacClientes & "\RecursosCFD33"
                                                archivoREPX = CFDI.PDF.Generar.ArchivoReporteBase(RutReporte, False)
                                                archivoPDF = CFDI.PDF.Generar.ArchivoPDF2(archivoREPX, archivoXMLPDF)
                                                upb.Value += 10 'Suma 70
                                                Application.DoEvents()
                                                rutaArchivoPDF = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".pdf"
                                                rutaArchivoXML = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".xml"
                                            Catch ex As Exception
                                            End Try

                                            Dim FeRegistro, Fecha, fechareal, Fecha11, Fecha22, Fecha33, FePnotasF As String
                                            Fecha11 = txtFechaVenta.Text.ToString.Substring(0, 2)
                                            Fecha22 = txtFechaVenta.Text.ToString.Substring(3, 2)
                                            Fecha33 = txtFechaVenta.Text.ToString.Substring(6, 4)
                                            FePnotasF = Fecha33 & Fecha22 & Fecha11
                                            fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
                                            FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha22 & Fecha11
                                            Fecha = fechareal

                                            Dim Tterm = CInt(Mid(Me.txtTransaccion.Text, 1, 3))
                                            Dim Nnota = CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text)))
                                            Dim NotaVta As String = Tterm & Format(CDbl(Nnota), "0000")
                                            Dim TieSuc As String = Me.grbTienda.Value & Me.grbSucursal.Value
                                            Dim PorIva As Double = 16.0


                                            objpro.Indicacion("Update VentaEnc set veEstatu = 1, veFac = '" & CInt(Me.txtFactura.Text) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text))) & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & CInt(Mid(Me.txtTransaccion.Text, 1, 3)) & " and  veEstado = 1 ", Motor)
                                            objpro.Indicacion("Update Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)


                                            ExisteClave = CStr(objpro.DatoString("cid", "catclie", "crfc = '" & Trim(Me.txtRfc.Text) & "' ", Motor))
                                            Dim Tipo As String = "F"
                                            Dim cantidadconletra As String = ""
                                            Dim FeCan = 0
                                            Dim FeMotCan As String = ""
                                            Dim ferisr = 0
                                            Dim feriva = 0

                                            Dim Subtotal = Me.Subtotal.Value
                                            Dim Iva = Me.Iva.Value
                                            Dim Total = Me.Total.Value
                                            Dim Descuento = Me.Descuento.Value

                                            Dim CadenaUUID As String = IO.File.ReadAllText(c.ArchivoCBB)
                                            Dim VectorUUID() As String = Split(CadenaUUID, "=")
                                            Dim simleUuid() As String
                                            simleUuid = Split(VectorUUID(1), "&")
                                            e_uuid = simleUuid(0)

                                            objpro.Indicacion("Insert into FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid,feFecha, " & _
                                                      "feHora,Cid,FeSubt,fePorIva,FeIva, " & _
                                                      "FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " & _
                                                      "FeCanlet,FeAbonos,FeEstatu,FeReg,feTip,feuuid) " & _
                                                      "values (" & CInt(tran) & ", " & grbTienda.Value & ", " & CInt(Mid(Trim(Me.txtTransaccion.Text), 1, 3)) & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "', " & _
                                                      " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & ", '" & Subtotal & "', '" & CDbl(PorIva) & "','" & CDbl(Me.Iva.Value) & "', " & _
                                                      " '" & Descuento & "','" & Descuento & "', '" & Descuento & "', '" & Descuento & "', '" & Me.Total.Value & "', " & _
                                                      " '" & CantidadLetra & "', '" & Total & "'," & Estatus & "," & FeRegistro & ",'" & Tipo & "', '" & e_uuid & "' )", Motor)

                                            upb.Value += 10 'Suma 80

                                            Dim correo As New System.Net.Mail.MailMessage()
                                            correo.From = New System.Net.Mail.MailAddress(DirEmail)
                                            correo.To.Add(Trim(Me.txtEmail.Text))

                                            correo.Subject = Trim(AsuntoEmail)
                                            correo.Body = Trim(MsjEmail)
                                            correo.Body += Environment.NewLine & Environment.NewLine
                                            correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
                                            correo.Body += Environment.NewLine & "BOSCO. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                                            correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                                            correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                                            correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                                            correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                                            correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                                            correo.Body += " ó a través de una solicitud vía correo electrónico a hfortiz@grupovigil.com.mx"

                                            correo.IsBodyHtml = False
                                            correo.Priority = System.Net.Mail.MailPriority.Normal
                                            Dim Archivo As New System.Net.Mail.Attachment(rutaArchivoPDF)
                                            For i = 0 To 1
                                                If i = 0 Then
                                                    Archivo = New System.Net.Mail.Attachment(rutaArchivoPDF)
                                                    upb.Value += 5 'Suma 85
                                                Else
                                                    Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                                                    upb.Value += 5 'Suma 90
                                                End If
                                                correo.Attachments.Add(Archivo)
                                            Next
                                            Dim smtp As New System.Net.Mail.SmtpClient
                                            smtp.Host = SmtpHost
                                            smtp.Port = Int(PuertoSmtp)
                                            smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                                            smtp.EnableSsl = False
                                            Application.DoEvents()
                                            smtp.Send(correo)
                                            upb.Value += 10 'Suma 100
                                            MsgBox("La factura se envío correctamente", MsgBoxStyle.Information, "Facturación .. ")
                                            BorrarRegistro()
                                            DatClientes()
                                            PDF(archivoPDF)
                                        End If
                                    Else
                                        Dim sll = c.ArchivoSello
                                        Dim txt = sll.ToString.Replace(".sll", ".txt")
                                        Dim xml = sll.ToString.Replace(".sll", ".xml")
                                        IO.File.Delete(sll) : IO.File.Delete(txt) : IO.File.Delete(xml)
                                        MsgBox(Respuesta.CodigoRespuesta.ToString & " " & Respuesta.MensajeError.ToString, MsgBoxStyle.Information, "Facturación")
                                    End If 'Cierra validación de operación exitosa


                                Else
                                    For Each cNG As CFDI.CFDI33.Comprobante In resultado.ComprobantesNoGenerados
                                        MessageBox.Show(cNG.ErrorGeneracion.Message)
                                    Next
                                End If 'Valida cierre de resultado si genera Error


                            Catch ex As Exception
                                MessageBox.Show(ex.Message)
                            End Try 'Cierra validación del try


                        Else
                            MsgBox("Falta seleccionar forma de pago .. ", MsgBoxStyle.Information, "Facturación .. ")
                        End If 'Valida Seleccion de Forma de Pago
                    End If 'Valida Campos con Valos de Cliente para generar Documento
                Else
                    MsgBox("UUID Relacionados no pueden ser iguales ..  ", MsgBoxStyle.Information, "Facturación .. ")
                End If
            Else
                MsgBox("UUID Relacionado no puede estar en blanco ..  ", MsgBoxStyle.Information, "Facturación .. ")
            End If 'Valida exista CFDI para relación de Nota de Credito
        Else
            MsgBox("Falta seleccionar Metodo de Pago .. ", MsgBoxStyle.Information, "Facturación .. ")
        End If 'Valida cierre de Metodo de Pago
    End Sub

    Private Sub grbPagos_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles grbPagos.InitializeLayout
        e.Layout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        With grbPagos
            .DisplayLayout.Bands(0).Columns(0).Hidden = True
        End With
    End Sub

    Private Sub grbPagos_ValueChanged(sender As Object, e As EventArgs) Handles grbPagos.ValueChanged
        Select Case grbPagos.Value
            Case "PUE"
                metodoPago = c_MetodoPago.PagoEnUnaSolaExhibicion_PUE
            Case "PPD"
                metodoPago = c_MetodoPago.PagoEnParcialidadesODiferido_PPD
        End Select
    End Sub

    Private Sub BorrarRegistro()
        UgrTicket.DataSource = Nothing
        limpiar()
        limpiar_FormaPago()
        limpi_UUID()
        Importe.Value = "0.0"
        Subtotal.Value = "0.0"
        Descuento.Value = "0.0"
        Iva.Value = "0.0"
        Total.Value = "0.0"

        Dim Fecha As String = Now
        Dim dia As String = Now.Day
        Dim mes As String = Month(Now)
        Dim amo As String = Year(Now)
        Me.txtFechaVenta.Value = dia + "/" + mes + "/" + amo
        Me.txtTransaccion.Text = ""
    End Sub

    Private Sub limpiar()
        Me.txtSerie.Text = ""
        Me.txtFactura.Text = ""
    End Sub

    Private Sub limpiar_FormaPago()
        grbformaPago.Text = ""
        txtNumCuenta.Text = ""
        grbPagos.Text = ""
    End Sub


    Private Sub limpi_UUID()
        txtUUID.Text = ""
        txtUUID1.Text = ""
        txtUUID2.Text = ""
    End Sub

    Public Function PDF(ByVal Ruta As String) As String
        System.Diagnostics.Process.Start(Ruta)
        Return False
    End Function

    Private Sub btnBuscaUUID2_Click(sender As Object, e As EventArgs) Handles btnBuscaUUID2.Click
        If grbSucursal.Text <> "" Then
            DevUUID = ""
            DevTienda = grbTienda.Value
            DevSucursal = grbSucursal.Value  REM grbSucursal.Value

            Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
            Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
            Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
            Dim FecVenta As String = a & m & d
            Devfeid = Val(FecVenta)
            Formulario(frmUUIDBoutiqueDetalle)
            txtUUID1.Text = DevUUID
        Else
            MsgBox("Sucursal a buscar el UUID Relacionado ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub

    Private Sub btnBuscaUUID3_Click(sender As Object, e As EventArgs) Handles btnBuscaUUID3.Click
        If grbSucursal.Text <> "" Then
            DevUUID = ""
            DevTienda = grbTienda.Value
            DevSucursal = grbSucursal.Value  REM grbSucursal.Value

            Dim d As String = txtFechaVenta.Value.ToString.Substring(0, 2)
            Dim m As String = txtFechaVenta.Value.ToString.Substring(3, 2)
            Dim a As String = txtFechaVenta.Value.ToString.Substring(6, 4)
            Dim FecVenta As String = a & m & d
            Devfeid = Val(FecVenta)
            Formulario(frmUUIDBoutiqueDetalle)
            txtUUID2.Text = DevUUID
        Else
            MsgBox("Sucursal a buscar el UUID Relacionado ", MsgBoxStyle.Information, "Facturación")
        End If
    End Sub
End Class