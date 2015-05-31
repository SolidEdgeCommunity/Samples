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
        Dim partDocument As SolidEdgePart.PartDocument = Nothing
        Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
        Dim refPlane As SolidEdgePart.RefPlane = Nothing
        Dim sketches As SolidEdgePart.Sketchs = Nothing
        Dim sketch As SolidEdgePart.Sketch = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim bsplineCurves2d As SolidEdgeFrameworkSupport.BSplineCurves2d = Nothing
        Dim bsplineCurve2d1 As SolidEdgeFrameworkSupport.BSplineCurve2d = Nothing
        Dim bsplineCurve2d2 As SolidEdgeFrameworkSupport.BSplineCurve2d = Nothing
        Dim startX As Double = 0
        Dim startY As Double = 0
        Dim endX As Double = 0
        Dim endY As Double = 0
        Dim arcs2d As SolidEdgeFrameworkSupport.Arcs2d = Nothing
        Dim arc2d As SolidEdgeFrameworkSupport.Arc2d = Nothing
        Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new part document.
            partDocument = documents.AddPartDocument()

            ' Always a good idea to give SE a chance to breathe.
            application.DoIdle()

            ' Get a reference to the RefPlanes collection.
            refPlanes = partDocument.RefPlanes

            ' Get a reference to front RefPlane.
            refPlane = refPlanes.GetFrontPlane()

            ' Get a reference to the Sketches collection.
            sketches = partDocument.Sketches

            ' Create a new sketch.
            sketch = sketches.Add()

            ' Get a reference to the Profiles collection.
            profiles = sketch.Profiles

            ' Create a new profile.
            profile = profiles.Add(refPlane)

            ' Get a reference to the BSplineCurves2d collection.
            bsplineCurves2d = profile.BSplineCurves2d

            Dim points As New List(Of Double)()
            points.Add(10.0 \ 1000)
            points.Add(0.0 \ 1000)
            points.Add(9.0 \ 1000)
            points.Add(6.0 \ 1000)
            points.Add(3.0 \ 1000)
            points.Add(12.0 \ 1000)

            ' Create initial b-spline.
            bsplineCurve2d1 = bsplineCurves2d.AddByPoints(Order:= 6, ArraySize:= 3, Array:= points.ToArray())

            ' Mirror initial b-spline.
            bsplineCurve2d2 = DirectCast(bsplineCurve2d1.Mirror(x1:= 0.0, y1:= 1.0, x2:= 0.0, y2:= -1.0, BooleanCopyFlag:= True), SolidEdgeFrameworkSupport.BSplineCurve2d)

            bsplineCurve2d1.GetNode(Index:= bsplineCurve2d1.NodeCount, x:= startX, y:= startY)

            bsplineCurve2d2.GetNode(Index:= bsplineCurve2d2.NodeCount, x:= endX, y:= endY)

            ' Get a reference to the Arcs2d collection.
            arcs2d = profile.Arcs2d

            ' Draw arc to connect the two b-splines.
            arc2d = arcs2d.AddByCenterStartEnd(xCenter:= 0.0, yCenter:= 0.0, xStart:= startX, yStart:= startY, xEnd:= endX, yEnd:= endY)

            Dim endKeyPointIndex1 As Integer = GetBSplineCurves2dEndKeyPointIndex(bsplineCurve2d1)
            Dim endKeyPointIndex2 As Integer = GetBSplineCurves2dEndKeyPointIndex(bsplineCurve2d2)

            ' Get a reference to the Relations2d collection.
            relations2d = DirectCast(profile.Relations2d, SolidEdgeFrameworkSupport.Relations2d)

            ' Connect BSplineCurve2d and arc.
            relations2d.AddKeypoint(Object1:= bsplineCurve2d1, Index1:= endKeyPointIndex1, Object2:= arc2d, Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igArcStart))

            ' Connect BSplineCurve2d and arc.
            relations2d.AddKeypoint(Object1:= bsplineCurve2d2, Index1:= endKeyPointIndex2, Object2:= arc2d, Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igArcEnd))

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub

    Private Shared Function GetBSplineCurves2dEndKeyPointIndex(ByVal bsplineCurve2d As SolidEdgeFrameworkSupport.BSplineCurve2d) As Integer
        ' Keypoint indices are zero-based......
        For i As Integer = 0 To bsplineCurve2d.KeyPointCount - 2
            Dim x As Double = 0
            Dim y As Double = 0
            Dim z As Double = 0
            Dim keypointType As SolidEdgeFramework.KeyPointType = Nothing
            Dim handleType As Integer = 0

            bsplineCurve2d.GetKeyPoint(Index:= i, x:= x, y:= y, z:= z, KeypointType:= keypointType, HandleType:= handleType)

            If keypointType = SolidEdgeFramework.KeyPointType.igKeyPointEnd Then
                Return i
            End If
        Next i

        Return 0
    End Function
End Class
