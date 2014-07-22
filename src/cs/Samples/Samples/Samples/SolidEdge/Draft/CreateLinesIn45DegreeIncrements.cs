using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Creates a new draft and draws multiple lines in 45 degree increments.
    /// </summary>
    class CreateLinesIn45DegreeIncrements
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

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
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

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
                OleMessageFilter.Unregister();
            }
        }
    }
}
