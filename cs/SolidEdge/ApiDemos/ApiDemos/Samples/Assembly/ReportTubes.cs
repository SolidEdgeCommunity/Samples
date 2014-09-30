using SolidEdgeCommunity.Extensions; // Enabled extension methods from SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiDemos.Assembly
{
    /// <summary>
    /// Reports all occurrences of the active assembly that are tubes.
    /// </summary>
    /// <remarks>
    /// If an active assembly is not open, an assembly from the training folder will be used.
    /// </remarks>
    class ReportTubes
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeAssembly.AssemblyDocument assemblyDocument = null;
            SolidEdgeAssembly.Occurrences occurrences = null;
            SolidEdgeAssembly.Tube tube = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                try
                {
                    // Get the active document.
                    assemblyDocument = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);
                }
                catch
                {
                }

                // If there is not an active assembly, we can use an assembly from the training folder.
                if (assemblyDocument == null)
                {
                    documents = application.Documents;

                    // Build path to part file.
                    string filename = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath(), @"Try It\zone_try_it.asm");
                    assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)documents.Open(filename);
                }

                if (assemblyDocument != null)
                {
                    // Get a reference to the Occurrences collection.
                    occurrences = assemblyDocument.Occurrences;

                    foreach (var occurrence in occurrences.OfType<SolidEdgeAssembly.Occurrence>())
                    {
                        if (occurrence.IsTube())
                        {
                            tube = occurrence.GetTube();

                            Console.WriteLine("Occurrences[{0}] is a tube.", occurrence.Index);
                            Console.WriteLine("PartFileName: {0}", tube.PartFileName);

                            object CutLength = 0.0;
                            object NumOfBends = 0;
                            object FeedLengthArray = new double[] { };
                            object RotationAngleArray = new double[] { };
                            object BendRadiusArray = new double[] { };
                            object ReverseBendOrder = 0;
                            object SaveToFileName = Missing.Value;
                            object BendAngleArray = new double[] { };

                            tube.BendTable(
                                CutLength: out CutLength,
                                NumOfBends: out NumOfBends,
                                FeedLength: out FeedLengthArray,
                                RotationAngle: out RotationAngleArray,
                                BendRadius: out BendRadiusArray,
                                ReverseBendOrder: ref ReverseBendOrder,
                                SaveToFileName: SaveToFileName,
                                BendAngle: out BendAngleArray);

                            StringBuilder FeedLength = new StringBuilder();
                            foreach (double d in (double[])FeedLengthArray)
                            {
                                FeedLength.AppendFormat("{0}, ", d);
                            }

                            StringBuilder RotationAngle = new StringBuilder();
                            foreach (double d in (double[])RotationAngleArray)
                            {
                                RotationAngle.AppendFormat("{0}, ", d);
                            }

                            StringBuilder BendRadius = new StringBuilder();
                            foreach (double d in (double[])BendRadiusArray)
                            {
                                BendRadius.AppendFormat("{0}, ", d);
                            }

                            StringBuilder BendAngle = new StringBuilder();
                            foreach (double d in (double[])BendAngleArray)
                            {
                                BendAngle.AppendFormat("{0}, ", d);
                            }

                            Console.WriteLine("BendTable information:");
                            Console.WriteLine("CutLength: {0}", CutLength);
                            Console.WriteLine("NumOfBends: {0}", NumOfBends);
                            Console.WriteLine("FeedLength: {0}", FeedLength.ToString().Trim().TrimEnd(','));
                            Console.WriteLine("RotationAngle: {0}", RotationAngle.ToString().Trim().TrimEnd(','));
                            Console.WriteLine("BendRadius: {0}", BendRadius.ToString().Trim().TrimEnd(','));
                            Console.WriteLine("BendAngle: {0}", BendAngle.ToString().Trim().TrimEnd(','));
                            Console.WriteLine();
                        }
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
