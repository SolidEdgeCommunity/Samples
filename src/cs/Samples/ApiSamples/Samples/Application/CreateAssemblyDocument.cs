using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Application
{
    /// <summary>
    /// Creates a new assembly document.
    /// </summary>
    class CreateAssemblyDocument
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new assembly document.
                assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)documents.Add(global::SolidEdge.PROGID.AssemblyDocument);
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
