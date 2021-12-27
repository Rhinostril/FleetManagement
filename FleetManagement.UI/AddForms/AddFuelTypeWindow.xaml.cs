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

namespace FleetManagement.UI.AddForms
{
    /// <summary>
    /// Interaction logic for AddFuelTypeWindow.xaml
    /// </summary>
    public partial class AddFuelTypeWindow : Window
    {

        private DataToListRepository repo = new DataToListRepository();
        private ObservableCollection<FuelType> fuelTypes = new ObservableCollection<FuelType>();
        public FuelType fuelType;

        public AddFuelTypeWindow()
        {
            InitializeComponent();
            fuelTypes = new ObservableCollection<FuelType>(repo.GetAllFuelTypes());
            lstFuelTypes.ItemsSource = fuelTypes;
        }

        private void btnAddFuelType_Click(object sender, RoutedEventArgs e)
        {
            fuelType = (FuelType)lstFuelTypes.SelectedItem;
            DialogResult = true;
            Close();
        }
    }
}
