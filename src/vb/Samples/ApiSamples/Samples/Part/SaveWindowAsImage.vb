Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Part
	''' <summary>
	''' Saves the active 3D window as an image.
	''' </summary>
	Friend Class SaveWindowAsImage
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim partDocument As SolidEdgePart.PartDocument = Nothing
			Dim window As SolidEdgeFramework.Window = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Start()

				' Get a reference to the active document.
				partDocument = application.GetActiveDocument(Of SolidEdgePart.PartDocument)(False)

				' Make sure we have a document.
				If partDocument IsNot Nothing Then
					' 3D windows are of type SolidEdgeFramework.Window.
					window = TryCast(application.ActiveWindow, SolidEdgeFramework.Window)

					If window IsNot Nothing Then
						WindowHelper.SaveAsImage(window)
					Else
						Throw New System.Exception(Resources.NoActive3dWindow)
					End If
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
