using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertDrawingViewsTo2D
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sections sections = null;
            SolidEdgeDraft.Section section = null;
            SolidEdgeDraft.SectionSheets sectionSheets = null;
            SolidEdgeDraft.DrawingViews drawingViews = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

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

                    foreach (var sheet in sectionSheets.OfType<SolidEdgeDraft.Sheet>())
                    {
                        // Get a reference to the DrawingViews collection.
                        drawingViews = sheet.DrawingViews;

                        foreach (var drawingView in drawingViews.OfType<SolidEdgeDraft.DrawingView>())
                        {
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
                    throw new System.Exception("No active document.");
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
