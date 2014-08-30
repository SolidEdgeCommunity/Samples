using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileProperties.WinForm
{
    public partial class MainForm : Form
    {
        private OpenFileDialog _openFileDialog = new OpenFileDialog();
        private AppDomain interopDomain = null;
        private InteropProxy interopProxy = null;

        public MainForm()
        {
            InitializeComponent();

            _openFileDialog.Filter = "Solid Edge Assembly (*.asm)|*.asm|Solid Edge Draft (*.dft)|*.dft|Solid Edge Part (*.par)|*.par|Solid Edge SheetMetal (*.psm)|*.psm";
            _openFileDialog.ShowReadOnly = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                toolStripStatusLabel1.Text = _openFileDialog.FileName;
                saveToolStripMenuItem.Enabled = !_openFileDialog.ReadOnlyChecked;

                if (interopDomain != null)
                {
                    closeToolStripMenuItem_Click(sender, e);
                }

                if (interopDomain == null)
                {
                    // Create a custom AppDomain to do COM Interop.
                    interopDomain = AppDomain.CreateDomain("Interop Domain");

                    Type proxyType = typeof(InteropProxy);

                    // Create a new instance of InteropProxy in the isolated application domain.
                    InteropProxy interopProxy = interopDomain.CreateInstanceAndUnwrap(
                        proxyType.Assembly.FullName,
                        proxyType.FullName) as InteropProxy;

                    interopProxy.Open(_openFileDialog.FileName, _openFileDialog.ReadOnlyChecked);

                    // Get the list of properties.
                    var properties = interopProxy.GetProperties();

                    List<ListViewItem> items = new List<ListViewItem>();

                    // Populate the ListView.
                    foreach (var property in properties)
                    {
                        if (listView1.Groups[property.PropertySetName] == null)
                        {
                            listView1.Groups.Add(property.PropertySetName, property.PropertySetName);
                        }

                        var item = new ListViewItem(listView1.Groups[property.PropertySetName]);
                        item.ImageIndex = 0;
                        item.Text = property.PropertyName;
                        item.SubItems.Add(String.Format("{0}", property.PropertyValue));
                        items.Add(item);
                    }

                    listView1.Items.AddRange(items.ToArray());
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = String.Empty;
            listView1.Items.Clear();

            if (interopProxy != null)
            {
                interopProxy.Close();
            }

            interopProxy = null;
            AppDomain.Unload(interopDomain);
            interopDomain = null;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (interopProxy != null)
            {
                try
                {
                    interopProxy.Save();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
