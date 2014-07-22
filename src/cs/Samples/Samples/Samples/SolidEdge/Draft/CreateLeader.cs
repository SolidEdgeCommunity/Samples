using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Creates a new draft with multiple leaders.
    /// </summary>
    class CreateLeader
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
            SolidEdgeFrameworkSupport.Leaders leaders = null;
            SolidEdgeFrameworkSupport.Leader leader = null;
            SolidEdgeFrameworkSupport.DimStyle dimStyle = null;

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

                // Add a new text box.
                textBox = textBoxes.Add(0.25, 0.25, 0);
                textBox.TextScale = 1;
                textBox.VerticalAlignment = SolidEdgeFrameworkSupport.TextVerticalAlignmentConstants.igTextHzAlignVCenter;
                textBox.Text = "Leader";

                // Get a reference to the Leaders collection.
                leaders = (SolidEdgeFrameworkSupport.Leaders)sheet.Leaders;

                Console.WriteLine("Creating a new leader. ");

                // Add a new leader.
                leader = leaders.Add(0.225, 0.225, 0, 0.25, 0.25, 0);
                dimStyle = leader.Style;
                dimStyle.FreeSpaceTerminatorType = SolidEdgeFrameworkSupport.DimTermTypeConstants.igDimStyleTermFilled;
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
