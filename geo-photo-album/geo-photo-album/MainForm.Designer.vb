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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MainTab = New System.Windows.Forms.TabControl()
        Me.TabSortCsv = New System.Windows.Forms.TabPage()
        Me.btnSort = New System.Windows.Forms.Button()
        Me.btnSortDestFile = New System.Windows.Forms.Button()
        Me.btnSortSrcDir = New System.Windows.Forms.Button()
        Me.lblSortDest = New System.Windows.Forms.Label()
        Me.txtSortDest = New System.Windows.Forms.TextBox()
        Me.btnSortDestDir = New System.Windows.Forms.Button()
        Me.btnSortSrcFile = New System.Windows.Forms.Button()
        Me.lblSortSrc = New System.Windows.Forms.Label()
        Me.txtSortSrc = New System.Windows.Forms.TextBox()
        Me.TabFilterCsv = New System.Windows.Forms.TabPage()
        Me.btnFilter = New System.Windows.Forms.Button()
        Me.btnFilterDestFile = New System.Windows.Forms.Button()
        Me.lblFilterDest = New System.Windows.Forms.Label()
        Me.txtFilterDest = New System.Windows.Forms.TextBox()
        Me.btnFilterDestDir = New System.Windows.Forms.Button()
        Me.btnFilterSrcDir = New System.Windows.Forms.Button()
        Me.btnFilterSrcFile = New System.Windows.Forms.Button()
        Me.lblFilterSrc = New System.Windows.Forms.Label()
        Me.txtFilterSrc = New System.Windows.Forms.TextBox()
        Me.TabTagFiles = New System.Windows.Forms.TabPage()
        Me.SplitContainer4 = New System.Windows.Forms.SplitContainer()
        Me.btnSaveJson = New System.Windows.Forms.Button()
        Me.btnLoadJson = New System.Windows.Forms.Button()
        Me.lblTagFile = New System.Windows.Forms.Label()
        Me.txtTagFile = New System.Windows.Forms.TextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer3 = New System.Windows.Forms.SplitContainer()
        Me.btnIconView = New System.Windows.Forms.Button()
        Me.btnDetailView = New System.Windows.Forms.Button()
        Me.btnFilterTags = New System.Windows.Forms.Button()
        Me.txtTagFilter = New System.Windows.Forms.TextBox()
        Me.lvFileTags = New GeoPhotoAlbums.FileView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LargeImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.SmallImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.SplitContainer5 = New System.Windows.Forms.SplitContainer()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.wmpPreview = New AxWMPLib.AxWindowsMediaPlayer()
        Me.lstTagsMRU = New System.Windows.Forms.CheckedListBox()
        Me.btnSaveTags = New System.Windows.Forms.Button()
        Me.txtTags = New System.Windows.Forms.TextBox()
        Me.MainTab.SuspendLayout()
        Me.TabSortCsv.SuspendLayout()
        Me.TabFilterCsv.SuspendLayout()
        Me.TabTagFiles.SuspendLayout()
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer4.Panel1.SuspendLayout()
        Me.SplitContainer4.Panel2.SuspendLayout()
        Me.SplitContainer4.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer3.Panel1.SuspendLayout()
        Me.SplitContainer3.Panel2.SuspendLayout()
        Me.SplitContainer3.SuspendLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer5.Panel1.SuspendLayout()
        Me.SplitContainer5.Panel2.SuspendLayout()
        Me.SplitContainer5.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.wmpPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainTab
        '
        Me.MainTab.Controls.Add(Me.TabSortCsv)
        Me.MainTab.Controls.Add(Me.TabFilterCsv)
        Me.MainTab.Controls.Add(Me.TabTagFiles)
        Me.MainTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainTab.Location = New System.Drawing.Point(0, 0)
        Me.MainTab.Name = "MainTab"
        Me.MainTab.SelectedIndex = 0
        Me.MainTab.Size = New System.Drawing.Size(1153, 654)
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
        Me.TabSortCsv.Size = New System.Drawing.Size(1145, 628)
        Me.TabSortCsv.TabIndex = 0
        Me.TabSortCsv.Text = "Sort CSV"
        Me.TabSortCsv.UseVisualStyleBackColor = True
        '
        'btnSort
        '
        Me.btnSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSort.Location = New System.Drawing.Point(1050, 595)
        Me.btnSort.Name = "btnSort"
        Me.btnSort.Size = New System.Drawing.Size(75, 23)
        Me.btnSort.TabIndex = 8
        Me.btnSort.Text = "Sort!"
        Me.btnSort.UseVisualStyleBackColor = True
        '
        'btnSortDestFile
        '
        Me.btnSortDestFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortDestFile.Location = New System.Drawing.Point(1009, 35)
        Me.btnSortDestFile.Name = "btnSortDestFile"
        Me.btnSortDestFile.Size = New System.Drawing.Size(55, 23)
        Me.btnSortDestFile.TabIndex = 7
        Me.btnSortDestFile.Text = "File..."
        Me.btnSortDestFile.UseVisualStyleBackColor = True
        '
        'btnSortSrcDir
        '
        Me.btnSortSrcDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortSrcDir.Location = New System.Drawing.Point(1070, 6)
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
        Me.txtSortDest.Size = New System.Drawing.Size(918, 20)
        Me.txtSortDest.TabIndex = 4
        Me.txtSortDest.Text = "C:\Users\Eyal\Documents\coding\geo-photo-album\sorted_csv"
        '
        'btnSortDestDir
        '
        Me.btnSortDestDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortDestDir.Location = New System.Drawing.Point(1070, 35)
        Me.btnSortDestDir.Name = "btnSortDestDir"
        Me.btnSortDestDir.Size = New System.Drawing.Size(55, 23)
        Me.btnSortDestDir.TabIndex = 3
        Me.btnSortDestDir.Text = "Folder..."
        Me.btnSortDestDir.UseVisualStyleBackColor = True
        '
        'btnSortSrcFile
        '
        Me.btnSortSrcFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSortSrcFile.Location = New System.Drawing.Point(1009, 6)
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
        Me.txtSortSrc.Size = New System.Drawing.Size(918, 20)
        Me.txtSortSrc.TabIndex = 0
        Me.txtSortSrc.Text = "C:\Users\Eyal\Documents\coding\geo-photo-album\csv\BT747log_20111018_199_FUJI.csv" & _
    ""
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
        Me.TabFilterCsv.Size = New System.Drawing.Size(1145, 628)
        Me.TabFilterCsv.TabIndex = 1
        Me.TabFilterCsv.Text = "Filter CSV"
        Me.TabFilterCsv.UseVisualStyleBackColor = True
        '
        'btnFilter
        '
        Me.btnFilter.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilter.Location = New System.Drawing.Point(1050, 595)
        Me.btnFilter.Name = "btnFilter"
        Me.btnFilter.Size = New System.Drawing.Size(75, 23)
        Me.btnFilter.TabIndex = 15
        Me.btnFilter.Text = "Filter!"
        Me.btnFilter.UseVisualStyleBackColor = True
        '
        'btnFilterDestFile
        '
        Me.btnFilterDestFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterDestFile.Location = New System.Drawing.Point(1009, 35)
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
        Me.txtFilterDest.Size = New System.Drawing.Size(918, 20)
        Me.txtFilterDest.TabIndex = 12
        Me.txtFilterDest.Text = "E:\Users\Eyal\Pictures\World Tour 2011-2012\output"
        '
        'btnFilterDestDir
        '
        Me.btnFilterDestDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterDestDir.Location = New System.Drawing.Point(1070, 35)
        Me.btnFilterDestDir.Name = "btnFilterDestDir"
        Me.btnFilterDestDir.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterDestDir.TabIndex = 11
        Me.btnFilterDestDir.Text = "Folder..."
        Me.btnFilterDestDir.UseVisualStyleBackColor = True
        '
        'btnFilterSrcDir
        '
        Me.btnFilterSrcDir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterSrcDir.Location = New System.Drawing.Point(1070, 6)
        Me.btnFilterSrcDir.Name = "btnFilterSrcDir"
        Me.btnFilterSrcDir.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterSrcDir.TabIndex = 10
        Me.btnFilterSrcDir.Text = "Folder..."
        Me.btnFilterSrcDir.UseVisualStyleBackColor = True
        '
        'btnFilterSrcFile
        '
        Me.btnFilterSrcFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterSrcFile.Location = New System.Drawing.Point(1009, 6)
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
        Me.txtFilterSrc.Size = New System.Drawing.Size(918, 20)
        Me.txtFilterSrc.TabIndex = 7
        Me.txtFilterSrc.Text = "E:\Users\Eyal\Pictures\World Tour 2011-2012\sorted_csv"
        '
        'TabTagFiles
        '
        Me.TabTagFiles.Controls.Add(Me.SplitContainer4)
        Me.TabTagFiles.Location = New System.Drawing.Point(4, 22)
        Me.TabTagFiles.Name = "TabTagFiles"
        Me.TabTagFiles.Padding = New System.Windows.Forms.Padding(3)
        Me.TabTagFiles.Size = New System.Drawing.Size(1145, 628)
        Me.TabTagFiles.TabIndex = 2
        Me.TabTagFiles.Text = "Tag Files"
        Me.TabTagFiles.UseVisualStyleBackColor = True
        '
        'SplitContainer4
        '
        Me.SplitContainer4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer4.Location = New System.Drawing.Point(3, 3)
        Me.SplitContainer4.Name = "SplitContainer4"
        Me.SplitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer4.Panel1
        '
        Me.SplitContainer4.Panel1.Controls.Add(Me.btnSaveJson)
        Me.SplitContainer4.Panel1.Controls.Add(Me.btnLoadJson)
        Me.SplitContainer4.Panel1.Controls.Add(Me.lblTagFile)
        Me.SplitContainer4.Panel1.Controls.Add(Me.txtTagFile)
        '
        'SplitContainer4.Panel2
        '
        Me.SplitContainer4.Panel2.Controls.Add(Me.SplitContainer1)
        Me.SplitContainer4.Size = New System.Drawing.Size(1139, 622)
        Me.SplitContainer4.SplitterDistance = 32
        Me.SplitContainer4.TabIndex = 1
        '
        'btnSaveJson
        '
        Me.btnSaveJson.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveJson.Location = New System.Drawing.Point(1081, 5)
        Me.btnSaveJson.Name = "btnSaveJson"
        Me.btnSaveJson.Size = New System.Drawing.Size(55, 23)
        Me.btnSaveJson.TabIndex = 23
        Me.btnSaveJson.Text = "Save"
        Me.btnSaveJson.UseVisualStyleBackColor = True
        '
        'btnLoadJson
        '
        Me.btnLoadJson.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLoadJson.Location = New System.Drawing.Point(1020, 5)
        Me.btnLoadJson.Name = "btnLoadJson"
        Me.btnLoadJson.Size = New System.Drawing.Size(55, 23)
        Me.btnLoadJson.TabIndex = 22
        Me.btnLoadJson.Text = "File..."
        Me.btnLoadJson.UseVisualStyleBackColor = True
        '
        'lblTagFile
        '
        Me.lblTagFile.AutoSize = True
        Me.lblTagFile.Location = New System.Drawing.Point(5, 8)
        Me.lblTagFile.Name = "lblTagFile"
        Me.lblTagFile.Size = New System.Drawing.Size(45, 13)
        Me.lblTagFile.TabIndex = 21
        Me.lblTagFile.Text = "Tag File"
        '
        'txtTagFile
        '
        Me.txtTagFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTagFile.Location = New System.Drawing.Point(82, 5)
        Me.txtTagFile.Name = "txtTagFile"
        Me.txtTagFile.Size = New System.Drawing.Size(932, 20)
        Me.txtTagFile.TabIndex = 20
        Me.txtTagFile.Text = "E:\Users\Eyal\Pictures\World Tour 2011-2012\photo_info.json"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainer3)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainer5)
        Me.SplitContainer1.Size = New System.Drawing.Size(1139, 586)
        Me.SplitContainer1.SplitterDistance = 649
        Me.SplitContainer1.TabIndex = 0
        '
        'SplitContainer3
        '
        Me.SplitContainer3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer3.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer3.Name = "SplitContainer3"
        Me.SplitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer3.Panel1
        '
        Me.SplitContainer3.Panel1.Controls.Add(Me.btnIconView)
        Me.SplitContainer3.Panel1.Controls.Add(Me.btnDetailView)
        Me.SplitContainer3.Panel1.Controls.Add(Me.btnFilterTags)
        Me.SplitContainer3.Panel1.Controls.Add(Me.txtTagFilter)
        '
        'SplitContainer3.Panel2
        '
        Me.SplitContainer3.Panel2.Controls.Add(Me.lvFileTags)
        Me.SplitContainer3.Size = New System.Drawing.Size(649, 586)
        Me.SplitContainer3.SplitterDistance = 59
        Me.SplitContainer3.TabIndex = 0
        '
        'btnIconView
        '
        Me.btnIconView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnIconView.Location = New System.Drawing.Point(64, 31)
        Me.btnIconView.Name = "btnIconView"
        Me.btnIconView.Size = New System.Drawing.Size(55, 23)
        Me.btnIconView.TabIndex = 25
        Me.btnIconView.Text = "Icons"
        Me.btnIconView.UseVisualStyleBackColor = True
        '
        'btnDetailView
        '
        Me.btnDetailView.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDetailView.Location = New System.Drawing.Point(3, 31)
        Me.btnDetailView.Name = "btnDetailView"
        Me.btnDetailView.Size = New System.Drawing.Size(55, 23)
        Me.btnDetailView.TabIndex = 24
        Me.btnDetailView.Text = "Details"
        Me.btnDetailView.UseVisualStyleBackColor = True
        '
        'btnFilterTags
        '
        Me.btnFilterTags.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterTags.Location = New System.Drawing.Point(591, 3)
        Me.btnFilterTags.Name = "btnFilterTags"
        Me.btnFilterTags.Size = New System.Drawing.Size(55, 23)
        Me.btnFilterTags.TabIndex = 23
        Me.btnFilterTags.Text = "Filter"
        Me.btnFilterTags.UseVisualStyleBackColor = True
        '
        'txtTagFilter
        '
        Me.txtTagFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTagFilter.Location = New System.Drawing.Point(3, 5)
        Me.txtTagFilter.Name = "txtTagFilter"
        Me.txtTagFilter.Size = New System.Drawing.Size(582, 20)
        Me.txtTagFilter.TabIndex = 0
        Me.txtTagFilter.Text = "E:\Users\Eyal\Pictures\World Tour 2011-2012\Thailand\Bangkok"
        '
        'lvFileTags
        '
        Me.lvFileTags.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvFileTags.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvFileTags.FullRowSelect = True
        Me.lvFileTags.GridLines = True
        Me.lvFileTags.LargeImageList = Me.LargeImageList
        Me.lvFileTags.Location = New System.Drawing.Point(0, 0)
        Me.lvFileTags.my_json = Nothing
        Me.lvFileTags.Name = "lvFileTags"
        Me.lvFileTags.ShowGroups = False
        Me.lvFileTags.Size = New System.Drawing.Size(649, 523)
        Me.lvFileTags.SmallImageList = Me.SmallImageList
        Me.lvFileTags.TabIndex = 0
        Me.lvFileTags.UseCompatibleStateImageBehavior = False
        Me.lvFileTags.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "File"
        Me.ColumnHeader1.Width = 100
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Tags"
        Me.ColumnHeader2.Width = 300
        '
        'LargeImageList
        '
        Me.LargeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.LargeImageList.ImageSize = New System.Drawing.Size(100, 100)
        Me.LargeImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'SmallImageList
        '
        Me.SmallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.SmallImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.SmallImageList.TransparentColor = System.Drawing.Color.Transparent
        '
        'SplitContainer5
        '
        Me.SplitContainer5.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer5.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer5.Name = "SplitContainer5"
        Me.SplitContainer5.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer5.Panel1
        '
        Me.SplitContainer5.Panel1.Controls.Add(Me.picPreview)
        Me.SplitContainer5.Panel1.Controls.Add(Me.wmpPreview)
        '
        'SplitContainer5.Panel2
        '
        Me.SplitContainer5.Panel2.Controls.Add(Me.lstTagsMRU)
        Me.SplitContainer5.Panel2.Controls.Add(Me.btnSaveTags)
        Me.SplitContainer5.Panel2.Controls.Add(Me.txtTags)
        Me.SplitContainer5.Size = New System.Drawing.Size(486, 586)
        Me.SplitContainer5.SplitterDistance = 263
        Me.SplitContainer5.TabIndex = 5
        '
        'picPreview
        '
        Me.picPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picPreview.Location = New System.Drawing.Point(0, 0)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(486, 263)
        Me.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picPreview.TabIndex = 0
        Me.picPreview.TabStop = False
        '
        'wmpPreview
        '
        Me.wmpPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wmpPreview.Enabled = True
        Me.wmpPreview.Location = New System.Drawing.Point(0, 0)
        Me.wmpPreview.Name = "wmpPreview"
        Me.wmpPreview.OcxState = CType(resources.GetObject("wmpPreview.OcxState"), System.Windows.Forms.AxHost.State)
        Me.wmpPreview.Size = New System.Drawing.Size(486, 263)
        Me.wmpPreview.TabIndex = 0
        '
        'lstTagsMRU
        '
        Me.lstTagsMRU.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstTagsMRU.CheckOnClick = True
        Me.lstTagsMRU.FormattingEnabled = True
        Me.lstTagsMRU.IntegralHeight = False
        Me.lstTagsMRU.Location = New System.Drawing.Point(3, 104)
        Me.lstTagsMRU.Name = "lstTagsMRU"
        Me.lstTagsMRU.Size = New System.Drawing.Size(478, 210)
        Me.lstTagsMRU.TabIndex = 2
        '
        'btnSaveTags
        '
        Me.btnSaveTags.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSaveTags.Enabled = False
        Me.btnSaveTags.Location = New System.Drawing.Point(433, 3)
        Me.btnSaveTags.Name = "btnSaveTags"
        Me.btnSaveTags.Size = New System.Drawing.Size(48, 23)
        Me.btnSaveTags.TabIndex = 1
        Me.btnSaveTags.Text = "Save"
        Me.btnSaveTags.UseVisualStyleBackColor = True
        '
        'txtTags
        '
        Me.txtTags.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTags.Location = New System.Drawing.Point(3, 5)
        Me.txtTags.Multiline = True
        Me.txtTags.Name = "txtTags"
        Me.txtTags.Size = New System.Drawing.Size(424, 93)
        Me.txtTags.TabIndex = 0
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1153, 654)
        Me.Controls.Add(Me.MainTab)
        Me.Name = "MainForm"
        Me.Text = "GeoPhotoAlbum"
        Me.MainTab.ResumeLayout(False)
        Me.TabSortCsv.ResumeLayout(False)
        Me.TabSortCsv.PerformLayout()
        Me.TabFilterCsv.ResumeLayout(False)
        Me.TabFilterCsv.PerformLayout()
        Me.TabTagFiles.ResumeLayout(False)
        Me.SplitContainer4.Panel1.ResumeLayout(False)
        Me.SplitContainer4.Panel1.PerformLayout()
        Me.SplitContainer4.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer4.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.ResumeLayout(False)
        Me.SplitContainer3.Panel1.PerformLayout()
        Me.SplitContainer3.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer3.ResumeLayout(False)
        Me.SplitContainer5.Panel1.ResumeLayout(False)
        Me.SplitContainer5.Panel2.ResumeLayout(False)
        Me.SplitContainer5.Panel2.PerformLayout()
        CType(Me.SplitContainer5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer5.ResumeLayout(False)
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.wmpPreview, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents TabTagFiles As System.Windows.Forms.TabPage
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnSaveTags As System.Windows.Forms.Button
    Friend WithEvents txtTags As System.Windows.Forms.TextBox
    Friend WithEvents picPreview As System.Windows.Forms.PictureBox
    Friend WithEvents wmpPreview As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents SplitContainer4 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnLoadJson As System.Windows.Forms.Button
    Friend WithEvents lblTagFile As System.Windows.Forms.Label
    Friend WithEvents txtTagFile As System.Windows.Forms.TextBox
    Friend WithEvents SplitContainer5 As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer3 As System.Windows.Forms.SplitContainer
    Friend WithEvents btnFilterTags As System.Windows.Forms.Button
    Friend WithEvents txtTagFilter As System.Windows.Forms.TextBox
    Friend WithEvents SmallImageList As System.Windows.Forms.ImageList
    Friend WithEvents btnSaveJson As System.Windows.Forms.Button
    Friend WithEvents lstTagsMRU As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnDetailView As System.Windows.Forms.Button
    Friend WithEvents btnIconView As System.Windows.Forms.Button
    Friend WithEvents LargeImageList As System.Windows.Forms.ImageList
    Friend WithEvents lvFileTags As GeoPhotoAlbums.FileView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader

End Class
