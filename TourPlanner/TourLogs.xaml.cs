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

        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
            //CreateTour createTourWindow = new CreateTour();
            //createTourWindow.Show();

        }

        private void ModifyTourButton_Click(object sender, RoutedEventArgs e)
        {
            //ModifyTour createTourWindow = new ModifyTour();
            //createTourWindow.Show();

        }
    }
}
