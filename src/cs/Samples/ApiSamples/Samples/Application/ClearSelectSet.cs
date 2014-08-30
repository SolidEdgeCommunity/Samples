using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Application
{
    /// <summary>
    /// Clears the application active select set.
    /// </summary>
    class ClearSelectSet
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Start();

                // Get a reference to the active select set.
                selectSet = application.ActiveSelectSet;

                // Clear the SelectSet.
                selectSet.RemoveAll();
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
