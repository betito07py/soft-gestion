Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Clientes</c>.
''' </summary>
Public Class ClienteRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT ClienteId, Codigo, RazonSocial, NombreFantasia, Documento, RUC, Direccion, Telefono, Email, " &
        "CondicionPagoId, ListaPrecioId, LimiteCredito, Observaciones, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Clientes WHERE ClienteId = @ClienteId;"

    Private Const SqlListar As String =
        "SELECT c.ClienteId, c.Codigo, c.RazonSocial, c.Documento, c.RUC, " &
        "cp.Codigo AS CondicionPagoCodigo, lp.Codigo AS ListaPrecioCodigo, " &
        "c.LimiteCredito, c.Activo, c.FechaCreacion, c.UsuarioCreacion, c.FechaModificacion, c.UsuarioModificacion " &
        "FROM dbo.Clientes c " &
        "LEFT JOIN dbo.CondicionesPago cp ON cp.CondicionPagoId = c.CondicionPagoId " &
        "LEFT JOIN dbo.ListasPrecios lp ON lp.ListaPrecioId = c.ListaPrecioId " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR c.Codigo LIKE @Patron OR c.RazonSocial LIKE @Patron " &
        "OR ISNULL(c.Documento, N'') LIKE @Patron OR ISNULL(c.RUC, N'') LIKE @Patron) " &
        "ORDER BY c.Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Clientes WHERE Codigo = @Codigo " &
        "AND (@ExcluirClienteId IS NULL OR ClienteId <> @ExcluirClienteId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Clientes (Codigo, RazonSocial, NombreFantasia, Documento, RUC, Direccion, Telefono, Email, " &
        "CondicionPagoId, ListaPrecioId, LimiteCredito, Observaciones, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.ClienteId " &
        "VALUES (@Codigo, @RazonSocial, @NombreFantasia, @Documento, @RUC, @Direccion, @Telefono, @Email, " &
        "@CondicionPagoId, @ListaPrecioId, @LimiteCredito, @Observaciones, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Clientes SET Codigo = @Codigo, RazonSocial = @RazonSocial, NombreFantasia = @NombreFantasia, " &
        "Documento = @Documento, RUC = @RUC, Direccion = @Direccion, Telefono = @Telefono, Email = @Email, " &
        "CondicionPagoId = @CondicionPagoId, ListaPrecioId = @ListaPrecioId, LimiteCredito = @LimiteCredito, " &
        "Observaciones = @Observaciones, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ClienteId = @ClienteId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Clientes SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE ClienteId = @ClienteId;"

    Public Function ObtenerPorId(clienteId As Integer) As Cliente
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@ClienteId", clienteId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearCliente(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of ClienteResumen)
        Dim lista As New List(Of ClienteResumen)()
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
                        lista.Add(MapearClienteResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As String, excluirClienteId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirClienteId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirClienteId", excluirClienteId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirClienteId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(cliente As Cliente) As Integer
        If cliente Is Nothing Then Throw New ArgumentNullException(NameOf(cliente))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", cliente.Codigo.Trim())
                AgregarParametro(cmd, "@RazonSocial", cliente.RazonSocial.Trim())
                AgregarParametro(cmd, "@NombreFantasia", If(String.IsNullOrWhiteSpace(cliente.NombreFantasia), CType(Nothing, Object), cliente.NombreFantasia.Trim()))
                AgregarParametro(cmd, "@Documento", If(String.IsNullOrWhiteSpace(cliente.Documento), CType(Nothing, Object), cliente.Documento.Trim()))
                AgregarParametro(cmd, "@RUC", If(String.IsNullOrWhiteSpace(cliente.RUC), CType(Nothing, Object), cliente.RUC.Trim()))
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(cliente.Direccion), CType(Nothing, Object), cliente.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(cliente.Telefono), CType(Nothing, Object), cliente.Telefono.Trim()))
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(cliente.Email), CType(Nothing, Object), cliente.Email.Trim()))
                If cliente.CondicionPagoId.HasValue Then
                    AgregarParametro(cmd, "@CondicionPagoId", cliente.CondicionPagoId.Value)
                Else
                    AgregarParametro(cmd, "@CondicionPagoId", DBNull.Value)
                End If
                If cliente.ListaPrecioId.HasValue Then
                    AgregarParametro(cmd, "@ListaPrecioId", cliente.ListaPrecioId.Value)
                Else
                    AgregarParametro(cmd, "@ListaPrecioId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@LimiteCredito", cliente.LimiteCredito)
                AgregarParametro(cmd, "@Observaciones", If(String.IsNullOrWhiteSpace(cliente.Observaciones), CType(Nothing, Object), cliente.Observaciones.Trim()))
                AgregarParametro(cmd, "@Activo", cliente.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", cliente.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(cliente As Cliente, usuarioModificador As String) As Integer
        If cliente Is Nothing Then Throw New ArgumentNullException(NameOf(cliente))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@ClienteId", cliente.ClienteId)
                AgregarParametro(cmd, "@Codigo", cliente.Codigo.Trim())
                AgregarParametro(cmd, "@RazonSocial", cliente.RazonSocial.Trim())
                AgregarParametro(cmd, "@NombreFantasia", If(String.IsNullOrWhiteSpace(cliente.NombreFantasia), CType(Nothing, Object), cliente.NombreFantasia.Trim()))
                AgregarParametro(cmd, "@Documento", If(String.IsNullOrWhiteSpace(cliente.Documento), CType(Nothing, Object), cliente.Documento.Trim()))
                AgregarParametro(cmd, "@RUC", If(String.IsNullOrWhiteSpace(cliente.RUC), CType(Nothing, Object), cliente.RUC.Trim()))
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(cliente.Direccion), CType(Nothing, Object), cliente.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(cliente.Telefono), CType(Nothing, Object), cliente.Telefono.Trim()))
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(cliente.Email), CType(Nothing, Object), cliente.Email.Trim()))
                If cliente.CondicionPagoId.HasValue Then
                    AgregarParametro(cmd, "@CondicionPagoId", cliente.CondicionPagoId.Value)
                Else
                    AgregarParametro(cmd, "@CondicionPagoId", DBNull.Value)
                End If
                If cliente.ListaPrecioId.HasValue Then
                    AgregarParametro(cmd, "@ListaPrecioId", cliente.ListaPrecioId.Value)
                Else
                    AgregarParametro(cmd, "@ListaPrecioId", DBNull.Value)
                End If
                AgregarParametro(cmd, "@LimiteCredito", cliente.LimiteCredito)
                AgregarParametro(cmd, "@Observaciones", If(String.IsNullOrWhiteSpace(cliente.Observaciones), CType(Nothing, Object), cliente.Observaciones.Trim()))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(clienteId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(clienteId, True, usuarioModificador)
    End Function

    Public Function Desactivar(clienteId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(clienteId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(clienteId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@ClienteId", clienteId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearCliente(reader As SqlDataReader) As Cliente
        Return New Cliente With {
            .ClienteId = LeerInt32(reader, "ClienteId"),
            .Codigo = LeerString(reader, "Codigo"),
            .RazonSocial = LeerString(reader, "RazonSocial"),
            .NombreFantasia = LeerString(reader, "NombreFantasia"),
            .Documento = LeerString(reader, "Documento"),
            .RUC = LeerString(reader, "RUC"),
            .Direccion = LeerString(reader, "Direccion"),
            .Telefono = LeerString(reader, "Telefono"),
            .Email = LeerString(reader, "Email"),
            .CondicionPagoId = LeerInt32Nullable(reader, "CondicionPagoId"),
            .ListaPrecioId = LeerInt32Nullable(reader, "ListaPrecioId"),
            .LimiteCredito = LeerDecimal(reader, "LimiteCredito"),
            .Observaciones = LeerString(reader, "Observaciones"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearClienteResumen(reader As SqlDataReader) As ClienteResumen
        Return New ClienteResumen With {
            .ClienteId = LeerInt32(reader, "ClienteId"),
            .Codigo = LeerString(reader, "Codigo"),
            .RazonSocial = LeerString(reader, "RazonSocial"),
            .Documento = LeerString(reader, "Documento"),
            .RUC = LeerString(reader, "RUC"),
            .CondicionPagoCodigo = LeerString(reader, "CondicionPagoCodigo"),
            .ListaPrecioCodigo = LeerString(reader, "ListaPrecioCodigo"),
            .LimiteCredito = LeerDecimal(reader, "LimiteCredito"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
