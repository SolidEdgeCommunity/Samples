Imports log4net
Imports log4net.Config
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text

Public Class InteropProxy
	Inherits MarshalByRefObject

	Private Shared _log As ILog = LogManager.GetLogger(GetType(InteropProxy))
	Private _openSaveSettings As OpenSaveSettings

	Public Sub DoOpenSave(ByVal filePath As String, ByVal openSaveSettings As OpenSaveSettings)
		_openSaveSettings = openSaveSettings

		' Register with OLE to handle concurrency issues on the current thread.
		SolidEdgeCommunity.OleMessageFilter.Register()

		Try
			Dim application As SolidEdgeFramework.Application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True)
			Dim documents As SolidEdgeFramework.Documents = Nothing
			Dim document As SolidEdgeFramework.SolidEdgeDocument = Nothing

			application.Visible = _openSaveSettings.Application.Visible

			If _openSaveSettings.Application.DisableAddins = True Then
				DisableAddins(application)
			End If

			' Disable (most) prompts.
			application.DisplayAlerts = False

			' Get a reference to the documents collection.
			documents = application.Documents

			' Close any documents that may be left open for whatever reason.
			documents.Close()

			' Open the file.
			document = DirectCast(documents.Open(filePath), SolidEdgeFramework.SolidEdgeDocument)

			application.DoIdle()

			If document IsNot Nothing Then
				' Environment specific routines.
				If TypeOf document Is SolidEdgeAssembly.AssemblyDocument Then
					DoOpenSave(DirectCast(document, SolidEdgeAssembly.AssemblyDocument), _openSaveSettings.Assembly)
				ElseIf TypeOf document Is SolidEdgeDraft.DraftDocument Then
					DoOpenSave(DirectCast(document, SolidEdgeDraft.DraftDocument), _openSaveSettings.Draft)
				ElseIf TypeOf document Is SolidEdgePart.PartDocument Then
					DoOpenSave(DirectCast(document, SolidEdgePart.PartDocument), _openSaveSettings.Part)
				ElseIf TypeOf document Is SolidEdgePart.SheetMetalDocument Then
					DoOpenSave(DirectCast(document, SolidEdgePart.SheetMetalDocument), _openSaveSettings.SheetMetal)
				ElseIf TypeOf document Is SolidEdgePart.WeldmentDocument Then
					DoOpenSave(DirectCast(document, SolidEdgePart.WeldmentDocument), _openSaveSettings.Weldment)
				End If

				' Save document.
				document.Save()

				' Close document.
				document.Close()

				application.DoIdle()
			End If
		Finally
			SolidEdgeCommunity.OleMessageFilter.Unregister()
		End Try
	End Sub

	Private Sub DoOpenSave(ByVal assemblyDocument As SolidEdgeAssembly.AssemblyDocument, ByVal assemblySettings As AssemblySettings)
	End Sub

	Private Sub DoOpenSave(ByVal draftDocument As SolidEdgeDraft.DraftDocument, ByVal draftSettings As DraftSettings)
		Dim sections As SolidEdgeDraft.Sections = Nothing
		Dim section As SolidEdgeDraft.Section = Nothing
		Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
		Dim sheet As SolidEdgeDraft.Sheet = Nothing
		Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing
		Dim drawingView As SolidEdgeDraft.DrawingView = Nothing

		If draftSettings.UpdateDrawingViews Then
			sections = draftDocument.Sections
			section = sections.WorkingSection

			sectionSheets = section.Sheets

			For i As Integer = 1 To sectionSheets.Count
				sheet = sectionSheets.Item(i)
				drawingViews = sheet.DrawingViews

				For j As Integer = 1 To drawingViews.Count
					drawingView = drawingViews.Item(j)
					drawingView.Update()
				Next j
			Next i
		End If
	End Sub

	Private Sub DoOpenSave(ByVal partDocument As SolidEdgePart.PartDocument, ByVal partSettings As PartSettings)
	End Sub

	Private Sub DoOpenSave(ByVal sheetMetalDocument As SolidEdgePart.SheetMetalDocument, ByVal sheetMetalSettings As SheetMetalSettings)
	End Sub

	Private Sub DoOpenSave(ByVal weldmentDocument As SolidEdgePart.WeldmentDocument, ByVal weldmentSettings As WeldmentSettings)
	End Sub

	Private Sub DisableAddins(ByVal application As SolidEdgeFramework.Application)
		Dim addins As SolidEdgeFramework.AddIns = Nothing
		Dim addin As SolidEdgeFramework.AddIn = Nothing

		addins = application.AddIns

		For i As Integer = 1 To addins.Count
			addin = addins.Item(i)
			addin.Connect = False
		Next i
	End Sub
End Class
