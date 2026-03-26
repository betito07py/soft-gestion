Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Sucursales</c>.
''' </summary>
Public Class SucursalRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT SucursalId, EmpresaId, Codigo, Nombre, Direccion, Telefono, Responsable, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Sucursales WHERE SucursalId = @SucursalId;"

    Private Const SqlListar As String =
        "SELECT s.SucursalId, s.EmpresaId, e.Codigo AS EmpresaCodigo, s.Codigo, s.Nombre, s.Responsable, s.Activo, " &
        "s.FechaCreacion, s.UsuarioCreacion, s.FechaModificacion, s.UsuarioModificacion " &
        "FROM dbo.Sucursales s " &
        "INNER JOIN dbo.Empresas e ON e.EmpresaId = s.EmpresaId " &
        "WHERE (@EmpresaId IS NULL OR s.EmpresaId = @EmpresaId) " &
        "AND (@Filtro IS NULL OR @Filtro = N'' OR s.Codigo LIKE @Patron OR s.Nombre LIKE @Patron OR ISNULL(s.Responsable, N'') LIKE @Patron) " &
        "ORDER BY e.Codigo, s.Codigo;"

    Private Const SqlExisteCodigoEnEmpresa As String =
        "SELECT 1 FROM dbo.Sucursales WHERE EmpresaId = @EmpresaId AND Codigo = @Codigo " &
        "AND (@ExcluirSucursalId IS NULL OR SucursalId <> @ExcluirSucursalId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Sucursales (EmpresaId, Codigo, Nombre, Direccion, Telefono, Responsable, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.SucursalId " &
        "VALUES (@EmpresaId, @Codigo, @Nombre, @Direccion, @Telefono, @Responsable, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Sucursales SET EmpresaId = @EmpresaId, Codigo = @Codigo, Nombre = @Nombre, " &
        "Direccion = @Direccion, Telefono = @Telefono, Responsable = @Responsable, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE SucursalId = @SucursalId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Sucursales SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE SucursalId = @SucursalId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT s.SucursalId, (s.Codigo + N' — ' + s.Nombre) AS TextoCombo " &
        "FROM dbo.Sucursales s " &
        "WHERE s.Activo = 1 " &
        "AND (@EmpresaId IS NULL OR s.EmpresaId = @EmpresaId) " &
        "ORDER BY s.Codigo;"

    Private Const SqlExisteSucursalActiva As String =
        "SELECT 1 FROM dbo.Sucursales WHERE SucursalId = @Id AND Activo = 1;"

    Public Function ObtenerPorId(sucursalId As Integer) As Sucursal
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@SucursalId", sucursalId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearSucursal(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String, empresaId As Integer?) As List(Of SucursalResumen)
        Dim lista As New List(Of SucursalResumen)()
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
                AgregarParametro(cmd, "@Filtro", filtroParam)
                AgregarParametro(cmd, "@Patron", patron)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearSucursalResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(empresaId As Integer, codigo As String, excluirSucursalId As Integer?) As Boolean
        If empresaId <= 0 OrElse String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigoEnEmpresa, CommandType.Text)
                AgregarParametro(cmd, "@EmpresaId", empresaId)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirSucursalId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirSucursalId", excluirSucursalId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirSucursalId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(sucursal As Sucursal) As Integer
        If sucursal Is Nothing Then Throw New ArgumentNullException(NameOf(sucursal))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@EmpresaId", sucursal.EmpresaId)
                AgregarParametro(cmd, "@Codigo", sucursal.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", sucursal.Nombre.Trim())
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(sucursal.Direccion), CType(Nothing, Object), sucursal.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(sucursal.Telefono), CType(Nothing, Object), sucursal.Telefono.Trim()))
                AgregarParametro(cmd, "@Responsable", If(String.IsNullOrWhiteSpace(sucursal.Responsable), CType(Nothing, Object), sucursal.Responsable.Trim()))
                AgregarParametro(cmd, "@Activo", sucursal.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", sucursal.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(sucursal As Sucursal, usuarioModificador As String) As Integer
        If sucursal Is Nothing Then Throw New ArgumentNullException(NameOf(sucursal))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@SucursalId", sucursal.SucursalId)
                AgregarParametro(cmd, "@EmpresaId", sucursal.EmpresaId)
                AgregarParametro(cmd, "@Codigo", sucursal.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", sucursal.Nombre.Trim())
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(sucursal.Direccion), CType(Nothing, Object), sucursal.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(sucursal.Telefono), CType(Nothing, Object), sucursal.Telefono.Trim()))
                AgregarParametro(cmd, "@Responsable", If(String.IsNullOrWhiteSpace(sucursal.Responsable), CType(Nothing, Object), sucursal.Responsable.Trim()))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(sucursalId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(sucursalId, True, usuarioModificador)
    End Function

    Public Function Desactivar(sucursalId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(sucursalId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(sucursalId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@SucursalId", sucursalId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Sucursales activas para combos (p. ej. usuarios, depósitos).
    ''' Si <paramref name="empresaId"/> tiene valor, limita a esa empresa.
    ''' </summary>
    Public Function ListarActivasParaCombo(Optional empresaId As Integer? = Nothing) As List(Of SucursalSelectorItem)
        Dim lista As New List(Of SucursalSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                If empresaId.HasValue AndAlso empresaId.Value > 0 Then
                    AgregarParametro(cmd, "@EmpresaId", empresaId.Value)
                Else
                    AgregarParametro(cmd, "@EmpresaId", DBNull.Value)
                End If
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New SucursalSelectorItem With {
                            .SucursalId = LeerInt32(reader, "SucursalId"),
                            .Texto = LeerString(reader, "TextoCombo")
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteActiva(sucursalId As Integer) As Boolean
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteSucursalActiva, CommandType.Text)
                AgregarParametro(cmd, "@Id", sucursalId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Private Shared Function MapearSucursal(reader As SqlDataReader) As Sucursal
        Return New Sucursal With {
            .SucursalId = LeerInt32(reader, "SucursalId"),
            .EmpresaId = LeerInt32(reader, "EmpresaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Direccion = LeerString(reader, "Direccion"),
            .Telefono = LeerString(reader, "Telefono"),
            .Responsable = LeerString(reader, "Responsable"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearSucursalResumen(reader As SqlDataReader) As SucursalResumen
        Return New SucursalResumen With {
            .SucursalId = LeerInt32(reader, "SucursalId"),
            .EmpresaId = LeerInt32(reader, "EmpresaId"),
            .EmpresaCodigo = LeerString(reader, "EmpresaCodigo"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Responsable = LeerString(reader, "Responsable"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
