using Lib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using UI.Pages;

namespace UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для RedactingWindow.xaml
    /// </summary>
    public partial class RedactingWindow : Window
    {
        public RedactingWindow()
        {
            InitializeComponent();

            var str = new RedactSingleChoicePage(new Lib.TaskTypes.SingleChoice());

            var content = str.Content as Grid;

            str.Content = null;
            settingsGrid.Children.Add(content);
        }

        private void TopicTreeSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }
    }
}
