using System;
using System.Collections.Generic;
using System.Windows;

namespace UI.DialogWindows
{
    public partial class ChooseTaskTypeWindow : Window
    {
        private readonly List<Type> taskTypes;
        public Type ChosenType => taskTypes[TypesBox.SelectedIndex] as Type;

        public ChooseTaskTypeWindow(List<Type> taskTypes)
        {
            InitializeComponent();

            this.taskTypes = taskTypes;

            foreach (var type in taskTypes)
            {
                TypesBox.Items.Add(type.Name);
            }

            TypesBox.SelectedIndex = 0;

            OkButton.Click += OkButtonClick;
            CancelButton.Click += CancelButtonClick;
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
