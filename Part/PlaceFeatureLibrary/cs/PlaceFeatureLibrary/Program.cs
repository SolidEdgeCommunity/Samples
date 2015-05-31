using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlaceFeatureLibrary
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
            SolidEdgeFramework.SelectSet selectSet = null;
            Array features = Array.CreateInstance(typeof(object), 0);
            SolidEdgePart.ExtrudedProtrusion extrudedProtrustion = null;
            SolidEdgePart.Round round = null;

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

                // Get a reference to the top RefPlane using extension method.
                refPlane = refPlanes.GetTopPlane();

                // Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
                var trainingDirectory = new System.IO.DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath());

                // Build path to source part document.
                var LibName = System.IO.Path.Combine(trainingDirectory.FullName, "block.par");

                // This method will take all features from block.par and place them into the new part document.
                partDocument.PlaceFeatureLibrary(LibName, refPlane, 0.0, 0.0, 0.0, out features);

                // Optionally, iterate through all of the added features.
                foreach (object feature in features)
                {
                    // Use helper class to get the feature type.
                    var featureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgePart.FeatureTypeConstants>(feature, "Type", (SolidEdgePart.FeatureTypeConstants)0);

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
