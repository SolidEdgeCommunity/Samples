using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiDemos.Assembly
{
    /// <summary>
    /// Creates a new configuration in the active assembly.
    /// </summary>
    class AddDerivedConfiguration
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument document = null;
            SolidEdgeAssembly.Configurations configurations = null;
            SolidEdgeAssembly.Configuration configuration = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active assembly document.
                document = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (document != null)
                {
                    // Get a reference tot he Configurations collection.
                    configurations = document.Configurations;

                    // Configuration name has to be unique so for demonstration
                    // purposes, use a random number.
                    Random random = new Random();
                    string configName = String.Format("Configuration {0}", random.Next());

                    configuration = configurations.Item(1);

                    object configList = new string[] { configuration.Name };

                    object missing = Missing.Value;

                    // NOTE: Not sure why but this is causing Solid Edge ST6 to crash.
                    // Add the new configuration.
                    configuration = configurations.AddDerivedConfig(1, 0, 0, ref configList, ref missing, ref missing, configName);
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
