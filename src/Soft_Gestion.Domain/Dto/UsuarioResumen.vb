''' <summary>
''' Fila de listado de usuarios sin datos sensibles (sin contraseña ni hash).
''' </summary>
Public Class UsuarioResumen
    Public Property UsuarioId As Integer
    Public Property Login As String
    Public Property NombreCompleto As String
    Public Property Email As String
    Public Property EsAdministrador As Boolean
    Public Property SucursalId As Integer?
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
