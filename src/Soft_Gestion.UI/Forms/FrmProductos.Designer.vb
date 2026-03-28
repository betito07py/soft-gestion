<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmProductos
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
        Me.cmbFiltroActivo = New System.Windows.Forms.ComboBox()
        Me.lblFiltroActivo = New System.Windows.Forms.Label()
        Me.cmbFiltroMarca = New System.Windows.Forms.ComboBox()
        Me.lblFiltroMarca = New System.Windows.Forms.Label()
        Me.cmbFiltroGrupo = New System.Windows.Forms.ComboBox()
        Me.lblFiltroGrupo = New System.Windows.Forms.Label()
        Me.cmbFiltroSubCategoria = New System.Windows.Forms.ComboBox()
        Me.lblFiltroSubCategoria = New System.Windows.Forms.Label()
        Me.cmbFiltroCategoria = New System.Windows.Forms.ComboBox()
        Me.lblFiltroCategoria = New System.Windows.Forms.Label()
        Me.pnlListaMaestro = New System.Windows.Forms.Panel()
        Me.lblTituloGrilla = New System.Windows.Forms.Label()
        Me.dgvProductos = New System.Windows.Forms.DataGridView()
        Me.pnlEdicion = New System.Windows.Forms.Panel()
        Me.btnDesactivar = New System.Windows.Forms.Button()
        Me.btnActivar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.grpDatos = New System.Windows.Forms.GroupBox()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.lblObservaciones = New System.Windows.Forms.Label()
        Me.chkControlaStock = New System.Windows.Forms.CheckBox()
        Me.chkPermiteStockNegativo = New System.Windows.Forms.CheckBox()
        Me.txtCostoUltimo = New System.Windows.Forms.TextBox()
        Me.lblCostoUltimo = New System.Windows.Forms.Label()
        Me.txtSifenRepr = New System.Windows.Forms.TextBox()
        Me.txtSifenDescrip = New System.Windows.Forms.TextBox()
        Me.txtSifenCodigo = New System.Windows.Forms.TextBox()
        Me.lblSifenTitulo = New System.Windows.Forms.Label()
        Me.cmbUnidadMedida = New System.Windows.Forms.ComboBox()
        Me.lblUnidadMedida = New System.Windows.Forms.Label()
        Me.cmbMarca = New System.Windows.Forms.ComboBox()
        Me.lblMarca = New System.Windows.Forms.Label()
        Me.cmbImpuesto = New System.Windows.Forms.ComboBox()
        Me.lblImpuesto = New System.Windows.Forms.Label()
        Me.cmbGrupo = New System.Windows.Forms.ComboBox()
        Me.lblGrupo = New System.Windows.Forms.Label()
        Me.cmbSubCategoria = New System.Windows.Forms.ComboBox()
        Me.lblSubCategoria = New System.Windows.Forms.Label()
        Me.cmbCategoria = New System.Windows.Forms.ComboBox()
        Me.lblCategoria = New System.Windows.Forms.Label()
        Me.txtDescripcion = New System.Windows.Forms.TextBox()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.btnQuitarCodigoBarra = New System.Windows.Forms.Button()
        Me.btnAgregarCodigoBarra = New System.Windows.Forms.Button()
        Me.txtNuevoCodigoBarra = New System.Windows.Forms.TextBox()
        Me.lstCodigosBarras = New System.Windows.Forms.ListBox()
        Me.lblCodigoBarras = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.lblCodigo = New System.Windows.Forms.Label()
        Me.chkEsServicio = New System.Windows.Forms.CheckBox()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.lblIdValor = New System.Windows.Forms.Label()
        Me.lblIdTitulo = New System.Windows.Forms.Label()
        Me.pnlBusqueda.SuspendLayout()
        CType(Me.dgvProductos, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroActivo)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroActivo)
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroMarca)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroMarca)
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroGrupo)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroGrupo)
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroSubCategoria)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroSubCategoria)
        Me.pnlBusqueda.Controls.Add(Me.cmbFiltroCategoria)
        Me.pnlBusqueda.Controls.Add(Me.lblFiltroCategoria)
        Me.pnlBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBusqueda.Location = New System.Drawing.Point(0, 0)
        Me.pnlBusqueda.Name = "pnlBusqueda"
        Me.pnlBusqueda.Padding = New System.Windows.Forms.Padding(8, 8, 8, 4)
        Me.pnlBusqueda.Size = New System.Drawing.Size(1170, 71)
        Me.pnlBusqueda.TabIndex = 0
        '
        'btnNuevo
        '
        Me.btnNuevo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevo.Location = New System.Drawing.Point(1075, 33)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(75, 23)
        Me.btnNuevo.TabIndex = 13
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnBuscar
        '
        Me.btnBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuscar.Location = New System.Drawing.Point(994, 33)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(75, 23)
        Me.btnBuscar.TabIndex = 12
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.Location = New System.Drawing.Point(163, 35)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(815, 20)
        Me.txtBusqueda.TabIndex = 11
        '
        'lblBusqueda
        '
        Me.lblBusqueda.AutoSize = True
        Me.lblBusqueda.Location = New System.Drawing.Point(6, 38)
        Me.lblBusqueda.Name = "lblBusqueda"
        Me.lblBusqueda.Size = New System.Drawing.Size(148, 13)
        Me.lblBusqueda.TabIndex = 10
        Me.lblBusqueda.Text = "Código, código barras o desc."
        '
        'cmbFiltroActivo
        '
        Me.cmbFiltroActivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroActivo.FormattingEnabled = True
        Me.cmbFiltroActivo.Location = New System.Drawing.Point(1038, 8)
        Me.cmbFiltroActivo.Name = "cmbFiltroActivo"
        Me.cmbFiltroActivo.Size = New System.Drawing.Size(120, 21)
        Me.cmbFiltroActivo.TabIndex = 9
        '
        'lblFiltroActivo
        '
        Me.lblFiltroActivo.AutoSize = True
        Me.lblFiltroActivo.Location = New System.Drawing.Point(993, 12)
        Me.lblFiltroActivo.Name = "lblFiltroActivo"
        Me.lblFiltroActivo.Size = New System.Drawing.Size(37, 13)
        Me.lblFiltroActivo.TabIndex = 8
        Me.lblFiltroActivo.Text = "Activo"
        '
        'cmbFiltroMarca
        '
        Me.cmbFiltroMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroMarca.FormattingEnabled = True
        Me.cmbFiltroMarca.Location = New System.Drawing.Point(798, 8)
        Me.cmbFiltroMarca.Name = "cmbFiltroMarca"
        Me.cmbFiltroMarca.Size = New System.Drawing.Size(180, 21)
        Me.cmbFiltroMarca.TabIndex = 7
        '
        'lblFiltroMarca
        '
        Me.lblFiltroMarca.AutoSize = True
        Me.lblFiltroMarca.Location = New System.Drawing.Point(753, 12)
        Me.lblFiltroMarca.Name = "lblFiltroMarca"
        Me.lblFiltroMarca.Size = New System.Drawing.Size(37, 13)
        Me.lblFiltroMarca.TabIndex = 6
        Me.lblFiltroMarca.Text = "Marca"
        '
        'cmbFiltroGrupo
        '
        Me.cmbFiltroGrupo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroGrupo.FormattingEnabled = True
        Me.cmbFiltroGrupo.Location = New System.Drawing.Point(546, 8)
        Me.cmbFiltroGrupo.Name = "cmbFiltroGrupo"
        Me.cmbFiltroGrupo.Size = New System.Drawing.Size(190, 21)
        Me.cmbFiltroGrupo.TabIndex = 5
        '
        'lblFiltroGrupo
        '
        Me.lblFiltroGrupo.AutoSize = True
        Me.lblFiltroGrupo.Location = New System.Drawing.Point(498, 12)
        Me.lblFiltroGrupo.Name = "lblFiltroGrupo"
        Me.lblFiltroGrupo.Size = New System.Drawing.Size(36, 13)
        Me.lblFiltroGrupo.TabIndex = 4
        Me.lblFiltroGrupo.Text = "Grupo"
        '
        'cmbFiltroSubCategoria
        '
        Me.cmbFiltroSubCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroSubCategoria.FormattingEnabled = True
        Me.cmbFiltroSubCategoria.Location = New System.Drawing.Point(318, 8)
        Me.cmbFiltroSubCategoria.Name = "cmbFiltroSubCategoria"
        Me.cmbFiltroSubCategoria.Size = New System.Drawing.Size(160, 21)
        Me.cmbFiltroSubCategoria.TabIndex = 3
        '
        'lblFiltroSubCategoria
        '
        Me.lblFiltroSubCategoria.AutoSize = True
        Me.lblFiltroSubCategoria.Location = New System.Drawing.Point(243, 12)
        Me.lblFiltroSubCategoria.Name = "lblFiltroSubCategoria"
        Me.lblFiltroSubCategoria.Size = New System.Drawing.Size(72, 13)
        Me.lblFiltroSubCategoria.TabIndex = 2
        Me.lblFiltroSubCategoria.Text = "Subcategoría"
        '
        'cmbFiltroCategoria
        '
        Me.cmbFiltroCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFiltroCategoria.FormattingEnabled = True
        Me.cmbFiltroCategoria.Location = New System.Drawing.Point(80, 8)
        Me.cmbFiltroCategoria.Name = "cmbFiltroCategoria"
        Me.cmbFiltroCategoria.Size = New System.Drawing.Size(150, 21)
        Me.cmbFiltroCategoria.TabIndex = 1
        '
        'lblFiltroCategoria
        '
        Me.lblFiltroCategoria.AutoSize = True
        Me.lblFiltroCategoria.Location = New System.Drawing.Point(11, 12)
        Me.lblFiltroCategoria.Name = "lblFiltroCategoria"
        Me.lblFiltroCategoria.Size = New System.Drawing.Size(54, 13)
        Me.lblFiltroCategoria.TabIndex = 0
        Me.lblFiltroCategoria.Text = "Categoría"
        '
        'pnlListaMaestro
        '
        Me.pnlListaMaestro.Location = New System.Drawing.Point(0, 0)
        Me.pnlListaMaestro.Name = "pnlListaMaestro"
        Me.pnlListaMaestro.Size = New System.Drawing.Size(200, 100)
        Me.pnlListaMaestro.TabIndex = 0
        '
        'lblTituloGrilla
        '
        Me.lblTituloGrilla.Location = New System.Drawing.Point(0, 0)
        Me.lblTituloGrilla.Name = "lblTituloGrilla"
        Me.lblTituloGrilla.Size = New System.Drawing.Size(100, 23)
        Me.lblTituloGrilla.TabIndex = 0
        '
        'dgvProductos
        '
        Me.dgvProductos.AllowUserToAddRows = False
        Me.dgvProductos.AllowUserToDeleteRows = False
        Me.dgvProductos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProductos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvProductos.Location = New System.Drawing.Point(0, 71)
        Me.dgvProductos.Name = "dgvProductos"
        Me.dgvProductos.ReadOnly = True
        Me.dgvProductos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvProductos.Size = New System.Drawing.Size(1170, 194)
        Me.dgvProductos.TabIndex = 1
        '
        'pnlEdicion
        '
        Me.pnlEdicion.Controls.Add(Me.btnDesactivar)
        Me.pnlEdicion.Controls.Add(Me.btnActivar)
        Me.pnlEdicion.Controls.Add(Me.btnCancelar)
        Me.pnlEdicion.Controls.Add(Me.btnGuardar)
        Me.pnlEdicion.Controls.Add(Me.grpDatos)
        Me.pnlEdicion.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlEdicion.Location = New System.Drawing.Point(0, 265)
        Me.pnlEdicion.Name = "pnlEdicion"
        Me.pnlEdicion.Padding = New System.Windows.Forms.Padding(8, 4, 8, 8)
        Me.pnlEdicion.Size = New System.Drawing.Size(1170, 485)
        Me.pnlEdicion.TabIndex = 2
        '
        'btnDesactivar
        '
        Me.btnDesactivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDesactivar.Location = New System.Drawing.Point(781, 433)
        Me.btnDesactivar.Name = "btnDesactivar"
        Me.btnDesactivar.Size = New System.Drawing.Size(85, 28)
        Me.btnDesactivar.TabIndex = 4
        Me.btnDesactivar.Text = "Desactivar"
        Me.btnDesactivar.UseVisualStyleBackColor = True
        '
        'btnActivar
        '
        Me.btnActivar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnActivar.Location = New System.Drawing.Point(872, 433)
        Me.btnActivar.Name = "btnActivar"
        Me.btnActivar.Size = New System.Drawing.Size(85, 28)
        Me.btnActivar.TabIndex = 3
        Me.btnActivar.Text = "Activar"
        Me.btnActivar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(963, 433)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(85, 28)
        Me.btnCancelar.TabIndex = 2
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(1054, 433)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(100, 28)
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
        Me.grpDatos.Controls.Add(Me.chkControlaStock)
        Me.grpDatos.Controls.Add(Me.chkPermiteStockNegativo)
        Me.grpDatos.Controls.Add(Me.txtCostoUltimo)
        Me.grpDatos.Controls.Add(Me.lblCostoUltimo)
        Me.grpDatos.Controls.Add(Me.txtSifenRepr)
        Me.grpDatos.Controls.Add(Me.txtSifenDescrip)
        Me.grpDatos.Controls.Add(Me.txtSifenCodigo)
        Me.grpDatos.Controls.Add(Me.lblSifenTitulo)
        Me.grpDatos.Controls.Add(Me.cmbUnidadMedida)
        Me.grpDatos.Controls.Add(Me.lblUnidadMedida)
        Me.grpDatos.Controls.Add(Me.cmbMarca)
        Me.grpDatos.Controls.Add(Me.lblMarca)
        Me.grpDatos.Controls.Add(Me.cmbImpuesto)
        Me.grpDatos.Controls.Add(Me.lblImpuesto)
        Me.grpDatos.Controls.Add(Me.cmbGrupo)
        Me.grpDatos.Controls.Add(Me.lblGrupo)
        Me.grpDatos.Controls.Add(Me.cmbSubCategoria)
        Me.grpDatos.Controls.Add(Me.lblSubCategoria)
        Me.grpDatos.Controls.Add(Me.cmbCategoria)
        Me.grpDatos.Controls.Add(Me.lblCategoria)
        Me.grpDatos.Controls.Add(Me.txtDescripcion)
        Me.grpDatos.Controls.Add(Me.lblDescripcion)
        Me.grpDatos.Controls.Add(Me.btnQuitarCodigoBarra)
        Me.grpDatos.Controls.Add(Me.btnAgregarCodigoBarra)
        Me.grpDatos.Controls.Add(Me.txtNuevoCodigoBarra)
        Me.grpDatos.Controls.Add(Me.lstCodigosBarras)
        Me.grpDatos.Controls.Add(Me.lblCodigoBarras)
        Me.grpDatos.Controls.Add(Me.txtCodigo)
        Me.grpDatos.Controls.Add(Me.lblCodigo)
        Me.grpDatos.Controls.Add(Me.chkEsServicio)
        Me.grpDatos.Controls.Add(Me.chkActivo)
        Me.grpDatos.Controls.Add(Me.lblIdValor)
        Me.grpDatos.Controls.Add(Me.lblIdTitulo)
        Me.grpDatos.Location = New System.Drawing.Point(11, 6)
        Me.grpDatos.Name = "grpDatos"
        Me.grpDatos.Size = New System.Drawing.Size(1148, 421)
        Me.grpDatos.TabIndex = 0
        Me.grpDatos.TabStop = False
        Me.grpDatos.Text = "Datos del producto"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObservaciones.Location = New System.Drawing.Point(833, 121)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(275, 78)
        Me.txtObservaciones.TabIndex = 31
        '
        'lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(749, 124)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(78, 13)
        Me.lblObservaciones.TabIndex = 30
        Me.lblObservaciones.Text = "Observaciones"
        '
        'chkControlaStock
        '
        Me.chkControlaStock.AutoSize = True
        Me.chkControlaStock.Location = New System.Drawing.Point(616, 23)
        Me.chkControlaStock.Name = "chkControlaStock"
        Me.chkControlaStock.Size = New System.Drawing.Size(94, 17)
        Me.chkControlaStock.TabIndex = 29
        Me.chkControlaStock.Text = "Controla stock"
        Me.chkControlaStock.UseVisualStyleBackColor = True
        '
        'chkPermiteStockNegativo
        '
        Me.chkPermiteStockNegativo.AutoSize = True
        Me.chkPermiteStockNegativo.Location = New System.Drawing.Point(432, 23)
        Me.chkPermiteStockNegativo.Name = "chkPermiteStockNegativo"
        Me.chkPermiteStockNegativo.Size = New System.Drawing.Size(134, 17)
        Me.chkPermiteStockNegativo.TabIndex = 28
        Me.chkPermiteStockNegativo.Text = "Permite stock negativo"
        Me.chkPermiteStockNegativo.UseVisualStyleBackColor = True
        '
        'txtCostoUltimo
        '
        Me.txtCostoUltimo.Location = New System.Drawing.Point(404, 148)
        Me.txtCostoUltimo.Name = "txtCostoUltimo"
        Me.txtCostoUltimo.Size = New System.Drawing.Size(90, 20)
        Me.txtCostoUltimo.TabIndex = 23
        '
        'lblCostoUltimo
        '
        Me.lblCostoUltimo.AutoSize = True
        Me.lblCostoUltimo.Location = New System.Drawing.Point(304, 151)
        Me.lblCostoUltimo.Name = "lblCostoUltimo"
        Me.lblCostoUltimo.Size = New System.Drawing.Size(64, 13)
        Me.lblCostoUltimo.TabIndex = 22
        Me.lblCostoUltimo.Text = "Costo último"
        '
        'txtSifenRepr
        '
        Me.txtSifenRepr.Location = New System.Drawing.Point(642, 121)
        Me.txtSifenRepr.Name = "txtSifenRepr"
        Me.txtSifenRepr.ReadOnly = True
        Me.txtSifenRepr.Size = New System.Drawing.Size(100, 20)
        Me.txtSifenRepr.TabIndex = 21
        Me.txtSifenRepr.TabStop = False
        '
        'txtSifenDescrip
        '
        Me.txtSifenDescrip.Location = New System.Drawing.Point(412, 121)
        Me.txtSifenDescrip.Name = "txtSifenDescrip"
        Me.txtSifenDescrip.ReadOnly = True
        Me.txtSifenDescrip.Size = New System.Drawing.Size(220, 20)
        Me.txtSifenDescrip.TabIndex = 20
        Me.txtSifenDescrip.TabStop = False
        '
        'txtSifenCodigo
        '
        Me.txtSifenCodigo.Location = New System.Drawing.Point(344, 121)
        Me.txtSifenCodigo.Name = "txtSifenCodigo"
        Me.txtSifenCodigo.ReadOnly = True
        Me.txtSifenCodigo.Size = New System.Drawing.Size(56, 20)
        Me.txtSifenCodigo.TabIndex = 19
        Me.txtSifenCodigo.TabStop = False
        '
        'lblSifenTitulo
        '
        Me.lblSifenTitulo.AutoSize = True
        Me.lblSifenTitulo.Location = New System.Drawing.Point(300, 123)
        Me.lblSifenTitulo.Name = "lblSifenTitulo"
        Me.lblSifenTitulo.Size = New System.Drawing.Size(38, 13)
        Me.lblSifenTitulo.TabIndex = 18
        Me.lblSifenTitulo.Text = "SIFEN"
        '
        'cmbUnidadMedida
        '
        Me.cmbUnidadMedida.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbUnidadMedida.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbUnidadMedida.FormattingEnabled = True
        Me.cmbUnidadMedida.Location = New System.Drawing.Point(78, 121)
        Me.cmbUnidadMedida.Name = "cmbUnidadMedida"
        Me.cmbUnidadMedida.Size = New System.Drawing.Size(216, 21)
        Me.cmbUnidadMedida.TabIndex = 17
        '
        'lblUnidadMedida
        '
        Me.lblUnidadMedida.AutoSize = True
        Me.lblUnidadMedida.Location = New System.Drawing.Point(31, 124)
        Me.lblUnidadMedida.Name = "lblUnidadMedida"
        Me.lblUnidadMedida.Size = New System.Drawing.Size(41, 13)
        Me.lblUnidadMedida.TabIndex = 16
        Me.lblUnidadMedida.Text = "Unidad"
        '
        'cmbMarca
        '
        Me.cmbMarca.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMarca.FormattingEnabled = True
        Me.cmbMarca.Location = New System.Drawing.Point(893, 90)
        Me.cmbMarca.Name = "cmbMarca"
        Me.cmbMarca.Size = New System.Drawing.Size(215, 21)
        Me.cmbMarca.TabIndex = 15
        '
        'lblMarca
        '
        Me.lblMarca.AutoSize = True
        Me.lblMarca.Location = New System.Drawing.Point(855, 94)
        Me.lblMarca.Name = "lblMarca"
        Me.lblMarca.Size = New System.Drawing.Size(37, 13)
        Me.lblMarca.TabIndex = 14
        Me.lblMarca.Text = "Marca"
        '
        'cmbImpuesto
        '
        Me.cmbImpuesto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbImpuesto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbImpuesto.FormattingEnabled = True
        Me.cmbImpuesto.Location = New System.Drawing.Point(78, 148)
        Me.cmbImpuesto.Name = "cmbImpuesto"
        Me.cmbImpuesto.Size = New System.Drawing.Size(216, 21)
        Me.cmbImpuesto.TabIndex = 33
        '
        'lblImpuesto
        '
        Me.lblImpuesto.AutoSize = True
        Me.lblImpuesto.Location = New System.Drawing.Point(22, 151)
        Me.lblImpuesto.Name = "lblImpuesto"
        Me.lblImpuesto.Size = New System.Drawing.Size(50, 13)
        Me.lblImpuesto.TabIndex = 32
        Me.lblImpuesto.Text = "Impuesto"
        '
        'cmbGrupo
        '
        Me.cmbGrupo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbGrupo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGrupo.FormattingEnabled = True
        Me.cmbGrupo.Location = New System.Drawing.Point(633, 90)
        Me.cmbGrupo.Name = "cmbGrupo"
        Me.cmbGrupo.Size = New System.Drawing.Size(217, 21)
        Me.cmbGrupo.TabIndex = 13
        '
        'lblGrupo
        '
        Me.lblGrupo.AutoSize = True
        Me.lblGrupo.Location = New System.Drawing.Point(596, 94)
        Me.lblGrupo.Name = "lblGrupo"
        Me.lblGrupo.Size = New System.Drawing.Size(36, 13)
        Me.lblGrupo.TabIndex = 12
        Me.lblGrupo.Text = "Grupo"
        '
        'cmbSubCategoria
        '
        Me.cmbSubCategoria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbSubCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSubCategoria.FormattingEnabled = True
        Me.cmbSubCategoria.Location = New System.Drawing.Point(373, 90)
        Me.cmbSubCategoria.Name = "cmbSubCategoria"
        Me.cmbSubCategoria.Size = New System.Drawing.Size(217, 21)
        Me.cmbSubCategoria.TabIndex = 11
        '
        'lblSubCategoria
        '
        Me.lblSubCategoria.AutoSize = True
        Me.lblSubCategoria.Location = New System.Drawing.Point(300, 94)
        Me.lblSubCategoria.Name = "lblSubCategoria"
        Me.lblSubCategoria.Size = New System.Drawing.Size(72, 13)
        Me.lblSubCategoria.TabIndex = 10
        Me.lblSubCategoria.Text = "Subcategoría"
        '
        'cmbCategoria
        '
        Me.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCategoria.FormattingEnabled = True
        Me.cmbCategoria.Location = New System.Drawing.Point(78, 90)
        Me.cmbCategoria.Name = "cmbCategoria"
        Me.cmbCategoria.Size = New System.Drawing.Size(217, 21)
        Me.cmbCategoria.TabIndex = 9
        '
        'lblCategoria
        '
        Me.lblCategoria.AutoSize = True
        Me.lblCategoria.Location = New System.Drawing.Point(18, 93)
        Me.lblCategoria.Name = "lblCategoria"
        Me.lblCategoria.Size = New System.Drawing.Size(54, 13)
        Me.lblCategoria.TabIndex = 8
        Me.lblCategoria.Text = "Categoría"
        '
        'txtDescripcion
        '
        Me.txtDescripcion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcion.Location = New System.Drawing.Point(273, 60)
        Me.txtDescripcion.Name = "txtDescripcion"
        Me.txtDescripcion.Size = New System.Drawing.Size(814, 20)
        Me.txtDescripcion.TabIndex = 7
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Location = New System.Drawing.Point(204, 63)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(63, 13)
        Me.lblDescripcion.TabIndex = 6
        Me.lblDescripcion.Text = "Descripción"
        '
        'btnQuitarCodigoBarra
        '
        Me.btnQuitarCodigoBarra.Location = New System.Drawing.Point(273, 233)
        Me.btnQuitarCodigoBarra.Name = "btnQuitarCodigoBarra"
        Me.btnQuitarCodigoBarra.Size = New System.Drawing.Size(60, 23)
        Me.btnQuitarCodigoBarra.TabIndex = 8
        Me.btnQuitarCodigoBarra.Text = "Quitar"
        Me.btnQuitarCodigoBarra.UseVisualStyleBackColor = True
        '
        'btnAgregarCodigoBarra
        '
        Me.btnAgregarCodigoBarra.Location = New System.Drawing.Point(273, 205)
        Me.btnAgregarCodigoBarra.Name = "btnAgregarCodigoBarra"
        Me.btnAgregarCodigoBarra.Size = New System.Drawing.Size(60, 23)
        Me.btnAgregarCodigoBarra.TabIndex = 7
        Me.btnAgregarCodigoBarra.Text = "Añadir"
        Me.btnAgregarCodigoBarra.UseVisualStyleBackColor = True
        '
        'txtNuevoCodigoBarra
        '
        Me.txtNuevoCodigoBarra.Location = New System.Drawing.Point(273, 179)
        Me.txtNuevoCodigoBarra.Name = "txtNuevoCodigoBarra"
        Me.txtNuevoCodigoBarra.Size = New System.Drawing.Size(109, 20)
        Me.txtNuevoCodigoBarra.TabIndex = 6
        '
        'lstCodigosBarras
        '
        Me.lstCodigosBarras.FormattingEnabled = True
        Me.lstCodigosBarras.Location = New System.Drawing.Point(104, 179)
        Me.lstCodigosBarras.Name = "lstCodigosBarras"
        Me.lstCodigosBarras.Size = New System.Drawing.Size(163, 121)
        Me.lstCodigosBarras.TabIndex = 5
        '
        'lblCodigoBarras
        '
        Me.lblCodigoBarras.AutoSize = True
        Me.lblCodigoBarras.Location = New System.Drawing.Point(6, 179)
        Me.lblCodigoBarras.Name = "lblCodigoBarras"
        Me.lblCodigoBarras.Size = New System.Drawing.Size(92, 13)
        Me.lblCodigoBarras.TabIndex = 4
        Me.lblCodigoBarras.Text = "Códigos de barras"
        '
        'txtCodigo
        '
        Me.txtCodigo.Location = New System.Drawing.Point(78, 60)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(120, 20)
        Me.txtCodigo.TabIndex = 3
        '
        'lblCodigo
        '
        Me.lblCodigo.AutoSize = True
        Me.lblCodigo.Location = New System.Drawing.Point(32, 63)
        Me.lblCodigo.Name = "lblCodigo"
        Me.lblCodigo.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigo.TabIndex = 2
        Me.lblCodigo.Text = "Código"
        '
        'chkEsServicio
        '
        Me.chkEsServicio.AutoSize = True
        Me.chkEsServicio.Location = New System.Drawing.Point(320, 22)
        Me.chkEsServicio.Name = "chkEsServicio"
        Me.chkEsServicio.Size = New System.Drawing.Size(77, 17)
        Me.chkEsServicio.TabIndex = 1
        Me.chkEsServicio.Text = "Es servicio"
        Me.chkEsServicio.UseVisualStyleBackColor = True
        '
        'chkActivo
        '
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Enabled = False
        Me.chkActivo.Location = New System.Drawing.Point(200, 22)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(56, 17)
        Me.chkActivo.TabIndex = 0
        Me.chkActivo.Text = "Activo"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'lblIdValor
        '
        Me.lblIdValor.AutoSize = True
        Me.lblIdValor.Location = New System.Drawing.Point(45, 24)
        Me.lblIdValor.Name = "lblIdValor"
        Me.lblIdValor.Size = New System.Drawing.Size(13, 13)
        Me.lblIdValor.TabIndex = 0
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
        'FrmProductos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1170, 750)
        Me.Controls.Add(Me.dgvProductos)
        Me.Controls.Add(Me.pnlEdicion)
        Me.Controls.Add(Me.pnlBusqueda)
        Me.MinimizeBox = False
        Me.Name = "FrmProductos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Productos"
        Me.pnlBusqueda.ResumeLayout(False)
        Me.pnlBusqueda.PerformLayout()
        CType(Me.dgvProductos, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents cmbFiltroActivo As ComboBox
    Friend WithEvents lblFiltroActivo As Label
    Friend WithEvents cmbFiltroMarca As ComboBox
    Friend WithEvents lblFiltroMarca As Label
    Friend WithEvents cmbFiltroGrupo As ComboBox
    Friend WithEvents lblFiltroGrupo As Label
    Friend WithEvents cmbFiltroSubCategoria As ComboBox
    Friend WithEvents lblFiltroSubCategoria As Label
    Friend WithEvents cmbFiltroCategoria As ComboBox
    Friend WithEvents lblFiltroCategoria As Label
    Friend WithEvents pnlListaMaestro As Panel
    Friend WithEvents lblTituloGrilla As Label
    Friend WithEvents dgvProductos As DataGridView
    Friend WithEvents pnlEdicion As Panel
    Friend WithEvents btnDesactivar As Button
    Friend WithEvents btnActivar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents grpDatos As GroupBox
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents lblObservaciones As Label
    Friend WithEvents chkControlaStock As CheckBox
    Friend WithEvents chkPermiteStockNegativo As CheckBox
    Friend WithEvents txtCostoUltimo As TextBox
    Friend WithEvents lblCostoUltimo As Label
    Friend WithEvents txtSifenRepr As TextBox
    Friend WithEvents txtSifenDescrip As TextBox
    Friend WithEvents txtSifenCodigo As TextBox
    Friend WithEvents lblSifenTitulo As Label
    Friend WithEvents cmbUnidadMedida As ComboBox
    Friend WithEvents lblUnidadMedida As Label
    Friend WithEvents cmbMarca As ComboBox
    Friend WithEvents lblMarca As Label
    Friend WithEvents cmbImpuesto As ComboBox
    Friend WithEvents lblImpuesto As Label
    Friend WithEvents cmbGrupo As ComboBox
    Friend WithEvents lblGrupo As Label
    Friend WithEvents cmbSubCategoria As ComboBox
    Friend WithEvents lblSubCategoria As Label
    Friend WithEvents cmbCategoria As ComboBox
    Friend WithEvents lblCategoria As Label
    Friend WithEvents txtDescripcion As TextBox
    Friend WithEvents lblDescripcion As Label
    Friend WithEvents btnQuitarCodigoBarra As Button
    Friend WithEvents btnAgregarCodigoBarra As Button
    Friend WithEvents txtNuevoCodigoBarra As TextBox
    Friend WithEvents lstCodigosBarras As ListBox
    Friend WithEvents lblCodigoBarras As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents lblCodigo As Label
    Friend WithEvents chkEsServicio As CheckBox
    Friend WithEvents chkActivo As CheckBox
    Friend WithEvents lblIdValor As Label
    Friend WithEvents lblIdTitulo As Label
End Class
