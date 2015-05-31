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
        Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing
        Dim circle2d As SolidEdgeFrameworkSupport.Circle2d = Nothing
        Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

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

            circles2d = profile.Circles2d

            circle2d = circles2d.AddByCenterRadius(0.04, 0.05, 0.02)

            profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

            ' Get a reference to the ActiveSelectSet.
            selectSet = application.ActiveSelectSet

            ' Empty ActiveSelectSet.
            selectSet.RemoveAll()

            ' Add new FaceRotate to ActiveSelectSet.
            selectSet.Add(circle2d)

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
