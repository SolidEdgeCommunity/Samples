Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Application
	''' <summary>
	''' Adds objects from the active document to the document select set.
	''' </summary>
	Friend Class AddObjectsToSelectSet
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

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
				application = SolidEdgeCommunity.SolidEdgeInstall.Start()

				' Get a reference to the active document.
				document = application.GetActiveDocument(Of SolidEdgeFramework.SolidEdgeDocument)(False)

				If document IsNot Nothing Then
					' Determine document type.
					Select Case document.Type
						Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument
							assemblyDocument = DirectCast(document, SolidEdgeAssembly.AssemblyDocument)
							asmRefPlanes = assemblyDocument.AsmRefPlanes
							selectSet = assemblyDocument.SelectSet

							For i As Integer = 1 To asmRefPlanes.Count
								selectSet.Add(asmRefPlanes.Item(i))
							Next i

						Case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument
							draftDocument = DirectCast(document, SolidEdgeDraft.DraftDocument)
							sheet = draftDocument.ActiveSheet
							drawingViews = sheet.DrawingViews
							selectSet = draftDocument.SelectSet

							For i As Integer = 1 To drawingViews.Count
								draftDocument.SelectSet.Add(drawingViews.Item(i))
							Next i

						Case SolidEdgeFramework.DocumentTypeConstants.igPartDocument
							partDocument = DirectCast(document, SolidEdgePart.PartDocument)
							edgeBarFeatures = partDocument.DesignEdgebarFeatures
							selectSet = partDocument.SelectSet

							For i As Integer = 1 To edgeBarFeatures.Count
								partDocument.SelectSet.Add(edgeBarFeatures.Item(i))
							Next i

						Case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument
							sheetMetalDocument = DirectCast(document, SolidEdgePart.SheetMetalDocument)
							edgeBarFeatures = sheetMetalDocument.DesignEdgebarFeatures
							selectSet = sheetMetalDocument.SelectSet

							For i As Integer = 1 To edgeBarFeatures.Count
								partDocument.SelectSet.Add(edgeBarFeatures.Item(i))
							Next i
					End Select
				Else
					Throw New System.Exception(Resources.NoActiveDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
