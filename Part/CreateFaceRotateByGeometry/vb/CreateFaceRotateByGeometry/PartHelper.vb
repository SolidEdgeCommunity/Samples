Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class PartHelper
    Public Shared Function CreateFiniteExtrudedProtrusion(ByVal partDocument As SolidEdgePart.PartDocument, ByVal refPlane As SolidEdgePart.RefPlane, ByVal linesArray()() As Double, ByVal profilePlaneSide As SolidEdgePart.FeaturePropertyConstants, ByVal extrusionDistance As Double) As SolidEdgePart.Model
        Dim profileSets As SolidEdgePart.ProfileSets = Nothing
        Dim profileSet As SolidEdgePart.ProfileSet = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing
        Dim relation2d As SolidEdgeFrameworkSupport.Relation2d = Nothing
        Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
        Dim line2d As SolidEdgeFrameworkSupport.Line2d = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim aProfiles As System.Array = Nothing

        ' Get a reference to the profile sets collection.
        profileSets = partDocument.ProfileSets

        ' Add a new profile set.
        profileSet = profileSets.Add()

        ' Get a reference to the profiles collection.
        profiles = profileSet.Profiles

        ' Add a new profile.
        profile = profiles.Add(refPlane)

        ' Get a reference to the lines2d collection.
        lines2d = profile.Lines2d

        ' Draw the Base Profile.
        For i As Integer = 0 To linesArray.GetUpperBound(0)
            line2d = lines2d.AddBy2Points(x1:= linesArray(i)(0), y1:= linesArray(i)(1), x2:= linesArray(i)(2), y2:= linesArray(i)(3))
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

            relation2d = relations2d.AddKeypoint(Object1:= lines2d.Item(i), Index1:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), Object2:= lines2d.Item(j), Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), guaranteed_ok:= True)
        Next i

        ' Close the profile.
        profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

        ' Hide the profile.
        profile.Visible = False

        ' Create a new array of profile objects.
        aProfiles = Array.CreateInstance(GetType(SolidEdgePart.Profile), 1)
        aProfiles.SetValue(profile, 0)

        ' Get a reference to the models collection.
        models = partDocument.Models

        ' Create the extended protrusion.
        model = models.AddFiniteExtrudedProtrusion(NumberOfProfiles:= aProfiles.Length, ProfileArray:= aProfiles, ProfilePlaneSide:= profilePlaneSide, ExtrusionDistance:= extrusionDistance)

        Return model
    End Function

    Public Shared Function CreateFiniteRevolvedProtrusion(ByVal partDocument As SolidEdgePart.PartDocument) As SolidEdgePart.Model
        Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
        Dim refPlane As SolidEdgePart.RefPlane = Nothing
        Dim profileSets As SolidEdgePart.ProfileSets = Nothing
        Dim profileSet As SolidEdgePart.ProfileSet = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
        Dim axis As SolidEdgeFrameworkSupport.Line2d = Nothing
        Dim arcs2d As SolidEdgeFrameworkSupport.Arcs2d = Nothing
        Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing
        Dim refaxis As SolidEdgePart.RefAxis = Nothing
        Dim aProfiles As Array = Nothing

        ' Get a reference to the models collection.
        models = DirectCast(partDocument.Models, SolidEdgePart.Models)

        ' D1 to FA are parameters in a form, introduced by the user.
        Dim D1 As Double = 0.020
        Dim D2 As Double = 0.026
        Dim D3 As Double = 0.003
        Dim D4 As Double = 0.014
        Dim L1 As Double = 0.040
        Dim L2 As Double = 0.030
        Dim L3 As Double = 0.005

        ' Get a reference to the ref planes collection.
        refPlanes = partDocument.RefPlanes

        ' Get a reference to front RefPlane.
        refPlane = refPlanes.GetFrontPlane()

        ' Get a reference to the profile sets collection.
        profileSets = DirectCast(partDocument.ProfileSets, SolidEdgePart.ProfileSets)

        ' Create a new profile set.
        profileSet = profileSets.Add()

        ' Get a reference to the profiles collection.
        profiles = profileSet.Profiles

        ' Create a new profile.
        profile = profiles.Add(refPlane)

        ' Get a reference to the profile lines2d collection.
        lines2d = profile.Lines2d

        ' Get a reference to the profile arcs2d collection.
        arcs2d = profile.Arcs2d

        Dim H As Double = L1 - L2
        Dim y As Double = L1 - L3 - (D4 - D3) / (2 * Math.Tan((118 \ 2) * (Math.PI / 180)))

        lines2d.AddBy2Points(D3 / 2, 0, D2 / 2, 0) ' Line1
        lines2d.AddBy2Points(D2 / 2, 0, D2 / 2, H) ' Line2
        lines2d.AddBy2Points(D2 / 2, H, D1 / 2, H) ' Line3
        lines2d.AddBy2Points(D1 / 2, H, D1 / 2, L1) ' Line4
        lines2d.AddBy2Points(D1 / 2, L1, D4 / 2, L1) ' Line5
        lines2d.AddBy2Points(D4 / 2, L1, D4 / 2, L1 - L3) ' Line6
        lines2d.AddBy2Points(D4 / 2, L1 - L3, D3 / 2, y) ' Line7
        lines2d.AddBy2Points(D3 / 2, y, D3 / 2, 0) ' Line8

        axis = lines2d.AddBy2Points(0, 0, 0, L1)
        profile.ToggleConstruction(axis)

        ' relations
        relations2d = DirectCast(profile.Relations2d, SolidEdgeFrameworkSupport.Relations2d)
        relations2d.AddKeypoint(lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(2), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(2), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(3), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(3), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(4), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(4), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(5), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(5), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(6), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(6), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(7), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(7), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(8), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)
        relations2d.AddKeypoint(lines2d.Item(8), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart), True)

        refaxis = DirectCast(profile.SetAxisOfRevolution(axis), SolidEdgePart.RefAxis)

        ' Close the profile.
        Dim status As Integer = profile.End(SolidEdgePart.ProfileValidationType.igProfileRefAxisRequired)
        profile.Visible = False

        ' Create a new array of profile objects.
        aProfiles = Array.CreateInstance(GetType(SolidEdgePart.Profile), 1)
        aProfiles.SetValue(profile, 0)

        ' add Finite Revolved Protrusion.
        model = models.AddFiniteRevolvedProtrusion(aProfiles.Length, aProfiles, refaxis, SolidEdgePart.FeaturePropertyConstants.igRight, 2 * Math.PI, Nothing, Nothing)

        Return model
    End Function

    Public Shared Sub CreateHolesWithUserDefinedPattern(ByVal partDocument As SolidEdgePart.PartDocument)
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
        CreateFiniteExtrudedProtrusion(partDocument, refplane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005)

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
    End Sub

    Public Shared Function CreateSweptProtrusion(ByVal partDocument As SolidEdgePart.PartDocument) As SolidEdgePart.Model
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

        Return model
    End Function
End Class