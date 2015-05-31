using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateEtch
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Models models = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.Sketchs sketchs = null;
            SolidEdgePart.Sketch sketch = null;
            SolidEdgePart.Profiles profiles = null;
            SolidEdgePart.Profile profile = null;
            SolidEdgePart.Etches etches = null;
            SolidEdgePart.Etch etch = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                SolidEdgeCommunity.OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true, true);

                // Get a reference to the documents collection.
                documents = application.Documents;

                // Add a new sheet metal document.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Invoke existing sample to create geometry.
                SheetMetalHelper.CreateHolesWithUserDefinedPattern(sheetMetalDocument);

                // Get a reference to the Sketches collection.
                sketchs = sheetMetalDocument.Sketches;

                // Get the 1st Sketch.
                sketch = sketchs.Item(1);

                // Get a reference to the Profiles collection.
                profiles = sketch.Profiles;

                // Get the 1st Profile.
                profile = profiles.Item(1);

                // Get a reference to the Models collection.
                models = sheetMetalDocument.Models;

                // Get the 1st Model.
                model = models.Item(1);

                // Get a reference to the Etches collection.
                etches = model.Etches;

                // Add the Etch.
                etch = etches.Add(profile);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new Dimple to ActiveSelectSet.
                selectSet.Add(etch);

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView);
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
