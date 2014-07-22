using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Reports all sheets of the active draft.
    /// </summary>
    class ReportSheets
    {
        internal static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sheets sheets = null;
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
                    // Get a reference to the sheets collection.
                    sheets = draftDocument.Sheets;

                    for (int i = 1; i <= sheets.Count; i++)
                    {
                        sheet = sheets.Item(i);

                        Console.WriteLine("Name: {0}", sheet.Name);
                        Console.WriteLine("Index: {0}", sheet.Index);
                        Console.WriteLine("Number: {0}", sheet.Number);
                        Console.WriteLine("SectionType: {0}", sheet.SectionType);
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
