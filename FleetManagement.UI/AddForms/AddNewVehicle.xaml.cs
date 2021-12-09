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
using FleetManagement.Data.Repositories;

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for AddNewVehicle.xaml
    /// </summary>
    public partial class AddNewVehicle : Window
    {
        public AddNewVehicle()
        {
            InitializeComponent();
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
                string fuel = lstBoxFuelTypes.SelectedItem.ToString();

                fuelTypes.Add(new FuelType(fuel));


                if (chasis.Length < 17)
                {
                    lblException.Content = "ChasisNr requires 17 characters!";
                }

                vehicleRepository.AddVehicle(new Vehicle(brand,model,chasis,plate,new List<FuelType>(),type,color,int.Parse(doors)));
                MessageBox.Show("FuelCard succesfully added !", "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Add new vehicle", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
