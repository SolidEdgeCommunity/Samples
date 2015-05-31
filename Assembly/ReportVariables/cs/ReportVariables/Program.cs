using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportVariables
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeAssembly.AssemblyDocument document = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect();

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeAssembly.AssemblyDocument>(false);

                // Make sure we have a document.
                if (document != null)
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
