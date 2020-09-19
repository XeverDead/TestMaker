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

        public RedactSingleChoicePage(SingleChoice task)
        {
            InitializeComponent();

            //this.task = task;

            //optionBox.Text = task.Question;

            //foreach (var option in task.Options)
            //{
            //    optionList.Items.Add(new ComboBoxItem() { Content = option });
            //}

            //optionList.Items.Add(new ComboBoxItem() { Content = "Add new" });
        }
    }
}
