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
using WinForms = System.Windows.Forms;


namespace Wizard
{
    /// <summary>
    /// Interaction logic for PageFunctionSelectDirectory.xaml
    /// </summary>
    public partial class PageFunctionSelectDirectory : PageFunction<WizardResult>
    {
        WizardData wizardDataRef;

        public PageFunctionSelectDirectory(WizardData wizardData)
        {
            InitializeComponent();
            DataContext = wizardData;
            ShowsNavigationUI = false;
            wizardDataRef = wizardData;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to previous wizard page
            NavigationService?.GoBack();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var pageFunctionSelectTarget = new PageFunctionSelectTarget((WizardData)DataContext);
            pageFunctionSelectTarget.Return += wizardPage_Return;
            NavigationService?.Navigate(pageFunctionSelectTarget);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Cancel the wizard and don't return any data
            OnReturn(new ReturnEventArgs<WizardResult>(WizardResult.Canceled));
        }

        public void wizardPage_Return(object sender, ReturnEventArgs<WizardResult> e)
        {
            // If returning, wizard was completed (finished or canceled),
            // so continue returning to calling page
            OnReturn(e);
        }

        private void SelectDirectroyButton_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog openFileDialog = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = openFileDialog.ShowDialog();
            if (result == WinForms.DialogResult.OK)
            {
                string filePath;
                filePath = openFileDialog.SelectedPath;
                if (System.IO.Directory.Exists(filePath))
                {
                    TargetDirectoryPathLabel.Content = filePath;
                    wizardDataRef.FolderPath = filePath;
                }

            }
        }
    }
}
