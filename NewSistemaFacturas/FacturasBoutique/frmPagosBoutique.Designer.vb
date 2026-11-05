<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPagosBoutique
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
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance55 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.upb = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnAplicarPago = New System.Windows.Forms.Button()
        Me.lblMetodoPago = New System.Windows.Forms.Label()
        Me.grbformaPago = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.lblNumCuenta = New System.Windows.Forms.Label()
        Me.txtNumCuenta = New System.Windows.Forms.TextBox()
        Me.txtRfc = New System.Windows.Forms.TextBox()
        Me.lblrasoncial = New Infragistics.Win.Misc.UltraLabel()
        Me.lblrfc = New Infragistics.Win.Misc.UltraLabel()
        Me.txtRazonSocial = New System.Windows.Forms.TextBox()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblEmail = New Infragistics.Win.Misc.UltraLabel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.DGFacturas = New System.Windows.Forms.DataGridView()
        Me.nofacR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.serieR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.uuidR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fetimR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.fevtaR = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Descuento_Cero = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_Rel_Fac = New System.Windows.Forms.Button()
        Me.UltraGroupBox3 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblNumFactura = New System.Windows.Forms.Label()
        Me.lblSerieFactura = New System.Windows.Forms.Label()
        Me.lblFechaEmision = New System.Windows.Forms.Label()
        Me.txtFactura = New System.Windows.Forms.TextBox()
        Me.txtSerie = New System.Windows.Forms.TextBox()
        Me.txtFechaFactura = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.txtUUID = New System.Windows.Forms.TextBox()
        Me.UltraGroupBox1 = New Infragistics.Win.Misc.UltraGroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Saldo = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Abono = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Total = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Importe = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.fechaPago = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.DGFacturas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGroupBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox3.SuspendLayout()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.UltraGroupBox1.SuspendLayout()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraLabel7
        '
        Appearance5.BackColor = System.Drawing.Color.Transparent
        Appearance5.TextHAlignAsString = "Left"
        Me.UltraLabel7.Appearance = Appearance5
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel7.Location = New System.Drawing.Point(89, 231)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UltraLabel7.Size = New System.Drawing.Size(94, 16)
        Me.UltraLabel7.TabIndex = 128
        Me.UltraLabel7.Text = "Fecha de Pago:"
        '
        'upb
        '
        Me.upb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.upb.Location = New System.Drawing.Point(0, 424)
        Me.upb.Name = "upb"
        Me.upb.Size = New System.Drawing.Size(569, 29)
        Me.upb.TabIndex = 199
        Me.upb.Text = "[Formatted]"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 388)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 13)
        Me.Label3.TabIndex = 200
        Me.Label3.Text = "Importe que desea abonar ..?"
        '
        'btnAplicarPago
        '
        Me.btnAplicarPago.BackColor = System.Drawing.Color.Black
        Me.btnAplicarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAplicarPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAplicarPago.ForeColor = System.Drawing.Color.White
        Me.btnAplicarPago.Location = New System.Drawing.Point(344, 382)
        Me.btnAplicarPago.Name = "btnAplicarPago"
        Me.btnAplicarPago.Size = New System.Drawing.Size(210, 27)
        Me.btnAplicarPago.TabIndex = 202
        Me.btnAplicarPago.Text = "Aplicar Complemento de Pago"
        Me.btnAplicarPago.UseVisualStyleBackColor = False
        '
        'lblMetodoPago
        '
        Me.lblMetodoPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetodoPago.ForeColor = System.Drawing.Color.Black
        Me.lblMetodoPago.Location = New System.Drawing.Point(7, 104)
        Me.lblMetodoPago.Name = "lblMetodoPago"
        Me.lblMetodoPago.Size = New System.Drawing.Size(71, 36)
        Me.lblMetodoPago.TabIndex = 185
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
        Me.grbformaPago.Location = New System.Drawing.Point(69, 110)
        Me.grbformaPago.Name = "grbformaPago"
        Me.grbformaPago.ReadOnly = True
        Me.grbformaPago.Size = New System.Drawing.Size(153, 22)
        Me.grbformaPago.TabIndex = 186
        '
        'lblNumCuenta
        '
        Me.lblNumCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumCuenta.ForeColor = System.Drawing.Color.Black
        Me.lblNumCuenta.Location = New System.Drawing.Point(244, 105)
        Me.lblNumCuenta.Name = "lblNumCuenta"
        Me.lblNumCuenta.Size = New System.Drawing.Size(60, 27)
        Me.lblNumCuenta.TabIndex = 187
        Me.lblNumCuenta.Text = "Núm. Cuenta"
        Me.lblNumCuenta.Visible = False
        '
        'txtNumCuenta
        '
        Me.txtNumCuenta.Location = New System.Drawing.Point(296, 107)
        Me.txtNumCuenta.MaxLength = 4
        Me.txtNumCuenta.Name = "txtNumCuenta"
        Me.txtNumCuenta.Size = New System.Drawing.Size(53, 20)
        Me.txtNumCuenta.TabIndex = 188
        Me.txtNumCuenta.Visible = False
        '
        'txtRfc
        '
        Me.txtRfc.Location = New System.Drawing.Point(84, 144)
        Me.txtRfc.MaxLength = 13
        Me.txtRfc.Name = "txtRfc"
        Me.txtRfc.ReadOnly = True
        Me.txtRfc.Size = New System.Drawing.Size(129, 20)
        Me.txtRfc.TabIndex = 192
        Me.txtRfc.TabStop = False
        '
        'lblrasoncial
        '
        Appearance52.BackColor = System.Drawing.Color.Transparent
        Me.lblrasoncial.Appearance = Appearance52
        Me.lblrasoncial.AutoSize = True
        Me.lblrasoncial.Location = New System.Drawing.Point(6, 170)
        Me.lblrasoncial.Name = "lblrasoncial"
        Me.lblrasoncial.Size = New System.Drawing.Size(74, 14)
        Me.lblrasoncial.TabIndex = 195
        Me.lblrasoncial.Text = "Razón Social:"
        '
        'lblrfc
        '
        Appearance54.BackColor = System.Drawing.Color.Transparent
        Me.lblrfc.Appearance = Appearance54
        Me.lblrfc.AutoSize = True
        Me.lblrfc.Location = New System.Drawing.Point(52, 147)
        Me.lblrfc.Name = "lblrfc"
        Me.lblrfc.Size = New System.Drawing.Size(31, 14)
        Me.lblrfc.TabIndex = 194
        Me.lblrfc.Text = "RFC:"
        '
        'txtRazonSocial
        '
        Me.txtRazonSocial.Location = New System.Drawing.Point(84, 170)
        Me.txtRazonSocial.Multiline = True
        Me.txtRazonSocial.Name = "txtRazonSocial"
        Me.txtRazonSocial.ReadOnly = True
        Me.txtRazonSocial.Size = New System.Drawing.Size(268, 20)
        Me.txtRazonSocial.TabIndex = 193
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(84, 197)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.ReadOnly = True
        Me.txtEmail.Size = New System.Drawing.Size(188, 20)
        Me.txtEmail.TabIndex = 196
        '
        'lblEmail
        '
        Appearance55.BackColor = System.Drawing.Color.Transparent
        Me.lblEmail.Appearance = Appearance55
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(47, 203)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(36, 14)
        Me.lblEmail.TabIndex = 197
        Me.lblEmail.Text = "Email:"
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.DGFacturas)
        Me.Panel3.Controls.Add(Me.btn_Rel_Fac)
        Me.Panel3.Location = New System.Drawing.Point(0, 261)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(566, 99)
        Me.Panel3.TabIndex = 203
        '
        'DGFacturas
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGFacturas.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DGFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGFacturas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.nofacR, Me.serieR, Me.uuidR, Me.fetimR, Me.fevtaR, Me.Descuento_Cero})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DGFacturas.DefaultCellStyle = DataGridViewCellStyle2
        Me.DGFacturas.Location = New System.Drawing.Point(116, 0)
        Me.DGFacturas.Name = "DGFacturas"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DGFacturas.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.DGFacturas.Size = New System.Drawing.Size(447, 96)
        Me.DGFacturas.TabIndex = 1
        '
        'nofacR
        '
        Me.nofacR.HeaderText = "No Fac"
        Me.nofacR.Name = "nofacR"
        Me.nofacR.ReadOnly = True
        '
        'serieR
        '
        Me.serieR.HeaderText = "Serie"
        Me.serieR.Name = "serieR"
        Me.serieR.ReadOnly = True
        '
        'uuidR
        '
        Me.uuidR.HeaderText = "UUID"
        Me.uuidR.Name = "uuidR"
        Me.uuidR.ReadOnly = True
        Me.uuidR.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'fetimR
        '
        Me.fetimR.HeaderText = "Fec Tim"
        Me.fetimR.Name = "fetimR"
        Me.fetimR.ReadOnly = True
        '
        'fevtaR
        '
        Me.fevtaR.HeaderText = "Fec Vta"
        Me.fevtaR.Name = "fevtaR"
        Me.fevtaR.ReadOnly = True
        '
        'Descuento_Cero
        '
        Me.Descuento_Cero.HeaderText = "Tipo"
        Me.Descuento_Cero.Name = "Descuento_Cero"
        Me.Descuento_Cero.ReadOnly = True
        '
        'btn_Rel_Fac
        '
        Me.btn_Rel_Fac.BackColor = System.Drawing.Color.Azure
        Me.btn_Rel_Fac.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_Rel_Fac.Location = New System.Drawing.Point(3, 0)
        Me.btn_Rel_Fac.Name = "btn_Rel_Fac"
        Me.btn_Rel_Fac.Size = New System.Drawing.Size(110, 98)
        Me.btn_Rel_Fac.TabIndex = 0
        Me.btn_Rel_Fac.Text = "+ Complemento de Pago (Relacionado)"
        Me.btn_Rel_Fac.UseVisualStyleBackColor = False
        '
        'UltraGroupBox3
        '
        Appearance1.BackColor = System.Drawing.Color.Cyan
        Appearance1.BackColor2 = System.Drawing.Color.SteelBlue
        Appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance1.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered
        Me.UltraGroupBox3.Appearance = Appearance1
        Appearance2.BackColor = System.Drawing.Color.Cyan
        Appearance2.BackColor2 = System.Drawing.Color.SteelBlue
        Me.UltraGroupBox3.ContentAreaAppearance = Appearance2
        Me.UltraGroupBox3.Controls.Add(Me.Label1)
        Me.UltraGroupBox3.Controls.Add(Me.lblNumFactura)
        Me.UltraGroupBox3.Controls.Add(Me.lblSerieFactura)
        Me.UltraGroupBox3.Controls.Add(Me.lblFechaEmision)
        Me.UltraGroupBox3.Controls.Add(Me.txtFactura)
        Me.UltraGroupBox3.Controls.Add(Me.txtSerie)
        Me.UltraGroupBox3.Controls.Add(Me.txtFechaFactura)
        Me.UltraGroupBox3.Controls.Add(Me.txtUUID)
        Me.UltraGroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGroupBox3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.UltraGroupBox3.Location = New System.Drawing.Point(0, 1)
        Me.UltraGroupBox3.Name = "UltraGroupBox3"
        Me.UltraGroupBox3.Size = New System.Drawing.Size(568, 95)
        Me.UltraGroupBox3.TabIndex = 206
        Me.UltraGroupBox3.Text = "Factura Seleccionada"
        Me.UltraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(6, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 32)
        Me.Label1.TabIndex = 184
        Me.Label1.Text = "Folio Fiscal (UUID)"
        '
        'lblNumFactura
        '
        Me.lblNumFactura.BackColor = System.Drawing.Color.Transparent
        Me.lblNumFactura.Location = New System.Drawing.Point(6, 57)
        Me.lblNumFactura.Name = "lblNumFactura"
        Me.lblNumFactura.Size = New System.Drawing.Size(95, 34)
        Me.lblNumFactura.TabIndex = 178
        Me.lblNumFactura.Text = "No. de Factura Relacionada"
        '
        'lblSerieFactura
        '
        Me.lblSerieFactura.BackColor = System.Drawing.Color.Transparent
        Me.lblSerieFactura.Location = New System.Drawing.Point(349, 25)
        Me.lblSerieFactura.Name = "lblSerieFactura"
        Me.lblSerieFactura.Size = New System.Drawing.Size(68, 32)
        Me.lblSerieFactura.TabIndex = 179
        Me.lblSerieFactura.Text = "Serie de la Factura"
        '
        'lblFechaEmision
        '
        Me.lblFechaEmision.BackColor = System.Drawing.Color.Transparent
        Me.lblFechaEmision.Location = New System.Drawing.Point(349, 60)
        Me.lblFechaEmision.Name = "lblFechaEmision"
        Me.lblFechaEmision.Size = New System.Drawing.Size(72, 31)
        Me.lblFechaEmision.TabIndex = 180
        Me.lblFechaEmision.Text = "Fecha Venta"
        '
        'txtFactura
        '
        Me.txtFactura.Location = New System.Drawing.Point(104, 59)
        Me.txtFactura.Name = "txtFactura"
        Me.txtFactura.ReadOnly = True
        Me.txtFactura.Size = New System.Drawing.Size(163, 20)
        Me.txtFactura.TabIndex = 181
        '
        'txtSerie
        '
        Me.txtSerie.Location = New System.Drawing.Point(423, 28)
        Me.txtSerie.Name = "txtSerie"
        Me.txtSerie.ReadOnly = True
        Me.txtSerie.Size = New System.Drawing.Size(126, 20)
        Me.txtSerie.TabIndex = 182
        '
        'txtFechaFactura
        '
        Me.txtFechaFactura.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.txtFechaFactura.Location = New System.Drawing.Point(424, 58)
        Me.txtFechaFactura.Name = "txtFechaFactura"
        Me.txtFechaFactura.ReadOnly = True
        Me.txtFechaFactura.Size = New System.Drawing.Size(123, 21)
        Me.txtFechaFactura.TabIndex = 183
        Me.txtFechaFactura.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'txtUUID
        '
        Me.txtUUID.Location = New System.Drawing.Point(84, 28)
        Me.txtUUID.Name = "txtUUID"
        Me.txtUUID.ReadOnly = True
        Me.txtUUID.Size = New System.Drawing.Size(257, 20)
        Me.txtUUID.TabIndex = 185
        '
        'UltraGroupBox1
        '
        Appearance15.BackColor = System.Drawing.Color.Cornsilk
        Appearance15.BackColor2 = System.Drawing.Color.Gold
        Appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance15.BorderColor = System.Drawing.Color.Cornsilk
        Appearance15.BorderColor2 = System.Drawing.Color.Gold
        Appearance15.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Centered
        Me.UltraGroupBox1.Appearance = Appearance15
        Appearance18.BackColor = System.Drawing.Color.Cornsilk
        Appearance18.BackColor2 = System.Drawing.Color.Gold
        Me.UltraGroupBox1.ContentAreaAppearance = Appearance18
        Me.UltraGroupBox1.Controls.Add(Me.Label4)
        Me.UltraGroupBox1.Controls.Add(Me.Saldo)
        Me.UltraGroupBox1.Controls.Add(Me.Label2)
        Me.UltraGroupBox1.Controls.Add(Me.UltraLabel6)
        Me.UltraGroupBox1.Controls.Add(Me.Label11)
        Me.UltraGroupBox1.Controls.Add(Me.Label8)
        Me.UltraGroupBox1.Controls.Add(Me.Abono)
        Me.UltraGroupBox1.Controls.Add(Me.Total)
        Me.UltraGroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraGroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Appearance19.BackColor = System.Drawing.Color.Goldenrod
        Me.UltraGroupBox1.HeaderAppearance = Appearance19
        Me.UltraGroupBox1.Location = New System.Drawing.Point(358, 95)
        Me.UltraGroupBox1.Name = "UltraGroupBox1"
        Me.UltraGroupBox1.Size = New System.Drawing.Size(208, 160)
        Me.UltraGroupBox1.TabIndex = 207
        Me.UltraGroupBox1.Text = "Importes de Factura Principal"
        Me.UltraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(187, 57)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(14, 13)
        Me.Label4.TabIndex = 206
        Me.Label4.Text = "+"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Saldo
        '
        Me.Saldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Saldo.Location = New System.Drawing.Point(60, 113)
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Size = New System.Drawing.Size(123, 24)
        Me.Saldo.TabIndex = 205
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 204
        Me.Label2.Text = "Saldo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(45, 97)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(147, 10)
        Me.UltraLabel6.TabIndex = 203
        Me.UltraLabel6.Text = "------------------------------------"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(17, 43)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 202
        Me.Label11.Text = "Total"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 76)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 201
        Me.Label8.Text = "Abono"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Abono
        '
        Me.Abono.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Abono.Location = New System.Drawing.Point(60, 69)
        Me.Abono.Name = "Abono"
        Me.Abono.ReadOnly = True
        Me.Abono.Size = New System.Drawing.Size(121, 24)
        Me.Abono.TabIndex = 199
        '
        'Total
        '
        Me.Total.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Total.Location = New System.Drawing.Point(59, 36)
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Size = New System.Drawing.Size(124, 24)
        Me.Total.TabIndex = 200
        '
        'Importe
        '
        Appearance3.BackColor = System.Drawing.Color.YellowGreen
        Me.Importe.Appearance = Appearance3
        Me.Importe.BackColor = System.Drawing.Color.YellowGreen
        Me.Importe.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Importe.Location = New System.Drawing.Point(198, 383)
        Me.Importe.Name = "Importe"
        Me.Importe.Size = New System.Drawing.Size(123, 24)
        Me.Importe.TabIndex = 208
        '
        'fechaPago
        '
        Appearance7.BackColor = System.Drawing.Color.YellowGreen
        Me.fechaPago.Appearance = Appearance7
        Me.fechaPago.BackColor = System.Drawing.Color.YellowGreen
        Me.fechaPago.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.fechaPago.Location = New System.Drawing.Point(189, 227)
        Me.fechaPago.Name = "fechaPago"
        Me.fechaPago.Size = New System.Drawing.Size(123, 21)
        Me.fechaPago.TabIndex = 209
        Me.fechaPago.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'frmPagosBoutique
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(569, 453)
        Me.Controls.Add(Me.fechaPago)
        Me.Controls.Add(Me.Importe)
        Me.Controls.Add(Me.UltraGroupBox1)
        Me.Controls.Add(Me.UltraGroupBox3)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.UltraLabel7)
        Me.Controls.Add(Me.btnAplicarPago)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.upb)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtRazonSocial)
        Me.Controls.Add(Me.lblrfc)
        Me.Controls.Add(Me.lblrasoncial)
        Me.Controls.Add(Me.txtRfc)
        Me.Controls.Add(Me.txtNumCuenta)
        Me.Controls.Add(Me.lblNumCuenta)
        Me.Controls.Add(Me.grbformaPago)
        Me.Controls.Add(Me.lblMetodoPago)
        Me.Name = "frmPagosBoutique"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Complemento de Pago"
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        CType(Me.DGFacturas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGroupBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox3.ResumeLayout(False)
        Me.UltraGroupBox3.PerformLayout()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UltraGroupBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.UltraGroupBox1.ResumeLayout(False)
        Me.UltraGroupBox1.PerformLayout()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents upb As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Label3 As Label
    Friend WithEvents btnAplicarPago As Button
    Friend WithEvents lblMetodoPago As Label
    Friend WithEvents grbformaPago As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblNumCuenta As Label
    Friend WithEvents txtNumCuenta As TextBox
    Friend WithEvents txtRfc As TextBox
    Friend WithEvents lblrasoncial As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblrfc As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtRazonSocial As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblEmail As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents DGFacturas As DataGridView
    Friend WithEvents btn_Rel_Fac As Button
    Friend WithEvents nofacR As DataGridViewTextBoxColumn
    Friend WithEvents serieR As DataGridViewTextBoxColumn
    Friend WithEvents uuidR As DataGridViewTextBoxColumn
    Friend WithEvents fetimR As DataGridViewTextBoxColumn
    Friend WithEvents fevtaR As DataGridViewTextBoxColumn
    Friend WithEvents Descuento_Cero As DataGridViewTextBoxColumn
    Friend WithEvents UltraGroupBox3 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents UltraGroupBox1 As Infragistics.Win.Misc.UltraGroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Saldo As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Label2 As Label
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Abono As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Total As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Label1 As Label
    Friend WithEvents lblNumFactura As Label
    Friend WithEvents lblSerieFactura As Label
    Friend WithEvents lblFechaEmision As Label
    Friend WithEvents txtFactura As TextBox
    Friend WithEvents txtSerie As TextBox
    Friend WithEvents txtFechaFactura As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents txtUUID As TextBox
    Friend WithEvents Importe As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents fechaPago As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
End Class
