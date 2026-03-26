''' <summary>
''' Fila de listado de roles para grillas y consultas ligeras.
''' </summary>
Public Class RolResumen
    Public Property RolId As Integer
    Public Property Nombre As String
    Public Property Descripcion As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
