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
        Dim leaders As SolidEdgeFrameworkSupport.Leaders = Nothing
        Dim leader As SolidEdgeFrameworkSupport.Leader = Nothing
        Dim dimStyle As SolidEdgeFrameworkSupport.DimStyle = Nothing

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

            ' Add a new text box.
            textBox = textBoxes.Add(0.25, 0.25, 0)
            textBox.TextScale = 1
            textBox.VerticalAlignment = SolidEdgeFrameworkSupport.TextVerticalAlignmentConstants.igTextHzAlignVCenter
            textBox.Text = "Leader"

            ' Get a reference to the Leaders collection.
            leaders = DirectCast(sheet.Leaders, SolidEdgeFrameworkSupport.Leaders)

            Console.WriteLine("Creating a new leader. ")

            ' Add a new leader.
            leader = leaders.Add(0.225, 0.225, 0, 0.25, 0.25, 0)
            dimStyle = leader.Style
            dimStyle.FreeSpaceTerminatorType = SolidEdgeFrameworkSupport.DimTermTypeConstants.igDimStyleTermFilled
        Catch ex As System.Exception
            Console.WriteLine(ex.Message)
        Finally
            SolidEdgeCommunity.OleMessageFilter.Unregister()
        End Try
    End Sub
End Class
