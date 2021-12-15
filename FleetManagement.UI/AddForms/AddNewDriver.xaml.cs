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
using FleetManagement.Data.Repositories;
using FleetManagement.Business.Managers;
using FleetManagement.Business.Interfaces;
using FleetManagement.Business.Entities;
namespace FleetManagement.UI

{
    /// <summary>
    /// Interaction logic for AddNewDriver.xaml
    /// </summary>
    public partial class AddNewDriver : Window
    {

        private DriverManager driverManager = new DriverManager(new DriverRepository());

        public AddNewDriver()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                DateTime dateOfBirth = (DateTime)DriverDateOfBirthPicker.SelectedDate;
                string securityNr = txtSecurityNumber.Text;
                string street = txtStreet.Text;
                string houseNr = txtHouseNr.Text;
                string postalCode = txtPostalCode.Text;
                string city = txtCity.Text;
                string country = txtCountry.Text;
                Driver driver = new Driver(firstName, lastName, dateOfBirth, securityNr);
                Address address = new Address(street, houseNr, postalCode, city, country);
                driver.SetAddress(address);
                driverManager.AddDriver(driver);
                DialogResult = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void DriverDateOfBirthPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime dob = (DateTime)DriverDateOfBirthPicker.SelectedDate;
            string years = dob.Year.ToString().Substring(2, 2);
            string months = dob.Month.ToString();
            if (months.Length < 2) months = "0" + months;
            string days = dob.Day.ToString();
            if (days.Length < 2) days = "0" + days;
            string securityNumberTemp = $"{years}.{months}.{days}-XXX.XX";
            txtSecurityNumber.Text = securityNumberTemp;
        }
    }
}
