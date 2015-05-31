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
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new draft document using PROGID.
            draftDocument = DirectCast(documents.Add("SolidEdge.DraftDocument"), SolidEdgeDraft.DraftDocument)

            ' Create a new draft document using PROGID defined in Interop.SolidEdge.dll.
            draftDocument = DirectCast(documents.Add(SolidEdgeSDK.PROGID.SolidEdge_DraftDocument), SolidEdgeDraft.DraftDocument)

            ' Create a new draft document using SolidEdge.Community.dll extension method.
            draftDocument = documents.AddDraftDocument()

            ' Create a new draft document using SolidEdge.Community.dll extension method.
            draftDocument = documents.Add(Of SolidEdgeDraft.DraftDocument)()
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
