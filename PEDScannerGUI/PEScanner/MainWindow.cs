using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using PEDScannerLib.Core;
using PEDScannerLib.Objects;

namespace PEScanner
{
    public partial class MainWindow : Form
    {
        PortableExecutable portableExecutable;
        PortableExecutable SelectedPortableExecutable;

        public MainWindow()
        {
            InitializeComponent();
            this.portableExecutable = null;
            labelNoImports.Hide();
            labelNoExports.Hide();
        }

        public MainWindow(String filePath)
        {
            InitializeComponent();
            this.UpdateState(filePath);
        }

        public MainWindow(PortableExecutable portableExecutable)
        {
            InitializeComponent();
            this.portableExecutable = portableExecutable;

            if (portableExecutable != null)
            {
                UpdateUI(this.portableExecutable);
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            dataGridViewHeaders.Columns[0].Name = "Property";
            dataGridViewHeaders.Columns[1].Name = "Value";
            dataGridViewHeaders.RowHeadersVisible = false;
            dataGridViewDirectories.RowHeadersVisible = false;
            dataGridViewSections.RowHeadersVisible = false;

        }



        private void PopulateImports(List<ImportFunctionObject> imports)
        {
            treeViewImports.Nodes.Clear();

            if (imports.Count == 0)
            {
                treeViewImports.Hide();
                labelNoImports.Text = "No Imports";
                labelNoImports.Show();
            }
            else
            {
                treeViewImports.Show();
                labelNoImports.Hide();
                foreach (ImportFunctionObject import in imports)
                {
                    string[] row = { import.function, import.baseAddress.ToString() };
                    TreeNode treeNode = treeViewImports.Nodes.Add(import.function);
                    treeNode.Tag = import;
                    treeNode.Nodes.Add(import.dependency);
                }
            }


        }

        private void PopulateExports(List<FunctionObject> exports)
        {
            if (exports.Count == 0)
            {
                listBoxExports.Hide();
                labelNoExports.Text = "No Exports";
                labelNoExports.Show();
            }
            else
            {
                labelNoExports.Hide();
                listBoxExports.Show();

                foreach (FunctionObject export in exports)
                {
                    listBoxExports.Items.Add(export.function);
                }
            }
        }

        private void PopulateHeaders(List<HeaderObject> headers)
        {

            dataGridViewHeaders.Columns[0].Name = "Property";
            dataGridViewHeaders.Columns[1].Name = "Value";
            dataGridViewHeaders.RowHeadersVisible = false;

            dataGridViewHeaders.Rows.Clear();

            foreach (HeaderObject headerObject in headers)
            {
                string[] row = { headerObject.name, headerObject.value.ToString() };
                dataGridViewHeaders.Rows.Add(row);
            }

        }

        private void PopulateSections(List<SectionObject> sections)
        {

            dataGridViewSections.Columns[0].Name = "name";
            dataGridViewSections.Columns[1].Name = "virtualAddress";
            dataGridViewSections.Columns[2].Name = "virtualsize";
            dataGridViewSections.Columns[3].Name = "rawDataOffset";
            dataGridViewSections.Columns[4].Name = "rawDataSize";

            dataGridViewSections.RowHeadersVisible = false;

            dataGridViewSections.Rows.Clear();

            foreach (SectionObject sectionObject in sections)
            {
                string[] row = { sectionObject.name, sectionObject.virtualAddress.ToString(), sectionObject.virtualsize.ToString() , sectionObject.rawDataOffset.ToString()
                ,sectionObject.rawDataSize.ToString()};
                dataGridViewSections.Rows.Add(row);
            }

        }

        private void PopulateDirectories(List<DirectoryObject> directories)
        {

            dataGridViewDirectories.Columns[0].Name = "name";
            dataGridViewDirectories.Columns[1].Name = "RVA";
            dataGridViewDirectories.Columns[2].Name = "Size";

            dataGridViewDirectories.RowHeadersVisible = false;

            dataGridViewDirectories.Rows.Clear();

            foreach (DirectoryObject directoryObject in directories)
            {
                string[] row = { directoryObject.name, directoryObject.RVA.ToString(), directoryObject.Size.ToString() };
                dataGridViewDirectories.Rows.Add(row);
            }

        }


        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.ShowDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath;
                filePath = dlg.FileName;

                this.UpdateState(filePath);

                ToolStripItem item = new ToolStripMenuItem();
                //Name that will apear on the menu
                item.Text = filePath;
                //Put in the Name property whatever neccessery to retrive your data on click event
                /*  item.Name = this.ExtractFileNameFromPath(filePath); */
                item.Name = filePath;
                //On-Click event
                item.Click += new EventHandler(item_Click);
                //Add the submenu to the parent menu
                //  fileToolStripMenuItemRecent.DropDownItems.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            String filePath = item.Name;
            this.UpdateState(filePath);
        }

        void RecursivelyPopulateTheTree(PortableExecutable portableExecutable, TreeNodeCollection tNodes)
        {

            TreeNode treeNode = tNodes.Add(portableExecutable.Name);
            treeNode.Name = portableExecutable.FilePath;
            treeNode.Tag = portableExecutable;
            treeNode.ImageIndex = (portableExecutable.FilePath != null) ? 1 : 0;
            treeNode.SelectedImageIndex = 2;
           // MessageBox.Show(treeNode.ImageIndex + "tree" + treeNode.Name);
            if (portableExecutable.Dependencies.Count == 0)
            {
                return;
            }
            else
            {
                TreeNodeCollection tNodesNextLevel = treeNode.Nodes;
                foreach (object __o in portableExecutable.Dependencies)
                {
                    PortableExecutable pe = (PortableExecutable)__o;
                    // loop body
                    RecursivelyPopulateTheTree(pe, tNodesNextLevel);
                }
            }
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.ShowDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath;
                filePath = dlg.FileName;
                this.UpdateState(filePath);
                ToolStripItem item = new ToolStripMenuItem();
                //Name that will apear on the menu
                item.Text = filePath;
                //Put in the Name property whatever neccessery to retrive your data on click event
                item.Name = filePath;
                //On-Click event
                 item.Click += new EventHandler(item_Click);
                //Add the submenu to the parent menu
                recentToolStripMenuItem.DropDownItems.Add(item);
            }

        }

