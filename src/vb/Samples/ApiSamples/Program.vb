Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows.Forms

Namespace ApiSamples
	Friend NotInheritable Class Program

		Private Sub New()
		End Sub

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread> _
		Shared Sub Main()
			System.Windows.Forms.Application.EnableVisualStyles()
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(False)
			System.Windows.Forms.Application.Run(New MainForm())
		End Sub
	End Class
End Namespace
