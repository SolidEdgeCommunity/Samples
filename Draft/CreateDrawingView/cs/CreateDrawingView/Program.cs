using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateDrawingView
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.ModelLinks modelLinks = null;
            SolidEdgeDraft.ModelLink modelLink = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeDraft.DrawingViews drawingViews = null;
            SolidEdgeDraft.DrawingView drawingView = null;
            string filename = null;

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

                // Build path to part file.
                filename = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath(), "Coffee Pot.asm");

                // Get a reference to the ModelLinks collection.
                modelLinks = draftDocument.ModelLinks;

                // Add a new model link.
                modelLink = modelLinks.Add(filename);

                // Get a reference to the DrawingViews collection.
                drawingViews = sheet.DrawingViews;

                // Add a new part drawing view.
                drawingView = drawingViews.AddAssemblyView(
                    From: modelLink,
                    Orientation: SolidEdgeDraft.ViewOrientationConstants.igDimetricTopBackLeftView,
                    Scale: 1.0,
                    x: 0.4,
                    y: 0.4,
                    ViewType: SolidEdgeDraft.AssemblyDrawingViewTypeConstants.seAssemblyDesignedView);
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
