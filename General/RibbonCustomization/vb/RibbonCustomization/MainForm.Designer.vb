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
        Me.buttonCreateTheme = New System.Windows.Forms.Button()
        Me.buttonDeleteTheme = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        ' 
        ' buttonCreateTheme
        ' 
        Me.buttonCreateTheme.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.buttonCreateTheme.Location = New System.Drawing.Point(77, 46)
        Me.buttonCreateTheme.Name = "buttonCreateTheme"
        Me.buttonCreateTheme.Size = New System.Drawing.Size(97, 23)
        Me.buttonCreateTheme.TabIndex = 2
        Me.buttonCreateTheme.Text = "CreateTheme"
        Me.buttonCreateTheme.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonCreateTheme.Click += new System.EventHandler(this.buttonCreateTheme_Click);
        ' 
        ' buttonDeleteTheme
        ' 
        Me.buttonDeleteTheme.Anchor = (CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles))
        Me.buttonDeleteTheme.Location = New System.Drawing.Point(77, 75)
        Me.buttonDeleteTheme.Name = "buttonDeleteTheme"
        Me.buttonDeleteTheme.Size = New System.Drawing.Size(97, 23)
        Me.buttonDeleteTheme.TabIndex = 3
        Me.buttonDeleteTheme.Text = "DeleteTheme"
        Me.buttonDeleteTheme.UseVisualStyleBackColor = True
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.buttonDeleteTheme.Click += new System.EventHandler(this.buttonDeleteTheme_Click);
        ' 
        ' MainForm
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(259, 148)
        Me.Controls.Add(Me.buttonDeleteTheme)
        Me.Controls.Add(Me.buttonCreateTheme)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "MainForm"
        Me.Text = "Solid Edge Ribbon Customization"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.MainForm_Load);
        Me.ResumeLayout(False)

    End Sub

    #End Region

    Private WithEvents buttonCreateTheme As System.Windows.Forms.Button
    Private WithEvents buttonDeleteTheme As System.Windows.Forms.Button
End Class

