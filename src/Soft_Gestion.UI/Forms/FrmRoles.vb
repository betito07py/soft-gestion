Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain
Imports System.Windows.Forms

''' <summary>
''' ABM de roles. Sin SQL en la UI; usa RolService (capa Business).
''' La pantalla de permisos por rol (<c>RolPermisos</c>) se agregará después.
''' </summary>
Public Partial Class FrmRoles

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.SeguridadRoles
        End Get
    End Property

    Private ReadOnly _servicio As New RolService()
    Private _rolEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmRoles_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvRoles.AutoGenerateColumns = False
        dgvRoles.Columns.Clear()
        dgvRoles.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RolId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvRoles.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 120, .FillWeight = 150})
        dgvRoles.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Descripcion", .HeaderText = "Descripción", .Name = "colDesc", .MinimumWidth = 150, .FillWeight = 200})
        dgvRoles.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarRoles(txtBusqueda.Text)
            dgvRoles.DataSource = Nothing
            dgvRoles.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de roles.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvRoles.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _rolEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtNombre.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvRoles_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRoles.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerRolIdSeleccionado()
        If id <= 0 Then Return
        CargarRolEnFormulario(id)
    End Sub

    Private Function ObtenerRolIdSeleccionado() As Integer
        If dgvRoles.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvRoles.CurrentRow.DataBoundItem, RolResumen)
        If item Is Nothing Then Return 0
        Return item.RolId
    End Function

    Private Sub CargarRolEnFormulario(rolId As Integer)
        Try
            Dim r = _servicio.ObtenerPorId(rolId)
            If r Is Nothing Then
                MessageBox.Show("No se encontró el rol.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _rolEdicionId = r.RolId
            lblIdValor.Text = r.RolId.ToString()
            txtNombre.Text = r.Nombre
            txtDescripcion.Text = If(r.Descripcion, String.Empty)
            chkActivo.Checked = r.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el rol.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _rolEdicionId = 0
        lblIdValor.Text = "—"
        txtNombre.Clear()
        txtDescripcion.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _rolEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Rol With {
            .RolId = _rolEdicionId,
            .Nombre = txtNombre.Text,
            .Descripcion = txtDescripcion.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _rolEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _rolEdicionId)
            RefrescarListado()
            SeleccionarFilaPorRolId(nuevoId)
            If nuevoId > 0 Then
                CargarRolEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorRolId(rolId As Integer)
        If rolId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvRoles.Rows
                Dim item = TryCast(row.DataBoundItem, RolResumen)
                If item IsNot Nothing AndAlso item.RolId = rolId Then
                    dgvRoles.ClearSelection()
                    row.Selected = True
                    dgvRoles.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerRolIdSeleccionado()
        If id > 0 Then
            CargarRolEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _rolEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este rol?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_rolEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorRolId(_rolEdicionId)
        CargarRolEnFormulario(_rolEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _rolEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este rol?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_rolEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorRolId(_rolEdicionId)
        CargarRolEnFormulario(_rolEdicionId)
    End Sub
End Class
