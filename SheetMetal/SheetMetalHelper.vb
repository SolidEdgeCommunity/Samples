Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class SheetMetalHelper
    Public Shared Function CreateBaseTab(ByVal sheetMetalDocument As SolidEdgePart.SheetMetalDocument) As SolidEdgePart.Model
        Dim profileSets As SolidEdgePart.ProfileSets = Nothing
        Dim profileSet As SolidEdgePart.ProfileSet = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim refplanes As SolidEdgePart.RefPlanes = Nothing
        Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing
        Dim relation2d As SolidEdgeFrameworkSupport.Relation2d = Nothing
        Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
        Dim line2d As SolidEdgeFrameworkSupport.Line2d = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing

        ' Get a reference to the profile sets collection.
        profileSets = sheetMetalDocument.ProfileSets

        ' Add a new profile set.
        profileSet = profileSets.Add()

        ' Get a reference to the profiles collection.
        profiles = profileSet.Profiles

        ' Get a reference to the ref planes collection.
        refplanes = sheetMetalDocument.RefPlanes

        ' Add a new profile.
        profile = profiles.Add(refplanes.Item(1))

        ' Get a reference to the lines2d collection.
        lines2d = profile.Lines2d

        ' UOM = meters.
        Dim lineMatrix(,) As Double = { _
            {0.05, 0.025, 0.05, 0.025}, _
            {-0.05, 0.025, -0.05, -0.025}, _
            {-0.05, -0.025, 0.05, -0.025}, _
            {0.05, -0.025, 0.05, 0.025} _
        }

        ' Draw the Base Profile.
        For i As Integer = 0 To lineMatrix.GetUpperBound(0)
            line2d = lines2d.AddBy2Points(lineMatrix(i, 0), lineMatrix(i, 1), lineMatrix(i, 2), lineMatrix(i, 3))
        Next i

        ' Define Relations among the Line objects to make the Profile closed.
        relations2d = DirectCast(profile.Relations2d, SolidEdgeFrameworkSupport.Relations2d)

        ' Connect all of the lines.
        For i As Integer = 1 To lines2d.Count
            Dim j As Integer = i + 1

            ' When we reach the last line, wrap around and connect it to the first line.
            If j > lines2d.Count Then
                j = 1
            End If

            relation2d = relations2d.AddKeypoint(lines2d.Item(i), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(j), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        Next i

        ' Close the profile.
        profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

        ' Hide the profile.
        profile.Visible = False

        ' Get a reference to the models collection.
        models = sheetMetalDocument.Models

        ' Create the base tab.
        model = models.AddBaseTab(profile, SolidEdgePart.FeaturePropertyConstants.igRight)

        Return model
    End Function

    Public Shared Function CreateBaseTabByCircle(ByVal sheetMetalDocument As SolidEdgePart.SheetMetalDocument) As SolidEdgePart.Model
        Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
        Dim refPlane As SolidEdgePart.RefPlane = Nothing
        Dim sketchs As SolidEdgePart.Sketchs = Nothing
        Dim sketch As SolidEdgePart.Sketch = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing
        Dim circle2d As SolidEdgeFrameworkSupport.Circle2d = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim x As Double = 0
        Dim y As Double = 0
        Dim radius As Double = 0.05

        ' Get refplane.
        refPlanes = sheetMetalDocument.RefPlanes

        ' Get a reference to front RefPlane.
        refPlane = refPlanes.GetFrontPlane()

        ' Create sketch.
        sketchs = sheetMetalDocument.Sketches
        sketch = sketchs.Add()

        ' Create profile.
        profiles = sketch.Profiles
        profile = profiles.Add(refPlane)

        ' Create 2D circle.
        circles2d = profile.Circles2d
        circle2d = circles2d.AddByCenterRadius(x, y, radius)

        ' Hide profile.
        profile.Visible = False

        ' Create extruded protrusion.
        models = sheetMetalDocument.Models
        model = models.AddBaseTab(profile, SolidEdgePart.FeaturePropertyConstants.igRight)

        Return model
    End Function

    Public Shared Sub CreateHolesWithUserDefinedPattern(ByVal sheetMetalDocument As SolidEdgePart.SheetMetalDocument)
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

        ' Call helper method to create the actual geometry.
        model = CreateBaseTabByCircle(sheetMetalDocument)

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
    End Sub
End Class