Imports System.Windows.Forms

''' <summary>
''' Formulario hijo temporal hasta implementar cada módulo. La clave de formulario enlaza con permisos futuros.
''' </summary>
Public Partial Class FrmPlaceholder

    Private ReadOnly _claveFormulario As String

    Public ReadOnly Property ClaveFormulario As String
        Get
            Return _claveFormulario
        End Get
    End Property

    ''' <summary>Solo para el diseñador de Visual Studio.</summary>
    Public Sub New()
        InitializeComponent()
        _claveFormulario = String.Empty
    End Sub

    Public Sub New(tituloVentana As String, claveFormulario As String)
        InitializeComponent()
        _claveFormulario = claveFormulario
        Me.Text = tituloVentana
        lblMensaje.Text = "Módulo en desarrollo." & Environment.NewLine & Environment.NewLine &
            "Clave de formulario (permisos): " & claveFormulario
    End Sub

End Class
