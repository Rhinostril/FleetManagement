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
using System.Collections.ObjectModel;

namespace FleetManagement.UI.SelectForms
{
    /// <summary>
    /// Interaction logic for SelectDriverWindow.xaml
    /// </summary>
    public partial class SelectDriverWindow : Window
    {

        private DriverManager driverManager = new DriverManager(new DriverRepository());
        private ObservableCollection<Driver> drivers = new ObservableCollection<Driver>();
        public Driver driver { get; private set; }

        public SelectDriverWindow()
        {
            InitializeComponent();
        }

        private void btnSelectDriver_Click(object sender, RoutedEventArgs e)
        {
            driver = (Driver)lstDrivers.SelectedItem;
            DialogResult = true;
            Close();
        }

        private void btnSearchDrivers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                DateTime? dateOfBirth = dtpDateOfBirth.SelectedDate;
                string street = txtStreet.Text;
                string number = txtNumber.Text;
                string postalCode = txtPostalCode.Text;
                string city = txtCity.Text;
                string country = txtCountry.Text;
                Address address = new Address(street, number, postalCode, city, country);
                drivers = new ObservableCollection<Driver>(driverManager.SearchDrivers(null, lastName, firstName, dateOfBirth, null, street, number, postalCode));
                lstDrivers.ItemsSource = drivers;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }
    }
}
