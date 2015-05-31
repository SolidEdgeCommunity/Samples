Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateSlot
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim partDocument As SolidEdgePart.PartDocument = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
            Dim refPlane As SolidEdgePart.RefPlane = Nothing
            Dim sketches As SolidEdgePart.Sketchs = Nothing
            Dim sketch As SolidEdgePart.Sketch = Nothing
            Dim profiles As SolidEdgePart.Profiles = Nothing
            Dim profile As SolidEdgePart.Profile = Nothing
            Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
            Dim line2d As SolidEdgeFrameworkSupport.Line2d = Nothing
            Dim slots As SolidEdgePart.Slots = Nothing
            Dim slot As SolidEdgePart.Slot = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Get a reference to the active document.
                partDocument = application.Documents.AddPartDocument()

                model = PartHelper.CreateSweptProtrusion(partDocument)

                ' Get a reference to the RefPlanes collection.
                refPlanes = partDocument.RefPlanes

                refPlane = refPlanes.GetRightPlane()

                ' Get a reference to the Sketches collection.
                sketches = partDocument.Sketches

                ' Create a new sketch.
                sketch = sketches.Add()

                ' Get a reference to the Profiles collection.
                profiles = sketch.Profiles

                ' Create a new profile.
                profile = profiles.Add(refPlane)

                ' Get a reference to the Lines2d collection.
                lines2d = profile.Lines2d

                ' Add a new line.
                line2d = lines2d.AddBy2Points(0, 0, 0.01, 0.01)

                ' Get a reference to the Slots collection.
                slots = model.Slots

                ' Add a new slot.
                slot = slots.Add(profile, SolidEdgePart.FeaturePropertyConstants.igRegularSlot, SolidEdgePart.FeaturePropertyConstants.igFormedEnd, 0.01, 0.0, 0.0, SolidEdgePart.FeaturePropertyConstants.igFinite, SolidEdgePart.FeaturePropertyConstants.igLeft, 0.5, SolidEdgePart.KeyPointExtentConstants.igTangentNormal, Nothing, SolidEdgePart.FeaturePropertyConstants.igNone, SolidEdgePart.FeaturePropertyConstants.igNone, 0.0, SolidEdgePart.KeyPointExtentConstants.igTangentNormal, Nothing, Nothing, SolidEdgePart.OffsetSideConstants.seOffsetNone, 0.0, Nothing, SolidEdgePart.OffsetSideConstants.seOffsetNone, 0.0)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new Slot to ActiveSelectSet.
                selectSet.Add(slot)
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            Finally
                SolidEdgeCommunity.OleMessageFilter.Unregister()
            End Try
        End Sub
    End Class
End Namespace
