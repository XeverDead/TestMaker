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
        private TaskTopicTestView testView;

        private ITaskPage currentPage;

        private string studentName;

        private List<Lib.Task> tasks;
        private int currentTaskIndex;

        private List<TaskResult> results;

        private Dictionary<Lib.Task, TreeViewItem> tasksTreeItems;
        public PassingWindow()
        { 
            InitializeComponent();

            studentName = GetStudentName();

            core = new DefaultPassingCore(new JsonDataProvider<Test>("D:\\Test"));
            testView = core.GetTest();

            tasks = new List<Lib.Task>(testView.TasksAndTopics.Keys);
            currentTaskIndex = 0;

            results = new List<TaskResult>();
            foreach (var task in testView.TasksAndTopics.Keys)
            {
                results.Add(new TaskResult(task, null));
            }

            tasksTreeItems = new Dictionary<Lib.Task, TreeViewItem>();

            prevButton.Click += PrevButtonClick;
            nextButton.Click += NextButtonClick;
            finishButton.Click += FinishButtonClick;

            testTree.SelectedItemChanged += TestTreeSelectedItemChanged;

            SetTestToTree();

            SetNewPage();
        }

        private void TestTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var currentdItem = testTree.SelectedItem as TreeViewItem;

            var condition = currentdItem.Parent is TreeViewItem;

            while (condition)
            {
                currentdItem = currentdItem.Parent as TreeViewItem;

                currentdItem.IsExpanded = true;

                condition = currentdItem.Parent is TreeViewItem;
            }
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
            taskGrid.Children.Clear();

            tasksTreeItems[tasks[currentTaskIndex]].IsSelected = true;

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

            taskGrid.Children.Add(pageGrid);
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
                core.SetMarksToResults(ref results, out double maxMark);

                var marks = new StringBuilder();
                var totalMark = 0.0;

                foreach (var taskResult in results)
                {
                    marks.Append(taskResult.Mark + " ");
                    totalMark += taskResult.Mark;
                }

                marks.Append("\n" + totalMark + " out of " + maxMark);

                MessageBox.Show(marks.ToString());

                Close();
            }
        }

        private string GetStudentName()
        {
            var enterNameWindow = new TextInputWindow("Enter your name");

            enterNameWindow.ShowDialog();
            return enterNameWindow.EnteredText;
        }

        private void SetTestToTree()
        {
            var headItem = new TreeViewItem() { Header = testView.Test };
            testTree.Items.Add(headItem);

            foreach (var topic in testView.Test.Topics)
            {
                var topicItem = new TreeViewItem() { Header = topic };
                headItem.Items.Add(topicItem);

                SetTopicItemsToTree(topicItem);
            }
        }

        private void SetTopicItemsToTree(TreeViewItem topicItem)
        {
            var topic = topicItem.Header as Topic;

            if (topic.HasTasks)
            {
                foreach (var task in topic.Tasks)
                {
                    var taskItem = new TreeViewItem() { Header = task };

                    tasksTreeItems.Add(task, taskItem);

                    topicItem.Items.Add(taskItem);
                }
            }

            if (topic.HasSubTopics)
            {
                foreach (var subTopic in topic.SubTopics)
                {
                    var subTopicItem = new TreeViewItem() { Header = subTopic };

                    topicItem.Items.Add(subTopicItem);
                    SetTopicItemsToTree(subTopicItem);
                }
            }
        }
    }
}
