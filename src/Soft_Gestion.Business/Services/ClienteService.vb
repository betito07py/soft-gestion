Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para clientes.
''' </summary>
Public Class ClienteService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxRazonSocial As Integer = 150
    Private Const LongitudMaxNombreFantasia As Integer = 150
    Private Const LongitudMaxDocumento As Integer = 30
    Private Const LongitudMaxRuc As Integer = 30
    Private Const LongitudMaxDireccion As Integer = 200
    Private Const LongitudMaxTelefono As Integer = 50
    Private Const LongitudMaxEmail As Integer = 150
    Private Const LongitudMaxObservaciones As Integer = 255

    Private ReadOnly _repositorio As ClienteRepository
    Private ReadOnly _condicionesPago As CondicionPagoRepository
    Private ReadOnly _listasPrecio As ListaPrecioRepository

    Public Sub New(Optional repositorio As ClienteRepository = Nothing,
                   Optional condicionesPago As CondicionPagoRepository = Nothing,
                   Optional listasPrecio As ListaPrecioRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New ClienteRepository())
        _condicionesPago = If(condicionesPago, New CondicionPagoRepository())
        _listasPrecio = If(listasPrecio, New ListaPrecioRepository())
    End Sub

    Public Function ListarClientes(filtroTexto As String) As List(Of ClienteResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(clienteId As Integer) As Cliente
        Return _repositorio.ObtenerPorId(clienteId)
    End Function

    Public Function ListarCondicionesPagoActivasParaSelector() As List(Of CondicionPagoSelectorItem)
        Return _condicionesPago.ListarActivasParaCombo()
    End Function

    Public Function ListarListasPrecioActivasParaSelector() As List(Of ListaPrecioSelectorItem)
        Return _listasPrecio.ListarActivasParaCombo()
    End Function

    ''' <summary>
    ''' Incluye condición inactiva si el cliente ya la tiene asignada y no está en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemCondicionPagoParaEdicion(condicionPagoId As Integer?) As CondicionPagoSelectorItem
        If Not condicionPagoId.HasValue OrElse condicionPagoId.Value <= 0 Then Return Nothing
        Return _condicionesPago.ObtenerPorId(condicionPagoId.Value)
    End Function

    ''' <summary>
    ''' Incluye lista inactiva si el cliente ya la tiene asignada y no está en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemListaPrecioParaEdicion(listaPrecioId As Integer?) As ListaPrecioSelectorItem
        If Not listaPrecioId.HasValue OrElse listaPrecioId.Value <= 0 Then Return Nothing
        Return _listasPrecio.ObtenerPorId(listaPrecioId.Value)
    End Function

    Public Function GuardarNuevo(cliente As Cliente, usuarioAuditoria As String) As ResultadoOperacion
        If cliente Is Nothing Then Return ResultadoOperacion.Fallo("Datos de cliente no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarCliente(cliente)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(cliente.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un cliente con ese código.")
        End If
        Dim fk = ValidarReferenciasOpcionales(cliente)
        If Not fk.Exitoso Then Return fk

        Dim nuevo As New Cliente With {
            .Codigo = cliente.Codigo,
            .RazonSocial = cliente.RazonSocial.Trim(),
            .NombreFantasia = NormalizarOpcional(cliente.NombreFantasia),
            .Documento = NormalizarOpcional(cliente.Documento),
            .RUC = NormalizarOpcional(cliente.RUC),
            .Direccion = NormalizarOpcional(cliente.Direccion),
            .Telefono = NormalizarOpcional(cliente.Telefono),
            .Email = NormalizarOpcional(cliente.Email),
            .CondicionPagoId = cliente.CondicionPagoId,
            .ListaPrecioId = cliente.ListaPrecioId,
            .LimiteCredito = cliente.LimiteCredito,
            .Observaciones = NormalizarOpcional(cliente.Observaciones),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el cliente.")
        End Try
    End Function

    Public Function EditarExistente(cliente As Cliente, usuarioAuditoria As String) As ResultadoOperacion
        If cliente Is Nothing OrElse cliente.ClienteId <= 0 Then Return ResultadoOperacion.Fallo("Cliente no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        cliente.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(cliente.Codigo)
        Dim val = ValidarCliente(cliente)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(cliente.ClienteId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El cliente no existe.")
        If _repositorio.ExisteCodigo(cliente.Codigo, cliente.ClienteId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro cliente con ese código.")
        End If
        Dim fk = ValidarReferenciasOpcionales(cliente)
        If Not fk.Exitoso Then Return fk

        existente.Codigo = cliente.Codigo
        existente.RazonSocial = cliente.RazonSocial.Trim()
        existente.NombreFantasia = NormalizarOpcional(cliente.NombreFantasia)
        existente.Documento = NormalizarOpcional(cliente.Documento)
        existente.RUC = NormalizarOpcional(cliente.RUC)
        existente.Direccion = NormalizarOpcional(cliente.Direccion)
        existente.Telefono = NormalizarOpcional(cliente.Telefono)
        existente.Email = NormalizarOpcional(cliente.Email)
        existente.CondicionPagoId = cliente.CondicionPagoId
        existente.ListaPrecioId = cliente.ListaPrecioId
        existente.LimiteCredito = cliente.LimiteCredito
        existente.Observaciones = NormalizarOpcional(cliente.Observaciones)

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el cliente.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el cliente.")
        End Try
    End Function

    Public Function Activar(clienteId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(clienteId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(clienteId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(clienteId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(clienteId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If clienteId <= 0 Then Return ResultadoOperacion.Fallo("Cliente no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(clienteId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El cliente no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(clienteId, usuarioAuditoria.Trim()), _repositorio.Desactivar(clienteId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del cliente.")
        End Try
    End Function

    Private Function ValidarReferenciasOpcionales(cliente As Cliente) As ResultadoOperacion
        If cliente.CondicionPagoId.HasValue AndAlso cliente.CondicionPagoId.Value > 0 Then
            If Not _condicionesPago.ExisteActiva(cliente.CondicionPagoId.Value) Then
                Return ResultadoOperacion.Fallo("La condición de pago no existe o está inactiva.")
            End If
        End If
        If cliente.ListaPrecioId.HasValue AndAlso cliente.ListaPrecioId.Value > 0 Then
            If Not _listasPrecio.ExisteActiva(cliente.ListaPrecioId.Value) Then
                Return ResultadoOperacion.Fallo("La lista de precios no existe o está inactiva.")
            End If
        End If
        Return ResultadoOperacion.Ok()
    End Function

    Private Function ValidarCliente(cliente As Cliente) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(cliente.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If cliente.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(cliente.RazonSocial) Then Return ResultadoOperacion.Fallo("La razón social es obligatoria.")
        If cliente.RazonSocial.Trim().Length > LongitudMaxRazonSocial Then Return ResultadoOperacion.Fallo("La razón social supera la longitud permitida.")

        Dim nf = If(cliente.NombreFantasia, String.Empty).Trim()
        If nf.Length > LongitudMaxNombreFantasia Then Return ResultadoOperacion.Fallo("El nombre de fantasía supera la longitud permitida.")

        Dim doc = If(cliente.Documento, String.Empty).Trim()
        If doc.Length > LongitudMaxDocumento Then Return ResultadoOperacion.Fallo("El documento supera la longitud permitida.")

        Dim ruc = If(cliente.RUC, String.Empty).Trim()
        If ruc.Length > LongitudMaxRuc Then Return ResultadoOperacion.Fallo("El RUC supera la longitud permitida.")

        Dim dir = If(cliente.Direccion, String.Empty).Trim()
        If dir.Length > LongitudMaxDireccion Then Return ResultadoOperacion.Fallo("La dirección supera la longitud permitida.")

        Dim tel = If(cliente.Telefono, String.Empty).Trim()
        If tel.Length > LongitudMaxTelefono Then Return ResultadoOperacion.Fallo("El teléfono supera la longitud permitida.")

        Dim em = If(cliente.Email, String.Empty).Trim()
        If em.Length > LongitudMaxEmail Then Return ResultadoOperacion.Fallo("El correo supera la longitud permitida.")
        If em.Length > 0 AndAlso Not em.Contains("@"c) Then Return ResultadoOperacion.Fallo("El correo electrónico no es válido.")

        Dim obs = If(cliente.Observaciones, String.Empty).Trim()
        If obs.Length > LongitudMaxObservaciones Then Return ResultadoOperacion.Fallo("Las observaciones superan la longitud permitida.")

        If cliente.LimiteCredito < 0D Then Return ResultadoOperacion.Fallo("El límite de crédito no puede ser negativo.")

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarOpcional(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Return valor.Trim()
    End Function
End Class
