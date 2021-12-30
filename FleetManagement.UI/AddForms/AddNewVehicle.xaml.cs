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

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for AddNewVehicle.xaml
    /// </summary>
    public partial class AddNewVehicle : Window
    {
        public FuelTypeManager fuelTypeManager { get; set; } = new FuelTypeManager(new FuelTypeRepository());
        public ObservableCollection<FuelType> fueltypecollection = new ObservableCollection<FuelType>();
        public AddNewVehicle()
        {
            InitializeComponent();
            var allfueltypes = fuelTypeManager.GetAllFuelTypes();
            foreach (FuelType type in allfueltypes) {
                fueltypecollection.Add(type);
            }

            lstBoxFuelTypes.ItemsSource = fueltypecollection;
            lstBoxFuelTypes.Items.Refresh();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VehicleRepository vehicleRepository = new VehicleRepository();

                string brand = txtBrand.Text;
                string model = txtModel.Text;
                string chasis = txtChasis.Text;
                string plate = txtPlate.Text;
                string type = txtType.Text;
                string color = txtColor.Text;
                string doors =txtDoors.Text;
                List<FuelType> fuelTypes = new List<FuelType>();
                var selectedfuels = lstBoxFuelTypes.SelectedItems;
                if (selectedfuels.Count != 0) {
                    foreach (FuelType fueltype in selectedfuels) {
                        fuelTypes.Add(fueltype);
                    }
                    if (chasis.Length < 17) {
                        lblException.Content = "ChasisNr requires 17 characters!";
                    }

                    int id = (int)vehicleRepository.AddVehicle(new Vehicle(brand, model, chasis, plate, fuelTypes, type, color, int.Parse(doors)));
                    foreach(FuelType fueltype in fuelTypes) {
                        fuelTypeManager.AddFuelTypeToVehicle(fueltype.FuelTypeId, id);
                    }
                    MessageBox.Show("FuelCard succesfully added !", "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                } else {
                    MessageBox.Show("Choose at least 1 fueltype", "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Add new vehicle", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
