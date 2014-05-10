using SolidEdgeContrib.AddIn;
using SolidEdgeContrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AddInDemo
{
    [ComVisible(true)]
    [Guid("00000000-0000-0000-0000-50FC26B3C501")]
    [ProgId("SolidEdge.Samples.AddInDemo")]
    public class MyAddIn : SolidEdgeContrib.AddIn.SolidEdgeAddIn
    {
        public override void OnConnection(SolidEdgeFramework.Application application, SolidEdgeFramework.SeConnectMode ConnectMode, SolidEdgeFramework.AddIn AddInInstance)
        {
            base.OnConnection(application, ConnectMode, AddInInstance);

            // Put your custom OnConnection code here.
            var applicationEvents = application.GetApplicationEvents();
            applicationEvents.AfterWindowActivate += applicationEvents_AfterWindowActivate;
        }

        public override void OnConnectToEnvironment(SolidEdgeFramework.Environment environment, bool firstTime)
        {
            base.OnConnectToEnvironment(environment, firstTime);

            // Put your custom OnConnectToEnvironment code here.
        }

        public override void OnDisconnection(SolidEdgeFramework.SeDisconnectMode DisconnectMode)
        {
            base.OnDisconnection(DisconnectMode);

            // Put your custom OnDisconnection code here.
        }

        public override void OnInitializeRibbon(Ribbon ribbon, bool firstTime)
        {
            // Let the base class handle initializing the ribbon via addin.manifest.
            base.OnInitializeRibbon(ribbon, firstTime);

            // Now we can customize the ribbon.
            var boxButton = ribbon.LookupControl("Box");

            if (boxButton != null)
            {
                boxButton.Enabled = false;
            }
        }

        public override void OnRibbonControl(RibbonControl ribbonControl)
        {
            // Base class doesn't do anything special when a ribbon control is invoked unless a macro is assigned.
            //base.OnRibbonControl(ribbonControl);

            if (ribbonControl.Name.Equals("Save"))
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    // The ShowDialog() extension method is exposed by: using SolidEdgeContrib.Extensions
                    if (this.Application.ShowDialog(dialog) == DialogResult.OK)
                    {
                    }
                }
            }
            else if (ribbonControl.Name.Equals("Folder"))
            {
                using (FolderBrowserDialog dialog = new FolderBrowserDialog())
                {
                    // The ShowDialog() extension method is exposed by: using SolidEdgeContrib.Extensions
                    if (this.Application.ShowDialog(dialog) == DialogResult.OK)
                    {
                    }
                }
            }
            else if (ribbonControl.Name.Equals("Monitor"))
            {
                using (CustomDialog dialog = new CustomDialog())
                {
                    // The ShowDialog() extension method is exposed by: using SolidEdgeContrib.Extensions
                    if (this.Application.ShowDialog(dialog) == DialogResult.OK)
                    {
                    }
                }
            }
            else if (ribbonControl.Name.Equals("Tools"))
            {
                this.Application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartToolsOptions);
            }
            else if (ribbonControl.Name.Equals("Help"))
            {
                this.Application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartHelpSolidEdgeontheWeb);
            }
            else if (ribbonControl.Name.Equals("BoundingBox"))
            {
                // Toggle the check state.
                ribbonControl.Checked = !ribbonControl.Checked;

                var window = this.Application.ActiveWindow as SolidEdgeFramework.Window;

                if (window != null)
                {
                    // Find the relevant overlay(s).
                    var overlays = this.ViewOverlays.OfType<MyViewOverlay>().Where(x => x.Window.Equals(window));

                    foreach (var overlay in overlays)
                    {
                        overlay.ShowBoundingBox = ribbonControl.Checked;

                        // For the view to update.
                        window.View.Update();
                    }
                }
            }
            else if (ribbonControl.Name.Equals("OpenGLBoxes"))
            {
                // Toggle the check state.
                ribbonControl.Checked = !ribbonControl.Checked;

                var window = this.Application.ActiveWindow as SolidEdgeFramework.Window;

                if (window != null)
                {
                    // Find the relevant overlay(s).
                    var overlays = this.ViewOverlays.OfType<MyViewOverlay>().Where(x => x.Window.Equals(window));

                    foreach (var overlay in overlays)
                    {
                        overlay.ShowOpenGlBoxes = ribbonControl.Checked;

                        // For the view to update.
                        window.View.Update();
                    }
                }
            }
            else if (ribbonControl.Name.Equals("GDIPlus"))
            {
                // Toggle the check state.
                ribbonControl.Checked = !ribbonControl.Checked;

                var window = this.Application.ActiveWindow as SolidEdgeFramework.Window;

                if (window != null)
                {
                    // Find the relevant overlay(s).
                    var overlays = this.ViewOverlays.OfType<MyViewOverlay>().Where(x => x.Window.Equals(window));

                    foreach (var overlay in overlays)
                    {
                        overlay.ShowGDIPlus = ribbonControl.Checked;

                        // For the view to update.
                        window.View.Update();
                    }
                }
            }
            else
            {
                // Demonstrate toggling the check state.
                ribbonControl.Checked = !ribbonControl.Checked;
            }
        }

        public override string NativeResourcesDllPath
        {
            get
            {
                // You can override the path to your native images if you need.
                return base.NativeResourcesDllPath;
            }
        }

        void applicationEvents_AfterWindowActivate(object theWindow)
        {
            var window = theWindow as SolidEdgeFramework.Window;

            if (window != null)
            {
                // Add the overlay.
                this.ViewOverlays.Add<MyViewOverlay>(window);
            }
        }

        [ComRegisterFunction]
        public static void OnRegister(Type t)
        {
            try
            {
                // SolidEdgeAddIn.Register() will throw an exception if it cannot locate an embedded resource named [DEFAULT_NAMESPACE].addin.manifest.
                // If you want to take control of the registration process, simply don't call SolidEdgeAddIn.Register().
                SolidEdgeContrib.AddIn.SolidEdgeAddIn.Register(t);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }

            // Perform any additional registration procedures if needed.
        }

        [ComUnregisterFunction]
        public static void OnUnregister(Type t)
        {
            SolidEdgeContrib.AddIn.SolidEdgeAddIn.Unregister(t);
        }
    }
}
