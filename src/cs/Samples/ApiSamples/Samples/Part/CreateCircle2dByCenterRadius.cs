using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using SolidEdgePart.Extensions; // SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Part
{
    /// <summary>
    /// Creates a new part and adds a Circle2d by center radius.
    /// </summary>
    class CreateCircle2dByCenterRadius
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgePart.Sketchs sketches = null;
            SolidEdgePart.Sketch sketch = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgeFrameworkSupport.Circles2d circles2d = null;
            SolidEdgeFrameworkSupport.Circle2d circle2d = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new part document.
                partDocument = documents.AddPartDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Get a reference to the RefPlanes collection.
                refPlanes = partDocument.RefPlanes;

                // Get a reference to front RefPlane.
                refPlane = refPlanes.GetFrontPlane();

                // Get a reference to the Sketches collection.
                sketches = partDocument.Sketches;

                // Create a new sketch.
                sketch = sketches.Add();

                // Get a reference to the Profiles collection.
                profiles = sketch.Profiles;

                // Create a new profile.
                profile = profiles.Add(refPlane);

                circles2d = profile.Circles2d;

                circle2d = circles2d.AddByCenterRadius(0.04, 0.05, 0.02);

                profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new FaceRotate to ActiveSelectSet.
                selectSet.Add(circle2d);

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView);
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
