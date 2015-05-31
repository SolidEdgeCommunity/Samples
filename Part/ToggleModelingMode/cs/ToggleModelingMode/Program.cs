using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToggleModelingMode
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active sheetmetal document.
                partDocument = application.GetActiveDocument<SolidEdgePart.PartDocument>(false);

                if (partDocument != null)
                {
                    switch (partDocument.ModelingMode)
                    {
                        case SolidEdgePart.ModelingModeConstants.seModelingModeOrdered:
                            partDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous;
                            Console.WriteLine("Modeling mode changed to synchronous.");
                            break;
                        case SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous:
                            partDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeOrdered;
                            Console.WriteLine("Modeling mode changed to ordered (traditional).");
                            break;
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
