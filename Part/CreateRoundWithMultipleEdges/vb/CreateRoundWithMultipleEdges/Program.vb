Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateRoundWithMultipleEdges
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim partDocument As SolidEdgePart.PartDocument = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim revolvedProtrusions As SolidEdgePart.RevolvedProtrusions = Nothing
            Dim revolvedProtrusion As SolidEdgePart.RevolvedProtrusion = Nothing
            Dim edges As SolidEdgeGeometry.Edges = Nothing
            Dim rounds As SolidEdgePart.Rounds = Nothing
            Dim round As SolidEdgePart.Round = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing

            Try
                ' Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register()

                ' Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

                ' Get a reference to the Documents collection.
                documents = application.Documents

                ' Create a new PartDocument.
                partDocument = documents.AddPartDocument()

                ' Always a good idea to give SE a chance to breathe.
                application.DoIdle()

                ' Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteRevolvedProtrusion(partDocument)

                ' Get a reference to the RevolvedProtrusions collection.
                revolvedProtrusions = model.RevolvedProtrusions

                ' Get a reference to the new RevolvedProtrusion.
                revolvedProtrusion = revolvedProtrusions.Item(1)

                ' Get a all Edges.
                edges = DirectCast(revolvedProtrusion.Edges(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll), SolidEdgeGeometry.Edges)

                Dim edgeList As New List(Of SolidEdgeGeometry.Edge)()
                Dim radiusList As New List(Of Double)()

                ' Build arrays.
                For Each edge As SolidEdgeGeometry.Edge In edges
                    edgeList.Add(edge)
                    radiusList.Add(0.002)
                Next edge

                ' Get a reference to the Rounds collection.
                rounds = model.Rounds

                ' Add single round with multiple Edges.
                round = rounds.Add(edgeList.Count, edgeList.ToArray(), radiusList.ToArray())

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new Round to ActiveSelectSet.
                selectSet.Add(round)

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
