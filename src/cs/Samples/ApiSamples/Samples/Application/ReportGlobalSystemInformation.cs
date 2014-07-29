using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Application
{
    /// <summary>
    /// Reports the Solid Edge global system information.
    /// </summary>
    class ReportGlobalSystemInformation
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            object val = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get the global system information global parameter.
                application.GetGlobalParameter(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo, ref val);

                Console.WriteLine(val);
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
