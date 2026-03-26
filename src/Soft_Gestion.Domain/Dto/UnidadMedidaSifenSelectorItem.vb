''' <summary>Elemento para combo de códigos SIFEN (catálogo de referencia, sin ABM en UI).</summary>
Public Class UnidadMedidaSifenSelectorItem
    ''' <summary>Nothing en la fila “sin código”; en filas del catálogo, el código numérico SIFEN.</summary>
    Public Property CodigoSifen As Short?
    ''' <summary>Descrip_Unidad del catálogo SIFEN (vacío en la fila “sin código”).</summary>
    Public Property DescripUnidad As String
    ''' <summary>Codigo_Repr del catálogo SIFEN.</summary>
    Public Property CodigoRepr As String
    Public Property Texto As String
End Class
