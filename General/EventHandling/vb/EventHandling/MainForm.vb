Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Private _uiContext As SynchronizationContext
    Private _application As SolidEdgeFramework.Application = Nothing
    Private _applicationEvents As SolidEdgeFramework.ISEApplicationEvents_Event
    Private _documentEvents As SolidEdgeFramework.ISEDocumentEvents_Event

#Region "Assembly document events"

    Private _assemblyChangeEvents As SolidEdgeFramework.ISEAssemblyChangeEvents_Event
    Private _assemblyFamilyEvents As SolidEdgeFramework.ISEAssemblyFamilyEvents_Event
    Private _assemblyRecomputeEvents As SolidEdgeFramework.ISEAssemblyRecomputeEvents_Event

#End Region

#Region "Draft document events"

    Private _blockTableEvents As SolidEdgeFramework.ISEBlockTableEvents_Event
    Private _connectorTableEvents As SolidEdgeFramework.ISEConnectorTableEvents_Event
    Private _draftBendTableEvents As SolidEdgeFramework.ISEDraftBendTableEvents_Event
    Private _drawingViewEvents As SolidEdgeFramework.ISEDrawingViewEvents_Event
    Private _partsListEvents As SolidEdgeFramework.ISEPartsListEvents_Event

#End Region

#Region "Part \ SheetMetal document events"

    Private _dividePartEvents As SolidEdgeFramework.ISEDividePartEvents_Event
    Private _familyOfPartsEvents As SolidEdgeFramework.ISEFamilyOfPartsEvents_Event
    Private _familyOfPartsExEvents As SolidEdgeFramework.ISEFamilyOfPartsExEvents_Event

