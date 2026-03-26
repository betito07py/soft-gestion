Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Consultas de permisos efectivos por usuario: <c>UsuarioRoles</c>, roles activos, <c>RolPermisos</c>, <c>Permisos</c> activos.
''' Devuelve códigos distintos (sin duplicados en SQL).
''' </summary>
Public Class PermisoEfectivoRepository
    Inherits RepositorioBase

    ''' <summary>
    ''' Códigos distintos de permisos que el usuario tiene por sus roles activos.
    ''' </summary>
    Private Const SqlCodigosPorUsuario As String =
        "SELECT DISTINCT P.Codigo " &
        "FROM dbo.UsuarioRoles UR " &
        "INNER JOIN dbo.Roles R ON R.RolId = UR.RolId AND R.Activo = 1 " &
        "INNER JOIN dbo.RolPermisos RP ON RP.RolId = R.RolId " &
        "INNER JOIN dbo.Permisos P ON P.PermisoId = RP.PermisoId AND P.Activo = 1 " &
        "WHERE UR.UsuarioId = @UsuarioId;"

    Public Function ObtenerCodigosPermisosEfectivosPorUsuario(usuarioId As Integer) As List(Of String)
        Dim lista As New List(Of String)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlCodigosPorUsuario, CommandType.Text)
                AgregarParametro(cmd, "@UsuarioId", usuarioId)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim c = LeerString(reader, "Codigo")
                        If Not String.IsNullOrWhiteSpace(c) Then lista.Add(c.Trim())
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function
End Class
