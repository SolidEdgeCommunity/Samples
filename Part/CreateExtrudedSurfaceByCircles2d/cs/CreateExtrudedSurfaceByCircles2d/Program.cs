using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateExtrudedSurfaceByCircles2d
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
            SolidEdgePart.Sketchs sketches = null;
            SolidEdgePart.Sketch sketch = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgeFrameworkSupport.Circles2d circles2d = null;
            SolidEdgeFrameworkSupport.Circle2d circle2d = null;
            SolidEdgePart.Constructions constructions = null;
            SolidEdgePart.ExtrudedSurfaces extrudedSurfaces = null;
            SolidEdgePart.ExtrudedSurface extrudedSurface = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the Documents collection.
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

                // Get a reference to the Constructions collection.
                constructions = partDocument.Constructions;

                // Get a reference to the ExtrudedSurfaces collection.
                extrudedSurfaces = constructions.ExtrudedSurfaces;

                // These parameter variables are declared because we have to pass them as pointers.
                SolidEdgePart.KeyPointExtentConstants KeyPointFlags1 = SolidEdgePart.KeyPointExtentConstants.igTangentNormal;
                SolidEdgePart.KeyPointExtentConstants KeyPointFlags2 = SolidEdgePart.KeyPointExtentConstants.igTangentNormal;

                List<SolidEdgePart.Profile> profileList = new List<SolidEdgePart.Profile>();

                for (int i = 1; i <= profiles.Count; i++)
                {
                    profileList.Add(profiles.Item(i));
                }

                Array profileArray = profileList.ToArray();

                // Add a new ExtrudedSurface.
                extrudedSurface = extrudedSurfaces.Add(
                    NumberOfProfiles: profileArray.Length,
                    ProfileArray: ref profileArray,
                    ExtentType1: SolidEdgePart.FeaturePropertyConstants.igFinite,
                    ExtentSide1: SolidEdgePart.FeaturePropertyConstants.igRight,
                    FiniteDepth1: 0.0127,
                    KeyPointOrTangentFace1: null,
                    KeyPointFlags1: ref KeyPointFlags1,
                    FromFaceOrRefPlane: null,
                    FromFaceOffsetSide: SolidEdgePart.OffsetSideConstants.seOffsetNone,
                    FromFaceOffsetDistance: 0,
                    TreatmentType1: SolidEdgePart.TreatmentTypeConstants.seTreatmentCrown,
                    TreatmentDraftSide1: SolidEdgePart.DraftSideConstants.seDraftInside,
                    TreatmentDraftAngle1: 0.1,
                    TreatmentCrownType1: SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownByOffset,
                    TreatmentCrownSide1: SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideInside,
                    TreatmentCrownCurvatureSide1: SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureInside,
                    TreatmentCrownRadiusOrOffset1: 0.003,
                    TreatmentCrownTakeOffAngle1: 0,
                    ExtentType2: SolidEdgePart.FeaturePropertyConstants.igFinite,
                    ExtentSide2: SolidEdgePart.FeaturePropertyConstants.igLeft,
                    FiniteDepth2: 0.0127,
                    KeyPointOrTangentFace2: null,
                    KeyPointFlags2: ref KeyPointFlags2,
                    ToFaceOrRefPlane: null,
                    ToFaceOffsetSide: SolidEdgePart.OffsetSideConstants.seOffsetNone,
                    ToFaceOffsetDistance: 0,
                    TreatmentType2: SolidEdgePart.TreatmentTypeConstants.seTreatmentCrown,
                    TreatmentDraftSide2: SolidEdgePart.DraftSideConstants.seDraftNone,
                    TreatmentDraftAngle2: 0,
                    TreatmentCrownType2: SolidEdgePart.TreatmentCrownTypeConstants.seTreatmentCrownByOffset,
                    TreatmentCrownSide2: SolidEdgePart.TreatmentCrownSideConstants.seTreatmentCrownSideInside,
                    TreatmentCrownCurvatureSide2: SolidEdgePart.TreatmentCrownCurvatureSideConstants.seTreatmentCrownCurvatureInside,
                    TreatmentCrownRadiusOrOffset2: 0.003,
                    TreatmentCrownTakeOffAngle2: 0,
                    WantEndCaps: true
                    );

                SolidEdgePart.FeaturePropertyConstants ExtentType;
                SolidEdgePart.FeaturePropertyConstants ExtentSide;
                double FiniteDepth = 0.0;
                SolidEdgePart.KeyPointExtentConstants KeyPointFlags = SolidEdgePart.KeyPointExtentConstants.igTangentNormal;

                // Get extent information for the first direction.
                extrudedSurface.GetDirection1Extent(out ExtentType, out ExtentSide, out FiniteDepth);

                // Modify parameters.
                FiniteDepth = 2.0;
                ExtentType = SolidEdgePart.FeaturePropertyConstants.igFinite;
                ExtentSide = SolidEdgePart.FeaturePropertyConstants.igRight;

                // Apply extent information for the first direction.
                extrudedSurface.ApplyDirection1Extent(
                    ExtentType,
                    ExtentSide,
                    FiniteDepth,
                    null,
                    ref KeyPointFlags);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new FaceRotate to ActiveSelectSet.
                selectSet.Add(extrudedSurface);

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
