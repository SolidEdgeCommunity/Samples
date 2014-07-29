using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FileProperties.WinForm
{
    class ListViewEx : ListView
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        public ListViewEx()
            : base()
        {
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
    }
}
