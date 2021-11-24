using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());
        private AddressManager addressManager = new AddressManager(new AddressRepository());

        private ObservableCollection<Vehicle> vehicles = new ObservableCollection<Vehicle>();
        private ObservableCollection<Driver> drivers = new ObservableCollection<Driver>();
        private ObservableCollection<FuelCard> fuelCards = new ObservableCollection<FuelCard>();

        public MainWindow()
        {
            InitializeComponent();
            vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetAllVehicles());
            VehiclesDataGrid.ItemsSource = vehicles;

        }

        private void SearchVehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            

        }


        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Vehicle vehicle = (Vehicle)VehiclesDataGrid.SelectedItem;
            VehicleDetailsWindow objWindow = new VehicleDetailsWindow(vehicle.VehicleId);
            objWindow.Show();
        }

        private void NewDriverButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewDriver addNewDriver = new AddNewDriver();
            addNewDriver.Show();
        }

        private void NewVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewVehicle addNewVehicle = new AddNewVehicle();
            addNewVehicle.Show();
        }

        private void NewFuelCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewFuelCard addNewFuelCard = new AddNewFuelCard();
            addNewFuelCard.Show();
        }
    }
}
