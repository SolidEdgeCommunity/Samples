Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateThickenFeatures
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
            Dim faces As SolidEdgeGeometry.Faces = Nothing
            Dim face As SolidEdgeGeometry.Face = Nothing
            Dim thickens As SolidEdgePart.Thickens = Nothing
            Dim thicken As SolidEdgePart.Thicken = Nothing

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Get a reference to the documents collection.
                documents = application.Documents

                ' Add a new part document.
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

                ' Get a reference to the models collection.
                models = partDocument.Models

                ' Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005)

                body = DirectCast(model.Body, SolidEdgeGeometry.Body)

                faces = DirectCast(body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)

                thickens = model.Thickens

                Dim type As Type = GetType(SolidEdgePart.FeaturePropertyConstants)
                Dim fields = type.GetFields()

                For i As Integer = 1 To faces.Count
                    model = models.Item(1)
                    body = DirectCast(model.Body, SolidEdgeGeometry.Body)
                    faces = DirectCast(body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)
                    thickens = model.Thickens

                    face = TryCast(faces.Item(i), SolidEdgeGeometry.Face)

                    Dim facesArray As Array = Array.CreateInstance(GetType(SolidEdgeGeometry.Face), 1)

                    facesArray.SetValue(face, 0)

                    For Each field In fields
                        If field.IsSpecialName Then
                            Continue For
                        End If
                        Dim side As SolidEdgePart.FeaturePropertyConstants = DirectCast(field.GetRawConstantValue(), SolidEdgePart.FeaturePropertyConstants)

                        Try
                            thicken = thickens.Add(side, 0.0254, 1, facesArray)

                            thicken.Delete()
                            Exit For
                        Catch
                        End Try
                    Next field
                    'thicken = thickens.Add(SolidEdgePart.FeaturePropertyConstants.igReverseNormalSideDummy, 0.0254, 1, ref facesArray);

                    'selectSet = application.ActiveSelectSet;
                    'selectSet.RemoveAll();
                    'selectSet.Add(faces.Item(1));
                Next i
            Catch ex As System.Exception
                Console.WriteLine(ex.Message)
            Finally
                SolidEdgeCommunity.OleMessageFilter.Unregister()
            End Try
        End Sub
    End Class
End Namespace
