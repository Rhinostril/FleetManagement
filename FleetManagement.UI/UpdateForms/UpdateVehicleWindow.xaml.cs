﻿using System;
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

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for VehicleDetailsWindow.xaml
    /// </summary>
    public partial class UpdateVehicleWindow : Window
    {


        private VehicleManager vehicleManager = new VehicleManager(new VehicleRepository());
        private ObservableCollection<FuelType> fuelTypes = new ObservableCollection<FuelType>();
        private Vehicle vehicle;

        public UpdateVehicleWindow(int vehicleId)
        {
            InitializeComponent();
            vehicle = vehicleManager.GetVehicle(vehicleId);
            fuelTypes = new ObservableCollection<FuelType>(vehicle.FuelTypes);
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
            }
            else
            {
                txtDriver.Text = "";
            }
            lstFuelTypes.ItemsSource = fuelTypes;
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

        }

        private void btnRemoveFuelType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuelType fuelType = (FuelType)lstFuelTypes.SelectedItem;

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void btnRemoveDriver_Click(object sender, RoutedEventArgs e) // geen reference?
        {
            vehicle.RemoveDriver();
            txtDriver.Text = "None";
            btnRemoveDriver.IsEnabled = false;
        }
    }
}
