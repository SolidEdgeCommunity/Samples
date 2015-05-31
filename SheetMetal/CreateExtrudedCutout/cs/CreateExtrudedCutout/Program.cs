using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateExtrudedCutout
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgePart.Sketchs sketchs = null;
            SolidEdgePart.Sketch sketch = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgeFrameworkSupport.Circles2d circles2d = null;
            SolidEdgeFrameworkSupport.Circle2d circle2d = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.ExtrudedCutouts extrudedCutouts = null;
            SolidEdgePart.ExtrudedCutout extrudedCutout = null;
            List<SolidEdgePart.Profile> profileList = new List<SolidEdgePart.Profile>();
            double finiteDepth1 = 0.5;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the Documents collection.
                documents = application.Documents;

                // Create a new sheetmetal document.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Call helper method to create the actual geometry.
                model = SheetMetalHelper.CreateBaseTabByCircle(sheetMetalDocument);

                // Get a reference to the RefPlanes collection.
                refPlanes = sheetMetalDocument.RefPlanes;

                // Get a reference to right RefPlane.
                refPlane = refPlanes.GetRightPlane();

                // Get a reference to the Sketches collection.
                sketchs = sheetMetalDocument.Sketches;

                // Add a new Sketch.
                sketch = sketchs.Add();

                // Get a reference to the Profiles collection.
                profiles = sketch.Profiles;

                // Add a new Profile.
                profile = profiles.Add(refPlane);

                profileList.Add(profile);

                // Create 2D circle.
                circles2d = profile.Circles2d;
                circle2d = circles2d.AddByCenterRadius(0, 0, 0.025);

                profile.Visible = false;

                // Get a reference to the ExtrudedCutouts collection.
                extrudedCutouts = model.ExtrudedCutouts;

                // Add a new ExtrudedCutout.
                extrudedCutout = extrudedCutouts.Add(
                    profileList.Count,
                    profileList.ToArray(),
                    SolidEdgePart.FeaturePropertyConstants.igLeft,
                    SolidEdgePart.FeaturePropertyConstants.igFinite,
                    SolidEdgePart.FeaturePropertyConstants.igSymmetric,
                    finiteDepth1,
                    null,
                    SolidEdgePart.KeyPointExtentConstants.igTangentNormal,
                    null,
                    SolidEdgePart.OffsetSideConstants.seOffsetNone,
                    0,
                    SolidEdgePart.TreatmentTypeConstants.seTreatmentNone,
                    SolidEdgePart.DraftSideConstants.seDraftNone,
                    0,
                    SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownNone,
                    SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideNone,
                    SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureNone,
                    0,
                    0,
                    SolidEdgePart.FeaturePropertyConstants.igNone,
                    SolidEdgePart.FeaturePropertyConstants.igNone,
                    0,
                    null,
                    SolidEdgePart.KeyPointExtentConstants.igTangentNormal,
                    null,
                    SolidEdgePart.OffsetSideConstants.seOffsetNone,
                    0,
                    SolidEdgePart.TreatmentTypeConstants.seTreatmentNone,
                    SolidEdgePart.DraftSideConstants.seDraftNone,
                    0,
                    SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownNone,
                    SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideNone,
                    SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureNone,
                    0,
                    0);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new ExtrudedCutout to ActiveSelectSet.
                selectSet.Add(extrudedCutout);

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
