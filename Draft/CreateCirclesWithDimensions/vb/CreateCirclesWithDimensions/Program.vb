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
        Dim sheet As SolidEdgeDraft.Sheet = Nothing
        Dim circles2d As SolidEdgeFrameworkSupport.Circles2d = Nothing
        Dim circle2d As SolidEdgeFrameworkSupport.Circle2d = Nothing
        Dim dimensions As SolidEdgeFrameworkSupport.Dimensions = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True, True)

            ' Get a reference to the documents collection.
            documents = application.Documents

            ' Create a new draft document.
            draftDocument = documents.AddDraftDocument()

            ' Get a reference to the active sheet.
            sheet = draftDocument.ActiveSheet

            ' Get a reference to the Circles2d collection.
            circles2d = sheet.Circles2d

            ' Get a reference to the Dimensions collection.
            dimensions = DirectCast(sheet.Dimensions, SolidEdgeFrameworkSupport.Dimensions)

            Dim x As Double = 0.1
            Dim y As Double = 0.1
            Dim radius As Double = 0.01

            For i As Integer = 0 To 4
                ' Add the circle.
                circle2d = circles2d.AddByCenterRadius(x, y, radius)

                ' Dimension the circle.
                dimensions.AddRadialDiameter(circle2d)

                x += 0.05
                y += 0.05
                radius += 0.01
            Next i
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
