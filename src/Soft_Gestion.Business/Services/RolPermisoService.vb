Imports System.Collections.Generic
Imports System.Linq
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Asignación de permisos a roles (<c>RolPermisos</c>).
''' </summary>
Public Class RolPermisoService
    Inherits ServicioBase

    Private ReadOnly _repositorioRolPermiso As RolPermisoRepository
    Private ReadOnly _repositorioPermiso As PermisoRepository
    Private ReadOnly _repositorioRol As RolRepository

    Public Sub New(Optional repositorioRolPermiso As RolPermisoRepository = Nothing,
                   Optional repositorioPermiso As PermisoRepository = Nothing,
                   Optional repositorioRol As RolRepository = Nothing)
        MyBase.New()
        _repositorioRolPermiso = If(repositorioRolPermiso, New RolPermisoRepository())
        _repositorioPermiso = If(repositorioPermiso, New PermisoRepository())
        _repositorioRol = If(repositorioRol, New RolRepository())
    End Sub

    ''' <summary>
    ''' Identificadores de permisos actualmente asignados al rol.
    ''' </summary>
    Public Function ListarPermisosAsignados(rolId As Integer) As List(Of Integer)
        If rolId <= 0 Then Return New List(Of Integer)()
        Return _repositorioRolPermiso.ObtenerPermisosPorRol(rolId)
    End Function

    ''' <summary>
    ''' Líneas para la grilla: catálogo activo con marca según asignación al rol.
    ''' </summary>
    Public Function ObtenerLineasParaAsignacion(rolId As Integer) As List(Of PermisoLineaAsignacion)
        Dim catalogo = _repositorioPermiso.ListarActivos()
        Dim asignados As HashSet(Of Integer) = Nothing
        If rolId > 0 Then
            asignados = New HashSet(Of Integer)(_repositorioRolPermiso.ObtenerPermisosPorRol(rolId))
        Else
            asignados = New HashSet(Of Integer)()
        End If

        Dim lineas As New List(Of PermisoLineaAsignacion)()
        For Each p In catalogo
            lineas.Add(New PermisoLineaAsignacion With {
                .PermisoId = p.PermisoId,
                .Codigo = p.Codigo,
                .Nombre = p.Nombre,
                .Modulo = p.Modulo,
                .Asignado = asignados.Contains(p.PermisoId)
            })
        Next
        Return lineas
    End Function

    ''' <summary>
    ''' Persiste la asignación completa del rol (reemplazo atómico en base de datos).
    ''' </summary>
    Public Function GuardarAsignacionCompleta(rolId As Integer, permisoIdsSeleccionados As IList(Of Integer), usuarioAuditoria As String) As ResultadoOperacion
        If rolId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar un rol válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")

        Dim rol = _repositorioRol.ObtenerPorId(rolId)
        If rol Is Nothing Then Return ResultadoOperacion.Fallo("El rol no existe.")
        If Not rol.Activo Then Return ResultadoOperacion.Fallo("El rol está inactivo.")

        Dim lista = If(permisoIdsSeleccionados, New List(Of Integer)())
        Dim distintos = lista.Distinct().ToList()

        For Each pid In distintos
            If pid <= 0 Then Return ResultadoOperacion.Fallo("Hay identificadores de permiso no válidos.")
            If Not _repositorioPermiso.ExisteActivo(pid) Then
                Return ResultadoOperacion.Fallo("Uno de los permisos no existe o está inactivo.")
            End If
        Next

        Dim aud = usuarioAuditoria.Trim()
        If aud.Length > 50 Then Return ResultadoOperacion.Fallo("El usuario de auditoría supera la longitud permitida.")

        Try
            _repositorioRolPermiso.ReemplazarPermisosDeRol(rolId, distintos, aud)
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la asignación de permisos.")
        End Try
    End Function
End Class
