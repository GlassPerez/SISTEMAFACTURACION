<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPagosCoffe
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance55 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance52 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Me.upb = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.Label4 = New System.Windows.Forms.Label()
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
        Me.btnAplicarPago = New System.Windows.Forms.Button()
        Me.Importe = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.UltraLabel7 = New Infragistics.Win.Misc.UltraLabel()
        Me.fechaPago = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.lblEmail = New Infragistics.Win.Misc.UltraLabel()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.txtRazonSocial = New System.Windows.Forms.TextBox()
        Me.lblrfc = New Infragistics.Win.Misc.UltraLabel()
        Me.lblrasoncial = New Infragistics.Win.Misc.UltraLabel()
        Me.txtRfc = New System.Windows.Forms.TextBox()
        Me.lblEmpresa = New System.Windows.Forms.Label()
        Me.txtNumCuenta = New System.Windows.Forms.TextBox()
        Me.lblNumCuenta = New System.Windows.Forms.Label()
        Me.grbformaPago = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.lblMetodoPago = New System.Windows.Forms.Label()
        Me.Saldo = New Infragistics.Win.UltraWinEditors.UltraCurrencyEditor()
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'upb
        '
        Me.upb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.upb.Location = New System.Drawing.Point(0, 486)
        Me.upb.Name = "upb"
        Me.upb.Size = New System.Drawing.Size(470, 29)
        Me.upb.TabIndex = 200
        Me.upb.Text = "[Formatted]"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(311, 172)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(14, 13)
        Me.Label4.TabIndex = 221
        Me.Label4.Text = "+"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(132, 191)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 214
        Me.Label2.Text = "Saldo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UltraLabel6
        '
        Me.UltraLabel6.Location = New System.Drawing.Point(169, 172)
        Me.UltraLabel6.Name = "UltraLabel6"
        Me.UltraLabel6.Size = New System.Drawing.Size(147, 10)
        Me.UltraLabel6.TabIndex = 213
        Me.UltraLabel6.Text = "------------------------------------"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(141, 127)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(34, 13)
        Me.Label11.TabIndex = 212
        Me.Label11.Text = "Total"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(132, 157)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 211
        Me.Label8.Text = "Abono"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Abono
        '
        Me.Abono.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Abono.Location = New System.Drawing.Point(184, 150)
        Me.Abono.Name = "Abono"
        Me.Abono.ReadOnly = True
        Me.Abono.Size = New System.Drawing.Size(128, 24)
        Me.Abono.TabIndex = 209
        '
        'Total
        '
        Me.Total.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Total.Location = New System.Drawing.Point(183, 120)
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Size = New System.Drawing.Size(129, 24)
        Me.Total.TabIndex = 210
        '
        'txtUUID
        '
        Me.txtUUID.Location = New System.Drawing.Point(184, 19)
        Me.txtUUID.Name = "txtUUID"
        Me.txtUUID.ReadOnly = True
        Me.txtUUID.Size = New System.Drawing.Size(279, 20)
        Me.txtUUID.TabIndex = 208
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(85, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 207
        Me.Label1.Text = "Folio Fiscal (UUID)"
        '
        'txtFechaFactura
        '
        Me.txtFechaFactura.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.txtFechaFactura.Location = New System.Drawing.Point(184, 91)
        Me.txtFechaFactura.Name = "txtFechaFactura"
        Me.txtFechaFactura.ReadOnly = True
        Me.txtFechaFactura.Size = New System.Drawing.Size(128, 21)
        Me.txtFechaFactura.TabIndex = 206
        Me.txtFechaFactura.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'txtSerie
        '
        Me.txtSerie.Location = New System.Drawing.Point(184, 67)
        Me.txtSerie.Name = "txtSerie"
        Me.txtSerie.ReadOnly = True
        Me.txtSerie.Size = New System.Drawing.Size(163, 20)
        Me.txtSerie.TabIndex = 205
        '
        'txtFactura
        '
        Me.txtFactura.Location = New System.Drawing.Point(184, 43)
        Me.txtFactura.Name = "txtFactura"
        Me.txtFactura.ReadOnly = True
        Me.txtFactura.Size = New System.Drawing.Size(163, 20)
        Me.txtFactura.TabIndex = 204
        '
        'lblFechaEmision
        '
        Me.lblFechaEmision.AutoSize = True
        Me.lblFechaEmision.Location = New System.Drawing.Point(26, 95)
        Me.lblFechaEmision.Name = "lblFechaEmision"
        Me.lblFechaEmision.Size = New System.Drawing.Size(155, 13)
        Me.lblFechaEmision.TabIndex = 203
        Me.lblFechaEmision.Text = "Fecha que se realizó la Factura"
        '
        'lblSerieFactura
        '
        Me.lblSerieFactura.AutoSize = True
        Me.lblSerieFactura.Location = New System.Drawing.Point(83, 71)
        Me.lblSerieFactura.Name = "lblSerieFactura"
        Me.lblSerieFactura.Size = New System.Drawing.Size(96, 13)
        Me.lblSerieFactura.TabIndex = 202
        Me.lblSerieFactura.Text = "Serie de la Factura"
        '
        'lblNumFactura
        '
        Me.lblNumFactura.AutoSize = True
        Me.lblNumFactura.Location = New System.Drawing.Point(42, 46)
        Me.lblNumFactura.Name = "lblNumFactura"
        Me.lblNumFactura.Size = New System.Drawing.Size(141, 13)
        Me.lblNumFactura.TabIndex = 201
        Me.lblNumFactura.Text = "No. de Factura Relacionada"
        '
        'btnAplicarPago
        '
        Me.btnAplicarPago.BackColor = System.Drawing.Color.Black
        Me.btnAplicarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAplicarPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAplicarPago.ForeColor = System.Drawing.Color.White
        Me.btnAplicarPago.Location = New System.Drawing.Point(360, 400)
        Me.btnAplicarPago.Name = "btnAplicarPago"
        Me.btnAplicarPago.Size = New System.Drawing.Size(98, 60)
        Me.btnAplicarPago.TabIndex = 232
        Me.btnAplicarPago.Text = "Aplicar Pago Relacionado"
        Me.btnAplicarPago.UseVisualStyleBackColor = False
        '
        'Importe
        '
        Appearance4.TextHAlignAsString = "Center"
        Me.Importe.Appearance = Appearance4
        Me.Importe.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Importe.Location = New System.Drawing.Point(212, 427)
        Me.Importe.Name = "Importe"
        Me.Importe.Size = New System.Drawing.Size(138, 24)
        Me.Importe.TabIndex = 231
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(21, 433)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 13)
        Me.Label3.TabIndex = 230
        Me.Label3.Text = "Importe que desea abonar ..?"
        '
        'UltraLabel7
        '
        Appearance5.BackColor = System.Drawing.Color.Transparent
        Appearance5.TextHAlignAsString = "Left"
        Me.UltraLabel7.Appearance = Appearance5
        Me.UltraLabel7.AutoSize = True
        Me.UltraLabel7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel7.Location = New System.Drawing.Point(255, 360)
        Me.UltraLabel7.Name = "UltraLabel7"
        Me.UltraLabel7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.UltraLabel7.Size = New System.Drawing.Size(87, 14)
        Me.UltraLabel7.TabIndex = 234
        Me.UltraLabel7.Text = "Fecha de Pago:"
        '
        'fechaPago
        '
        Me.fechaPago.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.fechaPago.Location = New System.Drawing.Point(348, 360)
        Me.fechaPago.Name = "fechaPago"
        Me.fechaPago.Size = New System.Drawing.Size(113, 21)
        Me.fechaPago.TabIndex = 233
        Me.fechaPago.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'lblEmail
        '
        Appearance55.BackColor = System.Drawing.Color.Transparent
        Me.lblEmail.Appearance = Appearance55
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(14, 360)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(36, 14)
        Me.lblEmail.TabIndex = 245
        Me.lblEmail.Text = "Email:"
        '
        'txtEmail
        '
        Me.txtEmail.Location = New System.Drawing.Point(51, 354)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.ReadOnly = True
        Me.txtEmail.Size = New System.Drawing.Size(188, 20)
        Me.txtEmail.TabIndex = 244
        '
        'txtRazonSocial
        '
        Me.txtRazonSocial.Location = New System.Drawing.Point(88, 303)
        Me.txtRazonSocial.Multiline = True
        Me.txtRazonSocial.Name = "txtRazonSocial"
        Me.txtRazonSocial.ReadOnly = True
        Me.txtRazonSocial.Size = New System.Drawing.Size(364, 36)
        Me.txtRazonSocial.TabIndex = 241
        '
        'lblrfc
        '
        Appearance54.BackColor = System.Drawing.Color.Transparent
        Me.lblrfc.Appearance = Appearance54
        Me.lblrfc.AutoSize = True
        Me.lblrfc.Location = New System.Drawing.Point(195, 280)
        Me.lblrfc.Name = "lblrfc"
        Me.lblrfc.Size = New System.Drawing.Size(31, 14)
        Me.lblrfc.TabIndex = 242
        Me.lblrfc.Text = "RFC:"
        '
        'lblrasoncial
        '
        Appearance52.BackColor = System.Drawing.Color.Transparent
        Me.lblrasoncial.Appearance = Appearance52
        Me.lblrasoncial.AutoSize = True
        Me.lblrasoncial.Location = New System.Drawing.Point(10, 303)
        Me.lblrasoncial.Name = "lblrasoncial"
        Me.lblrasoncial.Size = New System.Drawing.Size(74, 14)
        Me.lblrasoncial.TabIndex = 243
        Me.lblrasoncial.Text = "Razón Social:"
        '
        'txtRfc
        '
        Me.txtRfc.Location = New System.Drawing.Point(227, 277)
        Me.txtRfc.MaxLength = 13
        Me.txtRfc.Name = "txtRfc"
        Me.txtRfc.ReadOnly = True
        Me.txtRfc.Size = New System.Drawing.Size(129, 20)
        Me.txtRfc.TabIndex = 240
        Me.txtRfc.TabStop = False
        '
        'lblEmpresa
        '
        Me.lblEmpresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEmpresa.ForeColor = System.Drawing.Color.Black
        Me.lblEmpresa.Location = New System.Drawing.Point(14, 277)
        Me.lblEmpresa.Name = "lblEmpresa"
        Me.lblEmpresa.Size = New System.Drawing.Size(181, 24)
        Me.lblEmpresa.TabIndex = 239
        Me.lblEmpresa.Text = "Empresa o cliente a Aplicar:"
        '
        'txtNumCuenta
        '
        Me.txtNumCuenta.Location = New System.Drawing.Point(374, 235)
        Me.txtNumCuenta.MaxLength = 4
        Me.txtNumCuenta.Name = "txtNumCuenta"
        Me.txtNumCuenta.Size = New System.Drawing.Size(53, 20)
        Me.txtNumCuenta.TabIndex = 238
        Me.txtNumCuenta.Visible = False
        '
        'lblNumCuenta
        '
        Me.lblNumCuenta.AutoSize = True
        Me.lblNumCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNumCuenta.ForeColor = System.Drawing.Color.Black
        Me.lblNumCuenta.Location = New System.Drawing.Point(288, 238)
        Me.lblNumCuenta.Name = "lblNumCuenta"
        Me.lblNumCuenta.Size = New System.Drawing.Size(80, 13)
        Me.lblNumCuenta.TabIndex = 237
        Me.lblNumCuenta.Text = "Núm. Cuenta"
        Me.lblNumCuenta.Visible = False
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
        Me.grbformaPago.Location = New System.Drawing.Point(70, 235)
        Me.grbformaPago.Name = "grbformaPago"
        Me.grbformaPago.ReadOnly = True
        Me.grbformaPago.Size = New System.Drawing.Size(195, 22)
        Me.grbformaPago.TabIndex = 236
        '
        'lblMetodoPago
        '
        Me.lblMetodoPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetodoPago.ForeColor = System.Drawing.Color.Black
        Me.lblMetodoPago.Location = New System.Drawing.Point(11, 229)
        Me.lblMetodoPago.Name = "lblMetodoPago"
        Me.lblMetodoPago.Size = New System.Drawing.Size(71, 36)
        Me.lblMetodoPago.TabIndex = 235
        Me.lblMetodoPago.Text = "Forma de Pago:"
        '
        'Saldo
        '
        Me.Saldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Saldo.Location = New System.Drawing.Point(184, 188)
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Size = New System.Drawing.Size(123, 24)
        Me.Saldo.TabIndex = 246
        '
        'frmPagosCoffe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(470, 515)
        Me.Controls.Add(Me.Saldo)
        Me.Controls.Add(Me.UltraLabel7)
        Me.Controls.Add(Me.fechaPago)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtRazonSocial)
        Me.Controls.Add(Me.lblrfc)
        Me.Controls.Add(Me.lblrasoncial)
        Me.Controls.Add(Me.txtRfc)
        Me.Controls.Add(Me.lblEmpresa)
        Me.Controls.Add(Me.txtNumCuenta)
        Me.Controls.Add(Me.lblNumCuenta)
        Me.Controls.Add(Me.grbformaPago)
        Me.Controls.Add(Me.lblMetodoPago)
        Me.Controls.Add(Me.btnAplicarPago)
        Me.Controls.Add(Me.Importe)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label4)
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
        Me.Name = "frmPagosCoffe"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pagos Cafeteria"
        CType(Me.Abono, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Total, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFechaFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Importe, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.fechaPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbformaPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Saldo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents upb As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents UltraLabel6 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Label11 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Abono As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Total As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents txtUUID As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtFechaFactura As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents txtSerie As TextBox
    Friend WithEvents txtFactura As TextBox
    Friend WithEvents lblFechaEmision As Label
    Friend WithEvents lblSerieFactura As Label
    Friend WithEvents lblNumFactura As Label
    Friend WithEvents btnAplicarPago As Button
    Friend WithEvents Importe As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
    Friend WithEvents Label3 As Label
    Friend WithEvents UltraLabel7 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents fechaPago As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents lblEmail As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents txtRazonSocial As TextBox
    Friend WithEvents lblrfc As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents lblrasoncial As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents txtRfc As TextBox
    Friend WithEvents lblEmpresa As Label
    Friend WithEvents txtNumCuenta As TextBox
    Friend WithEvents lblNumCuenta As Label
    Friend WithEvents grbformaPago As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents lblMetodoPago As Label
    Friend WithEvents Saldo As Infragistics.Win.UltraWinEditors.UltraCurrencyEditor
End Class
