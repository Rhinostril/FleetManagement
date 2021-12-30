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
using System.Collections.ObjectModel;

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for AddNewFuelCard.xaml
    /// </summary>
    public partial class AddNewFuelCard : Window {

        public ObservableCollection<FuelType> fueltypecollection = new ObservableCollection<FuelType>();
        public FuelCardRepository fuelCardRepository { get; set; } = new FuelCardRepository();
        public AddNewFuelCard()
        {
         
            InitializeComponent();
            var allfueltypes = fuelCardRepository.GetAllFuelTypes();
            foreach (FuelType type in allfueltypes) {
                fueltypecollection.Add(type);
            }

            fueltypeListbox.ItemsSource = fueltypecollection;
            fueltypeListbox.Items.Refresh();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
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
                //TODO ADD FUELTYPE SELECTION
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
