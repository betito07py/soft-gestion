Imports System.Data.SqlClient

''' <summary>
''' Fábrica de <see cref="SqlConnection"/>. La cadena proviene de <see cref="ConfiguracionConexion"/> o se indica explícitamente.
''' </summary>
Public NotInheritable Class ConexionSqlServer
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Crea una conexión con la cadena principal (<see cref="ConfiguracionConexion.NombreConexionPrincipal"/>).
    ''' </summary>
    Public Shared Function Crear() As SqlConnection
        Return New SqlConnection(ConfiguracionConexion.ObtenerCadenaPrincipal())
    End Function

    ''' <summary>
    ''' Crea una conexión usando un nombre de entrada en connectionStrings del archivo de configuración.
    ''' </summary>
    Public Shared Function Crear(nombreCadenaConexion As String) As SqlConnection
        If String.IsNullOrWhiteSpace(nombreCadenaConexion) Then
            Return Crear()
        End If
        Return New SqlConnection(ConfiguracionConexion.ObtenerCadena(nombreCadenaConexion))
    End Function

    ''' <summary>
    ''' Crea una conexión con una cadena explícita (pruebas o escenarios especiales).
    ''' </summary>
    Public Shared Function CrearConCadena(cadenaConexion As String) As SqlConnection
        If String.IsNullOrWhiteSpace(cadenaConexion) Then
            Throw New ArgumentException("La cadena de conexión no puede estar vacía.", NameOf(cadenaConexion))
        End If
        Return New SqlConnection(cadenaConexion)
    End Function

    ''' <summary>
    ''' Abre una conexión, ejecuta la acción y la cierra al terminar (Using + Open).
    ''' </summary>
    Public Shared Sub EjecutarConConexionAbierta(accion As Action(Of SqlConnection))
        If accion Is Nothing Then Throw New ArgumentNullException(NameOf(accion))
        Using cn As SqlConnection = Crear()
            cn.Open()
            accion(cn)
        End Using
    End Sub

    ''' <summary>
    ''' Igual que <see cref="EjecutarConConexionAbierta"/> usando una entrada nombrada de connectionStrings.
    ''' </summary>
    Public Shared Sub EjecutarConConexionAbierta(nombreCadenaConexion As String, accion As Action(Of SqlConnection))
        If accion Is Nothing Then Throw New ArgumentNullException(NameOf(accion))
        Using cn As SqlConnection = Crear(nombreCadenaConexion)
            cn.Open()
            accion(cn)
        End Using
    End Sub
End Class
