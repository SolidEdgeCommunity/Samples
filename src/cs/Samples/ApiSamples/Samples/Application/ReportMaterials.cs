using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Application
{
    /// <summary>
    /// Reports all of the materials defined in the material table.
    /// </summary>
    class ReportMaterials
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.MatTable matTable = null;
            object varMatLibName = null;
            int plNumMaterials = 0;
            object listOfMaterials = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the material table.
                matTable = application.GetMaterialTable();

                // Get the path to Material.mtl.
                matTable.GetMatLibFileName(out varMatLibName);

                matTable.GetMaterialList(out plNumMaterials, out listOfMaterials);

                object[] materialArray = (object[])listOfMaterials;

                Console.WriteLine("Material Table: '{0}'.", varMatLibName);

                // Loop through and report each material.
                foreach (string material in materialArray)
                {
                    Console.WriteLine(material);
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
