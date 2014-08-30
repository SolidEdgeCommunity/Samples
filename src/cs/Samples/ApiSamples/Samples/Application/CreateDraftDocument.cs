using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Application
{
    /// <summary>
    /// Creates a new draft document.
    /// </summary>
    class CreateDraftDocument
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new draft document.
                draftDocument = (SolidEdgeDraft.DraftDocument)documents.Add(SolidEdgeSDK.PROGID.SolidEdge_DraftDocument);
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
