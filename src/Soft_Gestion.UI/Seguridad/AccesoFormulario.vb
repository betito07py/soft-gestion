Imports System.Windows.Forms

''' <summary>
''' Comprueba permiso al cargar formularios sensibles (defensa ante apertura directa o reordenación de llamadas).
''' </summary>
Public NotInheritable Class AccesoFormulario
    Private Sub New()
    End Sub

    ''' <returns><c>True</c> si puede continuar el <c>Load</c>; <c>False</c> si se cerrará el formulario.</returns>
    Public Shared Function ExigirPermisoAlCargar(formulario As Form, clavePermiso As String) As Boolean
        If formulario Is Nothing Then Return False
        If SesionAplicacion.UsuarioTienePermiso(clavePermiso) Then Return True
        MessageBox.Show("No tiene permiso para acceder a esta pantalla.", formulario.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
        formulario.BeginInvoke(New MethodInvoker(Sub() formulario.Close()))
        Return False
    End Function
End Class
