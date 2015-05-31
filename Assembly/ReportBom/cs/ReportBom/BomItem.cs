using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportBom
{
    /// <summary>
    /// Class to hold BOM data.
    /// </summary>
    public class BomItem
    {
        private List<BomItem> _children = new List<BomItem>();

        public BomItem()
        {
        }

        public BomItem(SolidEdgeAssembly.Occurrence occurrence, int level)
        {
            Level = level;
            FileName = System.IO.Path.GetFullPath(occurrence.OccurrenceFileName);
            IsMissing = occurrence.FileMissing();
            Quantity = 1;
            IsSubassembly = occurrence.Subassembly;

            // If the file exists, extract file properties.
            if (IsMissing == false)
            {
                var document = (SolidEdgeFramework.SolidEdgeDocument)occurrence.OccurrenceDocument;
                var summaryInfo = (SolidEdgeFramework.SummaryInfo)document.SummaryInfo;

                DocumentNumber = summaryInfo.DocumentNumber;
                Title = summaryInfo.Title;
                Revision = summaryInfo.RevisionNumber;
            }
        }

        public int? Level { get; set; }
        public string DocumentNumber { get; set; }
        public string Revision { get; set; }
        public string Title { get; set; }
        public int? Quantity { get; set; }
        public string FileName { get; set; }

        [JsonIgnore]
        public bool? IsSubassembly { get; set; }

        [JsonIgnore]
        public bool? IsMissing;

        /// <summary>
        /// Returns all direct children.
        /// </summary>
        [JsonProperty("Child")]
        public List<BomItem> Children { get { return _children; } set { _children = value; } }

        /// <summary>
        /// Returns all direct and descendant children.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<BomItem> AllChildren
        {
            get
            {
                foreach (var bomItem in Children)
                {
                    yield return bomItem;

                    if (bomItem.IsSubassembly == true)
                    {
                        foreach (var childBomItem in bomItem.AllChildren)
                        {
                            yield return childBomItem;
                        }
                    }
                }
            }
        }

        // Demonstration of how to exclude empty collections during JSON.NET serialization.
        public bool ShouldSerializeChildren()
        {
            return Children.Count > 0;
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
