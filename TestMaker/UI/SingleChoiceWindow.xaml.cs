using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для SingleChoiceWindow.xaml
    /// </summary>
    public partial class SingleChoiceWindow : Window
    {
        public SingleChoiceWindow()
        {
            InitializeComponent();
        }

        private void AddOptionButton(int num)
        {
            if (optionGrid.Children.Count % 4 == 0 && optionGrid.Children.Count > 0) 
            {
                optionGrid.Rows++;
            }

            var optionButton = new Button()
            {
                Name = "button" + num               
            };
            optionButton.Click += OptionButtonClick;

            optionGrid.Children.Add(optionButton);
        }

        private void OptionButtonClick(object sender, RoutedEventArgs e)
        {
            var optionButton = sender as Button;
            ShowRightAnswer(1,2);
        }

        private void ShowRightAnswer(int rightIndex, int wrongIndex)
        {
            foreach (var child in optionGrid.Children)
            {
                if (child is Button)
                {
                    ((Button)child).IsEnabled = false;
                }
            }

            if ((optionGrid.Children[rightIndex] is Button) && (optionGrid.Children[wrongIndex] is Button))
            {
                ((Button)optionGrid.Children[rightIndex]).Background = new SolidColorBrush(Color.FromArgb(200, 0, 200, 0));
                ((Button)optionGrid.Children[wrongIndex]).Background = new SolidColorBrush(Color.FromArgb(200, 200, 0, 0));
            }
        }

        private void OpenNextWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
