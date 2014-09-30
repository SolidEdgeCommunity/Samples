using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Part
{
    /// <summary>
    /// Saves the active 3D window as an image.
    /// </summary>
    class SaveWindowAsImage
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgeFramework.Window window = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Start();

                // Get a reference to the active document.
                partDocument = application.GetActiveDocument<SolidEdgePart.PartDocument>(false);

                // Make sure we have a document.
                if (partDocument != null)
                {
                    // 3D windows are of type SolidEdgeFramework.Window.
                    window = application.ActiveWindow as SolidEdgeFramework.Window;

                    if (window != null)
                    {
                        WindowHelper.SaveAsImage(window);
                    }
                    else
                    {
                        throw new System.Exception(Resources.NoActive3dWindow);
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveSheetMetalDocument);
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
