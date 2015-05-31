using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportDrawingViews
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sections sections = null;
            SolidEdgeDraft.Section section = null;
            SolidEdgeDraft.SectionSheets sectionSheets = null;
            SolidEdgeDraft.DrawingViews drawingViews = null;
            SolidEdgeDraft.ModelMembers modelMembers = null;
            SolidEdgeDraft.GraphicMembers graphicMembers = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(false);

                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>(false);

                if (draftDocument != null)
                {
                    // Get a reference to the Sections collection.
                    sections = draftDocument.Sections;

                    // Get a reference to the WorkingSection.
                    section = sections.WorkingSection;

                    // Get a reference to the Sheets collection.
                    sectionSheets = section.Sheets;

                    foreach (var sheet in sectionSheets.OfType<SolidEdgeDraft.Sheet>())
                    {
                        Console.WriteLine();
                        Console.WriteLine("Processing sheet '{0}'.", sheet.Name);

                        // Get a reference to the DrawingViews collection.
                        drawingViews = sheet.DrawingViews;

                        foreach (var drawingView in drawingViews.OfType<SolidEdgeDraft.DrawingView>())
                        {
                            Console.WriteLine();
                            Console.WriteLine("Processing drawing view '{0}'.", drawingView.Name);

                            double xOrigin = 0;
                            double yOrigin = 0;
                            drawingView.GetOrigin(out xOrigin, out yOrigin);

                            Console.WriteLine("Origin: x={0} y={1}.", xOrigin, yOrigin);

                            // Get a reference to the ModelMembers collection.
                            modelMembers = drawingView.ModelMembers;

                            foreach (var modelMember in modelMembers.OfType<SolidEdgeDraft.ModelMember>())
                            {
                                Console.WriteLine("Processing model member '{0}'.", modelMember.FileName);
                                Console.WriteLine("ComponentType: '{0}'.", modelMember.ComponentType);
                                Console.WriteLine("DisplayType: '{0}'.", modelMember.DisplayType);
                                Console.WriteLine("Type: '{0}'.", modelMember.Type);
                            }

                            // Get a reference to the ModelMembers collection.
                            graphicMembers = drawingView.GraphicMembers;

                            ReportGraphicMembers(graphicMembers);
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

        static void ReportGraphicMembers(SolidEdgeDraft.GraphicMembers graphicMembers)
        {
            // This is a good example of how to deal with collections that contain different types.
            foreach (var graphicMember in graphicMembers.OfType<object>())
            {
                Console.WriteLine();
                var graphicMemberType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(graphicMember);

                double xKeypoint;
                double yKeypoint;
                double zKeypoint;
                SolidEdgeFramework.KeyPointType keypointType;
                int handleType;

                if (graphicMemberType.Equals(typeof(SolidEdgeDraft.DVLine2d)))
                {
                    var line2d = graphicMember as SolidEdgeDraft.DVLine2d;
                    Console.WriteLine("Processing graphic member '{0}' ({1}).", line2d.Name, graphicMemberType);
                    Console.WriteLine("Angle: {0}.", line2d.Angle);
                    Console.WriteLine("Length: {0}.", line2d.Length);

                    for (int i = 1; i <= line2d.KeyPointCount; i++)
                    {
                        line2d.GetKeyPoint(i, out xKeypoint, out yKeypoint, out zKeypoint, out keypointType, out handleType);
                        Console.WriteLine("Keypoint {0}: x={1} y={2} z={3} type={4} handle={5}", i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType);
                    }
                }
                else if (graphicMemberType.Equals(typeof(SolidEdgeDraft.DVArc2d)))
                {
                    var arc2d = graphicMember as SolidEdgeDraft.DVArc2d;
                    Console.WriteLine("Processing graphic member '{0}' ({1}).", arc2d.Name, graphicMemberType);
                    Console.WriteLine("BendRadius: {0}.", arc2d.BendRadius);
                    Console.WriteLine("Radius: {0}.", arc2d.Radius);
                    Console.WriteLine("StartAngle: {0}.", arc2d.StartAngle);
                    Console.WriteLine("SweepAngle: {0}.", arc2d.SweepAngle);

                    for (int i = 1; i <= arc2d.KeyPointCount; i++)
                    {
                        arc2d.GetKeyPoint(i, out xKeypoint, out yKeypoint, out zKeypoint, out keypointType, out handleType);
                        Console.WriteLine("Keypoint {0}: x={1} y={2} z={3} type={4} handle={5}", i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType);
                    }
                }
                else if (graphicMemberType.Equals(typeof(SolidEdgeDraft.DVCircle2d)))
                {
                    var circle2d = graphicMember as SolidEdgeDraft.DVCircle2d;
                    Console.WriteLine("Processing graphic member '{0}' ({1}).", circle2d.Name, graphicMemberType);
                    Console.WriteLine("Area: {0}.", circle2d.Area);
                    Console.WriteLine("BendAngle: {0}.", circle2d.BendAngle);
                    Console.WriteLine("BendRadius: {0}.", circle2d.BendRadius);
                    Console.WriteLine("Circumference: {0}.", circle2d.Circumference);
                    Console.WriteLine("Diameter: {0}.", circle2d.Diameter);

                    for (int i = 1; i <= circle2d.KeyPointCount; i++)
                    {
                        circle2d.GetKeyPoint(i, out xKeypoint, out yKeypoint, out zKeypoint, out keypointType, out handleType);
                        Console.WriteLine("Keypoint {0}: x={1} y={2} z={3} type={4} handle={5}", i, xKeypoint, yKeypoint, zKeypoint, keypointType, handleType);
                    }
                }
            }
        }
    }
}
