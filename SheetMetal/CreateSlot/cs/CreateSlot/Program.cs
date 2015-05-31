using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateSlot
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgePart.Sketchs sketches = null;
            SolidEdgePart.Sketch sketch = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgeFrameworkSupport.Lines2d lines2d = null;
            SolidEdgeFrameworkSupport.Line2d line2d = null;
            SolidEdgePart.Slots slots = null;
            SolidEdgePart.Slot slot = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new sheetmetal document.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Invoke existing sample to create geometry.
                SheetMetalHelper.CreateBaseTab(sheetMetalDocument);

                // Get a reference to the Models collection.
                models = sheetMetalDocument.Models;

                // Get a reference to the 1st model.
                model = models.Item(1);

                // Get a reference to the RefPlanes collection.
                refPlanes = sheetMetalDocument.RefPlanes;

                refPlane = refPlanes.GetTopPlane();

                // Get a reference to the Sketches collection.
                sketches = sheetMetalDocument.Sketches;

                // Create a new sketch.
                sketch = sketches.Add();

                // Get a reference to the Profiles collection.
                profiles = sketch.Profiles;

                // Create a new profile.
                profile = profiles.Add(refPlane);

                // Get a reference to the Lines2d collection.
                lines2d = profile.Lines2d;

                // Add a new line.
                line2d = lines2d.AddBy2Points(-0.01, 0.0, -0.01, 0.01);

                // Get a reference to the Slots collection.
                slots = model.Slots;

                // Add a new slot.
                slot = slots.Add(profile, SolidEdgePart.FeaturePropertyConstants.igRegularSlot,
                    SolidEdgePart.FeaturePropertyConstants.igFormedEnd,
                    0.01,
                    0.0,
                    0.0,
                    SolidEdgePart.FeaturePropertyConstants.igFinite,
                    SolidEdgePart.FeaturePropertyConstants.igRight,
                    0.0005,
                    SolidEdgePart.KeyPointExtentConstants.igTangentNormal,
                    null,
                    SolidEdgePart.FeaturePropertyConstants.igNone,
                    SolidEdgePart.FeaturePropertyConstants.igNone,
                    0.0,
                    SolidEdgePart.KeyPointExtentConstants.igTangentNormal,
                    null,
                    null,
                    SolidEdgePart.OffsetSideConstants.seOffsetNone,
                    0.0,
                    null,
                    SolidEdgePart.OffsetSideConstants.seOffsetNone,
                    0.0);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new Slot to ActiveSelectSet.
                selectSet.Add(slot);
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
