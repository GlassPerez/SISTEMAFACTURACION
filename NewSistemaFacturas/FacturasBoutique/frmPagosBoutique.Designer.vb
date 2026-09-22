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
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Saldo = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.UltraLabel6 = New Infragistics.Win.Misc.UltraLabel()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Abono = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Total = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.txtUUID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFechaFactura = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.txtSerie = New System.Windows.Forms.TextBox()
        Me.txtFactura = New System.Windows.Forms.TextBox()
        Me.lblFechaEmision = New System.Windows.Forms.Label()
        Me.lblSerieFactura = New System.Windows.Forms.Label()
        Me.lblNumFactura = New System.Windows.Forms.Label()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.fechaPago = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.upb = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Importe = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.btnAplicarPago = New System.Windows.Forms.Button()
        Me.lblMetodoPago = New System.Windows.Forms.Label()
        Me.grbformaPago = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.lblNumCuenta = New System.Windows.Forms.Label()
        Me.txtNumCuenta = New System.Windows.Forms.TextBox()
        Me.lblEmpresa = New System.Windows.Forms.Label()
        Me.txtRfc = New System.Windows.Forms.TextBox()
        Me.lblrasoncial = New Infragistics.Win.Misc.UltraLabel()
        Me.lblrfc = New Infragistics.Win.Misc.UltraLabel()
        Me.txtRazonSocial = New System.Windows.Forms.TextBox()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblEmail = New Infragistics.Win.Misc.UltraLabel()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(290, 165)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(14, 13)
        Me.Label4.TabIndex = 190
        Me.Label4.Text = "+"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Saldo
        '
        Me.Saldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Saldo.Location = New System.Drawing.Point(163, 181)
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Size = New System.Drawing.Size(123, 24)
        Me.Saldo.TabIndex = 184
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(111, 184)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 183
        Me.Label2.Text = "Saldo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(148, 165)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(147, 10)
        Me.UltraLabel6.TabIndex = 182
        Me.UltraLabel6.Text = "------------------------------------"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(120, 120)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 181
        Me.Label11.Text = "Total"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(111, 150)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 180
        Me.Label8.Text = "Abono"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Abono
        '
        Me.Abono.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Abono.Location = New System.Drawing.Point(163, 143)
        Me.Abono.Name = "Abono"
        Me.Abono.ReadOnly = True
        Me.Abono.Size = New System.Drawing.Size(121, 24)
        Me.Abono.TabIndex = 178
        '
        'Total
        '
        Me.Total.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Total.Location = New System.Drawing.Point(162, 113)
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Size = New System.Drawing.Size(124, 24)
        Me.Total.TabIndex = 179
        '
        'txtUUID
        '
        Me.txtUUID.Location = New System.Drawing.Point(163, 12)
        Me.txtUUID.Name = "txtUUID"
        Me.txtUUID.ReadOnly = True
        Me.txtUUID.Size = New System.Drawing.Size(279, 20)
        Me.txtUUID.TabIndex = 177
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(64, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 176
        Me.Label1.Text = "Folio Fiscal (UUID)"
        '
        'txtFechaFactura
        '
        Me.txtFechaFactura.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.txtFechaFactura.Location = New System.Drawing.Point(163, 84)
        Me.txtFechaFactura.Name = "txtFechaFactura"
        Me.txtFechaFactura.ReadOnly = True
        Me.txtFechaFactura.Size = New System.Drawing.Size(123, 21)
        Me.txtFechaFactura.TabIndex = 175
        Me.txtFechaFactura.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'txtSerie
        '
        Me.txtSerie.Location = New System.Drawing.Point(163, 60)
        Me.txtSerie.Name = "txtSerie"
        Me.txtSerie.ReadOnly = True
        Me.txtSerie.Size = New System.Drawing.Size(163, 20)
        Me.txtSerie.TabIndex = 174
        '
        'txtFactura
        '
        Me.txtFactura.Location = New System.Drawing.Point(163, 36)
        Me.txtFactura.Name = "txtFactura"
        Me.txtFactura.ReadOnly = True
        Me.txtFactura.Size = New System.Drawing.Size(163, 20)
        Me.txtFactura.TabIndex = 173
        '
        'lblFechaEmision
        '
        Me.lblFechaEmision.AutoSize = True
        Me.lblFechaEmision.Location = New System.Drawing.Point(24, 88)
        Me.lblFechaEmision.Name = "lblFechaEmision"
        Me.lblFechaEmision.Size = New System.Drawing.Size(133, 13)
        Me.lblFechaEmision.TabIndex = 172
        Me.lblFechaEmision.Text = "Fecha Venta de la Factura"
        '
        'lblSerieFactura
        '
        Me.lblSerieFactura.AutoSize = True
        Me.lblSerieFactura.Location = New System.Drawing.Point(62, 64)
        Me.lblSerieFactura.Name = "lblSerieFactura"
        Me.lblSerieFactura.Size = New System.Drawing.Size(96, 13)
        Me.lblSerieFactura.TabIndex = 171
        Me.lblSerieFactura.Text = "Serie de la Factura"
        '
        'lblNumFactura
        '
        Me.lblNumFactura.AutoSize = True
        Me.lblNumFactura.Location = New System.Drawing.Point(21, 39)
        Me.lblNumFactura.Name = "lblNumFactura"
        Me.lblNumFactura.Size = New System.Drawing.Size(141, 13)
        Me.lblNumFactura.TabIndex = 170
        Me.lblNumFactura.Text = "No. de Factura Relacionada"
        '
        'UltraLabel7
        '
        Appearance5.BackColor = System.Drawing.Color.Transparent
        Appearance5.TextHAlignAsString = "Left"
        Me.UltraLabel7.Appearance = Appearance5
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel7.Location = New System.Drawing.Point(257, 341)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UltraLabel7.Size = New System.Drawing.Size(87, 14)
        Me.UltraLabel7.TabIndex = 128
        Me.UltraLabel7.Text = "Fecha de Pago:"
        '
        'fechaPago
        '
        Me.fechaPago.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.fechaPago.Location = New System.Drawing.Point(350, 341)
        Me.fechaPago.Name = "fechaPago"
        Me.fechaPago.Size = New System.Drawing.Size(113, 21)
        Me.fechaPago.TabIndex = 127
        Me.fechaPago.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'upb
        '
        Me.upb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.upb.Location = New System.Drawing.Point(0, 467)
        Me.upb.Name = "upb"
        Me.upb.Size = New System.Drawing.Size(475, 29)
        Me.upb.TabIndex = 199
        Me.upb.Text = "[Formatted]"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(17, 402)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 13)
        Me.Label3.TabIndex = 200
        Me.Label3.Text = "Importe que desea abonar ..?"
        '
        'Importe
        '
        Appearance4.TextHAlignAsString = "Center"
        Me.Importe.Appearance = Appearance4
        Me.Importe.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Importe.Location = New System.Drawing.Point(198, 396)
        Me.Importe.Name = "Importe"
        Me.Importe.Size = New System.Drawing.Size(140, 24)
        Me.Importe.TabIndex = 184
        '
        'btnAplicarPago
        '
        Me.btnAplicarPago.BackColor = System.Drawing.Color.Black
        Me.btnAplicarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAplicarPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAplicarPago.ForeColor = System.Drawing.Color.White
        Me.btnAplicarPago.Location = New System.Drawing.Point(354, 385)
        Me.btnAplicarPago.Name = "btnAplicarPago"
        Me.btnAplicarPago.Size = New System.Drawing.Size(100, 47)
        Me.btnAplicarPago.TabIndex = 202
        Me.btnAplicarPago.Text = "Aplicar Pago Relacionado"
        Me.btnAplicarPago.UseVisualStyleBackColor = False
        '
        'lblMetodoPago
        '
        Me.lblMetodoPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetodoPago.ForeColor = System.Drawing.Color.Black
        Me.lblMetodoPago.Location = New System.Drawing.Point(13, 210)
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
        Me.grbformaPago.Location = New System.Drawing.Point(72, 216)
        Me.grbformaPago.Name = "grbformaPago"
        Me.grbformaPago.ReadOnly = True
        Me.grbformaPago.Size = New System.Drawing.Size(195, 22)
        Me.grbformaPago.TabIndex = 186
        '
        'lblNumCuenta
        '
        Me.lblNumCuenta.AutoSize = True
        Me.lblNumCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumCuenta.ForeColor = System.Drawing.Color.Black
        Me.lblNumCuenta.Location = New System.Drawing.Point(290, 219)
        Me.lblNumCuenta.Name = "lblNumCuenta"
        Me.lblNumCuenta.Size = New System.Drawing.Size(80, 13)
        Me.lblNumCuenta.TabIndex = 187
        Me.lblNumCuenta.Text = "Núm. Cuenta"
        Me.lblNumCuenta.Visible = False
        '
        'txtNumCuenta
        '
        Me.txtNumCuenta.Location = New System.Drawing.Point(376, 216)
        Me.txtNumCuenta.MaxLength = 4
        Me.txtNumCuenta.Name = "txtNumCuenta"
        Me.txtNumCuenta.Size = New System.Drawing.Size(53, 20)
        Me.txtNumCuenta.TabIndex = 188
        Me.txtNumCuenta.Visible = False
        '
        'lblEmpresa
        '
        Me.lblEmpresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmpresa.ForeColor = System.Drawing.Color.Black
        Me.lblEmpresa.Location = New System.Drawing.Point(16, 258)
        Me.lblEmpresa.Name = "lblEmpresa"
        Me.lblEmpresa.Size = New System.Drawing.Size(181, 24)
        Me.lblEmpresa.TabIndex = 189
        Me.lblEmpresa.Text = "Empresa o cliente a Aplicar:"
        '
        'txtRfc
        '
        Me.txtRfc.Location = New System.Drawing.Point(229, 258)
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
        Me.lblrasoncial.Location = New System.Drawing.Point(12, 284)
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
        Me.lblrfc.Location = New System.Drawing.Point(197, 261)
        Me.lblrfc.Name = "lblrfc"
        Me.lblrfc.Size = New System.Drawing.Size(31, 14)
        Me.lblrfc.TabIndex = 194
        Me.lblrfc.Text = "RFC:"
        '
        'txtRazonSocial
        '
        Me.txtRazonSocial.Location = New System.Drawing.Point(90, 284)
        Me.txtRazonSocial.Multiline = True
        Me.txtRazonSocial.Name = "txtRazonSocial"
        Me.txtRazonSocial.ReadOnly = True
        Me.txtRazonSocial.Size = New System.Drawing.Size(364, 36)
        Me.txtRazonSocial.TabIndex = 193
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(53, 335)
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
        Me.lblEmail.Location = New System.Drawing.Point(16, 341)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(36, 14)
        Me.lblEmail.TabIndex = 197
        Me.lblEmail.Text = "Email:"
        '
        'frmPagosBoutique
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(475, 496)
        Me.Controls.Add(Me.UltraLabel7)
        Me.Controls.Add(Me.btnAplicarPago)
        Me.Controls.Add(Me.fechaPago)
        Me.Controls.Add(Me.Importe)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.upb)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtRazonSocial)
        Me.Controls.Add(Me.lblrfc)
        Me.Controls.Add(Me.lblrasoncial)
        Me.Controls.Add(Me.txtRfc)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblEmpresa)
        Me.Controls.Add(Me.txtNumCuenta)
        Me.Controls.Add(Me.lblNumCuenta)
        Me.Controls.Add(Me.grbformaPago)
        Me.Controls.Add(Me.lblMetodoPago)
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
        Me.Name = "frmPagosBoutique"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Complemento de Pago"
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Saldo As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Abono As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Total As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents txtUUID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtFechaFactura As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents txtSerie As System.Windows.Forms.TextBox
    Friend WithEvents txtFactura As System.Windows.Forms.TextBox
    Friend WithEvents lblFechaEmision As System.Windows.Forms.Label
    Friend WithEvents lblSerieFactura As System.Windows.Forms.Label
    Friend WithEvents lblNumFactura As System.Windows.Forms.Label
    Friend WithEvents upb As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents fechaPago As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents Label3 As Label
    Friend WithEvents Importe As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents btnAplicarPago As Button
    Friend WithEvents lblMetodoPago As Label
    Friend WithEvents grbformaPago As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblNumCuenta As Label
    Friend WithEvents txtNumCuenta As TextBox
    Friend WithEvents lblEmpresa As Label
    Friend WithEvents txtRfc As TextBox
    Friend WithEvents lblrasoncial As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblrfc As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtRazonSocial As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblEmail As Infragistics.Win.Misc.UltraLabel
End Class
