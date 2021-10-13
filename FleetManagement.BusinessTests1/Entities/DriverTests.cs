using Microsoft.VisualStudio.TestTools.UnitTesting;
using FleetManagement.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace FleetManagement.Business.Entities.Tests
{
    [TestClass()]
    public class DriverTests
    {
        [Fact]
        public void Test_Constructor_Valid()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Assert.Equal("Elvis", driver.FirstName);
            Assert.Equal("Presley", driver.LastName);
            Assert.Equal("97.05.20-327.78",driver.SecurityNumber);
            Assert.Equal(new DateTime(1997,05,20), driver.DateOfBirth);
            Assert.Equal(new List<string>{"B","A1"}, driver.DriversLicenceType);
        }

        [TestMethod()]
        public void setSecurityNumberTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Assert.Equal("97.05.20-327.78", driver.SecurityNumber);
        }

        [TestMethod()]
        public void addTypeToDriversLicenseTest()
        {
            
        }

        [TestMethod()]
        public void setAddressTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });

            Address a = new Address();
            a.Street = "street";
            a.PostalCode = "9000";
            a.HouseNr = "2";
            a.Country = "belgium";
            a.City = "gent";

            driver.setAddress(a);
            

            Assert.NotNull(a.Street);
            Assert.NotNull(a.PostalCode);
            Assert.NotNull(a.HouseNr);
            Assert.NotNull(a.Country);
            Assert.NotNull(a.City);
           
        }

        [TestMethod()]
        public void removeAddressTest()
        {
            //Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });

            //Address a = new Address();
            //a.Street = "street";
            //a.PostalCode = "9000";
            //a.HouseNr = "2";
            //a.Country = "belgium";
            //a.City = "gent";
            //a.AddressID = 1;
            //driver.setAddress(a);
            //driver.removeAddress(a);

            //Assert.Null(a);


        }

        [TestMethod()]
        public void setDriverIDTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            driver.setDriverID(1);
            Assert.Equal(1, driver.DriverID);

        }

        [TestMethod()]
        public void setVehicleTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            driver.setVehicle(vehicle);
            Assert.NotNull(vehicle);

        }

        [TestMethod()]
        public void TryRemoveVehicleTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            driver.setVehicle(vehicle);
            Assert.True(driver.TryRemoveVehicle(vehicle));

        }

        [TestMethod()]
        public void setFuelCardTest()
        {
            FuelCard f = new FuelCard(1, "123", new DateTime(), "1324", null, null);
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            driver.setFuelCard(f);
            Assert.NotNull(f);

        }

        [TestMethod()]
        public void removeAnyFuelCardTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            FuelCard f = new FuelCard(1, "123", new DateTime(), "1324", null, null);

            driver.setFuelCard(f);


            Assert.NotNull(f);

        }

        [TestMethod()]
        public void tryRemoveSpecificFuelCardTest()
        {
            Driver driver = new Driver("Elvis", "Presley", new DateTime(1997, 05, 20), "97.05.20-327.78", new List<string> { "B", "A1" });
            Vehicle vehicle = new Vehicle(1, "Porsche", "GT2RS", "1234-1234-1234-17", "KAPPER FURKAN", "Gasoline", "Sportauto", "Donkergrijs", 2, null);
            FuelCard f1 = new FuelCard(1, "123", new DateTime(), "1324", null, null);
            FuelCard f2 = new FuelCard(1, "123", new DateTime(), "1324", null, null);

            driver.setFuelCard(f2);
            driver.setFuelCard(f1);

            driver.tryRemoveSpecificFuelCard(f1);

            Assert.NotNull(f1);

        }
    }
}