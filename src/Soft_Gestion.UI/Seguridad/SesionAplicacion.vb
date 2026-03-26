Imports System.Collections.Generic
Imports Soft_Gestion.Domain

''' <summary>
''' Usuario autenticado en esta instancia de la aplicación. Los permisos efectivos se cargan al abrir el MDI principal.
''' </summary>
Public NotInheritable Class SesionAplicacion
    Private Sub New()
    End Sub

    Public Shared Property UsuarioActual As Usuario

    ''' <summary>
    ''' Códigos de permiso (<c>dbo.Permisos.Codigo</c>) efectivos para el usuario actual, sin duplicados.
    ''' Deben coincidir con ClavesFormulario y con los Tag del menú.
    ''' </summary>
    Public Shared ReadOnly CodigosPermisoDelUsuario As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    ''' <summary>
    ''' Misma colección que CodigosPermisoDelUsuario; nombre orientado a lectura de permisos actuales del logueado.
    ''' </summary>
    Public Shared ReadOnly Property PermisosActuales As ISet(Of String)
        Get
            Return CodigosPermisoDelUsuario
        End Get
    End Property

    ''' <summary>
    ''' Indica si el usuario puede usar una clave de permiso (menú, formularios). Los administradores tienen acceso total.
    ''' </summary>
    Public Shared Function UsuarioTienePermiso(clave As String) As Boolean
        If String.IsNullOrWhiteSpace(clave) Then Return False
        If UsuarioActual Is Nothing Then Return False
        If UsuarioActual.EsAdministrador Then Return True
        Return CodigosPermisoDelUsuario.Contains(clave.Trim())
    End Function
End Class
