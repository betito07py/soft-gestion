''' <summary>Entidad correspondiente a la tabla Sucursales.</summary>
Public Class Sucursal
    Public Property SucursalId As Integer
    Public Property EmpresaId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Direccion As String
    Public Property Telefono As String
    Public Property Responsable As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
