using Lib;
using Lib.TaskTypes;
using System;
using System.Windows;
using System.Windows.Controls;
using UI.Pages;
using Core;
using Lib.SaveLoaders;
using UI.DialogWindows;

namespace UI.Windows
{
    public partial class RedactingWindow : Window
    {
        private readonly DefaultRedactingCore core;
        private Test test;

        public bool IsLoadedProperly { get; }

        public RedactingWindow(string path, bool isNewTest)
        {
            InitializeComponent();

            core = new DefaultRedactingCore(new JsonDataProvider<Test>(path), isNewTest);
            test = core.GetTest(out bool wasTestLoaded);

            IsLoadedProperly = wasTestLoaded;

            if (isNewTest)
            {
                var testName = path.Substring(path.LastIndexOf('\\') + 1);
                testName = testName[0..^4];

                test.Name = testName;
            }

            if (!wasTestLoaded)
            {
                MessageBox.Show("Test file was corrupted. Returning to hub.");

                Close();
            }
            else
            {
                if (test.HasPassword)
                {
                    var passwordWindow = new TextInputWindow("Enter password");

                    passwordWindow.ShowDialog();

                    if (!passwordWindow.EnteredText.Equals(test.Password))
                    {
                        IsLoadedProperly = false;

                        MessageBox.Show("Password is wrong. Returning to hub.");

                        Close();
                    }
                }

                SetTestToTree();

                TestTree.SelectedItemChanged += TestTreeSelectedItemChanged;

                AddTaskButton.IsEnabled = false;
                AddTopicButton.IsEnabled = false;
                RemoveButton.IsEnabled = false;
                RenameButton.IsEnabled = false;

                AddTaskButton.Click += AddTaskButtonClick;
                AddTopicButton.Click += AddTopicButtonClick;
                RemoveButton.Click += RemoveButtonClick;
                RenameButton.Click += RenameButtonClick;
                DeclineChangesButton.Click += DeclineChangesButtonClick;
                FinishButton.Click += FinishButtonClick;
            }
        }

        private void DeclineChangesButtonClick(object sender, RoutedEventArgs e)
        {
            GoToMenuWithoutSaving();
        }

        private void GoToMenuWithoutSaving()
        {
            var result = MessageBox.Show("Would you like to decline changes and go to main menu?", "Decline changes", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var mainWindow = new MainWindow();

                Close();
                mainWindow.Show();
            }
        }

        private void RenameButtonClick(object sender, RoutedEventArgs e)
        {
            Rename(TestTree.SelectedItem as TreeViewItem);
        }

        private void FinishButtonClick(object sender, RoutedEventArgs e)
        {
            FinishRedacting();
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            Remove(TestTree.SelectedItem as TreeViewItem);
        }

        private void AddTopicButtonClick(object sender, RoutedEventArgs e)
        {
            AddTopic(TestTree.SelectedItem as TreeViewItem);
        }

        private void AddTaskButtonClick(object sender, RoutedEventArgs e)
        {
            AddTask(TestTree.SelectedItem as TreeViewItem);
        }

