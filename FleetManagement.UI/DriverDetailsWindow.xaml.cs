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

namespace FleetManagement.UI {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DriverDetailWindow : Window {
        private DriverManager drivermanager = new DriverManager(new DriverRepository());
        public DriverDetailWindow(int driverID) {
            InitializeComponent();
            Driver driver = drivermanager.GetDriverByID(driverID);
            TxtDriverId.Text = driver.DriverID.ToString();
            TxtFirstname.Text = driver.FirstName;
            TxtLastname.Text = driver.LastName;
            DateOfBirth.SelectedDate = driver.DateOfBirth;
            TxtSecurityNumber.Text = driver.SecurityNumber;
          

            if (driver.Address != null)
            {
                TxtAdres.Text = $"{driver.Address.Street} {driver.Address.HouseNr}, {driver.Address.PostalCode} {driver.Address.City}";
            }
            if(driver.Vehicle != null)
            {
                TxtVehicle.Text = $"{driver.Vehicle.Brand} {driver.Vehicle.Model}, {driver.Vehicle.LicensePlate}";
            }
            if (driver.FuelCard != null)
            { 
                TxtFuelCard.Text = $"{driver.FuelCard.CardNumber}, PIN:{driver.FuelCard.Pin}";
            }
                //fuelTypes = new ObservableCollection<FuelType>(vehicle.FuelTypes);
                //txtVehicleId.Text = $"{vehicle.VehicleId}";
                //txtBrand.Text = vehicle.Brand;
                //txtModel.Text = vehicle.Model;
                //txtChassisNumber.Text = vehicle.ChassisNumber;
                //txtLicensePlate.Text = vehicle.LicensePlate;
                //txtColor.Text = vehicle.Color;
                //txtDoors.Text = $"{vehicle.Doors}";
                //if (vehicle.Driver != null) {
                //    txtDriver.Text = $"{vehicle.Driver.FirstName} {vehicle.Driver.LastName}";
                //} else {
                //    txtDriver.Text = "";
                //}
                //lstFuelTypes.ItemsSource = fuelTypes;

            }
    }
}
