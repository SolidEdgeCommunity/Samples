using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Draft
{
    /// <summary>
    /// Updates all drawing views in the working section of the active draft.
    /// </summary>
    class UpdateAllDrawingViewsInWorkingSection
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
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(false);

                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

                if (draftDocument != null)
                {
                    // Get a reference to the Sections collection.
                    sections = draftDocument.Sections;

                    // Get a reference to the WorkingSection.
                    section = sections.WorkingSection;

                    // Get a reference to the Sheets collection.
                    sectionSheets = section.Sheets;

                    for (int i = 1; i <= sectionSheets.Count; i++)
                    {
                        sheet = sectionSheets.Item(i);

                        // Get a reference to the DrawingViews collection.
                        drawingViews = sheet.DrawingViews;

                        for (int j = 1; j < drawingViews.Count; j++)
                        {
                            drawingView = drawingViews.Item(j);

                            // Updates an out-of-date drawing view.
                            drawingView.Update();

                            // Updates the drawing view even if it is not out-of-date.
                            //drawingView.ForceUpdate();
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
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
