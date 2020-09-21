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
using System.Windows.Shapes;

namespace UI.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для ChooseTaskTypeWindow.xaml
    /// </summary>
    public partial class ChooseTaskTypeWindow : Window
    {
        private List<Type> taskTypes;
        public Type ChosenType { get => taskTypes[typesBox.SelectedIndex] as Type; }

        public ChooseTaskTypeWindow(List<Type> taskTypes)
        {
            InitializeComponent();

            this.taskTypes = taskTypes;

            foreach (var type in taskTypes)
            {
                typesBox.Items.Add(type.Name);
            }

            typesBox.SelectedIndex = 0;

            okButton.Click += OkButtonClick;
            cancelButton.Click += CancelButtonClick;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
