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
using FleetManagement.UI.DetailForms;

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
            vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetLatestVehicles());
            VehiclesDataGrid.ItemsSource = vehicles;
            drivers = new ObservableCollection<Driver>(driverManager.GetLatestDrivers());
            DriversDataGrid.ItemsSource = drivers;
            fuelCards = new ObservableCollection<FuelCard>(fuelCardManager.GetLatestFuelcards());
            FuelCardsDataGrid.ItemsSource = fuelCards;
        }

        private void SearchVehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            

        }

        


        private void SearchFuelCardsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cardNr = txtFuelCardNumber.Text;
                DateTime? valDate = null;
                if(FuelCardValidityDatePicker.SelectedDate != null)
                {
                    valDate = FuelCardValidityDatePicker.SelectedDate;
                }
                fuelCards = new ObservableCollection<FuelCard>(fuelCardManager.SearchFuelCards(cardNr, valDate));
                FuelCardsDataGrid.ItemsSource = fuelCards;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }


        private void Details_Click(object sender, RoutedEventArgs e)
        {
            Vehicle vehicle = (Vehicle)VehiclesDataGrid.SelectedItem;
            VehicleDetailsWindow objWindow = new VehicleDetailsWindow(vehicle.VehicleId);
            objWindow.Show();
        }

        private void DriverDetails_Click(object sender, RoutedEventArgs e)
        {
            Driver driver = (Driver)DriversDataGrid.SelectedItem;
            DriverDetailWindow objWindow = new DriverDetailWindow(driver.DriverID);
            objWindow.Show();
        }
        private void FuelCardsDetails_Click(object sender, RoutedEventArgs e)
        {
            FuelCard fuelCard = (FuelCard)FuelCardsDataGrid.SelectedItem;
            FuelCardDetailsWindow objWindow = new FuelCardDetailsWindow(fuelCard.FuelCardId);
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

        private void UpdateVehicle_Click(object sender, RoutedEventArgs e)
        {
            Vehicle vehicle = (Vehicle)VehiclesDataGrid.SelectedItem;
            UpdateVehicleWindow objWindow = new UpdateVehicleWindow(vehicle.VehicleId);
            if(objWindow.ShowDialog() == true)
            {
                vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetLatestVehicles());
                VehiclesDataGrid.ItemsSource = vehicles;
            }
        }



    }
}
