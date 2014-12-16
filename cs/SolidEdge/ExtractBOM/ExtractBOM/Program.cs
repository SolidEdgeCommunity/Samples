using Newtonsoft.Json;
using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ExtractBOM
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
                    // Stopwatch class is good for performance testing.
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    var rootItem = new BomItem();
                    rootItem.FileName = assemblyDocument.FullName;

                    foreach (var bomItem in GetBomItems(assemblyDocument))
                    {
                        rootItem.Occurrence.Add(bomItem);
                    }

                    // There are numerous ways to serialize data. In the following example, I'm
                    // demonstrating using JSON.NET.
                    var jsonSettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented, jsonSettings);
                    var xml = Newtonsoft.Json.JsonConvert.DeserializeXNode(json, "Root").ToString();

                    stopwatch.Stop();
                    var elapsedTime = stopwatch.Elapsed;
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

        static IEnumerable<BomItem> GetBomItems(SolidEdgeAssembly.AssemblyDocument assemblyDocument)
        {
            foreach (SolidEdgeAssembly.Occurrence occurrence in assemblyDocument.Occurrences)
            {
                BomItem bomItem = new BomItem(occurrence);

                // Filter out certain occurrences.
                if (!occurrence.IncludeInBom) { continue; }
                if (occurrence.IsPatternItem) { continue; }

                if (occurrence.FileMissing() == false)
                {
                    if (occurrence.OccurrenceDocument == null) { continue; }

                    // Get a reference to the SolidEdgeDocument.
                    SolidEdgeFramework.SolidEdgeDocument document = (SolidEdgeFramework.SolidEdgeDocument)occurrence.OccurrenceDocument;

                    if (occurrence.Subassembly)
                    {
                        var occurrenceAssemblyDocument = (SolidEdgeAssembly.AssemblyDocument)occurrence.OccurrenceDocument;
                        foreach (var childBomItem in GetBomItems(occurrenceAssemblyDocument))
                        {
                            bomItem.Occurrence.Add(childBomItem);
                        }
                    }
                }
                else
                {
                    bomItem.Missing = true;
                }

                yield return bomItem;
            }
        }
    }
}
