Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateHoles
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
            Dim refplanes As SolidEdgePart.RefPlanes = Nothing
            Dim refplane As SolidEdgePart.RefPlane = Nothing
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

                ' Get a reference to the Documents collection.
                documents = application.Documents

                ' Create a new sheetmetal document.
                sheetMetalDocument = documents.AddSheetMetalDocument()

                ' Always a good idea to give SE a chance to breathe.
                application.DoIdle()

                ' Call helper method to create the actual geometry.
                model = SheetMetalHelper.CreateBaseTabByCircle(sheetMetalDocument)

                ' Get a reference to the RefPlanes collection.
                refplanes = sheetMetalDocument.RefPlanes

                ' Get a reference to front RefPlane.
                refplane = refplanes.GetFrontPlane()

                ' Get a reference to the ProfileSets collection.
                profileSets = sheetMetalDocument.ProfileSets

                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                ' Add new Profile.
                profile = profiles.Add(refplane)

                ' Get a reference to the Holes2d collection.
                holes2d = profile.Holes2d

                ' This creates a cross pattern of holes.
                Dim holeMatrix(,) As Double = { _
                    {0.00, 0.00}, _
                    {-0.01, 0.00}, _
                    {-0.02, 0.00}, _
                    {-0.03, 0.00}, _
                    {-0.04, 0.00}, _
                    {0.01, 0.00}, _
                    {0.02, 0.00}, _
                    {0.03, 0.00}, _
                    {0.04, 0.00}, _
                    {0.00, -0.01}, _
                    {0.00, -0.02}, _
                    {0.00, -0.03}, _
                    {0.00, -0.04}, _
                    {0.00, 0.01}, _
                    {0.00, 0.02}, _
                    {0.00, 0.03}, _
                    {0.00, 0.04} _
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
                holeDataCollection = sheetMetalDocument.HoleDataCollection

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
                application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView)
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            Finally
                SolidEdgeCommunity.OleMessageFilter.Unregister()
            End Try
        End Sub
    End Class
End Namespace
