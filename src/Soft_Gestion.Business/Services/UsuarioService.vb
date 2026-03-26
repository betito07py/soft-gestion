Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de usuarios y autenticación. La auditoría de acceso se extiende con <see cref="RegistrarAuditoriaAcceso"/>.
''' </summary>
Public Class UsuarioService
    Inherits ServicioBase

    Private Const LongitudMaxLogin As Integer = 50
    Private Const LongitudMaxNombre As Integer = 150
    Private Const LongitudMaxEmail As Integer = 150
    Private Const LongitudMinimaPassword As Integer = 6

    Private ReadOnly _repositorio As UsuarioRepository
    Private ReadOnly _sucursales As SucursalRepository

    Public Sub New(Optional repositorio As UsuarioRepository = Nothing,
                   Optional sucursales As SucursalRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New UsuarioRepository())
        _sucursales = If(sucursales, New SucursalRepository())
    End Sub

    Public Function ListarUsuarios(filtroTexto As String) As List(Of UsuarioResumen)
        Return _repositorio.Listar(filtroTexto)
    End Function

    Public Function ListarSucursalesActivasParaSelector() As List(Of SucursalSelectorItem)
        Return _sucursales.ListarActivasParaCombo()
    End Function

    ''' <summary>
    ''' Carga usuario para edición. <see cref="Usuario.PasswordHash"/> queda vacío en memoria; no exponer en UI.
    ''' </summary>
    Public Function ObtenerPorIdParaEdicion(usuarioId As Integer) As Usuario
        Dim u = _repositorio.ObtenerPorId(usuarioId)
        If u IsNot Nothing Then
            u.PasswordHash = String.Empty
        End If
        Return u
    End Function

    Public Function ObtenerPorLogin(login As String) As Usuario
        Return _repositorio.ObtenerPorLogin(login)
    End Function

    ''' <summary>
    ''' Alta de usuario. Genera <c>PasswordHash</c> a partir de <paramref name="passwordPlano"/>.
    ''' </summary>
    Public Function GuardarNuevo(usuario As Usuario, passwordPlano As String, usuarioAuditoria As String) As ResultadoOperacion
        If usuario Is Nothing Then Return ResultadoOperacion.Fallo("Datos de usuario no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarCampos(usuario, passwordPlano, requierePassword:=True)
        If Not val.Exitoso Then Return val
        If _repositorio.ExisteLogin(usuario.Login, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un usuario con ese login.")
        End If
        If usuario.SucursalId.HasValue AndAlso Not _sucursales.ExisteActiva(usuario.SucursalId.Value) Then
            Return ResultadoOperacion.Fallo("La sucursal indicada no existe o está inactiva.")
        End If

        Dim nuevo As New Usuario With {
            .Login = usuario.Login.Trim(),
            .NombreCompleto = usuario.NombreCompleto.Trim(),
            .Email = NormalizarEmail(usuario.Email),
            .EsAdministrador = usuario.EsAdministrador,
            .SucursalId = usuario.SucursalId,
            .Activo = True,
            .PasswordHash = VerificadorPasswordPbkdf2.GenerarHash(passwordPlano),
            .UsuarioCreacion = usuarioAuditoria
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el usuario.")
        End Try
    End Function

    ''' <summary>
    ''' Actualiza datos. Si <paramref name="passwordPlanoOpcional"/> no es vacío, regenera el hash.
    ''' </summary>
    Public Function EditarExistente(usuario As Usuario, passwordPlanoOpcional As String, usuarioAuditoria As String) As ResultadoOperacion
        If usuario Is Nothing OrElse usuario.UsuarioId <= 0 Then Return ResultadoOperacion.Fallo("Usuario no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim val = ValidarCampos(usuario, passwordPlanoOpcional, requierePassword:=False)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(usuario.UsuarioId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El usuario no existe.")

        If _repositorio.ExisteLogin(usuario.Login, usuario.UsuarioId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro usuario con ese login.")
        End If
        If usuario.SucursalId.HasValue AndAlso Not _sucursales.ExisteActiva(usuario.SucursalId.Value) Then
            Return ResultadoOperacion.Fallo("La sucursal indicada no existe o está inactiva.")
        End If

        existente.Login = usuario.Login.Trim()
        existente.NombreCompleto = usuario.NombreCompleto.Trim()
        existente.Email = NormalizarEmail(usuario.Email)
        existente.EsAdministrador = usuario.EsAdministrador
        existente.SucursalId = usuario.SucursalId

        Dim pwdTrim = If(passwordPlanoOpcional, String.Empty).Trim()
        If pwdTrim.Length > 0 Then
            existente.PasswordHash = VerificadorPasswordPbkdf2.GenerarHash(pwdTrim)
        End If

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria)
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el usuario.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el usuario.")
        End Try
    End Function

    Public Function Activar(usuarioId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(usuarioId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(usuarioId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(usuarioId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(usuarioId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If usuarioId <= 0 Then Return ResultadoOperacion.Fallo("Usuario no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("Usuario de auditoría no definido.")
        Dim existente = _repositorio.ObtenerPorId(usuarioId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El usuario no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(usuarioId, usuarioAuditoria.Trim()), _repositorio.Desactivar(usuarioId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del usuario.")
        End Try
    End Function

    Private Function ValidarCampos(usuario As Usuario, passwordPlano As String, requierePassword As Boolean) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(usuario.Login) Then Return ResultadoOperacion.Fallo("El login es obligatorio.")
        If usuario.Login.Trim().Length > LongitudMaxLogin Then Return ResultadoOperacion.Fallo("El login supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(usuario.NombreCompleto) Then Return ResultadoOperacion.Fallo("El nombre completo es obligatorio.")
        If usuario.NombreCompleto.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre completo supera la longitud permitida.")

        Dim emailNorm = NormalizarEmail(usuario.Email)
        If emailNorm IsNot Nothing Then
            If emailNorm.Length > LongitudMaxEmail Then Return ResultadoOperacion.Fallo("El email supera la longitud permitida.")
            If Not emailNorm.Contains("@") Then Return ResultadoOperacion.Fallo("El email no tiene un formato válido.")
        End If

        Dim pwd = If(passwordPlano, String.Empty)
        If requierePassword Then
            If pwd.Trim().Length < LongitudMinimaPassword Then
                Return ResultadoOperacion.Fallo("La contraseña debe tener al menos " & LongitudMinimaPassword.ToString() & " caracteres.")
            End If
        Else
            If pwd.Trim().Length > 0 AndAlso pwd.Trim().Length < LongitudMinimaPassword Then
                Return ResultadoOperacion.Fallo("La contraseña debe tener al menos " & LongitudMinimaPassword.ToString() & " caracteres.")
            End If
        End If

        Return ResultadoOperacion.Ok()
    End Function

    Private Shared Function NormalizarEmail(email As String) As String
        If String.IsNullOrWhiteSpace(email) Then Return Nothing
        Return email.Trim()
    End Function

    ''' <summary>
    ''' Valida login, usuario activo y contraseña frente a <c>PasswordHash</c>.
    ''' </summary>
    Public Function ValidarCredenciales(login As String, passwordPlano As String) As ResultadoAutenticacion
        Dim ahora As DateTime = DateTime.Now
        Dim equipo As String = Environment.MachineName

        If String.IsNullOrWhiteSpace(login) OrElse passwordPlano Is Nothing Then
            Auditar(Nothing, If(login, String.Empty).Trim(), ahora, equipo, False, Nothing)
            Return New ResultadoAutenticacion With {.Exitoso = False, .Mensaje = "Datos de acceso incompletos."}
        End If

        Dim loginTrim As String = login.Trim()
        Dim usuario As Usuario = _repositorio.ObtenerPorLogin(loginTrim)

        If usuario Is Nothing Then
            Auditar(Nothing, loginTrim, ahora, equipo, False, Nothing)
            Return New ResultadoAutenticacion With {.Exitoso = False, .Mensaje = "Usuario o contraseña incorrectos."}
        End If

        If Not usuario.Activo Then
            Auditar(usuario.UsuarioId, loginTrim, ahora, equipo, False, Nothing)
            Return New ResultadoAutenticacion With {.Exitoso = False, .Mensaje = "El usuario está inactivo."}
        End If

        If Not VerificadorPasswordPbkdf2.Verificar(passwordPlano, usuario.PasswordHash) Then
            Auditar(usuario.UsuarioId, loginTrim, ahora, equipo, False, Nothing)
            Return New ResultadoAutenticacion With {.Exitoso = False, .Mensaje = "Usuario o contraseña incorrectos."}
        End If

        Auditar(usuario.UsuarioId, loginTrim, ahora, equipo, True, Nothing)
        Return New ResultadoAutenticacion With {.Exitoso = True, .Usuario = usuario, .Mensaje = Nothing}
    End Function

    Private Sub Auditar(usuarioId As Integer?, loginIngresado As String, fechaHora As DateTime, equipo As String, exitoso As Boolean, observacion As String)
        RegistrarAuditoriaAcceso(New DatosAuditoriaAccesoIntento With {
            .UsuarioId = usuarioId,
            .LoginIngresado = loginIngresado,
            .FechaHora = fechaHora,
            .Equipo = equipo,
            .IpLocal = Nothing,
            .Exitoso = exitoso,
            .Observacion = observacion
        })
    End Sub

    ''' <summary>
    ''' Punto de extensión para persistir en <c>AuditoriaAcceso</c>. Por defecto no hace nada.
    ''' </summary>
    Protected Overridable Sub RegistrarAuditoriaAcceso(datos As DatosAuditoriaAccesoIntento)
    End Sub
End Class
