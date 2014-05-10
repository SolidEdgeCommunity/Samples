using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.FileProperties
{
    /// <summary>
    /// Reports file properties of a file.
    /// </summary>
    class ReportProperties
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFileProperties.PropertySets propertySets = null;
            SolidEdgeFileProperties.Properties properties = null;
            SolidEdgeFileProperties.Property property = null;

            // Build path to file.
            string fileName = Path.Combine(InstallDataHelper.GetTrainingFolderPath(), "Coffee Pot.par");

            FileInfo fileInfo = new FileInfo(fileName);

            Console.WriteLine("Processing '{0}'.", fileName);

            try
            {
                if (fileInfo.Exists == false)
                {
                    throw new FileNotFoundException("File not found.", fileName);
                }

                // Create a new instance of PropertySets.
                propertySets = new SolidEdgeFileProperties.PropertySets();

                // Open the file in readonly mode.
                propertySets.Open(fileName, true);

                // Loop through the PropertySets.
                for (int i = 0; i < propertySets.Count; i++)
                {
                    // Get a reference to the current PropertySet.
                    properties = (SolidEdgeFileProperties.Properties)propertySets[i];

                    Console.WriteLine(properties.Name);

                    // Loop through the properties for the current PropertySet.
                    for (int j = 0; j < properties.Count; j++)
                    {
                        // Get a reference to the current PropertySet.
                        property = (SolidEdgeFileProperties.Property)properties[j];

                        object propertyValue = null;
                        string typeName = "Unknown";

                        // property.Value can throw an exception!
                        try
                        {
                            // Attempt to get the property value.
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
                if (propertySets != null)
                {
                    propertySets.Close();
                }
            }
        }
    }
}
