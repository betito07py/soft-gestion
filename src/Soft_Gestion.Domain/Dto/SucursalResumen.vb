''' <summary>
''' Fila de listado de sucursales para grillas (incluye datos de empresa para contexto).
''' </summary>
Public Class SucursalResumen
    Public Property SucursalId As Integer
    Public Property EmpresaId As Integer
    Public Property EmpresaCodigo As String
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Responsable As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
