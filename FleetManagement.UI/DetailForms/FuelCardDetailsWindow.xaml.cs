using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FleetManagement.UI.DetailForms {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class FuelCardDetailsWindow : Window {

        private FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());
        private ObservableCollection<FuelType> fuelTypes = new ObservableCollection<FuelType>();

        public FuelCardDetailsWindow(FuelCard fuelCard) {
            InitializeComponent();
            if (fuelCard != null)
            {
                FuelCard fuelcard = fuelCardManager.GetFuelCardById(fuelCard.FuelCardId);

                fuelTypes = new ObservableCollection<FuelType>(fuelcard.FuelTypes);
                lstFuelTypes.ItemsSource = fuelTypes;
                lstFuelTypes.Items.Refresh();
                TxtFuelCardID.Text = $"{fuelcard.FuelCardId}";
                TxtCardNr.Text = $"{fuelcard.CardNumber}";
                Validityate.SelectedDate = fuelcard.ValidityDate;
                TxtPin.Text = $"{fuelcard.Pin}";
                CheckBoxIsEnabled.IsChecked = fuelcard.IsEnabled;

                if(fuelcard.Driver != null)
                {
                    TxtDriverId.Text = $"{fuelcard.Driver.DriverID}";
                }
                else
                {
                    TxtDriverId.Text = $"None";
                }
            }
            else
            {
                MessageBox.Show($"Error could not retrieve fuelcard");
            }
        }
    }
}
