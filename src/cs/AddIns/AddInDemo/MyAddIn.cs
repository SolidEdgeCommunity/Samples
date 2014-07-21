using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AddInDemo
{
    [ComVisible(true)]
    [Guid("BF1C1BB8-75EE-444A-8DCE-0F1521D0764B")] // Must be unique!
    [ProgId("SolidEdge.Community.AddInDemo.MyAddIn")] // Must be unique!
    public class MyAddIn : SolidEdge.Community.AddIn.SolidEdgeAddIn
    {
        /// <summary>
        /// Called when the addin is first loaded by Solid Edge.
        /// </summary>
        public override void OnConnection(SolidEdgeFramework.Application application, SolidEdgeFramework.SeConnectMode ConnectMode, SolidEdgeFramework.AddIn AddInInstance)
        {
            // If you makes changes to your ribbon, be sure to increment the GuiVersion or your ribbon
            // will not initialize properly.
            AddInEx.GuiVersion = 1;
        }

        /// <summary>
        /// Called when the addin first connects to a new Solid Edge environment.
        /// </summary>
        public override void OnConnectToEnvironment(SolidEdgeFramework.Environment environment, bool firstTime)
        {
        }

        /// <summary>
        /// Called when the addin is about to be unloaded by Solid Edge.
        /// </summary>
        public override void OnDisconnection(SolidEdgeFramework.SeDisconnectMode DisconnectMode)
        {
        }

        /// <summary>
        /// Called when Solid Edge raises the SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() event.
        /// </summary>
        public override void OnCreateEdgeBarPage(SolidEdge.Community.AddIn.EdgeBarController controller, SolidEdgeFramework.SolidEdgeDocument document)
        {
            // Note: Due to a bug in the API, OnCreateEdgeBarPage does not get called when Solid Edge is first open and the first document is open.
            // i.e. Under the hood, SolidEdgeFramework.ISEAddInEdgeBarEvents[Ex].AddPage() is not getting called. I am currently verifying the bug with development.
            // As an alternative, you can call MyAddIn.Instance.EdgeBarController.Add() in some other event if you need.
            
            // Get the document type of the passed in document.
            var documentType = document.Type;

            // Depending on the document type, you may have different edgebar controls.
            switch (documentType)
            {
                case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                    controller.Add<MyEdgeBarControl>(document, 1);
                    break;
            }
        }

        /// <summary>
        /// Called directly after OnConnectToEnvironment() to give you an opportunity to configure a ribbon for a specific environment.
        /// </summary>
        public override void OnCreateRibbon(SolidEdge.Community.AddIn.RibbonController controller, Guid environmentCategory, bool firstTime)
        {
            // Depending on environment, you may or may not want to load different ribbons.
            if (environmentCategory.Equals(SolidEdge.CATID.SEDraftGuid))
            {
                // Draft Environment
                controller.Add<DraftRibbon>(environmentCategory, firstTime);
            }
            else if (environmentCategory.Equals(SolidEdge.CATID.SEPartGuid))
            {
                // Traditional Part Environment
                controller.Add<PartRibbon>(environmentCategory, firstTime);
            }
            else if (environmentCategory.Equals(SolidEdge.CATID.SEDMPartGuid))
            {
                // Synchronous Part Environment
                controller.Add<PartRibbon>(environmentCategory, firstTime);
            }
        }

        /// <summary>
        /// Called when regasm.exe is executed against the assembly.
        /// </summary>
        [ComRegisterFunction]
        public static void OnRegister(Type t)
        {
            string title = "SolidEdge.Community.AddInDemo.MyAddIn";
            string summary = "Solid Edge Addin in .NET 4.0.";
            var enabled = true; // You have the option to register the addin in a disabled state.

            // List of environments that your addin supports.
            Guid[] environments = {
                                        SolidEdge.CATID.SEApplicationGuid,
                                        SolidEdge.CATID.SEAllDocumentEnvrionmentsGuid
                                    };

            try
            {
                MyAddIn.Register(t, title, summary, enabled, environments);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        /// <summary>
        /// Called when regasm.exe /u is executed against the assembly.
        /// </summary>
        [ComUnregisterFunction]
        public static void OnUnregister(Type t)
        {
            MyAddIn.Unregister(t);
        }
    }
}
