Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Runtime.InteropServices.ComTypes
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Reflection

Partial Public Class MainForm
    Inherits Form

    Private _uiContext As SynchronizationContext
    Private _application As SolidEdgeFramework.Application = Nothing
    Private _applicationEvents As SolidEdgeFramework.ISEApplicationEvents_Event

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
        ' Unhook events.
        DisconnectApplicationEvents()
        _application = Nothing
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

    ''' <summary>
    ''' Synchronously updates the UI. This will block the UI thread until the event is complete.
    ''' </summary>
    ''' <remarks>
    ''' If one of the event arguments is a COM object, you should use this approach as the COM object could be
    ''' freed before the method completes causing an exception.
    ''' </remarks>
    Public Sub LogEvent(ByVal method As MethodBase, ByVal args() As Object)
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
    Public Sub LogEventAsync(ByVal method As MethodBase, ByVal args() As Object)
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
        End If
    End Sub

    #Region "SolidEdgeFramework.ISEApplicationEvents"

    Private Sub ISEApplicationEvents_AfterActiveDocumentChange(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument })
    End Sub

    Private Sub ISEApplicationEvents_AfterCommandRun(ByVal theCommandID As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theCommandID })
    End Sub

    Private Sub ISEApplicationEvents_AfterDocumentOpen(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument })
    End Sub

    Private Sub ISEApplicationEvents_AfterDocumentPrint(ByVal theDocument As Object, ByVal hDC As Integer, ByRef ModelToDC As Double, ByRef Rect As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument, hDC, ModelToDC, Rect })
    End Sub

    Private Sub ISEApplicationEvents_AfterDocumentSave(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument })
    End Sub

    Private Sub ISEApplicationEvents_AfterEnvironmentActivate(ByVal theEnvironment As Object)

        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theEnvironment })
    End Sub

    Private Sub ISEApplicationEvents_AfterNewDocumentOpen(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument })
    End Sub

    Private Sub ISEApplicationEvents_AfterNewWindow(ByVal theWindow As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theWindow })
    End Sub

    Private Sub ISEApplicationEvents_AfterWindowActivate(ByVal theWindow As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theWindow })
    End Sub

    Private Sub ISEApplicationEvents_BeforeCommandRun(ByVal theCommandID As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theCommandID })
    End Sub

    Private Sub ISEApplicationEvents_BeforeDocumentClose(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument })
    End Sub

    Private Sub ISEApplicationEvents_BeforeDocumentPrint(ByVal theDocument As Object, ByVal hDC As Integer, ByRef ModelToDC As Double, ByRef Rect As Integer)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument, hDC, ModelToDC, Rect })
    End Sub

    Private Sub ISEApplicationEvents_BeforeDocumentSave(ByVal theDocument As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theDocument })
    End Sub

    Private Sub ISEApplicationEvents_BeforeEnvironmentDeactivate(ByVal theEnvironment As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theEnvironment })
    End Sub

    Private Sub ISEApplicationEvents_BeforeQuit()
        ' COM events are received on background threads.
        If Thread.CurrentThread.IsBackground Then
            ' Dispatch an synchronous message to the UI thread.
            _uiContext.Send(New SendOrPostCallback(Sub(x) ISEApplicationEvents_BeforeQuit()), Nothing)
        Else
            LogEvent(MethodInfo.GetCurrentMethod(), New Object() { })

            eventButton.Checked = False
        End If
    End Sub

    Private Sub ISEApplicationEvents_BeforeWindowDeactivate(ByVal theWindow As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { theWindow })
    End Sub

    #End Region

    #Region "SolidEdgeFramework.ISEDocumentEvents"

    Private Sub ISEDocumentEvents_AfterSave()
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { })
    End Sub

    Private Sub ISEDocumentEvents_BeforeClose()
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { })
    End Sub

    Private Sub ISEDocumentEvents_BeforeSave()
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { })
    End Sub

    Private Sub ISEDocumentEvents_SelectSetChanged(ByVal SelectSet As Object)
        LogEvent(MethodInfo.GetCurrentMethod(), New Object() { SelectSet })
    End Sub

    #End Region

    #region "Event hooking-unhooking"

    Private Sub ConnectApplicationEvents()
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
End Class
