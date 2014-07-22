using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Converts all drawing views of the active draft to 2D views.
    /// </summary>
    class ConvertDrawingViewsTo2D
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sections sections = null;
            SolidEdgeDraft.Section section = null;
            SolidEdgeDraft.SectionSheets sectionSheets = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeDraft.DrawingViews drawingViews = null;
            SolidEdgeDraft.DrawingView drawingView = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the active draft document.
                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

                if (draftDocument != null)
                {
                    // Get a reference to the Sections collection.
                    sections = draftDocument.Sections;

                    // Get a reference to the working section.
                    section = sections.WorkingSection;

                    // Get a reference to the working section sheets.
                    sectionSheets = section.Sheets;

                    for (int i = 1; i <= sectionSheets.Count; i++)
                    {
                        // Get a reference to the sheet.
                        sheet = sectionSheets.Item(i);

                        // Get a reference to the DrawingViews collection.
                        drawingViews = sheet.DrawingViews;

                        for (int j = 1; j < drawingViews.Count; j++)
                        {
                            drawingView = drawingViews.Item(j);

                            // DrawingView's of type igUserView cannot be converted.
                            if (drawingView.DrawingViewType != SolidEdgeDraft.DrawingViewTypeConstants.igUserView)
                            {
                                // Converts the current DrawingView to an igUserView type containing simple geometry
                                // and disassociates the drawing view from the source 3d Model.
                                drawingView.Drop();
                            }
                        }
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveDraftDocument);
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
