using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateSimpleLoftedCutout
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
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.ProfileSets profileSets = null;
            SolidEdgePart.ProfileSet profileSet = null;
            SolidEdgePart.Profiles profiles = null;
            List<SolidEdgePart.Profile> crossSectionProfiles = new List<SolidEdgePart.Profile>();
            SolidEdgeFrameworkSupport.Lines2d lines2d = null;
            SolidEdgeFrameworkSupport.Circles2d circles2d = null;
            SolidEdgeFrameworkSupport.Relations2d relations2d = null;
            List<object> OriginArray = new List<object>();
            SolidEdgePart.LoftedCutouts loftedCutouts = null;
            SolidEdgePart.LoftedCutout loftedCutout = null;
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
                refPlanes = partDocument.RefPlanes;

                // Get a reference to top RefPlane.
                refPlane = refPlanes.GetTopPlane();

                // Get a reference to the ProfileSets collection.
                profileSets = partDocument.ProfileSets;

                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                // Get a reference to the Models collection.
                models = partDocument.Models;

                #region Base Profile

                List<double[]> linesArray = new List<double[]>();
                linesArray.Add(new double[] { 0, 0, 0.1, 0 });
                linesArray.Add(new double[] { 0.1, 0, 0.1, 0.1 });
                linesArray.Add(new double[] { 0.1, 0.1, 0, 0.1 });
                linesArray.Add(new double[] { 0, 0.1, 0, 0 });

                // Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.1);

                #endregion

                #region CrossSection Profile #1

                refPlane = refPlanes.AddParallelByDistance(
                    ParentPlane: refPlanes.GetRightPlane(),
                    Distance: 0.1,
                    NormalSide: SolidEdgePart.ReferenceElementConstants.igNormalSide,
                    Local: true);

                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                crossSectionProfiles.Add(profiles.Add(refPlane));

                OriginArray.Add(new double[] { 0.03, 0.03 });

                lines2d = crossSectionProfiles[0].Lines2d;
                lines2d.AddBy2Points(0.03, 0.03, 0.07, 0.03);
                lines2d.AddBy2Points(0.07, 0.03, 0.07, 0.07);
                lines2d.AddBy2Points(0.07, 0.07, 0.03, 0.07);
                lines2d.AddBy2Points(0.03, 0.07, 0.03, 0.03);

                relations2d = (SolidEdgeFrameworkSupport.Relations2d)crossSectionProfiles[0].Relations2d;

                relations2d.AddKeypoint(lines2d.Item(1), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(2), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);
                relations2d.AddKeypoint(lines2d.Item(2), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(3), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);
                relations2d.AddKeypoint(lines2d.Item(3), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(4), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);
                relations2d.AddKeypoint(lines2d.Item(4), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(1), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);

                crossSectionProfiles[0].End(SolidEdgePart.ProfileValidationType.igProfileClosed);

                crossSectionProfiles[0].Visible = false;

                #endregion

                #region CrossSection Profile #2

                refPlane = refPlanes.AddParallelByDistance(
                                        ParentPlane: refPlanes.GetRightPlane(),
                                        Distance: 0.05,
                                        NormalSide: SolidEdgePart.ReferenceElementConstants.igNormalSide,
                                        Local: true);
                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                crossSectionProfiles.Add(profiles.Add(refPlane));

                OriginArray.Add(new double[] { 0.0, 0.0 });

                circles2d = crossSectionProfiles[1].Circles2d;
                circles2d.AddByCenterRadius(0.05, 0.05, 0.015);

                crossSectionProfiles[1].End(SolidEdgePart.ProfileValidationType.igProfileClosed);

                crossSectionProfiles[1].Visible = false;

                #endregion

                #region CrossSection Profile #3

                refPlane = refPlanes.AddParallelByDistance(
                                        ParentPlane: refPlanes.GetRightPlane(),
                                        Distance: 0,
                                        NormalSide: SolidEdgePart.ReferenceElementConstants.igNormalSide,
                                        Local: true);

                // Add new ProfileSet.
                profileSet = profileSets.Add();

                // Get a reference to the Profiles collection.
                profiles = profileSet.Profiles;

                crossSectionProfiles.Add(profiles.Add(refPlane));

                OriginArray.Add(new double[] { 0.03, 0.03 });

                lines2d = crossSectionProfiles[2].Lines2d;
                lines2d.AddBy2Points(0.03, 0.03, 0.07, 0.03);
                lines2d.AddBy2Points(0.07, 0.03, 0.07, 0.07);
                lines2d.AddBy2Points(0.07, 0.07, 0.03, 0.07);
                lines2d.AddBy2Points(0.03, 0.07, 0.03, 0.03);

                relations2d = (SolidEdgeFrameworkSupport.Relations2d)crossSectionProfiles[2].Relations2d;

                relations2d.AddKeypoint(lines2d.Item(1), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(2), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);
                relations2d.AddKeypoint(lines2d.Item(2), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(3), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);
                relations2d.AddKeypoint(lines2d.Item(3), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(4), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);
                relations2d.AddKeypoint(lines2d.Item(4), (int)SolidEdgeConstants.KeypointIndexConstants.igLineEnd, lines2d.Item(1), (int)SolidEdgeConstants.KeypointIndexConstants.igLineStart);

                crossSectionProfiles[2].End(SolidEdgePart.ProfileValidationType.igProfileClosed);

                crossSectionProfiles[2].Visible = false;

                #endregion

                // Get a reference to the LoftedCutouts collection.
                loftedCutouts = model.LoftedCutouts;

                // Build cross section type array.
                List<object> crossSectionTypes = new List<object>();
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);
                crossSectionTypes.Add(SolidEdgePart.FeaturePropertyConstants.igProfileBasedCrossSection);

                // Create the lofted cutout.
                loftedCutout = loftedCutouts.AddSimple(crossSectionProfiles.Count,
                    crossSectionProfiles.ToArray(),
                    crossSectionTypes.ToArray(),
                    OriginArray.ToArray(),
                    SolidEdgePart.FeaturePropertyConstants.igLeft,
                    SolidEdgePart.FeaturePropertyConstants.igNone,
                    SolidEdgePart.FeaturePropertyConstants.igNone);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new LoftedCutout to ActiveSelectSet.
                selectSet.Add(loftedCutout);

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
