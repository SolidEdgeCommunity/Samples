using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ApiDemos.Application
{
    /// <summary>
    /// Reports all Solid Edge global parameters.
    /// </summary>
    class ReportGlobalParameters
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get the ApplicationGlobalConstants type.
                Type type = typeof(SolidEdgeFramework.ApplicationGlobalConstants);

                // Get the fields of the type.
                FieldInfo[] fields = type.GetFields();

                // Enumerate the fields.
                foreach (FieldInfo field in fields)
                {
                    if (field.IsSpecialName) continue;

                    // Cast the raw constant value as ApplicationGlobalConstants.
                    SolidEdgeFramework.ApplicationGlobalConstants globalConstant = (SolidEdgeFramework.ApplicationGlobalConstants)field.GetRawConstantValue();
                    object val = null;

                    try
                    {
                        // GetGlobalParameter() may throw an exception.
                        application.GetGlobalParameter(globalConstant, ref val);
                    }
                    catch
                    {
                    }

                    Console.WriteLine("{0} = '{1}'", field.Name, val);
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
