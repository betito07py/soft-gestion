Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para categorías de producto.
''' </summary>
Public Class CategoriaService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100

    Private ReadOnly _repositorio As CategoriaRepository

    Public Sub New(Optional repositorio As CategoriaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New CategoriaRepository())
    End Sub

    Public Function ListarCategorias(filtroTexto As String) As List(Of CategoriaResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(categoriaId As Integer) As Categoria
        Return _repositorio.ObtenerPorId(categoriaId)
    End Function

    ''' <summary>Código numérico sugerido para una categoría nueva (último en uso + 1).</summary>
    Public Function ObtenerCodigoSugeridoParaNuevaCategoria() As String
        Return _repositorio.ObtenerSiguienteCodigoNumericoSugerido().ToString()
    End Function

    Public Function GuardarNuevo(categoria As Categoria, usuarioAuditoria As String) As ResultadoOperacion
        If categoria Is Nothing Then Return ResultadoOperacion.Fallo("Datos de categoría no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        categoria.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(categoria.Codigo)
        Dim val = ValidarCategoria(categoria)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(categoria.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una categoría con ese código.")
        End If
        If _repositorio.ExisteNombre(categoria.Nombre, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una categoría con ese nombre.")
        End If

        Dim nuevo As New Categoria With {
            .Codigo = categoria.Codigo,
            .Nombre = categoria.Nombre.Trim(),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la categoría.")
        End Try
    End Function

    Public Function EditarExistente(categoria As Categoria, usuarioAuditoria As String) As ResultadoOperacion
        If categoria Is Nothing OrElse categoria.CategoriaId <= 0 Then Return ResultadoOperacion.Fallo("Categoría no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        categoria.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(categoria.Codigo)
        Dim val = ValidarCategoria(categoria)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(categoria.CategoriaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La categoría no existe.")
        If _repositorio.ExisteCodigo(categoria.Codigo, categoria.CategoriaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra categoría con ese código.")
        End If
        If _repositorio.ExisteNombre(categoria.Nombre, categoria.CategoriaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra categoría con ese nombre.")
        End If

        existente.Codigo = categoria.Codigo
        existente.Nombre = categoria.Nombre.Trim()

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar la categoría.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar la categoría.")
        End Try
    End Function

    Public Function Activar(categoriaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(categoriaId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(categoriaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(categoriaId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(categoriaId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If categoriaId <= 0 Then Return ResultadoOperacion.Fallo("Categoría no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(categoriaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La categoría no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(categoriaId, usuarioAuditoria.Trim()), _repositorio.Desactivar(categoriaId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado de la categoría.")
        End Try
    End Function

    Private Function ValidarCategoria(categoria As Categoria) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(categoria.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If categoria.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(categoria.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If categoria.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        Return ResultadoOperacion.Ok()
    End Function
End Class
