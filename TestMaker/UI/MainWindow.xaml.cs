using Core;
using Lib;
using Lib.TaskTypes;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
using UI.DialogWindows;
using UI.Pages;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DefaultPassingCore core;
        public MainWindow()
        { 
            InitializeComponent();

            core = new DefaultPassingCore("Test.tmt", new SaveLoad());

            mainPanel.Children.Add(new SingleChoicePage(core.CurrentTask as SingleChoice).GetAsGrid());
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if (core.SetNextTaskToCurrent())
            {
                foreach (var element in mainPanel.Children)
                {
                    if (element is Grid optionsGrid)
                    {
                        mainPanel.Children.Remove(optionsGrid);
                        break;
                    }
                }

                mainPanel.Children.Add(new SingleChoicePage(core.CurrentTask as SingleChoice).GetAsGrid());
            }
        }

        private void PrevButtonClick(object sender, RoutedEventArgs e)
        {
            if (core.SetPrevTaskToCurrent())
            {
                foreach (var element in mainPanel.Children)
                {
                    if (element is Grid optionsGrid)
                    {
                        mainPanel.Children.Remove(optionsGrid);
                        break;
                    }
                }

                mainPanel.Children.Add(new SingleChoicePage(core.CurrentTask as SingleChoice).GetAsGrid());
            }
        }
    }
}
