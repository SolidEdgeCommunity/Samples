namespace ApiSamples
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllDocumentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllDocumentssilentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.codePlexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.samplesForSolidEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.interopForSolidEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spyForSolidEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.solidEdgeST6SDKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.buttonRun = new System.Windows.Forms.ToolStripButton();
            this.buttonBreakpoint = new System.Windows.Forms.ToolStripButton();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainerOuter = new System.Windows.Forms.SplitContainer();
            this.splitContainerInner = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainerListView = new System.Windows.Forms.SplitContainer();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.listView = new ApiSamples.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.sampleRichTextBox = new ApiSamples.Forms.RichTextBoxEx();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).BeginInit();
            this.splitContainerOuter.Panel1.SuspendLayout();
            this.splitContainerOuter.Panel2.SuspendLayout();
            this.splitContainerOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInner)).BeginInit();
            this.splitContainerInner.Panel1.SuspendLayout();
            this.splitContainerInner.Panel2.SuspendLayout();
            this.splitContainerInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerListView)).BeginInit();
            this.splitContainerListView.Panel1.SuspendLayout();
            this.splitContainerListView.Panel2.SuspendLayout();
            this.splitContainerListView.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeAllDocumentsToolStripMenuItem,
            this.closeAllDocumentssilentToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // closeAllDocumentsToolStripMenuItem
            // 
            this.closeAllDocumentsToolStripMenuItem.Name = "closeAllDocumentsToolStripMenuItem";
            this.closeAllDocumentsToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.closeAllDocumentsToolStripMenuItem.Text = "Close all documents";
            this.closeAllDocumentsToolStripMenuItem.Click += new System.EventHandler(this.closeAllDocumentsToolStripMenuItem_Click);
            // 
            // closeAllDocumentssilentToolStripMenuItem
            // 
            this.closeAllDocumentssilentToolStripMenuItem.Name = "closeAllDocumentssilentToolStripMenuItem";
            this.closeAllDocumentssilentToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.closeAllDocumentssilentToolStripMenuItem.Text = "Close all documents (silent)";
            this.closeAllDocumentssilentToolStripMenuItem.Click += new System.EventHandler(this.closeAllDocumentssilentToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(217, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.codePlexToolStripMenuItem,
            this.toolStripMenuItem2,
            this.solidEdgeST6SDKToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // codePlexToolStripMenuItem
            // 
            this.codePlexToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.samplesForSolidEdgeToolStripMenuItem,
            this.interopForSolidEdgeToolStripMenuItem,
            this.spyForSolidEdgeToolStripMenuItem});
            this.codePlexToolStripMenuItem.Name = "codePlexToolStripMenuItem";
            this.codePlexToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.codePlexToolStripMenuItem.Text = "&CodePlex";
            // 
            // samplesForSolidEdgeToolStripMenuItem
            // 
            this.samplesForSolidEdgeToolStripMenuItem.Name = "samplesForSolidEdgeToolStripMenuItem";
            this.samplesForSolidEdgeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.samplesForSolidEdgeToolStripMenuItem.Text = "Samples for Solid Edge";
            this.samplesForSolidEdgeToolStripMenuItem.Click += new System.EventHandler(this.samplesForSolidEdgeToolStripMenuItem_Click);
            // 
            // interopForSolidEdgeToolStripMenuItem
            // 
            this.interopForSolidEdgeToolStripMenuItem.Name = "interopForSolidEdgeToolStripMenuItem";
            this.interopForSolidEdgeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.interopForSolidEdgeToolStripMenuItem.Text = "Interop for Solid Edge";
            this.interopForSolidEdgeToolStripMenuItem.Click += new System.EventHandler(this.interopForSolidEdgeToolStripMenuItem_Click);
            // 
            // spyForSolidEdgeToolStripMenuItem
            // 
            this.spyForSolidEdgeToolStripMenuItem.Name = "spyForSolidEdgeToolStripMenuItem";
            this.spyForSolidEdgeToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
            this.spyForSolidEdgeToolStripMenuItem.Text = "Spy for Solid Edge";
            this.spyForSolidEdgeToolStripMenuItem.Click += new System.EventHandler(this.spyForSolidEdgeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(172, 6);
            // 
            // solidEdgeST6SDKToolStripMenuItem
            // 
            this.solidEdgeST6SDKToolStripMenuItem.Name = "solidEdgeST6SDKToolStripMenuItem";
            this.solidEdgeST6SDKToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.solidEdgeST6SDKToolStripMenuItem.Text = "&Solid Edge ST6 SDK";
            this.solidEdgeST6SDKToolStripMenuItem.Click += new System.EventHandler(this.solidEdgeST6SDKToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(784, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonRun,
            this.buttonBreakpoint});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 25);
            this.toolStrip.TabIndex = 2;
            // 
            // buttonRun
            // 
            this.buttonRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonRun.Image = global::ApiSamples.Resources.Run_16x16;
            this.buttonRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(23, 22);
            this.buttonRun.Text = "Run";
            this.buttonRun.ToolTipText = "Run the selected sample.";
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonBreakpoint
            // 
            this.buttonBreakpoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonBreakpoint.Image = global::ApiSamples.Resources.Breakpoint_16x16;
            this.buttonBreakpoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonBreakpoint.Name = "buttonBreakpoint";
            this.buttonBreakpoint.Size = new System.Drawing.Size(23, 22);
            this.buttonBreakpoint.Text = "Breakpoint";
            this.buttonBreakpoint.ToolTipText = "Break at start of sample.";
            this.buttonBreakpoint.Click += new System.EventHandler(this.buttonBreakpoint_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "FolderClosed_16x16.png");
            this.imageList.Images.SetKeyName(1, "FolderOpen_16x16.png");
            this.imageList.Images.SetKeyName(2, "Method_16x16.png");
            // 
            // splitContainerOuter
            // 
            this.splitContainerOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOuter.Location = new System.Drawing.Point(0, 49);
            this.splitContainerOuter.Name = "splitContainerOuter";
            this.splitContainerOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerOuter.Panel1
            // 
            this.splitContainerOuter.Panel1.Controls.Add(this.splitContainerInner);
            // 
            // splitContainerOuter.Panel2
            // 
            this.splitContainerOuter.Panel2.Controls.Add(this.outputTextBox);
            this.splitContainerOuter.Size = new System.Drawing.Size(784, 490);
            this.splitContainerOuter.SplitterDistance = 366;
            this.splitContainerOuter.TabIndex = 4;
            // 
            // splitContainerInner
            // 
            this.splitContainerInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerInner.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerInner.Location = new System.Drawing.Point(0, 0);
            this.splitContainerInner.Name = "splitContainerInner";
            // 
            // splitContainerInner.Panel1
            // 
            this.splitContainerInner.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainerInner.Panel2
            // 
            this.splitContainerInner.Panel2.Controls.Add(this.splitContainerListView);
            this.splitContainerInner.Size = new System.Drawing.Size(784, 366);
            this.splitContainerInner.SplitterDistance = 175;
            this.splitContainerInner.TabIndex = 4;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(175, 366);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // splitContainerListView
            // 
            this.splitContainerListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerListView.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerListView.Location = new System.Drawing.Point(0, 0);
            this.splitContainerListView.Name = "splitContainerListView";
            this.splitContainerListView.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerListView.Panel1
            // 
            this.splitContainerListView.Panel1.Controls.Add(this.listView);
            // 
            // splitContainerListView.Panel2
            // 
            this.splitContainerListView.Panel2.Controls.Add(this.sampleRichTextBox);
            this.splitContainerListView.Size = new System.Drawing.Size(605, 366);
            this.splitContainerListView.SplitterDistance = 267;
            this.splitContainerListView.TabIndex = 4;
            // 
            // outputTextBox
            // 
            this.outputTextBox.AcceptsReturn = true;
            this.outputTextBox.AcceptsTab = true;
            this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextBox.Location = new System.Drawing.Point(0, 0);
            this.outputTextBox.MaxLength = 0;
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputTextBox.Size = new System.Drawing.Size(784, 120);
            this.outputTextBox.TabIndex = 2;
            this.outputTextBox.WordWrap = false;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(605, 267);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sample";
            this.columnHeader1.Width = 207;
            // 
            // sampleRichTextBox
            // 
            this.sampleRichTextBox.BackColor = System.Drawing.Color.White;
            this.sampleRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sampleRichTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.sampleRichTextBox.Location = new System.Drawing.Point(0, 0);
            this.sampleRichTextBox.Name = "sampleRichTextBox";
            this.sampleRichTextBox.ReadOnly = true;
            this.sampleRichTextBox.Size = new System.Drawing.Size(605, 95);
            this.sampleRichTextBox.TabIndex = 0;
            this.sampleRichTextBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainerOuter);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Solid Edge API Samples";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainerOuter.Panel1.ResumeLayout(false);
            this.splitContainerOuter.Panel2.ResumeLayout(false);
            this.splitContainerOuter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOuter)).EndInit();
            this.splitContainerOuter.ResumeLayout(false);
            this.splitContainerInner.Panel1.ResumeLayout(false);
            this.splitContainerInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInner)).EndInit();
            this.splitContainerInner.ResumeLayout(false);
            this.splitContainerListView.Panel1.ResumeLayout(false);
            this.splitContainerListView.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerListView)).EndInit();
            this.splitContainerListView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private ListViewEx listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStripButton buttonRun;
        private System.Windows.Forms.SplitContainer splitContainerOuter;
        private System.Windows.Forms.SplitContainer splitContainerInner;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.ToolStripButton buttonBreakpoint;
        private System.Windows.Forms.SplitContainer splitContainerListView;
        private Forms.RichTextBoxEx sampleRichTextBox;
        private System.Windows.Forms.ToolStripMenuItem closeAllDocumentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllDocumentssilentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solidEdgeST6SDKToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem codePlexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem samplesForSolidEdgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem interopForSolidEdgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem spyForSolidEdgeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

