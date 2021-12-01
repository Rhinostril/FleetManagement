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

namespace FleetManagement.UI.SelectForms
{
    /// <summary>
    /// Interaction logic for SelectVehicleWindow.xaml
    /// </summary>
    public partial class SelectVehicleWindow : Window
    {
        public SelectVehicleWindow()
        {
            InitializeComponent();
        }
    }
}
