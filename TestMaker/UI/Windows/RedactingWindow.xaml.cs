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

namespace UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для RedactingWindow.xaml
    /// </summary>
    public partial class RedactingWindow : Window
    {
        private DefaultRedactingCore core;
        private Test test;

        public RedactingWindow()
        {
            InitializeComponent();

            core = new DefaultRedactingCore(new JsonDataProvider<Test>("Test"), false);
            test = GetTempTest();

            //if (!isNewTest)
            {
                SetTestToTree();
            }

            topicTree.SelectedItemChanged += TopicTreeSelectedItemChanged;

            addTaskButton.IsEnabled = false;
            addTopicButton.IsEnabled = false;
            removeButton.IsEnabled = false;
            renameButton.IsEnabled = false;

            addTaskButton.Click += AddTaskButtonClick;
            addTopicButton.Click += AddTopicButtonClick;
            removeButton.Click += RemoveButtonClick;
            renameButton.Click += RenameButtonClick;
            finishButton.Click += FinishButtonClick;
        }

        private void RenameButtonClick(object sender, RoutedEventArgs e)
        {
            Rename(topicTree.SelectedItem as TreeViewItem);
        }

        private void FinishButtonClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RemoveButtonClick(object sender, RoutedEventArgs e)
        {
            Remove(topicTree.SelectedItem as TreeViewItem);
        }

        private void AddTopicButtonClick(object sender, RoutedEventArgs e)
        {
            AddTopic(topicTree.SelectedItem as TreeViewItem);
        }

        private void AddTaskButtonClick(object sender, RoutedEventArgs e)
        {
            AddTask(topicTree.SelectedItem as TreeViewItem);
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

        private void TopicTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = topicTree.SelectedItem as TreeViewItem;

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
            topicTree.Items.Add(headItem);

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

            var enterTextWindow = new TextInputWindow();

            testOrTopic.Name = enterTextWindow.EnteredText;

            RefreshTreeItem(ref treeItem);
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
