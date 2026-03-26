Imports System.Collections.Generic
Imports Soft_Gestion.Common
Imports Soft_Gestion.Data
Imports Soft_Gestion.Domain

''' <summary>
''' Reglas de negocio para grupos de producto (tercer nivel jerárquico).
''' Valida explícitamente que la subcategoría corresponda a la categoría (además de la FK en SQL),
''' para mostrar errores entendibles antes de un fallo por restricción en base de datos.
''' </summary>
Public Class GrupoService
    Inherits ServicioBase

    Private Const LongitudMaxCodigo As Integer = 20
    Private Const LongitudMaxNombre As Integer = 100

    Private ReadOnly _repositorio As GrupoRepository
    Private ReadOnly _categorias As CategoriaRepository
    Private ReadOnly _subcategorias As SubCategoriaRepository

    Public Sub New(Optional repositorio As GrupoRepository = Nothing,
                   Optional categorias As CategoriaRepository = Nothing,
                   Optional subcategorias As SubCategoriaRepository = Nothing)
        MyBase.New()
        _repositorio = If(repositorio, New GrupoRepository())
        _categorias = If(categorias, New CategoriaRepository())
        _subcategorias = If(subcategorias, New SubCategoriaRepository())
    End Sub

    Public Function ListarGrupos(filtroTexto As String, categoriaId As Integer?, subCategoriaId As Integer?) As List(Of GrupoResumen)
        Return _repositorio.Listar(filtroTexto, categoriaId, subCategoriaId)
    End Function

    Public Function ObtenerPorId(grupoId As Integer) As Grupo
        Return _repositorio.ObtenerPorId(grupoId)
    End Function

    ''' <summary>Código sugerido para un grupo nuevo bajo la subcategoría indicada.</summary>
    Public Function ObtenerCodigoSugeridoParaNuevoGrupo(subCategoriaId As Integer) As String
        If subCategoriaId <= 0 Then Return String.Empty
        Return _repositorio.ObtenerSiguienteCodigoNumericoSugerido(subCategoriaId).ToString()
    End Function

    Public Function ListarCategoriasActivasParaSelector() As List(Of CategoriaSelectorItem)
        Return _categorias.ListarActivasParaCombo()
    End Function

    Public Function ListarSubCategoriasActivasParaSelector(categoriaId As Integer?) As List(Of SubCategoriaSelectorItem)
        Return _subcategorias.ListarActivasParaCombo(categoriaId)
    End Function

    ''' <summary>
    ''' Incluye categoría inactiva si el grupo ya la tenía asignada y no está en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemCategoriaParaEdicion(categoriaId As Integer?) As CategoriaSelectorItem
        If Not categoriaId.HasValue OrElse categoriaId.Value <= 0 Then Return Nothing
        Dim c = _categorias.ObtenerPorId(categoriaId.Value)
        If c Is Nothing Then Return Nothing
        Return New CategoriaSelectorItem With {
            .CategoriaId = c.CategoriaId,
            .Texto = c.Codigo & " — " & c.Nombre
        }
    End Function

    ''' <summary>
    ''' Incluye subcategoría inactiva si el grupo ya la tenía asignada y no está en el combo de activas.
    ''' </summary>
    Public Function ObtenerItemSubCategoriaParaEdicion(subCategoriaId As Integer?) As SubCategoriaSelectorItem
        If Not subCategoriaId.HasValue OrElse subCategoriaId.Value <= 0 Then Return Nothing
        Dim s = _subcategorias.ObtenerPorId(subCategoriaId.Value)
        If s Is Nothing Then Return Nothing
        Return New SubCategoriaSelectorItem With {
            .SubCategoriaId = s.SubCategoriaId,
            .Texto = s.Codigo & " — " & s.Nombre
        }
    End Function

    Public Function GuardarNuevo(grupo As Grupo, usuarioAuditoria As String) As ResultadoOperacion
        If grupo Is Nothing Then Return ResultadoOperacion.Fallo("Datos de grupo no válidos.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        grupo.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(grupo.Codigo)
        Dim val = ValidarGrupo(grupo)
        If Not val.Exitoso Then Return val
        Dim coh = ValidarCoherenciaYEstados(grupo)
        If Not coh.Exitoso Then Return coh
        If _repositorio.ExisteCodigo(grupo.SubCategoriaId, grupo.Codigo, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un grupo con ese código en la subcategoría seleccionada.")
        End If
        If _repositorio.ExisteNombre(grupo.SubCategoriaId, grupo.Nombre, Nothing) Then
            Return ResultadoOperacion.Fallo("Ya existe un grupo con ese nombre en la subcategoría seleccionada.")
        End If

        Dim nuevo As New Grupo With {
            .Codigo = grupo.Codigo,
            .Nombre = grupo.Nombre.Trim(),
            .CategoriaId = grupo.CategoriaId,
            .SubCategoriaId = grupo.SubCategoriaId,
            .Activo = True,
            .UsuarioCreacion = usuarioAuditoria.Trim()
        }
        Try
            Dim id = _repositorio.Insertar(nuevo)
            Return ResultadoOperacion.Ok(id)
        Catch
            Return ResultadoOperacion.Fallo("No se pudo guardar el grupo.")
        End Try
    End Function

    Public Function EditarExistente(grupo As Grupo, usuarioAuditoria As String) As ResultadoOperacion
        If grupo Is Nothing OrElse grupo.GrupoId <= 0 Then Return ResultadoOperacion.Fallo("Grupo no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        grupo.Codigo = CodigoNegocio.NormalizarSinCerosIzquierda(grupo.Codigo)
        Dim val = ValidarGrupo(grupo)
        If Not val.Exitoso Then Return val

        Dim existente = _repositorio.ObtenerPorId(grupo.GrupoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El grupo no existe.")

        Dim coh = ValidarCoherenciaYEstados(grupo, existente)
        If Not coh.Exitoso Then Return coh
        If _repositorio.ExisteCodigo(grupo.SubCategoriaId, grupo.Codigo, grupo.GrupoId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro grupo con ese código en la subcategoría seleccionada.")
        End If
        If _repositorio.ExisteNombre(grupo.SubCategoriaId, grupo.Nombre, grupo.GrupoId) Then
            Return ResultadoOperacion.Fallo("Ya existe otro grupo con ese nombre en la subcategoría seleccionada.")
        End If

        existente.Codigo = grupo.Codigo
        existente.Nombre = grupo.Nombre.Trim()
        existente.CategoriaId = grupo.CategoriaId
        existente.SubCategoriaId = grupo.SubCategoriaId

        Try
            Dim filas = _repositorio.Actualizar(existente, usuarioAuditoria.Trim())
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el grupo.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el grupo.")
        End Try
    End Function

    Public Function Activar(grupoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(grupoId, True, usuarioAuditoria)
    End Function

    Public Function Desactivar(grupoId As Integer, usuarioAuditoria As String) As ResultadoOperacion
        Return EstablecerActividad(grupoId, False, usuarioAuditoria)
    End Function

    Private Function EstablecerActividad(grupoId As Integer, activo As Boolean, usuarioAuditoria As String) As ResultadoOperacion
        If grupoId <= 0 Then Return ResultadoOperacion.Fallo("Grupo no válido.")
        If String.IsNullOrWhiteSpace(usuarioAuditoria) Then Return ResultadoOperacion.Fallo("No se pudo determinar el usuario de auditoría.")
        Dim existente = _repositorio.ObtenerPorId(grupoId)
        If existente Is Nothing Then Return ResultadoOperacion.Fallo("El grupo no existe.")
        Try
            Dim filas = If(activo, _repositorio.Activar(grupoId, usuarioAuditoria.Trim()), _repositorio.Desactivar(grupoId, usuarioAuditoria.Trim()))
            If filas = 0 Then Return ResultadoOperacion.Fallo("No se pudo actualizar el estado.")
            Return ResultadoOperacion.Ok()
        Catch
            Return ResultadoOperacion.Fallo("No se pudo actualizar el estado del grupo.")
        End Try
    End Function

    Private Function ValidarGrupo(grupo As Grupo) As ResultadoOperacion
        If String.IsNullOrWhiteSpace(grupo.Codigo) Then Return ResultadoOperacion.Fallo("El código es obligatorio.")
        If grupo.Codigo.Length > LongitudMaxCodigo Then Return ResultadoOperacion.Fallo("El código supera la longitud permitida.")
        If String.IsNullOrWhiteSpace(grupo.Nombre) Then Return ResultadoOperacion.Fallo("El nombre es obligatorio.")
        If grupo.Nombre.Trim().Length > LongitudMaxNombre Then Return ResultadoOperacion.Fallo("El nombre supera la longitud permitida.")
        If grupo.CategoriaId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar una categoría.")
        If grupo.SubCategoriaId <= 0 Then Return ResultadoOperacion.Fallo("Debe seleccionar una subcategoría.")
        Return ResultadoOperacion.Ok()
    End Function

    ''' <summary>
    ''' Coherencia entre categoría y subcategoría. No alcanza con la FK compuesta en SQL
    ''' (<c>SubCategorias(SubCategoriaId, CategoriaId)</c>): aquí se valida en negocio para devolver
    ''' un mensaje claro antes de un error de restricción en base de datos.
    ''' </summary>
    ''' <param name="existente">Nothing en altas; en edición, el registro previo para permitir categoría/subcategoría inactivas si no se cambian.</param>
    Private Function ValidarCoherenciaYEstados(grupo As Grupo, Optional existente As Grupo = Nothing) As ResultadoOperacion
        If Not _subcategorias.PerteneceACategoria(grupo.SubCategoriaId, grupo.CategoriaId) Then
            Return ResultadoOperacion.Fallo(
                "La subcategoría elegida no corresponde a la categoría seleccionada. " &
                "Elija una subcategoría que pertenezca a esa categoría (o cambie primero la categoría).")
        End If

        Dim esNuevo = existente Is Nothing
        Dim catCambio = esNuevo OrElse grupo.CategoriaId <> existente.CategoriaId
        Dim subCambio = esNuevo OrElse grupo.SubCategoriaId <> existente.SubCategoriaId

        If catCambio Then
            If Not _categorias.ExisteActiva(grupo.CategoriaId) Then
                Return ResultadoOperacion.Fallo("La categoría no existe o está inactiva.")
            End If
        Else
            If _categorias.ObtenerPorId(grupo.CategoriaId) Is Nothing Then
                Return ResultadoOperacion.Fallo("La categoría no existe.")
            End If
        End If

        If subCambio Then
            If Not _subcategorias.ExisteActiva(grupo.SubCategoriaId) Then
                Return ResultadoOperacion.Fallo("La subcategoría no existe o está inactiva.")
            End If
        Else
            If _subcategorias.ObtenerPorId(grupo.SubCategoriaId) Is Nothing Then
                Return ResultadoOperacion.Fallo("La subcategoría no existe.")
            End If
        End If

        Return ResultadoOperacion.Ok()
    End Function
End Class
