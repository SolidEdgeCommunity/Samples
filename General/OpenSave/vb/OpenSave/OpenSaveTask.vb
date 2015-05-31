Imports log4net
Imports SolidEdgeCommunity
Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Class OpenSaveTask
    Inherits IsolatedTaskProxy

    Private Shared _log As ILog = LogManager.GetLogger(GetType(OpenSaveTask))

    Public Sub DoOpenSave(ByVal filePath As String, ByVal openSaveSettings As OpenSaveSettings)
        InvokeSTAThread(Of String, OpenSaveSettings)(AddressOf DoOpenSaveInternal, filePath, openSaveSettings)
    End Sub

    Private Sub DoOpenSaveInternal(ByVal filePath As String, ByVal openSaveSettings As OpenSaveSettings)
        ' Register with OLE to handle concurrency issues on the current thread.
        SolidEdgeCommunity.OleMessageFilter.Register()

        Try
'INSTANT VB NOTE: The variable application was renamed since Visual Basic does not handle local variables named the same as class members well:
            Dim application_Renamed As SolidEdgeFramework.Application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True)
            Dim documents As SolidEdgeFramework.Documents = Nothing
'INSTANT VB NOTE: The variable document was renamed since Visual Basic does not handle local variables named the same as class members well:
            Dim document_Renamed As SolidEdgeFramework.SolidEdgeDocument = Nothing

            application_Renamed.DisplayAlerts = openSaveSettings.Application.DisplayAlerts
            application_Renamed.Visible = openSaveSettings.Application.Visible

            If openSaveSettings.Application.DisableAddins = True Then
                DisableAddins(application_Renamed)
            End If

            ' Disable (most) prompts.
            application_Renamed.DisplayAlerts = False

            ' Get a reference to the documents collection.
            documents = application_Renamed.Documents

            ' Close any documents that may be left open for whatever reason.
            documents.Close()

            ' Open the file.
            document_Renamed = documents.Open(Of SolidEdgeFramework.SolidEdgeDocument)(filePath)

            application_Renamed.DoIdle()

            If document_Renamed IsNot Nothing Then
                ' Environment specific routines.
                If TypeOf document_Renamed Is SolidEdgeAssembly.AssemblyDocument Then
                    DoOpenSave(DirectCast(document_Renamed, SolidEdgeAssembly.AssemblyDocument), openSaveSettings.Assembly)
                ElseIf TypeOf document_Renamed Is SolidEdgeDraft.DraftDocument Then
                    DoOpenSave(DirectCast(document_Renamed, SolidEdgeDraft.DraftDocument), openSaveSettings.Draft)
                ElseIf TypeOf document_Renamed Is SolidEdgePart.PartDocument Then
                    DoOpenSave(DirectCast(document_Renamed, SolidEdgePart.PartDocument), openSaveSettings.Part)
                ElseIf TypeOf document_Renamed Is SolidEdgePart.SheetMetalDocument Then
                    DoOpenSave(DirectCast(document_Renamed, SolidEdgePart.SheetMetalDocument), openSaveSettings.SheetMetal)
                ElseIf TypeOf document_Renamed Is SolidEdgePart.WeldmentDocument Then
                    DoOpenSave(DirectCast(document_Renamed, SolidEdgePart.WeldmentDocument), openSaveSettings.Weldment)
                End If

                ' Save document.
                document_Renamed.Save()

                ' Close document.
                document_Renamed.Close()

                application_Renamed.DoIdle()
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
