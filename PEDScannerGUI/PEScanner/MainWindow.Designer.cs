namespace PEScanner
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDependencies = new System.Windows.Forms.TabPage();
            this.treeViewDependencies = new System.Windows.Forms.TreeView();
            this.tabPageImports = new System.Windows.Forms.TabPage();
            this.tabPageExport = new System.Windows.Forms.TabPage();
            this.tabPageHeaders = new System.Windows.Forms.TabPage();
            this.dataGridViewHeaders = new System.Windows.Forms.DataGridView();
            this.ColumnProperties = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageSections = new System.Windows.Forms.TabPage();
            this.dataGridViewSections = new System.Windows.Forms.DataGridView();
            this.tabPageDirectories = new System.Windows.Forms.TabPage();
            this.dataGridViewDirectories = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnRVA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAddFile = new System.Windows.Forms.Button();
            this.buttonExamine = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripQuickAccess = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.virtualsize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rawDataOffset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rawDataSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.treeViewImports = new System.Windows.Forms.TreeView();
            this.listViewExports = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.tabPageDependencies.SuspendLayout();
            this.tabPageImports.SuspendLayout();
            this.tabPageExport.SuspendLayout();
            this.tabPageHeaders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHeaders)).BeginInit();
            this.tabPageSections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSections)).BeginInit();
            this.tabPageDirectories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDirectories)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.toolStripQuickAccess.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDependencies);
            this.tabControl1.Controls.Add(this.tabPageImports);
            this.tabControl1.Controls.Add(this.tabPageExport);
            this.tabControl1.Controls.Add(this.tabPageHeaders);
            this.tabControl1.Controls.Add(this.tabPageSections);
            this.tabControl1.Controls.Add(this.tabPageDirectories);
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl1.ItemSize = new System.Drawing.Size(81, 21);
            this.tabControl1.Location = new System.Drawing.Point(12, 65);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeftLayout = true;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(760, 336);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageDependencies
            // 
            this.tabPageDependencies.Controls.Add(this.treeViewDependencies);
            this.tabPageDependencies.Location = new System.Drawing.Point(4, 25);
            this.tabPageDependencies.Name = "tabPageDependencies";
            this.tabPageDependencies.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDependencies.Size = new System.Drawing.Size(752, 307);
            this.tabPageDependencies.TabIndex = 0;
            this.tabPageDependencies.Text = "Dependencies";
            this.tabPageDependencies.UseVisualStyleBackColor = true;
            // 
            // treeViewDependencies
            // 
            this.treeViewDependencies.Location = new System.Drawing.Point(22, 27);
            this.treeViewDependencies.Name = "treeViewDependencies";
            this.treeViewDependencies.Size = new System.Drawing.Size(589, 276);
            this.treeViewDependencies.TabIndex = 0;
            this.treeViewDependencies.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewDependencies_AfterSelect);
            // 
            // tabPageImports
            // 
            this.tabPageImports.Controls.Add(this.treeViewImports);
            this.tabPageImports.Location = new System.Drawing.Point(4, 25);
            this.tabPageImports.Name = "tabPageImports";
            this.tabPageImports.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImports.Size = new System.Drawing.Size(752, 307);
            this.tabPageImports.TabIndex = 1;
            this.tabPageImports.Text = "Imports ";
            this.tabPageImports.UseVisualStyleBackColor = true;
            // 
            // tabPageExport
            // 
            this.tabPageExport.Controls.Add(this.listViewExports);
            this.tabPageExport.Location = new System.Drawing.Point(4, 25);
            this.tabPageExport.Name = "tabPageExport";
            this.tabPageExport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExport.Size = new System.Drawing.Size(752, 307);
            this.tabPageExport.TabIndex = 2;
            this.tabPageExport.Text = "Exports ";
            this.tabPageExport.UseVisualStyleBackColor = true;
            // 
            // tabPageHeaders
            // 
            this.tabPageHeaders.Controls.Add(this.dataGridViewHeaders);
            this.tabPageHeaders.Location = new System.Drawing.Point(4, 25);
            this.tabPageHeaders.Name = "tabPageHeaders";
            this.tabPageHeaders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHeaders.Size = new System.Drawing.Size(752, 307);
            this.tabPageHeaders.TabIndex = 3;
            this.tabPageHeaders.Text = "Headers";
            this.tabPageHeaders.UseVisualStyleBackColor = true;
            // 
            // dataGridViewHeaders
            // 
            this.dataGridViewHeaders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewHeaders.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewHeaders.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewHeaders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHeaders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnProperties,
            this.ColumnValues});
            this.dataGridViewHeaders.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewHeaders.Name = "dataGridViewHeaders";
            this.dataGridViewHeaders.Size = new System.Drawing.Size(596, 310);
            this.dataGridViewHeaders.TabIndex = 0;
            // 
            // ColumnProperties
            // 
            this.ColumnProperties.HeaderText = "Properties";
            this.ColumnProperties.MinimumWidth = 50;
            this.ColumnProperties.Name = "ColumnProperties";
            // 
            // ColumnValues
            // 
            this.ColumnValues.HeaderText = "Values";
            this.ColumnValues.MinimumWidth = 100;
            this.ColumnValues.Name = "ColumnValues";
            // 
            // tabPageSections
            // 
            this.tabPageSections.Controls.Add(this.dataGridViewSections);
            this.tabPageSections.Location = new System.Drawing.Point(4, 25);
            this.tabPageSections.Name = "tabPageSections";
            this.tabPageSections.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSections.Size = new System.Drawing.Size(752, 307);
            this.tabPageSections.TabIndex = 4;
            this.tabPageSections.Text = "Sections";
            this.tabPageSections.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSections
            // 
            this.dataGridViewSections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSections.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewSections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.virtualAddress,
            this.virtualsize,
            this.rawDataOffset,
            this.rawDataSize});
            this.dataGridViewSections.Location = new System.Drawing.Point(22, 33);
            this.dataGridViewSections.Name = "dataGridViewSections";
            this.dataGridViewSections.Size = new System.Drawing.Size(584, 259);
            this.dataGridViewSections.TabIndex = 0;
            // 
            // tabPageDirectories
            // 
            this.tabPageDirectories.Controls.Add(this.dataGridViewDirectories);
            this.tabPageDirectories.Location = new System.Drawing.Point(4, 25);
            this.tabPageDirectories.Name = "tabPageDirectories";
            this.tabPageDirectories.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDirectories.Size = new System.Drawing.Size(752, 307);
            this.tabPageDirectories.TabIndex = 5;
            this.tabPageDirectories.Text = "Directories";
            this.tabPageDirectories.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDirectories
            // 
            this.dataGridViewDirectories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDirectories.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewDirectories.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridViewDirectories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDirectories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnRVA,
            this.ColumnSize});
            this.dataGridViewDirectories.Location = new System.Drawing.Point(16, 38);
            this.dataGridViewDirectories.Name = "dataGridViewDirectories";
            this.dataGridViewDirectories.Size = new System.Drawing.Size(550, 273);
            this.dataGridViewDirectories.TabIndex = 0;
            // 
            // ColumnName
            // 
            this.ColumnName.HeaderText = "Name";
            this.ColumnName.Name = "ColumnName";
            // 
            // ColumnRVA
            // 
            this.ColumnRVA.HeaderText = "RVA";
            this.ColumnRVA.Name = "ColumnRVA";
            // 
            // ColumnSize
            // 
            this.ColumnSize.HeaderText = "ColumnSize";
            this.ColumnSize.Name = "ColumnSize";
            // 
            // buttonAddFile
            // 
            this.buttonAddFile.Location = new System.Drawing.Point(628, 410);
            this.buttonAddFile.Name = "buttonAddFile";
            this.buttonAddFile.Size = new System.Drawing.Size(139, 31);
            this.buttonAddFile.TabIndex = 1;
            this.buttonAddFile.Text = "AddFIle";
            this.buttonAddFile.UseVisualStyleBackColor = true;
            this.buttonAddFile.Click += new System.EventHandler(this.buttonAddFile_Click);
            // 
            // buttonExamine
            // 
            this.buttonExamine.Location = new System.Drawing.Point(466, 410);
            this.buttonExamine.Name = "buttonExamine";
            this.buttonExamine.Size = new System.Drawing.Size(139, 31);
            this.buttonExamine.TabIndex = 2;
            this.buttonExamine.Text = "Examine";
            this.buttonExamine.UseVisualStyleBackColor = true;
            this.buttonExamine.Click += new System.EventHandler(this.buttonExamine_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.recentToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // helpToolStripMenuItemHelp
            // 
            this.helpToolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItemHelp.Name = "helpToolStripMenuItemHelp";
            this.helpToolStripMenuItemHelp.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItemHelp.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStripQuickAccess
            // 
            this.toolStripQuickAccess.AutoSize = false;
            this.toolStripQuickAccess.CanOverflow = false;
            this.toolStripQuickAccess.GripMargin = new System.Windows.Forms.Padding(4);
            this.toolStripQuickAccess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonClose,
            this.toolStripButtonAbout});
            this.toolStripQuickAccess.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStripQuickAccess.Location = new System.Drawing.Point(0, 24);
            this.toolStripQuickAccess.MaximumSize = new System.Drawing.Size(500, 45);
            this.toolStripQuickAccess.MinimumSize = new System.Drawing.Size(100, 40);
            this.toolStripQuickAccess.Name = "toolStripQuickAccess";
            this.toolStripQuickAccess.Size = new System.Drawing.Size(500, 40);
            this.toolStripQuickAccess.Stretch = true;
            this.toolStripQuickAccess.TabIndex = 4;
            this.toolStripQuickAccess.Text = "Quick Access";
            // 
            // toolStripButtonOpen
            // 
            this.toolStripButtonOpen.AutoSize = false;
            this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
            this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpen.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButtonOpen.Name = "toolStripButtonOpen";
            this.toolStripButtonOpen.Size = new System.Drawing.Size(40, 35);
            this.toolStripButtonOpen.Text = "Open";
            this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.AutoSize = false;
            this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(40, 35);
            this.toolStripButtonClose.Text = "Close File";
            this.toolStripButtonClose.ToolTipText = "Close File";
            this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
            // 
            // toolStripButtonAbout
            // 
            this.toolStripButtonAbout.AutoSize = false;
            this.toolStripButtonAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAbout.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAbout.Image")));
            this.toolStripButtonAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAbout.Name = "toolStripButtonAbout";
            this.toolStripButtonAbout.Size = new System.Drawing.Size(40, 35);
            this.toolStripButtonAbout.Text = "About";
            this.toolStripButtonAbout.Click += new System.EventHandler(this.toolStripButtontoolStripButtonAbout_Click);
            // 
            // name
            // 
            this.name.HeaderText = "name";
            this.name.Name = "name";
            // 
            // virtualAddress
            // 
            this.virtualAddress.HeaderText = "virtualAddress";
            this.virtualAddress.Name = "virtualAddress";
            // 
            // virtualsize
            // 
            this.virtualsize.HeaderText = "virtualsize";
            this.virtualsize.Name = "virtualsize";
            // 
            // rawDataOffset
            // 
            this.rawDataOffset.HeaderText = "rawDataOffset";
            this.rawDataOffset.Name = "rawDataOffset";
            // 
            // rawDataSize
            // 
            this.rawDataSize.HeaderText = "rawDataSize";
            this.rawDataSize.Name = "rawDataSize";
            // 
            // treeViewImports
            // 
            this.treeViewImports.Location = new System.Drawing.Point(18, 20);
            this.treeViewImports.Name = "treeViewImports";
            this.treeViewImports.Size = new System.Drawing.Size(437, 269);
            this.treeViewImports.TabIndex = 0;
            // 
            // listViewExports
            // 
            this.listViewExports.Location = new System.Drawing.Point(46, 30);
            this.listViewExports.Name = "listViewExports";
            this.listViewExports.Size = new System.Drawing.Size(404, 252);
            this.listViewExports.TabIndex = 0;
            this.listViewExports.UseCompatibleStateImageBehavior = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripQuickAccess);
            this.Controls.Add(this.buttonExamine);
            this.Controls.Add(this.buttonAddFile);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "PEDScanner";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageDependencies.ResumeLayout(false);
            this.tabPageImports.ResumeLayout(false);
            this.tabPageExport.ResumeLayout(false);
            this.tabPageHeaders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHeaders)).EndInit();
            this.tabPageSections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSections)).EndInit();
            this.tabPageDirectories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDirectories)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStripQuickAccess.ResumeLayout(false);
            this.toolStripQuickAccess.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDependencies;
        private System.Windows.Forms.TabPage tabPageImports;
        private System.Windows.Forms.TabPage tabPageExport;
        private System.Windows.Forms.TabPage tabPageHeaders;
        private System.Windows.Forms.DataGridView dataGridViewHeaders;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProperties;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValues;
        private System.Windows.Forms.TabPage tabPageSections;
        private System.Windows.Forms.TabPage tabPageDirectories;
        private System.Windows.Forms.DataGridView dataGridViewDirectories;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnRVA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSize;
        private System.Windows.Forms.TreeView treeViewDependencies;
        private System.Windows.Forms.DataGridView dataGridViewSections;
        private System.Windows.Forms.Button buttonAddFile;
        private System.Windows.Forms.Button buttonExamine;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripQuickAccess;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
        private System.Windows.Forms.ToolStripButton toolStripButtonClose;
        private System.Windows.Forms.ToolStripButton toolStripButtonAbout;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn virtualsize;
        private System.Windows.Forms.DataGridViewTextBoxColumn rawDataOffset;
        private System.Windows.Forms.DataGridViewTextBoxColumn rawDataSize;
        private System.Windows.Forms.TreeView treeViewImports;
        private System.Windows.Forms.ListView listViewExports;
    }
}

