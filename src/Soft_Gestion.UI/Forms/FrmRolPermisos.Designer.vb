<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmRolPermisos
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlSuperior = New System.Windows.Forms.Panel()
        Me.btnRefrescar = New System.Windows.Forms.Button()
        Me.cmbRol = New System.Windows.Forms.ComboBox()
        Me.lblRol = New System.Windows.Forms.Label()
        Me.pnlListaMaestro = New System.Windows.Forms.Panel()
        Me.lblTituloGrilla = New System.Windows.Forms.Label()
        Me.dgvPermisos = New System.Windows.Forms.DataGridView()
        Me.pnlInferior = New System.Windows.Forms.Panel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.pnlSuperior.SuspendLayout()
        Me.pnlListaMaestro.SuspendLayout()
        CType(Me.dgvPermisos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlInferior.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSuperior
        '
        Me.pnlSuperior.Controls.Add(Me.btnRefrescar)
        Me.pnlSuperior.Controls.Add(Me.cmbRol)
        Me.pnlSuperior.Controls.Add(Me.lblRol)
        Me.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSuperior.Location = New System.Drawing.Point(0, 0)
        Me.pnlSuperior.Name = "pnlSuperior"
        Me.pnlSuperior.Padding = New System.Windows.Forms.Padding(8, 8, 8, 4)
        Me.pnlSuperior.Size = New System.Drawing.Size(884, 44)
        Me.pnlSuperior.TabIndex = 0
        '
        'btnRefrescar
        '
        Me.btnRefrescar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRefrescar.Location = New System.Drawing.Point(797, 8)
        Me.btnRefrescar.Name = "btnRefrescar"
        Me.btnRefrescar.Size = New System.Drawing.Size(75, 23)
        Me.btnRefrescar.TabIndex = 2
        Me.btnRefrescar.Text = "Refrescar"
        Me.btnRefrescar.UseVisualStyleBackColor = True
        '
        'cmbRol
        '
        Me.cmbRol.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRol.FormattingEnabled = True
        Me.cmbRol.Location = New System.Drawing.Point(44, 10)
        Me.cmbRol.Name = "cmbRol"
        Me.cmbRol.Size = New System.Drawing.Size(747, 21)
        Me.cmbRol.TabIndex = 1
        '
        'lblRol
        '
        Me.lblRol.AutoSize = True
        Me.lblRol.Location = New System.Drawing.Point(11, 13)
        Me.lblRol.Name = "lblRol"
        Me.lblRol.Size = New System.Drawing.Size(23, 13)
        Me.lblRol.TabIndex = 0
        Me.lblRol.Text = "Rol"
        '
        'dgvPermisos
        '
        Me.dgvPermisos.AllowUserToAddRows = False
        Me.dgvPermisos.AllowUserToDeleteRows = False
        Me.dgvPermisos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPermisos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvPermisos.Location = New System.Drawing.Point(0, 44)
        Me.dgvPermisos.Name = "dgvPermisos"
        Me.dgvPermisos.Size = New System.Drawing.Size(884, 473)
        Me.dgvPermisos.TabIndex = 1
        '
        'pnlInferior
        '
        Me.pnlInferior.Controls.Add(Me.btnGuardar)
        Me.pnlInferior.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlInferior.Location = New System.Drawing.Point(0, 517)
        Me.pnlInferior.Name = "pnlInferior"
        Me.pnlInferior.Padding = New System.Windows.Forms.Padding(8, 4, 8, 8)
        Me.pnlInferior.Size = New System.Drawing.Size(884, 44)
        Me.pnlInferior.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(797, 8)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(75, 28)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'FrmRolPermisos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 561)
        Me.Controls.Add(Me.dgvPermisos)
        Me.Controls.Add(Me.pnlInferior)
        Me.Controls.Add(Me.pnlSuperior)
        Me.MinimizeBox = False
        Me.Name = "FrmRolPermisos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Permisos por rol"
        Me.pnlSuperior.ResumeLayout(False)
        Me.pnlSuperior.PerformLayout()
        CType(Me.dgvPermisos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlListaMaestro.ResumeLayout(False)
        Me.pnlInferior.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlSuperior As Panel
    Friend WithEvents btnRefrescar As Button
    Friend WithEvents cmbRol As ComboBox
    Friend WithEvents lblRol As Label
    Friend WithEvents pnlListaMaestro As Panel
    Friend WithEvents lblTituloGrilla As Label
    Friend WithEvents dgvPermisos As DataGridView
    Friend WithEvents pnlInferior As Panel
    Friend WithEvents btnGuardar As Button
End Class
