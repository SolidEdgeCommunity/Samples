using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PerformRenameAction
{
    class Program
    {
        static void Main(string[] args)
        {
            // For testing purposes, change the path to the .asm.
            var assemblyPath = @"C:\Users\jason\Desktop\Asm3.asm";

            // Start Revision Manager.
            var application = new RevisionManager.Application();

            // Open the assembly.
            var assemblyDocument = (RevisionManager.Document)application.OpenFileInRevisionManager(assemblyPath);

            // Get the linked documents.
            var linkedDocuments = (RevisionManager.LinkedDocuments)assemblyDocument.LinkedDocuments;

            // Allocate input arrays.
            var ListOfInputFileNames = new List<string>();
            var ListOfNewFileNames = new List<string>();
            var ListOfInputActions = new List<RevisionManager.RevisionManagerAction>();

            // Process each linked document.
            for (int i = 1; i <= linkedDocuments.Count; i++)
            {
                // Get the specified linked document by index.
                var linkedDocument = (RevisionManager.Document)linkedDocuments.Item[i];

                // Get the current path, folder path, filename and extension to the linked document.
                var linkedDocumentPath = linkedDocument.AbsolutePath;
                var linkedDocumentDirectory = System.IO.Path.GetDirectoryName(linkedDocumentPath);
                var linkedDocumentFilename = System.IO.Path.GetFileName(linkedDocumentPath);
                var linkedDocumentExtension = System.IO.Path.GetExtension(linkedDocumentPath);

                // Generate a new random filename for the linked document.
                var linkedDocumentNewPath = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss-fff", CultureInfo.InvariantCulture);
                linkedDocumentNewPath = System.IO.Path.ChangeExtension(linkedDocumentNewPath, linkedDocumentExtension);
                linkedDocumentNewPath = System.IO.Path.Combine(linkedDocumentDirectory, linkedDocumentNewPath);

                // Sleep for 1 millisecond to avoid filename collision. Only relevant for this example.
                System.Threading.Thread.Sleep(1);

                // Populate the arrays.
                ListOfInputFileNames.Add(linkedDocumentPath);
                ListOfNewFileNames.Add(linkedDocumentNewPath);
                ListOfInputActions.Add(RevisionManager.RevisionManagerAction.RenameAction);
            }

            // Set the action.
            application.SetActionInRevisionManager(ListOfInputFileNames.Count, ListOfInputFileNames.ToArray(), ListOfInputActions.ToArray(), ListOfNewFileNames.ToArray());

            // Perform the action.
            application.PerformActionInRevisionManager();

            // Close the assembly.
            assemblyDocument.Close();

            // Close Revision Manager.
            application.Quit();
        }
    }
}
