''' <summary>
''' Calcula el siguiente código numérico sugerido para subcategorías y grupos:
''' máximo entre hermanos + 1, o primer hijo codificado como <c>codigoPadre × 100 + 1</c>.
''' </summary>
Public NotInheritable Class ClasificacionCodigoSugeridor

    Private Sub New()
    End Sub

    ''' <param name="maximoEntreHermanos">Máximo <see cref="Integer"/> ya parseado en SQL, o Nothing si no hay filas numéricas.</param>
    ''' <param name="codigoPadre">Código del padre (categoría o subcategoría) tal como está en BD.</param>
    Public Shared Function SiguienteDesdeMaxYPadre(maximoEntreHermanos As Integer?, codigoPadre As String) As Integer
        If maximoEntreHermanos.HasValue Then Return maximoEntreHermanos.Value + 1
        If String.IsNullOrWhiteSpace(codigoPadre) Then Return 1
        Dim norm = CodigoNegocio.NormalizarSinCerosIzquierda(codigoPadre)
        Dim p As Integer
        If Integer.TryParse(norm, p) AndAlso p > 0 Then Return p * 100 + 1
        Return 1
    End Function
End Class
