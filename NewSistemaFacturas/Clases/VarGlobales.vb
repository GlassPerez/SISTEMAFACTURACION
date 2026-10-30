Imports System.Configuration

Module VarGlobales

#Region "Conexion"
    Public As400 As String = ConfigurationManager.ConnectionStrings("As400").ConnectionString
    Public Motor As String = ConfigurationManager.ConnectionStrings("Motor").ConnectionString
    Public MotorBoutique As String = ConfigurationManager.ConnectionStrings("MotorBoutique").ConnectionString
    Public conNomina As String = ConfigurationManager.ConnectionStrings("conNomina").ConnectionString

#End Region

#Region "Acceso"
    Public Autorizado As Boolean
#End Region

#Region "Sistema"
    Public IP As String
    Public NombrePC As String = SystemInformation.ComputerName
    Public Tienda As String = ConfigurationManager.AppSettings("NumTienda")
    Public Sucursal As String = ConfigurationManager.AppSettings("NumSucursal")
    Public RutaArchivos As String = ConfigurationManager.AppSettings("RutaApoyoFactura")
    Public RFCPG As String = ConfigurationManager.AppSettings("RFCGeneral")
    Public TipoSistema As String = ConfigurationManager.AppSettings("TipoSistema")
    Public USUAR As String = ConfigurationManager.AppSettings("USER")
    Public PASSWRD As String = ConfigurationManager.AppSettings("PASW")
    Public LIIB As String
    Public FecVenta As String
    Public filtroP As String
    Public Tienda400 As Integer
    Public Suc400 As Integer
#End Region

#Region "Var Solicitante"
    Public VacanteId As Integer
    Public SolicitanteEmail As String
    Public SolicitaId As Integer
#End Region

#Region "Bosco"
    Public idCliente As Integer
    Public DevTienda As Integer
    Public DevSucursal As Integer
    Public Devfeid As Integer
    Public DevUUID As String
    Public DevReg As String
    Public FecFac As String
    Public TotFac As Double
    Public dfefact As String
    Public termin As Integer
    Public transacc As Integer
    Public FESERIE As String
#End Region

#Region "Pagos"
    Public P_Id As Integer
    Public P_Tid As Integer
    Public P_Suc As Integer
    Public P_Term As Integer
    Public P_Fact As Integer
    Public P_Serie As String
#End Region

#Region "UsoCFDI"
    Public cfdiuso As String
    Public cfdides As String
    Public UsoCFDI As Integer
    Public regimenFisc As String
    Public regimen As String
    Public regFiscal As Integer
#End Region

End Module
