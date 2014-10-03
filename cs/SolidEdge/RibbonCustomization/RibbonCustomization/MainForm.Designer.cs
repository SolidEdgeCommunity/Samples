namespace RibbonCustomization
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCreateTheme = new System.Windows.Forms.Button();
            this.buttonDeleteTheme = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCreateTheme
            // 
            this.buttonCreateTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateTheme.Location = new System.Drawing.Point(77, 46);
            this.buttonCreateTheme.Name = "buttonCreateTheme";
            this.buttonCreateTheme.Size = new System.Drawing.Size(97, 23);
            this.buttonCreateTheme.TabIndex = 2;
            this.buttonCreateTheme.Text = "CreateTheme";
            this.buttonCreateTheme.UseVisualStyleBackColor = true;
            this.buttonCreateTheme.Click += new System.EventHandler(this.buttonCreateTheme_Click);
            // 
            // buttonDeleteTheme
            // 
            this.buttonDeleteTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteTheme.Location = new System.Drawing.Point(77, 75);
            this.buttonDeleteTheme.Name = "buttonDeleteTheme";
            this.buttonDeleteTheme.Size = new System.Drawing.Size(97, 23);
            this.buttonDeleteTheme.TabIndex = 3;
            this.buttonDeleteTheme.Text = "DeleteTheme";
            this.buttonDeleteTheme.UseVisualStyleBackColor = true;
            this.buttonDeleteTheme.Click += new System.EventHandler(this.buttonDeleteTheme_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 148);
            this.Controls.Add(this.buttonDeleteTheme);
            this.Controls.Add(this.buttonCreateTheme);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.Text = "Solid Edge Ribbon Customization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCreateTheme;
        private System.Windows.Forms.Button buttonDeleteTheme;
    }
}

