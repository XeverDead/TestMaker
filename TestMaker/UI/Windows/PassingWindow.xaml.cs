using Core;
using Lib;
using Lib.ResultTypes;
using Lib.TaskTypes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
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
using UI.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PassingWindow : Window
    {
        public bool IsLoadedProperely { get; private set; }

        private readonly DefaultPassingCore core;
        private TaskTopicTestView testView;

        private ITaskPage currentPage;

        private string studentName;

        private List<Task> tasks;
        private int currentTaskIndex;

        private List<TaskResult> results;

        private Dictionary<Task, TreeViewItem> tasksTreeItems;

        private bool isShowingResults;
        public PassingWindow(string testPath, string resultPath, bool isShowingResults, string studentName)
        { 
            InitializeComponent();

            this.isShowingResults = isShowingResults;

            core = new DefaultPassingCore(new JsonDataProvider<Test>(testPath), new JsonDataProvider<TestResult>(resultPath), isShowingResults);

            if (isShowingResults)
            {
                results = core.GetResults(out bool wereResultsLoaded);

                IsLoadedProperely = wereResultsLoaded;

                if (!wereResultsLoaded)
                {
                    MessageBox.Show("Result file was corrupted. Returning to hub.");

                    var hubWindow = new HubWindow(TestActions.ViewResult);

                    Close();
                }
            }

            testView = core.GetTest(out bool wasTestLoaded);

            IsLoadedProperely = wasTestLoaded;

            if (!wasTestLoaded)
            {
                MessageBox.Show("Test file was corrupted. Returning to hub.");

                var hubWindow = new HubWindow(TestActions.PassTest);

                Close();
            }
            else
            {
                tasks = new List<Task>(testView.TasksAndTopics.Keys);
                currentTaskIndex = 0;

                if (!isShowingResults)
                {
                    this.studentName = studentName;

                    results = new List<TaskResult>();
                    foreach (var task in testView.TasksAndTopics.Keys)
                    {
                        results.Add(new TaskResult(task, null));
                    }
                }

                tasksTreeItems = new Dictionary<Task, TreeViewItem>();

                prevButton.Click += PrevButtonClick;
                nextButton.Click += NextButtonClick;
                finishButton.Click += FinishButtonClick;

                testTree.SelectedItemChanged += TestTreeSelectedItemChanged;

                SetTestToTree();

                SetNewPage();
            }
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
            if (isShowingResults)
            {
                AskToMoveFromResultscreen();
            }
            else
            {
                SaveAnswer();

                AskToMoveToResultScreen();
            }
        }

        private void SetNewPage()
        {
            taskGrid.Children.Clear();

            tasksTreeItems[tasks[currentTaskIndex]].IsSelected = true;

            if (tasks[currentTaskIndex] is SingleChoice scTask)
            {
                if (isShowingResults)
                {
                    var answer = -1;
                    if (results[currentTaskIndex].Answer != null)
                    {
                        answer = (int)results[currentTaskIndex].Answer;
                    }

                    currentPage = new SingleChoicePage(scTask, answer, scTask.RightAnswerIndex);
                }
                else if (results[currentTaskIndex].Answer != null)
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
                else if (isShowingResults) 
                {
                    currentPage = new MultipleChoicePage(mcTask, results[currentTaskIndex].Answer, mcTask.RightAnswersIndexes);
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
    
        private void AskToMoveToResultScreen()
        {
            var result = MessageBox.Show("Would you like to finish this try?", "Ending", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                core.SetMarksToResults(ref results, out double maxMark);
                core.SaveResult(results, studentName);

                var marks = new StringBuilder();
                var totalMark = 0.0;

                foreach (var taskResult in results)
                {
                    totalMark += taskResult.Mark;
                }

                marks.Append("You've got " + totalMark + " out of " + maxMark + "points\nWould you like to check your answers?");

                if (MessageBox.Show(marks.ToString(), "Result", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {

                    isShowingResults = true;

                    currentTaskIndex = 0;

                    SetNewPage();
                }
                else
                {
                    var mainWindow = new MainWindow();

                    mainWindow.Show();
                    Close();
                }
            }
        }

        private void AskToMoveFromResultscreen()
        {
            var result = MessageBox.Show("Would you like to quit to main menu?", "Quit", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();

                mainWindow.Show();
                Close();
            }
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
