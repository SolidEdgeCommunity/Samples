Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.Revision_Manager
	''' <summary>
	''' Starts a new instance of RevisionManager.
	''' </summary>
	''' <remarks>
	''' Note that the application will terminate when the thread ends.
	''' </remarks>
	Friend Class StartRevisionManager
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As RevisionManager.Application = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = New RevisionManager.Application()

				' Make sure RevisionManager is visible to user.
				application.Visible = 1
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
