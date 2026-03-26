<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmConexiones
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblPerfil = New System.Windows.Forms.Label()
        Me.cmbNombre = New System.Windows.Forms.ComboBox()
        Me.lblServidor = New System.Windows.Forms.Label()
        Me.txtServidor = New System.Windows.Forms.TextBox()
        Me.lblBaseDatos = New System.Windows.Forms.Label()
        Me.txtBaseDatos = New System.Windows.Forms.TextBox()
        Me.chkIntegrada = New System.Windows.Forms.CheckBox()
        Me.lblUsuario = New System.Windows.Forms.Label()
        Me.txtUsuario = New System.Windows.Forms.TextBox()
        Me.lblClaveSql = New System.Windows.Forms.Label()
        Me.txtClaveSql = New System.Windows.Forms.TextBox()
        Me.chkTrust = New System.Windows.Forms.CheckBox()
        Me.btnProbar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblPerfil
        '
        Me.lblPerfil.AutoSize = True
        Me.lblPerfil.Location = New System.Drawing.Point(20, 18)
        Me.lblPerfil.Name = "lblPerfil"
        Me.lblPerfil.Size = New System.Drawing.Size(82, 13)
        Me.lblPerfil.TabIndex = 0
        Me.lblPerfil.Text = "Cadena a editar"
        '
        'cmbNombre
        '
        Me.cmbNombre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNombre.FormattingEnabled = True
        Me.cmbNombre.Location = New System.Drawing.Point(20, 34)
        Me.cmbNombre.Name = "cmbNombre"
        Me.cmbNombre.Size = New System.Drawing.Size(448, 21)
        Me.cmbNombre.TabIndex = 1
        '
        'lblServidor
        '
        Me.lblServidor.AutoSize = True
        Me.lblServidor.Location = New System.Drawing.Point(20, 72)
        Me.lblServidor.Name = "lblServidor"
        Me.lblServidor.Size = New System.Drawing.Size(46, 13)
        Me.lblServidor.TabIndex = 2
        Me.lblServidor.Text = "Servidor"
        '
        'txtServidor
        '
        Me.txtServidor.Location = New System.Drawing.Point(120, 69)
        Me.txtServidor.Name = "txtServidor"
        Me.txtServidor.Size = New System.Drawing.Size(348, 20)
        Me.txtServidor.TabIndex = 3
        '
        'lblBaseDatos
        '
        Me.lblBaseDatos.AutoSize = True
        Me.lblBaseDatos.Location = New System.Drawing.Point(20, 98)
        Me.lblBaseDatos.Name = "lblBaseDatos"
        Me.lblBaseDatos.Size = New System.Drawing.Size(75, 13)
        Me.lblBaseDatos.TabIndex = 4
        Me.lblBaseDatos.Text = "Base de datos"
        '
        'txtBaseDatos
        '
        Me.txtBaseDatos.Location = New System.Drawing.Point(120, 95)
        Me.txtBaseDatos.Name = "txtBaseDatos"
        Me.txtBaseDatos.Size = New System.Drawing.Size(348, 20)
        Me.txtBaseDatos.TabIndex = 5
        '
        'chkIntegrada
        '
        Me.chkIntegrada.AutoSize = True
        Me.chkIntegrada.Location = New System.Drawing.Point(120, 124)
        Me.chkIntegrada.Name = "chkIntegrada"
        Me.chkIntegrada.Size = New System.Drawing.Size(138, 17)
        Me.chkIntegrada.TabIndex = 6
        Me.chkIntegrada.Text = "Autenticación integrada"
        Me.chkIntegrada.UseVisualStyleBackColor = True
        '
        'lblUsuario
        '
        Me.lblUsuario.AutoSize = True
        Me.lblUsuario.Location = New System.Drawing.Point(20, 154)
        Me.lblUsuario.Name = "lblUsuario"
        Me.lblUsuario.Size = New System.Drawing.Size(43, 13)
        Me.lblUsuario.TabIndex = 7
        Me.lblUsuario.Text = "Usuario"
        '
        'txtUsuario
        '
        Me.txtUsuario.Location = New System.Drawing.Point(120, 151)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(348, 20)
        Me.txtUsuario.TabIndex = 8
        '
        'lblClaveSql
        '
        Me.lblClaveSql.AutoSize = True
        Me.lblClaveSql.Location = New System.Drawing.Point(20, 180)
        Me.lblClaveSql.Name = "lblClaveSql"
        Me.lblClaveSql.Size = New System.Drawing.Size(61, 13)
        Me.lblClaveSql.TabIndex = 9
        Me.lblClaveSql.Text = "Contraseña"
        '
        'txtClaveSql
        '
        Me.txtClaveSql.Location = New System.Drawing.Point(120, 177)
        Me.txtClaveSql.Name = "txtClaveSql"
        Me.txtClaveSql.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.txtClaveSql.Size = New System.Drawing.Size(348, 20)
        Me.txtClaveSql.TabIndex = 10
        '
        'chkTrust
        '
        Me.chkTrust.AutoSize = True
        Me.chkTrust.Location = New System.Drawing.Point(120, 206)
        Me.chkTrust.Name = "chkTrust"
        Me.chkTrust.Size = New System.Drawing.Size(294, 17)
        Me.chkTrust.TabIndex = 11
        Me.chkTrust.Text = "Confiar en certificado del servidor (TrustServerCertificate)"
        Me.chkTrust.UseVisualStyleBackColor = True
        '
        'btnProbar
        '
        Me.btnProbar.Location = New System.Drawing.Point(120, 244)
        Me.btnProbar.Name = "btnProbar"
        Me.btnProbar.Size = New System.Drawing.Size(100, 28)
        Me.btnProbar.TabIndex = 12
        Me.btnProbar.Text = "Probar"
        Me.btnProbar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(244, 244)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(100, 28)
        Me.btnGuardar.TabIndex = 13
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Location = New System.Drawing.Point(368, 244)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(100, 28)
        Me.btnCerrar.TabIndex = 14
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'FrmConexiones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(492, 292)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.btnProbar)
        Me.Controls.Add(Me.chkTrust)
        Me.Controls.Add(Me.txtClaveSql)
        Me.Controls.Add(Me.lblClaveSql)
        Me.Controls.Add(Me.txtUsuario)
        Me.Controls.Add(Me.lblUsuario)
        Me.Controls.Add(Me.chkIntegrada)
        Me.Controls.Add(Me.txtBaseDatos)
        Me.Controls.Add(Me.lblBaseDatos)
        Me.Controls.Add(Me.txtServidor)
        Me.Controls.Add(Me.lblServidor)
        Me.Controls.Add(Me.cmbNombre)
        Me.Controls.Add(Me.lblPerfil)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FrmConexiones"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de conexiones"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblPerfil As Label
    Friend WithEvents cmbNombre As ComboBox
    Friend WithEvents lblServidor As Label
    Friend WithEvents txtServidor As TextBox
    Friend WithEvents lblBaseDatos As Label
    Friend WithEvents txtBaseDatos As TextBox
    Friend WithEvents chkIntegrada As CheckBox
    Friend WithEvents lblUsuario As Label
    Friend WithEvents txtUsuario As TextBox
    Friend WithEvents lblClaveSql As Label
    Friend WithEvents txtClaveSql As TextBox
    Friend WithEvents chkTrust As CheckBox
    Friend WithEvents btnProbar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCerrar As Button
End Class
