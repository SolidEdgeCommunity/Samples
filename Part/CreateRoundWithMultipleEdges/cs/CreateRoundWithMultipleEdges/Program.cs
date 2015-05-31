using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateRoundWithMultipleEdges
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
            SolidEdgeGeometry.Edges edges = null;
            SolidEdgePart.Rounds rounds = null;
            SolidEdgePart.Round round = null;
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

                // Get a all Edges.
                edges = (SolidEdgeGeometry.Edges)revolvedProtrusion.Edges[SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll];

                List<SolidEdgeGeometry.Edge> edgeList = new List<SolidEdgeGeometry.Edge>();
                List<double> radiusList = new List<double>();

                // Build arrays.
                foreach (SolidEdgeGeometry.Edge edge in edges)
                {
                    edgeList.Add(edge);
                    radiusList.Add(0.002);
                }

                // Get a reference to the Rounds collection.
                rounds = model.Rounds;

                // Add single round with multiple Edges.
                round = rounds.Add(edgeList.Count, edgeList.ToArray(), radiusList.ToArray());

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new Round to ActiveSelectSet.
                selectSet.Add(round);

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
