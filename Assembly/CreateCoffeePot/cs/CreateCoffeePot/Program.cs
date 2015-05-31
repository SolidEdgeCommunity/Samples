using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CreateCoffeePot
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Occurrences occurrences = null;
            SolidEdgeAssembly.Occurrence occurrence = null;
            string[] transformFilenames = { "strainer.asm", "handle.par" };

            // A single-dimension array that defines a valid transformation matrix. 
            double[] matrix = 
                {
                    1.0,
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                    1.0,
                    0.0,
                    0.0,
                    0.0,
                    0.0,
                    1.0,
                    0.0,
                    0.0,
                    0.0,
                    0.07913,
                    1.0
                };

            // Jagged array
            // {OriginX, OriginY, OriginZ, AngleX, AngleY, AngleZ}
            // Origin in meters.
            // Angle in radians.
            double[][] transforms = new double[][] 
                {
                    new double[] {0, 0, 0.02062, 0, 0, 0},
                    new double[] {-0.06943, -0.00996, 0.05697, 0, 0, 0},
                };

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new assembly document.
                assemblyDocument = documents.AddAssemblyDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
                DirectoryInfo trainingDirectory = new DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath());

                // Build path to file.
                string filename = System.IO.Path.Combine(trainingDirectory.FullName, "Coffee Pot.par");

                // Get a reference to the occurrences collection.
                occurrences = assemblyDocument.Occurrences;

                // Add the base feature at 0 (X), 0 (Y), 0 (Z).
                occurrences.AddByFilename(
                    OccurrenceFileName: filename
                    );

                // Add each occurrence in array.
                for (int i = 0; i < transforms.Length; i++)
                {
                    // Build path to file.
                    filename = Path.Combine(trainingDirectory.FullName, transformFilenames[i]);

                    // Add the new occurrence using a transform.
                    occurrence = occurrences.AddWithTransform(
                        OccurrenceFileName: filename,
                        OriginX: transforms[i][0],
                        OriginY: transforms[i][1],
                        OriginZ: transforms[i][2],
                        AngleX: transforms[i][3],
                        AngleY: transforms[i][4],
                        AngleZ: transforms[i][5]
                        );
                }

                // Build path to part file.
                filename = System.IO.Path.Combine(trainingDirectory.FullName, "Strap Handle.par");

                // Convert from double[] to System.Array.
                Array matrixArray = matrix;

                // Add the new occurrence using a matrix.
                occurrences.AddWithMatrix(
                    OccurrenceFileName: filename,
                    Matrix: ref matrixArray
                    );

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyViewISOView);
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
