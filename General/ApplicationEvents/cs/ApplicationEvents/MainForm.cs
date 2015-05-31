using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using SolidEdge.ApplicationEvents.Properties;

namespace SolidEdge.ApplicationEvents
{
    public partial class MainForm : Form
    {
        private SynchronizationContext _uiContext;
        private SolidEdgeFramework.Application _application = null;
        private SolidEdgeFramework.ISEApplicationEvents_Event _applicationEvents;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _uiContext = SynchronizationContext.Current;

            imageList1.Images.Add(Resources.Event_16x16);

            // Register with OLE to handle concurrency issues on the current thread.
            SolidEdgeCommunity.OleMessageFilter.Register();

            try
            {
                _application = SolidEdgeCommunity.SolidEdgeUtils.Connect();
                eventButton.Checked = true;
            }
            catch
            {
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unhook events.
            DisconnectApplicationEvents();
            _application = null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void eventButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (eventButton.Checked)
                {
                    if (_application == null)
                    {
                        _application = SolidEdgeCommunity.SolidEdgeUtils.Connect(true);
                        _application.Visible = true;
                    }

                    ConnectApplicationEvents();
                }
                else
                {
                    DisconnectApplicationEvents();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            lvEvents.Items.Clear();
        }

        /// <summary>
        /// Synchronously updates the UI. This will block the UI thread until the event is complete.
        /// </summary>
        /// <remarks>
        /// If one of the event arguments is a COM object, you should use this approach as the COM object could be
        /// freed before the method completes causing an exception.
        /// </remarks>
        public void LogEvent(MethodBase method, object[] args)
        {
            // COM events are received on background threads.
            if (Thread.CurrentThread.IsBackground)
            {
                // Dispatch a synchronous message to the UI thread.
                _uiContext.Send(new SendOrPostCallback(x => { LogEvent(method, args); }), null);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                var parameters = method.GetParameters();

                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        sb.AppendFormat("{0} = '{1}', ", parameter.Name, args[i]);
                    }

                    sb.Remove(sb.Length - 2, 2);
                }

                ListViewItem item = new ListViewItem(method.Name);
                item.SubItems.Add(sb.ToString());
                item.ImageIndex = 0;

                lvEvents.Items.Add(item);
                item.EnsureVisible();

                lvEvents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        /// <summary>
        /// Asynchronously updates the UI. This will no block the UI thread until the event is complete.
        /// </summary>
        public void LogEventAsync(MethodBase method, object[] args)
        {
            // COM events are received on background threads.
            if (Thread.CurrentThread.IsBackground)
            {
                // Dispatch an asynchronous message to the UI thread.
                _uiContext.Post(new SendOrPostCallback(x => { LogEventAsync(method, args); }), null);
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                var parameters = method.GetParameters();

                if (parameters.Length > 0)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var parameter = parameters[i];
                        sb.AppendFormat("{0} = '{1}',", parameter.Name, args[i]);
                    }

                    sb.Remove(sb.Length - 2, 2);
                }

                ListViewItem item = new ListViewItem(method.Name);
                item.SubItems.Add(sb.ToString());
                item.ImageIndex = 0;

                lvEvents.Items.Add(item);
                item.EnsureVisible();
            }
        }

        #region SolidEdgeFramework.ISEApplicationEvents

