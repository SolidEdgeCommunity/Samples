using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Part
{
    /// <summary>
    /// Saves the active 3D window as an image using the BitBlt Win32 function.
    /// </summary>
    class SaveWindowAsImageUsingBitBlt
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
                OleMessageFilter.Register();

                // Connect to Solid Edge.
                application = ApplicationHelper.Connect();

                // Get a reference to the active document.
                partDocument = application.TryActiveDocumentAs<SolidEdgePart.PartDocument>();

                // Make sure we have a document.
                if (partDocument != null)
                {
                    // 3D windows are of type SolidEdgeFramework.Window.
                    window = application.ActiveWindow as SolidEdgeFramework.Window;

                    if (window != null)
                    {
                        WindowHelper.SaveAsImageUsingBitBlt(window);
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
                OleMessageFilter.Unregister();
            }
        }
    }
}
