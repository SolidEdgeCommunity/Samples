using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractStructure
{
    public class OccurrenceItem
    {
        private List<OccurrenceItem> _children = new List<OccurrenceItem>();

        public OccurrenceItem()
        {
        }

        public OccurrenceItem(SolidEdgeAssembly.Occurrence occurrence)
        {
            FileName = occurrence.OccurrenceFileName;
        }

        public OccurrenceItem(SolidEdgeAssembly.SubOccurrence subOccurrence)
        {
            FileName = subOccurrence.SubOccurrenceFileName;
        }

        public string FileName;
        public List<OccurrenceItem> Occurrence { get { return _children; } }

        public override string ToString()
        {
            return FileName;
        }
    }
}
