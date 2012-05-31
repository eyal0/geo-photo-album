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
        Me.TabSortCsv = New System.Windows.Forms.TabPage()
        Me.btnSortDestFile = New System.Windows.Forms.Button()
        Me.btnSortSrcDir = New System.Windows.Forms.Button()
        Me.lblSortDest = New System.Windows.Forms.Label()
        Me.txtSortDest = New System.Windows.Forms.TextBox()
        Me.btnSortDestDir = New System.Windows.Forms.Button()
        Me.btnSortSrcFile = New System.Windows.Forms.Button()
        Me.lblSortSrc = New System.Windows.Forms.Label()
        Me.txtSortSrc = New System.Windows.Forms.TextBox()
        Me.TabFilterCsv = New System.Windows.Forms.TabPage()
        Me.btnSort = New System.Windows.Forms.Button()
        Me.btnFilterSrcDir = New System.Windows.Forms.Button()
        Me.btnFilterSrcFile = New System.Windows.Forms.Button()
        Me.lblFilterSrc = New System.Windows.Forms.Label()
        Me.txtFilterSrc = New System.Windows.Forms.TextBox()
        Me.btnFilterDestFile = New System.Windows.Forms.Button()
        Me.lblFilterDest = New System.Windows.Forms.Label()
        Me.txtFilterDest = New System.Windows.Forms.TextBox()
        Me.btnFilterDestDir = New System.Windows.Forms.Button()
        Me.btnFilter = New System.Windows.Forms.Button()
        Me.MainTab.SuspendLayout()
        Me.TabSortCsv.SuspendLayout()
        Me.TabFilterCsv.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainTab
        '
        Me.MainTab.Controls.Add(Me.TabSortCsv)
        Me.MainTab.Controls.Add(Me.TabFilterCsv)
        Me.MainTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainTab.Location = New System.Drawing.Point(0, 0)
        Me.MainTab.Name = "MainTab"
        Me.MainTab.SelectedIndex = 0
        Me.MainTab.Size = New System.Drawing.Size(821, 373)
        Me.MainTab.TabIndex = 0
        '
        'TabSortCsv
        '
        Me.TabSortCsv.Controls.Add(Me.btnSort)
        Me.TabSortCsv.Controls.Add(Me.btnSortDestFile)
        Me.TabSortCsv.Controls.Add(Me.btnSortSrcDir)
        Me.TabSortCsv.Controls.Add(Me.lblSortDest)
        Me.TabSortCsv.Controls.Add(Me.txtSortDest)
        Me.TabSortCsv.Controls.Add(Me.btnSortDestDir)
        Me.TabSortCsv.Controls.Add(Me.btnSortSrcFile)
        Me.TabSortCsv.Controls.Add(Me.lblSortSrc)
        Me.TabSortCsv.Controls.Add(Me.txtSortSrc)
        Me.TabSortCsv.Location = New System.Drawing.Point(4, 22)
        Me.TabSortCsv.Name = "TabSortCsv"
        Me.TabSortCsv.Padding = New System.Windows.Forms.Padding(3)
        Me.TabSortCsv.Size = New System.Drawing.Size(813, 347)
        Me.TabSortCsv.TabIndex = 0
        Me.TabSortCsv.Text = "Sort CSV"
        Me.TabSortCsv.UseVisualStyleBackColor = True
        '
        'btnSortDestFile
        '
        Me.btnSortDestFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortDestFile.Location = New System.Drawing.Point(691, 35)
        Me.btnSortDestFile.Name = "btnSortDestFile"
        Me.btnSortDestFile.Size = New System.Drawing.Size(55, 23)
        Me.btnSortDestFile.TabIndex = 7
        Me.btnSortDestFile.Text = "File..."
        Me.btnSortDestFile.UseVisualStyleBackColor = True
        '
        'btnSortSrcDir
        '
        Me.btnSortSrcDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortSrcDir.Location = New System.Drawing.Point(752, 6)
        Me.btnSortSrcDir.Name = "btnSortSrcDir"
        Me.btnSortSrcDir.Size = New System.Drawing.Size(55, 23)
        Me.btnSortSrcDir.TabIndex = 6
        Me.btnSortSrcDir.Text = "Folder..."
        Me.btnSortSrcDir.UseVisualStyleBackColor = True
        '
        'lblSortDest
        '
        Me.lblSortDest.AutoSize = True
        Me.lblSortDest.Location = New System.Drawing.Point(8, 40)
        Me.lblSortDest.Name = "lblSortDest"
        Me.lblSortDest.Size = New System.Drawing.Size(60, 13)
        Me.lblSortDest.TabIndex = 5
        Me.lblSortDest.Text = "Destination"
        '
        'txtSortDest
        '
        Me.txtSortDest.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSortDest.Location = New System.Drawing.Point(85, 37)
        Me.txtSortDest.Name = "txtSortDest"
        Me.txtSortDest.Size = New System.Drawing.Size(600, 20)
        Me.txtSortDest.TabIndex = 4
        Me.txtSortDest.Text = "C:\Users\Eyal\Documents\coding\geo-photo-albums\sorted_csv"
        '
        'btnSortDestDir
        '
        Me.btnSortDestDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortDestDir.Location = New System.Drawing.Point(752, 35)
        Me.btnSortDestDir.Name = "btnSortDestDir"
        Me.btnSortDestDir.Size = New System.Drawing.Size(55, 23)
        Me.btnSortDestDir.TabIndex = 3
        Me.btnSortDestDir.Text = "Folder..."
        Me.btnSortDestDir.UseVisualStyleBackColor = True
        '
        'btnSortSrcFile
        '
        Me.btnSortSrcFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortSrcFile.Location = New System.Drawing.Point(691, 6)
        Me.btnSortSrcFile.Name = "btnSortSrcFile"
        Me.btnSortSrcFile.Size = New System.Drawing.Size(55, 23)
        Me.btnSortSrcFile.TabIndex = 2
        Me.btnSortSrcFile.Text = "File..."
        Me.btnSortSrcFile.UseVisualStyleBackColor = True
        '
        'lblSortSrc
        '
        Me.lblSortSrc.AutoSize = True
        Me.lblSortSrc.Location = New System.Drawing.Point(8, 11)
        Me.lblSortSrc.Name = "lblSortSrc"
        Me.lblSortSrc.Size = New System.Drawing.Size(41, 13)
        Me.lblSortSrc.TabIndex = 1
        Me.lblSortSrc.Text = "Source"
        '
        'txtSortSrc
        '
        Me.txtSortSrc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSortSrc.Location = New System.Drawing.Point(85, 8)
        Me.txtSortSrc.Name = "txtSortSrc"
        Me.txtSortSrc.Size = New System.Drawing.Size(600, 20)
        Me.txtSortSrc.TabIndex = 0
        Me.txtSortSrc.Text = "C:\Users\Eyal\Documents\coding\geo-photo-albums\csv\BT747log_20111018_199_FUJI.cs" & _
    "v"
        '
        'TabFilterCsv
        '
        Me.TabFilterCsv.Controls.Add(Me.btnFilter)
        Me.TabFilterCsv.Controls.Add(Me.btnFilterDestFile)
        Me.TabFilterCsv.Controls.Add(Me.lblFilterDest)
        Me.TabFilterCsv.Controls.Add(Me.txtFilterDest)
        Me.TabFilterCsv.Controls.Add(Me.btnFilterDestDir)
        Me.TabFilterCsv.Controls.Add(Me.btnFilterSrcDir)
        Me.TabFilterCsv.Controls.Add(Me.btnFilterSrcFile)
        Me.TabFilterCsv.Controls.Add(Me.lblFilterSrc)
        Me.TabFilterCsv.Controls.Add(Me.txtFilterSrc)
        Me.TabFilterCsv.Location = New System.Drawing.Point(4, 22)
        Me.TabFilterCsv.Name = "TabFilterCsv"
        Me.TabFilterCsv.Padding = New System.Windows.Forms.Padding(3)
        Me.TabFilterCsv.Size = New System.Drawing.Size(813, 347)
        Me.TabFilterCsv.TabIndex = 1
        Me.TabFilterCsv.Text = "Filter CSV"
        Me.TabFilterCsv.UseVisualStyleBackColor = True
        '
        'btnSort
        '
        Me.btnSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSort.Location = New System.Drawing.Point(732, 318)
        Me.btnSort.Name = "btnSort"
        Me.btnSort.Size = New System.Drawing.Size(75, 23)
        Me.btnSort.TabIndex = 8
        Me.btnSort.Text = "Sort!"
        Me.btnSort.UseVisualStyleBackColor = True
        '
        'btnFilterSrcDir
        '
        Me.btnFilterSrcDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterSrcDir.Location = New System.Drawing.Point(752, 6)
        Me.btnFilterSrcDir.Name = "btnFilterSrcDir"
        Me.btnFilterSrcDir.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterSrcDir.TabIndex = 10
        Me.btnFilterSrcDir.Text = "Folder..."
        Me.btnFilterSrcDir.UseVisualStyleBackColor = True
        '
        'btnFilterSrcFile
        '
        Me.btnFilterSrcFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterSrcFile.Location = New System.Drawing.Point(691, 6)
        Me.btnFilterSrcFile.Name = "btnFilterSrcFile"
        Me.btnFilterSrcFile.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterSrcFile.TabIndex = 9
        Me.btnFilterSrcFile.Text = "File..."
        Me.btnFilterSrcFile.UseVisualStyleBackColor = True
        '
        'lblFilterSrc
        '
        Me.lblFilterSrc.AutoSize = True
        Me.lblFilterSrc.Location = New System.Drawing.Point(8, 11)
        Me.lblFilterSrc.Name = "lblFilterSrc"
        Me.lblFilterSrc.Size = New System.Drawing.Size(41, 13)
        Me.lblFilterSrc.TabIndex = 8
        Me.lblFilterSrc.Text = "Source"
        '
        'txtFilterSrc
        '
        Me.txtFilterSrc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilterSrc.Location = New System.Drawing.Point(85, 8)
        Me.txtFilterSrc.Name = "txtFilterSrc"
        Me.txtFilterSrc.Size = New System.Drawing.Size(600, 20)
        Me.txtFilterSrc.TabIndex = 7
        Me.txtFilterSrc.Text = "C:\Users\Eyal\Documents\coding\geo-photo-albums\sorted_csv"
        '
        'btnFilterDestFile
        '
        Me.btnFilterDestFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterDestFile.Location = New System.Drawing.Point(691, 35)
        Me.btnFilterDestFile.Name = "btnFilterDestFile"
        Me.btnFilterDestFile.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterDestFile.TabIndex = 14
        Me.btnFilterDestFile.Text = "File..."
        Me.btnFilterDestFile.UseVisualStyleBackColor = True
        '
        'lblFilterDest
        '
        Me.lblFilterDest.AutoSize = True
        Me.lblFilterDest.Location = New System.Drawing.Point(8, 40)
        Me.lblFilterDest.Name = "lblFilterDest"
        Me.lblFilterDest.Size = New System.Drawing.Size(60, 13)
        Me.lblFilterDest.TabIndex = 13
        Me.lblFilterDest.Text = "Destination"
        '
        'txtFilterDest
        '
        Me.txtFilterDest.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilterDest.Location = New System.Drawing.Point(85, 37)
        Me.txtFilterDest.Name = "txtFilterDest"
        Me.txtFilterDest.Size = New System.Drawing.Size(600, 20)
        Me.txtFilterDest.TabIndex = 12
        Me.txtFilterDest.Text = "C:\Users\Eyal\Documents\coding\geo-photo-albums\filtered_csv"
        '
        'btnFilterDestDir
        '
        Me.btnFilterDestDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterDestDir.Location = New System.Drawing.Point(752, 35)
        Me.btnFilterDestDir.Name = "btnFilterDestDir"
        Me.btnFilterDestDir.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterDestDir.TabIndex = 11
        Me.btnFilterDestDir.Text = "Folder..."
        Me.btnFilterDestDir.UseVisualStyleBackColor = True
        '
        'btnFilter
        '
        Me.btnFilter.Location = New System.Drawing.Point(732, 318)
        Me.btnFilter.Name = "btnFilter"
        Me.btnFilter.Size = New System.Drawing.Size(75, 23)
        Me.btnFilter.TabIndex = 15
        Me.btnFilter.Text = "Filter!"
        Me.btnFilter.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(821, 373)
        Me.Controls.Add(Me.MainTab)
        Me.Name = "MainForm"
        Me.Text = "GeoPhotoAlbums"
        Me.MainTab.ResumeLayout(False)
        Me.TabSortCsv.ResumeLayout(False)
        Me.TabSortCsv.PerformLayout()
        Me.TabFilterCsv.ResumeLayout(False)
        Me.TabFilterCsv.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MainTab As System.Windows.Forms.TabControl
    Friend WithEvents TabSortCsv As System.Windows.Forms.TabPage
    Friend WithEvents lblSortDest As System.Windows.Forms.Label
    Friend WithEvents txtSortDest As System.Windows.Forms.TextBox
    Friend WithEvents btnSortDestDir As System.Windows.Forms.Button
    Friend WithEvents btnSortSrcFile As System.Windows.Forms.Button
    Friend WithEvents lblSortSrc As System.Windows.Forms.Label
    Friend WithEvents txtSortSrc As System.Windows.Forms.TextBox
    Friend WithEvents TabFilterCsv As System.Windows.Forms.TabPage
    Friend WithEvents btnSortSrcDir As System.Windows.Forms.Button
    Friend WithEvents btnSortDestFile As System.Windows.Forms.Button
    Friend WithEvents btnSort As System.Windows.Forms.Button
    Friend WithEvents btnFilterSrcDir As System.Windows.Forms.Button
    Friend WithEvents btnFilterSrcFile As System.Windows.Forms.Button
    Friend WithEvents lblFilterSrc As System.Windows.Forms.Label
    Friend WithEvents txtFilterSrc As System.Windows.Forms.TextBox
    Friend WithEvents btnFilter As System.Windows.Forms.Button
    Friend WithEvents btnFilterDestFile As System.Windows.Forms.Button
    Friend WithEvents lblFilterDest As System.Windows.Forms.Label
    Friend WithEvents txtFilterDest As System.Windows.Forms.TextBox
    Friend WithEvents btnFilterDestDir As System.Windows.Forms.Button

End Class
