using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolidEdge.OpenSave
{
    static class ControlExtensions
    {
        public static void Do<TControl>(this TControl control, Action<TControl> action)
          where TControl : Control
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action, control);
            }
            else
            {
                action(control);
            }
        }
    }
}
