using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AddOccurrencesWithTransform
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
            string[] filenames = { "strainer.asm", "handle.par" };

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

                // Get a reference to the Occurrences collection.
                occurrences = assemblyDocument.Occurrences;

                // Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
                DirectoryInfo trainingDirectory = new DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath());

                // Add each occurrence in array.
                for (int i = 0; i < transforms.Length; i++)
                {
                    // Build path to file.
                    string filename = Path.Combine(trainingDirectory.FullName, filenames[i]);

                    // Add the new occurrence using a transform.
                    occurrence = occurrences.AddWithTransform(
                        OccurrenceFileName: filename,
                        OriginX: transforms[i][0],
                        OriginY: transforms[i][1],
                        OriginZ: transforms[i][2],
                        AngleX: transforms[i][3],
                        AngleY: transforms[i][4],
                        AngleZ: transforms[i][5]);

                }

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
