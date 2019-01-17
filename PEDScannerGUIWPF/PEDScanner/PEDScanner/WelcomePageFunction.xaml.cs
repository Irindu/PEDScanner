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
    /// Interaction logic for WelcomePageFunction.xaml
    /// </summary>
    /// 


    public partial class WelcomePageFunction : PageFunction<WizardResult>
    {
        public WelcomePageFunction(WizardData wizardData)
        {
            InitializeComponent();
            DataContext = wizardData;
            ShowsNavigationUI = false;

        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            //var wizardPage1 = new WizardPage1((WizardData)DataContext);
              var SelectDirectoryPage = new PageFunctionSelectDirectory((WizardData)DataContext);
              SelectDirectoryPage.Return += wizardPage_Return;
              NavigationService?.Navigate(SelectDirectoryPage);

            //wizardPage1.Return += wizardPage_Return;
            //NavigationService?.Navigate(wizardPage1);
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
    }
}
