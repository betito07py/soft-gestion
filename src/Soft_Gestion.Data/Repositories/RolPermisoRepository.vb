Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq

''' <summary>
''' Acceso a datos de <c>dbo.RolPermisos</c>.
''' </summary>
Public Class RolPermisoRepository
    Inherits RepositorioBase

    Private Const SqlPermisosPorRol As String =
        "SELECT PermisoId FROM dbo.RolPermisos WHERE RolId = @RolId;"

    Private Const SqlEliminarPorRol As String =
        "DELETE FROM dbo.RolPermisos WHERE RolId = @RolId;"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.RolPermisos (RolId, PermisoId, UsuarioCreacion) VALUES (@RolId, @PermisoId, @UsuarioCreacion);"

    ''' <summary>
    ''' Identificadores de permisos asignados al rol (sin orden contractual).
    ''' </summary>
    Public Function ObtenerPermisosPorRol(rolId As Integer) As List(Of Integer)
        Dim lista As New List(Of Integer)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlPermisosPorRol, CommandType.Text)
                AgregarParametro(cmd, "@RolId", rolId)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(LeerInt32(reader, "PermisoId"))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    ''' <summary>
    ''' Elimina todas las filas del rol e inserta las indicadas, en una sola transacción.
    ''' </summary>
    Public Sub ReemplazarPermisosDeRol(rolId As Integer, listaPermisoIds As IList(Of Integer), usuarioAuditoria As String)
        If listaPermisoIds Is Nothing Then Throw New ArgumentNullException(NameOf(listaPermisoIds))
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Throw New ArgumentException("El usuario de auditoría es obligatorio.", NameOf(usuarioAuditoria))

        Dim distintos = listaPermisoIds.Distinct().ToList()

        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Using cmdDel As SqlCommand = CrearComando(cn, SqlEliminarPorRol, CommandType.Text, tx)
                        AgregarParametro(cmdDel, "@RolId", rolId)
                        cmdDel.ExecuteNonQuery()
                    End Using

                    For Each permisoId In distintos
                        Using cmdIns As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text, tx)
                            AgregarParametro(cmdIns, "@RolId", rolId)
                            AgregarParametro(cmdIns, "@PermisoId", permisoId)
                            AgregarParametro(cmdIns, "@UsuarioCreacion", usuarioAuditoria.Trim())
                            cmdIns.ExecuteNonQuery()
                        End Using
                    Next

                    tx.Commit()
                Catch
                    Try
                        tx.Rollback()
                    Catch
                    End Try
                    Throw
                End Try
            End Using
        End Using
    End Sub
End Class
