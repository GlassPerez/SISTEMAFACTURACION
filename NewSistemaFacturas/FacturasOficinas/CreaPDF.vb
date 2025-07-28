Imports System.Xml

Public Class CreaPDF
    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Using ofd As OpenFileDialog = New OpenFileDialog() With {.Filter = "Archivo xml|*.pdf.xml"}
    '        If ofd.ShowDialog() = DialogResult.OK Then

    '            TextBox1.Text = ofd.FileName
    '            Dim Version_xml As String
    '            Dim uuid As String
    '            Dim uuidrefe As String
    '            Dim ftimb As String
    '            Dim formpag As String
    '            Dim fecVta As String
    '            Dim Subto As String
    '            Dim Iva As String
    '            Dim Desc As String
    '            Dim Total As String
    '            Dim Folio As String
    '            Dim Serie As String
    '            Dim Comprobante As String
    '            Dim MetPago As String
    '            Dim NodoDesc As XmlNode
    '            Dim Uudiref As XmlNode

    '            Dim DS As DataSet = New DataSet("DataSetFac")
    '            Dim Tabla1 As DataTable = DS.Tables.Add("DatosGen")

    '            Tabla1.Columns.Add("Erfc")
    '            Tabla1.Columns.Add("Erazonsoc")
    '            Tabla1.Columns.Add("Eregfis")
    '            Tabla1.Columns.Add("Edireccion")
    '            Tabla1.Columns.Add("Edircomp")
    '            Tabla1.Columns.Add("NoDoc")
    '            Tabla1.Columns.Add("foliofisc")
    '            Tabla1.Columns.Add("provcert")
    '            Tabla1.Columns.Add("tipocomp")
    '            Tabla1.Columns.Add("usocfdi")
    '            Tabla1.Columns.Add("metpago")
    '            Tabla1.Columns.Add("formpago")
    '            Tabla1.Columns.Add("exporta")
    '            Tabla1.Columns.Add("fechaemi")
    '            Tabla1.Columns.Add("lugaremi")
    '            Tabla1.Columns.Add("serieceremi")
    '            Tabla1.Columns.Add("Rrfc")
    '            Tabla1.Columns.Add("Rrazonsoc")
    '            Tabla1.Columns.Add("Rdireccion")
    '            Tabla1.Columns.Add("Rcp")
    '            Tabla1.Columns.Add("moneda")
    '            Tabla1.Columns.Add("sellodig")
    '            Tabla1.Columns.Add("sellosat")
    '            Tabla1.Columns.Add("complesat")
    '            Tabla1.Columns.Add("Rregimen")
    '            Tabla1.Columns.Add("Fsubtotal")
    '            Tabla1.Columns.Add("Fdescuento")
    '            Tabla1.Columns.Add("Fiva")
    '            Tabla1.Columns.Add("Ftotal")
    '            Tabla1.Columns.Add("cantidadl")
    '            Tabla1.Columns.Add("certSAT")
    '            Tabla1.Columns.Add("hryfechcer")
    '            Tabla1.Columns.Add("sucursal")
    '            Tabla1.Columns.Add("rcomdir")
    '            Tabla1.Columns.Add("rusocfdi")
    '            Tabla1.Columns.Add("nocuenta")

    '            Dim Fila As DataRow

    '            Fila = DS.Tables(0).NewRow() ' Acá insertas una fila nueva
    '            Fila("Erfc") = TRfc  ' Acá insertas valores a la nueva fila
    '            Fila("Erazonsoc") = TRazonSocial
    '            Fila("Eregfis") = TRFiscal.ToString + " - " + descreg.Trim
    '            Fila("Edireccion") = Tabla.Rows(0).Item("tdir").ToString + " " + Tabla.Rows(0).Item("tnumext").ToString + " " + Tabla.Rows(0).Item("tcolonia").ToString
    '            Fila("Edircomp") = Tabla.Rows(0).Item("tmun").ToString + " " + Tabla.Rows(0).Item("testado").ToString + " " + Tabla.Rows(0).Item("tlocal").ToString + " " + Tabla.Rows(0).Item("tpais").ToString

    '            Fila("exporta") = "01-No Aplica"

    '            Dim NumPiezas As Integer
    '            Cadena = txtFechaVenta.Value.ToString.Substring(0, 10) & "- " & Trim(Me.txtTransaccion.Text) & " -" & " $" & Str(FormatCurrency(Me.Total.Value))

    '            Fila("sucursal") = Cadena
    '            Fila("Rcp") = txtCP.Text 'TCp
    '            Fila("Rrfc") = txtRfc.Text '"XAXX010101000"
    '            Fila("Rrazonsoc") = txtRazonSocial.Text '"PUBLICO EN GENERAL"
    '            Fila("Rdireccion") = cDireccion '""
    '            Fila("Rregimen") = txtidrf.Text & " - " & txtdesrf.Text '
    '            Fila("rusocfdi") = "Uso CFDI: " & txtclvUsoCfdi.Text ' & " - " & txtUsoCfdi.Text '"S01"
    '            Fila("moneda") = "MXN"
    '            Fila("rcomdir") = cComDir
    '            Fila("nocuenta") = txtNumCuenta.Text

    '            Dim i As Integer
    '            Dim sb As Double = 0
    '            Dim des As Double = 0
    '            Dim iv As Double = 0
    '            Dim iiv As Double = 0
    '            Dim PuCa As Double = 0
    '            Dim Imp As Double = 0
    '            Dim bas As Double = 0
    '            Dim des1 As Double = 0
    '            Dim IMTE As Double = 0


    '            Dim Importe As Double = 0
    '            Dim Impuesto As String = ""
    '            Dim TasaOCuota As String = ""
    '            Dim TipoFactor As String = ""
    '            Dim Impu As String = ""
    '            Dim ToC As String = ""
    '            Dim TFac As String = ""
    '            Dim dcto As String = ""
    '            'Dim ipt As String = ""

    '            Dim Tabla2 As DataTable = DS.Tables.Add("Detalle")
    '            Tabla2.Columns.Add("Cantidad")
    '            Tabla2.Columns.Add("Unidad")
    '            Tabla2.Columns.Add("UniSAT")
    '            Tabla2.Columns.Add("Descrip")
    '            Tabla2.Columns.Add("Punitario")
    '            Tabla2.Columns.Add("Importe")
    '            Dim Fila2 As DataRow

    '            For i = 0 To Me.UgrTicket.Rows.Count - 1
    '                Dim codsat As String
    '                If Me.UgrTicket.Rows(i).Cells("codsat").Value = "" Then
    '                    codsat = "01010101"
    '                Else
    '                    codsat = Me.UgrTicket.Rows(i).Cells("codsat").Value
    '                End If
    '                Dim pun As Double = Format(Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value, "###,###.00")
    '                Dim producto As String = Trim(Me.UgrTicket.Rows(i).Cells("DESCRIPCION").Value)
    '                'Dim Puni As String
    '                'Puni = pun.Replace(",", "")
    '                Dim impt As Double = Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00")
    '                ' ipt = impt.ToString.Replace(",", "")
    '                IMTE = Format(Me.UgrTicket.Rows(i).Cells("Importe").Value, "###,###.00")
    '                sb += Format((Me.UgrTicket.Rows(i).Cells("PrecioUnitario").Value) * (Me.UgrTicket.Rows(i).Cells("Cantidad").Value), "###,###.00")
    '                NumPiezas += Me.UgrTicket.Rows(i).Cells("Cantidad").Value
    '                Fila2 = DS.Tables(1).NewRow() ' Acá insertas una fila nueva
    '                Fila2("Cantidad") = Trim(Me.UgrTicket.Rows(i).Cells("Cantidad").Value)
    '                Fila2("Unidad") = "PZA"
    '                Fila2("UniSAT") = "H87"
    '                Fila2("Descrip") = producto
    '                Fila2("Punitario") = Format(pun, "###,###.00")
    '                Fila2("Importe") = Format(impt, "###,###.00")
    '                DS.Tables(1).Rows.Add(Fila2)
    '                Dim descto As Double = 0

    '            Next

    '            Impuesto = "002"
    '            TasaOCuota = "0.160000"
    '            TipoFactor = "Tasa"
    '            Importe = Format(iv, "###,###.00")

    '            Dim TotalImpuestosTrasladados As Double = 0
    '            TotalImpuestosTrasladados = Format(iv, "###,###.00")

    '            'Agregamos los Impuestos
    '            EasyOne_cfdi33_SetComprobanteImpuestos(p, "", TotalImpuestosTrasladados)
    '            EasyOne_cfdi33_AddTrasladosTraslado(p, Impuesto, TipoFactor, TasaOCuota, Importe)


    '            Dim cantletra As String
    '            cantletra = Letras(Total)
    '            Fila("cantidadl") = cantletra

    '            Dim ValorTicket As String = Cadena
    '            NoCuenta = txtNumCuenta.Text

    '            Dim f1, f2, f3, freal, FeReg, hr As String
    '            hr = Now.ToLocalTime.ToString.Substring(11, 8)
    '            freal = f3 & "-" & Now.Date.ToString.Substring(3, 2) & "-" & Now.Date.ToString.Substring(0, 2) & "T" & hr

    '            Dim metpg As String
    '            Dim tipcom As String
    '            Dim formpg As String
    '            metpg = "PUE"
    '            tipcom = "I"

    '            Fila("metpago") = metpg
    '            If tipcom = "I" Then
    '                Fila("tipocomp") = "I - Ingreso"
    '            Else
    '                Fila("tipocomp") = "E - Egreso"
    '            End If
    '            Fila("formpago") = FormPago 'fp


    '            Application.DoEvents()
    '            Dim resultadoXML As String = [String].Empty
    '            resultadoXML = EasyOne_LeeResultado(p, 8)
    '            'Obtenemos el XML completo y timbrado
    '            Console.WriteLine(resultadoXML)

    '            Application.DoEvents()
    '            'IO.File.WriteAllText(Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml"), resultadoXML)

    '            '''''''''''''''''''''''''''''' RECORRO XML''''''''''''''''''''''''''''
    '            'Dim Version_xml As String
    '            'Dim VarConceptos As XmlNodeList
    '            Dim version As String
    '            'Dim uuid As String
    '            'Dim ftimb As String
    '            Dim rfcprov As String
    '            Dim sellocfd As String
    '            Dim nocert As String

    '            'Dim nav As XPathNavigator
    '            Dim documento As XmlDocument = New XmlDocument()
    '            Dim VarManager As XmlNamespaceManager = New XmlNamespaceManager(documento.NameTable)
    '            documento.Load(ofd.FileName)
    '            VarManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")
    '            Version_xml = documento.SelectSingleNode("/cfdi:Comprobante/@Version", VarManager).InnerText

    '            Dim Estatus As String = 1
    '            If Version_xml = "4.0" Then
    '                Fila("NoDoc") = documento.SelectSingleNode("/cfdi:Comprobante/@Folio", VarManager).InnerText
    '                Fila("sellodig") = documento.SelectSingleNode("/cfdi:Comprobante/@Sello", VarManager).InnerText
    '                Fila("lugaremi") = documento.SelectSingleNode("/cfdi:Comprobante/@LugarExpedicion", VarManager).InnerText
    '                Fila("fechaemi") = documento.SelectSingleNode("/cfdi:Comprobante/@Fecha", VarManager).InnerText
    '                Fila("serieceremi") = documento.SelectSingleNode("/cfdi:Comprobante/@NoCertificado", VarManager).InnerText
    '                uuid = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", VarManager).InnerText
    '                Fila("foliofisc") = uuid
    '                rfcprov = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@RfcProvCertif", VarManager).InnerText
    '                Fila("provcert") = rfcprov
    '                Fila("sellosat") = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloSAT", VarManager).InnerText
    '                nocert = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@NoCertificadoSAT", VarManager).InnerText
    '                Fila("certSAT") = nocert
    '                ftimb = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", VarManager).InnerText
    '                Fila("hryfechcer") = ftimb
    '                version = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@Version", VarManager).InnerText
    '                sellocfd = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@SelloCFD", VarManager).InnerText
    '                Fila("complesat") = "|" + version + "|" + uuid + "|" + ftimb + "|" + rfcprov + "||" + sellocfd + "|" + nocert + "||"

    '                RutaLogo = rutlogo 'RutFacGlobal & "\RecursosCFD33\Penguin.bmp"
    '                RutaQR = Trim(RFacImpresas & "\" & Trim(TRfc) & "-" & txtSerie.Text & "-" & txtFactura.Text & ".xml.png")

    '                '''''''''''''''''''''''''''''''''''''''''''''''''''
    '                '''
    '                Dim xe As XmlElement = documento.DocumentElement

    '                For Each el As XmlElement In xe
    '                    If el.Name = "Impresion" Then
    '                        Dim im As XmlElement = el
    '                        For Each x As XmlElement In im
    '                            If x.Name = "NumTicket" Then
    '                                If Uudiref Is Nothing Then
    '                                    fecVta = x.InnerText.Substring(0, 10)
    '                                Else
    '                                    fecVta = x.InnerText.Substring(21, 10)
    '                                End If
    '                            ElseIf x.Name = "IVA" Then
    '                                Iva = x.InnerText
    '                            End If
    '                        Next
    '                    End If
    '                Next

    '                If NodoDesc Is Nothing Then
    '                    'MessageBox.Show("Not exists")
    '                Else
    '                    Desc = documento.SelectSingleNode("/cfdi:Comprobante/@Descuento", VarManager).InnerText
    '                    'MessageBox.Show("Exists")
    '                End If
    '                If Uudiref Is Nothing Then
    '                    'MessageBox.Show("Not exists")
    '                Else
    '                    uuidrefe = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:CfdiRelacionados/cfdi:CfdiRelacionado/@UUID", VarManager).InnerText
    '                    'MessageBox.Show("Exists")
    '                End If

    '                Total = documento.SelectSingleNode("/cfdi:Comprobante/@Total", VarManager).InnerText
    '                Comprobante = documento.SelectSingleNode("/cfdi:Comprobante/@TipoDeComprobante", VarManager).InnerText
    '                MetPago = documento.SelectSingleNode("/cfdi:Comprobante/@MetodoPago", VarManager).InnerText
    '                uuid = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@UUID", VarManager).InnerText
    '                ftimb = documento.SelectSingleNode("/cfdi:Comprobante/cfdi:Complemento/tfd:TimbreFiscalDigital/@FechaTimbrado", VarManager).InnerText.Substring(0, 10)


    '                'Dim Fecha11, Fecha22, Fecha33, FePnotasF As String
    '                'Fecha11 = txtFechaVenta.Text.ToString.Substring(0, 2)
    '                'Fecha22 = txtFechaVenta.Text.ToString.Substring(3, 2)
    '                'Fecha33 = txtFechaVenta.Text.ToString.Substring(6, 4)
    '                'FePnotasF = Fecha33 & Fecha22 & Fecha11
    '                ''    fechareal = Fecha11 & "/" & Fecha22 & "/" & Fecha33.Substring(2, 2)
    '                ''    FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha22 & Fecha11
    '                ''    Fecha = fechareal

    '                'Dim Tterm = CInt(Mid(Me.txtTransaccion.Text, 1, 3))
    '                'Dim Nnota = CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text)))
    '                'Dim NotaVta As String = Tterm & Format(CDbl(Nnota), "0000")
    '                'Dim TieSuc As String = Me.grbTienda.Value & Me.grbSucursal.Value
    '                'Dim PorIva As Double = 16.0

    '                'objpro.Indicacion("Update bgxfacelec.VentaEnc set veEstatu = 1, veFac = '" & CInt(Me.txtFactura.Text) & "' Where veFolio = " & Me.grbTienda.Value & " and  veIdFolv = " & CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text))) & " and sid = " & Me.grbSucursal.Value & " and veTerm = " & CInt(Mid(Me.txtTransaccion.Text, 1, 3)) & " and  veEstado = 1 ", As400)
    '                'objpro.Indicacion("Update bgxfacelec.Serie set seCant = " & CInt(Me.txtFactura.Text) & " Where seSerie = '" & Me.txtSerie.Text & "' and  SId = " & Me.grbSucursal.Value & " and tid = " & Me.grbTienda.Value & "  and seEstat = 1", As400)
    '                'objpro.Indicacion("Update bgxfacelec.pfnotasf set NFSTS = 'F' Where NFSUC = " & CInt(TieSuc) & " and NFFECHA = " & Trim(FePnotasF) & " and NFNOTA = " & NotaVta & "", As400)
    '                'upb.Value += 10 'Suma 40 
    '                ExisteClave = CStr(objpro.DatoString("cid", "BGXFacElec.catclie", "crfc = '" & Trim(Me.txtRfc.Text) & "' ", As400))
    '                'Dim Tipo As String = "F"
    '                'Dim cantidadconletra As String = ""
    '                'Dim FeCan = 0
    '                'Dim FeMotCan As String = ""
    '                'Dim ferisr = 0
    '                'Dim feriva = 0

    '                'Dim Subtotal = Me.Subtotal.Value
    '                'Dim Iva = Me.Iva.Value
    '                'Dim Total1 = Me.Total.Value
    '                'Dim Descuento = Me.Descuento.Value

    '                'Dim FeRegistro, Fecha, fechareal, Fecha1, Fecha2, Fecha3 As String
    '                'Fecha1 = txtFechaVenta.Text.ToString.Substring(0, 2)
    '                'Fecha2 = txtFechaVenta.Text.ToString.Substring(3, 2)
    '                'Fecha3 = txtFechaVenta.Text.ToString.Substring(6, 4)
    '                'FePnotasF = Fecha3 & Fecha2 & Fecha1
    '                'fechareal = Fecha1 & "/" & Fecha2 & "/" & Fecha3.Substring(2, 2)
    '                'FeRegistro = txtFechaVenta.Text.ToString.Substring(6, 4) & Fecha2 & Fecha1
    '                'Fecha = fechareal

    '                'objpro.Indicacion("Insert into BGXFacElec.FEncabez (FeId,tId,FeTerm,FeFact,feSerie,sid," &
    '                '        " feFecha,feHora,Cid,FeSubt,fePorIva,FeIva, " &
    '                '        " FeDesc1,FePDesc1,FeDesc2,FePDesc2,FeTotal, " &
    '                '        " FeCanlet,FeAbonos,FeEstatu, FeReg, fefeccan, femotcan, fetip, feuuid, ferisr, feriva) " &
    '                '        " values (" & CInt(Mid(Me.txtTransaccion.Text, 5, Len(Me.txtTransaccion.Text))) & "," & grbTienda.Value & "," & CInt(Mid(Me.txtTransaccion.Text, 1, 3)) & "," & CInt(Me.txtFactura.Text) & ",'" & Me.txtSerie.Text & "'," & Me.grbSucursal.Value & ",'" & fechareal & "'," &
    '                '        " '" & Trim(FechaHora) & "', " & CInt(ExisteClave) & "," & Subtotal & ", " & CDbl(PorIva) & "," & Me.Iva.Value & "," &
    '                '        " " & Descuento & "," & Descuento & ", " & Descuento & ", " & Descuento & ", " & Me.Total.Value & ", " &
    '                '        " '" & cantletra & "', " & Total1 & "," & Estatus & "," & FeRegistro & ", '" & FeCan & "', '" & FeMotCan & "', '" & Tipo & "','" & uuid & "', " & ferisr & "," & feriva & ")", As400)

    '                'upb.Value += 20 'Suma 60
    '                ''''''nuevoooooooooooooooooooooooooooooooooooooooooo

    '                DS.Tables(0).Rows.Add(Fila)
    '                Dim objreport As New factura40
    '                objreport.SetParameterValue("Logo", RutaLogo)
    '                objreport.SetParameterValue("QR", RutaQR)
    '                Dim rpFact As New factura40
    '                rpFact.SetDataSource(DS)
    '                rpFact.SetParameterValue("QR", RutaQR)
    '                rpFact.SetParameterValue("Logo", RutaLogo)
    '                GFactura.CrystalReportViewer1.ReportSource = objreport
    '                GFactura.CrystalReportViewer1.ReportSource = rpFact
    '                GFactura.WindowState = FormWindowState.Maximized
    '                GFactura.Show()

    '                'upb.Value += 10 'Suma 70
    '                rutaArchivoPDF = RFacImpresas & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & Me.txtFactura.Text & ".pdf"
    '                Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()

    '                Dim cryRpt As New ReportDocument

    '                Try
    '                    Dim CrExportOptions As ExportOptions
    '                    Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
    '                    Dim crf As New PdfRtfWordFormatOptions()
    '                    CrDiskFileDestinationOptions.DiskFileName = rutaArchivoPDF
    '                    CrExportOptions = rpFact.ExportOptions
    '                    With CrExportOptions
    '                        .ExportDestinationType = ExportDestinationType.DiskFile
    '                        .ExportFormatType = ExportFormatType.PortableDocFormat
    '                        .DestinationOptions = CrDiskFileDestinationOptions
    '                        .FormatOptions = CrFormatTypeOptions
    '                    End With
    '                    rpFact.Export()
    '                    'upb.Value += 10 'Suma 80  
    '                Catch ex As Exception
    '                    MsgBox(ex.ToString)
    '                End Try

    '            Else
    '                MsgBox("Archivo XML no es una factura version 4.0")
    '            End If 'Cierra validación de operación exitosa
    '            'Cierra validación del try


    '            'Dim dt As DataTable = tables(cboSheet.SelectedItem.ToString())
    '            'DataGridView1.DataSource = dt
    '            'DataGridView1.Rows.Add(Folio, Serie, uuid, ftimb, fecVta, formpag, Subto, Iva, Desc, Total, Comprobante, MetPago, uuidrefe, Version_xml)
    '        End If
    '    End Using
    'End Sub
End Class