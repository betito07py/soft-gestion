<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmProveedores
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
        Me.pnlBusqueda = New System.Windows.Forms.Panel()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.lblBusqueda = New System.Windows.Forms.Label()
        Me.pnlListaMaestro = New System.Windows.Forms.Panel()
        Me.lblTituloGrilla = New System.Windows.Forms.Label()
        Me.dgvProveedores = New System.Windows.Forms.DataGridView()
        Me.pnlEdicion = New System.Windows.Forms.Panel()
        Me.btnDesactivar = New System.Windows.Forms.Button()
        Me.btnActivar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.grpDatos = New System.Windows.Forms.GroupBox()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.lblObservaciones = New System.Windows.Forms.Label()
        Me.cmbCondicionPago = New System.Windows.Forms.ComboBox()
        Me.lblCondicionPago = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.txtTelefono = New System.Windows.Forms.TextBox()
        Me.lblTelefono = New System.Windows.Forms.Label()
        Me.txtDireccion = New System.Windows.Forms.TextBox()
        Me.lblDireccion = New System.Windows.Forms.Label()
        Me.txtRUC = New System.Windows.Forms.TextBox()
        Me.lblRUC = New System.Windows.Forms.Label()
        Me.txtRazonSocial = New System.Windows.Forms.TextBox()
        Me.lblRazonSocial = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.lblIdValor = New System.Windows.Forms.Label()
        Me.lblIdTitulo = New System.Windows.Forms.Label()
        Me.pnlBusqueda.SuspendLayout()
        Me.pnlListaMaestro.SuspendLayout()
        CType(Me.dgvProveedores, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEdicion.SuspendLayout()
        Me.grpDatos.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlBusqueda
        '
        Me.pnlBusqueda.Controls.Add(Me.btnNuevo)
        Me.pnlBusqueda.Controls.Add(Me.btnBuscar)
        Me.pnlBusqueda.Controls.Add(Me.txtBusqueda)
        Me.pnlBusqueda.Controls.Add(Me.lblBusqueda)
        Me.pnlBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBusqueda.Location = New System.Drawing.Point(0, 0)
        Me.pnlBusqueda.Name = "pnlBusqueda"
        Me.pnlBusqueda.Padding = New System.Windows.Forms.Padding(8, 8, 8, 4)
        Me.pnlBusqueda.Size = New System.Drawing.Size(900, 44)
        Me.pnlBusqueda.TabIndex = 0
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.Location = New System.Drawing.Point(813, 8)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(75, 23)
        Me.btnNuevo.TabIndex = 3
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnBuscar
        '
        Me.btnBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuscar.Location = New System.Drawing.Point(732, 8)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(75, 23)
        Me.btnBuscar.TabIndex = 2
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.Location = New System.Drawing.Point(200, 10)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(526, 20)
        Me.txtBusqueda.TabIndex = 1
        '
        'lblBusqueda
        '
        Me.lblBusqueda.AutoSize = True
        Me.lblBusqueda.Location = New System.Drawing.Point(11, 13)
        Me.lblBusqueda.Name = "lblBusqueda"
        Me.lblBusqueda.Size = New System.Drawing.Size(137, 13)
        Me.lblBusqueda.TabIndex = 0
        Me.lblBusqueda.Text = "Código, razón social o RUC"
        '
        'dgvProveedores
        '
        Me.dgvProveedores.AllowUserToAddRows = False
        Me.dgvProveedores.AllowUserToDeleteRows = False
        Me.dgvProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProveedores.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvProveedores.Location = New System.Drawing.Point(0, 44)
        Me.dgvProveedores.MultiSelect = False
        Me.dgvProveedores.Name = "dgvProveedores"
        Me.dgvProveedores.ReadOnly = True
        Me.dgvProveedores.RowHeadersVisible = False
        Me.dgvProveedores.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvProveedores.Size = New System.Drawing.Size(900, 236)
        Me.dgvProveedores.TabIndex = 1
        '
        'pnlEdicion
        '
        Me.pnlEdicion.Controls.Add(Me.btnDesactivar)
        Me.pnlEdicion.Controls.Add(Me.btnActivar)
        Me.pnlEdicion.Controls.Add(Me.btnCancelar)
        Me.pnlEdicion.Controls.Add(Me.btnGuardar)
        Me.pnlEdicion.Controls.Add(Me.grpDatos)
        Me.pnlEdicion.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlEdicion.Location = New System.Drawing.Point(0, 280)
        Me.pnlEdicion.Name = "pnlEdicion"
        Me.pnlEdicion.Padding = New System.Windows.Forms.Padding(8, 4, 8, 8)
        Me.pnlEdicion.Size = New System.Drawing.Size(900, 400)
        Me.pnlEdicion.TabIndex = 2
        '
        'btnDesactivar
        '
        Me.btnDesactivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDesactivar.Location = New System.Drawing.Point(197, 364)
        Me.btnDesactivar.Name = "btnDesactivar"
        Me.btnDesactivar.Size = New System.Drawing.Size(88, 28)
        Me.btnDesactivar.TabIndex = 4
        Me.btnDesactivar.Text = "Desactivar"
        Me.btnDesactivar.UseVisualStyleBackColor = True
        '
        'btnActivar
        '
        Me.btnActivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActivar.Location = New System.Drawing.Point(103, 364)
        Me.btnActivar.Name = "btnActivar"
        Me.btnActivar.Size = New System.Drawing.Size(88, 28)
        Me.btnActivar.TabIndex = 3
        Me.btnActivar.Text = "Activar"
        Me.btnActivar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(717, 364)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(80, 28)
        Me.btnCancelar.TabIndex = 2
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(803, 364)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(85, 28)
        Me.btnGuardar.TabIndex = 1
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'grpDatos
        '
        Me.grpDatos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpDatos.Controls.Add(Me.txtObservaciones)
        Me.grpDatos.Controls.Add(Me.lblObservaciones)
        Me.grpDatos.Controls.Add(Me.cmbCondicionPago)
        Me.grpDatos.Controls.Add(Me.lblCondicionPago)
        Me.grpDatos.Controls.Add(Me.txtEmail)
        Me.grpDatos.Controls.Add(Me.lblEmail)
        Me.grpDatos.Controls.Add(Me.txtTelefono)
        Me.grpDatos.Controls.Add(Me.lblTelefono)
        Me.grpDatos.Controls.Add(Me.txtDireccion)
        Me.grpDatos.Controls.Add(Me.lblDireccion)
        Me.grpDatos.Controls.Add(Me.txtRUC)
        Me.grpDatos.Controls.Add(Me.lblRUC)
        Me.grpDatos.Controls.Add(Me.txtRazonSocial)
        Me.grpDatos.Controls.Add(Me.lblRazonSocial)
        Me.grpDatos.Controls.Add(Me.txtCodigo)
        Me.grpDatos.Controls.Add(Me.lblCodigo)
        Me.grpDatos.Controls.Add(Me.chkActivo)
        Me.grpDatos.Controls.Add(Me.lblIdValor)
        Me.grpDatos.Controls.Add(Me.lblIdTitulo)
        Me.grpDatos.Location = New System.Drawing.Point(11, 4)
        Me.grpDatos.Name = "grpDatos"
        Me.grpDatos.Size = New System.Drawing.Size(877, 358)
        Me.grpDatos.TabIndex = 0
        Me.grpDatos.TabStop = False
        Me.grpDatos.Text = "Datos del proveedor"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObservaciones.Location = New System.Drawing.Point(16, 300)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(845, 50)
        Me.txtObservaciones.TabIndex = 17
        '
        'lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(13, 284)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(78, 13)
        Me.lblObservaciones.TabIndex = 16
        Me.lblObservaciones.Text = "Observaciones"
        '
        'cmbCondicionPago
        '
        Me.cmbCondicionPago.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCondicionPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCondicionPago.FormattingEnabled = True
        Me.cmbCondicionPago.Location = New System.Drawing.Point(16, 256)
        Me.cmbCondicionPago.Name = "cmbCondicionPago"
        Me.cmbCondicionPago.Size = New System.Drawing.Size(845, 21)
        Me.cmbCondicionPago.TabIndex = 15
        '
        'lblCondicionPago
        '
        Me.lblCondicionPago.AutoSize = True
        Me.lblCondicionPago.Location = New System.Drawing.Point(13, 240)
        Me.lblCondicionPago.Name = "lblCondicionPago"
        Me.lblCondicionPago.Size = New System.Drawing.Size(96, 13)
        Me.lblCondicionPago.TabIndex = 14
        Me.lblCondicionPago.Text = "Condición de pago"
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(440, 212)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(421, 20)
        Me.txtEmail.TabIndex = 13
        '
        'lblEmail
        '
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(437, 196)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(93, 13)
        Me.lblEmail.TabIndex = 12
        Me.lblEmail.Text = "Correo electrónico"
        '
        'txtTelefono
        '
        Me.txtTelefono.Location = New System.Drawing.Point(16, 212)
        Me.txtTelefono.Name = "txtTelefono"
        Me.txtTelefono.Size = New System.Drawing.Size(400, 20)
        Me.txtTelefono.TabIndex = 11
        '
        'lblTelefono
        '
        Me.lblTelefono.AutoSize = True
        Me.lblTelefono.Location = New System.Drawing.Point(13, 196)
        Me.lblTelefono.Name = "lblTelefono"
        Me.lblTelefono.Size = New System.Drawing.Size(49, 13)
        Me.lblTelefono.TabIndex = 10
        Me.lblTelefono.Text = "Teléfono"
        '
        'txtDireccion
        '
        Me.txtDireccion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDireccion.Location = New System.Drawing.Point(16, 168)
        Me.txtDireccion.Name = "txtDireccion"
        Me.txtDireccion.Size = New System.Drawing.Size(845, 20)
        Me.txtDireccion.TabIndex = 9
        '
        'lblDireccion
        '
        Me.lblDireccion.AutoSize = True
        Me.lblDireccion.Location = New System.Drawing.Point(13, 152)
        Me.lblDireccion.Name = "lblDireccion"
        Me.lblDireccion.Size = New System.Drawing.Size(52, 13)
        Me.lblDireccion.TabIndex = 8
        Me.lblDireccion.Text = "Dirección"
        '
        'txtRUC
        '
        Me.txtRUC.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRUC.Location = New System.Drawing.Point(16, 124)
        Me.txtRUC.Name = "txtRUC"
        Me.txtRUC.Size = New System.Drawing.Size(845, 20)
        Me.txtRUC.TabIndex = 7
        '
        'lblRUC
        '
        Me.lblRUC.AutoSize = True
        Me.lblRUC.Location = New System.Drawing.Point(13, 108)
        Me.lblRUC.Name = "lblRUC"
        Me.lblRUC.Size = New System.Drawing.Size(30, 13)
        Me.lblRUC.TabIndex = 6
        Me.lblRUC.Text = "RUC"
        '
        'txtRazonSocial
        '
        Me.txtRazonSocial.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRazonSocial.Location = New System.Drawing.Point(240, 68)
        Me.txtRazonSocial.Name = "txtRazonSocial"
        Me.txtRazonSocial.Size = New System.Drawing.Size(621, 20)
        Me.txtRazonSocial.TabIndex = 5
        '
        'lblRazonSocial
        '
        Me.lblRazonSocial.AutoSize = True
        Me.lblRazonSocial.Location = New System.Drawing.Point(237, 52)
        Me.lblRazonSocial.Name = "lblRazonSocial"
        Me.lblRazonSocial.Size = New System.Drawing.Size(68, 13)
        Me.lblRazonSocial.TabIndex = 4
        Me.lblRazonSocial.Text = "Razón social"
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(16, 68)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(200, 20)
        Me.txtCodigo.TabIndex = 3
        '
        'lblCodigo
        '
        Me.lblCodigo.AutoSize = True
        Me.lblCodigo.Location = New System.Drawing.Point(13, 52)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigo.TabIndex = 2
        Me.lblCodigo.Text = "Código"
        '
        'chkActivo
        '
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Enabled = False
        Me.chkActivo.Location = New System.Drawing.Point(520, 22)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(56, 17)
        Me.chkActivo.TabIndex = 1
        Me.chkActivo.Text = "Activo"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'lblIdValor
        '
        Me.lblIdValor.AutoSize = True
        Me.lblIdValor.Location = New System.Drawing.Point(45, 24)
        Me.lblIdValor.Name = "lblIdValor"
        Me.lblIdValor.Size = New System.Drawing.Size(13, 13)
        Me.lblIdValor.TabIndex = 1
        Me.lblIdValor.Text = "—"
        '
        'lblIdTitulo
        '
        Me.lblIdTitulo.AutoSize = True
        Me.lblIdTitulo.Location = New System.Drawing.Point(13, 24)
        Me.lblIdTitulo.Name = "lblIdTitulo"
        Me.lblIdTitulo.Size = New System.Drawing.Size(16, 13)
        Me.lblIdTitulo.TabIndex = 0
        Me.lblIdTitulo.Text = "Id"
        '
        'FrmProveedores
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 680)
        Me.Controls.Add(Me.dgvProveedores)
        Me.Controls.Add(Me.pnlEdicion)
        Me.Controls.Add(Me.pnlBusqueda)
        Me.MinimizeBox = False
        Me.Name = "FrmProveedores"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Proveedores"
        Me.pnlBusqueda.ResumeLayout(False)
        Me.pnlBusqueda.PerformLayout()
        CType(Me.dgvProveedores, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlListaMaestro.ResumeLayout(False)
        Me.pnlEdicion.ResumeLayout(False)
        Me.grpDatos.ResumeLayout(False)
        Me.grpDatos.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlBusqueda As Panel
    Friend WithEvents btnNuevo As Button
    Friend WithEvents btnBuscar As Button
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents lblBusqueda As Label
    Friend WithEvents pnlListaMaestro As Panel
    Friend WithEvents lblTituloGrilla As Label
    Friend WithEvents dgvProveedores As DataGridView
    Friend WithEvents pnlEdicion As Panel
    Friend WithEvents btnDesactivar As Button
    Friend WithEvents btnActivar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents grpDatos As GroupBox
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents lblObservaciones As Label
    Friend WithEvents cmbCondicionPago As ComboBox
    Friend WithEvents lblCondicionPago As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtTelefono As TextBox
    Friend WithEvents lblTelefono As Label
    Friend WithEvents txtDireccion As TextBox
    Friend WithEvents lblDireccion As Label
    Friend WithEvents txtRUC As TextBox
    Friend WithEvents lblRUC As Label
    Friend WithEvents txtRazonSocial As TextBox
    Friend WithEvents lblRazonSocial As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents lblCodigo As Label
    Friend WithEvents chkActivo As CheckBox
    Friend WithEvents lblIdValor As Label
    Friend WithEvents lblIdTitulo As Label
End Class
