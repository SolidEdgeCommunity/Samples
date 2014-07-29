using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Part
{
    /// <summary>
    /// Sets the ModelingMode of the active part to synchronous.
    /// </summary>
    class SetModelingModeToSynchronous
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active sheetmetal document.
                partDocument = application.GetActiveDocument<SolidEdgePart.PartDocument>(false);

                if (partDocument != null)
                {
                    partDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous;
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
