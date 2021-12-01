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

        private Driver driver;

        public UpdateDriverWindow(Driver driver)
        {
            InitializeComponent();
            this.driver = driver;
        }

        private void btnUpdateDriver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                DateTime dateOfBirth = (DateTime)dtpDateOfBirth.SelectedDate;
                string securityNumber = txtSecurityNumber.Text;
                string street = txtStreet.Text;
                string houseNr = txtHouseNumber.Text;
                string postalCode = txtPostalCode.Text;
                string city = txtCity.Text;
                string country = txtCountry.Text;
                
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

            }
        }

        private void btnSelectFuelCard_Click(object sender, RoutedEventArgs e)
        {
            SelectFuelCardWindow objWindow = new SelectFuelCardWindow()
            if(objWindow.ShowDialog() == true)
            {

            }
        }
    }
}
