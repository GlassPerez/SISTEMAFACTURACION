<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPreparaInfo
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim EditorButton1 As Infragistics.Win.UltraWinEditors.EditorButton = New Infragistics.Win.UltraWinEditors.EditorButton()
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
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.FechaInicio = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.lblSucursal = New System.Windows.Forms.Label()
        Me.lblTienda = New System.Windows.Forms.Label()
        Me.grbSucursal = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.grbTienda = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.Upgbar = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.Panel1.SuspendLayout()
        CType(Me.FechaInicio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbSucursal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbTienda, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.SteelBlue
        Me.Panel1.Controls.Add(Me.FechaInicio)
        Me.Panel1.Controls.Add(Me.btnBuscar)
        Me.Panel1.Controls.Add(Me.lblSucursal)
        Me.Panel1.Controls.Add(Me.lblTienda)
        Me.Panel1.Controls.Add(Me.grbSucursal)
        Me.Panel1.Controls.Add(Me.grbTienda)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(663, 158)
        Me.Panel1.TabIndex = 12
        '
        'FechaInicio
        '
        EditorButton1.Text = "De la Fecha"
        Me.FechaInicio.ButtonsLeft.Add(EditorButton1)
        Me.FechaInicio.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.FechaInicio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaInicio.Location = New System.Drawing.Point(104, 65)
        Me.FechaInicio.Name = "FechaInicio"
        Me.FechaInicio.Size = New System.Drawing.Size(183, 21)
        Me.FechaInicio.TabIndex = 62
        Me.FechaInicio.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'btnBuscar
        '
        Me.btnBuscar.BackColor = System.Drawing.Color.Black
        Me.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscar.ForeColor = System.Drawing.Color.White
        Me.btnBuscar.Location = New System.Drawing.Point(386, 56)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(202, 30)
        Me.btnBuscar.TabIndex = 56
        Me.btnBuscar.Text = "PROCESAR"
        Me.btnBuscar.UseVisualStyleBackColor = False
        '
        'lblSucursal
        '
        Me.lblSucursal.AutoSize = True
        Me.lblSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSucursal.ForeColor = System.Drawing.Color.White
        Me.lblSucursal.Location = New System.Drawing.Point(308, 17)
        Me.lblSucursal.Name = "lblSucursal"
        Me.lblSucursal.Size = New System.Drawing.Size(72, 16)
        Me.lblSucursal.TabIndex = 3
        Me.lblSucursal.Text = "Sucursal:"
        '
        'lblTienda
        '
        Me.lblTienda.AutoSize = True
        Me.lblTienda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTienda.ForeColor = System.Drawing.Color.White
        Me.lblTienda.Location = New System.Drawing.Point(37, 17)
        Me.lblTienda.Name = "lblTienda"
        Me.lblTienda.Size = New System.Drawing.Size(61, 16)
        Me.lblTienda.TabIndex = 2
        Me.lblTienda.Text = "Tienda:"
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
        Me.grbSucursal.Location = New System.Drawing.Point(386, 11)
        Me.grbSucursal.Name = "grbSucursal"
        Me.grbSucursal.Size = New System.Drawing.Size(202, 22)
        Me.grbSucursal.TabIndex = 1
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
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grbTienda.DisplayLayout.Override.ActiveCellAppearance = Appearance1
        Appearance2.BackColor = System.Drawing.SystemColors.Highlight
        Appearance2.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grbTienda.DisplayLayout.Override.ActiveRowAppearance = Appearance2
        Me.grbTienda.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grbTienda.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance4.BackColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.Override.CardAreaAppearance = Appearance4
        Appearance5.BorderColor = System.Drawing.Color.Silver
        Appearance5.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grbTienda.DisplayLayout.Override.CellAppearance = Appearance5
        Me.grbTienda.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grbTienda.DisplayLayout.Override.CellPadding = 0
        Appearance6.BackColor = System.Drawing.SystemColors.Control
        Appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance6.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance6.BorderColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.Override.GroupByRowAppearance = Appearance6
        Appearance7.TextHAlignAsString = "Left"
        Me.grbTienda.DisplayLayout.Override.HeaderAppearance = Appearance7
        Me.grbTienda.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grbTienda.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance8.BackColor = System.Drawing.SystemColors.Window
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Me.grbTienda.DisplayLayout.Override.RowAppearance = Appearance8
        Me.grbTienda.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance9.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grbTienda.DisplayLayout.Override.TemplateAddRowAppearance = Appearance9
        Me.grbTienda.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grbTienda.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grbTienda.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.grbTienda.Location = New System.Drawing.Point(104, 11)
        Me.grbTienda.Name = "grbTienda"
        Me.grbTienda.Size = New System.Drawing.Size(183, 22)
        Me.grbTienda.TabIndex = 0
        '
        'Upgbar
        '
        Appearance17.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance17.FontData.BoldAsString = "True"
        Appearance17.ForeColor = System.Drawing.Color.Black
        Me.Upgbar.Appearance = Appearance17
        Me.Upgbar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Upgbar.Location = New System.Drawing.Point(0, 110)
        Me.Upgbar.Maximum = 0
        Me.Upgbar.Name = "Upgbar"
        Me.Upgbar.Size = New System.Drawing.Size(663, 52)
        Me.Upgbar.TabIndex = 13
        Me.Upgbar.Text = "[Value]"
        '
        'frmPreparaInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(663, 162)
        Me.Controls.Add(Me.Upgbar)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmPreparaInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ventas para facturar del día"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.FechaInicio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbSucursal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbTienda, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents lblSucursal As Label
    Friend WithEvents lblTienda As Label
    Friend WithEvents grbSucursal As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents grbTienda As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents Upgbar As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents FechaInicio As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents btnBuscar As Button
End Class
