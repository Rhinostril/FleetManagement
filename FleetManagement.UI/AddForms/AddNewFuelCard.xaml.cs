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
using FleetManagement.Business.Managers;
using FleetManagement.Business.Entities;
using FleetManagement.Data.Repositories;

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for AddNewFuelCard.xaml
    /// </summary>
    public partial class AddNewFuelCard : Window
    {
        public AddNewFuelCard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FuelCardRepository fuelCardRepository = new FuelCardRepository();

                string cardnumber = txtCardnr.Text;
                DateTime validatyDate =datePicker.DisplayDate;
                string pin = txtPin.Text;
                if (pin.Length < 4)
                {
                    exceptionPinlbl.Content = "Pin requires 4 characters!";
                }
                bool isEnabled = false;
                if (chkbxIsEnabled.IsChecked == true)
                {
                    isEnabled = true;
                }

                fuelCardRepository.AddFuelCard(new FuelCard(cardnumber, validatyDate, pin, isEnabled));
                MessageBox.Show("FuelCard succesfully added !", "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {

                 MessageBox.Show(ex.Message, "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
               
           
        }
    }
}
