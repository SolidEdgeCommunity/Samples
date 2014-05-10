Imports ApiSamples.Samples.SolidEdge
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge.Draft
	Friend Class ReportSections
		Friend Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sections As SolidEdgeDraft.Sections = Nothing
			Dim section As SolidEdgeDraft.Section = Nothing
			Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
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
					' Get a reference to the sections collection.
					sections = draftDocument.Sections

					For i As Integer = 1 To sections.Count
						section = sections.Item(i)

						Console.WriteLine("Name: {0}", section.Name)

						Try
							' Index may throw an exception.
							Console.WriteLine("Index: {0}", section.Index)
						Catch
						End Try

						sectionSheets = section.Sheets

						For j As Integer = 1 To sectionSheets.Count
							sheet = sectionSheets.Item(j)

							Console.WriteLine("SectionSheets[{0}]: {1}", j, sheet.Name)
						Next j

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
