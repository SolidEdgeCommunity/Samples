Imports log4net
Imports log4net.Config
Imports SolidEdgeCommunity
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms

Partial Public Class MainForm
    Inherits Form

    Private Shared _log As ILog = LogManager.GetLogger(GetType(Program))
    Private _currentVersion As Version
    Private _openSaveSettings As New OpenSaveSettings()

    Shared Sub New()
        XmlConfigurator.Configure()
    End Sub

    Public Sub New()
        Me.Font = SystemFonts.MessageBoxFont
        InitializeComponent()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim textBoxAppender = LogManager.GetRepository().GetAppenders().OfType(Of TextBoxAppender)().FirstOrDefault()

        If textBoxAppender IsNot Nothing Then
            textBoxAppender.TextBox = outputTextBox
        End If

        _currentVersion = SolidEdgeCommunity.SolidEdgeUtils.GetVersion()
        propertyGrid.SelectedObject = _openSaveSettings
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        ' If background threads are executing, prevent closing of form.
        If searchBackgroundWorker.IsBusy OrElse openSaveBackgroundWorker.IsBusy Then
            e.Cancel = True
        End If
    End Sub

    Private Sub exitToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles exitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub buttonSelectFolder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonSelectFolder.Click
        Dim dialog As New SearchOptionsDialog()

        If dialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
            ' Start background thread.
            searchBackgroundWorker.RunWorkerAsync(New Object() { dialog.SelectedExtensions, dialog.SelectedFolders, dialog.IncludeSubDirectories, _currentVersion })
        End If

        UpdateUIState()
    End Sub

    Private Sub buttonStart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonStart.Click
        If openSaveBackgroundWorker.IsBusy = False Then
            Dim files As New List(Of String)()

            For Each item As ListViewItem In listViewFiles.Items
                Dim lastSavedVersion As New Version(item.SubItems(1).Text)

                ' Make sure file has not already been upgraded.
                If _currentVersion.CompareTo(lastSavedVersion) > 0 Then
                    ' Name property contains full path to file.
                    files.Add(item.Name)
                End If
            Next item

            ' Start background thread.
            openSaveBackgroundWorker.RunWorkerAsync(files.ToArray())

            UpdateUIState()
        End If
    End Sub

    Private Sub buttonStop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles buttonStop.Click
        searchBackgroundWorker.CancelAsync()
        openSaveBackgroundWorker.CancelAsync()
    End Sub

    Private Sub searchBackgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles searchBackgroundWorker.DoWork
        Dim args() As Object = DirectCast(e.Argument, Object())
        Dim extensions() As String = DirectCast(args(0), String())
        Dim folders() As String = DirectCast(args(1), String())
        Dim includeSubDirectories As Boolean = DirectCast(args(2), Boolean)
        Dim currentVersion As Version = DirectCast(args(3), Version)
        Dim searchOption As SearchOption = If(includeSubDirectories, System.IO.SearchOption.AllDirectories, System.IO.SearchOption.TopDirectoryOnly)

        Dim files As New List(Of String)()
        Dim results As New List(Of SearchResultItem)()

        ' Build file list.
        For Each folder As String In folders
            If searchBackgroundWorker.CancellationPending Then
                Exit For
            End If

            _log.Info(String.Format("Scanning {0}.", folder))

            Try
                For Each extension As String In extensions
                    If searchBackgroundWorker.CancellationPending Then
                        Exit For
                    End If

                    files.AddRange(Directory.GetFiles(folder, String.Format("*{0}", extension), searchOption))
                Next extension
            Catch ex As System.Exception
                _log.Error(String.Format("Error while scanning {0}.", folder), ex)
            End Try
        Next folder

        ' Determine if each file needs to be upgraded by comparing versions.
        ' Note that we're leveraging http://www.nuget.org/packages/PowerToys.SolidEdge.Core to get the last saved version.
        For Each file As String In files
            If searchBackgroundWorker.CancellationPending Then
                Exit For
            End If

            Try
                Dim lastSavedVersion As Version = SolidEdgeCommunity.Reader.SolidEdgeDocument.GetLastSavedVersion(file)

                If currentVersion.CompareTo(lastSavedVersion) > 0 Then
                    results.Add(New SearchResultItem(file, lastSavedVersion))
                End If
            Catch ex As System.Exception
                _log.Error(String.Format("Error while processing {0}.", file), ex)
            End Try
        Next file

        e.Result = results.ToArray()
    End Sub

    Private Sub searchBackgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles searchBackgroundWorker.RunWorkerCompleted
        Dim results() As SearchResultItem = TryCast(e.Result, SearchResultItem())

        ' Process search results.
        If results IsNot Nothing Then
            Dim items As New List(Of ListViewItem)()

            For Each result As SearchResultItem In results
                Dim item As New ListViewItem()
                item.Text = Path.GetFileName(result.FileName)
                item.SubItems.Add(result.Version.ToString())
                item.SubItems.Add(Path.GetDirectoryName(result.FileName))
                item.Name = result.FileName

                Try
                    Dim extension As String = Path.GetExtension(result.FileName).ToLower()
                    item.ImageKey = extension

                    If smallImageList.Images.ContainsKey(extension) = False Then
