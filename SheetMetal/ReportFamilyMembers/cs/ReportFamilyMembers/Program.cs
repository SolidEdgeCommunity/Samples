using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportFamilyMembers
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.FamilyMembers familyMembers = null;
            SolidEdgePart.Round round = null;
            SolidEdgePart.UserDefinedPattern userDefinedPattern = null;
            SolidEdgeFrameworkSupport.Dimension dimension = null;

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
                    // Get a reference to the FamilyMembers collection.
                    familyMembers = sheetMetalDocument.FamilyMembers;

                    // Interate through the family members.
                    foreach (var familyMember in familyMembers.OfType<SolidEdgePart.FamilyMember>())
                    {
                        Console.WriteLine(familyMember.Name);

                        // Determine FamilyMember MovePrecedence.
                        switch (familyMember.MovePrecedence)
                        {
                            case SolidEdgePart.MovePrecedenceConstants.igModelMovePredecence:
                                break;
                            case SolidEdgePart.MovePrecedenceConstants.igSelectSetMovePrecedence:
                                break;
                        }

                        // Warning: Accessing certain LiveRule[...] properties may throw an exception.
                        //Console.WriteLine("igConcentricLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igConcentricLiveRule]);
                        //Console.WriteLine("igCoplanarAxesAboutXLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesAboutXLiveRule]);
                        //Console.WriteLine("igCoplanarAxesAboutYLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesAboutYLiveRule]);
                        //Console.WriteLine("igCoplanarAxesAboutZLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesAboutZLiveRule]);
                        //Console.WriteLine("igCoplanarAxesLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarAxesLiveRule]);
                        //Console.WriteLine("igCoplanarLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igCoplanarLiveRule]);
                        //Console.WriteLine("igMaintainRadiusLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igMaintainRadiusLiveRule]);
                        //Console.WriteLine("igOrthoLockingLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igOrthoLockingLiveRule]);
                        //Console.WriteLine("igParallelLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igParallelLiveRule]);
                        //Console.WriteLine("igPerpendicularLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igPerpendicularLiveRule]);
                        //Console.WriteLine("igSymmetricLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricLiveRule]);
                        //Console.WriteLine("igSymmetricXYLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricXYLiveRule]);
                        //Console.WriteLine("igSymmetricYZLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricYZLiveRule]);
                        //Console.WriteLine("igSymmetricZXLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igSymmetricZXLiveRule]);
                        //Console.WriteLine("igTangentEdgeLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igTangentEdgeLiveRule]);
                        //Console.WriteLine("igTangentTouchingLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igTangentTouchingLiveRule]);
                        //Console.WriteLine("igThicknessChainLiveRule - {0}", familyMember.LiveRule[SolidEdgePart.LiveRulesConstants.igThicknessChainLiveRule]);

                        // Interate through the suppressed features of the current family member.
                        for (int j = 1; j <= familyMember.SuppressedFeatureCount; j++)
                        {
                            object suppressedFeature = familyMember.SuppressedFeature[j];

                            // Use helper class to get the feature type.
                            var featureType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgePart.FeatureTypeConstants>(suppressedFeature, "Type", (SolidEdgePart.FeatureTypeConstants)0);

                            switch (featureType)
                            {
                                case SolidEdgePart.FeatureTypeConstants.igRoundFeatureObject:
                                    round = (SolidEdgePart.Round)suppressedFeature;
                                    break;
                                case SolidEdgePart.FeatureTypeConstants.igUserDefinedPatternFeatureObject:
                                    userDefinedPattern = (SolidEdgePart.UserDefinedPattern)suppressedFeature;
                                    break;
                            }
                        }

                        // Interate through the variables of the current family member.
                        for (int j = 1; j <= familyMember.VariableCount; j++)
                        {
                            object variable = familyMember.Variable[j];

                            // Use helper class to get the object type.
                            var objectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgeFramework.ObjectType>(variable, "Type", (SolidEdgeFramework.ObjectType)0);

                            switch (objectType)
                            {
                                case SolidEdgeFramework.ObjectType.igDimension:
                                    dimension = (SolidEdgeFrameworkSupport.Dimension)variable;
                                    break;
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
