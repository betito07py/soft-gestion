Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.ListasPrecios</c>.
''' </summary>
Public Class ListaPrecioRepository
    Inherits RepositorioBase

    Private Const SqlListarActivas As String =
        "SELECT ListaPrecioId, Codigo, Nombre FROM dbo.ListasPrecios WHERE Activo = 1 ORDER BY Codigo;"

    Private Const SqlObtenerPorId As String =
        "SELECT ListaPrecioId, Codigo, Nombre, Activo FROM dbo.ListasPrecios WHERE ListaPrecioId = @ListaPrecioId;"

    Private Const SqlExisteActiva As String =
        "SELECT 1 FROM dbo.ListasPrecios WHERE ListaPrecioId = @ListaPrecioId AND Activo = 1;"

    Public Function ListarActivasParaCombo() As List(Of ListaPrecioSelectorItem)
        Dim lista As New List(Of ListaPrecioSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivas, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim id = LeerInt32(reader, "ListaPrecioId")
                        Dim cod = LeerString(reader, "Codigo")
                        Dim nom = LeerString(reader, "Nombre")
                        lista.Add(New ListaPrecioSelectorItem With {
                            .ListaPrecioId = id,
                            .Texto = cod & " — " & nom
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(listaPrecioId As Integer) As ListaPrecioSelectorItem
        If listaPrecioId <= 0 Then Return Nothing
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@ListaPrecioId", listaPrecioId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Dim id = LeerInt32(reader, "ListaPrecioId")
                    Dim cod = LeerString(reader, "Codigo")
                    Dim nom = LeerString(reader, "Nombre")
                    Return New ListaPrecioSelectorItem With {
                        .ListaPrecioId = id,
                        .Texto = cod & " — " & nom
                    }
                End Using
            End Using
        End Using
    End Function

    Public Function ExisteActiva(listaPrecioId As Integer) As Boolean
        If listaPrecioId <= 0 Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteActiva, CommandType.Text)
                AgregarParametro(cmd, "@ListaPrecioId", listaPrecioId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function
End Class
