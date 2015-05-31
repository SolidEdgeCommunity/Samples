using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportActiveSelectSet
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect();

                // Get a reference to the active select set.
                selectSet = application.ActiveSelectSet;

                if (selectSet.Count > 0)
                {
                    // Loop through the items and report each one.
                    for (int i = 1; i <= selectSet.Count; i++)
                    {
                        // Get a reference to the item.
                        object item = selectSet.Item(i);

                        // Get the managed type.
                        var type = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(item);

                        Console.WriteLine("Item({0}) is of type '{1}'", i, type);

                        ReportItem(item, type);

                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("SelectSet is empty.");
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

        static void ReportItem(object item, Type type)
        {
            // Once we know the type, we can cast accordingly.
            // Obviously not all types are handled here. Here are some examples.

            if (type.Equals(typeof(SolidEdgeAssembly.Occurrence)))
            {
                ReportItem((SolidEdgeAssembly.Occurrence)item);
            }
            else if (type.Equals(typeof(SolidEdgeDraft.DrawingView)))
            {
                ReportItem((SolidEdgeDraft.DrawingView)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.ExtrudedCutout)))
            {
                ReportItem((SolidEdgePart.ExtrudedCutout)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.ExtrudedProtrusion)))
            {
                ReportItem((SolidEdgePart.ExtrudedProtrusion)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.FeatureGroup)))
            {
                ReportItem((SolidEdgePart.FeatureGroup)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.Flange)))
            {
                ReportItem((SolidEdgePart.Flange)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.Hole)))
            {
                ReportItem((SolidEdgePart.Hole)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.Round)))
            {
                ReportItem((SolidEdgePart.Round)item);
            }
            else if (type.Equals(typeof(SolidEdgePart.Sketch)))
            {
                ReportItem((SolidEdgePart.Sketch)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.Arc2d)))
            {
                ReportItem((SolidEdgeFrameworkSupport.Arc2d)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.CenterMark)))
            {
                ReportItem((SolidEdgeFrameworkSupport.CenterMark)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.Circle2d)))
            {
                ReportItem((SolidEdgeFrameworkSupport.Circle2d)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.DatumFrame)))
            {
                ReportItem((SolidEdgeFrameworkSupport.DatumFrame)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.Dimension)))
            {
                ReportItem((SolidEdgeFrameworkSupport.Dimension)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.Line2d)))
            {
                ReportItem((SolidEdgeFrameworkSupport.Line2d)item);
            }
            else if (type.Equals(typeof(SolidEdgeFrameworkSupport.TextBox)))
            {
                ReportItem((SolidEdgeFrameworkSupport.TextBox)item);
            }
        }

        static void ReportItem(SolidEdgeAssembly.Occurrence occurrence)
        {
            Console.WriteLine("Name: {0}", occurrence.Name);
            Console.WriteLine("OccurrenceFileName: {0}", occurrence.OccurrenceFileName);
        }

        static void ReportItem(SolidEdgeDraft.DrawingView drawingView)
        {
            Console.WriteLine("Name: {0}", drawingView.Name);
        }

        static void ReportItem(SolidEdgePart.ExtrudedCutout extrudedCutout)
        {
            Console.WriteLine("DisplayName: {0}", extrudedCutout.DisplayName);
            Console.WriteLine("EdgebarName: {0}", extrudedCutout.EdgebarName);
            Console.WriteLine("Name: {0}", extrudedCutout.Name);
            Console.WriteLine("SystemName: {0}", extrudedCutout.SystemName);
        }

        static void ReportItem(SolidEdgePart.ExtrudedProtrusion extrudedProtrusion)
        {
            Console.WriteLine("DisplayName: {0}", extrudedProtrusion.DisplayName);
            Console.WriteLine("EdgebarName: {0}", extrudedProtrusion.EdgebarName);
            Console.WriteLine("Name: {0}", extrudedProtrusion.Name);
            Console.WriteLine("SystemName: {0}", extrudedProtrusion.SystemName);
        }

        static void ReportItem(SolidEdgePart.FeatureGroup featureGroup)
        {
            Console.WriteLine("DisplayName: {0}", featureGroup.DisplayName);
            Console.WriteLine("EdgebarName: {0}", featureGroup.EdgebarName);
            Console.WriteLine("Name: {0}", featureGroup.Name);
            Console.WriteLine("SystemName: {0}", featureGroup.SystemName);
        }

        static void ReportItem(SolidEdgePart.Flange flange)
        {
            Console.WriteLine("DisplayName: {0}", flange.DisplayName);
            Console.WriteLine("EdgebarName: {0}", flange.EdgebarName);
            Console.WriteLine("Name: {0}", flange.Name);
            Console.WriteLine("SystemName: {0}", flange.SystemName);
        }

        static void ReportItem(SolidEdgePart.Hole hole)
        {
            Console.WriteLine("DisplayName: {0}", hole.DisplayName);
            Console.WriteLine("EdgebarName: {0}", hole.EdgebarName);
            Console.WriteLine("Name: {0}", hole.Name);
            Console.WriteLine("SystemName: {0}", hole.SystemName);
        }

        static void ReportItem(SolidEdgePart.Round round)
        {
            Console.WriteLine("DisplayName: {0}", round.DisplayName);
            Console.WriteLine("EdgebarName: {0}", round.EdgebarName);
            Console.WriteLine("Name: {0}", round.Name);
            Console.WriteLine("SystemName: {0}", round.SystemName);
        }

        static void ReportItem(SolidEdgePart.Sketch sketch)
        {
            Console.WriteLine("Name: {0}", sketch.Name);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.Arc2d arc2d)
        {
            Console.WriteLine("Name: {0}", arc2d.Name);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.CenterMark centerMark)
        {
            Console.WriteLine("Name: {0}", centerMark.Name);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.Circle2d circle2d)
        {
            Console.WriteLine("Name: {0}", circle2d.Name);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.DatumFrame datumFrame)
        {
            Console.WriteLine("Name: {0}", datumFrame.Name);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.Dimension dimension)
        {
            Console.WriteLine("DisplayName: {0}", dimension.DisplayName);
            Console.WriteLine("ExposeName: {0}", dimension.ExposeName);
            Console.WriteLine("Name: {0}", dimension.Name);
            Console.WriteLine("SystemName: {0}", dimension.SystemName);
            Console.WriteLine("VariableTableName: {0}", dimension.VariableTableName);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.Line2d line2d)
        {
            Console.WriteLine("Name: {0}", line2d.Name);
        }

        static void ReportItem(SolidEdgeFrameworkSupport.TextBox textBox)
        {
            Console.WriteLine("Name: {0}", textBox.Name);
        }
    }
}
