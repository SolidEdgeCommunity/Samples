using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace ApiSamples.Samples.SolidEdge
{
    class ApplicationHelper
    {
        /// <summary>
        /// Creates and returns a new instance of Solid Edge.
        /// </summary>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Start()
        {
            // On a system where Solid Edge is installed, the COM ProgID will be
            // defined in registry: HKEY_CLASSES_ROOT\SolidEdge.Application
            Type t = Type.GetTypeFromProgID(ProgId.Application, throwOnError: true);

            // Using the discovered Type, create and return a new instance of Solid Edge.
            return (SolidEdgeFramework.Application)Activator.CreateInstance(type: t);
        }

        /// <summary>
        /// Connects to a running instance of Solid Edge.
        /// </summary>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Connect()
        {
            return Connect(startIfNotRunning: false);
        }

        /// <summary>
        /// Connects to or starts a new instance of Solid Edge.
        /// </summary>
        /// <param name="startIfNotRunning"></param>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Connect(bool startIfNotRunning)
        {
            try
            {
                // Attempt to connect to a running instance of Solid Edge.
                return (SolidEdgeFramework.Application)Marshal.GetActiveObject(ProgId.Application);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                switch (ex.ErrorCode)
                {
                    // Solid Edge is not running.
                    case HRESULT.MK_E_UNAVAILABLE:
                        if (startIfNotRunning)
                        {
                            // Start Solid Edge.
                            return Start();
                        }
                        else
                        {
                            // Rethrow exception.
                            throw;
                        }
                    default:
                        // Rethrow exception.
                        throw;
                }
            }
            catch
            {
                // Rethrow exception.
                throw;
            }
        }

        /// <summary>
        /// Connects to or starts a new instance of Solid Edge.
        /// </summary>
        /// <param name="startIfNotRunning"></param>
        /// <param name="ensureVisible"></param>
        /// <returns>
        /// An object of type SolidEdgeFramework.Application.
        /// </returns>
        public static SolidEdgeFramework.Application Connect(bool startIfNotRunning, bool ensureVisible)
        {
            SolidEdgeFramework.Application application = null;

            try
            {
                // Attempt to connect to a running instance of Solid Edge.
                application = (SolidEdgeFramework.Application)Marshal.GetActiveObject(ProgId.Application);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                switch (ex.ErrorCode)
                {
                    // Solid Edge is not running.
                    case HRESULT.MK_E_UNAVAILABLE:
                        if (startIfNotRunning)
                        {
                            // Start Solid Edge.
                            application = Start();
                            break;
                        }
                        else
                        {
                            // Rethrow exception.
                            throw;
                        }
                    default:
                        // Rethrow exception.
                        throw;
                }
            }
            catch
            {
                // Rethrow exception.
                throw;
            }

            if ((application != null) && (ensureVisible))
            {
                application.Visible = true;
            }

            return application;
        }
    }

    class InstallDataHelper
    {
        internal static string GetInstalledPath()
        {
            /* Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. */
            DirectoryInfo programDirectory = new DirectoryInfo(GetProgramFolderPath());

            /* Get path to Solid Edge installation directory.  Typically, 'C:\Program Files\Solid Edge XXX'. */
            DirectoryInfo installationDirectory = programDirectory.Parent;

            return installationDirectory.FullName;
        }

        internal static string GetProgramFolderPath()
        {
            SEInstallDataLib.SEInstallData installData = new SEInstallDataLib.SEInstallData();

            /* Get path to Solid Edge program directory.  Typically, 'C:\Program Files\Solid Edge XXX\Program'. */
            return installData.GetInstalledPath();
        }

        internal static string GetTrainingFolderPath()
        {
            /* Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'. */
            DirectoryInfo trainingDirectory = new DirectoryInfo(Path.Combine(GetInstalledPath(), "Training"));

            return trainingDirectory.FullName;
        }
    }

    static class ReflectionHelper
    {
        /// <summary>
        /// Returns the Solid Edge object type by invoking the 'Name' property.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetPropertyValueAsString(object o, string propertyName)
        {
            // Using .NET reflection, attempt to obtain the Name value.
            var val = o.GetType().InvokeMember(propertyName, BindingFlags.GetProperty, null, o, null);

            return val.ToString();
        }

        /// <summary>
        /// Returns the Solid Edge object type by invoking the 'Type' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>SolidEdgeFramework.ObjectType</returns>
        public static SolidEdgeFramework.ObjectType GetObjectType(object o)
        {
            // Using .NET reflection, attempt to obtain the Type value.
            var val = o.GetType().InvokeMember("Type", BindingFlags.GetProperty, null, o, null);

            return (SolidEdgeFramework.ObjectType)val;
        }

        /// <summary>
        /// Returns the Solid Edge Part feature modeling mode by invoking the 'ModelingModeType' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>SolidEdgePart.ModelingModeConstants</returns>
        public static SolidEdgePart.ModelingModeConstants GetPartFeatureModelingMode(object o)
        {
            // Using .NET reflection, attempt to obtain the ModelingModeType value.
            var val = o.GetType().InvokeMember("ModelingModeType", BindingFlags.GetProperty, null, o, null);

            return (SolidEdgePart.ModelingModeConstants)val;
        }

        /// <summary>
        /// Returns the Solid Edge Part feature type by invoking the 'Type' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>SolidEdgePart.FeatureTypeConstants</returns>
        public static SolidEdgePart.FeatureTypeConstants GetPartFeatureType(object o)
        {
            // Using .NET reflection, attempt to obtain the Type value.
            var val = o.GetType().InvokeMember("Type", BindingFlags.GetProperty, null, o, null);

            return (SolidEdgePart.FeatureTypeConstants)val;
        }

        /// <summary>
        /// Returns the Solid Edge value of the object by invoking the 'Value' property.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object GetObjectValue(object o)
        {
            // Using .NET reflection, attempt to obtain the Value value.
            return o.GetType().InvokeMember("Value", BindingFlags.GetProperty, null, o, null);
        }
    }

    class SolidEdgeDocumentHelper
    {
        public static void SaveAsJT(SolidEdgeAssembly.AssemblyDocument document)
        {
            SaveAsJT((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void SaveAsJT(SolidEdgePart.PartDocument document)
        {
            SaveAsJT((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void SaveAsJT(SolidEdgePart.SheetMetalDocument document)
        {
            SaveAsJT((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void SaveAsJT(SolidEdgePart.WeldmentDocument document)
        {
            SaveAsJT((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void SaveAsJT(SolidEdgeFramework.SolidEdgeDocument document)
        {
            // Note: Some of the parameters are obvious by their name but we need to work on getting better descriptions for some.
            var NewName = String.Empty;
            var Include_PreciseGeom = 0;
            var Prod_Structure_Option = 1;
            var Export_PMI = 0;
            var Export_CoordinateSystem = 0;
            var Export_3DBodies = 0;
            var NumberofLODs = 1;
            var JTFileUnit = 0;
            var Write_Which_Files = 1;
            var Use_Simplified_TopAsm = 0;
            var Use_Simplified_SubAsm = 0;
            var Use_Simplified_Part = 0;
            var EnableDefaultOutputPath = 0;
            var IncludeSEProperties = 0;
            var Export_VisiblePartsOnly = 0;
            var Export_VisibleConstructionsOnly = 0;
            var RemoveUnsafeCharacters = 0;
            var ExportSEPartFileAsSingleJTFile = 0;

            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            switch (document.Type)
            {
                case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument:
                case SolidEdgeFramework.DocumentTypeConstants.igWeldmentAssemblyDocument:
                case SolidEdgeFramework.DocumentTypeConstants.igWeldmentDocument:
                    NewName = Path.ChangeExtension(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), document.Name), ".jt");
                    document.SaveAsJT(
                                    NewName,
                                    Include_PreciseGeom,
                                    Prod_Structure_Option,
                                    Export_PMI,
                                    Export_CoordinateSystem,
                                    Export_3DBodies,
                                    NumberofLODs,
                                    JTFileUnit,
                                    Write_Which_Files,
                                    Use_Simplified_TopAsm,
                                    Use_Simplified_SubAsm,
                                    Use_Simplified_Part,
                                    EnableDefaultOutputPath,
                                    IncludeSEProperties,
                                    Export_VisiblePartsOnly,
                                    Export_VisibleConstructionsOnly,
                                    RemoveUnsafeCharacters,
                                    ExportSEPartFileAsSingleJTFile);
                    break;
                default:
                    throw new System.Exception(String.Format("'{0}' cannot be converted to JT.", document.Type));
            }
        }
    }

    class VariablesHelper
    {
        public static void ReportVariables(SolidEdgeAssembly.AssemblyDocument document)
        {
            ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void ReportVariables(SolidEdgeDraft.DraftDocument document)
        {
            ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void ReportVariables(SolidEdgePart.PartDocument document)
        {
            ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void ReportVariables(SolidEdgePart.SheetMetalDocument document)
        {
            ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void ReportVariables(SolidEdgePart.WeldmentDocument document)
        {
            ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
        }

        public static void ReportVariables(SolidEdgeFramework.SolidEdgeDocument document)
        {
            SolidEdgeFramework.Variables variables = null;
            SolidEdgeFramework.VariableList variableList = null;
            SolidEdgeFramework.variable variable = null;
            SolidEdgeFrameworkSupport.Dimension dimension = null;
            object variableListItem = null;

            if (document == null) throw new ArgumentNullException("document");

            // Get a reference to the Variables collection.
            variables = (SolidEdgeFramework.Variables)document.Variables;

            // Get a reference to the variablelist.
            variableList = (SolidEdgeFramework.VariableList)variables.Query(
                pFindCriterium: "*",
                NamedBy: SolidEdgeConstants.VariableNameBy.seVariableNameByBoth,
                VarType: SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth);

            // Process variables.
            for (int i = 1; i <= variableList.Count; i++)
            {
                // Get a reference to variable item.
                variableListItem = variableList.Item(i);

                // Use ReflectionHelper class to get the object type.
                SolidEdgeFramework.ObjectType objectType = ReflectionHelper.GetObjectType(variableListItem);

                // Process the specific variable item type.
                switch (objectType)
                {
                    case SolidEdgeFramework.ObjectType.igDimension:
                        // Get a reference to the dimension.
                        dimension = (SolidEdgeFrameworkSupport.Dimension)variableListItem;
                        Console.WriteLine("Dimension: '{0}' = '{1}' ({2})", dimension.DisplayName, dimension.Value, objectType);
                        break;
                    case SolidEdgeFramework.ObjectType.igVariable:
                        variable = (SolidEdgeFramework.variable)variableListItem;
                        Console.WriteLine("Variable: '{0}' = '{1}' ({2})", variable.DisplayName, variable.Value, objectType);
                        break;
                    default:
                        // Other SolidEdgeConstants.ObjectType's may exist.
                        break;
                }
            }
        }
    }

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
        struct RECT
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
}
