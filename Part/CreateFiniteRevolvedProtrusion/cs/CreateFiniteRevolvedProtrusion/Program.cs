using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateFiniteRevolvedProtrusion
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.RevolvedProtrusions revolvedProtrusions = null;
            SolidEdgePart.RevolvedProtrusion revolvedProtrusion = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the Documents collection.
                documents = application.Documents;

                // Create a new PartDocument.
                partDocument = documents.AddPartDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteRevolvedProtrusion(partDocument);

                // Get a reference to the RevolvedProtrusions collection.
                revolvedProtrusions = model.RevolvedProtrusions;

                // Get a reference to the new RevolvedProtrusion.
                revolvedProtrusion = revolvedProtrusions.Item(1);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new RevolvedProtrusion to ActiveSelectSet.
                selectSet.Add(revolvedProtrusion);

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartViewISOView);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