        void ISEApplicationEvents_AfterActiveDocumentChange(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEApplicationEvents_AfterCommandRun(int theCommandID)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theCommandID });
        }

        void ISEApplicationEvents_AfterDocumentOpen(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEApplicationEvents_AfterDocumentPrint(object theDocument, int hDC, ref double ModelToDC, ref int Rect)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, hDC, ModelToDC, Rect });
        }

        void ISEApplicationEvents_AfterDocumentSave(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEApplicationEvents_AfterEnvironmentActivate(object theEnvironment)
        {
            
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theEnvironment });
        }

        void ISEApplicationEvents_AfterNewDocumentOpen(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEApplicationEvents_AfterNewWindow(object theWindow)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theWindow });
        }

        void ISEApplicationEvents_AfterWindowActivate(object theWindow)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theWindow });
        }

        void ISEApplicationEvents_BeforeCommandRun(int theCommandID)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theCommandID });
        }

        void ISEApplicationEvents_BeforeDocumentClose(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEApplicationEvents_BeforeDocumentPrint(object theDocument, int hDC, ref double ModelToDC, ref int Rect)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, hDC, ModelToDC, Rect });
        }

        void ISEApplicationEvents_BeforeDocumentSave(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEApplicationEvents_BeforeEnvironmentDeactivate(object theEnvironment)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theEnvironment });
        }

        void ISEApplicationEvents_BeforeQuit()
        {
            // COM events are received on background threads.
            if (Thread.CurrentThread.IsBackground)
            {
                // Dispatch an synchronous message to the UI thread.
                _uiContext.Send(new SendOrPostCallback(x => { ISEApplicationEvents_BeforeQuit(); }), null);
            }
            else
            {
                LogEvent(MethodInfo.GetCurrentMethod(), new object[] { });
                
                eventButton.Checked = false;
            }
        }

        void ISEApplicationEvents_BeforeWindowDeactivate(object theWindow)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theWindow });
        }

        #endregion

        #region SolidEdgeFramework.ISEDocumentEvents

        void ISEDocumentEvents_AfterSave()
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { });
        }

        void ISEDocumentEvents_BeforeClose()
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { });
        }

        void ISEDocumentEvents_BeforeSave()
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { });
        }

        void ISEDocumentEvents_SelectSetChanged(object SelectSet)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { SelectSet });
        }

        #endregion

        #region "Event hooking-unhooking"

        private void ConnectApplicationEvents()
        {
            _applicationEvents = (SolidEdgeFramework.ISEApplicationEvents_Event)_application.ApplicationEvents;

            _applicationEvents.AfterActiveDocumentChange += ISEApplicationEvents_AfterActiveDocumentChange;
            _applicationEvents.AfterCommandRun += ISEApplicationEvents_AfterCommandRun;
            _applicationEvents.AfterDocumentOpen += ISEApplicationEvents_AfterDocumentOpen;
            _applicationEvents.AfterDocumentPrint += ISEApplicationEvents_AfterDocumentPrint;
            _applicationEvents.AfterDocumentSave += ISEApplicationEvents_AfterDocumentSave;
            _applicationEvents.AfterEnvironmentActivate += ISEApplicationEvents_AfterEnvironmentActivate;
            _applicationEvents.AfterNewDocumentOpen += ISEApplicationEvents_AfterNewDocumentOpen;
            _applicationEvents.AfterNewWindow += ISEApplicationEvents_AfterNewWindow;
            _applicationEvents.AfterWindowActivate += ISEApplicationEvents_AfterWindowActivate;
            _applicationEvents.BeforeCommandRun += ISEApplicationEvents_BeforeCommandRun;
            _applicationEvents.BeforeDocumentClose += ISEApplicationEvents_BeforeDocumentClose;
            _applicationEvents.BeforeDocumentPrint += ISEApplicationEvents_BeforeDocumentPrint;
            _applicationEvents.BeforeDocumentSave += ISEApplicationEvents_BeforeDocumentSave;
            _applicationEvents.BeforeEnvironmentDeactivate += ISEApplicationEvents_BeforeEnvironmentDeactivate;
            _applicationEvents.BeforeQuit += ISEApplicationEvents_BeforeQuit;
            _applicationEvents.BeforeWindowDeactivate += ISEApplicationEvents_BeforeWindowDeactivate;
        }

        private void DisconnectApplicationEvents()
        {
            if (_applicationEvents != null)
            {
                _applicationEvents.AfterActiveDocumentChange -= ISEApplicationEvents_AfterActiveDocumentChange;
                _applicationEvents.AfterCommandRun -= ISEApplicationEvents_AfterCommandRun;
                _applicationEvents.AfterDocumentOpen -= ISEApplicationEvents_AfterDocumentOpen;
                _applicationEvents.AfterDocumentPrint -= ISEApplicationEvents_AfterDocumentPrint;
                _applicationEvents.AfterDocumentSave -= ISEApplicationEvents_AfterDocumentSave;
                _applicationEvents.AfterEnvironmentActivate -= ISEApplicationEvents_AfterEnvironmentActivate;
                _applicationEvents.AfterNewDocumentOpen -= ISEApplicationEvents_AfterNewDocumentOpen;
                _applicationEvents.AfterNewWindow -= ISEApplicationEvents_AfterNewWindow;
                _applicationEvents.AfterWindowActivate -= ISEApplicationEvents_AfterWindowActivate;
                _applicationEvents.BeforeCommandRun -= ISEApplicationEvents_BeforeCommandRun;
                _applicationEvents.BeforeDocumentClose -= ISEApplicationEvents_BeforeDocumentClose;
                _applicationEvents.BeforeDocumentPrint -= ISEApplicationEvents_BeforeDocumentPrint;
                _applicationEvents.BeforeDocumentSave -= ISEApplicationEvents_BeforeDocumentSave;
                _applicationEvents.BeforeEnvironmentDeactivate -= ISEApplicationEvents_BeforeEnvironmentDeactivate;
                _applicationEvents.BeforeQuit -= ISEApplicationEvents_BeforeQuit;
                _applicationEvents.BeforeWindowDeactivate -= ISEApplicationEvents_BeforeWindowDeactivate;

                _applicationEvents = null;
            }
        }

        #endregion
    }
}
