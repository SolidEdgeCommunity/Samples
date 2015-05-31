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

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new sheetmetal document using PROGID.
            sheetMetalDocument = DirectCast(documents.Add("SolidEdge.SheetMetalDocument"), SolidEdgePart.SheetMetalDocument)

            ' Create a new sheetmetal document using PROGID defined in Interop.SolidEdge.dll.
            sheetMetalDocument = DirectCast(documents.Add(SolidEdgeSDK.PROGID.SolidEdge_SheetMetalDocument), SolidEdgePart.SheetMetalDocument)

            ' Create a new sheetmetal document using SolidEdge.Community.dll extension method.
            sheetMetalDocument = documents.AddSheetMetalDocument()

            ' Create a new sheetmetal document using SolidEdge.Community.dll extension method.
            sheetMetalDocument = documents.Add(Of SolidEdgePart.SheetMetalDocument)()

        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
