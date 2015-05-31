using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class WindowHelper
{
    public const int SRCCOPY = 0x00CC0020;

    [DllImport("gdi32.dll")]
    static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

    [DllImport("gdi32.dll")]
    static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

    [DllImport("gdi32.dll")]
    static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    static extern bool DeleteDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    [DllImport("User32.dll")]
    extern static IntPtr GetDC(IntPtr hWnd);

    [DllImport("User32.dll")]
    extern static int ReleaseDC(System.IntPtr hWnd, System.IntPtr hDC);

    [DllImport("user32.dll")]
    static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public static void SaveAsImage(SolidEdgeFramework.Window window)
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
            string filename = Path.ChangeExtension(guid.ToString(), extension);
            filename = Path.Combine(folder, filename);

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

    public static void SaveAsImageUsingBitBlt(SolidEdgeFramework.Window window)
    {
        System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
        dialog.FileName = Path.ChangeExtension(window.Caption, ".bmp");
        dialog.Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpeg)|*.jpeg|PNG (.png)|*.png|TIFF (.tiff)|*.tiff|WMF Image (.wmf)|*.wmf";
        dialog.FilterIndex = 1;

        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            IntPtr handle = new IntPtr(window.DrawHwnd);

            // Capture the window to an Image object.
            using (Image image = Capture(handle))
            {
                ImageFormat imageFormat = default(ImageFormat);

                // Determine the selected image format.
                // The index is 1-based.
                switch (dialog.FilterIndex)
                {
                    case 1:
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case 2:
                        imageFormat = ImageFormat.Gif;
                        break;
                    case 3:
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case 4:
                        imageFormat = ImageFormat.Png;
                        break;
                    case 5:
                        imageFormat = ImageFormat.Tiff;
                        break;
                    case 6:
                        imageFormat = ImageFormat.Wmf;
                        break;
                }

                Console.WriteLine("Saving {0}.", dialog.FileName);

                image.Save(dialog.FileName, imageFormat);
            }
        }
    }

    public static Image Capture(IntPtr hWnd)
    {
        // Get the device context of the client.
        IntPtr hdcSrc = GetDC(hWnd);

        // Get the client rectangle.
        RECT windowRect = new RECT();
        GetClientRect(hWnd, out windowRect);

        // Calculate the size of the window.
        int width = windowRect.right - windowRect.left;
        int height = windowRect.bottom - windowRect.top;

        // Create a new device context that is compatible with the source device context.
        IntPtr hdcDest = CreateCompatibleDC(hdcSrc);

        // Creates a bitmap compatible with the device that is associated with the specified device context.
        IntPtr hBitmap = CreateCompatibleBitmap(hdcSrc, width, height);

        // Select the new bitmap object into the destination device context.
        IntPtr hOld = SelectObject(hdcDest, hBitmap);

        // Perform a bit-block transfer of the color data corresponding to a rectangle of pixels from the specified source device context into a destination device context.
        BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);

        // Restore selection.
        SelectObject(hdcDest, hOld);

        // Clean up device contexts.
        DeleteDC(hdcDest);
        ReleaseDC(hWnd, hdcSrc);

        // Create an Image from a handle to a GDI bitmap.
        Image image = Image.FromHbitmap(hBitmap);

        // Free up the Bitmap object.
        DeleteObject(hBitmap);

        return image;
    }
}