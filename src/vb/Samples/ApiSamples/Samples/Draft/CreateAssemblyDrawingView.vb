Imports SolidEdgeFramework.Extensions 'SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace ApiSamples.Draft
	''' <summary>
	''' Creates a new draft with a drawing view of an assembly.
	''' </summary>
	Friend Class CreateAssemblyDrawingView
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim modelLinks As SolidEdgeDraft.ModelLinks = Nothing
			Dim modelLink As SolidEdgeDraft.ModelLink = Nothing
			Dim sheet As SolidEdgeDraft.Sheet = Nothing
			Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing
			Dim drawingView As SolidEdgeDraft.DrawingView = Nothing
			Dim filename As String = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeInstall.Connect(True, True)

				' Get a reference to the documents collection.
				documents = application.Documents

				' Create a new draft document.
				draftDocument = documents.AddDraftDocument()

				' Get a reference to the active sheet.
				sheet = draftDocument.ActiveSheet

				' Build path to part file.
				filename = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeInstall.GetTrainingFolderPath(), "Coffee Pot.asm")

				' Get a reference to the ModelLinks collection.
				modelLinks = draftDocument.ModelLinks

				' Add a new model link.
				modelLink = modelLinks.Add(filename)

				' Get a reference to the DrawingViews collection.
				drawingViews = sheet.DrawingViews

				' Add a new part drawing view.
				drawingView = drawingViews.AddAssemblyView([From]:= modelLink, Orientation:= SolidEdgeDraft.ViewOrientationConstants.igDimetricTopBackLeftView, Scale:= 1.0, x:= 0.4, y:= 0.4, ViewType:= SolidEdgeDraft.AssemblyDrawingViewTypeConstants.seAssemblyDesignedView)
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
