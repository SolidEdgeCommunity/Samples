using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Draft
{
    class ReportSections
    {
        internal static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sections sections = null;
            SolidEdgeDraft.SectionSheets sectionSheets = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active document.
                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

                // Make sure we have a document.
                if (draftDocument != null)
                {
                    // Get a reference to the sections collection.
                    sections = draftDocument.Sections;

                    foreach (var section in sections.OfType<SolidEdgeDraft.Section>())
                    {
                        Console.WriteLine("Name: {0}", section.Name);

                        try
                        {
                            // Index may throw an exception.
                            Console.WriteLine("Index: {0}", section.Index);
                        }
                        catch
                        {
                        }

                        sectionSheets = section.Sheets;

                        foreach (var sheet in sectionSheets.OfType<SolidEdgeDraft.Sheet>())
                        {
                            Console.WriteLine("SectionSheets[{0}]: {1}", sheet.Index, sheet.Name);
                        }

                        Console.WriteLine();
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
