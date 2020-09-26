using Core;
using Lib;
using Lib.ResultTypes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UI.DialogWindows;

namespace UI.Windows
{
    public enum TestActions
    {
        PassTest,
        RedactTest,
        ViewResult
    }

    public partial class HubWindow : Window
    {
        private readonly List<string> filePaths;
        private readonly TestActions action;

        public HubWindow(TestActions action)
        {
            InitializeComponent();

            this.action = action;

            var hubCore = new HubCore();

            if (action == TestActions.ViewResult)
            {
                filePaths = new List<string>(hubCore.GetFiles<TestResult>());
            }
            else
            {
                filePaths = new List<string>(hubCore.GetFiles<Test>());
            }

            foreach (var filePath in filePaths)
            {
                PathsList.Items.Add(new ListBoxItem() { Content = filePath.Substring(filePath.LastIndexOf('\\') + 1) });
            }

            ChoosePathButton.Click += ChoosePathButtonClick;
            AddNewTestButton.Click += AddNewTestButtonClick;
            BackToMenuButton.Click += BackToMenuButtonClick;

            PathsList.SelectionChanged += PathsListSelectionChanged;

            ChoosePathButton.IsEnabled = false;

            if (action != TestActions.RedactTest)
            {
                AddNewTestButton.IsEnabled = false;
            }
        }

        private void BackToMenuButtonClick(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();

            Close();
            mainWindow.Show();
        }

        private void AddNewTestButtonClick(object sender, RoutedEventArgs e)
        {
            AddNewTest();
        }

        private void PathsListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChoosePathButton.IsEnabled = true;
        }

        private void ChoosePathButtonClick(object sender, RoutedEventArgs e)
        {
            if (action == TestActions.PassTest)
            {
                AskToStartTest();
            }
            else if (action == TestActions.ViewResult)
            {
                AskToViewResult();
            }
            else
            {
                AskToRedactTest();
            }
        }

        private void AskToStartTest()
        {
            var result = MessageBox.Show("Would you like to start this test?", "Start test", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var textInputWindow = new TextInputWindow("Enter your name");
                textInputWindow.ShowDialog();

                if (textInputWindow.DialogResult == true)
                {
                    var studentName = textInputWindow.EnteredText;

                    var testPath = filePaths[PathsList.SelectedIndex];
                    var testDirectory = testPath.Substring(0, testPath.LastIndexOf('\\'));

                    var passingWindow = new PassingWindow(testPath, $"{testDirectory}\\Results\\{studentName}.tmr", false, studentName);

                    if (passingWindow.IsLoadedProperly)
                    {
                        passingWindow.Show();
                        Close();
                    }
                }
            }
        }

        private void AskToViewResult()
        {
            var result = MessageBox.Show("Would you like to view this result?", "View result", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var resultPath = filePaths[PathsList.SelectedIndex];
                
                var resultWindow = new PassingWindow(resultPath, resultPath, true, null);

                if (resultWindow.IsLoadedProperly)
                {
                    resultWindow.Show();
                    Close();
                }
            }
        }

        private void AskToRedactTest()
        {
            var result = MessageBox.Show("Would you like to redact this test?", "Redact test", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var resultPath = filePaths[PathsList.SelectedIndex];

                var redactingWindow = new RedactingWindow(resultPath, false);

                if (redactingWindow.IsLoadedProperly)
                {
                    redactingWindow.Show();
                    Close();
                }
            }
        }

        private void AddNewTest()
        {
            var textInputWindow = new TextInputWindow("Enter the name of the test");
            textInputWindow.ShowDialog();

            if (textInputWindow.DialogResult == true)
            {
                var testName = textInputWindow.EnteredText;

                var pathToTest = $"Tests\\{testName}\\{testName}.tmt";
                var redactingWindow = new RedactingWindow(pathToTest, true);

                redactingWindow.Show();
                Close();
            }
        }
    }
}
