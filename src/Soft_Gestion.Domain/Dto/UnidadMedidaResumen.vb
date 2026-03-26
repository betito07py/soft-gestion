''' <summary>Fila de listado de unidades de medida.</summary>
Public Class UnidadMedidaResumen
    Public Property UnidadMedidaId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Abreviatura As String
    Public Property Codigo_SIFEN As Short?
    ''' <summary>Rellenado con LEFT JOIN a UnidadesMedidaSIFEN en listados/combo.</summary>
    Public Property Descrip_Unidad_SIFEN As String
    ''' <summary>Rellenado con LEFT JOIN a UnidadesMedidaSIFEN.</summary>
    Public Property Codigo_Repr_SIFEN As String
    Public Property Activo As Boolean
End Class
