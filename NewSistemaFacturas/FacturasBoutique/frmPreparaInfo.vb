Imports System
Imports N400
Imports N400.Componentes
Imports N400.Componentes.CFDI.CFDI33
Imports System.Runtime.InteropServices
Imports N400.QR
Imports System.IO
Imports System.Xml
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Xml.Linq
Imports System.Xml.XPath

Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Public Class frmPreparaInfo
#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
    'Dim datosG As New dsFactura40.dsFac40DataTable
#End Region

#Region "VariablesGlobales"
    Dim contador As Integer = 0
    Dim TIP As String
    Dim fechventa As String
    Dim fechFin As String
    Dim FechaFactura As String
    Dim term As String

    Dim tventas, tventas2, tvtas, tvtas2, OKVTA, chk As Data.DataTable
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

    Dim TE1 As Int16 = 0
    Dim nfol2 As Int16 = 0
    Dim SUBT As Double = 0
    Dim DESCU1 As Double = 0
    Dim PDESC1 As Double = 0
    Dim DESCUE2 As Double = 0
    Dim PDESC2 As Double = 0
    Dim TOTAL As Double = 0
    Dim TIVA As Double = 0
    Dim ter As Int16 = 0
    Dim pfec As String = ""
    Dim descu As Double = 0
    Dim descOK As Double = 0
    Dim prec1 As Double = 0
    ' Dim descFG As Double = 0
#End Region

#Region "Datos de Factura"
    Dim txtFactura As String
    Dim txtSerie As String
    Dim txtNumAprobacion As String
    Dim txtAmoAprob As String
    Dim txtLugarExpedicion As String
