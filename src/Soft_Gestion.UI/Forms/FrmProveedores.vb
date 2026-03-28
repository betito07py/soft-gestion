Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de proveedores. Sin SQL en la UI; usa ProveedorService (capa Business).
''' </summary>
Public Partial Class FrmProveedores

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        MaestroListaEncabezadoHelper.AplicarEncabezadoGrilla(Me, pnlListaMaestro, lblTituloGrilla, dgvProveedores, "Proveedores")
    End Sub

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosProveedores
        End Get
    End Property

    Private ReadOnly _servicio As New ProveedorService()
    Private _proveedorEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmProveedores_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarCombosEdicion()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvProveedores.AutoGenerateColumns = False
        dgvProveedores.Columns.Clear()
        dgvProveedores.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ProveedorId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvProveedores.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvProveedores.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RazonSocial", .HeaderText = "Razón social", .Name = "colRazon", .MinimumWidth = 140, .FillWeight = 200})
        dgvProveedores.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RUC", .HeaderText = "RUC", .Name = "colRuc", .MinimumWidth = 80, .FillWeight = 90})
        dgvProveedores.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CondicionPagoCodigo", .HeaderText = "Cond. pago", .Name = "colCp", .MinimumWidth = 70, .FillWeight = 80})
        dgvProveedores.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub CargarCombosEdicion()
        Dim cond = _servicio.ListarCondicionesPagoActivasParaSelector()
        Dim listaCond As New List(Of CondicionPagoSelectorItem) From {
            New CondicionPagoSelectorItem With {.CondicionPagoId = 0, .Texto = "(Sin condición)"}
        }
        listaCond.AddRange(cond)
        cmbCondicionPago.DisplayMember = "Texto"
        cmbCondicionPago.ValueMember = "CondicionPagoId"
        cmbCondicionPago.DataSource = listaCond
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarProveedores(txtBusqueda.Text)
            dgvProveedores.DataSource = Nothing
            dgvProveedores.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de proveedores.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvProveedores.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _proveedorEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvProveedores_SelectionChanged(sender As Object, e As EventArgs) Handles dgvProveedores.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerProveedorIdSeleccionado()
        If id <= 0 Then Return
        CargarProveedorEnFormulario(id)
    End Sub

    Private Function ObtenerProveedorIdSeleccionado() As Integer
        If dgvProveedores.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvProveedores.CurrentRow.DataBoundItem, ProveedorResumen)
        If item Is Nothing Then Return 0
        Return item.ProveedorId
    End Function

    Private Sub AsegurarCondicionEnCombo(proveedor As Proveedor)
        Dim lista = TryCast(cmbCondicionPago.DataSource, List(Of CondicionPagoSelectorItem))
        If lista Is Nothing Then Return
        Dim id = If(proveedor.CondicionPagoId.HasValue, proveedor.CondicionPagoId.Value, 0)
        If id <= 0 Then Return
        If lista.Any(Function(x) x.CondicionPagoId = id) Then Return
        Dim extra = _servicio.ObtenerItemCondicionPagoParaEdicion(proveedor.CondicionPagoId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        cmbCondicionPago.DataSource = Nothing
        cmbCondicionPago.DisplayMember = "Texto"
        cmbCondicionPago.ValueMember = "CondicionPagoId"
        cmbCondicionPago.DataSource = lista
    End Sub

    Private Sub CargarProveedorEnFormulario(proveedorId As Integer)
        Try
            Dim p = _servicio.ObtenerPorId(proveedorId)
            If p Is Nothing Then
                MessageBox.Show("No se encontró el proveedor.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _proveedorEdicionId = p.ProveedorId
            lblIdValor.Text = p.ProveedorId.ToString()
            CargarCombosEdicion()
            AsegurarCondicionEnCombo(p)
            cmbCondicionPago.SelectedValue = If(p.CondicionPagoId.HasValue, p.CondicionPagoId.Value, 0)
            txtCodigo.Text = p.Codigo
            txtRazonSocial.Text = p.RazonSocial
            txtRUC.Text = If(p.RUC, String.Empty)
            txtDireccion.Text = If(p.Direccion, String.Empty)
            txtTelefono.Text = If(p.Telefono, String.Empty)
            txtEmail.Text = If(p.Email, String.Empty)
            txtObservaciones.Text = If(p.Observaciones, String.Empty)
            chkActivo.Checked = p.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el proveedor.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _proveedorEdicionId = 0
        lblIdValor.Text = "—"
        CargarCombosEdicion()
        cmbCondicionPago.SelectedValue = 0
        txtCodigo.Clear()
        txtRazonSocial.Clear()
        txtRUC.Clear()
        txtDireccion.Clear()
        txtTelefono.Clear()
        txtEmail.Clear()
        txtObservaciones.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _proveedorEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Function ObtenerCondicionPagoIdEdicion() As Integer?
        Dim it = TryCast(cmbCondicionPago.SelectedItem, CondicionPagoSelectorItem)
        If it Is Nothing OrElse it.CondicionPagoId <= 0 Then Return Nothing
        Return it.CondicionPagoId
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Proveedor With {
            .ProveedorId = _proveedorEdicionId,
            .Codigo = txtCodigo.Text,
            .RazonSocial = txtRazonSocial.Text,
            .RUC = txtRUC.Text,
            .Direccion = txtDireccion.Text,
            .Telefono = txtTelefono.Text,
            .Email = txtEmail.Text,
            .CondicionPagoId = ObtenerCondicionPagoIdEdicion(),
            .Observaciones = txtObservaciones.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _proveedorEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _proveedorEdicionId)
            RefrescarListado()
            SeleccionarFilaPorProveedorId(nuevoId)
            If nuevoId > 0 Then
                CargarProveedorEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorProveedorId(proveedorId As Integer)
        If proveedorId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvProveedores.Rows
                Dim item = TryCast(row.DataBoundItem, ProveedorResumen)
                If item IsNot Nothing AndAlso item.ProveedorId = proveedorId Then
                    dgvProveedores.ClearSelection()
                    row.Selected = True
                    dgvProveedores.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerProveedorIdSeleccionado()
        If id > 0 Then
            CargarProveedorEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _proveedorEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este proveedor?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_proveedorEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorProveedorId(_proveedorEdicionId)
        CargarProveedorEnFormulario(_proveedorEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _proveedorEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este proveedor?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_proveedorEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorProveedorId(_proveedorEdicionId)
        CargarProveedorEnFormulario(_proveedorEdicionId)
    End Sub
End Class
