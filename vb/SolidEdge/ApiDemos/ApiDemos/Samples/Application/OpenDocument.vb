Imports SolidEdgeCommunity.Extensions
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Namespace Application
	''' <summary>
	''' Creates a new part document.
	''' </summary>
	Friend Class OpenDocument
		Private Shared Sub RunSample(ByVal breakOnStart As Boolean)
			If breakOnStart Then
				System.Diagnostics.Debugger.Break()
			End If

			Dim application As SolidEdgeFramework.Application = Nothing
			Dim documents As SolidEdgeFramework.Documents = Nothing

			Try
				' Register with OLE to handle concurrency issues on the current thread.
				SolidEdgeCommunity.OleMessageFilter.Register()

				' Connect to or start Solid Edge.
				application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

				' Get a reference to the documents collection.
				documents = application.Documents

				Dim dialog = New OpenFileDialog()
				dialog.Filter = "All Solid Edge documents|*.asm;*.dft;*.par;*.psm;*.pwd"
				dialog.InitialDirectory = SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath()

				If dialog.ShowDialog() = DialogResult.OK Then
					' Open existing document using extension method.
					Dim document = documents.Open(Of SolidEdgeFramework.SolidEdgeDocument)(dialog.FileName)

					' Determine the document type and cast accordingly.
					Select Case document.Type
						Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument
							Dim assemblyDocument = DirectCast(document, SolidEdgeAssembly.AssemblyDocument)
						Case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument
							Dim draftDocument = DirectCast(document, SolidEdgeDraft.DraftDocument)
						Case SolidEdgeFramework.DocumentTypeConstants.igPartDocument, SolidEdgeFramework.DocumentTypeConstants.igSyncPartDocument
							Dim partDocument = DirectCast(document, SolidEdgePart.PartDocument)
						Case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument, SolidEdgeFramework.DocumentTypeConstants.igSyncSheetMetalDocument
							Dim sheetMetalDocument = DirectCast(document, SolidEdgePart.SheetMetalDocument)
					End Select
				End If
			Catch ex As System.Exception
				Console.WriteLine(ex.Message)
			Finally
				SolidEdgeCommunity.OleMessageFilter.Unregister()
			End Try
		End Sub
	End Class
End Namespace