        private void toolStripButtonClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButtontoolStripButtonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Portable Executable Dependecy Scanner 2018! \n Source is available @ https://github.com/Irindu/PEDScanner", "About PED Scanner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Portable Executable Dependecy Scanner 2018!", "About PED Scanner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonExamine_Click(object sender, EventArgs e)
        {
            if (SelectedPortableExecutable != null)
            {

                if (SelectedPortableExecutable.FilePath == null)
                {
                    MessageBox.Show("The Selected Dependecy " + SelectedPortableExecutable.Name +
                    " cannot be examined because it is missing!");
                }
                else {
                    MainWindow newMainWindowForm = new MainWindow(SelectedPortableExecutable);
                    newMainWindowForm.Closed += (s, args) => { newMainWindowForm.Close(); };
                    newMainWindowForm.ShowDialog();
                }

            }
        }

        protected void treeViewDependencies_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            this.SelectedPortableExecutable = (PortableExecutable)e.Node.Tag;
            if (SelectedPortableExecutable.FilePath != null)
            {
                this.labelDependecyPath.Text = "File Path : " + ((PortableExecutable)e.Node.Tag).FilePath;
            }
            else {
                this.labelDependecyPath.Text = "This Dependecy Named " + SelectedPortableExecutable.Name + " is Missing";
            }

            this.labelDependecyPath.Font = new Font(this.labelDependecyPath.Font, FontStyle.Bold);
        }

        private void treeViewDependencies_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            PortableExecutable portableExecutable = (PortableExecutable)e.Node.Tag;
            Application.Run(new MainWindow(portableExecutable));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.ShowDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath;
                filePath = dlg.FileName;
                this.UpdateState(filePath);

                ToolStripItem item = new ToolStripMenuItem();
                //Name that will apear on the menu
                item.Text = filePath;
                //Put in the Name property whatever neccessery to retrive your data on click event
                item.Name = filePath;
                //On-Click event
                item.Click += new EventHandler(item_Click);
                //Add the submenu to the parent menu
                recentToolStripMenuItem.DropDownItems.Add(item);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void treeViewImports_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ImportFunctionObject importFunctionObject = (ImportFunctionObject)e.Node.Tag;

            if (importFunctionObject != null)
            {
                dataGridViewImportExamined.Rows.Clear();
                dataGridViewImportExamined.Show();
                string[] row = { importFunctionObject.function, importFunctionObject.baseAddress.ToString(), importFunctionObject.dependency };
                dataGridViewImportExamined.Rows.Add(row);

            }

        }

        //update the state of the UI window
        void UpdateState(String filePath)
        {
            try
            {
                this.portableExecutable = new PortableExecutable(this.ExtractFileNameFromPath(filePath), filePath , true);
                this.UpdateUI(this.portableExecutable);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
           //     Application.Exit();
            }

        }

        void UpdateUI(PortableExecutable portableExecutable)
        {
            dataGridViewImportExamined.Hide();
            treeViewDependencies.Nodes.Clear();
            treeViewImports.Nodes.Clear();
            listBoxExports.Items.Clear();
            labelDependecyPath.Text = "";

            TreeNodeCollection tNodes = treeViewDependencies.Nodes;

            if (portableExecutable != null)
            {
                RecursivelyPopulateTheTree(portableExecutable, tNodes);
                PopulateHeaders(portableExecutable.Headers);
                PopulateImports(portableExecutable.ImportFunctions);
                PopulateExports(portableExecutable.ExportedFunctions);
                PopulateSections(portableExecutable.GetSections());
                PopulateDirectories(portableExecutable.GetDirectories());
            }

            //  pe.MakeImports();
            //pe.MakeExports();

            //listBox_Imports.Items.Clear();
            //foreach (object __o in pe.GetImports())
            //{
            //    String import = (String)__o;
            //    // loop body
            //    listBox_Imports.Items.Add(import);
            //}

            //listBox_Exports.Items.Clear();
            //foreach (object __o in pe.GetExports())
            //{
            //    String import = (String)__o;
            //    // loop body
            //    listBox_Exports.Items.Add(import);
            //}
        }

        private void treeViewDependencies_MouseHover(object sender, EventArgs e)
        {
            // Get the node at the current mouse pointer location.
            TreeNode theNode = this.treeViewDependencies.GetNodeAt(treeViewDependencies.PointToClient(Cursor.Position));

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if ((theNode != null))
            {
                // Verify that the tag property is not "null".
                if (theNode.Tag != null)
                {
                    PortableExecutable portableExecutable = (PortableExecutable)theNode.Tag;
                    this.toolTipDependencies.SetToolTip(this.treeViewDependencies, portableExecutable.FilePath);
                }
                else
                {
                    this.toolTipDependencies.SetToolTip(this.treeViewDependencies, "");
                }
            }
            else     // Pointer is not over a node so clear the ToolTip.
            {
                this.toolTipDependencies.SetToolTip(this.treeViewDependencies, "");
            }
        }

        //utility function to extract file name given the file path
        private String ExtractFileNameFromPath(String FilePath)
        {
            String[] arrayofNames = FilePath.Split('\\');
            return arrayofNames[arrayofNames.Length - 1];
        }
    }
}
