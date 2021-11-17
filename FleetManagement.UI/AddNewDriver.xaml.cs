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
        public AddNewDriver()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IDriverRepository driverRepository = new DriverRepository();

           //try catch zetten en melding van toevoeging
            
            string firstname = txtFirstName.Text;
            string lastname = txtLastName.Text;
            DateTime birthDate = new DateTime(1997, 05, 20);
            string securityNumber = txtSecurityNumber.Text;

            driverRepository.AddDriver(new Driver(firstname,lastname,birthDate,securityNumber));


        }
    }
}
