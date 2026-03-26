Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para unidades de medida. Valida <see cref="UnidadMedida.Codigo_SIFEN"/> contra el catálogo SIFEN.
''' </summary>
Public Class UnidadMedidaService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100
    Private Const LongitudMaxAbreviatura As Integer = 10

    Private ReadOnly _repositorio As UnidadMedidaRepository
    Private ReadOnly _sifen As UnidadMedidaSIFENRepository

    Public Sub New(Optional repositorio As UnidadMedidaRepository = Nothing,
                   Optional sifen As UnidadMedidaSIFENRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New UnidadMedidaRepository())
        _sifen = If(sifen, New UnidadMedidaSIFENRepository())
    End Sub

    Public Function ListarUnidadesMedida(filtroTexto As String) As List(Of UnidadMedidaResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    ''' <summary>Unidades activas para combo (incluye Codigo_SIFEN y datos denormalizados del catálogo SIFEN).</summary>
    Public Function ListarActivasParaCombo() As List(Of UnidadMedidaResumen)
        Return _repositorio.ListarActivasParaCombo()
    End Function

    Public Function ObtenerPorId(unidadMedidaId As Integer) As UnidadMedida
        Return _repositorio.ObtenerPorId(unidadMedidaId)
    End Function

    Public Function ListarCodigosSifenParaSelector() As List(Of UnidadMedidaSifenSelectorItem)
        Return _sifen.ListarParaSelector()
    End Function

    Public Function GuardarNuevo(um As UnidadMedida) As ResultadoOperacion
        If um Is Nothing Then Return ResultadoOperacion.Fallo("Datos de unidad de medida no válidos.")
        um.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(um.Codigo)
        Dim val = ValidarUnidadMedida(um)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(um.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una unidad de medida con ese código.")
        End If
        If _repositorio.ExisteAbreviatura(um.Abreviatura, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una unidad de medida con esa abreviatura.")
        End If

        Dim nuevo As New UnidadMedida With {
            .Codigo = um.Codigo,
            .Nombre = um.Nombre.Trim(),
            .Abreviatura = um.Abreviatura.Trim(),
            .Activo = True,
            .Codigo_SIFEN = um.Codigo_SIFEN
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la unidad de medida.")
        End Try
    End Function

    Public Function EditarExistente(um As UnidadMedida) As ResultadoOperacion
        If um Is Nothing OrElse um.UnidadMedidaId <= 0 Then Return ResultadoOperacion.Fallo("Unidad de medida no válida.")
        um.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(um.Codigo)
        Dim val = ValidarUnidadMedida(um)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(um.UnidadMedidaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La unidad de medida no existe.")
        If _repositorio.ExisteCodigo(um.Codigo, um.UnidadMedidaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra unidad de medida con ese código.")
        End If
        If _repositorio.ExisteAbreviatura(um.Abreviatura, um.UnidadMedidaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra unidad de medida con esa abreviatura.")
        End If

        existente.Codigo = um.Codigo
        existente.Nombre = um.Nombre.Trim()
        existente.Abreviatura = um.Abreviatura.Trim()
        existente.Codigo_SIFEN = um.Codigo_SIFEN

        Try
            Dim filas = _repositorio.Actualizar(existente)
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar la unidad de medida.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar la unidad de medida.")
        End Try
    End Function

    Public Function Activar(unidadMedidaId As Integer) As ResultadoOperacion
        Return EstablecerActividad(unidadMedidaId, True)
    End Function

    Public Function Desactivar(unidadMedidaId As Integer) As ResultadoOperacion
        Return EstablecerActividad(unidadMedidaId, False)
    End Function

    Private Function EstablecerActividad(unidadMedidaId As Integer, activo As Boolean) As ResultadoOperacion
        If unidadMedidaId <= 0 Then Return ResultadoOperacion.Fallo("Unidad de medida no válida.")
        Dim existente = _repositorio.ObtenerPorId(unidadMedidaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La unidad de medida no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(unidadMedidaId), _repositorio.Desactivar(unidadMedidaId))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado de la unidad de medida.")
        End Try
    End Function

    Private Function ValidarUnidadMedida(um As UnidadMedida) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(um.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If um.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(um.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If um.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(um.Abreviatura) Then Return ResultadoOperacion.Fallo("La abreviatura es obligatoria.")
        If um.Abreviatura.Trim().Length > LongitudMaxAbreviatura Then Return ResultadoOperacion.Fallo("La abreviatura supera la longitud permitida.")

        If um.Codigo_SIFEN.HasValue AndAlso Not _sifen.ExisteCodigo(um.Codigo_SIFEN.Value) Then
            Return ResultadoOperacion.Fallo("El código SIFEN indicado no existe en el catálogo de unidades SIFEN.")
        End If

        Return ResultadoOperacion.Ok()
    End Function
End Class
