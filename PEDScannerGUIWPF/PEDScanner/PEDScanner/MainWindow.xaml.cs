﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;
using PEDScannerLib.Core;
using PEDScannerLib.Objects;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Objects;
using PEDScannerLib;
using System.Data;
using PEDScanner;

namespace Wizard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //the portable executable file that is currently loaded into the program and displayed in the UI
        PortableExecutable portableExecutable;
        //the portable executable selected by the user from the UI
        PortableExecutable SelectedPortableExecutable;
        //the Server Process for 64-bit portable executable loading 
        Process exeProcess;

        public MainWindow()
        {
            InitializeComponent();
            this.portableExecutable = null;
            exeProcess = System.Diagnostics.Process.Start("Server64BitConsoleApp.exe");

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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(exeProcess != null)
            {
                exeProcess.Kill();
            }
        }

        private TreeViewItem CreateTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = o.ToString();
            item.Tag = o;
            item.Selected += treeItem_Selected;
            return item;
        }

        private TreeViewItem CreateImportsTreeItem(object o)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = o.ToString();
            item.Tag = o;
            return item;
        }

        private TreeViewItem CreateImportsTreeLibrary(String DependencyName)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = DependencyName,
            };

            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image image = new Image
            {
                MaxWidth = 20,
                MaxHeight = 20,
                Margin = new Thickness(0, 1, 5, 1),
                Source = new BitmapImage(new Uri(@"/PEDScanner;component/Resources/library16.png", UriKind.Relative))

            };
            //   ImageSource imageSource = Resources.Source.
            TextBlock textBlock = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.Black),
                Text = DependencyName,
            };

            ////  item.Header = o.ToString();

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(textBlock);
            item.Header = stackPanel;

            return item;
        }

        private TreeViewItem CreateImportsTreeFunction(String functionName)
        {
            TreeViewItem item = new TreeViewItem
            {
                Header = functionName,
            };

            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            Image image = new Image
            {
                MaxWidth = 20,
                MaxHeight = 20,
                Margin = new Thickness(0, 1, 5, 1),
                Source = new BitmapImage(new Uri(@"/PEDScanner;component/Resources/function16.png", UriKind.Relative))

            };
            //   ImageSource imageSource = Resources.Source.
            TextBlock textBlock = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.CadetBlue),
                Text = functionName,
            };

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(textBlock);
            item.Header = stackPanel;

            return item;
        }
   

        private TreeViewItem CreateDependecyTreeItem(PortableExecutable portableExecutable)
        {
            Image image;
            TextBlock textBlock;
           
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            if (portableExecutable.IsLoadable)
            {
                 image = new Image
                {
                    MaxWidth = 16,
                    MaxHeight = 16,
                    Margin = new Thickness(0,1,5,1),
                    Source = new BitmapImage(new Uri(@"/PEDScanner;component/Resources/Loadable.png", UriKind.Relative))

                };

                textBlock = new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.Green),
                    Text = portableExecutable.Name
                };
            }
            else
            {
                image = new Image
                {
                    MaxWidth = 16,
                    MaxHeight = 16,
                    Margin = new Thickness(0, 1, 5, 1),
                    Source = new BitmapImage(new Uri(@"/PEDScanner;component/Resources/missing16.png", UriKind.Relative))

                };

                textBlock = new TextBlock
                {
                    Foreground = new SolidColorBrush(Colors.Red),
                    Text = portableExecutable.Name
                };
            }

            stackPanel.Children.Add(image);
            stackPanel.Children.Add(textBlock);

            TreeViewItem item = new TreeViewItem
            {
                IsExpanded = true,
                Tag = portableExecutable,
                Header = stackPanel,
        };

            item.Selected += treeItem_Selected;

            return item;
        }

        public void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if ((item.Items.Count == 1) && (item.Items[0] is string))
            {
                item.Items.Clear();

                DirectoryInfo expandedDir = null;
                if (item.Tag is DriveInfo)
                    expandedDir = (item.Tag as DriveInfo).RootDirectory;
                if (item.Tag is DirectoryInfo)
                    expandedDir = (item.Tag as DirectoryInfo);
                try
                {
                    foreach (DirectoryInfo subDir in expandedDir.GetDirectories())
                        item.Items.Add(CreateTreeItem(subDir));
                }
                catch { }
            }
        }

        
        public void PopulateImports(List<ImportFunctionObject> imports)
        {

            treeViewImports.Items.Clear();
            if (imports.Count == 0)
            {
                treeViewImports.Visibility = Visibility.Hidden;
                // labelNoImports.Text = "No Imports";
                // labelNoImports.Show();
            }
            else
            {
                treeViewImports.Visibility = Visibility.Visible;
                // labelNoImports.Hide();

                Dictionary<string, List<ImportFunctionObject>> ImportsDependecyMap = new Dictionary<string, List<ImportFunctionObject>>();

                foreach (ImportFunctionObject import in imports)
                {
                    String nameOfDependecy = import.Dependency;
                    List<ImportFunctionObject> ImportsList;
                    if (ImportsDependecyMap.ContainsKey(nameOfDependecy))
                    {
                        ImportsList = ImportsDependecyMap[nameOfDependecy];
                    }
                    else
                    {
                        ImportsList = new List<ImportFunctionObject>();
                        ImportsDependecyMap[nameOfDependecy] = ImportsList;
                    }
                    ImportsList.Add(import);
                }

                foreach (string Dependecy in ImportsDependecyMap.Keys)
                {
                    TreeViewItem treeViewItem = CreateImportsTreeLibrary(Dependecy);
                    treeViewImports.Items.Add(treeViewItem);
                    List<ImportFunctionObject> ImportsList = ImportsDependecyMap[Dependecy];
                    foreach (ImportFunctionObject import in ImportsList)
                    {
                        TreeViewItem childTreeViewItem = CreateImportsTreeFunction(import.Function);
                        treeViewItem.Items.Add(childTreeViewItem);
                        childTreeViewItem.Tag = import;
                    }
                }
            }
        }

    public void PopulateExports(List<FunctionObject> exports)
        {         
            dataGridExports.ItemsSource = exports;
        }

        public void PopulateHeaders(List<HeaderObject> headers)
        {

            dataGridHeaders.ItemsSource = headers;

        }

        private void PopulateSections(List<SectionObject> sections)
        {
          
            dataGridSections.ItemsSource = sections;
        }


        // populate the Directories tab given the list of Directories 
        private void PopulateDirectories(List<DirectoryObject> directories)
        {

            dataGridDirectories.ItemsSource = directories;

        }


        class Issues
        {
            public Issues(String PortableExecutableName, String Issue) {
                this.PortableExecutableName = PortableExecutableName;
                this.Issue = Issue;
            }

            public String PortableExecutableName { get; set; }
            public String Issue { get; set; }

        }

        // populate the Issues tab given the list of Issues 
        private void PopulateIssues(Dictionary<string, List<string>> issueMap)
        {
            List<Issues> issues = new List<Issues>();

            foreach (String Key in issueMap.Keys)
            {
                //  List<string> listOfErrors = new List<string>();
                //    issueMap.TryGetValue(Key, out listOfErrors);
                foreach (String Issue in issueMap[Key])
                {
                    Issues issue = new Issues(Key, Issue);
                    issues.Add(issue);
                }
            }
            dataGridIssues.ItemsSource = issues;

        }


        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Portable Executable Dependecy Scanner 2018!", "About PED Scanner", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Exit_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DLL files (*.dll)|*.dll|EXE files (*.exe)|*.exe";

          
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath;
                filePath = openFileDialog.FileName;

                this.UpdateState(filePath);
                AddRecentMenuItem(filePath);
            }
        }


        private void AddRecentMenuItem(String FilePath)
        {
            MenuItem newMenuItem = new MenuItem();

            newMenuItem.Header = FilePath;
            newMenuItem.Click += RecetntsItem_Click;

            RecentsMenu.Items.Add(newMenuItem);

        }

        private void RecetntsItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Sender = (MenuItem)sender;
            String filePath = (String)Sender.Header;
            this.UpdateState(filePath);

        }

        // the even fired after selecting a certain dependecy from the dependecy tree is handled here 
        protected void treeViewDependencies_AfterSelect(object sender, RoutedEventArgs e)
        {
          //  this.SelectedPortableExecutable = (PortableExecutable)sender.Node.Tag;
            //if (SelectedPortableExecutable.FilePath != null)
            //{
            //    this.labelDependecyPath.Text = "File Path : " + ((PortableExecutable)e.Node.Tag).FilePath;
            //}
            //else
            //{
            //    this.labelDependecyPath.Text = "This Dependecy Named " + SelectedPortableExecutable.Name + " is Missing";
            //}

            //this.labelDependecyPath.Font = new Font(this.labelDependecyPath.Font, FontStyle.Bold);
        }
        
        // the event handler for imports tree it is used to display information about the import node clicked
        private void treeViewImports_Selected(object sender, RoutedEventArgs e)
        {
            if (((TreeViewItem)((TreeView)sender).SelectedItem).Tag != null) {
                dataGridImportsTable.Visibility = Visibility.Visible;
                 object obj = ((TreeViewItem)((TreeView)sender).SelectedItem).Tag;
                ImportFunctionObject importFunctionObject = (ImportFunctionObject)(obj);

                List<ImportFunctionObject> listOfImportFunctionObjects = new List<ImportFunctionObject>();
                listOfImportFunctionObjects.Add(importFunctionObject);
                dataGridImportsTable.ItemsSource = listOfImportFunctionObjects;
            }
        }
        
        // handle the "Add File" button click event
        private void buttonAddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DLL files (*.dll)|*.dll|EXE files (*.exe)|*.exe";

            //   if (openFileDialog.ShowDialog() == true)
            // txtEditor.Text = File.ReadAllText(openFileDialog.FileName);

            //     if (dlg.ShowDialog() == DialogResult.)
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath;
                filePath = openFileDialog.FileName;


                 this.UpdateState(filePath);

                AddRecentMenuItem(filePath);


            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //handle "Examine button" click event the selectedPortableExecutable stored in the form class is used here
        private void ButtonExamine_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPortableExecutable != null)
            {

                if (SelectedPortableExecutable.FilePath == null)
                {
                    MessageBox.Show("The Selected Dependecy " + SelectedPortableExecutable.Name +
                    " cannot be examined because it is missing!");
                }
                else
                {
                    MainWindow newMainWindowForm = new MainWindow(SelectedPortableExecutable);
                    newMainWindowForm.Closed += (s, args) => { newMainWindowForm.Close(); };
                    newMainWindowForm.ShowDialog();
                }

            }
        }

        void UpdateState(String filePath)
        {
            try
            {
                this.portableExecutable = new PortableExecutable(this.ExtractFileNameFromPath(filePath), filePath, true,new List<string>());
                this.UpdateUI(this.portableExecutable);
            }
            catch(ArrayTypeMismatchException e)
            {
                MessageBox.Show(e.Message);
            }
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //    //     Application.Exit();
            //}

        }

        // recursively update the dependecy tree on Dependecies tab using the dependecy information of the PE provided
        void RecursivelyPopulateTheTree(PortableExecutable portableExecutable, ItemCollection itemCollection)
        {

            TreeViewItem treeViewItem = new TreeViewItem
            {
                Header = (String)portableExecutable.FilePath,
                Tag = portableExecutable,
            };

            treeViewItem = CreateDependecyTreeItem(portableExecutable);

            itemCollection.Add(treeViewItem);
            if (portableExecutable.Dependencies.Count == 0)
            {
                return;
            }
            else
            {
                ItemCollection childrenItemCollection = treeViewItem.Items;
                foreach (object __o in portableExecutable.Dependencies)
                {
                    PortableExecutable pe = (PortableExecutable)__o;
                    // loop body
                    RecursivelyPopulateTheTree(pe, childrenItemCollection);
                }
            }
        }

        void UpdateUI(PortableExecutable portableExecutable)
        {
            treeViewDependencies.Items.Clear();
            dataGridImportsTable.Visibility = Visibility.Hidden;
            LabelStatus.Text = "";
            treeViewImports.ItemsSource = null;
            dataGridExports.ItemsSource = null;
            dataGridHeaders.ItemsSource = null;
            dataGridSections.ItemsSource = null;
            dataGridDirectories.ItemsSource = null;

            ItemCollection itemCollection = treeViewDependencies.Items;

            if (portableExecutable != null)
            {
                using (new WaitCursor())
                {
                    PortableExecutableLoader portableExecutableLoader = new PortableExecutableLoader();
                    portableExecutableLoader.Load(portableExecutable);
                    RecursivelyPopulateTheTree(portableExecutable, itemCollection);
                    PopulateHeaders(portableExecutable.Headers);
                    PopulateImports(portableExecutable.ImportFunctions);
                    PopulateExports(portableExecutable.ExportedFunctions);
                    PopulateSections(portableExecutable.Sections);
                    PopulateDirectories(portableExecutable.Directories);
                    PopulateIssues(portableExecutableLoader.smartSuggestionEngine.error_list);
                }        
            }
        }
        
        //utility function to extract file name given the file path
        private String ExtractFileNameFromPath(String FilePath)
        {
            String[] arrayofNames = FilePath.Split('\\');
            return arrayofNames[arrayofNames.Length - 1];
        }

        void treeItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = sender as TreeViewItem;
            this.SelectedPortableExecutable = (PortableExecutable)(PortableExecutable)item.Tag;
            if (SelectedPortableExecutable.FilePath != null)
            {
                LabelStatus.Text = SelectedPortableExecutable.FilePath;

            }
            else
            {
                LabelStatus.Text = "The selected portable executable is missing";
            }
            e.Handled = true;
        }

        
        private void Reverse_Dependencies_Click(object sender, RoutedEventArgs e)
        {
            var wizard = new WizardDialogBox();
            var showDialog = wizard.ShowDialog();
            var dialogResult = showDialog != null && (bool)showDialog;
            //MessageBox.Show(
            //    dialogResult
            //        ? $"Succesfully Detected Reverse Dependencies in {wizard.WizardData.FolderPath} for {ExtractFileNameFromPath(wizard.WizardData.FilePath)}\n"
            //        : "Canceled.");
        }

        private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
    }
}
