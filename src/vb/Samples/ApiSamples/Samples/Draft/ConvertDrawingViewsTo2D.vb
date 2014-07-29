Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Draft
	''' <summary>
	''' Converts all drawing views of the active draft to 2D views.
	''' </summary>
	Friend Class ConvertDrawingViewsTo2D
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
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get a reference to the active draft document.
				draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

				If draftDocument IsNot Nothing Then
					' Get a reference to the Sections collection.
					sections = draftDocument.Sections

					' Get a reference to the working section.
					section = sections.WorkingSection

					' Get a reference to the working section sheets.
					sectionSheets = section.Sheets

					For i As Integer = 1 To sectionSheets.Count
						' Get a reference to the sheet.
						sheet = sectionSheets.Item(i)

						' Get a reference to the DrawingViews collection.
						drawingViews = sheet.DrawingViews

						For j As Integer = 1 To drawingViews.Count - 1
							drawingView = drawingViews.Item(j)

							' DrawingView's of type igUserView cannot be converted.
							If drawingView.DrawingViewType <> SolidEdgeDraft.DrawingViewTypeConstants.igUserView Then
								' Converts the current DrawingView to an igUserView type containing simple geometry
								' and disassociates the drawing view from the source 3d Model.
								drawingView.Drop()
							End If
						Next j
					Next i
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
