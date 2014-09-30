Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Draft
	''' <summary>
	''' Creates a new draft and adds a circle by center radius.
	''' </summary>
	Friend Class CreateCircle2dByCenterRadius
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
			Dim sheet As SolidEdgeDraft.Sheet = Nothing
			Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing
			Dim circle2d As SolidEdgeFrameworkSupport.Circle2d = Nothing
			Dim geometryStyle2d As SolidEdgeFrameworkSupport.GeometryStyle2d = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the Documents collection.
				documents = application.Documents

				' Create a new draft document.
				draftDocument = documents.AddDraftDocument()

				' Get a reference to the active sheet.
				sheet = draftDocument.ActiveSheet

				' Get a reference to the Circles2d collection.
				circles2d = sheet.Circles2d

				' Add the circle.
				circle2d = circles2d.AddByCenterRadius(0.2, 0.2, 0.1)

				' Get a reference to the GeometryStyle2d to modify the style.
				geometryStyle2d = circle2d.Style

			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
