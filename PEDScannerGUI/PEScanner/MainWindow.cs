using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PEScannerLibrary;


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
            this.portableExecutable = new PortableExecutable(fileName);
        }

        public MainWindow(PortableExecutable portableExecutable)
        {
            InitializeComponent();
            this.fileName = portableExecutable.FileName;
            this.portableExecutable = portableExecutable;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            dataGridViewHeaders.Columns[0].Name = "Release Date";
            dataGridViewHeaders.Columns[1].Name = "Track";
            PopulateDataGridView();
            dataGridViewHeaders.RowHeadersVisible = false;

        }

        private void PopulateDataGridView()
        {

            string[] row0 = { "11/22/1968", "29" };
            string[] row1 = { "1960", "6"};
            string[] row2 = { "11/11/1971", "1", };
            string[] row3 = { "1988", "7"};
            string[] row4 = { "5/1981", "9"};
            string[] row5 = { "6/10/2003", "13"};

            dataGridViewHeaders.Rows.Add(row0);
            dataGridViewHeaders.Rows.Add(row1);
            dataGridViewHeaders.Rows.Add(row2);
            dataGridViewHeaders.Rows.Add(row3);
            dataGridViewHeaders.Rows.Add(row4);
            dataGridViewHeaders.Rows.Add(row5);

            dataGridViewHeaders.Columns[0].DisplayIndex = 1;
            dataGridViewHeaders.Columns[1].DisplayIndex = 0;
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
            PortableExecutable pe = new PortableExecutable(fileName);
            pe.MakeDependencies();
            TreeNodeCollection tNodes = treeViewDependencies.Nodes;

            RecursivelyPopulateTheTree(pe, tNodes);

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

            TreeNode treeNode = tNodes.Add(portableExecutable.FileName);
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
                this.portableExecutable = new PortableExecutable(fileName);
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
