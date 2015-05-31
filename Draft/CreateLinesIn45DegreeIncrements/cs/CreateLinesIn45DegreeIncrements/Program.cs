using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateLinesIn45DegreeIncrements
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
            double lineLength = 3.0; // Inches

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

                // Work with angle in degrees.
                for (int angle = 0; angle < 360; angle += 45)
                {
                    // {x1, y1, x2, y2}
                    double[] startPoint = { 0.2, 0.2 };
                    double[] endPoint = { 0.3, 0.2 };

                    // Add the line.
                    line2d = lines2d.AddBy2Points(
                        x1: startPoint[0],
                        y1: startPoint[1],
                        x2: endPoint[0],
                        y2: endPoint[1]);

                    // Set the line length by converting inches to meters.
                    line2d.Length = lineLength * 0.0254;

                    // Set the angle by converting degrees to radians.
                    line2d.Angle = (Math.PI / 180) * angle;
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
