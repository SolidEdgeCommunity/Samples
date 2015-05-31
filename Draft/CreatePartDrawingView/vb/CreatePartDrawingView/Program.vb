Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim documents As SolidEdgeFramework.Documents = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim modelLinks As SolidEdgeDraft.ModelLinks = Nothing
        Dim modelLink As SolidEdgeDraft.ModelLink = Nothing
        Dim sheet As SolidEdgeDraft.Sheet = Nothing
        Dim drawingViews As SolidEdgeDraft.DrawingViews = Nothing
        Dim drawingView As SolidEdgeDraft.DrawingView = Nothing
        Dim dvLines2d As SolidEdgeDraft.DVLines2d = Nothing
        Dim dvLine2d As SolidEdgeDraft.DVLine2d = Nothing
        Dim dimensions As SolidEdgeFrameworkSupport.Dimensions = Nothing
        Dim dimension As SolidEdgeFrameworkSupport.Dimension = Nothing
        Dim dimStyle As SolidEdgeFrameworkSupport.DimStyle = Nothing
        Dim filename As String = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new draft document.
            draftDocument = documents.AddDraftDocument()

            ' Get a reference to the ModelLinks collection.
            modelLinks = draftDocument.ModelLinks

            ' Build path to part file.
            filename = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath(), "2holebar.par")

            ' Add a new model link.
            modelLink = modelLinks.Add(filename)

            ' Get a reference to the active sheet.
            sheet = draftDocument.ActiveSheet

            ' Get a reference to the DrawingViews collection.
            drawingViews = sheet.DrawingViews

            ' Add a new part drawing view.
            drawingView = drawingViews.AddPartView([From]:= modelLink, Orientation:= SolidEdgeDraft.ViewOrientationConstants.igDimetricTopBackLeftView, Scale:= 5.0, x:= 0.4, y:= 0.4, ViewType:= SolidEdgeDraft.PartDrawingViewTypeConstants.sePartDesignedView)

            ' Get a reference to the DVLines2d collection.
            dvLines2d = drawingView.DVLines2d

            ' Get the 1st drawing view 2D line.
            dvLine2d = dvLines2d.Item(1)

            ' Get a reference to the Dimensions collection.
            dimensions = DirectCast(sheet.Dimensions, SolidEdgeFrameworkSupport.Dimensions)

            ' Add a dimension to the line.
            dimension = dimensions.AddLength(dvLine2d.Reference)

            ' Few changes to make the dimensions look right.
            dimension.ProjectionLineDirection = True
            dimension.TrackDistance = 0.02

            ' Get a reference to the dimension style.
            ' DimStyle has a ton of options...
            dimStyle = dimension.Style
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
