using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Part
{
    /// <summary>
    /// Heals and optimizes all models in the active part.
    /// </summary>
    class HealAndOptimizeBody
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;

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
                    models = partDocument.Models;

                    for (int i = 1; i <= models.Count; i++)
                    {
                        model = models.Item(i);
                        model.HealAndOptimizeBody(true, true);
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
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
