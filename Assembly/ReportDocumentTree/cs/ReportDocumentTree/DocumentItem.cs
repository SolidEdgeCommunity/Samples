using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportDocumentTree
{
    public class DocumentItem
    {
       public DocumentItem()
        {
            DocumentItems = new List<DocumentItem>();
        }

        public DocumentItem(SolidEdgeAssembly.Occurrence occurrence)
            : this()
        {
            FileName = occurrence.OccurrenceFileName;
        }

        public DocumentItem(SolidEdgeAssembly.SubOccurrence subOccurrence)
            : this()
        {
            FileName = subOccurrence.SubOccurrenceFileName;
        }

        public string FileName { get; set; }

        /// <summary>
        /// Returns all direct occurrences.
        /// </summary>
        public List<DocumentItem> DocumentItems { get; set; }

        /// <summary>
        /// Returns all direct and descendant children.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DocumentItem> AllDocumentItems
        {
            get
            {
                foreach (var bomItem in DocumentItems)
                {
                    yield return bomItem;

                    foreach (var childBomItem in bomItem.AllDocumentItems)
                    {
                        yield return childBomItem;
                    }
                }
            }
        }

        public override int GetHashCode()
        {
            return FileName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            
            if (obj is DocumentItem)
            {
                return Equals((DocumentItem)obj);
            }

            return base.Equals(obj);
        }

        public bool Equals(DocumentItem obj)
        {
            if (obj == null) return false;

            return obj.FileName.Equals(FileName);
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
