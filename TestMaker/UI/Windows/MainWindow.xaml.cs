using System.Windows;

namespace UI.Windows
{
    public partial class MainWindow : Window
    {
        private TestActions action;

        public MainWindow()
        {
            InitializeComponent();

            PassButton.Click += PassButtonClick;
            RedactButton.Click += RedactButtonClick;
            ViewResultsButton.Click += ViewResultsButtonClick;
            ExitButton.Click += ExitButtonClick;
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
