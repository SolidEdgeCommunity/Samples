Namespace AddInDemo
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
			Me.richTextBox1.ReadOnly = True
			Me.richTextBox1.Size = New System.Drawing.Size(275, 364)
			Me.richTextBox1.TabIndex = 0
			Me.richTextBox1.Text = ""
			Me.richTextBox1.WordWrap = False
			' 
			' MyEdgeBarControl
			' 
			Me.AutoScaleDimensions = New System.Drawing.SizeF(6F, 13F)
			Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
			Me.Controls.Add(Me.richTextBox1)
			Me.Name = "MyEdgeBarControl"
			Me.Size = New System.Drawing.Size(275, 364)
			Me.ToolTip = "My EdgeBar Control"
'			Me.AfterInitialize += New System.EventHandler(Me.MyEdgeBarControl_AfterInitialize)
'			Me.Load += New System.EventHandler(Me.MyEdgeBarControl_Load)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private richTextBox1 As System.Windows.Forms.RichTextBox
	End Class
End Namespace
