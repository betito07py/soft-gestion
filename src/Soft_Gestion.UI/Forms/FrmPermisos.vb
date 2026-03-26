Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain
Imports System.Windows.Forms

''' <summary>
''' ABM de permisos. Sin SQL en la UI; usa PermisoService (capa Business).
''' La asignación a roles (<c>RolPermisos</c>) se agregará después.
''' </summary>
Public Partial Class FrmPermisos

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.SeguridadPermisos
        End Get
    End Property

    Private ReadOnly _servicio As New PermisoService()
    Private _permisoEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmPermisos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvPermisos.AutoGenerateColumns = False
        dgvPermisos.Columns.Clear()
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PermisoId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 100, .FillWeight = 120})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 120, .FillWeight = 140})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Modulo", .HeaderText = "Módulo", .Name = "colModulo", .MinimumWidth = 80, .FillWeight = 100})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Formulario", .HeaderText = "Formulario", .Name = "colForm", .MinimumWidth = 80, .FillWeight = 100})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Accion", .HeaderText = "Acción", .Name = "colAccion", .MinimumWidth = 60, .FillWeight = 80})
        dgvPermisos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarPermisos(txtBusqueda.Text)
            dgvPermisos.DataSource = Nothing
            dgvPermisos.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de permisos.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvPermisos.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _permisoEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvPermisos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPermisos.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerPermisoIdSeleccionado()
        If id <= 0 Then Return
        CargarPermisoEnFormulario(id)
    End Sub

    Private Function ObtenerPermisoIdSeleccionado() As Integer
        If dgvPermisos.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvPermisos.CurrentRow.DataBoundItem, PermisoResumen)
        If item Is Nothing Then Return 0
        Return item.PermisoId
    End Function

    Private Sub CargarPermisoEnFormulario(permisoId As Integer)
        Try
            Dim p = _servicio.ObtenerPorId(permisoId)
            If p Is Nothing Then
                MessageBox.Show("No se encontró el permiso.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _permisoEdicionId = p.PermisoId
            lblIdValor.Text = p.PermisoId.ToString()
            txtCodigo.Text = p.Codigo
            txtNombre.Text = p.Nombre
            txtModulo.Text = p.Modulo
            txtFormulario.Text = If(p.Formulario, String.Empty)
            txtAccion.Text = If(p.Accion, String.Empty)
            txtDescripcion.Text = If(p.Descripcion, String.Empty)
            chkActivo.Checked = p.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el permiso.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _permisoEdicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtNombre.Clear()
        txtModulo.Clear()
        txtFormulario.Clear()
        txtAccion.Clear()
        txtDescripcion.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _permisoEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Permiso With {
            .PermisoId = _permisoEdicionId,
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text,
            .Modulo = txtModulo.Text,
            .Formulario = txtFormulario.Text,
            .Accion = txtAccion.Text,
            .Descripcion = txtDescripcion.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _permisoEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _permisoEdicionId)
            RefrescarListado()
            SeleccionarFilaPorPermisoId(nuevoId)
            If nuevoId > 0 Then
                CargarPermisoEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorPermisoId(permisoId As Integer)
        If permisoId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvPermisos.Rows
                Dim item = TryCast(row.DataBoundItem, PermisoResumen)
                If item IsNot Nothing AndAlso item.PermisoId = permisoId Then
                    dgvPermisos.ClearSelection()
                    row.Selected = True
                    dgvPermisos.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerPermisoIdSeleccionado()
        If id > 0 Then
            CargarPermisoEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _permisoEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este permiso?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_permisoEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorPermisoId(_permisoEdicionId)
        CargarPermisoEnFormulario(_permisoEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _permisoEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este permiso?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_permisoEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorPermisoId(_permisoEdicionId)
        CargarPermisoEnFormulario(_permisoEdicionId)
    End Sub
End Class
