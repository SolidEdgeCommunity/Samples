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
        Dim constructions As SolidEdgePart.Constructions = Nothing
        Dim extrudedSurfaces As SolidEdgePart.ExtrudedSurfaces = Nothing
        Dim extrudedSurface As SolidEdgePart.ExtrudedSurface = Nothing
        Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the Documents collection.
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

            ' Get a reference to the Constructions collection.
            constructions = partDocument.Constructions

            ' Get a reference to the ExtrudedSurfaces collection.
            extrudedSurfaces = constructions.ExtrudedSurfaces

            ' These parameter variables are declared because we have to pass them as pointers.
            Dim KeyPointFlags1 As SolidEdgePart.KeyPointExtentConstants = SolidEdgePart.KeyPointExtentConstants.igTangentNormal
            Dim KeyPointFlags2 As SolidEdgePart.KeyPointExtentConstants = SolidEdgePart.KeyPointExtentConstants.igTangentNormal

            Dim profileList As New List(Of SolidEdgePart.Profile)()

            For i As Integer = 1 To profiles.Count
                profileList.Add(profiles.Item(i))
            Next i

            Dim profileArray As Array = profileList.ToArray()

            ' Add a new ExtrudedSurface.
            extrudedSurface = extrudedSurfaces.Add(NumberOfProfiles:= profileArray.Length, ProfileArray:= profileArray, ExtentType1:= SolidEdgePart.FeaturePropertyConstants.igFinite, ExtentSide1:= SolidEdgePart.FeaturePropertyConstants.igRight, FiniteDepth1:= 0.0127, KeyPointOrTangentFace1:= Nothing, KeyPointFlags1:= KeyPointFlags1, FromFaceOrRefPlane:= Nothing, FromFaceOffsetSide:= SolidEdgePart.OffsetSideConstants.seOffsetNone, FromFaceOffsetDistance:= 0, TreatmentType1:= SolidEdgePart.TreatmentTypeConstants.seTreatmentCrown, TreatmentDraftSide1:= SolidEdgePart.DraftSideConstants.seDraftInside, TreatmentDraftAngle1:= 0.1, TreatmentCrownType1:= SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownByOffset, TreatmentCrownSide1:= SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideInside, TreatmentCrownCurvatureSide1:= SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureInside, TreatmentCrownRadiusOrOffset1:= 0.003, TreatmentCrownTakeOffAngle1:= 0, ExtentType2:= SolidEdgePart.FeaturePropertyConstants.igFinite, ExtentSide2:= SolidEdgePart.FeaturePropertyConstants.igLeft, FiniteDepth2:= 0.0127, KeyPointOrTangentFace2:= Nothing, KeyPointFlags2:= KeyPointFlags2, ToFaceOrRefPlane:= Nothing, ToFaceOffsetSide:= SolidEdgePart.OffsetSideConstants.seOffsetNone, ToFaceOffsetDistance:= 0, TreatmentType2:= SolidEdgePart.TreatmentTypeConstants.seTreatmentCrown, TreatmentDraftSide2:= SolidEdgePart.DraftSideConstants.seDraftNone, TreatmentDraftAngle2:= 0, TreatmentCrownType2:= SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownByOffset, TreatmentCrownSide2:= SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideInside, TreatmentCrownCurvatureSide2:= SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureInside, TreatmentCrownRadiusOrOffset2:= 0.003, TreatmentCrownTakeOffAngle2:= 0, WantEndCaps:= True)

            Dim ExtentType As SolidEdgePart.FeaturePropertyConstants = Nothing
            Dim ExtentSide As SolidEdgePart.FeaturePropertyConstants = Nothing
            Dim FiniteDepth As Double = 0.0
            Dim KeyPointFlags As SolidEdgePart.KeyPointExtentConstants = SolidEdgePart.KeyPointExtentConstants.igTangentNormal

            ' Get extent information for the first direction.
            extrudedSurface.GetDirection1Extent(ExtentType, ExtentSide, FiniteDepth)

            ' Modify parameters.
            FiniteDepth = 2.0
            ExtentType = SolidEdgePart.FeaturePropertyConstants.igFinite
            ExtentSide = SolidEdgePart.FeaturePropertyConstants.igRight

            ' Apply extent information for the first direction.
            extrudedSurface.ApplyDirection1Extent(ExtentType, ExtentSide, FiniteDepth, Nothing, KeyPointFlags)

            ' Get a reference to the ActiveSelectSet.
            selectSet = application.ActiveSelectSet

            ' Empty ActiveSelectSet.
            selectSet.RemoveAll()

            ' Add new FaceRotate to ActiveSelectSet.
            selectSet.Add(extrudedSurface)

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
