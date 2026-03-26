Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' ABM de productos. Sin SQL en la UI; usa ProductoService (capa Business).
''' </summary>
Public Partial Class FrmProductos

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.MaestrosProductos
        End Get
    End Property

    Private ReadOnly _servicio As New ProductoService()
    Private _edicionId As Integer = 0
    Private _suspendSeleccion As Boolean = False
    Private _suspendEdicionCombos As Boolean = False
    Private _inicializando As Boolean = True

    Private Sub FrmProductos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        txtSifenCodigo.BackColor = System.Drawing.SystemColors.Control
        txtSifenDescrip.BackColor = System.Drawing.SystemColors.Control
        txtSifenRepr.BackColor = System.Drawing.SystemColors.Control
        ConfigurarGrilla()
        ConfigurarComboActivoFiltro()
        CargarComboFiltroCategoria()
        CargarComboFiltroSubCategoria()
        CargarComboFiltroGrupo()
        CargarComboFiltroMarca()
        CargarComboEdicionCategoria()
        CargarComboEdicionSubCategoria(0)
        CargarComboEdicionGrupo(0)
        CargarComboEdicionMarca()
        CargarComboEdicionUnidad()
        LimpiarFormularioEdicion()
        RefrescarListado()
        _inicializando = False
    End Sub

    Private Sub ConfigurarGrilla()
        dgvProductos.AutoGenerateColumns = False
        dgvProductos.Columns.Clear()
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ProductoId", .HeaderText = "Id", .Name = "colId", .Width = 44})
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .MinimumWidth = 70, .FillWeight = 70})
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Descripcion", .HeaderText = "Descripción", .Name = "colDesc", .MinimumWidth = 120, .FillWeight = 140})
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "GrupoCodigo", .HeaderText = "Grupo", .Name = "colGrupo", .Width = 56})
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "MarcaCodigo", .HeaderText = "Marca", .Name = "colMarca", .Width = 52})
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "UnidadAbreviatura", .HeaderText = "Um.", .Name = "colUm", .Width = 40})
        dgvProductos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "PrecioBase", .HeaderText = "P. base", .Name = "colPrecio", .Width = 72,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "N2"}})
        dgvProductos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Activo", .HeaderText = "Act.", .Name = "colActivo", .Width = 42})
    End Sub

    Private Sub ConfigurarComboActivoFiltro()
        cmbFiltroActivo.Items.Clear()
        cmbFiltroActivo.Items.Add("Todos")
        cmbFiltroActivo.Items.Add("Solo activos")
        cmbFiltroActivo.Items.Add("Solo inactivos")
        cmbFiltroActivo.SelectedIndex = 0
    End Sub

    Private Function ObtenerActivoFiltro() As Boolean?
        Select Case cmbFiltroActivo.SelectedIndex
            Case 1
                Return True
            Case 2
                Return False
            Case Else
                Return Nothing
        End Select
    End Function

    Private Sub CargarComboFiltroCategoria()
        Dim cats = _servicio.ListarCategoriasActivasParaSelector()
        Dim lista As New List(Of CategoriaSelectorItem) From {
            New CategoriaSelectorItem With {.CategoriaId = 0, .Texto = "(Todas)"}
        }
        lista.AddRange(cats)
        cmbFiltroCategoria.DisplayMember = "Texto"
        cmbFiltroCategoria.ValueMember = "CategoriaId"
        cmbFiltroCategoria.DataSource = lista
        cmbFiltroCategoria.SelectedIndex = 0
    End Sub

    Private Sub CargarComboFiltroSubCategoria()
        Dim catId = ObtenerCategoriaFiltroId()
        Dim subs = _servicio.ListarSubCategoriasActivasParaSelector(catId)
        Dim lista As New List(Of SubCategoriaSelectorItem) From {
            New SubCategoriaSelectorItem With {.SubCategoriaId = 0, .Texto = "(Todas)"}
        }
        lista.AddRange(subs)
        cmbFiltroSubCategoria.DisplayMember = "Texto"
        cmbFiltroSubCategoria.ValueMember = "SubCategoriaId"
        cmbFiltroSubCategoria.DataSource = lista
        cmbFiltroSubCategoria.SelectedIndex = 0
    End Sub

    Private Sub CargarComboFiltroGrupo()
        Dim subId = ObtenerSubCategoriaFiltroId()
        Dim grps = _servicio.ListarGruposActivasParaSelector(subId)
        Dim lista As New List(Of GrupoSelectorItem) From {
            New GrupoSelectorItem With {.GrupoId = 0, .Texto = "(Todos)"}
        }
        lista.AddRange(grps)
        cmbFiltroGrupo.DisplayMember = "Texto"
        cmbFiltroGrupo.ValueMember = "GrupoId"
        cmbFiltroGrupo.DataSource = lista
        cmbFiltroGrupo.SelectedIndex = 0
    End Sub

    Private Sub CargarComboFiltroMarca()
        Dim marcas = _servicio.ListarMarcasActivasParaSelector()
        Dim lista As New List(Of MarcaSelectorItem) From {
            New MarcaSelectorItem With {.MarcaId = 0, .Texto = "(Todas)"}
        }
        lista.AddRange(marcas)
        cmbFiltroMarca.DisplayMember = "Texto"
        cmbFiltroMarca.ValueMember = "MarcaId"
        cmbFiltroMarca.DataSource = lista
        cmbFiltroMarca.SelectedIndex = 0
    End Sub

    Private Function ObtenerCategoriaFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroCategoria.SelectedItem, CategoriaSelectorItem)
        If it Is Nothing OrElse it.CategoriaId <= 0 Then Return Nothing
        Return it.CategoriaId
    End Function

    Private Function ObtenerSubCategoriaFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroSubCategoria.SelectedItem, SubCategoriaSelectorItem)
        If it Is Nothing OrElse it.SubCategoriaId <= 0 Then Return Nothing
        Return it.SubCategoriaId
    End Function

    Private Function ObtenerGrupoFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroGrupo.SelectedItem, GrupoSelectorItem)
        If it Is Nothing OrElse it.GrupoId <= 0 Then Return Nothing
        Return it.GrupoId
    End Function

    Private Function ObtenerMarcaFiltroId() As Integer?
        Dim it = TryCast(cmbFiltroMarca.SelectedItem, MarcaSelectorItem)
        If it Is Nothing OrElse it.MarcaId <= 0 Then Return Nothing
        Return it.MarcaId
    End Function

    Private Sub CargarComboEdicionCategoria()
        Dim cats = _servicio.ListarCategoriasActivasParaSelector()
        Dim lista As New List(Of CategoriaSelectorItem) From {
            New CategoriaSelectorItem With {.CategoriaId = 0, .Texto = "(Seleccione categoría)"}
        }
        lista.AddRange(cats)
        cmbCategoria.DisplayMember = "Texto"
        cmbCategoria.ValueMember = "CategoriaId"
        cmbCategoria.DataSource = lista
    End Sub

    Private Sub CargarComboEdicionSubCategoria(categoriaId As Integer)
        Dim lista As New List(Of SubCategoriaSelectorItem) From {
            New SubCategoriaSelectorItem With {.SubCategoriaId = 0, .Texto = "(Seleccione subcategoría)"}
        }
        If categoriaId > 0 Then
            lista.AddRange(_servicio.ListarSubCategoriasActivasParaSelector(categoriaId))
        End If
        cmbSubCategoria.DisplayMember = "Texto"
        cmbSubCategoria.ValueMember = "SubCategoriaId"
        cmbSubCategoria.DataSource = lista
    End Sub

    Private Sub CargarComboEdicionGrupo(subCategoriaId As Integer)
        Dim lista As New List(Of GrupoSelectorItem) From {
            New GrupoSelectorItem With {.GrupoId = 0, .Texto = "(Seleccione grupo)"}
        }
        If subCategoriaId > 0 Then
            lista.AddRange(_servicio.ListarGruposActivasParaSelector(subCategoriaId))
        End If
        cmbGrupo.DisplayMember = "Texto"
        cmbGrupo.ValueMember = "GrupoId"
        cmbGrupo.DataSource = lista
    End Sub

    Private Sub CargarComboEdicionMarca()
        Dim marcas = _servicio.ListarMarcasActivasParaSelector()
        Dim lista As New List(Of MarcaSelectorItem) From {
            New MarcaSelectorItem With {.MarcaId = 0, .Texto = "(Sin marca)"}
        }
        lista.AddRange(marcas)
        cmbMarca.DisplayMember = "Texto"
        cmbMarca.ValueMember = "MarcaId"
        cmbMarca.DataSource = lista
    End Sub

    Private Sub CargarComboEdicionUnidad()
        Dim uds = _servicio.ListarUnidadesActivasParaSelector()
        cmbUnidadMedida.DisplayMember = "Codigo"
        cmbUnidadMedida.ValueMember = "UnidadMedidaId"
        cmbUnidadMedida.DataSource = uds
    End Sub

    Private Sub AsegurarCategoriaEnCombo(categoriaId As Integer)
        Dim lista = TryCast(cmbCategoria.DataSource, List(Of CategoriaSelectorItem))
        If lista Is Nothing OrElse categoriaId <= 0 Then Return
        If lista.Any(Function(x) x.CategoriaId = categoriaId) Then Return
        Dim extra = _servicio.ObtenerItemCategoriaParaEdicion(categoriaId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        RefrescarDataSourceCombo(cmbCategoria, lista)
    End Sub

    Private Sub AsegurarSubCategoriaEnCombo(subCategoriaId As Integer)
        Dim lista = TryCast(cmbSubCategoria.DataSource, List(Of SubCategoriaSelectorItem))
        If lista Is Nothing OrElse subCategoriaId <= 0 Then Return
        If lista.Any(Function(x) x.SubCategoriaId = subCategoriaId) Then Return
        Dim extra = _servicio.ObtenerItemSubCategoriaParaEdicion(subCategoriaId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        RefrescarDataSourceCombo(cmbSubCategoria, lista)
    End Sub

    Private Sub AsegurarGrupoEnCombo(grupoId As Integer)
        Dim lista = TryCast(cmbGrupo.DataSource, List(Of GrupoSelectorItem))
        If lista Is Nothing OrElse grupoId <= 0 Then Return
        If lista.Any(Function(x) x.GrupoId = grupoId) Then Return
        Dim extra = _servicio.ObtenerItemGrupoParaEdicion(grupoId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        RefrescarDataSourceCombo(cmbGrupo, lista)
    End Sub

    Private Sub AsegurarMarcaEnCombo(marcaId As Integer)
        Dim lista = TryCast(cmbMarca.DataSource, List(Of MarcaSelectorItem))
        If lista Is Nothing OrElse marcaId <= 0 Then Return
        If lista.Any(Function(x) x.MarcaId = marcaId) Then Return
        Dim extra = _servicio.ObtenerItemMarcaParaEdicion(marcaId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        RefrescarDataSourceCombo(cmbMarca, lista)
    End Sub

    Private Sub AsegurarUnidadEnCombo(unidadMedidaId As Integer)
        Dim lista = TryCast(cmbUnidadMedida.DataSource, List(Of UnidadMedidaResumen))
        If lista Is Nothing OrElse unidadMedidaId <= 0 Then Return
        If lista.Any(Function(x) x.UnidadMedidaId = unidadMedidaId) Then Return
        Dim extra = _servicio.ObtenerResumenUnidadParaEdicion(unidadMedidaId)
        If extra Is Nothing Then Return
        lista.Insert(1, extra)
        RefrescarDataSourceUnidad(lista)
    End Sub

    Private Shared Sub RefrescarDataSourceCombo(cmb As ComboBox, lista As Object)
        Dim dm = cmb.DisplayMember
        Dim vm = cmb.ValueMember
        cmb.DataSource = Nothing
        cmb.DisplayMember = dm
        cmb.ValueMember = vm
        cmb.DataSource = lista
    End Sub

    Private Sub RefrescarDataSourceUnidad(lista As List(Of UnidadMedidaResumen))
        cmbUnidadMedida.DataSource = Nothing
        cmbUnidadMedida.DisplayMember = "Codigo"
        cmbUnidadMedida.ValueMember = "UnidadMedidaId"
        cmbUnidadMedida.DataSource = lista
    End Sub

    Private Function ObtenerCategoriaIdEdicion() As Integer
        Dim it = TryCast(cmbCategoria.SelectedItem, CategoriaSelectorItem)
        If it Is Nothing Then Return 0
        Return it.CategoriaId
    End Function

    Private Function ObtenerSubCategoriaIdEdicion() As Integer
        Dim it = TryCast(cmbSubCategoria.SelectedItem, SubCategoriaSelectorItem)
        If it Is Nothing Then Return 0
        Return it.SubCategoriaId
    End Function

    Private Function ObtenerGrupoIdEdicion() As Integer
        Dim it = TryCast(cmbGrupo.SelectedItem, GrupoSelectorItem)
        If it Is Nothing Then Return 0
        Return it.GrupoId
    End Function

    Private Function ObtenerMarcaIdEdicion() As Integer?
        Dim it = TryCast(cmbMarca.SelectedItem, MarcaSelectorItem)
        If it Is Nothing OrElse it.MarcaId <= 0 Then Return Nothing
        Return it.MarcaId
    End Function

    Private Function ObtenerUnidadMedidaIdEdicion() As Integer
        Dim it = TryCast(cmbUnidadMedida.SelectedItem, UnidadMedidaResumen)
        If it Is Nothing Then Return 0
        Return it.UnidadMedidaId
    End Function

    Private Sub RefrescarListado()
        Try
            _suspendSeleccion = True
            Dim lista = _servicio.ListarProductos(txtBusqueda.Text,
                ObtenerCategoriaFiltroId(), ObtenerSubCategoriaFiltroId(), ObtenerGrupoFiltroId(),
                ObtenerMarcaFiltroId(), ObtenerActivoFiltro())
            dgvProductos.DataSource = Nothing
            dgvProductos.DataSource = lista
        Catch ex As Exception
            Dim detalle = If(ex.Message, String.Empty).Trim()
            If detalle.Length > 0 Then
                MessageBox.Show(
                    "No se pudo cargar el listado de productos." & vbCrLf & vbCrLf & detalle,
                    Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                MessageBox.Show("No se pudo cargar el listado de productos.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub cmbFiltroCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroCategoria.SelectedIndexChanged
        If _inicializando Then Return
        CargarComboFiltroSubCategoria()
        CargarComboFiltroGrupo()
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroSubCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroSubCategoria.SelectedIndexChanged
        If _inicializando Then Return
        CargarComboFiltroGrupo()
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroGrupo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroGrupo.SelectedIndexChanged
        If _inicializando Then Return
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroMarca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroMarca.SelectedIndexChanged
        If _inicializando Then Return
        RefrescarListado()
    End Sub

    Private Sub cmbFiltroActivo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFiltroActivo.SelectedIndexChanged
        If _inicializando Then Return
        RefrescarListado()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        RefrescarListado()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        _suspendSeleccion = True
        dgvProductos.ClearSelection()
        _suspendSeleccion = False
        LimpiarFormularioEdicion()
        _edicionId = 0
        lblIdValor.Text = "(nuevo)"
        txtCodigo.Focus()
        ActualizarBotonesEstado()
    End Sub

    Private Sub dgvProductos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvProductos.SelectionChanged
        If _suspendSeleccion Then Return
        Dim id = ObtenerProductoIdSeleccionado()
        If id <= 0 Then Return
        CargarEnFormulario(id)
    End Sub

    Private Function ObtenerProductoIdSeleccionado() As Integer
        If dgvProductos.CurrentRow Is Nothing Then Return 0
        Dim item = TryCast(dgvProductos.CurrentRow.DataBoundItem, ProductoResumen)
        If item Is Nothing Then Return 0
        Return item.ProductoId
    End Function

    Private Sub CargarEnFormulario(productoId As Integer)
        Try
            Dim p = _servicio.ObtenerPorId(productoId)
            If p Is Nothing Then
                MessageBox.Show("No se encontró el producto.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            _suspendEdicionCombos = True
            _edicionId = p.ProductoId
            lblIdValor.Text = p.ProductoId.ToString()
            txtCodigo.Text = p.Codigo
            txtCodigoBarras.Text = If(p.CodigoBarras, String.Empty)
            txtDescripcion.Text = p.Descripcion
            txtCostoUltimo.Text = p.CostoUltimo.ToString("N4", CultureInfo.CurrentCulture)
            txtPrecioBase.Text = p.PrecioBase.ToString("N4", CultureInfo.CurrentCulture)
            txtPorcentajeIVA.Text = p.PorcentajeIVA.ToString("N2", CultureInfo.CurrentCulture)
            chkPermiteStockNegativo.Checked = p.PermiteStockNegativo
            chkControlaStock.Checked = p.ControlaStock
            chkEsServicio.Checked = p.EsServicio
            AplicarEstadoServicioEnControles()
            txtObservaciones.Text = If(p.Observaciones, String.Empty)
            chkActivo.Checked = p.Activo

            Dim g As Grupo = Nothing
            If p.GrupoId.HasValue AndAlso p.GrupoId.Value > 0 Then
                g = _servicio.ObtenerGrupoPorId(p.GrupoId.Value)
            End If

            CargarComboEdicionCategoria()
            If g IsNot Nothing Then
                AsegurarCategoriaEnCombo(g.CategoriaId)
                cmbCategoria.SelectedValue = g.CategoriaId
                CargarComboEdicionSubCategoria(g.CategoriaId)
                AsegurarSubCategoriaEnCombo(g.SubCategoriaId)
                cmbSubCategoria.SelectedValue = g.SubCategoriaId
                CargarComboEdicionGrupo(g.SubCategoriaId)
                AsegurarGrupoEnCombo(g.GrupoId)
                cmbGrupo.SelectedValue = g.GrupoId
            Else
                cmbCategoria.SelectedValue = 0
                CargarComboEdicionSubCategoria(0)
                CargarComboEdicionGrupo(0)
            End If

            CargarComboEdicionMarca()
            If p.MarcaId.HasValue AndAlso p.MarcaId.Value > 0 Then
                AsegurarMarcaEnCombo(p.MarcaId.Value)
                cmbMarca.SelectedValue = p.MarcaId.Value
            Else
                cmbMarca.SelectedValue = 0
            End If

            CargarComboEdicionUnidad()
            AsegurarUnidadEnCombo(p.UnidadMedidaId)
            cmbUnidadMedida.SelectedValue = p.UnidadMedidaId

            ActualizarSifenDesdeUnidadCombo()
            ActualizarBotonesEstado()
        Catch
            MessageBox.Show("No se pudo cargar el producto.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendEdicionCombos = False
        End Try
    End Sub

    Private Sub LimpiarFormularioEdicion()
        _suspendEdicionCombos = True
        _edicionId = 0
        lblIdValor.Text = "—"
        txtCodigo.Clear()
        txtCodigoBarras.Clear()
        txtDescripcion.Clear()
        txtCostoUltimo.Text = "0"
        txtPrecioBase.Text = "0"
        txtPorcentajeIVA.Text = "0"
        chkPermiteStockNegativo.Checked = False
        chkControlaStock.Checked = True
        chkEsServicio.Checked = False
        AplicarEstadoServicioEnControles()
        txtObservaciones.Clear()
        chkActivo.Checked = True
        LimpiarCamposSifen()
        CargarComboEdicionCategoria()
        cmbCategoria.SelectedValue = 0
        CargarComboEdicionSubCategoria(0)
        CargarComboEdicionGrupo(0)
        CargarComboEdicionMarca()
        cmbMarca.SelectedValue = 0
        CargarComboEdicionUnidad()
        If cmbUnidadMedida.Items.Count > 0 Then
            cmbUnidadMedida.SelectedIndex = 0
        End If
        _suspendEdicionCombos = False
        ActualizarBotonesEstado()
    End Sub

    Private Sub LimpiarCamposSifen()
        txtSifenCodigo.Clear()
        txtSifenDescrip.Clear()
        txtSifenRepr.Clear()
    End Sub

    Private Sub ActualizarSifenDesdeUnidadCombo()
        Dim it = TryCast(cmbUnidadMedida.SelectedItem, UnidadMedidaResumen)
        If it Is Nothing Then
            LimpiarCamposSifen()
            Return
        End If
        If it.Codigo_SIFEN.HasValue Then
            txtSifenCodigo.Text = it.Codigo_SIFEN.Value.ToString(CultureInfo.CurrentCulture)
        Else
            txtSifenCodigo.Clear()
        End If
        txtSifenDescrip.Text = If(it.Descrip_Unidad_SIFEN, String.Empty)
        txtSifenRepr.Text = If(it.Codigo_Repr_SIFEN, String.Empty)
    End Sub

    Private Sub cmbCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategoria.SelectedIndexChanged
        If _inicializando OrElse _suspendEdicionCombos Then Return
        Dim cid = ObtenerCategoriaIdEdicion()
        CargarComboEdicionSubCategoria(cid)
        If cmbSubCategoria.Items.Count > 0 Then cmbSubCategoria.SelectedIndex = 0
        Dim sid = ObtenerSubCategoriaIdEdicion()
        CargarComboEdicionGrupo(sid)
        If cmbGrupo.Items.Count > 0 Then cmbGrupo.SelectedIndex = 0
    End Sub

    Private Sub cmbSubCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubCategoria.SelectedIndexChanged
        If _inicializando OrElse _suspendEdicionCombos Then Return
        Dim sid = ObtenerSubCategoriaIdEdicion()
        CargarComboEdicionGrupo(sid)
        If cmbGrupo.Items.Count > 0 Then cmbGrupo.SelectedIndex = 0
    End Sub

    Private Sub cmbUnidadMedida_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUnidadMedida.SelectedIndexChanged
        ActualizarSifenDesdeUnidadCombo()
    End Sub

    Private Sub chkEsServicio_CheckedChanged(sender As Object, e As EventArgs) Handles chkEsServicio.CheckedChanged
        AplicarEstadoServicioEnControles()
    End Sub

    Private Sub AplicarEstadoServicioEnControles()
        If chkEsServicio.Checked Then
            chkControlaStock.Checked = False
            chkControlaStock.Enabled = False
        Else
            chkControlaStock.Enabled = True
        End If
    End Sub

    Private Sub ActualizarBotonesEstado()
        Dim hay As Boolean = _edicionId > 0
        btnActivar.Enabled = hay AndAlso Not chkActivo.Checked
        btnDesactivar.Enabled = hay AndAlso chkActivo.Checked
    End Sub

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Function ParseDecimalObligatorio(txt As TextBox, nombreCampo As String) As Decimal?
        Dim s = txt.Text.Trim()
        If s.Length = 0 Then
            MessageBox.Show("Indique un valor para " & nombreCampo & ".", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If
        Dim d As Decimal
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, d) Then Return d
        If Decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, d) Then Return d
        MessageBox.Show(nombreCampo & " no es un número válido.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Return Nothing
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim costo = ParseDecimalObligatorio(txtCostoUltimo, "Costo último")
        If Not costo.HasValue Then Return
        Dim precio = ParseDecimalObligatorio(txtPrecioBase, "Precio base")
        If Not precio.HasValue Then Return
        Dim iva = ParseDecimalObligatorio(txtPorcentajeIVA, "Porcentaje IVA")
        If Not iva.HasValue Then Return

        Dim grupoSel = ObtenerGrupoIdEdicion()
        Dim umId = ObtenerUnidadMedidaIdEdicion()
        If umId <= 0 Then
            MessageBox.Show("Debe seleccionar una unidad de medida.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        Dim modelo As New Producto With {
            .ProductoId = _edicionId,
            .Codigo = txtCodigo.Text,
            .CodigoBarras = txtCodigoBarras.Text,
            .Descripcion = txtDescripcion.Text,
            .GrupoId = If(grupoSel > 0, CType(grupoSel, Integer?), Nothing),
            .MarcaId = ObtenerMarcaIdEdicion(),
            .UnidadMedidaId = umId,
            .CostoUltimo = costo.Value,
            .PrecioBase = precio.Value,
            .PorcentajeIVA = iva.Value,
            .PermiteStockNegativo = chkPermiteStockNegativo.Checked,
            .ControlaStock = chkControlaStock.Checked,
            .EsServicio = chkEsServicio.Checked,
            .Observaciones = txtObservaciones.Text
        }

        btnGuardar.Enabled = False
        Try
            Dim aud = LoginAuditoria()
            Dim res As ResultadoOperacion
            If _edicionId = 0 Then
                res = _servicio.GuardarNuevo(modelo, aud)
            Else
                res = _servicio.EditarExistente(modelo, aud)
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
            For Each row As DataGridViewRow In dgvProductos.Rows
                Dim item = TryCast(row.DataBoundItem, ProductoResumen)
                If item IsNot Nothing AndAlso item.ProductoId = id Then
                    dgvProductos.ClearSelection()
                    row.Selected = True
                    dgvProductos.CurrentCell = row.Cells(0)
                    Exit For
                End If
            Next
        Finally
            _suspendSeleccion = False
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Dim id = ObtenerProductoIdSeleccionado()
        If id > 0 Then CargarEnFormulario(id) Else LimpiarFormularioEdicion()
    End Sub

    Private Sub btnActivar_Click(sender As Object, e As EventArgs) Handles btnActivar.Click
        If _edicionId <= 0 Then Return
        If MessageBox.Show("¿Activar este producto?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
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
        If MessageBox.Show("¿Desactivar este producto?", Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return
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