#End Region

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _uiContext = SynchronizationContext.Current

        imageList1.Images.Add(My.Resources.Event_16x16)

        ' Register with OLE to handle concurrency issues on the current thread.
        SolidEdgeCommunity.OleMessageFilter.Register()

        Try
            _application = SolidEdgeCommunity.SolidEdgeUtils.Connect()
            eventButton.Checked = True
        Catch
        End Try
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        ' Disconnect from all events.
        DisconnectDocumentEvents()
        DisconnectApplicationEvents()

        SolidEdgeCommunity.OleMessageFilter.Unregister()
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub eventButton_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles eventButton.CheckedChanged
        Try
            If eventButton.Checked Then
                If _application Is Nothing Then
                    _application = SolidEdgeCommunity.SolidEdgeUtils.Connect(True)
                    _application.Visible = True
                End If

                ConnectApplicationEvents()

                If _application IsNot Nothing Then
                    Dim documents = _application.Documents

                    If documents.Count > 0 Then
                        Dim document = DirectCast(_application.ActiveDocument, SolidEdgeFramework.SolidEdgeDocument)
                        ConnectDocumentEvents(document)
                    End If
                End If
            Else
                DisconnectApplicationEvents()
            End If
        Catch ex As System.Exception
            MessageBox.Show(Me, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub clearButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles clearButton.Click
        lvEvents.Items.Clear()
    End Sub

#Region "SolidEdgeFramework.ISEApplicationEvents"

    Private Sub ISEApplicationEvents_AfterActiveDocumentChange(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})
    End Sub

    Private Sub ISEApplicationEvents_AfterCommandRun(ByVal theCommandID As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theCommandID})
    End Sub

    Private Sub ISEApplicationEvents_AfterDocumentOpen(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})

        ' Since a document was opened, we need to connect to the document events.
        Dim documents = _application.Documents

        ' Application.ActiveDocument can throw an exception rather than return null
        ' so first check Documents.Count.
        If documents.Count > 0 Then
            If theDocument Is _application.ActiveDocument Then
                Dim document = DirectCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
                ConnectDocumentEvents(document)
            End If
        End If
    End Sub

    Private Sub ISEApplicationEvents_AfterDocumentPrint(ByVal theDocument As Object, ByVal hDC As Integer, ByRef ModelToDC As Double, ByRef Rect As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, hDC, ModelToDC, Rect})
    End Sub

    Private Sub ISEApplicationEvents_AfterDocumentSave(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})
    End Sub

    Private Sub ISEApplicationEvents_AfterEnvironmentActivate(ByVal theEnvironment As Object)

        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theEnvironment})
    End Sub

    Private Sub ISEApplicationEvents_AfterNewDocumentOpen(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})

        ' Since a new document was created, we need to connect to the document events.
        Dim document = DirectCast(theDocument, SolidEdgeFramework.SolidEdgeDocument)
        ConnectDocumentEvents(document)
    End Sub

    Private Sub ISEApplicationEvents_AfterNewWindow(ByVal theWindow As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theWindow})
    End Sub

    Private Sub ISEApplicationEvents_AfterWindowActivate(ByVal theWindow As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theWindow})
    End Sub

    Private Sub ISEApplicationEvents_BeforeCommandRun(ByVal theCommandID As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theCommandID})
    End Sub

    Private Sub ISEApplicationEvents_BeforeDocumentClose(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})
    End Sub

    Private Sub ISEApplicationEvents_BeforeDocumentPrint(ByVal theDocument As Object, ByVal hDC As Integer, ByRef ModelToDC As Double, ByRef Rect As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, hDC, ModelToDC, Rect})
    End Sub

    Private Sub ISEApplicationEvents_BeforeDocumentSave(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})
    End Sub

    Private Sub ISEApplicationEvents_BeforeEnvironmentDeactivate(ByVal theEnvironment As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theEnvironment})
    End Sub

    Private Sub ISEApplicationEvents_BeforeQuit()
        ' COM events are received on background threads.
        If Thread.CurrentThread.IsBackground Then
            ' Dispatch an synchronous message to the UI thread.
            _uiContext.Send(New SendOrPostCallback(Sub(x) ISEApplicationEvents_BeforeQuit()), Nothing)
        Else
            LogEvent(MethodInfo.GetCurrentMethod(), New Object() {})

            eventButton.Checked = False

        End If
    End Sub

    Private Sub ISEApplicationEvents_BeforeWindowDeactivate(ByVal theWindow As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theWindow})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEDocumentEvents"

    Private Sub ISEDocumentEvents_AfterSave()
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {})
    End Sub

    Private Sub ISEDocumentEvents_BeforeClose()
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {})

        DisconnectDocumentEvents()
    End Sub

    Private Sub ISEDocumentEvents_BeforeSave()
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {})
    End Sub

    Private Sub ISEDocumentEvents_SelectSetChanged(ByVal SelectSet As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {SelectSet})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEAssemblyChangeEvents"

    Private Sub ISEAssemblyChangeEvents_AfterChange(ByVal theDocument As Object, ByVal TheObject As Object, ByVal Type As SolidEdgeFramework.ObjectType, ByVal ChangeType As SolidEdgeFramework.seAssemblyChangeEventsConstants)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, TheObject, Type, ChangeType})
    End Sub

    Private Sub ISEAssemblyChangeEvents_BeforeChange(ByVal theDocument As Object, ByVal TheObject As Object, ByVal Type As SolidEdgeFramework.ObjectType, ByVal ChangeType As SolidEdgeFramework.seAssemblyChangeEventsConstants)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, TheObject, Type, ChangeType})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEAssemblyFamilyEvents"

    Private Sub ISEAssemblyFamilyEvents_AfterMemberActivate(ByVal theDocument As Object, ByVal memberName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, memberName})
    End Sub

    Private Sub ISEAssemblyFamilyEvents_AfterMemberCreate(ByVal theDocument As Object, ByVal memberName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, memberName})
    End Sub

    Private Sub ISEAssemblyFamilyEvents_AfterMemberDelete(ByVal theDocument As Object, ByVal memberName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, memberName})
    End Sub

    Private Sub ISEAssemblyFamilyEvents_BeforeMemberActivate(ByVal theDocument As Object, ByVal memberName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, memberName})
    End Sub

    Private Sub ISEAssemblyFamilyEvents_BeforeMemberCreate(ByVal theDocument As Object, ByVal memberName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, memberName})
    End Sub

    Private Sub ISEAssemblyFamilyEvents_BeforeMemberDelete(ByVal theDocument As Object, ByVal memberName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, memberName})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEAssemblyRecomputeEvents"

    Private Sub ISEAssemblyRecomputeEvents_AfterAdd(ByVal theDocument As Object, ByVal TheObject As Object, ByVal Type As SolidEdgeFramework.ObjectType)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, TheObject, Type})
    End Sub

    Private Sub ISEAssemblyRecomputeEvents_AfterModify(ByVal theDocument As Object, ByVal TheObject As Object, ByVal Type As SolidEdgeFramework.ObjectType, ByVal ModifyType As SolidEdgeFramework.seAssemblyEventConstants)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, TheObject, Type, ModifyType})
    End Sub

    Private Sub ISEAssemblyRecomputeEvents_AfterRecompute(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})
    End Sub

    Private Sub ISEAssemblyRecomputeEvents_BeforeDelete(ByVal theDocument As Object, ByVal TheObject As Object, ByVal Type As SolidEdgeFramework.ObjectType)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument, TheObject, Type, Type})
    End Sub

    Private Sub ISEAssemblyRecomputeEvents_BeforeRecompute(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theDocument})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEBlockTableEvents"

    Private Sub ISEBlockTableEvents_AfterUpdate(ByVal BlockTable As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {BlockTable})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEConnectorTableEvents"

    Private Sub ISEConnectorTableEvents_AfterUpdate(ByVal ConnectorTable As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {ConnectorTable})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEDraftBendTableEvents"

    Private Sub ISEDraftBendTableEvents_AfterUpdate(ByVal DraftBendTable As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {DraftBendTable})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEDrawingViewEvents"

    Private Sub ISEDrawingViewEvents_AfterUpdate(ByVal DrawingView As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {DrawingView})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEPartsListEvents"

    Private Sub ISEPartsListEvents_AfterUpdate(ByVal PartsList As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {PartsList})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEDividePartEvents"

    Private Sub ISEDividePartEvents_AfterDividePartDocumentCreated(ByVal theMember As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember})
    End Sub

    Private Sub ISEDividePartEvents_AfterDividePartDocumentRenamed(ByVal theMember As Object, ByVal OldName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember, OldName})
    End Sub

    Private Sub ISEDividePartEvents_BeforeDividePartDocumentDeleted(ByVal theMember As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEFamilyOfPartsEvents"

    Private Sub ISEFamilyOfPartsEvents_AfterMemberDocumentCreated(ByVal theMember As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember})
    End Sub

    Private Sub ISEFamilyOfPartsEvents_AfterMemberDocumentRenamed(ByVal theMember As Object, ByVal OldName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember, OldName})
    End Sub

    Private Sub ISEFamilyOfPartsEvents_BeforeMemberDocumentDeleted(ByVal theMember As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember})
    End Sub

