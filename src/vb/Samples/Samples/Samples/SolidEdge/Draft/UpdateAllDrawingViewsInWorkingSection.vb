Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Draft
	''' <summary>
	''' Updates all drawing views in the working section of the active draft.
	''' </summary>
	Friend Class UpdateAllDrawingViewsInWorkingSection
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sections As SolidEdgeDraft.Sections = Nothing
			Dim section As SolidEdgeDraft.Section = Nothing
			Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
			Dim sheet As SolidEdgeDraft.Sheet = Nothing
			Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing
			Dim drawingView As SolidEdgeDraft.DrawingView = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = ApplicationHelper.Connect(False)

				draftDocument = application.TryActiveDocumentAs(Of SolidEdgeDraft.DraftDocument)()

				If draftDocument IsNot Nothing Then
					' Get a reference to the Sections collection.
					sections = draftDocument.Sections

					' Get a reference to the WorkingSection.
					section = sections.WorkingSection

					' Get a reference to the Sheets collection.
					sectionSheets = section.Sheets

					For i As Integer = 1 To sectionSheets.Count
						sheet = sectionSheets.Item(i)

						' Get a reference to the DrawingViews collection.
						drawingViews = sheet.DrawingViews

						For j As Integer = 1 To drawingViews.Count - 1
							drawingView = drawingViews.Item(j)

							' Updates an out-of-date drawing view.
							drawingView.Update()

							' Updates the drawing view even if it is not out-of-date.
							'drawingView.ForceUpdate();
						Next j
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
