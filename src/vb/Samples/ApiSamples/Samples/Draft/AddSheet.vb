Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Draft
	''' <summary>
	''' Adds a new sheet in the working section of the active draft.
	''' </summary>
	Friend Class AddSheet
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sheets As SolidEdgeDraft.Sheets = Nothing
			Dim sheet As SolidEdgeDraft.Sheet = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(False)

				' Get a reference to the active draft document.
				draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

				If draftDocument IsNot Nothing Then
					' Get a reference to the Sheets collection.
					sheets = draftDocument.Sheets

					' Add a new sheet.
					sheet = sheets.AddSheet()

					' Make the new sheet the active sheet.
					sheet.Activate()
				Else
					Throw New System.Exception(Resources.NoActiveDraftDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
