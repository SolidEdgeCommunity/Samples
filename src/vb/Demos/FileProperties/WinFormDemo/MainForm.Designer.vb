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
		Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.menuStrip1 = New System.Windows.Forms.MenuStrip()
		Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.openToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.closeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
		Me.saveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.toolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
		Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
		Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
		Me.toolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
		Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
		Me.listView1 = New FileProperties.WinForm.ListViewEx()
		Me.columnHeader2 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
		Me.columnHeader3 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
		Me.menuStrip1.SuspendLayout()
		Me.statusStrip1.SuspendLayout()
		Me.SuspendLayout()
		' 
		' menuStrip1
		' 
		Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.fileToolStripMenuItem})
		Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
		Me.menuStrip1.Name = "menuStrip1"
		Me.menuStrip1.Size = New System.Drawing.Size(784, 24)
		Me.menuStrip1.TabIndex = 0
		Me.menuStrip1.Text = "menuStrip1"
		' 
		' fileToolStripMenuItem
		' 
		Me.fileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() { Me.openToolStripMenuItem, Me.closeToolStripMenuItem, Me.toolStripMenuItem1, Me.saveToolStripMenuItem, Me.toolStripMenuItem2, Me.exitToolStripMenuItem})
		Me.fileToolStripMenuItem.Name = "fileToolStripMenuItem"
		Me.fileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
		Me.fileToolStripMenuItem.Text = "&File"
		' 
		' openToolStripMenuItem
		' 
		Me.openToolStripMenuItem.Name = "openToolStripMenuItem"
		Me.openToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
		Me.openToolStripMenuItem.Text = "&Open"
		' 
		' closeToolStripMenuItem
		' 
		Me.closeToolStripMenuItem.Name = "closeToolStripMenuItem"
		Me.closeToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
		Me.closeToolStripMenuItem.Text = "&Close"
		' 
		' toolStripMenuItem1
		' 
		Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
		Me.toolStripMenuItem1.Size = New System.Drawing.Size(100, 6)
		' 
		' saveToolStripMenuItem
		' 
		Me.saveToolStripMenuItem.Name = "saveToolStripMenuItem"
		Me.saveToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
		Me.saveToolStripMenuItem.Text = "&Save"
		' 
		' toolStripMenuItem2
		' 
		Me.toolStripMenuItem2.Name = "toolStripMenuItem2"
		Me.toolStripMenuItem2.Size = New System.Drawing.Size(100, 6)
		' 
		' exitToolStripMenuItem
		' 
		Me.exitToolStripMenuItem.Name = "exitToolStripMenuItem"
		Me.exitToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
		Me.exitToolStripMenuItem.Text = "&Exit"
		' 
		' statusStrip1
		' 
		Me.statusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.toolStripStatusLabel1})
		Me.statusStrip1.Location = New System.Drawing.Point(0, 539)
		Me.statusStrip1.Name = "statusStrip1"
		Me.statusStrip1.Size = New System.Drawing.Size(784, 22)
		Me.statusStrip1.TabIndex = 2
		Me.statusStrip1.Text = "statusStrip1"
		' 
		' toolStripStatusLabel1
		' 
		Me.toolStripStatusLabel1.Name = "toolStripStatusLabel1"
		Me.toolStripStatusLabel1.Size = New System.Drawing.Size(769, 17)
		Me.toolStripStatusLabel1.Spring = True
		Me.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		' 
		' imageList1
		' 
		Me.imageList1.ImageStream = (DirectCast(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer))
		Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
		Me.imageList1.Images.SetKeyName(0, "Property_16x16.png")
		' 
		' listView1
		' 
		Me.listView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader2, Me.columnHeader3})
		Me.listView1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.listView1.FullRowSelect = True
		Me.listView1.HideSelection = False
		Me.listView1.Location = New System.Drawing.Point(0, 24)
		Me.listView1.Name = "listView1"
		Me.listView1.Size = New System.Drawing.Size(784, 515)
		Me.listView1.SmallImageList = Me.imageList1
		Me.listView1.TabIndex = 3
		Me.listView1.UseCompatibleStateImageBehavior = False
		Me.listView1.View = System.Windows.Forms.View.Details
		' 
		' columnHeader2
		' 
		Me.columnHeader2.Text = "Property"
		Me.columnHeader2.Width = 123
		' 
		' columnHeader3
		' 
		Me.columnHeader3.Text = "Value"
		Me.columnHeader3.Width = 158
		' 
		' MainForm
		' 
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(784, 561)
		Me.Controls.Add(Me.listView1)
		Me.Controls.Add(Me.statusStrip1)
		Me.Controls.Add(Me.menuStrip1)
		Me.MainMenuStrip = Me.menuStrip1
		Me.Name = "MainForm"
		Me.Text = "File Properties API Demo"
		Me.menuStrip1.ResumeLayout(False)
		Me.menuStrip1.PerformLayout()
		Me.statusStrip1.ResumeLayout(False)
		Me.statusStrip1.PerformLayout()
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub

	#End Region

	Private menuStrip1 As System.Windows.Forms.MenuStrip
	Private fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents openToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents saveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private WithEvents closeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private toolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
	Private exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
	Private statusStrip1 As System.Windows.Forms.StatusStrip
	Private listView1 As ListViewEx
	Private columnHeader2 As System.Windows.Forms.ColumnHeader
	Private columnHeader3 As System.Windows.Forms.ColumnHeader
	Private toolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
	Private imageList1 As System.Windows.Forms.ImageList
	Private toolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
End Class

