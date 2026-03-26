Imports System.Collections.Generic
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para permisos. La matriz <c>RolPermisos</c> se gestionará en un flujo dedicado.
''' </summary>
Public Class PermisoService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 100
    Private Const LongitudMaxNombre As Integer = 150
    Private Const LongitudMaxModulo As Integer = 100
    Private Const LongitudMaxFormulario As Integer = 100
    Private Const LongitudMaxAccion As Integer = 50
    Private Const LongitudMaxDescripcion As Integer = 255

    Private ReadOnly _repositorio As PermisoRepository

    Public Sub New(Optional repositorio As PermisoRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New PermisoRepository())
    End Sub

    Public Function ListarPermisos(filtroTexto As String) As List(Of PermisoResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(permisoId As Integer) As Permiso
        Return _repositorio.ObtenerPorId(permisoId)
    End Function

    Public Function GuardarNuevo(permiso As Permiso, usuarioAuditoria As String) As ResultadoOperacion
        If permiso Is Nothing Then Return ResultadoOperacion.Fallo("Datos de permiso no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarPermiso(permiso)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(permiso.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un permiso con ese código.")
        End If

        Dim nuevo As New Permiso With {
            .Codigo = permiso.Codigo.Trim(),
            .Nombre = permiso.Nombre.Trim(),
            .Modulo = permiso.Modulo.Trim(),
            .Formulario = NormalizarOpcional(permiso.Formulario),
            .Accion = NormalizarOpcional(permiso.Accion),
            .Descripcion = NormalizarOpcional(permiso.Descripcion),
            .Activo = True
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el permiso.")
        End Try
    End Function

    Public Function EditarExistente(permiso As Permiso, usuarioAuditoria As String) As ResultadoOperacion
        If permiso Is Nothing OrElse permiso.PermisoId <= 0 Then Return ResultadoOperacion.Fallo("Permiso no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarPermiso(permiso)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(permiso.PermisoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El permiso no existe.")
        If _repositorio.ExisteCodigo(permiso.Codigo, permiso.PermisoId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro permiso con ese código.")
        End If

        existente.Codigo = permiso.Codigo.Trim()
        existente.Nombre = permiso.Nombre.Trim()
        existente.Modulo = permiso.Modulo.Trim()
        existente.Formulario = NormalizarOpcional(permiso.Formulario)
        existente.Accion = NormalizarOpcional(permiso.Accion)
        existente.Descripcion = NormalizarOpcional(permiso.Descripcion)

        Try
            Dim filas = _repositorio.Actualizar(existente)
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el permiso.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el permiso.")
        End Try
    End Function

    Public Function Activar(permisoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(permisoId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(permisoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(permisoId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(permisoId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If permisoId <= 0 Then Return ResultadoOperacion.Fallo("Permiso no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(permisoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El permiso no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(permisoId), _repositorio.Desactivar(permisoId))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del permiso.")
        End Try
    End Function

    Private Function ValidarPermiso(permiso As Permiso) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(permiso.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If permiso.Codigo.Trim().Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(permiso.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If permiso.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(permiso.Modulo) Then Return ResultadoOperacion.Fallo("El módulo es obligatorio.")
        If permiso.Modulo.Trim().Length > LongitudMaxModulo Then Return ResultadoOperacion.Fallo("El módulo supera la longitud permitida.")

        Dim form = If(permiso.Formulario, String.Empty).Trim()
        If form.Length > LongitudMaxFormulario Then Return ResultadoOperacion.Fallo("El formulario supera la longitud permitida.")

        Dim acc = If(permiso.Accion, String.Empty).Trim()
        If acc.Length > LongitudMaxAccion Then Return ResultadoOperacion.Fallo("La acción supera la longitud permitida.")

        Dim desc = If(permiso.Descripcion, String.Empty).Trim()
        If desc.Length > LongitudMaxDescripcion Then Return ResultadoOperacion.Fallo("La descripción supera la longitud permitida.")

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarOpcional(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Return valor.Trim()
    End Function
End Class
