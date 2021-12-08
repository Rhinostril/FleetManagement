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
using System.Windows.Shapes;
using FleetManagement.Business.Entities;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Interfaces;
using FleetManagement.Data.Repositories;
using FleetManagement.UI.SelectForms;
using System.Collections.ObjectModel;

namespace FleetManagement.UI.SelectForms
{
    /// <summary>
    /// Interaction logic for SelectVehicleWindow.xaml
    /// </summary>
    public partial class SelectVehicleWindow : Window
    {

        public Vehicle vehicle { get; private set; }

        private VehicleManager vehicleManager = new VehicleManager(new VehicleRepository());
        private ObservableCollection<Vehicle> vehicles = new ObservableCollection<Vehicle>();

        public SelectVehicleWindow()
        {
            InitializeComponent();
            vehicles = new ObservableCollection<Vehicle>(vehicleManager.GetLatestVehicles());
            lstVehicles.ItemsSource = vehicles;
        }

        private void btnSearchVehicles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string brand = txtBrand.Text;
                string model = txtModel.Text;
                string chassisNr = txtChassisNumber.Text;
                string licensePlate = txtLicensePlate.Text;
                string type = txtType.Text;
                string color = txtColor.Text;
                if (!string.IsNullOrWhiteSpace(txtDoors.Text))
                {
                    int doors = int.Parse(txtDoors.Text);
                    vehicles = new ObservableCollection<Vehicle>(vehicleManager.Searchvehicles(null, brand, model, chassisNr, licensePlate, type, color, doors));
                }
                else
                {
                    vehicles = new ObservableCollection<Vehicle>(vehicleManager.Searchvehicles(null, brand, model, chassisNr, licensePlate, type, color, null));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }

        private void btnSelectVehicle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vehicle = (Vehicle)lstVehicles.SelectedItem;
                DialogResult = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
    }
}
