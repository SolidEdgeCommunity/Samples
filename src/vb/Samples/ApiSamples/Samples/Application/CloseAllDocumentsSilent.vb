Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Application
	''' <summary>
	''' Closes all open documents without prompting to save.
	''' </summary>
	Friend Class CloseAllDocumentsSilent
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(False)

				If application IsNot Nothing Then
					' Disable alerts. This will prevent the Save dialog from showing.
					application.DisplayAlerts = False

					' Get a reference to the documents collection.
					documents = application.Documents

					' Close all documents.
					documents.Close()
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				If application IsNot Nothing Then
					application.DisplayAlerts = True
				End If

				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
