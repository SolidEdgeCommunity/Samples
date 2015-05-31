using EventHandling.Properties;
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

namespace EventHandling
{
    public partial class MainForm : Form
    {
        private SynchronizationContext _uiContext;
        private SolidEdgeFramework.Application _application = null;
        private SolidEdgeFramework.ISEApplicationEvents_Event _applicationEvents;
        private SolidEdgeFramework.ISEDocumentEvents_Event _documentEvents;

        #region Assembly document events

        private SolidEdgeFramework.ISEAssemblyChangeEvents_Event _assemblyChangeEvents;
        private SolidEdgeFramework.ISEAssemblyFamilyEvents_Event _assemblyFamilyEvents;
        private SolidEdgeFramework.ISEAssemblyRecomputeEvents_Event _assemblyRecomputeEvents;
        
        #endregion

        #region Draft document events

        private SolidEdgeFramework.ISEBlockTableEvents_Event _blockTableEvents;
        private SolidEdgeFramework.ISEConnectorTableEvents_Event _connectorTableEvents;
        private SolidEdgeFramework.ISEDraftBendTableEvents_Event _draftBendTableEvents;
        private SolidEdgeFramework.ISEDrawingViewEvents_Event _drawingViewEvents;
        private SolidEdgeFramework.ISEPartsListEvents_Event _partsListEvents;

        #endregion

        #region Part \ SheetMetal document events

        private SolidEdgeFramework.ISEDividePartEvents_Event _dividePartEvents;
        private SolidEdgeFramework.ISEFamilyOfPartsEvents_Event _familyOfPartsEvents;
        private SolidEdgeFramework.ISEFamilyOfPartsExEvents_Event _familyOfPartsExEvents;

        #endregion

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
            // Disconnect from all events.
            DisconnectDocumentEvents();
            DisconnectApplicationEvents();

            SolidEdgeCommunity.OleMessageFilter.Unregister();
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

                    if (_application != null)
                    {
                        var documents = _application.Documents;

                        if (documents.Count > 0)
                        {
                            var document = (SolidEdgeFramework.SolidEdgeDocument)_application.ActiveDocument;
                            ConnectDocumentEvents(document);
                        }
                    }
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

            // Since a document was opened, we need to connect to the document events.
            var documents = _application.Documents;

            // Application.ActiveDocument can throw an exception rather than return null
            // so first check Documents.Count.
            if (documents.Count > 0)
            {
                if (theDocument == _application.ActiveDocument)
                {
                    var document = (SolidEdgeFramework.SolidEdgeDocument)theDocument;
                    ConnectDocumentEvents(document);
                }
            }
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

            // Since a new document was created, we need to connect to the document events.
            var document = (SolidEdgeFramework.SolidEdgeDocument)theDocument;
            ConnectDocumentEvents(document);
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

            DisconnectDocumentEvents();
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

        #region SolidEdgeFramework.ISEAssemblyChangeEvents

