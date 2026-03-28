Imports System.Collections.Generic
Imports System.Linq
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain
Imports System.Windows.Forms

''' <summary>
''' ABM de depósitos. Sin SQL en la UI; usa DepositoService (capa Business).
''' </summary>
Public Partial Class FrmDepositos

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        MaestroListaEncabezadoHelper.AplicarEncabezadoGrilla(Me, pnlListaMaestro, lblTituloGrilla, dgvDepositos, "Depósitos")
    End Sub

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosDepositos
        End Get
    End Property

    Private ReadOnly _servicio As New DepositoService()
    Private _depositoEdicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False
    Private _suspendFiltros As Boolean = False

    Private Sub FrmDepositos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        _suspendFiltros = True
        CargarComboEmpresaFiltro()
        CargarComboSucursalFiltro()
        CargarComboSucursalEdicion()
        _suspendFiltros = False
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvDepositos.AutoGenerateColumns = False
        dgvDepositos.Columns.Clear()
        dgvDepositos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "DepositoId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvDepositos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "EmpresaCodigo", .HeaderText = "Empresa", .Name = "colEmpresa", .MinimumWidth = 60, .FillWeight = 70})
        dgvDepositos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "SucursalCodigo", .HeaderText = "Sucursal", .Name = "colSucursal", .MinimumWidth = 60, .FillWeight = 70})
        dgvDepositos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 80})
        dgvDepositos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 120, .FillWeight = 160})
        dgvDepositos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "EsPrincipal", .HeaderText = "Principal", .Name = "colPrincipal", .Width = 60})
        dgvDepositos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub CargarComboEmpresaFiltro()
        Dim activas = _servicio.ListarEmpresasActivasParaSelector()
        Dim filtro As New List(Of EmpresaSelectorItem) From {
            New EmpresaSelectorItem With {.EmpresaId = 0, .Texto = "(Todas las empresas)"}
        }
        filtro.AddRange(activas)
        cmbEmpresaFiltro.DisplayMember = "Texto"
        cmbEmpresaFiltro.ValueMember = "EmpresaId"
        cmbEmpresaFiltro.DataSource = filtro
    End Sub

    Private Sub CargarComboSucursalFiltro()
        Dim empId = ObtenerEmpresaFiltroId()
        Dim sucursales = _servicio.ListarSucursalesActivasParaSelector(empId)
        Dim filtro As New List(Of SucursalSelectorItem) From {
            New SucursalSelectorItem With {.SucursalId = 0, .Texto = "(Todas las sucursales)"}
        }
        filtro.AddRange(sucursales)
        cmbSucursalFiltro.DisplayMember = "Texto"
        cmbSucursalFiltro.ValueMember = "SucursalId"
        cmbSucursalFiltro.DataSource = filtro
    End Sub

    Private Sub CargarComboSucursalEdicion()
        Dim lista = _servicio.ListarSucursalesActivasParaSelector(Nothing)
        cmbSucursal.DisplayMember = "Texto"
        cmbSucursal.ValueMember = "SucursalId"
        cmbSucursal.DataSource = New List(Of SucursalSelectorItem)(lista)
    End Sub

    Private Function ObtenerEmpresaFiltroId() As Integer?
        Dim it = TryCast(cmbEmpresaFiltro.SelectedItem, EmpresaSelectorItem)
        If it Is Nothing OrElse it.EmpresaId <= 0 Then Return Nothing
        Return it.EmpresaId
    End Function

    Private Function ObtenerSucursalFiltroId() As Integer?
        Dim it = TryCast(cmbSucursalFiltro.SelectedItem, SucursalSelectorItem)
        If it Is Nothing OrElse it.SucursalId <= 0 Then Return Nothing
        Return it.SucursalId
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarDepositos(txtBusqueda.Text, ObtenerEmpresaFiltroId(), ObtenerSucursalFiltroId())
            dgvDepositos.DataSource = Nothing
            dgvDepositos.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado de depósitos.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub cmbEmpresaFiltro_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEmpresaFiltro.SelectedIndexChanged
        If _suspendFiltros Then Return
        _suspendFiltros = True
        CargarComboSucursalFiltro()
        _suspendFiltros = False
        RefrescarListado()
    End Sub

    Private Sub cmbSucursalFiltro_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSucursalFiltro.SelectedIndexChanged
        If _suspendFiltros Then Return
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvDepositos.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _depositoEdicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvDepositos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvDepositos.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerDepositoIdSeleccionado()
        If id <= 0 Then Return
        CargarDepositoEnFormulario(id)
    End Sub

    Private Function ObtenerDepositoIdSeleccionado() As Integer
        If dgvDepositos.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvDepositos.CurrentRow.DataBoundItem, DepositoResumen)
        If item Is Nothing Then Return 0
        Return item.DepositoId
    End Function

    Private Sub AsegurarSucursalEnComboEdicion(deposito As Deposito)
        Dim lista = TryCast(cmbSucursal.DataSource, List(Of SucursalSelectorItem))
        If lista Is Nothing Then Return
        If lista.Any(Function(x) x.SucursalId = deposito.SucursalId) Then Return
        Dim extra = _servicio.ObtenerItemSucursalParaEdicion(deposito.SucursalId)
        If extra Is Nothing Then Return
        lista.Insert(0, extra)
        cmbSucursal.DataSource = Nothing
        cmbSucursal.DisplayMember = "Texto"
        cmbSucursal.ValueMember = "SucursalId"
        cmbSucursal.DataSource = lista
    End Sub

    Private Sub CargarDepositoEnFormulario(depositoId As Integer)
        Try
            Dim d = _servicio.ObtenerPorId(depositoId)
            If d Is Nothing Then
                MessageBox.Show("No se encontró el depósito.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _depositoEdicionId = d.DepositoId
            lblIdValor.Text = d.DepositoId.ToString()
            AsegurarSucursalEnComboEdicion(d)
            cmbSucursal.SelectedValue = d.SucursalId
            txtCodigo.Text = d.Codigo
            txtNombre.Text = d.Nombre
            txtDescripcion.Text = If(d.Descripcion, String.Empty)
            chkEsPrincipal.Checked = d.EsPrincipal
            chkActivo.Checked = d.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el depósito.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _depositoEdicionId = 0
        lblIdValor.Text = "—"
        CargarComboSucursalEdicion()
        If cmbSucursal.Items.Count > 0 Then
            cmbSucursal.SelectedIndex = 0
        End If
        txtCodigo.Clear()
        txtNombre.Clear()
        txtDescripcion.Clear()
        chkEsPrincipal.Checked = False
        chkActivo.Checked = True
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hayEdicion As Boolean = _depositoEdicionId > 0
        btnActivar.Enabled = hayEdicion AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hayEdicion AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Function ObtenerSucursalIdEdicion() As Integer
        Dim it = TryCast(cmbSucursal.SelectedItem, SucursalSelectorItem)
        If it Is Nothing Then Return 0
        Return it.SucursalId
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim aud = LoginAuditoria()
        Dim modelo As New Deposito With {
            .DepositoId = _depositoEdicionId,
            .SucursalId = ObtenerSucursalIdEdicion(),
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text,
            .Descripcion = txtDescripcion.Text,
            .EsPrincipal = chkEsPrincipal.Checked
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _depositoEdicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
            End If

            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _depositoEdicionId)
            RefrescarListado()
            SeleccionarFilaPorDepositoId(nuevoId)
            If nuevoId > 0 Then
                CargarDepositoEnFormulario(nuevoId)
            End If
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorDepositoId(depositoId As Integer)
        If depositoId <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvDepositos.Rows
                Dim item = TryCast(row.DataBoundItem, DepositoResumen)
                If item IsNot Nothing AndAlso item.DepositoId = depositoId Then
                    dgvDepositos.ClearSelection()
                    row.Selected = True
                    dgvDepositos.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerDepositoIdSeleccionado()
        If id > 0 Then
            CargarDepositoEnFormulario(id)
        Else
            LimpiarFormularioEdicion()
        End If
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _depositoEdicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este depósito?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_depositoEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorDepositoId(_depositoEdicionId)
        CargarDepositoEnFormulario(_depositoEdicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _depositoEdicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar este depósito?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_depositoEdicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorDepositoId(_depositoEdicionId)
        CargarDepositoEnFormulario(_depositoEdicionId)
    End Sub
End Class
