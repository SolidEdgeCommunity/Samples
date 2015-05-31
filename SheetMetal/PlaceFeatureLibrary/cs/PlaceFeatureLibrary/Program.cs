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
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            Array features = Array.CreateInstance(typeof(object), 0);
            SolidEdgePart.ExtrudedProtrusion extrudedProtrustion = null;
            SolidEdgePart.UserDefinedPattern userDefinedPattern = null;
            SolidEdgePart.MirrorCopy mirrorCopy = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new sheet metal document.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Get a reference to the RefPlanes collection.
                refPlanes = sheetMetalDocument.RefPlanes;

                // Get a reference to the top RefPlane using extension method.
                refPlane = refPlanes.GetTopPlane();

                // Get path to Solid Edge training directory.  Typically, 'C:\Program Files\Solid Edge XXX\Training'.
                var trainingDirectory = new System.IO.DirectoryInfo(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath());

                // Build path to source part document.
                //string LibName = System.IO.Path.Combine(trainingDirectory.FullName, "Foot1.psm");
                string LibName = System.IO.Path.Combine(trainingDirectory.FullName, "base.par");

                // This method will take all features from block.par and place them into the new part document.
                sheetMetalDocument.PlaceFeatureLibrary(LibName, refPlane, 0.0, 0.0, 0.0, out features);

                // Optionally, iterate through all of the added features.
                foreach (var feature in features.OfType<object>())
                {
                    // Use helper class to get the feature type.
                    var featureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgePart.FeatureTypeConstants>(feature, "Type", (SolidEdgePart.FeatureTypeConstants)0);

                    // Depending on the feature type, we can cast the weakly typed feature to a strongly typed feature.
                    switch (featureType)
                    {
                        case SolidEdgePart.FeatureTypeConstants.igExtrudedProtrusionFeatureObject:
                            extrudedProtrustion = (SolidEdgePart.ExtrudedProtrusion)feature;
                            break;
                        case SolidEdgePart.FeatureTypeConstants.igUserDefinedPatternFeatureObject:
                            userDefinedPattern = (SolidEdgePart.UserDefinedPattern)feature;
                            break;
                        case SolidEdgePart.FeatureTypeConstants.igMirrorCopyFeatureObject:
                            mirrorCopy = (SolidEdgePart.MirrorCopy)feature;
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
