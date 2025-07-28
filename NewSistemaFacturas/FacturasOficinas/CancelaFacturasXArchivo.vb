Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports ExcelDataReader
Imports N400.Componentes.CFDI.CFDI33
Imports XElement


Public Class CancelaFacturasXArchivo

    Dim objpro As New Procedures
    Dim objfun As New Funciones
    Dim tables As DataTableCollection

#Region "VariablesGlobales"
    Dim fol As String
    Dim ser As String
    Dim uui As String
    Dim fet As String
    Dim fev As String
    Dim fop As String
    Dim subt As String
    Dim iv As String
    Dim des As String
    Dim tot As String
    Dim tic As String
    Dim mep As String
    Dim uur As String
    Dim ver As String
    Dim cuuid As String
    Dim RFiscal As String
    Dim freal, hr As String

    Dim contador As Integer = 0
    Dim TIP As String
    Dim fechventa As String
    Dim FechaFactura As String

    Dim TSucursal, TablaSucursal, DatGrid, TablaTienda, tiendaPrinc, sucursal, vtaenca, vtadetalle As Data.DataTable
    'Dim datosGen As Data.DataSet
    Dim Terminal, TpDev As Integer
    Dim Transaccion As Integer
    Dim Timbrado As Boolean = False
    Dim TpRegimen As String
    Dim fp As String
    Dim NoCuenta As String
    Dim DesRegimen As String
    Dim Cadena As String
    Dim e_uuid As String
    Dim PorIVa As Double
    Dim NumPiezas As Integer
    Dim td As Integer
