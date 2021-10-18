
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
    }
    //[Fact]
    //public void Test_Expected_Invalid() {
    //    foreach(int key in InValidtestSecurityNumbers.Keys) {
    //        bool result = SocialSecurityNumberChecker.checknormalSecurityNumber(InValidtestSecurityNumbers[key]);
    //        Assert.False(result);
    //    }
    //}

//}
}
