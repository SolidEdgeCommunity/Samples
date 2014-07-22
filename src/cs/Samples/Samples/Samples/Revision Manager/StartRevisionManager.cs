using SolidEdgeCommunity; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.Revision_Manager
{
    /// <summary>
    /// Starts a new instance of RevisionManager.
    /// </summary>
    /// <remarks>
    /// Note that the application will terminate when the thread ends.
    /// </remarks>
    class StartRevisionManager
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            RevisionManager.Application application = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = new RevisionManager.Application();

                // Make sure RevisionManager is visible to user.
                application.Visible = 1;
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
