<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUUIDBoscoDetalle
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUUIDBoscoDetalle))
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
        Me.txtUUID = New System.Windows.Forms.TextBox()
        Me.Ugr = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.toolMenu.SuspendLayout()
        CType(Me.Ugr, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'toolMenu
        '
        Me.toolMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.toolMenu.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.toolMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnGuardar, Me.btnSalir})
        Me.toolMenu.Location = New System.Drawing.Point(0, 527)
        Me.toolMenu.Name = "toolMenu"
        Me.toolMenu.Size = New System.Drawing.Size(544, 54)
        Me.toolMenu.TabIndex = 39
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
        'txtUUID
        '
        Me.txtUUID.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.txtUUID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUUID.ForeColor = System.Drawing.Color.Maroon
        Me.txtUUID.Location = New System.Drawing.Point(0, 501)
        Me.txtUUID.Name = "txtUUID"
        Me.txtUUID.ReadOnly = True
        Me.txtUUID.Size = New System.Drawing.Size(544, 26)
        Me.txtUUID.TabIndex = 41
        Me.txtUUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Ugr
        '
        Appearance31.BackColor = System.Drawing.Color.White
        Appearance31.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(221, Byte), Integer), CType(CType(221, Byte), Integer))
        Appearance31.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance31.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Ugr.DisplayLayout.AddNewBox.Appearance = Appearance31
        Me.Ugr.DisplayLayout.AddNewBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.None
        Appearance32.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance32.ImageBackground = CType(resources.GetObject("Appearance32.ImageBackground"), System.Drawing.Image)
        Appearance32.ImageBackgroundAlpha = Infragistics.Win.Alpha.UseAlphaLevel
        Appearance32.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(6, 3, 6, 3)
        Appearance32.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.Ugr.DisplayLayout.AddNewBox.ButtonAppearance = Appearance32
        Me.Ugr.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.Ugr.DisplayLayout.AddNewBox.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless
        Appearance33.BackColor = System.Drawing.Color.SteelBlue
        Appearance33.BackColor2 = System.Drawing.Color.Cyan
        Appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Me.Ugr.DisplayLayout.Appearance = Appearance33
        Me.Ugr.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn
        Appearance34.BackColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance34.FontData.Name = "Trebuchet MS"
        Appearance34.FontData.SizeInPoints = 8.0!
        Appearance34.ForeColor = System.Drawing.Color.White
        Appearance34.TextHAlignAsString = "Left"
        Me.Ugr.DisplayLayout.CaptionAppearance = Appearance34
        Me.Ugr.DisplayLayout.FixedHeaderOffImage = CType(resources.GetObject("Ugr.DisplayLayout.FixedHeaderOffImage"), System.Drawing.Image)
        Me.Ugr.DisplayLayout.FixedHeaderOnImage = CType(resources.GetObject("Ugr.DisplayLayout.FixedHeaderOnImage"), System.Drawing.Image)
        Me.Ugr.DisplayLayout.FixedRowOffImage = CType(resources.GetObject("Ugr.DisplayLayout.FixedRowOffImage"), System.Drawing.Image)
        Me.Ugr.DisplayLayout.FixedRowOnImage = CType(resources.GetObject("Ugr.DisplayLayout.FixedRowOnImage"), System.Drawing.Image)
        Appearance35.FontData.BoldAsString = "True"
        Appearance35.FontData.Name = "Trebuchet MS"
        Appearance35.FontData.SizeInPoints = 10.0!
        Appearance35.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(127, Byte), Integer), CType(CType(177, Byte), Integer))
        Appearance35.ImageBackground = CType(resources.GetObject("Appearance35.ImageBackground"), System.Drawing.Image)
        Appearance35.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(0, 2, 0, 3)
        Appearance35.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.Ugr.DisplayLayout.GroupByBox.Appearance = Appearance35
        Me.Ugr.DisplayLayout.GroupByBox.ButtonBorderStyle = Infragistics.Win.UIElementBorderStyle.None
        Me.Ugr.DisplayLayout.GroupByBox.Hidden = True
        Me.Ugr.DisplayLayout.MaxColScrollRegions = 1
        Me.Ugr.DisplayLayout.MaxRowScrollRegions = 1
        Me.Ugr.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid
        Me.Ugr.DisplayLayout.Override.BorderStyleHeader = Infragistics.Win.UIElementBorderStyle.None
        Me.Ugr.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None
        Me.Ugr.DisplayLayout.Override.ButtonStyle = Infragistics.Win.UIElementButtonStyle.FlatBorderless
        Appearance36.BackColor = System.Drawing.Color.Transparent
        Me.Ugr.DisplayLayout.Override.CardAreaAppearance = Appearance36
        Appearance37.BorderColor = System.Drawing.Color.Transparent
        Appearance37.FontData.Name = "Verdana"
        Me.Ugr.DisplayLayout.Override.CellAppearance = Appearance37
        Appearance38.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance38.ImageBackground = CType(resources.GetObject("Appearance38.ImageBackground"), System.Drawing.Image)
        Appearance38.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(6, 3, 6, 3)
        Appearance38.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.Ugr.DisplayLayout.Override.CellButtonAppearance = Appearance38
        Me.Ugr.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.Ugr.DisplayLayout.Override.CellPadding = 2
        Appearance59.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(251, Byte), Integer))
        Me.Ugr.DisplayLayout.Override.FilterCellAppearance = Appearance59
        Appearance60.ForeColor = System.Drawing.Color.FromArgb(CType(CType(21, Byte), Integer), CType(CType(27, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance60.ImageBackground = CType(resources.GetObject("Appearance60.ImageBackground"), System.Drawing.Image)
        Appearance60.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(6, 3, 6, 3)
        Me.Ugr.DisplayLayout.Override.FilterClearButtonAppearance = Appearance60
        Appearance61.BackColor = System.Drawing.Color.FromArgb(CType(CType(251, Byte), Integer), CType(CType(251, Byte), Integer), CType(CType(251, Byte), Integer))
        Appearance61.BackColorAlpha = Infragistics.Win.Alpha.Opaque
        Me.Ugr.DisplayLayout.Override.FilterRowPromptAppearance = Appearance61
        Me.Ugr.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow
        Appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.None
        Appearance62.FontData.BoldAsString = "True"
        Appearance62.FontData.Name = "Trebuchet MS"
        Appearance62.FontData.SizeInPoints = 7.5!
        Appearance62.ForeColor = System.Drawing.Color.FromArgb(CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(85, Byte), Integer))
        Appearance62.ImageBackground = CType(resources.GetObject("Appearance62.ImageBackground"), System.Drawing.Image)
        Appearance62.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Tiled
        Appearance62.TextHAlignAsString = "Left"
        Appearance62.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent
        Me.Ugr.DisplayLayout.Override.HeaderAppearance = Appearance62
        Me.Ugr.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me.Ugr.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed
        Appearance63.BorderColor = System.Drawing.Color.Transparent
        Me.Ugr.DisplayLayout.Override.RowAppearance = Appearance63
        Appearance64.BackColor = System.Drawing.Color.White
        Me.Ugr.DisplayLayout.Override.RowSelectorAppearance = Appearance64
        Appearance65.BorderColor = System.Drawing.Color.Transparent
        Appearance65.ForeColor = System.Drawing.Color.Black
        Me.Ugr.DisplayLayout.Override.SelectedCellAppearance = Appearance65
        Appearance66.BackColor = System.Drawing.Color.White
        Appearance66.BackColor2 = System.Drawing.Color.Yellow
        Appearance66.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance66.BorderColor = System.Drawing.Color.Transparent
        Appearance66.FontData.BoldAsString = "True"
        Appearance66.ForeColor = System.Drawing.Color.Black
        Appearance66.ImageBackgroundStretchMargins = New Infragistics.Win.ImageBackgroundStretchMargins(1, 1, 1, 4)
        Appearance66.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched
        Me.Ugr.DisplayLayout.Override.SelectedRowAppearance = Appearance66
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
        Me.Ugr.DisplayLayout.ScrollBarLook = ScrollBarLook1
        Me.Ugr.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.Ugr.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.Ugr.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Ugr.Font = New System.Drawing.Font("Times New Roman", 8.0!)
        Me.Ugr.Location = New System.Drawing.Point(0, 0)
        Me.Ugr.Name = "Ugr"
        Me.Ugr.Size = New System.Drawing.Size(544, 501)
        Me.Ugr.TabIndex = 42
        Me.Ugr.Text = "Lista de          UUID"
        Me.Ugr.UseOsThemes = Infragistics.Win.DefaultableBoolean.[False]
        '
        'frmUUIDBoscoDetalle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(544, 581)
        Me.Controls.Add(Me.Ugr)
        Me.Controls.Add(Me.txtUUID)
        Me.Controls.Add(Me.toolMenu)
        Me.Name = "frmUUIDBoscoDetalle"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UUID Bosco Detalle"
        Me.toolMenu.ResumeLayout(False)
        Me.toolMenu.PerformLayout()
        CType(Me.Ugr, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents toolMenu As System.Windows.Forms.ToolStrip
    Friend WithEvents btnGuardar As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSalir As System.Windows.Forms.ToolStripButton
    Friend WithEvents txtUUID As System.Windows.Forms.TextBox
    Friend WithEvents Ugr As Infragistics.Win.UltraWinGrid.UltraGrid
End Class