        private void FinishRedacting()
        {
            var messageBoxResult = MessageBox.Show("Do you want to finish redacting this test?", "Finish", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                core.SaveTest(test);

                var mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }

        private void Remove(TreeViewItem itemToRemove)
        {
            var parentItem = itemToRemove.Parent as TreeViewItem;

            if (itemToRemove.Header is Topic topicToRemove)
            {
                if (parentItem.Header is Test)
                {
                    test.Topics.Remove(topicToRemove);
                }
                else if (parentItem.Header is Topic parentTopic)
                {
                    parentTopic.SubTopics.Remove(topicToRemove);
                }
            }
            else if (itemToRemove.Header is Task taskToRemove) 
            {
                var parentTopic = parentItem.Header as Topic;
                parentTopic.Tasks.Remove(taskToRemove);
            }

            parentItem.Items.Remove(itemToRemove);
        }

        private void AddTopic(TreeViewItem parentItem)
        {
            var topicToAdd = new Topic() { Name = "New topic" };

            if (parentItem.Header is Test)
            {
                test.Topics.Add(topicToAdd);
            }
            else if (parentItem.Header is Topic topic) 
            {
                topic.SubTopics.Add(topicToAdd);
            }

            parentItem.Items.Add(new TreeViewItem() { Header = topicToAdd});
            parentItem.IsExpanded = true;
        }

        private void AddTask(TreeViewItem parentItem)
        {
            var topic = parentItem.Header as Topic;

            var chooseTaskTypeWindow = new ChooseTaskTypeWindow(core.GetAllTaskTypes());
            if (chooseTaskTypeWindow.ShowDialog() == true)
            {
                var taskToAdd = Activator.CreateInstance(chooseTaskTypeWindow.ChosenType);

                var task = taskToAdd as Task;
                task.Question = "Unknown";

                topic.Tasks.Add(task);

                var taskItem = new TreeViewItem() { Header = task };
                taskItem.Unselected += TaskItemUnselected;

                parentItem.Items.Add(taskItem);

                parentItem.IsExpanded = true;
            }
        }

        private void TestTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = TestTree.SelectedItem as TreeViewItem;

            if (selectedItem.Header is Topic)
            {
                AddTaskButton.IsEnabled = true;
                AddTopicButton.IsEnabled = true;
                RemoveButton.IsEnabled = true;
                RenameButton.IsEnabled = true;
            }
            else if (selectedItem.Header is Test)
            {
                AddTaskButton.IsEnabled = false;
                AddTopicButton.IsEnabled = true;
                RemoveButton.IsEnabled = false;
                RenameButton.IsEnabled = true;
            }
            else
            {
                AddTaskButton.IsEnabled = false;
                AddTopicButton.IsEnabled = false;
                RemoveButton.IsEnabled = true;
                RenameButton.IsEnabled = false;
            }

            SetRedactPage(selectedItem);
        }

        private void SetTestToTree()
        {
            var headItem = new TreeViewItem() { Header = test };
            TestTree.Items.Add(headItem);

            foreach (var topic in test.Topics)
            {
                var topicItem = new TreeViewItem() { Header = topic };
                headItem.Items.Add(topicItem);

                SetTopicItemsToTree(topicItem);
            }
        }

        public void Rename(TreeViewItem treeItem)
        {
            dynamic testOrTopic = treeItem.Header;

            var enterTextWindow = new TextInputWindow("Rename");

            if (enterTextWindow.ShowDialog() == true)
            {
                testOrTopic.Name = enterTextWindow.EnteredText;

                RefreshTreeItem(ref treeItem);
            }
        }

        private void TaskItemUnselected(object sender, RoutedEventArgs e)
        {
            var treeItem = sender as TreeViewItem;

            RefreshTreeItem(ref treeItem);
        }

        private void SetRedactPage(TreeViewItem treeItem)
        {
            if (SettingsGrid.Children.Count > 0)
            {
                SettingsGrid.Children.RemoveAt(0);
            }

            var page = new Page();

            if (treeItem.Header is Topic)
            {
                return;
            }
            else if (treeItem.Header is SingleChoice scTask)
            {
                page = new RedactSingleChoicePage(ref scTask);
            }
            else if (treeItem.Header is MultipleChoice mcTask)
            {
                page = new RedactMultipleChoicePage(ref mcTask);
            }
            else if (treeItem.Header is Test)
            {
                page = new RedactTestPage(ref test);
            }

            var taskGrid = page.Content as Grid;
            page.Content = null;
            SettingsGrid.Children.Add(taskGrid);
        }

        private void RefreshTreeItem(ref TreeViewItem treeItem)
        {
            var header = treeItem.Header;

            treeItem.Header = "";
            treeItem.Header = header;
        }

        private void SetTopicItemsToTree(TreeViewItem topicItem)
        {
            var topic = topicItem.Header as Topic;

            if (topic.HasTasks)
            {
                foreach (var task in topic.Tasks)
                {
                    var taskItem = new TreeViewItem() { Header = task };
                    taskItem.Unselected += TaskItemUnselected;

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
