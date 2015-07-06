using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SolidEdge.MouseEvents
{
    public partial class MainForm : Form
    {
        private SolidEdgeFramework.Application _application = null;
        private SolidEdgeFramework.Command _command = null;
        private SolidEdgeFramework.Mouse _mouse = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SolidEdgeCommunity.OleMessageFilter.Register();

            comboBoxEnableMouseMoveEvent.Items.AddRange(new object[] { true, false });
            comboBoxEnableMouseMoveEvent.SelectedIndex = 1;
            LoadLocateModes();
            LoadFilters();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonStartCommand_Click(object sender, EventArgs e)
        {
            try
            {
                _application = SolidEdgeCommunity.SolidEdgeUtils.Connect();
                _command = _application.CreateCommand((int)SolidEdgeConstants.seCmdFlag.seNoDeactivate);
                _command.Terminate += command_Terminate;
                _command.Start();
                _mouse = _command.Mouse;
                _mouse.LocateMode = comboBoxLocateModes.SelectedIndex;
                _mouse.EnabledMove = (bool)comboBoxEnableMouseMoveEvent.SelectedItem;
                _mouse.ScaleMode = 1;   // Design model coordinates.
                _mouse.WindowTypes = 1; // Graphic window's only.

                foreach (ListViewItem listViewItem in listViewFilters.CheckedItems)
                {
                    int filter = (int)listViewItem.Tag;
                    _mouse.AddToLocateFilter(filter);
                }

                _mouse.MouseDown += mouse_MouseDown;
                _mouse.MouseMove += mouse_MouseMove;

                outputTextBox.Clear();
                comboBoxEnableMouseMoveEvent.Enabled = false;
                buttonStopCommand.Enabled = true;
                buttonStartCommand.Checked = true;
                buttonStartCommand.Enabled = false;
                comboBoxLocateModes.Enabled = false;
                listViewFilters.Enabled = false;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonStopCommand_Click(object sender, EventArgs e)
        {
            if (_command != null)
            {
                _command.Done = true;
            }
        }

        private void LoadLocateModes()
        {
            Type type = typeof(SolidEdgeConstants.seLocateModes);

            FieldInfo[] fields = type.GetFields();
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (FieldInfo field in fields)
            {
                if (field.IsSpecialName) continue;

                comboBoxLocateModes.Items.Add(field.Name);
            }

            comboBoxLocateModes.SelectedIndex = (int)SolidEdgeConstants.seLocateModes.seLocateQuickPick;
        }

        private void LoadFilters()
        {
            Type type = typeof(SolidEdgeConstants.seLocateFilterConstants);

            FieldInfo[] fields = type.GetFields();
            List<ListViewItem> items = new List<ListViewItem>();

            foreach (FieldInfo field in fields)
            {
                if (field.IsSpecialName) continue;

                ListViewItem item = new ListViewItem(field.Name);
                item.Tag = field.GetRawConstantValue();
                items.Add(item);
            }

            items.Sort(delegate(ListViewItem x, ListViewItem y)
            {
                return (x.Text.CompareTo(y.Text));
            });

            listViewFilters.Items.AddRange(items.ToArray());
            listViewFilters.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void HandleCommandTerminate()
        {
            listViewFilters.Enabled = true;
            comboBoxEnableMouseMoveEvent.Enabled = true;
            comboBoxLocateModes.Enabled = true;
            buttonStartCommand.Checked = false;
            buttonStartCommand.Enabled = true;
            buttonStopCommand.Enabled = false;
        }

        private void LogEvent(string eventName, short sButton, short sShift, double dX, double dY, double dZ, object pWindowDispatch, int lKeyPointType, object pGraphicDispatch)
        {
            if (_mouse == null) return;

            List<string> entries = new List<string>();

            Type windowDispatchType = null;
            Type graphicDispatchType = null;

            if (pWindowDispatch != null)
            {
                windowDispatchType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(pWindowDispatch);
            }

            if (pGraphicDispatch != null)
            {
                graphicDispatchType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(pGraphicDispatch);
            }

            entries.Add("MouseDown event");
            entries.Add(String.Format("sButton: '{0}'", (MouseButtons)sButton));
            entries.Add(String.Format("sShift: '{0}'", sShift));
            entries.Add(String.Format("dX: '{0}'", dX));
            entries.Add(String.Format("dY: '{0}'", dY));
            entries.Add(String.Format("dZ: '{0}'", dZ));
            entries.Add(String.Format("pWindowDispatch: '{0}'", windowDispatchType));
            entries.Add(String.Format("lKeyPointType: '{0}'", lKeyPointType));
            entries.Add(String.Format("pGraphicDispatch: '{0}'", graphicDispatchType));

            if (pGraphicDispatch is SolidEdgeFramework.Reference)
            {
                var reference = (SolidEdgeFramework.Reference)pGraphicDispatch;
                var referenceObject = reference.Object;
                var referenceObjectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(referenceObject);
                entries.Add(String.Format("pGraphicDispatch reference: '{0}'", referenceObjectType));
            }

            int PointOnGraphicFlag;
            double PointOnGraphic_X;
            double PointOnGraphic_Y;
            double PointOnGraphic_Z;

            // Get the actual point on the graphic element (in database coordinates). Note this is not
            // the same as the input dX, dY, dZ coordinates. Those are either in window coordinates
            // or are the window coordinates transformed to data base coordinates (depends on the
            // value of ScaleMode). That is, the inputs to this routine are NOT the intersection of the 
            // bore line and the element.
            _mouse.PointOnGraphic(out PointOnGraphicFlag, out PointOnGraphic_X, out PointOnGraphic_Y, out PointOnGraphic_Z);

            // It would seem that the PointOnGraphicFlag only gets set on MouseDown and stays set until the next MouseDown event.
            if (PointOnGraphicFlag == 1)
            {
                entries.Add(String.Format("PointOnGraphicFlag: '{0}'", PointOnGraphicFlag));
                entries.Add(String.Format("PointOnGraphic_X: '{0}'", PointOnGraphic_X));
                entries.Add(String.Format("PointOnGraphic_Y: '{0}'", PointOnGraphic_Y));
                entries.Add(String.Format("PointOnGraphic_Z: '{0}'", PointOnGraphic_Z));
            }

            entries.Add(String.Empty);

            StringBuilder sb = new StringBuilder();
            foreach (string entry in entries)
            {
                sb.AppendLine(entry);
            }

            outputTextBox.AppendText(sb.ToString());
        }

        void command_Terminate()
        {
            this.Do(frm =>
            {
                frm.HandleCommandTerminate();
            });

            _mouse = null;
            _command = null;
            _application = null;
        }

        void mouse_MouseDown(short sButton, short sShift, double dX, double dY, double dZ, object pWindowDispatch, int lKeyPointType, object pGraphicDispatch)
        {
            // Note: Thread.CurrentThread.IsBackground = true so we must Invoke a call back to the main GUI thread.
            this.Do(frm =>
            {
                frm.LogEvent("MouseDown", sButton, sShift, dX, dY, dZ, pWindowDispatch, lKeyPointType, pGraphicDispatch);
            });
        }

        void mouse_MouseMove(short sButton, short sShift, double dX, double dY, double dZ, object pWindowDispatch, int lKeyPointType, object pGraphicDispatch)
        {
            // Note: Thread.CurrentThread.IsBackground = true so we must Invoke a call back to the main GUI thread.
            this.Do(frm =>
            {
                frm.LogEvent("MouseMove", sButton, sShift, dX, dY, dZ, pWindowDispatch, lKeyPointType, pGraphicDispatch);
            });
        }
    }
}
