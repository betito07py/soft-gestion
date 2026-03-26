Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Catálogo de referencia <c>dbo.UnidadesMedidaSIFEN</c> (solo lectura para combos y validación).
''' </summary>
Public Class UnidadMedidaSIFENRepository
    Inherits RepositorioBase

    Private Const SqlListar As String =
        "SELECT Codigo, Descrip_Unidad, Codigo_Repr FROM dbo.UnidadesMedidaSIFEN ORDER BY Codigo;"

    Private Const SqlExisteCodigo As String =
        "SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = @Codigo;"

    Public Function ListarParaSelector() As List(Of UnidadMedidaSifenSelectorItem)
        Dim lista As New List(Of UnidadMedidaSifenSelectorItem)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListar, CommandType.Text)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim cod = reader.GetInt16(reader.GetOrdinal("Codigo"))
                        Dim descrip = LeerString(reader, "Descrip_Unidad")
                        Dim repr = LeerString(reader, "Codigo_Repr")
                        lista.Add(New UnidadMedidaSifenSelectorItem With {
                            .CodigoSifen = cod,
                            .DescripUnidad = descrip,
                            .CodigoRepr = repr,
                            .Texto = cod.ToString() & " — " & descrip & " (" & repr & ")"
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Function ExisteCodigo(codigo As Short) As Boolean
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlExisteCodigo, CommandType.Text)
                AgregarParametro(cmd, "@Codigo", codigo)
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function
End Class
