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
        Dim selectSet As SolidEdgeFramework.SelectSet = Nothing
        Dim features As Array = Array.CreateInstance(GetType(Object), 0)
        Dim extrudedProtrustion As SolidEdgePart.ExtrudedProtrusion = Nothing
        Dim round As SolidEdgePart.Round = Nothing

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

            ' Get a reference to the top RefPlane using extension method.
            refPlane = refPlanes.GetTopPlane()

            ' Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
            Dim trainingDirectory = New System.IO.DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath())

            ' Build path to source part document.
            Dim LibName = System.IO.Path.Combine(trainingDirectory.FullName, "block.par")

            ' This method will take all features from block.par and place them into the new part document.
            partDocument.PlaceFeatureLibrary(LibName, refPlane, 0.0, 0.0, 0.0, features)

            ' Optionally, iterate through all of the added features.
            For Each feature As Object In features
                ' Use helper class to get the feature type.
                Dim featureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue(Of SolidEdgePart.FeatureTypeConstants)(feature, "Type", CType(0, SolidEdgePart.FeatureTypeConstants))

                ' Depending on the feature type, we can cast the weakly typed feature to a strongly typed feature.
                Select Case featureType
                    Case SolidEdgePart.FeatureTypeConstants.igExtrudedProtrusionFeatureObject
                        extrudedProtrustion = DirectCast(feature, SolidEdgePart.ExtrudedProtrusion)
                    Case SolidEdgePart.FeatureTypeConstants.igRoundFeatureObject
                        round = DirectCast(feature, SolidEdgePart.Round)
                End Select
            Next feature

            ' Get a reference to the ActiveSelectSet.
            selectSet = application.ActiveSelectSet

            ' Empty ActiveSelectSet.
            selectSet.RemoveAll()

            ' Add all features to ActiveSelectSet.
            For Each feature As Object In features
                selectSet.Add(feature)
            Next feature

            ' Switch to ISO view.
            application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
