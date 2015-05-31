using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportOccurrenceRelations
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
                        Console.WriteLine("Processing occurrence {0} relations.", occurrence.Name);

                        var relations3d = (SolidEdgeAssembly.Relations3d)occurrence.Relations3d;

                        foreach (var relation3d in relations3d)
                        {
                            // Determine the relation object type.
                            var objectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgeFramework.ObjectType>(relation3d, "Type");

                            switch (objectType)
                            {
                                case SolidEdgeFramework.ObjectType.igAngularRelation3d:
                                    ReportRelation((SolidEdgeAssembly.AngularRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igAxialRelation3d:
                                    ReportRelation((SolidEdgeAssembly.AxialRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igCamFollowerRelation3d:
                                    ReportRelation((SolidEdgeAssembly.CamFollowerRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igCenterPlaneRelation3d:
                                    ReportRelation((SolidEdgeAssembly.CenterPlaneRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igGearRelation3d:
                                    ReportRelation((SolidEdgeAssembly.GearRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igGroundRelation3d:
                                    ReportRelation((SolidEdgeAssembly.GroundRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igPathRelation3d:
                                    ReportRelation((SolidEdgeAssembly.PathRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igPlanarRelation3d:
                                    ReportRelation((SolidEdgeAssembly.PlanarRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igPointRelation3d:
                                    ReportRelation((SolidEdgeAssembly.PointRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igRigidSetRelation3d:
                                    ReportRelation((SolidEdgeAssembly.RigidSetRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.seSegmentAngularRelation3d:
                                    ReportRelation((SolidEdgeAssembly.SegmentAngularRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.seSegmentDirectionRelation3d:
                                    ReportRelation((SolidEdgeAssembly.SegmentDirectionRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.seSegmentDistanceRelation3d:
                                    ReportRelation((SolidEdgeAssembly.SegmentDistanceRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.seSegmentPointRelation3d:
                                    ReportRelation((SolidEdgeAssembly.SegmentPointRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.seSegmentRadiusRelation3d:
                                    ReportRelation((SolidEdgeAssembly.SegmentRadiusRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.seSegmentTangentRelation3d:
                                    ReportRelation((SolidEdgeAssembly.SegmentTangentRelation3d)relation3d, objectType);
                                    break;
                                case SolidEdgeFramework.ObjectType.igTangentRelation3d:
                                    ReportRelation((SolidEdgeAssembly.TangentRelation3d)relation3d, objectType);
                                    break;
                                default:
                                    break;
                            }
                        }

                        Console.WriteLine("----------------------------------");
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

        static void ReportRelation(SolidEdgeAssembly.AngularRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("Angle: {0}", relation3d.Angle);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangedAngle: {0}", relation3d.RangedAngle);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeLow: {0}", relation3d.RangeLow);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.AxialRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("FixedParallelOffset: {0}", relation3d.FixedParallelOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("FixedRotate: {0}", relation3d.FixedRotate);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Offset: {0}", relation3d.Offset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Orientation: {0}", relation3d.Orientation);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("ParallelOffset: {0}", relation3d.ParallelOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeLow: {0}", relation3d.RangeLow);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.CamFollowerRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.CenterPlaneRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.GearRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RatioValue1: {0}", relation3d.RatioValue1);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RatioValue2: {0}", relation3d.RatioValue2);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.GroundRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.PathRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.PlanarRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("FixedOffset: {0}", relation3d.FixedOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("NormalsAligned: {0}", relation3d.NormalsAligned);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Offset: {0}", relation3d.Offset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeLow: {0}", relation3d.RangeLow);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.PointRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeLow: {0}", relation3d.RangeLow);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.RigidSetRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("OccurrenceCount: {0}", relation3d.OccurrenceCount);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.SegmentAngularRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("AngleCounterclockwise: {0}", relation3d.AngleCounterclockwise);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("AngleToPositiveDirection: {0}", relation3d.AngleToPositiveDirection);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.SegmentDirectionRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DirectionType: {0}", relation3d.DirectionType);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.SegmentDistanceRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DistanceType: {0}", relation3d.DistanceType);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.SegmentPointRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.SegmentRadiusRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("Radius: {0}", relation3d.Radius);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.SegmentTangentRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            Console.WriteLine();
        }

        static void ReportRelation(SolidEdgeAssembly.TangentRelation3d relation3d, SolidEdgeFramework.ObjectType objectType)
        {
            Console.WriteLine("Index: {0} ({1})", relation3d.Index, objectType);

            try
            {
                Console.WriteLine("DetailedStatus: {0}", relation3d.DetailedStatus);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("HalfSpacePositive: {0}", relation3d.HalfSpacePositive);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Offset: {0}", relation3d.Offset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangedOffset: {0}", relation3d.RangedOffset);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeHigh: {0}", relation3d.RangeHigh);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("RangeLow: {0}", relation3d.RangeLow);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Status: {0}", relation3d.Status);
            }
            catch
            {
            }

            try
            {
                Console.WriteLine("Suppress: {0}", relation3d.Suppress);
            }
            catch
            {
            }

            Console.WriteLine();
        }
    }
}
