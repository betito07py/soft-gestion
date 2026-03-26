Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports Soft_Gestion.Common

''' <summary>
''' Base para repositorios ADO.NET. Proporciona creación de conexión y utilidades mínimas para comandos y parámetros.
''' </summary>
Public MustInherit Class RepositorioBase

    ''' <summary>
    ''' Si es Nothing o vacío, se usa la cadena principal. Las subclases pueden sobreescribir para otra entrada de configuración.
    ''' </summary>
    Protected Overridable ReadOnly Property NombreCadenaConexion As String
        Get
            Return Nothing
        End Get
    End Property

    ''' <summary>
    ''' Abre una nueva <see cref="SqlConnection"/>. El llamador debe usar Using o cerrar la conexión.
    ''' </summary>
    Protected Function CrearConexion() As SqlConnection
        If String.IsNullOrWhiteSpace(NombreCadenaConexion) Then
            Return ConexionSqlServer.Crear()
        End If
        Return ConexionSqlServer.Crear(NombreCadenaConexion)
    End Function

    ''' <summary>
    ''' Crea un comando asociado a la conexión y, opcionalmente, a una transacción.
    ''' </summary>
    Protected Shared Function CrearComando(conexion As SqlConnection,
                                          textoComando As String,
                                          tipo As CommandType,
                                          Optional transaccion As SqlTransaction = Nothing) As SqlCommand
        If conexion Is Nothing Then Throw New ArgumentNullException(NameOf(conexion))
        Dim cmd As New SqlCommand(textoComando, conexion, transaccion) With {
            .CommandType = tipo
        }
        Return cmd
    End Function

    ''' <summary>
    ''' Agrega un parámetro tipado. Si <paramref name="valor"/> es Nothing, se envía <see cref="DBNull.Value"/>.
    ''' </summary>
    Protected Shared Sub AgregarParametro(cmd As SqlCommand, nombre As String, tipo As SqlDbType, valor As Object)
        If cmd Is Nothing Then Throw New ArgumentNullException(NameOf(cmd))
        Dim p As New SqlParameter(nombre, tipo) With {
            .Value = If(valor Is Nothing, DBNull.Value, valor)
        }
        cmd.Parameters.Add(p)
    End Sub

    ''' <summary>
    ''' Agrega un parámetro con tipo inferido por el proveedor (útil para NVARCHAR sin tamaño fijo en el cliente).
    ''' </summary>
    Protected Shared Sub AgregarParametro(cmd As SqlCommand, nombre As String, valor As Object)
        If cmd Is Nothing Then Throw New ArgumentNullException(NameOf(cmd))
        cmd.Parameters.AddWithValue(nombre, If(valor Is Nothing, DBNull.Value, valor))
    End Sub

    Protected Shared Function LeerString(reader As SqlDataReader, columna As String) As String
        Dim i As Integer = reader.GetOrdinal(columna)
        Return If(reader.IsDBNull(i), Nothing, reader.GetString(i))
    End Function

    Protected Shared Function LeerInt32(reader As SqlDataReader, columna As String) As Integer
        Dim i As Integer = reader.GetOrdinal(columna)
        Return reader.GetInt32(i)
    End Function

    Protected Shared Function LeerInt32Nullable(reader As SqlDataReader, columna As String) As Integer?
        Dim i As Integer = reader.GetOrdinal(columna)
        If reader.IsDBNull(i) Then Return Nothing
        Return reader.GetInt32(i)
    End Function

    Protected Shared Function LeerInt16Nullable(reader As SqlDataReader, columna As String) As Short?
        Dim i As Integer = reader.GetOrdinal(columna)
        If reader.IsDBNull(i) Then Return Nothing
        Return reader.GetInt16(i)
    End Function

    ''' <summary>
    ''' Como <see cref="LeerInt16Nullable"/> pero tolera INT/BIGINT en el servidor (evita fallos de mapeo al listar).
    ''' </summary>
    Protected Shared Function LeerInt16NullableFlexible(reader As SqlDataReader, columna As String) As Short?
        Dim i As Integer = reader.GetOrdinal(columna)
        If reader.IsDBNull(i) Then Return Nothing
        Dim o = reader.GetValue(i)
        If TypeOf o Is Short Then Return DirectCast(o, Short)
        If TypeOf o Is Integer Then Return CShort(DirectCast(o, Integer))
        If TypeOf o Is Long Then Return CShort(DirectCast(o, Long))
        Return Convert.ToInt16(o, CultureInfo.InvariantCulture)
    End Function

    Protected Shared Function LeerBoolean(reader As SqlDataReader, columna As String) As Boolean
        Dim i As Integer = reader.GetOrdinal(columna)
        Return reader.GetBoolean(i)
    End Function

    Protected Shared Function LeerDateTime(reader As SqlDataReader, columna As String) As DateTime
        Dim i As Integer = reader.GetOrdinal(columna)
        Return reader.GetDateTime(i)
    End Function

    Protected Shared Function LeerDateTimeNullable(reader As SqlDataReader, columna As String) As DateTime?
        Dim i As Integer = reader.GetOrdinal(columna)
        If reader.IsDBNull(i) Then Return Nothing
        Return reader.GetDateTime(i)
    End Function

    Protected Shared Function LeerDecimal(reader As SqlDataReader, columna As String) As Decimal
        Dim i As Integer = reader.GetOrdinal(columna)
        Return reader.GetDecimal(i)
    End Function
End Class
