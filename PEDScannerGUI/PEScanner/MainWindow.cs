using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PEDScannerLib.Core;


namespace PEScanner
{
    public partial class MainWindow : Form
    {
        String fileName;
        PortableExecutable portableExecutable;
        PortableExecutable SelectedPortableExecutable;

        public MainWindow()
        {
            InitializeComponent();
            this.fileName = null;
            this.portableExecutable = null;
        }

        public MainWindow(String fileName)
        {
            InitializeComponent();
            this.fileName = fileName;
            this.portableExecutable = new PortableExecutable(fileName, fileName);
        }

        public MainWindow(PortableExecutable portableExecutable)
        {
            InitializeComponent();
            this.fileName = portableExecutable.FilePath;
            this.portableExecutable = portableExecutable;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            dataGridViewHeaders.Columns[0].Name = "Property";
            dataGridViewHeaders.Columns[1].Name = "Value";
            dataGridViewHeaders.RowHeadersVisible = false;

        }

       

        private void PopulateImports(List<ImportFunctionObject> imports)
        {
            foreach (ImportFunctionObject import in imports)
            {
                string[] row = { import.function, import.baseAddress.ToString() };
               TreeNode treeNode = treeViewImports.Nodes.Add(import.function);
                treeNode.Nodes.Add(import.dependency);
            }

          }

        private void PopulateExports(List<FunctionObject> exports)
        {
            foreach (FunctionObject export in exports)
            {
                listViewExports.Items.Add(export.function);
                MessageBox.Show(export.function);

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
                string[] row = { directoryObject.name, directoryObject.RVA.ToString(), directoryObject.Size.ToString()};
                dataGridViewDirectories.Rows.Add(row);
            }

             }


        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.ShowDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName;
               // CurrentPE = fileName;
                UpdateUI(fileName);

                ToolStripItem item = new ToolStripMenuItem();
                //Name that will apear on the menu
                item.Text = fileName;
                //Put in the Name property whatever neccessery to retrive your data on click event
                item.Name = fileName;
                //On-Click event
                item.Click += new EventHandler(item_Click);
                //Add the submenu to the parent menu
              //  fileToolStripMenuItemRecent.DropDownItems.Add(item);
            }
        }

        void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            String fileName = item.Text;
          //  CurrentPE = fileName;
            UpdateUI(fileName);
        }

        void UpdateUI(String fileName)
        {
            treeViewDependencies.Nodes.Clear();
            PortableExecutable pe = new PortableExecutable(fileName, fileName);
            pe.MakeDependencies();
            TreeNodeCollection tNodes = treeViewDependencies.Nodes;

            RecursivelyPopulateTheTree(pe, tNodes);
            PopulateHeaders(pe.Headers);
            PopulateImports(pe.ImportFunctions);
            PopulateSections(pe.GetSections());
            PopulateDirectories(pe.GetDirectories());
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

        void RecursivelyPopulateTheTree(PortableExecutable portableExecutable, TreeNodeCollection tNodes)
        {

            TreeNode treeNode = tNodes.Add(portableExecutable.FilePath);
             treeNode.Tag = portableExecutable;

            if (portableExecutable.Dependencies.Count == 0)
            {
                return;
            }
            else
            {
                TreeNodeCollection tNodesNextLevel = tNodes[0].Nodes;
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
                string fileName;
                fileName = dlg.FileName;
                // CurrentPE = fileName;
                UpdateUI(fileName);

                ToolStripItem item = new ToolStripMenuItem();
                //Name that will apear on the menu
                item.Text = fileName;
                //Put in the Name property whatever neccessery to retrive your data on click event
                item.Name = fileName;
                //On-Click event
                // item.Click += new EventHandler(item_Click);
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
            MessageBox.Show("Portable Executable Scanner 2018!");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Portable Executable Scanner 2018!");
        }

        private void buttonExamine_Click(object sender, EventArgs e)
        {
            if (SelectedPortableExecutable != null)
            {
                Application.Run(new MainWindow(portableExecutable));
            }
        }

        protected void treeViewDependencies_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            this.fileName = e.Node.Text;
            this.SelectedPortableExecutable =(PortableExecutable)e.Node.Tag;
        }

        private void treeViewDependencies_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            String fileName = e.Node.Text;
            PortableExecutable portableExecutable = (PortableExecutable)e.Node.Tag;
            Application.Run(new MainWindow(portableExecutable));
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.ShowDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName;
                this.fileName = fileName;
                this.portableExecutable = new PortableExecutable(fileName, fileName);
                UpdateUI(fileName);

                ToolStripItem item = new ToolStripMenuItem();
                //Name that will apear on the menu
                item.Text = fileName;
                //Put in the Name property whatever neccessery to retrive your data on click event
                item.Name = fileName;
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
    }
}
