using Lib.TaskTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для RedactMultipleChoicePage.xaml
    /// </summary>
    public partial class RedactMultipleChoicePage : Page
    {
        private MultipleChoice task;

        public RedactMultipleChoicePage(ref MultipleChoice task)
        {
            InitializeComponent();

            this.task = task;

            questionTextBox.Text = task.Question;

            foreach (var option in task.Options)
            {
                optionsList.Items.Add(new ComboBoxItem() { Content = option });
                rightOptionsSelector.Items.Add(new ComboBoxItem { Content = option });
            }
            SetAddNewOption();

            //rightOptionsSelector.SelectedIndex = task.RightAnswersIndexes[0];

            setQuestionButton.Click += SetQuestionButtonClick;
            setOptionButton.Click += SetOptionButtonClick;
            deleteOptionButton.Click += DeleteOptionButtonClick;

            optionTextBox.TextChanged += OptionTextBoxTextChanged;
            questionTextBox.TextChanged += QuestionTextBoxTextChanged;

            optionsList.SelectionChanged += OptionsListSelectionChanged;
            rightOptionsSelector.SelectionChanged += RightOptionSelectorSelectionChanged;

            setQuestionButton.IsEnabled = false;
            setOptionButton.IsEnabled = false;

            optionsList.SelectedIndex = 0;
        }

        private void QuestionTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            setQuestionButton.IsEnabled = true;
        }

        private void RightOptionSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetRightAnswerIndex();
        }

        private void OptionsListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (optionsList.SelectedIndex < task.Options.Count && optionsList.SelectedIndex != -1)
            {
                optionTextBox.Text = task.Options[optionsList.SelectedIndex];

                deleteOptionButton.IsEnabled = true;
            }
            else
            {
                deleteOptionButton.IsEnabled = false;

                optionTextBox.Text = string.Empty;
            }

            setOptionButton.IsEnabled = false;
        }

        private void OptionTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            setOptionButton.IsEnabled = true;
        }

        private void DeleteOptionButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteSelectedOption();
        }

        private void SetOptionButtonClick(object sender, RoutedEventArgs e)
        {
            SetOptionText();

            setOptionButton.IsEnabled = false;
        }

        private void SetQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            SetQuestionText();

            setQuestionButton.IsEnabled = false;
        }

        private void DeleteSelectedOption()
        {
            if (optionsList.SelectedIndex < task.Options.Count)
            {
                var selectedIndex = optionsList.SelectedIndex;

                var isSelectedRight = rightOptionsSelector.SelectedIndex == selectedIndex;

                optionsList.Items.RemoveAt(selectedIndex);
                rightOptionsSelector.Items.RemoveAt(selectedIndex);
                task.Options.RemoveAt(selectedIndex);

                if (selectedIndex - 1 >= 0)
                {
                    optionsList.SelectedIndex = selectedIndex - 1;

                    if (isSelectedRight)
                    {
                        rightOptionsSelector.SelectedIndex = selectedIndex - 1;
                    }
                }
                else
                {
                    optionsList.SelectedIndex = selectedIndex;

                    if (isSelectedRight)
                    {
                        rightOptionsSelector.SelectedIndex = selectedIndex;
                    }
                }
            }
        }

        private void SetOptionText()
        {
            var selectedIndex = optionsList.SelectedIndex;

            if (optionsList.SelectedIndex < task.Options.Count)
            {
                task.Options[selectedIndex] = optionTextBox.Text;

                rightOptionsSelector.Items[selectedIndex] = new ComboBoxItem() { Content = optionTextBox.Text };
                optionsList.Items[selectedIndex] = new ComboBoxItem() { Content = optionTextBox.Text };

                optionsList.SelectedIndex = selectedIndex;
                rightOptionsSelector.SelectedIndex = task.RightAnswersIndexes[0];
            }
            else
            {
                task.Options.Add(optionTextBox.Text);

                var newOptionText = optionTextBox.Text;

                optionsList.Items.RemoveAt(optionsList.SelectedIndex);

                optionsList.Items.Add(new ComboBoxItem() { Content = newOptionText });
                rightOptionsSelector.Items.Add(new ComboBoxItem() { Content = newOptionText });

                optionsList.SelectedIndex = selectedIndex;

                SetAddNewOption();
            }

            if (rightOptionsSelector.SelectedItem != null)
            {
                rightOptionsSelector.SelectedIndex = task.RightAnswersIndexes[0];
            }
            else
            {
                rightOptionsSelector.SelectedIndex = selectedIndex;
            }
        }

        private void SetQuestionText() => task.Question = questionTextBox.Text;

        private void SetAddNewOption()
        {
            optionsList.Items.Add(new ComboBoxItem() { Content = "Add new option" });
        }

        private void SetRightAnswerIndex() { } //task.RightAnswersIndexes[0] = rightOptionsSelector.SelectedIndex;
    }
}
