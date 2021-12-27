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

        private FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());
        private FuelCard fuelCard;

        public UpdateFuelCardWindow(int fuelCardId)
        {
            InitializeComponent();
            fuelCard = fuelCardManager.GetFuelCardById(fuelCardId);
            txtFuelCardId.Text = fuelCard.FuelCardId.ToString();
            txtCardNumber.Text = fuelCard.CardNumber;
            dtpValidityDate.SelectedDate = fuelCard.ValidityDate;
            txtPin.Text = fuelCard.Pin;
            cbxEnabled.IsChecked = fuelCard.IsEnabled;
            lstFuelTypes.ItemsSource = fuelCard.FuelTypes;
        }

        private void btnUpdateFuelCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fuelCard.SetCardNumber(txtCardNumber.Text);
                fuelCard.ValidityDate = (DateTime)dtpValidityDate.SelectedDate;
                fuelCard.SetPin(txtPin.Text);
                fuelCard.IsEnabled = (bool)cbxEnabled.IsChecked;
                fuelCardManager.UpdateFuelCard(fuelCard);
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
            if (objWindow.ShowDialog() == true)
            {
                txtDriver.Text = objWindow.driver.ToString();
                fuelCard.SetDriver(objWindow.driver);
                btnRemoveDriver.IsEnabled = true;
            }
        }

        private void btnRemoveDriver_Click(object sender, RoutedEventArgs e)
        {
            fuelCard.RemoveDriver();
            txtDriver.Text = "None";
            btnRemoveDriver.IsEnabled = false;
        }

        private void btnRemoveFuelType_Click(object sender, RoutedEventArgs e)
        {
            FuelType fuelType = (FuelType)lstFuelTypes.SelectedItem;
            
        }

        private void btnAddFuelType_Click(object sender, RoutedEventArgs e)
        {
            AddFuelTypeWindow objWindow = new AddFuelTypeWindow();
            if(objWindow.ShowDialog() == true)
            {

            }
        }
    }
}
