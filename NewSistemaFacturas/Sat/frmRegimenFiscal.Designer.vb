<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRegimenFiscal
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRegimenFiscal))
        Dim Appearance31 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance32 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance33 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance34 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance35 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance36 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance37 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance38 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance59 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance60 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance61 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance62 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance63 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance64 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance65 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance66 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim ScrollBarLook1 As Infragistics.Win.UltraWinScrollBar.ScrollBarLook = New Infragistics.Win.UltraWinScrollBar.ScrollBarLook()
        Dim Appearance67 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance68 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance69 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance70 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance80 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.toolMenu = New System.Windows.Forms.ToolStrip()
        Me.btnGuardar = New System.Windows.Forms.ToolStripButton()
        Me.btnSalir = New System.Windows.Forms.ToolStripButton()
        Me.txtRegimen = New System.Windows.Forms.TextBox()
        Me.UgrRF = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.toolMenu.SuspendLayout()
        CType(Me.UgrRF, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'toolMenu
        '
        Me.toolMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.toolMenu.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.toolMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnGuardar, Me.btnSalir})
        Me.toolMenu.Location = New System.Drawing.Point(0, 477)
        Me.toolMenu.Name = "toolMenu"
        Me.toolMenu.Size = New System.Drawing.Size(533, 54)
        Me.toolMenu.TabIndex = 41
        Me.toolMenu.Text = "ToolStrip2"
        '
        'btnGuardar
        '
        Me.btnGuardar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.btnGuardar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGuardar.ForeColor = System.Drawing.Color.Black
        Me.btnGuardar.Image = CType(resources.GetObject("btnGuardar.Image"), System.Drawing.Image)
        Me.btnGuardar.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.btnGuardar.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(56, 51)
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'btnSalir
        '
        Me.btnSalir.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnSalir.ForeColor = System.Drawing.Color.Black
        Me.btnSalir.Image = CType(resources.GetObject("btnSalir.Image"), System.Drawing.Image)
        Me.btnSalir.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(35, 51)
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'txtRegimen
        '
        Me.txtRegimen.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtRegimen.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRegimen.ForeColor = System.Drawing.Color.Maroon
        Me.txtRegimen.Location = New System.Drawing.Point(0, 451)
        Me.txtRegimen.Name = "txtRegimen"
        Me.txtRegimen.ReadOnly = True
        Me.txtRegimen.Size = New System.Drawing.Size(533, 26)
        Me.txtRegimen.TabIndex = 43
        Me.txtRegimen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'UgrRF
        '
        Appearance31.BackColor = System.Drawing.Color.White
        Appearance31.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(221, Byte), Integer), CType(CType(221, Byte), Integer))
        Appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance31.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.UgrRF.DisplayLayout.AddNewBox.Appearance = Appearance31
        Me.UgrRF.DisplayLayout.AddNewBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.None
        Appearance32.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance32.ImageBackground = CType(resources.GetObject("Appearance32.ImageBackground"), System.Drawing.Image)
        Appearance32.ImageBackgroundAlpha = Infragistics.Win.Alpha.UseAlphaLevel
        Appearance32.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(6, 3, 6, 3)
        Appearance32.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.UgrRF.DisplayLayout.AddNewBox.ButtonAppearance = Appearance32
        Me.UgrRF.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.UgrRF.DisplayLayout.AddNewBox.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless
        Appearance33.BackColor = System.Drawing.Color.SteelBlue
        Appearance33.BackColor2 = System.Drawing.Color.Cyan
        Appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Me.UgrRF.DisplayLayout.Appearance = Appearance33
        Me.UgrRF.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Appearance34.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance34.FontData.Name = "Trebuchet MS"
        Appearance34.FontData.SizeInPoints = 8.0!
        Appearance34.ForeColor = System.Drawing.Color.White
        Appearance34.TextHAlignAsString = "Left"
        Me.UgrRF.DisplayLayout.CaptionAppearance = Appearance34
        Me.UgrRF.DisplayLayout.FixedHeaderOffImage = CType(resources.GetObject("UgrRF.DisplayLayout.FixedHeaderOffImage"), System.Drawing.Image)
        Me.UgrRF.DisplayLayout.FixedHeaderOnImage = CType(resources.GetObject("UgrRF.DisplayLayout.FixedHeaderOnImage"), System.Drawing.Image)
        Me.UgrRF.DisplayLayout.FixedRowOffImage = CType(resources.GetObject("UgrRF.DisplayLayout.FixedRowOffImage"), System.Drawing.Image)
        Me.UgrRF.DisplayLayout.FixedRowOnImage = CType(resources.GetObject("UgrRF.DisplayLayout.FixedRowOnImage"), System.Drawing.Image)
        Appearance35.FontData.BoldAsString = "True"
        Appearance35.FontData.Name = "Trebuchet MS"
        Appearance35.FontData.SizeInPoints = 10.0!
        Appearance35.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(127, Byte), Integer), CType(CType(177, Byte), Integer))
        Appearance35.ImageBackground = CType(resources.GetObject("Appearance35.ImageBackground"), System.Drawing.Image)
        Appearance35.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(0, 2, 0, 3)
        Appearance35.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.UgrRF.DisplayLayout.GroupByBox.Appearance = Appearance35
        Me.UgrRF.DisplayLayout.GroupByBox.ButtonBorderStyle = Infragistics.Win.UIElementBorderStyle.None
        Me.UgrRF.DisplayLayout.GroupByBox.Hidden = True
        Me.UgrRF.DisplayLayout.MaxColScrollRegions = 1
        Me.UgrRF.DisplayLayout.MaxRowScrollRegions = 1
        Me.UgrRF.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid
        Me.UgrRF.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None
        Me.UgrRF.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None
        Me.UgrRF.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless
        Appearance36.BackColor = System.Drawing.Color.Transparent
        Me.UgrRF.DisplayLayout.Override.CardAreaAppearance = Appearance36
        Appearance37.BorderColor = System.Drawing.Color.Transparent
        Appearance37.FontData.Name = "Verdana"
        Me.UgrRF.DisplayLayout.Override.CellAppearance = Appearance37
        Appearance38.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance38.ImageBackground = CType(resources.GetObject("Appearance38.ImageBackground"), System.Drawing.Image)
        Appearance38.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(6, 3, 6, 3)
        Appearance38.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.UgrRF.DisplayLayout.Override.CellButtonAppearance = Appearance38
        Me.UgrRF.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.UgrRF.DisplayLayout.Override.CellPadding = 2
        Appearance59.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.UgrRF.DisplayLayout.Override.FilterCellAppearance = Appearance59
        Appearance60.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance60.ImageBackground = CType(resources.GetObject("Appearance60.ImageBackground"), System.Drawing.Image)
        Appearance60.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(6, 3, 6, 3)
        Me.UgrRF.DisplayLayout.Override.FilterClearButtonAppearance = Appearance60
        Appearance61.BackColor = System.Drawing.Color.FromArgb(CType(CType(251, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(251, Byte), Integer))
        Appearance61.BackColorAlpha = Infragistics.Win.Alpha.Opaque
        Me.UgrRF.DisplayLayout.Override.FilterRowPromptAppearance = Appearance61
        Me.UgrRF.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
        Appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.None
        Appearance62.FontData.BoldAsString = "True"
        Appearance62.FontData.Name = "Trebuchet MS"
        Appearance62.FontData.SizeInPoints = 7.5!
        Appearance62.ForeColor = System.Drawing.Color.FromArgb(CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance62.ImageBackground = CType(resources.GetObject("Appearance62.ImageBackground"), System.Drawing.Image)
        Appearance62.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled
        Appearance62.TextHAlignAsString = "Left"
        Appearance62.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent
        Me.UgrRF.DisplayLayout.Override.HeaderAppearance = Appearance62
        Me.UgrRF.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.UgrRF.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed
        Appearance63.BorderColor = System.Drawing.Color.Transparent
        Me.UgrRF.DisplayLayout.Override.RowAppearance = Appearance63
        Appearance64.BackColor = System.Drawing.Color.White
        Me.UgrRF.DisplayLayout.Override.RowSelectorAppearance = Appearance64
        Appearance65.BorderColor = System.Drawing.Color.Transparent
        Appearance65.ForeColor = System.Drawing.Color.Black
        Me.UgrRF.DisplayLayout.Override.SelectedCellAppearance = Appearance65
        Appearance66.BackColor = System.Drawing.Color.White
        Appearance66.BackColor2 = System.Drawing.Color.Yellow
        Appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance66.BorderColor = System.Drawing.Color.Transparent
        Appearance66.FontData.BoldAsString = "True"
        Appearance66.ForeColor = System.Drawing.Color.Black
        Appearance66.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(1, 1, 1, 4)
        Appearance66.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.UgrRF.DisplayLayout.Override.SelectedRowAppearance = Appearance66
        Appearance67.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(2, 4, 2, 4)
        Appearance67.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        ScrollBarLook1.Appearance = Appearance67
        Appearance68.ImageBackground = CType(resources.GetObject("Appearance68.ImageBackground"), System.Drawing.Image)
        Appearance68.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(3, 2, 3, 2)
        ScrollBarLook1.AppearanceHorizontal = Appearance68
        Appearance69.ImageBackground = CType(resources.GetObject("Appearance69.ImageBackground"), System.Drawing.Image)
        Appearance69.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(2, 3, 2, 3)
        Appearance69.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        ScrollBarLook1.AppearanceVertical = Appearance69
        Appearance70.ImageBackground = CType(resources.GetObject("Appearance70.ImageBackground"), System.Drawing.Image)
        Appearance70.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(0, 2, 0, 1)
        ScrollBarLook1.TrackAppearanceHorizontal = Appearance70
        Appearance80.ImageBackground = CType(resources.GetObject("Appearance80.ImageBackground"), System.Drawing.Image)
        Appearance80.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(2, 0, 1, 0)
        Appearance80.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        ScrollBarLook1.TrackAppearanceVertical = Appearance80
        Me.UgrRF.DisplayLayout.ScrollBarLook = ScrollBarLook1
        Me.UgrRF.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.UgrRF.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.UgrRF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.UgrRF.Font = New System.Drawing.Font("Times New Roman", 8.0!)
        Me.UgrRF.Location = New System.Drawing.Point(0, 0)
        Me.UgrRF.Name = "UgrRF"
        Me.UgrRF.Size = New System.Drawing.Size(533, 451)
        Me.UgrRF.TabIndex = 44
        Me.UgrRF.Text = "Regimen Fiscal"
        Me.UgrRF.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'frmRegimenFiscal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(533, 531)
        Me.Controls.Add(Me.UgrRF)
        Me.Controls.Add(Me.txtRegimen)
        Me.Controls.Add(Me.toolMenu)
        Me.Name = "frmRegimenFiscal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Regimen Fiscal"
        Me.toolMenu.ResumeLayout(False)
        Me.toolMenu.PerformLayout()
        CType(Me.UgrRF, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents toolMenu As ToolStrip
    Friend WithEvents btnGuardar As ToolStripButton
    Friend WithEvents btnSalir As ToolStripButton
    Friend WithEvents txtRegimen As TextBox
    Friend WithEvents UgrRF As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
