using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class VariablesHelper
{
    public static void ReportVariables(SolidEdgeAssembly.AssemblyDocument document)
    {
        ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
    }

    public static void ReportVariables(SolidEdgeDraft.DraftDocument document)
    {
        ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
    }

    public static void ReportVariables(SolidEdgePart.PartDocument document)
    {
        ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
    }

    public static void ReportVariables(SolidEdgePart.SheetMetalDocument document)
    {
        ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
    }

    public static void ReportVariables(SolidEdgePart.WeldmentDocument document)
    {
        ReportVariables((SolidEdgeFramework.SolidEdgeDocument)document);
    }

    public static void ReportVariables(SolidEdgeFramework.SolidEdgeDocument document)
    {
        SolidEdgeFramework.Variables variables = null;
        SolidEdgeFramework.VariableList variableList = null;
        SolidEdgeFramework.variable variable = null;
        SolidEdgeFrameworkSupport.Dimension dimension = null;

        if (document == null) throw new ArgumentNullException("document");

        // Get a reference to the Variables collection.
        variables = (SolidEdgeFramework.Variables)document.Variables;

        // Get a reference to the variablelist.
        variableList = (SolidEdgeFramework.VariableList)variables.Query(
            pFindCriterium: "*",
            NamedBy: SolidEdgeConstants.VariableNameBy.seVariableNameByBoth,
            VarType: SolidEdgeConstants.VariableVarType.SeVariableVarTypeBoth);

        // Process variables.
        foreach (var variableListItem in variableList.OfType<object>())
        {
            // Not used in this sample but a good example of how to get the runtime type.
            var variableListItemType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(variableListItem);

            // Use helper class to get the object type.
            var objectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetPropertyValue<SolidEdgeFramework.ObjectType>(variableListItem, "Type", (SolidEdgeFramework.ObjectType)0);

            // Process the specific variable item type.
            switch (objectType)
            {
                case SolidEdgeFramework.ObjectType.igDimension:
                    // Get a reference to the dimension.
                    dimension = (SolidEdgeFrameworkSupport.Dimension)variableListItem;
                    Console.WriteLine("Dimension: '{0}' = '{1}' ({2})", dimension.DisplayName, dimension.Value, objectType);
                    break;
                case SolidEdgeFramework.ObjectType.igVariable:
                    variable = (SolidEdgeFramework.variable)variableListItem;
                    Console.WriteLine("Variable: '{0}' = '{1}' ({2})", variable.DisplayName, variable.Value, objectType);
                    break;
                default:
                    // Other SolidEdgeConstants.ObjectType's may exist.
                    break;
            }
        }
    }
}