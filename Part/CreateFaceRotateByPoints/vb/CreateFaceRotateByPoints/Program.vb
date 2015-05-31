Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateFaceRotateByPoints
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
            Dim vertices As SolidEdgeGeometry.Vertices = Nothing
            Dim point1 As SolidEdgeGeometry.Vertex = Nothing
            Dim point2 As SolidEdgeGeometry.Vertex = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
            Dim angle As Double = 0.0872664625997165

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Get a reference to the Documents collection.
                documents = application.Documents

                ' Create a new part document.
                partDocument = documents.AddPartDocument()

                ' Get a reference to the RefPlanes collection.
                refPlanes = partDocument.RefPlanes

                ' Get a reference to a RefPlane.
                refPlane = refPlanes.GetFrontPlane()

                ' Always a good idea to give SE a chance to breathe.
                application.DoIdle()

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
                face = DirectCast(faces.Item(1), SolidEdgeGeometry.Face)

                vertices = DirectCast(face.Vertices, SolidEdgeGeometry.Vertices)
                point1 = DirectCast(vertices.Item(1), SolidEdgeGeometry.Vertex)
                point2 = DirectCast(vertices.Item(2), SolidEdgeGeometry.Vertex)

                faceRotates = model.FaceRotates

                ' Add face rotate.
                faceRotate = faceRotates.Add(face, SolidEdgePart.FaceRotateConstants.igFaceRotateByPoints, SolidEdgePart.FaceRotateConstants.igFaceRotateRecreateBlends, point1, point2, Nothing, SolidEdgePart.FaceRotateConstants.igFaceRotateNone, angle)

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
