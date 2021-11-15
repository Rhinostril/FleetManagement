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
using FleetManagement.Business.Entities;
using FleetManagement.Business.Interfaces;
using FleetManagement.Business.Managers;
using FleetManagement.Data.Repositories;

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private VehicleManager vehicleManager = new VehicleManager(new VehicleRepository());
        private DriverManager driverManager = new DriverManager(new DriverRepository());

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchVehiclesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
