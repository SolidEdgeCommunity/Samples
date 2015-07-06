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

    Private _application As SolidEdgeFramework.Application = Nothing
    Private _command As SolidEdgeFramework.Command = Nothing
    Private _mouse As SolidEdgeFramework.Mouse = Nothing

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        SolidEdgeCommunity.OleMessageFilter.Register()

        comboBoxEnableMouseMoveEvent.Items.AddRange(New Object() { True, False })
        comboBoxEnableMouseMoveEvent.SelectedIndex = 1
        LoadLocateModes()
        LoadFilters()
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub buttonStartCommand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonStartCommand.Click
        Try
            _application = SolidEdgeCommunity.SolidEdgeUtils.Connect()
            _command = _application.CreateCommand(CInt(SolidEdgeConstants.seCmdFlag.seNoDeactivate))
            AddHandler _command.Terminate, AddressOf command_Terminate
            _command.Start()
            _mouse = _command.Mouse
            _mouse.LocateMode = comboBoxLocateModes.SelectedIndex
            _mouse.EnabledMove = DirectCast(comboBoxEnableMouseMoveEvent.SelectedItem, Boolean)
            _mouse.ScaleMode = 1 ' Design model coordinates.
            _mouse.WindowTypes = 1 ' Graphic window's only.

            For Each listViewItem As ListViewItem In listViewFilters.CheckedItems
                Dim filter As Integer = DirectCast(listViewItem.Tag, Integer)
                _mouse.AddToLocateFilter(filter)
            Next listViewItem

            AddHandler _mouse.MouseDown, AddressOf mouse_MouseDown
            AddHandler _mouse.MouseMove, AddressOf mouse_MouseMove

            outputTextBox.Clear()
            comboBoxEnableMouseMoveEvent.Enabled = False
            buttonStopCommand.Enabled = True
            buttonStartCommand.Checked = True
            buttonStartCommand.Enabled = False
            comboBoxLocateModes.Enabled = False
            listViewFilters.Enabled = False
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub buttonStopCommand_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonStopCommand.Click
        If _command IsNot Nothing Then
            _command.Done = True
        End If
    End Sub

    Private Sub LoadLocateModes()
        Dim type As Type = GetType(SolidEdgeConstants.seLocateModes)

        Dim fields() As FieldInfo = type.GetFields()
        Dim items As New List(Of ListViewItem)()

        For Each field As FieldInfo In fields
            If field.IsSpecialName Then
                Continue For
            End If

            comboBoxLocateModes.Items.Add(field.Name)
        Next field

        comboBoxLocateModes.SelectedIndex = CInt(SolidEdgeConstants.seLocateModes.seLocateQuickPick)
    End Sub

    Private Sub LoadFilters()
        Dim type As Type = GetType(SolidEdgeConstants.seLocateFilterConstants)

        Dim fields() As FieldInfo = type.GetFields()
        Dim items As New List(Of ListViewItem)()

        For Each field As FieldInfo In fields
            If field.IsSpecialName Then
                Continue For
            End If

            Dim item As New ListViewItem(field.Name)
            item.Tag = field.GetRawConstantValue()
            items.Add(item)
        Next field

        items.Sort(Function(x As ListViewItem, y As ListViewItem) (x.Text.CompareTo(y.Text)))

        listViewFilters.Items.AddRange(items.ToArray())
        listViewFilters.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
    End Sub

    Private Sub HandleCommandTerminate()
        listViewFilters.Enabled = True
        comboBoxEnableMouseMoveEvent.Enabled = True
        comboBoxLocateModes.Enabled = True
        buttonStartCommand.Checked = False
        buttonStartCommand.Enabled = True
        buttonStopCommand.Enabled = False
    End Sub

    Private Sub LogEvent(ByVal eventName As String, ByVal sButton As Short, ByVal sShift As Short, ByVal dX As Double, ByVal dY As Double, ByVal dZ As Double, ByVal pWindowDispatch As Object, ByVal lKeyPointType As Integer, ByVal pGraphicDispatch As Object)
        If _mouse Is Nothing Then
            Return
        End If

        Dim entries As New List(Of String)()

        Dim windowDispatchType As Type = Nothing
        Dim graphicDispatchType As Type = Nothing

        If pWindowDispatch IsNot Nothing Then
            windowDispatchType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(pWindowDispatch)
        End If

        If pGraphicDispatch IsNot Nothing Then
            graphicDispatchType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(pGraphicDispatch)
        End If

        entries.Add("MouseDown event")
        entries.Add(String.Format("sButton: '{0}'", CType(sButton, MouseButtons)))
        entries.Add(String.Format("sShift: '{0}'", sShift))
        entries.Add(String.Format("dX: '{0}'", dX))
        entries.Add(String.Format("dY: '{0}'", dY))
        entries.Add(String.Format("dZ: '{0}'", dZ))
        entries.Add(String.Format("pWindowDispatch: '{0}'", windowDispatchType))
        entries.Add(String.Format("lKeyPointType: '{0}'", lKeyPointType))
        entries.Add(String.Format("pGraphicDispatch: '{0}'", graphicDispatchType))

        If TypeOf pGraphicDispatch Is SolidEdgeFramework.Reference Then
            Dim reference = CType(pGraphicDispatch, SolidEdgeFramework.Reference)
            Dim referenceObject = reference.Object
            Dim referenceObjectType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(referenceObject)
            entries.Add(String.Format("pGraphicDispatch reference: '{0}'", referenceObjectType))
        End If

        Dim PointOnGraphicFlag As Integer = Nothing
        Dim PointOnGraphic_X As Double = Nothing
        Dim PointOnGraphic_Y As Double = Nothing
        Dim PointOnGraphic_Z As Double = Nothing

        ' Get the actual point on the graphic element (in database coordinates). Note this is not
        ' the same as the input dX, dY, dZ coordinates. Those are either in window coordinates
        ' or are the window coordinates transformed to data base coordinates (depends on the
        ' value of ScaleMode). That is, the inputs to this routine are NOT the intersection of the 
        ' bore line and the element.
        _mouse.PointOnGraphic(PointOnGraphicFlag, PointOnGraphic_X, PointOnGraphic_Y, PointOnGraphic_Z)

        ' It would seem that the PointOnGraphicFlag only gets set on MouseDown and stays set until the next MouseDown event.
        If PointOnGraphicFlag = 1 Then
            entries.Add(String.Format("PointOnGraphicFlag: '{0}'", PointOnGraphicFlag))
            entries.Add(String.Format("PointOnGraphic_X: '{0}'", PointOnGraphic_X))
            entries.Add(String.Format("PointOnGraphic_Y: '{0}'", PointOnGraphic_Y))
            entries.Add(String.Format("PointOnGraphic_Z: '{0}'", PointOnGraphic_Z))
        End If

        entries.Add(String.Empty)

        Dim sb As New StringBuilder()
        For Each entry As String In entries
            sb.AppendLine(entry)
        Next entry

        outputTextBox.AppendText(sb.ToString())
    End Sub

    Private Sub command_Terminate()
        Me.Do(Sub(frm) frm.HandleCommandTerminate())

        _mouse = Nothing
        _command = Nothing
        _application = Nothing
    End Sub

    Private Sub mouse_MouseDown(ByVal sButton As Short, ByVal sShift As Short, ByVal dX As Double, ByVal dY As Double, ByVal dZ As Double, ByVal pWindowDispatch As Object, ByVal lKeyPointType As Integer, ByVal pGraphicDispatch As Object)
        ' Note: Thread.CurrentThread.IsBackground = true so we must Invoke a call back to the main GUI thread.
        Me.Do(Sub(frm) frm.LogEvent("MouseDown", sButton, sShift, dX, dY, dZ, pWindowDispatch, lKeyPointType, pGraphicDispatch))
    End Sub

    Private Sub mouse_MouseMove(ByVal sButton As Short, ByVal sShift As Short, ByVal dX As Double, ByVal dY As Double, ByVal dZ As Double, ByVal pWindowDispatch As Object, ByVal lKeyPointType As Integer, ByVal pGraphicDispatch As Object)
        ' Note: Thread.CurrentThread.IsBackground = true so we must Invoke a call back to the main GUI thread.
        Me.Do(Sub(frm) frm.LogEvent("MouseMove", sButton, sShift, dX, dY, dZ, pWindowDispatch, lKeyPointType, pGraphicDispatch))
    End Sub
End Class
