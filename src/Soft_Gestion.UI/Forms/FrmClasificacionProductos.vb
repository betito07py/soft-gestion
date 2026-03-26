Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' Clasificación de productos en cascada: Categoría → Subcategoría → Grupo (código, descripción, Agregar / Editar / Guardar).
''' Requiere permisos de los tres maestros.
''' </summary>
Public Partial Class FrmClasificacionProductos

    Private Enum ModoEdicionClasificacion
        Ninguno
        AgregarCascada
        EditarCategoria
        EditarSubcategoria
        EditarGrupo
    End Enum

    Private ReadOnly _svcCategoria As New CategoriaService()
    Private ReadOnly _svcSub As New SubCategoriaService()
    Private ReadOnly _svcGrupo As New GrupoService()

    Private _categoriaEdicionId As Integer
    Private _subEdicionId As Integer
    Private _grupoEdicionId As Integer

    Private _suspendSelCat As Boolean
    Private _suspendSelSub As Boolean
    Private _suspendSelGru As Boolean

    Private _modoEdicion As ModoEdicionClasificacion = ModoEdicionClasificacion.Ninguno

    Private Sub FrmClasificacionProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not UsuarioPuedeUsarEstaPantalla() Then
            MessageBox.Show("Se requieren los permisos de Categorías, Subcategorías y Grupos para usar esta pantalla.",
                            Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            BeginInvoke(New MethodInvoker(AddressOf Close))
            Return
        End If
        ConfigurarGrillas()
        LimpiarEdicionCategoria()
        LimpiarEdicionSub()
        LimpiarEdicionGrupo()
        RefrescarCategorias()
        RefrescarSubcategorias()
        RefrescarGrupos()
        DeshabilitarPanelesEdicion()
    End Sub

    ''' <summary>Bloque inferior: sin edición hasta Agregar (todo) o Editar (una columna).</summary>
    Private Sub DeshabilitarPanelesEdicion()
        grpEdicionCategoria.Enabled = False
        grpEdicionSub.Enabled = False
        grpEdicionGrupo.Enabled = False
        btnGuardarCategoria.Enabled = False
        btnGuardarSub.Enabled = False
        btnGuardarGrupo.Enabled = False
        _modoEdicion = ModoEdicionClasificacion.Ninguno
        ActualizarBotonCancelarEdicion()
    End Sub

    Private Sub HabilitarTodasLasSeccionesEdicion()
        grpEdicionCategoria.Enabled = True
        grpEdicionSub.Enabled = True
        grpEdicionGrupo.Enabled = True
        btnGuardarCategoria.Enabled = True
        btnGuardarSub.Enabled = True
        btnGuardarGrupo.Enabled = True
        _modoEdicion = ModoEdicionClasificacion.AgregarCascada
        ActualizarBotonCancelarEdicion()
    End Sub

    Private Sub HabilitarSoloEdicionCategoria()
        grpEdicionCategoria.Enabled = True
        grpEdicionSub.Enabled = False
        grpEdicionGrupo.Enabled = False
        btnGuardarCategoria.Enabled = True
        btnGuardarSub.Enabled = False
        btnGuardarGrupo.Enabled = False
        _modoEdicion = ModoEdicionClasificacion.EditarCategoria
        ActualizarBotonCancelarEdicion()
    End Sub

    Private Sub HabilitarSoloEdicionSubcategoria()
        grpEdicionCategoria.Enabled = False
        grpEdicionSub.Enabled = True
        grpEdicionGrupo.Enabled = False
        btnGuardarCategoria.Enabled = False
        btnGuardarSub.Enabled = True
        btnGuardarGrupo.Enabled = False
        _modoEdicion = ModoEdicionClasificacion.EditarSubcategoria
        ActualizarBotonCancelarEdicion()
    End Sub

    Private Sub HabilitarSoloEdicionGrupo()
        grpEdicionCategoria.Enabled = False
        grpEdicionSub.Enabled = False
        grpEdicionGrupo.Enabled = True
        btnGuardarCategoria.Enabled = False
        btnGuardarSub.Enabled = False
        btnGuardarGrupo.Enabled = True
        _modoEdicion = ModoEdicionClasificacion.EditarGrupo
        ActualizarBotonCancelarEdicion()
    End Sub

    Private Sub ActualizarBotonCancelarEdicion()
        btnCancelarEdicion.Enabled = _modoEdicion <> ModoEdicionClasificacion.Ninguno
    End Sub

    Private Sub btnCancelarEdicion_Click(sender As Object, e As EventArgs) Handles btnCancelarEdicion.Click
        Select Case _modoEdicion
            Case ModoEdicionClasificacion.AgregarCascada
                LimpiarEdicionCategoria()
                LimpiarEdicionSub()
                LimpiarEdicionGrupo()
                DeshabilitarPanelesEdicion()
            Case ModoEdicionClasificacion.EditarCategoria
                LimpiarEdicionCategoria()
                grpEdicionCategoria.Enabled = False
                btnGuardarCategoria.Enabled = False
                _modoEdicion = ModoEdicionClasificacion.Ninguno
                ActualizarBotonCancelarEdicion()
            Case ModoEdicionClasificacion.EditarSubcategoria
                LimpiarEdicionSub()
                grpEdicionSub.Enabled = False
                btnGuardarSub.Enabled = False
                _modoEdicion = ModoEdicionClasificacion.Ninguno
                ActualizarBotonCancelarEdicion()
            Case ModoEdicionClasificacion.EditarGrupo
                LimpiarEdicionGrupo()
                grpEdicionGrupo.Enabled = False
                btnGuardarGrupo.Enabled = False
                _modoEdicion = ModoEdicionClasificacion.Ninguno
                ActualizarBotonCancelarEdicion()
        End Select
    End Sub

    Private Shared Function UsuarioPuedeUsarEstaPantalla() As Boolean
        Return SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosCategorias) AndAlso
            SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosSubCategorias) AndAlso
            SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosGrupos)
    End Function

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

#Region "Grillas"
    Private Sub ConfigurarGrillas()
        ConfigurarGrilla(dgvCategorias, mostrarCatPadre:=False, mostrarSubPadre:=False)
        ConfigurarGrilla(dgvSubcategorias, mostrarCatPadre:=True, mostrarSubPadre:=False)
        ConfigurarGrilla(dgvGrupos, mostrarCatPadre:=True, mostrarSubPadre:=True)
    End Sub

    Private Shared Sub ConfigurarGrilla(dgv As DataGridView, mostrarCatPadre As Boolean, mostrarSubPadre As Boolean)
        dgv.AutoGenerateColumns = False
        dgv.Columns.Clear()
        If mostrarCatPadre Then
            dgv.Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "CategoriaCodigo", .HeaderText = "Cat.", .Name = "colCat", .MinimumWidth = 36, .Width = 42})
        End If
        If mostrarSubPadre Then
            dgv.Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "SubCategoriaCodigo", .HeaderText = "Sub.", .Name = "colSub", .MinimumWidth = 36, .Width = 42})
        End If
        dgv.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCod", .MinimumWidth = 56, .FillWeight = 60})
        dgv.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Descripción", .Name = "colNom", .MinimumWidth = 80, .FillWeight = 100})
        dgv.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colAct", .Width = 48})
    End Sub

    Private Function ObtenerCategoriaIdSeleccionada() As Integer
        If dgvCategorias.CurrentRow Is Nothing Then Return 0
        Dim r = TryCast(dgvCategorias.CurrentRow.DataBoundItem, CategoriaResumen)
        If r Is Nothing Then Return 0
        Return r.CategoriaId
    End Function

    Private Function ObtenerSubCategoriaIdSeleccionada() As Integer
        If dgvSubcategorias.CurrentRow Is Nothing Then Return 0
        Dim r = TryCast(dgvSubcategorias.CurrentRow.DataBoundItem, SubCategoriaResumen)
        If r Is Nothing Then Return 0
        Return r.SubCategoriaId
    End Function

    Private Sub RefrescarCategorias()
        Try
            _suspendSelCat = True
            Dim lista = _svcCategoria.ListarCategorias(txtFiltroCategoria.Text)
            dgvCategorias.DataSource = Nothing
            dgvCategorias.DataSource = lista
        Catch ex As Exception
            MessageBox.Show("No se pudieron cargar las categorías." & Environment.NewLine & ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSelCat = False
        End Try
    End Sub

    Private Sub RefrescarSubcategorias()
        Try
            _suspendSelSub = True
            Dim cid = ObtenerCategoriaIdSeleccionada()
            Dim lista As List(Of SubCategoriaResumen)
            If cid <= 0 Then
                lista = New List(Of SubCategoriaResumen)()
            Else
                lista = _svcSub.ListarSubCategorias(txtFiltroSub.Text, cid)
            End If
            dgvSubcategorias.DataSource = Nothing
            dgvSubcategorias.DataSource = lista
        Catch ex As Exception
            MessageBox.Show("No se pudieron cargar las subcategorías." & Environment.NewLine & ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSelSub = False
        End Try
    End Sub

    Private Sub RefrescarGrupos()
        Try
            _suspendSelGru = True
            Dim sid = ObtenerSubCategoriaIdSeleccionada()
            Dim lista As List(Of GrupoResumen)
            If sid <= 0 Then
                lista = New List(Of GrupoResumen)()
            Else
                Dim s = _svcSub.ObtenerPorId(sid)
                If s Is Nothing Then
                    lista = New List(Of GrupoResumen)()
                Else
                    lista = _svcGrupo.ListarGrupos(txtFiltroGrupo.Text, s.CategoriaId, sid)
                End If
            End If
            dgvGrupos.DataSource = Nothing
            dgvGrupos.DataSource = lista
        Catch ex As Exception
            MessageBox.Show("No se pudieron cargar los grupos." & Environment.NewLine & ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSelGru = False
        End Try
    End Sub

    Private Sub dgvCategorias_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCategorias.SelectionChanged
        If _suspendSelCat Then Return
        DeshabilitarPanelesEdicion()
        LimpiarEdicionCategoria()
        LimpiarEdicionSub()
        LimpiarEdicionGrupo()
        RefrescarSubcategorias()
        RefrescarGrupos()
    End Sub

    Private Sub dgvSubcategorias_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubcategorias.SelectionChanged
        If _suspendSelSub Then Return
        DeshabilitarPanelesEdicion()
        LimpiarEdicionSub()
        LimpiarEdicionGrupo()
        RefrescarGrupos()
    End Sub

    Private Sub dgvGrupos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvGrupos.SelectionChanged
        If _suspendSelGru Then Return
        DeshabilitarPanelesEdicion()
        LimpiarEdicionGrupo()
    End Sub

    Private Sub txtFiltroCategoria_Leave(sender As Object, e As EventArgs) Handles txtFiltroCategoria.Leave
        RefrescarCategorias()
    End Sub

    Private Sub txtFiltroSub_Leave(sender As Object, e As EventArgs) Handles txtFiltroSub.Leave
        RefrescarSubcategorias()
    End Sub

    Private Sub txtFiltroGrupo_Leave(sender As Object, e As EventArgs) Handles txtFiltroGrupo.Leave
        RefrescarGrupos()
    End Sub
#End Region

#Region "Edición — Categoría"
    Private Sub LimpiarEdicionCategoria()
        _categoriaEdicionId = 0
        txtCodigoCategoria.Clear()
        txtDescripcionCategoria.Clear()
        chkActivoCategoria.Checked = True
    End Sub

    Private Sub btnAgregarCategoria_Click(sender As Object, e As EventArgs) Handles btnAgregarCategoria.Click
        LimpiarEdicionCategoria()
        Try
            txtCodigoCategoria.Text = _svcCategoria.ObtenerCodigoSugeridoParaNuevaCategoria()
        Catch
            txtCodigoCategoria.Clear()
        End Try
        HabilitarTodasLasSeccionesEdicion()
        txtDescripcionCategoria.Focus()
    End Sub

    Private Sub btnEditarCategoria_Click(sender As Object, e As EventArgs) Handles btnEditarCategoria.Click
        Dim r = ObtenerCategoriaResumenSeleccionado()
        If r Is Nothing Then
            MessageBox.Show("Seleccione una categoría en la grilla.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        _categoriaEdicionId = r.CategoriaId
        txtCodigoCategoria.Text = r.Codigo
        txtDescripcionCategoria.Text = r.Nombre
        chkActivoCategoria.Checked = r.Activo
        HabilitarSoloEdicionCategoria()
    End Sub

    Private Function ObtenerCategoriaResumenSeleccionado() As CategoriaResumen
        If dgvCategorias.CurrentRow Is Nothing Then Return Nothing
        Return TryCast(dgvCategorias.CurrentRow.DataBoundItem, CategoriaResumen)
    End Function

    Private Sub btnGuardarCategoria_Click(sender As Object, e As EventArgs) Handles btnGuardarCategoria.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Categoria With {
            .CategoriaId = _categoriaEdicionId,
            .Codigo = txtCodigoCategoria.Text,
            .Nombre = txtDescripcionCategoria.Text
        }
        Dim res As ResultadoOperacion
        If _categoriaEdicionId = 0 Then
            res = _svcCategoria.GuardarNuevo(modelo, aud)
        Else
            res = _svcCategoria.EditarExistente(modelo, aud)
            If res.Exitoso Then
                SincronizarActivoCategoria(aud)
            End If
        End If
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim nuevoId = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _categoriaEdicionId)
        RefrescarCategorias()
        SeleccionarCategoriaPorId(nuevoId)
        LimpiarEdicionCategoria()
        DeshabilitarPanelesEdicion()
        RefrescarSubcategorias()
        RefrescarGrupos()
    End Sub

    Private Sub SincronizarActivoCategoria(aud As String)
        If _categoriaEdicionId <= 0 Then Return
        Dim db = _svcCategoria.ObtenerPorId(_categoriaEdicionId)
        If db Is Nothing Then Return
        If db.Activo = chkActivoCategoria.Checked Then Return
        Dim r = If(chkActivoCategoria.Checked, _svcCategoria.Activar(_categoriaEdicionId, aud), _svcCategoria.Desactivar(_categoriaEdicionId, aud))
        If Not r.Exitoso Then MessageBox.Show(r.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub SeleccionarCategoriaPorId(id As Integer)
        If id <= 0 Then Return
        _suspendSelCat = True
        Try
            For Each row As DataGridViewRow In dgvCategorias.Rows
                Dim it = TryCast(row.DataBoundItem, CategoriaResumen)
                If it IsNot Nothing AndAlso it.CategoriaId = id Then
                    dgvCategorias.ClearSelection()
                    row.Selected = True
                    dgvCategorias.CurrentCell = row.Cells(Math.Min(0, row.Cells.Count - 1))
                    Exit For
                End If
            Next
        Finally
            _suspendSelCat = False
        End Try
    End Sub
#End Region

#Region "Edición — Subcategoría"
    Private Sub LimpiarEdicionSub()
        _subEdicionId = 0
        txtCodigoSub.Clear()
        txtDescripcionSub.Clear()
        chkActivoSub.Checked = True
    End Sub

    Private Sub btnAgregarSub_Click(sender As Object, e As EventArgs) Handles btnAgregarSub.Click
        Dim cid = ObtenerCategoriaIdSeleccionada()
        If cid <= 0 Then
            MessageBox.Show("Seleccione una categoría en la primera columna para dar de alta una subcategoría.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        LimpiarEdicionSub()
        Try
            txtCodigoSub.Text = _svcSub.ObtenerCodigoSugeridoParaNuevaSubCategoria(cid)
        Catch
            txtCodigoSub.Clear()
        End Try
        HabilitarTodasLasSeccionesEdicion()
        txtDescripcionSub.Focus()
    End Sub

    Private Sub btnEditarSub_Click(sender As Object, e As EventArgs) Handles btnEditarSub.Click
        Dim r = ObtenerSubCategoriaResumenSeleccionado()
        If r Is Nothing Then
            MessageBox.Show("Seleccione una subcategoría en la grilla.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        _subEdicionId = r.SubCategoriaId
        txtCodigoSub.Text = r.Codigo
        txtDescripcionSub.Text = r.Nombre
        chkActivoSub.Checked = r.Activo
        HabilitarSoloEdicionSubcategoria()
    End Sub

    Private Function ObtenerSubCategoriaResumenSeleccionado() As SubCategoriaResumen
        If dgvSubcategorias.CurrentRow Is Nothing Then Return Nothing
        Return TryCast(dgvSubcategorias.CurrentRow.DataBoundItem, SubCategoriaResumen)
    End Function

    Private Sub btnGuardarSub_Click(sender As Object, e As EventArgs) Handles btnGuardarSub.Click
        Dim cid = ObtenerCategoriaIdSeleccionada()
        If cid <= 0 Then
            MessageBox.Show("Seleccione una categoría (columna izquierda).", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim aud = LoginAuditoria()
        Dim modelo As New SubCategoria With {
            .SubCategoriaId = _subEdicionId,
            .Codigo = txtCodigoSub.Text,
            .Nombre = txtDescripcionSub.Text,
            .CategoriaId = cid
        }
        Dim res As ResultadoOperacion
        If _subEdicionId = 0 Then
            res = _svcSub.GuardarNuevo(modelo, aud)
        Else
            res = _svcSub.EditarExistente(modelo, aud)
            If res.Exitoso Then SincronizarActivoSub(aud)
        End If
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim nuevoId = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _subEdicionId)
        RefrescarSubcategorias()
        SeleccionarSubPorId(nuevoId)
        LimpiarEdicionSub()
        DeshabilitarPanelesEdicion()
        RefrescarGrupos()
    End Sub

    Private Sub SincronizarActivoSub(aud As String)
        If _subEdicionId <= 0 Then Return
        Dim db = _svcSub.ObtenerPorId(_subEdicionId)
        If db Is Nothing Then Return
        If db.Activo = chkActivoSub.Checked Then Return
        Dim r = If(chkActivoSub.Checked, _svcSub.Activar(_subEdicionId, aud), _svcSub.Desactivar(_subEdicionId, aud))
        If Not r.Exitoso Then MessageBox.Show(r.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub SeleccionarSubPorId(id As Integer)
        If id <= 0 Then Return
        _suspendSelSub = True
        Try
            For Each row As DataGridViewRow In dgvSubcategorias.Rows
                Dim it = TryCast(row.DataBoundItem, SubCategoriaResumen)
                If it IsNot Nothing AndAlso it.SubCategoriaId = id Then
                    dgvSubcategorias.ClearSelection()
                    row.Selected = True
                    dgvSubcategorias.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSelSub = False
        End Try
    End Sub
#End Region

#Region "Edición — Grupo"
    Private Sub LimpiarEdicionGrupo()
        _grupoEdicionId = 0
        txtCodigoGrupo.Clear()
        txtDescripcionGrupo.Clear()
        chkActivoGrupo.Checked = True
    End Sub

    Private Sub btnAgregarGrupo_Click(sender As Object, e As EventArgs) Handles btnAgregarGrupo.Click
        Dim sid = ObtenerSubCategoriaIdSeleccionada()
        If sid <= 0 Then
            MessageBox.Show("Seleccione una subcategoría en la columna central para dar de alta un grupo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        LimpiarEdicionGrupo()
        Try
            txtCodigoGrupo.Text = _svcGrupo.ObtenerCodigoSugeridoParaNuevoGrupo(sid)
        Catch
            txtCodigoGrupo.Clear()
        End Try
        HabilitarTodasLasSeccionesEdicion()
        txtDescripcionGrupo.Focus()
    End Sub

    Private Sub btnEditarGrupo_Click(sender As Object, e As EventArgs) Handles btnEditarGrupo.Click
        Dim r = ObtenerGrupoResumenSeleccionado()
        If r Is Nothing Then
            MessageBox.Show("Seleccione un grupo en la grilla.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        _grupoEdicionId = r.GrupoId
        txtCodigoGrupo.Text = r.Codigo
        txtDescripcionGrupo.Text = r.Nombre
        chkActivoGrupo.Checked = r.Activo
        HabilitarSoloEdicionGrupo()
    End Sub

    Private Function ObtenerGrupoResumenSeleccionado() As GrupoResumen
        If dgvGrupos.CurrentRow Is Nothing Then Return Nothing
        Return TryCast(dgvGrupos.CurrentRow.DataBoundItem, GrupoResumen)
    End Function

    Private Sub btnGuardarGrupo_Click(sender As Object, e As EventArgs) Handles btnGuardarGrupo.Click
        Dim sid = ObtenerSubCategoriaIdSeleccionada()
        If sid <= 0 Then
            MessageBox.Show("Seleccione una subcategoría (columna central).", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim s = _svcSub.ObtenerPorId(sid)
        If s Is Nothing Then
            MessageBox.Show("Subcategoría no válida.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim aud = LoginAuditoria()
        Dim modelo As New Grupo With {
            .GrupoId = _grupoEdicionId,
            .Codigo = txtCodigoGrupo.Text,
            .Nombre = txtDescripcionGrupo.Text,
            .CategoriaId = s.CategoriaId,
            .SubCategoriaId = sid
        }
        Dim res As ResultadoOperacion
        If _grupoEdicionId = 0 Then
            res = _svcGrupo.GuardarNuevo(modelo, aud)
        Else
            res = _svcGrupo.EditarExistente(modelo, aud)
            If res.Exitoso Then SincronizarActivoGrupo(aud)
        End If
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim nuevoId = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _grupoEdicionId)
        RefrescarGrupos()
        SeleccionarGrupoPorId(nuevoId)
        LimpiarEdicionGrupo()
        DeshabilitarPanelesEdicion()
    End Sub

    Private Sub SincronizarActivoGrupo(aud As String)
        If _grupoEdicionId <= 0 Then Return
        Dim db = _svcGrupo.ObtenerPorId(_grupoEdicionId)
        If db Is Nothing Then Return
        If db.Activo = chkActivoGrupo.Checked Then Return
        Dim r = If(chkActivoGrupo.Checked, _svcGrupo.Activar(_grupoEdicionId, aud), _svcGrupo.Desactivar(_grupoEdicionId, aud))
        If Not r.Exitoso Then MessageBox.Show(r.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Private Sub SeleccionarGrupoPorId(id As Integer)
        If id <= 0 Then Return
        _suspendSelGru = True
        Try
            For Each row As DataGridViewRow In dgvGrupos.Rows
                Dim it = TryCast(row.DataBoundItem, GrupoResumen)
                If it IsNot Nothing AndAlso it.GrupoId = id Then
                    dgvGrupos.ClearSelection()
                    row.Selected = True
                    dgvGrupos.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSelGru = False
        End Try
    End Sub
#End Region

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub
End Class
