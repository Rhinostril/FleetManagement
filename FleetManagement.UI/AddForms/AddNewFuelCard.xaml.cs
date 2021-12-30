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
        public FuelCardManager fuelCardManager { get; set; } = new FuelCardManager(new FuelCardRepository());
        public FuelTypeManager fuelTypeManager { get; set; } = new FuelTypeManager(new FuelTypeRepository());
        
        public AddNewFuelCard()
        {
         
            InitializeComponent();
            var allfueltypes = fuelTypeManager.GetAllFuelTypes();
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
                var selecteditems = fueltypeListbox.SelectedItems;
                if(selecteditems.Count != 0) {
                    List<FuelType> fueltypelist = new List<FuelType>();
                    foreach (FuelType fueltype in selecteditems) {
                        fueltypelist.Add(fueltype);
                    }
                    FuelCard fuelcard = new FuelCard(cardnumber, validatyDate, pin, isEnabled);
                    fuelcard.SetFuelTypes(fueltypelist);
                    int id = (int)fuelCardManager.AddFuelCard(fuelcard); //EXECUTESCALAR
                    foreach (FuelType fueltype in selecteditems) {
                        fuelTypeManager.AddFuelTypeToFuelCard(fueltype.FuelTypeId, id);
                    }
                    

                    MessageBox.Show("FuelCard succesfully added !", "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                } else {
                    MessageBox.Show("Choose at least 1 fueltype", "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
                }
               
            }
            catch (Exception ex)
            {

                 MessageBox.Show(ex.Message, "Add new fuelcard", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
               
           
        }
    }
}
