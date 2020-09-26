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
    /// Логика взаимодействия для RedactSingleChoicePage.xaml
    /// </summary>
    public partial class RedactSingleChoicePage : Page
    {
        private SingleChoice task;

        public RedactSingleChoicePage(ref SingleChoice task)
        {
            InitializeComponent();

            this.task = task;

            markTextBox.Text = task.Mark.ToString();

            questionTextBox.Text = task.Question;

            foreach (var option in task.Options)
            {
                optionsList.Items.Add(new ComboBoxItem() { Content = option });
                rightOptionSelector.Items.Add(new ComboBoxItem { Content = option });
            }
            SetAddNewOption();

            rightOptionSelector.SelectedIndex = task.RightAnswerIndex;

            setQuestionButton.Click += SetQuestionButtonClick;
            setOptionButton.Click += SetOptionButtonClick;
            deleteOptionButton.Click += DeleteOptionButtonClick;
            setMarkButton.Click += SetMarkButtonClick;

            optionTextBox.TextChanged += OptionTextBoxTextChanged;
            questionTextBox.TextChanged += QuestionTextBoxTextChanged;
            markTextBox.TextChanged += MarkTextBoxTextChanged;

            optionsList.SelectionChanged += OptionsListSelectionChanged;
            rightOptionSelector.SelectionChanged += RightOptionSelectorSelectionChanged;

            setQuestionButton.IsEnabled = false;
            setOptionButton.IsEnabled = false;
            setMarkButton.IsEnabled = false;

            optionsList.SelectedIndex = 0;
        }

        private void MarkTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(markTextBox.Text, out _))
            {
                setMarkButton.IsEnabled = true;
            }
            else
            {
                setMarkButton.IsEnabled = false;
            }
        }

        private void SetMarkButtonClick(object sender, RoutedEventArgs e)
        {
            SetMark();

            setMarkButton.IsEnabled = false;
        }

        private void SetMark()
        {
            if (int.TryParse(markTextBox.Text, out int mark))
            {
                task.Mark = mark;
            }
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

                var isSelectedRight = rightOptionSelector.SelectedIndex == selectedIndex;

                optionsList.Items.RemoveAt(selectedIndex);
                rightOptionSelector.Items.RemoveAt(selectedIndex);
                task.Options.RemoveAt(selectedIndex);

                if (selectedIndex - 1 >= 0)
                {
                    optionsList.SelectedIndex = selectedIndex - 1;

                    if (isSelectedRight)
                    {
                        rightOptionSelector.SelectedIndex = selectedIndex - 1;
                    }
                }
                else
                {
                    optionsList.SelectedIndex = selectedIndex;

                    if (isSelectedRight)
                    {
                        rightOptionSelector.SelectedIndex = selectedIndex;
                    }
                }

                SetRightAnswerIndex();
            }
        }

        private void SetOptionText()
        {
            var selectedIndex = optionsList.SelectedIndex;

            if (optionsList.SelectedIndex < task.Options.Count)
            {
                task.Options[selectedIndex] = optionTextBox.Text;

                rightOptionSelector.Items[selectedIndex] = new ComboBoxItem() { Content = optionTextBox.Text };
                optionsList.Items[selectedIndex] = new ComboBoxItem() { Content = optionTextBox.Text };

                optionsList.SelectedIndex = selectedIndex;
                rightOptionSelector.SelectedIndex = task.RightAnswerIndex;
            }
            else
            {
                task.Options.Add(optionTextBox.Text);

                var newOptionText = optionTextBox.Text;

                optionsList.Items.RemoveAt(optionsList.SelectedIndex);

                optionsList.Items.Add(new ComboBoxItem() { Content = newOptionText });
                rightOptionSelector.Items.Add(new ComboBoxItem() { Content = newOptionText });

                optionsList.SelectedIndex = selectedIndex;

                SetAddNewOption();
            }

            if (rightOptionSelector.SelectedItem != null)
            {
                rightOptionSelector.SelectedIndex = task.RightAnswerIndex;
            }
            else
            {
                rightOptionSelector.SelectedIndex = selectedIndex;
            }
        }

        private void SetQuestionText() => task.Question = questionTextBox.Text;

        private void SetAddNewOption()
        {
            optionsList.Items.Add(new ComboBoxItem() { Content = "Add new option" });
        }

        private void SetRightAnswerIndex() => task.RightAnswerIndex = rightOptionSelector.SelectedIndex;
    }
}
