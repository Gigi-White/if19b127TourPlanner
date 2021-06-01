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



namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateTourButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTour createTourWindow = new CreateTour();
            createTourWindow.Show();

        }

        private void ModifyTourButton_Click(object sender, RoutedEventArgs e)
        {
            ModifyTour createTourWindow = new ModifyTour();
            createTourWindow.Show();

        }

        private void ShowLogsButton_Click(object sender, RoutedEventArgs e)
        {
            TourLogs tourLogsWindow = new TourLogs();
            tourLogsWindow.Show();
        }
        private void ImportTourButton_Click(object sender, RoutedEventArgs e)
        {
            ImportTour importTourWindow = new ImportTour();
            importTourWindow.Show();
        }

        private void listTourNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