        void ISEAssemblyChangeEvents_AfterChange(object theDocument, object Object, SolidEdgeFramework.ObjectType Type, SolidEdgeFramework.seAssemblyChangeEventsConstants ChangeType)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, Object, Type, ChangeType });
        }

        void ISEAssemblyChangeEvents_BeforeChange(object theDocument, object Object, SolidEdgeFramework.ObjectType Type, SolidEdgeFramework.seAssemblyChangeEventsConstants ChangeType)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, Object, Type, ChangeType });
        }

        #endregion

        #region SolidEdgeFramework.ISEAssemblyFamilyEvents

        void ISEAssemblyFamilyEvents_AfterMemberActivate(object theDocument, string memberName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, memberName });
        }

        void ISEAssemblyFamilyEvents_AfterMemberCreate(object theDocument, string memberName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, memberName });
        }

        void ISEAssemblyFamilyEvents_AfterMemberDelete(object theDocument, string memberName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, memberName });
        }

        void ISEAssemblyFamilyEvents_BeforeMemberActivate(object theDocument, string memberName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, memberName });
        }

        void ISEAssemblyFamilyEvents_BeforeMemberCreate(object theDocument, string memberName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, memberName });
        }

        void ISEAssemblyFamilyEvents_BeforeMemberDelete(object theDocument, string memberName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, memberName });
        }

        #endregion

        #region SolidEdgeFramework.ISEAssemblyRecomputeEvents

        void ISEAssemblyRecomputeEvents_AfterAdd(object theDocument, object Object, SolidEdgeFramework.ObjectType Type)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, Object, Type });
        }

        void ISEAssemblyRecomputeEvents_AfterModify(object theDocument, object Object, SolidEdgeFramework.ObjectType Type, SolidEdgeFramework.seAssemblyEventConstants ModifyType)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, Object, Type, ModifyType });
        }

        void ISEAssemblyRecomputeEvents_AfterRecompute(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        void ISEAssemblyRecomputeEvents_BeforeDelete(object theDocument, object Object, SolidEdgeFramework.ObjectType Type)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument, Object, Type, Type });
        }

        void ISEAssemblyRecomputeEvents_BeforeRecompute(object theDocument)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theDocument });
        }

        #endregion

        #region SolidEdgeFramework.ISEBlockTableEvents

        void ISEBlockTableEvents_AfterUpdate(object BlockTable)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { BlockTable });
        }

        #endregion

        #region SolidEdgeFramework.ISEConnectorTableEvents

        void ISEConnectorTableEvents_AfterUpdate(object ConnectorTable)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { ConnectorTable });
        }

        #endregion

        #region SolidEdgeFramework.ISEDraftBendTableEvents

        void ISEDraftBendTableEvents_AfterUpdate(object DraftBendTable)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { DraftBendTable });
        }

        #endregion

        #region SolidEdgeFramework.ISEDrawingViewEvents

        void ISEDrawingViewEvents_AfterUpdate(object DrawingView)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { DrawingView });
        }

        #endregion

        #region SolidEdgeFramework.ISEPartsListEvents

        void ISEPartsListEvents_AfterUpdate(object PartsList)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { PartsList });
        }

        #endregion

        #region SolidEdgeFramework.ISEDividePartEvents

        void ISEDividePartEvents_AfterDividePartDocumentCreated(object theMember)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember });
        }

        void ISEDividePartEvents_AfterDividePartDocumentRenamed(object theMember, string OldName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember, OldName });
        }

        void ISEDividePartEvents_BeforeDividePartDocumentDeleted(object theMember)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember });
        }

        #endregion

        #region SolidEdgeFramework.ISEFamilyOfPartsEvents

        void ISEFamilyOfPartsEvents_AfterMemberDocumentCreated(object theMember)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember });
        }

        void ISEFamilyOfPartsEvents_AfterMemberDocumentRenamed(object theMember, string OldName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember, OldName });
        }

        void ISEFamilyOfPartsEvents_BeforeMemberDocumentDeleted(object theMember)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember });
        }

        #endregion

        #region SolidEdgeFramework.ISEFamilyOfPartsExEvents

        void ISEFamilyOfPartsExEvents_AfterMemberDocumentCreated(object theMember, bool DocumentExists)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember, DocumentExists });
        }

        void ISEFamilyOfPartsExEvents_AfterMemberDocumentRenamed(object theMember, string OldName)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember, OldName });
        }

        void ISEFamilyOfPartsExEvents_BeforeMemberDocumentDeleted(object theMember)
        {
            LogEvent(MethodInfo.GetCurrentMethod(), new object[] { theMember });
        }

        #endregion

        #region "Application events connect\disconnect"

        private void ConnectApplicationEvents()
        {
            if (_application != null)
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

        #region "Document events connect\disconnect"

        private void ConnectDocumentEvents(SolidEdgeFramework.SolidEdgeDocument document)
        {
            _documentEvents = (SolidEdgeFramework.ISEDocumentEvents_Event)document.DocumentEvents;

            switch (document.Type)
            {
                case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument:
                    var assemblyDocument = (SolidEdgeAssembly.AssemblyDocument)document;

                    _assemblyChangeEvents = (SolidEdgeFramework.ISEAssemblyChangeEvents_Event)assemblyDocument.AssemblyChangeEvents;
                    _assemblyFamilyEvents = (SolidEdgeFramework.ISEAssemblyFamilyEvents_Event)assemblyDocument.AssemblyFamilyEvents;
                    _assemblyRecomputeEvents = (SolidEdgeFramework.ISEAssemblyRecomputeEvents_Event)assemblyDocument.AssemblyRecomputeEvents;

                    break;
                case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument:
                    var draftDocument = (SolidEdgeDraft.DraftDocument)document;

                    _blockTableEvents = (SolidEdgeFramework.ISEBlockTableEvents_Event)draftDocument.BlockTableEvents;
                    _connectorTableEvents = (SolidEdgeFramework.ISEConnectorTableEvents_Event)draftDocument.ConnectorTableEvents;
                    _draftBendTableEvents = (SolidEdgeFramework.ISEDraftBendTableEvents_Event)draftDocument.DraftBendTableEvents;
                    _drawingViewEvents = (SolidEdgeFramework.ISEDrawingViewEvents_Event)draftDocument.DrawingViewEvents;
                    _partsListEvents = (SolidEdgeFramework.ISEPartsListEvents_Event)draftDocument.PartsListEvents;
                    
                    break;
                case SolidEdgeFramework.DocumentTypeConstants.igPartDocument:
                    var partDocument = (SolidEdgePart.PartDocument)document;

                    _dividePartEvents = (SolidEdgeFramework.ISEDividePartEvents_Event)partDocument.DividePartEvents;
                    _familyOfPartsEvents = (SolidEdgeFramework.ISEFamilyOfPartsEvents_Event)partDocument.FamilyOfPartsEvents;
                    _familyOfPartsExEvents = (SolidEdgeFramework.ISEFamilyOfPartsExEvents_Event)partDocument.FamilyOfPartsExEvents;

                    break;
                case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument:
                    var sheetMetalDocument = (SolidEdgePart.SheetMetalDocument)document;

                    _dividePartEvents = (SolidEdgeFramework.ISEDividePartEvents_Event)sheetMetalDocument.DividePartEvents;
                    _familyOfPartsEvents = (SolidEdgeFramework.ISEFamilyOfPartsEvents_Event)sheetMetalDocument.FamilyOfPartsEvents;
                    _familyOfPartsExEvents = (SolidEdgeFramework.ISEFamilyOfPartsExEvents_Event)sheetMetalDocument.FamilyOfPartsExEvents;

                    break;
            }

            if (_documentEvents != null)
            {
                _documentEvents.AfterSave += ISEDocumentEvents_AfterSave;
                _documentEvents.BeforeClose += ISEDocumentEvents_BeforeClose;
                _documentEvents.BeforeSave += ISEDocumentEvents_BeforeSave;
                _documentEvents.SelectSetChanged += ISEDocumentEvents_SelectSetChanged;
            }

            if (_assemblyChangeEvents != null)
            {
                _assemblyChangeEvents.AfterChange += ISEAssemblyChangeEvents_AfterChange;
                _assemblyChangeEvents.BeforeChange += ISEAssemblyChangeEvents_BeforeChange;
            }

            if (_assemblyFamilyEvents != null)
            {
                _assemblyFamilyEvents.AfterMemberActivate += ISEAssemblyFamilyEvents_AfterMemberActivate;
                _assemblyFamilyEvents.AfterMemberCreate += ISEAssemblyFamilyEvents_AfterMemberCreate;
                _assemblyFamilyEvents.AfterMemberDelete += ISEAssemblyFamilyEvents_AfterMemberDelete;
                _assemblyFamilyEvents.BeforeMemberActivate += ISEAssemblyFamilyEvents_BeforeMemberActivate;
                _assemblyFamilyEvents.BeforeMemberCreate += ISEAssemblyFamilyEvents_BeforeMemberCreate;
                _assemblyFamilyEvents.BeforeMemberDelete += ISEAssemblyFamilyEvents_BeforeMemberDelete;
            }

            if (_assemblyRecomputeEvents != null)
            {
                _assemblyRecomputeEvents.AfterAdd += ISEAssemblyRecomputeEvents_AfterAdd;
                _assemblyRecomputeEvents.AfterModify += ISEAssemblyRecomputeEvents_AfterModify;
                _assemblyRecomputeEvents.AfterRecompute += ISEAssemblyRecomputeEvents_AfterRecompute;
                _assemblyRecomputeEvents.BeforeDelete += ISEAssemblyRecomputeEvents_BeforeDelete;
                _assemblyRecomputeEvents.BeforeRecompute += ISEAssemblyRecomputeEvents_BeforeRecompute;
            }

            if (_blockTableEvents != null)
            {
                _blockTableEvents.AfterUpdate += ISEBlockTableEvents_AfterUpdate;
            }

            if (_connectorTableEvents != null)
            {
                _connectorTableEvents.AfterUpdate += ISEConnectorTableEvents_AfterUpdate;
            }

            if (_draftBendTableEvents != null)
            {
                _draftBendTableEvents.AfterUpdate += ISEDraftBendTableEvents_AfterUpdate;
            }

            if (_drawingViewEvents != null)
            {
                _drawingViewEvents.AfterUpdate += ISEDrawingViewEvents_AfterUpdate;
            }

            if (_partsListEvents != null)
            {
                _partsListEvents.AfterUpdate += ISEPartsListEvents_AfterUpdate;
            }

            if (_dividePartEvents != null)
            {
                _dividePartEvents.AfterDividePartDocumentCreated += ISEDividePartEvents_AfterDividePartDocumentCreated;
                _dividePartEvents.AfterDividePartDocumentRenamed += ISEDividePartEvents_AfterDividePartDocumentRenamed;
                _dividePartEvents.BeforeDividePartDocumentDeleted += ISEDividePartEvents_BeforeDividePartDocumentDeleted;
            }

            if (_familyOfPartsEvents != null)
            {
                _familyOfPartsEvents.AfterMemberDocumentCreated += ISEFamilyOfPartsEvents_AfterMemberDocumentCreated;
                _familyOfPartsEvents.AfterMemberDocumentRenamed += ISEFamilyOfPartsEvents_AfterMemberDocumentRenamed;
                _familyOfPartsEvents.BeforeMemberDocumentDeleted += ISEFamilyOfPartsEvents_BeforeMemberDocumentDeleted;
            }

            if (_familyOfPartsExEvents != null)
            {
                _familyOfPartsExEvents.AfterMemberDocumentCreated += ISEFamilyOfPartsExEvents_AfterMemberDocumentCreated;
                _familyOfPartsExEvents.AfterMemberDocumentRenamed += ISEFamilyOfPartsExEvents_AfterMemberDocumentRenamed;
                _familyOfPartsExEvents.BeforeMemberDocumentDeleted += ISEFamilyOfPartsExEvents_BeforeMemberDocumentDeleted;
            }
        }

        private void DisconnectDocumentEvents()
        {
            if (_documentEvents != null)
            {
                _documentEvents.AfterSave -= ISEDocumentEvents_AfterSave;
                _documentEvents.BeforeClose -= ISEDocumentEvents_BeforeClose;
                _documentEvents.BeforeSave -= ISEDocumentEvents_BeforeSave;
                _documentEvents.SelectSetChanged -= ISEDocumentEvents_SelectSetChanged;

                _documentEvents = null;
            }

            if (_assemblyChangeEvents != null)
            {
                _assemblyChangeEvents.AfterChange -= ISEAssemblyChangeEvents_AfterChange;
                _assemblyChangeEvents.BeforeChange -= ISEAssemblyChangeEvents_BeforeChange;

                _assemblyChangeEvents = null;
            }

            if (_assemblyFamilyEvents != null)
            {
                _assemblyFamilyEvents.AfterMemberActivate -= ISEAssemblyFamilyEvents_AfterMemberActivate;
                _assemblyFamilyEvents.AfterMemberCreate -= ISEAssemblyFamilyEvents_AfterMemberCreate;
                _assemblyFamilyEvents.AfterMemberDelete -= ISEAssemblyFamilyEvents_AfterMemberDelete;
                _assemblyFamilyEvents.BeforeMemberActivate -= ISEAssemblyFamilyEvents_BeforeMemberActivate;
                _assemblyFamilyEvents.BeforeMemberCreate -= ISEAssemblyFamilyEvents_BeforeMemberCreate;
                _assemblyFamilyEvents.BeforeMemberDelete -= ISEAssemblyFamilyEvents_BeforeMemberDelete;
            }

            if (_assemblyRecomputeEvents != null)
            {
                _assemblyRecomputeEvents.AfterAdd -= ISEAssemblyRecomputeEvents_AfterAdd;
                _assemblyRecomputeEvents.AfterModify -= ISEAssemblyRecomputeEvents_AfterModify;
                _assemblyRecomputeEvents.AfterRecompute -= ISEAssemblyRecomputeEvents_AfterRecompute;
                _assemblyRecomputeEvents.BeforeDelete -= ISEAssemblyRecomputeEvents_BeforeDelete;
                _assemblyRecomputeEvents.BeforeRecompute -= ISEAssemblyRecomputeEvents_BeforeRecompute;
            }

            if (_blockTableEvents != null)
            {
                _blockTableEvents.AfterUpdate -= ISEBlockTableEvents_AfterUpdate;
            }

            if (_connectorTableEvents != null)
            {
                _connectorTableEvents.AfterUpdate -= ISEConnectorTableEvents_AfterUpdate;
            }

            if (_draftBendTableEvents != null)
            {
                _draftBendTableEvents.AfterUpdate -= ISEDraftBendTableEvents_AfterUpdate;
            }

            if (_drawingViewEvents != null)
            {
                _drawingViewEvents.AfterUpdate -= ISEDrawingViewEvents_AfterUpdate;
            }

            if (_partsListEvents != null)
            {
                _partsListEvents.AfterUpdate -= ISEPartsListEvents_AfterUpdate;
            }

            if (_dividePartEvents != null)
            {
                _dividePartEvents.AfterDividePartDocumentCreated -= ISEDividePartEvents_AfterDividePartDocumentCreated;
                _dividePartEvents.AfterDividePartDocumentRenamed -= ISEDividePartEvents_AfterDividePartDocumentRenamed;
                _dividePartEvents.BeforeDividePartDocumentDeleted -= ISEDividePartEvents_BeforeDividePartDocumentDeleted;
            }

            if (_familyOfPartsEvents != null)
            {
                _familyOfPartsEvents.AfterMemberDocumentCreated -= ISEFamilyOfPartsEvents_AfterMemberDocumentCreated;
                _familyOfPartsEvents.AfterMemberDocumentRenamed -= ISEFamilyOfPartsEvents_AfterMemberDocumentRenamed;
                _familyOfPartsEvents.BeforeMemberDocumentDeleted -= ISEFamilyOfPartsEvents_BeforeMemberDocumentDeleted;
            }

            if (_familyOfPartsExEvents != null)
            {
                _familyOfPartsExEvents.AfterMemberDocumentCreated -= ISEFamilyOfPartsExEvents_AfterMemberDocumentCreated;
                _familyOfPartsExEvents.AfterMemberDocumentRenamed -= ISEFamilyOfPartsExEvents_AfterMemberDocumentRenamed;
                _familyOfPartsExEvents.BeforeMemberDocumentDeleted -= ISEFamilyOfPartsExEvents_BeforeMemberDocumentDeleted;
            }
        }

        #endregion

        /// <summary>
        /// Synchronously updates the UI. This will block the UI thread until the event is complete.
        /// </summary>
        /// <remarks>
        /// If one of the event arguments is a COM object, you should use this approach as the COM object could be
        /// freed before the method completes causing an exception.
        /// </remarks>
        private void LogEvent(MethodBase method, object[] args)
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
        private void LogEventAsync(MethodBase method, object[] args)
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
                lvEvents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }
    }
}
