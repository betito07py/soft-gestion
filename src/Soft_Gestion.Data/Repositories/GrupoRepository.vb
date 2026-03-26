Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Common
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Grupos</c>.
''' </summary>
Public Class GrupoRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT GrupoId, Codigo, Nombre, CategoriaId, SubCategoriaId, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Grupos WHERE GrupoId = @GrupoId;"

    Private Const SqlListar As String =
        "SELECT g.GrupoId, g.Codigo, g.Nombre, g.CategoriaId, g.SubCategoriaId, " &
        "c.Codigo AS CategoriaCodigo, s.Codigo AS SubCategoriaCodigo, g.Activo, " &
        "g.FechaCreacion, g.UsuarioCreacion, g.FechaModificacion, g.UsuarioModificacion " &
        "FROM dbo.Grupos g " &
        "INNER JOIN dbo.Categorias c ON c.CategoriaId = g.CategoriaId " &
        "INNER JOIN dbo.SubCategorias s ON s.SubCategoriaId = g.SubCategoriaId " &
        "WHERE (@CategoriaFiltro IS NULL OR g.CategoriaId = @CategoriaFiltro) " &
        "AND (@SubCategoriaFiltro IS NULL OR g.SubCategoriaId = @SubCategoriaFiltro) " &
        "AND (@Filtro IS NULL OR @Filtro = N'' OR g.Codigo LIKE @Patron OR g.Nombre LIKE @Patron) " &
        "ORDER BY c.Codigo, s.Codigo, g.Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Grupos WHERE SubCategoriaId = @SubCategoriaId AND Codigo = @Codigo " &
        "AND (@ExcluirGrupoId IS NULL OR GrupoId <> @ExcluirGrupoId);"

    Private Const SqlExisteNombre As String =
        "SELECT 1 FROM dbo.Grupos WHERE SubCategoriaId = @SubCategoriaId AND Nombre = @Nombre " &
        "AND (@ExcluirGrupoId IS NULL OR GrupoId <> @ExcluirGrupoId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Grupos (Codigo, Nombre, CategoriaId, SubCategoriaId, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.GrupoId " &
        "VALUES (@Codigo, @Nombre, @CategoriaId, @SubCategoriaId, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Grupos SET Codigo = @Codigo, Nombre = @Nombre, CategoriaId = @CategoriaId, SubCategoriaId = @SubCategoriaId, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE GrupoId = @GrupoId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Grupos SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE GrupoId = @GrupoId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT GrupoId, Codigo, Nombre FROM dbo.Grupos WHERE Activo = 1 " &
        "AND (@SubCategoriaId IS NULL OR SubCategoriaId = @SubCategoriaId) " &
        "ORDER BY Codigo;"

    Private Const SqlDatosParaSugerenciaGrupo As String =
        "SELECT " &
        "(SELECT MAX(TRY_CAST(g.Codigo AS INT)) FROM dbo.Grupos g WHERE g.SubCategoriaId = @SubCategoriaId) AS MaxCod, " &
        "(SELECT s.Codigo FROM dbo.SubCategorias s WHERE s.SubCategoriaId = @SubCategoriaId) AS ParentCod;"

    ''' <summary>
    ''' Siguiente código sugerido para un grupo nuevo bajo la subcategoría indicada.
    ''' </summary>
    Public Function ObtenerSiguienteCodigoNumericoSugerido(subCategoriaId As Integer) As Integer
        If subCategoriaId <= 0 Then Return 1
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlDatosParaSugerenciaGrupo, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If Not reader.Read() Then Return 1
                    Dim maxCod = LeerInt32Nullable(reader, "MaxCod")
                    Dim parentCod = LeerString(reader, "ParentCod")
                    Return ClasificacionCodigoSugeridor.SiguienteDesdeMaxYPadre(maxCod, parentCod)
                End Using
            End Using
        End Using
    End Function

    Public Function ListarActivasParaCombo(subCategoriaId As Integer?) As List(Of GrupoSelectorItem)
        Dim lista As New List(Of GrupoSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                If subCategoriaId.HasValue Then
                    AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId.Value)
                Else
                    AgregarParametro(cmd, "@SubCategoriaId", DBNull.Value)
                End If
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim id = LeerInt32(reader, "GrupoId")
                        Dim cod = LeerString(reader, "Codigo")
                        Dim nom = LeerString(reader, "Nombre")
                        lista.Add(New GrupoSelectorItem With {
                            .GrupoId = id,
                            .Texto = cod & " — " & nom
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(grupoId As Integer) As Grupo
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@GrupoId", grupoId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearGrupo(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String, categoriaId As Integer?, subCategoriaId As Integer?) As List(Of GrupoResumen)
        Dim lista As New List(Of GrupoResumen)()
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
                If subCategoriaId.HasValue Then
                    AgregarParametro(cmd, "@SubCategoriaFiltro", subCategoriaId.Value)
                Else
                    AgregarParametro(cmd, "@SubCategoriaFiltro", DBNull.Value)
                End If
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearGrupoResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(subCategoriaId As Integer, codigo As String, excluirGrupoId As Integer?) As Boolean
        If subCategoriaId <= 0 OrElse String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirGrupoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirGrupoId", excluirGrupoId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirGrupoId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ExisteNombre(subCategoriaId As Integer, nombre As String, excluirGrupoId As Integer?) As Boolean
        If subCategoriaId <= 0 OrElse String.IsNullOrWhiteSpace(nombre) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteNombre, CommandType.Text)
                AgregarParametro(cmd, "@SubCategoriaId", subCategoriaId)
                AgregarParametro(cmd, "@Nombre", nombre.Trim())
                If excluirGrupoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirGrupoId", excluirGrupoId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirGrupoId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(grupo As Grupo) As Integer
        If grupo Is Nothing Then Throw New ArgumentNullException(NameOf(grupo))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", grupo.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", grupo.Nombre.Trim())
                AgregarParametro(cmd, "@CategoriaId", grupo.CategoriaId)
                AgregarParametro(cmd, "@SubCategoriaId", grupo.SubCategoriaId)
                AgregarParametro(cmd, "@Activo", grupo.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", grupo.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(grupo As Grupo, usuarioModificador As String) As Integer
        If grupo Is Nothing Then Throw New ArgumentNullException(NameOf(grupo))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@GrupoId", grupo.GrupoId)
                AgregarParametro(cmd, "@Codigo", grupo.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", grupo.Nombre.Trim())
                AgregarParametro(cmd, "@CategoriaId", grupo.CategoriaId)
                AgregarParametro(cmd, "@SubCategoriaId", grupo.SubCategoriaId)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(grupoId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(grupoId, True, usuarioModificador)
    End Function

    Public Function Desactivar(grupoId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(grupoId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(grupoId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@GrupoId", grupoId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearGrupo(reader As SqlDataReader) As Grupo
        Return New Grupo With {
            .GrupoId = LeerInt32(reader, "GrupoId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .CategoriaId = LeerInt32(reader, "CategoriaId"),
            .SubCategoriaId = LeerInt32(reader, "SubCategoriaId"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearGrupoResumen(reader As SqlDataReader) As GrupoResumen
        Return New GrupoResumen With {
            .GrupoId = LeerInt32(reader, "GrupoId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .CategoriaId = LeerInt32(reader, "CategoriaId"),
            .SubCategoriaId = LeerInt32(reader, "SubCategoriaId"),
            .CategoriaCodigo = LeerString(reader, "CategoriaCodigo"),
            .SubCategoriaCodigo = LeerString(reader, "SubCategoriaCodigo"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
