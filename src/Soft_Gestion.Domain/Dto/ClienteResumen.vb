''' <summary>
''' Fila de listado de clientes (sin datos sensibles adicionales).
''' </summary>
Public Class ClienteResumen
    Public Property ClienteId As Integer
    Public Property Codigo As String
    Public Property RazonSocial As String
    Public Property Documento As String
    Public Property RUC As String
    Public Property CondicionPagoCodigo As String
    Public Property ListaPrecioCodigo As String
    Public Property LimiteCredito As Decimal
    Public Property Activo As Boolean
    Public Property FechaCreacion As DateTime
    Public Property UsuarioCreacion As String
    Public Property FechaModificacion As DateTime?
    Public Property UsuarioModificacion As String
End Class
