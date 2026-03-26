Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Usuarios</c> mediante ADO.NET.
''' </summary>
Public Class UsuarioRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorLogin As String =
        "SELECT UsuarioId, Login, NombreCompleto, PasswordHash, Email, EsAdministrador, SucursalId, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Usuarios WHERE Login = @Login;"

    Private Const SqlObtenerPorId As String =
        "SELECT UsuarioId, Login, NombreCompleto, PasswordHash, Email, EsAdministrador, SucursalId, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Usuarios WHERE UsuarioId = @UsuarioId;"

    Private Const SqlListar As String =
        "SELECT UsuarioId, Login, NombreCompleto, Email, EsAdministrador, SucursalId, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Usuarios " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR Login LIKE @Patron OR NombreCompleto LIKE @Patron) " &
        "ORDER BY Login;"

    Private Const SqlExisteLogin As String =
        "SELECT 1 FROM dbo.Usuarios WHERE Login = @Login AND (@ExcluirUsuarioId IS NULL OR UsuarioId <> @ExcluirUsuarioId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Usuarios (Login, NombreCompleto, PasswordHash, Email, EsAdministrador, SucursalId, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.UsuarioId " &
        "VALUES (@Login, @NombreCompleto, @PasswordHash, @Email, @EsAdministrador, @SucursalId, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Usuarios SET Login = @Login, NombreCompleto = @NombreCompleto, PasswordHash = @PasswordHash, " &
        "Email = @Email, EsAdministrador = @EsAdministrador, SucursalId = @SucursalId, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE UsuarioId = @UsuarioId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Usuarios SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE UsuarioId = @UsuarioId;"

    ''' <summary>
    ''' Obtiene un usuario por <c>Login</c> (sin filtrar por Activo; la capa de negocio decide).
    ''' </summary>
    Public Function ObtenerPorLogin(login As String) As Usuario
        If String.IsNullOrWhiteSpace(login) Then Return Nothing
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorLogin, CommandType.Text)
                AgregarParametro(cmd, "@Login", login.Trim())
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearUsuario(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function ObtenerPorId(usuarioId As Integer) As Usuario
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@UsuarioId", usuarioId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearUsuario(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of UsuarioResumen)
        Dim lista As New List(Of UsuarioResumen)()
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
                        lista.Add(MapearUsuarioResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteLogin(login As String, excluirUsuarioId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(login) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteLogin, CommandType.Text)
                AgregarParametro(cmd, "@Login", login.Trim())
                If excluirUsuarioId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirUsuarioId", excluirUsuarioId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirUsuarioId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(usuario As Usuario) As Integer
        If usuario Is Nothing Then Throw New ArgumentNullException(NameOf(usuario))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Login", usuario.Login.Trim())
                AgregarParametro(cmd, "@NombreCompleto", usuario.NombreCompleto.Trim())
                AgregarParametro(cmd, "@PasswordHash", usuario.PasswordHash)
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(usuario.Email), CType(Nothing, Object), usuario.Email.Trim()))
                AgregarParametro(cmd, "@EsAdministrador", usuario.EsAdministrador)
                AgregarParametro(cmd, "@SucursalId", If(usuario.SucursalId.HasValue, CType(usuario.SucursalId.Value, Object), DBNull.Value))
                AgregarParametro(cmd, "@Activo", usuario.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", usuario.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(usuario As Usuario, usuarioModificador As String) As Integer
        If usuario Is Nothing Then Throw New ArgumentNullException(NameOf(usuario))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@UsuarioId", usuario.UsuarioId)
                AgregarParametro(cmd, "@Login", usuario.Login.Trim())
                AgregarParametro(cmd, "@NombreCompleto", usuario.NombreCompleto.Trim())
                AgregarParametro(cmd, "@PasswordHash", usuario.PasswordHash)
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(usuario.Email), CType(Nothing, Object), usuario.Email.Trim()))
                AgregarParametro(cmd, "@EsAdministrador", usuario.EsAdministrador)
                AgregarParametro(cmd, "@SucursalId", If(usuario.SucursalId.HasValue, CType(usuario.SucursalId.Value, Object), DBNull.Value))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(usuarioId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(usuarioId, True, usuarioModificador)
    End Function

    Public Function Desactivar(usuarioId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(usuarioId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(usuarioId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@UsuarioId", usuarioId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearUsuario(reader As SqlDataReader) As Usuario
        Return New Usuario With {
            .UsuarioId = LeerInt32(reader, "UsuarioId"),
            .Login = LeerString(reader, "Login"),
            .NombreCompleto = LeerString(reader, "NombreCompleto"),
            .PasswordHash = LeerString(reader, "PasswordHash"),
            .Email = LeerString(reader, "Email"),
            .EsAdministrador = LeerBoolean(reader, "EsAdministrador"),
            .SucursalId = LeerInt32Nullable(reader, "SucursalId"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearUsuarioResumen(reader As SqlDataReader) As UsuarioResumen
        Return New UsuarioResumen With {
            .UsuarioId = LeerInt32(reader, "UsuarioId"),
            .Login = LeerString(reader, "Login"),
            .NombreCompleto = LeerString(reader, "NombreCompleto"),
            .Email = LeerString(reader, "Email"),
            .EsAdministrador = LeerBoolean(reader, "EsAdministrador"),
            .SucursalId = LeerInt32Nullable(reader, "SucursalId"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
