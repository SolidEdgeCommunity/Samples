using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AttributeSetEditor
{
    public partial class AddAttributeDialog : Form
    {
        public AddAttributeDialog()
        {
            InitializeComponent();
        }

        private void textBoxSetName_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void textBoxAttributeName_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void textBoxAttributeValue_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            if ((textBoxSetName.Text.Length == 0) && (textBoxAttributeName.Text.Length == 0) && (textBoxAttributeValue.Text.Length == 0))
            {
                buttonOK.Enabled = false;
            }
            else
            {
                buttonOK.Enabled = true;
            }
        }

        public string SetName
        {
            get
            {
                return textBoxSetName.Text;
            }
        }

        public string AttributeName
        {
            get
            {
                return textBoxAttributeName.Text;
            }
        }

        public string AttributeValue
        {
            get
            {
                return textBoxAttributeValue.Text;
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
    }
}
