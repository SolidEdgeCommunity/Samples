Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateSimpleLoftedCutout
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim partDocument As SolidEdgePart.PartDocument = Nothing
            Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
            Dim refPlane As SolidEdgePart.RefPlane = Nothing
            Dim models As SolidEdgePart.Models = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim profileSets As SolidEdgePart.ProfileSets = Nothing
            Dim profileSet As SolidEdgePart.ProfileSet = Nothing
            Dim profiles As SolidEdgePart.Profiles = Nothing
            Dim crossSectionProfiles As New List(Of SolidEdgePart.Profile)()
            Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
            Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing
            Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing
            Dim OriginArray As New List(Of Object)()
            Dim loftedCutouts As SolidEdgePart.LoftedCutouts = Nothing
            Dim loftedCutout As SolidEdgePart.LoftedCutout = Nothing
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

                ' Get a reference to top RefPlane.
                refPlane = refPlanes.GetTopPlane()

                ' Get a reference to the ProfileSets collection.
                profileSets = partDocument.ProfileSets

                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                ' Get a reference to the Models collection.
                models = partDocument.Models

'                #Region "Base Profile"

                Dim linesArray As New List(Of Double())()
                linesArray.Add(New Double() { 0, 0, 0.1, 0 })
                linesArray.Add(New Double() { 0.1, 0, 0.1, 0.1 })
                linesArray.Add(New Double() { 0.1, 0.1, 0, 0.1 })
                linesArray.Add(New Double() { 0, 0.1, 0, 0 })

                ' Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.1)

'                #End Region

'                #Region "CrossSection Profile #1"

                refPlane = refPlanes.AddParallelByDistance(ParentPlane:= refPlanes.GetRightPlane(), Distance:= 0.1, NormalSide:= SolidEdgePart.ReferenceElementConstants.igNormalSide, Local:= True)

                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                crossSectionProfiles.Add(profiles.Add(refPlane))

                OriginArray.Add(New Double() { 0.03, 0.03 })

                lines2d = crossSectionProfiles(0).Lines2d
                lines2d.AddBy2Points(0.03, 0.03, 0.07, 0.03)
                lines2d.AddBy2Points(0.07, 0.03, 0.07, 0.07)
                lines2d.AddBy2Points(0.07, 0.07, 0.03, 0.07)
                lines2d.AddBy2Points(0.03, 0.07, 0.03, 0.03)

                relations2d = DirectCast(crossSectionProfiles(0).Relations2d, SolidEdgeFrameworkSupport.Relations2d)

                relations2d.AddKeypoint(lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(2), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                relations2d.AddKeypoint(lines2d.Item(2), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(3), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                relations2d.AddKeypoint(lines2d.Item(3), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(4), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                relations2d.AddKeypoint(lines2d.Item(4), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))

                crossSectionProfiles(0).End(SolidEdgePart.ProfileValidationType.igProfileClosed)

                crossSectionProfiles(0).Visible = False

'                #End Region

'                #Region "CrossSection Profile #2"

                refPlane = refPlanes.AddParallelByDistance(ParentPlane:= refPlanes.GetRightPlane(), Distance:= 0.05, NormalSide:= SolidEdgePart.ReferenceElementConstants.igNormalSide, Local:= True)
                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                crossSectionProfiles.Add(profiles.Add(refPlane))

                OriginArray.Add(New Double() { 0.0, 0.0 })

                circles2d = crossSectionProfiles(1).Circles2d
                circles2d.AddByCenterRadius(0.05, 0.05, 0.015)

                crossSectionProfiles(1).End(SolidEdgePart.ProfileValidationType.igProfileClosed)

                crossSectionProfiles(1).Visible = False

'                #End Region

'                #Region "CrossSection Profile #3"

                refPlane = refPlanes.AddParallelByDistance(ParentPlane:= refPlanes.GetRightPlane(), Distance:= 0, NormalSide:= SolidEdgePart.ReferenceElementConstants.igNormalSide, Local:= True)

                ' Add new ProfileSet.
                profileSet = profileSets.Add()

                ' Get a reference to the Profiles collection.
                profiles = profileSet.Profiles

                crossSectionProfiles.Add(profiles.Add(refPlane))

                OriginArray.Add(New Double() { 0.03, 0.03 })

                lines2d = crossSectionProfiles(2).Lines2d
                lines2d.AddBy2Points(0.03, 0.03, 0.07, 0.03)
                lines2d.AddBy2Points(0.07, 0.03, 0.07, 0.07)
                lines2d.AddBy2Points(0.07, 0.07, 0.03, 0.07)
                lines2d.AddBy2Points(0.03, 0.07, 0.03, 0.03)

                relations2d = DirectCast(crossSectionProfiles(2).Relations2d, SolidEdgeFrameworkSupport.Relations2d)

                relations2d.AddKeypoint(lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(2), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                relations2d.AddKeypoint(lines2d.Item(2), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(3), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                relations2d.AddKeypoint(lines2d.Item(3), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(4), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))
                relations2d.AddKeypoint(lines2d.Item(4), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), lines2d.Item(1), CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))

                crossSectionProfiles(2).End(SolidEdgePart.ProfileValidationType.igProfileClosed)

                crossSectionProfiles(2).Visible = False

'                #End Region

                ' Get a reference to the LoftedCutouts collection.
                loftedCutouts = model.LoftedCutouts

                ' Build cross section type array.
                Dim crossSectionTypes As New List(Of Object)()
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection)

                ' Create the lofted cutout.
                loftedCutout = loftedCutouts.AddSimple(crossSectionProfiles.Count, crossSectionProfiles.ToArray(), crossSectionTypes.ToArray(), OriginArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igLeft, SolidEdgePart.FeaturePropertyConstants.igNone, SolidEdgePart.FeaturePropertyConstants.igNone)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new LoftedCutout to ActiveSelectSet.
                selectSet.Add(loftedCutout)

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
