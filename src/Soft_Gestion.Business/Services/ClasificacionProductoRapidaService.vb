Imports Soft_Gestion.Data

''' <summary>
''' Alta combinada categoría → subcategoría → grupo (borrador asistido).
''' </summary>
Public Class ClasificacionProductoRapidaService
    Inherits ServicioBase

    Private Const LongitudMaxNombre As Integer = 100

    Private ReadOnly _repositorio As ClasificacionProductoRapidaRepository

    Public Sub New(Optional repositorio As ClasificacionProductoRapidaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New ClasificacionProductoRapidaRepository())
    End Sub

    Public Function GuardarJerarquiaNueva(nombreCategoria As String, nombreSubcategoria As String, nombreGrupo As String, usuarioAuditoria As String) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")

        Dim nc = If(nombreCategoria, String.Empty).Trim()
        Dim ns = If(nombreSubcategoria, String.Empty).Trim()
        Dim ng = If(nombreGrupo, String.Empty).Trim()

        If nc.Length = 0 Then Return ResultadoOperacion.Fallo("El nombre de categoría es obligatorio.")
        If ns.Length = 0 Then Return ResultadoOperacion.Fallo("El nombre de subcategoría es obligatorio.")
        If ng.Length = 0 Then Return ResultadoOperacion.Fallo("El nombre de grupo es obligatorio.")
        If nc.Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre de categoría supera la longitud permitida.")
        If ns.Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre de subcategoría supera la longitud permitida.")
        If ng.Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre de grupo supera la longitud permitida.")

        Try
            Dim t = _repositorio.InsertarJerarquiaNueva(nc, ns, ng, usuarioAuditoria.Trim())
            Dim res = ResultadoOperacion.Ok(t.Item3)
            res.Mensaje = "Se creó la jerarquía: categoría Id " & t.Item1.ToString() &
                ", subcategoría Id " & t.Item2.ToString() & ", grupo Id " & t.Item3.ToString() & "."
            Return res
        Catch ex As Exception
            Return ResultadoOperacion.Fallo(ex.Message)
        End Try
    End Function
End Class
