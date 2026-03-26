Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain
Imports System.Windows.Forms

''' <summary>
''' ABM de usuarios. Sin SQL en la UI; usa UsuarioService (capa Business).
''' </summary>
Public Partial Class FrmUsuarios

    ''' <summary>Clave alineada con el menú y permisos futuros.</summary>
    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.SeguridadUsuarios
        End Get
    End Property

    Private ReadOnly _servicio As New UsuarioService()
    Private _usuarioEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarComboSucursales()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvUsuarios.AutoGenerateColumns = False
        dgvUsuarios.Columns.Clear()
        dgvUsuarios.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "UsuarioId", .HeaderText = "Id", .Name = "colId", .Width = 48, .MinimumWidth = 40})
        dgvUsuarios.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Login", .HeaderText = "Login", .Name = "colLogin", .Width = 120, .MinimumWidth = 80})
        dgvUsuarios.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "NombreCompleto", .HeaderText = "Nombre", .Name = "colNombre", .FillWeight = 200, .MinimumWidth = 120})
        dgvUsuarios.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Email", .HeaderText = "Email", .Name = "colEmail", .Width = 180, .MinimumWidth = 100})
        dgvUsuarios.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "EsAdministrador", .HeaderText = "Admin", .Name = "colAdmin", .Width = 50})
        dgvUsuarios.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
        Dim colSuc As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "SucursalId", .HeaderText = "Sucursal Id", .Name = "colSucursal", .Width = 85}
        dgvUsuarios.Columns.Add(colSuc)
    End Sub

    Private Sub CargarComboSucursales()
        cmbSucursal.Items.Clear()
        cmbSucursal.Items.Add(New ItemComboSucursal With {.Id = Nothing, .Texto = "(Sin sucursal)"})
        Try
            For Each s In _servicio.ListarSucursalesActivasParaSelector()
                cmbSucursal.Items.Add(New ItemComboSucursal With {.Id = s.SucursalId, .Texto = s.Texto})
            Next
        Catch
            MessageBox.Show("No se pudieron cargar las sucursales.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        cmbSucursal.SelectedIndex = 0
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarUsuarios(txtBusqueda.Text)
            dgvUsuarios.DataSource = Nothing
            dgvUsuarios.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de usuarios.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvUsuarios.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _usuarioEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        lblPasswordAyuda.Text = "Obligatoria en el alta (mín. 6 caracteres)."
        txtLogin.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvUsuarios_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUsuarios.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerUsuarioIdSeleccionado()
        If id <= 0 Then Return
        CargarUsuarioEnFormulario(id)
    End Sub

    Private Function ObtenerUsuarioIdSeleccionado() As Integer
        If dgvUsuarios.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvUsuarios.CurrentRow.DataBoundItem, UsuarioResumen)
        If item Is Nothing Then Return 0
        Return item.UsuarioId
    End Function

    Private Sub CargarUsuarioEnFormulario(usuarioId As Integer)
        Try
            Dim u = _servicio.ObtenerPorIdParaEdicion(usuarioId)
            If u Is Nothing Then
                MessageBox.Show("No se encontró el usuario.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _usuarioEdicionId = u.UsuarioId
            lblIdValor.Text = u.UsuarioId.ToString()
            txtLogin.Text = u.Login
            txtNombreCompleto.Text = u.NombreCompleto
            txtEmail.Text = If(u.Email, String.Empty)
            chkEsAdministrador.Checked = u.EsAdministrador
            SeleccionarSucursalEnCombo(u.SucursalId)
            chkActivo.Checked = u.Activo
            txtPassword.Clear()
            lblPasswordAyuda.Text = "Deje vacía para no cambiar la contraseña."
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el usuario.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SeleccionarSucursalEnCombo(sucursalId As Integer?)
        For i As Integer = 0 To cmbSucursal.Items.Count - 1
            Dim it = TryCast(cmbSucursal.Items(i), ItemComboSucursal)
            If it IsNot Nothing AndAlso IgualNullable(it.Id, sucursalId) Then
                cmbSucursal.SelectedIndex = i
                Return
            End If
        Next
        cmbSucursal.SelectedIndex = 0
    End Sub

    Private Shared Function IgualNullable(a As Integer?, b As Integer?) As Boolean
        If Not a.HasValue AndAlso Not b.HasValue Then Return True
        If Not a.HasValue OrElse Not b.HasValue Then Return False
        Return a.Value = b.Value
    End Function

    Private Function ObtenerSucursalSeleccionada() As Integer?
        Dim it = TryCast(cmbSucursal.SelectedItem, ItemComboSucursal)
        If it Is Nothing Then Return Nothing
        Return it.Id
    End Function

    Private Sub LimpiarFormularioEdicion()
        _usuarioEdicionId = 0
        lblIdValor.Text = "—"
        txtLogin.Clear()
        txtNombreCompleto.Clear()
        txtEmail.Clear()
        chkEsAdministrador.Checked = False
        cmbSucursal.SelectedIndex = If(cmbSucursal.Items.Count > 0, 0, -1)
        txtPassword.Clear()
        chkActivo.Checked = True
        lblPasswordAyuda.Text = String.Empty
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _usuarioEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Usuario With {
            .UsuarioId = _usuarioEdicionId,
            .Login = txtLogin.Text,
            .NombreCompleto = txtNombreCompleto.Text,
            .Email = txtEmail.Text,
            .EsAdministrador = chkEsAdministrador.Checked,
            .SucursalId = ObtenerSucursalSeleccionada()
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _usuarioEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, txtPassword.Text, aud)
            Else
                res = _servicio.EditarExistente(modelo, txtPassword.Text, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _usuarioEdicionId)
            RefrescarListado()
            SeleccionarFilaPorUsuarioId(nuevoId)
            If nuevoId > 0 Then
                CargarUsuarioEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorUsuarioId(usuarioId As Integer)
        If usuarioId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvUsuarios.Rows
                Dim item = TryCast(row.DataBoundItem, UsuarioResumen)
                If item IsNot Nothing AndAlso item.UsuarioId = usuarioId Then
                    dgvUsuarios.ClearSelection()
                    row.Selected = True
                    dgvUsuarios.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerUsuarioIdSeleccionado()
        If id > 0 Then
            CargarUsuarioEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _usuarioEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este usuario?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_usuarioEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorUsuarioId(_usuarioEdicionId)
        CargarUsuarioEnFormulario(_usuarioEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _usuarioEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este usuario?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_usuarioEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorUsuarioId(_usuarioEdicionId)
        CargarUsuarioEnFormulario(_usuarioEdicionId)
    End Sub

    Private Class ItemComboSucursal
        Public Property Id As Integer?
        Public Property Texto As String
        Public Overrides Function ToString() As String
            Return Texto
        End Function
    End Class
End Class
