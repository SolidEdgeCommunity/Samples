Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace AddObjectsToSelectSet
	''' <summary>
	''' Adds objects from the active document to the document select set.
	''' </summary>
	Friend Class Program
		<STAThread>
		Shared Sub Main(ByVal args() As String)
			Dim application As SolidEdgeFramework.Application = Nothing
			Dim document As SolidEdgeFramework.SolidEdgeDocument = Nothing
			Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
			Dim assemblyDocument As SolidEdgeAssembly.AssemblyDocument = Nothing
			Dim asmRefPlanes As SolidEdgeAssembly.AsmRefPlanes = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sheet As SolidEdgeDraft.Sheet = Nothing
			Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
			Dim edgeBarFeatures As SolidEdgePart.EdgebarFeatures = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect()

				' Get a reference to the active selectset.
				selectSet = application.ActiveSelectSet

				' Temporarily suspend selectset UI updates.
				selectSet.SuspendDisplay()

				' Clear the selectset.
				selectSet.RemoveAll()

				' Get a reference to the active document.
				document = application.GetActiveDocument(Of SolidEdgeFramework.SolidEdgeDocument)(False)

				If document IsNot Nothing Then
					' Determine document type.
					Select Case document.Type
						Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument
							assemblyDocument = DirectCast(document, SolidEdgeAssembly.AssemblyDocument)
							asmRefPlanes = assemblyDocument.AsmRefPlanes

							For i As Integer = 1 To asmRefPlanes.Count
								selectSet.Add(asmRefPlanes.Item(i))
							Next i

						Case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument
							draftDocument = DirectCast(document, SolidEdgeDraft.DraftDocument)
							sheet = draftDocument.ActiveSheet
							drawingViews = sheet.DrawingViews

							For i As Integer = 1 To drawingViews.Count
								draftDocument.SelectSet.Add(drawingViews.Item(i))
							Next i

						Case SolidEdgeFramework.DocumentTypeConstants.igPartDocument
							partDocument = DirectCast(document, SolidEdgePart.PartDocument)
							edgeBarFeatures = partDocument.DesignEdgebarFeatures

							For i As Integer = 1 To edgeBarFeatures.Count
								partDocument.SelectSet.Add(edgeBarFeatures.Item(i))
							Next i

						Case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument
							sheetMetalDocument = DirectCast(document, SolidEdgePart.SheetMetalDocument)
							edgeBarFeatures = sheetMetalDocument.DesignEdgebarFeatures

							For i As Integer = 1 To edgeBarFeatures.Count
								partDocument.SelectSet.Add(edgeBarFeatures.Item(i))
							Next i
					End Select
				Else
					Throw New System.Exception("No active document")
				End If

				' Re-enable selectset UI display.
				selectSet.ResumeDisplay()

				' Manually refresh the selectset UI display.
				selectSet.RefreshDisplay()
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
