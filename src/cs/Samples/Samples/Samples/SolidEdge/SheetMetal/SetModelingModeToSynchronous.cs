﻿using SolidEdgeCommunity; //SolidEdge.Community.dll
using SolidEdgeFramework.Extensions; //SolidEdge.Community.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples.SolidEdge.SheetMetal
{
    /// <summary>
    /// Sets the ModelingMode of the active sheetmetal to synchronous.
    /// </summary>
    class SetModelingModeToSynchronous
    {
        static void RunSample(bool breakOnStart)
        {
            if (breakOnStart) System.Diagnostics.Debugger.Break(); 
            
            SolidEdgeFramework.Application application = null;
            SolidEdgePart.SheetMetalDocument sheetMetalDocument = null;

            try
            {
                // Register with OLE to handle concurrency issues on the current thread.
                OleMessageFilter.Register();

                // Connect to or start Solid Edge.
                application = SolidEdgeCommunity.SolidEdgeInstall.Connect(true);

                // Make sure user can see the GUI.
                application.Visible = true;

                // Bring Solid Edge to the foreground.
                application.Activate();

                // Get a reference to the active sheetmetal document.
                sheetMetalDocument = application.GetActiveDocument<SolidEdgePart.SheetMetalDocument>(false);

                if (sheetMetalDocument != null)
                {
                    sheetMetalDocument.ModelingMode = SolidEdgePart.ModelingModeConstants.seModelingModeSynchronous;
                }
                else
                {
                    throw new System.Exception(Resources.NoActiveSheetMetalDocument);
                }
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
