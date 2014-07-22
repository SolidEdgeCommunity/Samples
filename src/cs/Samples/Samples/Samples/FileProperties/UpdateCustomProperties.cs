using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.FileProperties
{
    /// <summary>
    /// Adds or updates custom file properties of a file.
    /// </summary>
    class UpdateCustomProperties
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFileProperties.PropertySets propertySets = null;

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

                // Add a string.
                AddOrUpdateCustomProperty(propertySets, "My String", "My text");

                // Add a number.
                AddOrUpdateCustomProperty(propertySets, "My Integer", int.MaxValue);

                // Add a boolean (Yes\No).
                AddOrUpdateCustomProperty(propertySets, "My Boolean", true);

                // Add a DateTime.
                AddOrUpdateCustomProperty(propertySets, "My Date", DateTime.Now);

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

        /// <summary>
        /// This method will either add a custom property if it doesn't exist or update an existing custom property.
        /// </summary>
        static void AddOrUpdateCustomProperty(SolidEdgeFileProperties.PropertySets propertySets, string propertyName, object propertyValue)
        {
            SolidEdgeFileProperties.Properties customPropertySet = null;
            SolidEdgeFileProperties.Property property = null;

            try
            {
                // Get a reference to the custom property set.
                customPropertySet = (SolidEdgeFileProperties.Properties)propertySets["Custom"];

                // Attempt to get the custom property.
                property = (SolidEdgeFileProperties.Property)customPropertySet[propertyName];

                // If we get here, the custom property exists so update the value.
                property.Value = propertyValue;

                Console.WriteLine("Updated '{0}' custom property.", propertyName);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // If we get here, the custom property does not exist so add it.
                property = (SolidEdgeFileProperties.Property)customPropertySet.Add(propertyName, propertyValue);

                Console.WriteLine("Added '{0}' custom property.", propertyName);
            }
            catch
            {
                throw;
            }
        }
    }
}
