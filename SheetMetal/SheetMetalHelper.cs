using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class SheetMetalHelper
{
    public static SolidEdgePart.Model CreateBaseTab(SolidEdgePart.SheetMetalDocument sheetMetalDocument)
    {
        SolidEdgePart.ProfileSets profileSets = null;
        SolidEdgePart.ProfileSet profileSet = null;
        SolidEdgePart.Profiles profiles = null;
        SolidEdgePart.Profile profile = null;
        SolidEdgePart.RefPlanes refplanes = null;
        SolidEdgeFrameworkSupport.Relations2d relations2d = null;
        SolidEdgeFrameworkSupport.Relation2d relation2d = null;
        SolidEdgeFrameworkSupport.Lines2d lines2d = null;
        SolidEdgeFrameworkSupport.Line2d line2d = null;
        SolidEdgePart.Models models = null;
        SolidEdgePart.Model model = null;

        // Get a reference to the profile sets collection.
        profileSets = sheetMetalDocument.ProfileSets;

        // Add a new profile set.
        profileSet = profileSets.Add();

        // Get a reference to the profiles collection.
        profiles = profileSet.Profiles;

        // Get a reference to the ref planes collection.
        refplanes = sheetMetalDocument.RefPlanes;

        // Add a new profile.
        profile = profiles.Add(refplanes.Item(1));

        // Get a reference to the lines2d collection.
        lines2d = profile.Lines2d;

        // UOM = meters.
        double[,] lineMatrix = new double[,]
				{
                    //{x1, y1, x2, y2}
                    {0.05, 0.025, 0.05, 0.025},
                    {-0.05, 0.025, -0.05, -0.025},
                    {-0.05, -0.025, 0.05, -0.025},
                    {0.05, -0.025, 0.05, 0.025}
				};

        // Draw the Base Profile.
        for (int i = 0; i <= lineMatrix.GetUpperBound(0); i++)
        {
            line2d = lines2d.AddBy2Points(
                                            lineMatrix[i, 0],
                                            lineMatrix[i, 1],
                                            lineMatrix[i, 2],
                                            lineMatrix[i, 3]);
        }

        // Define Relations among the Line objects to make the Profile closed.
        relations2d = (SolidEdgeFrameworkSupport.Relations2d)profile.Relations2d;

        // Connect all of the lines.
        for (int i = 1; i <= lines2d.Count; i++)
        {
            int j = i + 1;

            // When we reach the last line, wrap around and connect it to the first line.
            if (j > lines2d.Count)
            {
                j = 1;
            }

            relation2d = relations2d.AddKeypoint(
              lines2d.Item(i),
              (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd,
              lines2d.Item(j),
              (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart,
              true);
        }

        // Close the profile.
        profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

        // Hide the profile.
        profile.Visible = false;

        // Get a reference to the models collection.
        models = sheetMetalDocument.Models;

        // Create the base tab.
        model = models.AddBaseTab(profile, SolidEdgePart.FeaturePropertyConstants.igRight);

        return model;
    }

    public static SolidEdgePart.Model CreateBaseTabByCircle(SolidEdgePart.SheetMetalDocument sheetMetalDocument)
    {
        SolidEdgePart.RefPlanes refPlanes = null;
        SolidEdgePart.RefPlane refPlane = null;
        SolidEdgePart.Sketchs sketchs = null;
        SolidEdgePart.Sketch sketch = null;
        SolidEdgePart.Profiles profiles = null;
        SolidEdgePart.Profile profile = null;
        SolidEdgeFrameworkSupport.Circles2d circles2d = null;
        SolidEdgeFrameworkSupport.Circle2d circle2d = null;
        SolidEdgePart.Models models = null;
        SolidEdgePart.Model model = null;
        double x = 0;
        double y = 0;
        double radius = 0.05;

        // Get refplane.
        refPlanes = sheetMetalDocument.RefPlanes;

        // Get a reference to front RefPlane.
        refPlane = refPlanes.GetFrontPlane();

        // Create sketch.
        sketchs = sheetMetalDocument.Sketches;
        sketch = sketchs.Add();

        // Create profile.
        profiles = sketch.Profiles;
        profile = profiles.Add(refPlane);

        // Create 2D circle.
        circles2d = profile.Circles2d;
        circle2d = circles2d.AddByCenterRadius(x, y, radius);

        // Hide profile.
        profile.Visible = false;

        // Create extruded protrusion.
        models = sheetMetalDocument.Models;
        model = models.AddBaseTab(profile, SolidEdgePart.FeaturePropertyConstants.igRight);

        return model;
    }

    public static void CreateHolesWithUserDefinedPattern(SolidEdgePart.SheetMetalDocument sheetMetalDocument)
    {
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

        // Call helper method to create the actual geometry.
        model = CreateBaseTabByCircle(sheetMetalDocument);

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
    }
}