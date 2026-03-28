Imports System
Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para el maestro de impuestos (Paraguay).
''' </summary>
Public Class ImpuestoService
    Inherits ServicioBase

    Private Const LongitudMaxNombre As Integer = 100
    Private Const LongitudMaxTipo As Integer = 20

    Private ReadOnly _repositorio As ImpuestoRepository

    Public Sub New(Optional repositorio As ImpuestoRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New ImpuestoRepository())
    End Sub

    Public Function Listar(filtroTexto As String, soloActivos As Boolean) As List(Of ImpuestoResumen)
        Return _repositorio.Listar(filtroTexto, soloActivos)
    End Function

    Public Function ObtenerPorId(impuestoId As Integer) As Impuesto
        Return _repositorio.ObtenerPorId(impuestoId)
    End Function

    Public Function ListarParaSelector() As List(Of ImpuestoSelectorItem)
        Return _repositorio.ListarParaSelector()
    End Function

    ''' <summary>Alta o edición según <c>ImpuestoId</c>.</summary>
    Public Function Guardar(entidad As Impuesto, usuarioAuditoria As String) As ResultadoOperacion
        If entidad Is Nothing Then Return ResultadoOperacion.Fallo("Datos de impuesto no válidos.")
        If entidad.ImpuestoId <= 0 Then
            Return GuardarNuevo(entidad, usuarioAuditoria)
        End If
        Return EditarExistente(entidad, usuarioAuditoria)
    End Function

    Public Function GuardarNuevo(entidad As Impuesto, usuarioAuditoria As String) As ResultadoOperacion
        If entidad Is Nothing Then Return ResultadoOperacion.Fallo("Datos de impuesto no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarImpuesto(entidad, esNuevo:=True)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(entidad.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un impuesto con ese código.")
        End If

        Dim nuevo As New Impuesto With {
            .Codigo = entidad.Codigo,
            .Nombre = entidad.Nombre.Trim(),
            .TipoImpuesto = entidad.TipoImpuesto.Trim(),
            .Porcentaje = AjustarPorcentajeSegunExento(entidad),
            .EsExento = entidad.EsExento,
            .CodigoSIFEN = entidad.CodigoSIFEN,
            .EsActivo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el impuesto.")
        End Try
    End Function

    Public Function EditarExistente(entidad As Impuesto, usuarioAuditoria As String) As ResultadoOperacion
        If entidad Is Nothing OrElse entidad.ImpuestoId <= 0 Then Return ResultadoOperacion.Fallo("Impuesto no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarImpuesto(entidad, esNuevo:=False)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(entidad.ImpuestoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El impuesto no existe.")
        If _repositorio.ExisteCodigo(entidad.Codigo, entidad.ImpuestoId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro impuesto con ese código.")
        End If

        existente.Codigo = entidad.Codigo
        existente.Nombre = entidad.Nombre.Trim()
        existente.TipoImpuesto = entidad.TipoImpuesto.Trim()
        existente.EsExento = entidad.EsExento
        existente.Porcentaje = AjustarPorcentajeSegunExento(entidad)
        existente.CodigoSIFEN = entidad.CodigoSIFEN

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el impuesto.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el impuesto.")
        End Try
    End Function

    Public Function Activar(impuestoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(impuestoId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(impuestoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(impuestoId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(impuestoId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If impuestoId <= 0 Then Return ResultadoOperacion.Fallo("Impuesto no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        If _repositorio.ObtenerPorId(impuestoId) Is Nothing Then Return ResultadoOperacion.Fallo("El impuesto no existe.")
        Try
            If Not _repositorio.CambiarEstado(impuestoId, activo, usuarioAuditoria.Trim()) Then
                Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            End If
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del impuesto.")
        End Try
    End Function

    ''' <summary>
    ''' Monto de IVA incluido en un total con IVA (precio al consumidor), según alícuota Paraguay.
    ''' 10%: IVA = total/11; 5%: IVA = total/21; 0%: 0; otras alícuotas: total × p / (100 + p).
    ''' </summary>
    Public Shared Function CalcularMontoIVA(total As Decimal, porcentaje As Decimal) As Decimal
        If total <= 0D Then Return 0D
        If porcentaje = 0D Then Return 0D
        If porcentaje = 10D Then
            Return Decimal.Round(total / 11D, 2, MidpointRounding.AwayFromZero)
        End If
        If porcentaje = 5D Then
            Return Decimal.Round(total / 21D, 2, MidpointRounding.AwayFromZero)
        End If
        Return Decimal.Round(total * porcentaje / (100D + porcentaje), 2, MidpointRounding.AwayFromZero)
    End Function

    Private Shared Function AjustarPorcentajeSegunExento(entidad As Impuesto) As Decimal
        If entidad.EsExento Then Return 0D
        Return entidad.Porcentaje
    End Function

    Private Function ValidarImpuesto(entidad As Impuesto, esNuevo As Boolean) As ResultadoOperacion
        If entidad.Codigo <= 0 Then Return ResultadoOperacion.Fallo("El código es obligatorio y debe ser mayor que cero.")
        If String.IsNullOrWhiteSpace(entidad.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If entidad.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(entidad.TipoImpuesto) Then Return ResultadoOperacion.Fallo("El tipo de impuesto es obligatorio.")
        If entidad.TipoImpuesto.Trim().Length > LongitudMaxTipo Then Return ResultadoOperacion.Fallo("El tipo de impuesto supera la longitud permitida.")
        If entidad.Porcentaje < 0D Then Return ResultadoOperacion.Fallo("El porcentaje no puede ser negativo.")
        If entidad.EsExento AndAlso entidad.Porcentaje <> 0D Then
            Return ResultadoOperacion.Fallo("Si está exento, el porcentaje debe ser 0.")
        End If
        If Not entidad.EsExento AndAlso entidad.Porcentaje > 100D Then
            Return ResultadoOperacion.Fallo("El porcentaje no puede superar 100.")
        End If
        Return ResultadoOperacion.Ok()
    End Function
End Class
