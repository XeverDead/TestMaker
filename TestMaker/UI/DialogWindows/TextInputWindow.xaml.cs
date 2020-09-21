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

namespace UI.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для RenameWindow.xaml
    /// </summary>
    public partial class TextInputWindow : Window
    {
        public TextInputWindow()
        {
            InitializeComponent();

            okButton.Click += OkButtonClick;
            cancelButton.Click += CancelButtonClick;

            enteredTextBox.ForceCursor = true;
        }

        public string EnteredText
        {
            get
            {
                if (enteredTextBox.Text == null || string.IsNullOrWhiteSpace(enteredTextBox.Text))
                {
                    return "Unknown";
                }

                return enteredTextBox.Text;
            }
        }

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
