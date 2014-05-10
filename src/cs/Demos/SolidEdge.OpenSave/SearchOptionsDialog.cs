using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolidEdge.OpenSave
{
    public partial class SearchOptionsDialog : Form
    {
        private FolderBrowserDialog _folderBrowserDialog = new FolderBrowserDialog();

        public SearchOptionsDialog()
        {
            InitializeComponent();
        }

        private void SearchOptionsDialog_Load(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewExtensions.Items)
            {
                string extension = item.Text;
                Icon icon = IconTools.GetIconForExtension(extension, ShellIconSize.SmallIcon);
                if (icon != null)
                {
                    imageList.Images.Add(icon);
                    imageList.Images.SetKeyName(imageList.Images.Count - 1, extension);
                    item.ImageKey = extension;
                }
                item.Checked = true;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            _folderBrowserDialog.Description = "Select folder";
            _folderBrowserDialog.ShowNewFolderButton = false;

            if (_folderBrowserDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                listViewFolders.Items.Add(_folderBrowserDialog.SelectedPath);
                listViewFolders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            ListViewItem[] items = listViewFolders.SelectedItems.OfType<ListViewItem>().ToArray();
            foreach (ListViewItem item in items)
            {
                item.Remove();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        public string[] SelectedExtensions
        {
            get
            {
                List<string> list = new List<string>();

                foreach (ListViewItem item in listViewExtensions.CheckedItems)
                {
                    list.Add(item.Text);
                }

                return list.ToArray();
            }
        }

        public string[] SelectedFolders
        {
            get
            {
                List<string> list = new List<string>();

                foreach (ListViewItem item in listViewFolders.Items)
                {
                    list.Add(item.Text);
                }

                return list.ToArray();
            }
        }

        public bool IncludeSubDirectories
        {
            get
            {
                return checkBoxIncludeSubDirectories.Checked;
            }
        }
    }
}
