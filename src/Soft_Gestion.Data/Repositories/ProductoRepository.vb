Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Productos</c>.
''' </summary>
Public Class ProductoRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT ProductoId, Codigo, CodigoBarras, Descripcion, GrupoId, MarcaId, ImpuestoId, UnidadMedidaId, " &
        "CostoUltimo, PermiteStockNegativo, ControlaStock, EsServicio, Observaciones, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Productos WHERE ProductoId = @ProductoId;"

    Private Const SqlListar As String =
        "SELECT p.ProductoId, p.Codigo, " &
        "COALESCE(" &
        "(SELECT TOP (1) pb.CodBarras FROM dbo.Productos_Barras pb WHERE pb.ProductoId = p.ProductoId ORDER BY pb.FechaAlta ASC, pb.ProductoBarraId ASC), " &
        "p.CodigoBarras) AS CodigoBarras, p.Descripcion, p.GrupoId, " &
        "g.CategoriaId, g.SubCategoriaId, " &
        "cat.Codigo AS CategoriaCodigo, cat.Nombre AS CategoriaNombre, " &
        "sc.Codigo AS SubCategoriaCodigo, sc.Nombre AS SubCategoriaNombre, " &
        "g.Codigo AS GrupoCodigo, g.Nombre AS GrupoNombre, " &
        "p.MarcaId, m.Codigo AS MarcaCodigo, m.Nombre AS MarcaNombre, " &
        "p.ImpuestoId, imp.Codigo AS ImpuestoCodigo, imp.Nombre AS ImpuestoNombre, " &
        "p.UnidadMedidaId, um.Codigo AS UnidadCodigo, um.Abreviatura AS UnidadAbreviatura, " &
        "um.Codigo_SIFEN AS Codigo_SIFEN, sifen.Descrip_Unidad AS Descrip_Unidad_SIFEN, sifen.Codigo_Repr AS Codigo_Repr_SIFEN, " &
        "p.CostoUltimo, p.PermiteStockNegativo, p.ControlaStock, p.EsServicio, p.Activo " &
        "FROM dbo.Productos p " &
        "LEFT JOIN dbo.Grupos g ON g.GrupoId = p.GrupoId " &
        "LEFT JOIN dbo.Categorias cat ON cat.CategoriaId = g.CategoriaId " &
        "LEFT JOIN dbo.SubCategorias sc ON sc.SubCategoriaId = g.SubCategoriaId " &
        "LEFT JOIN dbo.Marcas m ON m.MarcaId = p.MarcaId " &
        "LEFT JOIN dbo.Impuestos imp ON imp.ImpuestoId = p.ImpuestoId " &
        "INNER JOIN dbo.UnidadesMedida um ON um.UnidadMedidaId = p.UnidadMedidaId " &
        "LEFT JOIN dbo.UnidadesMedidaSIFEN sifen ON sifen.Codigo = um.Codigo_SIFEN " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR p.Codigo LIKE @Patron OR p.Descripcion LIKE @Patron " &
        "OR (p.CodigoBarras IS NOT NULL AND p.CodigoBarras LIKE @Patron) " &
        "OR EXISTS (SELECT 1 FROM dbo.Productos_Barras pbf WHERE pbf.ProductoId = p.ProductoId AND pbf.CodBarras LIKE @Patron)) " &
        "AND (@CategoriaId IS NULL OR g.CategoriaId = @CategoriaId) " &
        "AND (@SubCategoriaId IS NULL OR g.SubCategoriaId = @SubCategoriaId) " &
        "AND (@GrupoId IS NULL OR p.GrupoId = @GrupoId) " &
        "AND (@MarcaId IS NULL OR p.MarcaId = @MarcaId) " &
        "AND (@Activo IS NULL OR p.Activo = @Activo) " &
        "ORDER BY p.Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Productos WHERE Codigo = @Codigo " &
        "AND (@ExcluirProductoId IS NULL OR ProductoId <> @ExcluirProductoId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Productos (Codigo, CodigoBarras, Descripcion, GrupoId, MarcaId, ImpuestoId, UnidadMedidaId, " &
        "CostoUltimo, PrecioBase, PorcentajeIVA, PermiteStockNegativo, ControlaStock, EsServicio, Observaciones, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.ProductoId " &
        "VALUES (@Codigo, @CodigoBarras, @Descripcion, @GrupoId, @MarcaId, @ImpuestoId, @UnidadMedidaId, " &
        "@CostoUltimo, @PrecioBase, @PorcentajeIVA, @PermiteStockNegativo, @ControlaStock, @EsServicio, @Observaciones, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Productos SET Codigo = @Codigo, CodigoBarras = @CodigoBarras, Descripcion = @Descripcion, " &
        "GrupoId = @GrupoId, MarcaId = @MarcaId, ImpuestoId = @ImpuestoId, UnidadMedidaId = @UnidadMedidaId, " &
        "CostoUltimo = @CostoUltimo, PrecioBase = @PrecioBase, PorcentajeIVA = @PorcentajeIVA, " &
        "PermiteStockNegativo = @PermiteStockNegativo, ControlaStock = @ControlaStock, EsServicio = @EsServicio, Observaciones = @Observaciones, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ProductoId = @ProductoId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Productos SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ProductoId = @ProductoId;"

    Public Function ObtenerPorId(productoId As Integer) As Producto
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@ProductoId", productoId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearProducto(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String,
                          categoriaId As Integer?,
                          subCategoriaId As Integer?,
                          grupoId As Integer?,
                          marcaId As Integer?,
                          activo As Boolean?) As List(Of ProductoResumen)
        Dim lista As New List(Of ProductoResumen)()
        Dim filtroTrim As String = If(filtroTexto, String.Empty).Trim()
        Dim filtroParam As Object = If(filtroTrim.Length = 0, CType(Nothing, Object), filtroTrim)
        Dim patron As Object = If(filtroTrim.Length = 0, DBNull.Value, CType("%" & filtroTrim & "%", Object))

        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListar, CommandType.Text)
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                AgregarParametroNullableInt(cmd, "@CategoriaId", categoriaId)
                AgregarParametroNullableInt(cmd, "@SubCategoriaId", subCategoriaId)
                AgregarParametroNullableInt(cmd, "@GrupoId", grupoId)
                AgregarParametroNullableInt(cmd, "@MarcaId", marcaId)
                If activo.HasValue Then
                    AgregarParametro(cmd, "@Activo", activo.Value)
                Else
                    AgregarParametro(cmd, "@Activo", DBNull.Value)
                End If
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Private Shared Sub AgregarParametroNullableInt(cmd As SqlCommand, nombre As String, valor As Integer?)
        If valor.HasValue Then
            AgregarParametro(cmd, nombre, valor.Value)
        Else
            AgregarParametro(cmd, nombre, DBNull.Value)
        End If
    End Sub

    Public Function ExisteCodigo(codigo As String, excluirProductoId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirProductoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirProductoId", excluirProductoId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirProductoId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    ''' <param name="porcentajeIvaPersistido">Valor almacenado en <c>dbo.Productos.PorcentajeIVA</c>, alineado al maestro <c>Impuestos</c>.</param>
    Public Function Insertar(p As Producto, porcentajeIvaPersistido As Decimal) As Integer
        If p Is Nothing Then Throw New ArgumentNullException(NameOf(p))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", p.Codigo.Trim())
                AgregarParametro(cmd, "@CodigoBarras", If(String.IsNullOrWhiteSpace(p.CodigoBarras), CType(DBNull.Value, Object), p.CodigoBarras.Trim()))
                AgregarParametro(cmd, "@Descripcion", p.Descripcion.Trim())
                AgregarParametroNullableInt(cmd, "@GrupoId", p.GrupoId)
                If p.MarcaId.HasValue Then
                    AgregarParametro(cmd, "@MarcaId", p.MarcaId.Value)
                Else
                    AgregarParametro(cmd, "@MarcaId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@ImpuestoId", p.ImpuestoId)
                AgregarParametro(cmd, "@UnidadMedidaId", p.UnidadMedidaId)
                AgregarParametro(cmd, "@CostoUltimo", p.CostoUltimo)
                AgregarParametro(cmd, "@PrecioBase", 0D)
                AgregarParametro(cmd, "@PorcentajeIVA", porcentajeIvaPersistido)
                AgregarParametro(cmd, "@PermiteStockNegativo", p.PermiteStockNegativo)
                AgregarParametro(cmd, "@ControlaStock", p.ControlaStock)
                AgregarParametro(cmd, "@EsServicio", p.EsServicio)
                AgregarParametro(cmd, "@Observaciones", If(String.IsNullOrWhiteSpace(p.Observaciones), CType(DBNull.Value, Object), p.Observaciones.Trim()))
                AgregarParametro(cmd, "@Activo", p.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", p.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(p As Producto, usuarioModificador As String, porcentajeIvaPersistido As Decimal) As Integer
        If p Is Nothing Then Throw New ArgumentNullException(NameOf(p))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@ProductoId", p.ProductoId)
                AgregarParametro(cmd, "@Codigo", p.Codigo.Trim())
                AgregarParametro(cmd, "@CodigoBarras", If(String.IsNullOrWhiteSpace(p.CodigoBarras), CType(DBNull.Value, Object), p.CodigoBarras.Trim()))
                AgregarParametro(cmd, "@Descripcion", p.Descripcion.Trim())
                AgregarParametroNullableInt(cmd, "@GrupoId", p.GrupoId)
                If p.MarcaId.HasValue Then
                    AgregarParametro(cmd, "@MarcaId", p.MarcaId.Value)
                Else
                    AgregarParametro(cmd, "@MarcaId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@ImpuestoId", p.ImpuestoId)
                AgregarParametro(cmd, "@UnidadMedidaId", p.UnidadMedidaId)
                AgregarParametro(cmd, "@CostoUltimo", p.CostoUltimo)
                AgregarParametro(cmd, "@PrecioBase", 0D)
                AgregarParametro(cmd, "@PorcentajeIVA", porcentajeIvaPersistido)
                AgregarParametro(cmd, "@PermiteStockNegativo", p.PermiteStockNegativo)
                AgregarParametro(cmd, "@ControlaStock", p.ControlaStock)
                AgregarParametro(cmd, "@EsServicio", p.EsServicio)
                AgregarParametro(cmd, "@Observaciones", If(String.IsNullOrWhiteSpace(p.Observaciones), CType(DBNull.Value, Object), p.Observaciones.Trim()))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(productoId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(productoId, True, usuarioModificador)
    End Function

    Public Function Desactivar(productoId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(productoId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(productoId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@ProductoId", productoId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearProducto(reader As SqlDataReader) As Producto
        Return New Producto With {
            .ProductoId = LeerInt32(reader, "ProductoId"),
            .Codigo = LeerString(reader, "Codigo"),
            .CodigoBarras = LeerString(reader, "CodigoBarras"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .GrupoId = LeerInt32Nullable(reader, "GrupoId"),
            .MarcaId = LeerInt32Nullable(reader, "MarcaId"),
            .ImpuestoId = LeerInt32(reader, "ImpuestoId"),
            .UnidadMedidaId = LeerInt32(reader, "UnidadMedidaId"),
            .CostoUltimo = LeerDecimal(reader, "CostoUltimo"),
            .PermiteStockNegativo = LeerBoolean(reader, "PermiteStockNegativo"),
            .ControlaStock = LeerBoolean(reader, "ControlaStock"),
            .EsServicio = LeerBoolean(reader, "EsServicio"),
            .Observaciones = LeerString(reader, "Observaciones"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearResumen(reader As SqlDataReader) As ProductoResumen
        Return New ProductoResumen With {
            .ProductoId = LeerInt32(reader, "ProductoId"),
            .Codigo = LeerString(reader, "Codigo"),
            .CodigoBarras = LeerString(reader, "CodigoBarras"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .GrupoId = LeerInt32Nullable(reader, "GrupoId"),
            .CategoriaId = LeerInt32Nullable(reader, "CategoriaId"),
            .SubCategoriaId = LeerInt32Nullable(reader, "SubCategoriaId"),
            .CategoriaCodigo = LeerString(reader, "CategoriaCodigo"),
            .CategoriaNombre = LeerString(reader, "CategoriaNombre"),
            .SubCategoriaCodigo = LeerString(reader, "SubCategoriaCodigo"),
            .SubCategoriaNombre = LeerString(reader, "SubCategoriaNombre"),
            .GrupoCodigo = LeerString(reader, "GrupoCodigo"),
            .GrupoNombre = LeerString(reader, "GrupoNombre"),
            .MarcaId = LeerInt32Nullable(reader, "MarcaId"),
            .MarcaCodigo = LeerString(reader, "MarcaCodigo"),
            .MarcaNombre = LeerString(reader, "MarcaNombre"),
            .ImpuestoId = LeerInt32(reader, "ImpuestoId"),
            .ImpuestoCodigo = LeerInt32Nullable(reader, "ImpuestoCodigo"),
            .ImpuestoNombre = LeerString(reader, "ImpuestoNombre"),
            .UnidadMedidaId = LeerInt32(reader, "UnidadMedidaId"),
            .UnidadCodigo = LeerString(reader, "UnidadCodigo"),
            .UnidadAbreviatura = LeerString(reader, "UnidadAbreviatura"),
            .Codigo_SIFEN = LeerInt16NullableFlexible(reader, "Codigo_SIFEN"),
            .Descrip_Unidad_SIFEN = LeerString(reader, "Descrip_Unidad_SIFEN"),
            .Codigo_Repr_SIFEN = LeerString(reader, "Codigo_Repr_SIFEN"),
            .CostoUltimo = LeerDecimal(reader, "CostoUltimo"),
            .PermiteStockNegativo = LeerBoolean(reader, "PermiteStockNegativo"),
            .ControlaStock = LeerBoolean(reader, "ControlaStock"),
            .EsServicio = LeerBoolean(reader, "EsServicio"),
            .Activo = LeerBoolean(reader, "Activo")
        }
    End Function
End Class
