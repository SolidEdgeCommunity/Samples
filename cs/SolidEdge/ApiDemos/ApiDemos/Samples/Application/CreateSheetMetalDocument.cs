using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Application
{
    /// <summary>
    /// Creates a new sheetmetal document.
    /// </summary>
    class CreateSheetMetalDocument
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new sheetmetal document.
                sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)documents.Add(SolidEdgeSDK.PROGID.SolidEdge_SheetMetalDocument);
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
