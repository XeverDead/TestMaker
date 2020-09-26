using Lib.TaskTypes;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace UI.Pages
{
    public partial class RedactMultipleChoicePage : Page
    {
        private readonly MultipleChoice task;

        public RedactMultipleChoicePage(ref MultipleChoice task)
        {
            InitializeComponent();

            this.task = task;

            MarkTextBox.Text = task.Mark.ToString(CultureInfo.InvariantCulture);

            QuestionTextBox.Text = task.Question;

            foreach (var option in task.Options)
            {
                OptionsList.Items.Add(new ComboBoxItem() { Content = option });

                var rightAnswerCheckBox = new CheckBox() { Content = option };
                if (task.RightAnswersIndexes.Contains(task.Options.IndexOf(option)))
                {
                    rightAnswerCheckBox.IsChecked = true;
                }

                rightAnswerCheckBox.Checked += RightAnswerCheckBoxChecked;
                rightAnswerCheckBox.Unchecked += RightAnswerCheckBoxUnchecked;

                RightOptionsSelector.Children.Add(rightAnswerCheckBox);
            }

            SetAddNewOption();

            SetQuestionButton.Click += SetQuestionButtonClick;
            SetOptionButton.Click += SetOptionButtonClick;
            DeleteOptionButton.Click += DeleteOptionButtonClick;
            SetMarkButton.Click += SetMarkButtonClick;

            OptionTextBox.TextChanged += OptionTextBoxTextChanged;
            QuestionTextBox.TextChanged += QuestionTextBoxTextChanged;
            MarkTextBox.TextChanged += MarkTextBoxTextChanged;

            OptionsList.SelectionChanged += OptionsListSelectionChanged;

            SetQuestionButton.IsEnabled = false;
            SetOptionButton.IsEnabled = false;
            SetMarkButton.IsEnabled = false;

            OptionsList.SelectedIndex = 0;
        }

        private void RightAnswerCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            RemoveRightAnswerIndex(RightOptionsSelector.Children.IndexOf(sender as CheckBox));
        }

        private void RightAnswerCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            AddRightAnswerIndex(RightOptionsSelector.Children.IndexOf(sender as CheckBox));
        }

        private void MarkTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(MarkTextBox.Text, out int mark) && mark >= 0) 
            {
                SetMarkButton.IsEnabled = true;
            }
            else
            {
                SetMarkButton.IsEnabled = false;
            }
        }

        private void SetMarkButtonClick(object sender, RoutedEventArgs e)
        {
            SetMark();

            SetMarkButton.IsEnabled = false;
        }

        private void SetMark()
        {
            if (int.TryParse(MarkTextBox.Text, out int mark))
            {
                task.Mark = mark;
            }
        }

        private void QuestionTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            SetQuestionButton.IsEnabled = true;
        }

        private void OptionsListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OptionsList.SelectedIndex < task.Options.Count && OptionsList.SelectedIndex != -1)
            {
                OptionTextBox.Text = task.Options[OptionsList.SelectedIndex];

                DeleteOptionButton.IsEnabled = true;
            }
            else
            {
                DeleteOptionButton.IsEnabled = false;

                OptionTextBox.Text = string.Empty;
            }

            SetOptionButton.IsEnabled = false;
        }

        private void OptionTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            SetOptionButton.IsEnabled = true;
        }

        private void DeleteOptionButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteSelectedOption();
        }

        private void SetOptionButtonClick(object sender, RoutedEventArgs e)
        {
            SetOptionText();

            SetOptionButton.IsEnabled = false;
        }

        private void SetQuestionButtonClick(object sender, RoutedEventArgs e)
        {
            SetQuestionText();

            SetQuestionButton.IsEnabled = false;
        }

        private void DeleteSelectedOption()
        {
            if (OptionsList.SelectedIndex < task.Options.Count)
            {
                var selectedIndex = OptionsList.SelectedIndex;

                OptionsList.Items.RemoveAt(selectedIndex);
                task.Options.RemoveAt(selectedIndex);

                if (task.RightAnswersIndexes.Contains(selectedIndex))
                {
                    RemoveRightAnswerIndex(selectedIndex);
                }

                RemoveRightOptionCheckBox(selectedIndex);

                if (selectedIndex - 1 >= 0)
                {
                    OptionsList.SelectedIndex = selectedIndex - 1;
                }
                else
                {
                    OptionsList.SelectedIndex = selectedIndex;
                }
            }
        }

        private void SetOptionText()
        {
            var selectedIndex = OptionsList.SelectedIndex;

            if (OptionsList.SelectedIndex < task.Options.Count)
            {
                task.Options[selectedIndex] = OptionTextBox.Text;

                AddRightOptionCheckBox(task.Options[selectedIndex]);

                OptionsList.Items[selectedIndex] = new ComboBoxItem() { Content = OptionTextBox.Text };

                OptionsList.SelectedIndex = selectedIndex;
            }
            else
            {
                task.Options.Add(OptionTextBox.Text);

                var newOptionText = OptionTextBox.Text;

                OptionsList.Items.RemoveAt(OptionsList.SelectedIndex);

                AddRightOptionCheckBox(task.Options[selectedIndex]);

                OptionsList.Items.Add(new ComboBoxItem() { Content = newOptionText });

                OptionsList.SelectedIndex = selectedIndex;

                SetAddNewOption();
            }
        }

        private void SetQuestionText() => task.Question = QuestionTextBox.Text;

        private void SetAddNewOption()
        {
            OptionsList.Items.Add(new ComboBoxItem() { Content = "Add new option" });
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

            RightOptionsSelector.Children.Add(checkBox);

            RefreshRightAnswersIndexes();
        }

        private void RemoveRightOptionCheckBox(int index)
        {
            RightOptionsSelector.Children.RemoveAt(index);

            RefreshRightAnswersIndexes();
        }

        private void RefreshRightAnswersIndexes()
        {
            task.RightAnswersIndexes.Clear();

            foreach (CheckBox rightOptionCheckBox in RightOptionsSelector.Children)
            {
                if (rightOptionCheckBox.IsChecked == true)
                {
                    AddRightAnswerIndex(RightOptionsSelector.Children.IndexOf(rightOptionCheckBox));
                }
            }
        }
    }
}
