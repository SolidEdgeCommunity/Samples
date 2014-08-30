using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Assembly
{
    /// <summary>
    /// Reports the variables of the active assembly.
    /// </summary>
    class ReportVariables
    {
        internal static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument document = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Start();

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                // Make sure we have a document.
                if (document != null)
                {
                    VariablesHelper.ReportVariables(document);
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveAssemblyDocument);
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
