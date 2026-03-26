Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para proveedores.
''' </summary>
Public Class ProveedorService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxRazonSocial As Integer = 150
    Private Const LongitudMaxRuc As Integer = 30
    Private Const LongitudMaxDireccion As Integer = 200
    Private Const LongitudMaxTelefono As Integer = 50
    Private Const LongitudMaxEmail As Integer = 150
    Private Const LongitudMaxObservaciones As Integer = 255

    Private ReadOnly _repositorio As ProveedorRepository
    Private ReadOnly _condicionesPago As CondicionPagoRepository

    Public Sub New(Optional repositorio As ProveedorRepository = Nothing,
                   Optional condicionesPago As CondicionPagoRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New ProveedorRepository())
        _condicionesPago = If(condicionesPago, New CondicionPagoRepository())
    End Sub

    Public Function ListarProveedores(filtroTexto As String) As List(Of ProveedorResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(proveedorId As Integer) As Proveedor
        Return _repositorio.ObtenerPorId(proveedorId)
    End Function

    Public Function ListarCondicionesPagoActivasParaSelector() As List(Of CondicionPagoSelectorItem)
        Return _condicionesPago.ListarActivasParaCombo()
    End Function

    ''' <summary>
    ''' Incluye condición inactiva si el proveedor ya la tiene asignada y no está en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemCondicionPagoParaEdicion(condicionPagoId As Integer?) As CondicionPagoSelectorItem
        If Not condicionPagoId.HasValue OrElse condicionPagoId.Value <= 0 Then Return Nothing
        Return _condicionesPago.ObtenerPorId(condicionPagoId.Value)
    End Function

    Public Function GuardarNuevo(proveedor As Proveedor, usuarioAuditoria As String) As ResultadoOperacion
        If proveedor Is Nothing Then Return ResultadoOperacion.Fallo("Datos de proveedor no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        proveedor.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(proveedor.Codigo)
        Dim val = ValidarProveedor(proveedor)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(proveedor.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un proveedor con ese código.")
        End If
        Dim fk = ValidarReferenciasOpcionales(proveedor)
        If Not fk.Exitoso Then Return fk

        Dim nuevo As New Proveedor With {
            .Codigo = proveedor.Codigo,
            .RazonSocial = proveedor.RazonSocial.Trim(),
            .RUC = NormalizarOpcional(proveedor.RUC),
            .Direccion = NormalizarOpcional(proveedor.Direccion),
            .Telefono = NormalizarOpcional(proveedor.Telefono),
            .Email = NormalizarOpcional(proveedor.Email),
            .CondicionPagoId = proveedor.CondicionPagoId,
            .Observaciones = NormalizarOpcional(proveedor.Observaciones),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el proveedor.")
        End Try
    End Function

    Public Function EditarExistente(proveedor As Proveedor, usuarioAuditoria As String) As ResultadoOperacion
        If proveedor Is Nothing OrElse proveedor.ProveedorId <= 0 Then Return ResultadoOperacion.Fallo("Proveedor no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        proveedor.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(proveedor.Codigo)
        Dim val = ValidarProveedor(proveedor)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(proveedor.ProveedorId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El proveedor no existe.")
        If _repositorio.ExisteCodigo(proveedor.Codigo, proveedor.ProveedorId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro proveedor con ese código.")
        End If
        Dim fk = ValidarReferenciasOpcionales(proveedor)
        If Not fk.Exitoso Then Return fk

        existente.Codigo = proveedor.Codigo
        existente.RazonSocial = proveedor.RazonSocial.Trim()
        existente.RUC = NormalizarOpcional(proveedor.RUC)
        existente.Direccion = NormalizarOpcional(proveedor.Direccion)
        existente.Telefono = NormalizarOpcional(proveedor.Telefono)
        existente.Email = NormalizarOpcional(proveedor.Email)
        existente.CondicionPagoId = proveedor.CondicionPagoId
        existente.Observaciones = NormalizarOpcional(proveedor.Observaciones)

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el proveedor.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el proveedor.")
        End Try
    End Function

    Public Function Activar(proveedorId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(proveedorId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(proveedorId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(proveedorId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(proveedorId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If proveedorId <= 0 Then Return ResultadoOperacion.Fallo("Proveedor no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(proveedorId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El proveedor no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(proveedorId, usuarioAuditoria.Trim()), _repositorio.Desactivar(proveedorId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del proveedor.")
        End Try
    End Function

    Private Function ValidarReferenciasOpcionales(proveedor As Proveedor) As ResultadoOperacion
        If proveedor.CondicionPagoId.HasValue AndAlso proveedor.CondicionPagoId.Value > 0 Then
            If Not _condicionesPago.ExisteActiva(proveedor.CondicionPagoId.Value) Then
                Return ResultadoOperacion.Fallo("La condición de pago no existe o está inactiva.")
            End If
        End If
        Return ResultadoOperacion.Ok()
    End Function

    Private Function ValidarProveedor(proveedor As Proveedor) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(proveedor.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If proveedor.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(proveedor.RazonSocial) Then Return ResultadoOperacion.Fallo("La razón social es obligatoria.")
        If proveedor.RazonSocial.Trim().Length > LongitudMaxRazonSocial Then Return ResultadoOperacion.Fallo("La razón social supera la longitud permitida.")

        Dim ruc = If(proveedor.RUC, String.Empty).Trim()
        If ruc.Length > LongitudMaxRuc Then Return ResultadoOperacion.Fallo("El RUC supera la longitud permitida.")

        Dim dir = If(proveedor.Direccion, String.Empty).Trim()
        If dir.Length > LongitudMaxDireccion Then Return ResultadoOperacion.Fallo("La dirección supera la longitud permitida.")

        Dim tel = If(proveedor.Telefono, String.Empty).Trim()
        If tel.Length > LongitudMaxTelefono Then Return ResultadoOperacion.Fallo("El teléfono supera la longitud permitida.")

        Dim em = If(proveedor.Email, String.Empty).Trim()
        If em.Length > LongitudMaxEmail Then Return ResultadoOperacion.Fallo("El correo supera la longitud permitida.")
        If em.Length > 0 AndAlso Not em.Contains("@"c) Then Return ResultadoOperacion.Fallo("El correo electrónico no es válido.")

        Dim obs = If(proveedor.Observaciones, String.Empty).Trim()
        If obs.Length > LongitudMaxObservaciones Then Return ResultadoOperacion.Fallo("Las observaciones superan la longitud permitida.")

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarOpcional(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Return valor.Trim()
    End Function
End Class
