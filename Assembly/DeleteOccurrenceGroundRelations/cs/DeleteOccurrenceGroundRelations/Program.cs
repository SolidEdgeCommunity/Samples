using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeleteOccurrenceGroundRelations
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active assembly document.
                var document = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (document != null)
                {
                    // Get a reference to the occurrences collection.
                    var occurrences = document.Occurrences;

                    foreach (var occurrence in occurrences.OfType<SolidEdgeAssembly.Occurrence>())
                    {
                        Console.WriteLine("Processing occurrence {0}.", occurrence.Name);

                        var relations3d = (SolidEdgeAssembly.Relations3d)occurrence.Relations3d;
                        var groundRelations3d = relations3d.OfType<SolidEdgeAssembly.GroundRelation3d>();

                        foreach (var groundRelation3d in groundRelations3d)
                        {
                            Console.WriteLine("Found and deleted ground relationship at index {0}.", groundRelation3d.Index);
                            groundRelation3d.Delete();
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
