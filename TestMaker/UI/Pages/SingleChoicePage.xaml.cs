using Lib.TaskTypes;
using System;
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

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для SingleChoicePage.xaml
    /// </summary>
    public partial class SingleChoicePage : Page
    {
        public int ChosenOption { get; protected set; }
        public bool IsOptionChosen { get; protected set; }
        private SingleChoice task;

        public SingleChoicePage(SingleChoice task)
        {
            InitializeComponent();

            this.task = task;

            questionBlock.Text = task.Question;

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }
        }   

        private void AddOption(int index, string content)
        {
            var button = new Button()
            {
                Name = $"option{index}",
                Content = content           
            };

            button.Click += OptionButtonClick;

            if (optionsGrid.Children.Count % 4 == 0)
            {
                optionsGrid.Rows++;
            }

            optionsGrid.Children.Add(button);
        }

        private void OptionButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var element in optionsGrid.Children)
            {
                var optionButton = element as Button;
                optionButton.Background = SystemColors.ControlLightBrush;
            }

            IsOptionChosen = true;

            var button = sender as Button;
            ChosenOption = Convert.ToInt32(button.Name.Substring(button.Name.Length - 1));

            button.Background = new SolidColorBrush(Colors.Red);
        }

        public Grid GetAsGrid()
        {
            var mainGrid = Content as Grid;
            Content = null;
            return mainGrid;
        }
    }
}
