using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileProperties.ConsoleDemo
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Build path to file.
            string filename = Path.Combine(SolidEdgeCommunity.SolidEdgeInstall.GetTrainingFolderPath(), "Coffee Pot.par");

            // Ideally, you want to run the Interop logic in a separate AppDomain.
            // If you don't, you will experience file locking. Even after Close().

            // Add or Update custom properties.
            UpdateCustomProperties(filename);
            
            // Report properties.
            ReportProperties(filename);

            // Delete previously added custom properties.
            DeleteCustomProperties(filename);
        }

        static void DeleteCustomProperties(string filename)
        {
            SolidEdgeFileProperties.PropertySets propertySets = null;
            SolidEdgeFileProperties.Properties customPropertySet = null;
            SolidEdgeFileProperties.Property property = null;
            List<SolidEdgeFileProperties.Property> customProperties = new List<SolidEdgeFileProperties.Property>();

            FileInfo fileInfo = new FileInfo(filename);

            Console.WriteLine("Processing '{0}'.", fileInfo.FullName);

            try
            {
                if (fileInfo.Exists == false)
                {
                    throw new FileNotFoundException("File not found.", fileInfo.FullName);
                }

                // Create a new instance of PropertySets.
                propertySets = new SolidEdgeFileProperties.PropertySets();

                // Open the file in edit mode.
                propertySets.Open(fileInfo.FullName, false);

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

        static void ReportProperties(string filename)
        {
            SolidEdgeFileProperties.PropertySets propertySets = null;
            SolidEdgeFileProperties.Properties properties = null;
            SolidEdgeFileProperties.Property property = null;

            FileInfo fileInfo = new FileInfo(filename);

            Console.WriteLine("Processing '{0}'.", fileInfo.FullName);

            try
            {
                if (fileInfo.Exists == false)
                {
                    throw new FileNotFoundException("File not found.", fileInfo.FullName);
                }

                // Create a new instance of PropertySets.
                propertySets = new SolidEdgeFileProperties.PropertySets();

                // Open the file in readonly mode.
                propertySets.Open(fileInfo.FullName, true);

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

        static void UpdateCustomProperties(string filename)
        {
            SolidEdgeFileProperties.PropertySets propertySets = null;

            FileInfo fileInfo = new FileInfo(filename);

            Console.WriteLine("Processing '{0}'.", fileInfo.FullName);

            try
            {
                if (fileInfo.Exists == false)
                {
                    throw new FileNotFoundException("File not found.", fileInfo.FullName);
                }

                // Create a new instance of PropertySets.
                propertySets = new SolidEdgeFileProperties.PropertySets();

                // Open the file in edit mode.
                propertySets.Open(fileInfo.FullName, false);

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
