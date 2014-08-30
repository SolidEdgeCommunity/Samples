using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Assembly
{
    /// <summary>
    /// Creates and applies a new configuration to the active assembly.
    /// </summary>
    class AddNewConfigurationAndApply
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Configurations configurations = null;
            SolidEdgeAssembly.Configuration configuration = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a refrence to the active assembly document.
                assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (assemblyDocument != null)
                {
                    // Get a reference tot he Configurations collection.
                    configurations = assemblyDocument.Configurations;

                    // Configuration name has to be unique so for demonstration
                    // purposes, use a random number.
                    Random random = new Random();
                    string configName = String.Format("Configuration {0}", random.Next());

                    // Add the new configuration.
                    configuration = configurations.Add(configName);

                    // Make the new configuration the active configuration.
                    configuration.Apply();
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
