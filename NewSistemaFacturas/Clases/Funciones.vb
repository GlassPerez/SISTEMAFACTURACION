Imports System.Configuration
Imports Infragistics.Win.UltraWinGrid
Imports Microsoft.VisualBasic
Imports System.Diagnostics.Process
Imports System.Runtime.InteropServices.Marshal
Imports System.Drawing
Imports System.Data.OleDb
Imports System.Net

Public Class Funciones

#Region "Variables"
    Dim Oldbcon As OleDbConnection
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim i As Integer
    Dim cadCon As String    
#End Region

    Function EjecutaSP_ds(ByVal cadenas As String, ByVal cadoldb As String) As Data.DataSet
        Dim ds As New Data.DataSet
        Oldbcon = New OleDbConnection
        Try
            Application.DoEvents()
            Oldbcon.ConnectionString = cadoldb
            Oldbcon.Open()            

            cmd = New OleDbCommand(cadenas, Oldbcon)
            cmd.Connection = Oldbcon
            da = New OleDbDataAdapter
            da.SelectCommand = cmd
            da.SelectCommand.CommandText = cadenas
            Application.DoEvents()
            da.Fill(ds)
            Application.DoEvents()
            Oldbcon.Close()
        Catch ex As Exception
            MsgBox(ex.Message.ToString, "ERROR")
        End Try

        Return ds
    End Function


    Public Sub FormatoUltragrid(ByVal Grd As UltraGrid, ByVal ColOcultaIni As Integer, ByVal ColOcultaFin As Integer, ByVal ColMuestra As String, ByVal DesAct As String)
        Dim i As Integer
        Dim Partes() As String
        Try
            For i = ColOcultaIni To ColOcultaFin
                OcultaColUltragrid(Grd, i)
            Next
        Catch ex As Exception
        End Try

        Try
            If ColMuestra <> "" Then
                Partes = Split(ColMuestra, "-")
                For i = 0 To Partes.GetUpperBound(0)
                    MuestracolUltraGrid(Grd, Partes(i))
                Next
            End If
        Catch ex As Exception
        End Try

        Try
            If DesAct <> "" Then
                Partes = Split(DesAct, "-")
                For i = 0 To Partes.GetUpperBound(0)
                    DesactivaColUltraGrid(Grd, Partes(i))
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub OcultaColUltragrid(ByVal Grd As UltraGrid, ByVal columna As Integer)
        With Grd
            Try
                Dim co As UltraGridColumn = .DisplayLayout.Bands(0).Columns(columna)
                co.Hidden = True
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub

    Public Sub MuestracolUltraGrid(ByVal Grd As UltraGrid, ByVal columna As Integer)
        With Grd
            Try
                Dim co As UltraGridColumn = .DisplayLayout.Bands(0).Columns(columna)
                co.Hidden = False
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End With
    End Sub

    Public Sub DesactivaColUltraGrid(ByVal Grd As UltraGrid, ByVal columna As Integer)
        With Grd
            Try
                Dim co As UltraGridColumn = .DisplayLayout.Bands(0).Columns(columna)
                co.CellActivation = Activation.NoEdit
            Catch ex As Exception
            End Try
        End With
    End Sub

    Public Sub PintaColUltragrid(ByVal Grd As UltraGrid, ByVal Columna As String, ByVal Renglones As Integer) ', ByVal Colores As Color
        With Grd
            Try
                Dim C0 As UltraGridCell = .DisplayLayout.Rows(Renglones).Cells(Columna)
                Dim Colores As Color
                C0.Appearance.BackColor = Colores
            Catch ex As Exception
            End Try
        End With
    End Sub

    Function Registros(ByVal Instruccion As String, ByVal cadoldb As String)
        Try
            Oldbcon = New OleDbConnection
            Oldbcon.ConnectionString = cadoldb
            Oldbcon.Close()
            Oldbcon.Open()
            cmd = New OleDbCommand(Instruccion, Oldbcon)
            cmd.ExecuteNonQuery()
            Oldbcon.Close()
        Catch ex As Exception
            MsgBox(ex.Message, "ERROR")
        End Try
        Return ""
    End Function

    Public Function ConsultaSelect(ByVal Campos As String, ByVal Tabla As String, ByVal Condicion As String, ByVal Ordenado As String, ByVal coneccion As String) As System.Data.DataSet
        Dim as400 As String
        as400 = "select " & Campos & " from " & Tabla
        If Condicion <> "" Then as400 = as400 & " Where " & Condicion
        If Ordenado <> "" Then as400 = as400 & " Order By " & Ordenado
        Return EjecutaSP_ds(as400, coneccion)
    End Function

    Public Function ConsultaSelect2(ByVal Campos As String, ByVal Tabla As String, ByVal Condicion As String, ByVal Ordenado As String, ByVal coneccion As String) As System.Data.DataSet
        Dim as400 As String
        as400 = "select " & Campos & " from " & Tabla
        If Condicion <> "" Then as400 = as400 & " Where " & Condicion
        If Ordenado <> "" Then as400 = as400 & " Order By " & Ordenado
        Return EjecutaSP_ds(as400, coneccion)
    End Function

    Function LlenaCombos(ByVal cadena As String, ByVal cadoldb As String) As Data.DataSet
        Dim ds As New Data.DataSet
        Oldbcon = New OleDbConnection
        Oldbcon.ConnectionString = cadoldb
        Oldbcon.Open()
        cmd = New OleDbCommand(cadena, Oldbcon)
        cmd.Connection = Oldbcon
        da = New OleDbDataAdapter
        da.SelectCommand = cmd
        da.SelectCommand.CommandText = cadena
        da.Fill(ds)
        Oldbcon.Close()
        Return ds
    End Function

#Region "Globales"
    Function ObtenIp()
        Dim Ips As IPHostEntry = Dns.GetHostByName(Dns.GetHostName)
        Dim Direcciones As IPAddress() = Ips.AddressList
        Return Direcciones(0).ToString
    End Function
#End Region

#Region "Horas"
    Function Horas(ByVal tipo As String)
        Dim hrs As String
        Select Case tipo
            Case 1
                hrs = "13"
            Case 2
                hrs = "14"
            Case 3
                hrs = "15"
            Case 4
                hrs = "16"
            Case 5
                hrs = "17"
            Case 6
                hrs = "18"
            Case 7
                hrs = "19"
            Case 8
                hrs = "20"
            Case 9
                hrs = "21"
            Case 10
                hrs = "22"
            Case 11
                hrs = "23"
            Case 12
                hrs = "24"
        End Select

        Return hrs
    End Function
#End Region

#Region "Mes"
    Function EscojeMes(ByVal BuscaMes As String)
        Dim Mes As String
        Select Case BuscaMes
            Case "ENERO"
                Mes = "01"
            Case "FEBRERO"
                Mes = "02"
            Case "MARZO"
                Mes = "03"
            Case "ABRIL"
                Mes = "04"
            Case "MAYO"
                Mes = "05"
            Case "JUNIO"
                Mes = "06"
            Case "JULIO"
                Mes = "07"
            Case "AGOSTO"
                Mes = "08"
            Case "SEPTIEMBRE"
                Mes = "09"
            Case "OCTUBRE"
                Mes = "10"
            Case "NOVIEMBRE"
                Mes = "11"
            Case "DICIEMBRE"
                Mes = "12"
        End Select
        Return Mes
    End Function
#End Region


End Class

