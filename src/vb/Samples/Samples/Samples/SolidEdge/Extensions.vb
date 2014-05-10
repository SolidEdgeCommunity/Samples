Imports System
Imports System.Collections.Generic
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

Namespace ApiSamples.Samples.SolidEdge
	''' <summary>
	''' SolidEdgeFramework.Application extension methods.
	''' </summary>
	Friend Module ApplicationExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Function TryActiveDocumentAs(Of T)(ByVal application As SolidEdgeFramework.Application) As T
			Try
				' ActiveDocument will throw an exception if no document is open.
				If TypeOf application.ActiveDocument Is T Then
					Return DirectCast(application.ActiveDocument, T)
				End If
			Catch
			End Try

			Return Nothing
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function GetActiveDocumentType(ByVal application As SolidEdgeFramework.Application) As SolidEdgeFramework.DocumentTypeConstants
			' ActiveDocumentType will throw an exception if no document is open.
			Return application.ActiveDocumentType
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
	Friend Module DocumentsExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Function AddAssemblyDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgeAssembly.AssemblyDocument
			Return DirectCast(documents.Add(ProgId.AssemblyDocument), SolidEdgeAssembly.AssemblyDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddAssemblyDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgeAssembly.AssemblyDocument
			Return DirectCast(documents.Add(ProgId.AssemblyDocument, TemplateDoc), SolidEdgeAssembly.AssemblyDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddDraftDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgeDraft.DraftDocument
			Return DirectCast(documents.Add(ProgId.DraftDocument), SolidEdgeDraft.DraftDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddDraftDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgeDraft.DraftDocument
			Return DirectCast(documents.Add(ProgId.DraftDocument, TemplateDoc), SolidEdgeDraft.DraftDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddPartDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgePart.PartDocument
			Return DirectCast(documents.Add(ProgId.PartDocument), SolidEdgePart.PartDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddPartDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgePart.PartDocument
			Return DirectCast(documents.Add(ProgId.PartDocument, TemplateDoc), SolidEdgePart.PartDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddSheetMetalDocument(ByVal documents As SolidEdgeFramework.Documents) As SolidEdgePart.SheetMetalDocument
			Return DirectCast(documents.Add(ProgId.SheetMetalDocument), SolidEdgePart.SheetMetalDocument)
		End Function

		<System.Runtime.CompilerServices.Extension> _
		Public Function AddSheetMetalDocument(ByVal documents As SolidEdgeFramework.Documents, ByVal TemplateDoc As Object) As SolidEdgePart.SheetMetalDocument
			Return DirectCast(documents.Add(ProgId.SheetMetalDocument, TemplateDoc), SolidEdgePart.SheetMetalDocument)
		End Function
	End Module

	''' <summary>
	''' SolidEdgePart.RefPlane extension methods.
	''' </summary>
	Friend Module RefPlanesExtensions
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

	Friend Module SheetExtensions
		<DllImport("user32.dll")> _
		Function CloseClipboard() As Boolean
		End Function

		<DllImport("user32.dll", CharSet := CharSet.Unicode, ExactSpelling := True, CallingConvention := CallingConvention.StdCall)> _
		Function GetClipboardData(ByVal format As UInteger) As IntPtr
		End Function

		<DllImport("user32.dll")> _
		Function GetClipboardOwner() As IntPtr
		End Function

		<DllImport("user32.dll")> _
		Function IsClipboardFormatAvailable(ByVal format As UInteger) As Boolean
		End Function

		<DllImport("user32.dll")> _
		Function OpenClipboard(ByVal hWndNewOwner As IntPtr) As Boolean
		End Function

		<DllImport("gdi32.dll")> _
		Function DeleteEnhMetaFile(ByVal hemf As IntPtr) As Boolean
		End Function

		<DllImport("gdi32.dll")> _
		Function GetEnhMetaFileBits(ByVal hemf As IntPtr, ByVal cbBuffer As UInteger, <Out()> ByVal lpbBuffer() As Byte) As UInteger
		End Function

		Private Const CF_ENHMETAFILE As UInteger = 14

		<System.Runtime.CompilerServices.Extension> _
		Public Sub SaveAsEMF(ByVal sheet As SolidEdgeDraft.Sheet, ByVal filename As String)
			Try
				' Copy the sheet as an EMF to the windows clipboard.
				sheet.CopyEMFToClipboard()

				If OpenClipboard(IntPtr.Zero) Then
					If IsClipboardFormatAvailable(CF_ENHMETAFILE) Then
						' Get the handle to the EMF.
						Dim hEMF As IntPtr = GetClipboardData(CF_ENHMETAFILE)

						' Query the size of the EMF.
						Dim len As UInteger = GetEnhMetaFileBits(hEMF, 0, Nothing)
						Dim rawBytes(len - 1) As Byte

						' Get all of the bytes of the EMF.
						GetEnhMetaFileBits(hEMF, len, rawBytes)

						' Write all of the bytes to a file.
						File.WriteAllBytes(filename, rawBytes)

						' Delete the EMF handle.
						DeleteEnhMetaFile(hEMF)
					Else
						Throw New System.Exception("CF_ENHMETAFILE is not available in clipboard.")
					End If
				Else
					Throw New System.Exception("Error opening clipboard.")
				End If
			Catch e As System.Exception
				Throw e
			Finally
				CloseClipboard()
			End Try
		End Sub
	End Module
End Namespace
