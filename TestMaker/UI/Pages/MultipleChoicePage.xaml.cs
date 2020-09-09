﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lib.TaskTypes;

namespace UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для MultipleChoicePage.xaml
    /// </summary>
    public partial class MultipleChoicePage : Page
    {
        public List<int> ChosenOptionsIndexes { get; protected set; }

        public MultipleChoicePage(MultipleChoice task)
        {
            InitializeComponent();

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }
        }

        private void AddOption(int index, string content)
        {           
            var button = new ToggleButton()
            {
                Name = $"option{index}",
                Content = content
            };
            button.Checked += OptionButtonChecked;
            button.Unchecked += OptionButtonUnchecked;

            if (optionsGrid.Children.Count % 4 == 0)
            {
                optionsGrid.Rows++;
            }

            optionsGrid.Children.Add(button);
        }

        private void OptionButtonUnchecked(object sender, RoutedEventArgs e)
        {
            var optionButton = sender as ToggleButton;
            optionButton.Background = SystemColors.ControlLightBrush;

            var buttonIndex = Convert.ToInt32(optionButton.Name.Substring(optionButton.Name.Length - 1));
            ChosenOptionsIndexes.Remove(buttonIndex);
        }

        private void OptionButtonChecked(object sender, RoutedEventArgs e)
        {
            var optionButton = sender as ToggleButton;
            optionButton.Background = SystemColors.ControlLightBrush;

            var buttonIndex = Convert.ToInt32(optionButton.Name.Substring(optionButton.Name.Length - 1));
            ChosenOptionsIndexes.Add(buttonIndex);
        }

        public Grid GetAsGrid()
        {
            var mainGrid = Content as Grid;
            Content = null;
            return mainGrid;
        }
    }
}