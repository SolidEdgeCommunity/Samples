using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreatePartDrawingView
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
            SolidEdgeDraft.DVLines2d dvLines2d = null;
            SolidEdgeDraft.DVLine2d dvLine2d = null;
            SolidEdgeFrameworkSupport.Dimensions dimensions = null;
            SolidEdgeFrameworkSupport.Dimension dimension = null;
            SolidEdgeFrameworkSupport.DimStyle dimStyle = null;
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

                // Get a reference to the ModelLinks collection.
                modelLinks = draftDocument.ModelLinks;

                // Build path to part file.
                filename = System.IO.Path.Combine(SolidEdgeCommunity.SolidEdgeUtils.GetTrainingFolderPath(), "2holebar.par");

                // Add a new model link.
                modelLink = modelLinks.Add(filename);

                // Get a reference to the active sheet.
                sheet = draftDocument.ActiveSheet;

                // Get a reference to the DrawingViews collection.
                drawingViews = sheet.DrawingViews;

                // Add a new part drawing view.
                drawingView = drawingViews.AddPartView(
                    From: modelLink,
                    Orientation: SolidEdgeDraft.ViewOrientationConstants.igDimetricTopBackLeftView,
                    Scale: 5.0,
                    x: 0.4,
                    y: 0.4,
                    ViewType: SolidEdgeDraft.PartDrawingViewTypeConstants.sePartDesignedView);

                // Get a reference to the DVLines2d collection.
                dvLines2d = drawingView.DVLines2d;

                // Get the 1st drawing view 2D line.
                dvLine2d = dvLines2d.Item(1);

                // Get a reference to the Dimensions collection.
                dimensions = (SolidEdgeFrameworkSupport.Dimensions)sheet.Dimensions;

                // Add a dimension to the line.
                dimension = dimensions.AddLength(dvLine2d.Reference);

                // Few changes to make the dimensions look right.
                dimension.ProjectionLineDirection = true;
                dimension.TrackDistance = 0.02;

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
