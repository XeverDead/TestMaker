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
            var passingWindow = new PassingWindow("D:\\Test", "D:\\Test", true);

            passingWindow.Show();
            Close();
        }

        private void PassButtonClick(object sender, RoutedEventArgs e)
        {
            var passingWindow = new PassingWindow("D:\\Test", "D:\\Test", false);

            passingWindow.Show();
            Close();
        }

        private void RedactButtonClick(object sender, RoutedEventArgs e)
        {
            var redactingWindow = new RedactingWindow("D:\\Test", false);

            redactingWindow.Show();
            Close();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
