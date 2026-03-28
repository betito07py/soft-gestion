Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Impuestos</c>.
''' </summary>
Public Class ImpuestoRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT ImpuestoId, Codigo, Nombre, TipoImpuesto, Porcentaje, EsExento, CodigoSIFEN, EsActivo, " &
        "UsuarioCreacion, FechaCreacion, UsuarioModificacion, FechaModificacion " &
        "FROM dbo.Impuestos WHERE ImpuestoId = @ImpuestoId;"

    Private Const SqlListar As String =
        "SELECT ImpuestoId, Codigo, Nombre, TipoImpuesto, Porcentaje, EsExento, EsActivo " &
        "FROM dbo.Impuestos " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR CAST(Codigo AS NVARCHAR(20)) LIKE @Patron OR Nombre LIKE @Patron OR TipoImpuesto LIKE @Patron) " &
        "AND ((@SoloActivos = 0) OR (EsActivo = 1)) " &
        "ORDER BY Codigo;"

    Private Const SqlExisteCodigoExcluir As String =
        "SELECT 1 FROM dbo.Impuestos WHERE Codigo = @Codigo AND ImpuestoId <> @ExcluirImpuestoId;"

    Private Const SqlExisteCodigoNuevo As String =
        "SELECT 1 FROM dbo.Impuestos WHERE Codigo = @Codigo;"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Impuestos (Codigo, Nombre, TipoImpuesto, Porcentaje, EsExento, CodigoSIFEN, EsActivo, UsuarioCreacion) " &
        "OUTPUT INSERTED.ImpuestoId " &
        "VALUES (@Codigo, @Nombre, @TipoImpuesto, @Porcentaje, @EsExento, @CodigoSIFEN, @EsActivo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Impuestos SET Codigo = @Codigo, Nombre = @Nombre, TipoImpuesto = @TipoImpuesto, " &
        "Porcentaje = @Porcentaje, EsExento = @EsExento, CodigoSIFEN = @CodigoSIFEN, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ImpuestoId = @ImpuestoId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Impuestos SET EsActivo = @EsActivo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ImpuestoId = @ImpuestoId;"

    Private Const SqlListarSelector As String =
        "SELECT ImpuestoId, CAST(Codigo AS NVARCHAR(20)) + N' — ' + Nombre AS NombreCombo " &
        "FROM dbo.Impuestos WHERE EsActivo = 1 ORDER BY Codigo;"

    Public Function ObtenerPorId(impuestoId As Integer) As Impuesto
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@ImpuestoId", impuestoId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearImpuesto(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String, soloActivos As Boolean) As List(Of ImpuestoResumen)
        Dim lista As New List(Of ImpuestoResumen)()
        Dim filtroTrim As String = If(filtroTexto, String.Empty).Trim()
        Dim filtroParam As Object = If(filtroTrim.Length = 0, CType(Nothing, Object), filtroTrim)
        Dim patron As Object = If(filtroTrim.Length = 0, DBNull.Value, CType("%" & filtroTrim & "%", Object))

        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListar, CommandType.Text)
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                AgregarParametro(cmd, "@SoloActivos", If(soloActivos, 1, 0))
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ListarParaSelector() As List(Of ImpuestoSelectorItem)
        Dim lista As New List(Of ImpuestoSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarSelector, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New ImpuestoSelectorItem With {
                            .ImpuestoId = LeerInt32(reader, "ImpuestoId"),
                            .Nombre = LeerString(reader, "NombreCombo")
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As Integer, Optional excluyeImpuestoId As Integer? = Nothing) As Boolean
        If codigo <= 0 Then Return False
        Dim sql = If(excluyeImpuestoId.HasValue, SqlExisteCodigoExcluir, SqlExisteCodigoNuevo)
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo)
                If excluyeImpuestoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirImpuestoId", excluyeImpuestoId.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(entidad As Impuesto) As Integer
        If entidad Is Nothing Then Throw New ArgumentNullException(NameOf(entidad))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", entidad.Codigo)
                AgregarParametro(cmd, "@Nombre", entidad.Nombre.Trim())
                AgregarParametro(cmd, "@TipoImpuesto", entidad.TipoImpuesto.Trim())
                AgregarParametro(cmd, "@Porcentaje", entidad.Porcentaje)
                AgregarParametro(cmd, "@EsExento", entidad.EsExento)
                If entidad.CodigoSIFEN.HasValue Then
                    AgregarParametro(cmd, "@CodigoSIFEN", entidad.CodigoSIFEN.Value)
                Else
                    AgregarParametro(cmd, "@CodigoSIFEN", DBNull.Value)
                End If
                AgregarParametro(cmd, "@EsActivo", entidad.EsActivo)
                AgregarParametro(cmd, "@UsuarioCreacion", entidad.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(entidad As Impuesto, usuarioModificador As String) As Integer
        If entidad Is Nothing Then Throw New ArgumentNullException(NameOf(entidad))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@ImpuestoId", entidad.ImpuestoId)
                AgregarParametro(cmd, "@Codigo", entidad.Codigo)
                AgregarParametro(cmd, "@Nombre", entidad.Nombre.Trim())
                AgregarParametro(cmd, "@TipoImpuesto", entidad.TipoImpuesto.Trim())
                AgregarParametro(cmd, "@Porcentaje", entidad.Porcentaje)
                AgregarParametro(cmd, "@EsExento", entidad.EsExento)
                If entidad.CodigoSIFEN.HasValue Then
                    AgregarParametro(cmd, "@CodigoSIFEN", entidad.CodigoSIFEN.Value)
                Else
                    AgregarParametro(cmd, "@CodigoSIFEN", DBNull.Value)
                End If
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function CambiarEstado(impuestoId As Integer, activo As Boolean, usuario As String) As Boolean
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@ImpuestoId", impuestoId)
                AgregarParametro(cmd, "@EsActivo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuario)
                Return cmd.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function

    Private Shared Function MapearImpuesto(reader As SqlDataReader) As Impuesto
        Return New Impuesto With {
            .ImpuestoId = LeerInt32(reader, "ImpuestoId"),
            .Codigo = LeerInt32(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .TipoImpuesto = LeerString(reader, "TipoImpuesto"),
            .Porcentaje = LeerDecimal(reader, "Porcentaje"),
            .EsExento = LeerBoolean(reader, "EsExento"),
            .CodigoSIFEN = LeerInt16Nullable(reader, "CodigoSIFEN"),
            .EsActivo = LeerBoolean(reader, "EsActivo"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion")
        }
    End Function

    Private Shared Function MapearResumen(reader As SqlDataReader) As ImpuestoResumen
        Return New ImpuestoResumen With {
            .ImpuestoId = LeerInt32(reader, "ImpuestoId"),
            .Codigo = LeerInt32(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .TipoImpuesto = LeerString(reader, "TipoImpuesto"),
            .Porcentaje = LeerDecimal(reader, "Porcentaje"),
            .EsExento = LeerBoolean(reader, "EsExento"),
            .EsActivo = LeerBoolean(reader, "EsActivo")
        }
    End Function
End Class
