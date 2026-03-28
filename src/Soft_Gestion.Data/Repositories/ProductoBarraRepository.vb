Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports Soft_Gestion.Domain

''' <summary>
''' Acceso a datos de <c>dbo.Productos_Barras</c>.
''' </summary>
Public Class ProductoBarraRepository
    Inherits RepositorioBase

    Private Const SqlListarPorProducto As String =
        "SELECT ProductoBarraId, ProductoId, CodBarras, FechaAlta " &
        "FROM dbo.Productos_Barras WHERE ProductoId = @ProductoId " &
        "ORDER BY FechaAlta ASC, ProductoBarraId ASC;"

    Private Const SqlDeletePorProducto As String =
        "DELETE FROM dbo.Productos_Barras WHERE ProductoId = @ProductoId;"

    Private Const SqlInsert As String =
        "INSERT INTO dbo.Productos_Barras (ProductoId, CodBarras) VALUES (@ProductoId, @CodBarras);"

    Private Const SqlExisteCodBarrasCualquierProducto As String =
        "SELECT 1 FROM dbo.Productos_Barras WHERE CodBarras = @CodBarras;"

    Private Const SqlExisteCodBarrasOtroProducto As String =
        "SELECT 1 FROM dbo.Productos_Barras WHERE CodBarras = @CodBarras AND ProductoId <> @ExcluirProductoId;"

    Private Const SqlSincronizarCodigoBarrasProducto As String =
        "UPDATE dbo.Productos SET CodigoBarras = " &
        "(SELECT TOP (1) pb.CodBarras FROM dbo.Productos_Barras pb WHERE pb.ProductoId = @ProductoId ORDER BY pb.FechaAlta ASC, pb.ProductoBarraId ASC) " &
        "WHERE ProductoId = @ProductoId;"

    Public Function ListarPorProductoId(productoId As Integer) As List(Of ProductoBarra)
        Dim lista As New List(Of ProductoBarra)()
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using cmd As SqlCommand = CrearComando(cn, SqlListarPorProducto, CommandType.Text)
                AgregarParametro(cmd, "@ProductoId", productoId)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New ProductoBarra With {
                            .ProductoBarraId = LeerInt32(reader, "ProductoBarraId"),
                            .ProductoId = LeerInt32(reader, "ProductoId"),
                            .CodBarras = LeerString(reader, "CodBarras"),
                            .FechaAlta = LeerDateTime(reader, "FechaAlta")
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    ''' <summary>
    ''' Indica si el código ya está asignado a otro producto (o al mismo si se excluye por edición).
    ''' </summary>
    Public Function ExisteCodBarrasEnOtroProducto(codBarras As String, excluirProductoId As Integer?) As Boolean
        If String.IsNullOrWhiteSpace(codBarras) Then Return False
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Dim sql = If(excluirProductoId.HasValue, SqlExisteCodBarrasOtroProducto, SqlExisteCodBarrasCualquierProducto)
            Using cmd As SqlCommand = CrearComando(cn, sql, CommandType.Text)
                AgregarParametro(cmd, "@CodBarras", codBarras.Trim())
                If excluirProductoId.HasValue Then
                    AgregarParametro(cmd, "@ExcluirProductoId", excluirProductoId.Value)
                End If
                Dim o = cmd.ExecuteScalar()
                Return o IsNot Nothing AndAlso o IsNot DBNull.Value
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Reemplaza todos los códigos del producto por la lista indicada (orden conservado).
    ''' </summary>
    Public Sub ReemplazarTodos(productoId As Integer, codigos As IList(Of String))
        If productoId <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(productoId))
        Using cn As SqlConnection = CrearConexion()
            cn.Open()
            Using tx As SqlTransaction = cn.BeginTransaction()
                Using cmdDel As SqlCommand = CrearComando(cn, SqlDeletePorProducto, CommandType.Text, tx)
                    AgregarParametro(cmdDel, "@ProductoId", productoId)
                    cmdDel.ExecuteNonQuery()
                End Using
                If codigos IsNot Nothing Then
                    For Each c In codigos
                        If String.IsNullOrWhiteSpace(c) Then Continue For
                        Using cmdIns As SqlCommand = CrearComando(cn, SqlInsert, CommandType.Text, tx)
                            AgregarParametro(cmdIns, "@ProductoId", productoId)
                            AgregarParametro(cmdIns, "@CodBarras", c.Trim())
                            cmdIns.ExecuteNonQuery()
                        End Using
                    Next
                End If
                Using cmdSync As SqlCommand = CrearComando(cn, SqlSincronizarCodigoBarrasProducto, CommandType.Text, tx)
                    AgregarParametro(cmdSync, "@ProductoId", productoId)
                    cmdSync.ExecuteNonQuery()
                End Using
                tx.Commit()
            End Using
        End Using
    End Sub
End Class
