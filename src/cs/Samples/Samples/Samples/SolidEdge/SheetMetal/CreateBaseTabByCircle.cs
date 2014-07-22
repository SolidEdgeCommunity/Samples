using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.SheetMetal
{
    /// <summary>
    /// Creates a new sheetmetal with a base tab by circle.
    /// </summary>
    class CreateBaseTabByCircle
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break();

            SolidEdgeFramework.Application application = null;
            SolidEdgeFramework.Documents documents = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;
            SolidEdgePart.Model model = null;
            SolidEdgePart.Tabs tabs = null;
            SolidEdgePart.Tab tab = null;
            SolidEdgeFramework.SelectSet selectSet = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true, true);

                // Get a reference to the Documents collection.
                documents = application.Documents;

                // Create a new sheetmetal document.
                sheetMetalDocument = documents.AddSheetMetalDocument();

                // Always a good idea to give SE a chance to breathe.
                application.DoIdle();

                // Call helper method to create the actual geometry.
                model = SheetMetalHelper.CreateBaseTabByCircle(sheetMetalDocument);

                // Get a reference to the Tabs collection.
                tabs = model.Tabs;

                // Get a reference to the new Tab.
                tab = tabs.Item(1);

                // Get a reference to the ActiveSelectSet.
                selectSet = application.ActiveSelectSet;

                // Empty ActiveSelectSet.
                selectSet.RemoveAll();

                // Add new Tab to ActiveSelectSet.
                selectSet.Add(tab);

                // Switch to ISO view.
                application.StartCommand(SolidEdgeConstants.SheetMetalCommandConstants.SheetMetalViewISOView);
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
