
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleetManagement.Business.Tools;
using Xunit;

namespace FleetManagement.Business.Entities.Tests {

    public class FuelTypeConfigManagerTest {

        public FuelTypeConfigManagerTest() {
        }

        [Fact]
        public void FuelTypeTest_Control_Valid() {
            IReadOnlyList<FuelType> fuellist = FuelTypeConfigManager.GetAllFuelTypes();
            var fuellistbynameProperty = fuellist.Select(x => x.FuelName).ToList();

            Assert.Contains("Petrol 98", fuellistbynameProperty);
            Assert.Contains("Ethanol", fuellistbynameProperty);
            Assert.Contains("super Diesel", fuellistbynameProperty);
            Assert.Contains("LPG", fuellistbynameProperty);
            Assert.Contains("Hydrogen", fuellistbynameProperty);

        }

        //[Fact]
        //public void FuelTypeTest_Control_Valid2() {
        //    string var1 = "aasd";
        //    string var2 = "sdsds";
        //    FuelTypeConfigManager.AddFuelTypeToConfigFile(var1, var2);
        //    IReadOnlyList<FuelType> fuellist2 = FuelTypeConfigManager.GetAllFuelTypes();
        //    var fuellist2bynameProperty = fuellist2.Select(x => x.FuelName).ToList();
        //    Assert.Contains("sdsds", fuellist2bynameProperty);
        //}

    }
}

