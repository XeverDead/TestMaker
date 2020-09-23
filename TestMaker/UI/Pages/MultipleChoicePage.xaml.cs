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
using Lib;
using Lib.TaskTypes;

namespace UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для MultipleChoicePage.xaml
    /// </summary>
    public partial class MultipleChoicePage : Page, ITaskPage
    {
        private Color chosenOptionColor = Color.FromArgb(200, 200, 200, 0);
        private Color chosenRightOptionColor = Color.FromArgb(200, 0, 200, 0);
        private Color chosenWrongOptionColor = Color.FromArgb(255, 255, 0, 0);
        private Color notChosenRightOptionColor = Color.FromArgb(200, 100, 0, 200);

        public dynamic Answer { get; protected set; }
        public bool IsAnswerChosen { get; protected set; }

        private List<ToggleButton> optionButtons;

        private bool isResultPage;

        public MultipleChoicePage(MultipleChoice task)
        {
            InitializeComponent();

            optionButtons = new List<ToggleButton>();

            isResultPage = false;

            questionBlock.Text = task.Question;

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }
        }

        public MultipleChoicePage(MultipleChoice task, List<int> chosenButtonIndexes)
            : this(task)
        {
            foreach (var index in chosenButtonIndexes)
            {
                optionButtons[index].Background = new SolidColorBrush(chosenOptionColor);
            }
        }

        public MultipleChoicePage(MultipleChoice task, List<int> chosenButtonIndexes, List<int> rightAnswersIndexes)
        {
            InitializeComponent();

            optionButtons = new List<ToggleButton>();

            isResultPage = true;

            questionBlock.Text = task.Question;

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }

            for (var index = 0; index < optionButtons.Count; index++)
            {
                if (chosenButtonIndexes.Contains(index) && rightAnswersIndexes.Contains(index))
                {
                    optionButtons[index].Background = new SolidColorBrush(chosenRightOptionColor);
                }
                else if (!chosenButtonIndexes.Contains(index) && rightAnswersIndexes.Contains(index))
                {
                    optionButtons[index].Background = new SolidColorBrush(notChosenRightOptionColor);
                }
                else if (chosenButtonIndexes.Contains(index) && !rightAnswersIndexes.Contains(index))
                {
                    optionButtons[index].Background = new SolidColorBrush(chosenWrongOptionColor);
                }
            }
        }

        private void AddOption(int index, string content)
        {           
            var button = new ToggleButton()
            {
                Name = $"option{index}",
                Content = content
            };

            if (!isResultPage)
            {
                button.Checked += OptionButtonChecked;
                button.Unchecked += OptionButtonUnchecked;
            }

            optionButtons.Add(button);

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

            var buttonIndex = optionButtons.IndexOf(optionButton);
            Answer.Remove(buttonIndex);

            if (Answer.Count == 0)
            {
                IsAnswerChosen = false;
            }
        }

        private void OptionButtonChecked(object sender, RoutedEventArgs e)
        {
            var optionButton = sender as ToggleButton;
            optionButton.Background = new SolidColorBrush(chosenOptionColor);

            var buttonIndex = optionButtons.IndexOf(optionButton);
            Answer.Add(buttonIndex);

            IsAnswerChosen = true;
        }
    }
}
