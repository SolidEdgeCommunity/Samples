using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertPropertyText
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sections sections = null;

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

                    // Convert property text on all sheets in background section.
                    ConvertSection(sections.BackgroundSection);

                    // Convert property text on all sheets in working section.
                    ConvertSection(sections.WorkingSection);
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

        static void ConvertSection(SolidEdgeDraft.Section section)
        {
            SolidEdgeDraft.SectionSheets sectionSheets = null;
            SolidEdgeFrameworkSupport.Balloons balloons = null;

            // Get a reference to the section sheets collection.
            sectionSheets = section.Sheets;

            // Process all of the sheets in the section.
            foreach (var sheet in sectionSheets.OfType<SolidEdgeDraft.Sheet>())
            {
                // Get a reference to the section balloons collection.
                balloons = (SolidEdgeFrameworkSupport.Balloons)sheet.Balloons;

                // Process all of the balloons in the sheet.
                foreach (var balloon in balloons.OfType<SolidEdgeFrameworkSupport.Balloon>())
                {
                    // If BalloonText has a formula, it will be overriden with BalloonDisplayedText.
                    balloon.BalloonText = balloon.BalloonDisplayedText;
                }

                // Note: There may be other controls that need to be updated...
            }
        }
    }
}
