using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveWindowAsImage
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Window window = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Start();

                // 3D windows are of type SolidEdgeFramework.Window.
                window = application.ActiveWindow as SolidEdgeFramework.Window;

                if (window != null)
                {
                    string[] extensions = { ".jpg", ".bmp", ".tif" };

                    SolidEdgeFramework.View view = null;
                    Guid guid = Guid.NewGuid();
                    string folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                    if (window == null) throw new ArgumentNullException("window");

                    // Get a reference to the 3D view.
                    view = window.View;

                    // Save each extension.
                    foreach (string extension in extensions)
                    {
                        // File saved to desktop.
                        string filename = System.IO.Path.ChangeExtension(guid.ToString(), extension);
                        filename = System.IO.Path.Combine(folder, filename);

                        double resolution = 1.0;  // DPI - Larger values have better quality but also lead to larger file.
                        int colorDepth = 24;
                        int width = window.UsableWidth;
                        int height = window.UsableHeight;

                        // You can specify .bmp (Windows Bitmap), .tif (TIFF), or .jpg (JPEG).
                        view.SaveAsImage(
                            Filename: filename,
                            Width: width,
                            Height: height,
                            AltViewStyle: null,
                            Resolution: resolution,
                            ColorDepth: colorDepth,
                            ImageQuality: SolidEdgeFramework.SeImageQualityType.seImageQualityHigh,
                            Invert: false);

                        Console.WriteLine("Saved '{0}'.", filename);
                    }
                }
                else
                {
                    throw new System.Exception("No active 3D window.");
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
