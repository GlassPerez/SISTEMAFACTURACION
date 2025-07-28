<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBorraVentas
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
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance47 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.UltraLabel8 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel1 = New Infragistics.Win.Misc.UltraLabel()
        Me.USERtxt = New System.Windows.Forms.TextBox()
        Me.PAWStxt = New System.Windows.Forms.TextBox()
        Me.upb = New Infragistics.Win.UltraWinProgressBar.UltraProgressBar()
        Me.txtFechaVenta = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor()
        Me.btnBorra = New System.Windows.Forms.Button()
        Me.btnEntra = New System.Windows.Forms.Button()
        Me.UltraLabel2 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel3 = New Infragistics.Win.Misc.UltraLabel()
        Me.UltraLabel4 = New Infragistics.Win.Misc.UltraLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.txtFechaVenta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'UltraLabel8
        '
        Appearance4.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel8.Appearance = Appearance4
        Me.UltraLabel8.AutoSize = True
        Me.UltraLabel8.Location = New System.Drawing.Point(31, 67)
        Me.UltraLabel8.Name = "UltraLabel8"
        Me.UltraLabel8.Size = New System.Drawing.Size(59, 14)
        Me.UltraLabel8.TabIndex = 64
        Me.UltraLabel8.Text = "USUARIO:"
        '
        'UltraLabel1
        '
        Appearance9.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel1.Appearance = Appearance9
        Me.UltraLabel1.AutoSize = True
        Me.UltraLabel1.Location = New System.Drawing.Point(226, 67)
        Me.UltraLabel1.Name = "UltraLabel1"
        Me.UltraLabel1.Size = New System.Drawing.Size(86, 14)
        Me.UltraLabel1.TabIndex = 65
        Me.UltraLabel1.Text = "CONTRASEÑA:"
        '
        'USERtxt
        '
        Me.USERtxt.Location = New System.Drawing.Point(96, 64)
        Me.USERtxt.MaxLength = 7
        Me.USERtxt.Name = "USERtxt"
        Me.USERtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.USERtxt.Size = New System.Drawing.Size(100, 20)
        Me.USERtxt.TabIndex = 66
        '
        'PAWStxt
        '
        Me.PAWStxt.Location = New System.Drawing.Point(318, 61)
        Me.PAWStxt.MaxLength = 5
        Me.PAWStxt.Name = "PAWStxt"
        Me.PAWStxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PAWStxt.Size = New System.Drawing.Size(100, 20)
        Me.PAWStxt.TabIndex = 67
        '
        'upb
        '
        Appearance47.BackColorAlpha = Infragistics.Win.Alpha.Transparent
        Appearance47.FontData.BoldAsString = "True"
        Appearance47.ForeColor = System.Drawing.Color.Black
        Me.upb.Appearance = Appearance47
        Me.upb.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.upb.Location = New System.Drawing.Point(0, 256)
        Me.upb.Maximum = 0
        Me.upb.Name = "upb"
        Me.upb.Size = New System.Drawing.Size(460, 40)
        Me.upb.TabIndex = 69
        Me.upb.Text = "[Value]"
        '
        'txtFechaVenta
        '
        Me.txtFechaVenta.DateTime = New Date(2013, 12, 3, 0, 0, 0, 0)
        Me.txtFechaVenta.Location = New System.Drawing.Point(101, 180)
        Me.txtFechaVenta.Name = "txtFechaVenta"
        Me.txtFechaVenta.Size = New System.Drawing.Size(113, 21)
        Me.txtFechaVenta.TabIndex = 70
        Me.txtFechaVenta.Value = New Date(2013, 12, 3, 0, 0, 0, 0)
        '
        'btnBorra
        '
        Me.btnBorra.BackColor = System.Drawing.Color.Black
        Me.btnBorra.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBorra.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBorra.ForeColor = System.Drawing.Color.White
        Me.btnBorra.Location = New System.Drawing.Point(280, 174)
        Me.btnBorra.Name = "btnBorra"
        Me.btnBorra.Size = New System.Drawing.Size(138, 32)
        Me.btnBorra.TabIndex = 71
        Me.btnBorra.Text = "BORRAR VENTA"
        Me.btnBorra.UseVisualStyleBackColor = False
        '
        'btnEntra
        '
        Me.btnEntra.BackColor = System.Drawing.Color.Black
        Me.btnEntra.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEntra.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEntra.ForeColor = System.Drawing.Color.White
        Me.btnEntra.Location = New System.Drawing.Point(31, 90)
        Me.btnEntra.Name = "btnEntra"
        Me.btnEntra.Size = New System.Drawing.Size(387, 32)
        Me.btnEntra.TabIndex = 72
        Me.btnEntra.Text = "CONFIRMAR"
        Me.btnEntra.UseVisualStyleBackColor = False
        '
        'UltraLabel2
        '
        Appearance1.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel2.Appearance = Appearance1
        Me.UltraLabel2.AutoSize = True
        Me.UltraLabel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel2.Location = New System.Drawing.Point(31, 137)
        Me.UltraLabel2.Name = "UltraLabel2"
        Me.UltraLabel2.Size = New System.Drawing.Size(392, 17)
        Me.UltraLabel2.TabIndex = 73
        Me.UltraLabel2.Text = "SELECCIONA LA FECHA DE VENTAS QUE DESEA BORRAR"
        '
        'UltraLabel3
        '
        Appearance2.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel3.Appearance = Appearance2
        Me.UltraLabel3.AutoSize = True
        Me.UltraLabel3.Location = New System.Drawing.Point(36, 184)
        Me.UltraLabel3.Name = "UltraLabel3"
        Me.UltraLabel3.Size = New System.Drawing.Size(46, 14)
        Me.UltraLabel3.TabIndex = 74
        Me.UltraLabel3.Text = "FECHA:"
        '
        'UltraLabel4
        '
        Appearance5.BackColor = System.Drawing.Color.Transparent
        Me.UltraLabel4.Appearance = Appearance5
        Me.UltraLabel4.AutoSize = True
        Me.UltraLabel4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UltraLabel4.Location = New System.Drawing.Point(31, 23)
        Me.UltraLabel4.Name = "UltraLabel4"
        Me.UltraLabel4.Size = New System.Drawing.Size(60, 17)
        Me.UltraLabel4.TabIndex = 75
        Me.UltraLabel4.Text = "TIENDA:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(93, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(0, 16)
        Me.Label1.TabIndex = 77
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(223, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(0, 16)
        Me.Label2.TabIndex = 78
        '
        'frmBorraVentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 296)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.UltraLabel4)
        Me.Controls.Add(Me.UltraLabel3)
        Me.Controls.Add(Me.UltraLabel2)
        Me.Controls.Add(Me.btnEntra)
        Me.Controls.Add(Me.btnBorra)
        Me.Controls.Add(Me.txtFechaVenta)
        Me.Controls.Add(Me.upb)
        Me.Controls.Add(Me.PAWStxt)
        Me.Controls.Add(Me.USERtxt)
        Me.Controls.Add(Me.UltraLabel1)
        Me.Controls.Add(Me.UltraLabel8)
        Me.Name = "frmBorraVentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Borra Ventas Cargadas"
        CType(Me.txtFechaVenta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents UltraLabel8 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel1 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents USERtxt As TextBox
    Friend WithEvents PAWStxt As TextBox
    Friend WithEvents upb As Infragistics.Win.UltraWinProgressBar.UltraProgressBar
    Friend WithEvents txtFechaVenta As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents btnBorra As Button
    Friend WithEvents btnEntra As Button
    Friend WithEvents UltraLabel2 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel3 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents UltraLabel4 As Infragistics.Win.Misc.UltraLabel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
