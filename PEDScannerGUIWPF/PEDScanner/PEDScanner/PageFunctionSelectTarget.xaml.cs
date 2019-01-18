﻿using System;
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
using Microsoft.Win32;


namespace Wizard
{
    /// <summary>
    /// Interaction logic for PageFunctionSelectTarget.xaml
    /// </summary>
    public partial class PageFunctionSelectTarget : PageFunction<WizardResult>
    {
        WizardData wizardDataRef;

        public PageFunctionSelectTarget(WizardData wizardData)
        {
            InitializeComponent();
            DataContext = wizardData;
            wizardDataRef = wizardData;
            ShowsNavigationUI = false;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to previous wizard page
            NavigationService?.GoBack();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            // Go to next wizard page
            var ReverseDependenciesDisplayPage = new PageFunctionDisplayReverseDependencies((WizardData)DataContext);
            ReverseDependenciesDisplayPage.Return += wizardPage_Return;
            NavigationService?.Navigate(ReverseDependenciesDisplayPage);
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

        private void FilePick_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DLL files (*.dll)|*.dll";


            if (openFileDialog.ShowDialog() == true)
            {
                string filePath;
                filePath = openFileDialog.FileName;

                //  Label label = new Label();
                //    label.FontSize = 5;
                //    label.Content = filePath;
                //  TargetDLLPath.Child = label;
                TargetDLLPatTextBox.Text = filePath;
              //  wizardDataRef.FilePath = filePath;
            }
        }
    }
}
