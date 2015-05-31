using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateHoleWithUserDefinedPattern
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.RefPlanes refplanes = null;
            SolidEdgePart.RefPlane refplane = null;
            SolidEdgePart.Models models = null;
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

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new part document.
                partDocument = documents.AddPartDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Get a reference to the RefPlanes collection.
                refplanes = partDocument.RefPlanes;

                // Get a reference to front RefPlane.
                refplane = refplanes.GetFrontPlane();

                List<double[]> linesArray = new List<double[]>();
                linesArray.Add(new double[] { 0, 0, 0.08, 0 });
                linesArray.Add(new double[] { 0.08, 0, 0.08, 0.06 });
                linesArray.Add(new double[] { 0.08, 0.06, 0.064, 0.06 });
                linesArray.Add(new double[] { 0.064, 0.06, 0.064, 0.02 });
                linesArray.Add(new double[] { 0.064, 0.02, 0.048, 0.02 });
                linesArray.Add(new double[] { 0.048, 0.02, 0.048, 0.06 });
                linesArray.Add(new double[] { 0.048, 0.06, 0.032, 0.06 });
                linesArray.Add(new double[] { 0.032, 0.06, 0.032, 0.02 });
                linesArray.Add(new double[] { 0.032, 0.02, 0.016, 0.02 });
                linesArray.Add(new double[] { 0.016, 0.02, 0.016, 0.06 });
                linesArray.Add(new double[] { 0.016, 0.06, 0, 0.06 });
                linesArray.Add(new double[] { 0, 0.06, 0, 0 });

                // Call helper method to create the actual geometry.
                PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refplane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005);

                // Get a reference to the Models collection.
                models = partDocument.Models;

                // Get a reference to Model.
                model = models.Item(1);

                // Get a reference to the ProfileSets collection.
                profileSets = partDocument.ProfileSets;

                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                // Add new Profile.
                profile = profiles.Add(refplane);

                // Get a reference to the Holes2d collection.
                holes2d = profile.Holes2d;

                double[,] holeMatrix = new double[,]
				{
                    //{x, y}
                    {0.01, 0.01},
                    {0.02, 0.01},
                    {0.03, 0.01},
                    {0.04, 0.01},
                    {0.05, 0.01},
                    {0.06, 0.01},
                    {0.07, 0.01}
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
                holeDataCollection = partDocument.HoleDataCollection;

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
