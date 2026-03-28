Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de grupos de producto (tercer nivel). Sin SQL en la UI; usa GrupoService (capa Business).
''' </summary>
Public Partial Class FrmGrupos

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        MaestroListaEncabezadoHelper.AplicarEncabezadoGrilla(Me, pnlListaMaestro, lblTituloGrilla, dgvGrupos, "Grupos")
    End Sub

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosGrupos
        End Get
    End Property

    Private ReadOnly _servicio As New GrupoService()
    Private _grupoEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False
    Private _suspendEdicionCombos As Boolean = False
    Private _inicializando As Boolean = True

    Private Sub FrmGrupos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarComboFiltroCategoria()
        CargarComboFiltroSubCategoria()
        CargarComboEdicionCategoria()
        CargarComboEdicionSubCategoria(0)
        LimpiarFormularioEdicion()
        RefrescarListado()
        _inicializando = False
    End Sub

    Private Sub ConfigurarGrilla()
        dgvGrupos.AutoGenerateColumns = False
        dgvGrupos.Columns.Clear()
        dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "GrupoId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CategoriaCodigo", .HeaderText = "Cat.", .Name = "colCat", .MinimumWidth = 50, .FillWeight = 50})
        dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "SubCategoriaCodigo", .HeaderText = "Sub.", .Name = "colSub", .MinimumWidth = 50, .FillWeight = 50})
        dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 140, .FillWeight = 200})
        dgvGrupos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub CargarComboFiltroCategoria()
        Dim cats = _servicio.ListarCategoriasActivasParaSelector()
        Dim lista As New List(Of CategoriaSelectorItem) From {
            New CategoriaSelectorItem With {.CategoriaId = 0, .Texto = "(Todas)"}
        }
        lista.AddRange(cats)
        cmbFiltroCategoria.DisplayMember = "Texto"
        cmbFiltroCategoria.ValueMember = "CategoriaId"
        cmbFiltroCategoria.DataSource = lista
        cmbFiltroCategoria.SelectedIndex = 0
    End Sub

    Private Sub CargarComboFiltroSubCategoria()
        Dim catId = ObtenerCategoriaFiltroId()
        Dim subs = _servicio.ListarSubCategoriasActivasParaSelector(catId)
        Dim lista As New List(Of SubCategoriaSelectorItem) From {
            New SubCategoriaSelectorItem With {.SubCategoriaId = 0, .Texto = "(Todas)"}
        }
        lista.AddRange(subs)
        cmbFiltroSubCategoria.DisplayMember = "Texto"
        cmbFiltroSubCategoria.ValueMember = "SubCategoriaId"
        cmbFiltroSubCategoria.DataSource = lista
        cmbFiltroSubCategoria.SelectedIndex = 0
    End Sub

    Private Function ObtenerCategoriaFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroCategoria.SelectedItem, CategoriaSelectorItem)
        If it Is Nothing OrElse it.CategoriaId <= 0 Then Return Nothing
        Return it.CategoriaId
    End Function

    Private Function ObtenerSubCategoriaFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroSubCategoria.SelectedItem, SubCategoriaSelectorItem)
        If it Is Nothing OrElse it.SubCategoriaId <= 0 Then Return Nothing
        Return it.SubCategoriaId
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarGrupos(txtBusqueda.Text, ObtenerCategoriaFiltroId(), ObtenerSubCategoriaFiltroId())
            dgvGrupos.DataSource = Nothing
            dgvGrupos.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de grupos.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroCategoria.SelectedIndexChanged
        If _inicializando Then Return
        CargarComboFiltroSubCategoria()
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroSubCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroSubCategoria.SelectedIndexChanged
        If _inicializando Then Return
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvGrupos.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _grupoEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvGrupos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvGrupos.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerGrupoIdSeleccionado()
        If id <= 0 Then Return
        CargarGrupoEnFormulario(id)
    End Sub

    Private Function ObtenerGrupoIdSeleccionado() As Integer
        If dgvGrupos.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvGrupos.CurrentRow.DataBoundItem, GrupoResumen)
        If item Is Nothing Then Return 0
        Return item.GrupoId
    End Function

    Private Sub CargarComboEdicionCategoria()
        Dim cats = _servicio.ListarCategoriasActivasParaSelector()
        Dim lista As New List(Of CategoriaSelectorItem) From {
            New CategoriaSelectorItem With {.CategoriaId = 0, .Texto = "(Seleccione categoría)"}
        }
        lista.AddRange(cats)
        cmbCategoria.DisplayMember = "Texto"
        cmbCategoria.ValueMember = "CategoriaId"
        cmbCategoria.DataSource = lista
    End Sub

    Private Sub CargarComboEdicionSubCategoria(categoriaId As Integer)
        Dim lista As New List(Of SubCategoriaSelectorItem) From {
            New SubCategoriaSelectorItem With {.SubCategoriaId = 0, .Texto = "(Seleccione subcategoría)"}
        }
        If categoriaId > 0 Then
            lista.AddRange(_servicio.ListarSubCategoriasActivasParaSelector(categoriaId))
        End If
        cmbSubCategoria.DisplayMember = "Texto"
        cmbSubCategoria.ValueMember = "SubCategoriaId"
        cmbSubCategoria.DataSource = lista
    End Sub

    Private Sub AsegurarCategoriaEnCombo(categoriaId As Integer)
        Dim lista = TryCast(cmbCategoria.DataSource, List(Of CategoriaSelectorItem))
        If lista Is Nothing Then Return
        If categoriaId <= 0 Then Return
        If lista.Any(Function(x) x.CategoriaId = categoriaId) Then Return
        Dim extra = _servicio.ObtenerItemCategoriaParaEdicion(categoriaId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        cmbCategoria.DataSource = Nothing
        cmbCategoria.DisplayMember = "Texto"
        cmbCategoria.ValueMember = "CategoriaId"
        cmbCategoria.DataSource = lista
    End Sub

    Private Sub AsegurarSubCategoriaEnCombo(subCategoriaId As Integer)
        Dim lista = TryCast(cmbSubCategoria.DataSource, List(Of SubCategoriaSelectorItem))
        If lista Is Nothing Then Return
        If subCategoriaId <= 0 Then Return
        If lista.Any(Function(x) x.SubCategoriaId = subCategoriaId) Then Return
        Dim extra = _servicio.ObtenerItemSubCategoriaParaEdicion(subCategoriaId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        cmbSubCategoria.DataSource = Nothing
        cmbSubCategoria.DisplayMember = "Texto"
        cmbSubCategoria.ValueMember = "SubCategoriaId"
        cmbSubCategoria.DataSource = lista
    End Sub

    Private Sub CargarGrupoEnFormulario(grupoId As Integer)
        Try
            Dim g = _servicio.ObtenerPorId(grupoId)
            If g Is Nothing Then
                MessageBox.Show("No se encontró el grupo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _suspendEdicionCombos = True
            _grupoEdicionId = g.GrupoId
            lblIdValor.Text = g.GrupoId.ToString()
            CargarComboEdicionCategoria()
            AsegurarCategoriaEnCombo(g.CategoriaId)
            cmbCategoria.SelectedValue = g.CategoriaId
            CargarComboEdicionSubCategoria(g.CategoriaId)
            AsegurarSubCategoriaEnCombo(g.SubCategoriaId)
            cmbSubCategoria.SelectedValue = g.SubCategoriaId
            txtCodigo.Text = g.Codigo
            txtNombre.Text = g.Nombre
            chkActivo.Checked = g.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el grupo.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendEdicionCombos = False
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _suspendEdicionCombos = True
        _grupoEdicionId = 0
        lblIdValor.Text = "—"
        CargarComboEdicionCategoria()
        cmbCategoria.SelectedValue = 0
        CargarComboEdicionSubCategoria(0)
        txtCodigo.Clear()
        txtNombre.Clear()
        chkActivo.Checked = True
        _suspendEdicionCombos = False
        ActualizarBotonesEstado()
    End Sub

    Private Sub cmbCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategoria.SelectedIndexChanged
        If _inicializando OrElse _suspendEdicionCombos Then Return
        Dim cid = ObtenerCategoriaIdEdicion()
        CargarComboEdicionSubCategoria(cid)
        If cmbSubCategoria.Items.Count > 0 Then
            cmbSubCategoria.SelectedIndex = 0
        End If
        If _grupoEdicionId = 0 Then
            txtCodigo.Clear()
        End If
    End Sub

    Private Sub AplicarCodigoSugeridoSiNuevoGrupo()
        If _grupoEdicionId <> 0 Then Return
        Dim sid = ObtenerSubCategoriaIdEdicion()
        If sid <= 0 Then
            txtCodigo.Clear()
            Return
        End If
        Try
            txtCodigo.Text = _servicio.ObtenerCodigoSugeridoParaNuevoGrupo(sid)
        Catch
            txtCodigo.Clear()
        End Try
    End Sub

    Private Sub cmbSubCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubCategoria.SelectedIndexChanged
        If _inicializando OrElse _suspendEdicionCombos Then Return
        If _grupoEdicionId = 0 Then AplicarCodigoSugeridoSiNuevoGrupo()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _grupoEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Function ObtenerCategoriaIdEdicion() As Integer
        Dim it = TryCast(cmbCategoria.SelectedItem, CategoriaSelectorItem)
        If it Is Nothing Then Return 0
        Return it.CategoriaId
    End Function

    Private Function ObtenerSubCategoriaIdEdicion() As Integer
        Dim it = TryCast(cmbSubCategoria.SelectedItem, SubCategoriaSelectorItem)
        If it Is Nothing Then Return 0
        Return it.SubCategoriaId
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Grupo With {
            .GrupoId = _grupoEdicionId,
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text,
            .CategoriaId = ObtenerCategoriaIdEdicion(),
            .SubCategoriaId = ObtenerSubCategoriaIdEdicion()
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _grupoEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _grupoEdicionId)
            RefrescarListado()
            SeleccionarFilaPorGrupoId(nuevoId)
            If nuevoId > 0 Then
                CargarGrupoEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorGrupoId(grupoId As Integer)
        If grupoId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvGrupos.Rows
                Dim item = TryCast(row.DataBoundItem, GrupoResumen)
                If item IsNot Nothing AndAlso item.GrupoId = grupoId Then
                    dgvGrupos.ClearSelection()
                    row.Selected = True
                    dgvGrupos.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerGrupoIdSeleccionado()
        If id > 0 Then
            CargarGrupoEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _grupoEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este grupo?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_grupoEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorGrupoId(_grupoEdicionId)
        CargarGrupoEnFormulario(_grupoEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _grupoEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este grupo?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_grupoEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorGrupoId(_grupoEdicionId)
        CargarGrupoEnFormulario(_grupoEdicionId)
    End Sub
End Class
