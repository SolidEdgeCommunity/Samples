using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.SheetMetal
{
    /// <summary>
    /// Recomputes the model of the active sheetmetal.
    /// </summary>
    class RecomputeModel
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Models models = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active part document.
                sheetMetalDocument = application.GetActiveDocument<SolidEdgePart.SheetMetalDocument>(false);

                if (sheetMetalDocument != null)
                {
                    models = sheetMetalDocument.Models;

                    foreach (var model in models.OfType<SolidEdgePart.Model>())
                    {
                        model.Recompute();
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveSheetMetalDocument);
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
