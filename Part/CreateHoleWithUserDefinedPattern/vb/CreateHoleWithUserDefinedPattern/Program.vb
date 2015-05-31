Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateHoleWithUserDefinedPattern
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim partDocument As SolidEdgePart.PartDocument = Nothing
            Dim refplanes As SolidEdgePart.RefPlanes = Nothing
            Dim refplane As SolidEdgePart.RefPlane = Nothing
            Dim models As SolidEdgePart.Models = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim holeDataCollection As SolidEdgePart.HoleDataCollection = Nothing
            Dim profileSets As SolidEdgePart.ProfileSets = Nothing
            Dim profileSet As SolidEdgePart.ProfileSet = Nothing
            Dim profiles As SolidEdgePart.Profiles = Nothing
            Dim profile As SolidEdgePart.Profile = Nothing
            Dim holes2d As SolidEdgePart.Holes2d = Nothing
            Dim hole2d As SolidEdgePart.Hole2d = Nothing
            Dim holes As SolidEdgePart.Holes = Nothing
            Dim hole As SolidEdgePart.Hole = Nothing
            Dim profileStatus As Long = 0
            Dim profileList As New List(Of SolidEdgePart.Profile)()
            Dim userDefinedPatterns As SolidEdgePart.UserDefinedPatterns = Nothing
            Dim userDefinedPattern As SolidEdgePart.UserDefinedPattern = Nothing
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
                refplanes = partDocument.RefPlanes

                ' Get a reference to front RefPlane.
                refplane = refplanes.GetFrontPlane()

                Dim linesArray As New List(Of Double())()
                linesArray.Add(New Double() { 0, 0, 0.08, 0 })
                linesArray.Add(New Double() { 0.08, 0, 0.08, 0.06 })
                linesArray.Add(New Double() { 0.08, 0.06, 0.064, 0.06 })
                linesArray.Add(New Double() { 0.064, 0.06, 0.064, 0.02 })
                linesArray.Add(New Double() { 0.064, 0.02, 0.048, 0.02 })
                linesArray.Add(New Double() { 0.048, 0.02, 0.048, 0.06 })
                linesArray.Add(New Double() { 0.048, 0.06, 0.032, 0.06 })
                linesArray.Add(New Double() { 0.032, 0.06, 0.032, 0.02 })
                linesArray.Add(New Double() { 0.032, 0.02, 0.016, 0.02 })
                linesArray.Add(New Double() { 0.016, 0.02, 0.016, 0.06 })
                linesArray.Add(New Double() { 0.016, 0.06, 0, 0.06 })
                linesArray.Add(New Double() { 0, 0.06, 0, 0 })

                ' Call helper method to create the actual geometry.
                PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refplane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005)

                ' Get a reference to the Models collection.
                models = partDocument.Models

                ' Get a reference to Model.
                model = models.Item(1)

                ' Get a reference to the ProfileSets collection.
                profileSets = partDocument.ProfileSets

                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                ' Add new Profile.
                profile = profiles.Add(refplane)

                ' Get a reference to the Holes2d collection.
                holes2d = profile.Holes2d

                Dim holeMatrix(,) As Double = { _
                    {0.01, 0.01}, _
                    {0.02, 0.01}, _
                    {0.03, 0.01}, _
                    {0.04, 0.01}, _
                    {0.05, 0.01}, _
                    {0.06, 0.01}, _
                    {0.07, 0.01} _
                }

                ' Draw the Base Profile.
                For i As Integer = 0 To holeMatrix.GetUpperBound(0)
                    ' Add new Hole2d.
                    hole2d = holes2d.Add(XCenter:= holeMatrix(i, 0), YCenter:= holeMatrix(i, 1))
                Next i

                ' Hide the profile.
                profile.Visible = False

                ' Close profile.
                profileStatus = profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

                ' Get a reference to the ProfileSet.
                profileSet = DirectCast(profile.Parent, SolidEdgePart.ProfileSet)

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                ' Add profiles to list for AddByProfiles().
                For i As Integer = 1 To profiles.Count
                    profileList.Add(profiles.Item(i))
                Next i

                ' Get a reference to the HoleDataCollection collection.
                holeDataCollection = partDocument.HoleDataCollection

                ' Add new HoleData.
                Dim holeData As SolidEdgePart.HoleData = holeDataCollection.Add(HoleType:= SolidEdgePart.FeaturePropertyConstants.igRegularHole, HoleDiameter:= 0.005, BottomAngle:= 90)

                ' Get a reference to the Holes collection.
                holes = model.Holes

                ' Add hole.
                hole = holes.AddFinite(Profile:= profile, ProfilePlaneSide:= SolidEdgePart.FeaturePropertyConstants.igRight, FiniteDepth:= 0.005, Data:= holeData)

                ' Get a reference to the UserDefinedPatterns collection.
                userDefinedPatterns = model.UserDefinedPatterns

                ' Create the user defined pattern.
                userDefinedPattern = userDefinedPatterns.AddByProfiles(NumberOfProfiles:= profileList.Count, ProfilesArray:= profileList.ToArray(), SeedFeature:= hole)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new UserDefinedPattern to ActiveSelectSet.
                selectSet.Add(userDefinedPattern)

                ' Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            Finally
                SolidEdgeCommunity.OleMessageFilter.Unregister()
            End Try
        End Sub
    End Class
End Namespace
