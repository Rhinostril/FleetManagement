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
using FleetManagement.UI.UpdateForms;

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
        private List<Vehicle> vehicleCache = new List<Vehicle>();
        private ObservableCollection<Driver> drivers = new ObservableCollection<Driver>();
        private List<Driver> driversCache = new List<Driver>();
        private ObservableCollection<FuelCard> fuelCards = new ObservableCollection<FuelCard>();

        public MainWindow()
        {
            InitializeComponent();
            vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetLatestVehicles());
            VehiclesDataGrid.ItemsSource = vehicles;
            vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetLatestVehicles());
            vehicleCache = vehicles.ToList();
            drivers = new ObservableCollection<Driver>(driverManager.GetLatestDrivers());
            DriversDataGrid.ItemsSource = drivers;
            driversCache = drivers.ToList();
            fuelCards = new ObservableCollection<FuelCard>(fuelCardManager.GetLatestFuelcards());
            FuelCardsDataGrid.ItemsSource = fuelCards;
        }

        private void SearchVehiclesButton_Click(object sender, RoutedEventArgs e)
        {
            int? doors = null;
            if (!String.IsNullOrWhiteSpace(Doortxtbox.Text)) {
                doors = Int32.Parse(Doortxtbox.Text);
            }
            var returnlist =  vehicleManager.Searchvehicles(null,brandtxtbox.Text, modeltxtbox.Text, ChassisNrtxtbox.Text, LicensePlatetxtbox.Text, typetxtbox.Text, colortxtbox.Text, doors);
            Console.WriteLine();
            if (returnlist.Any()) {

                vehicles.Clear();
                VehiclesDataGrid.ItemsSource = vehicles;
                VehiclesDataGrid.Items.Refresh();
                foreach (Vehicle result in returnlist) {
                    vehicles.Add(result);
                }
            } else {
                vehicles.Clear();
                foreach(Vehicle v in vehicleCache) {
                    vehicles.Add(v);
                }
            }
            
            Console.WriteLine();
            VehiclesDataGrid.Items.Refresh();
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
            FuelCardDetailsWindow objWindow = new FuelCardDetailsWindow(fuelCard);
            objWindow.Show();
        }

        private void NewDriverButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewDriver addNewDriver = new AddNewDriver();
            if(addNewDriver.ShowDialog() == true)
            {
                drivers = new ObservableCollection<Driver>(driverManager.GetLatestDrivers());
                DriversDataGrid.ItemsSource = drivers;
            }
        }

        private void NewVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewVehicle addNewVehicle = new AddNewVehicle();
            if(addNewVehicle.ShowDialog() == true)
            {
                vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetLatestVehicles());
                VehiclesDataGrid.ItemsSource = vehicles;
            }
        }

        private void NewFuelCardButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewFuelCard addNewFuelCard = new AddNewFuelCard();
            if(addNewFuelCard.ShowDialog() == true)
            {
                fuelCards = new ObservableCollection<FuelCard>(fuelCardManager.GetLatestFuelcards());
                FuelCardsDataGrid.ItemsSource = fuelCards;
            }
        }

        private void FuelCardUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FuelCardDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure?", "Delete Fuelcard", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    FuelCard fuelCard = (FuelCard)FuelCardsDataGrid.SelectedItem;
                    fuelCardManager.DeleteFuelCard(fuelCard);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
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

        private void DeleteVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure?", "Delete Vehicle", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Vehicle vehicle = (Vehicle)VehiclesDataGrid.SelectedItem;
                    vehicleManager.DeleteVehicle(vehicle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void DriverUpdate_Click(object sender, RoutedEventArgs e)
        {
            Driver driver = (Driver)DriversDataGrid.SelectedItem;
            UpdateDriverWindow objWindow = new UpdateDriverWindow(driver);
            if(objWindow.ShowDialog() == true)
            {
                drivers = new ObservableCollection<Driver>(driverManager.GetLatestDrivers());
                DriversDataGrid.ItemsSource = drivers;
            }
        }

        private void DriverDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(MessageBox.Show("Are you sure?", "Delete Driver", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Driver driver = (Driver)DriversDataGrid.SelectedItem;
                    driverManager.DeleteDriver(driver);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void SearchDriversButton_Click(object sender, RoutedEventArgs e) {
          
            var returnlist = driverManager.SearchDrivers(null, LastnameTxtBox.Text, FirstnameTxtBox.Text, DriverDateOfBirthPicker.SelectedDate, SecurityNrTxtBox.Text, streetTxtBox.Text, HouseNrTxtBox.Text, PostalCodeTxtBox.Text);
            Console.WriteLine();
            if (returnlist.Any()) {

                drivers.Clear();
                DriversDataGrid.ItemsSource = drivers;
                DriversDataGrid.Items.Refresh();
                foreach (Driver result in returnlist) {
                    drivers.Add(result);
                }
            } else {
                drivers.Clear();
                foreach (Driver v in driversCache) {
                    drivers.Add(v);
                }
                DriversDataGrid.ItemsSource = drivers;
            }

            Console.WriteLine();
            VehiclesDataGrid.Items.Refresh();
        }
    }
}
