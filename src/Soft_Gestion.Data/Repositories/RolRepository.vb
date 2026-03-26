Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Roles</c>. La asignación de permisos (<c>RolPermisos</c>) se implementará aparte.
''' </summary>
Public Class RolRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT RolId, Nombre, Descripcion, Activo, FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Roles WHERE RolId = @RolId;"

    Private Const SqlListar As String =
        "SELECT RolId, Nombre, Descripcion, Activo, FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Roles " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR Nombre LIKE @Patron OR Descripcion LIKE @Patron) " &
        "ORDER BY Nombre;"

    Private Const SqlExisteNombre As String =
        "SELECT 1 FROM dbo.Roles WHERE Nombre = @Nombre AND (@ExcluirRolId IS NULL OR RolId <> @ExcluirRolId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Roles (Nombre, Descripcion, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.RolId " &
        "VALUES (@Nombre, @Descripcion, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Roles SET Nombre = @Nombre, Descripcion = @Descripcion, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE RolId = @RolId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Roles SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE RolId = @RolId;"

    Public Function ObtenerPorId(rolId As Integer) As Rol
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@RolId", rolId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearRol(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of RolResumen)
        Dim lista As New List(Of RolResumen)()
        Dim filtroTrim As String = If(filtroTexto, String.Empty).Trim()
        Dim filtroParam As Object = If(filtroTrim.Length = 0, CType(Nothing, Object), filtroTrim)
        Dim patron As Object = If(filtroTrim.Length = 0, DBNull.Value, CType("%" & filtroTrim & "%", Object))

        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListar, CommandType.Text)
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearRolResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteNombre(nombre As String, excluirRolId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(nombre) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteNombre, CommandType.Text)
                AgregarParametro(cmd, "@Nombre", nombre.Trim())
                If excluirRolId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirRolId", excluirRolId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirRolId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(rol As Rol) As Integer
        If rol Is Nothing Then Throw New ArgumentNullException(NameOf(rol))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Nombre", rol.Nombre.Trim())
                AgregarParametro(cmd, "@Descripcion", If(String.IsNullOrWhiteSpace(rol.Descripcion), CType(Nothing, Object), rol.Descripcion.Trim()))
                AgregarParametro(cmd, "@Activo", rol.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", rol.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(rol As Rol, usuarioModificador As String) As Integer
        If rol Is Nothing Then Throw New ArgumentNullException(NameOf(rol))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@RolId", rol.RolId)
                AgregarParametro(cmd, "@Nombre", rol.Nombre.Trim())
                AgregarParametro(cmd, "@Descripcion", If(String.IsNullOrWhiteSpace(rol.Descripcion), CType(Nothing, Object), rol.Descripcion.Trim()))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(rolId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(rolId, True, usuarioModificador)
    End Function

    Public Function Desactivar(rolId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(rolId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(rolId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@RolId", rolId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearRol(reader As SqlDataReader) As Rol
        Return New Rol With {
            .RolId = LeerInt32(reader, "RolId"),
            .Nombre = LeerString(reader, "Nombre"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearRolResumen(reader As SqlDataReader) As RolResumen
        Return New RolResumen With {
            .RolId = LeerInt32(reader, "RolId"),
            .Nombre = LeerString(reader, "Nombre"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
