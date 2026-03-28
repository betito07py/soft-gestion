Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de impuestos. Usa ImpuestoService (capa Business).
''' </summary>
Public Partial Class FrmImpuestos

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        MaestroListaEncabezadoHelper.AplicarEncabezadoGrilla(Me, pnlListaMaestro, lblTituloGrilla, dgvImpuestos, "Impuestos")
    End Sub

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosImpuestos
        End Get
    End Property

    Private ReadOnly _servicio As New ImpuestoService()
    Private _edicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False

    Private Sub FrmImpuestos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        LimpiarFormularioEdicion()
        RefrescarListado()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvImpuestos.AutoGenerateColumns = False
        dgvImpuestos.Columns.Clear()
        dgvImpuestos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ImpuestoId", .HeaderText = "Id", .Name = "colId", .Width = 44})
        dgvImpuestos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .Width = 56})
        dgvImpuestos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .MinimumWidth = 120, .FillWeight = 120})
        dgvImpuestos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "TipoImpuesto", .HeaderText = "Tipo", .Name = "colTipo", .Width = 70})
        dgvImpuestos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Porcentaje", .HeaderText = "%", .Name = "colPct", .Width = 48,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "N2"}})
        dgvImpuestos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "EsExento", .HeaderText = "Exento", .Name = "colEx", .Width = 52})
        dgvImpuestos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "EsActivo", .HeaderText = "Activo", .Name = "colAct", .Width = 52})
    End Sub

    Private Function SoloActivosListado() As Boolean
        Return chkSoloActivos.Checked
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.Listar(txtBusqueda.Text, SoloActivosListado())
            dgvImpuestos.DataSource = Nothing
            dgvImpuestos.DataSource = lista
        Catch ex As Exception
            Dim detalle = If(ex.InnerException IsNot Nothing, ex.Message & Environment.NewLine & ex.InnerException.Message, ex.Message)
            MessageBox.Show("No se pudo cargar el listado de impuestos." & Environment.NewLine & detalle, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub chkSoloActivos_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloActivos.CheckedChanged
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvImpuestos.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _edicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvImpuestos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvImpuestos.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerImpuestoIdSeleccionado()
        If id <= 0 Then Return
        CargarEnFormulario(id)
    End Sub

    Private Function ObtenerImpuestoIdSeleccionado() As Integer
        If dgvImpuestos.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvImpuestos.CurrentRow.DataBoundItem, ImpuestoResumen)
        If item Is Nothing Then Return 0
        Return item.ImpuestoId
    End Function

    Private Sub CargarEnFormulario(impuestoId As Integer)
        Try
            Dim x = _servicio.ObtenerPorId(impuestoId)
            If x Is Nothing Then
                MessageBox.Show("No se encontró el impuesto.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _edicionId = x.ImpuestoId
            lblIdValor.Text = x.ImpuestoId.ToString()
            txtCodigo.Text = x.Codigo.ToString(CultureInfo.CurrentCulture)
            txtNombre.Text = x.Nombre
            txtTipoImpuesto.Text = x.TipoImpuesto
            txtPorcentaje.Text = x.Porcentaje.ToString("N2", CultureInfo.CurrentCulture)
            chkEsExento.Checked = x.EsExento
            txtCodigoSIFEN.Text = If(x.CodigoSIFEN.HasValue, x.CodigoSIFEN.Value.ToString(CultureInfo.CurrentCulture), String.Empty)
            chkEsActivo.Checked = x.EsActivo
            AplicarEstadoExentoEnControles()
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el impuesto.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _edicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtNombre.Clear()
        txtTipoImpuesto.Text = "IVA"
        txtPorcentaje.Text = "0"
        chkEsExento.Checked = False
        txtCodigoSIFEN.Clear()
        chkEsActivo.Checked = True
        AplicarEstadoExentoEnControles()
        ActualizarBotonesEstado()
    End Sub

    Private Sub chkEsExento_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsExento.CheckedChanged
        AplicarEstadoExentoEnControles()
    End Sub

    Private Sub AplicarEstadoExentoEnControles()
        If chkEsExento.Checked Then
            txtPorcentaje.Text = "0"
            txtPorcentaje.Enabled = False
        Else
            txtPorcentaje.Enabled = True
        End If
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hay As Boolean = _edicionId > 0
        btnActivar.Enabled = hay AndAlso Not chkEsActivo.Checked
        btnDesactivar.Enabled = hay AndAlso chkEsActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Function ParseCodigoEntero() As Integer?
        Dim s = txtCodigo.Text.Trim()
        If s.Length = 0 Then
            MessageBox.Show("Indique el código numérico del impuesto.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If
        Dim n As Integer
        If Not Integer.TryParse(s, NumberStyles.Integer, CultureInfo.CurrentCulture, n) Then
            If Not Integer.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, n) Then
                MessageBox.Show("El código debe ser un número entero válido.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return Nothing
            End If
        End If
        Return n
    End Function

    Private Function ParsePorcentaje() As Decimal?
        If chkEsExento.Checked Then Return 0D
        Dim s = txtPorcentaje.Text.Trim()
        If s.Length = 0 Then
            MessageBox.Show("Indique el porcentaje.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If
        Dim d As Decimal
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, d) Then Return d
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, d) Then Return d
        MessageBox.Show("El porcentaje no es válido.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return Nothing
    End Function

    Private Function TryParseCodigoSifenOpcional(ByRef valor As Short?) As Boolean
        valor = Nothing
        Dim s = txtCodigoSIFEN.Text.Trim()
        If s.Length = 0 Then Return True
        Dim n As Integer
        If Not Integer.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, n) Then
            MessageBox.Show("Código SIFEN debe ser un número entero (opcional).", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If n < Short.MinValue OrElse n > Short.MaxValue Then
            MessageBox.Show("Código SIFEN fuera de rango.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        valor = CShort(n)
        Return True
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim cod = ParseCodigoEntero()
        If Not cod.HasValue Then Return
        Dim pct = ParsePorcentaje()
        If Not pct.HasValue Then Return
        Dim sifen As Short? = Nothing
        If Not TryParseCodigoSifenOpcional(sifen) Then Return

        Dim modelo As New Impuesto With {
            .ImpuestoId = _edicionId,
            .Codigo = cod.Value,
            .Nombre = txtNombre.Text,
            .TipoImpuesto = txtTipoImpuesto.Text,
            .Porcentaje = pct.Value,
            .EsExento = chkEsExento.Checked,
            .CodigoSIFEN = sifen
        }

        btnGuardar.Enabled = False
        Try
            Dim aud = LoginAuditoria()
            Dim res = _servicio.Guardar(modelo, aud)
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
            For Each row As DataGridViewRow In dgvImpuestos.Rows
                Dim item = TryCast(row.DataBoundItem, ImpuestoResumen)
                If item IsNot Nothing AndAlso item.ImpuestoId = id Then
                    dgvImpuestos.ClearSelection()
                    row.Selected = True
                    dgvImpuestos.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerImpuestoIdSeleccionado()
        If id > 0 Then CargarEnFormulario(id) Else LimpiarFormularioEdicion()
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _edicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este impuesto?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Activar(_edicionId, LoginAuditoria())
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
        If MessageBox.Show("¿Desactivar este impuesto?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
        Dim res = _servicio.Desactivar(_edicionId, LoginAuditoria())
        If Not res.Exitoso Then
            MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        RefrescarListado()
        SeleccionarFilaPorId(_edicionId)
        CargarEnFormulario(_edicionId)
    End Sub
End Class
