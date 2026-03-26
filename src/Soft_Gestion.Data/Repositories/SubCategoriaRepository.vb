Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Common
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.SubCategorias</c>.
''' </summary>
Public Class SubCategoriaRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT SubCategoriaId, Codigo, Nombre, CategoriaId, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.SubCategorias WHERE SubCategoriaId = @SubCategoriaId;"

    Private Const SqlListar As String =
        "SELECT s.SubCategoriaId, s.Codigo, s.Nombre, s.CategoriaId, c.Codigo AS CategoriaCodigo, s.Activo, " &
        "s.FechaCreacion, s.UsuarioCreacion, s.FechaModificacion, s.UsuarioModificacion " &
        "FROM dbo.SubCategorias s " &
        "INNER JOIN dbo.Categorias c ON c.CategoriaId = s.CategoriaId " &
        "WHERE (@CategoriaFiltro IS NULL OR s.CategoriaId = @CategoriaFiltro) " &
        "AND (@Filtro IS NULL OR @Filtro = N'' OR s.Codigo LIKE @Patron OR s.Nombre LIKE @Patron) " &
        "ORDER BY c.Codigo, s.Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.SubCategorias WHERE CategoriaId = @CategoriaId AND Codigo = @Codigo " &
        "AND (@ExcluirSubCategoriaId IS NULL OR SubCategoriaId <> @ExcluirSubCategoriaId);"

    Private Const SqlExisteNombre As String =
        "SELECT 1 FROM dbo.SubCategorias WHERE CategoriaId = @CategoriaId AND Nombre = @Nombre " &
        "AND (@ExcluirSubCategoriaId IS NULL OR SubCategoriaId <> @ExcluirSubCategoriaId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.SubCategorias (Codigo, Nombre, CategoriaId, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.SubCategoriaId " &
        "VALUES (@Codigo, @Nombre, @CategoriaId, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.SubCategorias SET Codigo = @Codigo, Nombre = @Nombre, CategoriaId = @CategoriaId, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE SubCategoriaId = @SubCategoriaId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.SubCategorias SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE SubCategoriaId = @SubCategoriaId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT SubCategoriaId, Codigo, Nombre FROM dbo.SubCategorias WHERE Activo = 1 " &
        "AND (@CategoriaId IS NULL OR CategoriaId = @CategoriaId) " &
        "ORDER BY Codigo;"

    Private Const SqlExisteActiva As String =
        "SELECT 1 FROM dbo.SubCategorias WHERE SubCategoriaId = @SubCategoriaId AND Activo = 1;"

    Private Const SqlPerteneceACategoria As String =
        "SELECT 1 FROM dbo.SubCategorias WHERE SubCategoriaId = @SubCategoriaId AND CategoriaId = @CategoriaId;"

    Private Const SqlDatosParaSugerenciaSub As String =
        "SELECT " &
        "(SELECT MAX(TRY_CAST(s.Codigo AS INT)) FROM dbo.SubCategorias s WHERE s.CategoriaId = @CategoriaId) AS MaxCod, " &
        "(SELECT c.Codigo FROM dbo.Categorias c WHERE c.CategoriaId = @CategoriaId) AS ParentCod;"

    ''' <summary>
    ''' Indica si la subcategoría existe, está activa y puede usarse en nuevos grupos (validación de negocio).
    ''' </summary>
    ''' <summary>
    ''' Siguiente código sugerido para una subcategoría nueva bajo la categoría indicada.
    ''' </summary>
    Public Function ObtenerSiguienteCodigoNumericoSugerido(categoriaId As Integer) As Integer
        If categoriaId <= 0 Then Return 1
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlDatosParaSugerenciaSub, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If Not reader.Read() Then Return 1
                    Dim maxCod = LeerInt32Nullable(reader, "MaxCod")
                    Dim parentCod = LeerString(reader, "ParentCod")
                    Return ClasificacionCodigoSugeridor.SiguienteDesdeMaxYPadre(maxCod, parentCod)
                End Using
            End Using
        End Using
    End Function

    Public Function ExisteActiva(subCategoriaId As Integer) As Boolean
        If subCategoriaId <= 0 Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteActiva, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Comprueba que exista una fila en <c>SubCategorias</c> con el par
    ''' <c>(SubCategoriaId, CategoriaId)</c>. Misma regla que la FK compuesta hacia grupos en SQL;
    ''' el servicio de grupos llama a esto para fallar con mensaje claro y no solo por constraint.
    ''' </summary>
    Public Function PerteneceACategoria(subCategoriaId As Integer, categoriaId As Integer) As Boolean
        If subCategoriaId <= 0 OrElse categoriaId <= 0 Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlPerteneceACategoria, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ListarActivasParaCombo(categoriaId As Integer?) As List(Of SubCategoriaSelectorItem)
        Dim lista As New List(Of SubCategoriaSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                If categoriaId.HasValue Then
                    AgregarParametro(cmd, "@CategoriaId", categoriaId.Value)
                Else
                    AgregarParametro(cmd, "@CategoriaId", DBNull.Value)
                End If
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim id = LeerInt32(reader, "SubCategoriaId")
                        Dim cod = LeerString(reader, "Codigo")
                        Dim nom = LeerString(reader, "Nombre")
                        lista.Add(New SubCategoriaSelectorItem With {
                            .SubCategoriaId = id,
                            .Texto = cod & " — " & nom
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(subCategoriaId As Integer) As SubCategoria
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearSubCategoria(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String, categoriaId As Integer?) As List(Of SubCategoriaResumen)
        Dim lista As New List(Of SubCategoriaResumen)()
        Dim filtroTrim As String = If(filtroTexto, String.Empty).Trim()
        Dim filtroParam As Object = If(filtroTrim.Length = 0, CType(Nothing, Object), filtroTrim)
        Dim patron As Object = If(filtroTrim.Length = 0, DBNull.Value, CType("%" & filtroTrim & "%", Object))

        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListar, CommandType.Text)
                If categoriaId.HasValue Then
                    AgregarParametro(cmd, "@CategoriaFiltro", categoriaId.Value)
                Else
                    AgregarParametro(cmd, "@CategoriaFiltro", DBNull.Value)
                End If
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearSubCategoriaResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(categoriaId As Integer, codigo As String, excluirSubCategoriaId As Integer?) As Boolean
        If categoriaId <= 0 OrElse String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirSubCategoriaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirSubCategoriaId", excluirSubCategoriaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirSubCategoriaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ExisteNombre(categoriaId As Integer, nombre As String, excluirSubCategoriaId As Integer?) As Boolean
        If categoriaId <= 0 OrElse String.IsNullOrWhiteSpace(nombre) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteNombre, CommandType.Text)
                AgregarParametro(cmd, "@CategoriaId", categoriaId)
                AgregarParametro(cmd, "@Nombre", nombre.Trim())
                If excluirSubCategoriaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirSubCategoriaId", excluirSubCategoriaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirSubCategoriaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(subCategoria As SubCategoria) As Integer
        If subCategoria Is Nothing Then Throw New ArgumentNullException(NameOf(subCategoria))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", subCategoria.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", subCategoria.Nombre.Trim())
                AgregarParametro(cmd, "@CategoriaId", subCategoria.CategoriaId)
                AgregarParametro(cmd, "@Activo", subCategoria.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", subCategoria.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(subCategoria As SubCategoria, usuarioModificador As String) As Integer
        If subCategoria Is Nothing Then Throw New ArgumentNullException(NameOf(subCategoria))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoria.SubCategoriaId)
                AgregarParametro(cmd, "@Codigo", subCategoria.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", subCategoria.Nombre.Trim())
                AgregarParametro(cmd, "@CategoriaId", subCategoria.CategoriaId)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(subCategoriaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(subCategoriaId, True, usuarioModificador)
    End Function

    Public Function Desactivar(subCategoriaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(subCategoriaId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(subCategoriaId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearSubCategoria(reader As SqlDataReader) As SubCategoria
        Return New SubCategoria With {
            .SubCategoriaId = LeerInt32(reader, "SubCategoriaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .CategoriaId = LeerInt32(reader, "CategoriaId"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearSubCategoriaResumen(reader As SqlDataReader) As SubCategoriaResumen
        Return New SubCategoriaResumen With {
            .SubCategoriaId = LeerInt32(reader, "SubCategoriaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .CategoriaId = LeerInt32(reader, "CategoriaId"),
            .CategoriaCodigo = LeerString(reader, "CategoriaCodigo"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
