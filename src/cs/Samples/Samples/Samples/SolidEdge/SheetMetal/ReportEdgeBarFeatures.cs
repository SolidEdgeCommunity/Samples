using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.SheetMetal
{
    /// <summary>
    /// Reports the edgebar features of the current part.
    /// </summary>
    class ReportEdgebarFeatures
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.EdgebarFeatures edgebarFeatures = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true);

                // Make sure user can see the GUI.
                application.Visible = true;

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active sheetmetal document.
                sheetMetalDocument = application.GetActiveDocument<SolidEdgePart.SheetMetalDocument>(false);

                if (sheetMetalDocument != null)
                {
                    // Get a reference to the DesignEdgebarFeatures collection.
                    edgebarFeatures = sheetMetalDocument.DesignEdgebarFeatures;

                    // Interate through the features.
                    for (int i = 1; i <= edgebarFeatures.Count; i++)
                    {
                        // Get the EdgebarFeature at current index.
                        object edgebarFeature = edgebarFeatures.Item(i);

                        Type type = IDispatchHelper.GetManagedType(edgebarFeature);

                        Console.WriteLine("Item({0}) is of type '{1}'", i, type);

                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActivePartDocument);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                OleMessageFilter.Unregister();
            }
        }
    }
}
