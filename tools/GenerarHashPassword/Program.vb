Imports System
Imports Soft_Gestion.Common

''' <summary>
''' Utilidad de consola para generar PasswordHash para usuarios iniciales.
''' Uso: GenerarHashPassword.exe [contraseña]
'''      Si no se pasa argumento, pide la contraseña por consola.
''' </summary>
Module Program
    Sub Main(args As String())
        Dim password As String
        If args Is Nothing OrElse args.Length = 0 Then
            Console.Write("Contraseña: ")
            password = Console.ReadLine()
        Else
            password = args(0)
        End If

        If String.IsNullOrEmpty(password) Then
            Console.WriteLine("Error: debe indicar una contraseña.")
            Environment.ExitCode = 1
            Return
        End If

        Dim hash As String = VerificadorPasswordPbkdf2.GenerarHash(password)
        Console.WriteLine()
        Console.WriteLine("PasswordHash (copiar al script SQL):")
        Console.WriteLine(hash)
        Console.WriteLine()
    End Sub
End Module
