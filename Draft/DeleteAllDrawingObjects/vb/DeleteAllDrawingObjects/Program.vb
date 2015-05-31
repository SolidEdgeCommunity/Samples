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
        Dim sheet As SolidEdgeDraft.Sheet = Nothing
        Dim drawingObjects As SolidEdgeFrameworkSupport.DrawingObjects = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active draft document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)()

            ' Get a reference to the active Sheet.
            sheet = draftDocument.ActiveSheet

            ' Get a reference to the drawing objects collection.
            drawingObjects = sheet.DrawingObjects

            ' Disable screen updating for performance.
            application.ScreenUpdating = False

            ' Loop until count is 0.
            Do While drawingObjects.Count > 0
                ' Leverage dynamic keyword to allow invoking Delete() method.
'INSTANT VB NOTE: In the following line, Instant VB substituted 'Object' for 'dynamic' - this will work in VB with Option Strict Off:
                Dim drawingObject As Object = drawingObjects.Item(1)

                If TypeOf drawingObject Is SolidEdgeDraft.TablePage Then
                    ' TablePage does not have a Delete() method but its parent does.
'INSTANT VB NOTE: In the following line, Instant VB substituted 'Object' for 'dynamic' - this will work in VB with Option Strict Off:
                    Dim drawingObjectParent As Object = drawingObject.Parent
                    drawingObjectParent.Delete()
                Else
                    drawingObject.Delete()
                End If
            Loop

            ' Turn screen updating back on.
            application.ScreenUpdating = True

            ' Fit the view.
            application.StartCommand(SolidEdgeConstants.DetailCommandConstants.DetailViewFit)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
