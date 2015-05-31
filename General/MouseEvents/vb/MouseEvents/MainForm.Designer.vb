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
        Me.menuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.fileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.exitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.toolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.toolStripLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.comboBoxLocateModes = New System.Windows.Forms.ToolStripComboBox()
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.outputTextBox = New System.Windows.Forms.TextBox()
        Me.splitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.buttonStartCommand = New System.Windows.Forms.ToolStripButton()
        Me.buttonStopCommand = New System.Windows.Forms.ToolStripButton()
        Me.toolStripLabel2 = New System.Windows.Forms.ToolStripLabel()
        Me.comboBoxEnableMouseMoveEvent = New System.Windows.Forms.ToolStripComboBox()
        Me.listViewFilters = New SolidEdge.MouseEvents.ListViewEx()
        Me.columnHeader1 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.menuStrip1.SuspendLayout()
        Me.toolStrip1.SuspendLayout()
        DirectCast(Me.splitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer2.Panel1.SuspendLayout()
        Me.splitContainer2.Panel2.SuspendLayout()
        Me.splitContainer2.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' menuStrip1
        ' 
        Me.menuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.fileToolStripMenuItem})
        Me.menuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.menuStrip1.Name = "menuStrip1"
        Me.menuStrip1.Size = New System.Drawing.Size(622, 24)
        Me.menuStrip1.TabIndex = 2
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
        Me.toolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() { Me.buttonStartCommand, Me.buttonStopCommand, Me.toolStripSeparator1, Me.toolStripLabel1, Me.comboBoxLocateModes, Me.toolStripLabel2, Me.comboBoxEnableMouseMoveEvent})
        Me.toolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.toolStrip1.Name = "toolStrip1"
        Me.toolStrip1.Size = New System.Drawing.Size(622, 25)
        Me.toolStrip1.TabIndex = 3
        Me.toolStrip1.Text = "toolStrip1"
        ' 
        ' toolStripSeparator1
        ' 
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        Me.toolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        ' 
        ' toolStripLabel1
        ' 
        Me.toolStripLabel1.Name = "toolStripLabel1"
        Me.toolStripLabel1.Size = New System.Drawing.Size(76, 22)
        Me.toolStripLabel1.Text = "Locate Mode"
        ' 
        ' comboBoxLocateModes
        ' 
        Me.comboBoxLocateModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBoxLocateModes.Name = "comboBoxLocateModes"
        Me.comboBoxLocateModes.Size = New System.Drawing.Size(125, 25)
        ' 
        ' statusStrip1
        ' 
        Me.statusStrip1.Location = New System.Drawing.Point(0, 394)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Size = New System.Drawing.Size(622, 22)
        Me.statusStrip1.TabIndex = 4
        Me.statusStrip1.Text = "statusStrip1"
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
        Me.outputTextBox.Size = New System.Drawing.Size(411, 345)
        Me.outputTextBox.TabIndex = 3
        Me.outputTextBox.WordWrap = False
        ' 
        ' splitContainer2
        ' 
        Me.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer2.Location = New System.Drawing.Point(0, 49)
        Me.splitContainer2.Name = "splitContainer2"
        ' 
        ' splitContainer2.Panel1
        ' 
        Me.splitContainer2.Panel1.Controls.Add(Me.listViewFilters)
        ' 
        ' splitContainer2.Panel2
        ' 
        Me.splitContainer2.Panel2.Controls.Add(Me.outputTextBox)
        Me.splitContainer2.Size = New System.Drawing.Size(622, 345)
        Me.splitContainer2.SplitterDistance = 207
        Me.splitContainer2.TabIndex = 6
        ' 
        ' buttonStartCommand
        ' 
        Me.buttonStartCommand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonStartCommand.Image = My.Resources.Run_16x16
        Me.buttonStartCommand.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonStartCommand.Name = "buttonStartCommand"
        Me.buttonStartCommand.Size = New System.Drawing.Size(23, 22)
        Me.buttonStartCommand.Text = "Start Command"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonStartCommand.Click += new System.EventHandler(this.buttonStartCommand_Click);
        ' 
        ' buttonStopCommand
        ' 
        Me.buttonStopCommand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.buttonStopCommand.Enabled = False
        Me.buttonStopCommand.Image = My.Resources.Stop_16x16
        Me.buttonStopCommand.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.buttonStopCommand.Name = "buttonStopCommand"
        Me.buttonStopCommand.Size = New System.Drawing.Size(23, 22)
        Me.buttonStopCommand.Text = "Stop Command"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonStopCommand.Click += new System.EventHandler(this.buttonStopCommand_Click);
        ' 
        ' toolStripLabel2
        ' 
        Me.toolStripLabel2.Name = "toolStripLabel2"
        Me.toolStripLabel2.Size = New System.Drawing.Size(143, 22)
        Me.toolStripLabel2.Text = "Enable MouseMove Event"
        ' 
        ' comboBoxEnableMouseMoveEvent
        ' 
        Me.comboBoxEnableMouseMoveEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comboBoxEnableMouseMoveEvent.Name = "comboBoxEnableMouseMoveEvent"
        Me.comboBoxEnableMouseMoveEvent.Size = New System.Drawing.Size(121, 25)
        ' 
        ' listViewFilters
        ' 
        Me.listViewFilters.CheckBoxes = True
        Me.listViewFilters.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader1})
        Me.listViewFilters.Dock = System.Windows.Forms.DockStyle.Fill
        Me.listViewFilters.FullRowSelect = True
        Me.listViewFilters.HideSelection = False
        Me.listViewFilters.Location = New System.Drawing.Point(0, 0)
        Me.listViewFilters.Name = "listViewFilters"
        Me.listViewFilters.Size = New System.Drawing.Size(207, 345)
        Me.listViewFilters.TabIndex = 1
        Me.listViewFilters.UseCompatibleStateImageBehavior = False
        Me.listViewFilters.View = System.Windows.Forms.View.Details
        ' 
        ' columnHeader1
        ' 
        Me.columnHeader1.Text = "Filter"
        Me.columnHeader1.Width = 145
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(622, 416)
        Me.Controls.Add(Me.splitContainer2)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.toolStrip1)
        Me.Controls.Add(Me.menuStrip1)
        Me.DoubleBuffered = True
        Me.MainMenuStrip = Me.menuStrip1
        Me.Name = "MainForm"
        Me.Text = "Solid Edge Mouse Events"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.MainForm_Load);
        Me.menuStrip1.ResumeLayout(False)
        Me.menuStrip1.PerformLayout()
        Me.toolStrip1.ResumeLayout(False)
        Me.toolStrip1.PerformLayout()
        Me.splitContainer2.Panel1.ResumeLayout(False)
        Me.splitContainer2.Panel2.ResumeLayout(False)
        Me.splitContainer2.Panel2.PerformLayout()
        DirectCast(Me.splitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    #End Region

    Private listViewFilters As ListViewEx
    Private columnHeader1 As System.Windows.Forms.ColumnHeader
    Private menuStrip1 As System.Windows.Forms.MenuStrip
    Private fileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents exitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private toolStrip1 As System.Windows.Forms.ToolStrip
    Private WithEvents buttonStartCommand As System.Windows.Forms.ToolStripButton
    Private statusStrip1 As System.Windows.Forms.StatusStrip
    Private outputTextBox As System.Windows.Forms.TextBox
    Private splitContainer2 As System.Windows.Forms.SplitContainer
    Private toolStripLabel1 As System.Windows.Forms.ToolStripLabel
    Private comboBoxLocateModes As System.Windows.Forms.ToolStripComboBox
    Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents buttonStopCommand As System.Windows.Forms.ToolStripButton
    Private toolStripLabel2 As System.Windows.Forms.ToolStripLabel
    Private comboBoxEnableMouseMoveEvent As System.Windows.Forms.ToolStripComboBox
End Class

