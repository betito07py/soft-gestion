Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Common

''' <summary>
''' Alta atómica de categoría + subcategoría + grupo en una transacción (sin SQL en la UI).
''' </summary>
Public Class ClasificacionProductoRapidaRepository
    Inherits RepositorioBase

    Public Function InsertarJerarquiaNueva(nombreCategoria As String, nombreSub As String, nombreGrupo As String, usuarioCreacion As String) As Tuple(Of Integer, Integer, Integer)
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Try
                    Dim codCat = ObtenerSiguienteCodigoCategoria(cn, tx)
                    Dim codCatStr = codCat.ToString()
                    Dim catId = InsertarCategoria(cn, tx, codCatStr, nombreCategoria, usuarioCreacion)

                    Dim codSub = ObtenerSiguienteCodigoSub(cn, tx, catId, codCatStr)
                    Dim codSubStr = codSub.ToString()
                    Dim subId = InsertarSubCategoria(cn, tx, codSubStr, nombreSub, catId, usuarioCreacion)

                    Dim codGrupo = ObtenerSiguienteCodigoGrupo(cn, tx, subId, codSubStr)
                    Dim codGrupoStr = codGrupo.ToString()
                    Dim grupoId = InsertarGrupo(cn, tx, codGrupoStr, nombreGrupo, catId, subId, usuarioCreacion)

                    tx.Commit()
                    Return Tuple.Create(catId, subId, grupoId)
                Catch
                    tx.Rollback()
                    Throw
                End Try
            End Using
        End Using
    End Function

    Private Shared Function ObtenerSiguienteCodigoCategoria(cn As SqlConnection, tx As SqlTransaction) As Integer
        Const sql As String = "SELECT ISNULL(MAX(TRY_CAST(Codigo AS INT)), 0) + 1 FROM dbo.Categorias;"
        Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text, tx)
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    Private Shared Function ObtenerSiguienteCodigoSub(cn As SqlConnection, tx As SqlTransaction, categoriaId As Integer, codigoCategoriaInsertada As String) As Integer
        Const sql As String =
            "SELECT (SELECT MAX(TRY_CAST(s.Codigo AS INT)) FROM dbo.SubCategorias s WHERE s.CategoriaId = @CategoriaId) AS MaxCod;"
        Dim maxCod As Integer? = Nothing
        Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text, tx)
            cmd.Parameters.AddWithValue("@CategoriaId", categoriaId)
            Using reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() AndAlso Not reader.IsDBNull(0) Then
                    maxCod = reader.GetInt32(0)
                End If
            End Using
        End Using
        Return ClasificacionCodigoSugeridor.SiguienteDesdeMaxYPadre(maxCod, codigoCategoriaInsertada)
    End Function

    Private Shared Function ObtenerSiguienteCodigoGrupo(cn As SqlConnection, tx As SqlTransaction, subCategoriaId As Integer, codigoSubInsertada As String) As Integer
        Const sql As String =
            "SELECT (SELECT MAX(TRY_CAST(g.Codigo AS INT)) FROM dbo.Grupos g WHERE g.SubCategoriaId = @SubCategoriaId) AS MaxCod;"
        Dim maxCod As Integer? = Nothing
        Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text, tx)
            cmd.Parameters.AddWithValue("@SubCategoriaId", subCategoriaId)
            Using reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() AndAlso Not reader.IsDBNull(0) Then
                    maxCod = reader.GetInt32(0)
                End If
            End Using
        End Using
        Return ClasificacionCodigoSugeridor.SiguienteDesdeMaxYPadre(maxCod, codigoSubInsertada)
    End Function

    Private Shared Function InsertarCategoria(cn As SqlConnection, tx As SqlTransaction, codigo As String, nombre As String, usuario As String) As Integer
        Const sql As String =
            "INSERT INTO dbo.Categorias (Codigo, Nombre, Activo, UsuarioCreacion) " &
            "OUTPUT INSERTED.CategoriaId VALUES (@Codigo, @Nombre, 1, @Usuario);"
        Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text, tx)
            cmd.Parameters.AddWithValue("@Codigo", codigo)
            cmd.Parameters.AddWithValue("@Nombre", nombre)
            cmd.Parameters.AddWithValue("@Usuario", usuario)
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    Private Shared Function InsertarSubCategoria(cn As SqlConnection, tx As SqlTransaction, codigo As String, nombre As String, categoriaId As Integer, usuario As String) As Integer
        Const sql As String =
            "INSERT INTO dbo.SubCategorias (Codigo, Nombre, CategoriaId, Activo, UsuarioCreacion) " &
            "OUTPUT INSERTED.SubCategoriaId VALUES (@Codigo, @Nombre, @CategoriaId, 1, @Usuario);"
        Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text, tx)
            cmd.Parameters.AddWithValue("@Codigo", codigo)
            cmd.Parameters.AddWithValue("@Nombre", nombre)
            cmd.Parameters.AddWithValue("@CategoriaId", categoriaId)
            cmd.Parameters.AddWithValue("@Usuario", usuario)
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function

    Private Shared Function InsertarGrupo(cn As SqlConnection, tx As SqlTransaction, codigo As String, nombre As String, categoriaId As Integer, subCategoriaId As Integer, usuario As String) As Integer
        Const sql As String =
            "INSERT INTO dbo.Grupos (Codigo, Nombre, CategoriaId, SubCategoriaId, Activo, UsuarioCreacion) " &
            "OUTPUT INSERTED.GrupoId VALUES (@Codigo, @Nombre, @CategoriaId, @SubCategoriaId, 1, @Usuario);"
        Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text, tx)
            cmd.Parameters.AddWithValue("@Codigo", codigo)
            cmd.Parameters.AddWithValue("@Nombre", nombre)
            cmd.Parameters.AddWithValue("@CategoriaId", categoriaId)
            cmd.Parameters.AddWithValue("@SubCategoriaId", subCategoriaId)
            cmd.Parameters.AddWithValue("@Usuario", usuario)
            Return CInt(cmd.ExecuteScalar())
        End Using
    End Function
End Class
