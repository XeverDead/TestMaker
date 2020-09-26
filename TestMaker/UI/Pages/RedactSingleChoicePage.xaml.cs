using Lib.TaskTypes;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace UI.Pages
{
    public partial class RedactSingleChoicePage : Page
    {
        private readonly SingleChoice task;

        public RedactSingleChoicePage(ref SingleChoice task)
        {
            InitializeComponent();

            this.task = task;

            MarkTextBox.Text = task.Mark.ToString(CultureInfo.InvariantCulture);

            QuestionTextBox.Text = task.Question;

            foreach (var option in task.Options)
            {
                OptionsList.Items.Add(new ComboBoxItem() { Content = option });
                RightOptionSelector.Items.Add(new ComboBoxItem { Content = option });
            }
            SetAddNewOption();

            RightOptionSelector.SelectedIndex = task.RightAnswerIndex;

            SetQuestionButton.Click += SetQuestionButtonClick;
            SetOptionButton.Click += SetOptionButtonClick;
            DeleteOptionButton.Click += DeleteOptionButtonClick;
            SetMarkButton.Click += SetMarkButtonClick;

            OptionTextBox.TextChanged += OptionTextBoxTextChanged;
            QuestionTextBox.TextChanged += QuestionTextBoxTextChanged;
            MarkTextBox.TextChanged += MarkTextBoxTextChanged;

            OptionsList.SelectionChanged += OptionsListSelectionChanged;
            RightOptionSelector.SelectionChanged += RightOptionSelectorSelectionChanged;

            SetQuestionButton.IsEnabled = false;
            SetOptionButton.IsEnabled = false;
            SetMarkButton.IsEnabled = false;

            OptionsList.SelectedIndex = 0;
        }

        private void MarkTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            SetMarkButton.IsEnabled = int.TryParse(MarkTextBox.Text, out _);
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

        private void RightOptionSelectorSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetRightAnswerIndex();
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

                var isSelectedRight = RightOptionSelector.SelectedIndex == selectedIndex;

                OptionsList.Items.RemoveAt(selectedIndex);
                RightOptionSelector.Items.RemoveAt(selectedIndex);
                task.Options.RemoveAt(selectedIndex);

                if (selectedIndex - 1 >= 0)
                {
                    OptionsList.SelectedIndex = selectedIndex - 1;

                    if (isSelectedRight)
                    {
                        RightOptionSelector.SelectedIndex = selectedIndex - 1;
                    }
                }
                else
                {
                    OptionsList.SelectedIndex = selectedIndex;

                    if (isSelectedRight)
                    {
                        RightOptionSelector.SelectedIndex = selectedIndex;
                    }
                }

                SetRightAnswerIndex();
            }
        }

        private void SetOptionText()
        {
            var selectedIndex = OptionsList.SelectedIndex;

            if (OptionsList.SelectedIndex < task.Options.Count)
            {
                task.Options[selectedIndex] = OptionTextBox.Text;

                RightOptionSelector.Items[selectedIndex] = new ComboBoxItem() { Content = OptionTextBox.Text };
                OptionsList.Items[selectedIndex] = new ComboBoxItem() { Content = OptionTextBox.Text };

                OptionsList.SelectedIndex = selectedIndex;
                RightOptionSelector.SelectedIndex = task.RightAnswerIndex;
            }
            else
            {
                task.Options.Add(OptionTextBox.Text);

                var newOptionText = OptionTextBox.Text;

                OptionsList.Items.RemoveAt(OptionsList.SelectedIndex);

                OptionsList.Items.Add(new ComboBoxItem() { Content = newOptionText });
                RightOptionSelector.Items.Add(new ComboBoxItem() { Content = newOptionText });

                OptionsList.SelectedIndex = selectedIndex;

                SetAddNewOption();
            }

            if (RightOptionSelector.SelectedItem != null)
            {
                RightOptionSelector.SelectedIndex = task.RightAnswerIndex;
            }
            else
            {
                RightOptionSelector.SelectedIndex = selectedIndex;
            }
        }

        private void SetQuestionText() => task.Question = QuestionTextBox.Text;

        private void SetAddNewOption()
        {
            OptionsList.Items.Add(new ComboBoxItem() { Content = "Add new option" });
        }

        private void SetRightAnswerIndex() => task.RightAnswerIndex = RightOptionSelector.SelectedIndex;
    }
}