#End Region

#Region "SolidEdgeFramework.ISEFamilyOfPartsExEvents"

    Private Sub ISEFamilyOfPartsExEvents_AfterMemberDocumentCreated(ByVal theMember As Object, ByVal DocumentExists As Boolean)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember, DocumentExists})
    End Sub

    Private Sub ISEFamilyOfPartsExEvents_AfterMemberDocumentRenamed(ByVal theMember As Object, ByVal OldName As String)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember, OldName})
    End Sub

    Private Sub ISEFamilyOfPartsExEvents_BeforeMemberDocumentDeleted(ByVal theMember As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() {theMember})
    End Sub

#End Region

#Region "Application events connect\disconnect"

    Private Sub ConnectApplicationEvents()
        If _application IsNot Nothing Then
            _applicationEvents = DirectCast(_application.ApplicationEvents, SolidEdgeFramework.ISEApplicationEvents_Event)

            AddHandler _applicationEvents.AfterActiveDocumentChange, AddressOf ISEApplicationEvents_AfterActiveDocumentChange
            AddHandler _applicationEvents.AfterCommandRun, AddressOf ISEApplicationEvents_AfterCommandRun
            AddHandler _applicationEvents.AfterDocumentOpen, AddressOf ISEApplicationEvents_AfterDocumentOpen
            AddHandler _applicationEvents.AfterDocumentPrint, AddressOf ISEApplicationEvents_AfterDocumentPrint
            AddHandler _applicationEvents.AfterDocumentSave, AddressOf ISEApplicationEvents_AfterDocumentSave
            AddHandler _applicationEvents.AfterEnvironmentActivate, AddressOf ISEApplicationEvents_AfterEnvironmentActivate
            AddHandler _applicationEvents.AfterNewDocumentOpen, AddressOf ISEApplicationEvents_AfterNewDocumentOpen
            AddHandler _applicationEvents.AfterNewWindow, AddressOf ISEApplicationEvents_AfterNewWindow
            AddHandler _applicationEvents.AfterWindowActivate, AddressOf ISEApplicationEvents_AfterWindowActivate
            AddHandler _applicationEvents.BeforeCommandRun, AddressOf ISEApplicationEvents_BeforeCommandRun
            AddHandler _applicationEvents.BeforeDocumentClose, AddressOf ISEApplicationEvents_BeforeDocumentClose
            AddHandler _applicationEvents.BeforeDocumentPrint, AddressOf ISEApplicationEvents_BeforeDocumentPrint
            AddHandler _applicationEvents.BeforeDocumentSave, AddressOf ISEApplicationEvents_BeforeDocumentSave
            AddHandler _applicationEvents.BeforeEnvironmentDeactivate, AddressOf ISEApplicationEvents_BeforeEnvironmentDeactivate
            AddHandler _applicationEvents.BeforeQuit, AddressOf ISEApplicationEvents_BeforeQuit
            AddHandler _applicationEvents.BeforeWindowDeactivate, AddressOf ISEApplicationEvents_BeforeWindowDeactivate
        End If
    End Sub

    Private Sub DisconnectApplicationEvents()
        If _applicationEvents IsNot Nothing Then
            RemoveHandler _applicationEvents.AfterActiveDocumentChange, AddressOf ISEApplicationEvents_AfterActiveDocumentChange
            RemoveHandler _applicationEvents.AfterCommandRun, AddressOf ISEApplicationEvents_AfterCommandRun
            RemoveHandler _applicationEvents.AfterDocumentOpen, AddressOf ISEApplicationEvents_AfterDocumentOpen
            RemoveHandler _applicationEvents.AfterDocumentPrint, AddressOf ISEApplicationEvents_AfterDocumentPrint
            RemoveHandler _applicationEvents.AfterDocumentSave, AddressOf ISEApplicationEvents_AfterDocumentSave
            RemoveHandler _applicationEvents.AfterEnvironmentActivate, AddressOf ISEApplicationEvents_AfterEnvironmentActivate
            RemoveHandler _applicationEvents.AfterNewDocumentOpen, AddressOf ISEApplicationEvents_AfterNewDocumentOpen
            RemoveHandler _applicationEvents.AfterNewWindow, AddressOf ISEApplicationEvents_AfterNewWindow
            RemoveHandler _applicationEvents.AfterWindowActivate, AddressOf ISEApplicationEvents_AfterWindowActivate
            RemoveHandler _applicationEvents.BeforeCommandRun, AddressOf ISEApplicationEvents_BeforeCommandRun
            RemoveHandler _applicationEvents.BeforeDocumentClose, AddressOf ISEApplicationEvents_BeforeDocumentClose
            RemoveHandler _applicationEvents.BeforeDocumentPrint, AddressOf ISEApplicationEvents_BeforeDocumentPrint
            RemoveHandler _applicationEvents.BeforeDocumentSave, AddressOf ISEApplicationEvents_BeforeDocumentSave
            RemoveHandler _applicationEvents.BeforeEnvironmentDeactivate, AddressOf ISEApplicationEvents_BeforeEnvironmentDeactivate
            RemoveHandler _applicationEvents.BeforeQuit, AddressOf ISEApplicationEvents_BeforeQuit
            RemoveHandler _applicationEvents.BeforeWindowDeactivate, AddressOf ISEApplicationEvents_BeforeWindowDeactivate

            _applicationEvents = Nothing
        End If
    End Sub

