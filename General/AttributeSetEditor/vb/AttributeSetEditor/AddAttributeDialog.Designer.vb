Partial Public Class AddAttributeDialog
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
        Me.buttonOK = New System.Windows.Forms.Button()
        Me.buttonCancel = New System.Windows.Forms.Button()
        Me.textBoxSetName = New System.Windows.Forms.TextBox()
        Me.textBoxAttributeName = New System.Windows.Forms.TextBox()
        Me.textBoxAttributeValue = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        ' 
        ' buttonOK
        ' 
        Me.buttonOK.Anchor = (CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.buttonOK.Enabled = False
        Me.buttonOK.Location = New System.Drawing.Point(284, 128)
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
        Me.buttonCancel.Location = New System.Drawing.Point(365, 128)
        Me.buttonCancel.Name = "buttonCancel"
        Me.buttonCancel.Size = New System.Drawing.Size(75, 23)
        Me.buttonCancel.TabIndex = 1
        Me.buttonCancel.Text = "&Cancel"
        Me.buttonCancel.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        ' 
        ' textBoxSetName
        ' 
        Me.textBoxSetName.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.textBoxSetName.Location = New System.Drawing.Point(100, 14)
        Me.textBoxSetName.Name = "textBoxSetName"
        Me.textBoxSetName.Size = New System.Drawing.Size(340, 20)
        Me.textBoxSetName.TabIndex = 2
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.textBoxSetName.TextChanged += new System.EventHandler(this.textBoxSetName_TextChanged);
        ' 
        ' textBoxAttributeName
        ' 
        Me.textBoxAttributeName.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.textBoxAttributeName.Location = New System.Drawing.Point(100, 40)
        Me.textBoxAttributeName.Name = "textBoxAttributeName"
        Me.textBoxAttributeName.Size = New System.Drawing.Size(340, 20)
        Me.textBoxAttributeName.TabIndex = 3
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.textBoxAttributeName.TextChanged += new System.EventHandler(this.textBoxAttributeName_TextChanged);
        ' 
        ' textBoxAttributeValue
        ' 
        Me.textBoxAttributeValue.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.textBoxAttributeValue.Location = New System.Drawing.Point(100, 66)
        Me.textBoxAttributeValue.Name = "textBoxAttributeValue"
        Me.textBoxAttributeValue.Size = New System.Drawing.Size(340, 20)
        Me.textBoxAttributeValue.TabIndex = 4
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.textBoxAttributeValue.TextChanged += new System.EventHandler(this.textBoxAttributeValue_TextChanged);
        ' 
        ' label1
        ' 
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 14)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(57, 13)
        Me.label1.TabIndex = 5
        Me.label1.Text = "Set Name:"
        ' 
        ' label2
        ' 
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(12, 40)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(80, 13)
        Me.label2.TabIndex = 6
        Me.label2.Text = "Attribute Name:"
        ' 
        ' label3
        ' 
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(12, 66)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(79, 13)
        Me.label3.TabIndex = 7
        Me.label3.Text = "Attribute Value:"
        ' 
        ' label4
        ' 
        Me.label4.AutoSize = True
        Me.label4.ForeColor = System.Drawing.Color.Red
        Me.label4.Location = New System.Drawing.Point(12, 100)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(434, 13)
        Me.label4.TabIndex = 8
        Me.label4.Text = "* Only string values are supported in this sample. (AttributeTypeConstants.seStri" & "ngUnicode)"
        ' 
        ' AddAttributeDialog
        ' 
        Me.AcceptButton = Me.buttonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.buttonCancel
        Me.ClientSize = New System.Drawing.Size(452, 163)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.textBoxAttributeValue)
        Me.Controls.Add(Me.textBoxAttributeName)
        Me.Controls.Add(Me.textBoxSetName)
        Me.Controls.Add(Me.buttonCancel)
        Me.Controls.Add(Me.buttonOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddAttributeDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Attribute"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    #End Region

    Private WithEvents buttonOK As System.Windows.Forms.Button
    Private WithEvents buttonCancel As System.Windows.Forms.Button
    Private WithEvents textBoxSetName As System.Windows.Forms.TextBox
    Private WithEvents textBoxAttributeName As System.Windows.Forms.TextBox
    Private WithEvents textBoxAttributeValue As System.Windows.Forms.TextBox
    Private label1 As System.Windows.Forms.Label
    Private label2 As System.Windows.Forms.Label
    Private label3 As System.Windows.Forms.Label
    Private label4 As System.Windows.Forms.Label
End Class