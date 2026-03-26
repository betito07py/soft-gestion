Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Soft_Gestion.Business
Imports Soft_Gestion.Domain

''' <summary>
''' Formulario principal MDI. Menú modular con claves estables (ClavesFormulario) para permisos.
''' </summary>
Public Partial Class FrmPrincipal

    Private ReadOnly _usuario As Usuario

    ''' <summary>Solo para el diseñador de Visual Studio.</summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(usuarioAutenticado As Usuario)
        InitializeComponent()
        _usuario = usuarioAutenticado
    End Sub

    Private Sub FrmPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SesionAplicacion.UsuarioActual = _usuario
        SesionAplicacion.CodigosPermisoDelUsuario.Clear()
        If _usuario IsNot Nothing AndAlso Not _usuario.EsAdministrador Then
            Dim svc As New PermisoEfectivoService()
            Dim codigos = svc.ObtenerCodigosPermisosEfectivos(_usuario.UsuarioId)
            SesionAplicacion.CodigosPermisoDelUsuario.UnionWith(codigos)
        End If

        Const tituloBase As String = "Soft Gestion"
        If _usuario IsNot Nothing Then
            Me.Text = tituloBase & " — " & _usuario.NombreCompleto & " (" & _usuario.Login & ")"
            StatusLabelUsuario.Text = "Usuario: " & _usuario.Login
        Else
            Me.Text = tituloBase
            StatusLabelUsuario.Text = "Usuario: (no definido)"
        End If

        AsignarClavesFormularioAlMenu()
        AplicarPoliticaMenuPorPermisos()
        ConfigurarMenuClasificacionProductos()
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub FrmPrincipal_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        SesionAplicacion.UsuarioActual = Nothing
        SesionAplicacion.CodigosPermisoDelUsuario.Clear()
    End Sub

    ''' <summary>
    ''' Asocia cada ítem hoja del menú con su clave estable. Más adelante AplicarPoliticaMenuPorPermisos usará estas claves.
    ''' </summary>
    Private Sub AsignarClavesFormularioAlMenu()
        MenuSeguridadUsuarios.Tag = ClavesFormulario.SeguridadUsuarios
        MenuSeguridadRoles.Tag = ClavesFormulario.SeguridadRoles
        MenuSeguridadPermisos.Tag = ClavesFormulario.SeguridadPermisos
        MenuSeguridadRolPermisos.Tag = ClavesFormulario.SeguridadRolPermisos
        MenuMaestrosEmpresas.Tag = ClavesFormulario.MaestrosEmpresas
        MenuMaestrosSucursales.Tag = ClavesFormulario.MaestrosSucursales
        MenuMaestrosDepositos.Tag = ClavesFormulario.MaestrosDepositos
        MenuMaestrosClientes.Tag = ClavesFormulario.MaestrosClientes
        MenuMaestrosProveedores.Tag = ClavesFormulario.MaestrosProveedores
        MenuMaestrosCategorias.Tag = ClavesFormulario.MaestrosCategorias
        MenuMaestrosSubCategorias.Tag = ClavesFormulario.MaestrosSubCategorias
        MenuMaestrosGrupos.Tag = ClavesFormulario.MaestrosGrupos
        MenuMaestrosMarcas.Tag = ClavesFormulario.MaestrosMarcas
        MenuMaestrosUnidadesMedida.Tag = ClavesFormulario.MaestrosUnidadesMedida
        MenuMaestrosProductos.Tag = ClavesFormulario.MaestrosProductos
        MenuStockVista.Tag = ClavesFormulario.StockModulo
        MenuComprasVista.Tag = ClavesFormulario.ComprasModulo
        MenuVentasVista.Tag = ClavesFormulario.VentasModulo
        MenuCuentasCobrarVista.Tag = ClavesFormulario.CuentasCobrarModulo
        MenuCuentasPagarVista.Tag = ClavesFormulario.CuentasPagarModulo
        MenuTesoreriaVista.Tag = ClavesFormulario.TesoreriaModulo
    End Sub

    ''' <summary>
    ''' Habilita o deshabilita ítems con Tag del menú según SesionAplicacion.CodigosPermisoDelUsuario.
    ''' Los Tag deben coincidir con dbo.Permisos.Codigo (mismos valores que ClavesFormulario).
    ''' Sin permisos asignados, los ítems con clave quedan deshabilitados. Los administradores no reciben restricción.
    ''' </summary>
    Protected Overridable Sub AplicarPoliticaMenuPorPermisos()
        If SesionAplicacion.UsuarioActual IsNot Nothing AndAlso SesionAplicacion.UsuarioActual.EsAdministrador Then Return
        Dim codigos = SesionAplicacion.CodigosPermisoDelUsuario
        AplicarPermisosASubmenu(MenuStrip1.Items, codigos)
    End Sub

    ''' <summary>Habilita el ítem combinado solo si el usuario tiene los tres permisos de maestros de clasificación.</summary>
    Private Sub ConfigurarMenuClasificacionProductos()
        Dim ok = SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosCategorias) AndAlso
            SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosSubCategorias) AndAlso
            SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosGrupos)
        MenuMaestrosClasificacionProductos.Enabled = ok
    End Sub

    Private Shared Sub AplicarPermisosASubmenu(items As ToolStripItemCollection, codigos As ISet(Of String))
        For Each obj As Object In items
            Dim mi = TryCast(obj, ToolStripMenuItem)
            If mi Is Nothing Then Continue For
            If mi.HasDropDownItems Then
                AplicarPermisosASubmenu(mi.DropDownItems, codigos)
                mi.Enabled = TieneSubitemHabilitado(mi)
            ElseIf mi.Tag IsNot Nothing Then
                Dim clave = TryCast(mi.Tag, String)
                If clave IsNot Nothing Then mi.Enabled = codigos.Contains(clave)
            End If
        Next
    End Sub

    Private Shared Function TieneSubitemHabilitado(menu As ToolStripMenuItem) As Boolean
        For Each ch As ToolStripItem In menu.DropDownItems
            Dim m = TryCast(ch, ToolStripMenuItem)
            If m Is Nothing Then Continue For
            If m.HasDropDownItems Then
                If TieneSubitemHabilitado(m) Then Return True
            ElseIf m.Enabled Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub MenuItemFormulario_Click(sender As Object, e As EventArgs) Handles _
        MenuSeguridadUsuarios.Click, MenuSeguridadRoles.Click, MenuSeguridadPermisos.Click, MenuSeguridadRolPermisos.Click,
        MenuMaestrosEmpresas.Click, MenuMaestrosSucursales.Click, MenuMaestrosDepositos.Click,
        MenuMaestrosClientes.Click, MenuMaestrosProveedores.Click, MenuMaestrosCategorias.Click, MenuMaestrosSubCategorias.Click, MenuMaestrosGrupos.Click, MenuMaestrosMarcas.Click, MenuMaestrosUnidadesMedida.Click, MenuMaestrosProductos.Click,
        MenuStockVista.Click, MenuComprasVista.Click, MenuVentasVista.Click,
        MenuCuentasCobrarVista.Click, MenuCuentasPagarVista.Click, MenuTesoreriaVista.Click

        Dim item = TryCast(sender, ToolStripMenuItem)
        If item Is Nothing OrElse item.Tag Is Nothing Then Return
        Dim clave = item.Tag.ToString()
        If Not SesionAplicacion.UsuarioTienePermiso(clave) Then
            MessageBox.Show("No tiene permiso para acceder a esta opción.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim tituloVentana As String = TituloVentanaParaMenuItem(item)
        If clave = ClavesFormulario.SeguridadUsuarios Then
            AbrirOActivarFrmUsuarios(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.SeguridadRoles Then
            AbrirOActivarFrmRoles(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.SeguridadPermisos Then
            AbrirOActivarFrmPermisos(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.SeguridadRolPermisos Then
            AbrirOActivarFrmRolPermisos(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosEmpresas Then
            AbrirOActivarFrmEmpresas(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosSucursales Then
            AbrirOActivarFrmSucursales(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosDepositos Then
            AbrirOActivarFrmDepositos(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosClientes Then
            AbrirOActivarFrmClientes(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosProveedores Then
            AbrirOActivarFrmProveedores(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosCategorias Then
            AbrirOActivarFrmCategorias(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosSubCategorias Then
            AbrirOActivarFrmSubCategorias(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosGrupos Then
            AbrirOActivarFrmGrupos(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosMarcas Then
            AbrirOActivarFrmMarcas(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosUnidadesMedida Then
            AbrirOActivarFrmUnidadesMedida(tituloVentana)
            Return
        End If
        If clave = ClavesFormulario.MaestrosProductos Then
            AbrirOActivarFrmProductos(tituloVentana)
            Return
        End If
        AbrirOActivarPlaceholder(tituloVentana, clave)
    End Sub

    Private Sub AbrirOActivarFrmUsuarios(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmUsuarios Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmUsuarios()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmRoles(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmRoles Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmRoles()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmPermisos(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmPermisos Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmPermisos()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmRolPermisos(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmRolPermisos Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmRolPermisos()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmEmpresas(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmEmpresas Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmEmpresas()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmSucursales(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmSucursales Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmSucursales()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmDepositos(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmDepositos Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmDepositos()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmClientes(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmClientes Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmClientes()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmProveedores(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmProveedores Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmProveedores()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmCategorias(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmCategorias Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmCategorias()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmSubCategorias(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmSubCategorias Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmSubCategorias()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmGrupos(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmGrupos Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmGrupos()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmMarcas(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmMarcas Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmMarcas()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmUnidadesMedida(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmUnidadesMedida Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmUnidadesMedida()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub AbrirOActivarFrmProductos(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmProductos Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmProductos()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Sub MenuMaestrosClasificacionProductos_Click(sender As Object, e As EventArgs) Handles MenuMaestrosClasificacionProductos.Click
        If Not SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosCategorias) OrElse
            Not SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosSubCategorias) OrElse
            Not SesionAplicacion.UsuarioTienePermiso(ClavesFormulario.MaestrosGrupos) Then
            MessageBox.Show("No tiene permiso para acceder a esta opción.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        AbrirOActivarFrmClasificacionProductos("Maestros — Clasificación de productos")
    End Sub

    Private Sub AbrirOActivarFrmClasificacionProductos(tituloVentana As String)
        For Each frm As Form In MdiChildren
            If TypeOf frm Is FrmClasificacionProductos Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next
        Dim hijo As New FrmClasificacionProductos()
        hijo.MdiParent = Me
        hijo.Text = tituloVentana
        hijo.Show()
    End Sub

    Private Shared Function TituloVentanaParaMenuItem(item As ToolStripMenuItem) As String
        Dim parent = TryCast(item.OwnerItem, ToolStripMenuItem)
        If parent Is Nothing Then Return item.Text.Trim()

        Dim esVistaGeneral As Boolean = String.Equals(item.Text.Trim(), "Vista general", StringComparison.OrdinalIgnoreCase)
        If esVistaGeneral Then
            Return parent.Text.Trim()
        End If

        Return parent.Text.Trim() & " — " & item.Text.Trim()
    End Function

    Private Sub AbrirOActivarPlaceholder(tituloVentana As String, claveFormulario As String)
        For Each frm As Form In MdiChildren
            Dim ph = TryCast(frm, FrmPlaceholder)
            If ph IsNot Nothing AndAlso ph.ClaveFormulario = claveFormulario Then
                frm.BringToFront()
                frm.WindowState = FormWindowState.Normal
                Return
            End If
        Next

        Dim hijo As New FrmPlaceholder(tituloVentana, claveFormulario)
        hijo.MdiParent = Me
        hijo.Show()
    End Sub

    Private Sub MenuCascada_Click(sender As Object, e As EventArgs) Handles MenuCascada.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub MenuMosaicoH_Click(sender As Object, e As EventArgs) Handles MenuMosaicoH.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub MenuMosaicoV_Click(sender As Object, e As EventArgs) Handles MenuMosaicoV.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub MenuCerrarTodas_Click(sender As Object, e As EventArgs) Handles MenuCerrarTodas.Click
        For Each frm As Form In MdiChildren
            frm.Close()
        Next
    End Sub
End Class
