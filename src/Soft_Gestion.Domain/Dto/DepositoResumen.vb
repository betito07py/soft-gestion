''' <summary>
''' Fila de listado de depósitos para grillas (contexto empresa y sucursal).
''' </summary>
Public Class DepositoResumen
    Public Property DepositoId As Integer
    Public Property SucursalId As Integer
    Public Property EmpresaId As Integer
    Public Property EmpresaCodigo As String
    Public Property SucursalCodigo As String
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
