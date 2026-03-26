Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

''' <summary>
''' Relación <c>dbo.UsuarioRoles</c>.
''' </summary>
Public Class UsuarioRolRepository
    Inherits RepositorioBase

    Private Const SqlRolIdsPorUsuario As String =
        "SELECT RolId FROM dbo.UsuarioRoles WHERE UsuarioId = @UsuarioId;"

    ''' <summary>
    ''' Roles asignados al usuario (activos o no; filtrar en capa de negocio si aplica).
    ''' </summary>
    Public Function ListarRolIdsPorUsuario(usuarioId As Integer) As List(Of Integer)
        Dim lista As New List(Of Integer)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlRolIdsPorUsuario, CommandType.Text)
                AgregarParametro(cmd, "@UsuarioId", usuarioId)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(LeerInt32(reader, "RolId"))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function
End Class
