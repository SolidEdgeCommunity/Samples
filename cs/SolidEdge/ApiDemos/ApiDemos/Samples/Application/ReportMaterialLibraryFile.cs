using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiDemos.Application
{
    /// <summary>
    /// Reports information about the Material.mtl XML file.
    /// </summary>
    class ReportMaterialLibraryFile
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.MatTable matTable = null;
            object varMatLibName = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the material table.
                matTable = application.GetMaterialTable();

                // Get the path to Material.mtl.
                matTable.GetMatLibFileName(out varMatLibName);

                // Convert raw path string into FileInfo object.
                FileInfo fileInfo = new FileInfo(varMatLibName.ToString());

                if (fileInfo.Exists)
                {
                    Console.WriteLine("'{0}' found.", fileInfo.FullName);
                }
                else
                {
                    Console.WriteLine("'{0}' does not exist.", fileInfo.FullName);
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
