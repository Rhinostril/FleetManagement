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
using FleetManagement.Data.Repositories;
using FleetManagement.UI.SelectForms;
using FleetManagement.UI.AddForms;

namespace FleetManagement.UI.UpdateForms
{
    /// <summary>
    /// Interaction logic for UpdateFuelCardWindow.xaml
    /// </summary>
    public partial class UpdateFuelCardWindow : Window
    {
        private FuelTypeManager fuelTypeManager = new FuelTypeManager(new FuelTypeRepository());
        private FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());
        private FuelCard fuelCard;

        public UpdateFuelCardWindow(int fuelCardId)
        {
            InitializeComponent();
            fuelCard = fuelCardManager.GetFuelCardById(fuelCardId);
            fuelCard.SetFuelCardId(fuelCardId);
            txtFuelCardId.Text = fuelCard.FuelCardId.ToString();
            txtCardNumber.Text = fuelCard.CardNumber;
            dtpValidityDate.SelectedDate = fuelCard.ValidityDate;
            txtPin.Text = fuelCard.Pin;
           
            if (fuelCard.Driver != null) {
                txtDriver.Text = fuelCard.Driver.ToString();
                btnRemoveDriver.IsEnabled = true;
            }
            cbxEnabled.IsChecked = fuelCard.IsEnabled;
            lstFuelTypes.ItemsSource = fuelCard.FuelTypes;

        }

        private void btnUpdateFuelCard_Click(object sender, RoutedEventArgs e)
        {
            try{
               
                fuelCard.SetCardNumber(txtCardNumber.Text);
                fuelCard.ValidityDate = (DateTime)dtpValidityDate.SelectedDate;
                fuelCard.IsEnabled = (bool)cbxEnabled.IsChecked;
                if(txtPin.Text.Length != 4) {
                    MessageBox.Show("Pin must contain 4 characters");
                } else {
                    fuelCard.SetPin(txtPin.Text);
                    fuelCardManager.UpdateFuelCard(fuelCard);
                    DialogResult = true;
                    Close();
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void btnSelectDriver_Click(object sender, RoutedEventArgs e)
        {
            SelectDriverWindow objWindow = new SelectDriverWindow();
            if (objWindow.ShowDialog() == true)
            {
                txtDriver.Text = objWindow.driver.ToString();
                fuelCard.SetDriver(objWindow.driver);
                btnRemoveDriver.IsEnabled = true;
            }
        }

        private void btnRemoveDriver_Click(object sender, RoutedEventArgs e)
        {
            if (fuelCard.Driver != null) {
                int driverid = fuelCard.Driver.DriverID;
                fuelCardManager.RemoveDriverConnectionByDriverId(driverid);
                fuelCard.RemoveDriver();
                txtDriver.Text = "None";
                btnRemoveDriver.IsEnabled = false;
            } else {
                btnRemoveDriver.IsEnabled = false;

            }


        }

        private void btnRemoveFuelType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuelType fuelType = (FuelType)lstFuelTypes.SelectedItem;
                fuelCard.FuelTypes.Remove(fuelType);
                lstFuelTypes.Items.Refresh();
                // fuelTypeManager.RemoveFuelTypeFromFuelCard(fuelType.FuelTypeId, fuelCard.FuelCardId);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }

        private void btnAddFuelType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddFuelTypeWindow objWindow = new AddFuelTypeWindow();
                if (objWindow.ShowDialog() == true)
                {
                    fuelCard.FuelTypes.Add(objWindow.fuelType);
                    lstFuelTypes.Items.Refresh();
                    // fuelTypeManager.AddFuelTypeToFuelCard(objWindow.fuelType.FuelTypeId, fuelCard.FuelCardId);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Foutmelding!");
            }
        }
    }
}
