<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmUnidadesMedida
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
        Me.dgvUnidadesMedida = New System.Windows.Forms.DataGridView()
        Me.pnlEdicion = New System.Windows.Forms.Panel()
        Me.btnDesactivar = New System.Windows.Forms.Button()
        Me.btnActivar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.grpDatos = New System.Windows.Forms.GroupBox()
        Me.cmbCodigoSifen = New System.Windows.Forms.ComboBox()
        Me.lblCodigoSifen = New System.Windows.Forms.Label()
        Me.txtAbreviatura = New System.Windows.Forms.TextBox()
        Me.lblAbreviatura = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.lblIdValor = New System.Windows.Forms.Label()
        Me.lblIdTitulo = New System.Windows.Forms.Label()
        Me.pnlBusqueda.SuspendLayout()
        Me.pnlListaMaestro.SuspendLayout()
        CType(Me.dgvUnidadesMedida, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.txtBusqueda.Location = New System.Drawing.Point(140, 10)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(586, 20)
        Me.txtBusqueda.TabIndex = 1
        '
        'lblBusqueda
        '
        Me.lblBusqueda.AutoSize = True
        Me.lblBusqueda.Location = New System.Drawing.Point(11, 13)
        Me.lblBusqueda.Name = "lblBusqueda"
        Me.lblBusqueda.Size = New System.Drawing.Size(87, 13)
        Me.lblBusqueda.TabIndex = 0
        Me.lblBusqueda.Text = "Código o nombre"
        '
        'dgvUnidadesMedida
        '
        Me.dgvUnidadesMedida.AllowUserToAddRows = False
        Me.dgvUnidadesMedida.AllowUserToDeleteRows = False
        Me.dgvUnidadesMedida.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUnidadesMedida.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUnidadesMedida.Location = New System.Drawing.Point(0, 44)
        Me.dgvUnidadesMedida.MultiSelect = False
        Me.dgvUnidadesMedida.Name = "dgvUnidadesMedida"
        Me.dgvUnidadesMedida.ReadOnly = True
        Me.dgvUnidadesMedida.RowHeadersVisible = False
        Me.dgvUnidadesMedida.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUnidadesMedida.Size = New System.Drawing.Size(900, 246)
        Me.dgvUnidadesMedida.TabIndex = 1
        '
        'pnlEdicion
        '
        Me.pnlEdicion.Controls.Add(Me.btnDesactivar)
        Me.pnlEdicion.Controls.Add(Me.btnActivar)
        Me.pnlEdicion.Controls.Add(Me.btnCancelar)
        Me.pnlEdicion.Controls.Add(Me.btnGuardar)
        Me.pnlEdicion.Controls.Add(Me.grpDatos)
        Me.pnlEdicion.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlEdicion.Location = New System.Drawing.Point(0, 290)
        Me.pnlEdicion.Name = "pnlEdicion"
        Me.pnlEdicion.Padding = New System.Windows.Forms.Padding(8, 4, 8, 8)
        Me.pnlEdicion.Size = New System.Drawing.Size(900, 270)
        Me.pnlEdicion.TabIndex = 2
        '
        'btnDesactivar
        '
        Me.btnDesactivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDesactivar.Location = New System.Drawing.Point(197, 234)
        Me.btnDesactivar.Name = "btnDesactivar"
        Me.btnDesactivar.Size = New System.Drawing.Size(88, 28)
        Me.btnDesactivar.TabIndex = 4
        Me.btnDesactivar.Text = "Desactivar"
        Me.btnDesactivar.UseVisualStyleBackColor = True
        '
        'btnActivar
        '
        Me.btnActivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActivar.Location = New System.Drawing.Point(103, 234)
        Me.btnActivar.Name = "btnActivar"
        Me.btnActivar.Size = New System.Drawing.Size(88, 28)
        Me.btnActivar.TabIndex = 3
        Me.btnActivar.Text = "Activar"
        Me.btnActivar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(717, 234)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(80, 28)
        Me.btnCancelar.TabIndex = 2
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(803, 234)
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
        Me.grpDatos.Controls.Add(Me.cmbCodigoSifen)
        Me.grpDatos.Controls.Add(Me.lblCodigoSifen)
        Me.grpDatos.Controls.Add(Me.txtAbreviatura)
        Me.grpDatos.Controls.Add(Me.lblAbreviatura)
        Me.grpDatos.Controls.Add(Me.txtNombre)
        Me.grpDatos.Controls.Add(Me.lblNombre)
        Me.grpDatos.Controls.Add(Me.txtCodigo)
        Me.grpDatos.Controls.Add(Me.lblCodigo)
        Me.grpDatos.Controls.Add(Me.chkActivo)
        Me.grpDatos.Controls.Add(Me.lblIdValor)
        Me.grpDatos.Controls.Add(Me.lblIdTitulo)
        Me.grpDatos.Location = New System.Drawing.Point(11, 4)
        Me.grpDatos.Name = "grpDatos"
        Me.grpDatos.Size = New System.Drawing.Size(877, 224)
        Me.grpDatos.TabIndex = 0
        Me.grpDatos.TabStop = False
        Me.grpDatos.Text = "Datos de la unidad de medida"
        '
        'cmbCodigoSifen
        '
        Me.cmbCodigoSifen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCodigoSifen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCodigoSifen.FormattingEnabled = True
        Me.cmbCodigoSifen.Location = New System.Drawing.Point(16, 192)
        Me.cmbCodigoSifen.Name = "cmbCodigoSifen"
        Me.cmbCodigoSifen.Size = New System.Drawing.Size(845, 21)
        Me.cmbCodigoSifen.TabIndex = 11
        '
        'lblCodigoSifen
        '
        Me.lblCodigoSifen.AutoSize = True
        Me.lblCodigoSifen.Location = New System.Drawing.Point(13, 176)
        Me.lblCodigoSifen.Name = "lblCodigoSifen"
        Me.lblCodigoSifen.Size = New System.Drawing.Size(96, 13)
        Me.lblCodigoSifen.TabIndex = 10
        Me.lblCodigoSifen.Text = "Código SIFEN (FE)"
        '
        'txtAbreviatura
        '
        Me.txtAbreviatura.Location = New System.Drawing.Point(16, 148)
        Me.txtAbreviatura.MaxLength = 10
        Me.txtAbreviatura.Name = "txtAbreviatura"
        Me.txtAbreviatura.Size = New System.Drawing.Size(120, 20)
        Me.txtAbreviatura.TabIndex = 9
        '
        'lblAbreviatura
        '
        Me.lblAbreviatura.AutoSize = True
        Me.lblAbreviatura.Location = New System.Drawing.Point(13, 132)
        Me.lblAbreviatura.Name = "lblAbreviatura"
        Me.lblAbreviatura.Size = New System.Drawing.Size(61, 13)
        Me.lblAbreviatura.TabIndex = 8
        Me.lblAbreviatura.Text = "Abreviatura"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(16, 104)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(845, 20)
        Me.txtNombre.TabIndex = 7
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(13, 88)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(44, 13)
        Me.lblNombre.TabIndex = 6
        Me.lblNombre.Text = "Nombre"
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(16, 60)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(200, 20)
        Me.txtCodigo.TabIndex = 5
        '
        'lblCodigo
        '
        Me.lblCodigo.AutoSize = True
        Me.lblCodigo.Location = New System.Drawing.Point(13, 44)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigo.TabIndex = 4
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
        'FrmUnidadesMedida
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 560)
        Me.Controls.Add(Me.dgvUnidadesMedida)
        Me.Controls.Add(Me.pnlEdicion)
        Me.Controls.Add(Me.pnlBusqueda)
        Me.MinimizeBox = False
        Me.Name = "FrmUnidadesMedida"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Unidades de medida"
        Me.pnlBusqueda.ResumeLayout(False)
        Me.pnlBusqueda.PerformLayout()
        CType(Me.dgvUnidadesMedida, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents dgvUnidadesMedida As DataGridView
    Friend WithEvents pnlEdicion As Panel
    Friend WithEvents btnDesactivar As Button
    Friend WithEvents btnActivar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents grpDatos As GroupBox
    Friend WithEvents cmbCodigoSifen As ComboBox
    Friend WithEvents lblCodigoSifen As Label
    Friend WithEvents txtAbreviatura As TextBox
    Friend WithEvents lblAbreviatura As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents lblCodigo As Label
    Friend WithEvents chkActivo As CheckBox
    Friend WithEvents lblIdValor As Label
    Friend WithEvents lblIdTitulo As Label
End Class
