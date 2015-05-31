using SolidEdgeCommunity.AddIn;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoAddIn
{
    class Ribbon2d : SolidEdgeCommunity.AddIn.Ribbon
    {
        const string _embeddedResourceName = "DemoAddIn.Ribbon2d.xml";

        public Ribbon2d()
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
