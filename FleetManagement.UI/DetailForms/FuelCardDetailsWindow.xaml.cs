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
using System.Windows.Shapes;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Managers;
using FleetManagement.Data.Repositories;

namespace FleetManagement.UI.DetailForms {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FuelCardDetailsWindow : Window {
        private FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository()); 
        public FuelCardDetailsWindow(int fuelcardID) {
            InitializeComponent();
            FuelCard fuelcard = fuelCardManager.getFuelCardByID(fuelcardID);
            //public VehicleDetailsWindow(int vehicleId)
            //{
            //    InitializeComponent();
            //    Vehicle vehicle = vehicleManager.GetVehicle(vehicleId);
            //    fuelTypes = new ObservableCollection<FuelType>(vehicle.FuelTypes);
            //    txtVehicleId.Text = $"{vehicle.VehicleId}";
            //    txtBrand.Text = vehicle.Brand;
            //    txtModel.Text = vehicle.Model;
            //    txtChassisNumber.Text = vehicle.ChassisNumber;
            //    txtLicensePlate.Text = vehicle.LicensePlate;
            //    txtColor.Text = vehicle.Color;
            //    txtDoors.Text = $"{vehicle.Doors}";
            //    if(vehicle.Driver != null)
            //    {
            //        txtDriver.Text = $"{vehicle.Driver.FirstName} {vehicle.Driver.LastName}";
            //    }
            //    else
            //    {
            //        txtDriver.Text = "";
            //    }
            //    lstFuelTypes.ItemsSource = fuelTypes;
            //}
        }
    }
}
