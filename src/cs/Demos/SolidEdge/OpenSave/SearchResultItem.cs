using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolidEdge.OpenSave
{
    public struct SearchResultItem
    {
        public SearchResultItem(string file, Version version)
        {
            FileName = file;
            Version = version;
        }

        public string FileName;
        public Version Version;
    }
}