'INSTANT VB NOTE: The variable icon was renamed since Visual Basic does not handle local variables named the same as class members well:
                        Dim icon_Renamed As Icon = IconTools.GetIconForFile(result.FileName, ShellIconSize.SmallIcon)
                        If icon_Renamed IsNot Nothing Then
                            smallImageList.Images.Add(icon_Renamed)
                            smallImageList.Images.SetKeyName(smallImageList.Images.Count - 1, extension)
                        End If
                    End If
                Catch
                End Try

                items.Add(item)
            Next result

            listViewFiles.Items.AddRange(items.ToArray())

            If listViewFiles.Items.Count > 0 Then
                listViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
        End If

        UpdateUIState()
    End Sub

    Private Sub openSaveBackgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles openSaveBackgroundWorker.DoWork
        Dim files() As String = DirectCast(e.Argument, String())

        For Each file As String In files
            If openSaveBackgroundWorker.CancellationPending Then
                Exit For
            End If

            If System.IO.File.Exists(file) Then
                _log.InfoFormat("Processing '{0}'.", file)

                Try
                    Using task = New IsolatedTask(Of OpenSaveTask)()
                        task.Proxy.DoOpenSave(file, _openSaveSettings)
                    End Using
                Catch ex As System.Exception
                    _log.Error(ex.Message, ex)
                End Try
            End If
        Next file
    End Sub

    Private Sub openSaveBackgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles openSaveBackgroundWorker.ProgressChanged
        Dim key As String = TryCast(e.UserState, String)

        If listViewFiles.Items.ContainsKey(key) Then
            Dim item As ListViewItem = listViewFiles.Items(key)
            listViewFiles.EnsureVisible(item.Index)

            If e.ProgressPercentage < 100 Then
                item.Selected = True
                item.Focused = True
            Else
                item.Remove()
            End If
        End If
    End Sub

    Private Sub openSaveBackgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles openSaveBackgroundWorker.RunWorkerCompleted
        UpdateUIState()
    End Sub

    Private Sub UpdateUIState()
'INSTANT VB NOTE: The variable enabled was renamed since Visual Basic does not handle local variables named the same as class members well:
        Dim enabled_Renamed As Boolean = True

        If searchBackgroundWorker.IsBusy OrElse openSaveBackgroundWorker.IsBusy Then
            enabled_Renamed = False
        End If

        listViewFiles.Enabled = enabled_Renamed
        propertyGrid.Enabled = enabled_Renamed
        buttonSelectFolder.Enabled = enabled_Renamed
        buttonStart.Enabled = enabled_Renamed
    End Sub
End Class