#End Region

    Private Sub frmPreparaInfo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub btnBuscar_Click_1(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If grbTienda.Text <> "" Then
            If Me.grbSucursal.Text <> "" Then
                CargaVentas()
            Else
                MsgBox("Falta sucursal", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta seleccionar Tienda o Razón social", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub frmPreparaInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FechaFactura = Date.Now.ToString.Substring(0, 10)

        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien ", 0, 1, Motor)
        'Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        'Me.grbPeriodo.Text = Now.Date.Year
        fechventa = Now.Date.Day.ToString + "/" + Now.Date.Month.ToString + "/" + Now.Date.Year.ToString
        fechFin = Now.Date.Day.ToString + "/" + Now.Date.Month.ToString + "/" + Now.Date.Year.ToString
        FechaInicio.Value = CDate(fechventa)
        'FechaFin.Value = CDate(fechFin)
        Application.DoEvents()
    End Sub

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


    Private Sub CargaVentas()
        Application.DoEvents()
        OKVTA = objpro.ObtenDatos("SELECT VTAENC.VEIDFOLV AS folvta, VTAENC.VETOTAL AS vtotal " &
                                    "FROM VENTAENC AS VTAENC " &
                                    "WHERE VTAENC.SID = " & grbSucursal.Value & " AND VTAENC.VEESTADO = 1 and day(VTAENC.VEFECHA) = " & FechaInicio.Value.ToString.Substring(0, 2) & " and month(VTAENC.VEFECHA) = " & FechaInicio.Value.ToString.Substring(3, 2) & " and year(VTAENC.VEFECHA) = " & FechaInicio.Value.ToString.Substring(6, 4) & " ", Motor).Tables(0)

        If OKVTA.Rows.Count <> 0 Then
            MsgBox("YA SE HA CARGADO INFORMACIÓN DE VENTAS DEL PERIODO SELECCIONADO", MsgBoxStyle.Information, "Facturación .. ")
        Else

            If Me.grbSucursal.Value > 9 Then
                term = Trim(grbTienda.Value.ToString.Substring(0, 1)) & Trim(Str(Me.grbSucursal.Value))
            Else
                term = Trim(Str(grbTienda.Value)) + Trim(Str(Me.grbSucursal.Value))
            End If

            Upgbar.Value = 0
            Upgbar.Minimum = 0
            Upgbar.Maximum = 100 'tventas.Rows.Count

            Application.DoEvents()
            tventas = objpro.ObtenDatos("SELECT mvtapos1.mvpsuc AS SUC, mvtapos1.mvpfec AS FECVTA, mvtapos1.mvpter AS TER, mvtapos1.mvpfol AS NOTA, mvtapos.mvptmov AS MOV, mvtapos1.mcpren AS RENG, mvtapos1.mcpcodigo AS COD, mvtapos1.mcpdescri AS DES, " &
                                         "mvtapos1.mvpcant AS CANT, mvtapos1.mvpprec AS PREC, mvtapos1.mvpdes1 AS DES1, mvtapos1.mvpdes2 AS DES2, mvtapos1.mvpboni AS BON, mvtapos1.mvpdese AS DESE, mvtapos1.mvpbone AS BONE, mvtapos1.mvpimpt AS IMPT, " &
                                         "mvtapos1.mvpmon AS MON, mvtapos1.mvpcto AS PCTO, mvtapos1.mvpvenet AS VENET, mvtapos.mvpSTSF AS FAC " &
                                         "FROM mvtapos inner join mvtapos1 on mvtapos.mvpfec = mvtapos1.mvpfec and mvtapos.mvpfol = mvtapos1.mvpfol and mvtapos.mvpter = mvtapos1.mvpter " &
                                         "WHERE mvtapos.mvpsts = 'P' and mvtapos.mvpfill <> 'R' AND (mvtapos.mvpSTSF <> 'S' OR isnull(mvtapos.mvpSTSF)) and day(mvtapos.MVPFEC) = " & FechaInicio.Value.ToString.Substring(0, 2) & " and month(mvtapos.MVPFEC) = " & FechaInicio.Value.ToString.Substring(3, 2) & " and year(mvtapos.MVPFEC) = " & FechaInicio.Value.ToString.Substring(6, 4) & "  order by mvtapos.mvpfec", MotorBoutique).Tables(0)

            ''''AQUI CONSULTO SOLO DEVOLUCIONES
            tventas2 = objpro.ObtenDatos("SELECT mvtapos1.mvpsuc AS SUC, mvtapos1.mvpfec AS FECVTA, mvtapos1.mvpter AS TER, mvtapos1.mvpfol AS NOTA, mvtapos.mvptmov AS MOV, mvtapos1.mcpren AS RENG, mvtapos1.mcpcodigo AS COD, mvtapos1.mcpdescri AS DES, " &
                                         "mvtapos1.mvpcant AS CANT, mvtapos1.mvpprec AS PREC, mvtapos1.mvpdes1 AS DES1, mvtapos1.mvpdes2 AS DES2, mvtapos1.mvpboni AS BON, mvtapos1.mvpdese AS DESE, mvtapos1.mvpbone AS BONE, mvtapos1.mvpimpt AS IMPT, " &
                                         "mvtapos1.mvpmon AS MON, mvtapos1.mvpcto AS PCTO, mvtapos1.mvpvenet AS VENET, mvtapos.mvpSTSF AS FAC " &
                                         "FROM mvtapos inner join mvtapos1 on mvtapos.mvpfec = mvtapos1.mvpfec and mvtapos.mvpfol = mvtapos1.mvpfol and mvtapos.mvpter = mvtapos1.mvpter " &
                                         "WHERE mvtapos.MVPTMOV ='DEV' and mvtapos.mvpsts = 'P' and mvtapos.mvpfill <> 'R' AND (mvtapos.mvpSTSF <> 'S' OR isnull(mvtapos.mvpSTSF)) and day(mvtapos.MVPFEC) =" & FechaInicio.Value.ToString.Substring(0, 2) & " and month(mvtapos.MVPFEC) = " & FechaInicio.Value.ToString.Substring(3, 2) & " and year(mvtapos.MVPFEC) = " & FechaInicio.Value.ToString.Substring(6, 4) & " order by mvtapos.mvpfec ", MotorBoutique).Tables(0)


            If tventas.Rows.Count <> 0 Then
                Upgbar.Value += 25
                For Each j In tventas.Rows
                    Application.DoEvents()

                    Dim nfol1 As Int16 = Int(j("NOTA").ToString)
                    pfec = j("FECVTA").ToString()
                    Dim cod As Int64 = Int(j("COD").ToString)
                    ter = Int(j("TER").ToString)
                    Dim pren As Int16 = Int(j("RENG").ToString)
                    Dim desc As String = j("DES").ToString
                    Dim cant As Int16 = Int(j("CANT").ToString)
                    Dim precio As Double = CDbl(j("PREC").ToString) ''PRECIO DE VENTA
                    Dim desc1 As Int16 = Int(j("DES1").ToString)
                    Dim desc2 As Int16 = Int(j("DES2").ToString)
                    Dim boni As Double = CDbl(j("BON").ToString)
                    Dim dese As Int16 = Int(j("DESE").ToString)
                    Dim bone As Double = CDbl(j("BONE").ToString)
                    Dim import As Double = CDbl(j("IMPT").ToString)
                    Dim mov As String = j("MOV").ToString()
                    Dim FAC As String = j("FAC").ToString()

                    If mov = "DEV" Then
                        cant = (cant) * (-1)

                    End If
                    'AQUI COMIENZO A CALCULAR IMPORTES PARA EL VENTADET
                    ''PRIMERO EL PRECIO UNITARIO
                    If FAC.Trim <> "S" Then
                        Dim pu1 As Double = Format(precio / 1.16, "###,###.00") ''PRECIO UNITARIO

                        Dim bruto As Double = Format((precio * cant), "###,###.00") ''base, tal cual el precio X piezas


                        descu = Format(((desc1 / 100) * bruto), "###,###.00") ''DES1 

                        prec1 = Format(bruto - descu, "###,###.00") ''IMPORTE1

                        ''If prec1 > 0 Or prec1 <> ".00" Then
                        If desc2 <> 0 Then
                            Dim descU2 As Double = Format(((desc2 / 100) * prec1), "###,###.00") 'DES2
                            prec1 = Format(prec1 - descU2, "###,###.00") ''IMPORTE2
                            descu = descu + descU2
                        End If

                        If boni <> 0 Then ''SI TIENE BONIFICACION
                            Dim bonif As Double = Format((boni * cant), "###,###.00") 'BONI 'Format(((boni * cant) / 1.16), "###,###.00") 'BONI
                            prec1 = Format(prec1 - bonif, "###,###.00") ''IMPORTE3
                            descu = descu + bonif
                        End If

                        If dese <> 0 Then
                            Dim desee As Double = Format(((dese / 100) * prec1), "###,###.00") 'DESE
                            prec1 = Format(prec1 - desee, "###,###.00") ''IMPORTE4
                            descu = descu + desee
                        End If

                        If mov = "DEV" Then
                            descu = descu * (-1)
                        End If


                        Dim SUBT As Double = Format(prec1 / 1.16, "###,###.00") ''subtotal
                        Dim iva1 As Double = Format(CDbl(SUBT * 0.16), "###,###.00") ''iva
                        Dim tot As Double = Format(CDbl(SUBT + iva1), "###,###.00")
                        ''TOTALES PARA VENTAENC
                        TOTAL += Format(CDbl((tot + iva1) - descu), "###,###.00")
                        If mov = "DEV" Then
                            descu = descu * (-1)
                            DESCU1 = DESCU1 - descu
                            descu = descu * (-1)
                        Else
                            DESCU1 += descu 'descFG
                        End If
                        DESCUE2 += 0 ''desc2
                        PDESC1 += dese
                        PDESC2 += boni
                        TIVA += iva1

                        If mov <> "DEV" Or descu < 0 Then

                            If objpro.DatoExiste("vddescrI", "ventadet", "VDFOLIO = " & grbTienda.Value & " and sid = " & Me.grbSucursal.Value & " and vdidfolv = " & CInt(nfol1) & " and vdreng = " & CInt(pren) & " AND day(vdFecha) = " & FechaInicio.Value.ToString.Substring(0, 2) & " and Month(vdFecha) = " & FechaInicio.Value.ToString.Substring(3, 2) & " and year(vdFecha) = " & FechaInicio.Value.ToString.Substring(6, 4) & " ", Motor) = False Then

                                'AQUI EMPIEZO A INSERTAR LO DE VENTADET
                                objpro.Indicacion("Insert into Ventadet (VDFOLIO, VDTERM, VDIDFOLV, VDRENG, VDFECHA, SID, VDIDPROD, VDCLAPRO, VDDESCRI, VDCANT,VDUNIDAD,VDPORIVA,VDIVA,VDPREUNI,VDDESC1,VDPDESC1, VDDESC2,VDPDESC2,VDPRETOT) " &
                                                  " values (" & Me.grbTienda.Value & "," & CInt(term) & "," & CInt(nfol1) & "," & CInt(pren) & ",'" & pfec & "' ," & Me.grbSucursal.Value & ",'" & cod & "', '" & cod & "','" & desc & "'," & CInt(cant) & "," &
                                                  " 'PZA', 16 ," & CDbl(iva1) & ", " & CDbl(pu1) & ", " & CDbl(descu) & "," & CDbl(0) & ", " & CDbl(dese) & ", 0, " & CDbl(tot) & ")", Motor)
                            End If

                        End If

                        ''End If
                    Else
                        Console.WriteLine("venia una con factura")
                    End If
                Next

                Upgbar.Value += 25
                SUBT = Format(CDbl(TOTAL - TIVA), "###,###.00")
                objpro.Indicacion("Insert into VentaENC (VEFOLIO, VETERM, VEIDFOLV, VEFECHA, SID, VEESTATU, VEFAC, VESUBT, VEDESC1, VEPDESC1, VEDESC2, VEPDESC2, VEPORIVA , VEIVA, VETOTAL, VEESTADO) " &
                " values (" & Me.grbTienda.Value & "," & CInt(term) & ", 1 , '" & pfec & "' ," & Me.grbSucursal.Value & ", 0, 0," & CDbl(SUBT) & "," &
                "  " & CDbl(DESCU1) & "," & CDbl(DESCUE2) & " ," & CDbl(PDESC1) & ", " & CDbl(PDESC2) & ", 16, " & CDbl(TIVA) & "," & CDbl(TOTAL) & ", 1 )", Motor)
                Upgbar.Value += 25
                Console.WriteLine("Si trae registros la consulta")



                If tventas2.Rows.Count > 0 Then
                    'Me.Importe.Value = 0
                    TOTAL = 0
                    TIVA = 0
                    SUBT = 0
                    descu = 0
                    prec1 = 0

                    For Each x In tventas2.Rows
                        Application.DoEvents()

                        Dim nfol1 As Int16 = Int(x("NOTA").ToString)
                        pfec = x("FECVTA").ToString()
                        Dim cod As Int64 = Int(x("COD").ToString)
                        ter = Int(x("TER").ToString)
                        Dim pren As Int16 = Int(x("RENG").ToString)
                        Dim desc As String = x("DES").ToString
                        Dim cant As Int16 = Int(x("CANT").ToString)
                        Dim precio As Double = CDbl(x("PREC").ToString) ''PRECIO DE VENTA
                        Dim desc1 As Int16 = Int(x("DES1").ToString)
                        Dim desc2 As Int16 = Int(x("DES2").ToString)
                        Dim boni As Double = CDbl(x("BON").ToString)
                        Dim dese As Int16 = Int(x("DESE").ToString)
                        'Dim bone As Double = CDbl(x("BONE").ToString)
                        Dim import As Double = CDbl(x("IMPT").ToString)

                        cant = (cant) * (-1)
                        ''AQUI COMIENZO A CALCULAR IMPORTES PARA EL VENTADET
                        ''PRIMERO EL PRECIO UNITARIO

                        'Dim bruto As Double = Format((precio * cant), "###,###.00") ''base, tal cual el precio X piezas


                        'descu = Format(((desc1 / 100) * bruto), "###,###.00") ''DES1 

                        'prec1 = Format(bruto - descu, "###,###.00") ''IMPORTE1

                        'If desc2 <> 0 Then
                        '    Dim descU2 As Double = Format(((desc2 / 100) * prec1), "###,###.00") 'DES2
                        '    prec1 = Format(prec1 - descU2, "###,###.00") ''IMPORTE2
                        '    descu = descu + descU2
                        'End If

                        'If boni <> 0 Then ''SI TIENE BONIFICACION
                        '    Dim bonif As Double = Format((boni * cant), "###,###.00") 'BONI 'Format(((boni * cant) / 1.16), "###,###.00") 'BONI
                        '    prec1 = Format(prec1 - bonif, "###,###.00") ''IMPORTE3
                        '    descu = descu + bonif
                        'End If

                        'If dese <> 0 Then
                        '    Dim desee As Double = Format(((dese / 100) * prec1), "###,###.00") 'DESE
                        '    prec1 = Format(prec1 - desee, "###,###.00") ''IMPORTE4
                        '    descu = descu + desee
                        'End If

                        'Dim pu1 As Double = Format(prec1 / 1.16, "###,###.00") ''PRECIO UNITARIO
                        'Dim iva1 As Double = Format(CDbl((prec1 - pu1) * cant), "###,###.00") ''iva

                        Dim pu1 As Double = Format((precio * cant) / 1.16, "###,###.00") ''PRECIO UNITARIO
                        Dim iva1 As Double = Format(CDbl((precio - pu1) * cant), "###,###.00") ''iva
                        ''TOTALES PARA VENTAENC
                        TOTAL += Format(CDbl(precio * cant), "###,###.00")
                        DESCU1 += descu
                        TIVA += iva1
                        SUBT += Format(CDbl(pu1), "###,###.00")
                    Next
                    objpro.Indicacion("Insert into VentaENC (VEFOLIO, VETERM, VEIDFOLV, VEFECHA, SID, VEESTATU, VEFAC, VESUBT, VEDESC1, VEPDESC1, VEDESC2, VEPDESC2, VEPORIVA , VEIVA, VETOTAL, VEESTADO) " &
                        " values (" & Me.grbTienda.Value & "," & CInt(term) & ", 2 , '" & pfec & "' ," & Me.grbSucursal.Value & ", 0, 0," & CDbl(SUBT) & "," &
                        "  " & CDbl(0) & "," & CDbl(0) & " ," & CDbl(0) & ", " & CDbl(0) & ", 16, " & CDbl(TIVA) & "," & CDbl(TOTAL) & ", 1 )", Motor)
                    ' Console.WriteLine("Si trae registros la consulta")
                    Upgbar.Value += 25
                    MsgBox("INFORMACIÓN CARGADA CON ÉXITO", MsgBoxStyle.Information, "Facturación .. ")
                Else
                    Upgbar.Value += 25
                    MsgBox("INFORMACIÓN CARGADA CON ÉXITO", MsgBoxStyle.Information, "Facturación .. ")
                End If
            Else
                MsgBox("No se encontró información.. ", MsgBoxStyle.Information, "Facturación.. ")
            End If
        End If
    End Sub

End Class