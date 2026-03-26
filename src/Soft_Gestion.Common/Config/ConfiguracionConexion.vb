Imports System.Configuration

''' <summary>
''' Lee cadenas de conexión desde el archivo de configuración de la aplicación (App.config del ejecutable).
''' </summary>
Public NotInheritable Class ConfiguracionConexion
    Private Sub New()
    End Sub

    ''' <summary>
    ''' Nombre de la cadena de conexión principal en connectionStrings.
    ''' </summary>
    Public Const NombreConexionPrincipal As String = "SoftGestionPrincipal"

    ''' <summary>
    ''' Nombre opcional para servidor de configuración (si se usa en el futuro).
    ''' </summary>
    Public Const NombreConexionConfigServer As String = "ConfigServer"

    ''' <summary>
    ''' Obtiene la cadena de conexión para la base principal.
    ''' </summary>
    Public Shared Function ObtenerCadenaPrincipal() As String
        Return ObtenerCadena(NombreConexionPrincipal)
    End Function

    ''' <summary>
    ''' Obtiene una cadena de conexión por nombre desde connectionStrings.
    ''' </summary>
    Public Shared Function ObtenerCadena(nombre As String) As String
        If String.IsNullOrWhiteSpace(nombre) Then
            Throw New ArgumentException("El nombre de la cadena de conexión es obligatorio.", NameOf(nombre))
        End If
        Dim conn = ConfigurationManager.ConnectionStrings(nombre.Trim())
        If conn Is Nothing Then
            Throw New ConfigurationErrorsException($"No existe connectionString con name=""{nombre}"" en el archivo de configuración.")
        End If
        Return conn.ConnectionString
    End Function
End Class
