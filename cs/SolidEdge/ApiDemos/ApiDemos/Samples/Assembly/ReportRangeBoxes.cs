using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Assembly
{
    /// <summary>
    /// Reports all occurrences range boxes of the active assembly.
    /// </summary>
    class ReportRangeBoxes
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Occurrences occurrences = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get the active document.
                assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                if (assemblyDocument != null)
                {
                    // Get a reference to the Occurrences collection.
                    occurrences = assemblyDocument.Occurrences;

                    foreach (var occurrence in occurrences.OfType<SolidEdgeAssembly.Occurrence>())
                    {
                        Array MinRangePoint = Array.CreateInstance(typeof(double), 0);
                        Array MaxRangePoint = Array.CreateInstance(typeof(double), 0);
                        occurrence.GetRangeBox(ref MinRangePoint, ref MaxRangePoint);

                        // Convert from System.Array to double[].  double[] is easier to work with.
                        double[] a1 = MinRangePoint.OfType<double>().ToArray();
                        double[] a2 = MaxRangePoint.OfType<double>().ToArray();

                        // Report the occurrence matrix.
                        Console.WriteLine("{0} range box:", occurrence.Name);
                        Console.WriteLine("|MinRangePoint: {0}, {1}, {2}|",
                            a1[0],
                            a1[1],
                            a1[2]);
                        Console.WriteLine("|MaxRangePoint: {0}, {1}, {2}|",
                            a2[0],
                            a2[1],
                            a2[2]);
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveAssemblyDocument);
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
