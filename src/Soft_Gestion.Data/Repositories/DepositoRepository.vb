Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Depositos</c>.
''' </summary>
Public Class DepositoRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT DepositoId, SucursalId, Codigo, Nombre, Descripcion, EsPrincipal, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Depositos WHERE DepositoId = @DepositoId;"

    Private Const SqlListar As String =
        "SELECT d.DepositoId, d.SucursalId, s.EmpresaId, e.Codigo AS EmpresaCodigo, s.Codigo AS SucursalCodigo, " &
        "d.Codigo, d.Nombre, d.Descripcion, d.EsPrincipal, d.Activo, " &
        "d.FechaCreacion, d.UsuarioCreacion, d.FechaModificacion, d.UsuarioModificacion " &
        "FROM dbo.Depositos d " &
        "INNER JOIN dbo.Sucursales s ON s.SucursalId = d.SucursalId " &
        "INNER JOIN dbo.Empresas e ON e.EmpresaId = s.EmpresaId " &
        "WHERE (@EmpresaId IS NULL OR s.EmpresaId = @EmpresaId) " &
        "AND (@SucursalId IS NULL OR d.SucursalId = @SucursalId) " &
        "AND (@Filtro IS NULL OR @Filtro = N'' OR d.Codigo LIKE @Patron OR d.Nombre LIKE @Patron OR ISNULL(d.Descripcion, N'') LIKE @Patron) " &
        "ORDER BY e.Codigo, s.Codigo, d.Codigo;"

    Private Const SqlExisteCodigoEnSucursal As String =
        "SELECT 1 FROM dbo.Depositos WHERE SucursalId = @SucursalId AND Codigo = @Codigo " &
        "AND (@ExcluirDepositoId IS NULL OR DepositoId <> @ExcluirDepositoId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Depositos (SucursalId, Codigo, Nombre, Descripcion, EsPrincipal, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.DepositoId " &
        "VALUES (@SucursalId, @Codigo, @Nombre, @Descripcion, @EsPrincipal, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Depositos SET SucursalId = @SucursalId, Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, " &
        "EsPrincipal = @EsPrincipal, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE DepositoId = @DepositoId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Depositos SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE DepositoId = @DepositoId;"

    Public Function ObtenerPorId(depositoId As Integer) As Deposito
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@DepositoId", depositoId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearDeposito(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String, empresaId As Integer?, sucursalId As Integer?) As List(Of DepositoResumen)
        Dim lista As New List(Of DepositoResumen)()
        Dim filtroTrim As String = If(filtroTexto, String.Empty).Trim()
        Dim filtroParam As Object = If(filtroTrim.Length = 0, CType(Nothing, Object), filtroTrim)
        Dim patron As Object = If(filtroTrim.Length = 0, DBNull.Value, CType("%" & filtroTrim & "%", Object))

        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListar, CommandType.Text)
                If empresaId.HasValue AndAlso empresaId.Value > 0 Then
                    AgregarParametro(cmd, "@EmpresaId", empresaId.Value)
                Else
                    AgregarParametro(cmd, "@EmpresaId", DBNull.Value)
                End If
                If sucursalId.HasValue AndAlso sucursalId.Value > 0 Then
                    AgregarParametro(cmd, "@SucursalId", sucursalId.Value)
                Else
                    AgregarParametro(cmd, "@SucursalId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearDepositoResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(sucursalId As Integer, codigo As String, excluirDepositoId As Integer?) As Boolean
        If sucursalId <= 0 OrElse String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigoEnSucursal, CommandType.Text)
                AgregarParametro(cmd, "@SucursalId", sucursalId)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirDepositoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirDepositoId", excluirDepositoId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirDepositoId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(deposito As Deposito) As Integer
        If deposito Is Nothing Then Throw New ArgumentNullException(NameOf(deposito))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@SucursalId", deposito.SucursalId)
                AgregarParametro(cmd, "@Codigo", deposito.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", deposito.Nombre.Trim())
                AgregarParametro(cmd, "@Descripcion", If(String.IsNullOrWhiteSpace(deposito.Descripcion), CType(Nothing, Object), deposito.Descripcion.Trim()))
                AgregarParametro(cmd, "@EsPrincipal", deposito.EsPrincipal)
                AgregarParametro(cmd, "@Activo", deposito.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", deposito.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(deposito As Deposito, usuarioModificador As String) As Integer
        If deposito Is Nothing Then Throw New ArgumentNullException(NameOf(deposito))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@DepositoId", deposito.DepositoId)
                AgregarParametro(cmd, "@SucursalId", deposito.SucursalId)
                AgregarParametro(cmd, "@Codigo", deposito.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", deposito.Nombre.Trim())
                AgregarParametro(cmd, "@Descripcion", If(String.IsNullOrWhiteSpace(deposito.Descripcion), CType(Nothing, Object), deposito.Descripcion.Trim()))
                AgregarParametro(cmd, "@EsPrincipal", deposito.EsPrincipal)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(depositoId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(depositoId, True, usuarioModificador)
    End Function

    Public Function Desactivar(depositoId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(depositoId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(depositoId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@DepositoId", depositoId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearDeposito(reader As SqlDataReader) As Deposito
        Return New Deposito With {
            .DepositoId = LeerInt32(reader, "DepositoId"),
            .SucursalId = LeerInt32(reader, "SucursalId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .EsPrincipal = LeerBoolean(reader, "EsPrincipal"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearDepositoResumen(reader As SqlDataReader) As DepositoResumen
        Return New DepositoResumen With {
            .DepositoId = LeerInt32(reader, "DepositoId"),
            .SucursalId = LeerInt32(reader, "SucursalId"),
            .EmpresaId = LeerInt32(reader, "EmpresaId"),
            .EmpresaCodigo = LeerString(reader, "EmpresaCodigo"),
            .SucursalCodigo = LeerString(reader, "SucursalCodigo"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .EsPrincipal = LeerBoolean(reader, "EsPrincipal"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
