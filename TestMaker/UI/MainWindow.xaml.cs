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
        private Dictionary<Lib.Task, Topic> tasksAndTopics;

        private ITaskPage currentPage;

        private string studentName;

        private List<Lib.Task> tasks;
        private int currentTaskIndex;

        private Dictionary<Lib.Task, TaskResult> answers;
        public MainWindow()
        { 
            InitializeComponent();

            studentName = GetStudentName();

            core = new DefaultPassingCore(new JsonDataProvider<Test>("Test"));
            tasksAndTopics = core.GetTest();

            tasks = new List<Lib.Task>(tasksAndTopics.Keys);
            currentTaskIndex = 0;

            answers = new Dictionary<Lib.Task, TaskResult>();
            foreach (var task in tasks)
            {
                answers.Add(task, null);
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
        }

        private void PrevButtonClick(object sender, RoutedEventArgs e)
        {
            SaveAnswer();

            if (currentTaskIndex > 0) 
            {
                currentTaskIndex--;
                SetNewPage();
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
                if (answers[scTask] != null)
                {
                    currentPage = new SingleChoicePage(scTask, answers[scTask].Answer);
                }
                else
                {
                    currentPage = new SingleChoicePage(scTask);
                }
            }
            else if (tasks[currentTaskIndex] is MultipleChoice mcTask)
            {
                if (answers[mcTask] != null)
                {
                    currentPage = new MultipleChoicePage(mcTask, answers[mcTask].Answer);
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
                answers[tasks[currentTaskIndex]] = new TaskResult(tasks[currentTaskIndex], currentPage.Answer);
            }
        }
    
        private void AskToMoveToResultScreen()//Результаты показываются в заглушке просто для их контроля. Позже сделаю норм окно.
        {
            var result = MessageBox.Show("Would you like to finish this try?", "Ending", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var mark = core.CountTestMark(ref answers, out double maxMark);

                var marks = new StringBuilder();

                foreach (var taskResult in answers.Values)
                {
                    marks.Append(taskResult.Mark + " ");
                }

                marks.Append("\n" + mark + " out of " + maxMark);

                MessageBox.Show(marks.ToString());
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
