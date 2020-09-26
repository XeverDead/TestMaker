using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Core;
using Lib;
using Lib.ResultTypes;
using Lib.SaveLoaders;
using Lib.TaskTypes;
using UI.DialogWindows;
using UI.Pages;

namespace UI.Windows
{
    public partial class PassingWindow : Window
    {
        public bool IsLoadedProperly { get; }

        private readonly DefaultPassingCore core;
        private readonly TaskTopicTestView testView;

        private ITaskPage currentPage;

        private readonly string studentName;

        private readonly List<Task> tasks;
        private int currentTaskIndex;

        private List<TaskResult> results;

        private readonly Dictionary<Task, TreeViewItem> tasksTreeItems;

        private bool isShowingResults;

        private DispatcherTimer timer;
        private int timeLeft;
        public PassingWindow(string testPath, string resultPath, bool isShowingResults, string studentName)
        { 
            InitializeComponent();

            IsLoadedProperly = true;

            this.isShowingResults = isShowingResults;

            core = new DefaultPassingCore(new JsonDataProvider<Test>(testPath), new JsonDataProvider<TestResult>(resultPath), isShowingResults);

            if (isShowingResults)
            {
                results = core.GetResults(out bool wereResultsLoaded);

                IsLoadedProperly = wereResultsLoaded;

                if (!wereResultsLoaded)
                {
                    MessageBox.Show("Result file was corrupted. Returning to hub.");

                    Close();
                }
            }

            testView = core.GetTest(out bool wasTestLoaded);

            if (!wasTestLoaded && IsLoadedProperly)
            {
                IsLoadedProperly = wasTestLoaded;

                MessageBox.Show("Test file was corrupted. Returning to hub.");

                Close();
            }
            else if (wasTestLoaded)
            {
                if (testView.Test.HasPassword && isShowingResults)
                {
                    var passwordWindow = new TextInputWindow("Enter password");

                    passwordWindow.ShowDialog();

                    if (!passwordWindow.EnteredText.Equals(testView.Test.Password))
                    {
                        IsLoadedProperly = false;

                        MessageBox.Show("Password is wrong. Returning to hub.");

                        Close();
                    }
                }

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

                SetQuestionButton.Click += SetQuestionButtonClick;
                PrevButton.Click += PrevButtonClick;
                NextButton.Click += NextButtonClick;
                FinishButton.Click += FinishButtonClick;

                TestTree.SelectedItemChanged += TestTreeSelectedItemChanged;

                SetQuestionButton.IsEnabled = false;

                SetTestToTree();
                SetNewPage();

                if (!isShowingResults)
                {
                    SetTime();
                }
            }
        }

        private void SetQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            var currentItem = TestTree.SelectedItem as TreeViewItem;

            currentTaskIndex = tasks.IndexOf(currentItem.Header as Task);

            SetNewPage();
        }

        private void TestTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var currentItem = TestTree.SelectedItem as TreeViewItem;

            var condition = currentItem.Parent is TreeViewItem;

            while (condition)
            {
                currentItem = currentItem.Parent as TreeViewItem;

                currentItem.IsExpanded = true;

                condition = currentItem.Parent is TreeViewItem;
            }

            currentItem = TestTree.SelectedItem as TreeViewItem;

            SetQuestionButton.IsEnabled = currentItem.Header is Task;
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
                AskToMoveFromResultScreen();
            }
            else
            {
                SaveAnswer();

                AskToMoveToResultScreen(false);
            }
        }

        private void SetNewPage()
        {
            TaskGrid.Children.Clear();

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
                if (isShowingResults)
                {
                    currentPage = new MultipleChoicePage(mcTask, results[currentTaskIndex].Answer, mcTask.RightAnswersIndexes);
                }
                else if (results[currentTaskIndex].Answer != null)
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

            TaskGrid.Children.Add(pageGrid);
        }

        private void SaveAnswer()
        {
            if (currentPage.IsAnswerChosen)
            {
                results[currentTaskIndex].Answer = currentPage.Answer;
            }
        }
    
        private void AskToMoveToResultScreen(bool isTimeOver)
        {
            var result = MessageBoxResult.No;

            if (isTimeOver)
            {
                MessageBox.Show("Test time is over. Moving to result screen");
            }
            else
            {
                result = MessageBox.Show(this, "Would you like to finish this try?", "Ending", MessageBoxButton.YesNo);
            }

            if (result == MessageBoxResult.Yes || isTimeOver)
            {
                if (timer != null && !isTimeOver)
                {
                    timer.Stop();
                }

                core.SetMarksToResults(ref results, out double maxMark);
                core.SaveResult(results, studentName);

                var marks = new StringBuilder();
                var totalMark = 0.0;

                foreach (var taskResult in results)
                {
                    totalMark += taskResult.Mark;
                }

                marks.Append("You've got " + totalMark + " out of " + maxMark + " points\nWould you like to check your answers?");

                if (MessageBox.Show(marks.ToString(), "Result", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    TimeViewer.Text = "Watch results as long as you want";

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

        private void AskToMoveFromResultScreen()
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
            TestTree.Items.Add(headItem);

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

        private void SetTime()
        {
            if (testView.Test.IsTimeLimited)
            {
                timeLeft = testView.Test.Time;
                TimeViewer.Text = $"Time left: {timeLeft} seconds";

                timer = new DispatcherTimer(DispatcherPriority.Send)
                {
                    Interval = TimeSpan.FromSeconds(1),
                };

                timer.Tick += TimerTick;
                timer.Start();
            }
            else
            {
                TimeViewer.Text = $"Time unlimited";
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            TimeViewer.Text = $"Time left: {--timeLeft} seconds";

            if (timeLeft <= 0)
            {
                timer.Stop();

                SaveAnswer();

                AskToMoveToResultScreen(true);
            }
        }
    }
}
