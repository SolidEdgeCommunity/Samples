using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SolidEdge.OpenSave
{
    class ListViewEx : ListView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private bool _allowDeleteKey;

        public ListViewEx()
            : base()
        {
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            DoubleBuffered = true;

            if (!this.DesignMode && Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 6)
            {
                SetWindowTheme(this.Handle, "explorer", null);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.A && e.Control)
            {
                SelectAllItems();
            }
            else if (e.KeyCode == Keys.C && e.Control)
            {
                CopySelectedItemsToClipboard();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                SelectedItems.Clear();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (AllowDeleteKey)
                {
                    DeleteSelectedItems();
                }
            }
        }

        public void AutoResizeColumns()
        {
            foreach (ColumnHeader header in this.Columns)
            {
                header.Width = -2;
            }
        }

        public void DeleteSelectedItems()
        {
            ListViewItem[] items = SelectedItems.OfType<ListViewItem>().ToArray();
            foreach (ListViewItem item in items)
            {
                item.Remove();
            }
        }

        public void CopySelectedItemsToClipboard()
        {
            StringBuilder clipboardText = new StringBuilder();

            foreach (ListViewItem item in SelectedItems)
            {
                StringBuilder line = new StringBuilder();

                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    line.AppendFormat("{0}\t", subItem.Text);
                }

                clipboardText.AppendLine(line.ToString());
            }

            Clipboard.Clear();
            Clipboard.SetText(clipboardText.ToString(), TextDataFormat.UnicodeText);
        }

        public void SelectAllItems()
        {
            if (MultiSelect == true)
            {
                BeginUpdate();
                foreach (ListViewItem item in Items)
                {
                    item.Selected = true;
                }
                EndUpdate();
            }
        }

        public bool AllowDeleteKey
        {
            get { return _allowDeleteKey; }
            set { _allowDeleteKey = value; }
        }
    }
}
