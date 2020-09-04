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
        public SingleChoicePage(SingleChoice task)
        {
            InitializeComponent();

            BuildBasis();

            for (var optionNum = 0; optionNum < task.Options.Count; optionNum++)
            {
                AddOption(optionNum, task.Options[optionNum]);
            }
        }

        private void BuildBasis()
        {
            var mainGrid = new Grid()
            {
                Name = "mainGrid"
            };

            for (var counter = 0; counter < 2; counter++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }

            var optionsGrid = new UniformGrid()
            {
                Name = "optionsGrid",
                Columns = 4
            };

            Grid.SetRow(optionsGrid, 1);
            mainGrid.Children.Add(optionsGrid);

            Content = mainGrid;
        }

        private void AddOption(int index, string content)
        {
            var optionsGrid = new UniformGrid();

            foreach (var element in ((Grid)Content).Children)
            {
                if ((element is UniformGrid uniGrid) && (uniGrid.Name == "optionsGrid"))
                {
                    optionsGrid = uniGrid;
                }
            }

            var button = new Button()
            {
                Name = $"option{index}",
                Content = content
            };

            if (optionsGrid.Children.Count % 4 == 0)
            {
                optionsGrid.Rows++;
            }

            optionsGrid.Children.Add(button);
        }
    }
}
