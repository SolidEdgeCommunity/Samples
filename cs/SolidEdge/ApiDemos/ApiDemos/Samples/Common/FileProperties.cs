using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiDemos.Common
{
    static class FileProperties
    {
        public static void AddCustomProperties(SolidEdgeFramework.PropertySets propertySets)
        {
            var properties = (SolidEdgeFramework.Properties)propertySets.Item("Custom");
            var propertyValues = new object[] { "My text", int.MaxValue, 1.23, true, DateTime.Now };

            foreach (var propertyValue in propertyValues)
            {
                var propertyType = propertyValue.GetType();
                var propertyName = String.Format("My {0}", propertyType);
                var property = properties.Add(propertyName, propertyValue);

                Console.WriteLine("Added {0} - {1}.", property.Name, propertyValue);
            }
        }

        public static void DeleteCustomProperties(SolidEdgeFramework.PropertySets propertySets)
        {
            var properties = (SolidEdgeFramework.Properties)propertySets.Item("Custom");

            // Query for custom properties that start with "My".
            var query = properties.OfType<SolidEdgeFramework.Property>().Where(x => x.Name.StartsWith("My"));

            // Force ToArray() so that Delete() doesn't interfere with the enumeration.
            foreach (var property in query.ToArray())
            {
                var propertyName = property.Name;
                property.Delete();
                Console.WriteLine("Deleted {0}.", propertyName);
            }
        }

        public static void ReportAllProperties(SolidEdgeFramework.PropertySets propertySets)
        {
            foreach (var properties in propertySets.OfType<SolidEdgeFramework.Properties>())
            {
                Console.WriteLine("PropertSet '{0}'.", properties.Name);

                foreach (var property in properties.OfType<SolidEdgeFramework.Property>())
                {
                    System.Runtime.InteropServices.VarEnum nativePropertyType = System.Runtime.InteropServices.VarEnum.VT_EMPTY;
                    Type runtimePropertyType = null;

                    object value = null;

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
    }
}
