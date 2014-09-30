Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Friend Class TextBoxConsole
	Inherits TextWriter

	Private textBox As TextBox
	Private _buffer As New StringBuilder()

	Public Sub New(ByVal textBox As TextBox)
		Me.textBox = textBox
	End Sub

	Public Overrides Sub WriteLine()
		Try
			textBox.Do(Sub(ctl) ctl.AppendText(NewLine))
		Catch
		End Try
	End Sub

	Public Overrides Sub WriteLine(ByVal value As String)
		Try
			textBox.Do(Sub(ctl)
				ctl.AppendText(value)
				ctl.AppendText(NewLine)
			End Sub)
		Catch
		End Try
	End Sub

	Public Overrides ReadOnly Property Encoding() As Encoding
		Get
			Return System.Text.Encoding.UTF8
		End Get
	End Property
End Class

