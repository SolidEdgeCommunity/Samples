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
            SolidEdgePart.PartDocument partDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new part document using PROGID.
                partDocument = (SolidEdgePart.PartDocument)documents.Add("SolidEdge.PartDocument");

                // Create a new part document using PROGID defined in Interop.SolidEdge.dll.
                partDocument = (SolidEdgePart.PartDocument)documents.Add(SolidEdgeSDK.PROGID.SolidEdge_PartDocument);

                // Create a new part document using SolidEdge.Community.dll extension method.
                partDocument = documents.AddPartDocument();

                // Create a new part document using SolidEdge.Community.dll extension method.
                partDocument = documents.Add<SolidEdgePart.PartDocument>();

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
