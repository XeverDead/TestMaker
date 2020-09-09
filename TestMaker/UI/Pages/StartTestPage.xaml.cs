﻿using System;
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
using UI.DialogWindows;

namespace UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartTestPage.xaml
    /// </summary>
    public partial class StartTestPage : Page
    {
        public StartTestPage()
        {
            InitializeComponent();

            var name = "Unknown";

            var dialog = new EnterNameWindow();
            if ((bool)dialog.ShowDialog())
            {
                name = dialog.EnteredName;
            }

            MessageBox.Show(name);
        }
    }
}