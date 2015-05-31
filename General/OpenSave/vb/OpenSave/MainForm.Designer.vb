Partial Public Class MainForm
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private components As System.ComponentModel.IContainer = Nothing

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (components IsNot Nothing) Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    #Region "Windows Form Designer generated code"

    ''' <summary>
    ''' Required method for Designer support - do not modify
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.menuStrip = New System.Windows.Forms.MenuStrip()
        Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.statusStrip = New System.Windows.Forms.StatusStrip()
        Me.toolStrip = New System.Windows.Forms.ToolStrip()
        Me.buttonSelectFolder = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.buttonStart = New System.Windows.Forms.ToolStripButton()
        Me.buttonStop = New System.Windows.Forms.ToolStripButton()
        Me.splitContainerInner = New System.Windows.Forms.SplitContainer()
        Me.listViewFiles = New SolidEdge.OpenSave.ListViewEx()
        Me.columnHeader1 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.columnHeader2 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.columnHeader3 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.smallImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.outputTextBox = New System.Windows.Forms.TextBox()
        Me.searchBackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.openSaveBackgroundWorker = New System.ComponentModel.BackgroundWorker()
        Me.propertyGrid = New System.Windows.Forms.PropertyGrid()
        Me.splitContainerOuter = New System.Windows.Forms.SplitContainer()
        Me.menuStrip.SuspendLayout()
        Me.toolStrip.SuspendLayout()
        DirectCast(Me.splitContainerInner, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainerInner.Panel1.SuspendLayout()
        Me.splitContainerInner.Panel2.SuspendLayout()
        Me.splitContainerInner.SuspendLayout()
        DirectCast(Me.splitContainerOuter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainerOuter.Panel1.SuspendLayout()
        Me.splitContainerOuter.Panel2.SuspendLayout()
        Me.splitContainerOuter.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' menuStrip
        ' 
        Me.menuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.fileToolStripMenuItem})
        Me.menuStrip.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip.Name = "menuStrip"
        Me.menuStrip.Size = New System.Drawing.Size(784, 24)
        Me.menuStrip.TabIndex = 0
        Me.menuStrip.Text = "menuStrip1"
        ' 
        ' fileToolStripMenuItem
        ' 
        Me.fileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.exitToolStripMenuItem})
        Me.fileToolStripMenuItem.Name = "fileToolStripMenuItem"
        Me.fileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.fileToolStripMenuItem.Text = "&File"
        ' 
        ' exitToolStripMenuItem
        ' 
        Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.exitToolStripMenuItem.Text = "&Exit"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
        ' 
        ' statusStrip
        ' 
        Me.statusStrip.Location = New System.Drawing.Point(0, 539)
        Me.statusStrip.Name = "statusStrip"
        Me.statusStrip.Size = New System.Drawing.Size(784, 22)
        Me.statusStrip.TabIndex = 1
        Me.statusStrip.Text = "statusStrip1"
        ' 
        ' toolStrip
        ' 
        Me.toolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.buttonSelectFolder, Me.toolStripSeparator1, Me.buttonStart, Me.buttonStop})
        Me.toolStrip.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip.Name = "toolStrip"
        Me.toolStrip.Size = New System.Drawing.Size(784, 25)
        Me.toolStrip.TabIndex = 2
        Me.toolStrip.Text = "toolStrip1"
        ' 
        ' buttonSelectFolder
        ' 
        Me.buttonSelectFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonSelectFolder.Image = My.Resources.FolderOpen_16x16
        Me.buttonSelectFolder.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonSelectFolder.Name = "buttonSelectFolder"
        Me.buttonSelectFolder.Size = New System.Drawing.Size(23, 22)
        Me.buttonSelectFolder.Text = "Select files"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
        ' 
        ' toolStripSeparator1
        ' 
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        ' 
        ' buttonStart
        ' 
        Me.buttonStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonStart.Image = My.Resources.Run_16x16
        Me.buttonStart.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonStart.Name = "buttonStart"
        Me.buttonStart.Size = New System.Drawing.Size(23, 22)
        Me.buttonStart.Text = "Start"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
        ' 
        ' buttonStop
        ' 
        Me.buttonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonStop.Image = My.Resources.Stop_16x16
        Me.buttonStop.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonStop.Name = "buttonStop"
        Me.buttonStop.Size = New System.Drawing.Size(23, 22)
        Me.buttonStop.Text = "Stop"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
        ' 
        ' splitContainerInner
        ' 
        Me.splitContainerInner.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainerInner.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.splitContainerInner.Location = New System.Drawing.Point(0, 0)
        Me.splitContainerInner.Name = "splitContainerInner"
        Me.splitContainerInner.Orientation = System.Windows.Forms.Orientation.Horizontal
        ' 
        ' splitContainerInner.Panel1
        ' 
        Me.splitContainerInner.Panel1.Controls.Add(Me.listViewFiles)
        ' 
        ' splitContainerInner.Panel2
        ' 
        Me.splitContainerInner.Panel2.Controls.Add(Me.outputTextBox)
        Me.splitContainerInner.Size = New System.Drawing.Size(509, 490)
        Me.splitContainerInner.SplitterDistance = 352
        Me.splitContainerInner.TabIndex = 3
        ' 
        ' listViewFiles
        ' 
        Me.listViewFiles.AllowDeleteKey = True
        Me.listViewFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader1, Me.columnHeader2, Me.columnHeader3})
        Me.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listViewFiles.FullRowSelect = True
        Me.listViewFiles.HideSelection = False
        Me.listViewFiles.Location = New System.Drawing.Point(0, 0)
        Me.listViewFiles.Name = "listViewFiles"
        Me.listViewFiles.Size = New System.Drawing.Size(509, 352)
        Me.listViewFiles.SmallImageList = Me.smallImageList
        Me.listViewFiles.TabIndex = 0
        Me.listViewFiles.UseCompatibleStateImageBehavior = False
        Me.listViewFiles.View = System.Windows.Forms.View.Details
        ' 
        ' columnHeader1
        ' 
        Me.columnHeader1.Text = "Name"
        Me.columnHeader1.Width = 153
        ' 
        ' columnHeader2
        ' 
        Me.columnHeader2.Text = "Last Saved Version"
        Me.columnHeader2.Width = 142
        ' 
        ' columnHeader3
        ' 
        Me.columnHeader3.Text = "Folder"
        Me.columnHeader3.Width = 241
        ' 
        ' smallImageList
        ' 
        Me.smallImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.smallImageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.smallImageList.TransparentColor = System.Drawing.Color.Transparent
        ' 
        ' outputTextBox
        ' 
        Me.outputTextBox.AcceptsReturn = True
        Me.outputTextBox.AcceptsTab = True
        Me.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.outputTextBox.Font = New System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (CByte(0)))
        Me.outputTextBox.Location = New System.Drawing.Point(0, 0)
        Me.outputTextBox.MaxLength = 0
        Me.outputTextBox.Multiline = True
        Me.outputTextBox.Name = "outputTextBox"
        Me.outputTextBox.ReadOnly = True
        Me.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.outputTextBox.Size = New System.Drawing.Size(509, 134)
        Me.outputTextBox.TabIndex = 4
        Me.outputTextBox.WordWrap = False
        ' 
        ' searchBackgroundWorker
        ' 
        Me.searchBackgroundWorker.WorkerSupportsCancellation = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.searchBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.searchBackgroundWorker_DoWork);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.searchBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.searchBackgroundWorker_RunWorkerCompleted);
        ' 
        ' openSaveBackgroundWorker
        ' 
        Me.openSaveBackgroundWorker.WorkerReportsProgress = True
        Me.openSaveBackgroundWorker.WorkerSupportsCancellation = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.openSaveBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.openSaveBackgroundWorker_DoWork);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.openSaveBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.openSaveBackgroundWorker_ProgressChanged);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.openSaveBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.openSaveBackgroundWorker_RunWorkerCompleted);
        ' 
        ' propertyGrid
        ' 
        Me.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.propertyGrid.Location = New System.Drawing.Point(0, 0)
        Me.propertyGrid.Name = "propertyGrid"
        Me.propertyGrid.Size = New System.Drawing.Size(271, 490)
        Me.propertyGrid.TabIndex = 4
        ' 
        ' splitContainerOuter
        ' 
        Me.splitContainerOuter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainerOuter.Location = New System.Drawing.Point(0, 49)
        Me.splitContainerOuter.Name = "splitContainerOuter"
        ' 
        ' splitContainerOuter.Panel1
        ' 
        Me.splitContainerOuter.Panel1.Controls.Add(Me.splitContainerInner)
        ' 
        ' splitContainerOuter.Panel2
        ' 
        Me.splitContainerOuter.Panel2.Controls.Add(Me.propertyGrid)
        Me.splitContainerOuter.Size = New System.Drawing.Size(784, 490)
        Me.splitContainerOuter.SplitterDistance = 509
        Me.splitContainerOuter.TabIndex = 4
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 561)
        Me.Controls.Add(Me.splitContainerOuter)
        Me.Controls.Add(Me.toolStrip)
        Me.Controls.Add(Me.statusStrip)
        Me.Controls.Add(Me.menuStrip)
        Me.DoubleBuffered = True
        Me.MainMenuStrip = Me.menuStrip
        Me.Name = "MainForm"
        Me.Text = "Solid Edge Open\Save"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.MainForm_Load);
        Me.menuStrip.ResumeLayout(False)
        Me.menuStrip.PerformLayout()
        Me.toolStrip.ResumeLayout(False)
        Me.toolStrip.PerformLayout()
        Me.splitContainerInner.Panel1.ResumeLayout(False)
        Me.splitContainerInner.Panel2.ResumeLayout(False)
        Me.splitContainerInner.Panel2.PerformLayout()
        DirectCast(Me.splitContainerInner, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainerInner.ResumeLayout(False)
        Me.splitContainerOuter.Panel1.ResumeLayout(False)
        Me.splitContainerOuter.Panel2.ResumeLayout(False)
        DirectCast(Me.splitContainerOuter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainerOuter.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    #End Region

    Private menuStrip As System.Windows.Forms.MenuStrip
    Private fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private statusStrip As System.Windows.Forms.StatusStrip
    Private toolStrip As System.Windows.Forms.ToolStrip
    Private splitContainerInner As System.Windows.Forms.SplitContainer
    Private listViewFiles As ListViewEx
    Private outputTextBox As System.Windows.Forms.TextBox
    Private WithEvents buttonSelectFolder As System.Windows.Forms.ToolStripButton
    Private WithEvents buttonStart As System.Windows.Forms.ToolStripButton
    Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Private columnHeader1 As System.Windows.Forms.ColumnHeader
    Private columnHeader2 As System.Windows.Forms.ColumnHeader
    Private columnHeader3 As System.Windows.Forms.ColumnHeader
    Private WithEvents searchBackgroundWorker As System.ComponentModel.BackgroundWorker
    Private smallImageList As System.Windows.Forms.ImageList
    Private WithEvents openSaveBackgroundWorker As System.ComponentModel.BackgroundWorker
    Private propertyGrid As System.Windows.Forms.PropertyGrid
    Private splitContainerOuter As System.Windows.Forms.SplitContainer
    Private WithEvents buttonStop As System.Windows.Forms.ToolStripButton
End Class

