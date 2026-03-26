Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.UnidadesMedida</c>.
''' </summary>
Public Class UnidadMedidaRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT m.UnidadMedidaId, m.Codigo, m.Nombre, m.Abreviatura, m.Activo, m.Codigo_SIFEN, " &
        "s.Descrip_Unidad AS Descrip_Unidad_SIFEN, s.Codigo_Repr AS Codigo_Repr_SIFEN " &
        "FROM dbo.UnidadesMedida m " &
        "LEFT JOIN dbo.UnidadesMedidaSIFEN s ON s.Codigo = m.Codigo_SIFEN " &
        "WHERE m.UnidadMedidaId = @UnidadMedidaId;"

    Private Const SqlListar As String =
        "SELECT m.UnidadMedidaId, m.Codigo, m.Nombre, m.Abreviatura, m.Activo, m.Codigo_SIFEN, " &
        "s.Descrip_Unidad AS Descrip_Unidad_SIFEN, s.Codigo_Repr AS Codigo_Repr_SIFEN " &
        "FROM dbo.UnidadesMedida m " &
        "LEFT JOIN dbo.UnidadesMedidaSIFEN s ON s.Codigo = m.Codigo_SIFEN " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR m.Codigo LIKE @Patron OR m.Nombre LIKE @Patron OR m.Abreviatura LIKE @Patron) " &
        "ORDER BY m.Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.UnidadesMedida WHERE Codigo = @Codigo " &
        "AND (@ExcluirUnidadMedidaId IS NULL OR UnidadMedidaId <> @ExcluirUnidadMedidaId);"

    Private Const SqlExisteAbreviatura As String =
        "SELECT 1 FROM dbo.UnidadesMedida WHERE Abreviatura = @Abreviatura " &
        "AND (@ExcluirUnidadMedidaId IS NULL OR UnidadMedidaId <> @ExcluirUnidadMedidaId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.UnidadesMedida (Codigo, Nombre, Abreviatura, Activo, Codigo_SIFEN) " &
        "OUTPUT INSERTED.UnidadMedidaId " &
        "VALUES (@Codigo, @Nombre, @Abreviatura, @Activo, @Codigo_SIFEN);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.UnidadesMedida SET Codigo = @Codigo, Nombre = @Nombre, Abreviatura = @Abreviatura, Codigo_SIFEN = @Codigo_SIFEN " &
        "WHERE UnidadMedidaId = @UnidadMedidaId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.UnidadesMedida SET Activo = @Activo WHERE UnidadMedidaId = @UnidadMedidaId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT m.UnidadMedidaId, m.Codigo, m.Nombre, m.Abreviatura, m.Codigo_SIFEN, " &
        "s.Descrip_Unidad AS Descrip_Unidad_SIFEN, s.Codigo_Repr AS Codigo_Repr_SIFEN " &
        "FROM dbo.UnidadesMedida m " &
        "LEFT JOIN dbo.UnidadesMedidaSIFEN s ON s.Codigo = m.Codigo_SIFEN " &
        "WHERE m.Activo = 1 ORDER BY m.Codigo;"

    Public Function ListarActivasParaCombo() As List(Of UnidadMedidaResumen)
        Dim lista As New List(Of UnidadMedidaResumen)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New UnidadMedidaResumen With {
                            .UnidadMedidaId = LeerInt32(reader, "UnidadMedidaId"),
                            .Codigo = LeerString(reader, "Codigo"),
                            .Nombre = LeerString(reader, "Nombre"),
                            .Abreviatura = LeerString(reader, "Abreviatura"),
                            .Codigo_SIFEN = LeerInt16Nullable(reader, "Codigo_SIFEN"),
                            .Descrip_Unidad_SIFEN = LeerString(reader, "Descrip_Unidad_SIFEN"),
                            .Codigo_Repr_SIFEN = LeerString(reader, "Codigo_Repr_SIFEN")
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(unidadMedidaId As Integer) As UnidadMedida
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@UnidadMedidaId", unidadMedidaId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearUnidadMedida(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of UnidadMedidaResumen)
        Dim lista As New List(Of UnidadMedidaResumen)()
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
                        lista.Add(MapearResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As String, excluirUnidadMedidaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirUnidadMedidaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirUnidadMedidaId", excluirUnidadMedidaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirUnidadMedidaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ExisteAbreviatura(abreviatura As String, excluirUnidadMedidaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(abreviatura) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteAbreviatura, CommandType.Text)
                AgregarParametro(cmd, "@Abreviatura", abreviatura.Trim())
                If excluirUnidadMedidaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirUnidadMedidaId", excluirUnidadMedidaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirUnidadMedidaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(um As UnidadMedida) As Integer
        If um Is Nothing Then Throw New ArgumentNullException(NameOf(um))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", um.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", um.Nombre.Trim())
                AgregarParametro(cmd, "@Abreviatura", um.Abreviatura.Trim())
                AgregarParametro(cmd, "@Activo", um.Activo)
                If um.Codigo_SIFEN.HasValue Then
                    AgregarParametro(cmd, "@Codigo_SIFEN", um.Codigo_SIFEN.Value)
                Else
                    AgregarParametro(cmd, "@Codigo_SIFEN", DBNull.Value)
                End If
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(um As UnidadMedida) As Integer
        If um Is Nothing Then Throw New ArgumentNullException(NameOf(um))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@UnidadMedidaId", um.UnidadMedidaId)
                AgregarParametro(cmd, "@Codigo", um.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", um.Nombre.Trim())
                AgregarParametro(cmd, "@Abreviatura", um.Abreviatura.Trim())
                If um.Codigo_SIFEN.HasValue Then
                    AgregarParametro(cmd, "@Codigo_SIFEN", um.Codigo_SIFEN.Value)
                Else
                    AgregarParametro(cmd, "@Codigo_SIFEN", DBNull.Value)
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(unidadMedidaId As Integer) As Integer
        Return EstablecerActivo(unidadMedidaId, True)
    End Function

    Public Function Desactivar(unidadMedidaId As Integer) As Integer
        Return EstablecerActivo(unidadMedidaId, False)
    End Function

    Private Function EstablecerActivo(unidadMedidaId As Integer, activo As Boolean) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@UnidadMedidaId", unidadMedidaId)
                AgregarParametro(cmd, "@Activo", activo)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearUnidadMedida(reader As SqlDataReader) As UnidadMedida
        Return New UnidadMedida With {
            .UnidadMedidaId = LeerInt32(reader, "UnidadMedidaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Abreviatura = LeerString(reader, "Abreviatura"),
            .Activo = LeerBoolean(reader, "Activo"),
            .Codigo_SIFEN = LeerInt16Nullable(reader, "Codigo_SIFEN"),
            .Descrip_Unidad_SIFEN = LeerString(reader, "Descrip_Unidad_SIFEN"),
            .Codigo_Repr_SIFEN = LeerString(reader, "Codigo_Repr_SIFEN")
        }
    End Function

    Private Shared Function MapearResumen(reader As SqlDataReader) As UnidadMedidaResumen
        Return New UnidadMedidaResumen With {
            .UnidadMedidaId = LeerInt32(reader, "UnidadMedidaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Abreviatura = LeerString(reader, "Abreviatura"),
            .Activo = LeerBoolean(reader, "Activo"),
            .Codigo_SIFEN = LeerInt16Nullable(reader, "Codigo_SIFEN"),
            .Descrip_Unidad_SIFEN = LeerString(reader, "Descrip_Unidad_SIFEN"),
            .Codigo_Repr_SIFEN = LeerString(reader, "Codigo_Repr_SIFEN")
        }
    End Function
End Class
