''' <summary>
''' Clase base para entidades del dominio. Incluye campos de auditoría comunes.
''' </summary>
Public MustInherit Class EntidadBase
    Public Property Id As Integer
    Public Property FechaAlta As DateTime?
    Public Property UsuarioAlta As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
    Public Property Activo As Boolean = True
End Class
