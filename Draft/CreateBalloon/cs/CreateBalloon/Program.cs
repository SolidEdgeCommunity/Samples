using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateBalloon
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
            SolidEdgeFrameworkSupport.Balloons balloons = null;
            SolidEdgeFrameworkSupport.Balloon balloon = null;
            SolidEdgeFrameworkSupport.DimStyle dimStype = null;

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

                // Get a reference to the balloons collection.
                balloons = (SolidEdgeFrameworkSupport.Balloons)sheet.Balloons;

                balloon = balloons.Add(0.05, 0.05, 0);
                balloon.TextScale = 1.0;
                balloon.BalloonText = "B";
                balloon.Leader = true;
                balloon.BreakLine = true;
                balloon.BalloonSize = 3;
                balloon.SetTerminator(balloon, 0, 0, 0, false);
                balloon.BalloonType = SolidEdgeFrameworkSupport.DimBalloonTypeConstants.igDimBalloonCircle;

                dimStype = balloon.Style;
                dimStype.TerminatorType = SolidEdgeFrameworkSupport.DimTermTypeConstants.igDimStyleTermFilled;

                balloon = balloons.Add(0.1, 0.1, 0);
                balloon.Callout = 1;
                balloon.TextScale = 1.0;
                balloon.BalloonText = "This is a callout";
                balloon.Leader = true;
                balloon.BreakLine = true;
                balloon.BalloonSize = 3;
                balloon.SetTerminator(balloon, 0, 0, 0, false);
                balloon.BalloonType = SolidEdgeFrameworkSupport.DimBalloonTypeConstants.igDimBalloonCircle;

                dimStype = balloon.Style;
                dimStype.TerminatorType = SolidEdgeFrameworkSupport.DimTermTypeConstants.igDimStyleTermFilled;
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
