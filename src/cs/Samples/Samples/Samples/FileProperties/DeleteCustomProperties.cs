using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.FileProperties
{
    /// <summary>
    /// Deletes specific custom file properties of a file.
    /// </summary>
    class DeleteCustomProperties
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFileProperties.PropertySets propertySets = null;
            SolidEdgeFileProperties.Properties customPropertySet = null;
            SolidEdgeFileProperties.Property property = null;
            List<SolidEdgeFileProperties.Property> customProperties = new List<SolidEdgeFileProperties.Property>();

            // Build path to file.
            string fileName = Path.Combine(SolidEdgeCommunity.SolidEdgeInstall.GetTrainingFolderPath(), "Coffee Pot.par");

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

                // Open the file in edit mode.
                propertySets.Open(fileName, false);

                // Get a reference to the custom property set.
                customPropertySet = (SolidEdgeFileProperties.Properties)propertySets["Custom"];

                // Loop through the properties for the current PropertySet.
                for (int j = 0; j < customPropertySet.Count; j++)
                {
                    // Get a reference to the current PropertySet.
                    property = (SolidEdgeFileProperties.Property)customPropertySet[j];

                    if (property.Name.StartsWith("My"))
                    {
                        customProperties.Add(property);
                    }
                }

                // Loop through the custom properties in the List<>.
                foreach (SolidEdgeFileProperties.Property customProperty in customProperties)
                {
                    string customPropertyName = customProperty.Name;

                    // Delete the custom property.
                    customProperty.Delete();

                    Console.WriteLine("Deleted '{0}' custom property.", customPropertyName);
                }

                // Save the changes.
                propertySets.Save();
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
