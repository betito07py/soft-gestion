''' <summary>Entidad correspondiente a la tabla Depositos.</summary>
Public Class Deposito
    Public Property DepositoId As Integer
    Public Property SucursalId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Descripcion As String
    Public Property EsPrincipal As Boolean
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
