using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportEdgebarFeatures
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.EdgebarFeatures edgebarFeatures = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active part document.
                partDocument = application.GetActiveDocument<SolidEdgePart.PartDocument>(false);

                if (partDocument != null)
                {
                    // Get a reference to the DesignEdgebarFeatures collection.
                    edgebarFeatures = partDocument.DesignEdgebarFeatures;

                    // Interate through the features.
                    for (int i = 1; i <= edgebarFeatures.Count; i++)
                    {
                        // Get the EdgebarFeature at current index.
                        object edgebarFeature = edgebarFeatures.Item(i);

                        // Get the managed type.
                        var type = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(edgebarFeature);

                        Console.WriteLine("Item({0}) is of type '{1}'", i, type);

                    }
                }
                else
                {
                    throw new System.Exception("No active document.");
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
