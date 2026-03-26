Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.CondicionesPago</c>.
''' </summary>
Public Class CondicionPagoRepository
    Inherits RepositorioBase

    Private Const SqlListarActivas As String =
        "SELECT CondicionPagoId, Codigo, Nombre FROM dbo.CondicionesPago WHERE Activo = 1 ORDER BY Codigo;"

    Private Const SqlObtenerPorId As String =
        "SELECT CondicionPagoId, Codigo, Nombre, Activo FROM dbo.CondicionesPago WHERE CondicionPagoId = @CondicionPagoId;"

    Private Const SqlExisteActiva As String =
        "SELECT 1 FROM dbo.CondicionesPago WHERE CondicionPagoId = @CondicionPagoId AND Activo = 1;"

    Public Function ListarActivasParaCombo() As List(Of CondicionPagoSelectorItem)
        Dim lista As New List(Of CondicionPagoSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarActivas, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim id = LeerInt32(reader, "CondicionPagoId")
                        Dim cod = LeerString(reader, "Codigo")
                        Dim nom = LeerString(reader, "Nombre")
                        lista.Add(New CondicionPagoSelectorItem With {
                            .CondicionPagoId = id,
                            .Texto = cod & " — " & nom
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ObtenerPorId(condicionPagoId As Integer) As CondicionPagoSelectorItem
        If condicionPagoId <= 0 Then Return Nothing
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlObtenerPorId, CommandType.Text)
                AgregarParametro(cmd, "@CondicionPagoId", condicionPagoId)
                Using reader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not reader.Read() Then Return Nothing
                    Dim id = LeerInt32(reader, "CondicionPagoId")
                    Dim cod = LeerString(reader, "Codigo")
                    Dim nom = LeerString(reader, "Nombre")
                    Return New CondicionPagoSelectorItem With {
                        .CondicionPagoId = id,
                        .Texto = cod & " — " & nom
                    }
                End Using
            End Using
        End Using
    End Function

    Public Function ExisteActiva(condicionPagoId As Integer) As Boolean
        If condicionPagoId <= 0 Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteActiva, CommandType.Text)
                AgregarParametro(cmd, "@CondicionPagoId", condicionPagoId)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function
End Class
