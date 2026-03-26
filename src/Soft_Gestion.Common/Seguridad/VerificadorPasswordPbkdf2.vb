Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' Verificación de contraseñas frente a <c>PasswordHash</c> almacenado (PBKDF2-SHA256).
''' Formato del hash: <c>iteraciones$Base64(salt)$Base64(subclave)</c>
''' </summary>
Public NotInheritable Class VerificadorPasswordPbkdf2
    Private Const Separador As Char = "$"c
    Public Const IteracionesPredeterminadas As Integer = 100000
    Public Const LongitudSalt As Integer = 16
    Public Const LongitudSubclave As Integer = 32

    Private Sub New()
    End Sub

    ''' <summary>
    ''' Genera un hash para persistir en <c>Usuarios.PasswordHash</c> (alta o cambio de contraseña).
    ''' </summary>
    Public Shared Function GenerarHash(passwordPlano As String) As String
        If passwordPlano Is Nothing Then Throw New ArgumentNullException(NameOf(passwordPlano))
        Dim salt(LongitudSalt - 1) As Byte
        Using rng As New RNGCryptoServiceProvider()
            rng.GetBytes(salt)
        End Using
        Dim passwordBytes As Byte() = Encoding.UTF8.GetBytes(passwordPlano)
        Dim subclave As Byte() = DerivarSubclave(passwordBytes, salt, IteracionesPredeterminadas, LongitudSubclave)
        Return String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{4}",
                             IteracionesPredeterminadas, Separador,
                             Convert.ToBase64String(salt), Separador,
                             Convert.ToBase64String(subclave))
    End Function

    ''' <summary>
    ''' Comprueba si la contraseña en texto plano coincide con el hash almacenado.
    ''' </summary>
    Public Shared Function Verificar(passwordPlano As String, hashAlmacenado As String) As Boolean
        If String.IsNullOrEmpty(hashAlmacenado) OrElse passwordPlano Is Nothing Then Return False
        Try
            Dim partes As String() = hashAlmacenado.Split(Separador)
            If partes.Length <> 3 Then Return False
            Dim iteraciones As Integer = Integer.Parse(partes(0), CultureInfo.InvariantCulture)
            If iteraciones < 1 Then Return False
            Dim salt As Byte() = Convert.FromBase64String(partes(1))
            Dim esperado As Byte() = Convert.FromBase64String(partes(2))
            Dim passwordBytes As Byte() = Encoding.UTF8.GetBytes(passwordPlano)
            Dim calculado As Byte() = DerivarSubclave(passwordBytes, salt, iteraciones, esperado.Length)
            Return ComparacionConstante(calculado, esperado)
        Catch
            Return False
        End Try
    End Function

    Private Shared Function DerivarSubclave(password As Byte(), salt As Byte(), iteraciones As Integer, longitudBytes As Integer) As Byte()
        Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, iteraciones, HashAlgorithmName.SHA256)
            Return pbkdf2.GetBytes(longitudBytes)
        End Using
    End Function

    Private Shared Function ComparacionConstante(a As Byte(), b As Byte()) As Boolean
        If a Is Nothing OrElse b Is Nothing OrElse a.Length <> b.Length Then Return False
        Dim diff As Integer = 0
        For i As Integer = 0 To a.Length - 1
            diff = diff Or (a(i) Xor b(i))
        Next
        Return diff = 0
    End Function
End Class
