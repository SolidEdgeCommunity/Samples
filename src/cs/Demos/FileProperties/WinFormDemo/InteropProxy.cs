using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileProperties.WinForm
{
    public class InteropProxy : MarshalByRefObject
    {
        private bool _readOnly;
        private SolidEdgeFileProperties.PropertySets _propertySets;

        public void Close()
        {
            if (_propertySets != null)
            {
                _propertySets.Close();
            }
            else
            {
                throw new InvalidOperationException("File is not open.");
            }
        }

        public void Close(bool saveChanges)
        {
            if (_propertySets != null)
            {
                if (saveChanges)
                {
                    _propertySets.Save();
                }

                _propertySets.Close();
            }
            else
            {
                throw new InvalidOperationException("File is not open.");
            }
        }

        public PropertyMetadata[] GetProperties()
        {
            List<PropertyMetadata> list = new List<PropertyMetadata>();

            // Loop through the properties and create a PropertyMetadata array to return.
            for (int i = 0; i < _propertySets.Count; i++)
            {
                var properties = (SolidEdgeFileProperties.Properties)_propertySets[i];

                for (int j = 0; j < properties.Count; j++)
                {
                    var property = (SolidEdgeFileProperties.Property)properties[j];
                    var metadata = PropertyMetadata.FromProperty(properties.Name, property);
                    list.Add(metadata);
                }
            }

            return list.ToArray();
        }

        public void Open(string path, bool readOnly)
        {
            _readOnly = readOnly;
            _propertySets = new SolidEdgeFileProperties.PropertySets();
            _propertySets.Open(path, _readOnly);
        }

        public void Save()
        {
            if (_propertySets != null)
            {
                if (_readOnly == false)
                {
                    _propertySets.Save();
                }
                else
                {
                    throw new System.Exception("File is read only.");
                }
            }
            else
            {
                throw new InvalidOperationException("File is not open.");
            }
        }

        public void SetProperty(PropertyMetadata metadata)
        {
            var properties = (SolidEdgeFileProperties.Properties)_propertySets[0];
            //var properties = (SolidEdgeFileProperties.Properties)_propertySets[metadata.PropertySetName];
            var property = (SolidEdgeFileProperties.Property)properties[metadata.PropertyName];
            property.Value = metadata.PropertyValue;
        }
    }
}
