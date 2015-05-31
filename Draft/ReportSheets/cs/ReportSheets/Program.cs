using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportSheets
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sheets sheets = null;

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
                    // Get a reference to the sheets collection.
                    sheets = draftDocument.Sheets;

                    foreach (var sheet in sheets.OfType<SolidEdgeDraft.Sheet>())
                    {
                        Console.WriteLine("Name: {0}", sheet.Name);
                        Console.WriteLine("Index: {0}", sheet.Index);
                        Console.WriteLine("Number: {0}", sheet.Number);
                        Console.WriteLine("SectionType: {0}", sheet.SectionType);
                        Console.WriteLine();
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
