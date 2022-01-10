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
using FleetManagement.UI.SelectForms;
using FleetManagement.UI.AddForms;

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for VehicleDetailsWindow.xaml
    /// </summary>
    public partial class UpdateVehicleWindow : Window
    {

        private FuelTypeManager fuelTypeManager = new FuelTypeManager(new FuelTypeRepository());
        private VehicleManager vehicleManager = new VehicleManager(new VehicleRepository());
        private Vehicle vehicle;

        public UpdateVehicleWindow(int vehicleId)
        {
            InitializeComponent();
            vehicle = vehicleManager.GetVehicle(vehicleId);
            txtVehicleId.Text = $"{vehicle.VehicleId}";
            txtBrand.Text = vehicle.Brand;
            txtModel.Text = vehicle.Model;
            txtChassisNumber.Text = vehicle.ChassisNumber;
            txtLicensePlate.Text = vehicle.LicensePlate;
            txtColor.Text = vehicle.Color;
            txtDoors.Text = $"{vehicle.Doors}";
            if(vehicle.Driver != null)
            {
                txtDriver.Text = $"{vehicle.Driver.FirstName} {vehicle.Driver.LastName}";
                btnRemoveDriver.IsEnabled = true;
            }
            else
            {
                txtDriver.Text = "";
                btnRemoveDriver.IsEnabled = false;
            }
            lstFuelTypes.ItemsSource = vehicle.FuelTypes;
        }

        private void btnUpdateVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vehicle.SetBrand(txtBrand.Text);
                vehicle.SetModel(txtModel.Text);
                vehicle.SetChassisNumber(txtChassisNumber.Text);
                vehicle.SetLicensePlate(txtLicensePlate.Text);
                vehicle.SetVehicleType(txtVehicleType.Text);
                vehicle.SetVehicleColor(txtColor.Text);
                vehicle.SetVehicleDoors(int.Parse(txtDoors.Text));
                vehicleManager.UpdateVehicle(vehicle);
                DialogResult = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void btnSelectDriver_Click(object sender, RoutedEventArgs e)
        {
            SelectDriverWindow objWindow = new SelectDriverWindow();
            if(objWindow.ShowDialog() == true)
            {
                txtDriver.Text = objWindow.driver.ToString();
                vehicle.SetDriver(objWindow.driver);
                btnRemoveDriver.IsEnabled = true;
            }
        }

        private void btnAddFuelType_Click(object sender, RoutedEventArgs e)
        {
            AddFuelTypeWindow objWindow = new AddFuelTypeWindow();
            if(objWindow.ShowDialog() == true)
            {
                vehicle.FuelTypes.Add(objWindow.fuelType);
                lstFuelTypes.Items.Refresh();
                // fuelTypeManager.AddFuelTypeToVehicle(vehicle.VehicleId, objWindow.fuelType.FuelTypeId);
            }
        }

        private void btnRemoveFuelType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuelType fuelType = (FuelType)lstFuelTypes.SelectedItem;
                vehicle.FuelTypes.Remove(fuelType);
                lstFuelTypes.Items.Refresh();
                // fuelTypeManager.RemoveFuelTypeFromVehicle(vehicle.VehicleId, fuelType.FuelTypeId);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void btnRemoveDriver_Click(object sender, RoutedEventArgs e)   {
            
            if(vehicle.Driver != null) {
                Driver D = vehicle.Driver;

            }
            vehicle.RemoveDriver();
            
            txtDriver.Text = "None";
            btnRemoveDriver.IsEnabled = false;
        }
    }
}
