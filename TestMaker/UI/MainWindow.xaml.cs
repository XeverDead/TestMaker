using Core;
using Lib.TaskTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using UI.Pages;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        { 
            InitializeComponent();

            AddChild(new StartTestPage());

            var core = new DefaultPassingCore("Test.tmt");

            var allTopics = new StringBuilder();

            foreach (var topic in core.AllTopics)
            {
                allTopics.Append(topic.Name + " ");
            }

            MessageBox.Show(allTopics.ToString());
        }
    }
}
