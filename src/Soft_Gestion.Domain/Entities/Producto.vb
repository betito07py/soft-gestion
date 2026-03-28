''' <summary>Entidad correspondiente a la tabla Productos.</summary>
Public Class Producto
    Public Property ProductoId As Integer
    Public Property Codigo As String
    Public Property CodigoBarras As String
    Public Property Descripcion As String
    ''' <summary>Clasificación de tercer nivel; define categoría y subcategoría vía <c>Grupos</c>.</summary>
    Public Property GrupoId As Integer?
    Public Property MarcaId As Integer?
    ''' <summary>Impuesto de facturación (IVA / exento), maestro <c>Impuestos</c>.</summary>
    Public Property ImpuestoId As Integer
    Public Property UnidadMedidaId As Integer
    Public Property CostoUltimo As Decimal
    Public Property PermiteStockNegativo As Boolean
    Public Property ControlaStock As Boolean
    Public Property EsServicio As Boolean
    Public Property Observaciones As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
