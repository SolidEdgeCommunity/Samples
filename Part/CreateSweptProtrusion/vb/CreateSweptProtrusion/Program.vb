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
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim sketches As SolidEdgePart.Sketchs = Nothing
        Dim sketch As SolidEdgePart.Sketch = Nothing
        Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
        Dim refPlane As SolidEdgePart.RefPlane = Nothing
        Dim profileSets As SolidEdgePart.ProfileSets = Nothing
        Dim profileSet As SolidEdgePart.ProfileSet = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim sketchProfile As SolidEdgePart.Profile = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing

        Dim listPaths As New List(Of SolidEdgePart.Profile)()
        Dim listPathTypes As New List(Of SolidEdgePart.FeaturePropertyConstants)()
        Dim listSections As New List(Of SolidEdgePart.Profile)()
        Dim listSectionTypes As New List(Of SolidEdgePart.FeaturePropertyConstants)()
        Dim listOrigins As New List(Of Integer)()

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the Documents collection.
            documents = application.Documents

            ' Create a new PartDocument.
            partDocument = documents.AddPartDocument()

            ' Always a good idea to give SE a chance to breathe.
            application.DoIdle()

            ' Get a reference to the models collection.
            models = DirectCast(partDocument.Models, SolidEdgePart.Models)

            ' Get a reference to the Sketches collections.
            sketches = DirectCast(partDocument.Sketches, SolidEdgePart.Sketchs)

            ' Get a reference to the profile sets collection.
            profileSets = DirectCast(partDocument.ProfileSets, SolidEdgePart.ProfileSets)

            ' Get a reference to the ref planes collection.
            refPlanes = DirectCast(partDocument.RefPlanes, SolidEdgePart.RefPlanes)

            ' Get a reference to front RefPlane.
            refPlane = refPlanes.GetFrontPlane()

            ' Add a new sketch.
            sketch = DirectCast(sketches.Add(), SolidEdgePart.Sketch)

            ' Add profile for sketch on specified refplane.
            sketchProfile = sketch.Profiles.Add(refPlane)

            ' Get a reference to the Circles2d collection.
            circles2d = sketchProfile.Circles2d

            ' Draw the Base Profile.
            circles2d.AddByCenterRadius(0, 0, 0.02)

            ' Close the profile.
            sketchProfile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

            ' Arrays for AddSweptProtrusion().
            listPaths.Add(sketchProfile)
            listPathTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)

            ' NOTE: profile is the Curve.
            refPlane = refPlanes.AddNormalToCurve(sketchProfile, SolidEdgePart.ReferenceElementConstants.igCurveEnd, refPlanes.GetFrontPlane(), SolidEdgePart.ReferenceElementConstants.igPivotEnd, True, System.Reflection.Missing.Value)

            ' Add a new profile set.
            profileSet = DirectCast(profileSets.Add(), SolidEdgePart.ProfileSet)

            ' Get a reference to the profiles collection.
            profiles = DirectCast(profileSet.Profiles, SolidEdgePart.Profiles)

            ' add a new profile.
            profile = DirectCast(profiles.Add(refPlane), SolidEdgePart.Profile)

            ' Get a reference to the Circles2d collection.
            circles2d = profile.Circles2d

            ' Draw the Base Profile.
            circles2d.AddByCenterRadius(0, 0, 0.01)

            ' Close the profile.
            profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

            ' Arrays for AddSweptProtrusion().
            listSections.Add(profile)
            listSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)
            listOrigins.Add(0) 'Use 0 for closed profiles.

            ' Create the extended protrusion.
            model = models.AddSweptProtrusion(listPaths.Count, listPaths.ToArray(), listPathTypes.ToArray(), listSections.Count, listSections.ToArray(), listSectionTypes.ToArray(), listOrigins.ToArray(), 0, SolidEdgePart.FeaturePropertyConstants.igLeft, SolidEdgePart.FeaturePropertyConstants.igNone, 0.0, Nothing, SolidEdgePart.FeaturePropertyConstants.igNone, 0.0, Nothing)

            ' Hide profiles.
            sketchProfile.Visible = False
            profile.Visible = False

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
