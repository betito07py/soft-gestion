Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Proveedores</c>.
''' </summary>
Public Class ProveedorRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT ProveedorId, Codigo, RazonSocial, RUC, Direccion, Telefono, Email, " &
        "CondicionPagoId, Observaciones, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Proveedores WHERE ProveedorId = @ProveedorId;"

    Private Const SqlListar As String =
        "SELECT p.ProveedorId, p.Codigo, p.RazonSocial, p.RUC, " &
        "cp.Codigo AS CondicionPagoCodigo, p.Activo, " &
        "p.FechaCreacion, p.UsuarioCreacion, p.FechaModificacion, p.UsuarioModificacion " &
        "FROM dbo.Proveedores p " &
        "LEFT JOIN dbo.CondicionesPago cp ON cp.CondicionPagoId = p.CondicionPagoId " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR p.Codigo LIKE @Patron OR p.RazonSocial LIKE @Patron " &
        "OR ISNULL(p.RUC, N'') LIKE @Patron) " &
        "ORDER BY p.Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Proveedores WHERE Codigo = @Codigo " &
        "AND (@ExcluirProveedorId IS NULL OR ProveedorId <> @ExcluirProveedorId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Proveedores (Codigo, RazonSocial, RUC, Direccion, Telefono, Email, " &
        "CondicionPagoId, Observaciones, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.ProveedorId " &
        "VALUES (@Codigo, @RazonSocial, @RUC, @Direccion, @Telefono, @Email, " &
        "@CondicionPagoId, @Observaciones, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Proveedores SET Codigo = @Codigo, RazonSocial = @RazonSocial, RUC = @RUC, " &
        "Direccion = @Direccion, Telefono = @Telefono, Email = @Email, " &
        "CondicionPagoId = @CondicionPagoId, Observaciones = @Observaciones, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ProveedorId = @ProveedorId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Proveedores SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ProveedorId = @ProveedorId;"

    Public Function ObtenerPorId(proveedorId As Integer) As Proveedor
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@ProveedorId", proveedorId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearProveedor(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of ProveedorResumen)
        Dim lista As New List(Of ProveedorResumen)()
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
                        lista.Add(MapearProveedorResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As String, excluirProveedorId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirProveedorId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirProveedorId", excluirProveedorId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirProveedorId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(proveedor As Proveedor) As Integer
        If proveedor Is Nothing Then Throw New ArgumentNullException(NameOf(proveedor))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", proveedor.Codigo.Trim())
                AgregarParametro(cmd, "@RazonSocial", proveedor.RazonSocial.Trim())
                AgregarParametro(cmd, "@RUC", If(String.IsNullOrWhiteSpace(proveedor.RUC), CType(Nothing, Object), proveedor.RUC.Trim()))
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(proveedor.Direccion), CType(Nothing, Object), proveedor.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(proveedor.Telefono), CType(Nothing, Object), proveedor.Telefono.Trim()))
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(proveedor.Email), CType(Nothing, Object), proveedor.Email.Trim()))
                If proveedor.CondicionPagoId.HasValue Then
                    AgregarParametro(cmd, "@CondicionPagoId", proveedor.CondicionPagoId.Value)
                Else
                    AgregarParametro(cmd, "@CondicionPagoId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@Observaciones", If(String.IsNullOrWhiteSpace(proveedor.Observaciones), CType(Nothing, Object), proveedor.Observaciones.Trim()))
                AgregarParametro(cmd, "@Activo", proveedor.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", proveedor.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(proveedor As Proveedor, usuarioModificador As String) As Integer
        If proveedor Is Nothing Then Throw New ArgumentNullException(NameOf(proveedor))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@ProveedorId", proveedor.ProveedorId)
                AgregarParametro(cmd, "@Codigo", proveedor.Codigo.Trim())
                AgregarParametro(cmd, "@RazonSocial", proveedor.RazonSocial.Trim())
                AgregarParametro(cmd, "@RUC", If(String.IsNullOrWhiteSpace(proveedor.RUC), CType(Nothing, Object), proveedor.RUC.Trim()))
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(proveedor.Direccion), CType(Nothing, Object), proveedor.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(proveedor.Telefono), CType(Nothing, Object), proveedor.Telefono.Trim()))
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(proveedor.Email), CType(Nothing, Object), proveedor.Email.Trim()))
                If proveedor.CondicionPagoId.HasValue Then
                    AgregarParametro(cmd, "@CondicionPagoId", proveedor.CondicionPagoId.Value)
                Else
                    AgregarParametro(cmd, "@CondicionPagoId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@Observaciones", If(String.IsNullOrWhiteSpace(proveedor.Observaciones), CType(Nothing, Object), proveedor.Observaciones.Trim()))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(proveedorId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(proveedorId, True, usuarioModificador)
    End Function

    Public Function Desactivar(proveedorId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(proveedorId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(proveedorId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@ProveedorId", proveedorId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearProveedor(reader As SqlDataReader) As Proveedor
        Return New Proveedor With {
            .ProveedorId = LeerInt32(reader, "ProveedorId"),
            .Codigo = LeerString(reader, "Codigo"),
            .RazonSocial = LeerString(reader, "RazonSocial"),
            .RUC = LeerString(reader, "RUC"),
            .Direccion = LeerString(reader, "Direccion"),
            .Telefono = LeerString(reader, "Telefono"),
            .Email = LeerString(reader, "Email"),
            .CondicionPagoId = LeerInt32Nullable(reader, "CondicionPagoId"),
            .Observaciones = LeerString(reader, "Observaciones"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearProveedorResumen(reader As SqlDataReader) As ProveedorResumen
        Return New ProveedorResumen With {
            .ProveedorId = LeerInt32(reader, "ProveedorId"),
            .Codigo = LeerString(reader, "Codigo"),
            .RazonSocial = LeerString(reader, "RazonSocial"),
            .RUC = LeerString(reader, "RUC"),
            .CondicionPagoCodigo = LeerString(reader, "CondicionPagoCodigo"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
