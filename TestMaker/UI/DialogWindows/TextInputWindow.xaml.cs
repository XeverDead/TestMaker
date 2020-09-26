using System.Windows;

namespace UI.DialogWindows
{
    public partial class TextInputWindow : Window
    {
        public TextInputWindow(string header)
        {
            InitializeComponent();

            OkButton.Click += OkButtonClick;
            CancelButton.Click += CancelButtonClick;

            if (!(header == null || string.IsNullOrWhiteSpace(header)))
            {
                Title = header;
            }

            EnteredTextBox.ForceCursor = true;
        }

        public string EnteredText => string.IsNullOrWhiteSpace(EnteredTextBox.Text) ? "Unknown" : EnteredTextBox.Text;

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
