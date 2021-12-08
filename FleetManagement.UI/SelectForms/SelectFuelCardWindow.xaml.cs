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
using FleetManagement.UI.SelectForms;
using System.Collections.ObjectModel;

namespace FleetManagement.UI.SelectForms
{
    /// <summary>
    /// Interaction logic for SelectFuelCardWindow.xaml
    /// </summary>
    public partial class SelectFuelCardWindow : Window
    {

        private FuelCardManager fuelCardManager = new FuelCardManager(new FuelCardRepository());
        private ObservableCollection<FuelCard> fuelCards = new ObservableCollection<FuelCard>();
        public FuelCard fuelCard { get; private set; }
        

        public SelectFuelCardWindow()
        {
            InitializeComponent();
            fuelCards = new ObservableCollection<FuelCard>(fuelCardManager.GetLatestFuelcards());
            lstFuelCards.ItemsSource = fuelCards;
        }

        private void btnSearchFuelCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string cardNumber = txtCardNumber.Text;
                DateTime? valDate = (DateTime)dtpValidityDate.SelectedDate;
                fuelCards = new ObservableCollection<FuelCard>(fuelCardManager.SearchFuelCards(cardNumber, valDate));
                lstFuelCards.ItemsSource = fuelCards;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }

        private void btnSelectFuelCard_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fuelCard = (FuelCard)lstFuelCards.SelectedItem;
                DialogResult = true;
                Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
            }
        }
    }
}
