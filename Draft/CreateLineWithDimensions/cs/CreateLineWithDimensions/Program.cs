using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateLineWithDimensions
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
            SolidEdgeFrameworkSupport.Lines2d lines2d = null;
            SolidEdgeFrameworkSupport.Line2d line2d = null;
            SolidEdgeFrameworkSupport.Dimensions dimensions = null;
            SolidEdgeFrameworkSupport.Dimension dimension = null;
            SolidEdgeFrameworkSupport.DimStyle dimStyle = null;

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

                // Get a reference to the Lines2d collection.
                lines2d = sheet.Lines2d;

                // Draw a new line.
                line2d = lines2d.AddBy2Points(
                    x1: 0.2,
                    y1: 0.2,
                    x2: 0.3,
                    y2: 0.2);

                // Get a reference to the Dimensions collection.
                dimensions = (SolidEdgeFrameworkSupport.Dimensions)sheet.Dimensions;

                // Add a dimension to the line.
                dimension = dimensions.AddLength(line2d);

                // Get a reference to the dimension style.
                // DimStyle has a ton of options...
                dimStyle = dimension.Style;
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
