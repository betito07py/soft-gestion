Imports System

''' <summary>Entidad correspondiente a la tabla Impuestos (maestro fiscal Paraguay).</summary>
Public Class Impuesto
    Public Property ImpuestoId As Integer
    Public Property Codigo As Integer
    Public Property Nombre As String
    Public Property TipoImpuesto As String
    Public Property Porcentaje As Decimal
    Public Property EsExento As Boolean
    Public Property CodigoSIFEN As Short?
    Public Property EsActivo As Boolean
    Public Property UsuarioCreacion As String
    Public Property FechaCreacion As DateTime
    Public Property UsuarioModificacion As String
    Public Property FechaModificacion As DateTime?
End Class
