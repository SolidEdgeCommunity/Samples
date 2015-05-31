using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeleteAllDrawingObjects
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeDraft.DraftDocument draftDocument = null;
            SolidEdgeDraft.Sheet sheet = null;
            SolidEdgeFrameworkSupport.DrawingObjects drawingObjects = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the active draft document.
                draftDocument = application.GetActiveDocument<SolidEdgeDraft.DraftDocument>();

                // Get a reference to the active Sheet.
                sheet = draftDocument.ActiveSheet;

                // Get a reference to the drawing objects collection.
                drawingObjects = sheet.DrawingObjects;

                // Disable screen updating for performance.
                application.ScreenUpdating = false;

                // Loop until count is 0.
                while (drawingObjects.Count > 0)
                {
                    // Leverage dynamic keyword to allow invoking Delete() method.
                    dynamic drawingObject = drawingObjects.Item(1);

                    if (drawingObject is SolidEdgeDraft.TablePage)
                    {
                        // TablePage does not have a Delete() method but its parent does.
                        dynamic drawingObjectParent = drawingObject.Parent;
                        drawingObjectParent.Delete();
                    }
                    else
                    {
                        drawingObject.Delete();
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
