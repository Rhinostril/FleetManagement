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

namespace FleetManagement.UI
{
    /// <summary>
    /// Interaction logic for VehicleDetailsWindow.xaml
    /// </summary>
    public partial class VehicleDetailsWindow : Window
    {

        private VehicleManager vehicleManager = new VehicleManager(new VehicleRepository());

        private ObservableCollection<FuelType> fuelTypes = new ObservableCollection<FuelType>();

        public VehicleDetailsWindow(int vehicleId)
        {
            InitializeComponent();
            


        }
    }
}
