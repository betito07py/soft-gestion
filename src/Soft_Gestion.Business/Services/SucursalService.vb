Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para sucursales.
''' </summary>
Public Class SucursalService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100
    Private Const LongitudMaxDireccion As Integer = 200
    Private Const LongitudMaxTelefono As Integer = 50
    Private Const LongitudMaxResponsable As Integer = 100

    Private ReadOnly _repositorio As SucursalRepository
    Private ReadOnly _empresas As EmpresaRepository

    Public Sub New(Optional repositorio As SucursalRepository = Nothing,
                   Optional empresas As EmpresaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New SucursalRepository())
        _empresas = If(empresas, New EmpresaRepository())
    End Sub

    Public Function ListarSucursales(filtroTexto As String, empresaIdFiltro As Integer?) As List(Of SucursalResumen)
        Dim emp = If(empresaIdFiltro.HasValue AndAlso empresaIdFiltro.Value > 0, empresaIdFiltro, Nothing)
        Return _repositorio.Listar(filtroTexto, emp)
    End Function

    Public Function ListarEmpresasActivasParaSelector() As List(Of EmpresaSelectorItem)
        Return _empresas.ListarActivasParaCombo()
    End Function

    ''' <summary>
    ''' Para edición cuando la empresa de la sucursal no está en el combo de activas (p. ej. empresa desactivada).
    ''' </summary>
    Public Function ObtenerItemEmpresaParaEdicion(empresaId As Integer) As EmpresaSelectorItem
        If empresaId <= 0 Then Return Nothing
        Dim e = _empresas.ObtenerPorId(empresaId)
        If e Is Nothing Then Return Nothing
        Return New EmpresaSelectorItem With {
            .EmpresaId = e.EmpresaId,
            .Texto = e.Codigo & " — " & e.RazonSocial
        }
    End Function

    Public Function ObtenerPorId(sucursalId As Integer) As Sucursal
        Return _repositorio.ObtenerPorId(sucursalId)
    End Function

    Public Function GuardarNuevo(sucursal As Sucursal, usuarioAuditoria As String) As ResultadoOperacion
        If sucursal Is Nothing Then Return ResultadoOperacion.Fallo("Datos de sucursal no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        sucursal.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(sucursal.Codigo)
        Dim val = ValidarSucursal(sucursal, requiereEmpresa:=True)
        If Not val.Exitoso Then Return val
        If Not _empresas.ExisteActiva(sucursal.EmpresaId) Then
            Return ResultadoOperacion.Fallo("La empresa no existe o está inactiva.")
        End If
        If _repositorio.ExisteCodigo(sucursal.EmpresaId, sucursal.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una sucursal con ese código en la empresa seleccionada.")
        End If

        Dim nuevo As New Sucursal With {
            .EmpresaId = sucursal.EmpresaId,
            .Codigo = sucursal.Codigo,
            .Nombre = sucursal.Nombre.Trim(),
            .Direccion = NormalizarOpcional(sucursal.Direccion),
            .Telefono = NormalizarOpcional(sucursal.Telefono),
            .Responsable = NormalizarOpcional(sucursal.Responsable),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la sucursal.")
        End Try
    End Function

    Public Function EditarExistente(sucursal As Sucursal, usuarioAuditoria As String) As ResultadoOperacion
        If sucursal Is Nothing OrElse sucursal.SucursalId <= 0 Then Return ResultadoOperacion.Fallo("Sucursal no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        sucursal.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(sucursal.Codigo)
        Dim val = ValidarSucursal(sucursal, requiereEmpresa:=True)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(sucursal.SucursalId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La sucursal no existe.")
        If Not _empresas.ExisteActiva(sucursal.EmpresaId) Then
            Return ResultadoOperacion.Fallo("La empresa no existe o está inactiva.")
        End If
        If _repositorio.ExisteCodigo(sucursal.EmpresaId, sucursal.Codigo, sucursal.SucursalId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra sucursal con ese código en la empresa seleccionada.")
        End If

        existente.EmpresaId = sucursal.EmpresaId
        existente.Codigo = sucursal.Codigo
        existente.Nombre = sucursal.Nombre.Trim()
        existente.Direccion = NormalizarOpcional(sucursal.Direccion)
        existente.Telefono = NormalizarOpcional(sucursal.Telefono)
        existente.Responsable = NormalizarOpcional(sucursal.Responsable)

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar la sucursal.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar la sucursal.")
        End Try
    End Function

    Public Function Activar(sucursalId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(sucursalId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(sucursalId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(sucursalId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(sucursalId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If sucursalId <= 0 Then Return ResultadoOperacion.Fallo("Sucursal no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(sucursalId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La sucursal no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(sucursalId, usuarioAuditoria.Trim()), _repositorio.Desactivar(sucursalId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado de la sucursal.")
        End Try
    End Function

    Private Function ValidarSucursal(sucursal As Sucursal, requiereEmpresa As Boolean) As ResultadoOperacion
        If requiereEmpresa AndAlso sucursal.EmpresaId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar una empresa.")
        If String.IsNullOrWhiteSpace(sucursal.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If sucursal.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(sucursal.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If sucursal.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")

        Dim dir = If(sucursal.Direccion, String.Empty).Trim()
        If dir.Length > LongitudMaxDireccion Then Return ResultadoOperacion.Fallo("La dirección supera la longitud permitida.")

        Dim tel = If(sucursal.Telefono, String.Empty).Trim()
        If tel.Length > LongitudMaxTelefono Then Return ResultadoOperacion.Fallo("El teléfono supera la longitud permitida.")

        Dim resp = If(sucursal.Responsable, String.Empty).Trim()
        If resp.Length > LongitudMaxResponsable Then Return ResultadoOperacion.Fallo("El responsable supera la longitud permitida.")

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarOpcional(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Return valor.Trim()
    End Function
End Class
