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
        Me.menuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.eventButton = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.clearButton = New System.Windows.Forms.ToolStripButton()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.lvEvents = New SolidEdge.ApplicationEvents.ListViewEx()
        Me.columnHeader1 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.columnHeader2 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.menuStrip1.SuspendLayout()
        Me.toolStrip1.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' menuStrip1
        ' 
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.fileToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Padding = New System.Windows.Forms.Padding(7, 2, 0, 2)
        Me.menuStrip1.Size = New System.Drawing.Size(541, 24)
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
        Me.exitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.exitToolStripMenuItem.Text = "&Exit"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
        ' 
        ' toolStrip1
        ' 
        Me.toolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.eventButton, Me.toolStripSeparator1, Me.clearButton})
        Me.toolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip1.Name = "toolStrip1"
        Me.toolStrip1.Size = New System.Drawing.Size(541, 25)
        Me.toolStrip1.TabIndex = 1
        Me.toolStrip1.Text = "toolStrip1"
        ' 
        ' eventButton
        ' 
        Me.eventButton.CheckOnClick = True
        Me.eventButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.eventButton.Image = My.Resources.Event_16x16
        Me.eventButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.eventButton.Name = "eventButton"
        Me.eventButton.Size = New System.Drawing.Size(23, 22)
        Me.eventButton.Text = "Events"
        Me.eventButton.ToolTipText = "Toggles event connections"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.eventButton.CheckedChanged += new System.EventHandler(this.eventButton_CheckedChanged);
        ' 
        ' toolStripSeparator1
        ' 
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        ' 
        ' clearButton
        ' 
        Me.clearButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.clearButton.Image = My.Resources.Clear_16x16
        Me.clearButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.clearButton.Name = "clearButton"
        Me.clearButton.Size = New System.Drawing.Size(23, 22)
        Me.clearButton.Text = "toolStripButton1"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
        ' 
        ' statusStrip1
        ' 
        Me.statusStrip1.Location = New System.Drawing.Point(0, 384)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Padding = New System.Windows.Forms.Padding(1, 0, 16, 0)
        Me.statusStrip1.Size = New System.Drawing.Size(541, 22)
        Me.statusStrip1.TabIndex = 2
        Me.statusStrip1.Text = "statusStrip1"
        ' 
        ' imageList1
        ' 
        Me.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.imageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
        ' 
        ' lvEvents
        ' 
        Me.lvEvents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader1, Me.columnHeader2})
        Me.lvEvents.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvEvents.FullRowSelect = True
        Me.lvEvents.Location = New System.Drawing.Point(0, 49)
        Me.lvEvents.Name = "lvEvents"
        Me.lvEvents.Size = New System.Drawing.Size(541, 335)
        Me.lvEvents.TabIndex = 3
        Me.lvEvents.UseCompatibleStateImageBehavior = False
        Me.lvEvents.View = System.Windows.Forms.View.Details
        ' 
        ' columnHeader1
        ' 
        Me.columnHeader1.Text = "Event"
        Me.columnHeader1.Width = 155
        ' 
        ' columnHeader2
        ' 
        Me.columnHeader2.Text = "Parameters"
        Me.columnHeader2.Width = 178
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7F, 15F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(541, 406)
        Me.Controls.Add(Me.lvEvents)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.toolStrip1)
        Me.Controls.Add(Me.menuStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9F)
        Me.MainMenuStrip = Me.menuStrip1
        Me.Name = "MainForm"
        Me.Text = "Solid Edge Application Events Demo"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.MainForm_Load);
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.toolStrip1.ResumeLayout(False)
        Me.toolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    #End Region

    Private menuStrip1 As System.Windows.Forms.MenuStrip
    Private fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private toolStrip1 As System.Windows.Forms.ToolStrip
    Private statusStrip1 As System.Windows.Forms.StatusStrip
    Private WithEvents eventButton As System.Windows.Forms.ToolStripButton
    Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents clearButton As System.Windows.Forms.ToolStripButton
    Private lvEvents As ListViewEx
    Private columnHeader1 As System.Windows.Forms.ColumnHeader
    Private columnHeader2 As System.Windows.Forms.ColumnHeader
    Private imageList1 As System.Windows.Forms.ImageList
End Class

