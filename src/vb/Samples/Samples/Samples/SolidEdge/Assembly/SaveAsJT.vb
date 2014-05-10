Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Assembly
	''' <summary>
	''' Saves the active assembly as a JT file.
	''' </summary>
	Friend Class SaveAsJT
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim document As SolidEdgeAssembly.AssemblyDocument = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to Solid Edge.
				application = ApplicationHelper.Connect(True, True)

				' Get a reference to the active document.
				document = application.TryActiveDocumentAs(Of SolidEdgeAssembly.AssemblyDocument)()

				If document IsNot Nothing Then
					SolidEdgeDocumentHelper.SaveAsJT(document)
				Else
					Throw New System.Exception(Resources.NoActiveAssemblyDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
