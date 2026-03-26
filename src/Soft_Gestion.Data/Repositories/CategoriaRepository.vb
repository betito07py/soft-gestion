Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Categorias</c>.
''' </summary>
Public Class CategoriaRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT CategoriaId, Codigo, Nombre, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Categorias WHERE CategoriaId = @CategoriaId;"

    Private Const SqlListar As String =
        "SELECT CategoriaId, Codigo, Nombre, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Categorias " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR Codigo LIKE @Patron OR Nombre LIKE @Patron) " &
        "ORDER BY Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Categorias WHERE Codigo = @Codigo " &
        "AND (@ExcluirCategoriaId IS NULL OR CategoriaId <> @ExcluirCategoriaId);"

    Private Const SqlExisteNombre As String =
        "SELECT 1 FROM dbo.Categorias WHERE Nombre = @Nombre " &
        "AND (@ExcluirCategoriaId IS NULL OR CategoriaId <> @ExcluirCategoriaId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Categorias (Codigo, Nombre, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.CategoriaId " &
        "VALUES (@Codigo, @Nombre, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Categorias SET Codigo = @Codigo, Nombre = @Nombre, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE CategoriaId = @CategoriaId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Categorias SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE CategoriaId = @CategoriaId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT CategoriaId, Codigo, Nombre FROM dbo.Categorias WHERE Activo = 1 ORDER BY Codigo;"

    Private Const SqlExisteActiva As String =
        "SELECT 1 FROM dbo.Categorias WHERE CategoriaId = @CategoriaId AND Activo = 1;"

    Private Const SqlSiguienteCodigoCategoria As String =
        "SELECT ISNULL(MAX(TRY_CAST(Codigo AS INT)), 0) + 1 FROM dbo.Categorias;"

    ''' <summary>
    ''' Indica si la categoría existe y está activa (p. ej. validación al asignar subcategorías).
    ''' </summary>
    ''' <summary>
    ''' Siguiente código numérico sugerido para una categoría nueva (máximo existente + 1).
    ''' </summary>
    Public Function ObtenerSiguienteCodigoNumericoSugerido() As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlSiguienteCodigoCategoria, CommandType.Text)
                Return CInt(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    Public Function ExisteActiva(categoriaId As Integer) As Boolean
        If categoriaId <= 0 Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteActiva, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ListarActivasParaCombo() As List(Of CategoriaSelectorItem)
        Dim lista As New List(Of CategoriaSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim id = LeerInt32(reader, "CategoriaId")
                        Dim cod = LeerString(reader, "Codigo")
                        Dim nom = LeerString(reader, "Nombre")
                        lista.Add(New CategoriaSelectorItem With {
                            .CategoriaId = id,
                            .Texto = cod & " — " & nom
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(categoriaId As Integer) As Categoria
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearCategoria(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of CategoriaResumen)
        Dim lista As New List(Of CategoriaResumen)()
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
                        lista.Add(MapearCategoriaResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As String, excluirCategoriaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirCategoriaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirCategoriaId", excluirCategoriaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirCategoriaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ExisteNombre(nombre As String, excluirCategoriaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(nombre) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteNombre, CommandType.Text)
                AgregarParametro(cmd, "@Nombre", nombre.Trim())
                If excluirCategoriaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirCategoriaId", excluirCategoriaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirCategoriaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(categoria As Categoria) As Integer
        If categoria Is Nothing Then Throw New ArgumentNullException(NameOf(categoria))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", categoria.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", categoria.Nombre.Trim())
                AgregarParametro(cmd, "@Activo", categoria.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", categoria.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(categoria As Categoria, usuarioModificador As String) As Integer
        If categoria Is Nothing Then Throw New ArgumentNullException(NameOf(categoria))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoria.CategoriaId)
                AgregarParametro(cmd, "@Codigo", categoria.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", categoria.Nombre.Trim())
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(categoriaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(categoriaId, True, usuarioModificador)
    End Function

    Public Function Desactivar(categoriaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(categoriaId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(categoriaId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearCategoria(reader As SqlDataReader) As Categoria
        Return New Categoria With {
            .CategoriaId = LeerInt32(reader, "CategoriaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearCategoriaResumen(reader As SqlDataReader) As CategoriaResumen
        Return New CategoriaResumen With {
            .CategoriaId = LeerInt32(reader, "CategoriaId"),
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
