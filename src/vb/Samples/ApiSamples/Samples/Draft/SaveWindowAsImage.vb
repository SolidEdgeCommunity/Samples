Imports SolidEdgeCommunity.Extensions ' Enabled extension methods from SolidEdge.Community.dll
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Draft
	''' <summary>
	''' Saves the active 2D window as an image.
	''' </summary>
	Friend Class SaveWindowAsImage
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim document As SolidEdgeFramework.SolidEdgeDocument = Nothing
			Dim sheetWindow As SolidEdgeDraft.SheetWindow = Nothing

			Dim extensions() As String = { ".jpg", ".bmp", ".tif" }
			Dim guid As Guid = System.Guid.NewGuid()
			Dim folder As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Bring Solid Edge to the foreground.
				application.Activate()

				' Get a reference to the Documents collection.
				documents = application.Documents

				' Get a reference to the active document.
				document = application.GetActiveDocument(Of SolidEdgeFramework.SolidEdgeDocument)(False)

				' Make sure we have a document.
				If document Is Nothing Then
					Throw New System.Exception(Resources.NoActiveDocument)
				End If

				' 2D windows are of type SolidEdgeDraft.SheetWindow.
				sheetWindow = TryCast(application.ActiveWindow, SolidEdgeDraft.SheetWindow)

				If sheetWindow IsNot Nothing Then
					' Save each extension.
					For Each extension As String In extensions
						' File saved to desktop.
						Dim filename As String = System.IO.Path.ChangeExtension(guid.ToString(), extension)
						filename = System.IO.Path.Combine(folder, filename)

						Dim resolution As Double = 1 ' DPI - Larger values have better quality but also lead to larger file.
						Dim colorDepth As Integer = 24
						Dim width As Integer = sheetWindow.UsableWidth
						Dim height As Integer = sheetWindow.UsableHeight

						' You can specify .bmp (Windows Bitmap), .tif (TIFF), or .jpg (JPEG).
						sheetWindow.SaveAsImage(FileName:= filename, Width:= width, Height:= height, Resolution:= resolution, ColorDepth:= colorDepth, ImageQuality:= SolidEdgeFramework.SeImageQualityType.seImageQualityHigh, Invert:= False)

						Console.WriteLine("Saved '{0}'.", filename)
					Next extension
				Else
					Throw New System.Exception(Resources.NoActive2dWindow)
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
