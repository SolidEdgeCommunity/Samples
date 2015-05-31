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
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new sheetmetal document using PROGID.
                sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)documents.Add("SolidEdge.SheetMetalDocument");

                // Create a new sheetmetal document using PROGID defined in Interop.SolidEdge.dll.
                sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)documents.Add(SolidEdgeSDK.PROGID.SolidEdge_SheetMetalDocument);

                // Create a new sheetmetal document using SolidEdge.Community.dll extension method.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Create a new sheetmetal document using SolidEdge.Community.dll extension method.
                sheetMetalDocument = documents.Add<SolidEdgePart.SheetMetalDocument>();

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
