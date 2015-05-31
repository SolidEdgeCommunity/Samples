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
        Dim textBoxes As SolidEdgeFrameworkSupport.TextBoxes = Nothing
        Dim textBox As SolidEdgeFrameworkSupport.TextBox = Nothing

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

            ' Get a reference to the TextBoxes collection.
            textBoxes = DirectCast(sheet.TextBoxes, SolidEdgeFrameworkSupport.TextBoxes)

            ' Disable screen updating for performance.
            application.ScreenUpdating = False

            Dim x As Double = 0
            Dim y As Double = 0

            For i As Integer = 0 To 9
                x +=.05
                y = 0
                For j As Integer = 0 To 49
                    y +=.01
                    ' Add a new text box.
                    textBox = textBoxes.Add(x, y, 0)
                    textBox.TextScale = 1
                    textBox.VerticalAlignment = SolidEdgeFrameworkSupport.TextVerticalAlignmentConstants.igTextHzAlignVCenter
                    textBox.Text = String.Format("[X: {0:0.###} Y: {1:0.###}]", x, y)
                Next j
            Next i

            ' Turn screen updating back on.
            application.ScreenUpdating = True

            ' Fit the view.
            application.StartCommand(SolidEdgeConstants.DetailCommandConstants.DetailViewFit)
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
