Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Permisos</c>. La asignación a roles (<c>RolPermisos</c>) se implementará aparte.
''' </summary>
Public Class PermisoRepository
    Inherits RepositorioBase

    Private Const SqlObtenerPorId As String =
        "SELECT PermisoId, Codigo, Nombre, Modulo, Formulario, Accion, Descripcion, Activo " &
        "FROM dbo.Permisos WHERE PermisoId = @PermisoId;"

    Private Const SqlListar As String =
        "SELECT PermisoId, Codigo, Nombre, Modulo, Formulario, Accion, Descripcion, Activo " &
        "FROM dbo.Permisos " &
        "WHERE (@Filtro IS NULL OR @Filtro = N'' OR Codigo LIKE @Patron OR Nombre LIKE @Patron OR Modulo LIKE @Patron) " &
        "ORDER BY Modulo, Codigo;"

    Private Const SqlListarActivos As String =
        "SELECT PermisoId, Codigo, Nombre, Modulo, Formulario, Accion, Descripcion, Activo " &
        "FROM dbo.Permisos WHERE Activo = 1 ORDER BY Modulo, Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.Permisos WHERE Codigo = @Codigo AND (@ExcluirPermisoId IS NULL OR PermisoId <> @ExcluirPermisoId);"

    Private Const SqlInsertar As String =
        "INSERT INTO dbo.Permisos (Codigo, Nombre, Modulo, Formulario, Accion, Descripcion, Activo) " &
        "OUTPUT INSERTED.PermisoId " &
        "VALUES (@Codigo, @Nombre, @Modulo, @Formulario, @Accion, @Descripcion, @Activo);"

    Private Const SqlActualizar As String =
        "UPDATE dbo.Permisos SET Codigo = @Codigo, Nombre = @Nombre, Modulo = @Modulo, " &
        "Formulario = @Formulario, Accion = @Accion, Descripcion = @Descripcion " &
        "WHERE PermisoId = @PermisoId;"

    Private Const SqlEstablecerActivo As String =
        "UPDATE dbo.Permisos SET Activo = @Activo WHERE PermisoId = @PermisoId;"

    Public Function ObtenerPorId(permisoId As Integer) As Permiso
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@PermisoId", permisoId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Return MapearPermiso(reader)
                End Using
            End Using
        End Using
    End Function

    Public Function Listar(filtroTexto As String) As List(Of PermisoResumen)
        Dim lista As New List(Of PermisoResumen)()
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
                        lista.Add(MapearPermisoResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    ''' <summary>
    ''' Catálogo de permisos activos (asignación a roles, menús, etc.).
    ''' </summary>
    Public Function ListarActivos() As List(Of PermisoResumen)
        Dim lista As New List(Of PermisoResumen)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivos, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(MapearPermisoResumen(reader))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteActivo(permisoId As Integer) As Boolean
        Dim p = ObtenerPorId(permisoId)
        Return p IsNot Nothing AndAlso p.Activo
    End Function

    Public Function ExisteCodigo(codigo As String, excluirPermisoId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codigo) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo.Trim())
                If excluirPermisoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirPermisoId", excluirPermisoId.Value)
                Else
                    AgregarParametro(cmd, "@ExcluirPermisoId", DBNull.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    Public Function Insertar(permiso As Permiso) As Integer
        If permiso Is Nothing Then Throw New ArgumentNullException(NameOf(permiso))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlInsertar, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", permiso.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", permiso.Nombre.Trim())
                AgregarParametro(cmd, "@Modulo", permiso.Modulo.Trim())
                AgregarParametro(cmd, "@Formulario", If(String.IsNullOrWhiteSpace(permiso.Formulario), CType(Nothing, Object), permiso.Formulario.Trim()))
                AgregarParametro(cmd, "@Accion", If(String.IsNullOrWhiteSpace(permiso.Accion), CType(Nothing, Object), permiso.Accion.Trim()))
                AgregarParametro(cmd, "@Descripcion", If(String.IsNullOrWhiteSpace(permiso.Descripcion), CType(Nothing, Object), permiso.Descripcion.Trim()))
                AgregarParametro(cmd, "@Activo", permiso.Activo)
                Dim generado = cmd.ExecuteScalar()
                Return CInt(generado)
            End Using
        End Using
    End Function

    Public Function Actualizar(permiso As Permiso) As Integer
        If permiso Is Nothing Then Throw New ArgumentNullException(NameOf(permiso))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlActualizar, CommandType.Text)
                AgregarParametro(cmd, "@PermisoId", permiso.PermisoId)
                AgregarParametro(cmd, "@Codigo", permiso.Codigo.Trim())
                AgregarParametro(cmd, "@Nombre", permiso.Nombre.Trim())
                AgregarParametro(cmd, "@Modulo", permiso.Modulo.Trim())
                AgregarParametro(cmd, "@Formulario", If(String.IsNullOrWhiteSpace(permiso.Formulario), CType(Nothing, Object), permiso.Formulario.Trim()))
                AgregarParametro(cmd, "@Accion", If(String.IsNullOrWhiteSpace(permiso.Accion), CType(Nothing, Object), permiso.Accion.Trim()))
                AgregarParametro(cmd, "@Descripcion", If(String.IsNullOrWhiteSpace(permiso.Descripcion), CType(Nothing, Object), permiso.Descripcion.Trim()))
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Public Function Activar(permisoId As Integer) As Integer
        Return EstablecerActivo(permisoId, True)
    End Function

    Public Function Desactivar(permisoId As Integer) As Integer
        Return EstablecerActivo(permisoId, False)
    End Function

    Private Function EstablecerActivo(permisoId As Integer, activo As Boolean) As Integer
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlEstablecerActivo, CommandType.Text)
                AgregarParametro(cmd, "@PermisoId", permisoId)
                AgregarParametro(cmd, "@Activo", activo)
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    Private Shared Function MapearPermiso(reader As SqlDataReader) As Permiso
        Return New Permiso With {
            .PermisoId = LeerInt32(reader, "PermisoId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Modulo = LeerString(reader, "Modulo"),
            .Formulario = LeerString(reader, "Formulario"),
            .Accion = LeerString(reader, "Accion"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .Activo = LeerBoolean(reader, "Activo")
        }
    End Function

    Private Shared Function MapearPermisoResumen(reader As SqlDataReader) As PermisoResumen
        Return New PermisoResumen With {
            .PermisoId = LeerInt32(reader, "PermisoId"),
            .Codigo = LeerString(reader, "Codigo"),
            .Nombre = LeerString(reader, "Nombre"),
            .Modulo = LeerString(reader, "Modulo"),
            .Formulario = LeerString(reader, "Formulario"),
            .Accion = LeerString(reader, "Accion"),
            .Descripcion = LeerString(reader, "Descripcion"),
            .Activo = LeerBoolean(reader, "Activo")
        }
    End Function
End Class
