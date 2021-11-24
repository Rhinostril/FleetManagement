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

                string cardnumber = "98745";
                DateTime validatyDate = new DateTime(1968, 12, 5);
                string pin = "1234";
                bool isEnabled = true;

                fuelCardRepository.AddFuelCard(new FuelCard(cardnumber, validatyDate, pin, isEnabled));

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                MessageBox.Show("FuelCard succesfully added !","Add new fuelcard",MessageBoxButton.OK,MessageBoxImage.Information);
                Close();
            }
        }
    }
}