#End Region


    Private Sub grbTienda_ValueChanged(sender As Object, e As EventArgs) Handles grbTienda.ValueChanged
        Try
            objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
            'Me.ChlisFact.Items.Clear()
            'Me.ChlisNota.Items.Clear()
            Me.grbSucursal.Value = Nothing
            grbSucursal.Focus()
        Catch ex As Exception
            MsgBox("Sucursal: " & ex.Message, MsgBoxStyle.Information, "Facturación.. ")
        End Try
    End Sub

    Private Sub grbTienda_KeyPress(sender As Object, e As KeyPressEventArgs) Handles grbTienda.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.grbSucursal.Focus()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using ofd As OpenFileDialog = New OpenFileDialog() With {.Filter = "Excel 97-2003 workbook|*.xls|Excel workbook|*.xlsx"}
            If ofd.ShowDialog() = DialogResult.OK Then
                TextBox1.Text = ofd.FileName
                Using Stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read)
                    Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(Stream)
                        Dim result As DataSet = reader.AsDataSet(New ExcelDataSetConfiguration() With {.ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {.UseHeaderRow = True}})
                        tables = result.Tables
                        cboSheet.Items.Clear()
                        For Each table As DataTable In tables
                            cboSheet.Items.Add(table.TableName)
                        Next
                    End Using
                End Using
            End If
        End Using
    End Sub

    Private Sub cboSheet_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSheet.SelectedIndexChanged
        Dim dt As DataTable = tables(cboSheet.SelectedItem.ToString())
        DataGridView1.DataSource = dt
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnGenerarFactura.Click
        Try
            For i = 0 To Me.DataGridView1.Rows.Count - 1
                hr = Now.ToLocalTime.ToString.Substring(11, 8)
                freal = Now.Date.ToString.Substring(6, 4) & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr
                tot = Me.DataGridView1.Rows(i).Cells("Total").Value
                uui = Me.DataGridView1.Rows(i).Cells("UUID").Value
                fev = Me.DataGridView1.Rows(i).Cells("Fecha Emision").Value
            Next
            MsgBox("DOCUMENTO RECORRIDO CON ÉXITO, HASTA EL FOLIO: ", MsgBoxStyle.Exclamation, "Informacion")

        Catch ex As Exception
            MsgBox("No se pudo leer el grid :( SE QUEDÓ EN EL FOLIO: ", MsgBoxStyle.Information, "Informacion")
        End Try
    End Sub

    Public Function NotaCredito(ByVal fechaNC As String, ByVal n_TRfc As String, ByVal n_TRFiscal As String, ByVal n_Directorios As String, ByVal cer As String, ByVal key As String,
                                ByVal psw As String, ByVal n_uuid As String, n_LExpedicion As String)
        Dim n_serie As String
        Dim n_notacredito As String
        DesRegimen = ""
        NumPiezas = 0
        td = 2

        upb.Value = 0
        upb.Maximum = 100

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
        Dim pN As Integer = EasyOne_cfdi33_Create(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml")

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
        metpg = "PUE"
        tipcom = "E"
        formpg = "01"

        'Dim sbt As Double = 0
        Dim Pu As Double = Format((tot / 1.16), "###,###.00")
        'sbt = Format(Pu, "###,###.00")

        Fila("Ntipocomp") = "EGRESO"
        Fila("Nmetpag") = metpg
        Fila("Nformpag") = "EFECTIVO"


        'Tipo de relacion
        EasyOne_cfdi33_SetCfdiRelacionados(pN, "01")
        'CFDI Relacionado
        EasyOne_cfdi33_AddCfdiRelacionado(pN, n_uuid)

        'c.Moneda = CFDI.CFDI33.c_Moneda.MXN
        fp = "EFECTIVO"

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
        End Select


        'Llenamos la sección del Emisor
        EasyOne_cfdi33_SetEmisor(pN, n_TRfc, TRazonSocial, TpRegimen)

        'Llenamos la sección del Receptor
        'RFC, NOMBRE, RESIDENCIA FISCAL, NUMREGIDTRIB, USOCFDI
        EasyOne_cfdi33_SetReceptor(pN, "XAXX010101000", "PUBLICO EN GENERAL", "", "", "S01") 'estos paramentros son si o si para factura global
        EasyOne_cfdi33_SetReceptor40(pN, n_LExpedicion, "616") 'regimen fiscal para global 616

        'Dim Pu As Double = Format((Me.Devoluciones.Value / 1.16), "###,###.00")
        Dim iv As Double = 0  REM = Format((Pu * 0.16), "###,###.00")
        Dim descrip As String = ""
        descrip = "Devoluciones del día " & fev.ToString.Substring(0, 10) & " con referencia de Folio Fiscal (UUID) " & n_uuid & " con un importe de $ " & Format(Pu, "###,###.00")

        Fila("Nrcp") = TCp
        Fila("Nrrfc") = "XAXX010101000"
        Fila("Nrrazonsoc") = "PUBLICO EN GENERAL"
        Fila("Nrdireccion") = ""
        Fila("Nrregimen") = "616-Sin Obligaciones Fiscales"
        Fila("Nusocfdi") = "S01"
        Fila("Nmoneda") = "MXN"

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

        EasyOne_cfdi33_AddConcepto(pN, "84111506", "100", "1", "ACT", "SERVICIO", descrip.Trim, Pu, Pu, "")
        EasyOne_cfdi33_SetConcepto40(pN, "02")

        Fila3 = DSNC.Tables(1).NewRow() ' Acá insertas una fila nueva
        Fila3("Ncantidad") = "1"
        Fila3("Nunidad") = "SERVICIO"
        Fila3("NuniSAT") = "ACT"
        Fila3("Ndescip") = descrip.Trim
        Fila3("Npunitario") = Format(Pu, "###,###.00")
        Fila3("Nimporte") = Format(Pu, "###,###.00")
        Fila3("NclaveSAT") = "84111506"
        DSNC.Tables(1).Rows.Add(Fila3)

        'If Iva.Value <> 0 Then
        bas = Format(Pu, "###,###.00")
            Imp = Format((Pu * 0.16), "###,###.00")
            iv += Imp
            Impu = "002"
            ToC = "0.160000"
            TFac = "Tasa"
            EasyOne_cfdi33_AddTraslado(pN, bas.ToString, Impu.ToString, TFac.ToString, ToC.ToString, Imp.ToString)
        'End If

        Dim total As Double = 0
        total = Format((Pu + iv), "###,###.00")

        'Llenamos la sección del comprobante (2)
        EasyOne_cfdi33_SetComprobante(pN, n_serie, n_notacredito, fechaNC, "01", "", Pu.ToString, "", "MXN", TipoCambio, total.ToString, tipcom, "PUE", n_LExpedicion, "")

        Impuesto = "002"
        TasaOCuota = "0.160000"
        TipoFactor = "Tasa"
        Import = Format(iv, "###,###.00")

        Dim TotalImpuestosTrasladados As Double = 0
        TotalImpuestosTrasladados = Format(iv, "###,###.00")

        EasyOne_cfdi33_SetComprobanteImpuestos(pN, "", TotalImpuestosTrasladados)
        EasyOne_cfdi33_AddTrasladosTraslado(pN, Impuesto, TipoFactor, TasaOCuota, Import)

        Fila("Nfsubtotal") = Format(Pu, "###,###.00")
        'Fila("Nfdescuento") = ""
        Fila("Nfiva") = Format(iv, "###,###.00")
        Fila("Nftotal") = Format(total, "###,###.00")

        Dim cantl As String
        cantl = Letras(total)
        Fila("Ncantidadl") = cantl

        'EasyOne_SetInvoiceOneFachadaInfoPrueba(pN, "RHUUMRGL", "RHUUMRGL")
        EasyOne_SetInvoiceOneFachadaInfo(pN, "RHUUMRGL", "RHUUMRGL")

        Dim nc As Integer = EasyOne_FirmaComprobante(pN, cer, key, psw)

        'Obtención del resultado de la solicitud
        If nc = 0 Then
            Dim resultadoXMLNC As String = [String].Empty

            'Obtenemos el XML completo y timbrado
            resultadoXMLNC = EasyOne_LeeResultado(pN, 8)

            Console.WriteLine(resultadoXMLNC)
            Dim archivoXMLPDFNC As String = ""
            Dim archivoREPXNC As String = ""
            Dim archivoPDFNC As String = ""

            IO.File.WriteAllText(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"), resultadoXMLNC)

            '''''''''''''''''''''''''''''' RECORRO XML NOTA CREDITO''''''''''''''''''''''''''''
            Dim Version_xmlNC As String
            'Dim VarConceptos As XmlNodeList
            Dim versionNC As String
            Dim uuidNC As String
            Dim ftimbNC As String
            Dim rfcprovNC As String
            Dim sellocfdNC As String
            Dim nocertNC As String

            Dim nav As XPathNavigator
            Dim documentoNC As XmlDocument = New XmlDocument()
            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documentoNC.NameTable)
            documentoNC.Load(Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml"))
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

                'RutaLogoN = RutFac & "\RecursosCFD33\Penguin.bmp"
                RutaQRN = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".xml.png")

                '''''''''''''''''''''''''''''''''''''''''''''''''''
                'objpro.Indicacion("Update VentaEnc set veEstatu = 1, veFac = '" & CInt(n_notacredito) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & td & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & Terminal & " and  veEstado = 1 and day(veFecha) = " & txtFechaVenta.Value.ToString.Substring(0, 2) & " and Month(veFecha) = " & txtFechaVenta.Value.ToString.Substring(3, 2) & " and year(veFecha) = " & txtFechaVenta.Value.ToString.Substring(6, 4) & "", Motor)
                objpro.Indicacion("Update Serie set seCant = " & CInt(n_notacredito) & " Where seSerie = '" & n_serie & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", Motor)
                '''''nuevoooooooooooooooooooooooooooooooooooooooooo

                DSNC.Tables(0).Rows.Add(Fila)
                Dim objrep As New NotaCredito
                objrep.SetParameterValue("NLOGO", RutaLogo)
                objrep.SetParameterValue("NQR", RutaQRN)
                Dim NotaC As New NotaCredito
                NotaC.SetDataSource(DSNC)
                NotaC.SetParameterValue("NQR", RutaQRN)
                NotaC.SetParameterValue("NLOGO", RutaLogo)
                GNota.CrystalReportViewer1.ReportSource = objrep
                GNota.CrystalReportViewer1.ReportSource = NotaC
                GNota.WindowState = FormWindowState.Maximized
                GNota.Show()

                Dim Ruta As String = Trim(RFacImpresas2 & "\" & Trim(n_TRfc) & "-" & n_serie & "-" & n_notacredito & ".pdf")
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

            Console.ReadLine()
            MsgBox("ERROR AL GENERAR LA NOTA DE CREDITO 4.0")
        End If
        Return ""
    End Function

    Private Sub CancelaFacturasXArchivo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FechaFactura = Date.Now.ToString.Substring(0, 10)

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien ", 0, 1, Motor)
    End Sub
End Class