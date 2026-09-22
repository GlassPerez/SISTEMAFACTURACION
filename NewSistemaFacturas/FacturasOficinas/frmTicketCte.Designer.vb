<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTicketCte
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
        Dim Appearance42 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.grbSucursal = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.grbTienda = New Infragistics.Win.UltraWinGrid.UltraCombo()
        Me.txtFechaVenta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.Ticket = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.grbSucursal, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grbTienda, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtFechaVenta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(37, 118)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tienda:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(40, 158)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Sucursal:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(40, 195)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Fecha de Venta:"
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(40, 235)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(105, 16)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "No. de Ticket:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
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
        Appearance29.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance29.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance29.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance29.BorderColor = System.Drawing.SystemColors.Window
        Me.grbSucursal.DisplayLayout.GroupByBox.Appearance = Appearance29
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
        Me.grbSucursal.Location = New System.Drawing.Point(132, 152)
        Me.grbSucursal.Name = "grbSucursal"
        Me.grbSucursal.ReadOnly = True
        Me.grbSucursal.Size = New System.Drawing.Size(147, 22)
        Me.grbSucursal.TabIndex = 78
        '
        'grbTienda
        '
        Me.grbTienda.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
        Appearance14.BackColor = System.Drawing.SystemColors.Window
        Appearance14.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.grbTienda.DisplayLayout.Appearance = Appearance14
        Me.grbTienda.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Me.grbTienda.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.grbTienda.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance27.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance27.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance27.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance27.BorderColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.GroupByBox.Appearance = Appearance27
        Appearance15.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbTienda.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance15
        Me.grbTienda.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance17.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance17.BackColor2 = System.Drawing.SystemColors.Control
        Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance17.ForeColor = System.Drawing.SystemColors.GrayText
        Me.grbTienda.DisplayLayout.GroupByBox.PromptAppearance = Appearance17
        Me.grbTienda.DisplayLayout.MaxColScrollRegions = 1
        Me.grbTienda.DisplayLayout.MaxRowScrollRegions = 1
        Appearance20.BackColor = System.Drawing.SystemColors.Window
        Appearance20.ForeColor = System.Drawing.SystemColors.ControlText
        Me.grbTienda.DisplayLayout.Override.ActiveCellAppearance = Appearance20
        Appearance21.BackColor = System.Drawing.SystemColors.Highlight
        Appearance21.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.grbTienda.DisplayLayout.Override.ActiveRowAppearance = Appearance21
        Me.grbTienda.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.grbTienda.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance22.BackColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.Override.CardAreaAppearance = Appearance22
        Appearance23.BorderColor = System.Drawing.Color.Silver
        Appearance23.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.grbTienda.DisplayLayout.Override.CellAppearance = Appearance23
        Me.grbTienda.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.grbTienda.DisplayLayout.Override.CellPadding = 0
        Appearance24.BackColor = System.Drawing.SystemColors.Control
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        Me.grbTienda.DisplayLayout.Override.GroupByRowAppearance = Appearance24
        Appearance25.TextHAlignAsString = "Left"
        Me.grbTienda.DisplayLayout.Override.HeaderAppearance = Appearance25
        Me.grbTienda.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.grbTienda.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance26.BackColor = System.Drawing.SystemColors.Window
        Appearance26.BorderColor = System.Drawing.Color.Silver
        Me.grbTienda.DisplayLayout.Override.RowAppearance = Appearance26
        Me.grbTienda.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        Me.grbTienda.DisplayLayout.Override.TemplateAddRowAppearance = Appearance28
        Me.grbTienda.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.grbTienda.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.grbTienda.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.grbTienda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grbTienda.Location = New System.Drawing.Point(132, 112)
        Me.grbTienda.Name = "grbTienda"
        Me.grbTienda.ReadOnly = True
        Me.grbTienda.Size = New System.Drawing.Size(147, 22)
        Me.grbTienda.TabIndex = 77
        '
        'txtFechaVenta
        '
        Me.txtFechaVenta.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.txtFechaVenta.Location = New System.Drawing.Point(167, 190)
        Me.txtFechaVenta.Name = "txtFechaVenta"
        Me.txtFechaVenta.ReadOnly = True
        Me.txtFechaVenta.Size = New System.Drawing.Size(113, 21)
        Me.txtFechaVenta.TabIndex = 85
        Me.txtFechaVenta.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'Ticket
        '
        Me.Ticket.Location = New System.Drawing.Point(180, 231)
        Me.Ticket.Name = "Ticket"
        Me.Ticket.Size = New System.Drawing.Size(100, 20)
        Me.Ticket.TabIndex = 86
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Black
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(43, 270)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(237, 33)
        Me.Button1.TabIndex = 87
        Me.Button1.Text = "Agregar Ticket de Factura"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label5.Location = New System.Drawing.Point(63, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(195, 18)
        Me.Label5.TabIndex = 88
        Me.Label5.Text = "Ingresa aqui uno por uno"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label6.Location = New System.Drawing.Point(63, 36)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(180, 18)
        Me.Label6.TabIndex = 89
        Me.Label6.Text = "los folios de las notas "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.DarkBlue
        Me.Label7.Location = New System.Drawing.Point(40, 63)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(249, 18)
        Me.Label7.TabIndex = 90
        Me.Label7.Text = "que fueron facturadas a clientes"
        '
        'frmTicketCte
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(329, 333)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Ticket)
        Me.Controls.Add(Me.txtFechaVenta)
        Me.Controls.Add(Me.grbSucursal)
        Me.Controls.Add(Me.grbTienda)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmTicketCte"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Factura de Cliente"
        CType(Me.grbSucursal, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grbTienda, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtFechaVenta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents grbSucursal As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents grbTienda As Infragistics.Win.UltraWinGrid.UltraCombo
    Friend WithEvents txtFechaVenta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents Ticket As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
End Class
