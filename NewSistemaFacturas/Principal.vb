Public Class Principal

#Region "Clases"
    Dim objpro As New Procedures
    Dim objfun As New Funciones
#End Region

    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Inicializa_Menu()
        USB.Panels(0).Text = "Bienvenido: Sistema de Facturación"
        USB.Panels(1).Text = "Ip: " & objfun.ObtenIp
        USB.Panels(2).Text = "Pc: " & NombrePC
        USB.Panels(3).Text = "Fecha: " & Now.ToString.Substring(0, 10)

        Select Case TipoSistema
            Case "BOUTIQUE"
                UTBM.Tools.Item(0).SharedProps.Enabled = True
                UTBM.Tools.Item(1).SharedProps.Enabled = True
                UTBM.Tools.Item(4).SharedProps.Enabled = True
                UTBM.Tools.Item(5).SharedProps.Enabled = True
                UTBM.Tools.Item(35).SharedProps.Enabled = True
                UTBM.Tools.Item(36).SharedProps.Enabled = True
                UTBM.Tools.Item(37).SharedProps.Enabled = True
                UTBM.Tools.Item(38).SharedProps.Enabled = True
                UTBM.Tools.Item(34).SharedProps.Enabled = True
                UTBM.Tools.Item(39).SharedProps.Enabled = True
                UTBM.Tools.Item(40).SharedProps.Enabled = True
                UTBM.Tools.Item(41).SharedProps.Enabled = True
                UTBM.Tools.Item(49).SharedProps.Enabled = True
                UTBM.Tools.Item(50).SharedProps.Enabled = True
            Case "BOSCO"
                UTBM.Tools.Item(2).SharedProps.Enabled = True
                UTBM.Tools.Item(3).SharedProps.Enabled = True
                UTBM.Tools.Item(14).SharedProps.Enabled = True
                UTBM.Tools.Item(15).SharedProps.Enabled = True
                UTBM.Tools.Item(20).SharedProps.Enabled = True

            Case "CAFETERIA"
                UTBM.Tools.Item(16).SharedProps.Enabled = True
                UTBM.Tools.Item(17).SharedProps.Enabled = True
                UTBM.Tools.Item(18).SharedProps.Enabled = True
                UTBM.Tools.Item(19).SharedProps.Enabled = True
                UTBM.Tools.Item(21).SharedProps.Enabled = True
                UTBM.Tools.Item(35).SharedProps.Enabled = True
                UTBM.Tools.Item(43).SharedProps.Enabled = True
                UTBM.Tools.Item(44).SharedProps.Enabled = True
                UTBM.Tools.Item(54).SharedProps.Enabled = True
                UTBM.Tools.Item(55).SharedProps.Enabled = True
                UTBM.Tools.Item(57).SharedProps.Enabled = True
            Case "NUTRISA"
                UTBM.Tools.Item(28).SharedProps.Enabled = True
                UTBM.Tools.Item(29).SharedProps.Enabled = True
                UTBM.Tools.Item(30).SharedProps.Enabled = True
                UTBM.Tools.Item(31).SharedProps.Enabled = True
                UTBM.Tools.Item(32).SharedProps.Enabled = True
                UTBM.Tools.Item(45).SharedProps.Enabled = True
                UTBM.Tools.Item(46).SharedProps.Enabled = True
                UTBM.Tools.Item(27).SharedProps.Enabled = True
            Case "OFICINAS"
                UTBM.Tools.Item(6).SharedProps.Enabled = True
                UTBM.Tools.Item(7).SharedProps.Enabled = True
                UTBM.Tools.Item(8).SharedProps.Enabled = True
                UTBM.Tools.Item(9).SharedProps.Enabled = True
                UTBM.Tools.Item(10).SharedProps.Enabled = True
                UTBM.Tools.Item(11).SharedProps.Enabled = True
                UTBM.Tools.Item(12).SharedProps.Enabled = True
                UTBM.Tools.Item(13).SharedProps.Enabled = True
                UTBM.Tools.Item(21).SharedProps.Enabled = True
                UTBM.Tools.Item(24).SharedProps.Enabled = True
                UTBM.Tools.Item(22).SharedProps.Enabled = True
                UTBM.Tools.Item(23).SharedProps.Enabled = True
                UTBM.Tools.Item(25).SharedProps.Enabled = True
                UTBM.Tools.Item(26).SharedProps.Enabled = True
                UTBM.Tools.Item(33).SharedProps.Enabled = True
                UTBM.Tools.Item(35).SharedProps.Enabled = True
                'UTBM.Tools.Item(34).SharedProps.Enabled = True
                UTBM.Tools.Item(39).SharedProps.Enabled = True
                UTBM.Tools.Item(51).SharedProps.Enabled = True 'Nota Crédito RELACIONADA
                UTBM.Tools.Item(52).SharedProps.Enabled = True
                UTBM.Tools.Item(53).SharedProps.Enabled = True
        End Select
    End Sub

    Private Sub Inicializa_Menu()
        '*************************  BOUTIQUE **********************'
        UTBM.Tools.Item(0).SharedProps.Enabled = False
        UTBM.Tools.Item(1).SharedProps.Enabled = False
        UTBM.Tools.Item(4).SharedProps.Enabled = False
        UTBM.Tools.Item(5).SharedProps.Enabled = False
        UTBM.Tools.Item(10).SharedProps.Enabled = False
        UTBM.Tools.Item(21).SharedProps.Enabled = False
        UTBM.Tools.Item(24).SharedProps.Enabled = False
        UTBM.Tools.Item(33).SharedProps.Enabled = False
        UTBM.Tools.Item(34).SharedProps.Enabled = False
        UTBM.Tools.Item(35).SharedProps.Enabled = False
        UTBM.Tools.Item(36).SharedProps.Enabled = False
        UTBM.Tools.Item(37).SharedProps.Enabled = False
        UTBM.Tools.Item(38).SharedProps.Enabled = False
        UTBM.Tools.Item(40).SharedProps.Enabled = False
        UTBM.Tools.Item(43).SharedProps.Enabled = False
        UTBM.Tools.Item(44).SharedProps.Enabled = False
        UTBM.Tools.Item(45).SharedProps.Enabled = False
        UTBM.Tools.Item(46).SharedProps.Enabled = False
        UTBM.Tools.Item(47).SharedProps.Enabled = False
        UTBM.Tools.Item(49).SharedProps.Enabled = False
        UTBM.Tools.Item(50).SharedProps.Enabled = False
        UTBM.Tools.Item(54).SharedProps.Enabled = False
        UTBM.Tools.Item(55).SharedProps.Enabled = False
        UTBM.Tools.Item(57).SharedProps.Enabled = False
        '*************************  BOUTIQUE **********************'

        '*************************  BOSCO *************************'
        UTBM.Tools.Item(2).SharedProps.Enabled = False
        UTBM.Tools.Item(3).SharedProps.Enabled = False
        UTBM.Tools.Item(14).SharedProps.Enabled = False
        UTBM.Tools.Item(15).SharedProps.Enabled = False
        UTBM.Tools.Item(20).SharedProps.Enabled = False
        UTBM.Tools.Item(47).SharedProps.Enabled = False
        UTBM.Tools.Item(49).SharedProps.Enabled = False
        UTBM.Tools.Item(54).SharedProps.Enabled = False
        UTBM.Tools.Item(55).SharedProps.Enabled = False
        UTBM.Tools.Item(57).SharedProps.Enabled = False
        '*************************  BOSCO *************************'

        '************************* CAFETERIA *************************'
        UTBM.Tools.Item(16).SharedProps.Enabled = False
        UTBM.Tools.Item(17).SharedProps.Enabled = False
        UTBM.Tools.Item(18).SharedProps.Enabled = False
        UTBM.Tools.Item(19).SharedProps.Enabled = False
        UTBM.Tools.Item(21).SharedProps.Enabled = False
        UTBM.Tools.Item(40).SharedProps.Enabled = False
        UTBM.Tools.Item(41).SharedProps.Enabled = False
        UTBM.Tools.Item(45).SharedProps.Enabled = False
        UTBM.Tools.Item(46).SharedProps.Enabled = False
        UTBM.Tools.Item(47).SharedProps.Enabled = False
        UTBM.Tools.Item(4).SharedProps.Enabled = False
        '************************* CAFETERIA *************************'

        '************************** NUTRISA **************************'
        UTBM.Tools.Item(28).SharedProps.Enabled = False
        UTBM.Tools.Item(29).SharedProps.Enabled = False
        UTBM.Tools.Item(30).SharedProps.Enabled = False
        UTBM.Tools.Item(31).SharedProps.Enabled = False
        UTBM.Tools.Item(32).SharedProps.Enabled = False
        UTBM.Tools.Item(33).SharedProps.Enabled = False
        UTBM.Tools.Item(27).SharedProps.Enabled = False
        UTBM.Tools.Item(40).SharedProps.Enabled = False
        UTBM.Tools.Item(41).SharedProps.Enabled = False
        UTBM.Tools.Item(47).SharedProps.Enabled = False
        UTBM.Tools.Item(49).SharedProps.Enabled = False
        UTBM.Tools.Item(54).SharedProps.Enabled = False
        UTBM.Tools.Item(55).SharedProps.Enabled = False
        UTBM.Tools.Item(57).SharedProps.Enabled = False
        '************************** NUTRISA **************************'

        UTBM.Tools.Item(6).SharedProps.Enabled = False
        UTBM.Tools.Item(7).SharedProps.Enabled = False
        UTBM.Tools.Item(8).SharedProps.Enabled = False
        UTBM.Tools.Item(9).SharedProps.Enabled = False
        UTBM.Tools.Item(10).SharedProps.Enabled = False
        UTBM.Tools.Item(11).SharedProps.Enabled = False
        UTBM.Tools.Item(12).SharedProps.Enabled = False
        UTBM.Tools.Item(13).SharedProps.Enabled = False
        UTBM.Tools.Item(22).SharedProps.Enabled = False
        UTBM.Tools.Item(23).SharedProps.Enabled = False
        UTBM.Tools.Item(25).SharedProps.Enabled = False
        UTBM.Tools.Item(26).SharedProps.Enabled = False
        UTBM.Tools.Item(31).SharedProps.Enabled = False
        UTBM.Tools.Item(33).SharedProps.Enabled = False
        UTBM.Tools.Item(34).SharedProps.Enabled = False
        UTBM.Tools.Item(43).SharedProps.Enabled = False
        UTBM.Tools.Item(44).SharedProps.Enabled = False
        UTBM.Tools.Item(45).SharedProps.Enabled = False
        UTBM.Tools.Item(46).SharedProps.Enabled = False
        UTBM.Tools.Item(40).SharedProps.Enabled = False
        UTBM.Tools.Item(41).SharedProps.Enabled = False
        UTBM.Tools.Item(47).SharedProps.Enabled = False
        UTBM.Tools.Item(49).SharedProps.Enabled = False
        UTBM.Tools.Item(50).SharedProps.Enabled = False
        UTBM.Tools.Item(51).SharedProps.Enabled = False
        UTBM.Tools.Item(52).SharedProps.Enabled = False
        UTBM.Tools.Item(54).SharedProps.Enabled = False
        UTBM.Tools.Item(55).SharedProps.Enabled = False
        UTBM.Tools.Item(57).SharedProps.Enabled = False
    End Sub

    Private Sub toolFacClientesBosco_Click(sender As Object, e As EventArgs)
        Formulario(frmBosFacClientes)
    End Sub

    Public Function Formulario(ByVal form As Form) As Boolean
        form.ShowDialog()
        Return False
    End Function

    Private Sub UTBM_ToolClick(sender As Object, e As Infragistics.Win.UltraWinToolbars.ToolClickEventArgs) Handles UTBM.ToolClick        
        Select Case e.Tool.ToolTipTextResolved
            Case "Factura Clientes Boutique"
                Formulario(frmFacClientes) 'Boutique
            Case "Clientes Boutique"
                Formulario(frmClientesBoutique) 'Boutique
            Case "Reenvío Factura por Correo Boutique"
                Formulario(frmBoutiqueCorreo) 'Boutique
            Case "Re-Imprimir Factura Boutique"
                Formulario(frmBoutiqueReimpresion) 'Boutique                
            Case "Nota de Credito Boutique"
                Formulario(frmNotaCreditoBoutique) 'Boutique
            Case "Nota de Credito a Detalle Boutique"
                Formulario(frmNotaCreditoDetalleBoutique) 'Boutique 
            Case "Carga / Cierre de Ventas del Día"
                Formulario(frmPreparaInfo) 'Boutique prepara info
            Case "Factura Global Diaria"
                Formulario(frmGlobalBoutv2) 'Boutique FACTURA GLOBAL
            Case "Cancelar Ticket Para Facturar"
                Formulario(frmCancelarTicket) 'Boutique --------- Cancela Ticket Para Factura
            Case "Borrar Ventas"
                Formulario(frmBorraVentas) 'Boutique --------- Cancela BORRA VENTAS
            Case "Factura Global Repeco" 'Boutique
                Formulario(frmRepeco)
            Case "Complemento Pago Boutique"
                Formulario(frmComplementoPagoBoutique)

            Case "Factura Clientes Bosco"
                Formulario(frmBosFacClientes) 'Bosco
            Case "Clientes Bosco"
                Formulario(frmClientesBosco) 'Bosco
            Case "Reenvío Factura por Correo Bosco"
                Formulario(frmBoscoCorreo) 'Bosco
            Case "Re-Imprimir Facturas Bosco"
                Formulario(frmBoscoReimpresion) 'Bosco
            Case "Nota de Credito Bosco"
                Formulario(frmNotaCreditoBosco) 'Bosco 
            Case "Nota Crédito RELACIONADA"
                Formulario(frmNotaCreditoOfi) 'Bosco 

            Case "Factura Clientes Cafeteria"
                Formulario(frmClienteCafeteria) 'Cafeteria
            Case "Clientes de Cafeteria"
                Formulario(frmClientesBoutique) 'Cafeteria
            Case "Reenvio de Factura por Correo de Cafeteria"
                Formulario(frmBoutiqueCorreo) 'Cafeteria
            Case "Re-Imprimir Factura Cafeteria"
                Formulario(frmBoutiqueReimpresion) 'Cafeteria
            Case "Nota de Credito Cafeterias"
                Formulario(frmNotaCreditoBoutique) 'Cafeteria
            Case "Cierre Diario Cafeteria"
                Formulario(frmPreparaInfo)
            Case "Factura Global Cafeteria"
                Formulario(frmGlobalBoutv2) 'Cafeteria
            Case "Facturas en Conceptos Churreria"
                Formulario(frmConceptosCafeteria) 'Cafeteria
            Case "Factura Conceptos Global Churreria"
                Formulario(frmConceptosGlobal) 'Cafeteria
            Case "Complemento de Pago Churreria"
                Formulario(frmComplementoCafeteria)

            Case "Factura Global Contado Bosco"
                Formulario(frmGlobalBosco) 'Global Bosco
            Case "Factura Global Boutique"
                Formulario(frmGlobalBoutiqueV2) 'Global Boutique
            Case "Factura de Honorarios"
                Formulario(frmHonorarios) 'Administradora Honorarios
            Case "Complemento de Pagos"
                'Formulario(frmComplementoPagos) 'Administradora Pagos
                Formulario(frmCompPagos) 'Administradora Pagos
            Case "Arrendamiento"
                Formulario(frmArrendamiento) 'Administradora Pagos
            Case "Factura en Conceptos Bosco"
                Formulario(frmConceptosBosco) 'Oficinas
            Case "Factura en Conceptos BOUTIQUE"
                Formulario(frmConceptosBoutique) 'Oficinas      
            Case "Importar Ventas Nutrisa"
                Formulario(frmImportarNutrisa) 'Oficinas 
            Case "Factura Global Nutrisa"
                Formulario(frmGlobalNutrisa) 'Oficinas 
            Case "Factura Global"
                Formulario(frmGlobal) 'Oficinas 
            Case "Factura Global Credito Bosco"
                Formulario(frmGlobalBoscoCredito) 'Oficinas GLOBAL CREDITO
            Case "Factura Clientes Nutrisa"
                Formulario(frmFacClienteNutrisa) 'Nutrisa
            Case "Catalogo de Clientes Nutrisa"
                Formulario(frmClientesBoutique) 'Nutrisa
            Case "Reenvio Factura por Correo Nutrisa"
                Formulario(frmBoutiqueCorreo) 'Nutrisa
            Case "Re-Imprimir Factura de Cliente Nutrisa"
                Formulario(frmBoutiqueReimpresion) 'Nutrisa  
            Case "Cancelacion de Factura Nutrisa"
                Formulario(frmNotaCreditoBoutique) 'Nutrisa  
            Case "Envio de Cierre Diarios de Nutrisa"
                Formulario(frmEnvioCierreNutrisa) 'Nutrisa  
            Case "Nota Credito Oficinas Bosco" '"Nota Credito Detalle Bosco"
                Formulario(frmNotaCreditoOficinasBosco) 'Oficinas
                'Case "Cierre Diario Nutrisa"
                '    Formulario(frmImportarNutrisa)
                'Case "Fac. Global Nutrisa"
                '    Formulario(frmGlobalBoutv2)
                'Formulario(frmNotaCreditoDetalleBosco) 'Oficinas
        End Select
    End Sub

End Class
