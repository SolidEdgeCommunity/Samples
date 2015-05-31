Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Private _folderBrowserDialog As New FolderBrowserDialog()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        BeginInvoke(New MethodInvoker(AddressOf MainForm_Load_Async))
    End Sub

    ''' <summary>
    ''' Asynchronous version of MainForm_Load.
    ''' </summary>
    Private Sub MainForm_Load_Async()
        toolStripStatusLabel.Text = "Connecting to Solid Edge to get a copy of DraftPrintUtility settings."

        Dim application As SolidEdgeFramework.Application = Nothing

        Try
            ' Connect to or start Solid Edge.
            application = SolidEdgeUtils.Connect(True, True)

            ' Minimize Solid Edge.
            'application.WindowState = (int)FormWindowState.Minimized;

            ' Get a copy of the settings for use as a template.
            customListView._draftPrintUtilityOptions = New DraftPrintUtilityOptions(application)

            ' Enable UI.
            toolStrip.Enabled = True
            customListView.Enabled = True
        Catch ex As System.Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If application IsNot Nothing Then
                Marshal.ReleaseComObject(application)
            End If
        End Try

        toolStripStatusLabel.Text = "Tip: You can drag folders and files into the ListView."
    End Sub

    Private Sub buttonOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonOpen.Click
        _folderBrowserDialog.ShowNewFolderButton = False

        If _folderBrowserDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            Dim searchOption As SearchOption = System.IO.SearchOption.TopDirectoryOnly
            Dim directoryInfo As New DirectoryInfo(_folderBrowserDialog.SelectedPath)
            Dim subDirectoryInfos() As DirectoryInfo = directoryInfo.GetDirectories()

            ' If directory has subdirectories, ask user if we should process those as well.
            If subDirectoryInfos.Length > 0 Then
                ' Build the question to ask the user.
                Dim message As String = String.Format("'{0}' contains subdirectories. Would you like to include those in the search?", directoryInfo.FullName)

                ' Ask the question.
                Select Case MessageBox.Show(message, "Process subdirectories?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                    Case System.Windows.Forms.DialogResult.Yes
                        searchOption = System.IO.SearchOption.AllDirectories
                    Case System.Windows.Forms.DialogResult.Cancel
                        ' Bail out of entire OnDragDrop().
                        Return
                End Select
            End If

            customListView.AddFolder(directoryInfo, searchOption)
        End If
    End Sub

    Private Sub buttonPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonPrint.Click
        ' Not necessary but we'll later highlight each one as we print them.
        For Each listViewItem As ListViewItem In customListView.Items
            listViewItem.Selected = False
        Next listViewItem

        ' Loop through all of the files and print them.
        For Each listViewItem As ListViewItem In customListView.Items
            ' GUI sugar.  Highligh the item.
            listViewItem.Selected = True

            Dim filename As String = listViewItem.Text
            Dim options As DraftPrintUtilityOptions = DirectCast(listViewItem.Tag, DraftPrintUtilityOptions)

            'AppDomain interopDomain = null;

            Try
                toolStripStatusLabel.Text = "Setting up an isolated application domation for COM Interop."

                toolStripStatusLabel.Text = String.Format("Printing '{0}' in isolated application.", filename)

                Using task = New IsolatedTask(Of BatchPrintTask)()
                    task.Proxy.Print(filename, options)
                End Using

                toolStripStatusLabel.Text = String.Empty
            Catch ex As System.Exception
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                listViewItem.Selected = False
            End Try
        Next listViewItem

        toolStripStatusLabel.Text = String.Empty
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub customListView_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles customListView.SelectedIndexChanged
        Dim list As New List(Of Object)()

        For Each listViewItem As ListViewItem In customListView.Items
            If listViewItem.Selected Then
                list.Add(listViewItem.Tag)
            End If
        Next listViewItem

        propertyGrid.SelectedObjects = list.ToArray()
    End Sub
End Class
