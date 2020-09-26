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

            markTextBox.Text = task.Mark.ToString();

            questionTextBox.Text = task.Question;

            foreach (var option in task.Options)
            {
                optionsList.Items.Add(new ComboBoxItem() { Content = option });

                var rightAnswerCheckBox = new CheckBox() { Content = option };
                if (task.RightAnswersIndexes.Contains(task.Options.IndexOf(option)))
                {
                    rightAnswerCheckBox.IsChecked = true;
                }

                rightAnswerCheckBox.Checked += RightAnswerCheckBoxChecked;
                rightAnswerCheckBox.Unchecked += RightAnswerCheckBoxUnchecked;

                rightOptionsSelector.Children.Add(rightAnswerCheckBox);
            }

            SetAddNewOption();

            setQuestionButton.Click += SetQuestionButtonClick;
            setOptionButton.Click += SetOptionButtonClick;
            deleteOptionButton.Click += DeleteOptionButtonClick;
            setMarkButton.Click += SetMarkButtonClick;

            optionTextBox.TextChanged += OptionTextBoxTextChanged;
            questionTextBox.TextChanged += QuestionTextBoxTextChanged;
            markTextBox.TextChanged += MarkTextBoxTextChanged;

            optionsList.SelectionChanged += OptionsListSelectionChanged;

            setQuestionButton.IsEnabled = false;
            setOptionButton.IsEnabled = false;
            setMarkButton.IsEnabled = false;

            optionsList.SelectedIndex = 0;
        }

        private void RightAnswerCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            RemoveRightAnswerIndex(rightOptionsSelector.Children.IndexOf(sender as CheckBox));
        }

        private void RightAnswerCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            AddRightAnswerIndex(rightOptionsSelector.Children.IndexOf(sender as CheckBox));
        }

        private void MarkTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(markTextBox.Text, out int mark) && mark >= 0) 
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

                optionsList.Items.RemoveAt(selectedIndex);
                task.Options.RemoveAt(selectedIndex);

                if (task.RightAnswersIndexes.Contains(selectedIndex))
                {
                    RemoveRightAnswerIndex(selectedIndex);
                }

                RemoveRightOptionCheckBox(selectedIndex);

                if (selectedIndex - 1 >= 0)
                {
                    optionsList.SelectedIndex = selectedIndex - 1;
                }
                else
                {
                    optionsList.SelectedIndex = selectedIndex;
                }
            }
        }

        private void SetOptionText()
        {
            var selectedIndex = optionsList.SelectedIndex;

            if (optionsList.SelectedIndex < task.Options.Count)
            {
                task.Options[selectedIndex] = optionTextBox.Text;

                AddRightOptionCheckBox(task.Options[selectedIndex]);

                optionsList.Items[selectedIndex] = new ComboBoxItem() { Content = optionTextBox.Text };

                optionsList.SelectedIndex = selectedIndex;
            }
            else
            {
                task.Options.Add(optionTextBox.Text);

                var newOptionText = optionTextBox.Text;

                optionsList.Items.RemoveAt(optionsList.SelectedIndex);

                AddRightOptionCheckBox(task.Options[selectedIndex]);

                optionsList.Items.Add(new ComboBoxItem() { Content = newOptionText });

                optionsList.SelectedIndex = selectedIndex;

                SetAddNewOption();
            }
        }

        private void SetQuestionText() => task.Question = questionTextBox.Text;

        private void SetAddNewOption()
        {
            optionsList.Items.Add(new ComboBoxItem() { Content = "Add new option" });
        }

        private void AddRightAnswerIndex(int index) 
        {
            task.RightAnswersIndexes.Add(index);
        } 

        private void RemoveRightAnswerIndex(int index)
        {
            task.RightAnswersIndexes.Remove(index);
        }

        private void AddRightOptionCheckBox(string optionText)
        {
            var checkBox = new CheckBox()
            {
                Content = optionText
            };
            checkBox.Checked += RightAnswerCheckBoxChecked;
            checkBox.Unchecked += RightAnswerCheckBoxUnchecked;

            rightOptionsSelector.Children.Add(checkBox);

            RefreshRightAnswersIndexes();
        }

        private void RemoveRightOptionCheckBox(int index)
        {
            rightOptionsSelector.Children.RemoveAt(index);

            RefreshRightAnswersIndexes();
        }

        private void RefreshRightAnswersIndexes()
        {
            task.RightAnswersIndexes.Clear();

            foreach (CheckBox rightOptionCheckBox in rightOptionsSelector.Children)
            {
                if (rightOptionCheckBox.IsChecked == true)
                {
                    AddRightAnswerIndex(rightOptionsSelector.Children.IndexOf(rightOptionCheckBox));
                }
            }
        }
    }
}
