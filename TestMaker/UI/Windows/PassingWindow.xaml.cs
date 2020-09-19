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
    public partial class PassingWindow : Window
    {
        private readonly DefaultPassingCore core;
        private Dictionary<Lib.Task, Topic> tasksAndTopics;

        private ITaskPage currentPage;

        private string studentName;

        private List<Lib.Task> tasks;
        private int currentTaskIndex;
        private string testName;
        private bool isTestTimeLimited;
        private int testTime;

        private List<TaskResult> results;
        public PassingWindow()
        { 
            InitializeComponent();

            studentName = GetStudentName();

            core = new DefaultPassingCore(new JsonDataProvider<Test>("Test"));
            (testName, tasksAndTopics, isTestTimeLimited, testTime) = core.GetTest();

            tasks = new List<Lib.Task>(tasksAndTopics.Keys);
            currentTaskIndex = 0;

            results = new List<TaskResult>();
            foreach (var task in tasks)
            {
                results.Add(new TaskResult(task, null));
            }

            SetNewPage();
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAnswer();

            if (currentTaskIndex + 1 < tasks.Count)
            {
                currentTaskIndex++;
                SetNewPage();
            }
            else
            {
                MessageBox.Show("This is last task", "Last task");
            }
        }

        private void PrevButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAnswer();

            if (currentTaskIndex > 0) 
            {
                currentTaskIndex--;
                SetNewPage();
            }
            else
            {
                MessageBox.Show("This is first task", "First task");
            }
        }

        private void FinishButtonClick(object sender, RoutedEventArgs e)
        {
            AskToMoveToResultScreen();
        }

        private void SetNewPage()
        {
            foreach (var element in mainPanel.Children) 
            {
                if (element is Grid taskGrid && !element.Equals(buttonGrid))
                {
                    mainPanel.Children.Remove(taskGrid);
                    break;
                }
            }

            if (tasks[currentTaskIndex] is SingleChoice scTask)
            {
                if (results[currentTaskIndex].Answer != null)
                {
                    currentPage = new SingleChoicePage(scTask, results[currentTaskIndex].Answer);
                }
                else
                {
                    currentPage = new SingleChoicePage(scTask);
                }
            }
            else if (tasks[currentTaskIndex] is MultipleChoice mcTask)
            {
                if (results[currentTaskIndex].Answer != null)
                {
                    currentPage = new MultipleChoicePage(mcTask, results[currentTaskIndex].Answer);
                }
                else
                {
                    currentPage = new MultipleChoicePage(mcTask);
                }
            }

            var pageGrid = currentPage.Content as Grid;
            currentPage.Content = null;

            mainPanel.Children.Add(pageGrid);
        }

        private void SaveAnswer()
        {
            if (currentPage.IsAnswerChosen)
            {
                results[currentTaskIndex].Answer = currentPage.Answer;
            }
        }
    
        private void AskToMoveToResultScreen()//Результаты показываются в заглушке просто для их контроля. Позже сделаю норм окно.
        {
            var result = MessageBox.Show("Would you like to finish this try?", "Ending", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var mark = core.CountTestMark(ref results, out double maxMark);

                var marks = new StringBuilder();

                foreach (var taskResult in results)
                {
                    marks.Append(taskResult.Mark + " ");
                }

                marks.Append("\n" + mark + " out of " + maxMark);

                MessageBox.Show(marks.ToString());

                Close();
            }
        }

        private string GetStudentName()
        {
            var enterNameWindow = new EnterNameWindow();

            enterNameWindow.ShowDialog();
            return enterNameWindow.EnteredName;
        }
    }
}
