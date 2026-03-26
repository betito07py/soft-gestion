''' <summary>
''' Normalización de códigos de maestros: sin espacios laterales y sin ceros a la izquierda.
''' Si solo había ceros, queda <c>0</c>. Si al quitar ceros el primer carácter no sería letra, dígito ni <c>(</c>, se conserva el texto solo recortado (evita p. ej. <c>.5</c>).
''' </summary>
Public NotInheritable Class CodigoNegocio

    Private Sub New()
    End Sub

    Public Shared Function NormalizarSinCerosIzquierda(codigo As String) As String
        If codigo Is Nothing Then Return Nothing
        Dim t = codigo.Trim()
        If t.Length = 0 Then Return String.Empty

        Dim cut = 0
        While cut < t.Length AndAlso t(cut) = "0"c
            cut += 1
        End While

        If cut = t.Length Then Return "0"

        Dim r = t.Substring(cut)
        If cut > 0 AndAlso Not Char.IsLetterOrDigit(r(0)) AndAlso r(0) <> "("c Then
            Return t
        End If

        Return r
    End Function
End Class
