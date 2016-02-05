using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectUnderConstrainedOccurrences
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Occurrences occurrences = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect();

                // Get a reference to the active selectset.
                selectSet = application.ActiveSelectSet;

                // Temporarily suspend selectset UI updates.
                selectSet.SuspendDisplay();

                // Clear the selectset.
                selectSet.RemoveAll();

                // Get a reference to the active document.
                assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (assemblyDocument != null)
                {
                    // Get a reference to the occurrences collection.
                    occurrences = assemblyDocument.Occurrences;

                    // Loop through the occurrences.
                    foreach (var occurrence in occurrences.OfType<SolidEdgeAssembly.Occurrence>())
                    {
                        // If status equals seOccurrenceStatusUnderDefined, add to selectset.
                        if (occurrence.Status == SolidEdgeAssembly.OccurrenceStatusConstants.seOccurrenceStatusUnderDefined)
                        {
                            selectSet.Add(occurrence);
                        }
                    }
                }
                else
                {
                    throw new System.Exception("No active document");
                }

                // Re-enable selectset UI display.
                selectSet.ResumeDisplay();

                // Manually refresh the selectset UI display.
                selectSet.RefreshDisplay();
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
