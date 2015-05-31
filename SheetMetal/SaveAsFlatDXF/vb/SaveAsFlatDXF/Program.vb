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
        Dim sheetMetalDocument As SolidEdgePart.SheetMetalDocument = Nothing
        Dim models As SolidEdgePart.Models = Nothing
        Dim model As SolidEdgePart.Model = Nothing
        Dim flatPatternModels As SolidEdgePart.FlatPatternModels = Nothing
        Dim flatPatternModel As SolidEdgePart.FlatPatternModel = Nothing
        Dim body As SolidEdgeGeometry.Body = Nothing
        Dim faces As SolidEdgeGeometry.Faces
        Dim face As SolidEdgeGeometry.Face = Nothing
        Dim edges As SolidEdgeGeometry.Edges = Nothing
        Dim edge As SolidEdgeGeometry.Edge = Nothing
        Dim vertex As SolidEdgeGeometry.Vertex = Nothing
        Dim useFlatPattern As Boolean = True
        Dim flatPatternIsUpToDate As Boolean = False
        Dim outFile As String = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to Solid Edge,
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

            ' Get a reference to the Documents collection.
            documents = application.Documents

            ' Get a refernce to the active sheetmetal document.
            sheetMetalDocument = application.GetActiveDocument(Of SolidEdgePart.SheetMetalDocument)(False)

            If sheetMetalDocument Is Nothing Then
                Throw New System.Exception("No active document.")
            End If

            ' Get a reference to the Models collection,
            models = sheetMetalDocument.Models

            ' Check for geometry.
            If models.Count = 0 Then
                Throw New System.Exception("No geometry defined.")
            End If

            ' Get a reference to the one and only model.
            model = models.Item(1)

            ' Get a reference to the FlatPatternModels collection,
            flatPatternModels = sheetMetalDocument.FlatPatternModels

            ' Observation: SaveAsFlatDXFEx() will fail if useFlatPattern is specified and
            ' flatPatternModels.Count = 0.
            ' The following code will turn off the useFlatPattern switch if flatPatternModels.Count = 0.
            If useFlatPattern Then
                If flatPatternModels.Count > 0 Then
                    For i As Integer = 1 To flatPatternModels.Count
                        flatPatternModel = flatPatternModels.Item(i)
                        flatPatternIsUpToDate = flatPatternModel.IsUpToDate

                        ' If we find one that is up to date, call it good and bail.
                        If flatPatternIsUpToDate Then
                            Exit For
                        End If
                    Next i

                    If flatPatternIsUpToDate = False Then
                        ' Flat patterns exist but are out of date.
                        useFlatPattern = False
                    End If
                Else
                    ' Can't use flat pattern if none exist.
                    useFlatPattern = False
                End If
            End If


            ' In a real world scenario, you would want logic (or user input) to pick the desired
            ' face, edge & *vertex. Here, we just grab the 1st we can find. *Vertex can be null.

            ' Get a reference to the model's body.
            body = DirectCast(model.Body, SolidEdgeGeometry.Body)

            ' Query for all faces in the body.
            faces = DirectCast(body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Faces)

            ' Get a reference to the first face.
            face = DirectCast(faces.Item(1), SolidEdgeGeometry.Face)

            ' Get a reference to the face's Edges collection,
            edges = DirectCast(face.Edges, SolidEdgeGeometry.Edges)

            ' Get a reference to the first edge.
            edge = DirectCast(edges.Item(1), SolidEdgeGeometry.Edge)

            ' Not using vertex in this example.
            vertex = Nothing

            outFile = System.IO.Path.ChangeExtension(sheetMetalDocument.FullName, ".dxf")

            ' Observation: If .par or .psm is specified in outFile, SE will open the file.
            ' Even if useFlatPattern = true, it's a good idea to specify defaults for face, edge & vertex.
            ' I've seen where outFile of .dxf would work but .psm would fail if the defaults were null;
            models.SaveAsFlatDXFEx(outFile, face, edge, vertex, useFlatPattern)

            If useFlatPattern Then
                Console.WriteLine("Saved '{0}' using Flat Pattern.", outFile)
            Else
                Console.WriteLine("Saved '{0}' without using Flat Pattern.", outFile)
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
