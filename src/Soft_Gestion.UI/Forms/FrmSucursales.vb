Imports System.Collections.Generic
Imports System.Linq
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain
Imports System.Windows.Forms

''' <summary>
''' ABM de sucursales. Sin SQL en la UI; usa SucursalService (capa Business).
''' </summary>
Public Partial Class FrmSucursales

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosSucursales
        End Get
    End Property

    Private ReadOnly _servicio As New SucursalService()
    Private _sucursalEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False
    Private _suspendCombo As Boolean = False

    Private Sub FrmSucursales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarCombosEmpresa()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvSucursales.AutoGenerateColumns = False
        dgvSucursales.Columns.Clear()
        dgvSucursales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "SucursalId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvSucursales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "EmpresaCodigo", .HeaderText = "Empresa", .Name = "colEmpresa", .MinimumWidth = 70, .FillWeight = 80})
        dgvSucursales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvSucursales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 150, .FillWeight = 200})
        dgvSucursales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Responsable", .HeaderText = "Responsable", .Name = "colResp", .MinimumWidth = 100, .FillWeight = 120})
        dgvSucursales.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub CargarCombosEmpresa()
        Dim activas = _servicio.ListarEmpresasActivasParaSelector()
        Dim filtro As New List(Of EmpresaSelectorItem) From {
            New EmpresaSelectorItem With {.EmpresaId = 0, .Texto = "(Todas las empresas)"}
        }
        filtro.AddRange(activas)

        cmbEmpresaFiltro.DisplayMember = "Texto"
        cmbEmpresaFiltro.ValueMember = "EmpresaId"
        cmbEmpresaFiltro.DataSource = filtro

        cmbEmpresa.DisplayMember = "Texto"
        cmbEmpresa.ValueMember = "EmpresaId"
        cmbEmpresa.DataSource = New List(Of EmpresaSelectorItem)(activas)
    End Sub

    Private Function ObtenerEmpresaFiltroId() As Integer?
        Dim it = TryCast(cmbEmpresaFiltro.SelectedItem, EmpresaSelectorItem)
        If it Is Nothing OrElse it.EmpresaId <= 0 Then Return Nothing
        Return it.EmpresaId
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarSucursales(txtBusqueda.Text, ObtenerEmpresaFiltroId())
            dgvSucursales.DataSource = Nothing
            dgvSucursales.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de sucursales.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvSucursales.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _sucursalEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvSucursales_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSucursales.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerSucursalIdSeleccionado()
        If id <= 0 Then Return
        CargarSucursalEnFormulario(id)
    End Sub

    Private Function ObtenerSucursalIdSeleccionado() As Integer
        If dgvSucursales.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvSucursales.CurrentRow.DataBoundItem, SucursalResumen)
        If item Is Nothing Then Return 0
        Return item.SucursalId
    End Function

    Private Sub AsegurarEmpresaEnComboEdicion(sucursal As Sucursal)
        Dim lista = TryCast(cmbEmpresa.DataSource, List(Of EmpresaSelectorItem))
        If lista Is Nothing Then Return
        If lista.Any(Function(x) x.EmpresaId = sucursal.EmpresaId) Then Return
        Dim extra = _servicio.ObtenerItemEmpresaParaEdicion(sucursal.EmpresaId)
        If extra Is Nothing Then Return
        lista.Insert(0, extra)
        _suspendCombo = True
        cmbEmpresa.DataSource = Nothing
        cmbEmpresa.DisplayMember = "Texto"
        cmbEmpresa.ValueMember = "EmpresaId"
        cmbEmpresa.DataSource = lista
        _suspendCombo = False
    End Sub

    Private Sub CargarSucursalEnFormulario(sucursalId As Integer)
        Try
            Dim s = _servicio.ObtenerPorId(sucursalId)
            If s Is Nothing Then
                MessageBox.Show("No se encontró la sucursal.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _sucursalEdicionId = s.SucursalId
            lblIdValor.Text = s.SucursalId.ToString()
            AsegurarEmpresaEnComboEdicion(s)
            _suspendCombo = True
            cmbEmpresa.SelectedValue = s.EmpresaId
            _suspendCombo = False
            txtCodigo.Text = s.Codigo
            txtNombre.Text = s.Nombre
            txtDireccion.Text = If(s.Direccion, String.Empty)
            txtTelefono.Text = If(s.Telefono, String.Empty)
            txtResponsable.Text = If(s.Responsable, String.Empty)
            chkActivo.Checked = s.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar la sucursal.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _sucursalEdicionId = 0
        lblIdValor.Text = "—"
        _suspendCombo = True
        If cmbEmpresa.Items.Count > 0 Then
            cmbEmpresa.SelectedIndex = 0
        End If
        _suspendCombo = False
        txtCodigo.Clear()
        txtNombre.Clear()
        txtDireccion.Clear()
        txtTelefono.Clear()
        txtResponsable.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _sucursalEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Function ObtenerEmpresaIdEdicion() As Integer
        Dim it = TryCast(cmbEmpresa.SelectedItem, EmpresaSelectorItem)
        If it Is Nothing Then Return 0
        Return it.EmpresaId
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Sucursal With {
            .SucursalId = _sucursalEdicionId,
            .EmpresaId = ObtenerEmpresaIdEdicion(),
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text,
            .Direccion = txtDireccion.Text,
            .Telefono = txtTelefono.Text,
            .Responsable = txtResponsable.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _sucursalEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _sucursalEdicionId)
            RefrescarListado()
            SeleccionarFilaPorSucursalId(nuevoId)
            If nuevoId > 0 Then
                CargarSucursalEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorSucursalId(sucursalId As Integer)
        If sucursalId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvSucursales.Rows
                Dim item = TryCast(row.DataBoundItem, SucursalResumen)
                If item IsNot Nothing AndAlso item.SucursalId = sucursalId Then
                    dgvSucursales.ClearSelection()
                    row.Selected = True
                    dgvSucursales.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerSucursalIdSeleccionado()
        If id > 0 Then
            CargarSucursalEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _sucursalEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar esta sucursal?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_sucursalEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorSucursalId(_sucursalEdicionId)
        CargarSucursalEnFormulario(_sucursalEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _sucursalEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar esta sucursal?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_sucursalEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorSucursalId(_sucursalEdicionId)
        CargarSucursalEnFormulario(_sucursalEdicionId)
    End Sub
End Class
