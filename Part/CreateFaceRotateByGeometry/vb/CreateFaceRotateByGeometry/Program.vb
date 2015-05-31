Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateFaceRotateByGeometry
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
            Dim body As SolidEdgeGeometry.Body = Nothing
            Dim faceRotates As SolidEdgePart.FaceRotates = Nothing
            Dim faceRotate As SolidEdgePart.FaceRotate = Nothing
            Dim faces As SolidEdgeGeometry.Faces = Nothing
            Dim face As SolidEdgeGeometry.Face = Nothing
            Dim edges As SolidEdgeGeometry.Edges = Nothing
            Dim edge As SolidEdgeGeometry.Edge = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
            Dim angle As Double = 0.5

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

                ' Get a reference to a RefPlane.
                refPlane = refPlanes.GetFrontPlane()

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
                PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005)

                ' Get a reference to the models collection.
                models = partDocument.Models
                model = models.Item(1)
                body = DirectCast(model.Body, SolidEdgeGeometry.Body)
                faces = DirectCast(body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)
                face = DirectCast(faces.Item(2), SolidEdgeGeometry.Face)
                edges = DirectCast(body.Edges(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Edges)
                edge = DirectCast(edges.Item(5), SolidEdgeGeometry.Edge)

                faceRotates = model.FaceRotates

                ' Add face rotate.
                faceRotate = faceRotates.Add(face, SolidEdgePart.FaceRotateConstants.igFaceRotateByGeometry, SolidEdgePart.FaceRotateConstants.igFaceRotateRecreateBlends, Nothing, Nothing, edge, SolidEdgePart.FaceRotateConstants.igFaceRotateAxisEnd, angle)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new FaceRotate to ActiveSelectSet.
                selectSet.Add(faceRotate)

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
