''' <summary>
''' Fila de listado de grupos (incluye códigos de categoría y subcategoría).
''' </summary>
Public Class GrupoResumen
    Public Property GrupoId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property CategoriaId As Integer
    Public Property SubCategoriaId As Integer
    Public Property CategoriaCodigo As String
    Public Property SubCategoriaCodigo As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
