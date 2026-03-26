''' <summary>Entidad correspondiente a la tabla Proveedores.</summary>
Public Class Proveedor
    Public Property ProveedorId As Integer
    Public Property Codigo As String
    Public Property RazonSocial As String
    Public Property RUC As String
    Public Property Direccion As String
    Public Property Telefono As String
    Public Property Email As String
    Public Property CondicionPagoId As Integer?
    Public Property Observaciones As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
