''' <summary>
''' Identificadores estables de formularios / acciones de menú para permisos futuros.
''' Convención: Modulo.Entidad o Modulo.Modulo para vistas generales.
''' </summary>
Public NotInheritable Class ClavesFormulario
    Private Sub New()
    End Sub

    Public Const SeguridadUsuarios As String = "Seguridad.Usuarios"
    Public Const SeguridadRoles As String = "Seguridad.Roles"
    Public Const SeguridadPermisos As String = "Seguridad.Permisos"
    Public Const SeguridadRolPermisos As String = "Seguridad.RolPermisos"

    Public Const MaestrosEmpresas As String = "Maestros.Empresas"
    Public Const MaestrosSucursales As String = "Maestros.Sucursales"
    Public Const MaestrosDepositos As String = "Maestros.Depositos"
    Public Const MaestrosClientes As String = "Maestros.Clientes"
    Public Const MaestrosProveedores As String = "Maestros.Proveedores"
    Public Const MaestrosProductos As String = "Maestros.Productos"
    Public Const MaestrosCategorias As String = "Maestros.Categorias"
    Public Const MaestrosSubCategorias As String = "Maestros.SubCategorias"
    Public Const MaestrosGrupos As String = "Maestros.Grupos"
    Public Const MaestrosMarcas As String = "Maestros.Marcas"
    Public Const MaestrosUnidadesMedida As String = "Maestros.UnidadesMedida"
    Public Const MaestrosImpuestos As String = "Maestros.Impuestos"

    Public Const StockModulo As String = "Stock.Modulo"
    Public Const ComprasModulo As String = "Compras.Modulo"
    Public Const VentasModulo As String = "Ventas.Modulo"
    Public Const CuentasCobrarModulo As String = "Finanzas.CuentasCobrar"
    Public Const CuentasPagarModulo As String = "Finanzas.CuentasPagar"
    Public Const TesoreriaModulo As String = "Tesoreria.Modulo"
End Class
