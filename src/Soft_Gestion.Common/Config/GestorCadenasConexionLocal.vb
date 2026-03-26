Imports System.Configuration
Imports System.Data.SqlClient

''' <summary>
''' Lee y persiste cadenas de conexión en el archivo de configuración del ejecutable (equivalente práctico a gestión tipo Aux_Pgs sobre App.config / exe.config).
''' </summary>
Public NotInheritable Class GestorCadenasConexionLocal
    Private Const ProveedorSqlClient As String = "System.Data.SqlClient"

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Abre el exe.config en disco y devuelve la cadena actual (puede diferir del caché de <see cref="ConfigurationManager"/> hasta un Refresh).
    ''' </summary>
    Public Shared Function LeerCadenaDesdeArchivoLocal(nombreEntrada As String) As String
        If String.IsNullOrWhiteSpace(nombreEntrada) Then Throw New ArgumentException("El nombre de la cadena es obligatorio.", NameOf(nombreEntrada))
        Dim config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim cs = config.ConnectionStrings.ConnectionStrings(nombreEntrada.Trim())
        If cs Is Nothing Then
            Throw New ConfigurationErrorsException("No existe la entrada """ & nombreEntrada & """ en connectionStrings del archivo de configuración.")
        End If
        Return cs.ConnectionString
    End Function

    ''' <summary>
    ''' Guarda la cadena en el exe.config y refresca la sección en memoria.
    ''' </summary>
    Public Shared Sub GuardarCadenaEnArchivoLocal(nombreEntrada As String, connectionString As String)
        If String.IsNullOrWhiteSpace(nombreEntrada) Then Throw New ArgumentException("El nombre de la cadena es obligatorio.", NameOf(nombreEntrada))
        If connectionString Is Nothing Then Throw New ArgumentNullException(NameOf(connectionString))

        Dim config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim seccion = config.ConnectionStrings
        If seccion Is Nothing Then Throw New ConfigurationErrorsException("La sección connectionStrings no está disponible en la configuración.")

        Dim existente = seccion.ConnectionStrings(nombreEntrada.Trim())
        If existente Is Nothing Then
            seccion.ConnectionStrings.Add(New ConnectionStringSettings(nombreEntrada.Trim(), connectionString.Trim(), ProveedorSqlClient))
        Else
            seccion.ConnectionStrings(nombreEntrada.Trim()).ConnectionString = connectionString.Trim()
        End If

        config.Save(ConfigurationSaveMode.Modified)
        ConfigurationManager.RefreshSection("connectionStrings")
    End Sub

    ''' <summary>
    ''' Intenta abrir y cerrar la conexión. Devuelve Nothing si tuvo éxito; en caso contrario el mensaje de error.
    ''' </summary>
    Public Shared Function ProbarConexion(connectionString As String) As String
        If String.IsNullOrWhiteSpace(connectionString) Then Return "La cadena de conexión está vacía."
        Try
            Using cn As New SqlConnection(connectionString.Trim())
                cn.Open()
            End Using
            Return Nothing
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Class
