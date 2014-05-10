using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdge.Common
{
    /// <summary>
    /// SolidEdgeFramework.Application extension methods.
    /// </summary>
    public static class ApplicationExtensions
    {
        public static SolidEdgeAssembly.AssemblyDocument GetActiveAssemblyDocument(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocument will throw an exception if no document is open.
                // If cast fails method will return null;
                return application.ActiveDocument as SolidEdgeAssembly.AssemblyDocument;
            }
            catch
            {
            }

            return null;
        }

        public static SolidEdgeFramework.SolidEdgeDocument GetActiveDocument(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocument will throw an exception if no document is open.
                return (SolidEdgeFramework.SolidEdgeDocument)application.ActiveDocument;
            }
            catch
            {
            }

            return null;
        }

        public static SolidEdgeDraft.DraftDocument GetActiveDraftDocument(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocument will throw an exception if no document is open.
                // If cast fails method will return null;
                return application.ActiveDocument as SolidEdgeDraft.DraftDocument;
            }
            catch
            {
            }

            return null;
        }

        public static SolidEdgePart.PartDocument GetActivePartDocument(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocument will throw an exception if no document is open.
                // If cast fails method will return null;
                return application.ActiveDocument as SolidEdgePart.PartDocument;
            }
            catch
            {
            }

            return null;
        }

        public static SolidEdgePart.SheetMetalDocument GetActiveSheetMetalDocument(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocument will throw an exception if no document is open.
                // If cast fails method will return null;
                return application.ActiveDocument as SolidEdgePart.SheetMetalDocument;
            }
            catch
            {
            }

            return null;
        }

        public static SolidEdgeFramework.DocumentTypeConstants GetActiveDocumentType(this SolidEdgeFramework.Application application)
        {
            try
            {
                // ActiveDocumentType will throw an exception if no document is open.
                return application.ActiveDocumentType;
            }
            catch
            {
            }

            return SolidEdgeFramework.DocumentTypeConstants.igUnknownDocument;
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
    public static class DocumentsExtensions
    {
        public static SolidEdgeAssembly.AssemblyDocument AddAssemblyDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgeAssembly.AssemblyDocument)documents.Add(SolidEdge.Common.ProgIDs.AssemblyDocument);
        }

        public static SolidEdgeAssembly.AssemblyDocument AddAssemblyDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgeAssembly.AssemblyDocument)documents.Add(SolidEdge.Common.ProgIDs.AssemblyDocument, TemplateDoc);
        }

        public static SolidEdgeDraft.DraftDocument AddDraftDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgeDraft.DraftDocument)documents.Add(SolidEdge.Common.ProgIDs.DraftDocument);
        }

        public static SolidEdgeDraft.DraftDocument AddDraftDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgeDraft.DraftDocument)documents.Add(SolidEdge.Common.ProgIDs.DraftDocument, TemplateDoc);
        }

        public static SolidEdgePart.PartDocument AddPartDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgePart.PartDocument)documents.Add(SolidEdge.Common.ProgIDs.PartDocument);
        }

        public static SolidEdgePart.PartDocument AddPartDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgePart.PartDocument)documents.Add(SolidEdge.Common.ProgIDs.PartDocument, TemplateDoc);
        }

        public static SolidEdgePart.SheetMetalDocument AddSheetMetalDocument(this SolidEdgeFramework.Documents documents)
        {
            return (SolidEdgePart.SheetMetalDocument)documents.Add(SolidEdge.Common.ProgIDs.SheetMetalDocument);
        }

        public static SolidEdgePart.SheetMetalDocument AddSheetMetalDocument(this SolidEdgeFramework.Documents documents, object TemplateDoc)
        {
            return (SolidEdgePart.SheetMetalDocument)documents.Add(SolidEdge.Common.ProgIDs.SheetMetalDocument, TemplateDoc);
        }
    }

    public static class MouseExtensions
    {
        public static void AddToLocateFilter(this SolidEdgeFramework.Mouse mouse, SolidEdgeConstants.seLocateFilterConstants filter)
        {
            mouse.AddToLocateFilter((int)filter);
        }

        public static void SetLocateMode(this SolidEdgeFramework.Mouse mouse, SolidEdgeConstants.seLocateModes mode)
        {
            mouse.LocateMode = (int)mode;
        }

        public static SolidEdgeConstants.seLocateModes GetLocateMode(this SolidEdgeFramework.Mouse mouse, SolidEdgeConstants.seLocateModes mode)
        {
            return (SolidEdgeConstants.seLocateModes)mouse.LocateMode;
        }
    }

    /// <summary>
    /// SolidEdgePart.RefPlane extension methods.
    /// </summary>
    public static class RefPlanesExtensions
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
}
