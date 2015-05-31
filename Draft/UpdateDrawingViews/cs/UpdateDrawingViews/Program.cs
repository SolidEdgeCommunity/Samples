using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateDrawingViews
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
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(false);

                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

                if (draftDocument != null)
                {
                    // Get a reference to the Sections collection.
                    sections = draftDocument.Sections;

                    // Get a reference to the WorkingSection.
                    section = sections.WorkingSection;

                    // Get a reference to the Sheets collection.
                    sectionSheets = section.Sheets;

                    foreach (var sheet in sectionSheets.OfType<SolidEdgeDraft.Sheet>())
                    {
                        Console.WriteLine("Processing sheet '{0}'.", sheet.Name);

                        // Get a reference to the DrawingViews collection.
                        drawingViews = sheet.DrawingViews;

                        foreach (var drawingView in drawingViews.OfType<SolidEdgeDraft.DrawingView>())
                        {
                            // Updates an out-of-date drawing view.
                            drawingView.Update();

                            // Note: You can use ForceUpdate() even if it is not out-of-date.

                            Console.WriteLine("Updated drawing view '{0}'.", drawingView.Name);
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
