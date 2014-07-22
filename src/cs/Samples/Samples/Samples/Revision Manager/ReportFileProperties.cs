using SolidEdgeCommunity; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.Revision_Manager
{
    /// <summary>
    /// Reports file properties of a file.
    /// </summary>
    class ReportFileProperties
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            RevisionManager.Application application = null;
            RevisionManager.PropertySets propertySets = null;
            RevisionManager.Properties properties = null;
            RevisionManager.Property property = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = new RevisionManager.Application();

                // Build path to file.
                string fileName = Path.Combine(SolidEdgeCommunity.SolidEdgeInstall.GetTrainingFolderPath(), "Coffee Pot.par");

                FileInfo fileInfo = new FileInfo(fileName);

                Console.WriteLine("Processing '{0}'.", fileName);

                propertySets = (RevisionManager.PropertySets)application.PropertySets;
                propertySets.Open(fileName, true);

                for (int i = 0; i < propertySets.Count; i++)
                {
                    properties = (RevisionManager.Properties)propertySets.Item[i];

                    Console.WriteLine(properties.Name);

                    for (int j = 0; j < properties.Count; j++)
                    {
                        property = (RevisionManager.Property)properties.Item[j];

                        object propertyValue = null;
                        string typeName = "Unknown";

                        // property.Value can throw an exception!
                        try
                        {
                            // Attempt to get the property value.
                            // In my testing, iCnt.exe would crash when accessing some properties.
                            propertyValue = property.Value;
                            typeName = propertyValue.GetType().FullName;
                        }
                        catch (System.Exception ex)
                        {
                            propertyValue = ex.Message;
                        }

                        Console.WriteLine("\t{0} ({1}) = '{2}'", property.Name, typeName, propertyValue);
                    }
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
