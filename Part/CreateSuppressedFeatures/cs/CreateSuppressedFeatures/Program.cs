using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateSuppressedFeatures
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.FamilyMembers familyMembers = null;
            SolidEdgePart.FamilyMember familyMember = null;
            SolidEdgePart.EdgebarFeatures edgebarFeatures = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Add a new part document.
                partDocument = documents.AddPartDocument();

                // Call helper method to create the actual geometry.
                PartHelper.CreateHolesWithUserDefinedPattern(partDocument);

                // Get a reference to the FamilyMembers collection.
                familyMembers = partDocument.FamilyMembers;

                // Add a new FamilyMember.
                familyMember = familyMembers.Add("Member 1");

                // Get a reference to the DesignEdgebarFeatures collection.
                edgebarFeatures = partDocument.DesignEdgebarFeatures;

                // Iterate through the DesignEdgebarFeatures.
                for (int i = 1; i <= edgebarFeatures.Count; i++)
                {
                    // Get the EdgebarFeature at the current index.
                    object edgebarFeature = edgebarFeatures.Item(i);

                    // Use helper class to get the feature type.
                    var featureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgePart.FeatureTypeConstants>(edgebarFeature, "Type", (SolidEdgePart.FeatureTypeConstants)0);

                    // Looking for a Hole pattern to suppress.
                    if (featureType == SolidEdgePart.FeatureTypeConstants.igUserDefinedPatternFeatureObject)
                    {
                        // Suppress the feature.
                        familyMember.AddSuppressedFeature(edgebarFeature);
                    }
                }

                // Apply the FamilyMember.
                familyMember.Apply();
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
