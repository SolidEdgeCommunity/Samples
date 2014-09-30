using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Application
{
    /// <summary>
    /// Adds objects from the active document to the document select set.
    /// </summary>
    class AddObjectsToSelectSet
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.SolidEdgeDocument document = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.AsmRefPlanes asmRefPlanes = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeDraft.DrawingViews drawingViews = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.EdgebarFeatures edgeBarFeatures = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Start();

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeFramework.SolidEdgeDocument>(false);

                if (document != null)
                {
                    // Determine document type.
                    switch (document.Type)
                    {
                        case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                            assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)document;
                            asmRefPlanes = assemblyDocument.AsmRefPlanes;
                            selectSet = assemblyDocument.SelectSet;

                            for (int i = 1; i <= asmRefPlanes.Count; i++)
                            {
                                selectSet.Add(asmRefPlanes.Item(i));
                            }

                            break;
                        case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument:
                            draftDocument = (SolidEdgeDraft.DraftDocument)document;
                            sheet = draftDocument.ActiveSheet;
                            drawingViews = sheet.DrawingViews;
                            selectSet = draftDocument.SelectSet;

                            for (int i = 1; i <= drawingViews.Count; i++)
                            {
                                draftDocument.SelectSet.Add(drawingViews.Item(i));
                            }

                            break;
                        case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                            partDocument = (SolidEdgePart.PartDocument)document;
                            edgeBarFeatures = partDocument.DesignEdgebarFeatures;
                            selectSet = partDocument.SelectSet;

                            for (int i = 1; i <= edgeBarFeatures.Count; i++)
                            {
                                partDocument.SelectSet.Add(edgeBarFeatures.Item(i));
                            }

                            break;
                        case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument:
                            sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)document;
                            edgeBarFeatures = sheetMetalDocument.DesignEdgebarFeatures;
                            selectSet = sheetMetalDocument.SelectSet;

                            for (int i = 1; i <= edgeBarFeatures.Count; i++)
                            {
                                partDocument.SelectSet.Add(edgeBarFeatures.Item(i));
                            }
                            break;
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveDocument);
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
