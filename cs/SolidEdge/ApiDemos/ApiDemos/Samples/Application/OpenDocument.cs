using SolidEdgeCommunity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApiDemos.Application
{
    /// <summary>
    /// Creates a new part document.
    /// </summary>
    class OpenDocument
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                var dialog = new OpenFileDialog();
                dialog.Filter = "All Solid Edge documents|*.asm;*.dft;*.par;*.psm;*.pwd";
                dialog.InitialDirectory = SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Open existing document using extension method.
                    var document = documents.Open<SolidEdgeFramework.SolidEdgeDocument>(dialog.FileName);

                    // Determine the document type and cast accordingly.
                    switch (document.Type)
                    {
                        case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                            var assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)document;
                            break;
                        case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument:
                            var draftDocument = (SolidEdgeDraft.DraftDocument)document;
                            break;
                        case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                        case SolidEdgeFramework.DocumentTypeConstants.igSyncPartDocument:
                            var partDocument = (SolidEdgePart.PartDocument)document;
                            break;
                        case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument:
                        case SolidEdgeFramework.DocumentTypeConstants.igSyncSheetMetalDocument:
                            var sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)document;
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
