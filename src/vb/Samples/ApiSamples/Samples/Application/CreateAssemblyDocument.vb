Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Application
	''' <summary>
	''' Creates a new assembly document.
	''' </summary>
	Friend Class CreateAssemblyDocument
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get a reference to the documents collection.
				documents = application.Documents

				' Create a new assembly document.
				assemblyDocument = DirectCast(documents.Add(Global.SolidEdge.PROGID.AssemblyDocument), SolidEdgeAssembly.AssemblyDocument)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
