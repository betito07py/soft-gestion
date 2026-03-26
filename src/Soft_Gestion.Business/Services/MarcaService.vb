Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para marcas de producto.
''' </summary>
Public Class MarcaService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100

    Private ReadOnly _repositorio As MarcaRepository

    Public Sub New(Optional repositorio As MarcaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New MarcaRepository())
    End Sub

    Public Function ListarMarcas(filtroTexto As String) As List(Of MarcaResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(marcaId As Integer) As Marca
        Return _repositorio.ObtenerPorId(marcaId)
    End Function

    Public Function ListarActivasParaCombo() As List(Of MarcaSelectorItem)
        Return _repositorio.ListarActivasParaCombo()
    End Function

    Public Function GuardarNuevo(marca As Marca, usuarioAuditoria As String) As ResultadoOperacion
        If marca Is Nothing Then Return ResultadoOperacion.Fallo("Datos de marca no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        marca.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(marca.Codigo)
        Dim val = ValidarMarca(marca)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(marca.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una marca con ese código.")
        End If
        If _repositorio.ExisteNombre(marca.Nombre, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una marca con ese nombre.")
        End If

        Dim nuevo As New Marca With {
            .Codigo = marca.Codigo,
            .Nombre = marca.Nombre.Trim(),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la marca.")
        End Try
    End Function

    Public Function EditarExistente(marca As Marca, usuarioAuditoria As String) As ResultadoOperacion
        If marca Is Nothing OrElse marca.MarcaId <= 0 Then Return ResultadoOperacion.Fallo("Marca no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        marca.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(marca.Codigo)
        Dim val = ValidarMarca(marca)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(marca.MarcaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La marca no existe.")
        If _repositorio.ExisteCodigo(marca.Codigo, marca.MarcaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra marca con ese código.")
        End If
        If _repositorio.ExisteNombre(marca.Nombre, marca.MarcaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra marca con ese nombre.")
        End If

        existente.Codigo = marca.Codigo
        existente.Nombre = marca.Nombre.Trim()

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar la marca.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar la marca.")
        End Try
    End Function

    Public Function Activar(marcaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(marcaId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(marcaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(marcaId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(marcaId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If marcaId <= 0 Then Return ResultadoOperacion.Fallo("Marca no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(marcaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La marca no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(marcaId, usuarioAuditoria.Trim()), _repositorio.Desactivar(marcaId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado de la marca.")
        End Try
    End Function

    Private Function ValidarMarca(marca As Marca) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(marca.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If marca.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(marca.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If marca.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        Return ResultadoOperacion.Ok()
    End Function
End Class
