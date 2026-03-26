Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de subcategorías. Sin SQL en la UI; usa SubCategoriaService (capa Business).
''' </summary>
Public Partial Class FrmSubCategorias

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosSubCategorias
        End Get
    End Property

    Private ReadOnly _servicio As New SubCategoriaService()
    Private _subCategoriaEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False
    Private _inicializando As Boolean = True

    Private Sub FrmSubCategorias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarComboFiltroCategoria()
        CargarComboEdicion()
        LimpiarFormularioEdicion()
        RefrescarListado()
        _inicializando = False
    End Sub

    Private Sub ConfigurarGrilla()
        dgvSubCategorias.AutoGenerateColumns = False
        dgvSubCategorias.Columns.Clear()
        dgvSubCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "SubCategoriaId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvSubCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CategoriaCodigo", .HeaderText = "Cat.", .Name = "colCat", .MinimumWidth = 55, .FillWeight = 60})
        dgvSubCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvSubCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 140, .FillWeight = 200})
        dgvSubCategorias.Columns.Add(New DataGridViewCheckBoxColumn With {
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

    Private Sub CargarComboEdicion()
        Dim cats = _servicio.ListarCategoriasActivasParaSelector()
        Dim lista As New List(Of CategoriaSelectorItem) From {
            New CategoriaSelectorItem With {.CategoriaId = 0, .Texto = "(Seleccione categoría)"}
        }
        lista.AddRange(cats)
        cmbCategoria.DisplayMember = "Texto"
        cmbCategoria.ValueMember = "CategoriaId"
        cmbCategoria.DataSource = lista
    End Sub

    Private Function ObtenerCategoriaFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroCategoria.SelectedItem, CategoriaSelectorItem)
        If it Is Nothing OrElse it.CategoriaId <= 0 Then Return Nothing
        Return it.CategoriaId
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarSubCategorias(txtBusqueda.Text, ObtenerCategoriaFiltroId())
            dgvSubCategorias.DataSource = Nothing
            dgvSubCategorias.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de subcategorías.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroCategoria.SelectedIndexChanged
        If _inicializando Then Return
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvSubCategorias.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _subCategoriaEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        AplicarCodigoSugeridoSiNuevo()
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub AplicarCodigoSugeridoSiNuevo()
        If _subCategoriaEdicionId <> 0 Then Return
        Dim cid = ObtenerCategoriaIdEdicion()
        If cid <= 0 Then
            txtCodigo.Clear()
            Return
        End If
        Try
            txtCodigo.Text = _servicio.ObtenerCodigoSugeridoParaNuevaSubCategoria(cid)
        Catch
            txtCodigo.Clear()
        End Try
    End Sub

    Private Sub cmbCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategoria.SelectedIndexChanged
        If _inicializando Then Return
        If _subCategoriaEdicionId = 0 Then AplicarCodigoSugeridoSiNuevo()
    End Sub

    Private Sub dgvSubCategorias_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubCategorias.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerSubCategoriaIdSeleccionado()
        If id <= 0 Then Return
        CargarSubCategoriaEnFormulario(id)
    End Sub

    Private Function ObtenerSubCategoriaIdSeleccionado() As Integer
        If dgvSubCategorias.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvSubCategorias.CurrentRow.DataBoundItem, SubCategoriaResumen)
        If item Is Nothing Then Return 0
        Return item.SubCategoriaId
    End Function

    Private Sub AsegurarCategoriaEnCombo(subCategoria As SubCategoria)
        Dim lista = TryCast(cmbCategoria.DataSource, List(Of CategoriaSelectorItem))
        If lista Is Nothing Then Return
        Dim id = subCategoria.CategoriaId
        If id <= 0 Then Return
        If lista.Any(Function(x) x.CategoriaId = id) Then Return
        Dim extra = _servicio.ObtenerItemCategoriaParaEdicion(id)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        cmbCategoria.DataSource = Nothing
        cmbCategoria.DisplayMember = "Texto"
        cmbCategoria.ValueMember = "CategoriaId"
        cmbCategoria.DataSource = lista
    End Sub

    Private Sub CargarSubCategoriaEnFormulario(subCategoriaId As Integer)
        Try
            Dim s = _servicio.ObtenerPorId(subCategoriaId)
            If s Is Nothing Then
                MessageBox.Show("No se encontró la subcategoría.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _subCategoriaEdicionId = s.SubCategoriaId
            lblIdValor.Text = s.SubCategoriaId.ToString()
            CargarComboEdicion()
            AsegurarCategoriaEnCombo(s)
            cmbCategoria.SelectedValue = s.CategoriaId
            txtCodigo.Text = s.Codigo
            txtNombre.Text = s.Nombre
            chkActivo.Checked = s.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar la subcategoría.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _subCategoriaEdicionId = 0
        lblIdValor.Text = "—"
        CargarComboEdicion()
        cmbCategoria.SelectedValue = 0
        txtCodigo.Clear()
        txtNombre.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _subCategoriaEdicionId > 0
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

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New SubCategoria With {
            .SubCategoriaId = _subCategoriaEdicionId,
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text,
            .CategoriaId = ObtenerCategoriaIdEdicion()
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _subCategoriaEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _subCategoriaEdicionId)
            RefrescarListado()
            SeleccionarFilaPorSubCategoriaId(nuevoId)
            If nuevoId > 0 Then
                CargarSubCategoriaEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorSubCategoriaId(subCategoriaId As Integer)
        If subCategoriaId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvSubCategorias.Rows
                Dim item = TryCast(row.DataBoundItem, SubCategoriaResumen)
                If item IsNot Nothing AndAlso item.SubCategoriaId = subCategoriaId Then
                    dgvSubCategorias.ClearSelection()
                    row.Selected = True
                    dgvSubCategorias.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerSubCategoriaIdSeleccionado()
        If id > 0 Then
            CargarSubCategoriaEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _subCategoriaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar esta subcategoría?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_subCategoriaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorSubCategoriaId(_subCategoriaEdicionId)
        CargarSubCategoriaEnFormulario(_subCategoriaEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _subCategoriaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar esta subcategoría?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_subCategoriaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorSubCategoriaId(_subCategoriaEdicionId)
        CargarSubCategoriaEnFormulario(_subCategoriaEdicionId)
    End Sub
End Class
