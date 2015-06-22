Partial Public Class MyEdgeBarControl
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

    #Region "Component Designer generated code"

    ''' <summary> 
    ''' Required method for Designer support - do not modify 
    ''' the contents of this method with the code editor.
    ''' </summary>
    Private Sub InitializeComponent()
        Me.richTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.SuspendLayout()
        ' 
        ' richTextBox1
        ' 
        Me.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.richTextBox1.Location = New System.Drawing.Point(0, 0)
        Me.richTextBox1.Name = "richTextBox1"
        Me.richTextBox1.Size = New System.Drawing.Size(183, 205)
        Me.richTextBox1.TabIndex = 1
        Me.richTextBox1.Text = ""
        ' 
        ' MyEdgeBarControl
        ' 
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.richTextBox1)
        Me.Name = "MyEdgeBarControl"
        Me.Size = New System.Drawing.Size(183, 205)
        Me.ToolTip = "My EdgeBar Control"
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.AfterInitialize += new System.EventHandler(this.MyEdgeBarControl_AfterInitialize);
'INSTANT VB NOTE: The following InitializeComponent event wireup was converted to a 'Handles' clause:
'ORIGINAL LINE: this.Load += new System.EventHandler(this.MyEdgeBarControl_Load);
        Me.ResumeLayout(False)

    End Sub

    #End Region

    Private richTextBox1 As System.Windows.Forms.RichTextBox
End Class
