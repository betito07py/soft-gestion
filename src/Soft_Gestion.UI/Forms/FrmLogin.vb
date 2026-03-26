Imports Soft_Gestion.Business
Imports System.Windows.Forms

''' <summary>
''' Formulario de autenticación. Valida credenciales contra UsuarioService y abre el formulario principal.
''' </summary>
Public Partial Class FrmLogin
    Private ReadOnly _usuarioService As New UsuarioService()

    Private Sub BtnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        btnIngresar.Enabled = False
        Try
            Dim resultado = _usuarioService.ValidarCredenciales(txtLogin.Text, txtPassword.Text)

            If Not resultado.Exitoso Then
                MessageBox.Show(resultado.Mensaje, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtPassword.Clear()
                txtPassword.Focus()
                Return
            End If

            Me.Hide()
            Dim frmPrincipal As New FrmPrincipal(resultado.Usuario)
            AddHandler frmPrincipal.FormClosed, AddressOf FrmPrincipal_FormClosed
            frmPrincipal.Show()
        Catch ex As Exception
            Dim detalle As String = If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message)
            MessageBox.Show(
                "No se pudo validar el acceso (error al conectar o consultar la base de datos)." & vbCrLf & vbCrLf &
                "Revise en App.config la cadena SoftGestionPrincipal (servidor, base SoftGestion, que exista dbo.Usuarios y datos de seed)." & vbCrLf & vbCrLf &
                "Detalle: " & detalle,
                Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnIngresar.Enabled = True
        End Try
    End Sub

    Private Sub FrmPrincipal_FormClosed(sender As Object, e As FormClosedEventArgs)
        Me.Close()
    End Sub

    Private Sub BtnConexiones_Click(sender As Object, e As EventArgs) Handles btnConexiones.Click
        Using f As New FrmConexiones()
            f.ShowDialog(Me)
        End Using
    End Sub
End Class
