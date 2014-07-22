using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Assembly
{
    /// <summary>
    /// Saves the active 3D window as an image.
    /// </summary>
    class SaveAsImage
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break(); 
            
            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument document = null;
            SolidEdgeFramework.Window window = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Start();

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                // Make sure we have a document.
                if (document != null)
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
