''' <summary>Entidad correspondiente a la tabla UnidadesMedida.</summary>
Public Class UnidadMedida
    Public Property UnidadMedidaId As Integer
    Public Property Codigo As String
    Public Property Nombre As String
    Public Property Abreviatura As String
    Public Property Activo As Boolean
    ''' <summary>Código de unidad según catálogo SIFEN (nullable si no aplica).</summary>
    Public Property Codigo_SIFEN As Short?
    ''' <summary>Solo lectura al obtener con JOIN al catálogo SIFEN; no se persiste en INSERT/UPDATE.</summary>
    Public Property Descrip_Unidad_SIFEN As String
    ''' <summary>Solo lectura al obtener con JOIN al catálogo SIFEN; no se persiste en INSERT/UPDATE.</summary>
    Public Property Codigo_Repr_SIFEN As String
End Class
