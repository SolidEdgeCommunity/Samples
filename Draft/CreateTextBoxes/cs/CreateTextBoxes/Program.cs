using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateTextBoxes
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
            SolidEdgeFrameworkSupport.TextBoxes textBoxes = null;
            SolidEdgeFrameworkSupport.TextBox textBox = null;

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
                SolidEdgeCommunity.OleMessageFilter.Unregister();
            }
        }
    }
}
