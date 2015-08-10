Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Text

Namespace PerformRenameAction
    Friend Class Program
        Shared Sub Main(ByVal args() As String)
            ' For testing purposes, change the path to the .asm.
            Dim assemblyPath = "C:\Users\jason\Desktop\Asm3.asm"

            ' Start Revision Manager.
            Dim application = New RevisionManager.Application()

            ' Open the assembly.
            Dim assemblyDocument = DirectCast(application.OpenFileInRevisionManager(assemblyPath), RevisionManager.Document)

            ' Get the linked documents.
            Dim linkedDocuments = DirectCast(assemblyDocument.LinkedDocuments, RevisionManager.LinkedDocuments)

            ' Allocate input arrays.
            Dim ListOfInputFileNames = New List(Of String)()
            Dim ListOfNewFileNames = New List(Of String)()
            Dim ListOfInputActions = New List(Of RevisionManager.RevisionManagerAction)()

            ' Process each linked document.
            For i As Integer = 1 To linkedDocuments.Count
                ' Get the specified linked document by index.
                Dim linkedDocument = DirectCast(linkedDocuments.Item(i), RevisionManager.Document)

                ' Get the current path, folder path, filename and extension to the linked document.
                Dim linkedDocumentPath = linkedDocument.AbsolutePath
                Dim linkedDocumentDirectory = System.IO.Path.GetDirectoryName(linkedDocumentPath)
                Dim linkedDocumentFilename = System.IO.Path.GetFileName(linkedDocumentPath)
                Dim linkedDocumentExtension = System.IO.Path.GetExtension(linkedDocumentPath)

                ' Generate a new random filename for the linked document.
                Dim linkedDocumentNewPath = Date.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture)
                linkedDocumentNewPath = System.IO.Path.ChangeExtension(linkedDocumentNewPath, linkedDocumentExtension)
                linkedDocumentNewPath = System.IO.Path.Combine(linkedDocumentDirectory, linkedDocumentNewPath)

                ' Sleep for 1 millisecond to avoid filename collision. Only relevant for this example.
                System.Threading.Thread.Sleep(1)

                ' Populate the arrays.
                ListOfInputFileNames.Add(linkedDocumentPath)
                ListOfNewFileNames.Add(linkedDocumentNewPath)
                ListOfInputActions.Add(RevisionManager.RevisionManagerAction.RenameAction)
            Next i

            ' Set the action.
            application.SetActionInRevisionManager(ListOfInputFileNames.Count, ListOfInputFileNames.ToArray(), ListOfInputActions.ToArray(), ListOfNewFileNames.ToArray())

            ' Perform the action.
            application.PerformActionInRevisionManager()

            ' Close the assembly.
            assemblyDocument.Close()

            ' Close Revision Manager.
            application.Quit()
        End Sub
    End Class
End Namespace
