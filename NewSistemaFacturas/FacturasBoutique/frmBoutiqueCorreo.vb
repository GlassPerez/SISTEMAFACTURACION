Public Class frmBoutiqueCorreo

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

#Region "Variables"
    Dim AlgunaVez As Boolean = False
    Dim Algunavez2 As Boolean = False
    Dim TId, SId As Integer
    Dim TIP, FechaBusqueda As String
    Dim Mes As Integer = 0
    Dim Tabla, TSucursal, TablaTienda, TabExpeTicket As Data.DataTable
    Dim ExpCalle, ExpNoExterior, ExpNoInterior, ExpColonia, ExpEstado, ExpMunicipio, ExpLocalidad, ExpCp, ExpPais As String
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

    Private Sub grbSucursal_ValueChanged(sender As Object, e As EventArgs) Handles grbSucursal.ValueChanged
        REM objpro.CargaCombos(grbSucursal, "Select sId, sCsuc as Sucursal FROM BGXFacElec.CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, As400)
        objpro.CargaCombos(grbTipoFactura, "select setipo, sedesc from serie where tid = " & grbTienda.Value & " and sid = " & grbSucursal.Value & "", 0, 1, Motor)
        grbTipoFactura.Focus()
        Ugr.DataSource = Nothing
    End Sub

    Private Sub frmBoutiqueCorreo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        objpro.CargaCombos(grbTienda, "Select tId, trSocial FROM CatTien where Tid = " & Tienda & "", 0, 1, Motor)
        Me.grbTienda.Value = Tienda
        objpro.CargaCombos(grbSucursal, "Select sId,sCsuc FROM CatSucu Where tid = " & Me.grbTienda.Value & " ", 0, 1, Motor)
        Me.grbSucursal.Value = Sucursal

        Me.grbMes.Text = UCase(MonthName(Now.Date.Month))
        Me.grbPeriodo.Text = Now.Date.Year

        txtFechaFactura.Value = Now.Date
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        CargaGrid()
    End Sub

    Private Sub CargaGrid()
        Try           
            FechaBusqueda = Me.grbPeriodo.Text & OBJFUN.EscojeMes(Me.grbMes.Text)
            objpro.LLenaGrid(Ugr, "SELECT FEncabezado.FeFact as Factura,FEncabezado.fefecha as FechaTIM, FEncabezado.fereg as FechaVTA,FEncabezado.FeSerie as Serie,FEncabezado.FeEstatu as Estado,FEncabezado.Cid,FEncabezado.FeTerm as Terminal,Clientes.crfc as rfc_Clientes,Clientes.cRSocial as Clientes,Clientes.cemail as Email,FEncabezado.FeSubt as Subtotal, FEncabezado.fepdesc1 as Descuento ,FEncabezado.FeIva as Iva,FEncabezado.FeTotal as Total,FEncabezado.tid FROM FEncabez as FEncabezado left join CatClie as Clientes on FEncabezado.Cid = Clientes.Cid  Where mid(FEncabezado.FeReg,1,6) = " & FechaBusqueda & " and FEncabezado.Tid = " & Me.grbTienda.Value & " and FEncabezado.Sid = " & grbSucursal.Value & " order by FEncabezado.FeSerie,FEncabezado.FeFact", "FEncabez", Motor)
            Me.Ugr.DataBind()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try

    End Sub

    Private Sub Ugr_InitializeLayout(sender As Object, e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles Ugr.InitializeLayout
        With Ugr
            .DisplayLayout.Bands(0).Columns(0).Width = 50
            .DisplayLayout.Bands(0).Columns(1).Width = 100
            .DisplayLayout.Bands(0).Columns(2).Width = 100
            .DisplayLayout.Bands(0).Columns(3).Width = 55
            .DisplayLayout.Bands(0).Columns(4).Width = 65
            .DisplayLayout.Bands(0).Columns(5).Width = 70
            .DisplayLayout.Bands(0).Columns(6).Hidden = True
            .DisplayLayout.Bands(0).Columns(7).Width = 100
            .DisplayLayout.Bands(0).Columns(8).Width = 450
            .DisplayLayout.Bands(0).Columns(9).Width = 250
            .DisplayLayout.Bands(0).Columns(10).Width = 100
            .DisplayLayout.Bands(0).Columns(11).Width = 100
            .DisplayLayout.Bands(0).Columns(12).Width = 100
            .DisplayLayout.Bands(0).Columns(13).Width = 100
            .DisplayLayout.Bands(0).Columns(14).Hidden = True
        End With
    End Sub

    Private Sub Ugr_DoubleClick(sender As Object, e As EventArgs) Handles Ugr.DoubleClick
        DatosEnvio("")
        Try
            txtFactura.Text = Format(Ugr.ActiveRow.Cells("Factura").Value, "00000")
            txtSerie.Text = Ugr.ActiveRow.Cells("Serie").Value.ToString
            txtFechaFactura.Value = Ugr.ActiveRow.Cells("FechaTIM").Value.ToString
            txtfecvta.Text = Ugr.ActiveRow.Cells("FechaVTA").Value.ToString
            txtEmail.Text = Ugr.ActiveRow.Cells("Email").Value.ToString

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
        End Try
    End Sub

    Private Sub DatosEnvio(ByVal comodin As String)
        txtFactura.Text = comodin
        txtSerie.Text = comodin
        txtEmail.Text = comodin
        txtfecvta.Text = comodin
    End Sub

    Private Sub btnReenviarFactura_Click(sender As Object, e As EventArgs) Handles btnReenviarFactura.Click
        upb.Value = 0
        upb.Maximum = 100
        If Me.txtFactura.Text <> "" Then
            If Me.txtEmail.Text <> "" Then
                TablaTienda = objpro.ObtenDatos("SELECT * FROM CatTien as Tienda left join Catsucu as Sucursal on Tienda.tid = Sucursal.tid Where Tienda.tid = " & grbTienda.Value & " and Sucursal.sid = " & grbSucursal.Value & " ", Motor).Tables(0)
                Dim TRfc = Me.TablaTienda.Rows(0).Item("trfc").ToString
                Dim Directorios As String = Me.TablaTienda.Rows(0).Item("sUbic").ToString
                upb.Value += 10
                DirEmail = TablaTienda.Rows(0).Item("EMAILDIR").ToString
                PwdEmail = TablaTienda.Rows(0).Item("EMAILPWD").ToString
                Pop = TablaTienda.Rows(0).Item("EMAILPOP").ToString
                PuertoPop = TablaTienda.Rows(0).Item("EMAILPPOP").ToString
                SmtpHost = TablaTienda.Rows(0).Item("EMAILSMTP").ToString
                PuertoSmtp = TablaTienda.Rows(0).Item("EMAILPSMTP").ToString
                DescEmail = TablaTienda.Rows(0).Item("EMAILDESC").ToString
                'AsuntoEmail = TablaTienda.Rows(0).Item("EMAILASU").ToString
                MsjEmail = TablaTienda.Rows(0).Item("EMAILMSG").ToString
                Dim mfac As String = txtFechaFactura.Value.ToString.Substring(3, 2) ''mes de factura para saber en que carpeta se guardo
                upb.Value += 30
                If TablaTienda.Rows(0).Item("EMAILP").ToString = Nothing Then
                    SeguridadEmail = False
                ElseIf TablaTienda.Rows(0).Item("EMAILP").ToString = 0 Then
                    SeguridadEmail = False
                Else
                    SeguridadEmail = True
                End If
                Dim rutaArchivoPDF As String = ""
                Dim rutaArchivoXML As String = ""

                If grbTipoFactura.Value = "VG" Then
                    rutaArchivoPDF = Directorios & "\" & Me.grbPeriodo.Text & "\" & mfac & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & "-" & txtfecvta.Text & ".pdf"
                    rutaArchivoXML = Directorios & "\" & Me.grbPeriodo.Text & "\" & mfac & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & "-" & txtfecvta.Text & ".xml"
                    AsuntoEmail = "FACTURA CORRESPONDIENTE A: " & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & "-" & txtfecvta.Text
                Else
                    rutaArchivoPDF = Directorios & "\" & Me.grbPeriodo.Text & "\" & objfun.EscojeMes(Me.grbMes.Text) & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & ".pdf"
                    rutaArchivoXML = Directorios & "\" & Me.grbPeriodo.Text & "\" & objfun.EscojeMes(Me.grbMes.Text) & "\" & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text & ".xml"
                    AsuntoEmail = "FACTURA CORRESPONDIENTE A: " & Trim(TRfc) & "-" & Me.txtSerie.Text & "-" & txtFactura.Text
                End If

                upb.Value += 10

                    Dim correo As New System.Net.Mail.MailMessage()
                    correo.From = New System.Net.Mail.MailAddress(DirEmail)
                    correo.To.Add(Trim(Me.txtEmail.Text))
                    correo.Subject = Trim(AsuntoEmail)
                    upb.Value += 10

                    correo.Body = Trim(MsjEmail)
                    correo.Body += Environment.NewLine & Environment.NewLine
                    correo.Body += Environment.NewLine & "AVISO DE PRIVACIDAD MAIL."
                    correo.Body += Environment.NewLine & "GRUPO VIGIL. con domicilio en calle 37 Oriente, número 602, Colonia Anzures, Puebla Puebla, C.P. 72530, es la responsable" & vbCrLf
                    correo.Body += " y/o datos personales sensibles, los cuales en su caso serán recabados y/o utilizados con la finalidad de enviarle información" & vbCrLf
                    correo.Body += " vía correo electrónico respecto a la actualización de nuestros productos y servicios, así como para la realización de promociones" & vbCrLf
                    correo.Body += " y preventas (publicidad), actualización de base de datos de la empresa, almacenaje de imágenes para su seguridad y la de las personas que nos" & vbCrLf
                    correo.Body += " visitan en nustros dispositivos de seguridad y para el cobro por los servicios prestados por la empresa." & vbCrLf
                    correo.Body += Environment.NewLine & "Podrá conocer del aviso de privacidad integral podrá acudir al domicilio señalado en renglones anteriores" & vbCrLf
                    correo.Body += " ó a través de una solicitud vía correo electrónico a boutique@grupovigil.com.mx"
                    upb.Value += 10

                    correo.IsBodyHtml = False
                    correo.Priority = System.Net.Mail.MailPriority.Normal
                    Dim Archivo As New System.Net.Mail.Attachment(rutaArchivoPDF)
                    Dim i As Integer
                    For i = 0 To 1
                        If i = 0 Then
                            Archivo = New System.Net.Mail.Attachment(rutaArchivoPDF)
                            upb.Value += 5
                        Else
                            Archivo = New System.Net.Mail.Attachment(rutaArchivoXML)
                            upb.Value += 5
                        End If
                        correo.Attachments.Add(Archivo)
                    Next
                    Dim smtp As New System.Net.Mail.SmtpClient
                    smtp.Host = SmtpHost
                    smtp.Port = Int(PuertoSmtp)
                    smtp.Credentials = New System.Net.NetworkCredential(Trim(DirEmail), PwdEmail)
                    smtp.EnableSsl = SeguridadEmail
                    upb.Value += 10
                    Try
                        smtp.Send(correo)
                        upb.Value += 10
                        MsgBox("La factura se envío correctamente .. ", MsgBoxStyle.Information, "Facturación .. ")
                        DatosEnvio("")
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Information, "Facturación .. ")
                    End Try
                Else
                    MsgBox("Falta dirección de correo electrónico", MsgBoxStyle.Information, "Facturación .. ")
            End If
        Else
            MsgBox("Falta seleccionar factura para envíar", MsgBoxStyle.Information, "Facturación .. ")
        End If
    End Sub

    Private Sub frmBoutiqueCorreo_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    Private Sub txtEmail_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtEmail.KeyPress
        If e.KeyChar <> Chr(13) Then
            e.KeyChar = LCase(e.KeyChar)
        Else
            btnReenviarFactura.Focus()
        End If
    End Sub
End Class