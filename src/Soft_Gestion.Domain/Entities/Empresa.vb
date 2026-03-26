''' <summary>Entidad correspondiente a la tabla Empresas.</summary>
Public Class Empresa
    Public Property EmpresaId As Integer
    Public Property Codigo As String
    Public Property RazonSocial As String
    Public Property NombreFantasia As String
    Public Property RUC As String
    Public Property Direccion As String
    Public Property Telefono As String
    Public Property Email As String
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
