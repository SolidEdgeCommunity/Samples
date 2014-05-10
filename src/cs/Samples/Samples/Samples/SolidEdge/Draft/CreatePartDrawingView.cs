using ApiSamples.Samples.SolidEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.Draft
{
    /// <summary>
    /// Creates a new draft with a drawing view of a part.
    /// </summary>
    class CreatePartDrawingView
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

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
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = ApplicationHelper.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Create a new draft document.
                draftDocument = documents.AddDraftDocument();

                // Get a reference to the ModelLinks collection.
                modelLinks = draftDocument.ModelLinks;

                // Build path to part file.
                filename = System.IO.Path.Combine(InstallDataHelper.GetTrainingFolderPath(), "2holebar.par");

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
