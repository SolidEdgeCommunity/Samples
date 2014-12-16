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

namespace ExtractStructure
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

                    var rootItem = new OccurrenceItem();
                    rootItem.FileName = assemblyDocument.FullName;

                    foreach (var occurrenceItem in GetOccurrenceItems(assemblyDocument.Occurrences))
                    {
                        rootItem.Occurrence.Add(occurrenceItem);
                    }

                    // There are numerous ways to serialize data. In the following example, I'm
                    // demonstrating using JSON.NET.
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(rootItem, Newtonsoft.Json.Formatting.Indented);
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

        static IEnumerable<OccurrenceItem> GetOccurrenceItems(SolidEdgeAssembly.Occurrences occurrences)
        {
            foreach (SolidEdgeAssembly.Occurrence occurrence in occurrences)
            {
                var occurrenceItem = new OccurrenceItem(occurrence);

                if (occurrence.SubOccurrences != null)
                {
                    foreach (var childOccurrenceItem in GetOccurrenceItems(occurrence.SubOccurrences, occurrenceItem))
                    {
                        occurrenceItem.Occurrence.Add(childOccurrenceItem);
                    }
                }

                yield return occurrenceItem;
            }
        }

        static IEnumerable<OccurrenceItem> GetOccurrenceItems(SolidEdgeAssembly.SubOccurrences subOccurrences, OccurrenceItem parentOccurrenceItem)
        {
            foreach (SolidEdgeAssembly.SubOccurrence subOccurrence in subOccurrences)
            {
                var childOccurrenceItem = new OccurrenceItem(subOccurrence);

                if (subOccurrence.SubOccurrences != null)
                {
                    foreach (var childOccurrenceItem2 in GetOccurrenceItems(subOccurrence.SubOccurrences, childOccurrenceItem))
                    {
                        childOccurrenceItem.Occurrence.Add(childOccurrenceItem2);
                    }
                }

                parentOccurrenceItem.Occurrence.Add(childOccurrenceItem);

                yield return childOccurrenceItem;
            }
        }
    }
}
