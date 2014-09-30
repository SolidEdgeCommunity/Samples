using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.SheetMetal
{
    /// <summary>
    /// Recomputes the active sheetmetal.
    /// </summary>
    class Recompute
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active sheetmetal document.
                sheetMetalDocument = application.GetActiveDocument<SolidEdgePart.SheetMetalDocument>();

                if (sheetMetalDocument != null)
                {
                    sheetMetalDocument.Recompute();
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
