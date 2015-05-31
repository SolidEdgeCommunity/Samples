Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim sheets As SolidEdgeDraft.Sheets = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the active document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            ' Make sure we have a document.
            If draftDocument IsNot Nothing Then
                ' Get a reference to the sheets collection.
                sheets = draftDocument.Sheets

                For Each sheet In sheets.OfType(Of SolidEdgeDraft.Sheet)()
                    Console.WriteLine("Name: {0}", sheet.Name)
                    Console.WriteLine("Index: {0}", sheet.Index)
                    Console.WriteLine("Number: {0}", sheet.Number)
                    Console.WriteLine("SectionType: {0}", sheet.SectionType)
                    Console.WriteLine()
                Next sheet
            Else
                Throw New System.Exception("No active document.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
