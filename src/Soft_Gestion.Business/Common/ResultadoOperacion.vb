''' <summary>Resultado genérico de una operación de negocio (persistencia, reglas, etc.).</summary>
Public Class ResultadoOperacion
    Public Property Exitoso As Boolean
    Public Property Mensaje As String
    Public Property IdGenerado As Integer?

    Public Shared Function Ok(Optional idGenerado As Integer? = Nothing) As ResultadoOperacion
        Return New ResultadoOperacion With {.Exitoso = True, .Mensaje = Nothing, .IdGenerado = idGenerado}
    End Function

    Public Shared Function Fallo(mensaje As String) As ResultadoOperacion
        Return New ResultadoOperacion With {.Exitoso = False, .Mensaje = mensaje, .IdGenerado = Nothing}
    End Function
End Class
