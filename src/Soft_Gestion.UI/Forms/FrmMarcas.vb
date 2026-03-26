Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de marcas de producto. Sin SQL en la UI; usa MarcaService (capa Business).
''' </summary>
Public Partial Class FrmMarcas

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosMarcas
        End Get
    End Property

    Private ReadOnly _servicio As New MarcaService()
    Private _marcaEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmMarcas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvMarcas.AutoGenerateColumns = False
        dgvMarcas.Columns.Clear()
        dgvMarcas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "MarcaId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvMarcas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvMarcas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 150, .FillWeight = 200})
        dgvMarcas.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarMarcas(txtBusqueda.Text)
            dgvMarcas.DataSource = Nothing
            dgvMarcas.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de marcas.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvMarcas.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _marcaEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvMarcas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvMarcas.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerMarcaIdSeleccionado()
        If id <= 0 Then Return
        CargarMarcaEnFormulario(id)
    End Sub

    Private Function ObtenerMarcaIdSeleccionado() As Integer
        If dgvMarcas.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvMarcas.CurrentRow.DataBoundItem, MarcaResumen)
        If item Is Nothing Then Return 0
        Return item.MarcaId
    End Function

    Private Sub CargarMarcaEnFormulario(marcaId As Integer)
        Try
            Dim m = _servicio.ObtenerPorId(marcaId)
            If m Is Nothing Then
                MessageBox.Show("No se encontró la marca.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _marcaEdicionId = m.MarcaId
            lblIdValor.Text = m.MarcaId.ToString()
            txtCodigo.Text = m.Codigo
            txtNombre.Text = m.Nombre
            chkActivo.Checked = m.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar la marca.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _marcaEdicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtNombre.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _marcaEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Marca With {
            .MarcaId = _marcaEdicionId,
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _marcaEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _marcaEdicionId)
            RefrescarListado()
            SeleccionarFilaPorMarcaId(nuevoId)
            If nuevoId > 0 Then
                CargarMarcaEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorMarcaId(marcaId As Integer)
        If marcaId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvMarcas.Rows
                Dim item = TryCast(row.DataBoundItem, MarcaResumen)
                If item IsNot Nothing AndAlso item.MarcaId = marcaId Then
                    dgvMarcas.ClearSelection()
                    row.Selected = True
                    dgvMarcas.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerMarcaIdSeleccionado()
        If id > 0 Then
            CargarMarcaEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _marcaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar esta marca?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_marcaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorMarcaId(_marcaEdicionId)
        CargarMarcaEnFormulario(_marcaEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _marcaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar esta marca?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_marcaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorMarcaId(_marcaEdicionId)
        CargarMarcaEnFormulario(_marcaEdicionId)
    End Sub
End Class
