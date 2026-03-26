''' <summary>
''' Datos para registrar en <c>AuditoriaAcceso</c> tras un intento de autenticación.
''' </summary>
Public Class DatosAuditoriaAccesoIntento
    Public Property UsuarioId As Integer?
    Public Property LoginIngresado As String
    Public Property FechaHora As DateTime
    Public Property Equipo As String
    Public Property IpLocal As String
    Public Property Exitoso As Boolean
    Public Property Observacion As String
End Class
