using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ApiSamples.Samples.SolidEdge
{
    /// <summary>
    /// SolidEdgeFramework.Application extension methods.
    /// </summary>
    static class ApplicationExtensions
    {
        public static T TryActiveDocumentAs<T>(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocument will throw an exception if no document is open.
                if (application.ActiveDocument is T)
                {
                    return (T)application.ActiveDocument;
                }
            }
            catch
            {
            }

            return default(T);
        }

        public static SolidEdgeFramework.DocumentTypeConstants GetActiveDocumentType(this SolidEdgeFramework.Application application)
        {
            // ActiveDocumentType will throw an exception if no document is open.
            return application.ActiveDocumentType;
        }

        public static void StartCommand(this SolidEdgeFramework.Application application, SolidEdgeConstants.AssemblyCommandConstants CommandID)
        {
            application.StartCommand((SolidEdgeFramework.SolidEdgeCommandConstants)CommandID);
        }

        public static void StartCommand(this SolidEdgeFramework.Application application, SolidEdgeConstants.DetailCommandConstants CommandID)
        {
            application.StartCommand((SolidEdgeFramework.SolidEdgeCommandConstants)CommandID);
        }

        public static void StartCommand(this SolidEdgeFramework.Application application, SolidEdgeConstants.PartCommandConstants CommandID)
        {
            application.StartCommand((SolidEdgeFramework.SolidEdgeCommandConstants)CommandID);
        }

        public static void StartCommand(this SolidEdgeFramework.Application application, SolidEdgeConstants.SheetMetalCommandConstants CommandID)
        {
            application.StartCommand((SolidEdgeFramework.SolidEdgeCommandConstants)CommandID);
        }
    }

    /// <summary>
    /// SolidEdgeFramework.Documents extension methods.
    /// </summary>
    static class DocumentsExtensions
    {
        public static SolidEdgeAssembly.AssemblyDocument AddAssemblyDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgeAssembly.AssemblyDocument)documents.Add(ProgId.AssemblyDocument);
        }

        public static SolidEdgeAssembly.AssemblyDocument AddAssemblyDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgeAssembly.AssemblyDocument)documents.Add(ProgId.AssemblyDocument, TemplateDoc);
        }

        public static SolidEdgeDraft.DraftDocument AddDraftDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgeDraft.DraftDocument)documents.Add(ProgId.DraftDocument);
        }

        public static SolidEdgeDraft.DraftDocument AddDraftDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgeDraft.DraftDocument)documents.Add(ProgId.DraftDocument, TemplateDoc);
        }

        public static SolidEdgePart.PartDocument AddPartDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgePart.PartDocument)documents.Add(ProgId.PartDocument);
        }

        public static SolidEdgePart.PartDocument AddPartDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgePart.PartDocument)documents.Add(ProgId.PartDocument, TemplateDoc);
        }

        public static SolidEdgePart.SheetMetalDocument AddSheetMetalDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgePart.SheetMetalDocument)documents.Add(ProgId.SheetMetalDocument);
        }

        public static SolidEdgePart.SheetMetalDocument AddSheetMetalDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgePart.SheetMetalDocument)documents.Add(ProgId.SheetMetalDocument, TemplateDoc);
        }
    }

    /// <summary>
    /// SolidEdgePart.RefPlane extension methods.
    /// </summary>
    static class RefPlanesExtensions
    {
        /// <summary>
        /// Gets the top plane at index 1.
        /// </summary>
        /// <param name="refPlanes"></param>
        /// <returns></returns>
        public static SolidEdgePart.RefPlane GetTopPlane(this SolidEdgePart.RefPlanes refPlanes)
        {
            return refPlanes.Item(1);
        }

        /// <summary>
        /// Gets the right plane at index 2.
        /// </summary>
        /// <param name="refPlanes"></param>
        /// <returns></returns>
        public static SolidEdgePart.RefPlane GetRightPlane(this SolidEdgePart.RefPlanes refPlanes)
        {
            return refPlanes.Item(2);
        }

        /// <summary>
        /// Gets the front plane at index 3.
        /// </summary>
        /// <param name="refPlanes"></param>
        /// <returns></returns>
        public static SolidEdgePart.RefPlane GetFrontPlane(this SolidEdgePart.RefPlanes refPlanes)
        {
            return refPlanes.Item(3);
        }
    }

    static class SheetExtensions
    {
        [DllImport("user32.dll")]
        static extern bool CloseClipboard();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr GetClipboardData(uint format);

        [DllImport("user32.dll")]
        static extern IntPtr GetClipboardOwner();

        [DllImport("user32.dll")]
        static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll")]
        static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("gdi32.dll")]
        static extern bool DeleteEnhMetaFile(IntPtr hemf);

        [DllImport("gdi32.dll")]
        static extern uint GetEnhMetaFileBits(IntPtr hemf, uint cbBuffer, [Out] byte[] lpbBuffer);

        const uint CF_ENHMETAFILE = 14;

        public static void SaveAsEMF(this SolidEdgeDraft.Sheet sheet, string filename)
        {
            try
            {
                // Copy the sheet as an EMF to the windows clipboard.
                sheet.CopyEMFToClipboard();

                if (OpenClipboard(IntPtr.Zero))
                {
                    if (IsClipboardFormatAvailable(CF_ENHMETAFILE))
                    {
                        // Get the handle to the EMF.
                        IntPtr hEMF = GetClipboardData(CF_ENHMETAFILE);

                        // Query the size of the EMF.
                        uint len = GetEnhMetaFileBits(hEMF, 0, null);
                        byte[] rawBytes = new byte[len];

                        // Get all of the bytes of the EMF.
                        GetEnhMetaFileBits(hEMF, len, rawBytes);

                        // Write all of the bytes to a file.
                        File.WriteAllBytes(filename, rawBytes);

                        // Delete the EMF handle.
                        DeleteEnhMetaFile(hEMF);
                    }
                    else
                    {
                        throw new System.Exception("CF_ENHMETAFILE is not available in clipboard.");
                    }
                }
                else
                {
                    throw new System.Exception("Error opening clipboard.");
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                CloseClipboard();
            }
        }
    }
}
