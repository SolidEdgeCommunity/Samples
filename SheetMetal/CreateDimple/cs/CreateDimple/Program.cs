using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateDimple
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
            SolidEdgePart.Model model = null;
            SolidEdgePart.ProfileSets profileSets = null;
            SolidEdgePart.ProfileSet profileSet = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgeFrameworkSupport.Lines2d lines2d = null;
            SolidEdgeFrameworkSupport.Line2d line2d = null;
            SolidEdgePart.Dimple dimple = null;
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

                // Get a reference to front RefPlane.
                refPlane = refPlanes.GetFrontPlane();

                // Get a reference to the ProfileSets collection.
                profileSets = sheetMetalDocument.ProfileSets;

                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                // Add new Profile.
                profile = profiles.Add(refPlane);

                // Draw a line to define the dimple point.
                lines2d = profile.Lines2d;
                line2d = lines2d.AddBy2Points(0, 0, 0.01, 0);

                // Hide the profile.
                profile.Visible = false;

                double depth = 0.01;

                // Add new dimple.
                dimple = model.Dimples.Add(
                    Profile: profile,
                    Depth: depth,
                    ProfileSide: SolidEdgePart.DimpleFeatureConstants.seDimpleDepthLeft,
                    DepthSide: SolidEdgePart.DimpleFeatureConstants.seDimpleDepthRight);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new Dimple to ActiveSelectSet.
                selectSet.Add(dimple);

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
