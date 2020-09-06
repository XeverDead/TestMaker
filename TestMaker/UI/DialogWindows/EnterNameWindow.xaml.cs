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
using System.Windows.Shapes;

namespace UI.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для EnterNameWindow.xaml
    /// </summary>
    public partial class EnterNameWindow : Window
    {
        public EnterNameWindow()
        {
            InitializeComponent();

            Build();
        }

        public string EnteredName
        {
            get
            {
                var name = "Unknown";

                var grid = Content as Grid;
                foreach (var element in grid.Children)
                {
                    if (element is TextBox textBox)
                    {
                        name = textBox.Text;
                    }
                }

                return name;
            }
        }

        private void Build()
        {
            Width = 400;
            Height = 200;

            var mainGrid = new Grid()
            {
                Name = "mainGrid"
            };

            for (var counter = 0; counter < 3; counter++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
            }

            var label = new Label()
            {
                Name = "label",
                Content = "Enter your name"
            };
            Grid.SetRow(label, 0);

            var textBox = new TextBox()
            {
                Name = "textBox"
            };
            Grid.SetRow(textBox, 1);

            var okButton = new Button()
            {
                Name = "okButton",
                Content = "Ok"
            };
            Grid.SetRow(okButton, 2);

            okButton.Click += OkButtonClick;

            mainGrid.Children.Add(label);
            mainGrid.Children.Add(textBox);
            mainGrid.Children.Add(okButton);

            AddChild(mainGrid);
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
