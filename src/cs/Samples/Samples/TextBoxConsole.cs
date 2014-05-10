using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ApiSamples
{
    class TextBoxConsole : TextWriter
    {
        private TextBox textBox;
        private StringBuilder _buffer = new StringBuilder();

        public TextBoxConsole(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public override void WriteLine()
        {
            try
            {
                textBox.Do(ctl =>
                {
                    ctl.AppendText(NewLine);
                });
            }
            catch
            {
            }
        }

        public override void WriteLine(string value)
        {
            try
            {
                textBox.Do(ctl =>
                {
                    ctl.AppendText(value);
                    ctl.AppendText(NewLine);
                });
            }
            catch
            {
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

}
