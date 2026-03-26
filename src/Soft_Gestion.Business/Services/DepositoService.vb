Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para depósitos.
''' </summary>
Public Class DepositoService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100
    Private Const LongitudMaxDescripcion As Integer = 255

    Private ReadOnly _repositorio As DepositoRepository
    Private ReadOnly _sucursales As SucursalRepository
    Private ReadOnly _empresas As EmpresaRepository

    Public Sub New(Optional repositorio As DepositoRepository = Nothing,
                   Optional sucursales As SucursalRepository = Nothing,
                   Optional empresas As EmpresaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New DepositoRepository())
        _sucursales = If(sucursales, New SucursalRepository())
        _empresas = If(empresas, New EmpresaRepository())
    End Sub

    Public Function ListarDepositos(filtroTexto As String, empresaIdFiltro As Integer?, sucursalIdFiltro As Integer?) As List(Of DepositoResumen)
        Dim emp = If(empresaIdFiltro.HasValue AndAlso empresaIdFiltro.Value > 0, empresaIdFiltro, Nothing)
        Dim suc = If(sucursalIdFiltro.HasValue AndAlso sucursalIdFiltro.Value > 0, sucursalIdFiltro, Nothing)
        Return _repositorio.Listar(filtroTexto, emp, suc)
    End Function

    Public Function ListarEmpresasActivasParaSelector() As List(Of EmpresaSelectorItem)
        Return _empresas.ListarActivasParaCombo()
    End Function

    Public Function ListarSucursalesActivasParaSelector(Optional empresaId As Integer? = Nothing) As List(Of SucursalSelectorItem)
        Return _sucursales.ListarActivasParaCombo(empresaId)
    End Function

    ''' <summary>
    ''' Para edición cuando la sucursal del depósito no está en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemSucursalParaEdicion(sucursalId As Integer) As SucursalSelectorItem
        If sucursalId <= 0 Then Return Nothing
        Dim s = _sucursales.ObtenerPorId(sucursalId)
        If s Is Nothing Then Return Nothing
        Dim e = _empresas.ObtenerPorId(s.EmpresaId)
        Dim pref = If(e IsNot Nothing, e.Codigo & " | ", "")
        Return New SucursalSelectorItem With {
            .SucursalId = s.SucursalId,
            .Texto = pref & s.Codigo & " — " & s.Nombre
        }
    End Function

    Public Function ObtenerPorId(depositoId As Integer) As Deposito
        Return _repositorio.ObtenerPorId(depositoId)
    End Function

    Public Function GuardarNuevo(deposito As Deposito, usuarioAuditoria As String) As ResultadoOperacion
        If deposito Is Nothing Then Return ResultadoOperacion.Fallo("Datos de depósito no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        deposito.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(deposito.Codigo)
        Dim val = ValidarDeposito(deposito, requiereSucursal:=True)
        If Not val.Exitoso Then Return val
        If Not _sucursales.ExisteActiva(deposito.SucursalId) Then
            Return ResultadoOperacion.Fallo("La sucursal no existe o está inactiva.")
        End If
        If _repositorio.ExisteCodigo(deposito.SucursalId, deposito.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un depósito con ese código en la sucursal seleccionada.")
        End If

        Dim nuevo As New Deposito With {
            .SucursalId = deposito.SucursalId,
            .Codigo = deposito.Codigo,
            .Nombre = deposito.Nombre.Trim(),
            .Descripcion = NormalizarOpcional(deposito.Descripcion),
            .EsPrincipal = deposito.EsPrincipal,
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el depósito.")
        End Try
    End Function

    Public Function EditarExistente(deposito As Deposito, usuarioAuditoria As String) As ResultadoOperacion
        If deposito Is Nothing OrElse deposito.DepositoId <= 0 Then Return ResultadoOperacion.Fallo("Depósito no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        deposito.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(deposito.Codigo)
        Dim val = ValidarDeposito(deposito, requiereSucursal:=True)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(deposito.DepositoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El depósito no existe.")
        If Not _sucursales.ExisteActiva(deposito.SucursalId) Then
            Return ResultadoOperacion.Fallo("La sucursal no existe o está inactiva.")
        End If
        If _repositorio.ExisteCodigo(deposito.SucursalId, deposito.Codigo, deposito.DepositoId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro depósito con ese código en la sucursal seleccionada.")
        End If

        existente.SucursalId = deposito.SucursalId
        existente.Codigo = deposito.Codigo
        existente.Nombre = deposito.Nombre.Trim()
        existente.Descripcion = NormalizarOpcional(deposito.Descripcion)
        existente.EsPrincipal = deposito.EsPrincipal

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el depósito.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el depósito.")
        End Try
    End Function

    Public Function Activar(depositoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(depositoId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(depositoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(depositoId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(depositoId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If depositoId <= 0 Then Return ResultadoOperacion.Fallo("Depósito no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(depositoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El depósito no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(depositoId, usuarioAuditoria.Trim()), _repositorio.Desactivar(depositoId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del depósito.")
        End Try
    End Function

    Private Function ValidarDeposito(deposito As Deposito, requiereSucursal As Boolean) As ResultadoOperacion
        If requiereSucursal AndAlso deposito.SucursalId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar una sucursal.")
        If String.IsNullOrWhiteSpace(deposito.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If deposito.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(deposito.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If deposito.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")

        Dim desc = If(deposito.Descripcion, String.Empty).Trim()
        If desc.Length > LongitudMaxDescripcion Then Return ResultadoOperacion.Fallo("La descripción supera la longitud permitida.")

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarOpcional(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Return valor.Trim()
    End Function
End Class
