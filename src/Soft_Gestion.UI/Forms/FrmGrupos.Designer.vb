<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmGrupos
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
        Me.cmbFiltroSubCategoria = New System.Windows.Forms.ComboBox()
        Me.lblFiltroSubCategoria = New System.Windows.Forms.Label()
        Me.cmbFiltroCategoria = New System.Windows.Forms.ComboBox()
        Me.lblFiltroCategoria = New System.Windows.Forms.Label()
        Me.pnlListaMaestro = New System.Windows.Forms.Panel()
        Me.lblTituloGrilla = New System.Windows.Forms.Label()
        Me.dgvGrupos = New System.Windows.Forms.DataGridView()
        Me.pnlEdicion = New System.Windows.Forms.Panel()
        Me.btnDesactivar = New System.Windows.Forms.Button()
        Me.btnActivar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.grpDatos = New System.Windows.Forms.GroupBox()
        Me.cmbSubCategoria = New System.Windows.Forms.ComboBox()
        Me.lblSubCategoria = New System.Windows.Forms.Label()
        Me.cmbCategoria = New System.Windows.Forms.ComboBox()
        Me.lblCategoria = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.lblIdValor = New System.Windows.Forms.Label()
        Me.lblIdTitulo = New System.Windows.Forms.Label()
        Me.pnlBusqueda.SuspendLayout()
        Me.pnlListaMaestro.SuspendLayout()
        CType(Me.dgvGrupos, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroSubCategoria)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroSubCategoria)
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroCategoria)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroCategoria)
        Me.pnlBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBusqueda.Location = New System.Drawing.Point(0, 0)
        Me.pnlBusqueda.Name = "pnlBusqueda"
        Me.pnlBusqueda.Padding = New System.Windows.Forms.Padding(8, 8, 8, 4)
        Me.pnlBusqueda.Size = New System.Drawing.Size(900, 72)
        Me.pnlBusqueda.TabIndex = 0
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.Location = New System.Drawing.Point(813, 38)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(75, 23)
        Me.btnNuevo.TabIndex = 7
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnBuscar
        '
        Me.btnBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuscar.Location = New System.Drawing.Point(732, 38)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(75, 23)
        Me.btnBuscar.TabIndex = 6
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.Location = New System.Drawing.Point(120, 40)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(600, 20)
        Me.txtBusqueda.TabIndex = 5
        '
        'lblBusqueda
        '
        Me.lblBusqueda.AutoSize = True
        Me.lblBusqueda.Location = New System.Drawing.Point(11, 43)
        Me.lblBusqueda.Name = "lblBusqueda"
        Me.lblBusqueda.Size = New System.Drawing.Size(87, 13)
        Me.lblBusqueda.TabIndex = 4
        Me.lblBusqueda.Text = "Código o nombre"
        '
        'cmbFiltroSubCategoria
        '
        Me.cmbFiltroSubCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroSubCategoria.FormattingEnabled = True
        Me.cmbFiltroSubCategoria.Location = New System.Drawing.Point(420, 9)
        Me.cmbFiltroSubCategoria.Name = "cmbFiltroSubCategoria"
        Me.cmbFiltroSubCategoria.Size = New System.Drawing.Size(280, 21)
        Me.cmbFiltroSubCategoria.TabIndex = 3
        '
        'lblFiltroSubCategoria
        '
        Me.lblFiltroSubCategoria.AutoSize = True
        Me.lblFiltroSubCategoria.Location = New System.Drawing.Point(330, 13)
        Me.lblFiltroSubCategoria.Name = "lblFiltroSubCategoria"
        Me.lblFiltroSubCategoria.Size = New System.Drawing.Size(72, 13)
        Me.lblFiltroSubCategoria.TabIndex = 2
        Me.lblFiltroSubCategoria.Text = "Subcategoría"
        '
        'cmbFiltroCategoria
        '
        Me.cmbFiltroCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroCategoria.FormattingEnabled = True
        Me.cmbFiltroCategoria.Location = New System.Drawing.Point(75, 9)
        Me.cmbFiltroCategoria.Name = "cmbFiltroCategoria"
        Me.cmbFiltroCategoria.Size = New System.Drawing.Size(240, 21)
        Me.cmbFiltroCategoria.TabIndex = 1
        '
        'lblFiltroCategoria
        '
        Me.lblFiltroCategoria.AutoSize = True
        Me.lblFiltroCategoria.Location = New System.Drawing.Point(11, 13)
        Me.lblFiltroCategoria.Name = "lblFiltroCategoria"
        Me.lblFiltroCategoria.Size = New System.Drawing.Size(54, 13)
        Me.lblFiltroCategoria.TabIndex = 0
        Me.lblFiltroCategoria.Text = "Categoría"
        '
        'dgvGrupos
        '
        Me.dgvGrupos.AllowUserToAddRows = False
        Me.dgvGrupos.AllowUserToDeleteRows = False
        Me.dgvGrupos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGrupos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvGrupos.Location = New System.Drawing.Point(0, 72)
        Me.dgvGrupos.MultiSelect = False
        Me.dgvGrupos.Name = "dgvGrupos"
        Me.dgvGrupos.ReadOnly = True
        Me.dgvGrupos.RowHeadersVisible = False
        Me.dgvGrupos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvGrupos.Size = New System.Drawing.Size(900, 208)
        Me.dgvGrupos.TabIndex = 1
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
        Me.pnlEdicion.Size = New System.Drawing.Size(900, 280)
        Me.pnlEdicion.TabIndex = 2
        '
        'btnDesactivar
        '
        Me.btnDesactivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDesactivar.Location = New System.Drawing.Point(197, 244)
        Me.btnDesactivar.Name = "btnDesactivar"
        Me.btnDesactivar.Size = New System.Drawing.Size(88, 28)
        Me.btnDesactivar.TabIndex = 4
        Me.btnDesactivar.Text = "Desactivar"
        Me.btnDesactivar.UseVisualStyleBackColor = True
        '
        'btnActivar
        '
        Me.btnActivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnActivar.Location = New System.Drawing.Point(103, 244)
        Me.btnActivar.Name = "btnActivar"
        Me.btnActivar.Size = New System.Drawing.Size(88, 28)
        Me.btnActivar.TabIndex = 3
        Me.btnActivar.Text = "Activar"
        Me.btnActivar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(717, 244)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(80, 28)
        Me.btnCancelar.TabIndex = 2
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(803, 244)
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
        Me.grpDatos.Controls.Add(Me.cmbSubCategoria)
        Me.grpDatos.Controls.Add(Me.lblSubCategoria)
        Me.grpDatos.Controls.Add(Me.cmbCategoria)
        Me.grpDatos.Controls.Add(Me.lblCategoria)
        Me.grpDatos.Controls.Add(Me.txtNombre)
        Me.grpDatos.Controls.Add(Me.lblNombre)
        Me.grpDatos.Controls.Add(Me.txtCodigo)
        Me.grpDatos.Controls.Add(Me.lblCodigo)
        Me.grpDatos.Controls.Add(Me.chkActivo)
        Me.grpDatos.Controls.Add(Me.lblIdValor)
        Me.grpDatos.Controls.Add(Me.lblIdTitulo)
        Me.grpDatos.Location = New System.Drawing.Point(11, 4)
        Me.grpDatos.Name = "grpDatos"
        Me.grpDatos.Size = New System.Drawing.Size(877, 234)
        Me.grpDatos.TabIndex = 0
        Me.grpDatos.TabStop = False
        Me.grpDatos.Text = "Datos del grupo"
        '
        'cmbSubCategoria
        '
        Me.cmbSubCategoria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbSubCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubCategoria.FormattingEnabled = True
        Me.cmbSubCategoria.Location = New System.Drawing.Point(16, 112)
        Me.cmbSubCategoria.Name = "cmbSubCategoria"
        Me.cmbSubCategoria.Size = New System.Drawing.Size(845, 21)
        Me.cmbSubCategoria.TabIndex = 7
        '
        'lblSubCategoria
        '
        Me.lblSubCategoria.AutoSize = True
        Me.lblSubCategoria.Location = New System.Drawing.Point(13, 96)
        Me.lblSubCategoria.Name = "lblSubCategoria"
        Me.lblSubCategoria.Size = New System.Drawing.Size(72, 13)
        Me.lblSubCategoria.TabIndex = 6
        Me.lblSubCategoria.Text = "Subcategoría"
        '
        'cmbCategoria
        '
        Me.cmbCategoria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoria.FormattingEnabled = True
        Me.cmbCategoria.Location = New System.Drawing.Point(16, 68)
        Me.cmbCategoria.Name = "cmbCategoria"
        Me.cmbCategoria.Size = New System.Drawing.Size(845, 21)
        Me.cmbCategoria.TabIndex = 5
        '
        'lblCategoria
        '
        Me.lblCategoria.AutoSize = True
        Me.lblCategoria.Location = New System.Drawing.Point(13, 52)
        Me.lblCategoria.Name = "lblCategoria"
        Me.lblCategoria.Size = New System.Drawing.Size(54, 13)
        Me.lblCategoria.TabIndex = 4
        Me.lblCategoria.Text = "Categoría"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(16, 200)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(845, 20)
        Me.txtNombre.TabIndex = 11
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(13, 184)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(44, 13)
        Me.lblNombre.TabIndex = 10
        Me.lblNombre.Text = "Nombre"
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(16, 156)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(200, 20)
        Me.txtCodigo.TabIndex = 9
        '
        'lblCodigo
        '
        Me.lblCodigo.AutoSize = True
        Me.lblCodigo.Location = New System.Drawing.Point(13, 140)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigo.TabIndex = 8
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
        'FrmGrupos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 560)
        Me.Controls.Add(Me.dgvGrupos)
        Me.Controls.Add(Me.pnlEdicion)
        Me.Controls.Add(Me.pnlBusqueda)
        Me.MinimizeBox = False
        Me.Name = "FrmGrupos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Grupos"
        Me.pnlBusqueda.ResumeLayout(False)
        Me.pnlBusqueda.PerformLayout()
        CType(Me.dgvGrupos, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents cmbFiltroSubCategoria As ComboBox
    Friend WithEvents lblFiltroSubCategoria As Label
    Friend WithEvents cmbFiltroCategoria As ComboBox
    Friend WithEvents lblFiltroCategoria As Label
    Friend WithEvents pnlListaMaestro As Panel
    Friend WithEvents lblTituloGrilla As Label
    Friend WithEvents dgvGrupos As DataGridView
    Friend WithEvents pnlEdicion As Panel
    Friend WithEvents btnDesactivar As Button
    Friend WithEvents btnActivar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents grpDatos As GroupBox
    Friend WithEvents cmbSubCategoria As ComboBox
    Friend WithEvents lblSubCategoria As Label
    Friend WithEvents cmbCategoria As ComboBox
    Friend WithEvents lblCategoria As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents lblCodigo As Label
    Friend WithEvents chkActivo As CheckBox
    Friend WithEvents lblIdValor As Label
    Friend WithEvents lblIdTitulo As Label
End Class
