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
        Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
        Dim sketches As SolidEdgePart.Sketchs = Nothing
        Dim sketch As SolidEdgePart.Sketch = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile1 As SolidEdgePart.Profile = Nothing
        Dim profile2 As SolidEdgePart.Profile = Nothing
        Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
        Dim refPlane As SolidEdgePart.RefPlane = Nothing
        Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim crossSections As New List(Of Object)()
        Dim crossSectionTypes As New List(Of Object)()
        Dim origins As New List(Of Object)()
        Dim originRefs As New List(Of Object)()
        Dim bendRadius As Double = 0.001
        Dim neutralFactor As Double = 0.33
        Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
        Dim loftedFlanges As SolidEdgePart.LoftedFlanges = Nothing
        Dim loftedFlange As SolidEdgePart.LoftedFlange = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to Solid Edge,
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new sheetmetal document.
            sheetMetalDocument = documents.AddSheetMetalDocument()

            ' Get a reference to the Sketches collection.
            sketches = sheetMetalDocument.Sketches

            ' Get a reference to the RefPlanes collection.
            refPlanes = sheetMetalDocument.RefPlanes

            ' Get a reference to top RefPlane.
            refPlane = refPlanes.GetTopPlane()

            ' Create new sketch.
            sketch = sketches.Add()

            ' Get a reference to the Profiles collection.
            profiles = sketch.Profiles

            ' Create new profile.
            profile1 = profiles.Add(refPlanes.Item(1))

            ' Get a reference to the Lines2d collection.
            lines2d = profile1.Lines2d

            ' Multidimensional array
            Dim points(,) As Double = { _
                { 0.0, 0.0, 0.1, 0.2 }, _
                { 0.1, 0.2, 0.3, 0.2 }, _
                { 0.3, 0.2, 0.4, 0.0 } _
            }

            For i As Integer = 0 To points.GetUpperBound(0)
                lines2d.AddBy2Points(points(i, 0), points(i, 1), points(i, 2), points(i, 3))
            Next i

            origins.Add(lines2d.Item(1))

            ' Create Reference Plane Parallel to "XY" Plane at 1Meter distance.
            refPlane = refPlanes.AddParallelByDistance(ParentPlane:= refPlanes.Item(1), Distance:= 1, NormalSide:= SolidEdgePart.ReferenceElementConstants.igReverseNormalSide)

            ' Create new sketch.
            sketch = sketches.Add()

            ' Get a reference to the Profiles collection.
            profiles = sketch.Profiles

            ' Create new profile.
            profile2 = profiles.Add(refPlane)

            ' Get a reference to the Lines2d collection.
            lines2d = profile2.Lines2d

            For i As Integer = 0 To points.GetUpperBound(0)
                lines2d.AddBy2Points(points(i, 0), points(i, 1), points(i, 2), points(i, 3))
            Next i

            origins.Add(lines2d.Item(1))

            crossSections.Add(profile1)
            crossSections.Add(profile2)

            crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)
            crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)

            originRefs.Add(SolidEdgePart.FeaturePropertyConstants.igStart)
            originRefs.Add(SolidEdgePart.FeaturePropertyConstants.igStart)

            ' Get a reference to the Models collection.
            models = sheetMetalDocument.Models

            ' Create a new lofted flange.
            model = models.AddLoftedFlange(crossSections.Count, crossSections.ToArray(), crossSectionTypes.ToArray(), origins.ToArray(), originRefs.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, bendRadius, neutralFactor, SolidEdgePart.FeaturePropertyConstants.igNFType)

            ' Get a reference to the LoftedFlanges collection.
            loftedFlanges = model.LoftedFlanges

            ' Get a reference to the new LoftedFlange.
            loftedFlange = loftedFlanges.Item(1)

            ' Get a reference to the ActiveSelectSet.
            selectSet = application.ActiveSelectSet

            ' Empty ActiveSelectSet.
            selectSet.RemoveAll()

            ' Add new protrusion to ActiveSelectSet.
            selectSet.Add(loftedFlange)

            profile1.Visible = False
            profile2.Visible = False

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