#End Region

#Region "Document events connect\disconnect"

    Private Sub ConnectDocumentEvents(ByVal document As SolidEdgeFramework.SolidEdgeDocument)
        _documentEvents = DirectCast(document.DocumentEvents, SolidEdgeFramework.ISEDocumentEvents_Event)

        Select Case document.Type
            Case SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument
                Dim assemblyDocument = DirectCast(document, SolidEdgeAssembly.AssemblyDocument)

                _assemblyChangeEvents = DirectCast(assemblyDocument.AssemblyChangeEvents, SolidEdgeFramework.ISEAssemblyChangeEvents_Event)
                _assemblyFamilyEvents = DirectCast(assemblyDocument.AssemblyFamilyEvents, SolidEdgeFramework.ISEAssemblyFamilyEvents_Event)
                _assemblyRecomputeEvents = DirectCast(assemblyDocument.AssemblyRecomputeEvents, SolidEdgeFramework.ISEAssemblyRecomputeEvents_Event)

            Case SolidEdgeFramework.DocumentTypeConstants.igDraftDocument
                Dim draftDocument = DirectCast(document, SolidEdgeDraft.DraftDocument)

                _blockTableEvents = DirectCast(draftDocument.BlockTableEvents, SolidEdgeFramework.ISEBlockTableEvents_Event)
                _connectorTableEvents = DirectCast(draftDocument.ConnectorTableEvents, SolidEdgeFramework.ISEConnectorTableEvents_Event)
                _draftBendTableEvents = DirectCast(draftDocument.DraftBendTableEvents, SolidEdgeFramework.ISEDraftBendTableEvents_Event)
                _drawingViewEvents = DirectCast(draftDocument.DrawingViewEvents, SolidEdgeFramework.ISEDrawingViewEvents_Event)
                _partsListEvents = DirectCast(draftDocument.PartsListEvents, SolidEdgeFramework.ISEPartsListEvents_Event)

            Case SolidEdgeFramework.DocumentTypeConstants.igPartDocument
                Dim partDocument = DirectCast(document, SolidEdgePart.PartDocument)

                _dividePartEvents = DirectCast(partDocument.DividePartEvents, SolidEdgeFramework.ISEDividePartEvents_Event)
                _familyOfPartsEvents = DirectCast(partDocument.FamilyOfPartsEvents, SolidEdgeFramework.ISEFamilyOfPartsEvents_Event)
                _familyOfPartsExEvents = DirectCast(partDocument.FamilyOfPartsExEvents, SolidEdgeFramework.ISEFamilyOfPartsExEvents_Event)

            Case SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument
                Dim sheetMetalDocument = DirectCast(document, SolidEdgePart.SheetMetalDocument)

                _dividePartEvents = DirectCast(sheetMetalDocument.DividePartEvents, SolidEdgeFramework.ISEDividePartEvents_Event)
                _familyOfPartsEvents = DirectCast(sheetMetalDocument.FamilyOfPartsEvents, SolidEdgeFramework.ISEFamilyOfPartsEvents_Event)
                _familyOfPartsExEvents = DirectCast(sheetMetalDocument.FamilyOfPartsExEvents, SolidEdgeFramework.ISEFamilyOfPartsExEvents_Event)

        End Select

        If _documentEvents IsNot Nothing Then
            AddHandler _documentEvents.AfterSave, AddressOf ISEDocumentEvents_AfterSave
            AddHandler _documentEvents.BeforeClose, AddressOf ISEDocumentEvents_BeforeClose
            AddHandler _documentEvents.BeforeSave, AddressOf ISEDocumentEvents_BeforeSave
            AddHandler _documentEvents.SelectSetChanged, AddressOf ISEDocumentEvents_SelectSetChanged
        End If

        If _assemblyChangeEvents IsNot Nothing Then
            AddHandler _assemblyChangeEvents.AfterChange, AddressOf ISEAssemblyChangeEvents_AfterChange
            AddHandler _assemblyChangeEvents.BeforeChange, AddressOf ISEAssemblyChangeEvents_BeforeChange
        End If

        If _assemblyFamilyEvents IsNot Nothing Then
            AddHandler _assemblyFamilyEvents.AfterMemberActivate, AddressOf ISEAssemblyFamilyEvents_AfterMemberActivate
            AddHandler _assemblyFamilyEvents.AfterMemberCreate, AddressOf ISEAssemblyFamilyEvents_AfterMemberCreate
            AddHandler _assemblyFamilyEvents.AfterMemberDelete, AddressOf ISEAssemblyFamilyEvents_AfterMemberDelete
            AddHandler _assemblyFamilyEvents.BeforeMemberActivate, AddressOf ISEAssemblyFamilyEvents_BeforeMemberActivate
            AddHandler _assemblyFamilyEvents.BeforeMemberCreate, AddressOf ISEAssemblyFamilyEvents_BeforeMemberCreate
            AddHandler _assemblyFamilyEvents.BeforeMemberDelete, AddressOf ISEAssemblyFamilyEvents_BeforeMemberDelete
        End If

        If _assemblyRecomputeEvents IsNot Nothing Then
            AddHandler _assemblyRecomputeEvents.AfterAdd, AddressOf ISEAssemblyRecomputeEvents_AfterAdd
            AddHandler _assemblyRecomputeEvents.AfterModify, AddressOf ISEAssemblyRecomputeEvents_AfterModify
            AddHandler _assemblyRecomputeEvents.AfterRecompute, AddressOf ISEAssemblyRecomputeEvents_AfterRecompute
            AddHandler _assemblyRecomputeEvents.BeforeDelete, AddressOf ISEAssemblyRecomputeEvents_BeforeDelete
            AddHandler _assemblyRecomputeEvents.BeforeRecompute, AddressOf ISEAssemblyRecomputeEvents_BeforeRecompute
        End If

        If _blockTableEvents IsNot Nothing Then
            AddHandler _blockTableEvents.AfterUpdate, AddressOf ISEBlockTableEvents_AfterUpdate
        End If

        If _connectorTableEvents IsNot Nothing Then
            AddHandler _connectorTableEvents.AfterUpdate, AddressOf ISEConnectorTableEvents_AfterUpdate
        End If

        If _draftBendTableEvents IsNot Nothing Then
            AddHandler _draftBendTableEvents.AfterUpdate, AddressOf ISEDraftBendTableEvents_AfterUpdate
        End If

        If _drawingViewEvents IsNot Nothing Then
            AddHandler _drawingViewEvents.AfterUpdate, AddressOf ISEDrawingViewEvents_AfterUpdate
        End If

        If _partsListEvents IsNot Nothing Then
            AddHandler _partsListEvents.AfterUpdate, AddressOf ISEPartsListEvents_AfterUpdate
        End If

        If _dividePartEvents IsNot Nothing Then
            AddHandler _dividePartEvents.AfterDividePartDocumentCreated, AddressOf ISEDividePartEvents_AfterDividePartDocumentCreated
            AddHandler _dividePartEvents.AfterDividePartDocumentRenamed, AddressOf ISEDividePartEvents_AfterDividePartDocumentRenamed
            AddHandler _dividePartEvents.BeforeDividePartDocumentDeleted, AddressOf ISEDividePartEvents_BeforeDividePartDocumentDeleted
        End If

        If _familyOfPartsEvents IsNot Nothing Then
            AddHandler _familyOfPartsEvents.AfterMemberDocumentCreated, AddressOf ISEFamilyOfPartsEvents_AfterMemberDocumentCreated
            AddHandler _familyOfPartsEvents.AfterMemberDocumentRenamed, AddressOf ISEFamilyOfPartsEvents_AfterMemberDocumentRenamed
            AddHandler _familyOfPartsEvents.BeforeMemberDocumentDeleted, AddressOf ISEFamilyOfPartsEvents_BeforeMemberDocumentDeleted
        End If

        If _familyOfPartsExEvents IsNot Nothing Then
            AddHandler _familyOfPartsExEvents.AfterMemberDocumentCreated, AddressOf ISEFamilyOfPartsExEvents_AfterMemberDocumentCreated
            AddHandler _familyOfPartsExEvents.AfterMemberDocumentRenamed, AddressOf ISEFamilyOfPartsExEvents_AfterMemberDocumentRenamed
            AddHandler _familyOfPartsExEvents.BeforeMemberDocumentDeleted, AddressOf ISEFamilyOfPartsExEvents_BeforeMemberDocumentDeleted
        End If
    End Sub

    Private Sub DisconnectDocumentEvents()
        If _documentEvents IsNot Nothing Then
            RemoveHandler _documentEvents.AfterSave, AddressOf ISEDocumentEvents_AfterSave
            RemoveHandler _documentEvents.BeforeClose, AddressOf ISEDocumentEvents_BeforeClose
            RemoveHandler _documentEvents.BeforeSave, AddressOf ISEDocumentEvents_BeforeSave
            RemoveHandler _documentEvents.SelectSetChanged, AddressOf ISEDocumentEvents_SelectSetChanged

            _documentEvents = Nothing
        End If

        If _assemblyChangeEvents IsNot Nothing Then
            RemoveHandler _assemblyChangeEvents.AfterChange, AddressOf ISEAssemblyChangeEvents_AfterChange
            RemoveHandler _assemblyChangeEvents.BeforeChange, AddressOf ISEAssemblyChangeEvents_BeforeChange

            _assemblyChangeEvents = Nothing
        End If

        If _assemblyFamilyEvents IsNot Nothing Then
            RemoveHandler _assemblyFamilyEvents.AfterMemberActivate, AddressOf ISEAssemblyFamilyEvents_AfterMemberActivate
            RemoveHandler _assemblyFamilyEvents.AfterMemberCreate, AddressOf ISEAssemblyFamilyEvents_AfterMemberCreate
            RemoveHandler _assemblyFamilyEvents.AfterMemberDelete, AddressOf ISEAssemblyFamilyEvents_AfterMemberDelete
            RemoveHandler _assemblyFamilyEvents.BeforeMemberActivate, AddressOf ISEAssemblyFamilyEvents_BeforeMemberActivate
            RemoveHandler _assemblyFamilyEvents.BeforeMemberCreate, AddressOf ISEAssemblyFamilyEvents_BeforeMemberCreate
            RemoveHandler _assemblyFamilyEvents.BeforeMemberDelete, AddressOf ISEAssemblyFamilyEvents_BeforeMemberDelete
        End If

        If _assemblyRecomputeEvents IsNot Nothing Then
            RemoveHandler _assemblyRecomputeEvents.AfterAdd, AddressOf ISEAssemblyRecomputeEvents_AfterAdd
            RemoveHandler _assemblyRecomputeEvents.AfterModify, AddressOf ISEAssemblyRecomputeEvents_AfterModify
            RemoveHandler _assemblyRecomputeEvents.AfterRecompute, AddressOf ISEAssemblyRecomputeEvents_AfterRecompute
            RemoveHandler _assemblyRecomputeEvents.BeforeDelete, AddressOf ISEAssemblyRecomputeEvents_BeforeDelete
            RemoveHandler _assemblyRecomputeEvents.BeforeRecompute, AddressOf ISEAssemblyRecomputeEvents_BeforeRecompute
        End If

        If _blockTableEvents IsNot Nothing Then
            RemoveHandler _blockTableEvents.AfterUpdate, AddressOf ISEBlockTableEvents_AfterUpdate
        End If

        If _connectorTableEvents IsNot Nothing Then
            RemoveHandler _connectorTableEvents.AfterUpdate, AddressOf ISEConnectorTableEvents_AfterUpdate
        End If

        If _draftBendTableEvents IsNot Nothing Then
            RemoveHandler _draftBendTableEvents.AfterUpdate, AddressOf ISEDraftBendTableEvents_AfterUpdate
        End If

        If _drawingViewEvents IsNot Nothing Then
            RemoveHandler _drawingViewEvents.AfterUpdate, AddressOf ISEDrawingViewEvents_AfterUpdate
        End If

        If _partsListEvents IsNot Nothing Then
            RemoveHandler _partsListEvents.AfterUpdate, AddressOf ISEPartsListEvents_AfterUpdate
        End If

        If _dividePartEvents IsNot Nothing Then
            RemoveHandler _dividePartEvents.AfterDividePartDocumentCreated, AddressOf ISEDividePartEvents_AfterDividePartDocumentCreated
            RemoveHandler _dividePartEvents.AfterDividePartDocumentRenamed, AddressOf ISEDividePartEvents_AfterDividePartDocumentRenamed
            RemoveHandler _dividePartEvents.BeforeDividePartDocumentDeleted, AddressOf ISEDividePartEvents_BeforeDividePartDocumentDeleted
        End If

        If _familyOfPartsEvents IsNot Nothing Then
            RemoveHandler _familyOfPartsEvents.AfterMemberDocumentCreated, AddressOf ISEFamilyOfPartsEvents_AfterMemberDocumentCreated
            RemoveHandler _familyOfPartsEvents.AfterMemberDocumentRenamed, AddressOf ISEFamilyOfPartsEvents_AfterMemberDocumentRenamed
            RemoveHandler _familyOfPartsEvents.BeforeMemberDocumentDeleted, AddressOf ISEFamilyOfPartsEvents_BeforeMemberDocumentDeleted
        End If

        If _familyOfPartsExEvents IsNot Nothing Then
            RemoveHandler _familyOfPartsExEvents.AfterMemberDocumentCreated, AddressOf ISEFamilyOfPartsExEvents_AfterMemberDocumentCreated
            RemoveHandler _familyOfPartsExEvents.AfterMemberDocumentRenamed, AddressOf ISEFamilyOfPartsExEvents_AfterMemberDocumentRenamed
            RemoveHandler _familyOfPartsExEvents.BeforeMemberDocumentDeleted, AddressOf ISEFamilyOfPartsExEvents_BeforeMemberDocumentDeleted
        End If
    End Sub

