using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PartHelper
{
    public static SolidEdgePart.Model CreateFiniteExtrudedProtrusion(SolidEdgePart.PartDocument partDocument, SolidEdgePart.RefPlane refPlane, double[][] linesArray, SolidEdgePart.FeaturePropertyConstants profilePlaneSide, double extrusionDistance)
    {
        SolidEdgePart.ProfileSets profileSets = null;
        SolidEdgePart.ProfileSet profileSet = null;
        SolidEdgePart.Profiles profiles = null;
        SolidEdgePart.Profile profile = null;
        SolidEdgeFrameworkSupport.Relations2d relations2d = null;
        SolidEdgeFrameworkSupport.Relation2d relation2d = null;
        SolidEdgeFrameworkSupport.Lines2d lines2d = null;
        SolidEdgeFrameworkSupport.Line2d line2d = null;
        SolidEdgePart.Models models = null;
        SolidEdgePart.Model model = null;
        System.Array aProfiles = null;

        // Get a reference to the profile sets collection.
        profileSets = partDocument.ProfileSets;

        // Add a new profile set.
        profileSet = profileSets.Add();

        // Get a reference to the profiles collection.
        profiles = profileSet.Profiles;

        // Add a new profile.
        profile = profiles.Add(refPlane);

        // Get a reference to the lines2d collection.
        lines2d = profile.Lines2d;

        // Draw the Base Profile.
        for (int i = 0; i <= linesArray.GetUpperBound(0); i++)
        {
            line2d = lines2d.AddBy2Points(
                x1: linesArray[i][0],
                y1: linesArray[i][1],
                x2: linesArray[i][2],
                y2: linesArray[i][3]);
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
                Object1: lines2d.Item(i),
                Index1: (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd,
                Object2: lines2d.Item(j),
                Index2: (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart,
                guaranteed_ok: true);
        }

        // Close the profile.
        profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

        // Hide the profile.
        profile.Visible = false;

        // Create a new array of profile objects.
        aProfiles = Array.CreateInstance(typeof(SolidEdgePart.Profile), 1);
        aProfiles.SetValue(profile, 0);

        // Get a reference to the models collection.
        models = partDocument.Models;

        // Create the extended protrusion.
        model = models.AddFiniteExtrudedProtrusion(
            NumberOfProfiles: aProfiles.Length,
            ProfileArray: ref aProfiles,
            ProfilePlaneSide: profilePlaneSide,
            ExtrusionDistance: extrusionDistance);

        return model;
    }

    public static SolidEdgePart.Model CreateFiniteRevolvedProtrusion(SolidEdgePart.PartDocument partDocument)
    {
        SolidEdgePart.RefPlanes refPlanes = null;
        SolidEdgePart.RefPlane refPlane = null;
        SolidEdgePart.ProfileSets profileSets = null;
        SolidEdgePart.ProfileSet profileSet = null;
        SolidEdgePart.Profiles profiles = null;
        SolidEdgePart.Profile profile = null;
        SolidEdgePart.Models models = null;
        SolidEdgePart.Model model = null;
        SolidEdgeFrameworkSupport.Lines2d lines2d = null;
        SolidEdgeFrameworkSupport.Line2d axis = null;
        SolidEdgeFrameworkSupport.Arcs2d arcs2d = null;
        SolidEdgeFrameworkSupport.Relations2d relations2d = null;
        SolidEdgePart.RefAxis refaxis = null;
        Array aProfiles = null;

        // Get a reference to the models collection.
        models = (SolidEdgePart.Models)partDocument.Models;

        // D1 to FA are parameters in a form, introduced by the user.
        double D1 = 0.020;
        double D2 = 0.026;
        double D3 = 0.003;
        double D4 = 0.014;
        double L1 = 0.040;
        double L2 = 0.030;
        double L3 = 0.005;

        // Get a reference to the ref planes collection.
        refPlanes = partDocument.RefPlanes;

        // Get a reference to front RefPlane.
        refPlane = refPlanes.GetFrontPlane();

        // Get a reference to the profile sets collection.
        profileSets = (SolidEdgePart.ProfileSets)partDocument.ProfileSets;

        // Create a new profile set.
        profileSet = profileSets.Add();

        // Get a reference to the profiles collection.
        profiles = profileSet.Profiles;

        // Create a new profile.
        profile = profiles.Add(refPlane);

        // Get a reference to the profile lines2d collection.
        lines2d = profile.Lines2d;

        // Get a reference to the profile arcs2d collection.
        arcs2d = profile.Arcs2d;

        double H = L1 - L2;
        double y = L1 - L3 - (D4 - D3) / (2 * Math.Tan((118 / 2) * (Math.PI / 180)));

        lines2d.AddBy2Points(D3 / 2, 0, D2 / 2, 0);		// Line1
        lines2d.AddBy2Points(D2 / 2, 0, D2 / 2, H);		// Line2
        lines2d.AddBy2Points(D2 / 2, H, D1 / 2, H);		// Line3
        lines2d.AddBy2Points(D1 / 2, H, D1 / 2, L1);	// Line4
        lines2d.AddBy2Points(D1 / 2, L1, D4 / 2, L1);	// Line5
        lines2d.AddBy2Points(D4 / 2, L1, D4 / 2, L1 - L3);	// Line6
        lines2d.AddBy2Points(D4 / 2, L1 - L3, D3 / 2, y);	// Line7
        lines2d.AddBy2Points(D3 / 2, y, D3 / 2, 0);		// Line8

        axis = lines2d.AddBy2Points(0, 0, 0, L1);
        profile.ToggleConstruction(axis);

        // relations
        relations2d = (SolidEdgeFrameworkSupport.Relations2d)profile.Relations2d;
        relations2d.AddKeypoint(lines2d.Item(1), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(2), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(2), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(3), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(3), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(4), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(4), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(5), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(5), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(6), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(6), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(7), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(7), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(8), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);
        relations2d.AddKeypoint(lines2d.Item(8), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(1), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart, true);

        refaxis = (SolidEdgePart.RefAxis)profile.SetAxisOfRevolution(axis);

        // Close the profile.
        int status = profile.End(SolidEdgePart.ProfileValidationType.igProfileRefAxisRequired);
        profile.Visible = false;

        // Create a new array of profile objects.
        aProfiles = Array.CreateInstance(typeof(SolidEdgePart.Profile), 1);
        aProfiles.SetValue(profile, 0);

        // add Finite Revolved Protrusion.
        model = models.AddFiniteRevolvedProtrusion(
            aProfiles.Length,
            ref aProfiles,
            refaxis,
            SolidEdgePart.FeaturePropertyConstants.igRight,
            2 * Math.PI,
            null,
            null);

        return model;
    }

    public static void CreateHolesWithUserDefinedPattern(SolidEdgePart.PartDocument partDocument)
    {
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
        CreateFiniteExtrudedProtrusion(partDocument, refplane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005);

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
    }

    public static SolidEdgePart.Model CreateSweptProtrusion(SolidEdgePart.PartDocument partDocument)
    {
        SolidEdgePart.Models models = null;
        SolidEdgePart.Model model = null;
        SolidEdgePart.Sketchs sketches = null;
        SolidEdgePart.Sketch sketch = null;
        SolidEdgePart.RefPlanes refPlanes = null;
        SolidEdgePart.RefPlane refPlane = null;
        SolidEdgePart.ProfileSets profileSets = null;
        SolidEdgePart.ProfileSet profileSet = null;
        SolidEdgePart.Profiles profiles = null;
        SolidEdgePart.Profile sketchProfile = null;
        SolidEdgePart.Profile profile = null;
        SolidEdgeFrameworkSupport.Circles2d circles2d = null;

        List<SolidEdgePart.Profile> listPaths = new List<SolidEdgePart.Profile>();
        List<SolidEdgePart.FeaturePropertyConstants> listPathTypes = new List<SolidEdgePart.FeaturePropertyConstants>();
        List<SolidEdgePart.Profile> listSections = new List<SolidEdgePart.Profile>();
        List<SolidEdgePart.FeaturePropertyConstants> listSectionTypes = new List<SolidEdgePart.FeaturePropertyConstants>();
        List<int> listOrigins = new List<int>();

        // Get a reference to the models collection.
        models = (SolidEdgePart.Models)partDocument.Models;

        // Get a reference to the Sketches collections.
        sketches = (SolidEdgePart.Sketchs)partDocument.Sketches;

        // Get a reference to the profile sets collection.
        profileSets = (SolidEdgePart.ProfileSets)partDocument.ProfileSets;

        // Get a reference to the ref planes collection.
        refPlanes = (SolidEdgePart.RefPlanes)partDocument.RefPlanes;

        // Get a reference to front RefPlane.
        refPlane = refPlanes.GetFrontPlane();

        // Add a new sketch.
        sketch = (SolidEdgePart.Sketch)sketches.Add();

        // Add profile for sketch on specified refplane.
        sketchProfile = sketch.Profiles.Add(refPlane);

        // Get a reference to the Circles2d collection.
        circles2d = sketchProfile.Circles2d;

        // Draw the Base Profile.
        circles2d.AddByCenterRadius(0, 0, 0.02);

        // Close the profile.
        sketchProfile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

        // Arrays for AddSweptProtrusion().
        listPaths.Add(sketchProfile);
        listPathTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);

        // NOTE: profile is the Curve.
        refPlane = refPlanes.AddNormalToCurve(
            sketchProfile,
            SolidEdgePart.ReferenceElementConstants.igCurveEnd,
            refPlanes.GetFrontPlane(),
            SolidEdgePart.ReferenceElementConstants.igPivotEnd,
            true,
            System.Reflection.Missing.Value);

        // Add a new profile set.
        profileSet = (SolidEdgePart.ProfileSet)profileSets.Add();

        // Get a reference to the profiles collection.
        profiles = (SolidEdgePart.Profiles)profileSet.Profiles;

        // add a new profile.
        profile = (SolidEdgePart.Profile)profiles.Add(refPlane);

        // Get a reference to the Circles2d collection.
        circles2d = profile.Circles2d;

        // Draw the Base Profile.
        circles2d.AddByCenterRadius(0, 0, 0.01);

        // Close the profile.
        profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

        // Arrays for AddSweptProtrusion().
        listSections.Add(profile);
        listSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);
        listOrigins.Add(0); //Use 0 for closed profiles.

        // Create the extended protrusion.
        model = models.AddSweptProtrusion(
                                   listPaths.Count,
                                   listPaths.ToArray(),
                                   listPathTypes.ToArray(),
                                   listSections.Count,
                                   listSections.ToArray(),
                                   listSectionTypes.ToArray(),
                                   listOrigins.ToArray(),
                                   0,
                                   SolidEdgePart.FeaturePropertyConstants.igLeft,
                                   SolidEdgePart.FeaturePropertyConstants.igNone,
                                   0.0,
                                   null,
                                   SolidEdgePart.FeaturePropertyConstants.igNone,
                                   0.0,
                                   null);

        // Hide profiles.
        sketchProfile.Visible = false;
        profile.Visible = false;

        return model;
    }
}