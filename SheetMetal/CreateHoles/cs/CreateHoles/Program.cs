using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateHoles
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.RefPlanes refplanes = null;
            SolidEdgePart.RefPlane refplane = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.HoleDataCollection holeDataCollection = null;
            SolidEdgePart.ProfileSets profileSets = null;
            SolidEdgePart.ProfileSet profileSet = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgePart.Holes2d holes2d = null;
            SolidEdgePart.Hole2d hole2d = null;
            SolidEdgePart.Holes holes = null;
            SolidEdgePart.Hole hole = null;
            long profileStatus = 0;
            List<SolidEdgePart.Profile> profileList = new List<SolidEdgePart.Profile>();
            SolidEdgePart.UserDefinedPatterns userDefinedPatterns = null;
            SolidEdgePart.UserDefinedPattern userDefinedPattern = null;
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
                refplanes = sheetMetalDocument.RefPlanes;

                // Get a reference to front RefPlane.
                refplane = refplanes.GetFrontPlane();

                // Get a reference to the ProfileSets collection.
                profileSets = sheetMetalDocument.ProfileSets;

                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                // Add new Profile.
                profile = profiles.Add(refplane);

                // Get a reference to the Holes2d collection.
                holes2d = profile.Holes2d;

                // This creates a cross pattern of holes.
                double[,] holeMatrix = new double[,]
				{
                    //{x, y}
                    {0.00, 0.00},
                    {-0.01, 0.00},
                    {-0.02, 0.00},
                    {-0.03, 0.00},
                    {-0.04, 0.00},
                    {0.01, 0.00},
                    {0.02, 0.00},
                    {0.03, 0.00},
                    {0.04, 0.00},
                    {0.00, -0.01},
                    {0.00, -0.02},
                    {0.00, -0.03},
                    {0.00, -0.04},
                    {0.00, 0.01},
                    {0.00, 0.02},
                    {0.00, 0.03},
                    {0.00, 0.04}
				};

                // Draw the Base Profile.
                for (int i = 0; i <= holeMatrix.GetUpperBound(0); i++)
                {
                    // Add new Hole2d.
                    hole2d = holes2d.Add(
                        XCenter: holeMatrix[i, 0],
                        YCenter: holeMatrix[i, 1]);
                }

                // Hide the profile.
                profile.Visible = false;

                // Close profile.
                profileStatus = profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

                // Get a reference to the ProfileSet.
                profileSet = (SolidEdgePart.ProfileSet)profile.Parent;

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                // Add profiles to list for AddByProfiles().
                for (int i = 1; i <= profiles.Count; i++)
                {
                    profileList.Add(profiles.Item(i));
                }

                // Get a reference to the HoleDataCollection collection.
                holeDataCollection = sheetMetalDocument.HoleDataCollection;

                // Add new HoleData.
                SolidEdgePart.HoleData holeData = holeDataCollection.Add(
                    HoleType: SolidEdgePart.FeaturePropertyConstants.igRegularHole,
                    HoleDiameter: 0.005,
                    BottomAngle: 90);

                // Get a reference to the Holes collection.
                holes = model.Holes;

                // Add hole.
                hole = holes.AddFinite(
                    Profile: profile,
                    ProfilePlaneSide: SolidEdgePart.FeaturePropertyConstants.igRight,
                    FiniteDepth: 0.005,
                    Data: holeData);

                // Get a reference to the UserDefinedPatterns collection.
                userDefinedPatterns = model.UserDefinedPatterns;

                // Create the user defined pattern.
                userDefinedPattern = userDefinedPatterns.AddByProfiles(
                    NumberOfProfiles: profileList.Count,
                    ProfilesArray: profileList.ToArray(),
                    SeedFeature: hole);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new UserDefinedPattern to ActiveSelectSet.
                selectSet.Add(userDefinedPattern);

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
