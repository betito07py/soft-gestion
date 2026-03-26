Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de categorías de producto. Sin SQL en la UI; usa el servicio de categorías (capa Business).
''' </summary>
Public Partial Class FrmCategorias

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosCategorias
        End Get
    End Property

    Private ReadOnly _servicio As New CategoriaService()
    Private _categoriaEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmCategorias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvCategorias.AutoGenerateColumns = False
        dgvCategorias.Columns.Clear()
        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CategoriaId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 150, .FillWeight = 200})
        dgvCategorias.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarCategorias(txtBusqueda.Text)
            dgvCategorias.DataSource = Nothing
            dgvCategorias.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de categorías.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvCategorias.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _categoriaEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        Try
            txtCodigo.Text = _servicio.ObtenerCodigoSugeridoParaNuevaCategoria()
        Catch
            txtCodigo.Clear()
        End Try
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvCategorias_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCategorias.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerCategoriaIdSeleccionado()
        If id <= 0 Then Return
        CargarCategoriaEnFormulario(id)
    End Sub

    Private Function ObtenerCategoriaIdSeleccionado() As Integer
        If dgvCategorias.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvCategorias.CurrentRow.DataBoundItem, CategoriaResumen)
        If item Is Nothing Then Return 0
        Return item.CategoriaId
    End Function

    Private Sub CargarCategoriaEnFormulario(categoriaId As Integer)
        Try
            Dim cat = _servicio.ObtenerPorId(categoriaId)
            If cat Is Nothing Then
                MessageBox.Show("No se encontró la categoría.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _categoriaEdicionId = cat.CategoriaId
            lblIdValor.Text = cat.CategoriaId.ToString()
            txtCodigo.Text = cat.Codigo
            txtNombre.Text = cat.Nombre
            chkActivo.Checked = cat.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar la categoría.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _categoriaEdicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtNombre.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _categoriaEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Categoria With {
            .CategoriaId = _categoriaEdicionId,
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _categoriaEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _categoriaEdicionId)
            RefrescarListado()
            SeleccionarFilaPorCategoriaId(nuevoId)
            If nuevoId > 0 Then
                CargarCategoriaEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorCategoriaId(categoriaId As Integer)
        If categoriaId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvCategorias.Rows
                Dim item = TryCast(row.DataBoundItem, CategoriaResumen)
                If item IsNot Nothing AndAlso item.CategoriaId = categoriaId Then
                    dgvCategorias.ClearSelection()
                    row.Selected = True
                    dgvCategorias.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerCategoriaIdSeleccionado()
        If id > 0 Then
            CargarCategoriaEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _categoriaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar esta categoría?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_categoriaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorCategoriaId(_categoriaEdicionId)
        CargarCategoriaEnFormulario(_categoriaEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _categoriaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar esta categoría?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_categoriaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorCategoriaId(_categoriaEdicionId)
        CargarCategoriaEnFormulario(_categoriaEdicionId)
    End Sub
End Class
