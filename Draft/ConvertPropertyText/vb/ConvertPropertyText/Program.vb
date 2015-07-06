Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim sections As SolidEdgeDraft.Sections = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active draft document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            If draftDocument IsNot Nothing Then
                ' Get a reference to the Sections collection.
                sections = draftDocument.Sections

                ' Convert property text on all sheets in background section.
                ConvertSection(sections.BackgroundSection)

                ' Convert property text on all sheets in working section.
                ConvertSection(sections.WorkingSection)
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub

    Private Shared Sub ConvertSection(ByVal section As SolidEdgeDraft.Section)
        Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
        Dim balloons As SolidEdgeFrameworkSupport.Balloons = Nothing

        ' Get a reference to the section sheets collection.
        sectionSheets = section.Sheets

        ' Process all of the sheets in the section.
        For Each sheet In sectionSheets.OfType(Of SolidEdgeDraft.Sheet)()
            ' Get a reference to the section balloons collection.
            balloons = DirectCast(sheet.Balloons, SolidEdgeFrameworkSupport.Balloons)

            ' Process all of the balloons in the sheet.
            For Each balloon In balloons.OfType(Of SolidEdgeFrameworkSupport.Balloon)()
                ' If BalloonText has a formula, it will be overriden with BalloonDisplayedText.
                balloon.BalloonText = balloon.BalloonDisplayedText
            Next balloon

            ' Note: There may be other controls that need to be updated...
        Next sheet
    End Sub
End Class
