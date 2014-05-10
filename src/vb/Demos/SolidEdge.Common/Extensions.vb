Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace SolidEdge.Common
	''' <summary>
	''' SolidEdgeFramework.Application extension methods.
	''' </summary>
	Public Module ApplicationExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActiveAssemblyDocument(ByVal application As SolidEdgeFramework.Application) As SolidEdgeAssembly.AssemblyDocument
			Try
				' ActiveDocument will throw an exception if no document is open.
				' If cast fails method will return null;
				Return TryCast(application.ActiveDocument, SolidEdgeAssembly.AssemblyDocument)
			Catch
			End Try

			Return Nothing
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActiveDocument(ByVal application As SolidEdgeFramework.Application) As SolidEdgeFramework.SolidEdgeDocument
			Try
				' ActiveDocument will throw an exception if no document is open.
				Return DirectCast(application.ActiveDocument, SolidEdgeFramework.SolidEdgeDocument)
			Catch
			End Try

			Return Nothing
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActiveDraftDocument(ByVal application As SolidEdgeFramework.Application) As SolidEdgeDraft.DraftDocument
			Try
				' ActiveDocument will throw an exception if no document is open.
				' If cast fails method will return null;
				Return TryCast(application.ActiveDocument, SolidEdgeDraft.DraftDocument)
			Catch
			End Try

			Return Nothing
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActivePartDocument(ByVal application As SolidEdgeFramework.Application) As SolidEdgePart.PartDocument
			Try
				' ActiveDocument will throw an exception if no document is open.
				' If cast fails method will return null;
				Return TryCast(application.ActiveDocument, SolidEdgePart.PartDocument)
			Catch
			End Try

			Return Nothing
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActiveSheetMetalDocument(ByVal application As SolidEdgeFramework.Application) As SolidEdgePart.SheetMetalDocument
			Try
				' ActiveDocument will throw an exception if no document is open.
				' If cast fails method will return null;
				Return TryCast(application.ActiveDocument, SolidEdgePart.SheetMetalDocument)
			Catch
			End Try

			Return Nothing
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActiveDocumentType(ByVal application As SolidEdgeFramework.Application) As SolidEdgeFramework.DocumentTypeConstants
			Try
				' ActiveDocumentType will throw an exception if no document is open.
				Return application.ActiveDocumentType
			Catch
			End Try

			Return SolidEdgeFramework.DocumentTypeConstants.igUnknownDocument
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Sub StartCommand(ByVal application As SolidEdgeFramework.Application, ByVal CommandID As SolidEdgeConstants.AssemblyCommandConstants)
			application.StartCommand(CType(CommandID, SolidEdgeFramework.SolidEdgeCommandConstants))
		End Sub

		<System.Runtime.CompilerServices.Extension> _
		Public Sub StartCommand(ByVal application As SolidEdgeFramework.Application, ByVal CommandID As SolidEdgeConstants.DetailCommandConstants)
			application.StartCommand(CType(CommandID, SolidEdgeFramework.SolidEdgeCommandConstants))
		End Sub

		<System.Runtime.CompilerServices.Extension> _
		Public Sub StartCommand(ByVal application As SolidEdgeFramework.Application, ByVal CommandID As SolidEdgeConstants.PartCommandConstants)
			application.StartCommand(CType(CommandID, SolidEdgeFramework.SolidEdgeCommandConstants))
		End Sub

		<System.Runtime.CompilerServices.Extension> _
		Public Sub StartCommand(ByVal application As SolidEdgeFramework.Application, ByVal CommandID As SolidEdgeConstants.SheetMetalCommandConstants)
			application.StartCommand(CType(CommandID, SolidEdgeFramework.SolidEdgeCommandConstants))
		End Sub
	End Module

	''' <summary>
	''' SolidEdgeFramework.Documents extension methods.
	''' </summary>
	Public Module DocumentsExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Function AddAssemblyDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgeAssembly.AssemblyDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.AssemblyDocument), SolidEdgeAssembly.AssemblyDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddAssemblyDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgeAssembly.AssemblyDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.AssemblyDocument, TemplateDoc), SolidEdgeAssembly.AssemblyDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddDraftDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgeDraft.DraftDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.DraftDocument), SolidEdgeDraft.DraftDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddDraftDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgeDraft.DraftDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.DraftDocument, TemplateDoc), SolidEdgeDraft.DraftDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddPartDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgePart.PartDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.PartDocument), SolidEdgePart.PartDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddPartDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgePart.PartDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.PartDocument, TemplateDoc), SolidEdgePart.PartDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddSheetMetalDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgePart.SheetMetalDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.SheetMetalDocument), SolidEdgePart.SheetMetalDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddSheetMetalDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgePart.SheetMetalDocument
			Return DirectCast(documents.Add(SolidEdge.Common.ProgIDs.SheetMetalDocument, TemplateDoc), SolidEdgePart.SheetMetalDocument)
		End Function
	End Module

	Public Module MouseExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Sub AddToLocateFilter(ByVal mouse As SolidEdgeFramework.Mouse, ByVal filter As SolidEdgeConstants.seLocateFilterConstants)
			mouse.AddToLocateFilter(CInt(filter))
		End Sub

		<System.Runtime.CompilerServices.Extension> _
		Public Sub SetLocateMode(ByVal mouse As SolidEdgeFramework.Mouse, ByVal mode As SolidEdgeConstants.seLocateModes)
			mouse.LocateMode = CInt(mode)
		End Sub

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetLocateMode(ByVal mouse As SolidEdgeFramework.Mouse, ByVal mode As SolidEdgeConstants.seLocateModes) As SolidEdgeConstants.seLocateModes
			Return CType(mouse.LocateMode, SolidEdgeConstants.seLocateModes)
		End Function
	End Module

	''' <summary>
	''' SolidEdgePart.RefPlane extension methods.
	''' </summary>
	Public Module RefPlanesExtensions
		''' <summary>
		''' Gets the top plane at index 1.
		''' </summary>
		''' <param name="refPlanes"></param>
		''' <returns></returns>
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetTopPlane(ByVal refPlanes As SolidEdgePart.RefPlanes) As SolidEdgePart.RefPlane
			Return refPlanes.Item(1)
		End Function

		''' <summary>
		''' Gets the right plane at index 2.
		''' </summary>
		''' <param name="refPlanes"></param>
		''' <returns></returns>
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetRightPlane(ByVal refPlanes As SolidEdgePart.RefPlanes) As SolidEdgePart.RefPlane
			Return refPlanes.Item(2)
		End Function

		''' <summary>
		''' Gets the front plane at index 3.
		''' </summary>
		''' <param name="refPlanes"></param>
		''' <returns></returns>
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetFrontPlane(ByVal refPlanes As SolidEdgePart.RefPlanes) As SolidEdgePart.RefPlane
			Return refPlanes.Item(3)
		End Function
	End Module
End Namespace
