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

namespace TourPlanner
{
    /// <summary>
    /// Interaktionslogik für TourLogs.xaml
    /// </summary>
    public partial class TourLogs : Window
    {
        public TourLogs()
        {
            InitializeComponent();
        }

        private void CreateTourLogButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTourLog createLogWindow = new CreateTourLog();
            createLogWindow.Show();

        }

        private void ModifyTourLogButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyTourLog modifyLogWindow = new ModifyTourLog();
            modifyLogWindow.Show();

        }
    }
}
