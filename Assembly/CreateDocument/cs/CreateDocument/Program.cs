using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateDocument
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new assembly document using PROGID.
                assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)documents.Add("SolidEdge.AssemblyDocument");

                // Create a new assembly document using PROGID defined in Interop.SolidEdge.dll.
                assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)documents.Add(SolidEdgeSDK.PROGID.SolidEdge_AssemblyDocument);

                // Create a new assembly document using SolidEdge.Community.dll extension method.
                assemblyDocument = documents.AddAssemblyDocument();

                // Create a new assembly document using SolidEdge.Community.dll extension method.
                assemblyDocument = documents.Add<SolidEdgeAssembly.AssemblyDocument>();
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
