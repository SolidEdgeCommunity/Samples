using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using SolidEdgePart.Extensions; // SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiSamples.Part
{
    /// <summary>
    /// Creates a new part and places features from an existing document.
    /// </summary>
    class PlaceFeatureLibrary
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            Array features = Array.CreateInstance(typeof(object), 0);
            SolidEdgePart.ExtrudedProtrusion extrudedProtrustion = null;
            SolidEdgePart.Round round = null;

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

                // Get a reference to the top RefPlane using extension method.
                refPlane = refPlanes.GetTopPlane();

                // Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
                DirectoryInfo trainingDirectory = new DirectoryInfo(SolidEdgeCommunity.SolidEdgeInstall.GetTrainingFolderPath());

                // Build path to source part document.
                string LibName = Path.Combine(trainingDirectory.FullName, "block.par");

                // This method will take all features from block.par and place them into the new part document.
                partDocument.PlaceFeatureLibrary(LibName, refPlane, 0.0, 0.0, 0.0, out features);

                // Optionally, iterate through all of the added features.
                foreach (object feature in features)
                {
                    // Use ReflectionHelper class to get the feature type.
                    SolidEdgePart.FeatureTypeConstants featureType = ReflectionHelper.GetPartFeatureType(feature);

                    // Depending on the feature type, we can cast the weakly typed feature to a strongly typed feature.
                    switch (featureType)
                    {
                        case SolidEdgePart.FeatureTypeConstants.igExtrudedProtrusionFeatureObject:
                            extrudedProtrustion = (SolidEdgePart.ExtrudedProtrusion)feature;
                            break;
                        case SolidEdgePart.FeatureTypeConstants.igRoundFeatureObject:
                            round = (SolidEdgePart.Round)feature;
                            break;
                    }
                }

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add all features to ActiveSelectSet.
                foreach (object feature in features)
                {
                    selectSet.Add(feature);
                }

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
