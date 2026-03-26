Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de clientes. Sin SQL en la UI; usa ClienteService (capa Business).
''' </summary>
Public Partial Class FrmClientes

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosClientes
        End Get
    End Property

    Private ReadOnly _servicio As New ClienteService()
    Private _clienteEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmClientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarCombosEdicion()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvClientes.AutoGenerateColumns = False
        dgvClientes.Columns.Clear()
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ClienteId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RazonSocial", .HeaderText = "Razón social", .Name = "colRazon", .MinimumWidth = 140, .FillWeight = 200})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Documento", .HeaderText = "Documento", .Name = "colDoc", .MinimumWidth = 80, .FillWeight = 90})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RUC", .HeaderText = "RUC", .Name = "colRuc", .MinimumWidth = 80, .FillWeight = 90})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CondicionPagoCodigo", .HeaderText = "Cond. pago", .Name = "colCp", .MinimumWidth = 70, .FillWeight = 80})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ListaPrecioCodigo", .HeaderText = "Lista precio", .Name = "colLp", .MinimumWidth = 70, .FillWeight = 80})
        dgvClientes.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "LimiteCredito", .HeaderText = "Lím. crédito", .Name = "colLim", .MinimumWidth = 70, .FillWeight = 70})
        dgvClientes.Columns.Add(New DataGridViewCheckBoxColumn With {
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

        Dim lp = _servicio.ListarListasPrecioActivasParaSelector()
        Dim listaLp As New List(Of ListaPrecioSelectorItem) From {
            New ListaPrecioSelectorItem With {.ListaPrecioId = 0, .Texto = "(Sin lista de precios)"}
        }
        listaLp.AddRange(lp)
        cmbListaPrecio.DisplayMember = "Texto"
        cmbListaPrecio.ValueMember = "ListaPrecioId"
        cmbListaPrecio.DataSource = listaLp
    End Sub

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarClientes(txtBusqueda.Text)
            dgvClientes.DataSource = Nothing
            dgvClientes.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de clientes.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvClientes.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _clienteEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvClientes_SelectionChanged(sender As Object, e As EventArgs) Handles dgvClientes.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerClienteIdSeleccionado()
        If id <= 0 Then Return
        CargarClienteEnFormulario(id)
    End Sub

    Private Function ObtenerClienteIdSeleccionado() As Integer
        If dgvClientes.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvClientes.CurrentRow.DataBoundItem, ClienteResumen)
        If item Is Nothing Then Return 0
        Return item.ClienteId
    End Function

    Private Sub AsegurarCondicionEnCombo(cliente As Cliente)
        Dim lista = TryCast(cmbCondicionPago.DataSource, List(Of CondicionPagoSelectorItem))
        If lista Is Nothing Then Return
        Dim id = If(cliente.CondicionPagoId.HasValue, cliente.CondicionPagoId.Value, 0)
        If id <= 0 Then Return
        If lista.Any(Function(x) x.CondicionPagoId = id) Then Return
        Dim extra = _servicio.ObtenerItemCondicionPagoParaEdicion(cliente.CondicionPagoId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        cmbCondicionPago.DataSource = Nothing
        cmbCondicionPago.DisplayMember = "Texto"
        cmbCondicionPago.ValueMember = "CondicionPagoId"
        cmbCondicionPago.DataSource = lista
    End Sub

    Private Sub AsegurarListaPrecioEnCombo(cliente As Cliente)
        Dim lista = TryCast(cmbListaPrecio.DataSource, List(Of ListaPrecioSelectorItem))
        If lista Is Nothing Then Return
        Dim id = If(cliente.ListaPrecioId.HasValue, cliente.ListaPrecioId.Value, 0)
        If id <= 0 Then Return
        If lista.Any(Function(x) x.ListaPrecioId = id) Then Return
        Dim extra = _servicio.ObtenerItemListaPrecioParaEdicion(cliente.ListaPrecioId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        cmbListaPrecio.DataSource = Nothing
        cmbListaPrecio.DisplayMember = "Texto"
        cmbListaPrecio.ValueMember = "ListaPrecioId"
        cmbListaPrecio.DataSource = lista
    End Sub

    Private Sub CargarClienteEnFormulario(clienteId As Integer)
        Try
            Dim c = _servicio.ObtenerPorId(clienteId)
            If c Is Nothing Then
                MessageBox.Show("No se encontró el cliente.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _clienteEdicionId = c.ClienteId
            lblIdValor.Text = c.ClienteId.ToString()
            CargarCombosEdicion()
            AsegurarCondicionEnCombo(c)
            AsegurarListaPrecioEnCombo(c)
            cmbCondicionPago.SelectedValue = If(c.CondicionPagoId.HasValue, c.CondicionPagoId.Value, 0)
            cmbListaPrecio.SelectedValue = If(c.ListaPrecioId.HasValue, c.ListaPrecioId.Value, 0)
            txtCodigo.Text = c.Codigo
            txtRazonSocial.Text = c.RazonSocial
            txtNombreFantasia.Text = If(c.NombreFantasia, String.Empty)
            txtDocumento.Text = If(c.Documento, String.Empty)
            txtRUC.Text = If(c.RUC, String.Empty)
            txtDireccion.Text = If(c.Direccion, String.Empty)
            txtTelefono.Text = If(c.Telefono, String.Empty)
            txtEmail.Text = If(c.Email, String.Empty)
            txtLimiteCredito.Text = c.LimiteCredito.ToString(CultureInfo.CurrentCulture)
            txtObservaciones.Text = If(c.Observaciones, String.Empty)
            chkActivo.Checked = c.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el cliente.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _clienteEdicionId = 0
        lblIdValor.Text = "—"
        CargarCombosEdicion()
        cmbCondicionPago.SelectedValue = 0
        cmbListaPrecio.SelectedValue = 0
        txtCodigo.Clear()
        txtRazonSocial.Clear()
        txtNombreFantasia.Clear()
        txtDocumento.Clear()
        txtRUC.Clear()
        txtDireccion.Clear()
        txtTelefono.Clear()
        txtEmail.Clear()
        txtLimiteCredito.Text = "0"
        txtObservaciones.Clear()
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _clienteEdicionId > 0
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

    Private Function ObtenerListaPrecioIdEdicion() As Integer?
        Dim it = TryCast(cmbListaPrecio.SelectedItem, ListaPrecioSelectorItem)
        If it Is Nothing OrElse it.ListaPrecioId <= 0 Then Return Nothing
        Return it.ListaPrecioId
    End Function

    Private Function ObtenerLimiteCreditoDesdeTexto(ByRef mensajeError As String) As Decimal?
        mensajeError = Nothing
        Dim s = txtLimiteCredito.Text.Trim()
        If s.Length = 0 Then Return 0D
        Dim d As Decimal
        If Decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, d) Then Return d
        If Decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, d) Then Return d
        mensajeError = "El límite de crédito no es un valor numérico válido."
        Return Nothing
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim errLim As String = Nothing
        Dim limite = ObtenerLimiteCreditoDesdeTexto(errLim)
        If Not limite.HasValue Then
            MessageBox.Show(errLim, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim modelo As New Cliente With {
            .ClienteId = _clienteEdicionId,
            .Codigo = txtCodigo.Text,
            .RazonSocial = txtRazonSocial.Text,
            .NombreFantasia = txtNombreFantasia.Text,
            .Documento = txtDocumento.Text,
            .RUC = txtRUC.Text,
            .Direccion = txtDireccion.Text,
            .Telefono = txtTelefono.Text,
            .Email = txtEmail.Text,
            .CondicionPagoId = ObtenerCondicionPagoIdEdicion(),
            .ListaPrecioId = ObtenerListaPrecioIdEdicion(),
            .LimiteCredito = limite.Value,
            .Observaciones = txtObservaciones.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _clienteEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _clienteEdicionId)
            RefrescarListado()
            SeleccionarFilaPorClienteId(nuevoId)
            If nuevoId > 0 Then
                CargarClienteEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorClienteId(clienteId As Integer)
        If clienteId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvClientes.Rows
                Dim item = TryCast(row.DataBoundItem, ClienteResumen)
                If item IsNot Nothing AndAlso item.ClienteId = clienteId Then
                    dgvClientes.ClearSelection()
                    row.Selected = True
                    dgvClientes.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerClienteIdSeleccionado()
        If id > 0 Then
            CargarClienteEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _clienteEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este cliente?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_clienteEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorClienteId(_clienteEdicionId)
        CargarClienteEnFormulario(_clienteEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _clienteEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este cliente?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_clienteEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorClienteId(_clienteEdicionId)
        CargarClienteEnFormulario(_clienteEdicionId)
    End Sub
End Class
