using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateCirclesWithDimensions
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeFrameworkSupport.Circles2d circles2d = null;
            SolidEdgeFrameworkSupport.Circle2d circle2d = null;
            SolidEdgeFrameworkSupport.Dimensions dimensions = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new draft document.
                draftDocument = documents.AddDraftDocument();

                // Get a reference to the active sheet.
                sheet = draftDocument.ActiveSheet;

                // Get a reference to the Circles2d collection.
                circles2d = sheet.Circles2d;

                // Get a reference to the Dimensions collection.
                dimensions = (SolidEdgeFrameworkSupport.Dimensions)sheet.Dimensions;

                double x = 0.1;
                double y = 0.1;
                double radius = 0.01;

                for (int i = 0; i < 5; i++)
                {
                    // Add the circle.
                    circle2d = circles2d.AddByCenterRadius(x, y, radius);

                    // Dimension the circle.
                    dimensions.AddRadialDiameter(circle2d);

                    x += 0.05;
                    y += 0.05;
                    radius += 0.01;
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
