Imports Microsoft.VisualBasic
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win


Public Class Procedures

#Region "Variables"
    Dim ObjConecta As New Funciones
#End Region

    Public Function ObtenDatos(ByVal seleccion As String, ByVal coneccion As String) As System.Data.DataSet
        Return ObjConecta.EjecutaSP_ds(seleccion, coneccion)
    End Function

    Public Sub LLenaGrid(ByVal gr As UltraGrid, ByVal cadena As String, ByVal Opcion As String, ByVal bd As String)
        gr.DataSource = ObtenDatos(cadena, bd)
    End Sub

    Public Function Indicacion(ByVal Operacion As String, ByVal Coneccion As String)
        Return ObjConecta.Registros(Operacion, Coneccion)
    End Function

    Public Function DatoString(ByVal campo As String, ByVal tabla As String, ByVal where As String, ByVal Coneccion As String) As String
        Try
            Return ObjConecta.ConsultaSelect("(" & campo & ") as Dato", tabla, where, "", Coneccion).Tables(0).Rows(0).Item(0)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function DatoCount(ByVal Campo As String, ByVal Tabla As String, ByVal Where As String, ByVal Coneccion As String) As Integer
        Try
            Return ObjConecta.ConsultaSelect("Count (" & Campo & ") As Dato", Tabla, Where, "", Coneccion).Tables(0).Rows(0).Item(0)
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function DatoMaximo(ByVal Campo As String, ByVal Tabla As String, ByVal Where As String, ByVal Coneccion As String) As Double
        Try
            Return ObjConecta.ConsultaSelect("Max (" & Campo & ") As Dato", Tabla, Where, "", Coneccion).Tables(0).Rows(0).Item(0)
        Catch ex As Exception
            Return 0
        End Try
    End Function


    Public Function DatoExiste(ByVal Campo As String, ByVal Tabla As String, ByVal Where As String, ByVal Coneccion As String) As Boolean
        Try
            If ObjConecta.ConsultaSelect("(" & Campo & ") as Dato", Tabla, Where, "", Coneccion).Tables(0).Rows.Count >= 1 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub CargaCombos(ByVal combo As UltraCombo, ByVal consulta As String, ByVal id As Integer, ByVal Ncol As Integer, ByVal Coneccion As String)
        combo.DataSource = Nothing
        Try
            If ObjConecta.LlenaCombos(consulta, Coneccion).Tables(0).Rows.Count > 0 Then                
                combo.ValueMember = ObjConecta.LlenaCombos(consulta, Coneccion).Tables(0).Columns(0).Caption
                Application.DoEvents()
                combo.DisplayMember = ObjConecta.LlenaCombos(consulta, Coneccion).Tables(0).Columns(Ncol).Caption
                Application.DoEvents()
                combo.DataSource = ObjConecta.LlenaCombos(consulta, Coneccion).Tables(0)
                Application.DoEvents()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Function Dato(ByVal Campo As String, ByVal Tabla As String, ByVal Where As String, ByVal coneccion As String) As String
        Return ObjConecta.ConsultaSelect(" (" & Campo & ") as Dato", Tabla, Where, "", coneccion).Tables(0).Rows(0).Item(0)
    End Function

    'Public Function Dato2(ByVal Campo As String, ByVal Tabla As String, ByVal Where As String, ByVal coneccion As String) As String
    '    Return ObjConecta.ConsultaSelect2(" (" & Campo & ") as Dato", Tabla, Where, "", coneccion).Tables(0).Rows(0).Item(0)
    'End Function
End Class