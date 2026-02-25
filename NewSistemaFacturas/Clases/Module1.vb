Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml
Imports System.Xml.XPath
Imports CrystalDecisions
Imports System.Data
Imports System.Data.Odbc

Module Module1

    Dim parm As New factura40
    Public RutaLogo As String
    Public RutaQR As String
    Public RutaLogoN As String
    Public RutaQRN As String

#Region "para dataset"
    Public Conexion As OleDb.OleDbConnection
    Public Comando As OleDb.OleDbCommand
    Public Adaptador As OleDb.OleDbDataAdapter
#End Region


#Region "wrappers cfd33"
    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_AddCfdiRelacionado(ByVal p As Integer, ByVal UUID As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_AddConcepto(ByVal p As Integer, ByVal ClaveProdServ As String, ByVal NoIdentificacion As String, ByVal Cantidad As String, ByVal ClaveUnidad As String, ByVal Unidad As String, ByVal Descripcion As String, ByVal ValorUnitario As String, ByVal Importe As String, ByVal Descuento As String)
    'End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_AddInformacionAduanera(ByVal p As Integer, ByVal NumeroPedimento As String)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_AddParte(ByVal p As Integer, ByVal ClaveProdServ As String, ByVal NoIdentificacion As String, ByVal Cantidad As String, ByVal Unidad As String, ByVal Descripcion As String, ByVal ValorUnitario As String, ByVal Importe As String)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_AddParteInformacionAduanera(ByVal p As Integer, ByVal NumeroPedimento As String)
    End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_AddRetencion(ByVal p As Integer, ByVal Base As String, ByVal Impuesto As String, ByVal TipoFactor As String, ByVal TasaOCuota As String, ByVal Importe As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_AddRetencionesRetencion(ByVal p As Integer, ByVal Impuesto As String, ByVal Importe As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_AddTraslado(ByVal p As Integer, ByVal Base As String, ByVal Impuesto As String, ByVal TipoFactor As String, ByVal TasaOCuota As String, ByVal Importe As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_AddTrasladosTraslado(ByVal p As Integer, ByVal Impuesto As String, ByVal TipoFactor As String, ByVal TasaOCuota As String, ByVal Importe As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Function EasyOne_cfdi33_Create(ByVal outFile As String) As Integer
    'End Function

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_Free(ByVal p As Integer)
    End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_SetCfdiRelacionados(ByVal p As Integer, ByVal TipoRelacion As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_SetComprobante(ByVal p As Integer, ByVal Serie As String, ByVal Folio As String, ByVal Fecha As String, ByVal FormaPago As String, ByVal CondicionesDePago As String, ByVal SubTotal As String, ByVal Descuento As String, ByVal Moneda As String, ByVal TipoCambio As String, ByVal Total As String, ByVal TipoDeComprobante As String, ByVal MetodoPago As String, ByVal LugarExpedicion As String, ByVal Confirmacion As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_SetComprobanteImpuestos(ByVal p As Integer, ByVal TotalImpuestosRetenidos As String, ByVal TotalImpuestosTrasladados As String)
    'End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_SetCuentaPredial(ByVal p As Integer, ByVal Numero As String)
    End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_SetEmisor(ByVal p As Integer, ByVal Rfc As String, ByVal Nombre As String, ByVal RegimenFiscal As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_cfdi33_SetReceptor(ByVal p As Integer, ByVal Rfc As String, ByVal Nombre As String, ByVal ResidenciaFiscal As String, ByVal NumRegIdTrib As String, ByVal UsoCFDI As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Sub EasyOne_SetInvoiceOneFachadaInfoPrueba(ByVal p As Integer, ByVal AgenteId As String, ByVal SoftwareId As String)
    'End Sub

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Function EasyOne_LeeResultado(ByVal p As Integer, ByVal valor As Integer) As String
    'End Function

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Function EasyOne_FirmaComprobante(ByVal p As Integer, ByVal csd As String, ByVal key As String, ByVal pwd As String) As Integer
    'End Function

    '<DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    'Public Function EasyOne_SetQrCodeFormat(ByVal p As Integer, ByVal Img As String) As Integer
    'End Function
#End Region

#Region "CFDI"

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetReceptor40(ByVal p As Integer, ByVal DomicilioFiscalReceptor As String, ByVal RegimenFiscalReceptor As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetReceptor(ByVal p As Integer, ByVal Rfc As String, ByVal Nombre As String, ByVal ResidenciaFiscal As String, ByVal NumRegIdTrib As String, ByVal UsoCFDI As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetCfdiRelacionados(ByVal p As Integer, ByVal TipoRelacion As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_AddCfdiRelacionado(ByVal p As Integer, ByVal UUID As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetEmisor(ByVal p As Integer, ByVal Rfc As String, ByVal Nombre As String, ByVal RegimenFiscal As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetConcepto40(ByVal p As Integer, ByVal ObjetoImp As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetComprobante(ByVal p As Integer, ByVal Serie As String, ByVal Folio As String, ByVal Fecha As String, ByVal FormaPago As String, ByVal CondicionesDePago As String, ByVal SubTotal As String, ByVal Descuento As String, ByVal Moneda As String, ByVal TipoCambio As String, ByVal Total As String, ByVal TipoDeComprobante As String, ByVal MetodoPago As String, ByVal LugarExpedicion As String, ByVal Confirmacion As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetCfdi40(ByVal p As Integer, ByVal Exportacion As String, ByVal EmisorFacAtrAdquirente As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Function EasyOne_cfdi33_Create(ByVal OutFile As String) As Integer
    End Function

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_AddTraslado(ByVal p As Integer, ByVal Base As String, ByVal Impuesto As String, ByVal TipoFactor As String, ByVal TasaOCuota As String, ByVal Importe As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_AddTrasladosTraslado(ByVal p As Integer, ByVal Impuesto As String, ByVal TipoFactor As String, ByVal TasaOCuota As String, ByVal Importe As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_AddConcepto(ByVal p As Integer, ByVal ClaveProdServ As String, ByVal NoIdentificacion As String, ByVal Cantidad As String, ByVal ClaveUnidad As String, ByVal Unidad As String, ByVal Descripcion As String, ByVal ValorUnitario As String, ByVal Importe As String, ByVal Descuento As String)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_AddRetencion(ByVal p As Integer, ByVal Base As String, ByVal Impuesto As String, ByVal TipoFactor As String, ByVal TasaOCuota As String, ByVal Importe As String)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_AddRetencionesRetencion(ByVal p As Integer, ByVal Impuesto As String, ByVal Importe As String)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_cfdi33_SetComprobanteImpuestos(ByVal p As Integer, ByVal TotalImpuestosRetenidos As String, ByVal TotalImpuestosTrasladados As String)
    End Sub

#End Region

#Region "Pagos 2.0"
    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_SetVersion(ByVal p As Integer, ByVal version As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_SetTotales20(ByVal p As Integer, ByVal TotalRetencionesIVA As String, ByVal TotalRetencionesISR As String, ByVal TotalRetencionesIEPS As String, ByVal TotalTrasladosBaseIVA16 As String, ByVal TotalTrasladosImpuestoIVA16 As String, ByVal TotalTrasladosBaseIVA8 As String, ByVal TotalTrasladosImpuestoIVA8 As String, ByVal TotalTrasladosBaseIVA0 As String, ByVal TotalTrasladosImpuestoIVA0 As String, ByVal TotalTrasladosBaseIVAExento As String, ByVal MontoTotalPagos As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_SetDoctoRelacionado20(ByVal p As Integer, ByVal EquivalenciaDR As String, ByVal ObjetoImpDR As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Function EasyOne_pago10_Create() As Integer
    End Function

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_AddTrasladoP20(ByVal p As Integer, ByVal BaseP As String, ByVal ImpuestoP As String, ByVal TipoFactorP As String, ByVal TasaOCuotaP As String, ByVal ImporteP As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_AddTrasladoDR20(ByVal p As Integer, ByVal BaseDR As String, ByVal ImpuestoDR As String, ByVal TipoFactorDR As String, ByVal TasaOCuotaDR As String, ByVal ImporteDR As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_AddPago(ByVal p As Integer, ByVal FechaPago As String, ByVal FormaDePagoP As String, ByVal MonedaP As String, ByVal TipoCambioP As String, ByVal Monto As String, ByVal NumOperacion As String, ByVal RfcEmisorCtaOrd As String, ByVal NomBancoOrdExt As String, ByVal CtaOrdenante As String, ByVal RfcEmisorCtaBen As String, ByVal CtaBeneficiario As String, ByVal TipoCadPago As String, ByVal CertPago As String, ByVal CadPago As String, ByVal SelloPago As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_pago10_AddDoctoRelacionado(ByVal p As Integer, ByVal IdDocumento As String, ByVal Serie As String, ByVal Folio As String, ByVal MonedaDR As String, ByVal TipoCambioDR As String, ByVal MetodoDePagoDR As String, ByVal NumParcialidad As String, ByVal ImpSaldoAnt As String, ByVal ImpPagado As String, ByVal ImpSaldoInsoluto As String)
    End Sub
#End Region


#Region "wrappers easyone 40"

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl)>
    Public Function EasyOne_LeeResultado(ByVal p As Integer, ByVal valor As Integer) As String
    End Function

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Function EasyOne_FirmaComprobante(ByVal p As Integer, ByVal csd As String, ByVal key As String, ByVal pwd As String) As Integer
    End Function

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_SetInvoiceOneFachadaInfoPrueba(ByVal p As Integer, ByVal AgenteId As String, ByVal SoftwareId As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cfdi33_SetInformacionGlobal40(ByVal p As Integer, ByVal Periocidad As String, ByVal Meses As String, ByVal Anio As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_SetInvoiceOneFachadaInfo(ByVal p As Integer, ByVal AgenteId As String, ByVal SoftwareId As String)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_AddComplemento(ByVal p As Integer, ByVal pComp As Integer)
    End Sub

    <DllImport("EasyOne.DLL", CallingConvention:=CallingConvention.Cdecl)>
    Public Sub EasyOne_SetQrCodeFormat(ByVal p As Integer, ByVal Img As String)
    End Sub

#End Region

#Region "wrappers CartaPorte 2.0"
    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Function EasyOne_cartaporte20_Create() As Integer
    End Function

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetUbicacionDomicilio(ByVal p As Integer, ByVal Calle As String, ByVal NumeroExterior As String, ByVal NumeroInterior As String, ByVal Colonia As String, ByVal Localidad As String, ByVal Referencia As String, ByVal Municipio As String, ByVal Estado As String, ByVal Pais As String, ByVal CodigoPostal As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetMercancias(ByVal p As Integer, ByVal PesoBrutoTotal As String, ByVal UnidadPeso As String, ByVal PesoNetoTotal As String, ByVal NumTotalMercancias As String, ByVal CargoPorTasacion As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetMaritimoOpt(ByVal p As Integer, ByVal PermSCT As String, ByVal NumPermisoSCT As String, ByVal NombreAseg As String, ByVal NumPolizaSeguro As String, ByVal AnioEmbarcacion As String, ByVal NombreEmbarc As String, ByVal Eslora As String, ByVal Manga As String, ByVal Calado As String, ByVal LineaNaviera As String, ByVal NumViaje As String, ByVal NumConocEmbarc As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetMaritimo(ByVal p As Integer, ByVal TipoEmbarcacion As String, ByVal Matricula As String, ByVal NumeroOMI As String, ByVal NacionalidadEmbarc As String, ByVal UnidadesDeArqBruto As String, ByVal TipoCarga As String, ByVal NumCertITC As String, ByVal NombreAgenteNaviero As String, ByVal NumAutorizacionNaviero As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetFiguraTransporteDomicilio(ByVal p As Integer, ByVal Calle As String, ByVal NumeroExterior As String, ByVal NumeroInterior As String, ByVal Colonia As String, ByVal Localidad As String, ByVal Referencia As String, ByVal Municipio As String, ByVal Estado As String, ByVal Pais As String, ByVal CodigoPostal As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetFerroviario(ByVal p As Integer, ByVal TipoDeServicio As String, ByVal TipoDeTrafico As String, ByVal NombreAseg As String, ByVal NumPolizaSeguro As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetDetalleMercancia(ByVal p As Integer, ByVal UnidadPeso As String, ByVal PesoBruto As String, ByVal PesoNeto As String, ByVal PesoTara As String, ByVal NumPiezas As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetCartaPorte(ByVal p As Integer, ByVal TranspInternac As String, ByVal EntradaSalidaMerc As String, ByVal PaisOrigenDestino As String, ByVal ViaEntradaSalida As String, ByVal TotalDistRec As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetAutotransporteSeguros(ByVal p As Integer, ByVal AseguraRespCivil As String, ByVal PolizaRespCivil As String, ByVal AseguraMedAmbiente As String, ByVal PolizaMedAmbiente As String, ByVal AseguraCarga As String, ByVal PolizaCarga As String, ByVal PrimaSeguro As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetAutotransporteIdentificacionVehicular(ByVal p As Integer, ByVal ConfigVehicular As String, ByVal PlacaVM As String, ByVal AnioModeloVM As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetAutotransporte(ByVal p As Integer, ByVal PermSCT As String, ByVal NumPermisoSCT As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_SetAereo(ByVal p As Integer, ByVal PermSCT As String, ByVal NumPermisoSCT As String, ByVal MatriculaAeronave As String, ByVal NombreAseg As String, ByVal NumPolizaSeguro As String, ByVal NumeroGuia As String, ByVal LugarContrato As String, ByVal CodigoTransportista As String, ByVal RFCEmbarcador As String, ByVal NumRegIdTribEmbarc As String, ByVal ResidenciaFiscalEmbarc As String, ByVal NombreEmbarcador As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddUbicacion(ByVal p As Integer, ByVal TipoUbicacion As String, ByVal IDUbicacion As String, ByVal RFCRemitenteDestinatario As String, ByVal NombreRemitenteDestinatario As String, ByVal NumRegIdTrib As String, ByVal ResidenciaFiscal As String, ByVal NumEstacion As String, ByVal NombreEstacion As String, ByVal NavegacionTrafico As String, ByVal FechaHoraSalidaLlegada As String, ByVal TipoEstacion As String, ByVal DistanciaRecorrida As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddMercanciaPedimento(ByVal p As Integer, ByVal Pedimento As String)
    End Sub


    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddMercanciaGuiasIdentificacion(ByVal p As Integer, ByVal NumeroGuiaIdentificacion As String, ByVal DescripGuiaIdentificacion As String, ByVal PesoGuiaIdentificacion As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddMercanciaCantidadTransporta(ByVal p As String, ByVal Cantidad As String, ByVal IDOrigen As String, ByVal IDDestino As String, ByVal CvesTransporte As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddMercancia(ByVal p As Integer, ByVal BienesTransp As String, ByVal ClaveSTCC As String, ByVal Descripcion As String, ByVal Cantidad As String, ByVal ClaveUnidad As String, ByVal Unidad As String, ByVal Dimensiones As String, ByVal MaterialPeligroso As String, ByVal CveMaterialPeligroso As String, ByVal Embalaje As String, ByVal DescripEmbalaje As String, ByVal PesoEnKg As String, ByVal ValorMercancia As String, ByVal Moneda As String, ByVal FraccionArancelaria As String, ByVal UUIDComercioExt As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddMaritimoContenedor(ByVal p As Integer, ByVal MatriculaContenedor As String, ByVal TipoContenedor As String, ByVal NumPrecinto As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddFiguraTransportePartes(ByVal p As Integer, ByVal ParteTransporte As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddFiguraTransporte(ByVal p As Integer, ByVal TipoFigura As String, ByVal RFCFigura As String, ByVal NumLicencia As String, ByVal NombreFigura As String, ByVal NumRegIdTribFigura As String, ByVal ResidenciaFiscalFigura As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddFerroviarioDerechosDePaso(ByVal p As Integer, ByVal TipoDerechoDePaso As String, ByVal KilometrajePagado As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddFerroviarioContenedor(ByVal p As Integer, ByVal TipoContenedor As String, ByVal PesoContenedorVacio As String, ByVal PesoNetoMercancia As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddFerroviarioCarro(ByVal p As Integer, ByVal TipoCarro As String, ByVal MatriculaCarro As String, ByVal GuiaCarro As String, ByVal ToneladasNetasCarro As String)
    End Sub

    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Sub EasyOne_cartaporte20_AddAutotransporteRemolque(ByVal p As Integer, ByVal SubTipoRem As String, ByVal Placa As String)
    End Sub

#End Region



#Region "cancelacion 40"
    <DllImport("EasyOne.dll", CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi)>
    Public Function EasyOne_CancelaComprobante_v4(ByVal csd As String, ByVal key As String, ByVal pwd As String, ByVal uuid As String, ByVal rfcEmisor As String, ByVal agenteId As String, ByVal softwareId As String, ByVal rfcReceptor As String, ByVal total As String, ByVal motivo As String, ByVal FolioSustitucion As String) As Integer
    End Function

#End Region
End Module
