using SolidEdgeContrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AddInDemo
{
    public partial class MyEdgeBarControl : SolidEdgeContrib.AddIn.EdgeBarControl
    {
        public MyEdgeBarControl()
        {
            InitializeComponent();
        }

        private void MyEdgeBarControl_Load(object sender, EventArgs e)
        {
            // You can set the tooltip in the designer or at runtime.
            this.ToolTip = "My EdgeBar Control";

            // Trick to use the default system font.
            this.Font = SystemFonts.MessageBoxFont;
        }

        private void MyEdgeBarControl_AfterInitialize(object sender, EventArgs e)
        {
            // These properties are not initialized until AfterInitialize is called.
            var edgeBarPage = this.EdgeBarPage;
            var document = this.Document;

            var application = document.Application;
            this.richTextBox1.Text = application.GetGlobalParameter<string>(SolidEdgeFramework.ApplicationGlobalConstants.seApplicationGlobalSystemInfo);
        }
    }
}
