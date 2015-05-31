Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace CreateFiniteExtrudedProtrusion
    Friend Class Program
        <STAThread> _
        Shared Sub Main(ByVal args() As String)
            Dim application As SolidEdgeFramework.Application = Nothing
            Dim documents As SolidEdgeFramework.Documents = Nothing
            Dim partDocument As SolidEdgePart.PartDocument = Nothing
            Dim refPlanes As SolidEdgePart.RefPlanes = Nothing
            Dim refPlane As SolidEdgePart.RefPlane = Nothing
            Dim model As SolidEdgePart.Model = Nothing
            Dim extrudedProtrusions As SolidEdgePart.ExtrudedProtrusions = Nothing
            Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
            Dim extrudedProtrusion As SolidEdgePart.ExtrudedProtrusion = Nothing

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
                model = PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005)

                ' Get a reference to the ExtrudedProtrusions collection.
                extrudedProtrusions = model.ExtrudedProtrusions

                ' Get a reference to the new ExtrudedProtrusion.
                extrudedProtrusion = extrudedProtrusions.Item(1)

                ' Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet

                ' Empty ActiveSelectSet.
                selectSet.RemoveAll()

                ' Add new protrusion to ActiveSelectSet.
                selectSet.Add(extrudedProtrusion)

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
