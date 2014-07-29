using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileProperties.WinForm
{
    [Serializable]
    public struct PropertyMetadata
    {
        public PropertyMetadata(string propertySetName, string propertyName, bool propertyValue)
        {
            PropertySetName = propertySetName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public PropertyMetadata(string propertySetName, string propertyName, DateTime propertyValue)
        {
            PropertySetName = propertySetName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public PropertyMetadata(string propertySetName, string propertyName, double propertyValue)
        {
            PropertySetName = propertySetName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public PropertyMetadata(string propertySetName, string propertyName, int propertyValue)
        {
            PropertySetName = propertySetName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public PropertyMetadata(string propertySetName, string propertyName, string propertyValue)
        {
            PropertySetName = propertySetName;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
        }

        public static PropertyMetadata FromProperty(string propertySetName, SolidEdgeFileProperties.Property property)
        {
            object propertyValue = null;

            try
            {
                propertyValue = property.Value;
            }
            catch
            {
            }

            if (propertyValue != null)
            {
                var propertyType = property.Value.GetType();

                if (propertyType.Equals(typeof(bool)))
                {
                    return new PropertyMetadata(propertySetName, property.Name, (bool)property.Value);
                }
                else if (propertyType.Equals(typeof(DateTime)))
                {
                    return new PropertyMetadata(propertySetName, property.Name, (DateTime)property.Value);
                }
                else if (propertyType.Equals(typeof(double)))
                {
                    return new PropertyMetadata(propertySetName, property.Name, (double)property.Value);
                }
                else if (propertyType.Equals(typeof(int)))
                {
                    return new PropertyMetadata(propertySetName, property.Name, (int)property.Value);
                }
                else if (propertyType.Equals(typeof(string)))
                {
                    return new PropertyMetadata(propertySetName, property.Name, (string)property.Value);
                }
                else
                {
                    throw new System.Exception("Unknown property type.");
                }
            }
            else
            {
                return new PropertyMetadata(propertySetName, property.Name, String.Empty);
            }
        }

        public readonly string PropertySetName;
        public readonly string PropertyName;
        public object PropertyValue;
    }
}
