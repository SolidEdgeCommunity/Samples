using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoveOrderedFeaturesToSynchronous
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.Features features = null;
            bool bIgnoreWarnings = true;
            bool bExtendSelection = true;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active part document.
                sheetMetalDocument = application.GetActiveDocument<SolidEdgePart.SheetMetalDocument>(false);

                if (sheetMetalDocument != null)
                {
                    // Get a reference to the Models collection.
                    models = sheetMetalDocument.Models;

                    // Get a reference to the 1st model.
                    model = models.Item(1);

                    // Get a reference to the Features collection.
                    features = model.Features;

                    // Iterate through the features.
                    foreach (var feature in features.OfType<object>())
                    {
                        var featureEdgeBarName = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<string>(feature, "EdgeBarName", "Unknown");
                        var featureModelingMode = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgePart.ModelingModeConstants>(feature, "ModelingModeType", (SolidEdgePart.ModelingModeConstants)0);

                        // Check to see if the feature is ordered.
                        // NOTE: I've found that not all features have a ModelingModeType property. SolidEdgePart.FaceRotate is one of them.
                        // This is a bit of a problem because I see no way to know if the FaceRotate is Ordered or Synchronous...
                        if (featureModelingMode == SolidEdgePart.ModelingModeConstants.seModelingModeOrdered)
                        {
                            int NumberOfFeaturesCausingError = 0;
                            Array ErrorMessageArray = Array.CreateInstance(typeof(string), 0);
                            int NumberOfFeaturesCausingWarning = 0;
                            Array WarningMessageArray = Array.CreateInstance(typeof(string), 0);

                            // Move the ordered feature to synchronous.
                            sheetMetalDocument.MoveToSynchronous(
                                pFeatureUnk: feature,
                                bIgnoreWarnings: bIgnoreWarnings,
                                bExtendSelection: bExtendSelection,
                                NumberOfFeaturesCausingError: out NumberOfFeaturesCausingError,
                                ErrorMessageArray: out ErrorMessageArray,
                                NumberOfFeaturesCausingWarning: out NumberOfFeaturesCausingWarning,
                                WarningMessageArray: out WarningMessageArray);

                            Console.WriteLine("Feature '{0}' results:", featureEdgeBarName);

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
                    throw new System.Exception("No active document.");
                }
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
