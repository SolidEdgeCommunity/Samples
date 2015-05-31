using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportEnvironments
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Environments environments = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the Environments collection.
                environments = application.Environments;

                // Loop through each addin.
                foreach (var environment in environments.OfType<SolidEdgeFramework.Environment>())
                {
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
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
