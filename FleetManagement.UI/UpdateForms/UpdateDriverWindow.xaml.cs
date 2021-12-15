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

namespace FleetManagement.UI.UpdateForms
{
    /// <summary>
    /// Interaction logic for UpdateDriverWindow.xaml
    /// </summary>
    public partial class UpdateDriverWindow : Window
    {

        private DriverManager driverManager = new DriverManager(new DriverRepository());
        private Driver driver;

        public UpdateDriverWindow(Driver driver)
        {
            InitializeComponent();
            this.driver = driver;
            txtDriverId.Text = driver.DriverID.ToString();
            txtFirstName.Text = driver.FirstName;
            txtLastName.Text = driver.LastName;
            dtpDateOfBirth.SelectedDate = driver.DateOfBirth;
            txtSecurityNumber.Text = driver.SecurityNumber;
            if (driver.Vehicle != null) txtVehicle.Text = driver.Vehicle.ToString();
            if (driver.FuelCard != null) txtFuelCard.Text = driver.FuelCard.ToString();
            txtStreet.Text = driver.Address.Street;
            txtHouseNumber.Text = driver.Address.HouseNr;
            txtPostalCode.Text = driver.Address.PostalCode;
            txtCity.Text = driver.Address.City;
            txtCountry.Text = driver.Address.Country;
        }

        private void btnUpdateDriver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                driver.SetFirstName(txtFirstName.Text);
                driver.SetLastName(txtLastName.Text);
                driver.SetDateOfBirth((DateTime)dtpDateOfBirth.SelectedDate);
                driver.SetSecurityNumber(txtSecurityNumber.Text);
                driver.Address.SetHouseNr(txtHouseNumber.Text);
                driver.Address.SetPostalCode(txtPostalCode.Text);
                driver.Address.SetCity(txtCity.Text);
                driver.Address.SetCountry(txtCountry.Text);

                driverManager.UpdateDriver(driver);
                DialogResult = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void btnSelectVehicle_Click(object sender, RoutedEventArgs e)
        {
            SelectVehicleWindow objWindow = new SelectVehicleWindow();
            if(objWindow.ShowDialog() == true)
            {
                driver.SetVehicle(objWindow.vehicle);
                txtVehicle.Text = objWindow.vehicle.ToString();
                btnRemoveVehicle.IsEnabled = true;
            }
        }

        private void btnSelectFuelCard_Click(object sender, RoutedEventArgs e)
        {
            SelectFuelCardWindow objWindow = new SelectFuelCardWindow();
            if(objWindow.ShowDialog() == true)
            {
                driver.SetFuelCard(objWindow.fuelCard);
                txtFuelCard.Text = objWindow.fuelCard.ToString();
                btnRemoveFuelCard.IsEnabled = true;
            }
        }

        private void btnRemoveVehicle_Click(object sender, RoutedEventArgs e)
        {
            driver.RemoveVehicle();
            txtVehicle.Text = "None";
            btnRemoveVehicle.IsEnabled = false;
        }

        private void btnRemoveFuelCard_Click(object sender, RoutedEventArgs e)
        {
            driver.RemoveFuelCard();
            txtFuelCard.Text = "None";
            btnRemoveFuelCard.IsEnabled = false;
        }
    }
}
