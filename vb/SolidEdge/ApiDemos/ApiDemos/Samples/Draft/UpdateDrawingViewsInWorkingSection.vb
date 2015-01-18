Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Draft
	''' <summary>
	''' Updates all drawing views in the working section of the active draft.
	''' </summary>
	Friend Class UpdateDrawingViewsInWorkingSection
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sections As SolidEdgeDraft.Sections = Nothing
			Dim section As SolidEdgeDraft.Section = Nothing
			Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
			Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

				draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

				If draftDocument IsNot Nothing Then
					' Get a reference to the Sections collection.
					sections = draftDocument.Sections

					' Get a reference to the WorkingSection.
					section = sections.WorkingSection

					' Get a reference to the Sheets collection.
					sectionSheets = section.Sheets

					For Each sheet In sectionSheets.OfType(Of SolidEdgeDraft.Sheet)()
						Console.WriteLine("Processing sheet '{0}'.", sheet.Name)

						' Get a reference to the DrawingViews collection.
						drawingViews = sheet.DrawingViews

						For Each drawingView In drawingViews.OfType(Of SolidEdgeDraft.DrawingView)()
							' Updates an out-of-date drawing view.
							drawingView.Update()

							' Updates the drawing view even if it is not out-of-date.
							'drawingView.ForceUpdate();

							Console.WriteLine("Updated drawing view '{0}'.", drawingView.Name)
						Next drawingView
					Next sheet
				Else
					Throw New System.Exception(My.Resources.NoActiveDraftDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
