''' <summary>
''' Fila de listado de subcategorías (incluye código de categoría padre).
''' </summary>
Public Class SubCategoriaResumen
    Public Property SubCategoriaId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property CategoriaId As Integer
    Public Property CategoriaCodigo As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
