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
    /// Логика взаимодействия для EnterNameWindow.xaml
    /// </summary>
    public partial class EnterNameWindow : Window
    {
        public EnterNameWindow()
        {
            InitializeComponent();
        }

        public string EnteredName
        {
            get => nameBox.Text;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
