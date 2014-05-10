Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Draft
	''' <summary>
	''' Copies the 1st parts list of the active draft to the clipboard.
	''' </summary>
	Friend Class CopyPartsListsToClipboard
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim partsLists As SolidEdgeDraft.PartsLists = Nothing
			Dim partsList As SolidEdgeDraft.PartsList = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(False)

				' Get a reference to the active draft document.
				draftDocument = application.TryActiveDocumentAs(Of SolidEdgeDraft.DraftDocument)()

				If draftDocument IsNot Nothing Then
					' Get a reference to the PartsLists collection.
					partsLists = draftDocument.PartsLists

					If partsLists.Count > 0 Then
						' Get a reference to the 1st parts list.
						partsList = partsLists.Item(1)

						' Copy parts list to clipboard.
						partsList.CopyToClipboard()
					Else
						Throw New System.Exception(Resources.NoPartsListsInDraftDocument)
					End If
				Else
					Throw New System.Exception(Resources.NoActiveDraftDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
