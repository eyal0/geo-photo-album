<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MainTab = New System.Windows.Forms.TabControl()
        Me.SortCsvTab = New System.Windows.Forms.TabPage()
        Me.btnDestFile = New System.Windows.Forms.Button()
        Me.btnSourceDir = New System.Windows.Forms.Button()
        Me.lblDestination = New System.Windows.Forms.Label()
        Me.txtDestination = New System.Windows.Forms.TextBox()
        Me.btnDestDir = New System.Windows.Forms.Button()
        Me.btnSourceFile = New System.Windows.Forms.Button()
        Me.lblSource = New System.Windows.Forms.Label()
        Me.txtSource = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.MainTab.SuspendLayout()
        Me.SortCsvTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainTab
        '
        Me.MainTab.Controls.Add(Me.SortCsvTab)
        Me.MainTab.Controls.Add(Me.TabPage2)
        Me.MainTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainTab.Location = New System.Drawing.Point(0, 0)
        Me.MainTab.Name = "MainTab"
        Me.MainTab.SelectedIndex = 0
        Me.MainTab.Size = New System.Drawing.Size(562, 373)
        Me.MainTab.TabIndex = 0
        '
        'SortCsvTab
        '
        Me.SortCsvTab.Controls.Add(Me.btnDestFile)
        Me.SortCsvTab.Controls.Add(Me.btnSourceDir)
        Me.SortCsvTab.Controls.Add(Me.lblDestination)
        Me.SortCsvTab.Controls.Add(Me.txtDestination)
        Me.SortCsvTab.Controls.Add(Me.btnDestDir)
        Me.SortCsvTab.Controls.Add(Me.btnSourceFile)
        Me.SortCsvTab.Controls.Add(Me.lblSource)
        Me.SortCsvTab.Controls.Add(Me.txtSource)
        Me.SortCsvTab.Location = New System.Drawing.Point(4, 22)
        Me.SortCsvTab.Name = "SortCsvTab"
        Me.SortCsvTab.Padding = New System.Windows.Forms.Padding(3)
        Me.SortCsvTab.Size = New System.Drawing.Size(554, 347)
        Me.SortCsvTab.TabIndex = 0
        Me.SortCsvTab.Text = "Sort CSV"
        Me.SortCsvTab.UseVisualStyleBackColor = True
        '
        'btnDestFile
        '
        Me.btnDestFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDestFile.Location = New System.Drawing.Point(430, 35)
        Me.btnDestFile.Name = "btnDestFile"
        Me.btnDestFile.Size = New System.Drawing.Size(55, 23)
        Me.btnDestFile.TabIndex = 7
        Me.btnDestFile.Text = "File..."
        Me.btnDestFile.UseVisualStyleBackColor = True
        '
        'btnSourceDir
        '
        Me.btnSourceDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSourceDir.Location = New System.Drawing.Point(491, 6)
        Me.btnSourceDir.Name = "btnSourceDir"
        Me.btnSourceDir.Size = New System.Drawing.Size(55, 23)
        Me.btnSourceDir.TabIndex = 6
        Me.btnSourceDir.Text = "Folder..."
        Me.btnSourceDir.UseVisualStyleBackColor = True
        '
        'lblDestination
        '
        Me.lblDestination.AutoSize = True
        Me.lblDestination.Location = New System.Drawing.Point(8, 40)
        Me.lblDestination.Name = "lblDestination"
        Me.lblDestination.Size = New System.Drawing.Size(60, 13)
        Me.lblDestination.TabIndex = 5
        Me.lblDestination.Text = "Destination"
        '
        'txtDestination
        '
        Me.txtDestination.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDestination.Location = New System.Drawing.Point(85, 37)
        Me.txtDestination.Name = "txtDestination"
        Me.txtDestination.Size = New System.Drawing.Size(339, 20)
        Me.txtDestination.TabIndex = 4
        '
        'btnDestDir
        '
        Me.btnDestDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDestDir.Location = New System.Drawing.Point(491, 35)
        Me.btnDestDir.Name = "btnDestDir"
        Me.btnDestDir.Size = New System.Drawing.Size(55, 23)
        Me.btnDestDir.TabIndex = 3
        Me.btnDestDir.Text = "Folder..."
        Me.btnDestDir.UseVisualStyleBackColor = True
        '
        'btnSourceFile
        '
        Me.btnSourceFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSourceFile.Location = New System.Drawing.Point(430, 6)
        Me.btnSourceFile.Name = "btnSourceFile"
        Me.btnSourceFile.Size = New System.Drawing.Size(55, 23)
        Me.btnSourceFile.TabIndex = 2
        Me.btnSourceFile.Text = "File..."
        Me.btnSourceFile.UseVisualStyleBackColor = True
        '
        'lblSource
        '
        Me.lblSource.AutoSize = True
        Me.lblSource.Location = New System.Drawing.Point(8, 11)
        Me.lblSource.Name = "lblSource"
        Me.lblSource.Size = New System.Drawing.Size(41, 13)
        Me.lblSource.TabIndex = 1
        Me.lblSource.Text = "Source"
        '
        'txtSource
        '
        Me.txtSource.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSource.Location = New System.Drawing.Point(85, 8)
        Me.txtSource.Name = "txtSource"
        Me.txtSource.Size = New System.Drawing.Size(339, 20)
        Me.txtSource.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(554, 347)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(562, 373)
        Me.Controls.Add(Me.MainTab)
        Me.Name = "MainForm"
        Me.Text = "GeoPhotoAlbums"
        Me.MainTab.ResumeLayout(False)
        Me.SortCsvTab.ResumeLayout(False)
        Me.SortCsvTab.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainTab As System.Windows.Forms.TabControl
    Friend WithEvents SortCsvTab As System.Windows.Forms.TabPage
    Friend WithEvents lblDestination As System.Windows.Forms.Label
    Friend WithEvents txtDestination As System.Windows.Forms.TextBox
    Friend WithEvents btnDestDir As System.Windows.Forms.Button
    Friend WithEvents btnSourceFile As System.Windows.Forms.Button
    Friend WithEvents lblSource As System.Windows.Forms.Label
    Friend WithEvents txtSource As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnSourceDir As System.Windows.Forms.Button
    Friend WithEvents btnDestFile As System.Windows.Forms.Button

End Class
