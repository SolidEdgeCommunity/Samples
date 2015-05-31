using SolidEdgeCommunity.AddIn;
using SolidEdgeCommunity.Extensions; // https://github.com/SolidEdgeCommunity/SolidEdge.Community/wiki/Using-Extension-Methods
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoAddIn
{
    class Ribbon3d : SolidEdgeCommunity.AddIn.Ribbon
    {
        const string _embeddedResourceName = "DemoAddIn.Ribbon3d.xml";
        private RibbonButton _buttonBoundingBox;
        private RibbonButton _buttonOpenGlBoxes;
        private RibbonButton _buttonGdiPlus;

        public Ribbon3d()
        {
            // Get a reference to the current assembly. This is where the ribbon XML is embedded.
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // In this example, XML file must have a build action of "Embedded Resource".
            this.LoadXml(assembly, _embeddedResourceName);

            // Example of how to bind a local variable to a particular ribbon control.
            _buttonBoundingBox = GetButton(20);
            _buttonOpenGlBoxes = GetButton(21);
            _buttonGdiPlus = GetButton(22);

            // Example of how to bind a particular ribbon control click event.
            _buttonBoundingBox.Click += _buttonBoundingBox_Click;
            _buttonOpenGlBoxes.Click += _buttonOpenGlBoxes_Click;
            _buttonGdiPlus.Click += _buttonGdiPlus_Click;

            // Get the Solid Edge version.
            var version = DemoAddIn.Instance.SolidEdgeVersion;

            // View.GetModelRange() is only available in ST6 or greater.
            if (version.Major < 106)
            {
                _buttonBoundingBox.Enabled = false;
            }
        }

        public override void OnControlClick(RibbonControl control)
        {
            var application = DemoAddIn.Instance.Application;

            // Demonstrate how to handle commands without binding to a local variable.
            switch (control.CommandId)
            {
                case 0:
                    using (var dialog = new SaveFileDialog())
                    {
                        // The ShowDialog() extension method is exposed by:
                        // using SolidEdgeFramework.Extensions (SolidEdge.Community.dll)
                        if (application.ShowDialog(dialog) == DialogResult.OK)
                        {

                        }
                    }
                    break;
                case 1:
                    using (var dialog = new FolderBrowserDialog())
                    {
                        // The ShowDialog() extension method is exposed by:
                        // using SolidEdgeFramework.Extensions (SolidEdge.Community.dll)
                        if (application.ShowDialog(dialog) == DialogResult.OK)
                        {
                        }
                    }
                    break;
                case 2:
                    using (var dialog = new MyCustomDialog())
                    {
                        // The ShowDialog() extension method is exposed by:
                        // using SolidEdgeFramework.Extensions (SolidEdge.Community.dll)
                        if (application.ShowDialog(dialog) == DialogResult.OK)
                        {
                        }
                    }
                    break;
                case 8:
                    application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartToolsOptions);
                    break;
                case 11:
                    application.StartCommand(SolidEdgeConstants.PartCommandConstants.PartHelpSolidEdgeontheWeb);
                    break;
            }
        }

        void _buttonGdiPlus_Click(RibbonControl control)
        {
            var overlay = GetActiveOverlay();

            // Toggle the button check state.
            _buttonGdiPlus.Checked = !_buttonGdiPlus.Checked;
            overlay.ShowGDIPlus = _buttonGdiPlus.Checked;
        }

        void _buttonOpenGlBoxes_Click(RibbonControl control)
        {
            var overlay = GetActiveOverlay();

            // Toggle the button check state.
            _buttonOpenGlBoxes.Checked = !_buttonOpenGlBoxes.Checked;
            overlay.ShowOpenGlBoxes = _buttonOpenGlBoxes.Checked;
        }

        void _buttonBoundingBox_Click(RibbonControl control)
        {
            var overlay = GetActiveOverlay();

            // Toggle the button check state.
            _buttonBoundingBox.Checked = !_buttonBoundingBox.Checked;
            overlay.ShowBoundingBox = _buttonBoundingBox.Checked;
        }

        private MyViewOverlay GetActiveOverlay()
        {
            var controlller = DemoAddIn.Instance.ViewOverlayController;
            var window = (SolidEdgeFramework.Window)DemoAddIn.Instance.Application.ActiveWindow;
            var overlay = (MyViewOverlay)controlller.GetOverlay(window);

            if (overlay == null)
            {
                // If the overlay has not been created yet, add a new one.
                overlay = controlller.Add<MyViewOverlay>(window);
            }

            return overlay;
        }
    }
}
