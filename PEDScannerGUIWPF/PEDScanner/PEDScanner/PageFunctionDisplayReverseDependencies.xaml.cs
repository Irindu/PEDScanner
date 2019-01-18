using PEDScannerLib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wizard
{
    /// <summary>
    /// Interaction logic for PageFunctionDisplayReverseDependencies.xaml
    /// </summary>
    public partial class PageFunctionDisplayReverseDependencies : PageFunction<WizardResult>
    {
        WizardData wizardDataRef;
        public PageFunctionDisplayReverseDependencies(WizardData wizardData)
        {
            InitializeComponent();

            // Bind wizard state to UI
            DataContext = wizardData;
            wizardDataRef = wizardData;
            displayDependencies();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to previous wizard page
            NavigationService?.GoBack();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the wizard and don't return any data
            OnReturn(new ReturnEventArgs<WizardResult>(WizardResult.Canceled));
        }

        private void finishButton_Click(object sender, RoutedEventArgs e)
        {
            // Finish the wizard and return bound data to calling page
            OnReturn(new ReturnEventArgs<WizardResult>(WizardResult.Finished));
        }

        private void displayDependencies() {
            //ReverseDependencyDetector reverseDependencyDetector = new ReverseDependencyDetector();
            //String FilePath = @"E:\TEST";
            //String path2 = @"E:\TEST\CppDll.dll";
            //PortableExecutable testPE = new PortableExecutable(ExtractFileNameFromPath(path2), path2);
            //PortableExecutableLoader loader = new PortableExecutableLoader();
            //loader.Load(testPE);
            // MessageBox.Show("test" + ((PortableExecutable)((reverseDependencyDetector.Process(FilePath, testPE))[0])).FilePath);

            //List<PortableExecutable> list = new List<PortableExecutable>();
            //list.Add(testPE);
            //list.Add(testPE);
            //list.Add(testPE);

            ReverseDependencyDetector reverseDependencyDetector = new ReverseDependencyDetector();
            String FolderPath = wizardDataRef.FolderPath;
            String targetPath = wizardDataRef.FilePath;
            PortableExecutable targetPE = new PortableExecutable(ExtractFileNameFromPath(targetPath), targetPath);
            PortableExecutableLoader loader = new PortableExecutableLoader();
            loader.Load(targetPE);
            List<PortableExecutable> list = reverseDependencyDetector.Process(FolderPath, targetPE);

            ReverseDependenciesList.ItemsSource = list;
        }

        //utility function to extract file name given the file path
        private String ExtractFileNameFromPath(String FilePath)
        {
            String[] arrayofNames = FilePath.Split('\\');
            return arrayofNames[arrayofNames.Length - 1];
        }

       
    }
}
