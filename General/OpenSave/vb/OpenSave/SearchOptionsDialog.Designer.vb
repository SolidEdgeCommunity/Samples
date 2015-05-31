Partial Public Class SearchOptionsDialog
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
        Dim listViewItem6 As New System.Windows.Forms.ListViewItem(".asm")
        Dim listViewItem7 As New System.Windows.Forms.ListViewItem(".dft")
        Dim listViewItem8 As New System.Windows.Forms.ListViewItem(".par")
        Dim listViewItem9 As New System.Windows.Forms.ListViewItem(".psm")
        Dim listViewItem10 As New System.Windows.Forms.ListViewItem(".pwd")
        Me.buttonOK = New System.Windows.Forms.Button()
        Me.buttonCancel = New System.Windows.Forms.Button()
        Me.listViewExtensions = New SolidEdge.OpenSave.ListViewEx()
        Me.columnHeader1 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.imageList = New System.Windows.Forms.ImageList(Me.components)
        Me.listViewFolders = New SolidEdge.OpenSave.ListViewEx()
        Me.columnHeader2 = (CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader))
        Me.checkBoxIncludeSubDirectories = New System.Windows.Forms.CheckBox()
        Me.buttonAdd = New System.Windows.Forms.Button()
        Me.buttonRemove = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        ' 
        ' buttonOK
        ' 
        Me.buttonOK.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.buttonOK.Location = New System.Drawing.Point(365, 247)
        Me.buttonOK.Name = "buttonOK"
        Me.buttonOK.Size = New System.Drawing.Size(75, 23)
        Me.buttonOK.TabIndex = 0
        Me.buttonOK.Text = "&OK"
        Me.buttonOK.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
        ' 
        ' buttonCancel
        ' 
        Me.buttonCancel.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.buttonCancel.Location = New System.Drawing.Point(446, 247)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(75, 23)
        Me.buttonCancel.TabIndex = 1
        Me.buttonCancel.Text = "&Cancel"
        Me.buttonCancel.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        ' 
        ' listViewExtensions
        ' 
        Me.listViewExtensions.CheckBoxes = True
        Me.listViewExtensions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader1})
        Me.listViewExtensions.FullRowSelect = True
        listViewItem6.StateImageIndex = 0
        listViewItem7.StateImageIndex = 0
        listViewItem8.StateImageIndex = 0
        listViewItem9.StateImageIndex = 0
        listViewItem10.StateImageIndex = 0
        Me.listViewExtensions.Items.AddRange(New System.Windows.Forms.ListViewItem() { listViewItem6, listViewItem7, listViewItem8, listViewItem9, listViewItem10})
        Me.listViewExtensions.Location = New System.Drawing.Point(12, 12)
        Me.listViewExtensions.Name = "listViewExtensions"
        Me.listViewExtensions.Size = New System.Drawing.Size(92, 215)
        Me.listViewExtensions.SmallImageList = Me.imageList
        Me.listViewExtensions.TabIndex = 2
        Me.listViewExtensions.UseCompatibleStateImageBehavior = False
        Me.listViewExtensions.View = System.Windows.Forms.View.Details
        ' 
        ' columnHeader1
        ' 
        Me.columnHeader1.Text = "Extension"
        Me.columnHeader1.Width = 88
        ' 
        ' imageList
        ' 
        Me.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.imageList.ImageSize = New System.Drawing.Size(16, 16)
        Me.imageList.TransparentColor = System.Drawing.Color.Transparent
        ' 
        ' listViewFolders
        ' 
        Me.listViewFolders.Anchor = (CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.listViewFolders.Columns.AddRange(New System.Windows.Forms.ColumnHeader() { Me.columnHeader2})
        Me.listViewFolders.Location = New System.Drawing.Point(110, 35)
        Me.listViewFolders.Name = "listViewFolders"
        Me.listViewFolders.Size = New System.Drawing.Size(411, 163)
        Me.listViewFolders.TabIndex = 5
        Me.listViewFolders.UseCompatibleStateImageBehavior = False
        Me.listViewFolders.View = System.Windows.Forms.View.Details
        ' 
        ' columnHeader2
        ' 
        Me.columnHeader2.Text = "Folder"
        ' 
        ' checkBoxIncludeSubDirectories
        ' 
        Me.checkBoxIncludeSubDirectories.AutoSize = True
        Me.checkBoxIncludeSubDirectories.Location = New System.Drawing.Point(110, 12)
        Me.checkBoxIncludeSubDirectories.Name = "checkBoxIncludeSubDirectories"
        Me.checkBoxIncludeSubDirectories.Size = New System.Drawing.Size(132, 17)
        Me.checkBoxIncludeSubDirectories.TabIndex = 6
        Me.checkBoxIncludeSubDirectories.Text = "Include sub directories"
        Me.checkBoxIncludeSubDirectories.UseVisualStyleBackColor = True
        ' 
        ' buttonAdd
        ' 
        Me.buttonAdd.Location = New System.Drawing.Point(110, 204)
        Me.buttonAdd.Name = "buttonAdd"
        Me.buttonAdd.Size = New System.Drawing.Size(75, 23)
        Me.buttonAdd.TabIndex = 7
        Me.buttonAdd.Text = "Add"
        Me.buttonAdd.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
        ' 
        ' buttonRemove
        ' 
        Me.buttonRemove.Location = New System.Drawing.Point(191, 204)
        Me.buttonRemove.Name = "buttonRemove"
        Me.buttonRemove.Size = New System.Drawing.Size(75, 23)
        Me.buttonRemove.TabIndex = 8
        Me.buttonRemove.Text = "Remove"
        Me.buttonRemove.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
        ' 
        ' SearchOptionsDialog
        ' 
        Me.AcceptButton = Me.buttonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.buttonCancel
        Me.ClientSize = New System.Drawing.Size(533, 282)
        Me.Controls.Add(Me.buttonRemove)
        Me.Controls.Add(Me.buttonAdd)
        Me.Controls.Add(Me.checkBoxIncludeSubDirectories)
        Me.Controls.Add(Me.listViewFolders)
        Me.Controls.Add(Me.listViewExtensions)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SearchOptionsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Search Options"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.SearchOptionsDialog_Load);
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    #End Region

    Private WithEvents buttonOK As System.Windows.Forms.Button
    Private WithEvents buttonCancel As System.Windows.Forms.Button
    Private listViewExtensions As ListViewEx
    Private columnHeader1 As System.Windows.Forms.ColumnHeader
    Private imageList As System.Windows.Forms.ImageList
    Private listViewFolders As ListViewEx
    Private checkBoxIncludeSubDirectories As System.Windows.Forms.CheckBox
    Private WithEvents buttonAdd As System.Windows.Forms.Button
    Private WithEvents buttonRemove As System.Windows.Forms.Button
    Private columnHeader2 As System.Windows.Forms.ColumnHeader
End Class