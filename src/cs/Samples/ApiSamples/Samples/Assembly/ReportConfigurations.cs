using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Assembly
{
    /// <summary>
    /// Reports all configurations of the active assembly.
    /// </summary>
    class ReportConfigurations
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Configurations configurations = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to active assembly document.
                assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (assemblyDocument != null)
                {
                    // Get a reference tot he Configurations collection.
                    configurations = assemblyDocument.Configurations;

                    // Iterate through all of the configurations.
                    foreach (var configuration in configurations.OfType<SolidEdgeAssembly.Configuration>())
                    {
                        Console.WriteLine("Configuration Name: '{0}' | Configuration Type: {1}.", configuration.Name, configuration.ConfigurationType);
                    }
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
