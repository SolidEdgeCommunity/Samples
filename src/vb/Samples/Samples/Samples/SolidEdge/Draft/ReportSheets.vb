Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Draft
	''' <summary>
	''' Reports all sheets of the active draft.
	''' </summary>
	Friend Class ReportSheets
		Friend Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sheets As SolidEdgeDraft.Sheets = Nothing
			Dim sheet As SolidEdgeDraft.Sheet = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect()

				' Get a reference to the active document.
				draftDocument = application.TryActiveDocumentAs(Of SolidEdgeDraft.DraftDocument)()

				' Make sure we have a document.
				If draftDocument IsNot Nothing Then
					' Get a reference to the sheets collection.
					sheets = draftDocument.Sheets

					For i As Integer = 1 To sheets.Count
						sheet = sheets.Item(i)

						Console.WriteLine("Name: {0}", sheet.Name)
						Console.WriteLine("Index: {0}", sheet.Index)
						Console.WriteLine("Number: {0}", sheet.Number)
						Console.WriteLine("SectionType: {0}", sheet.SectionType)
						Console.WriteLine()
					Next i
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
