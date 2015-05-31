using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateStructuralFrame
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.LineSegments lineSegments = null;
            SolidEdgeAssembly.LineSegment lineSegment = null;
            List<SolidEdgeAssembly.LineSegment> lineSegmentList = new List<SolidEdgeAssembly.LineSegment>();
            SolidEdgeAssembly.StructuralFrames structuralFrames = null;
            SolidEdgeAssembly.StructuralFrame structuralFrame = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            Array startPointArray = new double[] { 0.0, 0.0, 0.0 };
            Array endPointArray = new double[] { 0.0, 0.0, 0.5 };

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

                // Get a reference to the LineSegments collection.
                lineSegments = assemblyDocument.LineSegments;

                // Add a new line segment.
                lineSegment = lineSegments.Add(
                    StartPoint: ref startPointArray,
                    EndPoint: ref endPointArray);

                // Store line segment in array.
                lineSegmentList.Add(lineSegment);

                // Get a reference to the StructuralFrames collection.
                structuralFrames = assemblyDocument.StructuralFrames;

                // Build path to part file.  In this case, it is a .par from standard install.
                string filename = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetInstalledPath(), @"Frames\DIN\I-Beam\I-Beam 80x46.par");

                // Add new structural frame.
                structuralFrame = structuralFrames.Add(
                    PartFileName: filename,
                    NumPaths: lineSegmentList.Count,
                    Path: lineSegmentList.ToArray());

                // Close the Frame environment.
                application.StartCommand(SolidEdgeConstants.AssemblyCommandConstants.AssemblyEnvironmentsExit);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Add the StructuralFrame to the select set.
                selectSet.Add(structuralFrame);

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
