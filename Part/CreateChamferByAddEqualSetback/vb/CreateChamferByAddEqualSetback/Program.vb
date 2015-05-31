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
        Dim profileSets As SolidEdgePart.ProfileSets = Nothing
        Dim profileSet As SolidEdgePart.ProfileSet = Nothing
        Dim profiles As SolidEdgePart.Profiles = Nothing
        Dim profile As SolidEdgePart.Profile = Nothing
        Dim lines2d As SolidEdgeFrameworkSupport.Lines2d = Nothing
        Dim line2d As SolidEdgeFrameworkSupport.Line2d = Nothing
        Dim relations2d As SolidEdgeFrameworkSupport.Relations2d = Nothing
        Dim relation2d As SolidEdgeFrameworkSupport.Relation2d = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim profileList As New List(Of SolidEdgePart.Profile)()
        Dim status As Integer = 0
        Dim extrudedProtrusions As SolidEdgePart.ExtrudedProtrusions = Nothing
        Dim extrudedProtrusion As SolidEdgePart.ExtrudedProtrusion = Nothing
        Dim edges As SolidEdgeGeometry.Edges = Nothing
        Dim edgeList As New List(Of Object)()
        Dim faces As SolidEdgeGeometry.Faces = Nothing
        Dim chamfers As SolidEdgePart.Chamfers = Nothing
        Dim chamfer As SolidEdgePart.Chamfer = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Bring Solid Edge to the foreground.
            application.Activate()

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new part document.
            partDocument = documents.AddPartDocument()

            ' Always a good idea to give SE a chance to breathe.
            application.DoIdle()

            refPlanes = partDocument.RefPlanes
            refPlane = refPlanes.GetTopPlane()

            profileSets = partDocument.ProfileSets
            profileSet = profileSets.Add()

            profiles = profileSet.Profiles
            profile = profiles.Add(refPlane)
            profileList.Add(profile)

            lines2d = profile.Lines2d
            line2d = lines2d.AddBy2Points(0, 0, 0.06, 0)
            line2d = lines2d.AddBy2Points(0.06, 0, 0.06, 0.06)
            line2d = lines2d.AddBy2Points(0.06, 0.06, 0, 0.06)
            line2d = lines2d.AddBy2Points(0, 0.06, 0, 0)

            relations2d = DirectCast(profile.Relations2d, SolidEdgeFrameworkSupport.Relations2d)
            relation2d = relations2d.AddKeypoint(Object1:= lines2d.Item(1), Index1:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), Object2:= lines2d.Item(2), Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))

            relation2d = relations2d.AddKeypoint(Object1:= lines2d.Item(2), Index1:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), Object2:= lines2d.Item(3), Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))

            relation2d = relations2d.AddKeypoint(Object1:= lines2d.Item(3), Index1:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), Object2:= lines2d.Item(4), Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))

            relation2d = relations2d.AddKeypoint(Object1:= lines2d.Item(4), Index1:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineEnd), Object2:= lines2d.Item(1), Index2:= CInt(SolidEdgeConstants.KeypointIndexConstants.igLineStart))

            ' Make sure profile is closed.
            status = profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed)

            If status <> 0 Then
                Throw New System.Exception("Profile not closed.")
            End If

            models = partDocument.Models

            model = models.AddFiniteExtrudedProtrusion(NumberOfProfiles:= profileList.Count, ProfileArray:= profileList.ToArray(), ProfilePlaneSide:= SolidEdgePart.FeaturePropertyConstants.igRight, ExtrusionDistance:= 0.02)

            profile.Visible = False

            extrudedProtrusions = model.ExtrudedProtrusions
            extrudedProtrusion = extrudedProtrusions.Item(1)

            'edges = (SolidEdgeGeometry.Edges)extrudedProtrusion.get_Edges(
            '    SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll);

            'edgeList.Add(edges.Item(5));
            'edgeList.Add(edges.Item(8));
            faces = DirectCast(extrudedProtrusion.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces) ' SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll);

            Dim face As SolidEdgeGeometry.Face = DirectCast(faces.Item(1), SolidEdgeGeometry.Face)

            edges = DirectCast(face.Edges, SolidEdgeGeometry.Edges)
            edgeList.Add(edges.Item(1))
            edgeList.Add(edges.Item(2))

            chamfers = model.Chamfers

            Dim setbackDistance As Double = 0.005

            chamfer = chamfers.AddEqualSetback(NumberOfEdgeSets:= edgeList.Count, EdgeSetArray:= edgeList.ToArray(), SetbackDistance:= setbackDistance)

            ' Switch to ISO view.
            'application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView);
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
