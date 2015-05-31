Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Partial Public Class SearchOptionsDialog
    Inherits Form

    Private _folderBrowserDialog As New FolderBrowserDialog()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub SearchOptionsDialog_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        For Each item As ListViewItem In listViewExtensions.Items
            Dim extension As String = item.Text
'INSTANT VB NOTE: The variable icon was renamed since Visual Basic does not handle local variables named the same as class members well:
            Dim icon_Renamed As Icon = IconTools.GetIconForExtension(extension, ShellIconSize.SmallIcon)
            If icon_Renamed IsNot Nothing Then
                imageList.Images.Add(icon_Renamed)
                imageList.Images.SetKeyName(imageList.Images.Count - 1, extension)
                item.ImageKey = extension
            End If
            item.Checked = True
        Next item
    End Sub

    Private Sub buttonAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonAdd.Click
        _folderBrowserDialog.Description = "Select folder"
        _folderBrowserDialog.ShowNewFolderButton = False

        If _folderBrowserDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            listViewFolders.Items.Add(_folderBrowserDialog.SelectedPath)
            listViewFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
        End If
    End Sub

    Private Sub buttonRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonRemove.Click
        Dim items() As ListViewItem = listViewFolders.SelectedItems.OfType(Of ListViewItem)().ToArray()
        For Each item As ListViewItem In items
            item.Remove()
        Next item
    End Sub

    Private Sub buttonOK_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonOK.Click
        DialogResult = System.Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub buttonCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonCancel.Click
        DialogResult = System.Windows.Forms.DialogResult.Cancel
        Close()
    End Sub

    Public ReadOnly Property SelectedExtensions() As String()
        Get
            Dim list As New List(Of String)()

            For Each item As ListViewItem In listViewExtensions.CheckedItems
                list.Add(item.Text)
            Next item

            Return list.ToArray()
        End Get
    End Property

    Public ReadOnly Property SelectedFolders() As String()
        Get
            Dim list As New List(Of String)()

            For Each item As ListViewItem In listViewFolders.Items
                list.Add(item.Text)
            Next item

            Return list.ToArray()
        End Get
    End Property

    Public ReadOnly Property IncludeSubDirectories() As Boolean
        Get
            Return checkBoxIncludeSubDirectories.Checked
        End Get
    End Property
End Class
