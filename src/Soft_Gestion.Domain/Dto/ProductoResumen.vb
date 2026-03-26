''' <summary>Fila de listado de productos con datos de clasificación, marca, unidad y SIFEN (JOINs).</summary>
Public Class ProductoResumen
    Public Property ProductoId As Integer
    Public Property Codigo As String
    Public Property CodigoBarras As String
    Public Property Descripcion As String
    Public Property GrupoId As Integer?
    Public Property CategoriaId As Integer?
    Public Property SubCategoriaId As Integer?
    Public Property CategoriaCodigo As String
    Public Property CategoriaNombre As String
    Public Property SubCategoriaCodigo As String
    Public Property SubCategoriaNombre As String
    Public Property GrupoCodigo As String
    Public Property GrupoNombre As String
    Public Property MarcaId As Integer?
    Public Property MarcaCodigo As String
    Public Property MarcaNombre As String
    Public Property UnidadMedidaId As Integer
    Public Property UnidadCodigo As String
    Public Property UnidadAbreviatura As String
    Public Property Codigo_SIFEN As Short?
    Public Property Descrip_Unidad_SIFEN As String
    Public Property Codigo_Repr_SIFEN As String
    Public Property CostoUltimo As Decimal
    Public Property PrecioBase As Decimal
    Public Property PorcentajeIVA As Decimal
    Public Property PermiteStockNegativo As Boolean
    Public Property ControlaStock As Boolean
    Public Property EsServicio As Boolean
    Public Property Activo As Boolean
End Class
