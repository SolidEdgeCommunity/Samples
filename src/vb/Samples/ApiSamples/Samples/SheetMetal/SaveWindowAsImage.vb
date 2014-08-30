Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace SheetMetal
	''' <summary>
	''' Saves the active 3D window as an image.
	''' </summary>
	Friend Class SaveWindowAsImage
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the active document.
				sheetMetalDocument = application.GetActiveDocument(Of SolidEdgePart.SheetMetalDocument)(False)

				If sheetMetalDocument IsNot Nothing Then
					SolidEdgeDocumentHelper.SaveAsJT(sheetMetalDocument)
				Else
					Throw New System.Exception(Resources.NoActiveSheetMetalDocument)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
