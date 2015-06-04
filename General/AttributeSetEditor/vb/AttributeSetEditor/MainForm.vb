Option Strict Off

Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Private _application As SolidEdgeFramework.Application
    Private _document As SolidEdgeFramework.SolidEdgeDocument
    Private _documentEvents As SolidEdgeFramework.ISEDocumentEvents_Event

    Public Sub New()
        InitializeComponent()

        imageList.Images.Add(My.Resources.AttributeSets_16x16)
        imageList.Images.Add(My.Resources.AttributeSet_16x16)
        imageList.Images.Add(My.Resources.AttributeSetMissing_16x16)
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            UpdateTreeView()
        Catch
        End Try
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        ReleaseDocument()
    End Sub

    Private Sub buttonRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonRefresh.Click
        UpdateTreeView()
    End Sub

    Private Sub UpdateTreeView()
        If _application Is Nothing Then
            Try
                _application = SolidEdgeUtils.Connect()
            Catch
                MessageBox.Show("Solid Edge is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If

        If _document Is Nothing Then
            Try
                _document = DirectCast(_application.ActiveDocument, SolidEdgeFramework.SolidEdgeDocument)
            Catch
            End Try
        End If

        If _document IsNot Nothing Then
            Try
                _documentEvents = DirectCast(_document.DocumentEvents, SolidEdgeFramework.ISEDocumentEvents_Event)
                AddHandler _documentEvents.BeforeClose, AddressOf _documentEvents_BeforeClose
                AddHandler _documentEvents.SelectSetChanged, AddressOf _documentEvents_SelectSetChanged

                UpdateTreeView(_application.ActiveSelectSet)
            Catch ex As System.Exception
                _document = Nothing
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub UpdateTreeView(ByVal selectSet As SolidEdgeFramework.SelectSet)
        Dim rootNode = tvSelectSet.Nodes(0)
        rootNode.Nodes.Clear()
        lvAttributes.Items.Clear()

        For i As Integer = 1 To selectSet.Count
            Dim item As Object = Nothing
            Dim itemType As Type = Nothing

            Try
                item = selectSet.Item(i)
                itemType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(item)
            Catch
            End Try

            If (item IsNot Nothing) AndAlso (itemType IsNot Nothing) Then
                Dim itemNode As New TreeNode(String.Format("Item {0} ({1})", i, itemType.FullName))
                Dim [property] = itemType.GetProperty("AttributeSets")

                If [property] IsNot Nothing Then
                    itemNode.ImageIndex = 1
                Else
                    itemNode.ImageIndex = 2
                End If

                itemNode.SelectedImageIndex = itemNode.ImageIndex
                itemNode.Tag = item

                rootNode.Nodes.Add(itemNode)
            End If
        Next i

        If rootNode.Nodes.Count > 0 Then
            tvSelectSet.SelectedNode = rootNode.Nodes(0)
        End If

        rootNode.ExpandAll()
    End Sub

    Private Sub UpdateListView()
        Dim attributeSets As SolidEdgeFramework.AttributeSets = Nothing
        Dim attributeSet As SolidEdgeFramework.AttributeSet = Nothing
        Dim attribute As SolidEdgeFramework.Attribute = Nothing

        lvAttributes.Items.Clear()
        buttonAddAttribute.Enabled = False
        buttonRemoveAttribute.Enabled = False

        If lvAttributes.Tag IsNot Nothing Then
            buttonAddAttribute.Enabled = True

            Dim selectSetItem As Object = lvAttributes.Tag
            attributeSets = selectSetItem.AttributeSets

            For i As Integer = 1 To attributeSets.Count
                attributeSet = attributeSets.Item(i)

                For j As Integer = 1 To attributeSet.Count
                    Dim lvItem = New ListViewItem()

                    attribute = attributeSet.Item(j)
                    lvItem.Text = attributeSet.SetName
                    lvItem.SubItems.Add(attribute.Name)
                    lvItem.SubItems.Add(attribute.Value.ToString())
                    lvItem.SubItems.Add(attribute.Value.GetType().FullName)

                    lvAttributes.Items.Add(lvItem)
                Next j
            Next i
        End If
    End Sub

    Private Sub _documentEvents_BeforeClose()
        ReleaseDocument()
    End Sub

    Private Sub _documentEvents_SelectSetChanged(ByVal SelectSet As Object)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of SolidEdgeFramework.SelectSet)(AddressOf UpdateTreeView), DirectCast(SelectSet, SolidEdgeFramework.SelectSet))
            Return
        End If
    End Sub

    Private Sub tvSelectSet_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) Handles tvSelectSet.AfterSelect
        lvAttributes.Tag = e.Node.Tag
        UpdateListView()
    End Sub

    Private Sub buttonAddAttribute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonAddAttribute.Click
        Dim attributeSets As SolidEdgeFramework.AttributeSets = Nothing
        Dim attributeSet As SolidEdgeFramework.AttributeSet = Nothing
        Dim attribute As SolidEdgeFramework.Attribute = Nothing

        Dim dialog = New AddAttributeDialog()

        If dialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Dim setName = dialog.SetName
            Dim attributeName = dialog.AttributeName
            Dim attributevalue = dialog.AttributeValue
            Dim selectSetItem As Object = lvAttributes.Tag

            attributeSets = selectSetItem.AttributeSets

            Try
                attributeSet = attributeSets.Item(setName)
            Catch
                attributeSet = attributeSets.Add(setName)
            End Try

            attribute = attributeSet.Add(attributeName, SolidEdgeFramework.AttributeTypeConstants.seStringUnicode)
            attribute.Value = attributevalue
        End If

        UpdateListView()
    End Sub

    Private Sub buttonRemoveAttribute_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonRemoveAttribute.Click
        Dim attributeSets As SolidEdgeFramework.AttributeSets = Nothing
        Dim attributeSet As SolidEdgeFramework.AttributeSet = Nothing

        Dim itemsToDelete As New List(Of ListViewItem)()

        For Each item As ListViewItem In lvAttributes.SelectedItems
            If item.Selected Then
                itemsToDelete.Add(item)
            End If
        Next item

        Dim selectSetItem As Object = lvAttributes.Tag

        For Each item In itemsToDelete
            Dim setName = item.Text
            Dim attributeName = item.SubItems(1).Text

            attributeSets = selectSetItem.AttributeSets
            attributeSet = attributeSets.Item(setName)
            attributeSet.Remove(attributeName)

            If attributeSet.Count = 0 Then
                attributeSets.Remove(attributeSet.SetName)
            End If

            lvAttributes.Items.Remove(item)
        Next item
    End Sub

    Private Sub lvAttributes_ItemSelectionChanged(ByVal sender As Object, ByVal e As ListViewItemSelectionChangedEventArgs) Handles lvAttributes.ItemSelectionChanged
        If e.Item IsNot Nothing Then
            buttonRemoveAttribute.Enabled = True
        Else
            buttonRemoveAttribute.Enabled = False
        End If
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub ReleaseDocument()
        If _documentEvents IsNot Nothing Then
            RemoveHandler _documentEvents.SelectSetChanged, AddressOf _documentEvents_SelectSetChanged
            RemoveHandler _documentEvents.BeforeClose, AddressOf _documentEvents_BeforeClose

            Marshal.FinalReleaseComObject(_documentEvents)
            _documentEvents = Nothing
        End If

        If _document IsNot Nothing Then
            Marshal.FinalReleaseComObject(_document)
            _document = Nothing
        End If
    End Sub
End Class
