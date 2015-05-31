using log4net;
using log4net.Config;
using SolidEdgeCommunity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolidEdge.OpenSave
{
    public partial class MainForm : Form
    {
        private static ILog _log = LogManager.GetLogger(typeof(Program));
        private Version _currentVersion;
        private OpenSaveSettings _openSaveSettings = new OpenSaveSettings();

        static MainForm()
        {
            XmlConfigurator.Configure();
        }

        public MainForm()
        {
            this.Font = SystemFonts.MessageBoxFont;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var textBoxAppender = LogManager.GetRepository().GetAppenders().OfType<TextBoxAppender>().FirstOrDefault();

            if (textBoxAppender != null)
            {
                textBoxAppender.TextBox = outputTextBox;
            }

            _currentVersion = SolidEdgeCommunity.SolidEdgeUtils.GetVersion();
            propertyGrid.SelectedObject = _openSaveSettings;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If background threads are executing, prevent closing of form.
            if (searchBackgroundWorker.IsBusy || openSaveBackgroundWorker.IsBusy)
            {
                e.Cancel = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            SearchOptionsDialog dialog = new SearchOptionsDialog();

            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                // Start background thread.
                searchBackgroundWorker.RunWorkerAsync(new object[] { dialog.SelectedExtensions, dialog.SelectedFolders, dialog.IncludeSubDirectories, _currentVersion });
            }

            UpdateUIState();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (openSaveBackgroundWorker.IsBusy == false)
            {
                List<string> files = new List<string>();

                foreach (ListViewItem item in listViewFiles.Items)
                {
                    Version lastSavedVersion = new Version(item.SubItems[1].Text);

                    // Make sure file has not already been upgraded.
                    if (_currentVersion.CompareTo(lastSavedVersion) > 0)
                    {
                        // Name property contains full path to file.
                        files.Add(item.Name);
                    }
                }

                // Start background thread.
                openSaveBackgroundWorker.RunWorkerAsync(files.ToArray());

                UpdateUIState();
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            searchBackgroundWorker.CancelAsync();
            openSaveBackgroundWorker.CancelAsync();
        }

        private void searchBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] args = (object[])e.Argument;
            string[] extensions = (string[])args[0];
            string[] folders = (string[])args[1];
            bool includeSubDirectories = (bool)args[2];
            Version currentVersion = (Version)args[3];
            SearchOption searchOption = includeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            List<string> files = new List<string>();
            List<SearchResultItem> results = new List<SearchResultItem>();

            // Build file list.
            foreach (string folder in folders)
            {
                if (searchBackgroundWorker.CancellationPending) break;

                _log.Info(String.Format("Scanning {0}.", folder));

                try
                {
                    foreach (string extension in extensions)
                    {
                        if (searchBackgroundWorker.CancellationPending) break;

                        files.AddRange(Directory.GetFiles(folder, String.Format("*{0}", extension), searchOption));
                    }
                }
                catch (System.Exception ex)
                {
                    _log.Error(String.Format("Error while scanning {0}.", folder), ex);
                }
            }

            // Determine if each file needs to be upgraded by comparing versions.
            // Note that we're leveraging http://www.nuget.org/packages/PowerToys.SolidEdge.Core to get the last saved version.
            foreach (string file in files)
            {
                if (searchBackgroundWorker.CancellationPending) break;

                try
                {
                    Version lastSavedVersion = SolidEdgeCommunity.Reader.SolidEdgeDocument.GetLastSavedVersion(file);

                    if (currentVersion.CompareTo(lastSavedVersion) > 0)
                    {
                        results.Add(new SearchResultItem(file, lastSavedVersion));
                    }
                }
                catch (System.Exception ex)
                {
                    _log.Error(String.Format("Error while processing {0}.", file), ex);
                }
            }

            e.Result = results.ToArray();
        }

        private void searchBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SearchResultItem[] results = e.Result as SearchResultItem[];

            // Process search results.
            if (results != null)
            {
                List<ListViewItem> items = new List<ListViewItem>();

                foreach (SearchResultItem result in results)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = Path.GetFileName(result.FileName);
                    item.SubItems.Add(result.Version.ToString());
                    item.SubItems.Add(Path.GetDirectoryName(result.FileName));
                    item.Name = result.FileName;

                    try
                    {
                        string extension = Path.GetExtension(result.FileName).ToLower();
                        item.ImageKey = extension;

                        if (smallImageList.Images.ContainsKey(extension) == false)
                        {
                            Icon icon = IconTools.GetIconForFile(result.FileName, ShellIconSize.SmallIcon);
                            if (icon != null)
                            {
                                smallImageList.Images.Add(icon);
                                smallImageList.Images.SetKeyName(smallImageList.Images.Count - 1, extension);
                            }
                        }
                    }
                    catch
                    {
                    }

                    items.Add(item);
                }

                listViewFiles.Items.AddRange(items.ToArray());

                if (listViewFiles.Items.Count > 0)
                {
                    listViewFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }

            UpdateUIState();
        }

        private void openSaveBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] files = (string[])e.Argument;

            foreach (string file in files)
            {
                if (openSaveBackgroundWorker.CancellationPending) break;

                if (File.Exists(file))
                {
                    _log.InfoFormat("Processing '{0}'.", file);

                    try
                    {
                        using (var task = new IsolatedTask<OpenSaveTask>())
                        {
                            task.Proxy.DoOpenSave(file, _openSaveSettings);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        _log.Error(ex.Message, ex);
                    }
                }
            }
        }

        private void openSaveBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string key = e.UserState as string;

            if (listViewFiles.Items.ContainsKey(key))
            {
                ListViewItem item = listViewFiles.Items[key];
                listViewFiles.EnsureVisible(item.Index);

                if (e.ProgressPercentage < 100)
                {
                    item.Selected = true;
                    item.Focused = true;
                }
                else
                {
                    item.Remove();
                }
            }
        }

        private void openSaveBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateUIState();
        }

        private void UpdateUIState()
        {
            bool enabled = true;

            if (searchBackgroundWorker.IsBusy || openSaveBackgroundWorker.IsBusy)
            {
                enabled = false;
            }

            listViewFiles.Enabled = enabled;
            propertyGrid.Enabled = enabled;
            buttonSelectFolder.Enabled = enabled;
            buttonStart.Enabled = enabled;
        }
    }
}
