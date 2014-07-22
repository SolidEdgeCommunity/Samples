using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Saves the active 2D window as an image.
    /// </summary>
    class SaveWindowAsImage
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeFramework.SolidEdgeDocument document = null;
            SolidEdgeDraft.SheetWindow sheetWindow = null;

            string[] extensions = { ".jpg", ".bmp", ".tif" };
            Guid guid = Guid.NewGuid();
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true);

                // Make sure user can see the GUI.
                application.Visible = true;

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the Documents collection.
                documents = application.Documents;

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeFramework.SolidEdgeDocument>(false);

                // Make sure we have a document.
                if (document == null)
                {
                    throw new System.Exception(Resources.NoActiveDocument);
                }

                // 2D windows are of type SolidEdgeDraft.SheetWindow.
                sheetWindow = application.ActiveWindow as SolidEdgeDraft.SheetWindow;

                if (sheetWindow != null)
                {
                    // Save each extension.
                    foreach (string extension in extensions)
                    {
                        // File saved to desktop.
                        string filename = System.IO.Path.ChangeExtension(guid.ToString(), extension);
                        filename = System.IO.Path.Combine(folder, filename);

                        double resolution = 1;  // DPI - Larger values have better quality but also lead to larger file.
                        int colorDepth = 24;
                        int width = sheetWindow.UsableWidth;
                        int height = sheetWindow.UsableHeight;

                        // You can specify .bmp (Windows Bitmap), .tif (TIFF), or .jpg (JPEG).
                        sheetWindow.SaveAsImage(
                            FileName: filename,
                            Width: width,
                            Height: height,
                            Resolution: resolution,
                            ColorDepth: colorDepth,
                            ImageQuality: SolidEdgeFramework.SeImageQualityType.seImageQualityHigh,
                            Invert: false);

                        Console.WriteLine("Saved '{0}'.", filename);
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActive2dWindow);
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
