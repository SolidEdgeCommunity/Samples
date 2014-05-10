using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Part
{
    /// <summary>
    /// Moves all existing ordered features of the active part to synchronous.
    /// </summary>
    class MoveAllOrderedFeaturesToSynchronous
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.Features features = null;
            bool bIgnoreWarnings = true;
            bool bExtendSelection = true;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = ApplicationHelper.Connect(true);

                // Make sure user can see the GUI.
                application.Visible = true;

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active part document.
                partDocument = application.TryActiveDocumentAs<SolidEdgePart.PartDocument>();

                if (partDocument != null)
                {
                    // Get a reference to the Models collection.
                    models = partDocument.Models;

                    // Get a reference to the 1st model.
                    model = models.Item(1);

                    // Get a reference to the Features collection.
                    features = model.Features;

                    // Iterate through the features.
                    foreach (object feature in features)
                    {
                        string featureEdgeBarName = ReflectionHelper.GetPropertyValueAsString(feature, "EdgeBarName");

                        SolidEdgePart.FaceRotate faceRotate = feature as SolidEdgePart.FaceRotate;

                        // Check to see if the feature is ordered.
                        // NOTE: I've found that not all features have a ModelingModeType property. SolidEdgePart.FaceRotate is one of them.
                        // This is a bit of a problem because I see no way to know if the FaceRotate is Ordered or Synchronous...
                        if (ReflectionHelper.GetPartFeatureModelingMode(feature) == SolidEdgePart.ModelingModeConstants.seModelingModeOrdered)
                        {
                            int NumberOfFeaturesCausingError = 0;
                            Array ErrorMessageArray = Array.CreateInstance(typeof(string), 0);
                            int NumberOfFeaturesCausingWarning = 0;
                            Array WarningMessageArray = Array.CreateInstance(typeof(string), 0);
                            double VolumeDifference = 0.0;

                            // Move the ordered feature to synchronous.
                            partDocument.MoveToSynchronous(
                                pFeatureUnk: feature,
                                bIgnoreWarnings: bIgnoreWarnings,
                                bExtendSelection: bExtendSelection,
                                NumberOfFeaturesCausingError: out NumberOfFeaturesCausingError,
                                ErrorMessageArray: out ErrorMessageArray,
                                NumberOfFeaturesCausingWarning: out NumberOfFeaturesCausingWarning,
                                WarningMessageArray: out WarningMessageArray,
                                VolumeDifference: out VolumeDifference);

                            Console.WriteLine("Feature '{0}' results:", featureEdgeBarName);
                            Console.WriteLine("VolumeDifference: '{0}'", VolumeDifference);

                            // Process error messages.
                            for (int i = 0; i < ErrorMessageArray.Length; i++)
                            {
                                Console.WriteLine("Error: '{0}'.", ErrorMessageArray.GetValue(i));
                            }

                            // Process warning messages.
                            for (int i = 0; i < WarningMessageArray.Length; i++)
                            {
                                Console.WriteLine("Warning: '{0}'.", WarningMessageArray.GetValue(i));
                            }

                            // If you get any error or warning messages, it's probably a good idea to stop.
                            if ((ErrorMessageArray.Length > 0) || (WarningMessageArray.Length > 0))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Success");
                            }
                        }
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActivePartDocument);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                OleMessageFilter.Unregister();
            }
        }
    }
}
