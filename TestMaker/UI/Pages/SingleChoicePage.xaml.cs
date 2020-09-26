using Lib;
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
using System.Windows.Threading;
using UI.Pages;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для SingleChoicePage.xaml
    /// </summary>
    public partial class SingleChoicePage : Page, ITaskPage
    {
        private Color chosenOptionColor = Color.FromArgb(200, 230, 230, 0);
        private Color chosenRightOptionColor = Color.FromArgb(200, 0, 200, 0);
        private Color chosenWrongOptionColor = Color.FromArgb(255, 255, 0, 0);
        private Color notChosenRightOptionColor = Color.FromArgb(200, 100, 0, 200);

        public dynamic Answer { get; protected set; }
        public bool IsAnswerChosen { get; protected set; }

        private List<Button> optionButtons;

        private bool isResultPage;

        public SingleChoicePage(SingleChoice task)
        {
            InitializeComponent();

            optionButtons = new List<Button>();

            isResultPage = false;

            questionBlock.Text = task.Question;

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }
        }

        public SingleChoicePage(SingleChoice task, int chosenButtonIndex)
            : this(task)
        {
            optionButtons[chosenButtonIndex].Background = new SolidColorBrush(chosenOptionColor);
        }

        public SingleChoicePage(SingleChoice task, int chosenButtonIndex, int rightAnswerIndex)
        {
            InitializeComponent();

            optionButtons = new List<Button>();

            isResultPage = true;

            questionBlock.Text = task.Question;

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }

            if (chosenButtonIndex >= 0)
            {
                if (chosenButtonIndex == rightAnswerIndex)
                {
                    optionButtons[chosenButtonIndex].Background = new SolidColorBrush(chosenRightOptionColor);
                }
                else
                {
                    optionButtons[chosenButtonIndex].Background = new SolidColorBrush(chosenWrongOptionColor);
                    optionButtons[rightAnswerIndex].Background = new SolidColorBrush(notChosenRightOptionColor);
                }
            }
            else
            {
                optionButtons[rightAnswerIndex].Background = new SolidColorBrush(notChosenRightOptionColor);
            }
        }

        private void AddOption(int index, string content)
        {
            var button = new Button()
            {
                Name = $"option{index}",
                Content = content           
            };

            if (!isResultPage)
            {
                button.Click += OptionButtonClick;
            }

            optionButtons.Add(button);

            if (optionsGrid.Children.Count % 4 == 0)
            {
                optionsGrid.Rows++;
            }

            optionsGrid.Children.Add(button);
        }

        private void OptionButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var optionButton in optionButtons)
            {
                optionButton.Background = SystemColors.ControlLightBrush;
            }

            IsAnswerChosen = true;

            var button = sender as Button;
            Answer = optionButtons.IndexOf(button);

            button.Background = new SolidColorBrush(chosenOptionColor);
        }
    }
}
