using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApiDemos
{
    static class ControlExtensions
    {
        public static void Do<TControl>(this TControl control, Action<TControl> action)
          where TControl : Control
        {
            if (control.InvokeRequired)
            {
                //control.Invoke(action, control);
                control.BeginInvoke(action, control);
            }
            else
            {
                action(control);
            }
        }
    }
}
