using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateChamferByAddSetbackAngle
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgePart.ProfileSets profileSets = null;
            SolidEdgePart.ProfileSet profileSet = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgeFrameworkSupport.Lines2d lines2d = null;
            SolidEdgeFrameworkSupport.Line2d line2d = null;
            SolidEdgeFrameworkSupport.Relations2d relations2d = null;
            SolidEdgeFrameworkSupport.Relation2d relation2d = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            List<SolidEdgePart.Profile> profileList = new List<SolidEdgePart.Profile>();
            int status = 0;
            SolidEdgePart.ExtrudedProtrusions extrudedProtrusions = null;
            SolidEdgePart.ExtrudedProtrusion extrudedProtrusion = null;
            SolidEdgeGeometry.Edges edges = null;
            List<object> edgeList = new List<object>();
            SolidEdgeGeometry.Faces faces = null;
            SolidEdgePart.Chamfers chamfers = null;
            SolidEdgePart.Chamfer chamfer = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new part document.
                partDocument = documents.AddPartDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                refPlanes = partDocument.RefPlanes;
                refPlane = refPlanes.Item(1);

                profileSets = partDocument.ProfileSets;
                profileSet = profileSets.Add();

                profiles = profileSet.Profiles;
                profile = profiles.Add(refPlane);
                profileList.Add(profile);

                lines2d = profile.Lines2d;
                line2d = lines2d.AddBy2Points(0, 0, 0.06, 0);
                line2d = lines2d.AddBy2Points(0.06, 0, 0.06, 0.06);
                line2d = lines2d.AddBy2Points(0.06, 0.06, 0, 0.06);
                line2d = lines2d.AddBy2Points(0, 0.06, 0, 0);

                relations2d = (SolidEdgeFrameworkSupport.Relations2d)profile.Relations2d;
                relation2d = relations2d.AddKeypoint(
                    Object1: lines2d.Item(1),
                    Index1: (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd,
                    Object2: lines2d.Item(2),
                    Index2: (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);

                relation2d = relations2d.AddKeypoint(
                    Object1: lines2d.Item(2),
                    Index1: (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd,
                    Object2: lines2d.Item(3),
                    Index2: (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);

                relation2d = relations2d.AddKeypoint(
                    Object1: lines2d.Item(3),
                    Index1: (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd,
                    Object2: lines2d.Item(4),
                    Index2: (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);

                relation2d = relations2d.AddKeypoint(
                    Object1: lines2d.Item(4),
                    Index1: (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd,
                    Object2: lines2d.Item(1),
                    Index2: (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);

                // Make sure profile is closed.
                status = profile.End(SolidEdgePart.ProfileValidationType.igProfileClosed);

                if (status != 0)
                {
                    throw new System.Exception("Profile not closed.");
                }

                models = partDocument.Models;

                model = models.AddFiniteExtrudedProtrusion(
                    NumberOfProfiles: profileList.Count,
                    ProfileArray: profileList.ToArray(),
                    ProfilePlaneSide: SolidEdgePart.FeaturePropertyConstants.igRight,
                    ExtrusionDistance: 0.02);

                profile.Visible = false;

                extrudedProtrusions = model.ExtrudedProtrusions;
                extrudedProtrusion = extrudedProtrusions.Item(1);

                //edges = (SolidEdgeGeometry.Edges)extrudedProtrusion.get_Edges(
                //    SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll);

                //edgeList.Add(edges.Item(1));
                //edgeList.Add(edges.Item(8));

                faces = (SolidEdgeGeometry.Faces)
                    extrudedProtrusion.get_Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll);

                SolidEdgeGeometry.Face face = (SolidEdgeGeometry.Face)faces.Item(1);

                double dblAngle = 45 * Math.PI / 180;
                double setbackDistance = 0.003175;// 0.005;

                edges = (SolidEdgeGeometry.Edges)face.Edges;
                edgeList.Add(edges.Item(1));

                chamfers = model.Chamfers;

                chamfer = chamfers.AddSetbackAngle(
                    ReferenceFace: face,
                    NumberOfEdgeSets: edgeList.Count,
                    EdgeSetArray: edgeList.ToArray(),
                    SetbackDistance: setbackDistance,
                    Angle: dblAngle);

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
