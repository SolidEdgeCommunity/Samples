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
        Dim sheet As SolidEdgeDraft.Sheet = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

            ' Get a reference to the active draft document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            If draftDocument IsNot Nothing Then
                ' Get a reference to the Sheets collection.
                sheets = draftDocument.Sheets

                ' Add a new sheet.
                sheet = sheets.AddSheet()

                ' Make the new sheet the active sheet.
                sheet.Activate()
            Else
                Throw New System.Exception("No active documet.")
            End If
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
