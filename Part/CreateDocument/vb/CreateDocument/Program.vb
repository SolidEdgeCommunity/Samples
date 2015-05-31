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

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new part document using PROGID.
            partDocument = DirectCast(documents.Add("SolidEdge.PartDocument"), SolidEdgePart.PartDocument)

            ' Create a new part document using PROGID defined in Interop.SolidEdge.dll.
            partDocument = DirectCast(documents.Add(SolidEdgeSDK.PROGID.SolidEdge_PartDocument), SolidEdgePart.PartDocument)

            ' Create a new part document using SolidEdge.Community.dll extension method.
            partDocument = documents.AddPartDocument()

            ' Create a new part document using SolidEdge.Community.dll extension method.
            partDocument = documents.Add(Of SolidEdgePart.PartDocument)()

        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
