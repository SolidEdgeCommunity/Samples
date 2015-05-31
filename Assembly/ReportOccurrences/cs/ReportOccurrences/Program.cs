using SolidEdgeCommunity;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportOccurrences
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
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
                        // Allocate a new array to hold transform.
                        double[] transform = new double[6];

                        // Get the occurrence transform.
                        occurrence.GetTransform(
                            OriginX: out transform[0],
                            OriginY: out transform[1],
                            OriginZ: out transform[2],
                            AngleX: out transform[3],
                            AngleY: out transform[4],
                            AngleZ: out transform[5]);

                        // Report the occurrence transform.
                        Console.WriteLine("{0} transform:", occurrence.Name);
                        Console.WriteLine("OriginX: {0} (meters)", transform[0]);
                        Console.WriteLine("OriginY: {0} (meters)", transform[1]);
                        Console.WriteLine("OriginZ: {0} (meters)", transform[2]);
                        Console.WriteLine("AngleX: {0} (radians)", transform[3]);
                        Console.WriteLine("AngleY: {0} (radians)", transform[4]);
                        Console.WriteLine("AngleZ: {0} (radians)", transform[5]);
                        Console.WriteLine();

                        // Allocate a new array to hold matrix.
                        Array matrix = Array.CreateInstance(typeof(double), 16);

                        // Get the occurrence matrix.
                        occurrence.GetMatrix(ref matrix);

                        // Convert from System.Array to double[].  double[] is easier to work with.
                        double[] m = matrix.OfType<double>().ToArray();

                        // Report the occurrence matrix.
                        Console.WriteLine("{0} matrix:", occurrence.Name);
                        Console.WriteLine("|{0}, {1}, {2}, {3}|",
                            m[0],
                            m[1],
                            m[2],
                            m[3]);
                        Console.WriteLine("|{0}, {1}, {2}, {3}|",
                            m[4],
                            m[5],
                            m[6],
                            m[7]);
                        Console.WriteLine("|{0}, {1}, {2}, {3}|",
                            m[8],
                            m[9],
                            m[10],
                            m[11]);
                        Console.WriteLine("|{0}, {1}, {2}, {3}|",
                            m[12],
                            m[13],
                            m[14],
                            m[15]);

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
