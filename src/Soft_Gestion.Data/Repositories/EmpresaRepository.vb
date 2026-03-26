Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Empresas</c>.
''' </summary>
Public Class EmpresaRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT EmpresaId, Codigo, RazonSocial, NombreFantasia, RUC, Direccion, Telefono, Email, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Empresas WHERE EmpresaId = @EmpresaId;"

    Private Const SqlListar As String =
        "SELECT EmpresaId, Codigo, RazonSocial, NombreFantasia, RUC, Activo, " &
        "FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion " &
        "FROM dbo.Empresas " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR Codigo LIKE @Patron OR RazonSocial LIKE @Patron) " &
        "ORDER BY Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Empresas WHERE Codigo = @Codigo AND (@ExcluirEmpresaId IS NULL OR EmpresaId <> @ExcluirEmpresaId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Empresas (Codigo, RazonSocial, NombreFantasia, RUC, Direccion, Telefono, Email, Activo, UsuarioCreacion) " &
        "OUTPUT INSERTED.EmpresaId " &
        "VALUES (@Codigo, @RazonSocial, @NombreFantasia, @RUC, @Direccion, @Telefono, @Email, @Activo, @UsuarioCreacion);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Empresas SET Codigo = @Codigo, RazonSocial = @RazonSocial, NombreFantasia = @NombreFantasia, " &
        "RUC = @RUC, Direccion = @Direccion, Telefono = @Telefono, Email = @Email, " &
        "FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE EmpresaId = @EmpresaId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Empresas SET Activo = @Activo, FechaModificacion = SYSDATETIME(), UsuarioModificacion = @UsuarioModificacion " &
        "WHERE EmpresaId = @EmpresaId;"

    Private Const SqlListarActivasCombo As String =
        "SELECT EmpresaId, (Codigo + N' — ' + RazonSocial) AS TextoCombo " &
        "FROM dbo.Empresas WHERE Activo = 1 ORDER BY Codigo;"

    Private Const SqlExisteActiva As String =
        "SELECT 1 FROM dbo.Empresas WHERE EmpresaId = @EmpresaId AND Activo = 1;"

    ''' <summary>
    ''' Empresas activas para combos (ABM sucursales, filtros, etc.).
    ''' </summary>
    Public Function ListarActivasParaCombo() As List(Of EmpresaSelectorItem)
        Dim lista As New List(Of EmpresaSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivasCombo, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New EmpresaSelectorItem With {
                            .EmpresaId = LeerInt32(reader, "EmpresaId"),
                            .Texto = LeerString(reader, "TextoCombo")
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteActiva(empresaId As Integer) As Boolean
        If empresaId <= 0 Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteActiva, CommandType.Text)
                AgregarParametro(cmd, "@EmpresaId", empresaId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function ObtenerPorId(empresaId As Integer) As Empresa
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@EmpresaId", empresaId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearEmpresa(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of EmpresaResumen)
        Dim lista As New List(Of EmpresaResumen)()
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
                        lista.Add(MapearEmpresaResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As String, excluirEmpresaId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirEmpresaId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirEmpresaId", excluirEmpresaId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirEmpresaId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(empresa As Empresa) As Integer
        If empresa Is Nothing Then Throw New ArgumentNullException(NameOf(empresa))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", empresa.Codigo.Trim())
                AgregarParametro(cmd, "@RazonSocial", empresa.RazonSocial.Trim())
                AgregarParametro(cmd, "@NombreFantasia", If(String.IsNullOrWhiteSpace(empresa.NombreFantasia), CType(Nothing, Object), empresa.NombreFantasia.Trim()))
                AgregarParametro(cmd, "@RUC", If(String.IsNullOrWhiteSpace(empresa.RUC), CType(Nothing, Object), empresa.RUC.Trim()))
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(empresa.Direccion), CType(Nothing, Object), empresa.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(empresa.Telefono), CType(Nothing, Object), empresa.Telefono.Trim()))
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(empresa.Email), CType(Nothing, Object), empresa.Email.Trim()))
                AgregarParametro(cmd, "@Activo", empresa.Activo)
                AgregarParametro(cmd, "@UsuarioCreacion", empresa.UsuarioCreacion)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(empresa As Empresa, usuarioModificador As String) As Integer
        If empresa Is Nothing Then Throw New ArgumentNullException(NameOf(empresa))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@EmpresaId", empresa.EmpresaId)
                AgregarParametro(cmd, "@Codigo", empresa.Codigo.Trim())
                AgregarParametro(cmd, "@RazonSocial", empresa.RazonSocial.Trim())
                AgregarParametro(cmd, "@NombreFantasia", If(String.IsNullOrWhiteSpace(empresa.NombreFantasia), CType(Nothing, Object), empresa.NombreFantasia.Trim()))
                AgregarParametro(cmd, "@RUC", If(String.IsNullOrWhiteSpace(empresa.RUC), CType(Nothing, Object), empresa.RUC.Trim()))
                AgregarParametro(cmd, "@Direccion", If(String.IsNullOrWhiteSpace(empresa.Direccion), CType(Nothing, Object), empresa.Direccion.Trim()))
                AgregarParametro(cmd, "@Telefono", If(String.IsNullOrWhiteSpace(empresa.Telefono), CType(Nothing, Object), empresa.Telefono.Trim()))
                AgregarParametro(cmd, "@Email", If(String.IsNullOrWhiteSpace(empresa.Email), CType(Nothing, Object), empresa.Email.Trim()))
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(empresaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(empresaId, True, usuarioModificador)
    End Function

    Public Function Desactivar(empresaId As Integer, usuarioModificador As String) As Integer
        Return EstablecerActivo(empresaId, False, usuarioModificador)
    End Function

    Private Function EstablecerActivo(empresaId As Integer, activo As Boolean, usuarioModificador As String) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@EmpresaId", empresaId)
                AgregarParametro(cmd, "@Activo", activo)
                AgregarParametro(cmd, "@UsuarioModificacion", usuarioModificador)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearEmpresa(reader As SqlDataReader) As Empresa
        Return New Empresa With {
            .EmpresaId = LeerInt32(reader, "EmpresaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .RazonSocial = LeerString(reader, "RazonSocial"),
            .NombreFantasia = LeerString(reader, "NombreFantasia"),
            .RUC = LeerString(reader, "RUC"),
            .Direccion = LeerString(reader, "Direccion"),
            .Telefono = LeerString(reader, "Telefono"),
            .Email = LeerString(reader, "Email"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function

    Private Shared Function MapearEmpresaResumen(reader As SqlDataReader) As EmpresaResumen
        Return New EmpresaResumen With {
            .EmpresaId = LeerInt32(reader, "EmpresaId"),
            .Codigo = LeerString(reader, "Codigo"),
            .RazonSocial = LeerString(reader, "RazonSocial"),
            .NombreFantasia = LeerString(reader, "NombreFantasia"),
            .RUC = LeerString(reader, "RUC"),
            .Activo = LeerBoolean(reader, "Activo"),
            .FechaCreacion = LeerDateTime(reader, "FechaCreacion"),
            .UsuarioCreacion = LeerString(reader, "UsuarioCreacion"),
            .FechaModificacion = LeerDateTimeNullable(reader, "FechaModificacion"),
            .UsuarioModificacion = LeerString(reader, "UsuarioModificacion")
        }
    End Function
End Class
