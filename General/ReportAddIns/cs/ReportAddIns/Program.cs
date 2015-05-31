using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportAddIns
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.AddIns addins = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the addins collection.
                addins = application.AddIns;

                // Loop through each addin.
                foreach (var addin in addins.OfType<SolidEdgeFramework.AddIn>())
                {
                    Console.WriteLine("Description: {0}", addin.Description);
                    Console.WriteLine("GUID: {0}", addin.GUID);
                    Console.WriteLine("GuiVersion: {0}", addin.GuiVersion);

                    try
                    {
                        // I've seen the ProgID property throw exceptions. Weird...
                        Console.WriteLine("ProgID: {0}", addin.ProgID);
                    }
                    catch
                    {
                    }

                    Console.WriteLine("Connect: {0}", addin.Connect);
                    Console.WriteLine("Visible: {0}", addin.Visible);
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
