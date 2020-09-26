using Lib;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для RedactTestPage.xaml
    /// </summary>
    public partial class RedactTestPage : Page
    {
        private readonly Test _test;

        public RedactTestPage(ref Test test)
        {
            InitializeComponent();

            _test = test;

            timeTextBox.Text = test.Time.ToString();

            timeTextBox.TextChanged += TimeTextBoxTextChanged;
            setTimeButton.Click += SetTimeButtonClick;

            setTimeButton.IsEnabled = false;
        }

        private void TimeTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(timeTextBox.Text, out int time) && time >= 0) 
            {
                setTimeButton.IsEnabled = true;
            }
            else
            {
                setTimeButton.IsEnabled = false;
            }
        }

        private void SetTimeButtonClick(object sender, RoutedEventArgs e)
        {
            SetTime();

            setTimeButton.IsEnabled = false;
        }

        private void SetTime()
        {
            if (int.TryParse(timeTextBox.Text, out int time)) 
            {
                _test.Time = time;
            }
        }
    }
}