#End Region

    ''' <summary>
    ''' Synchronously updates the UI. This will block the UI thread until the event is complete.
    ''' </summary>
    ''' <remarks>
    ''' If one of the event arguments is a COM object, you should use this approach as the COM object could be
    ''' freed before the method completes causing an exception.
    ''' </remarks>
    Private Sub LogEvent(ByVal method As MethodBase, ByVal args() As Object)
        ' COM events are received on background threads.
        If Thread.CurrentThread.IsBackground Then
            ' Dispatch a synchronous message to the UI thread.
            _uiContext.Send(New SendOrPostCallback(Sub(x) LogEvent(method, args)), Nothing)
        Else
            Dim sb As New StringBuilder()

            Dim parameters = method.GetParameters()

            If parameters.Length > 0 Then
                For i As Integer = 0 To parameters.Length - 1
                    Dim parameter = parameters(i)
                    sb.AppendFormat("{0} = '{1}', ", parameter.Name, args(i))
                Next i

                sb.Remove(sb.Length - 2, 2)
            End If

            Dim item As New ListViewItem(method.Name)
            item.SubItems.Add(sb.ToString())
            item.ImageIndex = 0

            lvEvents.Items.Add(item)
            item.EnsureVisible()
            lvEvents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        End If
    End Sub

    ''' <summary>
    ''' Asynchronously updates the UI. This will no block the UI thread until the event is complete.
    ''' </summary>
    Private Sub LogEventAsync(ByVal method As MethodBase, ByVal args() As Object)
        ' COM events are received on background threads.
        If Thread.CurrentThread.IsBackground Then
            ' Dispatch an asynchronous message to the UI thread.
            _uiContext.Post(New SendOrPostCallback(Sub(x) LogEventAsync(method, args)), Nothing)
        Else
            Dim sb As New StringBuilder()

            Dim parameters = method.GetParameters()

            If parameters.Length > 0 Then
                For i As Integer = 0 To parameters.Length - 1
                    Dim parameter = parameters(i)
                    sb.AppendFormat("{0} = '{1}',", parameter.Name, args(i))
                Next i

                sb.Remove(sb.Length - 2, 2)
            End If

            Dim item As New ListViewItem(method.Name)
            item.SubItems.Add(sb.ToString())
            item.ImageIndex = 0

            lvEvents.Items.Add(item)
            item.EnsureVisible()
            lvEvents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        End If
    End Sub
End Class
