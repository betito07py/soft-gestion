Imports System.Data.SqlClient
Imports System.Text
Imports System.Windows.Forms
Imports Soft_Gestion.Common

''' <summary>
''' Permite editar las cadenas SoftGestionPrincipal y ConfigServer del exe.config (patrón similar a Aux_Pgs).
''' </summary>
Public Partial Class FrmConexiones
    Private _plantillaCadena As String = String.Empty
    Private _sqlPasswordGuardado As String

    Private Sub FrmConexiones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbNombre.Items.Clear()
        cmbNombre.Items.Add(New EntradaConexion(ConfiguracionConexion.NombreConexionPrincipal, "Principal (SoftGestionPrincipal)"))
        cmbNombre.Items.Add(New EntradaConexion(ConfiguracionConexion.NombreConexionConfigServer, "ConfigServer (opcional)"))
        cmbNombre.DisplayMember = "Titulo"
        cmbNombre.ValueMember = "Nombre"
        cmbNombre.SelectedIndex = 0
    End Sub

    Private Sub CmbNombre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNombre.SelectedIndexChanged
        If cmbNombre.SelectedIndex < 0 Then Return
        CargarDesdeArchivo()
    End Sub

    Private Sub ChkIntegrada_CheckedChanged(sender As Object, e As EventArgs) Handles chkIntegrada.CheckedChanged
        AjustarCamposSqlAuth()
    End Sub

    Private Sub BtnProbar_Click(sender As Object, e As EventArgs) Handles btnProbar.Click
        Dim err = GestorCadenasConexionLocal.ProbarConexion(ConstruirCadenaActual())
        If err Is Nothing Then
            MessageBox.Show("Conexión correcta.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show(err, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            Dim nombre = NombreSeleccionado()
            GestorCadenasConexionLocal.GuardarCadenaEnArchivoLocal(nombre, ConstruirCadenaActual())
            _plantillaCadena = GestorCadenasConexionLocal.LeerCadenaDesdeArchivoLocal(nombre)
            MessageBox.Show("La cadena se guardó en el archivo de configuración del ejecutable.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Function NombreSeleccionado() As String
        Return DirectCast(cmbNombre.SelectedItem, EntradaConexion).Nombre
    End Function

    Private Sub CargarDesdeArchivo()
        Try
            Dim cad = GestorCadenasConexionLocal.LeerCadenaDesdeArchivoLocal(NombreSeleccionado())
            _plantillaCadena = cad
            CargarCamposDesdeCadena(cad)
        Catch ex As Exception
            MessageBox.Show(ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            _plantillaCadena = String.Empty
            txtServidor.Clear()
            txtBaseDatos.Clear()
            chkIntegrada.Checked = True
            txtUsuario.Clear()
            txtClaveSql.Clear()
            _sqlPasswordGuardado = Nothing
            chkTrust.Checked = False
            AjustarCamposSqlAuth()
        End Try
    End Sub

    Private Sub CargarCamposDesdeCadena(cadena As String)
        Try
            Dim b As New SqlConnectionStringBuilder(cadena)
            txtServidor.Text = b.DataSource
            txtBaseDatos.Text = b.InitialCatalog
            chkIntegrada.Checked = b.IntegratedSecurity
            If b.IntegratedSecurity Then
                txtUsuario.Clear()
                _sqlPasswordGuardado = Nothing
            Else
                txtUsuario.Text = b.UserID
                _sqlPasswordGuardado = b.Password
            End If
            txtClaveSql.Clear()
            chkTrust.Checked = CadenaTieneTrustTrue(cadena)
            AjustarCamposSqlAuth()
        Catch ex As ArgumentException
            MessageBox.Show("No se pudo interpretar la cadena de conexión: " & ex.Message, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub AjustarCamposSqlAuth()
        Dim habilitarSql = Not chkIntegrada.Checked
        lblUsuario.Enabled = habilitarSql
        txtUsuario.Enabled = habilitarSql
        lblClaveSql.Enabled = habilitarSql
        txtClaveSql.Enabled = habilitarSql
    End Sub

    Private Function ConstruirCadenaActual() As String
        Dim b As SqlConnectionStringBuilder
        Try
            b = New SqlConnectionStringBuilder(_plantillaCadena)
        Catch
            b = New SqlConnectionStringBuilder()
        End Try

        b.DataSource = txtServidor.Text.Trim()
        b.InitialCatalog = txtBaseDatos.Text.Trim()

        If chkIntegrada.Checked Then
            b.IntegratedSecurity = True
            If b.ContainsKey("User ID") Then b.Remove("User ID")
            If b.ContainsKey("Password") Then b.Remove("Password")
        Else
            b.IntegratedSecurity = False
            b.UserID = txtUsuario.Text.Trim()
            If txtClaveSql.TextLength > 0 Then
                b.Password = txtClaveSql.Text
            ElseIf _sqlPasswordGuardado IsNot Nothing Then
                b.Password = _sqlPasswordGuardado
            Else
                b.Password = String.Empty
            End If
        End If

        Dim s = QuitarTrust(b.ConnectionString)
        If chkTrust.Checked Then
            If s.Length > 0 AndAlso Not s.EndsWith(";"c) Then s &= ";"
            s &= "TrustServerCertificate=True"
        End If
        Return s
    End Function

    Private Shared Function CadenaTieneTrustTrue(cadena As String) As Boolean
        If String.IsNullOrEmpty(cadena) Then Return False
        Return cadena.IndexOf("TrustServerCertificate=True", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
            cadena.IndexOf("Trust Server Certificate=True", StringComparison.OrdinalIgnoreCase) >= 0
    End Function

    Private Shared Function QuitarTrust(connectionString As String) As String
        If String.IsNullOrEmpty(connectionString) Then Return String.Empty
        Dim partes = connectionString.Split(";"c)
        Dim sb As New StringBuilder()
        For Each p In partes
            Dim t = p.Trim()
            If t.Length = 0 Then Continue For
            If t.StartsWith("TrustServerCertificate=", StringComparison.OrdinalIgnoreCase) Then Continue For
            If t.StartsWith("Trust Server Certificate=", StringComparison.OrdinalIgnoreCase) Then Continue For
            If sb.Length > 0 Then sb.Append(";"c)
            sb.Append(t)
        Next
        Return sb.ToString()
    End Function

    Private NotInheritable Class EntradaConexion
        Public Property Nombre As String
        Public Property Titulo As String

        Public Sub New(nombre As String, titulo As String)
            Me.Nombre = nombre
            Me.Titulo = titulo
        End Sub
    End Class
End Class
