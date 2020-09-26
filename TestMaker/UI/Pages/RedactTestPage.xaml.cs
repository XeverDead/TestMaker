using Lib;
using System.Windows;
using System.Windows.Controls;

namespace UI.Pages
{
    public partial class RedactTestPage : Page
    {
        private readonly Test test;

        public RedactTestPage(ref Test test)
        {
            InitializeComponent();

            this.test = test;

            TimeTextBox.Text = test.Time.ToString();
            PasswordTextBox.Text = test.Password;

            TimeTextBox.TextChanged += TimeTextBoxTextChanged;
            PasswordTextBox.TextChanged += PasswordTextBoxTextChanged;
            SetTimeButton.Click += SetTimeButtonClick;
            SetPasswordButton.Click += SetPasswordButtonClick;

            SetTimeButton.IsEnabled = false;
            SetPasswordButton.IsEnabled = false;
        }

        private void PasswordTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            SetPasswordButton.IsEnabled = true;
        }

        private void SetPasswordButtonClick(object sender, RoutedEventArgs e)
        {
            test.Password = PasswordTextBox.Text;

            SetPasswordButton.IsEnabled = false;
        }

        private void TimeTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TimeTextBox.Text, out int time) && time >= 0) 
            {
                SetTimeButton.IsEnabled = true;
            }
            else
            {
                SetTimeButton.IsEnabled = false;
            }
        }

        private void SetTimeButtonClick(object sender, RoutedEventArgs e)
        {
            SetTime();

            SetTimeButton.IsEnabled = false;
        }

        private void SetTime()
        {
            if (int.TryParse(TimeTextBox.Text, out int time)) 
            {
                test.Time = time;
            }
        }
    }
}
