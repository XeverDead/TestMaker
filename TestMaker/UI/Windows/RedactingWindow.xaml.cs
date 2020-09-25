using Lib;
using Lib.TaskTypes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using UI.Pages;
using Core;
using System.Collections.ObjectModel;
using System.IO;
using UI.DialogWindows;
using System.Printing;
using System.Security.Cryptography.Xml;
using System.Net.Http.Headers;
using System.Threading;

namespace UI.Windows
{
    public partial class RedactingWindow : Window
    {
        private DefaultRedactingCore core;
        private Test test;

        public bool IsLoadedProperely { get; private set; }

        public RedactingWindow(string path, bool isNewTest)
        {
            InitializeComponent();

            core = new DefaultRedactingCore(new JsonDataProvider<Test>(path), isNewTest);
            test = core.GetTest(out bool wasTestLoaded);

            IsLoadedProperely = wasTestLoaded;

            if (isNewTest)
            {
                var testName = path.Substring(path.LastIndexOf('\\') + 1);
                testName = testName[0..^4];

                test.Name = testName;
            }

            if (!wasTestLoaded)
            {
                MessageBox.Show("Test file was corrupted. Returning to hub.");

                //var hubWindow = new HubWindow(TestActions.RedactTest);

                //hubWindow.Show();

                Close();
            }
            else
            {
                SetTestToTree();

                testTree.SelectedItemChanged += TestTreeSelectedItemChanged;

                addTaskButton.IsEnabled = false;
                addTopicButton.IsEnabled = false;
                removeButton.IsEnabled = false;
                renameButton.IsEnabled = false;

                addTaskButton.Click += AddTaskButtonClick;
                addTopicButton.Click += AddTopicButtonClick;
                removeButton.Click += RemoveButtonClick;
                renameButton.Click += RenameButtonClick;
                declineChangesButton.Click += DeclineChangesButtonClick;
                finishButton.Click += FinishButtonClick;
            }
        }

        private void DeclineChangesButtonClick(object sender, RoutedEventArgs e)
        {
            GoToMenuWithoitSaving();
        }

        private void GoToMenuWithoitSaving()
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
            Rename(testTree.SelectedItem as TreeViewItem);
        }

        private void FinishButtonClick(object sender, RoutedEventArgs e)
        {
            FinishRedacting();
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            Remove(testTree.SelectedItem as TreeViewItem);
        }

        private void AddTopicButtonClick(object sender, RoutedEventArgs e)
        {
            AddTopic(testTree.SelectedItem as TreeViewItem);
        }

        private void AddTaskButtonClick(object sender, RoutedEventArgs e)
        {
            AddTask(testTree.SelectedItem as TreeViewItem);
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
            var selectedItem = testTree.SelectedItem as TreeViewItem;

            if (selectedItem.Header is Topic)
            {
                addTaskButton.IsEnabled = true;
                addTopicButton.IsEnabled = true;
                removeButton.IsEnabled = true;
                renameButton.IsEnabled = true;
            }
            else if (selectedItem.Header is Test)
            {
                addTaskButton.IsEnabled = false;
                addTopicButton.IsEnabled = true;
                removeButton.IsEnabled = false;
                renameButton.IsEnabled = true;
            }
            else
            {
                addTaskButton.IsEnabled = false;
                addTopicButton.IsEnabled = false;
                removeButton.IsEnabled = true;
                renameButton.IsEnabled = false;
            }

            SetReadctPage(selectedItem);
        }

        private void SetTestToTree()
        {
            var headItem = new TreeViewItem() { Header = test };
            testTree.Items.Add(headItem);

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

        private void SetReadctPage(TreeViewItem treeItem)
        {
            if (settingsGrid.Children.Count > 0)
            {
                settingsGrid.Children.RemoveAt(0);
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
                return;
            }

            var taskGrid = page.Content as Grid;
            page.Content = null;
            settingsGrid.Children.Add(taskGrid);
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

        private Test GetTempTest()
        {
            var option1 = "o1";
            var option2 = "o2";
            var option3 = "o3";
            var option4 = "o4";

            var optionList1 = new List<string>
            {
                option1,
                option2
            };

            var optionList2 = new List<string>
            {
                option3,
                option4
            };

            var task1 = new SingleChoice()
            {
                Options = optionList1,
                Question = "q1",
                RightAnswerIndex = 0,
                Mark = 1
            };

            var task2 = new SingleChoice()
            {
                Options = optionList2,
                Question = "q2",
                RightAnswerIndex = 0,
                Mark = 2
            };

            var subSubTopic = new Topic()
            {
                Name = "subsub"
            };

            var subTopic1 = new Topic()
            {
                Name = "subTopic",
                SubTopics = new List<Topic> { subSubTopic }
            };

            var subTopic2 = new Topic()
            {
                Name = "One task",
                Tasks = new List<Task> { task1 }
            };

            var topic1 = new Topic()
            {
                Name = "Only Tasks",
                Tasks = new List<Task> { task1, task2 }
            };

            var topic2 = new Topic()
            {
                Name = "Taks and topic",
                Tasks = new List<Task>() { task2 },
                SubTopics = new List<Topic> { subTopic1 }
            };

            var topic3 = new Topic()
            {
                Name = "Only topics",
                SubTopics = new List<Topic> { subTopic1, subTopic2 }
            };

            var test = new Test()
            {
                Name = "test",
                Topics = new List<Topic> { topic1, topic2, topic3 }
            };

            return test;
        }
    }
}
