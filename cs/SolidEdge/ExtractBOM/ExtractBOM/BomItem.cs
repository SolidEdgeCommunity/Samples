using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractBOM
{
    public class BomItem
    {
        private List<BomItem> _children = new List<BomItem>();

        public BomItem()
        {
        }

        public BomItem(SolidEdgeAssembly.Occurrence occurrence)
        {
            FileName = occurrence.OccurrenceFileName;
        }

        public string FileName;
        public bool? Missing;
        public List<BomItem> Occurrence { get { return _children; } set { _children = value; } }

        public override string ToString()
        {
            return FileName;
        }
    }
}
