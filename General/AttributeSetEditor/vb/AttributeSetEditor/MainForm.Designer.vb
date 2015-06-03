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
        Dim treeNode2 As New System.Windows.Forms.TreeNode("Active Select Set")
        Dim listViewItem4 As New System.Windows.Forms.ListViewItem("asdfasdfadf")
        Me.menuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.toolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tvSelectSet = New System.Windows.Forms.TreeView()
        Me.imageList = New System.Windows.Forms.ImageList(Me.components)
        Me.lvAttributes = New System.Windows.Forms.ListView()
        Me.columnHeader1 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.columnHeader2 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.columnHeader3 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.columnHeader4 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.toolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.buttonAddAttribute = New System.Windows.Forms.ToolStripButton()
        Me.buttonRemoveAttribute = New System.Windows.Forms.ToolStripButton()
        Me.buttonRefresh = New System.Windows.Forms.ToolStripButton()
        Me.menuStrip1.SuspendLayout()
        Me.toolStrip1.SuspendLayout()
        DirectCast(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.toolStrip2.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' menuStrip1
        ' 
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.fileToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(715, 24)
        Me.menuStrip1.TabIndex = 0
        Me.menuStrip1.Text = "menuStrip1"
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
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.exitToolStripMenuItem.Text = "&Exit"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
        ' 
        ' statusStrip1
        ' 
        Me.statusStrip1.Location = New System.Drawing.Point(0, 441)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Size = New System.Drawing.Size(715, 22)
        Me.statusStrip1.TabIndex = 1
        Me.statusStrip1.Text = "statusStrip1"
        ' 
        ' toolStrip1
        ' 
        Me.toolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.buttonRefresh})
        Me.toolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip1.Name = "toolStrip1"
        Me.toolStrip1.Size = New System.Drawing.Size(715, 25)
        Me.toolStrip1.TabIndex = 2
        Me.toolStrip1.Text = "toolStrip1"
        ' 
        ' splitContainer1
        ' 
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(0, 49)
        Me.splitContainer1.Name = "splitContainer1"
        ' 
        ' splitContainer1.Panel1
        ' 
        Me.splitContainer1.Panel1.Controls.Add(Me.tvSelectSet)
        ' 
        ' splitContainer1.Panel2
        ' 
        Me.splitContainer1.Panel2.Controls.Add(Me.lvAttributes)
        Me.splitContainer1.Panel2.Controls.Add(Me.toolStrip2)
        Me.splitContainer1.Size = New System.Drawing.Size(715, 392)
        Me.splitContainer1.SplitterDistance = 315
        Me.splitContainer1.TabIndex = 3
        ' 
        ' tvSelectSet
        ' 
        Me.tvSelectSet.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvSelectSet.HideSelection = False
        Me.tvSelectSet.ImageIndex = 0
        Me.tvSelectSet.ImageList = Me.imageList
        Me.tvSelectSet.Location = New System.Drawing.Point(0, 0)
        Me.tvSelectSet.Name = "tvSelectSet"
        treeNode2.Name = "Node0"
        treeNode2.Text = "Active Select Set"
        Me.tvSelectSet.Nodes.AddRange(New System.Windows.Forms.TreeNode() { treeNode2})
        Me.tvSelectSet.SelectedImageIndex = 0
        Me.tvSelectSet.ShowRootLines = False
        Me.tvSelectSet.Size = New System.Drawing.Size(315, 392)
        Me.tvSelectSet.TabIndex = 0
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.tvSelectSet.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSelectSet_AfterSelect);
        ' 
        ' imageList
        ' 
        Me.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.imageList.TransparentColor = System.Drawing.Color.Transparent
        ' 
        ' lvAttributes
        ' 
        Me.lvAttributes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader1, Me.columnHeader2, Me.columnHeader3, Me.columnHeader4})
        Me.lvAttributes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvAttributes.FullRowSelect = True
        Me.lvAttributes.HideSelection = False
        Me.lvAttributes.Items.AddRange(New System.Windows.Forms.ListViewItem() { listViewItem4})
        Me.lvAttributes.Location = New System.Drawing.Point(0, 25)
        Me.lvAttributes.Name = "lvAttributes"
        Me.lvAttributes.Size = New System.Drawing.Size(396, 367)
        Me.lvAttributes.TabIndex = 0
        Me.lvAttributes.UseCompatibleStateImageBehavior = False
        Me.lvAttributes.View = System.Windows.Forms.View.Details
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.lvAttributes.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvAttributes_ItemSelectionChanged);
        ' 
        ' columnHeader1
        ' 
        Me.columnHeader1.Text = "Set Name"
        Me.columnHeader1.Width = 88
        ' 
        ' columnHeader2
        ' 
        Me.columnHeader2.Text = "Attribute Name"
        Me.columnHeader2.Width = 94
        ' 
        ' columnHeader3
        ' 
        Me.columnHeader3.Text = "Attribute Value"
        Me.columnHeader3.Width = 92
        ' 
        ' columnHeader4
        ' 
        Me.columnHeader4.Text = "Attribute Type"
        Me.columnHeader4.Width = 114
        ' 
        ' toolStrip2
        ' 
        Me.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.toolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.buttonAddAttribute, Me.buttonRemoveAttribute})
        Me.toolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.toolStrip2.Name = "toolStrip2"
        Me.toolStrip2.Size = New System.Drawing.Size(396, 25)
        Me.toolStrip2.TabIndex = 1
        Me.toolStrip2.Text = "toolStrip2"
        ' 
        ' buttonAddAttribute
        ' 
        Me.buttonAddAttribute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonAddAttribute.Image = My.Resources.AddAttribute
        Me.buttonAddAttribute.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonAddAttribute.Name = "buttonAddAttribute"
        Me.buttonAddAttribute.Size = New System.Drawing.Size(23, 22)
        Me.buttonAddAttribute.Text = "Add Attribute"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonAddAttribute.Click += new System.EventHandler(this.buttonAddAttribute_Click);
        ' 
        ' buttonRemoveAttribute
        ' 
        Me.buttonRemoveAttribute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonRemoveAttribute.Image = My.Resources.RemoveAttribute
        Me.buttonRemoveAttribute.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonRemoveAttribute.Name = "buttonRemoveAttribute"
        Me.buttonRemoveAttribute.Size = New System.Drawing.Size(23, 22)
        Me.buttonRemoveAttribute.Text = "Remove Attribute"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonRemoveAttribute.Click += new System.EventHandler(this.buttonRemoveAttribute_Click);
        ' 
        ' buttonRefresh
        ' 
        Me.buttonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonRefresh.Image = My.Resources.Refresh_16x16
        Me.buttonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonRefresh.Name = "buttonRefresh"
        Me.buttonRefresh.Size = New System.Drawing.Size(23, 22)
        Me.buttonRefresh.Text = "Refresh"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(715, 463)
        Me.Controls.Add(Me.splitContainer1)
        Me.Controls.Add(Me.toolStrip1)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.menuStrip1)
        Me.MainMenuStrip = Me.menuStrip1
        Me.Name = "MainForm"
        Me.Text = "AttributeSets Editor"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.MainForm_Load);
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.toolStrip1.ResumeLayout(False)
        Me.toolStrip1.PerformLayout()
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.Panel2.PerformLayout()
        DirectCast(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        Me.toolStrip2.ResumeLayout(False)
        Me.toolStrip2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    #End Region

    Private menuStrip1 As System.Windows.Forms.MenuStrip
    Private fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private statusStrip1 As System.Windows.Forms.StatusStrip
    Private toolStrip1 As System.Windows.Forms.ToolStrip
    Private WithEvents buttonRefresh As System.Windows.Forms.ToolStripButton
    Private splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents tvSelectSet As System.Windows.Forms.TreeView
    Private WithEvents lvAttributes As System.Windows.Forms.ListView
    Private columnHeader1 As System.Windows.Forms.ColumnHeader
    Private columnHeader2 As System.Windows.Forms.ColumnHeader
    Private columnHeader3 As System.Windows.Forms.ColumnHeader
    Private columnHeader4 As System.Windows.Forms.ColumnHeader
    Private imageList As System.Windows.Forms.ImageList
    Private toolStrip2 As System.Windows.Forms.ToolStrip
    Private WithEvents buttonAddAttribute As System.Windows.Forms.ToolStripButton
    Private WithEvents buttonRemoveAttribute As System.Windows.Forms.ToolStripButton
End Class

