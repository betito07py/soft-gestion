Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para subcategorías de producto.
''' </summary>
Public Class SubCategoriaService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100

    Private ReadOnly _repositorio As SubCategoriaRepository
    Private ReadOnly _categorias As CategoriaRepository

    Public Sub New(Optional repositorio As SubCategoriaRepository = Nothing,
                   Optional categorias As CategoriaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New SubCategoriaRepository())
        _categorias = If(categorias, New CategoriaRepository())
    End Sub

    Public Function ListarSubCategorias(filtroTexto As String, categoriaId As Integer?) As List(Of SubCategoriaResumen)
        Return _repositorio.Listar(filtroTexto, categoriaId)
    End Function

    Public Function ObtenerPorId(subCategoriaId As Integer) As SubCategoria
        Return _repositorio.ObtenerPorId(subCategoriaId)
    End Function

    ''' <summary>Código sugerido para una subcategoría nueva bajo la categoría indicada.</summary>
    Public Function ObtenerCodigoSugeridoParaNuevaSubCategoria(categoriaId As Integer) As String
        If categoriaId <= 0 Then Return String.Empty
        Return _repositorio.ObtenerSiguienteCodigoNumericoSugerido(categoriaId).ToString()
    End Function

    Public Function ListarCategoriasActivasParaSelector() As List(Of CategoriaSelectorItem)
        Return _categorias.ListarActivasParaCombo()
    End Function

    ''' <summary>
    ''' Incluye categoría inactiva si la subcategoría ya estaba asociada y no aparece en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemCategoriaParaEdicion(categoriaId As Integer?) As CategoriaSelectorItem
        If Not categoriaId.HasValue OrElse categoriaId.Value <= 0 Then Return Nothing
        Dim c = _categorias.ObtenerPorId(categoriaId.Value)
        If c Is Nothing Then Return Nothing
        Return New CategoriaSelectorItem With {
            .CategoriaId = c.CategoriaId,
            .Texto = c.Codigo & " — " & c.Nombre
        }
    End Function

    Public Function GuardarNuevo(subCategoria As SubCategoria, usuarioAuditoria As String) As ResultadoOperacion
        If subCategoria Is Nothing Then Return ResultadoOperacion.Fallo("Datos de subcategoría no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        subCategoria.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(subCategoria.Codigo)
        Dim val = ValidarSubCategoria(subCategoria)
        If Not val.Exitoso Then Return val
        If Not _categorias.ExisteActiva(subCategoria.CategoriaId) Then
            Return ResultadoOperacion.Fallo("La categoría no existe o está inactiva.")
        End If
        If _repositorio.ExisteCodigo(subCategoria.CategoriaId, subCategoria.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una subcategoría con ese código en la categoría seleccionada.")
        End If
        If _repositorio.ExisteNombre(subCategoria.CategoriaId, subCategoria.Nombre, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una subcategoría con ese nombre en la categoría seleccionada.")
        End If

        Dim nuevo As New SubCategoria With {
            .Codigo = subCategoria.Codigo,
            .Nombre = subCategoria.Nombre.Trim(),
            .CategoriaId = subCategoria.CategoriaId,
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la subcategoría.")
        End Try
    End Function

    Public Function EditarExistente(subCategoria As SubCategoria, usuarioAuditoria As String) As ResultadoOperacion
        If subCategoria Is Nothing OrElse subCategoria.SubCategoriaId <= 0 Then Return ResultadoOperacion.Fallo("Subcategoría no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        subCategoria.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(subCategoria.Codigo)
        Dim val = ValidarSubCategoria(subCategoria)
        If Not val.Exitoso Then Return val
        If Not _categorias.ExisteActiva(subCategoria.CategoriaId) Then
            Return ResultadoOperacion.Fallo("La categoría no existe o está inactiva.")
        End If

        Dim existente = _repositorio.ObtenerPorId(subCategoria.SubCategoriaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La subcategoría no existe.")
        If _repositorio.ExisteCodigo(subCategoria.CategoriaId, subCategoria.Codigo, subCategoria.SubCategoriaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra subcategoría con ese código en la categoría seleccionada.")
        End If
        If _repositorio.ExisteNombre(subCategoria.CategoriaId, subCategoria.Nombre, subCategoria.SubCategoriaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra subcategoría con ese nombre en la categoría seleccionada.")
        End If

        existente.Codigo = subCategoria.Codigo
        existente.Nombre = subCategoria.Nombre.Trim()
        existente.CategoriaId = subCategoria.CategoriaId

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar la subcategoría.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar la subcategoría.")
        End Try
    End Function

    Public Function Activar(subCategoriaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(subCategoriaId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(subCategoriaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(subCategoriaId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(subCategoriaId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If subCategoriaId <= 0 Then Return ResultadoOperacion.Fallo("Subcategoría no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(subCategoriaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La subcategoría no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(subCategoriaId, usuarioAuditoria.Trim()), _repositorio.Desactivar(subCategoriaId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado de la subcategoría.")
        End Try
    End Function

    Private Function ValidarSubCategoria(subCategoria As SubCategoria) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(subCategoria.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If subCategoria.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(subCategoria.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If subCategoria.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        If subCategoria.CategoriaId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar una categoría.")
        Return ResultadoOperacion.Ok()
    End Function
End Class
