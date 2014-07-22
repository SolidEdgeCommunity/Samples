﻿using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Part
{
    /// <summary>
    /// Creates a new part with a family member containing suppressed features.
    /// </summary>
    class CreateFamilyMemberSuppressedFeature
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break(); 
            
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.FamilyMembers familyMembers = null;
            SolidEdgePart.FamilyMember familyMember = null;
            SolidEdgePart.EdgebarFeatures edgebarFeatures = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Add a new part document.
                partDocument = documents.AddPartDocument();

                // Invoke existing sample to create part geometry.
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

                    // Use ReflectionHelper class to get the feature type.
                    SolidEdgePart.FeatureTypeConstants featureType = ReflectionHelper.GetPartFeatureType(edgebarFeature);

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
                OleMessageFilter.Unregister();
            }
        }
    }
}
