<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmClasificacionProductos
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.tlpColumnas = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlColCategoria = New System.Windows.Forms.Panel()
        Me.flpBotonesCategoria = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAgregarCategoria = New System.Windows.Forms.Button()
        Me.btnEditarCategoria = New System.Windows.Forms.Button()
        Me.btnGuardarCategoria = New System.Windows.Forms.Button()
        Me.grpEdicionCategoria = New System.Windows.Forms.GroupBox()
        Me.chkActivoCategoria = New System.Windows.Forms.CheckBox()
        Me.txtDescripcionCategoria = New System.Windows.Forms.TextBox()
        Me.txtCodigoCategoria = New System.Windows.Forms.TextBox()
        Me.lblDescripcionCategoria = New System.Windows.Forms.Label()
        Me.lblCodigoCategoria = New System.Windows.Forms.Label()
        Me.dgvCategorias = New System.Windows.Forms.DataGridView()
        Me.txtFiltroCategoria = New System.Windows.Forms.TextBox()
        Me.lblHeadCategoria = New System.Windows.Forms.Label()
        Me.pnlColSub = New System.Windows.Forms.Panel()
        Me.flpBotonesSub = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAgregarSub = New System.Windows.Forms.Button()
        Me.btnEditarSub = New System.Windows.Forms.Button()
        Me.btnGuardarSub = New System.Windows.Forms.Button()
        Me.grpEdicionSub = New System.Windows.Forms.GroupBox()
        Me.chkActivoSub = New System.Windows.Forms.CheckBox()
        Me.txtDescripcionSub = New System.Windows.Forms.TextBox()
        Me.txtCodigoSub = New System.Windows.Forms.TextBox()
        Me.lblDescripcionSub = New System.Windows.Forms.Label()
        Me.lblCodigoSub = New System.Windows.Forms.Label()
        Me.dgvSubcategorias = New System.Windows.Forms.DataGridView()
        Me.txtFiltroSub = New System.Windows.Forms.TextBox()
        Me.lblHeadSub = New System.Windows.Forms.Label()
        Me.pnlColGrupo = New System.Windows.Forms.Panel()
        Me.flpBotonesGrupo = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAgregarGrupo = New System.Windows.Forms.Button()
        Me.btnEditarGrupo = New System.Windows.Forms.Button()
        Me.btnGuardarGrupo = New System.Windows.Forms.Button()
        Me.grpEdicionGrupo = New System.Windows.Forms.GroupBox()
        Me.chkActivoGrupo = New System.Windows.Forms.CheckBox()
        Me.txtDescripcionGrupo = New System.Windows.Forms.TextBox()
        Me.txtCodigoGrupo = New System.Windows.Forms.TextBox()
        Me.lblDescripcionGrupo = New System.Windows.Forms.Label()
        Me.lblCodigoGrupo = New System.Windows.Forms.Label()
        Me.dgvGrupos = New System.Windows.Forms.DataGridView()
        Me.txtFiltroGrupo = New System.Windows.Forms.TextBox()
        Me.lblHeadGrupo = New System.Windows.Forms.Label()
        Me.pnlBottom = New System.Windows.Forms.Panel()
        Me.btnCancelarEdicion = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.pnlTop.SuspendLayout()
        Me.tlpColumnas.SuspendLayout()
        Me.pnlColCategoria.SuspendLayout()
        Me.flpBotonesCategoria.SuspendLayout()
        Me.grpEdicionCategoria.SuspendLayout()
        CType(Me.dgvCategorias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlColSub.SuspendLayout()
        Me.flpBotonesSub.SuspendLayout()
        Me.grpEdicionSub.SuspendLayout()
        CType(Me.dgvSubcategorias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlColGrupo.SuspendLayout()
        Me.flpBotonesGrupo.SuspendLayout()
        Me.grpEdicionGrupo.SuspendLayout()
        CType(Me.dgvGrupos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.Controls.Add(Me.lblInfo)
        Me.pnlTop.Controls.Add(Me.lblTitulo)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Padding = New System.Windows.Forms.Padding(12, 10, 12, 8)
        Me.pnlTop.Size = New System.Drawing.Size(1184, 56)
        Me.pnlTop.TabIndex = 0
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.Location = New System.Drawing.Point(12, 30)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(1160, 22)
        Me.lblInfo.TabIndex = 1
        Me.lblInfo.Text = "Elija una fila en la primera columna para filtrar la segunda, y una en la segunda" &
    " para filtrar la tercera. Código y descripción por nivel; en alta, el código se " &
    "sugiere automáticamente."
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitulo.Location = New System.Drawing.Point(12, 10)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(378, 17)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Clasificación en cascada: Categoría → Subcategoría → Grupo"
        '
        'tlpColumnas
        '
        Me.tlpColumnas.ColumnCount = 3
        Me.tlpColumnas.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.tlpColumnas.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33!))
        Me.tlpColumnas.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34!))
        Me.tlpColumnas.Controls.Add(Me.pnlColCategoria, 0, 0)
        Me.tlpColumnas.Controls.Add(Me.pnlColSub, 1, 0)
        Me.tlpColumnas.Controls.Add(Me.pnlColGrupo, 2, 0)
        Me.tlpColumnas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpColumnas.Location = New System.Drawing.Point(0, 56)
        Me.tlpColumnas.Name = "tlpColumnas"
        Me.tlpColumnas.Padding = New System.Windows.Forms.Padding(8, 0, 8, 0)
        Me.tlpColumnas.RowCount = 1
        Me.tlpColumnas.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpColumnas.Size = New System.Drawing.Size(1184, 505)
        Me.tlpColumnas.TabIndex = 1
        '
        'pnlColCategoria
        '
        Me.pnlColCategoria.Controls.Add(Me.flpBotonesCategoria)
        Me.pnlColCategoria.Controls.Add(Me.grpEdicionCategoria)
        Me.pnlColCategoria.Controls.Add(Me.dgvCategorias)
        Me.pnlColCategoria.Controls.Add(Me.txtFiltroCategoria)
        Me.pnlColCategoria.Controls.Add(Me.lblHeadCategoria)
        Me.pnlColCategoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlColCategoria.Location = New System.Drawing.Point(11, 3)
        Me.pnlColCategoria.Name = "pnlColCategoria"
        Me.pnlColCategoria.Padding = New System.Windows.Forms.Padding(0, 0, 6, 0)
        Me.pnlColCategoria.Size = New System.Drawing.Size(383, 499)
        Me.pnlColCategoria.TabIndex = 0
        '
        'flpBotonesCategoria
        '
        Me.flpBotonesCategoria.Controls.Add(Me.btnAgregarCategoria)
        Me.flpBotonesCategoria.Controls.Add(Me.btnEditarCategoria)
        Me.flpBotonesCategoria.Controls.Add(Me.btnGuardarCategoria)
        Me.flpBotonesCategoria.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.flpBotonesCategoria.Location = New System.Drawing.Point(0, 347)
        Me.flpBotonesCategoria.Name = "flpBotonesCategoria"
        Me.flpBotonesCategoria.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.flpBotonesCategoria.Size = New System.Drawing.Size(377, 44)
        Me.flpBotonesCategoria.TabIndex = 4
        '
        'btnAgregarCategoria
        '
        Me.btnAgregarCategoria.Location = New System.Drawing.Point(3, 7)
        Me.btnAgregarCategoria.Name = "btnAgregarCategoria"
        Me.btnAgregarCategoria.Size = New System.Drawing.Size(88, 28)
        Me.btnAgregarCategoria.TabIndex = 0
        Me.btnAgregarCategoria.Text = "Agregar"
        Me.btnAgregarCategoria.UseVisualStyleBackColor = True
        '
        'btnEditarCategoria
        '
        Me.btnEditarCategoria.Location = New System.Drawing.Point(97, 7)
        Me.btnEditarCategoria.Name = "btnEditarCategoria"
        Me.btnEditarCategoria.Size = New System.Drawing.Size(88, 28)
        Me.btnEditarCategoria.TabIndex = 1
        Me.btnEditarCategoria.Text = "Editar"
        Me.btnEditarCategoria.UseVisualStyleBackColor = True
        '
        'btnGuardarCategoria
        '
        Me.btnGuardarCategoria.Location = New System.Drawing.Point(191, 7)
        Me.btnGuardarCategoria.Name = "btnGuardarCategoria"
        Me.btnGuardarCategoria.Size = New System.Drawing.Size(88, 28)
        Me.btnGuardarCategoria.TabIndex = 2
        Me.btnGuardarCategoria.Text = "Guardar"
        Me.btnGuardarCategoria.UseVisualStyleBackColor = True
        '
        'grpEdicionCategoria
        '
        Me.grpEdicionCategoria.Controls.Add(Me.chkActivoCategoria)
        Me.grpEdicionCategoria.Controls.Add(Me.txtDescripcionCategoria)
        Me.grpEdicionCategoria.Controls.Add(Me.txtCodigoCategoria)
        Me.grpEdicionCategoria.Controls.Add(Me.lblDescripcionCategoria)
        Me.grpEdicionCategoria.Controls.Add(Me.lblCodigoCategoria)
        Me.grpEdicionCategoria.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpEdicionCategoria.Location = New System.Drawing.Point(0, 391)
        Me.grpEdicionCategoria.Name = "grpEdicionCategoria"
        Me.grpEdicionCategoria.Size = New System.Drawing.Size(377, 108)
        Me.grpEdicionCategoria.TabIndex = 3
        Me.grpEdicionCategoria.TabStop = False
        Me.grpEdicionCategoria.Text = "Edición"
        '
        'chkActivoCategoria
        '
        Me.chkActivoCategoria.AutoSize = True
        Me.chkActivoCategoria.Location = New System.Drawing.Point(268, 22)
        Me.chkActivoCategoria.Name = "chkActivoCategoria"
        Me.chkActivoCategoria.Size = New System.Drawing.Size(56, 17)
        Me.chkActivoCategoria.TabIndex = 2
        Me.chkActivoCategoria.Text = "Activo"
        Me.chkActivoCategoria.UseVisualStyleBackColor = True
        '
        'txtDescripcionCategoria
        '
        Me.txtDescripcionCategoria.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionCategoria.Location = New System.Drawing.Point(88, 48)
        Me.txtDescripcionCategoria.Name = "txtDescripcionCategoria"
        Me.txtDescripcionCategoria.Size = New System.Drawing.Size(277, 20)
        Me.txtDescripcionCategoria.TabIndex = 4
        '
        'txtCodigoCategoria
        '
        Me.txtCodigoCategoria.Location = New System.Drawing.Point(88, 22)
        Me.txtCodigoCategoria.Name = "txtCodigoCategoria"
        Me.txtCodigoCategoria.Size = New System.Drawing.Size(96, 20)
        Me.txtCodigoCategoria.TabIndex = 1
        '
        'lblDescripcionCategoria
        '
        Me.lblDescripcionCategoria.AutoSize = True
        Me.lblDescripcionCategoria.Location = New System.Drawing.Point(12, 51)
        Me.lblDescripcionCategoria.Name = "lblDescripcionCategoria"
        Me.lblDescripcionCategoria.Size = New System.Drawing.Size(63, 13)
        Me.lblDescripcionCategoria.TabIndex = 3
        Me.lblDescripcionCategoria.Text = "Descripción"
        '
        'lblCodigoCategoria
        '
        Me.lblCodigoCategoria.AutoSize = True
        Me.lblCodigoCategoria.Location = New System.Drawing.Point(12, 25)
        Me.lblCodigoCategoria.Name = "lblCodigoCategoria"
        Me.lblCodigoCategoria.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigoCategoria.TabIndex = 0
        Me.lblCodigoCategoria.Text = "Código"
        '
        'dgvCategorias
        '
        Me.dgvCategorias.AllowUserToAddRows = False
        Me.dgvCategorias.AllowUserToDeleteRows = False
        Me.dgvCategorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCategorias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCategorias.Location = New System.Drawing.Point(0, 46)
        Me.dgvCategorias.MultiSelect = False
        Me.dgvCategorias.Name = "dgvCategorias"
        Me.dgvCategorias.ReadOnly = True
        Me.dgvCategorias.RowHeadersVisible = False
        Me.dgvCategorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCategorias.Size = New System.Drawing.Size(377, 453)
        Me.dgvCategorias.TabIndex = 2
        '
        'txtFiltroCategoria
        '
        Me.txtFiltroCategoria.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtFiltroCategoria.Location = New System.Drawing.Point(0, 26)
        Me.txtFiltroCategoria.Name = "txtFiltroCategoria"
        Me.txtFiltroCategoria.Size = New System.Drawing.Size(377, 20)
        Me.txtFiltroCategoria.TabIndex = 1
        '
        'lblHeadCategoria
        '
        Me.lblHeadCategoria.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblHeadCategoria.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHeadCategoria.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeadCategoria.ForeColor = System.Drawing.Color.White
        Me.lblHeadCategoria.Location = New System.Drawing.Point(0, 0)
        Me.lblHeadCategoria.Name = "lblHeadCategoria"
        Me.lblHeadCategoria.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblHeadCategoria.Size = New System.Drawing.Size(377, 26)
        Me.lblHeadCategoria.TabIndex = 0
        Me.lblHeadCategoria.Text = "Categoría"
        Me.lblHeadCategoria.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlColSub
        '
        Me.pnlColSub.Controls.Add(Me.flpBotonesSub)
        Me.pnlColSub.Controls.Add(Me.grpEdicionSub)
        Me.pnlColSub.Controls.Add(Me.dgvSubcategorias)
        Me.pnlColSub.Controls.Add(Me.txtFiltroSub)
        Me.pnlColSub.Controls.Add(Me.lblHeadSub)
        Me.pnlColSub.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlColSub.Location = New System.Drawing.Point(400, 3)
        Me.pnlColSub.Name = "pnlColSub"
        Me.pnlColSub.Padding = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.pnlColSub.Size = New System.Drawing.Size(383, 499)
        Me.pnlColSub.TabIndex = 1
        '
        'flpBotonesSub
        '
        Me.flpBotonesSub.Controls.Add(Me.btnAgregarSub)
        Me.flpBotonesSub.Controls.Add(Me.btnEditarSub)
        Me.flpBotonesSub.Controls.Add(Me.btnGuardarSub)
        Me.flpBotonesSub.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.flpBotonesSub.Location = New System.Drawing.Point(6, 347)
        Me.flpBotonesSub.Name = "flpBotonesSub"
        Me.flpBotonesSub.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.flpBotonesSub.Size = New System.Drawing.Size(371, 44)
        Me.flpBotonesSub.TabIndex = 4
        '
        'btnAgregarSub
        '
        Me.btnAgregarSub.Location = New System.Drawing.Point(3, 7)
        Me.btnAgregarSub.Name = "btnAgregarSub"
        Me.btnAgregarSub.Size = New System.Drawing.Size(88, 28)
        Me.btnAgregarSub.TabIndex = 0
        Me.btnAgregarSub.Text = "Agregar"
        Me.btnAgregarSub.UseVisualStyleBackColor = True
        '
        'btnEditarSub
        '
        Me.btnEditarSub.Location = New System.Drawing.Point(97, 7)
        Me.btnEditarSub.Name = "btnEditarSub"
        Me.btnEditarSub.Size = New System.Drawing.Size(88, 28)
        Me.btnEditarSub.TabIndex = 1
        Me.btnEditarSub.Text = "Editar"
        Me.btnEditarSub.UseVisualStyleBackColor = True
        '
        'btnGuardarSub
        '
        Me.btnGuardarSub.Location = New System.Drawing.Point(191, 7)
        Me.btnGuardarSub.Name = "btnGuardarSub"
        Me.btnGuardarSub.Size = New System.Drawing.Size(88, 28)
        Me.btnGuardarSub.TabIndex = 2
        Me.btnGuardarSub.Text = "Guardar"
        Me.btnGuardarSub.UseVisualStyleBackColor = True
        '
        'grpEdicionSub
        '
        Me.grpEdicionSub.Controls.Add(Me.chkActivoSub)
        Me.grpEdicionSub.Controls.Add(Me.txtDescripcionSub)
        Me.grpEdicionSub.Controls.Add(Me.txtCodigoSub)
        Me.grpEdicionSub.Controls.Add(Me.lblDescripcionSub)
        Me.grpEdicionSub.Controls.Add(Me.lblCodigoSub)
        Me.grpEdicionSub.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpEdicionSub.Location = New System.Drawing.Point(6, 391)
        Me.grpEdicionSub.Name = "grpEdicionSub"
        Me.grpEdicionSub.Size = New System.Drawing.Size(371, 108)
        Me.grpEdicionSub.TabIndex = 3
        Me.grpEdicionSub.TabStop = False
        Me.grpEdicionSub.Text = "Edición"
        '
        'chkActivoSub
        '
        Me.chkActivoSub.AutoSize = True
        Me.chkActivoSub.Location = New System.Drawing.Point(262, 22)
        Me.chkActivoSub.Name = "chkActivoSub"
        Me.chkActivoSub.Size = New System.Drawing.Size(56, 17)
        Me.chkActivoSub.TabIndex = 2
        Me.chkActivoSub.Text = "Activo"
        Me.chkActivoSub.UseVisualStyleBackColor = True
        '
        'txtDescripcionSub
        '
        Me.txtDescripcionSub.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionSub.Location = New System.Drawing.Point(88, 48)
        Me.txtDescripcionSub.Name = "txtDescripcionSub"
        Me.txtDescripcionSub.Size = New System.Drawing.Size(271, 20)
        Me.txtDescripcionSub.TabIndex = 4
        '
        'txtCodigoSub
        '
        Me.txtCodigoSub.Location = New System.Drawing.Point(88, 22)
        Me.txtCodigoSub.Name = "txtCodigoSub"
        Me.txtCodigoSub.Size = New System.Drawing.Size(96, 20)
        Me.txtCodigoSub.TabIndex = 1
        '
        'lblDescripcionSub
        '
        Me.lblDescripcionSub.AutoSize = True
        Me.lblDescripcionSub.Location = New System.Drawing.Point(12, 51)
        Me.lblDescripcionSub.Name = "lblDescripcionSub"
        Me.lblDescripcionSub.Size = New System.Drawing.Size(63, 13)
        Me.lblDescripcionSub.TabIndex = 3
        Me.lblDescripcionSub.Text = "Descripción"
        '
        'lblCodigoSub
        '
        Me.lblCodigoSub.AutoSize = True
        Me.lblCodigoSub.Location = New System.Drawing.Point(12, 25)
        Me.lblCodigoSub.Name = "lblCodigoSub"
        Me.lblCodigoSub.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigoSub.TabIndex = 0
        Me.lblCodigoSub.Text = "Código"
        '
        'dgvSubcategorias
        '
        Me.dgvSubcategorias.AllowUserToAddRows = False
        Me.dgvSubcategorias.AllowUserToDeleteRows = False
        Me.dgvSubcategorias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSubcategorias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSubcategorias.Location = New System.Drawing.Point(6, 46)
        Me.dgvSubcategorias.MultiSelect = False
        Me.dgvSubcategorias.Name = "dgvSubcategorias"
        Me.dgvSubcategorias.ReadOnly = True
        Me.dgvSubcategorias.RowHeadersVisible = False
        Me.dgvSubcategorias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSubcategorias.Size = New System.Drawing.Size(371, 453)
        Me.dgvSubcategorias.TabIndex = 2
        '
        'txtFiltroSub
        '
        Me.txtFiltroSub.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtFiltroSub.Location = New System.Drawing.Point(6, 26)
        Me.txtFiltroSub.Name = "txtFiltroSub"
        Me.txtFiltroSub.Size = New System.Drawing.Size(371, 20)
        Me.txtFiltroSub.TabIndex = 1
        '
        'lblHeadSub
        '
        Me.lblHeadSub.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblHeadSub.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHeadSub.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeadSub.ForeColor = System.Drawing.Color.White
        Me.lblHeadSub.Location = New System.Drawing.Point(6, 0)
        Me.lblHeadSub.Name = "lblHeadSub"
        Me.lblHeadSub.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblHeadSub.Size = New System.Drawing.Size(371, 26)
        Me.lblHeadSub.TabIndex = 0
        Me.lblHeadSub.Text = "Subcategoría"
        Me.lblHeadSub.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlColGrupo
        '
        Me.pnlColGrupo.Controls.Add(Me.flpBotonesGrupo)
        Me.pnlColGrupo.Controls.Add(Me.grpEdicionGrupo)
        Me.pnlColGrupo.Controls.Add(Me.dgvGrupos)
        Me.pnlColGrupo.Controls.Add(Me.txtFiltroGrupo)
        Me.pnlColGrupo.Controls.Add(Me.lblHeadGrupo)
        Me.pnlColGrupo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlColGrupo.Location = New System.Drawing.Point(789, 3)
        Me.pnlColGrupo.Name = "pnlColGrupo"
        Me.pnlColGrupo.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.pnlColGrupo.Size = New System.Drawing.Size(384, 499)
        Me.pnlColGrupo.TabIndex = 2
        '
        'flpBotonesGrupo
        '
        Me.flpBotonesGrupo.Controls.Add(Me.btnAgregarGrupo)
        Me.flpBotonesGrupo.Controls.Add(Me.btnEditarGrupo)
        Me.flpBotonesGrupo.Controls.Add(Me.btnGuardarGrupo)
        Me.flpBotonesGrupo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.flpBotonesGrupo.Location = New System.Drawing.Point(6, 347)
        Me.flpBotonesGrupo.Name = "flpBotonesGrupo"
        Me.flpBotonesGrupo.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
        Me.flpBotonesGrupo.Size = New System.Drawing.Size(378, 44)
        Me.flpBotonesGrupo.TabIndex = 4
        '
        'btnAgregarGrupo
        '
        Me.btnAgregarGrupo.Location = New System.Drawing.Point(3, 7)
        Me.btnAgregarGrupo.Name = "btnAgregarGrupo"
        Me.btnAgregarGrupo.Size = New System.Drawing.Size(88, 28)
        Me.btnAgregarGrupo.TabIndex = 0
        Me.btnAgregarGrupo.Text = "Agregar"
        Me.btnAgregarGrupo.UseVisualStyleBackColor = True
        '
        'btnEditarGrupo
        '
        Me.btnEditarGrupo.Location = New System.Drawing.Point(97, 7)
        Me.btnEditarGrupo.Name = "btnEditarGrupo"
        Me.btnEditarGrupo.Size = New System.Drawing.Size(88, 28)
        Me.btnEditarGrupo.TabIndex = 1
        Me.btnEditarGrupo.Text = "Editar"
        Me.btnEditarGrupo.UseVisualStyleBackColor = True
        '
        'btnGuardarGrupo
        '
        Me.btnGuardarGrupo.Location = New System.Drawing.Point(191, 7)
        Me.btnGuardarGrupo.Name = "btnGuardarGrupo"
        Me.btnGuardarGrupo.Size = New System.Drawing.Size(88, 28)
        Me.btnGuardarGrupo.TabIndex = 2
        Me.btnGuardarGrupo.Text = "Guardar"
        Me.btnGuardarGrupo.UseVisualStyleBackColor = True
        '
        'grpEdicionGrupo
        '
        Me.grpEdicionGrupo.Controls.Add(Me.chkActivoGrupo)
        Me.grpEdicionGrupo.Controls.Add(Me.txtDescripcionGrupo)
        Me.grpEdicionGrupo.Controls.Add(Me.txtCodigoGrupo)
        Me.grpEdicionGrupo.Controls.Add(Me.lblDescripcionGrupo)
        Me.grpEdicionGrupo.Controls.Add(Me.lblCodigoGrupo)
        Me.grpEdicionGrupo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.grpEdicionGrupo.Location = New System.Drawing.Point(6, 391)
        Me.grpEdicionGrupo.Name = "grpEdicionGrupo"
        Me.grpEdicionGrupo.Size = New System.Drawing.Size(378, 108)
        Me.grpEdicionGrupo.TabIndex = 3
        Me.grpEdicionGrupo.TabStop = False
        Me.grpEdicionGrupo.Text = "Edición"
        '
        'chkActivoGrupo
        '
        Me.chkActivoGrupo.AutoSize = True
        Me.chkActivoGrupo.Location = New System.Drawing.Point(268, 22)
        Me.chkActivoGrupo.Name = "chkActivoGrupo"
        Me.chkActivoGrupo.Size = New System.Drawing.Size(56, 17)
        Me.chkActivoGrupo.TabIndex = 2
        Me.chkActivoGrupo.Text = "Activo"
        Me.chkActivoGrupo.UseVisualStyleBackColor = True
        '
        'txtDescripcionGrupo
        '
        Me.txtDescripcionGrupo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescripcionGrupo.Location = New System.Drawing.Point(88, 48)
        Me.txtDescripcionGrupo.Name = "txtDescripcionGrupo"
        Me.txtDescripcionGrupo.Size = New System.Drawing.Size(278, 20)
        Me.txtDescripcionGrupo.TabIndex = 4
        '
        'txtCodigoGrupo
        '
        Me.txtCodigoGrupo.Location = New System.Drawing.Point(88, 22)
        Me.txtCodigoGrupo.Name = "txtCodigoGrupo"
        Me.txtCodigoGrupo.Size = New System.Drawing.Size(96, 20)
        Me.txtCodigoGrupo.TabIndex = 1
        '
        'lblDescripcionGrupo
        '
        Me.lblDescripcionGrupo.AutoSize = True
        Me.lblDescripcionGrupo.Location = New System.Drawing.Point(12, 51)
        Me.lblDescripcionGrupo.Name = "lblDescripcionGrupo"
        Me.lblDescripcionGrupo.Size = New System.Drawing.Size(63, 13)
        Me.lblDescripcionGrupo.TabIndex = 3
        Me.lblDescripcionGrupo.Text = "Descripción"
        '
        'lblCodigoGrupo
        '
        Me.lblCodigoGrupo.AutoSize = True
        Me.lblCodigoGrupo.Location = New System.Drawing.Point(12, 25)
        Me.lblCodigoGrupo.Name = "lblCodigoGrupo"
        Me.lblCodigoGrupo.Size = New System.Drawing.Size(40, 13)
        Me.lblCodigoGrupo.TabIndex = 0
        Me.lblCodigoGrupo.Text = "Código"
        '
        'dgvGrupos
        '
        Me.dgvGrupos.AllowUserToAddRows = False
        Me.dgvGrupos.AllowUserToDeleteRows = False
        Me.dgvGrupos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGrupos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvGrupos.Location = New System.Drawing.Point(6, 46)
        Me.dgvGrupos.MultiSelect = False
        Me.dgvGrupos.Name = "dgvGrupos"
        Me.dgvGrupos.ReadOnly = True
        Me.dgvGrupos.RowHeadersVisible = False
        Me.dgvGrupos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvGrupos.Size = New System.Drawing.Size(378, 453)
        Me.dgvGrupos.TabIndex = 2
        '
        'txtFiltroGrupo
        '
        Me.txtFiltroGrupo.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtFiltroGrupo.Location = New System.Drawing.Point(6, 26)
        Me.txtFiltroGrupo.Name = "txtFiltroGrupo"
        Me.txtFiltroGrupo.Size = New System.Drawing.Size(378, 20)
        Me.txtFiltroGrupo.TabIndex = 1
        '
        'lblHeadGrupo
        '
        Me.lblHeadGrupo.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblHeadGrupo.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblHeadGrupo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHeadGrupo.ForeColor = System.Drawing.Color.White
        Me.lblHeadGrupo.Location = New System.Drawing.Point(6, 0)
        Me.lblHeadGrupo.Name = "lblHeadGrupo"
        Me.lblHeadGrupo.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblHeadGrupo.Size = New System.Drawing.Size(378, 26)
        Me.lblHeadGrupo.TabIndex = 0
        Me.lblHeadGrupo.Text = "Grupo"
        Me.lblHeadGrupo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.btnCancelarEdicion)
        Me.pnlBottom.Controls.Add(Me.btnCerrar)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 561)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Padding = New System.Windows.Forms.Padding(12, 6, 12, 10)
        Me.pnlBottom.Size = New System.Drawing.Size(1184, 44)
        Me.pnlBottom.TabIndex = 2
        '
        'btnCancelarEdicion
        '
        Me.btnCancelarEdicion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelarEdicion.Enabled = False
        Me.btnCancelarEdicion.Location = New System.Drawing.Point(1004, 8)
        Me.btnCancelarEdicion.Name = "btnCancelarEdicion"
        Me.btnCancelarEdicion.Size = New System.Drawing.Size(81, 28)
        Me.btnCancelarEdicion.TabIndex = 1
        Me.btnCancelarEdicion.Text = "Cancelar"
        Me.btnCancelarEdicion.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Location = New System.Drawing.Point(1091, 8)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(81, 28)
        Me.btnCerrar.TabIndex = 0
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'FrmClasificacionProductos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(1184, 605)
        Me.Controls.Add(Me.tlpColumnas)
        Me.Controls.Add(Me.pnlBottom)
        Me.Controls.Add(Me.pnlTop)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(960, 520)
        Me.Name = "FrmClasificacionProductos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Clasificación de productos"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.tlpColumnas.ResumeLayout(False)
        Me.pnlColCategoria.ResumeLayout(False)
        Me.pnlColCategoria.PerformLayout()
        Me.flpBotonesCategoria.ResumeLayout(False)
        Me.grpEdicionCategoria.ResumeLayout(False)
        Me.grpEdicionCategoria.PerformLayout()
        CType(Me.dgvCategorias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlColSub.ResumeLayout(False)
        Me.pnlColSub.PerformLayout()
        Me.flpBotonesSub.ResumeLayout(False)
        Me.grpEdicionSub.ResumeLayout(False)
        Me.grpEdicionSub.PerformLayout()
        CType(Me.dgvSubcategorias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlColGrupo.ResumeLayout(False)
        Me.pnlColGrupo.PerformLayout()
        Me.flpBotonesGrupo.ResumeLayout(False)
        Me.grpEdicionGrupo.ResumeLayout(False)
        Me.grpEdicionGrupo.PerformLayout()
        CType(Me.dgvGrupos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlTop As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents tlpColumnas As TableLayoutPanel
    Friend WithEvents pnlColCategoria As Panel
    Friend WithEvents lblHeadCategoria As Label
    Friend WithEvents txtFiltroCategoria As TextBox
    Friend WithEvents dgvCategorias As DataGridView
    Friend WithEvents grpEdicionCategoria As GroupBox
    Friend WithEvents chkActivoCategoria As CheckBox
    Friend WithEvents txtDescripcionCategoria As TextBox
    Friend WithEvents txtCodigoCategoria As TextBox
    Friend WithEvents lblDescripcionCategoria As Label
    Friend WithEvents lblCodigoCategoria As Label
    Friend WithEvents flpBotonesCategoria As FlowLayoutPanel
    Friend WithEvents btnAgregarCategoria As Button
    Friend WithEvents btnGuardarCategoria As Button
    Friend WithEvents btnEditarCategoria As Button
    Friend WithEvents pnlColSub As Panel
    Friend WithEvents lblHeadSub As Label
    Friend WithEvents txtFiltroSub As TextBox
    Friend WithEvents dgvSubcategorias As DataGridView
    Friend WithEvents grpEdicionSub As GroupBox
    Friend WithEvents chkActivoSub As CheckBox
    Friend WithEvents txtDescripcionSub As TextBox
    Friend WithEvents txtCodigoSub As TextBox
    Friend WithEvents lblDescripcionSub As Label
    Friend WithEvents lblCodigoSub As Label
    Friend WithEvents flpBotonesSub As FlowLayoutPanel
    Friend WithEvents btnAgregarSub As Button
    Friend WithEvents btnGuardarSub As Button
    Friend WithEvents btnEditarSub As Button
    Friend WithEvents pnlColGrupo As Panel
    Friend WithEvents lblHeadGrupo As Label
    Friend WithEvents txtFiltroGrupo As TextBox
    Friend WithEvents dgvGrupos As DataGridView
    Friend WithEvents grpEdicionGrupo As GroupBox
    Friend WithEvents chkActivoGrupo As CheckBox
    Friend WithEvents txtDescripcionGrupo As TextBox
    Friend WithEvents txtCodigoGrupo As TextBox
    Friend WithEvents lblDescripcionGrupo As Label
    Friend WithEvents lblCodigoGrupo As Label
    Friend WithEvents flpBotonesGrupo As FlowLayoutPanel
    Friend WithEvents btnAgregarGrupo As Button
    Friend WithEvents btnGuardarGrupo As Button
    Friend WithEvents btnEditarGrupo As Button
    Friend WithEvents pnlBottom As Panel
    Friend WithEvents btnCancelarEdicion As Button
    Friend WithEvents btnCerrar As Button
End Class
