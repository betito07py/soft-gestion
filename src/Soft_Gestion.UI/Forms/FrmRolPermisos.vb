Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' Asignación de permisos a roles. Sin SQL en la UI.
''' </summary>
Public Partial Class FrmRolPermisos

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        MaestroListaEncabezadoHelper.AplicarEncabezadoGrilla(Me, pnlListaMaestro, lblTituloGrilla, dgvPermisos, "Permisos del rol")
    End Sub

    Public Shared ReadOnly Property ClaveFormularioPermiso As String
        Get
            Return ClavesFormulario.SeguridadRolPermisos
        End Get
    End Property

    Private ReadOnly _servicioRolPermiso As New RolPermisoService()
    Private ReadOnly _servicioRol As New RolService()
    Private _suspendRol As Boolean = False

    Private Sub FrmRolPermisos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not AccesoFormulario.ExigirPermisoAlCargar(Me, ClaveFormularioPermiso) Then Return
        ConfigurarGrilla()
        CargarRolesCombo()
    End Sub

    Private Sub ConfigurarGrilla()
        dgvPermisos.AutoGenerateColumns = False
        dgvPermisos.Columns.Clear()
        dgvPermisos.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "Asignado", .HeaderText = "Asignado", .Name = "colAsignado", .Width = 70})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Codigo", .HeaderText = "Código", .Name = "colCodigo", .ReadOnly = True, .MinimumWidth = 100, .FillWeight = 120})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "colNombre", .ReadOnly = True, .MinimumWidth = 120, .FillWeight = 160})
        dgvPermisos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Modulo", .HeaderText = "Módulo", .Name = "colModulo", .ReadOnly = True, .MinimumWidth = 80, .FillWeight = 100})
        dgvPermisos.RowHeadersVisible = False
        dgvPermisos.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPermisos.MultiSelect = False
    End Sub

    Private Sub CargarRolesCombo()
        Try
            _suspendRol = True
            Dim todos = _servicioRol.ListarRoles(Nothing)
            Dim activos = todos.Where(Function(r) r.Activo).OrderBy(Function(r) r.Nombre).ToList()
            cmbRol.DataSource = activos
            cmbRol.DisplayMember = "Nombre"
            cmbRol.ValueMember = "RolId"
        Catch
            MessageBox.Show("No se pudo cargar la lista de roles.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _suspendRol = False
        End Try
        RefrescarGrillaSegunRol()
    End Sub

    Private Sub cmbRol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbRol.SelectedIndexChanged
        If _suspendRol Then Return
        RefrescarGrillaSegunRol()
    End Sub

    Private Sub RefrescarGrillaSegunRol()
        Dim rolId = ObtenerRolIdSeleccionado()
        If rolId <= 0 Then
            dgvPermisos.DataSource = Nothing
            Return
        End If
        Try
            Dim lineas = _servicioRolPermiso.ObtenerLineasParaAsignacion(rolId)
            dgvPermisos.DataSource = Nothing
            dgvPermisos.DataSource = lineas
        Catch
            MessageBox.Show("No se pudo cargar los permisos del rol.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function ObtenerRolIdSeleccionado() As Integer
        If cmbRol.SelectedValue Is Nothing Then Return 0
        Try
            Return CInt(cmbRol.SelectedValue)
        Catch
            Return 0
        End Try
    End Function

    Private Function LoginAuditoria() As String
        If SesionAplicacion.UsuarioActual Is Nothing Then Return "SISTEMA"
        Return SesionAplicacion.UsuarioActual.Login.Trim()
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim rolId = ObtenerRolIdSeleccionado()
        If rolId <= 0 Then
            MessageBox.Show("Seleccione un rol.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim seleccionados As New List(Of Integer)()
        For Each row As DataGridViewRow In dgvPermisos.Rows
            If row.IsNewRow Then Continue For
            Dim item = TryCast(row.DataBoundItem, PermisoLineaAsignacion)
            If item IsNot Nothing AndAlso item.Asignado Then
                seleccionados.Add(item.PermisoId)
            End If
        Next

        btnGuardar.Enabled = False
        Try
            Dim res = _servicioRolPermiso.GuardarAsignacionCompleta(rolId, seleccionados, LoginAuditoria())
            If Not res.Exitoso Then
                MessageBox.Show(res.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            MessageBox.Show("La asignación se guardó correctamente.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            RefrescarGrillaSegunRol()
        Finally
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Sub btnRefrescar_Click(sender As Object, e As EventArgs) Handles btnRefrescar.Click
        RefrescarGrillaSegunRol()
    End Sub
End Class
