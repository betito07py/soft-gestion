Imports System

''' <summary>
''' Fila de <c>dbo.Productos_Barras</c>: un código de barras asociado a un producto.
''' </summary>
Public Class ProductoBarra
    Public Property ProductoBarraId As Integer
    Public Property ProductoId As Integer
    Public Property CodBarras As String
    Public Property FechaAlta As DateTime
End Class
