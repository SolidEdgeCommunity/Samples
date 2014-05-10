using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Assembly
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
            SolidEdgeAssembly.Configuration configuration = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = ApplicationHelper.Connect(true, true);

                // Get a reference to active assembly document.
                assemblyDocument = application.TryActiveDocumentAs<SolidEdgeAssembly.AssemblyDocument>();

                if (assemblyDocument != null)
                {
                    // Get a reference tot he Configurations collection.
                    configurations = assemblyDocument.Configurations;

                    // Iterate through all of the configurations.
                    for (int i = 1; i <= configurations.Count; i++)
                    {
                        // Get the configuration at the specified index.
                        configuration = configurations.Item(i);

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
                OleMessageFilter.Unregister();
            }
        }
    }
}
