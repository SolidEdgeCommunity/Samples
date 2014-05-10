using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge
{
    /// <summary>
    /// Reports all of the Solid Edge environments.
    /// </summary>
    class ReportEnvironments
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Environments environments = null;
            SolidEdgeFramework.Environment environment = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = ApplicationHelper.Connect(true, true);

                // Get a reference to the Environments collection.
                environments = application.Environments;

                // Loop through each addin.
                for (int i = 1; i <= environments.Count; i++)
                {
                    environment = environments.Item(i);

                    Console.WriteLine("Caption: {0}", environment.Caption);
                    Console.WriteLine("CATID: {0}", environment.CATID);
                    Console.WriteLine("Index: {0}", environment.Index);
                    Console.WriteLine("Name: {0}", environment.Name);
                    Console.WriteLine("SubTypeName: {0}", environment.SubTypeName);
                    Console.WriteLine();
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
