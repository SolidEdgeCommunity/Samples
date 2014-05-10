using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.SheetMetal
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
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to Solid Edge.
                application = ApplicationHelper.Connect(true, true);

                // Get a reference to the active document.
                sheetMetalDocument = application.TryActiveDocumentAs<SolidEdgePart.SheetMetalDocument>();

                if (sheetMetalDocument != null)
                {
                    SolidEdgeDocumentHelper.SaveAsJT(sheetMetalDocument);
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
