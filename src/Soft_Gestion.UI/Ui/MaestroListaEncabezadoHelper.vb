Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' Encabezado de grilla alineado con <see cref="FrmClasificacionProductos"/> (barra gris 64 + texto blanco Segoe UI 9 negrita).
''' </summary>
Friend NotInheritable Class MaestroListaEncabezadoHelper
    Private Sub New()
    End Sub

    Friend Shared Sub AplicarEncabezadoGrilla(host As Form, pnlLista As Panel, lblTitulo As Label, dgv As DataGridView, titulo As String)
        Dim ix = host.Controls.IndexOf(dgv)
        If ix < 0 Then ix = 0

        lblTitulo.BackColor = Color.FromArgb(64, 64, 64)
        lblTitulo.Dock = DockStyle.Top
        lblTitulo.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point)
        lblTitulo.ForeColor = Color.White
        lblTitulo.Padding = New Padding(8, 0, 0, 0)
        lblTitulo.TextAlign = ContentAlignment.MiddleLeft
        lblTitulo.Text = titulo
        lblTitulo.Height = 26

        host.Controls.Remove(dgv)
        dgv.Dock = DockStyle.Fill

        pnlLista.SuspendLayout()
        pnlLista.Controls.Clear()
        pnlLista.Controls.Add(dgv)
        pnlLista.Controls.Add(lblTitulo)
        pnlLista.Dock = DockStyle.Fill
        pnlLista.ResumeLayout(False)

        host.Controls.Add(pnlLista)
        host.Controls.SetChildIndex(pnlLista, ix)
    End Sub
End Class
