using SolidEdge.Community.AddIn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AddInDemo
{
    class DraftRibbon : SolidEdge.Community.AddIn.Ribbon
    {
        const string _embeddedResourceName = "AddInDemo.DraftRibbon.xml";

        public DraftRibbon()
        {
            // Get a reference to the current assembly. This is where the ribbon XML is embedded.
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // In this example, XML file must have a build action of "Embedded Resource".
            this.LoadXml(assembly, _embeddedResourceName);
        }

        public override void OnControlClick(RibbonControl control)
        {
        }
    }
}
