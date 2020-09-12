using Core;
using Lib;
using Lib.ResultTypes;
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
        private readonly DefaultPassingCore core;
        private ITaskPage currentPage;
        public MainWindow()
        { 
            InitializeComponent();

            core = new DefaultPassingCore(new JsonTestProvider("Test.tmt"));

            SetNewPage();
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAnswer();

            if (core.SetNextTaskToCurrent())
            {
                SetNewPage();
            }
            else
            {
                MessageBox.Show(core.CheckTest().ToString());
            }
        }

        private void PrevButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAnswer();

            if (core.SetPrevTaskToCurrent())
            {
                SetNewPage();
            }
        }

        private void SetNewPage()
        {
            foreach (var element in mainPanel.Children)
            {
                if (element is Grid taskGrid)
                {                   
                    mainPanel.Children.Remove(taskGrid);
                    break;
                }
            }

            if (core.CurrentTask is SingleChoice)
            {
                currentPage = new SingleChoicePage(core.CurrentTask as SingleChoice);
            }
            else if (core.CurrentTask is MultipleChoice)
            {
                currentPage = new MultipleChoicePage(core.CurrentTask as MultipleChoice);
            }

            var pageGrid = currentPage.Content as Grid;
            currentPage.Content = null;

            mainPanel.Children.Add(pageGrid);
        }

        private void SaveAnswer()
        {
            if (currentPage.IsAnswerChosen)
            {
                core.SetResult(currentPage.Task, currentPage.Answer);
            }
        }
    }
}
