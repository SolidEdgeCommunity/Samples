using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateFiniteExtrudedProtrusion
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.RefPlanes refPlanes = null;
            SolidEdgePart.RefPlane refPlane = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.ExtrudedProtrusions extrudedProtrusions = null;
            SolidEdgeFramework.SelectSet selectSet = null;
            SolidEdgePart.ExtrudedProtrusion extrudedProtrusion = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new part document.
                partDocument = documents.AddPartDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Get a reference to the RefPlanes collection.
                refPlanes = partDocument.RefPlanes;

                // Get a reference to a RefPlane.
                refPlane = refPlanes.GetFrontPlane();

                List<double[]> linesArray = new List<double[]>();
                linesArray.Add(new double[] { 0, 0, 0.08, 0 });
                linesArray.Add(new double[] { 0.08, 0, 0.08, 0.06 });
                linesArray.Add(new double[] { 0.08, 0.06, 0.064, 0.06 });
                linesArray.Add(new double[] { 0.064, 0.06, 0.064, 0.02 });
                linesArray.Add(new double[] { 0.064, 0.02, 0.048, 0.02 });
                linesArray.Add(new double[] { 0.048, 0.02, 0.048, 0.06 });
                linesArray.Add(new double[] { 0.048, 0.06, 0.032, 0.06 });
                linesArray.Add(new double[] { 0.032, 0.06, 0.032, 0.02 });
                linesArray.Add(new double[] { 0.032, 0.02, 0.016, 0.02 });
                linesArray.Add(new double[] { 0.016, 0.02, 0.016, 0.06 });
                linesArray.Add(new double[] { 0.016, 0.06, 0, 0.06 });
                linesArray.Add(new double[] { 0, 0.06, 0, 0 });

                // Call helper method to create the actual geometry.
                model = PartHelper.CreateFiniteExtrudedProtrusion(partDocument, refPlane, linesArray.ToArray(), SolidEdgePart.FeaturePropertyConstants.igRight, 0.005);

                // Get a reference to the ExtrudedProtrusions collection.
                extrudedProtrusions = model.ExtrudedProtrusions;

                // Get a reference to the new ExtrudedProtrusion.
                extrudedProtrusion = extrudedProtrusions.Item(1);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new protrusion to ActiveSelectSet.
                selectSet.Add(extrudedProtrusion);

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
