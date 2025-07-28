<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CancelaFacturasXArchivo
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
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance54 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance39 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance40 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance41 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance50 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance45 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance44 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance43 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance49 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance48 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance46 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.btnGenerarFactura = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cboSheet = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.upb = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel8 = New Infragistics.Win.Misc.UltraLabel()
        Me.grbSucursal = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.grbTienda = New Infragistics.Win.UltraWinGrid.UltraCombo()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbSucursal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbTienda, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnGenerarFactura
        '
        Me.btnGenerarFactura.Location = New System.Drawing.Point(768, 125)
        Me.btnGenerarFactura.Name = "btnGenerarFactura"
        Me.btnGenerarFactura.Size = New System.Drawing.Size(153, 46)
        Me.btnGenerarFactura.TabIndex = 16
        Me.btnGenerarFactura.Text = "Timbrar"
        Me.btnGenerarFactura.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(296, 125)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Dirección del archivo"
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(12, 192)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(909, 299)
        Me.DataGridView1.TabIndex = 14
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(141, 150)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(423, 20)
        Me.TextBox1.TabIndex = 13
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 125)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(114, 46)
        Me.Button1.TabIndex = 12
        Me.Button1.Text = "Busca archivo"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cboSheet
        '
        Me.cboSheet.FormattingEnabled = True
        Me.cboSheet.Location = New System.Drawing.Point(594, 150)
        Me.cboSheet.Name = "cboSheet"
        Me.cboSheet.Size = New System.Drawing.Size(151, 21)
        Me.cboSheet.TabIndex = 18
        Me.cboSheet.Text = "Lee archivo excel .xls . xlsx "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(621, 125)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Nombre de la Hoja"
        '
        'upb
        '
        Appearance15.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance15.FontData.BoldAsString = "True"
        Appearance15.ForeColor = System.Drawing.Color.Black
        Me.upb.Appearance = Appearance15
        Me.upb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.upb.Location = New System.Drawing.Point(0, 519)
        Me.upb.Maximum = 0
        Me.upb.Name = "upb"
        Me.upb.Size = New System.Drawing.Size(943, 46)
        Me.upb.TabIndex = 19
        Me.upb.Text = "[Value]"
        '
        'UltraLabel1
        '
        Appearance54.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel1.Appearance = Appearance54
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(345, 40)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(51, 14)
        Me.UltraLabel1.TabIndex = 66
        Me.UltraLabel1.Text = "Sucursal:"
        '
        'UltraLabel8
        '
        Appearance14.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel8.Appearance = Appearance14
        Me.UltraLabel8.AutoSize = True
        Me.UltraLabel8.Location = New System.Drawing.Point(7, 41)
        Me.UltraLabel8.Name = "UltraLabel8"
        Me.UltraLabel8.Size = New System.Drawing.Size(42, 14)
        Me.UltraLabel8.TabIndex = 65
        Me.UltraLabel8.Text = "Tienda:"
        '
        'grbSucursal
        '
        Me.grbSucursal.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Appearance42.BackColor = System.Drawing.SystemColors.Window
        Appearance42.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grbSucursal.DisplayLayout.Appearance = Appearance42
        Me.grbSucursal.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Me.grbSucursal.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grbSucursal.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance39.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance39.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance39.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance39.BorderColor = System.Drawing.SystemColors.Window
        Me.grbSucursal.DisplayLayout.GroupByBox.Appearance = Appearance39
        Appearance40.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbSucursal.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance40
        Me.grbSucursal.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance41.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance41.BackColor2 = System.Drawing.SystemColors.Control
        Appearance41.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance41.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbSucursal.DisplayLayout.GroupByBox.PromptAppearance = Appearance41
        Me.grbSucursal.DisplayLayout.MaxColScrollRegions = 1
        Me.grbSucursal.DisplayLayout.MaxRowScrollRegions = 1
        Appearance50.BackColor = System.Drawing.SystemColors.Window
        Appearance50.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grbSucursal.DisplayLayout.Override.ActiveCellAppearance = Appearance50
        Appearance45.BackColor = System.Drawing.SystemColors.Highlight
        Appearance45.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grbSucursal.DisplayLayout.Override.ActiveRowAppearance = Appearance45
        Me.grbSucursal.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grbSucursal.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance44.BackColor = System.Drawing.SystemColors.Window
        Me.grbSucursal.DisplayLayout.Override.CardAreaAppearance = Appearance44
        Appearance43.BorderColor = System.Drawing.Color.Silver
        Appearance43.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grbSucursal.DisplayLayout.Override.CellAppearance = Appearance43
        Me.grbSucursal.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grbSucursal.DisplayLayout.Override.CellPadding = 0
        Appearance47.BackColor = System.Drawing.SystemColors.Control
        Appearance47.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance47.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance47.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance47.BorderColor = System.Drawing.SystemColors.Window
        Me.grbSucursal.DisplayLayout.Override.GroupByRowAppearance = Appearance47
        Appearance49.TextHAlignAsString = "Left"
        Me.grbSucursal.DisplayLayout.Override.HeaderAppearance = Appearance49
        Me.grbSucursal.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grbSucursal.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance48.BackColor = System.Drawing.SystemColors.Window
        Appearance48.BorderColor = System.Drawing.Color.Silver
        Me.grbSucursal.DisplayLayout.Override.RowAppearance = Appearance48
        Me.grbSucursal.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance46.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grbSucursal.DisplayLayout.Override.TemplateAddRowAppearance = Appearance46
        Me.grbSucursal.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grbSucursal.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grbSucursal.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.grbSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbSucursal.Location = New System.Drawing.Point(398, 37)
        Me.grbSucursal.Name = "grbSucursal"
        Me.grbSucursal.Size = New System.Drawing.Size(210, 22)
        Me.grbSucursal.TabIndex = 64
        '
        'grbTienda
        '
        Me.grbTienda.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Appearance30.BackColor = System.Drawing.SystemColors.Window
        Appearance30.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grbTienda.DisplayLayout.Appearance = Appearance30
        Me.grbTienda.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Me.grbTienda.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grbTienda.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance27.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance27.BorderColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.GroupByBox.Appearance = Appearance27
        Appearance28.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbTienda.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance28
        Me.grbTienda.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance29.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance29.BackColor2 = System.Drawing.SystemColors.Control
        Appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance29.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbTienda.DisplayLayout.GroupByBox.PromptAppearance = Appearance29
        Me.grbTienda.DisplayLayout.MaxColScrollRegions = 1
        Me.grbTienda.DisplayLayout.MaxRowScrollRegions = 1
        Appearance4.BackColor = System.Drawing.SystemColors.Window
        Appearance4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grbTienda.DisplayLayout.Override.ActiveCellAppearance = Appearance4
        Appearance5.BackColor = System.Drawing.SystemColors.Highlight
        Appearance5.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grbTienda.DisplayLayout.Override.ActiveRowAppearance = Appearance5
        Me.grbTienda.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grbTienda.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.Override.CardAreaAppearance = Appearance6
        Appearance7.BorderColor = System.Drawing.Color.Silver
        Appearance7.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grbTienda.DisplayLayout.Override.CellAppearance = Appearance7
        Me.grbTienda.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grbTienda.DisplayLayout.Override.CellPadding = 0
        Appearance8.BackColor = System.Drawing.SystemColors.Control
        Appearance8.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance8.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance8.BorderColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.Override.GroupByRowAppearance = Appearance8
        Appearance9.TextHAlignAsString = "Left"
        Me.grbTienda.DisplayLayout.Override.HeaderAppearance = Appearance9
        Me.grbTienda.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grbTienda.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance10.BackColor = System.Drawing.SystemColors.Window
        Appearance10.BorderColor = System.Drawing.Color.Silver
        Me.grbTienda.DisplayLayout.Override.RowAppearance = Appearance10
        Me.grbTienda.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance11.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grbTienda.DisplayLayout.Override.TemplateAddRowAppearance = Appearance11
        Me.grbTienda.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grbTienda.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grbTienda.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.grbTienda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbTienda.Location = New System.Drawing.Point(52, 37)
        Me.grbTienda.Name = "grbTienda"
        Me.grbTienda.Size = New System.Drawing.Size(267, 22)
        Me.grbTienda.TabIndex = 63
        '
        'CancelaFacturasXArchivo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(943, 565)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel8)
        Me.Controls.Add(Me.grbSucursal)
        Me.Controls.Add(Me.grbTienda)
        Me.Controls.Add(Me.upb)
        Me.Controls.Add(Me.cboSheet)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnGenerarFactura)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "CancelaFacturasXArchivo"
        Me.Text = "CancelaFacturasXArchivo"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbSucursal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbTienda, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnGenerarFactura As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents cboSheet As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents upb As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel8 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents grbSucursal As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents grbTienda As Infragistics.Win.UltraWinGrid.UltraCombo
End Class
