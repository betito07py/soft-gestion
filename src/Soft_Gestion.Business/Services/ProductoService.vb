Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para productos (clasificación por grupo, marca, unidad y SIFEN vía unidad).
''' </summary>
Public Class ProductoService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 30
    Private Const LongitudMaxCodigoBarras As Integer = 50
    Private Const LongitudMaxDescripcion As Integer = 200
    Private Const LongitudMaxObservaciones As Integer = 255

    Private ReadOnly _productos As ProductoRepository
    Private ReadOnly _productosBarras As ProductoBarraRepository
    Private ReadOnly _grupos As GrupoRepository
    Private ReadOnly _subcategorias As SubCategoriaRepository
    Private ReadOnly _marcas As MarcaRepository
    Private ReadOnly _unidades As UnidadMedidaRepository
    Private ReadOnly _categorias As CategoriaRepository
    Private ReadOnly _impuestos As ImpuestoRepository

    Public Sub New(Optional productos As ProductoRepository = Nothing,
                   Optional productosBarras As ProductoBarraRepository = Nothing,
                   Optional grupos As GrupoRepository = Nothing,
                   Optional subcategorias As SubCategoriaRepository = Nothing,
                   Optional marcas As MarcaRepository = Nothing,
                   Optional unidades As UnidadMedidaRepository = Nothing,
                   Optional categorias As CategoriaRepository = Nothing,
                   Optional impuestos As ImpuestoRepository = Nothing)
        MyBase.New()
        _productos = If(productos, New ProductoRepository())
        _productosBarras = If(productosBarras, New ProductoBarraRepository())
        _grupos = If(grupos, New GrupoRepository())
        _subcategorias = If(subcategorias, New SubCategoriaRepository())
        _marcas = If(marcas, New MarcaRepository())
        _unidades = If(unidades, New UnidadMedidaRepository())
        _categorias = If(categorias, New CategoriaRepository())
        _impuestos = If(impuestos, New ImpuestoRepository())
    End Sub

    Public Function ListarProductos(filtroTexto As String,
                                    categoriaId As Integer?,
                                    subCategoriaId As Integer?,
                                    grupoId As Integer?,
                                    marcaId As Integer?,
                                    activo As Boolean?) As List(Of ProductoResumen)
        Return _productos.Listar(filtroTexto, categoriaId, subCategoriaId, grupoId, marcaId, activo)
    End Function

    Public Function ObtenerPorId(productoId As Integer) As Producto
        Return _productos.ObtenerPorId(productoId)
    End Function

    ''' <summary>Códigos de barras asociados al producto (tabla <c>Productos_Barras</c>).</summary>
    Public Function ListarCodigosBarras(productoId As Integer) As List(Of ProductoBarra)
        If productoId <= 0 Then Return New List(Of ProductoBarra)()
        Return _productosBarras.ListarPorProductoId(productoId)
    End Function

    Public Function ObtenerGrupoPorId(grupoId As Integer) As Grupo
        Return _grupos.ObtenerPorId(grupoId)
    End Function

    ''' <summary>Incluye categoría inactiva si hace falta para edición.</summary>
    Public Function ObtenerItemCategoriaParaEdicion(categoriaId As Integer?) As CategoriaSelectorItem
        If Not categoriaId.HasValue OrElse categoriaId.Value <= 0 Then Return Nothing
        Dim c = _categorias.ObtenerPorId(categoriaId.Value)
        If c Is Nothing Then Return Nothing
        Return New CategoriaSelectorItem With {.CategoriaId = c.CategoriaId, .Texto = c.Codigo & " — " & c.Nombre}
    End Function

    ''' <summary>Incluye subcategoría inactiva si hace falta para edición.</summary>
    Public Function ObtenerItemSubCategoriaParaEdicion(subCategoriaId As Integer?) As SubCategoriaSelectorItem
        If Not subCategoriaId.HasValue OrElse subCategoriaId.Value <= 0 Then Return Nothing
        Dim s = _subcategorias.ObtenerPorId(subCategoriaId.Value)
        If s Is Nothing Then Return Nothing
        Return New SubCategoriaSelectorItem With {.SubCategoriaId = s.SubCategoriaId, .Texto = s.Codigo & " — " & s.Nombre}
    End Function

    Public Function ListarCategoriasActivasParaSelector() As List(Of CategoriaSelectorItem)
        Return _categorias.ListarActivasParaCombo()
    End Function

    Public Function ListarSubCategoriasActivasParaSelector(categoriaId As Integer?) As List(Of SubCategoriaSelectorItem)
        Return _subcategorias.ListarActivasParaCombo(categoriaId)
    End Function

    Public Function ListarGruposActivasParaSelector(subCategoriaId As Integer?) As List(Of GrupoSelectorItem)
        Return _grupos.ListarActivasParaCombo(subCategoriaId)
    End Function

    Public Function ListarMarcasActivasParaSelector() As List(Of MarcaSelectorItem)
        Return _marcas.ListarActivasParaCombo()
    End Function

    Public Function ListarUnidadesActivasParaSelector() As List(Of UnidadMedidaResumen)
        Return _unidades.ListarActivasParaCombo()
    End Function

    Public Function ListarImpuestosActivosParaSelector() As List(Of ImpuestoSelectorItem)
        Return _impuestos.ListarParaSelector()
    End Function

    ''' <summary>Incluye impuesto inactivo si el producto ya lo tenía asignado.</summary>
    Public Function ObtenerItemImpuestoParaEdicion(impuestoId As Integer?) As ImpuestoSelectorItem
        If Not impuestoId.HasValue OrElse impuestoId.Value <= 0 Then Return Nothing
        Dim i = _impuestos.ObtenerPorId(impuestoId.Value)
        If i Is Nothing Then Return Nothing
        Return New ImpuestoSelectorItem With {.ImpuestoId = i.ImpuestoId, .Nombre = i.Codigo.ToString() & " — " & i.Nombre}
    End Function

    ''' <summary>Incluye grupo inactivo si el producto ya lo tenía y no está en el combo de activos.</summary>
    Public Function ObtenerItemGrupoParaEdicion(grupoId As Integer?) As GrupoSelectorItem
        If Not grupoId.HasValue OrElse grupoId.Value <= 0 Then Return Nothing
        Dim g = _grupos.ObtenerPorId(grupoId.Value)
        If g Is Nothing Then Return Nothing
        Return New GrupoSelectorItem With {.GrupoId = g.GrupoId, .Texto = g.Codigo & " — " & g.Nombre}
    End Function

    ''' <summary>Incluye marca inactiva si el producto ya la tenía asignada.</summary>
    Public Function ObtenerItemMarcaParaEdicion(marcaId As Integer?) As MarcaSelectorItem
        If Not marcaId.HasValue OrElse marcaId.Value <= 0 Then Return Nothing
        Dim m = _marcas.ObtenerPorId(marcaId.Value)
        If m Is Nothing Then Return Nothing
        Return New MarcaSelectorItem With {.MarcaId = m.MarcaId, .Texto = m.Codigo & " — " & m.Nombre}
    End Function

    ''' <summary>Incluye unidad inactiva si el producto ya la tenía (con datos SIFEN desde JOIN del repositorio de unidades).</summary>
    Public Function ObtenerResumenUnidadParaEdicion(unidadMedidaId As Integer) As UnidadMedidaResumen
        Dim u = _unidades.ObtenerPorId(unidadMedidaId)
        If u Is Nothing Then Return Nothing
        Return New UnidadMedidaResumen With {
            .UnidadMedidaId = u.UnidadMedidaId,
            .Codigo = u.Codigo,
            .Nombre = u.Nombre,
            .Abreviatura = u.Abreviatura,
            .Activo = u.Activo,
            .Codigo_SIFEN = u.Codigo_SIFEN,
            .Descrip_Unidad_SIFEN = u.Descrip_Unidad_SIFEN,
            .Codigo_Repr_SIFEN = u.Codigo_Repr_SIFEN
        }
    End Function

    Public Function GuardarNuevo(p As Producto, usuarioAuditoria As String, Optional codigosBarras As IList(Of String) = Nothing) As ResultadoOperacion
        If p Is Nothing Then Return ResultadoOperacion.Fallo("Datos de producto no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        p.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(p.Codigo)
        AplicarReglaServicioSinStock(p)
        Dim val = ValidarProducto(p)
        If Not val.Exitoso Then Return val
        Dim listaBarras = NormalizarListaCodigosBarrasEntrada(codigosBarras)
        Dim valBarras = ValidarCodigosBarras(listaBarras, Nothing)
        If Not valBarras.Exitoso Then Return valBarras
        Dim ref = ValidarGrupoMarcaUnidad(p, Nothing)
        If Not ref.Exitoso Then Return ref
        Dim refImp = ValidarImpuestoProducto(p, Nothing)
        If Not refImp.Exitoso Then Return refImp
        Dim impGuardar = _impuestos.ObtenerPorId(p.ImpuestoId)
        If impGuardar Is Nothing Then Return ResultadoOperacion.Fallo("El impuesto no existe.")
        If _productos.ExisteCodigo(p.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un producto con ese código.")
        End If

        Dim primeroBarra As String = If(listaBarras.Count > 0, listaBarras(0), Nothing)
        Dim nuevo As New Producto With {
            .Codigo = p.Codigo,
            .CodigoBarras = primeroBarra,
            .Descripcion = p.Descripcion.Trim(),
            .GrupoId = p.GrupoId,
            .MarcaId = p.MarcaId,
            .ImpuestoId = p.ImpuestoId,
            .UnidadMedidaId = p.UnidadMedidaId,
            .CostoUltimo = p.CostoUltimo,
            .PermiteStockNegativo = p.PermiteStockNegativo,
            .ControlaStock = p.ControlaStock,
            .EsServicio = p.EsServicio,
            .Observaciones = NormalizarOpcional(p.Observaciones),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _productos.Insertar(nuevo, impGuardar.Porcentaje)
            Try
                _productosBarras.ReemplazarTodos(id, listaBarras)
            Catch
                Return ResultadoOperacion.Fallo("El producto se guardó pero no se pudieron registrar los códigos de barras (¿duplicado en otro producto?).")
            End Try
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el producto.")
        End Try
    End Function

    Public Function EditarExistente(p As Producto, usuarioAuditoria As String, Optional codigosBarras As IList(Of String) = Nothing) As ResultadoOperacion
        If p Is Nothing OrElse p.ProductoId <= 0 Then Return ResultadoOperacion.Fallo("Producto no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        p.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(p.Codigo)
        AplicarReglaServicioSinStock(p)
        Dim val = ValidarProducto(p)
        If Not val.Exitoso Then Return val
        Dim listaBarras = NormalizarListaCodigosBarrasEntrada(codigosBarras)
        Dim valBarras = ValidarCodigosBarras(listaBarras, p.ProductoId)
        If Not valBarras.Exitoso Then Return valBarras

        Dim existente = _productos.ObtenerPorId(p.ProductoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El producto no existe.")
        Dim ref = ValidarGrupoMarcaUnidad(p, existente)
        If Not ref.Exitoso Then Return ref
        Dim refImp = ValidarImpuestoProducto(p, existente)
        If Not refImp.Exitoso Then Return refImp
        Dim impGuardar = _impuestos.ObtenerPorId(p.ImpuestoId)
        If impGuardar Is Nothing Then Return ResultadoOperacion.Fallo("El impuesto no existe.")
        If _productos.ExisteCodigo(p.Codigo, p.ProductoId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro producto con ese código.")
        End If

        existente.Codigo = p.Codigo
        existente.CodigoBarras = If(listaBarras.Count > 0, listaBarras(0), Nothing)
        existente.Descripcion = p.Descripcion.Trim()
        existente.GrupoId = p.GrupoId
        existente.MarcaId = p.MarcaId
        existente.ImpuestoId = p.ImpuestoId
        existente.UnidadMedidaId = p.UnidadMedidaId
        existente.CostoUltimo = p.CostoUltimo
        existente.PermiteStockNegativo = p.PermiteStockNegativo
        existente.ControlaStock = p.ControlaStock
        existente.EsServicio = p.EsServicio
        existente.Observaciones = NormalizarOpcional(p.Observaciones)

        Try
            Dim filas = _productos.Actualizar(existente, usuarioAuditoria.Trim(), impGuardar.Porcentaje)
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el producto.")
            Try
                _productosBarras.ReemplazarTodos(p.ProductoId, listaBarras)
            Catch
                Return ResultadoOperacion.Fallo("El producto se actualizó pero no se pudieron guardar los códigos de barras (¿duplicado en otro producto?).")
            End Try
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el producto.")
        End Try
    End Function

    Public Function Activar(productoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(productoId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(productoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(productoId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(productoId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If productoId <= 0 Then Return ResultadoOperacion.Fallo("Producto no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        If _productos.ObtenerPorId(productoId) Is Nothing Then Return ResultadoOperacion.Fallo("El producto no existe.")
        Try
            Dim filas = If(activo, _productos.Activar(productoId, usuarioAuditoria.Trim()), _productos.Desactivar(productoId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del producto.")
        End Try
    End Function

    Private Shared Sub AplicarReglaServicioSinStock(p As Producto)
        If p.EsServicio Then p.ControlaStock = False
    End Sub

    Private Function ValidarProducto(p As Producto) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(p.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If p.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(p.Descripcion) Then Return ResultadoOperacion.Fallo("La descripción es obligatoria.")
        If p.Descripcion.Trim().Length > LongitudMaxDescripcion Then Return ResultadoOperacion.Fallo("La descripción supera la longitud permitida.")
        If Not String.IsNullOrWhiteSpace(p.Observaciones) AndAlso p.Observaciones.Trim().Length > LongitudMaxObservaciones Then
            Return ResultadoOperacion.Fallo("Las observaciones superan la longitud permitida.")
        End If
        If p.UnidadMedidaId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar una unidad de medida.")
        If p.CostoUltimo < 0 Then Return ResultadoOperacion.Fallo("El costo último no puede ser negativo.")
        If p.EsServicio AndAlso p.ControlaStock Then
            Return ResultadoOperacion.Fallo("Un servicio no puede controlar stock.")
        End If
        Return ResultadoOperacion.Ok()
    End Function

    Private Function ValidarCodigosBarras(codigosNormalizados As List(Of String), excluirProductoId As Integer?) As ResultadoOperacion
        If codigosNormalizados Is Nothing Then Return ResultadoOperacion.Ok()
        Dim vistos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each c In codigosNormalizados
            If c.Length > LongitudMaxCodigoBarras Then
                Return ResultadoOperacion.Fallo("Un código de barras supera la longitud permitida (" & LongitudMaxCodigoBarras & " caracteres).")
            End If
            If Not vistos.Add(c) Then
                Return ResultadoOperacion.Fallo("Hay códigos de barras repetidos en la lista.")
            End If
            If _productosBarras.ExisteCodBarrasEnOtroProducto(c, excluirProductoId) Then
                Return ResultadoOperacion.Fallo("El código de barras """ & c & """ ya está asignado a otro producto.")
            End If
        Next
        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarListaCodigosBarrasEntrada(codigos As IList(Of String)) As List(Of String)
        Dim r As New List(Of String)()
        If codigos Is Nothing Then Return r
        Dim vistos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each c In codigos
            If String.IsNullOrWhiteSpace(c) Then Continue For
            Dim t = c.Trim()
            If vistos.Add(t) Then r.Add(t)
        Next
        Return r
    End Function

    Private Function ValidarImpuestoProducto(p As Producto, existente As Producto) As ResultadoOperacion
        If p.ImpuestoId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar un impuesto.")
        Dim imp = _impuestos.ObtenerPorId(p.ImpuestoId)
        If imp Is Nothing Then Return ResultadoOperacion.Fallo("El impuesto no existe.")
        Dim esNuevo = existente Is Nothing
        Dim cambioImpuesto = esNuevo OrElse existente.ImpuestoId <> p.ImpuestoId
        If Not imp.EsActivo AndAlso cambioImpuesto Then
            Return ResultadoOperacion.Fallo("El impuesto seleccionado no está activo.")
        End If
        Return ResultadoOperacion.Ok()
    End Function

    ''' <param name="existente">Nothing en altas.</param>
    Private Function ValidarGrupoMarcaUnidad(p As Producto, existente As Producto) As ResultadoOperacion
        Dim esNuevo = existente Is Nothing

        If Not p.GrupoId.HasValue OrElse p.GrupoId.Value <= 0 Then
            Return ResultadoOperacion.Fallo("Debe seleccionar un grupo.")
        End If
        Dim g = _grupos.ObtenerPorId(p.GrupoId.Value)
        If g Is Nothing Then Return ResultadoOperacion.Fallo("El grupo no existe.")
        If Not _subcategorias.PerteneceACategoria(g.SubCategoriaId, g.CategoriaId) Then
            Return ResultadoOperacion.Fallo("El grupo seleccionado es inconsistente con su subcategoría.")
        End If
        Dim cambioGrupo = esNuevo OrElse Not existente.GrupoId.HasValue OrElse existente.GrupoId.Value <> p.GrupoId.Value
        If Not g.Activo AndAlso cambioGrupo Then
            Return ResultadoOperacion.Fallo("El grupo seleccionado no está activo.")
        End If

        If p.MarcaId.HasValue AndAlso p.MarcaId.Value > 0 Then
            Dim m = _marcas.ObtenerPorId(p.MarcaId.Value)
            If m Is Nothing Then Return ResultadoOperacion.Fallo("La marca no existe.")
            Dim cambioMarca = esNuevo OrElse Not IgualNullableInt(existente.MarcaId, p.MarcaId)
            If Not m.Activo AndAlso cambioMarca Then
                Return ResultadoOperacion.Fallo("La marca seleccionada no está activa.")
            End If
        End If

        Dim u = _unidades.ObtenerPorId(p.UnidadMedidaId)
        If u Is Nothing Then Return ResultadoOperacion.Fallo("La unidad de medida no existe.")
        Dim cambioUnidad = esNuevo OrElse existente.UnidadMedidaId <> p.UnidadMedidaId
        If Not u.Activo AndAlso cambioUnidad Then
            Return ResultadoOperacion.Fallo("La unidad de medida seleccionada no está activa.")
        End If

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function IgualNullableInt(a As Integer?, b As Integer?) As Boolean
        If Not a.HasValue AndAlso Not b.HasValue Then Return True
        If a.HasValue <> b.HasValue Then Return False
        Return a.Value = b.Value
    End Function

    Private Shared Function NormalizarOpcional(s As String) As String
        If String.IsNullOrWhiteSpace(s) Then Return Nothing
        Return s.Trim()
    End Function
End Class
