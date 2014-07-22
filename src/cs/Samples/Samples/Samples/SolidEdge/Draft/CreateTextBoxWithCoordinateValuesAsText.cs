﻿using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Creates a new draft with multiple text boxes with coordinates as text.
    /// </summary>
    class CreateTextBoxWithCoordinateValuesAsText
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeFrameworkSupport.TextBoxes textBoxes = null;
            SolidEdgeFrameworkSupport.TextBox textBox = null;

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

                // Get a reference to the TextBoxes collection.
                textBoxes = (SolidEdgeFrameworkSupport.TextBoxes)sheet.TextBoxes;

                // Disable screen updating for performance.
                application.ScreenUpdating = false;

                double x = 0;
                double y = 0;

                for (int i = 0; i < 10; i++)
                {
                    x += .05;
                    y = 0;
                    for (int j = 0; j < 50; j++)
                    {
                        y += .01;
                        // Add a new text box.
                        textBox = textBoxes.Add(x, y, 0);
                        textBox.TextScale = 1;
                        textBox.VerticalAlignment = SolidEdgeFrameworkSupport.TextVerticalAlignmentConstants.igTextHzAlignVCenter;
                        textBox.Text = String.Format("[X: {0:0.###} Y: {1:0.###}]", x, y);
                    }
                }

                // Turn screen updating back on.
                application.ScreenUpdating = true;

                // Fit the view.
                application.StartCommand(SolidEdgeConstants.DetailCommandConstants.DetailViewFit);
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
