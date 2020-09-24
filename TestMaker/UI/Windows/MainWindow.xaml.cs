using Lib;
using Lib.ResultTypes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestActions action;

        public MainWindow()
        {
            InitializeComponent();

            passButton.Click += PassButtonClick;
            redactButton.Click += RedactButtonClick;
            viewResultsButton.Click += ViewResultsButtonClick;
            exitButton.Click += ExitButtonClick;
        }

        private void ViewResultsButtonClick(object sender, RoutedEventArgs e)
        {
            action = TestActions.ViewResult;

            MoveToHub();
        }

        private void PassButtonClick(object sender, RoutedEventArgs e)
        {
            action = TestActions.PassTest;

            MoveToHub();
        }

        private void RedactButtonClick(object sender, RoutedEventArgs e)
        {
            action = TestActions.RedactTest;

            MoveToHub();
        }

        private void MoveToHub()
        {
            var hubWindow = new HubWindow(action);

            Close();
            hubWindow.Show();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
