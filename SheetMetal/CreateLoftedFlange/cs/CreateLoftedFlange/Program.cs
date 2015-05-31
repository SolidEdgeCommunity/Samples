using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateLoftedFlange
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Sketchs sketches = null;
            SolidEdgePart.Sketch sketch = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile1 = null;
            SolidEdgePart.Profile profile2 = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgeFrameworkSupport.Lines2d lines2d = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            List<object> crossSections = new List<object>();
            List<object> crossSectionTypes = new List<object>();
            List<object> origins = new List<object>();
            List<object> originRefs = new List<object>();
            double bendRadius = 0.001;
            double neutralFactor = 0.33;
            SolidEdgeFramework.SelectSet selectSet = null;
            SolidEdgePart.LoftedFlanges loftedFlanges = null;
            SolidEdgePart.LoftedFlange loftedFlange = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to Solid Edge,
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new sheetmetal document.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Get a reference to the Sketches collection.
                sketches = sheetMetalDocument.Sketches;

                // Get a reference to the RefPlanes collection.
                refPlanes = sheetMetalDocument.RefPlanes;

                // Get a reference to top RefPlane.
                refPlane = refPlanes.GetTopPlane();

                // Create new sketch.
                sketch = sketches.Add();

                // Get a reference to the Profiles collection.
                profiles = sketch.Profiles;

                // Create new profile.
                profile1 = profiles.Add(refPlanes.Item(1));

                // Get a reference to the Lines2d collection.
                lines2d = profile1.Lines2d;

                // Multidimensional array
                double[,] points = new double[,]
                    {
                        { 0.0, 0.0, 0.1, 0.2 },
                        { 0.1, 0.2, 0.3, 0.2 },
                        { 0.3, 0.2, 0.4, 0.0 }
                    };

                for (int i = 0; i <= points.GetUpperBound(0); i++)
                {
                    lines2d.AddBy2Points(points[i, 0], points[i, 1], points[i, 2], points[i, 3]);
                }

                origins.Add(lines2d.Item(1));

                // Create Reference Plane Parallel to "XY" Plane at 1Meter distance.
                refPlane = refPlanes.AddParallelByDistance(
                    ParentPlane: refPlanes.Item(1),
                    Distance: 1,
                    NormalSide: SolidEdgePart.ReferenceElementConstants.igReverseNormalSide);

                // Create new sketch.
                sketch = sketches.Add();

                // Get a reference to the Profiles collection.
                profiles = sketch.Profiles;

                // Create new profile.
                profile2 = profiles.Add(refPlane);

                // Get a reference to the Lines2d collection.
                lines2d = profile2.Lines2d;

                for (int i = 0; i <= points.GetUpperBound(0); i++)
                {
                    lines2d.AddBy2Points(points[i, 0], points[i, 1], points[i, 2], points[i, 3]);
                }

                origins.Add(lines2d.Item(1));

                crossSections.Add(profile1);
                crossSections.Add(profile2);

                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);

                originRefs.Add(SolidEdgePart.FeaturePropertyConstants.igStart);
                originRefs.Add(SolidEdgePart.FeaturePropertyConstants.igStart);

                // Get a reference to the Models collection.
                models = sheetMetalDocument.Models;

                // Create a new lofted flange.
                model = models.AddLoftedFlange(crossSections.Count,
                    crossSections.ToArray(),
                    crossSectionTypes.ToArray(),
                    origins.ToArray(),
                    originRefs.ToArray(),
                    SolidEdgePart.FeaturePropertyConstants.igRight,
                    bendRadius,
                    neutralFactor,
                    SolidEdgePart.FeaturePropertyConstants.igNFType);

                // Get a reference to the LoftedFlanges collection.
                loftedFlanges = model.LoftedFlanges;

                // Get a reference to the new LoftedFlange.
                loftedFlange = loftedFlanges.Item(1);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new protrusion to ActiveSelectSet.
                selectSet.Add(loftedFlange);

                profile1.Visible = false;
                profile2.Visible = false;

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView);
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
