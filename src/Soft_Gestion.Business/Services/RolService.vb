Imports System.Collections.Generic
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para roles. La matriz <c>RolPermisos</c> se gestionará en un flujo dedicado.
''' </summary>
Public Class RolService
    Inherits ServicioBase

    Private Const LongitudMaxNombre As Integer = 100
    Private Const LongitudMaxDescripcion As Integer = 255

    Private ReadOnly _repositorio As RolRepository

    Public Sub New(Optional repositorio As RolRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New RolRepository())
    End Sub

    Public Function ListarRoles(filtroTexto As String) As List(Of RolResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(rolId As Integer) As Rol
        Return _repositorio.ObtenerPorId(rolId)
    End Function

    Public Function GuardarNuevo(rol As Rol, usuarioAuditoria As String) As ResultadoOperacion
        If rol Is Nothing Then Return ResultadoOperacion.Fallo("Datos de rol no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarRol(rol)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteNombre(rol.Nombre, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un rol con ese nombre.")
        End If

        Dim nuevo As New Rol With {
            .Nombre = rol.Nombre.Trim(),
            .Descripcion = NormalizarDescripcion(rol.Descripcion),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el rol.")
        End Try
    End Function

    Public Function EditarExistente(rol As Rol, usuarioAuditoria As String) As ResultadoOperacion
        If rol Is Nothing OrElse rol.RolId <= 0 Then Return ResultadoOperacion.Fallo("Rol no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarRol(rol)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(rol.RolId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El rol no existe.")
        If _repositorio.ExisteNombre(rol.Nombre, rol.RolId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro rol con ese nombre.")
        End If

        existente.Nombre = rol.Nombre.Trim()
        existente.Descripcion = NormalizarDescripcion(rol.Descripcion)
        ' Activo se gestiona con Activar/Desactivar, no desde esta edición

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el rol.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el rol.")
        End Try
    End Function

    Public Function Activar(rolId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(rolId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(rolId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(rolId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(rolId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If rolId <= 0 Then Return ResultadoOperacion.Fallo("Rol no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(rolId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El rol no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(rolId, usuarioAuditoria.Trim()), _repositorio.Desactivar(rolId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del rol.")
        End Try
    End Function

    Private Function ValidarRol(rol As Rol) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(rol.Nombre) Then Return ResultadoOperacion.Fallo("El nombre del rol es obligatorio.")
        If rol.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        Dim desc = If(rol.Descripcion, String.Empty).Trim()
        If desc.Length > LongitudMaxDescripcion Then Return ResultadoOperacion.Fallo("La descripción supera la longitud permitida.")
        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarDescripcion(descripcion As String) As String
        If String.IsNullOrWhiteSpace(descripcion) Then Return Nothing
        Return descripcion.Trim()
    End Function
End Class
