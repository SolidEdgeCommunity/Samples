Imports SolidEdgeCommunity.Extensions ' https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Friend Class Program
    <STAThread> _
    Shared Sub Main(ByVal args() As String)
        Dim application As SolidEdgeFramework.Application = Nothing
        Dim draftDocument As SolidEdgeDraft.DraftDocument = Nothing
        Dim sheet As SolidEdgeDraft.Sheet = Nothing

        Try
            ' Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register()

            ' Connect to or start Solid Edge.
            application = SolidEdgeCommunity.SolidEdgeUtils.Connect(False)

            ' Get a reference to the active draft document.
            draftDocument = application.GetActiveDocument(Of SolidEdgeDraft.DraftDocument)(False)

            If draftDocument IsNot Nothing Then
                ' Get a reference to the active sheet.
                sheet = draftDocument.ActiveSheet

                Dim dialog As New SaveFileDialog()

                ' Set a default file name
                dialog.FileName = System.IO.Path.ChangeExtension(sheet.Name, ".emf")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                dialog.Filter = "Enhanced Metafile (*.emf)|*.emf"

                If dialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    ' Save the sheet as an EMF file.
                    sheet.SaveAsEnhancedMetafile(dialog.FileName)

                    Console.WriteLine("Created '{0}'", dialog.FileName)
                End If
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
