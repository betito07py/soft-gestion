''' <summary>Entidad correspondiente a la tabla Categorias (nivel superior de clasificación de productos).</summary>
Public Class Categoria
    Public Property CategoriaId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
