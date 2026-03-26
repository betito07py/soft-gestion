Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de unidades de medida. Selector SIFEN desde catálogo (sin formulario de mantenimiento SIFEN).
''' </summary>
Public Partial Class FrmUnidadesMedida

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosUnidadesMedida
        End Get
    End Property

    Private ReadOnly _servicio As New UnidadMedidaService()
    Private _edicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmUnidadesMedida_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarComboSifen()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvUnidadesMedida.AutoGenerateColumns = False
        dgvUnidadesMedida.Columns.Clear()
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "UnidadMedidaId", .HeaderText = "Id", .Name = "colId", .Width = 48})
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 70})
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 120, .FillWeight = 160})
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Abreviatura", .HeaderText = "Abrev.", .Name = "colAbrev", .Width = 60})
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo_SIFEN", .HeaderText = "SIFEN", .Name = "colSifen", .Width = 52})
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Descrip_Unidad_SIFEN", .HeaderText = "Unidad SIFEN", .Name = "colSifenDesc", .MinimumWidth = 90, .FillWeight = 80})
        dgvUnidadesMedida.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo_Repr_SIFEN", .HeaderText = "Repr. SIFEN", .Name = "colSifenRepr", .Width = 85})
        dgvUnidadesMedida.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Activo", .Name = "colActivo", .Width = 55})
    End Sub

    Private Sub CargarComboSifen()
        Dim lista As New List(Of UnidadMedidaSifenSelectorItem) From {
            New UnidadMedidaSifenSelectorItem With {.CodigoSifen = Nothing, .DescripUnidad = String.Empty, .CodigoRepr = String.Empty, .Texto = "(Sin código SIFEN)"}
        }
        Try
            lista.AddRange(_servicio.ListarCodigosSifenParaSelector())
        Catch
            MessageBox.Show("No se pudo cargar el catálogo SIFEN.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
        cmbCodigoSifen.DisplayMember = "Texto"
        cmbCodigoSifen.DataSource = lista
        cmbCodigoSifen.SelectedIndex = 0
    End Sub

    Private Sub SeleccionarCodigoSifenEnCombo(codigo As Short?)
        Dim lista = TryCast(cmbCodigoSifen.DataSource, List(Of UnidadMedidaSifenSelectorItem))
        If lista Is Nothing Then Return
        If codigo.HasValue AndAlso Not lista.Any(Function(x) x.CodigoSifen.HasValue AndAlso x.CodigoSifen.Value = codigo.Value) Then
            lista.Add(New UnidadMedidaSifenSelectorItem With {
                .CodigoSifen = codigo.Value,
                .DescripUnidad = String.Empty,
                .CodigoRepr = String.Empty,
                .Texto = codigo.Value.ToString() & " — (valor actual; no figura en catálogo cargado)"
            })
            cmbCodigoSifen.DataSource = Nothing
            cmbCodigoSifen.DisplayMember = "Texto"
            cmbCodigoSifen.DataSource = lista
        End If
        For i As Integer = 0 To cmbCodigoSifen.Items.Count - 1
            Dim it = TryCast(cmbCodigoSifen.Items(i), UnidadMedidaSifenSelectorItem)
            If it Is Nothing Then Continue For
            Dim matchSin = Not codigo.HasValue AndAlso Not it.CodigoSifen.HasValue
            Dim matchCon = codigo.HasValue AndAlso it.CodigoSifen.HasValue AndAlso it.CodigoSifen.Value = codigo.Value
            If matchSin OrElse matchCon Then
                cmbCodigoSifen.SelectedIndex = i
                Return
            End If
        Next
        cmbCodigoSifen.SelectedIndex = 0
    End Sub

    Private Function ObtenerCodigoSifenSeleccionado() As Short?
        Dim it = TryCast(cmbCodigoSifen.SelectedItem, UnidadMedidaSifenSelectorItem)
        If it Is Nothing Then Return Nothing
        Return it.CodigoSifen
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarUnidadesMedida(txtBusqueda.Text)
            dgvUnidadesMedida.DataSource = Nothing
            dgvUnidadesMedida.DataSource = lista
        Catch
            MessageBox.Show("No se pudo cargar el listado.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvUnidadesMedida.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _edicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvUnidadesMedida_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUnidadesMedida.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerIdSeleccionado()
        If id <= 0 Then Return
        CargarEnFormulario(id)
    End Sub

    Private Function ObtenerIdSeleccionado() As Integer
        If dgvUnidadesMedida.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvUnidadesMedida.CurrentRow.DataBoundItem, UnidadMedidaResumen)
        If item Is Nothing Then Return 0
        Return item.UnidadMedidaId
    End Function

    Private Sub CargarEnFormulario(unidadMedidaId As Integer)
        Try
            Dim u = _servicio.ObtenerPorId(unidadMedidaId)
            If u Is Nothing Then
                MessageBox.Show("No se encontró el registro.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _edicionId = u.UnidadMedidaId
            lblIdValor.Text = u.UnidadMedidaId.ToString()
            txtCodigo.Text = u.Codigo
            txtNombre.Text = u.Nombre
            txtAbreviatura.Text = u.Abreviatura
            CargarComboSifen()
            SeleccionarCodigoSifenEnCombo(u.Codigo_SIFEN)
            chkActivo.Checked = u.Activo
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el registro.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _edicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtNombre.Clear()
        txtAbreviatura.Clear()
        CargarComboSifen()
        ActualizarBotonesEstado()
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hay As Boolean = _edicionId > 0
        btnActivar.Enabled = hay AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hay AndAlso chkActivo.Checked
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim modelo As New UnidadMedida With {
            .UnidadMedidaId = _edicionId,
            .Codigo = txtCodigo.Text,
            .Nombre = txtNombre.Text,
            .Abreviatura = txtAbreviatura.Text,
            .Codigo_SIFEN = ObtenerCodigoSifenSeleccionado()
        }

        btnGuardar.Enabled = False
        Try
            Dim res As ResultadoOperacion
            If _edicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo)
            Else
                res = _servicio.EditarExistente(modelo)
            End If
            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Dim nuevoId As Integer = If(res.IdGenerado.HasValue, res.IdGenerado.Value, _edicionId)
            RefrescarListado()
            SeleccionarFilaPorId(nuevoId)
            If nuevoId > 0 Then CargarEnFormulario(nuevoId)
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub SeleccionarFilaPorId(id As Integer)
        If id <= 0 Then Return
        _suspendSeleccion = True
        Try
            For Each row As DataGridViewRow In dgvUnidadesMedida.Rows
                Dim item = TryCast(row.DataBoundItem, UnidadMedidaResumen)
                If item IsNot Nothing AndAlso item.UnidadMedidaId = id Then
                    dgvUnidadesMedida.ClearSelection()
                    row.Selected = True
                    dgvUnidadesMedida.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerIdSeleccionado()
        If id > 0 Then CargarEnFormulario(id) Else LimpiarFormularioEdicion()
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _edicionId <= 0 Then Return
        If MessageBox.Show("¿Activar esta unidad de medida?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_edicionId)
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorId(_edicionId)
        CargarEnFormulario(_edicionId)
    End Sub

    Private Sub btnDesactivar_Click(sender As Object, e As EventArgs) Handles btnDesactivar.Click
        If _edicionId <= 0 Then Return
        If MessageBox.Show("¿Desactivar esta unidad de medida?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_edicionId)
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorId(_edicionId)
        CargarEnFormulario(_edicionId)
    End Sub
End Class
