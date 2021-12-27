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
    /// Interaction logic for AddLicenseTypeWindow.xaml
    /// </summary>
    public partial class AddLicenseTypeWindow : Window
    {

        private LicenseTypeManager licenseTypeManager = new LicenseTypeManager(new LicenseTypeRepository());
        private ObservableCollection<(int, string)> licenseTypes = new ObservableCollection<(int, string)>();
        private (int, string) LicenseType;

        public AddLicenseTypeWindow()
        {
            InitializeComponent();
            licenseTypes = new ObservableCollection<(int, string)>(licenseTypeManager.GetAllLicenseTypes());
            lstLicenseTypes.ItemsSource = licenseTypes;
        }

        private void btnAddFuelType_Click(object sender, RoutedEventArgs e)
        {
            

        }
    }
}
