<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmPrincipal
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
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MenuSeguridad = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSeguridadUsuarios = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSeguridadRoles = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSeguridadPermisos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuSeguridadRolPermisos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestros = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosEmpresas = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosSucursales = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosDepositos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosClientes = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosProveedores = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosCategorias = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosSubCategorias = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosGrupos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosClasificacionProductos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosMarcas = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosUnidadesMedida = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosImpuestos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMaestrosProductos = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStock = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStockVista = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCompras = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuComprasVista = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuVentas = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuVentasVista = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCuentasCobrar = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCuentasCobrarVista = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCuentasPagar = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCuentasPagarVista = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTesoreria = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuTesoreriaVista = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuVentana = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCascada = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMosaicoH = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuMosaicoV = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuCerrarTodas = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabelEspacio = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusLabelUsuario = New System.Windows.Forms.ToolStripStatusLabel()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuSeguridad, Me.MenuMaestros, Me.MenuStock, Me.MenuCompras, Me.MenuVentas, Me.MenuCuentasCobrar, Me.MenuCuentasPagar, Me.MenuTesoreria, Me.MenuVentana})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(900, 24)
        Me.MenuStrip1.TabIndex = 0
        '
        'MenuSeguridad
        '
        Me.MenuSeguridad.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuSeguridadUsuarios, Me.MenuSeguridadRoles, Me.MenuSeguridadPermisos, Me.MenuSeguridadRolPermisos})
        Me.MenuSeguridad.Name = "MenuSeguridad"
        Me.MenuSeguridad.Size = New System.Drawing.Size(72, 20)
        Me.MenuSeguridad.Text = "Seguridad"
        '
        'MenuSeguridadUsuarios
        '
        Me.MenuSeguridadUsuarios.Name = "MenuSeguridadUsuarios"
        Me.MenuSeguridadUsuarios.Size = New System.Drawing.Size(180, 22)
        Me.MenuSeguridadUsuarios.Text = "Usuarios"
        '
        'MenuSeguridadRoles
        '
        Me.MenuSeguridadRoles.Name = "MenuSeguridadRoles"
        Me.MenuSeguridadRoles.Size = New System.Drawing.Size(180, 22)
        Me.MenuSeguridadRoles.Text = "Roles"
        '
        'MenuSeguridadPermisos
        '
        Me.MenuSeguridadPermisos.Name = "MenuSeguridadPermisos"
        Me.MenuSeguridadPermisos.Size = New System.Drawing.Size(180, 22)
        Me.MenuSeguridadPermisos.Text = "Permisos"
        '
        'MenuSeguridadRolPermisos
        '
        Me.MenuSeguridadRolPermisos.Name = "MenuSeguridadRolPermisos"
        Me.MenuSeguridadRolPermisos.Size = New System.Drawing.Size(180, 22)
        Me.MenuSeguridadRolPermisos.Text = "Permisos por rol"
        '
        'MenuMaestros
        '
        Me.MenuMaestros.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuMaestrosEmpresas, Me.MenuMaestrosSucursales, Me.MenuMaestrosDepositos, Me.MenuMaestrosClientes, Me.MenuMaestrosProveedores, Me.MenuMaestrosCategorias, Me.MenuMaestrosSubCategorias, Me.MenuMaestrosGrupos, Me.MenuMaestrosClasificacionProductos, Me.MenuMaestrosMarcas, Me.MenuMaestrosUnidadesMedida, Me.MenuMaestrosImpuestos, Me.MenuMaestrosProductos})
        Me.MenuMaestros.Name = "MenuMaestros"
        Me.MenuMaestros.Size = New System.Drawing.Size(67, 20)
        Me.MenuMaestros.Text = "Maestros"
        '
        'MenuMaestrosEmpresas
        '
        Me.MenuMaestrosEmpresas.Name = "MenuMaestrosEmpresas"
        Me.MenuMaestrosEmpresas.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosEmpresas.Text = "Empresas"
        '
        'MenuMaestrosSucursales
        '
        Me.MenuMaestrosSucursales.Name = "MenuMaestrosSucursales"
        Me.MenuMaestrosSucursales.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosSucursales.Text = "Sucursales"
        '
        'MenuMaestrosDepositos
        '
        Me.MenuMaestrosDepositos.Name = "MenuMaestrosDepositos"
        Me.MenuMaestrosDepositos.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosDepositos.Text = "Depósitos"
        '
        'MenuMaestrosClientes
        '
        Me.MenuMaestrosClientes.Name = "MenuMaestrosClientes"
        Me.MenuMaestrosClientes.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosClientes.Text = "Clientes"
        '
        'MenuMaestrosProveedores
        '
        Me.MenuMaestrosProveedores.Name = "MenuMaestrosProveedores"
        Me.MenuMaestrosProveedores.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosProveedores.Text = "Proveedores"
        '
        'MenuMaestrosCategorias
        '
        Me.MenuMaestrosCategorias.Name = "MenuMaestrosCategorias"
        Me.MenuMaestrosCategorias.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosCategorias.Text = "Categorías"
        Me.MenuMaestrosCategorias.Visible = False
        '
        'MenuMaestrosSubCategorias
        '
        Me.MenuMaestrosSubCategorias.Name = "MenuMaestrosSubCategorias"
        Me.MenuMaestrosSubCategorias.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosSubCategorias.Text = "Subcategorías"
        Me.MenuMaestrosSubCategorias.Visible = False
        '
        'MenuMaestrosGrupos
        '
        Me.MenuMaestrosGrupos.Name = "MenuMaestrosGrupos"
        Me.MenuMaestrosGrupos.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosGrupos.Text = "Grupos"
        Me.MenuMaestrosGrupos.Visible = False
        '
        'MenuMaestrosClasificacionProductos
        '
        Me.MenuMaestrosClasificacionProductos.Name = "MenuMaestrosClasificacionProductos"
        Me.MenuMaestrosClasificacionProductos.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosClasificacionProductos.Text = "Clasificación de productos"
        '
        'MenuMaestrosMarcas
        '
        Me.MenuMaestrosMarcas.Name = "MenuMaestrosMarcas"
        Me.MenuMaestrosMarcas.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosMarcas.Text = "Marcas"
        '
        'MenuMaestrosUnidadesMedida
        '
        Me.MenuMaestrosUnidadesMedida.Name = "MenuMaestrosUnidadesMedida"
        Me.MenuMaestrosUnidadesMedida.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosUnidadesMedida.Text = "Unidades de medida"
        '
        'MenuMaestrosImpuestos
        '
        Me.MenuMaestrosImpuestos.Name = "MenuMaestrosImpuestos"
        Me.MenuMaestrosImpuestos.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosImpuestos.Text = "Impuestos"
        '
        'MenuMaestrosProductos
        '
        Me.MenuMaestrosProductos.Name = "MenuMaestrosProductos"
        Me.MenuMaestrosProductos.Size = New System.Drawing.Size(180, 22)
        Me.MenuMaestrosProductos.Text = "Productos"
        '
        'MenuStock
        '
        Me.MenuStock.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuStockVista})
        Me.MenuStock.Name = "MenuStock"
        Me.MenuStock.Size = New System.Drawing.Size(48, 20)
        Me.MenuStock.Text = "Stock"
        '
        'MenuStockVista
        '
        Me.MenuStockVista.Name = "MenuStockVista"
        Me.MenuStockVista.Size = New System.Drawing.Size(180, 22)
        Me.MenuStockVista.Text = "Vista general"
        '
        'MenuCompras
        '
        Me.MenuCompras.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuComprasVista})
        Me.MenuCompras.Name = "MenuCompras"
        Me.MenuCompras.Size = New System.Drawing.Size(67, 20)
        Me.MenuCompras.Text = "Compras"
        '
        'MenuComprasVista
        '
        Me.MenuComprasVista.Name = "MenuComprasVista"
        Me.MenuComprasVista.Size = New System.Drawing.Size(180, 22)
        Me.MenuComprasVista.Text = "Vista general"
        '
        'MenuVentas
        '
        Me.MenuVentas.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuVentasVista})
        Me.MenuVentas.Name = "MenuVentas"
        Me.MenuVentas.Size = New System.Drawing.Size(61, 20)
        Me.MenuVentas.Text = "Ventas"
        '
        'MenuVentasVista
        '
        Me.MenuVentasVista.Name = "MenuVentasVista"
        Me.MenuVentasVista.Size = New System.Drawing.Size(180, 22)
        Me.MenuVentasVista.Text = "Vista general"
        '
        'MenuCuentasCobrar
        '
        Me.MenuCuentasCobrar.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCuentasCobrarVista})
        Me.MenuCuentasCobrar.Name = "MenuCuentasCobrar"
        Me.MenuCuentasCobrar.Size = New System.Drawing.Size(115, 20)
        Me.MenuCuentasCobrar.Text = "Cuentas a Cobrar"
        '
        'MenuCuentasCobrarVista
        '
        Me.MenuCuentasCobrarVista.Name = "MenuCuentasCobrarVista"
        Me.MenuCuentasCobrarVista.Size = New System.Drawing.Size(180, 22)
        Me.MenuCuentasCobrarVista.Text = "Vista general"
        '
        'MenuCuentasPagar
        '
        Me.MenuCuentasPagar.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCuentasPagarVista})
        Me.MenuCuentasPagar.Name = "MenuCuentasPagar"
        Me.MenuCuentasPagar.Size = New System.Drawing.Size(105, 20)
        Me.MenuCuentasPagar.Text = "Cuentas a Pagar"
        '
        'MenuCuentasPagarVista
        '
        Me.MenuCuentasPagarVista.Name = "MenuCuentasPagarVista"
        Me.MenuCuentasPagarVista.Size = New System.Drawing.Size(180, 22)
        Me.MenuCuentasPagarVista.Text = "Vista general"
        '
        'MenuTesoreria
        '
        Me.MenuTesoreria.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuTesoreriaVista})
        Me.MenuTesoreria.Name = "MenuTesoreria"
        Me.MenuTesoreria.Size = New System.Drawing.Size(67, 20)
        Me.MenuTesoreria.Text = "Tesorería"
        '
        'MenuTesoreriaVista
        '
        Me.MenuTesoreriaVista.Name = "MenuTesoreriaVista"
        Me.MenuTesoreriaVista.Size = New System.Drawing.Size(180, 22)
        Me.MenuTesoreriaVista.Text = "Vista general"
        '
        'MenuVentana
        '
        Me.MenuVentana.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuCascada, Me.MenuMosaicoH, Me.MenuMosaicoV, Me.MenuCerrarTodas})
        Me.MenuVentana.Name = "MenuVentana"
        Me.MenuVentana.Size = New System.Drawing.Size(61, 20)
        Me.MenuVentana.Text = "Ventana"
        '
        'MenuCascada
        '
        Me.MenuCascada.Name = "MenuCascada"
        Me.MenuCascada.Size = New System.Drawing.Size(175, 22)
        Me.MenuCascada.Text = "Cascada"
        '
        'MenuMosaicoH
        '
        Me.MenuMosaicoH.Name = "MenuMosaicoH"
        Me.MenuMosaicoH.Size = New System.Drawing.Size(175, 22)
        Me.MenuMosaicoH.Text = "Mosaico horizontal"
        '
        'MenuMosaicoV
        '
        Me.MenuMosaicoV.Name = "MenuMosaicoV"
        Me.MenuMosaicoV.Size = New System.Drawing.Size(175, 22)
        Me.MenuMosaicoV.Text = "Mosaico vertical"
        '
        'MenuCerrarTodas
        '
        Me.MenuCerrarTodas.Name = "MenuCerrarTodas"
        Me.MenuCerrarTodas.Size = New System.Drawing.Size(175, 22)
        Me.MenuCerrarTodas.Text = "Cerrar todas"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabelEspacio, Me.StatusLabelUsuario})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 428)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(900, 22)
        Me.StatusStrip1.TabIndex = 1
        '
        'StatusLabelEspacio
        '
        Me.StatusLabelEspacio.Name = "StatusLabelEspacio"
        Me.StatusLabelEspacio.Size = New System.Drawing.Size(785, 17)
        Me.StatusLabelEspacio.Spring = True
        '
        'StatusLabelUsuario
        '
        Me.StatusLabelUsuario.Name = "StatusLabelUsuario"
        Me.StatusLabelUsuario.Size = New System.Drawing.Size(100, 17)
        Me.StatusLabelUsuario.Text = "Usuario"
        Me.StatusLabelUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FrmPrincipal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 450)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FrmPrincipal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Soft Gestion"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents MenuSeguridad As ToolStripMenuItem
    Friend WithEvents MenuSeguridadUsuarios As ToolStripMenuItem
    Friend WithEvents MenuSeguridadRoles As ToolStripMenuItem
    Friend WithEvents MenuSeguridadPermisos As ToolStripMenuItem
    Friend WithEvents MenuSeguridadRolPermisos As ToolStripMenuItem
    Friend WithEvents MenuMaestros As ToolStripMenuItem
    Friend WithEvents MenuMaestrosEmpresas As ToolStripMenuItem
    Friend WithEvents MenuMaestrosSucursales As ToolStripMenuItem
    Friend WithEvents MenuMaestrosDepositos As ToolStripMenuItem
    Friend WithEvents MenuMaestrosClientes As ToolStripMenuItem
    Friend WithEvents MenuMaestrosProveedores As ToolStripMenuItem
    Friend WithEvents MenuMaestrosCategorias As ToolStripMenuItem
    Friend WithEvents MenuMaestrosSubCategorias As ToolStripMenuItem
    Friend WithEvents MenuMaestrosGrupos As ToolStripMenuItem
    Friend WithEvents MenuMaestrosClasificacionProductos As ToolStripMenuItem
    Friend WithEvents MenuMaestrosMarcas As ToolStripMenuItem
    Friend WithEvents MenuMaestrosUnidadesMedida As ToolStripMenuItem
    Friend WithEvents MenuMaestrosImpuestos As ToolStripMenuItem
    Friend WithEvents MenuMaestrosProductos As ToolStripMenuItem
    Friend WithEvents MenuStock As ToolStripMenuItem
    Friend WithEvents MenuStockVista As ToolStripMenuItem
    Friend WithEvents MenuCompras As ToolStripMenuItem
    Friend WithEvents MenuComprasVista As ToolStripMenuItem
    Friend WithEvents MenuVentas As ToolStripMenuItem
    Friend WithEvents MenuVentasVista As ToolStripMenuItem
    Friend WithEvents MenuCuentasCobrar As ToolStripMenuItem
    Friend WithEvents MenuCuentasCobrarVista As ToolStripMenuItem
    Friend WithEvents MenuCuentasPagar As ToolStripMenuItem
    Friend WithEvents MenuCuentasPagarVista As ToolStripMenuItem
    Friend WithEvents MenuTesoreria As ToolStripMenuItem
    Friend WithEvents MenuTesoreriaVista As ToolStripMenuItem
    Friend WithEvents MenuVentana As ToolStripMenuItem
    Friend WithEvents MenuCascada As ToolStripMenuItem
    Friend WithEvents MenuMosaicoH As ToolStripMenuItem
    Friend WithEvents MenuMosaicoV As ToolStripMenuItem
    Friend WithEvents MenuCerrarTodas As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabelEspacio As ToolStripStatusLabel
    Friend WithEvents StatusLabelUsuario As ToolStripStatusLabel
End Class
