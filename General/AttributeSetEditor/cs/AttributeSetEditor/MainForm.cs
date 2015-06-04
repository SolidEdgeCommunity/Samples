using SolidEdgeCommunity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AttributeSetEditor
{
    public partial class MainForm : Form
    {
        private SolidEdgeFramework.Application _application;
        private SolidEdgeFramework.SolidEdgeDocument _document;
        private SolidEdgeFramework.ISEDocumentEvents_Event _documentEvents;

        public MainForm()
        {
            InitializeComponent();

            imageList.Images.Add(AttributeSetEditor.Properties.Resources.AttributeSets_16x16);
            imageList.Images.Add(AttributeSetEditor.Properties.Resources.AttributeSet_16x16);
            imageList.Images.Add(AttributeSetEditor.Properties.Resources.AttributeSetMissing_16x16);
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateTreeView();
            }
            catch
            {
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ReleaseDocument();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            UpdateTreeView();
        }

        private void UpdateTreeView()
        {
            if (_application == null)
            {
                try
                {
                    _application = SolidEdgeUtils.Connect();
                }
                catch
                {
                    MessageBox.Show("Solid Edge is not running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (_document == null)
            {
                try
                {
                    _document = (SolidEdgeFramework.SolidEdgeDocument)_application.ActiveDocument;
                }
                catch
                {
                }
            }

            if (_document != null)
            {
                try
                {
                    _documentEvents = (SolidEdgeFramework.ISEDocumentEvents_Event)_document.DocumentEvents;
                    _documentEvents.BeforeClose += _documentEvents_BeforeClose;
                    _documentEvents.SelectSetChanged += _documentEvents_SelectSetChanged;

                    UpdateTreeView(_application.ActiveSelectSet);
                }
                catch (System.Exception ex)
                {
                    _document = null;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateTreeView(SolidEdgeFramework.SelectSet selectSet)
        {
            var rootNode = tvSelectSet.Nodes[0];
            rootNode.Nodes.Clear();
            lvAttributes.Items.Clear();

            for (int i = 1; i <= selectSet.Count; i++)
            {
                dynamic item = null;
                Type itemType = null;

                try
                {
                    item = selectSet.Item(i);
                    itemType = SolidEdgeCommunity.Runtime.InteropServices.ComObject.GetType(item);
                }
                catch
                {
                }

                if ((item != null) && (itemType != null))
                {
                    TreeNode itemNode = new TreeNode(String.Format("Item {0} ({1})", i, itemType.FullName));
                    var property = itemType.GetProperty("AttributeSets");

                    if (property != null)
                    {
                        itemNode.ImageIndex = 1;
                    }
                    else
                    {
                        itemNode.ImageIndex = 2;
                    }
                    
                    itemNode.SelectedImageIndex = itemNode.ImageIndex;
                    itemNode.Tag = item;

                    rootNode.Nodes.Add(itemNode);
                }
            }

            if (rootNode.Nodes.Count > 0)
            {
                tvSelectSet.SelectedNode = rootNode.Nodes[0];
            }

            rootNode.ExpandAll();
        }

        private void UpdateListView()
        {
            SolidEdgeFramework.AttributeSets attributeSets = null;
            SolidEdgeFramework.AttributeSet attributeSet = null;
            SolidEdgeFramework.Attribute attribute = null;

            lvAttributes.Items.Clear();
            buttonAddAttribute.Enabled = false;
            buttonRemoveAttribute.Enabled = false;

            if (lvAttributes.Tag != null)
            {
                buttonAddAttribute.Enabled = true;

                dynamic selectSetItem = lvAttributes.Tag;
                attributeSets = selectSetItem.AttributeSets;

                for (int i = 1; i <= attributeSets.Count; i++)
                {
                    attributeSet = attributeSets.Item(i);

                    for (int j = 1; j <= attributeSet.Count; j++)
                    {
                        var lvItem = new ListViewItem();

                        attribute = attributeSet.Item(j);
                        lvItem.Text = attributeSet.SetName;
                        lvItem.SubItems.Add(attribute.Name);
                        lvItem.SubItems.Add(attribute.Value.ToString());
                        lvItem.SubItems.Add(attribute.Value.GetType().FullName);

                        lvAttributes.Items.Add(lvItem);
                    }
                }
            }
        }

        void _documentEvents_BeforeClose()
        {
            ReleaseDocument();
        }

        void _documentEvents_SelectSetChanged(object SelectSet)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<SolidEdgeFramework.SelectSet>(UpdateTreeView), (SolidEdgeFramework.SelectSet)SelectSet);
                return;
            }   
        }

        private void tvSelectSet_AfterSelect(object sender, TreeViewEventArgs e)
        {
            lvAttributes.Tag = e.Node.Tag;
            UpdateListView();
        }

        private void buttonAddAttribute_Click(object sender, EventArgs e)
        {
            SolidEdgeFramework.AttributeSets attributeSets = null;
            SolidEdgeFramework.AttributeSet attributeSet = null;
            SolidEdgeFramework.Attribute attribute = null;

            var dialog = new AddAttributeDialog();
            
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                var setName = dialog.SetName;
                var attributeName = dialog.AttributeName;
                var attributevalue = dialog.AttributeValue;

                dynamic selectSetItem = lvAttributes.Tag;

                attributeSets = selectSetItem.AttributeSets;
                
                try
                {
                    attributeSet = attributeSets.Item(setName);
                }
                catch
                {
                    attributeSet = attributeSets.Add(setName);
                }

                attribute = attributeSet.Add(attributeName, SolidEdgeFramework.AttributeTypeConstants.seStringUnicode);
                attribute.Value = attributevalue;
            }

            UpdateListView();
        }

        private void buttonRemoveAttribute_Click(object sender, EventArgs e)
        {
            SolidEdgeFramework.AttributeSets attributeSets = null;
            SolidEdgeFramework.AttributeSet attributeSet = null;

            List<ListViewItem> itemsToDelete = new List<ListViewItem>();

            foreach (ListViewItem item in lvAttributes.SelectedItems)
            {
                if (item.Selected)
                {
                    itemsToDelete.Add(item);
                }
            }

            dynamic selectSetItem = lvAttributes.Tag;

            foreach (var item in itemsToDelete)
            {
                var setName = item.Text;
                var attributeName = item.SubItems[1].Text;

                attributeSets = selectSetItem.AttributeSets;
                attributeSet = attributeSets.Item(setName);
                attributeSet.Remove(attributeName);

                if (attributeSet.Count == 0)
                {
                    attributeSets.Remove(attributeSet.SetName);
                }

                lvAttributes.Items.Remove(item);
            }
        }

        private void lvAttributes_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.Item != null)
            {
                buttonRemoveAttribute.Enabled = true;
            }
            else
            {
                buttonRemoveAttribute.Enabled = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReleaseDocument()
        {
            if (_documentEvents != null)
            {
                _documentEvents.SelectSetChanged -= _documentEvents_SelectSetChanged;
                _documentEvents.BeforeClose -= _documentEvents_BeforeClose;

                Marshal.FinalReleaseComObject(_documentEvents);
                _documentEvents = null;
            }

            if (_document != null)
            {
                Marshal.FinalReleaseComObject(_document);
                _document = null;
            }
        }
    }
}
