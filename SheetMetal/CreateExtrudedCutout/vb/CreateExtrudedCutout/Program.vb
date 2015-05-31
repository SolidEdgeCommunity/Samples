Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateExtrudedCutout
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
            Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
            Dim refPlane As SolidEdgePart.RefPlane = Nothing
            Dim sketchs As SolidEdgePart.Sketchs = Nothing
            Dim sketch As SolidEdgePart.Sketch = Nothing
            Dim profiles As SolidEdgePart.Profiles = Nothing
            Dim profile As SolidEdgePart.Profile = Nothing
            Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing
            Dim circle2d As SolidEdgeFrameworkSupport.Circle2d = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim extrudedCutouts As SolidEdgePart.ExtrudedCutouts = Nothing
            Dim extrudedCutout As SolidEdgePart.ExtrudedCutout = Nothing
            Dim profileList As New List(Of SolidEdgePart.Profile)()
            Dim finiteDepth1 As Double = 0.5
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
                refPlanes = sheetMetalDocument.RefPlanes

                ' Get a reference to right RefPlane.
                refPlane = refPlanes.GetRightPlane()

                ' Get a reference to the Sketches collection.
                sketchs = sheetMetalDocument.Sketches

                ' Add a new Sketch.
                sketch = sketchs.Add()

                ' Get a reference to the Profiles collection.
                profiles = sketch.Profiles

                ' Add a new Profile.
                profile = profiles.Add(refPlane)

                profileList.Add(profile)

                ' Create 2D circle.
                circles2d = profile.Circles2d
                circle2d = circles2d.AddByCenterRadius(0, 0, 0.025)

                profile.Visible = False

                ' Get a reference to the ExtrudedCutouts collection.
                extrudedCutouts = model.ExtrudedCutouts

                ' Add a new ExtrudedCutout.
                extrudedCutout = extrudedCutouts.Add(profileList.Count, profileList.ToArray(), SolidEdgePart.FeaturePropertyConstants.igLeft, SolidEdgePart.FeaturePropertyConstants.igFinite, SolidEdgePart.FeaturePropertyConstants.igSymmetric, finiteDepth1, Nothing, SolidEdgePart.KeyPointExtentConstants.igTangentNormal, Nothing, SolidEdgePart.OffsetSideConstants.seOffsetNone, 0, SolidEdgePart.TreatmentTypeConstants.seTreatmentNone, SolidEdgePart.DraftSideConstants.seDraftNone, 0, SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownNone, SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideNone, SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureNone, 0, 0, SolidEdgePart.FeaturePropertyConstants.igNone, SolidEdgePart.FeaturePropertyConstants.igNone, 0, Nothing, SolidEdgePart.KeyPointExtentConstants.igTangentNormal, Nothing, SolidEdgePart.OffsetSideConstants.seOffsetNone, 0, SolidEdgePart.TreatmentTypeConstants.seTreatmentNone, SolidEdgePart.DraftSideConstants.seDraftNone, 0, SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownNone, SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideNone, SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureNone, 0, 0)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new ExtrudedCutout to ActiveSelectSet.
                selectSet.Add(extrudedCutout)

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
