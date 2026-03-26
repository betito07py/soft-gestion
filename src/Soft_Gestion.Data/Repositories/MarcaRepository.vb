Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Marcas</c>.
''' </summary>
Public Class MarcaRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT MarcaId, Codigo, Nombre, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Marcas WHERE MarcaId = @MarcaId;"

    Private Const SqlListar As String =
        "SELECT MarcaId, Codigo, Nombre, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Marcas " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR Codigo LIKE @Patron OR Nombre LIKE @Patron) " &
        "ORDER BY Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Marcas WHERE Codigo = @Codigo " &
        "AND (@ExcluirMarcaId IS NULL OR MarcaId <> @ExcluirMarcaId);"

    Private Const SqlExisteNombre As String =
        "SELECT 1 FROM dbo.Marcas WHERE Nombre = @Nombre " &
        "AND (@ExcluirMarcaId IS NULL OR MarcaId <> @ExcluirMarcaId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Marcas (Codigo, Nombre, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.MarcaId " &
        "VALUES (@Codigo, @Nombre, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Marcas SET Codigo = @Codigo, Nombre = @Nombre, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE MarcaId = @MarcaId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Marcas SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE MarcaId = @MarcaId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT MarcaId, Codigo, Nombre FROM dbo.Marcas WHERE Activo = 1 ORDER BY Codigo;"

    Public Function ListarActivasParaCombo() As List(Of MarcaSelectorItem)
        Dim lista As New List(Of MarcaSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim id = LeerInt32(reader, "MarcaId")
                        Dim cod = LeerString(reader, "Codigo")
                        Dim nom = LeerString(reader, "Nombre")
                        lista.Add(New MarcaSelectorItem With {
                            .MarcaId = id,
                            .Texto = cod & " — " & nom
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(marcaId As Integer) As Marca
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@MarcaId", marcaId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearMarca(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of MarcaResumen)
        Dim lista As New List(Of MarcaResumen)()
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
                        lista.Add(MapearMarcaResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As String, excluirMarcaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirMarcaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirMarcaId", excluirMarcaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirMarcaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ExisteNombre(nombre As String, excluirMarcaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(nombre) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteNombre, CommandType.Text)
                AgregarParametro(cmd, "@Nombre", nombre.Trim())
                If excluirMarcaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirMarcaId", excluirMarcaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirMarcaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(marca As Marca) As Integer
        If marca Is Nothing Then Throw New ArgumentNullException(NameOf(marca))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", marca.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", marca.Nombre.Trim())
                AgregarParametro(cmd, "@Activo", marca.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", marca.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(marca As Marca, usuarioModificador As String) As Integer
        If marca Is Nothing Then Throw New ArgumentNullException(NameOf(marca))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@MarcaId", marca.MarcaId)
                AgregarParametro(cmd, "@Codigo", marca.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", marca.Nombre.Trim())
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(marcaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(marcaId, True, usuarioModificador)
    End Function

    Public Function Desactivar(marcaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(marcaId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(marcaId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@MarcaId", marcaId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearMarca(reader As SqlDataReader) As Marca
        Return New Marca With {
            .MarcaId = LeerInt32(reader, "MarcaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearMarcaResumen(reader As SqlDataReader) As MarcaResumen
        Return New MarcaResumen With {
            .MarcaId = LeerInt32(reader, "MarcaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
