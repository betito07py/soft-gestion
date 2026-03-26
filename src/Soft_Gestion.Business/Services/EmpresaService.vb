Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para empresas.
''' </summary>
Public Class EmpresaService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxRazonSocial As Integer = 150
    Private Const LongitudMaxNombreFantasia As Integer = 150
    Private Const LongitudMaxRuc As Integer = 30
    Private Const LongitudMaxDireccion As Integer = 200
    Private Const LongitudMaxTelefono As Integer = 50
    Private Const LongitudMaxEmail As Integer = 150

    Private ReadOnly _repositorio As EmpresaRepository

    Public Sub New(Optional repositorio As EmpresaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New EmpresaRepository())
    End Sub

    Public Function ListarEmpresas(filtroTexto As String) As List(Of EmpresaResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ObtenerPorId(empresaId As Integer) As Empresa
        Return _repositorio.ObtenerPorId(empresaId)
    End Function

    Public Function GuardarNuevo(empresa As Empresa, usuarioAuditoria As String) As ResultadoOperacion
        If empresa Is Nothing Then Return ResultadoOperacion.Fallo("Datos de empresa no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        empresa.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(empresa.Codigo)
        Dim val = ValidarEmpresa(empresa)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteCodigo(empresa.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe una empresa con ese código.")
        End If

        Dim nuevo As New Empresa With {
            .Codigo = empresa.Codigo,
            .RazonSocial = empresa.RazonSocial.Trim(),
            .NombreFantasia = NormalizarOpcional(empresa.NombreFantasia),
            .RUC = NormalizarOpcional(empresa.RUC),
            .Direccion = NormalizarOpcional(empresa.Direccion),
            .Telefono = NormalizarOpcional(empresa.Telefono),
            .Email = NormalizarOpcional(empresa.Email),
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar la empresa.")
        End Try
    End Function

    Public Function EditarExistente(empresa As Empresa, usuarioAuditoria As String) As ResultadoOperacion
        If empresa Is Nothing OrElse empresa.EmpresaId <= 0 Then Return ResultadoOperacion.Fallo("Empresa no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        empresa.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(empresa.Codigo)
        Dim val = ValidarEmpresa(empresa)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(empresa.EmpresaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La empresa no existe.")
        If _repositorio.ExisteCodigo(empresa.Codigo, empresa.EmpresaId) Then
            Return ResultadoOperacion.Fallo("Ya existe otra empresa con ese código.")
        End If

        existente.Codigo = empresa.Codigo
        existente.RazonSocial = empresa.RazonSocial.Trim()
        existente.NombreFantasia = NormalizarOpcional(empresa.NombreFantasia)
        existente.RUC = NormalizarOpcional(empresa.RUC)
        existente.Direccion = NormalizarOpcional(empresa.Direccion)
        existente.Telefono = NormalizarOpcional(empresa.Telefono)
        existente.Email = NormalizarOpcional(empresa.Email)

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar la empresa.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar la empresa.")
        End Try
    End Function

    Public Function Activar(empresaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(empresaId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(empresaId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(empresaId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(empresaId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If empresaId <= 0 Then Return ResultadoOperacion.Fallo("Empresa no válida.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(empresaId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("La empresa no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(empresaId, usuarioAuditoria.Trim()), _repositorio.Desactivar(empresaId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado de la empresa.")
        End Try
    End Function

    Private Function ValidarEmpresa(empresa As Empresa) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(empresa.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If empresa.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(empresa.RazonSocial) Then Return ResultadoOperacion.Fallo("La razón social es obligatoria.")
        If empresa.RazonSocial.Trim().Length > LongitudMaxRazonSocial Then Return ResultadoOperacion.Fallo("La razón social supera la longitud permitida.")

        Dim nf = If(empresa.NombreFantasia, String.Empty).Trim()
        If nf.Length > LongitudMaxNombreFantasia Then Return ResultadoOperacion.Fallo("El nombre de fantasía supera la longitud permitida.")

        Dim ruc = If(empresa.RUC, String.Empty).Trim()
        If ruc.Length > LongitudMaxRuc Then Return ResultadoOperacion.Fallo("El RUC/documento supera la longitud permitida.")

        Dim dir = If(empresa.Direccion, String.Empty).Trim()
        If dir.Length > LongitudMaxDireccion Then Return ResultadoOperacion.Fallo("La dirección supera la longitud permitida.")

        Dim tel = If(empresa.Telefono, String.Empty).Trim()
        If tel.Length > LongitudMaxTelefono Then Return ResultadoOperacion.Fallo("El teléfono supera la longitud permitida.")

        Dim em = If(empresa.Email, String.Empty).Trim()
        If em.Length > LongitudMaxEmail Then Return ResultadoOperacion.Fallo("El correo supera la longitud permitida.")
        If em.Length > 0 AndAlso Not em.Contains("@"c) Then Return ResultadoOperacion.Fallo("El formato del correo no es válido.")

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarOpcional(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Return valor.Trim()
    End Function
End Class
