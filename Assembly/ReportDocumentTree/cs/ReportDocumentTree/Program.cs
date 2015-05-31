using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportDocumentTree
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;

            try
            {
                OleMessageFilter.Register();
                application = SolidEdgeUtils.Connect();
                var assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                // Optional settings you may tweak for performance improvements. Results may vary.
                application.DelayCompute = true;
                application.DisplayAlerts = false;
                application.Interactive = false;
                application.ScreenUpdating = false;

                if (assemblyDocument != null)
                {
                    var rootItem = new DocumentItem();
                    rootItem.FileName = assemblyDocument.FullName;

                    // Begin the recurisve extraction process.
                    PopulateDocumentItems(assemblyDocument.Occurrences, rootItem);

                    // Write each DocumentItem to console.
                    foreach (var documentItem in rootItem.AllDocumentItems)
                    {
                        Console.WriteLine(documentItem.FileName);
                    }

                    // Demonstration of how to save the BOM to various formats.

                    // Convert the document items to JSON.
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (application != null)
                {
                    application.DelayCompute = false;
                    application.DisplayAlerts = true;
                    application.Interactive = true;
                    application.ScreenUpdating = true;
                }

                OleMessageFilter.Unregister();
            }
        }

        static void PopulateDocumentItems(SolidEdgeAssembly.Occurrences occurrences, DocumentItem parentItem)
        {
            foreach (SolidEdgeAssembly.Occurrence occurrence in occurrences)
            {
                var occurrenceItem = new DocumentItem(occurrence);

                // Make sure the DocumentItem hasn't already been added.
                if (parentItem.DocumentItems.Contains(occurrenceItem) == false)
                {
                    parentItem.DocumentItems.Add(occurrenceItem);

                    if (occurrence.SubOccurrences != null)
                    {
                        PopulateDocumentItems(occurrence.SubOccurrences, occurrenceItem);
                    }
                }
            }
        }

        static void PopulateDocumentItems(SolidEdgeAssembly.SubOccurrences subOccurrences, DocumentItem parentItem)
        {
            foreach (SolidEdgeAssembly.SubOccurrence subOccurrence in subOccurrences)
            {
                var occurrenceItem = new DocumentItem(subOccurrence);

                // Make sure the DocumentItem hasn't already been added.
                if (parentItem.DocumentItems.Contains(occurrenceItem) == false)
                {
                    parentItem.DocumentItems.Add(occurrenceItem);

                    if (subOccurrence.SubOccurrences != null)
                    {
                        PopulateDocumentItems(subOccurrence.SubOccurrences, occurrenceItem);
                    }
                }
            }
        }
    }
}
