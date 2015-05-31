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
        Dim section As SolidEdgeDraft.Section = Nothing
        Dim sectionSheets As SolidEdgeDraft.SectionSheets = Nothing
        Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing

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

                ' Get a reference to the working section.
                section = sections.WorkingSection

                ' Get a reference to the working section sheets.
                sectionSheets = section.Sheets

                For Each sheet In sectionSheets.OfType(Of SolidEdgeDraft.Sheet)()
                    ' Get a reference to the DrawingViews collection.
                    drawingViews = sheet.DrawingViews

                    For Each drawingView In drawingViews.OfType(Of SolidEdgeDraft.DrawingView)()
                        ' DrawingView's of type igUserView cannot be converted.
                        If drawingView.DrawingViewType <> SolidEdgeDraft.DrawingViewTypeConstants.igUserView Then
                            ' Converts the current DrawingView to an igUserView type containing simple geometry
                            ' and disassociates the drawing view from the source 3d Model.
                            drawingView.Drop()
                        End If
                    Next drawingView
                Next sheet
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
