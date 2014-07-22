using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    class ReportSections
    {
        internal static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sections sections = null;
            SolidEdgeDraft.Section section = null;
            SolidEdgeDraft.SectionSheets sectionSheets = null;
            SolidEdgeDraft.Sheet sheet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Start();

                // Get a reference to the active document.
                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

                // Make sure we have a document.
                if (draftDocument != null)
                {
                    // Get a reference to the sections collection.
                    sections = draftDocument.Sections;

                    for (int i = 1; i <= sections.Count; i++)
                    {
                        section = sections.Item(i);

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

                        for (int j = 1; j <= sectionSheets.Count; j++)
                        {
                            sheet = sectionSheets.Item(j);

                            Console.WriteLine("SectionSheets[{0}]: {1}", j, sheet.Name);
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
                OleMessageFilter.Unregister();
            }
        }
    }
}
