''' <summary>
''' Fila de listado de proveedores.
''' </summary>
Public Class ProveedorResumen
    Public Property ProveedorId As Integer
    Public Property Codigo As String
    Public Property RazonSocial As String
    Public Property RUC As String
    Public Property CondicionPagoCodigo As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
