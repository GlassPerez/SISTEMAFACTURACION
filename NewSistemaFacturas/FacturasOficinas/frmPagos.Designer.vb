<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPagos
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraToolTipInfo1 As Infragistics.Win.UltraWinToolTip.UltraToolTipInfo = New Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Se puede registrar el número de cuenta en donde se recibió el pago. --  CtaBenefi" &
        "ciario = 123456789101114558 ", Infragistics.Win.ToolTipImage.[Default], Nothing, Infragistics.Win.DefaultableBoolean.[Default])
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraToolTipInfo2 As Infragistics.Win.UltraWinToolTip.UltraToolTipInfo = New Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Se puede registrar el número de la cuenta con la que se realizó el pago -- CtaOrd" &
        "enante= 123456789101112131 ", Infragistics.Win.ToolTipImage.[Default], Nothing, Infragistics.Win.DefaultableBoolean.[Default])
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraToolTipInfo3 As Infragistics.Win.UltraWinToolTip.UltraToolTipInfo = New Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Si no tiene banco dejar el valor en 0", Infragistics.Win.ToolTipImage.[Default], Nothing, Infragistics.Win.DefaultableBoolean.[Default])
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraToolTipInfo4 As Infragistics.Win.UltraWinToolTip.UltraToolTipInfo = New Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Se puede registrar el número de cheque, número de autorización, número de referen" &
        "cia, clave de rastreo en caso de ser SPEI, ", Infragistics.Win.ToolTipImage.[Default], Nothing, Infragistics.Win.DefaultableBoolean.[Default])
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance40 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance41 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance43 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance82 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPagos))
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance55 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.upb = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.txtUUID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFechaFactura = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.txtSerie = New System.Windows.Forms.TextBox()
        Me.txtFactura = New System.Windows.Forms.TextBox()
        Me.lblFechaEmision = New System.Windows.Forms.Label()
        Me.lblSerieFactura = New System.Windows.Forms.Label()
        Me.lblNumFactura = New System.Windows.Forms.Label()
        Me.Saldo = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Abono = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Total = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Importe = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.btnAplicarPago = New System.Windows.Forms.Button()
        Me.gpoDatos = New Infragistics.Win.Misc.UltraGroupBox()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.fechaPago = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.txtCtaBeneficiario = New System.Windows.Forms.TextBox()
        Me.UltraLabel5 = New Infragistics.Win.Misc.UltraLabel()
        Me.txtCtaOrdenante = New System.Windows.Forms.TextBox()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.grbBancos = New System.Windows.Forms.ComboBox()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.txtNumOperacion = New System.Windows.Forms.TextBox()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.lblMetodoPago = New System.Windows.Forms.Label()
        Me.grbformaPago = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.txtNumCuenta = New System.Windows.Forms.TextBox()
        Me.lblNumCuenta = New System.Windows.Forms.Label()
        Me.lblEmpresa = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.umsg = New Infragistics.Win.UltraWinToolTip.UltraToolTipManager(Me.components)
        Me.btnClientes = New System.Windows.Forms.Button()
        Me.txtRazonSocial = New System.Windows.Forms.TextBox()
        Me.lblrfc = New Infragistics.Win.Misc.UltraLabel()
        Me.lblrasoncial = New Infragistics.Win.Misc.UltraLabel()
        Me.txtRfc = New System.Windows.Forms.TextBox()
        Me.lblEmail = New Infragistics.Win.Misc.UltraLabel()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gpoDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gpoDatos.SuspendLayout()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'upb
        '
        Me.upb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.upb.Location = New System.Drawing.Point(0, 532)
        Me.upb.Name = "upb"
        Me.upb.Size = New System.Drawing.Size(452, 33)
        Me.upb.TabIndex = 118
        Me.upb.Text = "[Formatted]"
        '
        'txtUUID
        '
        Me.txtUUID.Location = New System.Drawing.Point(169, 12)
        Me.txtUUID.Name = "txtUUID"
        Me.txtUUID.ReadOnly = True
        Me.txtUUID.Size = New System.Drawing.Size(279, 20)
        Me.txtUUID.TabIndex = 128
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(70, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 127
        Me.Label1.Text = "Folio Fiscal (UUID)"
        '
        'txtFechaFactura
        '
        Me.txtFechaFactura.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.txtFechaFactura.Location = New System.Drawing.Point(169, 84)
        Me.txtFechaFactura.Name = "txtFechaFactura"
        Me.txtFechaFactura.ReadOnly = True
        Me.txtFechaFactura.Size = New System.Drawing.Size(113, 21)
        Me.txtFechaFactura.TabIndex = 124
        Me.txtFechaFactura.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'txtSerie
        '
        Me.txtSerie.Location = New System.Drawing.Point(169, 60)
        Me.txtSerie.Name = "txtSerie"
        Me.txtSerie.ReadOnly = True
        Me.txtSerie.Size = New System.Drawing.Size(163, 20)
        Me.txtSerie.TabIndex = 123
        '
        'txtFactura
        '
        Me.txtFactura.Location = New System.Drawing.Point(169, 36)
        Me.txtFactura.Name = "txtFactura"
        Me.txtFactura.ReadOnly = True
        Me.txtFactura.Size = New System.Drawing.Size(163, 20)
        Me.txtFactura.TabIndex = 122
        '
        'lblFechaEmision
        '
        Me.lblFechaEmision.AutoSize = True
        Me.lblFechaEmision.Location = New System.Drawing.Point(11, 88)
        Me.lblFechaEmision.Name = "lblFechaEmision"
        Me.lblFechaEmision.Size = New System.Drawing.Size(155, 13)
        Me.lblFechaEmision.TabIndex = 121
        Me.lblFechaEmision.Text = "Fecha que se realizó la Factura"
        '
        'lblSerieFactura
        '
        Me.lblSerieFactura.AutoSize = True
        Me.lblSerieFactura.Location = New System.Drawing.Point(68, 64)
        Me.lblSerieFactura.Name = "lblSerieFactura"
        Me.lblSerieFactura.Size = New System.Drawing.Size(96, 13)
        Me.lblSerieFactura.TabIndex = 120
        Me.lblSerieFactura.Text = "Serie de la Factura"
        '
        'lblNumFactura
        '
        Me.lblNumFactura.AutoSize = True
        Me.lblNumFactura.Location = New System.Drawing.Point(27, 39)
        Me.lblNumFactura.Name = "lblNumFactura"
        Me.lblNumFactura.Size = New System.Drawing.Size(136, 13)
        Me.lblNumFactura.TabIndex = 119
        Me.lblNumFactura.Text = "No. de Factura erlacionada"
        '
        'Saldo
        '
        Me.Saldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Saldo.Location = New System.Drawing.Point(169, 181)
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Size = New System.Drawing.Size(113, 24)
        Me.Saldo.TabIndex = 146
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(117, 184)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 145
        Me.Label2.Text = "Saldo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(154, 165)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(147, 10)
        Me.UltraLabel6.TabIndex = 144
        Me.UltraLabel6.Text = "------------------------------------"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(117, 150)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 142
        Me.Label8.Text = "Abono"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(126, 120)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 143
        Me.Label11.Text = "Total"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Abono
        '
        Me.Abono.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Abono.Location = New System.Drawing.Point(169, 143)
        Me.Abono.Name = "Abono"
        Me.Abono.ReadOnly = True
        Me.Abono.Size = New System.Drawing.Size(113, 24)
        Me.Abono.TabIndex = 140
        '
        'Total
        '
        Me.Total.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Total.Location = New System.Drawing.Point(168, 113)
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Size = New System.Drawing.Size(114, 24)
        Me.Total.TabIndex = 141
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 491)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 13)
        Me.Label3.TabIndex = 147
        Me.Label3.Text = "Importe que desea abonar ..?"
        '
        'Importe
        '
        Me.Importe.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Importe.Location = New System.Drawing.Point(184, 485)
        Me.Importe.Name = "Importe"
        Me.Importe.Size = New System.Drawing.Size(113, 24)
        Me.Importe.TabIndex = 148
        '
        'btnAplicarPago
        '
        Me.btnAplicarPago.BackColor = System.Drawing.Color.Black
        Me.btnAplicarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAplicarPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAplicarPago.ForeColor = System.Drawing.Color.White
        Me.btnAplicarPago.Location = New System.Drawing.Point(303, 474)
        Me.btnAplicarPago.Name = "btnAplicarPago"
        Me.btnAplicarPago.Size = New System.Drawing.Size(145, 47)
        Me.btnAplicarPago.TabIndex = 149
        Me.btnAplicarPago.Text = "Aplicar Pago Relacionado"
        Me.btnAplicarPago.UseVisualStyleBackColor = False
        '
        'gpoDatos
        '
        Appearance2.BackColor = System.Drawing.Color.SteelBlue
        Appearance2.BackColor2 = System.Drawing.Color.White
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Me.gpoDatos.Appearance = Appearance2
        Me.gpoDatos.Controls.Add(Me.UltraLabel7)
        Me.gpoDatos.Controls.Add(Me.fechaPago)
        Me.gpoDatos.Controls.Add(Me.txtCtaBeneficiario)
        Me.gpoDatos.Controls.Add(Me.UltraLabel5)
        Me.gpoDatos.Controls.Add(Me.txtCtaOrdenante)
        Me.gpoDatos.Controls.Add(Me.UltraLabel4)
        Me.gpoDatos.Controls.Add(Me.UltraLabel3)
        Me.gpoDatos.Controls.Add(Me.grbBancos)
        Me.gpoDatos.Controls.Add(Me.UltraLabel1)
        Me.gpoDatos.Controls.Add(Me.txtNumOperacion)
        Me.gpoDatos.Controls.Add(Me.UltraLabel2)
        Me.gpoDatos.Location = New System.Drawing.Point(0, 365)
        Me.gpoDatos.Name = "gpoDatos"
        Me.gpoDatos.Size = New System.Drawing.Size(453, 108)
        Me.gpoDatos.TabIndex = 152
        Me.gpoDatos.Visible = False
        '
        'UltraLabel7
        '
        Appearance14.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel7.Appearance = Appearance14
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel7.Location = New System.Drawing.Point(223, 60)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UltraLabel7.Size = New System.Drawing.Size(87, 14)
        Me.UltraLabel7.TabIndex = 126
        Me.UltraLabel7.Text = "Fecha de Pago:"
        '
        'fechaPago
        '
        Me.fechaPago.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.fechaPago.Location = New System.Drawing.Point(219, 76)
        Me.fechaPago.Name = "fechaPago"
        Me.fechaPago.Size = New System.Drawing.Size(113, 21)
        Me.fechaPago.TabIndex = 125
        Me.fechaPago.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'txtCtaBeneficiario
        '
        Me.txtCtaBeneficiario.Location = New System.Drawing.Point(120, 34)
        Me.txtCtaBeneficiario.Name = "txtCtaBeneficiario"
        Me.txtCtaBeneficiario.Size = New System.Drawing.Size(171, 20)
        Me.txtCtaBeneficiario.TabIndex = 72
        UltraToolTipInfo1.ToolTipText = "Se puede registrar el número de cuenta en donde se recibió el pago. --  CtaBenefi" &
    "ciario = 123456789101114558 "
        Me.umsg.SetUltraToolTip(Me.txtCtaBeneficiario, UltraToolTipInfo1)
        '
        'UltraLabel5
        '
        Appearance3.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel5.Appearance = Appearance3
        Me.UltraLabel5.AutoSize = True
        Me.UltraLabel5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel5.Location = New System.Drawing.Point(120, 18)
        Me.UltraLabel5.Name = "UltraLabel5"
        Me.UltraLabel5.Size = New System.Drawing.Size(112, 14)
        Me.UltraLabel5.TabIndex = 71
        Me.UltraLabel5.Text = "No. Cta. Beneficiario"
        '
        'txtCtaOrdenante
        '
        Me.txtCtaOrdenante.Location = New System.Drawing.Point(64, 76)
        Me.txtCtaOrdenante.Name = "txtCtaOrdenante"
        Me.txtCtaOrdenante.Size = New System.Drawing.Size(100, 20)
        Me.txtCtaOrdenante.TabIndex = 70
        UltraToolTipInfo2.ToolTipText = "Se puede registrar el número de la cuenta con la que se realizó el pago -- CtaOrd" &
    "enante= 123456789101112131 "
        Me.umsg.SetUltraToolTip(Me.txtCtaOrdenante, UltraToolTipInfo2)
        '
        'UltraLabel4
        '
        Appearance15.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel4.Appearance = Appearance15
        Me.UltraLabel4.AutoSize = True
        Me.UltraLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel4.Location = New System.Drawing.Point(64, 57)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(106, 14)
        Me.UltraLabel4.TabIndex = 69
        Me.UltraLabel4.Text = "No. Cta. Ordenante"
        '
        'UltraLabel3
        '
        Appearance1.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel3.Appearance = Appearance1
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(249, 44)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(0, 0)
        Me.UltraLabel3.TabIndex = 68
        '
        'grbBancos
        '
        Me.grbBancos.FormattingEnabled = True
        Me.grbBancos.Items.AddRange(New Object() {"Santander", "BBVA Bancomer", "Banorte", "Banamex", "HSBC", "Scotiabank", "Inbursa", "Interacciones", "Banco del Bajío", "Afirme"})
        Me.grbBancos.Location = New System.Drawing.Point(299, 33)
        Me.grbBancos.Name = "grbBancos"
        Me.grbBancos.Size = New System.Drawing.Size(144, 21)
        Me.grbBancos.TabIndex = 67
        UltraToolTipInfo3.ToolTipText = "Si no tiene banco dejar el valor en 0"
        Me.umsg.SetUltraToolTip(Me.grbBancos, UltraToolTipInfo3)
        '
        'UltraLabel1
        '
        Appearance4.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel1.Appearance = Appearance4
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel1.Location = New System.Drawing.Point(299, 18)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UltraLabel1.Size = New System.Drawing.Size(96, 14)
        Me.UltraLabel1.TabIndex = 66
        Me.UltraLabel1.Text = "Banco Ordenante"
        '
        'txtNumOperacion
        '
        Me.txtNumOperacion.Location = New System.Drawing.Point(6, 34)
        Me.txtNumOperacion.Name = "txtNumOperacion"
        Me.txtNumOperacion.Size = New System.Drawing.Size(100, 20)
        Me.txtNumOperacion.TabIndex = 65
        UltraToolTipInfo4.ToolTipText = "Se puede registrar el número de cheque, número de autorización, número de referen" &
    "cia, clave de rastreo en caso de ser SPEI, "
        Me.umsg.SetUltraToolTip(Me.txtNumOperacion, UltraToolTipInfo4)
        '
        'UltraLabel2
        '
        Appearance12.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel2.Appearance = Appearance12
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel2.Location = New System.Drawing.Point(6, 16)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(96, 14)
        Me.UltraLabel2.TabIndex = 64
        Me.UltraLabel2.Text = "No. de Operación"
        '
        'lblMetodoPago
        '
        Me.lblMetodoPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetodoPago.ForeColor = System.Drawing.Color.Black
        Me.lblMetodoPago.Location = New System.Drawing.Point(19, 210)
        Me.lblMetodoPago.Name = "lblMetodoPago"
        Me.lblMetodoPago.Size = New System.Drawing.Size(71, 36)
        Me.lblMetodoPago.TabIndex = 153
        Me.lblMetodoPago.Text = "Forma de Pago:"
        '
        'grbformaPago
        '
        Me.grbformaPago.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Appearance40.BackColor = System.Drawing.SystemColors.Window
        Appearance40.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grbformaPago.DisplayLayout.Appearance = Appearance40
        Me.grbformaPago.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Me.grbformaPago.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grbformaPago.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance41.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance41.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance41.BorderColor = System.Drawing.SystemColors.Window
        Me.grbformaPago.DisplayLayout.GroupByBox.Appearance = Appearance41
        Appearance42.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbformaPago.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance42
        Me.grbformaPago.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance43.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance43.BackColor2 = System.Drawing.SystemColors.Control
        Appearance43.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance43.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbformaPago.DisplayLayout.GroupByBox.PromptAppearance = Appearance43
        Me.grbformaPago.DisplayLayout.MaxColScrollRegions = 1
        Me.grbformaPago.DisplayLayout.MaxRowScrollRegions = 1
        Appearance44.BackColor = System.Drawing.SystemColors.Window
        Appearance44.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grbformaPago.DisplayLayout.Override.ActiveCellAppearance = Appearance44
        Appearance45.BackColor = System.Drawing.SystemColors.Highlight
        Appearance45.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grbformaPago.DisplayLayout.Override.ActiveRowAppearance = Appearance45
        Me.grbformaPago.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grbformaPago.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance46.BackColor = System.Drawing.SystemColors.Window
        Me.grbformaPago.DisplayLayout.Override.CardAreaAppearance = Appearance46
        Appearance47.BorderColor = System.Drawing.Color.Silver
        Appearance47.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grbformaPago.DisplayLayout.Override.CellAppearance = Appearance47
        Me.grbformaPago.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grbformaPago.DisplayLayout.Override.CellPadding = 0
        Appearance48.BackColor = System.Drawing.SystemColors.Control
        Appearance48.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance48.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance48.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance48.BorderColor = System.Drawing.SystemColors.Window
        Me.grbformaPago.DisplayLayout.Override.GroupByRowAppearance = Appearance48
        Appearance49.TextHAlignAsString = "Left"
        Me.grbformaPago.DisplayLayout.Override.HeaderAppearance = Appearance49
        Me.grbformaPago.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grbformaPago.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance50.BackColor = System.Drawing.SystemColors.Window
        Appearance50.BorderColor = System.Drawing.Color.Silver
        Me.grbformaPago.DisplayLayout.Override.RowAppearance = Appearance50
        Me.grbformaPago.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance82.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grbformaPago.DisplayLayout.Override.TemplateAddRowAppearance = Appearance82
        Me.grbformaPago.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grbformaPago.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grbformaPago.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.grbformaPago.Location = New System.Drawing.Point(78, 216)
        Me.grbformaPago.Name = "grbformaPago"
        Me.grbformaPago.Size = New System.Drawing.Size(195, 22)
        Me.grbformaPago.TabIndex = 154
        '
        'txtNumCuenta
        '
        Me.txtNumCuenta.Location = New System.Drawing.Point(382, 216)
        Me.txtNumCuenta.MaxLength = 4
        Me.txtNumCuenta.Name = "txtNumCuenta"
        Me.txtNumCuenta.Size = New System.Drawing.Size(53, 20)
        Me.txtNumCuenta.TabIndex = 156
        Me.txtNumCuenta.Visible = False
        '
        'lblNumCuenta
        '
        Me.lblNumCuenta.AutoSize = True
        Me.lblNumCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumCuenta.ForeColor = System.Drawing.Color.Black
        Me.lblNumCuenta.Location = New System.Drawing.Point(296, 219)
        Me.lblNumCuenta.Name = "lblNumCuenta"
        Me.lblNumCuenta.Size = New System.Drawing.Size(80, 13)
        Me.lblNumCuenta.TabIndex = 155
        Me.lblNumCuenta.Text = "Núm. Cuenta"
        Me.lblNumCuenta.Visible = False
        '
        'lblEmpresa
        '
        Me.lblEmpresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmpresa.ForeColor = System.Drawing.Color.Black
        Me.lblEmpresa.Location = New System.Drawing.Point(11, 248)
        Me.lblEmpresa.Name = "lblEmpresa"
        Me.lblEmpresa.Size = New System.Drawing.Size(123, 36)
        Me.lblEmpresa.TabIndex = 157
        Me.lblEmpresa.Text = "Empresa o cliente a Aplicar:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(296, 165)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(14, 13)
        Me.Label4.TabIndex = 162
        Me.Label4.Text = "+"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'umsg
        '
        Appearance13.BackColor = System.Drawing.Color.Red
        Appearance13.BackColor2 = System.Drawing.Color.Red
        Me.umsg.Appearance = Appearance13
        Me.umsg.ContainingControl = Me
        Me.umsg.InitialDelay = 100
        '
        'btnClientes
        '
        Me.btnClientes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnClientes.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClientes.Image = CType(resources.GetObject("btnClientes.Image"), System.Drawing.Image)
        Me.btnClientes.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClientes.Location = New System.Drawing.Point(139, 248)
        Me.btnClientes.Name = "btnClientes"
        Me.btnClientes.Size = New System.Drawing.Size(296, 40)
        Me.btnClientes.TabIndex = 163
        Me.btnClientes.Text = "Seleccionar Cliente o Empresa"
        Me.btnClientes.UseVisualStyleBackColor = True
        '
        'txtRazonSocial
        '
        Me.txtRazonSocial.Location = New System.Drawing.Point(84, 323)
        Me.txtRazonSocial.Multiline = True
        Me.txtRazonSocial.Name = "txtRazonSocial"
        Me.txtRazonSocial.Size = New System.Drawing.Size(364, 36)
        Me.txtRazonSocial.TabIndex = 165
        '
        'lblrfc
        '
        Appearance54.BackColor = System.Drawing.Color.Transparent
        Me.lblrfc.Appearance = Appearance54
        Me.lblrfc.AutoSize = True
        Me.lblrfc.Location = New System.Drawing.Point(52, 300)
        Me.lblrfc.Name = "lblrfc"
        Me.lblrfc.Size = New System.Drawing.Size(31, 14)
        Me.lblrfc.TabIndex = 166
        Me.lblrfc.Text = "RFC:"
        '
        'lblrasoncial
        '
        Appearance52.BackColor = System.Drawing.Color.Transparent
        Me.lblrasoncial.Appearance = Appearance52
        Me.lblrasoncial.AutoSize = True
        Me.lblrasoncial.Location = New System.Drawing.Point(6, 323)
        Me.lblrasoncial.Name = "lblrasoncial"
        Me.lblrasoncial.Size = New System.Drawing.Size(74, 14)
        Me.lblrasoncial.TabIndex = 167
        Me.lblrasoncial.Text = "Razón Social:"
        '
        'txtRfc
        '
        Me.txtRfc.Location = New System.Drawing.Point(84, 297)
        Me.txtRfc.MaxLength = 13
        Me.txtRfc.Name = "txtRfc"
        Me.txtRfc.Size = New System.Drawing.Size(129, 20)
        Me.txtRfc.TabIndex = 164
        Me.txtRfc.TabStop = False
        '
        'lblEmail
        '
        Appearance55.BackColor = System.Drawing.Color.Transparent
        Me.lblEmail.Appearance = Appearance55
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(223, 300)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(36, 14)
        Me.lblEmail.TabIndex = 169
        Me.lblEmail.Text = "Email:"
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(260, 294)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(188, 20)
        Me.txtEmail.TabIndex = 168
        '
        'frmPagos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(452, 565)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtRazonSocial)
        Me.Controls.Add(Me.lblrfc)
        Me.Controls.Add(Me.lblrasoncial)
        Me.Controls.Add(Me.txtRfc)
        Me.Controls.Add(Me.btnClientes)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblEmpresa)
        Me.Controls.Add(Me.txtNumCuenta)
        Me.Controls.Add(Me.lblNumCuenta)
        Me.Controls.Add(Me.grbformaPago)
        Me.Controls.Add(Me.lblMetodoPago)
        Me.Controls.Add(Me.gpoDatos)
        Me.Controls.Add(Me.btnAplicarPago)
        Me.Controls.Add(Me.Importe)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Saldo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.UltraLabel6)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Abono)
        Me.Controls.Add(Me.Total)
        Me.Controls.Add(Me.txtUUID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtFechaFactura)
        Me.Controls.Add(Me.txtSerie)
        Me.Controls.Add(Me.txtFactura)
        Me.Controls.Add(Me.lblFechaEmision)
        Me.Controls.Add(Me.lblSerieFactura)
        Me.Controls.Add(Me.lblNumFactura)
        Me.Controls.Add(Me.upb)
        Me.Name = "frmPagos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pagos Parciales"
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gpoDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gpoDatos.ResumeLayout(False)
        Me.gpoDatos.PerformLayout()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents upb As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents txtUUID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFechaFactura As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents txtSerie As System.Windows.Forms.TextBox
    Friend WithEvents txtFactura As System.Windows.Forms.TextBox
    Friend WithEvents lblFechaEmision As System.Windows.Forms.Label
    Friend WithEvents lblSerieFactura As System.Windows.Forms.Label
    Friend WithEvents lblNumFactura As System.Windows.Forms.Label
    Friend WithEvents Saldo As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Abono As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Total As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Importe As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents btnAplicarPago As System.Windows.Forms.Button
    Friend WithEvents gpoDatos As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents txtNumOperacion As System.Windows.Forms.TextBox
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents grbBancos As System.Windows.Forms.ComboBox
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtCtaOrdenante As System.Windows.Forms.TextBox
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtCtaBeneficiario As System.Windows.Forms.TextBox
    Friend WithEvents UltraLabel5 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblMetodoPago As System.Windows.Forms.Label
    Friend WithEvents grbformaPago As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents txtNumCuenta As System.Windows.Forms.TextBox
    Friend WithEvents lblNumCuenta As System.Windows.Forms.Label
    Friend WithEvents lblEmpresa As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents umsg As Infragistics.Win.UltraWinToolTip.UltraToolTipManager
    Friend WithEvents btnClientes As System.Windows.Forms.Button
    Friend WithEvents txtRazonSocial As System.Windows.Forms.TextBox
    Friend WithEvents lblrfc As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblrasoncial As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtRfc As System.Windows.Forms.TextBox
    Friend WithEvents lblEmail As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtEmail As System.Windows.Forms.TextBox
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents fechaPago As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
End Class
