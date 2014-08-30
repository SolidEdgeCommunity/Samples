using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SolidEdge.OpenSave
{
    public class TextBoxAppender : AppenderSkeleton
    {
        private TextBox _textBox;

        public TextBoxAppender()
        {
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (_textBox != null)
            {
                _textBox.Do(t =>
                {
                    t.AppendText(String.Format("{0}{1}", loggingEvent.RenderedMessage, Environment.NewLine));
                });
            }
        }

        public TextBox TextBox
        {
            get { return _textBox; }
            set { _textBox = value; }
        }
    } 
}
