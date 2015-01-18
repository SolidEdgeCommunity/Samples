Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Draft
	Friend Class ReportSections
		Friend Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sections As SolidEdgeDraft.Sections = Nothing
			Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the active document.
				draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

				' Make sure we have a document.
				If draftDocument IsNot Nothing Then
					' Get a reference to the sections collection.
					sections = draftDocument.Sections

					For Each section In sections.OfType(Of SolidEdgeDraft.Section)()
						Console.WriteLine("Name: {0}", section.Name)

						Try
							' Index may throw an exception.
							Console.WriteLine("Index: {0}", section.Index)
						Catch
						End Try

						sectionSheets = section.Sheets

						For Each sheet In sectionSheets.OfType(Of SolidEdgeDraft.Sheet)()
							Console.WriteLine("SectionSheets[{0}]: {1}", sheet.Index, sheet.Name)
						Next sheet

						Console.WriteLine()
					Next section
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
