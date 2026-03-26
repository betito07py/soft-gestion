Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain
Imports System.Windows.Forms

''' <summary>
''' ABM de empresas. Sin SQL en la UI; usa EmpresaService (capa Business).
''' </summary>
Public Partial Class FrmEmpresas

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosEmpresas
        End Get
    End Property

    Private ReadOnly _servicio As New EmpresaService()
    Private _empresaEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmEmpresas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvEmpresas.AutoGenerateColumns = False
        dgvEmpresas.Columns.Clear()
        dgvEmpresas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "EmpresaId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvEmpresas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvEmpresas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RazonSocial", .HeaderText = "Razón social", .Name = "colRazon", .MinimumWidth = 150, .FillWeight = 200})
        dgvEmpresas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "NombreFantasia", .HeaderText = "Nombre fantasía", .Name = "colFantasia", .MinimumWidth = 100, .FillWeight = 120})
        dgvEmpresas.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RUC", .HeaderText = "RUC/DOC", .Name = "colRuc", .MinimumWidth = 80, .FillWeight = 90})
        dgvEmpresas.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarEmpresas(txtBusqueda.Text)
            dgvEmpresas.DataSource = Nothing
            dgvEmpresas.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de empresas.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvEmpresas.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _empresaEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvEmpresas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEmpresas.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerEmpresaIdSeleccionado()
        If id <= 0 Then Return
        CargarEmpresaEnFormulario(id)
    End Sub

    Private Function ObtenerEmpresaIdSeleccionado() As Integer
        If dgvEmpresas.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvEmpresas.CurrentRow.DataBoundItem, EmpresaResumen)
        If item Is Nothing Then Return 0
        Return item.EmpresaId
    End Function

    Private Sub CargarEmpresaEnFormulario(empresaId As Integer)
        Try
            Dim emp = _servicio.ObtenerPorId(empresaId)
            If emp Is Nothing Then
                MessageBox.Show("No se encontró la empresa.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _empresaEdicionId = emp.EmpresaId
            lblIdValor.Text = emp.EmpresaId.ToString()
            txtCodigo.Text = emp.Codigo
            txtRazonSocial.Text = emp.RazonSocial
            txtNombreFantasia.Text = If(emp.NombreFantasia, String.Empty)
            txtRUC.Text = If(emp.RUC, String.Empty)
            txtDireccion.Text = If(emp.Direccion, String.Empty)
            txtTelefono.Text = If(emp.Telefono, String.Empty)
            txtEmail.Text = If(emp.Email, String.Empty)
            chkActivo.Checked = emp.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar la empresa.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _empresaEdicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtRazonSocial.Clear()
        txtNombreFantasia.Clear()
        txtRUC.Clear()
        txtDireccion.Clear()
        txtTelefono.Clear()
        txtEmail.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _empresaEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Empresa With {
            .EmpresaId = _empresaEdicionId,
            .Codigo = txtCodigo.Text,
            .RazonSocial = txtRazonSocial.Text,
            .NombreFantasia = txtNombreFantasia.Text,
            .RUC = txtRUC.Text,
            .Direccion = txtDireccion.Text,
            .Telefono = txtTelefono.Text,
            .Email = txtEmail.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _empresaEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _empresaEdicionId)
            RefrescarListado()
            SeleccionarFilaPorEmpresaId(nuevoId)
            If nuevoId > 0 Then
                CargarEmpresaEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorEmpresaId(empresaId As Integer)
        If empresaId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvEmpresas.Rows
                Dim item = TryCast(row.DataBoundItem, EmpresaResumen)
                If item IsNot Nothing AndAlso item.EmpresaId = empresaId Then
                    dgvEmpresas.ClearSelection()
                    row.Selected = True
                    dgvEmpresas.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerEmpresaIdSeleccionado()
        If id > 0 Then
            CargarEmpresaEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _empresaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar esta empresa?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_empresaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorEmpresaId(_empresaEdicionId)
        CargarEmpresaEnFormulario(_empresaEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _empresaEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar esta empresa?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_empresaEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorEmpresaId(_empresaEdicionId)
        CargarEmpresaEnFormulario(_empresaEdicionId)
    End Sub
End Class
