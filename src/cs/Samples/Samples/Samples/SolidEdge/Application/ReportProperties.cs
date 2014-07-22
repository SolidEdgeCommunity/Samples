using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge
{
    /// <summary>
    /// Reports the file properties of the active document.
    /// </summary>
    class ReportProperties
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.SolidEdgeDocument document = null;
            SolidEdgeFramework.PropertySets propertySets = null;
            SolidEdgeFramework.Properties properties = null;
            SolidEdgeFramework.Property property = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true);

                // Make sure user can see the GUI.
                application.Visible = true;

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active document.
                document = application.GetActiveDocument<SolidEdgeFramework.SolidEdgeDocument>(false);

                if (document != null)
                {
                    // Get a reference to the Properties collection.
                    propertySets = (SolidEdgeFramework.PropertySets)document.Properties;

                    for (int i = 1; i <= propertySets.Count; i++)
                    {
                        properties = propertySets.Item(i);

                        Console.WriteLine("PropertSet '{0}'.", properties.Name);

                        for (int j = 1; j <= properties.Count; j++)
                        {
                            System.Runtime.InteropServices.VarEnum nativePropertyType = System.Runtime.InteropServices.VarEnum.VT_EMPTY;
                            Type runtimePropertyType = null;

                            object value = null;

                            property = properties.Item(j);
                            nativePropertyType = (System.Runtime.InteropServices.VarEnum)property.Type;

                            // Accessing Value property may throw an exception...
                            try
                            {
                                value = property.get_Value();
                            }
                            catch (System.Exception ex)
                            {
                                value = ex.Message;
                            }

                            if (value != null)
                            {
                                runtimePropertyType = value.GetType();
                            }

                            Console.WriteLine("\t{0} = '{1}' ({2} | {3}).", property.Name, value, nativePropertyType, runtimePropertyType);
                        }

                        Console.WriteLine();
                    }
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveDocument);
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
