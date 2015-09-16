using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportSolidBodies
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.PartDocument partDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgeGeometry.Body body = null;
            SolidEdgeGeometry.Shells shells = null;
            SolidEdgeGeometry.Shell shell = null;
            SolidEdgePart.Constructions constructions = null;
            SolidEdgePart.ConstructionModel constructionModel = null;
            int bodyCount = 0;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active part document.
                partDocument = application.GetActiveDocument<SolidEdgePart.PartDocument>(false);

                if (partDocument != null)
                {
                    models = partDocument.Models;

                    // Check for solid design bodies.
                    foreach (var model in models.OfType<SolidEdgePart.Model>())
                    {
                        body = (SolidEdgeGeometry.Body)model.Body;

                        if (body.IsSolid)
                        {
                            shells = (SolidEdgeGeometry.Shells)body.Shells;

                            for (int i = 1; i <= shells.Count; i++)
                            {
                                shell = (SolidEdgeGeometry.Shell)shells.Item(i);

                                if (shell != null)
                                {
                                    if ((shell.IsClosed) && (shell.IsVoid == false))
                                    {
                                        bodyCount++;
                                    }
                                }
                            }
                        }
                    }

                    // Check for solid construction bodies.
                    constructions = partDocument.Constructions;
                    
                    for (int i = 1; i <= constructions.Count; i++)
                    {
                        constructionModel = constructions.Item(i) as SolidEdgePart.ConstructionModel;

                        if (constructionModel != null)
                        {
                            body = (SolidEdgeGeometry.Body)constructionModel.Body;

                            if (body.IsSolid)
                            {
                                bodyCount++;
                            }
                        }
                    }

                    Console.WriteLine("Active part document contains ({0}) solid bodies.", bodyCount);
                }
                else
                {
                    throw new System.Exception("No active document.");
                }
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
