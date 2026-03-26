Imports System.Collections.Generic
Imports Soft_Gestion.Data

''' <summary>
''' Resuelve permisos efectivos: roles del usuario (activos) y permisos asignados a esos roles (activos).
''' </summary>
Public Class PermisoEfectivoService
    Inherits ServicioBase

    Private ReadOnly _usuarioRolRepo As UsuarioRolRepository
    Private ReadOnly _efectivoRepo As PermisoEfectivoRepository

    Public Sub New(Optional usuarioRolRepo As UsuarioRolRepository = Nothing,
                   Optional efectivoRepo As PermisoEfectivoRepository = Nothing)
        MyBase.New()
        _usuarioRolRepo = If(usuarioRolRepo, New UsuarioRolRepository())
        _efectivoRepo = If(efectivoRepo, New PermisoEfectivoRepository())
    End Sub

    ''' <summary>
    ''' Roles asignados al usuario (tabla <c>UsuarioRoles</c>).
    ''' </summary>
    Public Function ListarRolIdsDelUsuario(usuarioId As Integer) As List(Of Integer)
        If usuarioId <= 0 Then Return New List(Of Integer)()
        Return _usuarioRolRepo.ListarRolIdsPorUsuario(usuarioId)
    End Function

    ''' <summary>
    ''' Códigos de permiso efectivos del usuario (roles en <c>UsuarioRoles</c> → <c>RolPermisos</c> → <c>Permisos.Codigo</c>),
    ''' sin duplicados; solo roles y permisos activos.
    ''' </summary>
    Public Function ObtenerCodigosPermisosEfectivos(usuarioId As Integer) As HashSet(Of String)
        Dim hs As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        If usuarioId <= 0 Then Return hs
        For Each c In _efectivoRepo.ObtenerCodigosPermisosEfectivosPorUsuario(usuarioId)
            hs.Add(c)
        Next
        Return hs
    End Function
End Class
