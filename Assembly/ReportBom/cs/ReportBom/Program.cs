using Newtonsoft.Json;
using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportBom
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

                // Connect to a running instance of Solid Edge.
                application = SolidEdgeUtils.Connect();

                // Connect to the active assembly document.
                var assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                // Optional settings you may tweak for performance improvements. Results may vary.
                application.DelayCompute = true;
                application.DisplayAlerts = false;
                application.Interactive = false;
                application.ScreenUpdating = false;

                if (assemblyDocument != null)
                {
                    var rootBomItem = new BomItem();
                    rootBomItem.FileName = assemblyDocument.FullName;

                    // Begin the recurisve extraction process.
                    PopulateBom(0, assemblyDocument, rootBomItem);

                    // Write each BomItem to console.
                    foreach (var bomItem in rootBomItem.AllChildren)
                    {
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", bomItem.Level, bomItem.DocumentNumber, bomItem.Revision, bomItem.Title, bomItem.Quantity);
                    }

                    // Demonstration of how to save the BOM to various formats.

                    // Define the Json serializer settings.
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };

                    // Convert the BOM to JSON.
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(rootBomItem, Newtonsoft.Json.Formatting.Indented, jsonSettings);
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

        static void PopulateBom(int level, SolidEdgeAssembly.AssemblyDocument assemblyDocument, BomItem parentBomItem)
        {
            // Increment level (depth).
            level++;

            // This sample BOM is not exploded. Define a dictionary to store unique occurrences.
            Dictionary<string, SolidEdgeAssembly.Occurrence> uniqueOccurrences = new Dictionary<string, SolidEdgeAssembly.Occurrence>();

            // Populate the unique occurrences dictionary.
            foreach (SolidEdgeAssembly.Occurrence occurrence in assemblyDocument.Occurrences)
            {
                // To make sure nothing silly happens with our dictionary key, force the file path to lowercase.
                var lowerFileName = occurrence.OccurrenceFileName.ToLower();

                // If the dictionary does not already contain the occurrence, add it.
                if (uniqueOccurrences.ContainsKey(lowerFileName) == false)
                {
                    uniqueOccurrences.Add(lowerFileName, occurrence);
                }
            }

            // Loop through the unique occurrences.
            foreach (SolidEdgeAssembly.Occurrence occurrence in uniqueOccurrences.Values.ToArray())
            {
                // Filter out certain occurrences.
                if (!occurrence.IncludeInBom) { continue; }
                if (occurrence.IsPatternItem) { continue; }
                if (occurrence.OccurrenceDocument == null) { continue; }

                // Create an instance of the child BomItem.
                var bomItem = new BomItem(occurrence, level);

                // Add the child BomItem to the parent.
                parentBomItem.Children.Add(bomItem);

                if (bomItem.IsSubassembly == true)
                {
                    // Sub Assembly. Recurisve call to drill down.
                    PopulateBom(level, (SolidEdgeAssembly.AssemblyDocument)occurrence.OccurrenceDocument, bomItem);
                }
            }
        }
    }
}
